using System.Drawing;

namespace SoftwareRenderer
{
    struct Color4
    {
        private byte _r;
        private byte _g;
        private byte _b;
        private byte _a;

        public Color4(byte r, byte g, byte b, byte a = 255)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }

        public Color4(Color c)
        {
            _r = c.R;
            _g = c.G;
            _b = c.B;
            _a = c.A;
        }

        public Color4(Color4 c)
        {
            _r = c.r;
            _g = c.g;
            _b = c.b;
            _a = c.a;
        }

        public byte r
        {
            set { _r = (byte)Mathf.Clamp(value, 0, 255); }
            get { return _r; }
        }

        public byte g
        {
            set { _g = (byte)Mathf.Clamp(value, 0, 255); }
            get { return _g; }
        }

        public byte b
        {
            set { _b = (byte)Mathf.Clamp(value, 0, 255); }
            get { return _b; }
        }

        public byte a
        {
            set { _a = (byte)Mathf.Clamp(value, 0, 255); }
            get { return _a; }
        }

        public static Color4 operator *(Color4 c, float t)
        {
            return new Color4((byte)(c.r * t), 
                              (byte)(c.g * t), 
                              (byte)(c.b * t), 
                              (byte)(c.a * t));
        }

        public static Color4 operator +(Color4 a, Color4 b)
        {
            return new Color4((byte)(a.r + b.r),
                              (byte)(a.g + b.g),
                              (byte)(a.b + b.b),
                              (byte)(a.a + b.a));
        }

        public static Color4 Lerp(Color4 ca, Color4 cb, float t)
        {
            byte r = (byte)Mathf.Lerp(ca.r, cb.r, t);
            byte g = (byte)Mathf.Lerp(ca.g, cb.g, t);
            byte b = (byte)Mathf.Lerp(ca.b, cb.b, t);
            byte a = (byte)Mathf.Lerp(ca.a, cb.a, t);

            return new Color4(r, g, b, a);
        }

        public static implicit operator Color(Color4 c)
        {
            return Color.FromArgb(c.a, c.r, c.g, c.b);
        }

        public static Color4 white
        {
            get { return new Color4(Color.White); }
        }

        public static Color4 black
        {
            get { return new Color4(Color.Black); }
        }

        public static Color4 red
        {
            get { return new Color4(Color.Red); }
        }

        public static Color4 green
        {
            get { return new Color4(Color.Green); }
        }

        public static Color4 blue
        {
            get { return new Color4(Color.Blue); }
        }

        public static Color4 yellow
        {
            get { return new Color4(Color.Yellow); }
        }
    }
}
