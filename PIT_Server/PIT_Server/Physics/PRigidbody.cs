
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PITPhysics
{

    public class PRigidbody
    {

        public PVector3 Position;
        public PQuaternion Rotation;
        public PVector3 LocalPosition => Position - GravityBody.Position;
        public PVector3 forward => Rotation*PVector3.forward;
        public PVector3 up => Rotation*PVector3.up;
        public PVector3 right => Rotation*PVector3.right; 
        public double Mass;
        public PGravityBody GravityBody;
        public PTrajectory Trajectory;
        public PVector3 newVelocity = PVector3.zero;
        public List<PVector3> forces = new List<PVector3>();
        public bool TUpdated = true;

        PWorld world;

        public PVector3 Velocity => Trajectory.GetVelocityMagnitude();


        public PRigidbody(PVector3 position, PWorld world)
        {
            Position = position;
            Rotation = PQuaternion.identity;
            this.world = world;
            Trajectory = new PTrajectory(this, world);
        }

        public virtual void Update(double step)
        {
            if (Trajectory?.Curve == null) return;
            if (forces.Count > 0) UpdateForces(step);
            if (!(this is PGravityBody))
                CheckGRBD();
            if (newVelocity != PVector3.zero)
            {
                Trajectory.UpdateCurve(newVelocity);
                newVelocity = PVector3.zero;
                TUpdated = true;
            }

            Random r = new Random();
            double alpha = GetAlphaDt(Trajectory.Curve.PositionTime + step);
            while (double.IsNaN(alpha))
                alpha = GetAlphaDt(Trajectory.Curve.PositionTime + step + r.NextDouble()/100);
            var pos = Position;
            Position = Trajectory.Curve.RFuncAlphaV3(alpha) + GravityBody.Position;
            var dpos = Position - pos;
            if (this is PGravityBody)
            {
                var rbs = world.RigidBodies.FindAll(x => x.GravityBody == (PGravityBody)this);
                foreach (var rb in rbs)
                    rb.Position += dpos;
            }
            Trajectory.Curve.PositionAngle = alpha;
            Trajectory.Curve.PositionTime += step;
        }

        void UpdateForces(double step)
        {
            PVector3 force = PVector3.zero;
            foreach (var f in forces)
                force += f;
            if (force != PVector3.zero) newVelocity += force * step + Velocity;
        }

        void CheckGRBD()
        {
            Pair<double, PGravityBody> force = new Pair<double, PGravityBody>(0,null);
            foreach (var gb in world.Gravitybodies)
            {
                double f = PWorld.G*gb.Mass*Mass/(Position - gb.Position).sqrMagnitude;
                force = f > force.a ? new Pair<double, PGravityBody>(f,gb) : force;
            }
            if (GravityBody == force.b)
                return;
            var vel = Velocity;
            GravityBody = force.b;
            newVelocity = vel;
        }

        double GetAlphaDt(double dt)
        {
            PCurve curve = Trajectory.Curve;
            double a = curve.a; // большая полуось
            double n = Math.Sqrt(PWorld.G * GravityBody.Mass / (a * a * a)); //среднее движение
            double M = n * dt; // средняя аномалия

            var func = curve.KeplerFunc;
            var Dfunc = curve.KeplerDFunc;
            

            double M0 = curve.e < 1 ?  M : -M;

            int t;
            //double tEx = NumericalMethods.NewtonForKepler(curve.KeplerFunc, curve.KeplerDFunc, M0, M, out t);
            double tEx = NumericalMethods.StaticPointKepler(curve.e, M0,out t);
            
            double alpha = curve.AlphaFromEAnomaly(tEx);

            if(t>100)
                Console.Write("angle:" + alpha+" M:"+M + " E:"+tEx + " func:" + func(tEx, M) + " t:"+t);
            //UnityEngine.Debug.Log("e: " + curve.e+ " sqrt((e+1)/(e-1)):" + Math.Sqrt((curve.e + 1) / (curve.e - 1)) + " tanh(H/2)" + Math.Tanh(Ex/2));
            //UnityEngine.Debug.Log("dt " + dt + " Ex " + Ex + " alpha "+alpha + " M: " + M + " func "+ func(Ex,M));

            return alpha;
        }

    }

}


