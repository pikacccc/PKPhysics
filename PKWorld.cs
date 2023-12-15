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

        public static readonly int MinIterations = 1;
        public static readonly int MaxIterations = 64;

        private PKVector gravity;
        private List<PKBody> bodyList;
        private List<PKManifold> contactList;

        public List<PKVector> ContactPointList;
        public PKWorld()
        {
            gravity = new PKVector(0, -9.81f);
            bodyList = new List<PKBody>();
            contactList = new List<PKManifold>();
            ContactPointList = new List<PKVector>();
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

        public void Step(float time, int iteration)
        {
            iteration = PKMath.Clamp(iteration, MinIterations, MaxIterations);

            for (int k = 0; k < iteration; ++k)
            {
                for (int i = 0; i < bodyList.Count; i++)
                {
                    bodyList[i].Step(time, this.gravity, iteration);
                }
                ContactPointList.Clear();
                contactList.Clear();
                for (int i = 0; i < bodyList.Count - 1; i++)
                {
                    var bodyA = bodyList[i];
                    for (int j = i + 1; j < bodyList.Count; j++)
                    {
                        var bodyB = bodyList[j];
                        if (PKCollisions.Collide(bodyA, bodyB, out PKVector nor, out float depth))
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
                            PKCollisions.FindContactPoints(bodyA, bodyB, out PKVector contact1, out PKVector contact2, out int contactCount);
                            PKManifold tempContact = new PKManifold(bodyA, bodyB, nor, depth, contact1, contact2, contactCount);
                            contactList.Add(tempContact);
                        }
                    }
                }

                foreach (var contact in contactList)
                {
                    ProcessingCollide(contact);
                    if (contact.ContactCount > 0)
                    {
                        ContactPointList.Add(contact.Contact1);
                        if (contact.ContactCount > 1)
                        {
                            ContactPointList.Add(contact.Contact2);
                        }
                    }
                }
            }

        }

        public void ProcessingCollide(in PKManifold contact)
        {
            PKBody bodyA = contact.bodyA;
            PKBody bodyB = contact.bodyB;
            PKVector nor = contact.Nor;
            PKVector relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;
            if (PKMath.Dot(relativeVelocity, nor) > 0) return;

            float e = Math.Min(bodyA.Restitution, bodyB.Restitution);
            float j = -(1f + e) * PKMath.Dot(relativeVelocity, nor) / (bodyA.InvMass + bodyB.InvMass);
            PKVector I = j * nor;
            bodyA.LinearVelocity -= I * bodyA.InvMass;
            bodyB.LinearVelocity += I * bodyB.InvMass;
        }

    }
}

//空间中存在一种无的子空间吗=>空间可以无限细分吗=>存在无限小的物质吗=>如果存在无限小的物质，那么这个物质真的是无限小吗=>无限小的概念能表示一个既定物体吗=>物体真实存在吗
//所看即为真？=>所不见即为假？=>何为真假？=>如何确定一个东西是不是真实存在？
//什么是世界？=>应该如何定义真实世界？=>如何判断所处的世界是不是真实的？=>什么是真实？
//存在一系列规则
