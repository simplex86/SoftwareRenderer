using System.Drawing;

namespace SoftwareRenderer
{
    class GraphicsGDI : GraphicsDevice
    {
        public GraphicsGDI(int width, int height)
            :base(width, height)
        {
            
        }

        public override void DrawPoint(Vector p, Color color)
        {
            SetPixel(p.x, p.y, color);
        }

        public override void DrawLine(Vector a, Vector b, Color color)
        {
            _graphics.DrawLine(new Pen(color), a.x, a.y, b.x, b.y);
        }
    }
}
