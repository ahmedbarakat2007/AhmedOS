using IL2CPU.API;
using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using Cosmos.Core;


namespace AhmedOS
{
    internal class notepad
    {
        public static void launch()
        {
        About:
            Kernel.custom();
            //Lock Screen
            Console.Write("╔═════════════════════════════════════════════════════════════╦═══════════════╗\n");
            Console.Write("║ NotePad                                                     ║  'exit' Exit  ║\n");
            Console.Write("╚═════════════════════════════════════════════════════════════╩═══════════════╝\n");
            Console.WriteLine();
            while (true)
            {
                string gg = Console.ReadLine();
                if (gg == "exit")
                {
                    Console.Clear();
                    gui.launch();
                }
            }
        }
    }
}

