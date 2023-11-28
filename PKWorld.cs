using PKPhysics.PKShape;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace PKPhysics
{
    public sealed class PKWorld
    {
        public static readonly float MinBodySize = 0.01f * 0.01f;
        public static readonly float MaxBodySize = 64f * 64f;

        public static readonly float MinDensity = 0.5f;     // g/cm^3
        public static readonly float MaxDensity = 21.4f;

        private PKVector gravity;
        private List<PKBody> bodyList;

        public PKWorld()
        {
            gravity = new PKVector(0, -9.81f);
            bodyList = new List<PKBody>();
        }

        public void AddBody(PKBody body) { if (body == null) { return; } bodyList.Add(body); }
        public void RemoveBody(PKBody body) { if (body == null) { return; } bodyList.Remove(body); }
        public bool GetBody(int index, out PKBody body)
        {
            body = null;
            if (index < 0 || index >= bodyList.Count) return false;
            body = bodyList[index];
            return true;
        }
        public int BodyCount() { return bodyList.Count; }

        public void Update(float time)
        {
            for (int i = 0; i < bodyList.Count; i++)
            {
                bodyList[i].Update(time);
            }

            for (int i = 0; i < bodyList.Count - 1; i++)
            {
                var bodyA = bodyList[i];
                for (int j = i + 1; j < bodyList.Count; j++)
                {
                    var bodyB = bodyList[j];
                    if (Collide(bodyA, bodyB, out PKVector nor, out float depth))
                    {
                        ProcessCollide(bodyA, bodyB, nor);
                    }
                }
            }
        }

        public void ProcessCollide(PKBody bodyA, PKBody bodyB, PKVector nor)
        {
            PKVector relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;
            if (PKMath.Dot(relativeVelocity, nor) > 0) return;
            float e = Math.Min(bodyA.Restitution, bodyB.Restitution);

            float j = -(1f + e) * PKMath.Dot(relativeVelocity, nor) / (1 / bodyA.Mass + 1 / bodyB.Mass);
            bodyA.LinearVelocity -= (j / bodyA.Mass) * nor;
            bodyB.LinearVelocity += (j / bodyB.Mass) * nor;
        }
        public bool Collide(PKBody bodyA, PKBody bodyB, out PKVector nor, out float depth)
        {
            nor = PKVector.Zero;
            depth = 0f;

            ShapeType shapeTypeA = bodyA.shape.ShapeType;
            ShapeType shapeTypeB = bodyB.shape.ShapeType;

            if (shapeTypeA == ShapeType.Box)
            {
                if (shapeTypeB == ShapeType.Box)
                {
                    return PKCollisions.IntersectPolygons(bodyA.GetTransformedVertics(), bodyB.GetTransformedVertics(), out nor, out depth);
                }
                else if (shapeTypeB == ShapeType.Circle)
                {
                    bool res = PKCollisions.IntersectPolygonAndCricle(bodyA.GetTransformedVertics(), bodyB.Position, (bodyB.shape as Cricle).Radius, out nor, out depth);

                    nor = -nor;
                    return res;
                }
            }
            else if (shapeTypeA == ShapeType.Circle)
            {
                if (shapeTypeB == ShapeType.Box)
                {
                    return PKCollisions.IntersectPolygonAndCricle(bodyB.GetTransformedVertics(), bodyA.Position, (bodyA.shape as Cricle).Radius, out nor, out depth);
                }
                else if (shapeTypeB == ShapeType.Circle)
                {
                    return PKCollisions.IntersectCricles(bodyA.Position, bodyB.Position, (bodyA.shape as Cricle).Radius, (bodyA.shape as Cricle).Radius, out nor, out depth);
                }
            }
            return false;
        }
    }
}
