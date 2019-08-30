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

        /// <summary>
        /// 获取转置矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            Matrix4x4 t = identity;

            for (int i = 0; i < 4; i++)
            {
                for (int j = i; j < 4; j++)
                {
                    t[i, j] = m[j, i];
                }
            }

            return t;
        }

        /// <summary>
        /// 获取伴随矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix4x4 Adjoint(Matrix4x4 m)
        {            
            Matrix4x4 a = identity;
            float[,] t = new float[3, 3];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int q = 0; q < 3; ++q)
                        {
                            int x = k >= i ? k + 1 : k;
                            int y = q >= j ? q + 1 : q;

                            t[k, q] = m[x, y];
                        }
                    }
                    a[i, j] = (float)System.Math.Pow(-1, (1 + j) + (1 + i)) * Determinate(t, 3);
                }
            }

            return Transpose(a);
        }

        /// <summary>
        /// 矩阵是否可逆
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool IsInvertibled(Matrix4x4 m)
        {
            float d = Determinate(m._values, 4);
            return d != 0;
        }

        /// <summary>
        /// 获取逆矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Matrix4x4 Inverse(Matrix4x4 m)
        {
            float d = Determinate(m._values, 4);
            System.Diagnostics.Debug.Assert(d != 0, "矩阵不可逆");

            Matrix4x4 a = Adjoint(m);//伴随矩阵
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    a[i, j] = a[i, j] / d;
                }
            }

            return a;
        }

        /// <summary>
        /// 获取平移矩阵
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Matrix4x4 Translation(Vector4 t)
        {
            float[] values = new[]{ 1,   0,   0,   0,
                                    0,   1,   0,   0,
                                    0,   0,   1,   0,
                                    t.x, t.y, t.z, 1, };

            return new Matrix4x4(values);
        }

        /// <summary>
        /// 获取旋转矩阵
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Matrix4x4 Rotation(Vector4 r)
        {
            Matrix4x4 x = RotationX(r.x);
            Matrix4x4 y = RotationY(r.y);
            Matrix4x4 z = RotationZ(r.z);

            return z * x * y;
        }

        /// <summary>
        /// 获取绕X轴旋转的矩阵
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取绕Y轴旋转的矩阵
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取绕Z轴旋转的矩阵
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取缩放矩阵
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Matrix4x4 Scale(Vector4 s)
        {
            float[] values = new []{ s.x, 0,   0,   0,
                                     0,   s.y, 0,   0,
                                     0,   0,   s.z, 0,
                                     0,   0,   0,   1, };

            return new Matrix4x4(values);
        }

        /// <summary>
        /// 0矩阵
        /// </summary>
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

        /// <summary>
        /// I矩阵
        /// </summary>
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

        /// <summary>
        /// 行列式
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static float Determinate(float[, ] m, int n)
        {
            if (n == 1)
            {
                return m[0, 0];
            }
            
            float result = 0;
            float[, ] t = new float[n - 1, n - 1];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    for (int k = 0; k < n - 1; k++)
                    {
                        int x = j + 1;              //原矩阵行
                        int y = k >= i ? k + 1 : k; //原矩阵列
                        t[j, k] = m[x, y];
                    }
                }

                result += (float)System.Math.Pow(-1, 1 + (1 + i)) * m[0, i] * Determinate(t, n - 1);
            }

            return result;
        }
    }
}
