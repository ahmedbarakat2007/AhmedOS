using IL2CPU.API;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Cosmos.Core;
using System.Security.Cryptography.X509Certificates;

namespace AhmedOS
{
    internal class option
    {
        public static void launch()
        {
        Option:
            Kernel.custom();
            //Lock Screen
            Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
            Console.Write("║ Option                                                      ║  'ESC'  Exit  ║\n");
            Console.Write("╠═════════════════════════════════════════════════════════════╩═══════════════╣\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("║   '1'  Password Change                                                      ║\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("║   '2'  PC-Name Change                                                       ║\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("║   '3'  Customize                                                            ║\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("║   '4'  About                                                                ║\n");
            Console.Write("║                                                                             ║\n");
            Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
            Console.WriteLine();

            var gg = Console.ReadKey(true).Key;
            if (gg == ConsoleKey.Escape)
            {
                Console.Clear();
                gui.launch();
            }
            else if (gg == ConsoleKey.D1)
            {
                Console.Clear();
                Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                Console.Write("║ Option                                                      ║  'exit' Exit  ║\n");
                Console.Write("╠═════════════════════════════════════════════════════════════╩═══════════════╣\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("║   Type Your New Password                                                    ║\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
                Console.WriteLine();
                Console.Write(">");
                //Kernel.password
                string hh = Console.ReadLine();
                if (hh == "exit")
                {
                    Console.Clear();
                    goto Option;
                }
                else
                {
                    Kernel.password = hh;
                    Console.Clear();
                    Console.SetCursorPosition((Console.WindowWidth - 8) / 2, ((Console.WindowHeight / 2) - 1));
                    Console.Write("╔══════╗\n");
                    Console.SetCursorPosition((Console.WindowWidth - 8) / 2, ((Console.WindowHeight / 2)));
                    Console.Write("║ Done ║\n");
                    Console.SetCursorPosition((Console.WindowWidth - 8) / 2, ((Console.WindowHeight / 2) + 1));
                    Console.Write("╚══════╝\n");
                    Console.ReadKey();
                    Console.Clear();
                    goto Option;
                }
                
            }
            else if (gg == ConsoleKey.D2)
            {
                A7A:
                Console.Clear();
                Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                Console.Write("║ Option                                                      ║  'exit' Exit  ║\n");
                Console.Write("╠═════════════════════════════════════════════════════════════╩═══════════════╣\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("║   Type The New PC Name                                                      ║\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("║   ( It Must Contain 10 Characters )                                         ║\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
                Console.WriteLine();
                Console.Write(">");
                string aa = Console.ReadLine();
                if (aa == "exit")
                {
                    Console.Clear();
                    goto Option;
                }
                else
                {
                    if (aa.Length == 10)
                    {
                        Kernel.pcname = aa;
                        Console.Clear();
                        Console.SetCursorPosition((Console.WindowWidth - 8) / 2, ((Console.WindowHeight / 2) - 1));
                        Console.Write("╔══════╗\n");
                        Console.SetCursorPosition((Console.WindowWidth - 8) / 2, ((Console.WindowHeight / 2)));
                        Console.Write("║ Done ║\n");
                        Console.SetCursorPosition((Console.WindowWidth - 8) / 2, ((Console.WindowHeight / 2) + 1));
                        Console.Write("╚══════╝\n");
                        Console.ReadKey();
                        Console.Clear();
                        goto Option;
                    }
                    else
                    {
                        Console.Clear();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition((Console.WindowWidth - 32) / 2,((Console.WindowHeight / 2) - 1));
                        Console.Write("╔══════════════════════════════╗\n");
                        Console.SetCursorPosition((Console.WindowWidth - 32) / 2,((Console.WindowHeight / 2)));
                        Console.Write("║ It Must Contain 10 Character ║\n");
                        Console.SetCursorPosition((Console.WindowWidth - 32) / 2,((Console.WindowHeight / 2) + 1));
                        Console.Write("╚══════════════════════════════╝\n");
                        Console.ReadKey();
                        goto A7A;
                    }
                }
            }
            else if (gg == ConsoleKey.D3)
            {
            A7A:
                Console.Clear();
                Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
                Console.Write("║ Option                                                      ║  'ESC'  Exit  ║\n");
                Console.Write("╠═════════════════════════════════════════════════════════════╩═══════════════╣\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("║   Choose a Foreground Color                                                 ║\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("║   '1' White, Black                                                          ║\n");
                Console.Write("║   '2' Black, White                                                          ║\n");
                Console.Write("║   '3' Red, Black                                                            ║\n");
                Console.Write("║   '4' Green, Black                                                          ║\n");
                Console.Write("║   '5' Blue, Black                                                           ║\n");
                Console.Write("║   '6' White, Blue                                                           ║\n");
                Console.Write("║   '7' Yellow, Black                                                         ║\n");
                Console.Write("║                                                                             ║\n");
                Console.Write("╚═════════════════════════════════════════════════════════════════════════════╝\n");
                Console.WriteLine();
                Console.Write(">");
                var ab = Console.ReadKey(true).Key;
                if (ab == ConsoleKey.Escape)
                {
                    Console.Clear();
                    goto Option;
                }
                else if (ab == ConsoleKey.D1)
                {
                    Kernel.colors = "white";
                    Console.Clear();
                    goto Option;
                }
                else if (ab == ConsoleKey.D2)
                {
                    Kernel.colors = "black";
                    Console.Clear();
                    goto Option;
                }
                else if (ab == ConsoleKey.D3)
                {
                    Kernel.colors = "red";
                    Console.Clear();
                    goto Option;
                }
                else if (ab == ConsoleKey.D4)
                {
                    Kernel.colors = "green";
                    Console.Clear();
                    goto Option;
                }
                else if (ab == ConsoleKey.D5)
                {
                    Kernel.colors = "blueb";
                    Console.Clear();
                    goto Option;
                }
                else if (ab == ConsoleKey.D6)
                {
                    Kernel.colors = "bluew";
                    Console.Clear();
                    goto Option;
                }
                else if (ab == ConsoleKey.D7)
                {
                    Kernel.colors = "yellow";
                    Console.Clear();
                    goto Option;
                }
                else
                {
                    Console.Clear();
                    goto A7A;
                }
            }
            else if (gg == ConsoleKey.D4)
            {
                Console.Clear();
                about.launch();
            }
            else
            {
                Console.Clear();
                goto Option;
            }
        }
    }
}

