using System;

namespace SoftwareRenderer
{
    abstract class TriangleRasterizer : Rasterizer
    {
        protected void Color(Vertex a, Vertex b, Vertex c)
        {
            Vector4 pa = a.position;
            Vector4 pb = b.position;
            Vector4 pc = c.position;

            float t = 1 / GetTriangleArea(pa, pb, pc);

            foreach (Fragment fragment in _fragments)
            {
                Vector4 pp = new Vector4(fragment.x, fragment.y, fragment.depth);

                float s1 = GetTriangleArea(pp, pa, pb) * t;
                float s2 = GetTriangleArea(pp, pb, pc) * t;
                float s3 = GetTriangleArea(pp, pc, pa) * t;

                fragment.color = a.color * s2 + b.color * s3 + c.color * s1;
            }
        }

        private float GetTriangleArea(Vector4 a, Vector4 b, Vector4 c)
        {
            return Mathf.Abs(Vector4.Cross(a - b, a - c).z);
        }
    }
}
