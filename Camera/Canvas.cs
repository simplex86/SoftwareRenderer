using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SoftwareRenderer
{
    class Canvas : IDisposable
    {
        protected Bitmap _bitmap { get; private set; }
        protected readonly Graphics _graphics;

        public Canvas(int width, int height)
        {
            _bitmap = new Bitmap(width, height);
            _graphics = Graphics.FromImage(_bitmap);
        }

        public void DrawPoint(Vector4 p, Color4 color)
        {
            _bitmap.SetPixel((int)p.x, (int)p.y, color);
        }

        public void RenderByFrameBuffer(FrameBuffer frameBuffer)
        {
            BitmapData bits = _bitmap.LockBits(new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                                               ImageLockMode.WriteOnly,
                                               _bitmap.PixelFormat);
            Marshal.Copy(frameBuffer.cbuffer, 0, bits.Scan0, frameBuffer.cbuffer.Length);
            _bitmap.UnlockBits(bits);
        }

        public void DrawString(string text, Font font, Brush brush, float x = 0, float y = 0)
        {
            _graphics.DrawString(text, font, brush, x, y);
        }

        public void Flush(Graphics grap, float x = 0, float y = 0)
        {
            grap.DrawImage(_bitmap, x, y);
        }

        public void Clear(Color4 color)
        {
            _graphics.Clear(color);
        }

        public void Dispose()
        {
            _bitmap.Dispose();
            _graphics.Dispose();
        }
    }
}
