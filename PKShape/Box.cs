using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics.PKShape
{
    public class Box : ShapeBase
    {
        public float Width;
        public float Height;

        public Box(Box other)
        {
            this.Width = other.Width;
            this.Height = other.Height;
        }

        public Box(float width, float height)
        {
            this.Width = width;
            this.Height = height;
            this.ShapeType = ShapeType.Box;
            this.CreateTriangles();
            this.CreateVertics();
        }

        public override float GetArea()
        {
            return Width * Height;
        }

        protected override void CreateVertics()
        {
            float left = -this.Width / 2;
            float top = this.Height / 2;
            float right = this.Width / 2;
            float bottom = -this.Height / 2;

            this.vertics = new PKVector[4];
            this.vertics[0] = new PKVector(left, top);
            this.vertics[1] = new PKVector(right, top);
            this.vertics[2] = new PKVector(right, bottom);
            this.vertics[3] = new PKVector(left, bottom);
        }

        protected override void CreateTriangles()
        {
            this.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        }
    }
}
