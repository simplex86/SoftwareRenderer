using System.Collections.Generic;
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
    class TriangleBarycentricRasterizer : TriangleRasterizer
    {
        public override List<Fragment> Do(Vertex a, Vertex b, Vertex c)
        {
            _fragments.Clear();

            Vector4 pa = a.position;
            Vector4 pb = b.position;
            Vector4 pc = c.position;

            int left   = (int)Mathf.Min(pa.x, pb.x, pc.x);
            int right  = (int)Mathf.Max(pa.x, pb.x, pc.x);
            int top    = (int)Mathf.Min(pa.y, pb.y, pc.y);
            int bottom = (int)Mathf.Max(pa.y, pb.y, pc.y);

            Vector4 ab = pb - pa;
            Vector4 ac = pc - pa;

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Vector4 pp = new Vector4(x - pa.x, y - pa.y, 0, 0);

                    float s = Vector4.Cross(pp, ac).z / Vector4.Cross(ab, ac).z;
                    float t = Vector4.Cross(ab, pp).z / Vector4.Cross(ab, ac).z;

                    if ((s >= 0) && (t >= 0) && (s + t <= 1))
                    {
                        //TODO 还需要完成z和uv的插值
                        Fragment fg = new Fragment(x, y);
                        _fragments.Add(fg);
                    }
                }
            }
            Color(a, b, c);

            return _fragments;
        }
    }
}
