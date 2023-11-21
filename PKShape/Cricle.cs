using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics.PKShape
{
    public class Cricle : ShapeBase
    {
        public float Radius;
        public ShapeType ShapeType = ShapeType.Circle;

        public Cricle(float r)
        {
            this.Radius = r;
        }

        public Cricle(Cricle other)
        {
            this.Radius = other.Radius;
        }

        public override float GetArea()
        {
            return Radius * Radius * (float)Math.PI;
        }
    }
}
