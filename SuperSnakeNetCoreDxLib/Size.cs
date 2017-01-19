using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Net.Core
{
    public class Size
    {
        public Size()
        {
            Width = 0;
            Height = 0;
        }

        public Size(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Size);
        }

        public bool Equals(Size other)
        {
            if ((object)other == null) return false;
            return this.Width.Equals(other.Width) && this.Height.Equals(other.Height);
        }

        public static bool operator ==(Size l, Size r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Size l, Size r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}x{1}", Width, Height);
        }
    }
}
