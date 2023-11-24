using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics
{
    internal readonly struct PKTransform
    {
        public readonly float PositionX;
        public readonly float PositionY;

        public readonly float Sin;
        public readonly float Cos;

        public static readonly PKTransform Zero = new PKTransform(0f, 0f, 0f);

        public PKTransform(float positionX, float positionY, float Angle)
        {
            PositionX = positionX;
            PositionY = positionY;
            Sin = (float)Math.Sin(Angle);
            Cos = (float)Math.Cos(Angle);
        }

        public PKTransform(PKVector position, float Angle)
        {
            PositionX = position.X;
            PositionY = position.Y;
            Sin = (float)Math.Sin(Angle);
            Cos = (float)Math.Cos(Angle);
        }
    }
}
