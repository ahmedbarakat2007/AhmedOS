using IL2CPU.API;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Cosmos.Core;


namespace AhmedOS
{
    internal class lockscr
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

        Lock:
            Kernel.custom();
            //Lock Screen
            Console.Write("╔══════════════╦══════════════════════════════════════════════════════════════╗\n");
            Console.Write("║ 's' Shutdown ║                                                              ║\n");
            Console.Write("╠══════════════╩══════════════════════════════════════════════════════════════╣\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("║                                  .:-----:.                                  ║\n");
            Console.Write("║                               ---.       .---                               ║\n");
            Console.Write("║                             --    --:::--    --                             ║\n");
            Console.Write("║                            +    .+       +.    +                            ║\n");
            Console.Write("║                           +     -:       :-     +                           ║\n");
            Console.Write("║                          -:      +       +      :-                          ║\n");
            Console.Write("║                          =.       -------       .=                          ║\n");
            Console.Write("║                          -:                     :-                          ║\n");
            Console.Write("║                           +    :--:--=--:--:    +                           ║\n");
            Console.Write("║                            + .=.           .=. +                            ║\n");
            Console.Write("║                             -*               *-                             ║\n");
            Console.Write("║                               ---.       .---                               ║\n");
            Console.Write("║                                  .:--:--:.                                  ║\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("║                                  "+ Kernel.pcname +"                                 ║\n");
            Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
            Console.WriteLine();
        E:
            Console.WriteLine();
            Console.Write("Password : ");
            var pas = Console.ReadLine();
            var con = Kernel.password;
            if (pas == con)
            {
                Console.Clear();
                gui.launch();
            }
            else if (pas == "s")
            {
                Sys.Power.Shutdown();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("WRONG PASSWORD. TRY AGAIN!");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                goto Lock;
            }
        }
    }
}

