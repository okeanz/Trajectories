using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using PIT_Server;
namespace smallclient
{
    public class Programs
    {
        static void Main(string[] args)
        {
            Application.Run(new Settings());
            
        }
       
    }
    class handler : IMaster
    {
        public ClientTCP client;
        Character MyChar;
        public handler()
        {
            client = new ClientTCP("127.0.0.1", 2502, this);
            client.Connect();
            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(input));
            th.IsBackground = false;
            th.Start();
        }
        void input()
        {
            Console.WriteLine("Для помощи жмакни h");
            Console.Write("Интерактивное подключение(0) или вручную(1) ?:");
            int con = Convert.ToInt32(Console.ReadLine());
            if (con == 0)
            {
                Console.Write("Логинимся user? [1-6]:");
                string login = Console.ReadLine();
                client.Send("000_" + login + "_" + login);
                client.Send("001_1");
                client.Send("006");
                Thread.Sleep(50);
                Console.Write("\nПредпочитаемая профессия [1-5]:");
                
                int Choise = Convert.ToInt32(Console.ReadLine());
                if (Choise == 1)
                {
                    Console.Write("\nКакой корабль, кэп ? [");
                    foreach (var s in MyChar.Ships)
                        Console.Write(s + " ");
                    Console.Write("]:");
                    client.Send("008_1_" + Console.ReadLine());

                }
                else
                {
                    string tosend = "008";
                    string[] tt = new string[5];
                    for (int i = 0; i < 4; i++)
                    {
                        tt[i] = "_0";
                    }
                    tt[Choise - 1] = "_1";
                    tosend += string.Concat(tt);
                    client.Send(tosend);
                }
                client.Send("009");

            }
            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.H:
                        Console.WriteLine("Help:");
                        Console.WriteLine("C: очистить экран");
                        Console.WriteLine("S: ввести отправляемое сообщение вручную");
                        break;
                    case ConsoleKey.S:
                        Console.Write("Отправить: ");
                        client.Send(Console.ReadLine());
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        break;
                    default:
                        break;
                }
            }
        }
        public void OnReceive(string[] msg)
        {
            if (msg[0] == "000" && msg[1] == "0")
                Console.Title = msg[2];
            if (msg[0] == "000" && msg[1] == "1")
            {
                Console.WriteLine("Auth failed");
                Environment.Exit(-1);
            }
            if (msg[0] == "006")
            {
                MyChar = new Character(Convert.ToInt32(msg[1]), Convert.ToInt32(msg[2]), Convert.ToInt32(msg[3]), Convert.ToInt32(msg[4]), new List<int>());
            }
            if (msg[0] == "007")
            {
                List<int> ships = new List<int>();
                for (int i = 1; i <= msg.Length-1; i++)
                {
                    ships.Add(Convert.ToInt32(msg[i]));
                }
                MyChar.Ships = new List<int>(ships);
            }
            string build = "";
            foreach (var s in msg)
            {
                build += s + "_";
            }
            build = build.Substring(0, build.Length - 1);
            Console.WriteLine("\nОтвет сервера: " + build);
        }
        public void OnConnect(string s)
        { }
        public void OnConnectError(string s)
        {
            OnError();
        }
        public void OnSendError(string s)
        {
            OnError();
        }
        public void OnReceiveError(string s)
        {
            OnError();
        }
        public void OnError()
        {
            Console.WriteLine("Server error");
            Environment.Exit(0);
        }
    }
    public class Character
    {
        public int Commander, Pilot, Gunner, EW;
        public List<int> Ships;
        public Character(int commander, int pilot, int gunner, int ew, List<int> ships)
        {
            Commander = commander;
            Pilot = pilot;
            Gunner = gunner;
            EW = ew;
            Ships = new List<int>(ships);
        }
    }

}
