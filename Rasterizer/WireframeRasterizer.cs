using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class WireframeRasterizer : Rasterizer
    {
        public override void Do(Vertex a, Vertex b, Vertex c)
        {
            fragments.Clear();

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
                fragments.Add(new Fragment(x1, y1));
            }
            else if (x1 == x2)
            {
                int i = (y1 <= y2) ? 1 : -1;
                for (int y = y1; y != y2; y += i)
                {
                    fragments.Add(new Fragment(x1, y));
                }

                fragments.Add(new Fragment(x2, y2));
            }
            else if (y1 == y2)
            {
                int i = (x1 <= x2) ? 1 : -1;
                for (int x = x1; x != x2; x += i)
                {
                    fragments.Add(new Fragment(x, y1));
                }

                fragments.Add(new Fragment(x2, y2));
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
                        fragments.Add(new Fragment(x, y));

                        r += dy;
                        if (r >= dx)
                        {
                            r -= dx;
                            y += (y2 >= y1) ? 1 : -1;

                            fragments.Add(new Fragment(x, y));
                        }
                    }

                    fragments.Add(new Fragment(x2, y2));
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
                        fragments.Add(new Fragment(x, y));

                        r += dx;
                        if (r >= dy)
                        {
                            r -= dy;
                            x += (x2 >= x1) ? 1 : -1;

                            fragments.Add(new Fragment(x, y));
                        }
                    }

                    fragments.Add(new Fragment(x2, y2));
                }
            }
        }
    }
}
