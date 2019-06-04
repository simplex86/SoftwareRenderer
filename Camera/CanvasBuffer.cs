using System;
using System.Drawing;

namespace SoftwareRenderer
{
    class CanvasBuffer
    {
        public Canvas foreground { get; private set; }
        public Canvas background { get; private set; }

        public CanvasBuffer(int width, int height)
        {
            foreground = new Canvas(width, height);
            background = new Canvas(width, height);
        }

        public void Swap()
        {
            Canvas t = foreground;
            foreground = background;
            background = t;
        }
    }
}
