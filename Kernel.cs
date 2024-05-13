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
using System.Xml;

namespace AhmedOS
{
    public class Kernel : Sys.Kernel
    {

        public static VGAScreen VScreen = new VGAScreen();
        public abstract class AudioDriver;
        Canvas canvas;
        public static bool Audio = false;
        public static string password;
        public static string pcname;
        public static string colors = "white";
        public static void custom()
        {
            if (Kernel.colors == "white")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (Kernel.colors == "black")
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else if (Kernel.colors == "blueb")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (Kernel.colors == "red")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (Kernel.colors == "yellow")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (Kernel.colors == "bluew")
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (Kernel.colors == "green")
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        protected override void BeforeRun()
        {

            Console.Clear();
            pcname = "ahmedos_pc";
            // Draw the image on the screen
            /*Canvas canvas = FullScreenCanvas.GetFullScreenCanvas();
            canvas.Clear(Color.Blue);*/
            Encoding.RegisterProvider(CosmosEncodingProvider.Instance);
            Console.OutputEncoding = CosmosEncodingProvider.Instance.GetEncoding(437);
            /*Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║    __    ____  ___    ____  _____   ________  ║");
            Console.WriteLine("║   / /   / __ \\/   |  / __ \\/  _/ | / / ____/  ║");
            Console.WriteLine("║  / /   / / / / /| | / / / // //  |/ / / __    ║");
            Console.WriteLine("║ / /___/ /_/ / ___ |/ /_/ // // /|  / /_/ /    ║");
            Console.WriteLine("║/_____/\\____/_/  |_/_____/___/_/ |_/\\____/     ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");*/
            Console.CursorVisible = false;
            string i = "AhmedOS";
            Console.SetCursorPosition((Console.WindowWidth - i.Length) / 2, Console.CursorTop + ((Console.WindowHeight / 2)));
            Console.WriteLine(i);
            Thread.Sleep(8000);
            /*try
            {
                string format = "yyyy";
                string CurrentYear = DateTime.Now.ToString(format);
                int year = Convert.ToInt32(CurrentYear);
                if (year <= 2020)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[INFO -> Time] >> Time Is Not Set");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any Key to ignore the Message");
                    Console.ReadKey();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[INFO -> Time] >> Time is Set");
                }
            }
            catch (Exception E)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[INFO -> Time] >> " + E.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any Key to ignore the Message");
                Console.ReadKey();
            }
            Thread.Sleep(5000);
            */try
            {
                var driver = AC97.Initialize(8192);
                var mixer = new AudioMixer();
                bool AudioEnabled = true;
                Audio = true;
            }
            catch (InvalidOperationException)
            {
                bool AudioEnabled = false;
                Audio = false;
            }/*
            catch (Exception EX)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR -> DRIVERS:Audio] >> " + EX.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any Key to ignore the Message");
                Console.ReadKey();
            }
            Thread.Sleep(5000);
            try
            {
            }
            catch (Exception EX)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR -> DRIVERS:Keyboard] >> " + EX.Message);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any Key to ignore the Message");
                Console.ReadKey();
            }
            /*try
            {
                canvas = FullScreenCanvas.GetFullScreenCanvas();
                canvas.Mode = new Mode(320, 200, ColorDepth.ColorDepth32);
                Bitmap image = new Bitmap(wlpapr);
                canvas.DrawImage(image, 0, 0);
                canvas.DrawString("If You See This Say YAAAAAAYYY!!", PCScreenFont.Default, new Pen(Color.White), 0, 0);
                canvas.Clear();

            }
            catch (Exception E)
            {
                Console.WriteLine("[ERROR -> VIDEO] .. " + E.Message);
                Console.WriteLine("Press Any Key To Shut Down");
                Console.ReadKey();
                Sys.Power.Shutdown();
            }*/

        }
        protected override void Run()
        {
            //Type Password
            Console.Clear();
            bool trupass = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Type Your Password : ");
            Console.CursorVisible = true;
            password = Console.ReadLine();
            if (password.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                gui.launch();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                lockscr.launch();
            }
        }
    }
}


