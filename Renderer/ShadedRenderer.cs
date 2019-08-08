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

            foreach (Fragment fragment in _fragments)
            {
                Fragment fg = ps.Do(fragment, texture);

                if (fg.depth < buffer.GetDepth(fg.x, fg.y))
                {
                    buffer.SetDepth(fg.x, fg.y, fg.depth);
                    buffer.SetColor(fg.x, fg.y, fg.color);
                }
            }

            if (texture != null)
            {
                texture.EndSample();
            }
        }
    }
}