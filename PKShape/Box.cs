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

        public ShapeType ShapeType = ShapeType.Box;

        public Box(Box other)
        {
            this.Width = other.Width;
            this.Height = other.Height;
        }

        public Box(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        public override float GetArea()
        {
            return Width * Height;
        }
    }
}
