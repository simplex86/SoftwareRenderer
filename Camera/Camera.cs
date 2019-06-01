using System;
using System.Drawing;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Camera
    {
        public enum RenderType
        {
            WIREFRAME,
            COLOR,
        }

        private Vector _position = Vector.zero;
        private Vector _direction = Vector.forward;
        private Vector _up = Vector.up;
        private float _fov = 90.0f;
        private float _near = 0.1f;
        private float _far = 100;
        private bool _dirty = false;
        private CameraBuffer _gbuffer = new CameraBuffer(Screen.WIDTH, Screen.HEIGHT);
        private Matrix _worldToCameraMatrix;
        private Matrix _projectionMatrix;
        private VertexShader _vertexShader = new VertexShader();
        private Rasterizer _raster = new TriangleStandardRasterizer();
        private FragmentShader _fragmentShader = new FragmentShader();
        private float[] _zbuffer = new float[Screen.WIDTH * Screen.HEIGHT];

        public Camera()
        {
            aspect = Screen.WIDTH / (float)Screen.HEIGHT;
            _dirty = true;

            ClearZBuffer();
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
        public RenderType renderType { get; set; }

        public event Action<GraphicsDevice> OnPostRender;

        private void BuildMatrix()
        {
            _dirty = false;
            _worldToCameraMatrix = GetCameraMatrix();
            _projectionMatrix = GetPerspectiveMatrix();
        }

        private Matrix GetCameraMatrix()
        {
            Vector cz = Vector.Normalize(direction);
            Vector cx = Vector.Normalize(Vector.Cross(_up, cz));
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

        private Matrix GetPerspectiveMatrix()
        {
            Matrix m = Matrix.zero;
            float fax = 1.0f / Mathf.Tan(Mathf.Deg2Rad(_fov * 0.5f));

            m[0, 0] = fax / aspect;
            m[1, 1] = fax;
            m[2, 2] = far / (far - near);
            m[3, 2] = (near * far) / (near - far);
            m[2, 3] = 1.0f;

            return m;
        }

        private void DrawMesh(Mesh mesh)
        {
            if (mesh == null)
                return;

            Matrix mvp = mesh.modelToWorldMatrix * _worldToCameraMatrix * _projectionMatrix;
            foreach (Triangle t in mesh.triangles)
            {
                if (CullBackface(mesh, t))
                    continue;

                DrawPrimitive(mesh, t, mvp);
            }
        }

        private bool CullBackface(Mesh mesh, Triangle triangle)
        {
            Vector a = mesh.vertics[triangle.a.vertex] * mesh.modelToWorldMatrix;
            Vector b = mesh.vertics[triangle.b.vertex] * mesh.modelToWorldMatrix;
            Vector c = mesh.vertics[triangle.c.vertex] * mesh.modelToWorldMatrix;

            Vector d = _direction;
            Vector n = Vector.Cross(b - a, c - a);

            return Vector.Dot(n, d) >= 0.0f;
        }

        private void DrawPrimitive(Mesh mesh, Triangle triangle, Matrix mvp)
        {
            Vertex a = GetVertex(mesh, triangle.a.vertex, triangle.a.uv);
            Vertex b = GetVertex(mesh, triangle.b.vertex, triangle.b.uv);
            Vertex c = GetVertex(mesh, triangle.c.vertex, triangle.c.uv);
            
            a = _vertexShader.Do(a, mvp);
            b = _vertexShader.Do(b, mvp);
            c = _vertexShader.Do(c, mvp);

            Vector clip1 = a.position;
            Vector clip2 = b.position;
            Vector clip3 = c.position;
            //透视除法
            clip1.DivW();
            clip2.DivW();
            clip3.DivW();
            //屏幕映射
            float w = Screen.WIDTH  * 0.5f;
            float h = Screen.HEIGHT * 0.5f;
            a.position = new Vector(w * clip1.x + w, h - h * clip1.y, clip1.z);
            b.position = new Vector(w * clip2.x + w, h - h * clip2.y, clip2.z);
            c.position = new Vector(w * clip3.x + w, h - h * clip3.y, clip3.z);

            if (renderType == RenderType.WIREFRAME)
            {
                _gbuffer.foreground.DrawLine(a.position, b.position, Color.Black);
                _gbuffer.foreground.DrawLine(b.position, c.position, Color.Black);
                _gbuffer.foreground.DrawLine(c.position, a.position, Color.Black);
            }
            else if (renderType == RenderType.COLOR)
            {
                _raster.Do(a, b, c);

                WriteZBuffer(_raster.fragments);

                foreach (Fragment fragment in _raster.fragments)
                {
                    Fragment fg = _fragmentShader.Do(fragment);
                    if (ZTest(fg.x, fg.y, fg.depth))
                    {
                        _gbuffer.foreground.DrawPoint(new Vector(fg.x, fg.y, fg.depth, 0), Color.DarkBlue);
                    }
                }

                ClearZBuffer();
            }
        }

        public Vertex GetVertex(Mesh mesh, int vertex, int uv)
        {
            Vertex v = new Vertex();
            v.position = mesh.vertics[vertex];
            v.uv = mesh.uvs[uv];

            return v;
        }

        private void WriteZBuffer(List<Fragment> fragments)
        {
            foreach (Fragment fg in fragments)
            {
                int idx = fg.y * Screen.WIDTH + fg.x;

                if (_zbuffer[idx] < fg.depth)
                {
                    _zbuffer[idx] = fg.depth;
                }
            }
        }

        private bool ZTest(int x, int y, float z)
        {
            int idx = y * Screen.WIDTH + x;
            return z >= _zbuffer[idx];
        }

        private void ClearZBuffer()
        {
            for (int i = 0; i < _zbuffer.Length; i++)
            {
                _zbuffer[i] = float.MinValue;
            }
        }
    }
}
