using PKPhysics.PKShape;

namespace PKPhysics
{
    public static class PKBodyUtil
    {
        public static bool CreateCircleBody(float radius, PKVector position, float density, bool isStatic, float restitution, out PKBody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;
            Cricle cricle = new Cricle(radius);
            float area = cricle.GetArea();

            if (area < PKWorld.MinBodySize)
            {
                errorMessage = $"Circle radius is too small. Min circle area is {PKWorld.MinBodySize}.";
                return false;
            }

            if (area > PKWorld.MaxBodySize)
            {
                errorMessage = $"Circle radius is too large. Max circle area is {PKWorld.MaxBodySize}.";
                return false;
            }

            if (density < PKWorld.MinDensity)
            {
                errorMessage = $"Density is too small. Min density is {PKWorld.MinDensity}";
                return false;
            }

            if (density > PKWorld.MaxDensity)
            {
                errorMessage = $"Density is too large. Max density is {PKWorld.MaxDensity}";
                return false;
            }

            restitution = PKMath.Clamp(restitution, 0f, 1f);

            float mass = area * density;

            body = new PKBody(position, density, mass, restitution, isStatic, cricle);
            return true;
        }

        public static bool CreateBoxBody(float width, float height, PKVector position, float density, bool isStatic, float restitution, out PKBody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;

            Box box = new Box(width, height);
            float area = box.GetArea();

            if (area < PKWorld.MinBodySize)
            {
                errorMessage = $"Area is too small. Min area is {PKWorld.MinBodySize}.";
                return false;
            }

            if (area > PKWorld.MaxBodySize)
            {
                errorMessage = $"Area is too large. Max area is {PKWorld.MaxBodySize}.";
                return false;
            }

            if (density < PKWorld.MinDensity)
            {
                errorMessage = $"Density is too small. Min density is {PKWorld.MinDensity}";
                return false;
            }

            if (density > PKWorld.MaxDensity)
            {
                errorMessage = $"Density is too large. Max density is {PKWorld.MaxDensity}";
                return false;
            }

            restitution = PKMath.Clamp(restitution, 0f, 1f);

            float mass = area * density;


            body = new PKBody(position, density, mass, restitution, isStatic, box);
            return true;
        }
    }
}
