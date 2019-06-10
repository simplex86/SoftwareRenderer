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
    class TriangleBarycentricRasterizer : Rasterizer
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
                        Fragment fg = GetFragment(a, b, c, x, y);
                        _fragments.Add(fg);
                    }
                }
            }

            return _fragments;
        }

        protected Fragment GetFragment(Vertex a, Vertex b, Vertex c, int x, int y)
        {
            Vector4 pa = a.position;
            Vector4 pb = b.position;
            Vector4 pc = c.position;

            float z = float.MaxValue;//TODO 插值
            float s = 1 / GetTriangleArea(pa, pb, pc);
            Vector4 pt = new Vector4(x, y, z);

            float s1 = GetTriangleArea(pt, pa, pb) * s;
            float s2 = GetTriangleArea(pt, pb, pc) * s;
            float s3 = GetTriangleArea(pt, pc, pa) * s;

            Color4 cc = a.color * s2 + b.color * s3 + c.color * s1;
            //TODO 还需要完成uv的插值

            return new Fragment(x, y, z, cc);
        }

        private float GetTriangleArea(Vector4 a, Vector4 b, Vector4 c)
        {
            return Mathf.Abs(Vector4.Cross(a - b, a - c).z);
        }
    }
}
