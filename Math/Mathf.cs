using System;

namespace SoftwareRenderer
{
    class Mathf
    {
        private const float EPSILON = float.Epsilon;
        private const float DEG_TO_RAD = (float)Math.PI / 180.0f;

        public static float Abs(float a)
        {
            return Math.Abs(a);
        }

        public static float Max(float a, float b)
        {
            return Math.Max(a, b);
        }

        public static float Max(float a, float b, float c)
        {
            return Max(a, Max(b, c));
        }

        public static float Min(float a, float b)
        {
            return Math.Min(a, b);
        }

        public static float Min(float a, float b, float c)
        {
            return Min(a, Min(b, c));
        }

        public static float Sqrt(float a)
        {
            return (float)Math.Sqrt(a);
        }

        public static bool Eq(float a, float b)
        {
            return Abs(a - b) <= EPSILON;
        }

        public static bool IsZero(float a)
        {
            return Eq(a, 0.0f);
        }

        public static float Sin(float a)
        {
            return (float)Math.Sin(a);
        }

        public static float Cos(float a)
        {
            return (float)Math.Cos(a);
        }

        public static float Tan(float a)
        {
            return (float)Math.Tan(a);
        }

        public static float Deg2Rad(float d)
        {
            return DEG_TO_RAD * d;
        }

        public static float Clamp(float a, float min, float max)
        {
            if (a < min) return min;
            if (a > max) return max;

            return a;
        }

        public static float Clamp01(float a)
        {
            return Clamp(a, 0, 1);
        }

        public static float Lerp(float a, float b, float t)
        {
            t = Clamp01(t);
            return a + (b - a) * t;
        }

        public static float Reciprocal(float a)
        {
            return Mathf.IsZero(a) ? 0.0f : 1.0f / a;
        }
    }
}
