using System;
using System.Drawing;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    abstract class Rasterizer
    {
        private List<Fragment> _fragments = new List<Fragment>();

        public abstract void Do(Vector pa, Vector pb, Vector pc,
                                Color  ca, Color  cb, Color  cc,
                                UV     ua, UV     ub, UV     uc);

        public List<Fragment> fragments
        {
            get { return _fragments; }
        }

        protected float LerpZ(float s, float e, float x, float sz, float ez)
        {
            if (Math.Abs(e - s) < float.Epsilon)
                return sz;

            float t = (x - s) / (e - s);
            return sz + (ez - sz) * t;
        }
    }
}
