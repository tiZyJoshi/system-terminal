using System.ComponentModel;
using System.Text;

namespace System
{
    [Serializable]
    public readonly struct TerminalSize : IEquatable<TerminalSize>, IFormattable
    {
        public int Width { get; }

        public int Height { get; }

        internal TerminalSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out int width, out int height)
        {
            width = Width;
            height = Height;
        }

        public static bool operator ==(TerminalSize left, TerminalSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TerminalSize left, TerminalSize right)
        {
            return !left.Equals(right);
        }

        public bool Equals(TerminalSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object? obj)
        {
            return obj is TerminalSize s && Equals(s);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public override string ToString()
        {
            return ToString("G", null);
        }

        public string ToString(string? format)
        {
            return ToString(format, null);
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return new StringBuilder()
                .Append("{Width=")
                .Append(Width.ToString(format, formatProvider))
                .Append(" Height=")
                .Append(Height.ToString(format, formatProvider))
                .Append('}')
                .ToString();
        }
    }
}
