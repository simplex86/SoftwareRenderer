using System.Collections.Generic;

namespace SoftwareRenderer
{
    interface IRenderer
    {
        void RenderMesh(Material material, List<Fragment> fragments, FrameBuffer buffer);
    }
}