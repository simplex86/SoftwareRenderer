using System.Collections.Generic;

namespace SoftwareRenderer
{
    abstract class Renderer
    {
        public abstract void RenderMesh(Material material, List<Fragment> fragments, FrameBuffer buffer);
        public Rasterizer rasterizer { get; protected set; }
    }
}