using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIT_Server;
using System.Threading;
using System.Threading.Tasks;
namespace smallclient
{
    public partial class GUi : Form
    {
        int Count;
        public List<GuiHandler> handlers = new List<GuiHandler>();
        
        public GUi(int count)
        {
            InitializeComponent();
            Count = count;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
            KeyDown += GUi_KeyDown;

        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                ConsoleUpdate();
            }
            else
            {
                listBoxConsole.Items.Clear();
            }
        }

        void GUi_KeyDown(object sender, KeyEventArgs e)
        {
            if (ConsoleText.Focused && e.KeyCode == Keys.Enter)
                ConsoleSend_Click(sender, e);
        }
        private void GUi_Load(object sender, EventArgs e)
        {
            for (int i = 1; i < Count+1; i++)
                handlers.Add(new GuiHandler(i,this));
            string[] arr = handlers.Select<GuiHandler, string>(x => x.ID.ToString()).ToArray();
            listBox1.Items.AddRange(arr);
            ConsoleUpdate();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0);
        }

        

        private void ConsoleSend_Click(object sender, EventArgs e)
        {
            if(ConsoleText.Text !="")
                foreach (var s in listBox1.SelectedItems)
                {
                    GuiHandler gh = handlers.Find(new Predicate<GuiHandler>(x => x.ID == Convert.ToInt32(s.ToString())));
                    gh.Send(ConsoleText.Text);
                }
            ConsoleText.Text = "";
        }

        public void ConsoleUpdate()
        {
            GuiHandler gh = null;
            if(listBox1.SelectedItem != null)
            listBox1.Invoke(new Action(() => gh = handlers.Find(new Predicate<GuiHandler>(x => x.ID == Int32.Parse(listBox1.SelectedItem.ToString())))));
            
            if (gh != null)
            {
                listBoxConsole.Invoke(new Action(() =>
                {
                    listBoxConsole.Items.Clear();
                    listBoxConsole.Items.AddRange(gh.Answers.ToArray());
                    listBoxConsole.SelectedIndex = listBoxConsole.Items.Count - 1;
                }));
                
            }
        }

        public void DeleteThis(GuiHandler handler)
        {
            listBox1.Invoke(new Action(() => listBox1.Items.Remove(handler.ID.ToString())));
            handlers.Remove(handler);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Count++;
            GuiHandler gh = new GuiHandler(Count, this);
            handlers.Add(gh);
            listBox1.Items.Add(gh.ID.ToString());
        }

    }
    public class GuiHandler : IMaster
    {
        public ClientTCP client;
        public int ID;
        public string Name;
        public List<string> Answers = new List<string>();
        public GUi callback;
        public bool inHall = false;
        public GuiHandler(int id, GUi cb)
        {
            callback = cb;
            client = new ClientTCP("127.0.0.1", 2502, this);
            ID = id;
            client.Connect();
            Send("000_" + id + "_" + id);
            Send("001_1");
            Send("008_0_0");
            Thread.Sleep(200);
            Send("014_1");
            Thread.Sleep(200);
            Send("040");
            Thread.Sleep(200);
            Send("041");
        }
        //void input()
        //{
        //    if (true)
        //    {
        //        Console.Write("\nПредпочитаемая профессия [1-5]:");

        //        int Choise = Convert.ToInt32(Console.ReadLine());
        //        if (Choise == 1)
        //        {
        //            Console.Write("\nКакой корабль, кэп ? [");
        //            foreach (var s in MyChar.Ships)
        //                Console.Write(s + " ");
        //            Console.Write("]:");
        //            Send("008_1_" + Console.ReadLine());

        //        }
        //        else
        //        {
        //            string tosend = "008";
        //            string[] tt = new string[5];
        //            for (int i = 0; i < 5; i++)
        //            {
        //                tt[i] = "_0";
        //            }
        //            tt[Choise - 1] = "_1";
        //            tosend += string.Concat(tt);
        //            Send(tosend);
        //        }
        //        Send("009");

        //    }
        //    while (true)
        //    {
        //        ConsoleKey key = Console.ReadKey(true).Key;
        //        switch (key)
        //        {
        //            case ConsoleKey.H:
        //                Console.WriteLine("Help:");
        //                Console.WriteLine("C: очистить экран");
        //                Console.WriteLine("S: ввести отправляемое сообщение вручную");
        //                break;
        //            case ConsoleKey.S:
        //                Console.Write("Отправить: ");
        //                Send(Console.ReadLine());
        //                break;
        //            case ConsoleKey.C:
        //                Console.Clear();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}
        public void OnReceive(string[] msg)
        {
            string build = "";
            foreach (var s in msg)
            {
                build += s + "_";
            }
            build = build.Substring(0, build.Length - 1);
            Answers.Add("Rcv: " + build);

            if (msg[0] == "000" && msg[1] == "0")
            {
                Name = msg[2];
            }
            if (msg[0] == "000" && msg[1] == "1")
            {
                Console.WriteLine("Auth failed");
                Environment.Exit(-1);
            }
            
            callback.ConsoleUpdate();
        }
        public void OnConnect(string s)
        { }
        public void OnConnectError(string s)
        {
            OnErrors();
        }
        public void OnSendError(string s)
        {
            OnErrors();
        }
        public void OnReceiveError(string s)
        {
            OnErrors();
        }
        public void OnErrors()
        {
            Console.WriteLine("Server error");
            callback.DeleteThis(this);
        }
        public void Send(string msg)
        {
            client.Send(msg);
            Answers.Add("Sent: " + msg);
            callback.ConsoleUpdate();
        }
    }
    
}
