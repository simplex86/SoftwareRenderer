namespace SoftwareRenderer
{
    struct TexCoord
    {
        public float u;
        public float v;

        public TexCoord(float u, float v)
            : this()
        {
            this.u = u;
            this.v = v;
        }

        public static TexCoord operator +(TexCoord a, TexCoord b)
        {
            return new TexCoord(a.u + b.u, a.v + b.v);
        }

        public static TexCoord operator*(TexCoord a, float t)
        {
            return new TexCoord(a.u * t, a.v * t);
        }
    }
}
