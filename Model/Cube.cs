using System.IO;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Cube : Model
    {
        public Cube()
        {
            List<Vector4> vertics = mesh.vertics;
            vertics.Add(new Vector4(-1,  1, -1));
            vertics.Add(new Vector4( 1,  1, -1));
            vertics.Add(new Vector4( 1, -1, -1));
            vertics.Add(new Vector4(-1, -1, -1));
            vertics.Add(new Vector4(-1,  1,  1));
            vertics.Add(new Vector4( 1,  1,  1));
            vertics.Add(new Vector4( 1, -1,  1));
            vertics.Add(new Vector4(-1, -1,  1));

            List<Color4> colors = mesh.colors;
            colors.Add(Color4.yellow);
            colors.Add(Color4.red);
            colors.Add(Color4.green);
            colors.Add(Color4.blue);

            List<TexCoord> uvs = mesh.uvs;
            uvs.Add(new TexCoord(0.0f, 0.0f));
            uvs.Add(new TexCoord(1.0f, 0.0f));
            uvs.Add(new TexCoord(1.0f, 1.0f));
            uvs.Add(new TexCoord(0.0f, 1.0f));

            Triangle.Index idx0 = new Triangle.Index(0, 0, 0);
            Triangle.Index idx1 = new Triangle.Index(1, 1, 1);
            Triangle.Index idx2 = new Triangle.Index(2, 2, 2);
            Triangle.Index idx3 = new Triangle.Index(3, 3, 3);
            Triangle.Index idx4 = new Triangle.Index(4, 2, 1);
            Triangle.Index idx5 = new Triangle.Index(5, 0, 0);
            Triangle.Index idx6 = new Triangle.Index(6, 3, 3);
            Triangle.Index idx7 = new Triangle.Index(7, 1, 2);

            material.texture = new Texture(@"Res\xxx.jpg");

            //顺时针方向构造三角形
            List<Triangle> triangles = mesh.triangles;
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
    }
}
