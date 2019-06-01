using System;

namespace SoftwareRenderer
{
    class FragmentShader
    {
        public Fragment Do(Fragment fragment)
        {
            Fragment fg = new Fragment();

            fg.x = fragment.x;
            fg.y = fragment.y;
            fg.color = fragment.color;
            fg.depth = fragment.depth;
            fg.uv = fragment.uv;

            return fg;
        }
    }
}
