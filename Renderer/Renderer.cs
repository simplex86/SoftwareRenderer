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
            _rasterizer.Do(a, b, c);
        }

        public abstract void RenderMesh(Material material, FrameBuffer buffer);
    }
}