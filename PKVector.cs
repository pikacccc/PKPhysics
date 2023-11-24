using System;

namespace PKPhysics
{
    public struct PKVector
    {
        public float X;
        public float Y;

        public static PKVector Zero = new PKVector(0, 0);

        public static PKVector Down = new PKVector(0, -1);

        public static PKVector Up = new PKVector(0, 1);

        public static PKVector Right = new PKVector(1, 0);

        public static PKVector Left = new PKVector(-1, 0);

        public PKVector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static PKVector operator +(PKVector a, PKVector b)
        {
            return new PKVector(a.X + b.X, a.Y + b.Y);
        }

        public static PKVector operator -(PKVector a, PKVector b)
        {
            return new PKVector(a.X - b.X, a.Y - b.Y);
        }

        public static PKVector operator -(PKVector a)
        {
            return new PKVector(-a.X, -a.Y);
        }


        public static PKVector operator *(PKVector a, float b)
        {
            return new PKVector(a.X * b, a.Y * b);
        }

        public static PKVector operator /(PKVector a, float b)
        {
            return new PKVector(a.X / b, a.Y / b);
        }

        public static float Dot(PKVector a, PKVector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        internal static PKVector Transform(PKVector v, PKTransform trans)
        {
            return new PKVector(trans.Cos * v.X - trans.Sin * v.Y + trans.PositionX, trans.Sin * v.X + trans.Cos * v.Y + trans.PositionY);
        }

        public float GetModulus()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public PKVector Normalized()
        {
            var mod = GetModulus();
            return this / mod;
        }

        public bool Equals(PKVector other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is PKVector other)
            {
                return Equals(other);
            }
            return false;
        }

        public override string ToString()
        {
            return $"X:{X},Y:{Y}";
        }

        public override int GetHashCode()
        {
            return new { X, Y }.GetHashCode();
        }
    }
}
