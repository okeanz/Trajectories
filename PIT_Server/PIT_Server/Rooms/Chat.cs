using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIT_Server
{
    public static class Chat
    {
        public static void Comm(Agent sender, string[] msg)
        {
            foreach (var a in VisitorBase.GetVisitors())
                a.Send("030_"+sender.Name + ": " + msg[1]);
        }
    }
}
