using System.Collections.Generic;

namespace SoftwareRenderer
{
    class WireframeRenderer : Renderer
    {
        public WireframeRenderer()
            : base(new WireframeRasterizer())
        {

        }

        public override void RenderMesh(Material material, FrameBuffer buffer)
        {
            foreach (Fragment fragment in fragments)
            {
                buffer.SetColor(fragment.x, fragment.y, Color4.black);
            }
        }

        public override void RenderBatch(Material material, FrameBuffer buffer)
        {
            foreach (Fragment fragment in fragments)
            {
                buffer.SetColor(fragment.x, fragment.y, Color4.black);
            }
        }
    }
}