using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class TriangleM : Model
    {
        public TriangleM()
        {
            List<Vector4> vertics = mesh.vertics;
            vertics.Add(new Vector4(0, 1, 0));
            vertics.Add(new Vector4(1, -1, 1));
            vertics.Add(new Vector4(1, -1, -1));

            List<Color4> colors = mesh.colors;
            colors.Add(Color4.red);
            colors.Add(Color4.green);
            colors.Add(Color4.blue);

            Triangle.Index idx0 = new Triangle.Index(0, 0, 0);
            Triangle.Index idx1 = new Triangle.Index(1, 1, 0);
            Triangle.Index idx2 = new Triangle.Index(2, 2, 0);

            List<Triangle> triangles = mesh.triangles;
            //顺时针方向构造三角形
            triangles.Add(new Triangle(idx0, idx1, idx2));
        }
    }
}
