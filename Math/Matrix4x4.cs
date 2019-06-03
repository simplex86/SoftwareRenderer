using System;

namespace SoftwareRenderer
{
    struct Matrix4x4
    {
        private float[, ] _values;

        public Matrix4x4(float[] values)
            : this()
        {
            _values = new float[4, 4];
            int length = (int)Mathf.Min(16, values.Length);

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    _values[r, c] = values[r * 4 + c];
                }
            }
        }

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            float[] values = new float[16];

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    int i = r * 4 + c;
                    values[i] = a._values[r, 0] * b._values[0, c] +
                                a._values[r, 1] * b._values[1, c] +
                                a._values[r, 2] * b._values[2, c] +
                                a._values[r, 3] * b._values[3, c];
                }
            }

            return new Matrix4x4(values);
        }

        public static Vector4 operator *(Matrix4x4 m, Vector4 v)
        {
            float x = v.x * m[0, 0] + v.y * m[1, 0] + v.z * m[2, 0] + v.w * m[3, 0];
            float y = v.x * m[0, 1] + v.y * m[1, 1] + v.z * m[2, 1] + v.w * m[3, 1];
            float z = v.x * m[0, 2] + v.y * m[1, 2] + v.z * m[2, 2] + v.w * m[3, 2];
            float w = v.x * m[0, 3] + v.y * m[1, 3] + v.z * m[2, 3] + v.w * m[3, 3];

            return new Vector4(x, y, z, w);
        }

        public float this[int r, int c]
        {
            set { _values[r, c] = value; }
            get { return _values[r, c]; }
        }

        public static Matrix4x4 Translation(Vector4 t)
        {
            float[] values = new[]{ 1,   0,   0,   0,
                                    0,   1,   0,   0,
                                    0,   0,   1,   0,
                                    t.x, t.y, t.z, 1, };

            return new Matrix4x4(values);
        }

        public static Matrix4x4 Rotation(Vector4 r)
        {
            Matrix4x4 x = RotationX(r.x);
            Matrix4x4 y = RotationY(r.y);
            Matrix4x4 z = RotationZ(r.z);

            return z * x * y;
        }

        public static Matrix4x4 RotationX(float angle)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);
                          
            float[] values = new []{ 1,  0,  0, 0,
                                     0,  c,  s, 0,
                                     0, -s,  c, 0,
                                     0,  0,  0, 1, };

            return new Matrix4x4(values);
        }

        public static Matrix4x4 RotationY(float angle)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            float[] values = new []{ c, 0, -s,  0,
                                     0, 1,  0,  0,
                                     s, 0,  c,  0,
                                     0, 0,  0,  1, };

            return new Matrix4x4(values);
        }

        public static Matrix4x4 RotationZ(float angle)
        {
            float s = Mathf.Sin(angle);
            float c = Mathf.Cos(angle);

            float[] values = new []{  c, s, 0, 0,
                                     -s, c, 0, 0,
                                      0, 0, 1, 0,
                                      0, 0, 0, 1, };

            return new Matrix4x4(values);
        }

        public static Matrix4x4 Scale(Vector4 s)
        {
            float[] values = new []{ s.x, 0,   0,   0,
                                     0,   s.y, 0,   0,
                                     0,   0,   s.z, 0,
                                     0,   0,   0,   1, };

            return new Matrix4x4(values);
        }

        public static Matrix4x4 zero
        {
            get
            {
                return new Matrix4x4(new[]{ 0.0f, 0.0f, 0.0f, 0.0f,
                                            0.0f, 0.0f, 0.0f, 0.0f,
                                            0.0f, 0.0f, 0.0f, 0.0f,
                                            0.0f, 0.0f, 0.0f, 0.0f });
            }
        }

        public static Matrix4x4 identity
        {
            get
            {
                return new Matrix4x4(new[]{ 1.0f, 0.0f, 0.0f, 0.0f,
                                            0.0f, 1.0f, 0.0f, 0.0f,
                                            0.0f, 0.0f, 1.0f, 0.0f,
                                            0.0f, 0.0f, 0.0f, 1.0f });
            }
        }

        public new string ToString()
        {
            string str = string.Empty;

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    str += _values[r, c].ToString("F5");
                    str += ", ";
                }

                str += "\n";
            }

            return str;
        }
    }
}
