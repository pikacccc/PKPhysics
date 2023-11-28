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
        public float Density;
        public float Mass;
        public float Restitution;
        public float Area;

        public bool IsStatic;

        public ShapeBase shape;

        private PKVector[] transVertics;

        public PKVector Position
        {
            get
            {
                return position;
            }
        }

        public PKVector LinearVelocity
        {
            get { return linearVelocity; }
            set { linearVelocity = value; }
        }

        public bool transformUpdatedRequired;

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
        }

        public void Update(float dt)
        {
            this.linearVelocity += this.force / this.Mass * dt;
            this.position += linearVelocity * dt;
            this.rotation += rotationalVelocity * dt;
            this.force = PKVector.Zero;
            if (PKMath.Length(this.linearVelocity) != 0f)
            {
                this.transformUpdatedRequired = true;
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
    }
}
