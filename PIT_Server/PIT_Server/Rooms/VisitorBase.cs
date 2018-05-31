using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
namespace PIT_Server
{
    public static class VisitorBase
    {
        static ConcurrentDictionary<Agent, object> Visitors = new ConcurrentDictionary<Agent, object>(50,10000);

        public static void NewVisitor(Socket visitor)
        {
            Agent c = new Agent(visitor);
            Visitors.TryAdd(c, null);
            Logger.LogIt(typeof(VisitorBase), "New Visitor " + c.Name + ":" + visitor.LocalEndPoint.ToString());
            c.Start();
        }

        public static void DeleteVisitor(Agent agent)
        {
            object o;
            Visitors.TryRemove(agent, out o);
            agent.Dispose();
            Logger.LogIt(typeof(VisitorBase), "Visitor disconnected "+ agent.Name);
        }
        public static List<Agent> GetVisitors()
        {
            return new List<Agent>(Visitors.Keys);
        }

    }
}
