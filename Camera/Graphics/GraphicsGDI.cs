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

        public override void DrawTriangle(Vector a, Vector b, Vector c, Color color)
        {
            Pen pen = new Pen(color);
            _graphics.DrawLine(pen, a.x, a.y, b.x, b.y);
            _graphics.DrawLine(pen, b.x, b.y, c.x, c.y);
            _graphics.DrawLine(pen, c.x, c.y, a.x, a.y);
        }

        public override void DrawString(string text, Font font, Brush brush, float x = 0, float y = 0)
        {
            _graphics.DrawString(text, font, brush, x, y);
        }

        public override void Draw(Graphics graphics, float x = 0, float y = 0)
        {
            graphics.DrawImage(_bitmap, x, y);
        }
    }
}
