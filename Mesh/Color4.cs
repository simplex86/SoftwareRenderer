using System.Drawing;

namespace SoftwareRenderer
{
    struct Color4
    {
        private float _r;
        private float _g;
        private float _b;
        private float _a;

        public Color4(float r, float g, float b, float a = 1)
            : this()
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color4(Color c)
            : this()
        {
            r = c.R / 255f;
            g = c.G / 255f;
            b = c.B / 255f;
            a = c.A / 255f;
        }

        public float r
        {
            set { _r = Mathf.Clamp01(value); }
            get { return _r; }
        }

        public float g
        {
            set { _g = Mathf.Clamp01(value); }
            get { return _g; }
        }

        public float b
        {
            set { _b = Mathf.Clamp01(value); }
            get { return _b; }
        }

        public float a
        {
            set { _a = Mathf.Clamp01(value); }
            get { return _a; }
        }

        public static Color4 Lerp(Color4 ca, Color4 cb, float t)
        {
            float r = Mathf.Lerp(ca.r, cb.r, t);
            float g = Mathf.Lerp(ca.g, cb.g, t);
            float b = Mathf.Lerp(ca.b, cb.b, t);
            float a = Mathf.Lerp(ca.a, cb.a, t);

            return new Color4(r, g, b, a);
        }

        public static implicit operator Color(Color4 c)
        {
            int r = (int)(c.r * 255);
            int g = (int)(c.g * 255);
            int b = (int)(c.b * 255);
            int a = (int)(c.a * 255);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
