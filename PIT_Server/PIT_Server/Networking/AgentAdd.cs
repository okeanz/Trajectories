using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace PIT_Server
{
    public partial class Agent
    {
        public bool Authenticated = false;

        public bool ReadyToFlight = false;
        public Rocket RSite = null;

        public GameObject gameobject;
        public bool SceneReady = false;
    }
}
