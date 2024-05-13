using IL2CPU.API;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Cosmos.Core;


namespace AhmedOS
{
    internal class paint
    {
        public static void launch()
        {
            static string HideCharacter()

            {
                ConsoleKeyInfo key;
                string code = "";
                do
                {
                    key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        gui.launch();
                    }
                    else if (key.Key == ConsoleKey.Spacebar)
                    {
                        Console.Write(" ");
                    }
                    else if (key.Key == ConsoleKey.D1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (key.Key == ConsoleKey.D2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (key.Key == ConsoleKey.D3)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (key.Key == ConsoleKey.D4)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (key.Key == ConsoleKey.D5)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (key.Key == ConsoleKey.D6)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else if (key.Key == ConsoleKey.D7)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (key.Key == ConsoleKey.D8)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if (key.Key == ConsoleKey.D9)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        if (Char.IsNumber(key.KeyChar) || Char.IsLetter(key.KeyChar))
                        {
                            Console.Write("█");
                            code += key.KeyChar;
                        }
                    }
                    } while (key.Key != ConsoleKey.Enter) ;
                    return code;
            }


        About:
            Kernel.custom();
            //Lock Screen
            Console.Write("╔══════════════════════════════════════════════════════════════╦══════════════╗\n");
            Console.Write("║ Paint                                                        ║  'ESC' Exit  ║\n");
            Console.Write("╚══════════════════════════════════════════════════════════════╩══════════════╝\n");
            Console.WriteLine();
            while (true)
            {
                string gg = HideCharacter();
                
            }
        }
    }
}

