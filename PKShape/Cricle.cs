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

        public Cricle(float r)
        {
            this.Radius = r;
        }

        public Cricle(Cricle other)
        {
            this.Radius = other.Radius;
            this.ShapeType = ShapeType.Circle;
            this.CreateVertics();
            this.CreateTriangles();
        }

        public override float GetArea()
        {
            return Radius * Radius * (float)Math.PI;
        }

        protected override void CreateTriangles()
        {
            this.triangles = null;
        }

        protected override void CreateVertics()
        {
            this.vertics = null;
        }
    }
}
