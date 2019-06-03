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
    class TriangleStandardRasterizer : TriangleRasterizer
    {
        public override List<Fragment> Do(Vertex a, Vertex b, Vertex c)
        {
            _fragments.Clear();
            Sort(ref a, ref b, ref c);

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
                float z = LerpZ(pa.x, pc.x, x, 1 / pa.z, 1 / pb.z);

                Vertex m = new Vertex();
                m.position = new Vector4(x, y, 1 / z);
                m.uv = c.uv;//TODO 插值得到

                RasterizeBottomTriangle(a, b, m);
                RasterizeTopTriangle(b, m, c);
            }
            Color(a, b, c);

            return _fragments;
        }

        private void Sort(ref Vertex a, ref Vertex b, ref Vertex c)
        {
            Vector4   pt;
            Color4    ct;
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
                    float sz = LerpZ(pc.y, pa.y, y, 1 / pc.z, 1 / pa.z);
                    float ez = LerpZ(pc.y, pb.y, y, 1 / pc.z, 1 / pb.z);

                    ScanLine((int)x_ca, (int)x_cb, y, sz, ez);
                    x_ca -= invslope_ca;
                    x_cb -= invslope_cb;
                }
            }
            else
            {
                for (int y = (int)pc.y; y >= (int)pa.y; y--)
                {
                    float sz = LerpZ(pc.y, pb.y, y, 1 / pc.z, 1 / pb.z);
                    float ez = LerpZ(pc.y, pa.y, y, 1 / pc.z, 1 / pa.z);

                    ScanLine((int)x_cb, (int)x_ca, y, sz, ez);
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
                    float sz = LerpZ(pa.y, pb.y, y, 1 / pa.z, 1 / pb.z);
                    float ez = LerpZ(pa.y, pc.y, y, 1 / pa.z, 1 / pc.z);

                    ScanLine((int)x_ab, (int)x_ac, y, sz, ez);
                    x_ab += invslope_ab;
                    x_ac += invslope_ac;
                }
            }
            else
            {
                for (int y = (int)pa.y; y <= (int)pb.y; y++)
                {
                    float sz = LerpZ(pa.y, pc.y, y, 1 / pa.z, 1 / pc.z);
                    float ez = LerpZ(pa.y, pb.y, y, 1 / pa.z, 1 / pb.z);

                    ScanLine((int)x_ac, (int)x_ab, y, sz, ez);
                    x_ab += invslope_ab;
                    x_ac += invslope_ac;
                }
            }
        }

        private void ScanLine(int sx, int ex, int y, float sz, float ez)
        {
            for (int x = sx; x <= ex; x++)
            {
                float iz = LerpZ(sx, ex, x, sz, ez);
                //TODO 还需要完成uv的插值
                Fragment fg = new Fragment(x, y, 1 / iz);
                _fragments.Add(fg);
            }
        }
    }
}
