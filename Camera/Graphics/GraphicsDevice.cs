using System;
using System.Drawing;

namespace SoftwareRenderer
{
    abstract class GraphicsDevice : IDisposable
    {
        protected Bitmap _bitmap { get; private set; }
        protected readonly Graphics _graphics;

        public GraphicsDevice(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        public abstract void DrawPoint(Vector p, Color color);

        public abstract void DrawLine(Vector a, Vector b, Color color);

        public abstract void DrawTriangle(Vector a, Vector b, Vector c, Color color);

        public abstract void DrawString(string str, Font font, Brush brush, float x = 0, float y = 0);

        public abstract void Draw(Graphics graphics, float x = 0, float y = 0);

        public void Clear(Color color)
        {
            _graphics.Clear(color);
        }

        public void Dispose()
        {
            _bitmap.Dispose();
            _graphics.Dispose();
        }

        protected void SetPixel(int x, int y, Color color)
        {
            _bitmap.SetPixel(x, y, color);
        }

        protected void SetPixel(float x, float y, Color color)
        {
            SetPixel((int)x, (int)y, color);
        }
    }
}
