using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class WireframeRasterizer : Rasterizer
    {
        public override void Do(Vertex a, Vertex b, Vertex c)
        {
            // 获取屏幕尺寸
            float width = a.normal.x;
            float height = a.normal.y;

            // 透视除法和屏幕映射
            Vector4 posA = a.position.DivW();
            Vector4 posB = b.position.DivW();
            Vector4 posC = c.position.DivW();

            float w = width * 0.5f;
            float h = height * 0.5f;

            // 屏幕映射
            a.position = new Vector4(w * posA.x + w, h - h * posA.y, posA.z, a.position.w);
            b.position = new Vector4(w * posB.x + w, h - h * posB.y, posB.z, b.position.w);
            c.position = new Vector4(w * posC.x + w, h - h * posC.y, posC.z, c.position.w);

            ScanLine(a, b);
            ScanLine(b, c);
            ScanLine(c, a);
        }

        private void ScanLine(Vertex a, Vertex b)
        {
            Vector4 pa = a.position;
            Vector4 pb = b.position;

            int x1 = (int)pa.x;
            int y1 = (int)pa.y;
            int x2 = (int)pb.x;
            int y2 = (int)pb.y;

            if (x1 == x2 && y1 == y2)
            {
                fragments.Add(FragmentPool.Alloc(x1, y1, 0, Color4.black, new TexCoord()));

            }
            else if (x1 == x2)
            {
                int i = (y1 <= y2) ? 1 : -1;
                for (int y = y1; y != y2; y += i)
                {
                    fragments.Add(FragmentPool.Alloc(x1, y, 0, Color4.black, new TexCoord()));
                }

                fragments.Add(FragmentPool.Alloc(x2, y2, 0, Color4.black, new TexCoord()));

            }
            else if (y1 == y2)
            {
                int i = (x1 <= x2) ? 1 : -1;
                for (int x = x1; x != x2; x += i)
                {
                    fragments.Add(FragmentPool.Alloc(x, y1, 0, Color4.black, new TexCoord()));
                }

                fragments.Add(FragmentPool.Alloc(x2, y2, 0, Color4.black, new TexCoord()));

            }
            else
            {
                int dx = (x1 < x2) ? x2 - x1 : x1 - x2;
                int dy = (y1 < y2) ? y2 - y1 : y1 - y2;

                int x = 0;
                int y = 0;

                if (dx >= dy)
                {
                    if (x2 < x1)
                    {
                        x = x1;
                        y = y1;
                        x1 = x2;
                        y1 = y2;
                        x2 = x;
                        y2 = y;
                    }

                    int r = 0;

                    for (x = x1, y = y1; x <= x2; x++)
                    {
                        fragments.Add(FragmentPool.Alloc(x, y, 0, Color4.black, new TexCoord()));

                        r += dy;
                        if (r >= dx)
                        {
                            r -= dx;
                            y += (y2 >= y1) ? 1 : -1;

                            fragments.Add(FragmentPool.Alloc(x, y, 0, Color4.black, new TexCoord()));
                        }
                    }

                    fragments.Add(FragmentPool.Alloc(x2, y2, 0, Color4.black, new TexCoord()));
                }
                else
                {
                    if (y2 < y1)
                    {
                        x = x1;
                        y = y1;
                        x1 = x2;
                        y1 = y2;
                        x2 = x;
                        y2 = y;
                    }

                    int r = 0;

                    for (x = x1, y = y1; y <= y2; y++)
                    {
                        fragments.Add(FragmentPool.Alloc(x, y, 0, Color4.black, new TexCoord()));

                        r += dx;
                        if (r >= dy)
                        {
                            r -= dy;
                            x += (x2 >= x1) ? 1 : -1;

                            fragments.Add(FragmentPool.Alloc(x, y, 0, Color4.black, new TexCoord()));
                        }
                    }

                    fragments.Add(FragmentPool.Alloc(x2, y2, 0, Color4.black, new TexCoord()));
                }
            }
        }
    }
}
