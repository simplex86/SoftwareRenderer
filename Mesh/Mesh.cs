using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Mesh
    {
        public List<Vector> vertics { get; set; }
        public List<UV> uvs { get; set; }
        public List<Triangle> triangles { get; set; }

        private Vector _position = Vector.zero;
        private Vector _rotation = Vector.zero;
        private Vector _scale = Vector.one;

        private Matrix _modelToWorldMatrix = Matrix.identity;
        private bool _dirty = false;

        public Mesh()
        {
            vertics = new List<Vector>();
            uvs = new List<UV>();
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

                    Matrix t = new Matrix(new []{
                        1.0f, 0.0f, 0.0f, _position.x,
                        0.0f, 1.0f, 0.0f, _position.y,
                        0.0f, 0.0f, 1.0f, _position.z,
                        0.0f, 0.0f, 0.0f, 1.0f,
                    });
                    Matrix r = Matrix.Rotation(rotation);
                    Matrix s = new Matrix(new[]{
                        _scale.x, 0.0f,     0.0f,     0.0f,
                        0.0f,     _scale.y, 0.0f,     0.0f,
                        0.0f,     0.0f,     _scale.z, 0.0f,
                        0.0f,     0.0f,     0.0f,     1.0f,
                    });

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
