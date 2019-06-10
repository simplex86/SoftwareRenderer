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

            sz = Mathf.Inverse(sz);
            ez = Mathf.Inverse(ez);

            float t = Mathf.Eq(0.0f, e - s) ? 1.0f : (x - s) / (e - s);
            float z = Mathf.Lerp(sz, ez, t);

            return Mathf.Inverse(z);
        }

        protected Color4 LerpC(Color4 ca, Color4 cb, float sv, float ev, float tv)
        {
            float t = Mathf.Eq(0.0f, ev - sv) ? 1.0f : (tv - sv) / (ev - sv);
            return Color4.Lerp(ca, cb, t);
        }
    }
}
