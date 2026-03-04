using System.Collections.Generic;

namespace SoftwareRenderer
{
    abstract class Renderer
    {
        protected Rasterizer _rasterizer = null;
        protected List<Fragment> fragments { get { return _rasterizer.fragments; } }

        protected Renderer(Rasterizer rasterizer)
        {
            _rasterizer = rasterizer;
        }

        public void RasterizeMesh(Vertex a, Vertex b, Vertex c)
        {
            fragments.Clear();
            _rasterizer.Do(a, b, c);
        }

        public void RasterizeBatch(List<Vertex> vertices)
        {
            fragments.Clear();
            for (int i = 0; i < vertices.Count; i += 3)
            {
                if (i + 2 < vertices.Count)
                {
                    _rasterizer.Do(vertices[i], vertices[i + 1], vertices[i + 2]);
                }
            }
        }

        public abstract void RenderMesh(Material material, FrameBuffer buffer);
        public abstract void RenderBatch(Material material, FrameBuffer buffer);
    }
}