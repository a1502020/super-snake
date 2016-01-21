using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    public class ColorState
    {
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }

        public ColorState(int r, int g, int b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ColorState);
        }

        public bool Equals(ColorState other)
        {
            if ((object)other == null) return false;
            return this.R.Equals(other.R) && this.G.Equals(other.G) && this.B.Equals(other.B);
        }

        public static bool operator ==(ColorState l, ColorState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(ColorState l, ColorState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}", R, G, B);
        }
    }
}
