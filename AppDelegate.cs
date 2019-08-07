using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace SoftwareRenderer
{
    class AppDelegate
    {
        private Form _form = null;
        private Camera _camera = null;
        private float _cameraPosForward = -3;
        private float _fps = 30.0f;
        private readonly Font _font = new Font("Courier New", 12);
        private List<Camera.RenderTarget> _renderTargets = new List<Camera.RenderTarget>();
        private float _meshRotationUp = 0;

        private const int WIDTH  = 800;
        private const int HEIGHT = 600;
        
        public AppDelegate()
        {
            _form = new Form();
            _form.Size = new Size(WIDTH, HEIGHT);
            _form.StartPosition = FormStartPosition.CenterScreen;
            _form.KeyDown += OnFormKeyDown;

            _camera = new Camera(WIDTH, HEIGHT);
            _camera.OnPostRender += OnCameraPostRender;

            UpdateWindowTitle();
        }

        public void Run()
        {
            _camera.position = new Vector4(0, 0, _cameraPosForward);
            _camera.fov = 90;
            _camera.LookAt(new Vector4(0.0f, -0.5f, 0.2f), Vector4.up);

            Model model = new Cube();
            _renderTargets.Add(new Camera.RenderTarget(model.mesh, model.material));

            _form.Show();
            Stopwatch stopwatch = new Stopwatch();

            while (!_form.IsDisposed)
            {
                stopwatch.Start();

                using (var grap = _form.CreateGraphics())
                {
                    _camera.Render(grap, _renderTargets);
                }
                Application.DoEvents();

                stopwatch.Stop();
                _fps = 1000.0f / stopwatch.Elapsed.Milliseconds;
                stopwatch.Reset();
            }
        }

        void OnCameraPostRender(Canvas canvas)
        {
            //显示帧率
            canvas.DrawString(string.Format("FPS: {0:F1}", _fps),
                              _font,
                              Brushes.Black);
        }

        void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)//左转
            {
                _meshRotationUp += 0.05f;
                foreach (Camera.RenderTarget target in _renderTargets)
                {
                    Mesh mesh = target.mesh;
                    mesh.rotation = new Vector4(0, _meshRotationUp, 0);
                }
            }
            else if (e.KeyCode == Keys.Right)//右转
            {
                _meshRotationUp -= 0.05f;
                foreach (Camera.RenderTarget target in _renderTargets)
                {
                    Mesh mesh = target.mesh;
                    mesh.rotation = new Vector4(0, _meshRotationUp, 0);
                }
            }
            else if (e.KeyCode == Keys.Up)//拉近
            {
                _cameraPosForward += 0.05f;
                _camera.position = new Vector4(0, 0, _cameraPosForward);
            }
            else if (e.KeyCode == Keys.Down)//推远
            {
                _cameraPosForward -= 0.05f;
                _camera.position = new Vector4(0, 0, _cameraPosForward);
            }

            if (e.KeyCode == Keys.O)//正交相机
            {
                _camera.cameraType = Camera.CameraType.Orthogonal;
            }
            else if (e.KeyCode == Keys.P)//透视相机
            {
                _camera.cameraType = Camera.CameraType.Perspective;
            }

            if (e.KeyCode == Keys.D1)//切换到线框模式
            {
                _camera.renderType = Camera.RenderType.Wireframe;
            }
            else if (e.KeyCode == Keys.D2)//切换到着色模式
            {
                _camera.renderType = Camera.RenderType.Shaded;
            }

            if (e.KeyCode == Keys.D0)//不剔除
            {
                _camera.cullType = Camera.CullType.None;
            }
            else if (e.KeyCode == Keys.D8)//剔除背面
            {
                _camera.cullType = Camera.CullType.Back;
            }
            else if (e.KeyCode == Keys.D9)//剔除正面
            {
                _camera.cullType = Camera.CullType.Front;
            }

            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            string title = "SoftwareRenderer | ";

            if (_camera.cameraType == Camera.CameraType.Orthogonal)
            {
                title += "正交相机 | ";
            }
            else if (_camera.cameraType == Camera.CameraType.Perspective)
            {
                title += "透视相机 | ";
            }

            if (_camera.cullType == Camera.CullType.None)
            {
                title += "不剔除 | ";
            }
            else if (_camera.cullType == Camera.CullType.Front)
            {
                title += "剔除正面 | ";
            }
            else if (_camera.cullType == Camera.CullType.Back)
            {
                title += "剔除背面 | ";
            }

            if (_camera.renderType == Camera.RenderType.Wireframe)
            {
                title += "线框渲染";
            }
            else if (_camera.renderType == Camera.RenderType.Shaded)
            {
                title += "着色渲染";
            }

            _form.Text = title;
        }
    }
}
