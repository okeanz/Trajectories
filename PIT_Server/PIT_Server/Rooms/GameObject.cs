using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PITPhysics;

namespace PIT_Server
{
    public class GameObject
    {
        public int ID;
        public string Name;
        public Agent Owner;
        public PRigidbody RBody;
        public override string ToString()
        {
            return ID.ToString()+":"+Name;
        }
    }
}