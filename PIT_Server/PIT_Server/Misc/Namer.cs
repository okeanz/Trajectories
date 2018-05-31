using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIT_Server
{
    public static class Namer
    {
        static Random r = new Random();
        public static string GetRandomName()
        {
            string name = System.IO.Path.GetRandomFileName();
            name = name.Replace('_', '!');
            return name;
        }
        public static int GetRandomID()
        {
            return r.Next(int.MinValue, int.MaxValue);
        }
    }
}
