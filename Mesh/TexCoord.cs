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

        public static TexCoord Lerp(TexCoord a, TexCoord b, float t)
        {
            float u = Mathf.Lerp(a.u, b.u, t);
            float v = Mathf.Lerp(a.v, b.v, t);

            return new TexCoord(u, v);
        }
    }
}
