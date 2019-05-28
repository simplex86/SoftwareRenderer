using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Square : IModel
    {
        private Mesh _mesh = new Mesh();

        public Square()
        {
            List<Vector> vertics = _mesh.vertics;
            vertics.Add(new Vector(0, -1, -1));
            vertics.Add(new Vector(0, 1, -1));
            vertics.Add(new Vector(0, 1, 1));
            vertics.Add(new Vector(0, -1, 1));

            List<UV> uvs = _mesh.uvs;
            uvs.Add(new UV(0, 0));
            uvs.Add(new UV(1, 0));
            uvs.Add(new UV(1, 1));
            uvs.Add(new UV(0, 1));

            Triangle.Index idx0 = new Triangle.Index(0, 0);
            Triangle.Index idx1 = new Triangle.Index(1, 1);
            Triangle.Index idx2 = new Triangle.Index(2, 2);
            Triangle.Index idx3 = new Triangle.Index(3, 3);

            List<Triangle> triangles = _mesh.triangles;
            //顺时针方向构造三角形
            triangles.Add(new Triangle(idx0, idx1, idx2));
            triangles.Add(new Triangle(idx2, idx3, idx0));
        }

        public Mesh mesh
        {
            get { return _mesh; }
        }
    }
}
