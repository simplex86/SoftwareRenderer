using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Mesh
    {
        public List<Vector> vertics { get; set; }
        public List<TexCoord> uvs { get; set; }
        public List<Triangle> triangles { get; set; }

        private Vector _position = Vector.zero;
        private Vector _rotation = Vector.zero;
        private Vector _scale = Vector.one;

        private Matrix _modelToWorldMatrix = Matrix.identity;
        private bool _dirty = false;

        public Mesh()
        {
            vertics = new List<Vector>();
            uvs = new List<TexCoord>();
            triangles = new List<Triangle>();
        }

        public Vector position 
        {
            set 
            {
                _position = value;
                _dirty = true;
            }
            get { return _position; }
        }

        public Vector rotation 
        {
            set 
            {
                _rotation = value;
                _dirty = true;
            }
            get { return _rotation; }
        }

        public Vector scale 
        {
            set 
            {
                _scale = value;
                _dirty = true;
            }
            get { return _scale; }
        }

        public Matrix modelToWorldMatrix
        { 
            get 
            {
                if (_dirty)
                {
                    _dirty = false;

                    Matrix t = Matrix.Translation(position);
                    Matrix r = Matrix.Rotation(rotation);
                    Matrix s = Matrix.Scale(scale);

                    _modelToWorldMatrix = s * r * t;
                }

                return _modelToWorldMatrix; 
            }
        }

        public void Clear()
        {
            vertics.Clear();
            uvs.Clear();
            triangles.Clear();
        }
    }
}
