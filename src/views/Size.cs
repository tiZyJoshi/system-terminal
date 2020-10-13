using System.ComponentModel;
using System.Text;

namespace System.Views
{
    [Serializable]
    public readonly struct Size : IEquatable<Size>, IFormattable
    {
        const int Invalid = -1;

        public static Size Empty { get; }

        public int? Width => _width != Invalid ? _width : null;

        public int? Height => _height != Invalid ? _height : null;

        public bool IsEmpty => this == Empty;

        public bool IsUniform => Width == Height;

        readonly int _width;

        readonly int _height;

        public Size(int? width, int? height)
        {
            // We store the components in this somewhat strange way because we want to ensure that
            // a default-initialized instance is empty rather than invalid.

            _width = (width < 0 ? throw new ArgumentOutOfRangeException(nameof(width)) : width) ?? Invalid;
            _height = (height < 0 ? throw new ArgumentOutOfRangeException(nameof(height)) : height) ?? Invalid;

            // Normalize empty sizes so comparisons to Empty work as expected.
            if (width == 0 || height == 0)
                this = Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out int? width, out int? height)
        {
            width = Width;
            height = Height;
        }

        public static bool operator ==(Size left, Size right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !left.Equals(right);
        }

        public static Size operator *(Size left, int right)
        {
            return new(left.Width * right, left.Height * right);
        }

        public static Size operator /(Size left, int right)
        {
            return new(left.Width / right, left.Height / right);
        }

        public bool Equals(Size other)
        {
            return _width == other._width && _height == other._height;
        }

        public override bool Equals(object? obj)
        {
            return obj is Size s && Equals(s);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_width, _height);
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
                .Append(Width?.ToString(format, formatProvider) ?? "null")
                .Append(" Height=")
                .Append(Height?.ToString(format, formatProvider) ?? "null")
                .Append('}')
                .ToString();
        }
    }
}
