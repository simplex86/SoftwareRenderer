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
    }
}
