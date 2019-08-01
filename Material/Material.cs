namespace SoftwareRenderer
{
    class Material
    {
        public Material()
        {
            shader = new Shader();
        }

        public Shader shader { get; set; }

        public Texture texture { get; set; }
    }
}
