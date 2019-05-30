using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class TriangleM : IModel
    {
        private Mesh _mesh = new Mesh();

        public TriangleM()
        {
            List<Vector> vertics = _mesh.vertics;
            vertics.Add(new Vector(0, 0, 1));
            vertics.Add(new Vector(0, 0.5f, 0));
            vertics.Add(new Vector(0, -0.5f, 0));

            List<UV> uvs = _mesh.uvs;
            uvs.Add(new UV(0, 0));
            uvs.Add(new UV(1, 0));
            uvs.Add(new UV(1, 1));
            uvs.Add(new UV(0, 1));

            Triangle.Index idx0 = new Triangle.Index(0, 0);
            Triangle.Index idx1 = new Triangle.Index(1, 1);
            Triangle.Index idx2 = new Triangle.Index(2, 2);

            List<Triangle> triangles = _mesh.triangles;
            //顺时针方向构造三角形
            triangles.Add(new Triangle(idx0, idx1, idx2));
        }

        public Mesh mesh
        {
            get { return _mesh; }
        }
    }
}
