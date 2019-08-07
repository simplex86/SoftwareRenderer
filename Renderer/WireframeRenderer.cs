using System.Collections.Generic;

namespace SoftwareRenderer
{
    class WireframeRenderer : Renderer
    {
        public WireframeRenderer()
        {
            rasterizer = new WireframeRasterizer();
        }

        public override void RenderMesh(Material material, List<Fragment> fragments, FrameBuffer buffer)
        {
            foreach (Fragment fragment in fragments)
            {
                buffer.SetColor(fragment.x, fragment.y, Color4.black);
            }
        }
    }
}