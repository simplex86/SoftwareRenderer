using System;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Triangle
    {
        public class Index
        {
            public int vertex;
            public int uv;

            public Index(int vertex, int uv)
            {
                this.vertex  = vertex;
                this.uv = uv;
            }
        }

        public Index a = null;
        public Index b = null;
        public Index c = null;

        public Triangle(Index a, Index b, Index c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }
}
