using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace PIT_Server
{
    public static class Lobby
    {
        public static ConcurrentDictionary<Spaceport, object> SpacePorts = new ConcurrentDictionary<Spaceport, object>();
        public static Scene scene;

        public static void Init()
        {
            scene = new Scene();
        }
        public static void Comm(Agent sender, string[] msg)
        {
            switch (msg[0])
            {
                case "000":
                    Authenticate(sender, msg);
                    break;
                case "001":
                    GoToGameServer(sender, msg);
                    break;
                case "002":
                    sender.Send("002_Полезная информация");
                    break;
                case "005":
                    sender.Send("005");
                    break;
                case "008":
                    ToSpacePort(sender, msg);
                    break;
                case "009":
                    PSGR_NotReady(sender, msg);
                    break;
                case "014":
                    PSGR_Ready(sender, msg);
                    break;
                default:
                    break;
            }
        }
        static void Authenticate(Agent sender, string[] msg)
        {
            if (sender.Authenticated == false)
                if (Database.Authenticate(msg[1], msg[2]))
                {
                    Character c = Database.GetChar(msg[1]);
                    Agent a = VisitorBase.GetVisitors().Find(new Predicate<Agent>(x => x.Name == c.Name));
                    if (a != null)
                    {
                        Logger.LogIt(typeof(Lobby), "User " + sender.Name + " already on server");
                        sender.Send("000_1");
                        return;
                    }
                    sender.Authenticated = true;
                    sender.Name = c.Name;
                    Logger.LogIt(typeof(Lobby), "User " + c.Name + " Authenticated");
                    sender.Send("000_0_" + c.Name);
                    return;
                }
                else
                {
                    Logger.LogIt(typeof(Lobby), "User " + sender.Name + " Auth failed");
                    sender.Send("000_1");
                }
        }
        static void GoToGameServer(Agent sender, string[] msg)
        {
            if (sender.Authenticated)
            {
                sender.Send("001");
                Logger.LogIt(typeof(Lobby), "User " + sender.Name + " go to Gs");
                SendInfo_SPs(sender);
            }
            else
                sender.Send("999_Unauthorized");

        }
        static void ToSpacePort(Agent sender, string[] msg)
        {
            int sp = Convert.ToInt32(msg[1]);

            var SPort = SpacePorts.ToList().Find(new Predicate<KeyValuePair<Spaceport, object>>(x => x.Key.ID == sp)).Key;

            SPort.Requests.Enqueue(new Spaceport.Request(sender));
        }
        static void PSGR_Ready(Agent sender, string[] msg)
        {
            sender.ReadyToFlight = msg[1] == "1" ? true : false;

            if (sender.RSite.PSGR == null) return;
            if (!sender.RSite.PSGR.ReadyToFlight) return;

            Logger.LogIt("Rocket " + sender.RSite.ID + " starting");
            sender.RSite.PSGR.Send("014");
            scene.AddPlayer(sender);
        }
        static void PSGR_NotReady(Agent sender, string[] msg)
        {
            sender.ReadyToFlight = false;
            var rocket = sender.RSite;
            if (rocket == null) return;

            rocket.PSGR = null;
            SendInfo_SPs(sender);
            Logger.LogIt("Agent " + sender.Name + " gone out from Rocket " + rocket.ID + " on Spaceport " + rocket.SPort.ID);
            sender.Send("009");

        }
        public static void SendInfo_SPs(Agent sender = null)
        {
            foreach (var sp in SpacePorts.Keys)
            {
                string tosend = CommandBuilder.Build(new string[] { "006", sp.ID.ToString(), sp.rocket.ID.ToString(), sp.rocket.PSGR == null ? "0" : "1" });
                if (sender != null)
                    sender.Send(tosend);
                else
                {
                    var authVisitors = VisitorBase.GetVisitors().FindAll(new Predicate<Agent>(x => x.Authenticated));
                    authVisitors.ForEach(x => x.Send(tosend));
                }
            }
        }
    }
    public class Spaceport
    {
        public int ID;
        public Rocket rocket;
        public Spaceport() : this(0, new Rocket()) { }
        public Spaceport(int id, Rocket Rocket)
        {
            ID = id;
            rocket = Rocket;
            Rocket.SPort = this;
            Checker.Elapsed += (x, y) => CheckRequests();
            Checker.Start();
        }

        public ConcurrentQueue<Request> Requests = new ConcurrentQueue<Request>();
        System.Timers.Timer Checker = new System.Timers.Timer(200);

        void CheckRequests()
        {
            while (Requests.Count != 0)
            {
                Request rq;
                if (Requests.TryDequeue(out rq))
                {
                    if (rocket.PSGR == null)
                    {
                        Logger.LogIt("Agent " + rq.Who.Name + " go to SpacePort " + ID);
                        rocket.PSGR = rq.Who;
                        rq.Who.RSite = rocket;
                        rq.Who.Send("008_1");
                        Lobby.SendInfo_SPs();
                    }
                    else
                        rq.Who.Send("008_0");

                }
                else
                    Logger.LogIt("Request dequeue failure");
            }
        }

        public class Request
        {
            public Agent Who;
            public Request() : this(null) { }
            public Request(Agent who)
            {
                Who = who;
            }
        }
    }
    public class Rocket
    {
        public int ID;
        public Spaceport SPort;
        public Agent PSGR;
        public Rocket() : this(0) { }
        public Rocket(int id, Agent psgr = null, Spaceport where = null)
        {
            ID = id;
            PSGR = psgr;
            SPort = where;
        }
        public void Disconnect(Agent agent)
        {
            if (PSGR == agent)
                PSGR = null;
            Logger.LogIt("Agent " + agent.Name + " disconnected from rocket " + ID);
        }
    }
}
