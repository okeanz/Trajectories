using System;
using System.Collections.Generic;
namespace PITPhysics
{
    public class PWorld
    {
        public List<PRigidbody> RigidBodies;
        public List<PGravityBody> Gravitybodies;

        public PWorld()
        {
            RigidBodies = new List<PRigidbody>();
            Gravitybodies = new List<PGravityBody>();
        }
        public static double GMultiplier = 1.0;//  1e-9 - km / 1e-18 - thousands km
        public static double G => 6.6742867e-11 * GMultiplier;

        public void Step(double timeStep)
        {
            //CheckGRBDistances();
            try
            {
                foreach (var rb in RigidBodies)
                    rb.Update(timeStep);

                foreach (PRigidbody gb in Gravitybodies)
                    if (gb.GravityBody != null)
                        gb.Update(timeStep);
            }
            catch (Exception e)
            {
                Console.WriteLine("Physics Exception: " + e.ToString());
            }
        }

        void CheckGRBDistances()
        {

            var force = new Pair<double, PGravityBody>(0, null);
            foreach (var rb in RigidBodies)
            {
                foreach (var gb in Gravitybodies)
                {
                    double f = PWorld.G * gb.Mass * rb.Mass / (rb.Position - gb.Position).sqrMagnitude;
                    force = f > force.a ? new Pair<double, PGravityBody>(f, gb) : force;
                }
                rb.GravityBody = force.b;
                force = new Pair<double, PGravityBody>(0, null);
            }
        }

        public PRigidbody AddRigidBody(PVector3 position, PVector3 velocity, double mass)
        {
            var body = new PRigidbody(position, this) { Mass = mass };
            RigidBodies.Add(body);
            CheckGRBDistances();
            body.Trajectory.UpdateCurve(velocity);
            return body;
        }

        public void DeleteRigidBody(PRigidbody rbody)
        {
            RigidBodies.Remove(rbody);
            CheckGRBDistances();
        }
        public PGravityBody AddGravityBody(PVector3 position, PVector3 speed, string name, double mass)
        {
            var body = new PGravityBody(name, position, this) { Mass = mass };
            if (Gravitybodies.Count > 0) body.GravityBody = Gravitybodies[0];
            if(speed != PVector3.zero) body.Trajectory.UpdateCurve(speed);
            Gravitybodies.Add(body);
            Gravitybodies.Sort((x, y) => (int)(x.CatchDistance - y.CatchDistance));
            return body;
        }
    }
}
