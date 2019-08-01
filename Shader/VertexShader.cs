using System;

namespace SoftwareRenderer
{
    class VertexShader
    {
        public Vertex Do(Vertex vertex, Matrix4x4 mvp)
        {
            Vertex v = new Vertex
            {
                position = vertex.position * mvp,
                color = vertex.color,
                uv = vertex.uv
            };

            return v;
        }
    }
}
