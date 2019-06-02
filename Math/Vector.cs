using System;

namespace SoftwareRenderer
{
    struct Vector
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }

        public Vector(float x, float y, float z, float w = 1)
            : this()
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector(Vector v)
            : this()
        {
            x = v.x;
            y = v.y;
            z = v.z;
            w = v.w;
        }

        public float length
        {
            get { return Mathf.Sqrt(x * x + y * y + z * z); }
        }

        public static float Dot(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vector Cross(Vector a, Vector b)
        {
            return new Vector(a.y * b.z - a.z * b.y,
                              a.z * b.x - a.x * b.z,
                              a.x * b.y - a.y * b.x);
        }

        public static Vector Normalize(Vector v)
        {
            float length = v.length;
            float factor = Mathf.Eq(length, 0.0f) ? 0.0f : 1.0f / length;

            return new Vector(v.x * factor,
                              v.y * factor,
                              v.z * factor);
        }

        public void DivW()
        {
            if (!Mathf.Eq(w, 0.0f))
            {
                x /= w;
                y /= w;
                z /= w;
                w = 1.0f;
            }
        }

        public static Vector zero
        {
            get { return new Vector(0, 0, 0); }
        }

        public static Vector one
        {
            get { return new Vector(1, 1, 1); }
        }

        public static Vector right
        {
            get { return new Vector(1, 0, 0, 0); }
        }

        public static Vector up
        {
            get { return new Vector(0, 1, 0, 0); }
        }

        public static Vector forward
        {
            get { return new Vector(0, 0, 1, 0); }
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector operator *(Vector v, float factor)
        {
            return new Vector(v.x * factor, v.y * factor, v.z * factor);
        }

        public static Vector operator /(Vector v, float factor)
        {
            if (Mathf.Eq(factor, 0.0f))
            {
                return Vector.zero;
            }

            return new Vector(v.x / factor, v.y / factor, v.z / factor);
        }

        public static Vector Lerp(Vector a, Vector b, float factor)
        {
            return a + (b - a) * factor;
        }

        public static Vector operator *(Vector v, Matrix m)
        {
            float x = v.x * m[0, 0] + v.y * m[1, 0] + v.z * m[2, 0] + v.w * m[3, 0];
            float y = v.x * m[0, 1] + v.y * m[1, 1] + v.z * m[2, 1] + v.w * m[3, 1];
            float z = v.x * m[0, 2] + v.y * m[1, 2] + v.z * m[2, 2] + v.w * m[3, 2];
            float w = v.x * m[0, 3] + v.y * m[1, 3] + v.z * m[2, 3] + v.w * m[3, 3];

            return new Vector(x, y, z, w);
        }

        public new string ToString()
        {
            return string.Format("{0:F5}, {1:F5}, {2:F5}, {3:F5}", x, y, z, w);
        }
    }
}
