using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPhysics
{
    public readonly struct PKManifold
    {
        public readonly PKBody bodyA;
        public readonly PKBody bodyB;
        public readonly PKVector Nor;
        public readonly float depth;
        public readonly PKVector Contact1;
        public readonly PKVector Contact2;
        public readonly int ContactCount;

        public PKManifold(PKBody bodyA, PKBody bodyB, PKVector nor, float depth, PKVector contact1, PKVector contact2, int contactCount)
        {
            this.bodyA = bodyA;
            this.bodyB = bodyB;
            this.Nor = nor;
            this.depth = depth;
            this.ContactCount = contactCount;
            this.Contact1 = contact1;
            this.Contact2 = contact2;
        }
    }
}
