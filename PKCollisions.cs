﻿using PKPhysics.PKShape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace PKPhysics
{
    public static class PKCollisions
    {
        public static void FindContactPoints(PKBody bodyA, PKBody bodyB, out PKVector contact1, out PKVector contact2, out int contactCount)
        {
            contact1 = PKVector.Zero;
            contact2 = PKVector.Zero;
            contactCount = 0;

            ShapeType shapeTypeA = bodyA.shape.ShapeType;
            ShapeType shapeTypeB = bodyB.shape.ShapeType;

            if (shapeTypeA == ShapeType.Polygon)
            {
                if (shapeTypeB == ShapeType.Polygon)
                {

                }
                else if (shapeTypeB == ShapeType.Circle)
                {

                }
            }
            else if (shapeTypeA == ShapeType.Circle)
            {
                if (shapeTypeB == ShapeType.Polygon)
                {

                }
                else if (shapeTypeB == ShapeType.Circle)
                {
                    FindContactPoint(bodyA.Position, (bodyA.shape as Cricle).Radius, bodyB.Position, (bodyB.shape as Cricle).Radius, out contact1);
                    contactCount = 1;
                }
            }
        }

        public static bool IntersectPolygonAndCricle(PKVector[] vertics, PKVector polygonCenter, PKVector center, float radius, out PKVector nor, out float depth)
        {
            nor = PKVector.Zero;
            depth = float.MaxValue;

            float maxA, minB, maxB, minA;
            float tempDepth = 0;
            PKVector axis = PKVector.Zero;

            for (int i = 0; i < vertics.Length; i++)
            {
                PKVector edge = vertics[(i + 1) % vertics.Length] - vertics[i];
                axis = new PKVector(-edge.Y, edge.X);
                axis = PKMath.Normalize(axis);
                PKCollisions.ProjectVertics(vertics, axis, out minA, out maxA);
                PKCollisions.ProjectCricle(center, radius, axis, out minB, out maxB);

                if (maxA <= minB || maxB <= minA) { return false; }

                tempDepth = (float)Math.Min(maxA - minB, maxB - minA);
                if (depth > tempDepth)
                {
                    depth = tempDepth;
                    nor = axis;
                }
            }

            int index = FindClosestVerticsIndex(center, vertics);
            axis = vertics[index] - center;
            axis = axis.Normalized();
            PKCollisions.ProjectVertics(vertics, axis, out minA, out maxA);
            PKCollisions.ProjectCricle(center, radius, axis, out minB, out maxB);

            if (maxA <= minB || maxB <= minA) { return false; }

            tempDepth = (float)Math.Min(maxA - minB, maxB - minA);
            if (depth > tempDepth)
            {
                depth = tempDepth;
                nor = axis;
            }

            PKVector dir = polygonCenter - center;
            if (PKMath.Dot(nor, dir) < 0) { nor = -nor; }
            return true;
        }

        public static void FindContactPoint(PKVector centerA, float radiusA, PKVector centerB, float radiusB, out PKVector cp)
        {
            PKVector dir = PKMath.Normalize(centerB - centerA);
            cp = centerA + dir * radiusA;
        }

        public static bool IntersectPolygons(PKVector centerA, PKVector[] verticsA, PKVector centerB, PKVector[] verticsB, out PKVector nor, out float depth)
        {
            nor = PKVector.Zero;
            depth = float.MaxValue;
            for (int i = 0; i < verticsA.Length; i++)
            {
                PKVector edge = verticsA[(i + 1) % verticsA.Length] - verticsA[i];
                PKVector axis = new PKVector(-edge.Y, edge.X);
                axis = axis.Normalized();
                PKCollisions.ProjectVertics(verticsA, axis, out float minA, out float maxA);
                PKCollisions.ProjectVertics(verticsB, axis, out float minB, out float maxB);

                if (maxA <= minB || maxB <= minA) { return false; }

                float tempDepth = (float)Math.Min(maxA - minB, maxB - minA);
                if (depth > tempDepth)
                {
                    depth = tempDepth;
                    nor = axis;
                }
            }

            for (int i = 0; i < verticsB.Length; i++)
            {
                PKVector edge = verticsB[(i + 1) % verticsB.Length] - verticsB[i];
                PKVector axis = new PKVector(-edge.Y, edge.X);

                PKCollisions.ProjectVertics(verticsA, axis, out float minA, out float maxA);
                PKCollisions.ProjectVertics(verticsB, axis, out float minB, out float maxB);
                axis = axis.Normalized();
                if (maxA <= minB || maxB <= minA) { return false; }

                float tempDepth = (float)Math.Min(maxA - minB, maxB - minA);
                if (depth > tempDepth)
                {
                    depth = tempDepth;
                    nor = axis;
                }
            }

            PKVector dir = centerB - centerA;
            if (PKMath.Dot(nor, dir) < 0) { nor = -nor; }
            return true;
        }

        public static bool Collide(PKBody bodyA, PKBody bodyB, out PKVector nor, out float depth)
        {
            nor = PKVector.Zero;
            depth = 0f;

            ShapeType shapeTypeA = bodyA.shape.ShapeType;
            ShapeType shapeTypeB = bodyB.shape.ShapeType;

            if (shapeTypeA == ShapeType.Polygon)
            {
                if (shapeTypeB == ShapeType.Polygon)
                {
                    return PKCollisions.IntersectPolygons(bodyA.Position, bodyA.GetTransformedVertics(), bodyB.Position, bodyB.GetTransformedVertics(), out nor, out depth);
                }
                else if (shapeTypeB == ShapeType.Circle)
                {
                    bool res = PKCollisions.IntersectPolygonAndCricle(bodyA.GetTransformedVertics(), bodyA.Position, bodyB.Position, (bodyB.shape as Cricle).Radius, out nor, out depth);

                    nor = -nor;
                    return res;
                }
            }
            else if (shapeTypeA == ShapeType.Circle)
            {
                if (shapeTypeB == ShapeType.Polygon)
                {
                    return PKCollisions.IntersectPolygonAndCricle(bodyB.GetTransformedVertics(), bodyB.Position, bodyA.Position, (bodyA.shape as Cricle).Radius, out nor, out depth);
                }
                else if (shapeTypeB == ShapeType.Circle)
                {
                    return PKCollisions.IntersectCricles(bodyA.Position, bodyB.Position, (bodyA.shape as Cricle).Radius, (bodyB.shape as Cricle).Radius, out nor, out depth);
                }
            }
            return false;
        }

        private static PKVector GetArithmeticMean(PKVector[] vertics)
        {
            PKVector center = PKVector.Zero;
            for (int i = 0; i < vertics.Length; ++i)
            {
                center.X += vertics[i].X;
                center.Y += vertics[i].Y;
            }
            return center / (float)vertics.Length;
        }

        private static void ProjectVertics(PKVector[] vertics, PKVector axis, out float min, out float max)
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

        private static void ProjectCricle(PKVector center, float radius, PKVector axis, out float min, out float max)
        {
            PKVector dir = PKMath.Normalize(axis);
            PKVector tempVec = dir * radius;

            PKVector p1 = center + tempVec;
            PKVector p2 = center - tempVec;

            min = PKMath.Dot(axis, p2);
            max = PKMath.Dot(axis, p1);

            if (min > max)
            {
                var t = min;
                min = max;
                max = t;
            }
        }

        private static int FindClosestVerticsIndex(PKVector PO, PKVector[] vertics)
        {
            int index = -1;
            float minDis = float.MaxValue;
            for (int i = 0; i < vertics.Length; ++i)
            {
                float tempDis = PKMath.Distance(vertics[i], PO);
                if (tempDis < minDis)
                {
                    index = i;
                    minDis = tempDis;
                }

            }
            return index;
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
