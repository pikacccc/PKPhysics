﻿using PKPhysics.PKShape;
using System.Security.Cryptography.X509Certificates;

namespace PKPhysics
{
    public sealed class PKBody<T> where T : ShapeBase
    {
        private PKVector position;
        private PKVector linearVelocity;
        private float rotation;
        private float rotationalVelocity;

        public float Density;
        public float Mass;
        public float Restitution;
        public float Area;

        public bool IsStatic;

        public T shape;

        public PKVector[] transVertics;

        public PKVector Position
        {
            get
            {
                return position;
            }
        }

        public bool transformUpdatedRequired;

        public PKBody(PKVector pos, float density, float mass, float restitution, bool isStatic, T shape)
        {

            this.position = pos;
            this.Density = density;
            this.rotation = 0;
            this.rotationalVelocity = 0;
            this.Mass = mass;
            this.Restitution = restitution;
            this.IsStatic = isStatic;
            this.Area = shape.GetArea();
            this.shape = shape;

            this.transVertics = new PKVector[4];
            this.transformUpdatedRequired = true;
        }

        public void Move(PKVector amount)
        {
            this.position += amount;
            this.transformUpdatedRequired = true;
        }

        public void MoveTo(PKVector pos)
        {
            this.position = pos;
            this.transformUpdatedRequired = true;
        }

        public void Rotate(float dr)
        {
            this.rotation += dr;
            this.transformUpdatedRequired = true;
        }

        public PKVector[] GetTransformedVertics()
        {
            if (this.transVertics == null) return null;
            PKTransform trans = new PKTransform(this.position, this.rotation);

            for (int i = 0; i < this.transVertics.Length; i++)
            {
                transVertics[i] = PKVector.Transform(shape.Vertics[i], trans);
            }
            return transVertics;
        }
    }
}
