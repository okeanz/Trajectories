using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIT_Server
{
    public static class Communicator
    {
        public static void NewMessage(Agent agent, string[] info)
        {
            int comm = Convert.ToInt32(info[0]);
            if (comm < 15 && comm >= 0)
                Lobby.Comm(agent, info);
            if (comm < 60 && comm >= 30)
                Lobby.scene.Switcher(agent, info);

        }
        public static void OnError(Agent agent)
        {
            Lobby.scene.DisconnectPlayer(agent, true);
            agent.RSite?.Disconnect(agent);
            VisitorBase.DeleteVisitor(agent);
        }
    }
}
