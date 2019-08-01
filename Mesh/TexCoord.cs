namespace SoftwareRenderer
{
    struct TexCoord
    {
        private float _u;
        private float _v;

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

        public float u
        {
            get { return _u; }
            set { _u = Mathf.Clamp01(value); }
        }

        public float v
        {
            get { return _v; }
            set { _v = Mathf.Clamp01(value); }
        }
    }
}
