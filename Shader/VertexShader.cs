using System;

namespace SoftwareRenderer
{
    class VertexShader
    {
        public Vertex Do(Vertex vertex, Matrix4x4 mvp)
        {
            Vertex v = new Vertex();

            v.position = vertex.position * mvp;
            v.color = vertex.color;
            v.uv = vertex.uv;

            return v;
        }
    }
}
