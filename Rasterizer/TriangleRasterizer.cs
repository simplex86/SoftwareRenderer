using System;
using System.Collections.Generic;
using System.Drawing;

namespace SoftwareRenderer
{
    /// <summary>
    /// 将三角形拆分为平底三角形+平顶三角形，通过直接扫描三角形来进行光栅化
    ///      A     
    ///      *
    ///     * *
    ///    *   *
    ///   *     *
    /// B*-------*M
    ///    *      *
    ///      *     *
    ///        *    *
    ///          *   *
    ///            *  *
    ///              * *
    ///                 *
    ///                 C
    /// 参考文献：
    /// Standard Algorithm - http://www.sunshine2k.de/coding/java/TriangleRasterization/TriangleRasterization.html#algo1
    /// </summary>
    class TriangleRasterizer : Rasterizer
    {
        private Vertex _a;
        private Vertex _b;
        private Vertex _c;

        public override List<Fragment> Do(Vertex a, Vertex b, Vertex c)
        {
            _fragments.Clear();
            Sort(ref a, ref b, ref c);

            _a = a;
            _b = b;
            _c = c;

            if (Mathf.Eq(b.position.y, c.position.y))
            {
                RasterizeBottomTriangle(a, b, c);
            }
            else if (Mathf.Eq(a.position.y, b.position.y))
            {
                RasterizeTopTriangle(a, b, c);
            }
            else
            {
                Vector4 pa = a.position;
                Vector4 pb = b.position;
                Vector4 pc = c.position;

                float x = pa.x + (pb.y - pa.y) / (pc.y - pa.y) * (pc.x - pa.x);
                float y = pb.y;
                float z = LerpZ(pa.x, pc.x, x, pa.z, pb.z);

                Vertex m = new Vertex { position = new Vector4(x, y, z) };

                RasterizeBottomTriangle(a, b, m);
                RasterizeTopTriangle(b, m, c);
            }

            return _fragments;
        }

        private void Sort(ref Vertex a, ref Vertex b, ref Vertex c)
        {
            Vector4  pt;
            Color4   ct;
            TexCoord ut;

            if (a.position.y > b.position.y)
            {
                pt = a.position;
                a.position = b.position;
                b.position = pt;

                ct = a.color;
                a.color = b.color;
                b.color = ct;

                ut = a.uv;
                a.uv = b.uv;
                b.uv = ut;
            }

            if (a.position.y > c.position.y)
            {
                pt = a.position;
                a.position = c.position;
                c.position = pt;

                ct = a.color;
                a.color = c.color;
                c.color = ct;

                ut = a.uv;
                a.uv = c.uv;
                c.uv = ut;
            }

            if (b.position.y > c.position.y)
            {
                pt = b.position;
                b.position = c.position;
                c.position = pt;

                ct = b.color;
                b.color = c.color;
                c.color = ct;

                ut = b.uv;
                b.uv = c.uv;
                c.uv = ut;
            }
        }

        private void RasterizeTopTriangle(Vertex a, Vertex b, Vertex c)
        {
            Vector4 pa = a.position;
            Vector4 pb = b.position;
            Vector4 pc = c.position;

            float invslope_ca = (pc.x - pa.x) / (pc.y - pa.y);
            float invslope_cb = (pc.x - pb.x) / (pc.y - pb.y);

            float x_ca = pc.x;
            float x_cb = pc.x;

            if (invslope_ca > invslope_cb)
            {
                for (int y = (int)pc.y; y >= (int)pa.y; y--)
                {
                    float sz = LerpZ(pc.y, pa.y, y, pc.z, pa.z);
                    float ez = LerpZ(pc.y, pb.y, y, pc.z, pb.z);
                    int sx = (int)x_ca;
                    int ex = (int)x_cb;

                    ScanLine(sx, ex, y, sz, ez);
                    x_ca -= invslope_ca;
                    x_cb -= invslope_cb;
                }
            }
            else
            {
                for (int y = (int)pc.y; y >= (int)pa.y; y--)
                {
                    float sz = LerpZ(pc.y, pb.y, y, pc.z, pb.z);
                    float ez = LerpZ(pc.y, pa.y, y, pc.z, pa.z);
                    int sx = (int)x_cb;
                    int ex = (int)x_ca;

                    ScanLine(sx, ex, y, sz, ez);
                    x_ca -= invslope_ca;
                    x_cb -= invslope_cb;
                }
            }
        }

        private void RasterizeBottomTriangle(Vertex a, Vertex b, Vertex c)
        {
            Vector4 pa = a.position;
            Vector4 pb = b.position;
            Vector4 pc = c.position;

            float invslope_ab = (pa.x - pb.x) / (pa.y - pb.y);
            float invslope_ac = (pa.x - pc.x) / (pa.y - pc.y);

            float x_ab = pa.x;
            float x_ac = pa.x;

            if (invslope_ab < invslope_ac)
            {
                for (int y = (int)pa.y; y <= (int)pb.y; y++)
                {
                    int sx = (int)x_ab;
                    int ex = (int)x_ac;
                    float sz = LerpZ(pa.y, pb.y, y, pa.z, pb.z);
                    float ez = LerpZ(pa.y, pc.y, y, pa.z, pc.z);

                    ScanLine(sx, ex, y, sz, ez);
                    x_ab += invslope_ab;
                    x_ac += invslope_ac;
                }
            }
            else
            {
                for (int y = (int)pa.y; y <= (int)pb.y; y++)
                {
                    int sx = (int)x_ac;
                    int ex = (int)x_ab;
                    float sz = LerpZ(pa.y, pc.y, y, pa.z, pc.z);
                    float ez = LerpZ(pa.y, pb.y, y, pa.z, pb.z);

                    ScanLine(sx, ex, y, sz, ez);
                    x_ab += invslope_ab;
                    x_ac += invslope_ac;
                }
            }
        }

        private void ScanLine(int sx, int ex, int y, float sz, float ez)
        {
            for (int x = sx; x <= ex; x++)
            {
                float z = LerpZ(sx, ex, x, sz, ez);

                Vector4 pa = _a.position;
                Vector4 pb = _b.position;
                Vector4 pc = _c.position;
                //计算每个像素的重心坐标
                float t = ((pb.y - pc.y) * x + (pc.x - pb.x) * y + (pb.x * pc.y - pc.x * pb.y)) / ((pb.y - pc.y) * pa.x + (pc.x - pb.x) * pa.y + (pb.x * pc.y - pc.x * pb.y));
                float s = ((pa.y - pc.y) * x + (pc.x - pa.x) * y + (pa.x * pc.y - pc.x * pa.y)) / ((pa.y - pc.y) * pb.x + (pc.x - pa.x) * pb.y + (pa.x * pc.y - pc.x * pa.y)); 
                float w = 1 - t - s;
                //插值color和uv
                Color4 c = _a.color * t + _b.color * s + _c.color * w;
                TexCoord uv = _a.uv * t + _b.uv * s + _c.uv * w;

                Fragment fragment = new Fragment(x, y, z, c, uv);
                _fragments.Add(fragment);
            }
        }

        protected float LerpZ(float s, float e, float x, float sz, float ez)
        {
            if (Mathf.Eq(e, s))
                return sz;

            sz = Mathf.Inverse(sz);
            ez = Mathf.Inverse(ez);

            float t = Mathf.Eq(0.0f, e - s) ? 1.0f : (x - s) / (e - s);
            float z = Mathf.Lerp(sz, ez, t);

            return Mathf.Inverse(z);
        }
    }
}
