namespace SoftwareRenderer
{
    class Model
    {
        private Mesh _mesh = new Mesh();

        public Mesh mesh 
        {
            get { return _mesh; } 
        }

        protected Model()
        {

        }
    }
}
