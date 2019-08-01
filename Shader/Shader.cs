using System;

namespace SoftwareRenderer
{
    class Shader
    {
        public Shader() 
            : this(new VertexShader(), new FragmentShader())
        {

        }

        public Shader(VertexShader vs, FragmentShader ps)
        {
            this.vs = vs;
            this.ps = ps;
        }

        public VertexShader vs
        {
            get;
            private set;
        }

        public FragmentShader ps
        {
            get;
            private set;
        }
    }
}
