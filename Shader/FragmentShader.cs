using System;

namespace SoftwareRenderer
{
    class FragmentShader
    {
        public Fragment Do(Fragment fragment)
        {
            return FragmentPool.Alloc(fragment);
        }

        public Fragment Do(Fragment fragment, Texture texture)
        {
            if (texture == null)
            {
                return Do(fragment);
            }

            Color4 color = texture.Sample(fragment.uv);
            return FragmentPool.Alloc(fragment.x, fragment.y, fragment.depth, color, fragment.uv);
        }
    }
}
