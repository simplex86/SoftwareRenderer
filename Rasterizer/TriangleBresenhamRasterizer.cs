using System;
using System.Drawing;

namespace SoftwareRenderer
{
    /// <summary>
    /// 不拆分三角形，直接扫描
    /// 
    /// 参考文献：
    /// Bresenham Algorithm - http://www.sunshine2k.de/coding/java/TriangleRasterization/TriangleRasterization.html#algo2
    /// </summary>
    class TriangleBresenhamRasterizer : Rasterizer
    {
        public override void Do(Vector   pa, Vector   pb, Vector   pc,
                                Color    ca, Color    cb, Color    cc,
                                TexCoord ua, TexCoord ub, TexCoord uc)
        {
            //TODO 还没实现
        }
    }
}
