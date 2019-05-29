using System.Drawing;

namespace SoftwareRenderer
{
    class GraphicsIMP : GraphicsDevice
    {
        public GraphicsIMP(int width, int height)
            :base(width, height)
        {
            
        }

        public override void DrawPoint(Vector p, Color color)
        {
            SetPixel(p.x, p.y, color);
        }

        public override void DrawLine(Vector a, Vector b, Color color)
        {
            DrawLine(a, b, ref color);
        }

        private void DrawLine(Vector a, Vector b, ref Color color)
        {
            int x1 = (int)a.x;
            int y1 = (int)a.y;
            int x2 = (int)b.x;
            int y2 = (int)b.y;

            if (x1 == x2 && y1 == y2)
            {
                SetPixel(x1, y1, color);
            }
            else if (x1 == x2)
            {
                int i = (y1 <= y2) ? 1 : -1;
                for (int y = y1; y != y2; y += i)
                {
                    SetPixel(x1, y, color);
                }
                SetPixel(x2, y2, color);
            }
            else if (y1 == y2)
            {
                int i = (x1 <= x2) ? 1 : -1;
                for (int x = x1; x != x2; x += i)
                {
                    SetPixel(x, y1, color);
                }
                SetPixel(x2, y2, color);
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
                        SetPixel(x, y, color);
                        r += dy;
                        if (r >= dx)
                        {
                            r -= dx;
                            y += (y2 >= y1) ? 1 : -1;
                            SetPixel(x, y, color);
                        }
                    }
                    SetPixel(x2, y2, color);
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
                        SetPixel(x, y, color);
                        r += dx;
                        if (r >= dy)
                        {
                            r -= dy;
                            x += (x2 >= x1) ? 1 : -1;
                            SetPixel(x, y, color);
                        }
                    }
                    SetPixel(x2, y2, color);
                }
            }
        }
    }
}
