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
            List<Fragment> usedFragments = new List<Fragment>();
            foreach (Fragment fragment in fragments)
            {
                usedFragments.Add(fragment);
                buffer.SetColor(fragment.x, fragment.y, Color4.black);
            }

            // 回收Fragment对象
            foreach (Fragment fragment in usedFragments)
            {
                FragmentPool.Release(fragment);
            }
        }

        public override void RenderBatch(Material material, FrameBuffer buffer)
        {
            List<Fragment> usedFragments = new List<Fragment>();
            foreach (Fragment fragment in fragments)
            {
                usedFragments.Add(fragment);
                buffer.SetColor(fragment.x, fragment.y, Color4.black);
            }

            // 回收Fragment对象
            foreach (Fragment fragment in usedFragments)
            {
                FragmentPool.Release(fragment);
            }
        }
    }
}