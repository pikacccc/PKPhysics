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
                bodyList[i].Update(time, this.gravity);
            }

            for (int i = 0; i < bodyList.Count - 1; i++)
            {
                var bodyA = bodyList[i];
                for (int j = i + 1; j < bodyList.Count; j++)
                {
                    var bodyB = bodyList[j];
                    if (Collide(bodyA, bodyB, out PKVector nor, out float depth))
                    {
                        if (bodyA.IsStatic && bodyB.IsStatic)
                        {
                            continue;
                        }
                        else if (bodyA.IsStatic)
                        {
                            bodyB.Move(nor * depth * 0.5f);
                        }
                        else if (bodyB.IsStatic)
                        {
                            bodyA.Move(-nor * depth * 0.5f);
                        }
                        else
                        {
                            bodyA.Move(-nor * depth * 0.5f);
                            bodyB.Move(nor * depth * 0.5f);
                        }
                        ProcessingCollide(bodyA, bodyB, nor);
                    }
                }
            }
        }

        public void ProcessingCollide(PKBody bodyA, PKBody bodyB, PKVector nor)
        {
            PKVector relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;
            if (PKMath.Dot(relativeVelocity, nor) > 0) return;

            float e = Math.Min(bodyA.Restitution, bodyB.Restitution);
            float j = -(1f + e) * PKMath.Dot(relativeVelocity, nor) / (bodyA.InvMass + bodyB.InvMass);
            PKVector I = j * nor;
            bodyA.LinearVelocity -= I * bodyA.InvMass;
            bodyB.LinearVelocity += I * bodyB.InvMass;
        }

        public bool Collide(PKBody bodyA, PKBody bodyB, out PKVector nor, out float depth)
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
    }
}

//空间中存在一种无的子空间吗=>空间可以无限细分吗=>存在无限小的物质吗=>如果存在无限小的物质，那么这个物质真的是无限小吗=>无限小的概念能表示一个既定物体吗=>物体真实存在吗
//所看即为真？=>所不见即为假？=>何为真假？=>如何确定一个东西是不是真实存在？
//什么是世界？=>应该如何定义真实世界？=>如何判断所处的世界是不是真实的？=>什么是真实？
//存在一系列规则
