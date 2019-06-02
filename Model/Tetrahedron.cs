using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Tetrahedron : Model
    {
        public Tetrahedron()
        {
            List<Vector> vertics = mesh.vertics;
            vertics.Add(new Vector( 0,     1,     0));
            vertics.Add(new Vector(-0.5f, -0.5f, -Mathf.Sqrt(3f) * 0.25f));
            vertics.Add(new Vector( 0.5f, -0.5f, -Mathf.Sqrt(3f) * 0.25f));
            vertics.Add(new Vector( 0,    -0.5f,  Mathf.Sqrt(3f) * 0.25f));

            List<TexCoord> uvs = mesh.uvs;
            uvs.Add(new TexCoord(0, 0));
            uvs.Add(new TexCoord(1, 0));
            uvs.Add(new TexCoord(1, 1));
            uvs.Add(new TexCoord(0, 1));

            Triangle.Index idx0 = new Triangle.Index(0, 0);
            Triangle.Index idx1 = new Triangle.Index(1, 1);
            Triangle.Index idx2 = new Triangle.Index(2, 2);
            Triangle.Index idx3 = new Triangle.Index(3, 3);

            List<Triangle> triangles = mesh.triangles;
            //顺时针方向构造三角形
            triangles.Add(new Triangle(idx0, idx2, idx1));
            triangles.Add(new Triangle(idx0, idx3, idx2));
            triangles.Add(new Triangle(idx0, idx1, idx3));
            triangles.Add(new Triangle(idx1, idx2, idx3));
        }
    }
}
