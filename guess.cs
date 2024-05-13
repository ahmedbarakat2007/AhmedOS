using IL2CPU.API;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Cosmos.Core;
using System.Threading;


namespace AhmedOS
{
    internal class guessgame
    {
        static int GetIntegerDigitCountString(int value)
        {
            // Version 1: get digit count with ToString.
            return value.ToString().Length;
        }
        public static void launch()
        {
        main:
            try
            {
                Kernel.custom();
                //Lock Screen
                Console.Write("╔════════════════════════════════════════════════════════════════╦════════════╗\n");
                Console.Write("║ Guess Number                                                   ║  '0' Exit  ║\n");
                Console.Write("╠════════════════════════════════════════════════════════════════╩════════════╣\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("║   Choose a Number Between 1- 10                                             ║\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("║   ( If You Guess the Number Right You Win )                                 ║\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
                Console.WriteLine();

                int randno = Newnum(1, 11);
                int count = 1;
                while (true)
                {
                    int input1 = Convert.ToInt32(Console.ReadLine());
                    if (input1 == 0)
                    {
                        Console.Clear();
                        gui.launch();
                    }
                    else if (input1 < randno)
                    {
                        Console.WriteLine("╔════════════════════╗");
                        Console.WriteLine("║   Low, try again.  ║");
                        Console.WriteLine("╚════════════════════╝");
                        Thread.Sleep(2000);
                        Console.Clear();
                        goto main;
                        ++count;
                        continue;
                    }
                    else if (input1 > randno)
                    {
                        Console.WriteLine("╔═════════════════════╗");
                        Console.WriteLine("║   High, try again.  ║");
                        Console.WriteLine("╚═════════════════════╝\n");
                        Thread.Sleep(2000);
                        Console.Clear();
                        goto main;
                        ++count;
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("╔════════════════════════════════════╗");
                        Console.WriteLine("║  You guessed it! The number was {0}  ║", randno);
                        Console.WriteLine("╚════════════════════════════════════╝\n");
                        Console.ReadKey();
                        Console.Clear();
                        goto main;
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
                Console.Clear();
                goto main;
            }
        }
    }

}


