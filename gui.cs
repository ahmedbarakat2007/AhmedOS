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
    internal class gui
    {
        public static VGAScreen VScreen = new VGAScreen();
        public abstract class AudioDriver;
        Canvas canvas;
        public static bool Audio = false;
        public static string ramcount;

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
        public static string ram1;
        public static void launch()
        {

        C:
            while (true)
            {
                try
                {
                    string dd_mm_yyyy = DateTime.Now.ToString("dd/MM/yyyy").Replace('-', '/');
                    int ram = Convert.ToInt16(GetAmountOfRAM());
                    ramcount = "Memory :UNKNOWN";
                    if (GetIntegerDigitCountString(ram) == 4)
                    {
                        ram1 = Convert.ToString(" " + ram);
                        ramcount = "Memory : " + ram + "MB";
                    }
                    else if (GetIntegerDigitCountString(ram) == 3)
                    {
                        ram1 = Convert.ToString("  " + ram);
                        ramcount = "Memory : " + ram1 + "MB";
                    }
                    else
                    {
                        ram1 = ("UNKNOWN");
                    }
                GUI:
                    Kernel.custom();
                    //Main OS
                    Console.Write("╔═════════╦══════════╦═══════════╦══════════╦═════════════════════════════════╗\n");
                    Console.Write("║ '1' AOS ║ '2' Apps ║ '3' Games ║ '4' Help ║                  " + dd_mm_yyyy + "     ║\n");
                    Console.Write("╠═════════╩══════════╩════════╦══╩══════════╩═════════════════════════════════╣\n");
                    Console.Write("║                             ║                                               ║\n");
                    Console.Write("║   █████╗  ██████╗ ███████╗  ║     PC-Name : " + Kernel.pcname + "                      ║\n");
                    Console.Write("║  ██╔══██╗██╔═══██╗██╔════╝  ║                                               ║\n");
                    Console.Write("║  ███████║██║   ██║███████╗  ║     " + ramcount + "                           ║\n");
                    Console.Write("║  ██╔══██║██║   ██║╚════██║  ║                                               ║\n");
                    Console.Write("║  ██║  ██║╚██████╔╝███████║  ║     ████████████████                          ║\n");
                    Console.Write("║  ╚═╝  ╚═╝ ╚═════╝ ╚══════╝  ║                                               ║\n");
                    Console.Write("║                             ║                                               ║\n");
                    Console.Write("╚═════════════════════════════╩═══════════════════════════════════════════════╝\n");
                    Console.WriteLine();
                    while (true)
                    {
                        var fun = Console.ReadKey(true).Key;
                        if (fun == ConsoleKey.D1)
                        {
                        F1:
                            Console.Clear();
                            Console.Write("╔═════════╦══════════╦═══════════╦══════════╦═════════════════════════════════╗\n");
                            Console.Write("║ '1' AOS ║ '2' Apps ║ '3' Games ║ '4' Help ║                  " + dd_mm_yyyy + "     ║\n");
                            Console.Write("╠══╦══════╩═════╦════╩════════╦══╩══════════╩═════════════════════════════════╣\n");
                            Console.Write("║  ║'5'    About║             ║                                               ║\n");
                            Console.Write("║  ║'6' Terminal║█╗ ███████╗  ║     PC-Name : " + Kernel.pcname + "                      ║\n");
                            Console.Write("║  ║'7'     Lock║██╗██╔════╝  ║                                               ║\n");
                            Console.Write("║  ║'8' ShutDown║██║███████╗  ║     " + ramcount + "                           ║\n");
                            Console.Write("║  ║'9'  Restart║██║╚════██║  ║                                               ║\n");
                            Console.Write("║  ║'ESC'   Exit║█╔╝███████║  ║     ████████████████                          ║\n");
                            Console.Write("║  ╚════════════╝═╝ ╚══════╝  ║                                               ║\n");
                            Console.Write("║                             ║                                               ║\n");
                            Console.Write("╚═════════════════════════════╩═══════════════════════════════════════════════╝\n");
                            Console.WriteLine();
                            var fun1 = Console.ReadKey(true).Key;
                            if (fun1 == ConsoleKey.Escape)
                            {
                                Console.Clear();
                                goto GUI;
                            }
                            else if (fun1 == ConsoleKey.D5)
                            {
                                Console.Clear();
                                about.launch();
                            }
                            else if (fun1 == ConsoleKey.D8)
                            {
                                Sys.Power.Shutdown();
                            }
                            else if (fun1 == ConsoleKey.D9)
                            {
                                Sys.Power.Reboot();
                            }
                            else if (fun1 == ConsoleKey.D7)
                            {
                                Console.Clear();
                                lockscr.launch();
                            }
                            else if (fun1 == ConsoleKey.D6)
                            {
                                Console.Clear();
                                terminal.launch();
                            }

                            else
                            {
                                Console.Clear();
                                goto GUI;
                            }

                        }
                        else if (fun == ConsoleKey.D2)
                        {
                        F2:
                            Console.Clear();
                            Console.Write("╔═════════╦══════════╦═══════════╦══════════╦═════════════════════════════════╗\n");
                            Console.Write("║ '1' AOS ║ '2' Apps ║ '3' Games ║ '4' Help ║                  " + dd_mm_yyyy + "     ║\n");
                            Console.Write("╠═════════╩══╦═══════╩════╦═══╦══╩══════════╩═════════════════════════════════╣\n");
                            Console.Write("║            ║'5'Caclulate║   ║                                               ║\n");
                            Console.Write("║   █████╗  █║'6'  NotePad║╗  ║     PC-Name : " + Kernel.pcname + "                      ║\n");
                            Console.Write("║  ██╔══██╗██║'7'  Options║╝  ║                                               ║\n");
                            Console.Write("║  ███████║██║'8'    Paint║╗  ║     " + ramcount + "                           ║\n");
                            Console.Write("║  ██╔══██║██║'ESC'   Exit║║  ║                                               ║\n");
                            Console.Write("║  ██║  ██║╚█╚════════════╝║  ║     ████████████████                          ║\n");
                            Console.Write("║  ╚═╝  ╚═╝ ╚═════╝ ╚══════╝  ║                                               ║\n");
                            Console.Write("║                             ║                                               ║\n");
                            Console.Write("╚═════════════════════════════╩═══════════════════════════════════════════════╝\n");
                            Console.WriteLine();
                            var fun1 = Console.ReadKey(true).Key;
                            if (fun1 == ConsoleKey.Escape)
                            {
                                Console.Clear();
                                goto GUI;
                            }
                            else if (fun1 == ConsoleKey.D5)
                            {
                                Console.Clear();
                                calculator.launch();
                            }
                            else if (fun1 == ConsoleKey.D6)
                            {
                                Console.Clear();
                                notepad.launch();
                            }
                            else if (fun1 == ConsoleKey.D7)
                            {
                                Console.Clear();
                                option.launch();
                            }
                            else if (fun1 == ConsoleKey.D8)
                            {
                                Console.Clear();
                                paint.launch();
                            }
                            else
                            {
                                Console.Clear();
                                goto GUI;
                            }

                        }
                        else if (fun == ConsoleKey.D3)
                        {
                        F3:
                            Console.Clear();
                            Console.Write("╔═════════╦══════════╦═══════════╦══════════╦═════════════════════════════════╗\n");
                            Console.Write("║ '1' AOS ║ '2' Apps ║ '3' Games ║ '4' Help ║                  " + dd_mm_yyyy + "     ║\n");
                            Console.Write("╠═════════╩══════════╩══╦════════╩════╦═════╩═════════════════════════════════╣\n");
                            Console.Write("║                       ║'5' Guess Num║                                       ║\n");
                            Console.Write("║   █████╗  ██████╗ ████║'6' TicTacToc║Name : " + Kernel.pcname + "                      ║\n");
                            Console.Write("║  ██╔══██╗██╔═══██╗██╔═║'7'     Snack║                                       ║\n");
                            Console.Write("║  ███████║██║   ██║████║'8'    PacMan║" + "ory :" + ram1 + "MB" +"                           ║\n");
                            Console.Write("║  ██╔══██║██║   ██║╚═══║'9'    Tetris║                                       ║\n");
                            Console.Write("║  ██║  ██║╚██████╔╝████║'ESC'    Exit║█████████████                          ║\n");
                            Console.Write("║  ╚═╝  ╚═╝ ╚═════╝ ╚═══╚═════════════╝                                       ║\n");
                            Console.Write("║                             ║                                               ║\n");
                            Console.Write("╚═════════════════════════════╩═══════════════════════════════════════════════╝\n");
                            Console.WriteLine();
                            var fun1 = Console.ReadKey(true).Key;
                            if (fun1 == ConsoleKey.Escape)
                            {
                                Console.Clear();
                                goto GUI;
                            }
                            else if (fun1 == ConsoleKey.D5)
                            {
                                Console.Clear();
                                guessgame.launch();
                            }
                            else if (fun1 == ConsoleKey.D7)
                            {
                                Console.Clear();
                                Console.WriteLine("Unfortunally, It's Not Available In This Version (Press any Key to Go Back)");
                                Console.ReadLine();
                                Console.Clear();
                                goto GUI;
                            }
                            else if (fun1 == ConsoleKey.D6)
                            {
                                Console.Clear();
                                tictactoc.Main();
                            }
                            else if (fun1 == ConsoleKey.D8)
                            {
                                Console.Clear();
                                Console.WriteLine("Unfortunally, It's Not Available In This Version (Press any Key to Go Back)");
                                Console.ReadLine();
                                Console.Clear();
                                goto GUI;
                            }
                            else if (fun1 == ConsoleKey.D9)
                            {
                                Console.Clear();
                                Console.WriteLine("Unfortunally, It's Not Available In This Version (Press any Key to Go Back)");
                                Console.ReadLine();
                                Console.Clear();
                                goto GUI;
                            }

                            else
                            {
                                Console.Clear();
                                goto GUI;
                            }
                        }
                        else if (fun == ConsoleKey.D4)
                        {
                        F4:
                            Console.Clear();
                            Console.Write("╔═════════════════════════════════════════════════════════════════════════════╗\n");
                            Console.Write("║  Help                                                                       ║\n");
                            Console.Write("╠═════════════════════════════════════════════════════════════════════════════╣\n");
                            Console.Write("║                                                                             ║\n");
                            Console.Write("║   Press on the Keys That Between '' in Keyboard to Navigate With Ahmed OS.  ║\n");
                            Console.Write("║                                                                             ║\n");
                            Console.Write("║                                                                             ║\n");
                            Console.Write("║   To Exit An App You Have To Look At The Top Left Corner You Will           ║\n");
                            Console.Write("║   You Wii Find The Key or The Command That You Have To Type To The          ║\n");
                            Console.Write("║                                                                             ║\n");
                            Console.Write("║   Press Any Key To Go Back To Main Screen                                   ║\n");
                            Console.Write("║                                                                             ║\n");
                            Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
                            Console.WriteLine();
                            Console.ReadKey();
                            Console.Clear();
                            goto GUI;
                        }
                        else
                        {
                            Console.Clear();
                            goto GUI;
                        }
                    }
                }
                catch (Exception TTRS)
                {
                    Console.WriteLine("ERROR : " + TTRS.Message);
                }
            }
        }
    }
}

