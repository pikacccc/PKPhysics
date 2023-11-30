using PKPhysics.PKShape;
using System.Security.Cryptography.X509Certificates;

namespace PKPhysics
{
    public sealed class PKBody
    {
        private PKVector position;
        private PKVector linearVelocity;
        private float rotation;
        private float rotationalVelocity;
        private PKVector force;
        private PKVector[] transVertics;

        public float Restitution;
        public float Area;
        public bool IsStatic;
        public ShapeBase shape;
        public float Density;
        public float Mass;
        public PKVector Position
        {
            get
            {
                return position;
            }
        }

        public float InvMass
        {
            get
            {
                if (IsStatic)
                {
                    return 0;
                }
                else
                {
                    return 1f / Mass;
                }
            }
        }

        public PKVector LinearVelocity
        {
            get { return linearVelocity; }
            set { linearVelocity = value; }
        }

        public bool transformUpdatedRequired;
        public bool AABBUpdateRequired;

        public PKAABB AABB;

        public PKBody(PKVector pos, float density, float mass, float restitution, bool isStatic, ShapeBase shape)
        {
            this.position = pos;
            this.Density = density;
            this.rotation = 0;
            this.rotationalVelocity = 0;
            this.force = PKVector.Zero;
            this.Mass = mass;
            this.Restitution = restitution;
            this.IsStatic = isStatic;
            this.Area = shape.GetArea();
            this.shape = shape;

            this.transVertics = new PKVector[4];
            this.transformUpdatedRequired = true;
            this.AABBUpdateRequired = true;
        }

        internal void Update(float dt, PKVector gravity)
        {
            if (IsStatic) { return; }
            this.linearVelocity += this.force / this.Mass * dt + gravity * dt;
            this.position += linearVelocity * dt;
            this.rotation += rotationalVelocity * dt;
            this.force = PKVector.Zero;
            if (PKMath.Length(this.linearVelocity) != 0f)
            {
                this.transformUpdatedRequired = true;
                this.AABBUpdateRequired = true;
            }
        }

        public void AddForce(PKVector force)
        {
            this.force = force;
        }

        public void Move(PKVector amount)
        {
            this.position += amount;
            this.transformUpdatedRequired = true;
            this.AABBUpdateRequired = true;
        }

        public void MoveTo(PKVector pos)
        {
            this.position = pos;
            this.transformUpdatedRequired = true;
            this.AABBUpdateRequired = true;
        }

        public void Rotate(float dr)
        {
            this.rotation += dr;
            this.transformUpdatedRequired = true;
            this.AABBUpdateRequired = true;
        }

        public PKVector[] GetTransformedVertics()
        {
            if (this.transVertics == null)
            {
                this.transformUpdatedRequired = false;
                return null;
            }
            if (this.transformUpdatedRequired)
            {
                PKTransform trans = new PKTransform(this.position, this.rotation);

                for (int i = 0; i < this.transVertics.Length; i++)
                {
                    transVertics[i] = PKVector.Transform(shape.Vertics[i], trans);
                }
            }
            this.transformUpdatedRequired = false;

            return transVertics;
        }

        public PKAABB GetAABB()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            if (AABBUpdateRequired)
            {
                if (shape.ShapeType == ShapeType.Polygon)
                {
                    for (int i = 0; i < transVertics.Length; ++i)
                    {
                        PKVector v = transVertics[i];
                        if (minX > v.X) minX = v.X;
                        if (minY > v.Y) minY = v.Y;
                        if (maxX < v.X) maxX = v.X;
                        if (maxY < v.Y) maxY = v.Y;
                    }

                }
                else if (shape.ShapeType == ShapeType.Circle)
                {
                    minX = position.X - (shape as Cricle).Radius;
                    minY = position.Y - (shape as Cricle).Radius;
                    maxX = position.X + (shape as Cricle).Radius;
                    maxY = position.Y + (shape as Cricle).Radius;
                }
                else
                {
                    throw new System.Exception("未知形状");
                }

                AABB = new PKAABB(minX, minY, maxX, maxY);
                AABBUpdateRequired = false;
            }
            return AABB;
        }
    }
}
