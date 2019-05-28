using System;
using System.Drawing;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Camera
    {
        private Vector _position = Vector.zero;
        private Vector _direction = Vector.forward;
        private Vector _up = Vector.up;
        private float _fov = 90.0f;
        private float _near = 0.1f;
        private float _far = 100;
        private bool _dirty = false;
        private CameraBuffer _gbuffer = new CameraBuffer(Screen.WIDTH, Screen.HEIGHT);
        private Vector _offset;
        private Matrix _modelToWorldMatrix;
        private Matrix _worldToCameraMatrix;
        private Matrix _projectionMatrix;

        public event Action<GraphicsDevice> OnPostRender;

        private const float DEG_TO_RAD = (float)Math.PI / 180.0f;//角度转弧度

        public Camera()
        {
            aspect = Screen.WIDTH / (float)Screen.HEIGHT;
            _offset = new Vector(Screen.WIDTH * 0.5f, Screen.HEIGHT * 0.5f, 0);
            _dirty = true;
        }

        public void LookAt(Vector target, Vector up)
        {
            _direction = target - position;
            _up = up;
            _dirty = true;
        }

        public void Render(Graphics grap, List<Mesh> meshes)
        {
            _gbuffer.foreground.Clear(Color.White);

            if (_dirty)
            {
                BuildMatrix();
            }

            foreach (Mesh mesh in meshes)
            {
                DrawMesh(mesh);
            }

            if (OnPostRender != null)
            {
                OnPostRender(_gbuffer.foreground);
            }

            _gbuffer.Swap();
            _gbuffer.background.Draw(grap);
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

        public Vector direction
        {
            set
            {
                _direction = value;
                _dirty = true;
            }
            get { return _direction; }
        }

        public float fov
        {
            set
            {
                _fov = value;
                _dirty = true;
            }
            get { return _fov; }
        }

        public float near
        {
            set
            {
                _near = value;
                _dirty = true;
            }
            get { return _near; }
        }

        public float far
        {
            set
            {
                _far = value;
                _dirty = true;
            }
            get { return _far; }
        }

        public float aspect { get; private set; }

        private void BuildMatrix()
        {
            _modelToWorldMatrix = GetModelMatrix();
            System.Console.WriteLine("model to world mat: \n" + _modelToWorldMatrix.ToString());
            _worldToCameraMatrix = GetCameraMatrix();
            System.Console.WriteLine("world to camera mat: \n" + _worldToCameraMatrix.ToString());
            _projectionMatrix = GetPerspectiveMatrix();
            System.Console.WriteLine("camera to clip mat: \n" + _projectionMatrix.ToString());

            _dirty = false;
        }

        Matrix GetModelMatrix()
        {
            Matrix matrix = Matrix.identity; ;
            return matrix;
        }

        Matrix GetCameraMatrix()
        {
            Vector cz = direction.Normalize();
            Vector cx = Vector.Cross(_up, cz).Normalize();
            Vector cy = Vector.Cross(cz, cx);

            float tx = -Vector.Dot(position, cx);
            float ty = -Vector.Dot(position, cy);
            float tz = -Vector.Dot(position, cz);

            Matrix matrix = new Matrix(new[]{ cx.x, cy.x, cz.x, 0.0f,
                                              cx.y, cy.y, cz.y, 0.0f,
                                              cx.z, cy.z, cz.z, 0.0f,
                                              tx,   ty,   tz,   1.0f,
            });

            return matrix;
        }

        Matrix GetPerspectiveMatrix()
        {
            Matrix m = Matrix.zero;
            float fax = 1.0f / (float)Math.Tan(DEG_TO_RAD * _fov * 0.5f);

            m[0, 0] = fax / aspect;
            m[1, 1] = fax;
            m[2, 2] = far / (far - near);
            m[3, 2] = -(near * far) / (far - near);
            m[2, 3] = 1.0f;

            return m;
        }

        private void DrawMesh(Mesh mesh)
        {
            if (mesh == null)
                return;

            List<Vector> vertics = mesh.vertics;
            List<Triangle> triangles = mesh.triangles;
            List<UV> uvs = mesh.uvs;

            for (int i = 0; i < triangles.Count; i++)
            {
                Triangle t = triangles[i];

                Vertex a = new Vertex();
                a.position = vertics[t.a.vertex];
                a.uv = uvs[t.a.uv];

                Vertex b = new Vertex();
                b.position = vertics[t.b.vertex];
                b.uv = uvs[t.b.uv];

                Vertex c = new Vertex();
                c.position = vertics[t.c.vertex];
                c.uv = uvs[t.c.uv];

                DrawPrimitive(a, b, c);
            }
        }

        private void DrawPrimitive(Vertex v1, Vertex v2, Vertex v3)
        {
            Matrix mvp = _modelToWorldMatrix * _worldToCameraMatrix * _projectionMatrix;

            Vector clip1 = v1.position * mvp;
            Vector clip2 = v2.position * mvp;
            Vector clip3 = v3.position * mvp;

            clip1.Scale(1.0f / clip1.w);
            clip1.w = 1.0f;
            clip2.Scale(1.0f / clip2.w);
            clip2.w = 1.0f;
            clip3.Scale(1.0f / clip3.w);
            clip3.w = 1.0f;

            float w = Screen.WIDTH * 0.5f;
            float h = Screen.HEIGHT * 0.5f;

            Vector scr1 = new Vector(w * clip1.x + w, h - h * clip1.y, clip1.z);
            Vector scr2 = new Vector(w * clip2.x + w, h - h * clip2.y, clip2.z);
            Vector scr3 = new Vector(w * clip3.x + w, h - h * clip3.y, clip3.z);

            _gbuffer.foreground.DrawTriangle(scr1, scr2, scr3, Color.DarkBlue);
        }
    }
}
