using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PITPhysics;
namespace PIT_Server
{
    public class Scene
    {
        public static volatile int FPS = 30;
        public static double SPEED = 1;
        public static volatile bool PAUSE = false;
        public bool Warning = false;
        public PWorld world;
        public ConcurrentDictionary<Agent, object> agents = new ConcurrentDictionary<Agent, object>();
        public ConcurrentDictionary<GameObject, object> GameObjects = new ConcurrentDictionary<GameObject, object>();
        ManualResetEvent _REvent = new ManualResetEvent(true);


        public Scene()
        {
            Initialize();
            Task.Factory.StartNew(Cycle, TaskCreationOptions.LongRunning);
            _REvent.Reset();
        }

        void Initialize()
        {
            InitializePhysics();
            var obj = new GameObject()
            {
                ID = Namer.GetRandomID(),
                Name = "earth",
                Owner = null,
                RBody = world.AddGravityBody(PVector3.zero, PVector3.zero, "earth", 5.97219e+24)
            };
            var mun = new GameObject()
            {
                ID = Namer.GetRandomID(),
                Name = "mun",
                Owner = null,
                RBody = world.AddGravityBody(new PVector3(38.44, 0, 0), new PVector3(0, 0, 0.002), "mun", 7.3477E+22)
            };
            GameObjects.TryAdd(obj, null);
            GameObjects.TryAdd(mun, null);

        }

        void InitializePhysics()
        {
            world = new PWorld();
            PWorld.GMultiplier = 1e-18;

        }
        void Cycle()
        {
            long nexttick = DateTime.Now.Ticks / 10000;
            long sleeptime = 0;
            GameState state = GameState.Running;
            while (state != GameState.Stop)
            {
                if (PAUSE) continue;
                Update();
                nexttick += 1000 / FPS;
                sleeptime = nexttick - DateTime.Now.Ticks / 10000;
                if (sleeptime >= 0)
                {
                    Warning = false;
                    Thread.Sleep((int)sleeptime);
                }
                else
                    Warning = true;
            }
        }

        void Update()
        {
            _REvent.Reset();
            world.Step(SPEED * 0.02);
            SendUpdates();
            CheckScene();
            _REvent.Set();
        }

        //Пустая ли сцена
        void CheckScene()
        {

        }

        //Рассылка координат
        void SendUpdates()
        {
            foreach (var ag in agents)
            {
                if (!ag.Key.SceneReady)
                    continue;
                foreach (var rb in GameObjects.Keys)
                {
                    if (rb.RBody.Trajectory.Curve == null)
                    {
                        continue;
                    }
                    ag.Key.SendClear(
                        CommandBuilder.Build(new string[]
                        {
                            "042", rb.ID.ToString() , rb.RBody.Position.X.ToString(), rb.RBody.Position.Y.ToString(), rb.RBody.Position.Z.ToString(), rb.RBody.Rotation.X.ToString(), rb.RBody.Rotation.Y.ToString(), rb.RBody.Rotation.Z.ToString(), rb.RBody.Rotation.W.ToString()
                        }));
                    
                        ag.Key.SendClear(
                            CommandBuilder.Build(new string[]
                            {
                                "050", rb.ID.ToString(), rb.RBody.Trajectory.Curve.e.ToString(),
                                rb.RBody.Trajectory.Curve.p.ToString(), rb.RBody.Velocity.X.ToString(),
                                rb.RBody.Velocity.Y.ToString(), rb.RBody.Velocity.Z.ToString(),
                                GameObjects.Keys.ToList().Find(x => x.RBody == rb.RBody.GravityBody).ID.ToString()
                            }));
                        rb.RBody.TUpdated = false;
                    

                    PVector3 up = rb.RBody.Rotation * PVector3.up;

                    ag.Key.SendClear(
                        CommandBuilder.Build(new string[]
                        {
                            "043",rb.ID.ToString(), up.X.ToString(), up.Y.ToString(), up.Z.ToString()
                        }));
                    if (rb.Owner == ag.Key && (rb.RBody.GravityBody.Position - rb.RBody.Position).magnitude < 0.010)
                        ag.Key.Send("051");
                }
            }
        }


        public void AddPlayer(Agent agent)
        {
            _REvent.WaitOne();
            Logger.LogIt("Adding agent to Scene");

            agents.TryAdd(agent, null);

        }

        //Ретрансляция
        public void Switcher(Agent agent, string[] info)
        {
            switch (info[0])
            {
                case "040":
                    PlayerReady(agent, info);
                    break;
                case "041":
                    PlayerSpace(agent, info);
                    break;
                case "044":
                    PlayerMove(agent, info);
                    break;
                case "045":
                    PlayerRotate(agent, info);
                    break;
                case "046":
                    var newspd = double.Parse(info[1]);
                    SPEED = newspd > 0 ? newspd : 1;
                    break;
                default:
                    break;
            }
        }

        //040 - игрок готов
        public void PlayerReady(Agent agent, string[] info)
        {
            var rb = new GameObject()
            {
                ID = Namer.GetRandomID(),
                Name = "angara",
                Owner = agent,
                RBody = world.AddRigidBody(new PVector3(2.61, 7.89, 4.29), new PVector3(0, 0.001, 0.007), 100),
            };

            agent.gameobject = rb;
            agent.Send(CommandBuilder.Build(new string[]
                       {
                            "040", rb.ID.ToString(), rb.Name , rb.RBody.Position.X.ToString(), rb.RBody.Position.Y.ToString(), rb.RBody.Position.Z.ToString(), rb.RBody.Rotation.X.ToString(), rb.RBody.Rotation.Y.ToString(), rb.RBody.Rotation.Z.ToString(), rb.RBody.Rotation.W.ToString()
                       }));
            GameObjects.TryAdd(rb, null);

        }

        //041 - игрок просто космос
        public void PlayerSpace(Agent agent, string[] info)
        {
            var rb = agent.gameobject;
            PVector3 pos = new PVector3(double.Parse(info[1]), double.Parse(info[2]), double.Parse(info[3]));
            PQuaternion rot = new PQuaternion(double.Parse(info[4]), double.Parse(info[5]), double.Parse(info[6]),
                double.Parse(info[7]));
            PVector3 speed = new PVector3(double.Parse(info[8]), double.Parse(info[9]), double.Parse(info[10]));
            world.DeleteRigidBody(rb.RBody);
            rb.RBody = world.AddRigidBody(pos, speed, 100);
            rb.RBody.Rotation = rot;
            rb.RBody.forces.Add(new PVector3(0, 0, 0) { Tag = "March" });
            agent.SceneReady = true;
            foreach (var go in GameObjects.Keys)
            {
                if (go == rb) continue;
                agent.Send(CommandBuilder.Build(new string[]
                       {
                            "040", go.ID.ToString(), go.Name , go.RBody.Position.X.ToString(), go.RBody.Position.Y.ToString(), go.RBody.Position.Z.ToString(), go.RBody.Rotation.X.ToString(), go.RBody.Rotation.Y.ToString(), go.RBody.Rotation.Z.ToString(), go.RBody.Rotation.W.ToString()
                       }));
            }
            Thread.Sleep(500);
            agent.SendClear(
                CommandBuilder.Build(new string[]
                {
                            "050", rb.ID.ToString(), rb.RBody.Trajectory.Curve.e.ToString(),
                            rb.RBody.Trajectory.Curve.p.ToString(), rb.RBody.Velocity.X.ToString(),
                            rb.RBody.Velocity.Y.ToString(), rb.RBody.Velocity.Z.ToString(),
                            GameObjects.Keys.ToList().Find(x => x.RBody == rb.RBody.GravityBody).ID.ToString()
                }));
            
        }

        //044 - тяга маршевых
        public void PlayerMove(Agent agent, string[] info)
        {
            Console.WriteLine("044 run");
            var power = double.Parse(info[1]);
            var go = GameObjects.Keys.ToList().Find(x => x.Owner == agent);

            var force = (go.RBody.Rotation * PVector3.up).normalized * power / 100000;
            force.Tag = "March";
            var i = go.RBody.forces.FindIndex(x => x.Tag == "March");
            go.RBody.forces[i] = force;
            Console.WriteLine("044 end");
        }


        public void PlayerRotate(Agent agent, string[] info)
        {
            Console.WriteLine("045 run");
            PQuaternion rot = new PQuaternion(double.Parse(info[1]), double.Parse(info[2]), double.Parse(info[3]),
                double.Parse(info[4]));

            var go = GameObjects.Keys.ToList().Find(x => x.Owner == agent);
            go.RBody.Rotation = rot;
            Console.WriteLine("045 end");
        }

        public void DisconnectPlayer(Agent agent, bool Error)
        {
            if (Error)
            {
                PAUSE = true;
                var go = GameObjects.Keys.ToList().Find(x => x.Owner == agent);
                object obj;
                if (go != null)
                    GameObjects.TryRemove(go, out obj);
                agents.TryRemove(agent, out obj);
                PAUSE = false;
            }
            Logger.LogIt("Agent Disconnected from Scene");
        }




        private enum GameState { Paused, Running, Stop };
        private enum ObjectType { Ship0, Ship1, Ship2, Asteroid, Planet };
    }
}
