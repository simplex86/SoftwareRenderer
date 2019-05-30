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
    /// |B*       *       |
    /// |   *      *      |
    /// |     *     *     |
    /// |       *    *    |
    /// |         *   *   |
    /// |           *  *  |
    /// |             * * |
    /// |              C *|
    /// -------------------
    /// 
    /// 参考文献：
    /// Barycentric Algorithm - http://www.sunshine2k.de/coding/java/TriangleRasterization/TriangleRasterization.html#algo3
    /// 
    /// 实践表明，实现简单但是性能很差！！！！！！！！！
    /// </summary>
    class RasterizerBarycentric : Rasterizer
    {
        public override void Do(Vector pa, Vector pb, Vector pc,
                                Color  ca, Color  cb, Color  cc,
                                UV     ua, UV     ub, UV     uc)
        {
            fragments.Clear();

            int left   = (int)Mathf.Min(pa.x, pb.x, pc.x);
            int right  = (int)Mathf.Max(pa.x, pb.x, pc.x);
            int top    = (int)Mathf.Min(pa.y, pb.y, pc.y);
            int bottom = (int)Mathf.Max(pa.y, pb.y, pc.y);

            Vector ab = pb - pa;
            Vector ac = pc - pa;

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Vector pp = new Vector(x - pa.x, y - pa.y, 0, 0);

                    float s = Vector.Cross(pp, ac).z / Vector.Cross(ab, ac).z;
                    float t = Vector.Cross(ab, pp).z / Vector.Cross(ab, ac).z;

                    if ((s >= 0) && (t >= 0) && (s + t <= 1))
                    {
                        Fragment fg = new Fragment();
                        fg.x = x;
                        fg.y = y;
                        //TODO 还需要完成depth和uv的插值

                        fragments.Add(fg);
                    }
                }
            }
        }
    }
}
