using System;
using System.Drawing;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    abstract class Rasterizer
    {
        protected List<Fragment> _fragments = new List<Fragment>();

        public abstract List<Fragment> Do(Vertex a, Vertex b, Vertex c);

        protected float LerpZ(float s, float e, float x, float sz, float ez)
        {
            if (Mathf.Eq(e, s))
                return sz;

            return Mathf.Lerp(sz, ez, (x - s) / (e - s));
        }
    }
}
