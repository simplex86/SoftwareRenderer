namespace SoftwareRenderer
{
    class Model
    {
        protected Model()
        {

        }

        public Mesh mesh { get; } = new Mesh();

        public Material material { get; } = new Material();
    }
}
