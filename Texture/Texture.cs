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
            // 尝试直接使用相对路径
            string filePath = Path.Combine(Environment.CurrentDirectory, filename);
            if (!File.Exists(filePath))
            {
                // 如果相对路径不存在，尝试使用解决方案路径
                string solutionPath = Directory.GetParent(Environment.CurrentDirectory).Parent?.FullName;
                if (solutionPath != null)
                {
                    filePath = Path.Combine(solutionPath, filename);
                }
            }

            if (File.Exists(filePath))
            {
                _bitmap = new Bitmap(filePath);
            }
            else
            {
                // 如果文件不存在，使用默认颜色
                _bitmap = new Bitmap(1, 1);
                using (Graphics g = Graphics.FromImage(_bitmap))
                {
                    g.Clear(Color.Red);
                }
            }
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

            // 优化纹理坐标计算，确保在有效范围内
            float u = Mathf.Clamp01(uv.u);
            float v = Mathf.Clamp01(uv.v);

            // 预计算宽度和高度，避免重复访问属性
            int width = _bitmapData.Width;
            int height = _bitmapData.Height;
            int stride = _bitmapData.Stride;

            // 计算纹理坐标
            int x = (int)(u * width);
            int y = (int)(v * height);

            // 确保坐标在有效范围内
            x = (int)Mathf.Clamp(x, 0, width - 1);
            y = (int)Mathf.Clamp(y, 0, height - 1);

            if (_bitmapData.PixelFormat == PixelFormat.Format24bppRgb)
            {
                unsafe
                {
                    int p = y * stride + (x * 3);
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
                    int p = y * stride + (x * 4);
                    byte* ptr = (byte*)_bitmapData.Scan0.ToPointer();
                    r = ptr[p + 3];
                    g = ptr[p + 2];
                    b = ptr[p + 1];
                    a = ptr[p + 0];
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
