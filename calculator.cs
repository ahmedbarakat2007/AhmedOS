using IL2CPU.API;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Cosmos.Core;
using System.Linq.Expressions;
using System.Data;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AhmedOS
{
    internal static class calculator
    {
        public static int vv;
        public static int bb;
        public static string RepeatStrBuilder(this string text, uint n)
        {
            return new StringBuilder(text.Length * (int)n)
              .Insert(0, text, (int)n)
              .ToString();
        }

        public static string hh;
        static int GetIntegerDigitCountString(int value)
        {
            // Version 1: get digit count with ToString.
            return value.ToString().Length;
        }
        public static string ww;
        public static void launch()
        {


        Calculator:
            Kernel.custom();
            //Lock Screen
            Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
            Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
            Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");
            Console.WriteLine();
            Console.Write("╔═══════════════════════════╗\n");
            Console.Write("║                           ║\n");
            Console.Write("╚═══════════════════════════╝\n");
            while (true)
            {
                try
                {
                    string gg = Console.ReadLine();
                    if (gg == "exit")
                    {
                        Console.Clear();
                        gui.launch();
                    }
                    else
                    {
                        try
                        {
                            bb = Convert.ToInt16(gg);
                            Console.Clear();
                            Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                            Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                            Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");
                            if (GetIntegerDigitCountString(bb) == 1)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + "                         ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 2)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + "                        ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 3)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + "                       ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 4)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + "                      ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 5)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + "                     ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (bb == 0)
                            {
                                Console.Clear();
                                goto Calculator;
                            }
                            else
                            {
                                Console.Clear();
                                goto Calculator;
                            }
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                            Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                            Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");

                            Console.WriteLine();
                            Console.Write("╔══════════════╗\n");
                            Console.Write("║  Math Error  ║\n");
                            Console.Write("╚══════════════╝\n");
                            Console.ReadKey();
                            Console.Clear();
                            goto Calculator;
                        }
                            string DD = Console.ReadLine();
                        if (DD == "exit")
                        {
                            Console.Clear();
                            gui.launch();
                        }
                        else
                        {
                            
                            Console.Clear();
                            Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                            Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                            Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");

                            if (GetIntegerDigitCountString(bb) == 1)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + " " + DD + "                       ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 2)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + " " + DD + "                      ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 3)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + " " + DD + "                     ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 4)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + " " + DD + "                    ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else if (GetIntegerDigitCountString(bb) == 5)
                            {
                                Console.WriteLine();
                                Console.Write("╔═══════════════════════════╗\n");
                                Console.Write("║ " + bb + " " + DD + "                   ║\n");
                                Console.Write("╚═══════════════════════════╝\n");
                            }
                            else
                            {
                                Console.Clear();
                                goto Calculator;
                            }
                            string BB = Console.ReadLine();
                            vv = Convert.ToInt16(BB);
                        }
                        try
                        {
                            if (DD == "+")
                            {
                                int result = bb + vv;
                                uint hh = Convert.ToUInt16(GetIntegerDigitCountString(result));
                                string ww = RepeatStrBuilder("═", hh);
                                Console.Clear();
                                Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                                Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                                Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");

                                Console.WriteLine();
                                Console.Write("╔═" + ww + "═╗\n");
                                Console.Write("║ " + result + " ║\n");
                                Console.Write("╚═" + ww + "═╝\n");
                                Console.ReadKey();
                                Console.Clear();
                                goto Calculator;
                            }
                            else if (DD == "-")
                            {
                                int result = bb - vv;
                                Console.Clear();
                                Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                                Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                                Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");

                                Console.WriteLine();
                                Console.Write("╔═" + ww + "══╗\n");
                                Console.Write("║ " + result + " ║\n");
                                Console.Write("╚═" + ww + "══╝\n");
                                Console.ReadKey();
                                Console.Clear();
                                goto Calculator;
                            }
                            else if (DD == "*")
                            {
                                int result = bb * vv;
                                Console.Clear();
                                Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                                Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                                Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");

                                Console.WriteLine();
                                Console.Write("╔═" + ww + "══╗\n");
                                Console.Write("║ " + result + " ║\n");
                                Console.Write("╚═" + ww + "══╝\n");
                                Console.ReadKey();
                                Console.Clear();
                                goto Calculator;
                            }
                            else if (DD == "/")
                            {
                                int result = bb / vv;
                                Console.Clear();
                                Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                                Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                                Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");

                                Console.WriteLine();
                                Console.Write("╔═" + ww + "══╗\n");
                                Console.Write("║ " + result + " ║\n");
                                Console.Write("╚═" + ww + "══╝\n");
                                Console.ReadKey();
                                Console.Clear();
                                goto Calculator;
                            }
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                            Console.Write("║ Calculator                                                  ║  'exit' Exit  ║\n");
                            Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");

                            Console.WriteLine();
                            Console.Write("╔══════════════╗\n");
                            Console.Write("║  Math Error  ║\n");
                            Console.Write("╚══════════════╝\n");
                            Console.ReadKey();
                            Console.Clear();
                            goto Calculator;
                        }
                    }

                }
                catch (Exception)
                {
                    Console.Clear();
                    goto Calculator;
                }

            }
        }
    }
}
