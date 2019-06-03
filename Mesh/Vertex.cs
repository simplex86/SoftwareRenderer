using System.Drawing;

namespace SoftwareRenderer
{
    struct Vertex
    {
        public Vector4 position { get; set; }
        public Color4 color { get; set; }
        public TexCoord uv { get; set; }
    }
}
