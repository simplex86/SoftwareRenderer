using System.Drawing;

namespace SoftwareRenderer
{
    class Vertex
    {
        public Vector position { get; set; }
        public Color color { get; set; }
        public TexCoord uv { get; set; }

        public Vertex()
        {
            position = new Vector();
            color = Color.White;
            uv = new TexCoord(0, 0);
        }

        public static Vertex Lerp(Vertex va, Vertex vb, float factor)
        {
            Vertex v = new Vertex();
            v.position = Vector.Lerp(va.position, vb.position, factor);

            float uu = va.uv.u + (vb.uv.u - va.uv.u) * factor;
            float vv = va.uv.v + (vb.uv.v - va.uv.v) * factor;
            v.uv = new TexCoord(uu, vv);

            float r = va.color.R + (vb.color.R - va.color.R) * factor * 255;
            float g = va.color.G + (vb.color.G - va.color.G) * factor * 255;
            float b = va.color.B + (vb.color.B - va.color.B) * factor * 255;
            float a = va.color.A + (vb.color.A - va.color.A) * factor * 255;
            v.color = Color.FromArgb((int)a, (int)r, (int)g, (int)b);

            return v;
        }
    }
}
