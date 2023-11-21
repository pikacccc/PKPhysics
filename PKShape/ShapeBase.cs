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
        Box,
        Circle
    }

    public abstract class ShapeBase
    {
        public ShapeType ShapeType = ShapeType.None;

        public abstract float GetArea();
    }
}
 