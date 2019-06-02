using System;
using System.Drawing;

namespace SoftwareRenderer
{
    class CameraBuffer
    {
        public CameraCanvas foreground { get; private set; }
        public CameraCanvas background { get; private set; }

        public CameraBuffer(int width, int height)
        {
            foreground = new CameraCanvas(width, height);
            background = new CameraCanvas(width, height);
        }

        public void Swap()
        {
            CameraCanvas t = foreground;
            foreground = background;
            background = t;
        }
    }
}
