namespace SoftwareRenderer
{
    struct Fragment
    {
        public readonly int x;
        public readonly int y;
        public readonly float depth;
        public readonly Color4 color;
        public readonly TexCoord uv;

        public Fragment(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.depth = 0;
            this.color = Color4.black;
            this.uv = new TexCoord();
        }

        public Fragment(int x, int y, float depth)
            : this(x, y)
        {
            this.depth = depth;
        }

        public Fragment(int x, int y, float depth, Color4 color)
            : this(x, y, depth)
        {
            this.color = color;
        }

        public Fragment(int x, int y, float depth, Color4 color, TexCoord uv)
            : this(x, y, depth, color)
        {
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
