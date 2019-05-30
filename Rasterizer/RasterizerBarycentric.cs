using System;
using System.Drawing;

namespace SoftwareRenderer
{
    /// <summary>
    /// 扫描三角形的BoundingRect实现三角形光栅化
    /// ___________________
    /// |     * A         |
    /// |    * *          |
    /// |   *   *         |
    /// |  *     *        |
    /// |B*       *M      |
    /// |   *      *      |
    /// |     *     *     |
    /// |       *    *    |
    /// |         *   *   |
    /// |           *  *  |
    /// |             * * |
    /// |             C  *|
    /// -------------------
    /// 参考文献：
    /// Barycentric Algorithm - http://www.sunshine2k.de/coding/java/TriangleRasterization/TriangleRasterization.html#algo3
    /// 实践表明，实现简单但是性能很差！
    /// </summary>
    class RasterizerBarycentric : Rasterizer
    {
        public override void Do(Vector pa, Vector pb, Vector pc,
                                Color  ca, Color  cb, Color  cc,
                                UV     ua, UV     ub, UV     uc)
        {
            fragments.Clear();

            int left   = (int)Math.Min(pa.x, Math.Min(pb.x, pc.x));
            int right  = (int)Math.Max(pa.x, Math.Max(pb.x, pc.x));
            int top    = (int)Math.Min(pa.y, Math.Min(pb.y, pc.y));
            int bottom = (int)Math.Max(pa.y, Math.Max(pb.y, pc.y));

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Vector pp = new Vector(x, y, 0, 0);

                    if (InTriangle(ref pp, ref pa, ref pb, ref pc))
                    {
                        Fragment fg = new Fragment();
                        fg.x = x;
                        fg.y = y;
                        //TODO 还需要完成depth、color和uv的插值

                        fragments.Add(fg);
                    }
                }
            }
        }

        private bool InTriangle(ref Vector p, ref Vector a, ref Vector b, ref Vector c)
        {
            Vector pa = a - p;
            Vector pb = b - p;
            Vector pc = c - p;

            Vector t1 = Vector.Cross(pa, pb);
            Vector t2 = Vector.Cross(pb, pc);
            Vector t3 = Vector.Cross(pc, pa);

            return (t1.z < 0 && t2.z < 0 && t3.z < 0) ||
                   (t1.z > 0 && t2.z > 0 && t3.z > 0);
        }
    }
}
