using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics
{
    public static class PKCollisions
    {
        public static bool IntersectCricles(PKVector centerA, PKVector centerB, float radiusA, float radiusB, out PKVector nor, out float depth)
        {
            nor = PKVector.Zero;
            depth = 0;

            float curDis = PKMath.Distance(centerA, centerB);
            float totalRadius = radiusA + radiusB;
            if (curDis >= totalRadius) { return false; }
            else
            {
                nor = (centerB - centerA).Normalized();
                depth = totalRadius - curDis;
                return true;
            }
        }
    }
}
