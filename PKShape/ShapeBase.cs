using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics.PKShape
{
    public enum ShapeType
    {
        None,
        Polygon,
        Circle
    }

    public abstract class ShapeBase
    {
        public ShapeType ShapeType;

        protected PKVector[] vertics;
        public PKVector[] Vertics
        {
            get { return vertics; }
        }

        protected int[] triangles = { };
        public int[] Tiangles
        {
            get { return triangles; }
        }

        public abstract float GetArea();

        protected abstract void CreateVertics();
        protected abstract void CreateTriangles();
    }
}
