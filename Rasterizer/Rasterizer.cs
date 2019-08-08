using System;
using System.Drawing;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    abstract class Rasterizer
    {
        public List<Fragment> fragments { get; } = new List<Fragment>();

        public abstract void Do(Vertex a, Vertex b, Vertex c);
    }
}
