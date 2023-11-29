using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics
{
    public class PKAABB
    {
        public PKVector Min;
        public PKVector Max;

        public PKAABB(PKVector min, PKVector max)
        {
            this.Min = min;
            this.Max = max;
        }

        public PKAABB(float minx, float miny, float maxx, float maxy)
        {
            this.Min = new PKVector(minx, miny);
            this.Max = new PKVector(maxx, maxy);
        }
    }
}
