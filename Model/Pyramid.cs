using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Pyramid : IModel
    {
        private Mesh _mesh = new Mesh();

        public Pyramid()
        {
            List<Vector> vertics = _mesh.vertics;
            vertics.Add(new Vector(0, 0, -1));
            vertics.Add(new Vector(-1, -1, 1));
            vertics.Add(new Vector(-1, 1, 1));
            vertics.Add(new Vector(1, 1, 1));
            vertics.Add(new Vector(1, -1, 1));

            List<UV> uvs = _mesh.uvs;
            uvs.Add(new UV(0, 0));
            uvs.Add(new UV(1, 0));
            uvs.Add(new UV(1, 1));
            uvs.Add(new UV(0, 1));

            Triangle.Index idx0 = new Triangle.Index(0, 0);
            Triangle.Index idx1 = new Triangle.Index(1, 0);
            Triangle.Index idx2 = new Triangle.Index(2, 0);
            Triangle.Index idx3 = new Triangle.Index(3, 0);
            Triangle.Index idx4 = new Triangle.Index(4, 0);

            List<Triangle> triangles = _mesh.triangles;
            //顺时针方向构造三角形
            triangles.Add(new Triangle(idx0, idx2, idx1));
            triangles.Add(new Triangle(idx0, idx3, idx2));
            triangles.Add(new Triangle(idx0, idx4, idx3));
            triangles.Add(new Triangle(idx0, idx1, idx4));
            triangles.Add(new Triangle(idx1, idx2, idx3));
            triangles.Add(new Triangle(idx3, idx4, idx1));
        }

        public Mesh mesh
        {
            get { return _mesh; }
        }
    }
}
