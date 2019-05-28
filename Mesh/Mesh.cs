using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Mesh
    {
        public Vector position { get; set; }
        public Vector rotation { get; set; }
        public Vector scale { get; set; }

        public List<Vector> vertics { get; set; }
        public List<UV> uvs { get; set; }
        public List<Triangle> triangles { get; set; }

        public Mesh()
        {
            position = Vector.zero;
            rotation = Vector.zero;
            scale = Vector.one;

            vertics = new List<Vector>();
            uvs = new List<UV>();
            triangles = new List<Triangle>();
        }

        public void Clear()
        {
            vertics.Clear();
            uvs.Clear();
            triangles.Clear();
        }
    }
}
