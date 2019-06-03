using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Mesh
    {
        public List<Vector4> vertics { get; private set; }
        public List<Color4> colors { get; private set; }
        public List<TexCoord> uvs { get; private set; }
        public List<Triangle> triangles { get; private set; }

        private Vector4 _position = Vector4.zero;
        private Vector4 _rotation = Vector4.zero;
        private Vector4 _scale = Vector4.one;

        private Matrix4x4 _modelToWorldMatrix = Matrix4x4.identity;
        private bool _dirty = false;

        public Mesh()
        {
            vertics = new List<Vector4>();
            colors = new List<Color4>();
            uvs = new List<TexCoord>();
            triangles = new List<Triangle>();
        }

        public Vector4 position 
        {
            set 
            {
                _position = value;
                _dirty = true;
            }
            get { return _position; }
        }

        public Vector4 rotation 
        {
            set 
            {
                _rotation = value;
                _dirty = true;
            }
            get { return _rotation; }
        }

        public Vector4 scale 
        {
            set 
            {
                _scale = value;
                _dirty = true;
            }
            get { return _scale; }
        }

        public Matrix4x4 modelToWorldMatrix
        { 
            get 
            {
                if (_dirty)
                {
                    _dirty = false;

                    Matrix4x4 t = Matrix4x4.Translation(position);
                    Matrix4x4 r = Matrix4x4.Rotation(rotation);
                    Matrix4x4 s = Matrix4x4.Scale(scale);

                    _modelToWorldMatrix = s * r * t;
                }

                return _modelToWorldMatrix; 
            }
        }

        public void Clear()
        {
            vertics.Clear();
            colors.Clear();
            uvs.Clear();
            triangles.Clear();
        }
    }
}
