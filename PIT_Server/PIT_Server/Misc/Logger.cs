using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PIT_Server
{
    public static class Logger
    {
        static List<Msg> Log = new List<Msg>(100000);
        public static bool ShowOnAccept = false;
        public static void LogIt(Type sender, string message)
        {
            Msg m = new Msg(DateTime.Now, sender, message);
            if(ShowOnAccept)
                Console.WriteLine("[{0}.{3}](From: {1}): {2}", m.Time.ToLongTimeString(), m.Sender.ToString(), m.Message, m.Time.Millisecond);
            Log.Add(m);
        }
        public static void LogIt(string message)
        {
            Msg m = new Msg(DateTime.Now, null, message);
            if (ShowOnAccept)
                Console.WriteLine("[{0}.{2}]: {1}", m.Time.ToLongTimeString(), m.Message, m.Time.Millisecond);
            Log.Add(m);
        }

        public static void SaveLogs()
        {
            string str = DateTime.Now.ToLongTimeString();
            str = str.Replace(':', ' ');
            var f = File.CreateText(str+".txt");
            foreach (var msg in Log)
            {
                string s = string.Format("[{0}.{3}](From: {1}): {2}", msg.Time.ToLongTimeString(), msg.Sender.ToString(), msg.Message, msg.Time.Millisecond);
                f.WriteLine(s);
            }
            f.Close();
        }
        public static List<Msg> GetLog()
        {
            return Log;
        }
        public static void ClearLog()
        {
            Log.Clear();
        }
        public static void Write()
        {
            List<Msg> save = new List<Msg>(Log);
            if (save.Count != 0)
                foreach (var m in save)
                    Console.WriteLine("[{0}.{3}](From: {1}): {2}", m.Time.ToLongTimeString(), m.Sender, m.Message, m.Time.Millisecond);
            else
                Console.WriteLine("Log is empty");
        }
    }
    public class Msg
    {
        public DateTime Time;
        public Type Sender;
        public string Message;
        public Msg(DateTime time, Type sender, string message)
        {
            Time = time;
            Sender = sender;
            Message = message;
        }
    }
}
