using IL2CPU.API;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Media;
using IL2CPU.API.Attribs;
using System.Drawing;
using Cosmos.System.Audio;
using Cosmos.HAL.Drivers.PCI.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio.DSP.Processing;

using Cosmos.Core;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using System.Threading;
using System.Buffers.Text;
using Cosmos.System.Graphics.Fonts;
using System.Globalization;
using Cosmos.System.ExtendedASCII;
using Cosmos.System.ScanMaps;

namespace AhmedOS
{
    internal class terminal
    {
        public static VGAScreen VScreen = new VGAScreen();
        public abstract class AudioDriver;
        Canvas canvas;
        public static bool Audio = false;

        public static uint GetAmountOfRAM()
        {
            return Multiboot2.GetMemUpper() / 1024;
        }
        public static int CanReadCPUID() => throw new NotImplementedException();
        public static void ReadCPUID(uint type, ref int eax, ref int ebx, ref int ecx, ref int edx) => throw new NotImplementedException();
        public static string GetCPUBrandString()
        {
            if (CanReadCPUID() != 0)
            {
                // See https://c9x.me/x86/html/file_module_x86_id_45.html

                int eax = 0;
                int ebx = 0;
                int ecx = 0;
                int edx = 0;
                int[] s = new int[64];
                string rs = "";

                for (uint i = 0; i < 3; i++)
                {
                    ReadCPUID(0x80000002 + i, ref eax, ref ebx, ref ecx, ref edx);
                    s[i * 16 + 0] = eax % 256;
                    s[i * 16 + 1] = (eax >> 8) % 256;
                    s[i * 16 + 2] = (eax >> 16) % 256;
                    s[i * 16 + 3] = (eax >> 24) % 256;
                    s[i * 16 + 4] = ebx % 256;
                    s[i * 16 + 5] = (ebx >> 8) % 256;
                    s[i * 16 + 6] = (ebx >> 16) % 256;
                    s[i * 16 + 7] = (ebx >> 24) % 256;
                    s[i * 16 + 8] = ecx % 256;
                    s[i * 16 + 9] = (ecx >> 8) % 256;
                    s[i * 16 + 10] = (ecx >> 16) % 256;
                    s[i * 16 + 11] = (ecx >> 24) % 256;
                    s[i * 16 + 12] = edx % 256;
                    s[i * 16 + 13] = (edx >> 8) % 256;
                    s[i * 16 + 14] = (edx >> 16) % 256;
                    s[i * 16 + 15] = (edx >> 24) % 256;
                }
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == 0x00)
                    {
                        continue;
                    }
                    rs += (char)s[i];
                }

                if (!(rs == ""))
                {
                    return rs.Trim();
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            throw new NotSupportedException();
        }
        public static string GetCPUVendorName()
        {
            if (CanReadCPUID() != 0)
            {
                int eax = 0;
                int ebx = 0;
                int ecx = 0;
                int edx = 0;
                ReadCPUID(0, ref eax, ref ebx, ref ecx, ref edx); // 0 is vendor name

                string s = "";
                s += (char)(ebx & 0xff);
                s += (char)((ebx >> 8) & 0xff);
                s += (char)((ebx >> 16) & 0xff);
                s += (char)(ebx >> 24);
                s += (char)(edx & 0xff);
                s += (char)((edx >> 8) & 0xff);
                s += (char)((edx >> 16) & 0xff);
                s += (char)(edx >> 24);
                s += (char)(ecx & 0xff);
                s += (char)((ecx >> 8) & 0xff);
                s += (char)((ecx >> 16) & 0xff);
                s += (char)(ecx >> 24);

                return s;
            }

            throw new NotSupportedException();
        }
        public static long EstimateCPUSpeedFromName(string s)
        {
            var _words = new List<string>();
            string curr = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ' || (byte)s[i] == 0)
                {
                    if (curr != "")
                    {
                        _words.Add(curr);
                    }
                    curr = "";
                }
                else
                {
                    curr += s[i];
                }
            }
            _words.Add(curr);
            string[] words = _words.ToArray();
            string[] w = new string[words.Length];
            for (int i = 0; i < words.Length; i++) // Switch order
            {
                w[i] = words[words.Length - i - 1];
            }
            words = w;
            double multiplier = 0;
            double value = 0;
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                var wordEnd = word.Substring(word.Length - 3, 3);
                if (word == "MHz" || wordEnd == "MHz")
                {
                    multiplier = 1e6;
                }
                else if (word == "GHz" || wordEnd == "GHz")
                {
                    multiplier = 1e9;
                }
                else if (word == "THz" || wordEnd == "THz")
                {
                    multiplier = 1e12;
                }
                if (value == 0)
                {
                    if (double.TryParse(word, out value) || double.TryParse(word[0..^3], out value))
                    {
                        break;
                    }
                }
            }
            value *= multiplier;
            if ((long)value == 0)
            {
                //Global.Send("Unable to calculate cycle speed from " + s);
                // throw new NotSupportedException("Unable to calculate cycle speed from " + s);
            }
            return (long)value;
        }
        public static long GetCPUCycleSpeed()
        {
            if (CanReadCPUID() != 0)
            {
                string s = GetCPUBrandString();
                return EstimateCPUSpeedFromName(s);
            }

            throw new NotSupportedException();
        }
        static int GetIntegerDigitCountString(int value)
        {
            // Version 1: get digit count with ToString.
            return value.ToString().Length;
        }
        public static void launch()
        {

        C:
            Console.Write("╔═══════════════════════════════════════════════════════╦══════════╦══════════╗\n");
            Console.Write("║ Terminal                                              ║  'help'  ║  'exit'  ║\n");
            Console.Write("╠═══════════════════════════════════════════════════════╩══════════╩══════════╣\n");
            Console.Write("║                                                                             ║\n");            
            Console.Write("║      ████████╗███████╗██████╗ ███╗   ███╗██╗███╗   ██╗ █████╗ ██╗           ║\n");
            Console.Write("║      ╚══██╔══╝██╔════╝██╔══██╗████╗ ████║██║████╗  ██║██╔══██╗██║           ║\n");
            Console.Write("║         ██║   █████╗  ██████╔╝██╔████╔██║██║██╔██╗ ██║███████║██║           ║\n");
            Console.Write("║         ██║   ██╔══╝  ██╔══██╗██║╚██╔╝██║██║██║╚██╗██║██╔══██║██║           ║\n");
            Console.Write("║         ██║   ███████╗██║  ██║██║ ╚═╝ ██║██║██║ ╚████║██║  ██║███████╗      ║\n");
            Console.Write("║         ╚═╝   ╚══════╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝      ║\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
            Console.WriteLine();
            while (true)
            {
                try
                {
                    Console.Write("root@" + Kernel.pcname +":~$ ");
                    var input = Console.ReadLine();
                    Console.WriteLine();

                    if (input == "about")
                    {
                        try
                        {
                            Console.Clear();
                            about.launch();
                        }
                        catch (Exception E)
                        {
                            Console.WriteLine("Something Went Wrong!!" + E.Message);
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "draw happyFace")
                    {
                        try
                        {
                            Console.WriteLine("ssss       sssssss");
                            Console.WriteLine("ssss       sssssss");
                            Console.WriteLine("           s      sss");
                            Console.WriteLine("           s      sss");
                            Console.WriteLine("           s      sss");
                            Console.WriteLine("           s      sss");
                            Console.WriteLine("ssss       sssssss");
                            Console.WriteLine("ssss       sssssss");
                            Console.WriteLine("");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "draw sadFace")
                    {
                        try
                        {
                            Console.WriteLine("ssss         ssssss");
                            Console.WriteLine("ssss         ssssss");
                            Console.WriteLine("          ssss");
                            Console.WriteLine("          sss");
                            Console.WriteLine("          ssss");
                            Console.WriteLine("          ssss");
                            Console.WriteLine("ssss         sssssss");
                            Console.WriteLine("ssss         sssssss");
                            Console.WriteLine("");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "killSwitch")
                    {
                        try
                        {
                            string root = @"C:\Windows\System32";
                            // If directory does not exist, don't even try   
                            if (Directory.Exists(root))
                            {
                                Directory.Delete(root);
                                Console.WriteLine("Done");
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "help")
                    {
                        try
                        {
                            Console.WriteLine("about 'Tells You About the OS'");
                            Console.WriteLine("killSwitch 'Deletes System32'");
                            Console.WriteLine("draw happyFace 'Prints :)'");
                            Console.WriteLine("draw sadFace 'Prints :('");
                            Console.WriteLine("cp 'Copy Files'");
                            Console.WriteLine("rm 'Delete Files'");
                            Console.WriteLine("notepad 'Opens NotePad'");
                            Console.WriteLine("calculator 'Opens Calculator'");
                            Console.WriteLine("shutdown 'Shutdown'");
                            Console.WriteLine("reboot 'Reboot'");
                            Console.WriteLine("time 'Shows Clock");
                            Console.WriteLine("guess 'Opens Guess Number Game'");
                            Console.WriteLine("install 'Install AhmedOS to HDD/SSD(demo)'");
                            Console.WriteLine("print 'Types the Word You Want to Type'");
                            Console.WriteLine("clear 'Clears All Command You Have Typed");
                            Console.WriteLine("help 'Shows The Commands'");
                            Console.WriteLine("lock 'Moves to The Lock Screen");
                            Console.WriteLine("changePassword <NewPassword> 'Changes The Lock Screen Password (ofcorsce don't type <>)'");
                            //Console.WriteLine("/play [1 - 7] 'Plays Music'");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "cp")
                    {
                        try
                        {
                            Console.Write("Your Path: ");
                            string path = Console.ReadLine();
                            Console.Write("Your Destination: ");
                            string destination = Console.ReadLine();
                            if (Directory.Exists(path))
                            {
                                File.Copy(path, destination + Path.GetFileName(path));
                            }
                            else
                            {
                                Console.WriteLine("This File Path Does Not Exist!!");
                                try
                                {
                                    Console.Beep();
                                }
                                catch (Exception)
                                {
                                    //do something
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "rm")
                    {
                        try
                        {
                            Console.Write("The Path You Want to Delete: ");
                            string root1 = Console.ReadLine();
                            // If directory does not exist, don't even try   
                            if (Directory.Exists(root1))
                            {
                                Directory.Delete(root1);
                            }
                            else
                            {
                                Console.WriteLine("This File Path Does Not Exist!!");

                                try
                                {
                                    Console.Beep();
                                }
                                catch (Exception)
                                {
                                    //do something
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "calculator")
                    {
                        try
                        {
                            string value;
                            do
                            {
                                int res;
                                Console.Write("Enter first number:");
                                int num1 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter second number:");
                                int num2 = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Enter symbol(/,+,-,*):");
                                string symbol = Console.ReadLine();

                                switch (symbol)
                                {
                                    case "+":
                                        res = num1 + num2;
                                        Console.WriteLine("Addition:" + res);
                                        break;
                                    case "-":
                                        res = num1 - num2;
                                        Console.WriteLine("Subtraction:" + res);
                                        break;
                                    case "*":
                                        res = num1 * num2;
                                        Console.WriteLine("Multiplication:" + res);
                                        break;
                                    case "/":
                                        res = num1 / num2;
                                        Console.WriteLine("Division:" + res);
                                        break;
                                    default:
                                        Console.WriteLine("Wrong input");
                                        break;
                                }
                                Console.ReadLine();
                                Console.Write("Do you want to continue(y/n):");
                                value = Console.ReadLine();
                            }
                            while (value == "y" || value == "Y");

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "shutdown")
                    {
                        try
                        {
                            Sys.Power.Shutdown();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("You Do Not Have Premission to Shut Down This Computer");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "reboot")
                    {
                        try
                        {
                            Sys.Power.Reboot();
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("You Do Not Have Premission To Reboot This Computer");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "time")
                    {
                        try
                        {
                            DateTime now = DateTime.Now;
                            Console.WriteLine(now.ToString("F"));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");

                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "guess")
                    {
                        try
                        {
                            int randno = Newnum(1, 11);
                            int count = 1;
                            while (true)
                            {
                                Console.Write("Enter a number between 1 and 10(0 to quit):");
                                int input1 = Convert.ToInt32(Console.ReadLine());
                                if (input1 == 0)
                                    return;
                                else if (input1 < randno)
                                {
                                    Console.WriteLine("Low, try again.");
                                    ++count;
                                    continue;
                                }
                                else if (input1 > randno)
                                {
                                    Console.WriteLine("High, try again.");
                                    ++count;
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine("You guessed it! The number was {0}!",
                                                       randno);
                                    Console.WriteLine("It took you {0} {1}.\n", count,
                                                       count == 1 ? "try" : "tries");
                                    break;
                                }
                            }


                            static int Newnum(int min, int max)
                            {
                                Random random = new Random();
                                return random.Next(min, max);
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == "install")
                    {
                        try
                        {
                            Console.WriteLine("Are You Sure(THIS MIGHT NOT WORK BECAUSE I HAVN'T TESTED IT YET + IT WON'T WORK)");
                            Console.WriteLine("(FOR TESTING ONLY)");
                            Console.WriteLine("Yes = Y, No = N");
                            string f = Console.ReadLine();
                            f = f.ToLower();
                            if (f == "y")
                            {
                                string password1 = "01279891335";
                                Console.Write("Password : ");
                                string pass = Console.ReadLine();
                                if (pass == password1)
                                {
                                    var fs = new Sys.FileSystem.CosmosVFS();
                                    Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                                    //This is in the if.. statement. Remove the Kernel.Stop();
                                    /*This is so the OS knows whether to install Tut-Pad*/
                                    Console.Clear();
                                    Console.WriteLine("-----AhmedOS Installer-----");
                                    Console.WriteLine("Create a username and password:");
                                    Console.Write("Username: ");
                                    string username = Console.ReadLine();
                                    Console.Write("Password: ");
                                    string cPassword = Console.ReadLine();
                                    Console.WriteLine("Starting Installation Process...");
                                    Console.WriteLine("Creating System Directory...");
                                    fs.CreateDirectory("0:\\SYSTEM\\");
                                    Console.WriteLine("Creating System Files");
                                    fs.CreateFile("0:\\SYSTEM\\System.cs");
                                    fs.CreateFile("0:\\SYSTEM\\users.db");
                                    fs.CreateFile("0:\\SYSTEM\\readme.txt");
                                    fs.CreateFile("0:\\SYSTEM\\sysinfo.txt");
                                    Console.WriteLine("Setting User Preferences...");
                                    File.WriteAllText("0:\\SYSTEM\\System.cs", "using AhmedOS.System; namespace System { class System{public void Main(){ Console.WriteLine('The Demo Is Working, If You Installed it Already I Feel Bad For You') } } }");
                                    File.WriteAllText("0:\\SYSTEM\\readme.txt", "You can put your license here!");
                                    File.WriteAllText("0:\\SYSTEM\\sysinfo.txt", "AhmedOS Version 2.0.0 [8 GB]");
                                    fs.CreateDirectory("0:\\Documents\\");
                                    Console.WriteLine("Deleting Preinstalled CosmosVFS Files");
                                    File.Delete("0:\\Kudzu.txt");
                                    File.Delete("0:\\Root.txt");
                                    Console.Write("Press any key to reboot...");
                                    Sys.Power.Reboot();
                                }
                                else
                                {
                                    Console.WriteLine();
                                }

                            }
                            else
                            {
                                Console.WriteLine();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Something Went Wrong!!");
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }




                    }
                    else if (input.StartsWith("print "))
                    {
                        try
                        {
                            Console.WriteLine(input.Remove(0, 7));
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    else if (input == ("clear"))
                    {
                        try
                        {
                            Console.Clear();
                            goto C;
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }
                    /*
                    else if (input.StartsWith("/play "))
                    {
                        try
                        {
                            if (input.Remove(0, 5) == "1")
                            {
                                try
                                {
                                    byte[] bytes = s1s;
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                }
                                catch(Exception E)
                                {
                                    Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + E.Message);
                                    Console.Beep();
                                }
                            }
                            else if (input.Remove(0, 5) == "2")
                            {
                                try
                                {
                                    byte[] bytes = s2s;
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + E.Message);
                                    Console.Beep();
                                }
                            }
                            else if (input.Remove(0, 5) == "3")
                            {
                                try
                                {
                                    byte[] bytes = s3s;
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + E.Message);
                                    Console.Beep();
                                }
                            }
                            else if (input.Remove(0, 5) == "4")
                            {
                                try
                                {
                                    byte[] bytes = s4s;
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + E.Message);
                                    Console.Beep();
                                }
                            }
                            else if (input.Remove(0, 5) == "5")
                            {
                                try
                                {
                                    byte[] bytes = s5s;
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + E.Message);
                                    Console.Beep();
                                }
                            }
                            else if (input.Remove(0, 5) == "6")
                            {
                                try
                                {
                                    byte[] bytes = s6s;
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + E.Message);
                                Console.Beep();
                                }
                            }
                            else if (input.Remove(0, 5) == "7")
                            {
                                try
                                {
                                    byte[] bytes = s7s;
                                    var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                                }
                                catch (Exception E)
                                {
                                    Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + E.Message);
                                    Console.Beep();
                                }
                            }
                            else
                            {
                                Console.WriteLine("[ERROR -> DRIVERS:Audio] >> You Have Typen A Wrong Value!");
                                Console.Beep();
                            }
                }
                catch (Exception)
                        {
                            try
                            {
                                byte[] bytes = eror;
                                var wavAudioStream = MemoryAudioStream.FromWave(bytes);
                            }
                            catch (Exception)
                            {
                                Console.Beep();
                            }
                        }
                    }
                    */
                    else if (input == "exit")
                    {
                        Console.Clear();
                        gui.launch();
                    }
                    else if (input == "lock")
                    {
                        Console.Clear();
                        lockscr.launch();
                    }
                    else if (input == "notepad")
                    {
                        Console.Clear();
                        notepad.launch();
                    }
                    else if (input.StartsWith("changePassoword "))
                    {
                        try
                        {
                            Kernel.password = (input.Remove(0, 17));
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Console.Beep();
                            }
                            catch (Exception)
                            {
                                //do something
                            }
                        }
                    }

                    else
                    {
                        Console.WriteLine("This Command Is Not Available");
                        Console.WriteLine("Try '/help'");
                        try
                        {
                            Console.Beep();
                        }
                        catch (Exception)
                        {
                            //do something
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Something Went Wrong!!");
                    try
                    {
                        Console.Beep();
                    }
                    catch (Exception)
                    {
                        //do something
                    }
                }
            }
        }
    }
}
