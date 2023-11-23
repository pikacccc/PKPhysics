using System;

namespace PKPhysics
{
    public class PKMath
    {
        public static float Clamp(float value, float min, float max)
        {
            if (min == max) return min;
            if (min > max) throw new ArgumentOutOfRangeException("最小值大于最大值");
            if (value > max) return max;
            if (value < min) return min;
            return value;
        }

        public static float Length(PKVector a)
        {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y);
        }

        public static float Distance(PKVector a, PKVector b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }


        public static PKVector Normalize(PKVector a)
        {
            float len = Length(a);
            if (len == 0) return a;
            return a / len;
        }

        public static float Dot(PKVector a, PKVector b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float Cross(PKVector a, PKVector b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
