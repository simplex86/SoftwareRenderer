using System;

namespace SoftwareRenderer
{
    class FrameBuffer
    {
        private int _width = 0;

        private int _height = 0;

        private float[,] _zbuffer = null;

        private byte[] _cbuffer = null;

        public FrameBuffer(int width, int height)
        {
            _width = width;
            _height = height;

            _zbuffer = new float[width, height];
            _cbuffer = new byte[width * height * 4];

            Clear();
        }

        public void SetDepth(int x, int y, float depth)
        {
            _zbuffer[x, y] = depth;
        }

        public float GetDepth(int x, int y)
        {
            return _zbuffer[x, y];
        }

        public byte[] cbuffer
        {
            get { return _cbuffer; }
        }

        public void SetColor(int x, int y, Color4 c)
        {
            int idx = (y * _width + x) * 4;

            _cbuffer[idx + 0] = c.b;
            _cbuffer[idx + 1] = c.g;
            _cbuffer[idx + 2] = c.r;
            _cbuffer[idx + 3] = c.a;
        }

        public Color4 GetColor(int x, int y)
        {
            int idx = (y * _width + x) * 4;

            byte b = _cbuffer[idx + 0];
            byte g = _cbuffer[idx + 1];
            byte r = _cbuffer[idx + 2];
            byte a = _cbuffer[idx + 3];

            return new Color4(r, g, b, a);
        }

        public void Clear()
        {
            ResetZBuffer();
            ResetCBuffer();
        }

        private void ResetZBuffer()
        {
            int rows = _zbuffer.GetLength(0);
            int cols = _zbuffer.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    _zbuffer[r, c] = float.MaxValue;
                }
            }
        }

        private void ResetCBuffer()
        {
            for (int i=0; i<_cbuffer.Length; i++)
            {
                _cbuffer[i] = 255;
            }
        }
    }
}
