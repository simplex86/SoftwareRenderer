using System.Collections.Generic;
using System.Drawing;

namespace SoftwareRenderer
{
    /// <summary>
    /// 不拆分三角形，直接扫描
    /// 
    /// 参考文献：
    /// Bresenham Algorithm - http://www.sunshine2k.de/coding/java/TriangleRasterization/TriangleRasterization.html#algo2
    /// </summary>
    class TriangleBresenhamRasterizer : TriangleRasterizer
    {
        public override List<Fragment> Do(Vertex a, Vertex b, Vertex c)
        {
            _fragments.Clear();
            //TODO 还没实现
            return _fragments;
        }
    }
}
