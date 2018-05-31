using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using PIT_Server.Resources;
namespace PIT_Server
{
    class Program
    {
        //public static int ConcurrencyLevel=50;
        //public static int Capacity = 10000;
        public static bool Opened = false;
        static void Main(string[] args)
        {
#if !DEBUG
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
            Console.Title = "P:IT Server";
            Thread t = new Thread(new ThreadStart(Input));
            t.Start();
            Logger.ShowOnAccept = true;
            Lobby.Init();
            Database.Init();
            Server s = new Server(2502);
            s.Start();
        }

        private static void CurrentDomain_UnhandledException1(object sender, UnhandledExceptionEventArgs e)
        {
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Process.Start("PIT_Server.exe");
            Process.GetCurrentProcess().Kill();
        }

        static ControlPanel panel;
        static void Input()
        {
            ConsoleKey key;
            while (true)
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.I:
                        DrawInfo();
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        break;
                    case ConsoleKey.L:
                        Logger.Write();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.M:
                        Logger.ShowOnAccept = !Logger.ShowOnAccept;
                        Console.WriteLine("AutoShow Log turned: " + (Logger.ShowOnAccept == true ? "On" : "Off"));
                        break;
                    case ConsoleKey.NumPad2:
                        Scene.SPEED = (Scene.SPEED - 100 < 1) ? 1 : (Scene.SPEED - 100);
                        Logger.LogIt("Speed:" + Scene.SPEED);
                        break;
                    case ConsoleKey.NumPad8:
                        Scene.SPEED += 100;
                        Logger.LogIt("Speed:" + Scene.SPEED);
                        break;
                    case ConsoleKey.NumPad0:
                        Scene.SPEED = 1;
                        Logger.LogIt("Speed:" + Scene.SPEED);
                        break;
                    case ConsoleKey.D:
                        PSettings.Debug = !PSettings.Debug;
                        Logger.LogIt("Debug mode: " + PSettings.Debug);
                        break;
                    case ConsoleKey.P:
                        Scene.PAUSE = !Scene.PAUSE;
                        Logger.LogIt("Pause: " + Scene.PAUSE);
                        break;
                    case ConsoleKey.S:
                        Logger.SaveLogs();
                        break;
                    case ConsoleKey.T:
                        panel = new ControlPanel();
                        Task.Factory.StartNew(() => Application.Run(panel), TaskCreationOptions.LongRunning);
                        break;
                    default:
                        break;
                }

            }
        }
        static void DrawInfo()
        {
            Console.WriteLine("Visitors: " + VisitorBase.GetVisitors().Count);
            foreach (var v in VisitorBase.GetVisitors())
                Console.WriteLine("Visitor: " + v.Name);

        }
    }
}
