using PKPhysics.PKShape;

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

        public PKVector Position
        {
            get
            {
                return position;
            }
        }

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
        }

        public void Move(PKVector amount)
        {
            this.position += amount;
        }

        public void MoveTo(PKVector pos)
        {
            this.position = pos;
        }
    }
}
