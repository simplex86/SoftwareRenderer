using System;

namespace SoftwareRenderer
{
    class FragmentShader
    {
        public Fragment Do(Fragment fragment)
        {
            Fragment fg = new Fragment(fragment);
            return fg;
        }
    }
}
