using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics
{
    public static class PKCollisions
    {
        public static bool IntersectPolygons(PKVector[] verticsA, PKVector[] verticsB)
        {
            for (int i = 0; i < verticsA.Length; i++)
            {
                PKVector edge = verticsA[(i + 1) % verticsA.Length] - verticsA[i];
                PKVector axis = new PKVector(-edge.Y, edge.X);

                PKCollisions.ProjectVertics(verticsA, axis, out float minA, out float maxA);
                PKCollisions.ProjectVertics(verticsB, axis, out float minB, out float maxB);

                if (maxA <= minB || maxB <= minA) { return false; }
            }

            for (int i = 0; i < verticsB.Length; i++)
            {
                PKVector edge = verticsB[(i + 1) % verticsB.Length] - verticsB[i];
                PKVector axis = new PKVector(-edge.Y, edge.X);

                PKCollisions.ProjectVertics(verticsA, axis, out float minA, out float maxA);
                PKCollisions.ProjectVertics(verticsB, axis, out float minB, out float maxB);

                if (maxA <= minB || maxB <= minA) { return false; }
            }

            return true;
        }

        public static void ProjectVertics(PKVector[] vertics, PKVector axis, out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;
            for (int i = 0; i < vertics.Length; i++)
            {
                float proj = PKVector.Dot(vertics[i], axis);
                if (proj < min) min = proj;
                if (proj > max) max = proj;
            }
        }

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
