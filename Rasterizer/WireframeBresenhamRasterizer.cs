using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class WireframeBresenhamRasterizer : Rasterizer
    {
        public override List<Fragment> Do(Vertex a, Vertex b, Vertex c)
        {
            _fragments.Clear();

            ScanLine(a, b);
            ScanLine(b, c);
            ScanLine(c, a);

            return _fragments;
        }

        private void ScanLine(Vertex a, Vertex b)
        {
            Vector pa = a.position;
            Vector pb = b.position;

            int x1 = (int)pa.x;
            int y1 = (int)pa.y;
            int x2 = (int)pb.x;
            int y2 = (int)pb.y;

            Fragment fragment = null;

            if (x1 == x2 && y1 == y2)
            {
                fragment = new Fragment(x1, y1);
                _fragments.Add(fragment);
            }
            else if (x1 == x2)
            {
                int i = (y1 <= y2) ? 1 : -1;
                for (int y = y1; y != y2; y += i)
                {
                    fragment = new Fragment(x1, y);
                    _fragments.Add(fragment);
                }

                fragment = new Fragment(x2, y2);
                _fragments.Add(fragment);
            }
            else if (y1 == y2)
            {
                int i = (x1 <= x2) ? 1 : -1;
                for (int x = x1; x != x2; x += i)
                {
                    fragment = new Fragment(x, y1);
                    _fragments.Add(fragment);
                }

                fragment = new Fragment(x2, y2);
                _fragments.Add(fragment);
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
                        fragment = new Fragment(x, y);
                        _fragments.Add(fragment);

                        r += dy;
                        if (r >= dx)
                        {
                            r -= dx;
                            y += (y2 >= y1) ? 1 : -1;

                            fragment = new Fragment(x, y);
                            _fragments.Add(fragment);
                        }
                    }

                    fragment = new Fragment(x2, y2);
                    _fragments.Add(fragment);
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
                        fragment = new Fragment(x, y);
                        _fragments.Add(fragment);

                        r += dx;
                        if (r >= dy)
                        {
                            r -= dy;
                            x += (x2 >= x1) ? 1 : -1;

                            fragment = new Fragment(x, y);
                            _fragments.Add(fragment);
                        }
                    }

                    fragment = new Fragment(x2, y2);
                    _fragments.Add(fragment);
                }
            }
        }
    }
}
