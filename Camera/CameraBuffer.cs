using System;
using System.Drawing;

namespace SoftwareRenderer
{
    class CameraBuffer
    {
        public GraphicsDevice foreground { get; private set; }
        public GraphicsDevice background { get; private set; }

        public CameraBuffer(int width, int height)
        {
            foreground = new GraphicsIMP(width, height);
            background = new GraphicsIMP(width, height);
        }

        public void Swap()
        {
            GraphicsDevice t = foreground;
            foreground = background;
            background = t;
        }
    }
}
