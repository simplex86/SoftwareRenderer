using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SoftwareRenderer
{
    class Texture
    {
        private Bitmap _bitmap;
        private BitmapData _bitmapData;

        public Texture(string filename)
        {
            string solutionPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            string filePath = Path.Combine(solutionPath, filename);

            _bitmap = new Bitmap(filePath);
        }

        public void BeginSample()
        {
            if (_bitmap != null)
            {
                _bitmapData = _bitmap.LockBits(new Rectangle(0, 0, width, height),
                                               ImageLockMode.ReadOnly,
                                               _bitmap.PixelFormat);
            }
        }

        public Color4 Sample(TexCoord uv)
        {
            byte r = 255;
            byte g = 255;
            byte b = 255;
            byte a = 255;

            int x = (int)(uv.u * _bitmapData.Width);
            int y = (int)(uv.v * _bitmapData.Height) * _bitmapData.Stride;

            if (_bitmapData.PixelFormat == PixelFormat.Format24bppRgb)
            {
                unsafe
                {
                    int p = y + (x * 3);
                    byte* ptr = (byte*)_bitmapData.Scan0.ToPointer();
                    r = ptr[p + 2];
                    g = ptr[p + 1];
                    b = ptr[p + 0];
                }
            }
            else if (_bitmapData.PixelFormat == PixelFormat.Format32bppArgb)
            {
                unsafe
                {
                    int p = y + (x * 4);
                    byte* ptr = (byte*)_bitmapData.Scan0.ToPointer();
                    r = ptr[p * 4 + 3];
                    g = ptr[p * 4 + 2];
                    b = ptr[p * 4 + 1];
                    a = ptr[p * 4 + 0];
                }
            }

            return new Color4(r, g, b, a);
        }

        public void EndSample()
        {
            if (_bitmapData != null)
            {
                _bitmap.UnlockBits(_bitmapData);
                _bitmapData = null;
            }
        }

        public int width
        {
            get { return _bitmap.Width; }
        }

        public int height
        {
            get { return _bitmap.Height; }
        }
    }
}
