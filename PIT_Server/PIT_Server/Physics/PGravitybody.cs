
namespace PITPhysics
{
    public class PGravityBody:PRigidbody
    {
        public string Name;
        public double CatchDistance;

        public PGravityBody(string name, PVector3 position, PWorld world):base(position,world)
        {
            Name = name;
            Position = position;
        }
        
        public override void Update(double step)
        {
            if (GravityBody != null) base.Update(step);
        }


    }
}
