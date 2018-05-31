using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIT_Server
{
    public static class CommandBuilder
    {
        public static string Build(string[] strarr)
        {
            return string.Join("_", strarr);
        }
    }
}
