using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Cube : IModel
    {
        private Mesh _mesh = new Mesh();

        public Cube()
        {
            List<Vector> vertics = _mesh.vertics;
            vertics.Add(new Vector(-1, -1, 1));
            vertics.Add(new Vector(1, -1, 1));
            vertics.Add(new Vector(1, 1, 1));
            vertics.Add(new Vector(-1, 1, 1));
            vertics.Add(new Vector(-1, -1, -1));
            vertics.Add(new Vector(1, -1, -1));
            vertics.Add(new Vector(1, 1, -1));
            vertics.Add(new Vector(-1, 1, -1));

            List<UV> uvs = _mesh.uvs;
            uvs.Add(new UV(0, 0));
            uvs.Add(new UV(1, 0));
            uvs.Add(new UV(1, 1));
            uvs.Add(new UV(0, 1));

            Triangle.Index idx0 = new Triangle.Index(0, 0);
            Triangle.Index idx1 = new Triangle.Index(1, 1);
            Triangle.Index idx2 = new Triangle.Index(2, 2);
            Triangle.Index idx3 = new Triangle.Index(3, 3);
            Triangle.Index idx4 = new Triangle.Index(4, 1);
            Triangle.Index idx5 = new Triangle.Index(5, 0);
            Triangle.Index idx6 = new Triangle.Index(6, 3);
            Triangle.Index idx7 = new Triangle.Index(7, 2);
            //顺时针方向构造三角形
            List<Triangle> triangles = _mesh.triangles;
            //前
            triangles.Add(new Triangle(idx0, idx1, idx2));
            triangles.Add(new Triangle(idx2, idx3, idx0));
            //后
            triangles.Add(new Triangle(idx7, idx6, idx5));
            triangles.Add(new Triangle(idx5, idx4, idx7));
            //上
            triangles.Add(new Triangle(idx0, idx4, idx5));
            triangles.Add(new Triangle(idx5, idx1, idx0));
            //右
            triangles.Add(new Triangle(idx1, idx5, idx6));
            triangles.Add(new Triangle(idx6, idx2, idx1));
            //下
            triangles.Add(new Triangle(idx2, idx6, idx7));
            triangles.Add(new Triangle(idx7, idx3, idx2));
            //左
            triangles.Add(new Triangle(idx3, idx7, idx4));
            triangles.Add(new Triangle(idx4, idx0, idx3));
        }

        public Mesh mesh
        {
            get { return _mesh; }
        }
    }
}
