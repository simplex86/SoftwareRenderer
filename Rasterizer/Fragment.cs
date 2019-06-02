using System.Drawing;

namespace SoftwareRenderer
{
    class Fragment
    {
        public int x;
        public int y;
        public float depth;
        public Color color;
        public TexCoord uv;

        public Fragment(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Fragment(int x, int y, float depth)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
        }

        public Fragment(int x, int y, float depth, Color color)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.color = color;
        }

        public Fragment(int x, int y, float depth, Color color, TexCoord uv)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.color = color;
            this.uv = uv;
        }

        public Fragment(Fragment fragment)
        {
            this.x = fragment.x;
            this.y = fragment.y;
            this.color = fragment.color;
            this.depth = fragment.depth;
            this.uv = fragment.uv;
        }
    }
}
