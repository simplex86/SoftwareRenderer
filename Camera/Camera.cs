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

        private Vector4 _position = Vector4.zero;
        private Vector4 _direction = Vector4.forward;
        private Vector4 _up = Vector4.up;
        private float _fov = 90.0f;
        private float _near = 0.1f;
        private float _far = 100;
        private RenderType _renderType = RenderType.WIREFRAME;
        private bool _dirty = false;
        private CanvasBuffer _gbuffer = new CanvasBuffer(Screen.WIDTH, Screen.HEIGHT);
        private Matrix4x4 _worldToCameraMatrix;
        private Matrix4x4 _projectionMatrix;
        private VertexShader _vertexShader = new VertexShader();
        private Rasterizer _raster = new WireframeBresenhamRasterizer();
        private FragmentShader _fragmentShader = new FragmentShader();
        //private float[, ] _zbuffer = new float[Screen.WIDTH, Screen.HEIGHT];
        private FrameBuffer _frameBuffer = new FrameBuffer(Screen.WIDTH, Screen.HEIGHT);

        public Camera()
        {
            aspect = Screen.WIDTH / (float)Screen.HEIGHT;
            _dirty = true;
        }

        public void LookAt(Vector4 target, Vector4 up)
        {
            _direction = target - position;
            _up = up;
            _dirty = true;
        }

        public void Render(Graphics grap, List<Mesh> meshes)
        {
            _gbuffer.foreground.Clear(Color4.white);

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
            _gbuffer.background.Flush(grap);
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

        public Vector4 direction
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
        public RenderType renderType 
        { 
            set
            {
                if (_renderType != value)
                {
                    _renderType = value;

                    if (_renderType == RenderType.WIREFRAME)
                    {
                        _raster = new WireframeBresenhamRasterizer();
                    }
                    else if (_renderType == RenderType.COLOR)
                    {
                        _raster = new TriangleStandardRasterizer();
                    }
                }
            }
            get { return _renderType; }
        }

        public event Action<Canvas> OnPostRender;

        private void BuildMatrix()
        {
            _dirty = false;
            _worldToCameraMatrix = GetCameraMatrix();
            _projectionMatrix = GetPerspectiveMatrix();
        }

        private Matrix4x4 GetCameraMatrix()
        {
            Vector4 cz = Vector4.Normalize(direction);
            Vector4 cx = Vector4.Normalize(Vector4.Cross(_up, cz));
            Vector4 cy = Vector4.Cross(cz, cx);

            float tx = -Vector4.Dot(position, cx);
            float ty = -Vector4.Dot(position, cy);
            float tz = -Vector4.Dot(position, cz);

            Matrix4x4 matrix = new Matrix4x4(new[]{ cx.x, cy.x, cz.x, 0.0f,
                                                    cx.y, cy.y, cz.y, 0.0f,
                                                    cx.z, cy.z, cz.z, 0.0f,
                                                    tx,   ty,   tz,   1.0f,
            });

            return matrix;
        }

        private Matrix4x4 GetPerspectiveMatrix()
        {
            Matrix4x4 m = Matrix4x4.zero;
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

            Matrix4x4 mvp = mesh.modelToWorldMatrix * _worldToCameraMatrix * _projectionMatrix;
            foreach (Triangle t in mesh.triangles)
            {
                Vertex a = GetVertex(mesh, t.a.vertex, t.a.color, t.a.uv);
                Vertex b = GetVertex(mesh, t.b.vertex, t.b.color, t.b.uv);
                Vertex c = GetVertex(mesh, t.c.vertex, t.c.color, t.c.uv);

                //背面剔除
                if (CullBackface(a.position, b.position, c.position, mesh.modelToWorldMatrix))
                    continue;

                //执行vertexShader
                a = _vertexShader.Do(a, mvp);
                b = _vertexShader.Do(b, mvp);
                c = _vertexShader.Do(c, mvp);

                //裁剪
                Clip(a, b, c);

                //透视除法
                Vector4 clip1 = a.position;
                Vector4 clip2 = b.position;
                Vector4 clip3 = c.position;
                clip1.DivW();
                clip2.DivW();
                clip3.DivW();

                //屏幕映射
                float w = Screen.WIDTH * 0.5f;
                float h = Screen.HEIGHT * 0.5f;
                a.position = new Vector4(w * clip1.x + w, h - h * clip1.y, clip1.z);
                b.position = new Vector4(w * clip2.x + w, h - h * clip2.y, clip2.z);
                c.position = new Vector4(w * clip3.x + w, h - h * clip3.y, clip3.z);

                //光栅化
                List<Fragment> fragments = _raster.Do(a, b, c);

                //执行fragmentShader并修改frameBuffer
                foreach (Fragment fragment in fragments)
                {
                    Fragment fg = _fragmentShader.Do(fragment);

                    if (fg.depth < _frameBuffer.zbuffer[fg.x, fg.y])
                    {
                        _frameBuffer.zbuffer[fg.x, fg.y] = fg.depth;
                        _frameBuffer.cbuffer[fg.x, fg.y] = fg.color;
                    }
                }

                //渲染到屏幕
                foreach (Fragment fragment in fragments)
                {
                    int    x     = fragment.x;
                    int    y     = fragment.y;
                    float  z     = fragment.depth;
                    Color4 color = (_renderType == RenderType.WIREFRAME) ? Color4.black : 
                                                                           _frameBuffer.cbuffer[x, y];

                    _gbuffer.foreground.DrawPoint(new Vector4(x, y, z, 0), color);
                }

                _frameBuffer.Clear();
            }
        }

        private Vertex GetVertex(Mesh mesh, int vertex, int color, int uv)
        {
            Vertex v = new Vertex();
            v.position = mesh.vertics[vertex];
            v.color = mesh.colors[color];

            if (mesh.uvs.Count > 0)
            {
                v.uv = mesh.uvs[uv];
            }

            return v;
        }

        private bool CullBackface(Vector4 a, Vector4 b, Vector4 c, Matrix4x4 modelToWorldMatrix)
        {
            a *= modelToWorldMatrix;
            b *= modelToWorldMatrix;
            c *= modelToWorldMatrix;

            Vector4 d = _direction;
            Vector4 n = Vector4.Cross(b - a, c - a);

            return (n.z >= 0.0f) || (Vector4.Dot(n, d) >= 0.0f);
        }

        private void Clip(Vertex a, Vertex b, Vertex c)
        {
            //TODO 未实现
        }
    }
}
