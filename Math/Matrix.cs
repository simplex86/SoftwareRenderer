using System;

namespace SoftwareRenderer
{
    struct Matrix
    {
        private float[] _values;
        private const int VALUES_LENGTH = 16;

        public Matrix(float[] values)
            : this()
        {
            _values = new float[VALUES_LENGTH];
            int length = Math.Min(VALUES_LENGTH, values.Length);

            for (int i = 0; i < length; i++)
            {
                _values[i] = values[i];
            }
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            float[] values = new float[VALUES_LENGTH];

            for (int i = 0; i < values.Length; i++)
            {
                int u = i / 4;
                int v = i % 4;

                values[i] = a._values[u * 4 + 0] * b._values[0 * 4 + v] +
                            a._values[u * 4 + 1] * b._values[1 * 4 + v] +
                            a._values[u * 4 + 2] * b._values[2 * 4 + v] +
                            a._values[u * 4 + 3] * b._values[3 * 4 + v];
            }

            return new Matrix(values);
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            float x = v.x * m[0, 0] + v.y * m[1, 0] + v.z * m[2, 0] + v.w * m[3, 0];
            float y = v.x * m[0, 1] + v.y * m[1, 1] + v.z * m[2, 1] + v.w * m[3, 1];
            float z = v.x * m[0, 2] + v.y * m[1, 2] + v.z * m[2, 2] + v.w * m[3, 2];
            float w = v.x * m[0, 3] + v.y * m[1, 3] + v.z * m[2, 3] + v.w * m[3, 3];

            return new Vector(x, y, z, w);
        }

        public float this[int row, int column]
        {
            set { _values[row * 4 + column] = value; }
            get { return _values[row * 4 + column]; }
        }

        public static Matrix Translation(Vector t)
        {
            float[] values = new[]{ 1,   0,   0,   0,
                                    0,   1,   0,   0,
                                    0,   0,   1,   0,
                                    t.x, t.y, t.z, 1, };

            return new Matrix(values);
        }

        public static Matrix Rotation(Vector r)
        {
            Matrix x = RotationX(r.x);
            Matrix y = RotationY(r.y);
            Matrix z = RotationZ(r.z);

            return z * x * y;
        }

        public static Matrix RotationX(float angle)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            float[] values = new []{ 1, 0,  0, 0,
                                     0, c,  s, 0,
                                     0, -s, c, 0,
                                     0, 0,  0, 1, };

            return new Matrix(values);
        }

        public static Matrix RotationY(float angle)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            float[] values = new []{ c, 0, -s, 0,
                                     0, 1, 0,  0,
                                     s, 0, c,  0,
                                     0, 0, 0,  1, };

            return new Matrix(values);
        }

        public static Matrix RotationZ(float angle)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            float[] values = new []{ c,  s, 0, 0,
                                     -s, c, 0, 0,
                                     0,  0, 1, 0,
                                     0,  0, 0, 1, };

            return new Matrix(values);
        }

        public static Matrix Scale(Vector s)
        {
            float[] values = new []{ s.x, 0,   0,   0,
                                     0,   s.y, 0,   0,
                                     0,   0,   s.z, 0,
                                     0,   0,   0,   1, };

            return new Matrix(values);
        }

        public static Matrix zero
        {
            get
            {
                return new Matrix(new[]{ 0.0f, 0.0f, 0.0f, 0.0f,
                                         0.0f, 0.0f, 0.0f, 0.0f,
                                         0.0f, 0.0f, 0.0f, 0.0f,
                                         0.0f, 0.0f, 0.0f, 0.0f });
            }
        }

        public static Matrix identity
        {
            get
            {
                return new Matrix(new[]{ 1.0f, 0.0f, 0.0f, 0.0f,
                                         0.0f, 1.0f, 0.0f, 0.0f,
                                         0.0f, 0.0f, 1.0f, 0.0f,
                                         0.0f, 0.0f, 0.0f, 1.0f });
            }
        }

        public new string ToString()
        {
            string str = string.Empty;

            for (int r=0; r<4; r++)
            {
                for (int c=0; c<4; c++)
                {
                    str += _values[r * 4 + c].ToString("F5");
                    str += ", ";
                }

                str += "\n";
            }

            return str;
        }
    }
}
