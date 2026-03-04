using System.Collections.Generic;

namespace SoftwareRenderer
{
    class FragmentPool
    {
        private static readonly Queue<Fragment> _pool = new Queue<Fragment>();
        private static readonly object _lock = new object();

        public static Fragment Alloc(int x, int y, float depth, Color4 color, TexCoord uv)
        {
            lock (_lock)
            {
                if (_pool.Count > 0)
                {
                    Fragment fragment = _pool.Dequeue();
                    fragment.x = x;
                    fragment.y = y;
                    fragment.depth = depth;
                    fragment.color = color;
                    fragment.uv = uv;
                    return fragment;
                }
            }

            return new Fragment(x, y, depth, color, uv);
        }

        public static Fragment Alloc(Fragment fragment)
        {
            return Alloc(fragment.x, fragment.y, fragment.depth, fragment.color, fragment.uv);
        }

        public static void Release(Fragment fragment)
        {
            lock (_lock)
            {
                _pool.Enqueue(fragment);
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                _pool.Clear();
            }
        }
    }
}