using System.Collections.Generic;

namespace SoftwareRenderer
{
    class ShadedRenderer : Renderer
    {
        public ShadedRenderer()
            : base(new TriangleRasterizer())
        {
            
        }

        public override void RenderMesh(Material material, FrameBuffer buffer)
        {
            Texture texture = material.texture;
            FragmentShader ps = material.shader.ps;

            if (texture != null)
            {
                texture.BeginSample();
            }

            List<Fragment> usedFragments = new List<Fragment>();
            foreach (Fragment fragment in fragments)
            {
                usedFragments.Add(fragment);
                // 先进行深度测试
                if (fragment.depth >= buffer.GetDepth(fragment.x, fragment.y))
                {
                    continue; // 深度测试失败，跳过
                }

                // 深度测试通过，进行着色
                Fragment fg = ps.Do(fragment, texture);
                usedFragments.Add(fg);

                buffer.SetDepth(fg.x, fg.y, fg.depth);
                buffer.SetColor(fg.x, fg.y, fg.color);
            }

            if (texture != null)
            {
                texture.EndSample();
            }

            // 回收Fragment对象
            foreach (Fragment fragment in usedFragments)
            {
                FragmentPool.Release(fragment);
            }
        }

        public override void RenderBatch(Material material, FrameBuffer buffer)
        {
            Texture texture = material.texture;
            FragmentShader ps = material.shader.ps;

            if (texture != null)
            {
                texture.BeginSample();
            }

            List<Fragment> usedFragments = new List<Fragment>();
            foreach (Fragment fragment in fragments)
            {
                usedFragments.Add(fragment);
                // 先进行深度测试
                if (fragment.depth >= buffer.GetDepth(fragment.x, fragment.y))
                {
                    continue; // 深度测试失败，跳过
                }

                // 深度测试通过，进行着色
                Fragment fg = ps.Do(fragment, texture);
                usedFragments.Add(fg);

                buffer.SetDepth(fg.x, fg.y, fg.depth);
                buffer.SetColor(fg.x, fg.y, fg.color);
            }

            if (texture != null)
            {
                texture.EndSample();
            }

            // 回收Fragment对象
            foreach (Fragment fragment in usedFragments)
            {
                FragmentPool.Release(fragment);
            }
        }
    }
}