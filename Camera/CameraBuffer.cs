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
            foreground = new GraphicsGDI(width, height);
            background = new GraphicsGDI(width, height);
        }

        public void Swap()
        {
            GraphicsDevice t = foreground;
            foreground = background;
            background = t;
        }
    }
}
