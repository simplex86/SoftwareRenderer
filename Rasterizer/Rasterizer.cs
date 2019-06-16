using System;
using System.Drawing;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    abstract class Rasterizer
    {
        protected List<Fragment> _fragments = new List<Fragment>();

        public abstract List<Fragment> Do(Vertex a, Vertex b, Vertex c);
    }
}
