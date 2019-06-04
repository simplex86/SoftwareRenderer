using System;

namespace SoftwareRenderer
{
    class FrameBuffer
    {
        private float[,] _zbuffer = null;

        private Color4[,] _cbuffer = null;

        public FrameBuffer(int width, int height)
        {
            _zbuffer = new float[width, height];
            _cbuffer = new Color4[width, height];
            
            ResetZBuffer();
        }

        public float[,] zbuffer
        {
            get { return _zbuffer; }
        }

        public Color4[,] cbuffer
        {
            get { return _cbuffer; }
        }

        public void Clear()
        {
            ResetZBuffer();
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
    }
}
