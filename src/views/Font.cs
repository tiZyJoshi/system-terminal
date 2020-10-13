using System.ComponentModel;
using System.Text;

namespace System.Views
{
    [Serializable]
    public readonly struct Font : IEquatable<Font>, IFormattable
    {
        public static Font None { get; }

        public Color? Color { get; }

        public Decorations Decorations { get; }

        public bool IsNone => this == None;

        public Font(Color color)
            : this(color, Decorations.None)
        {
        }

        public Font(Decorations decorations)
            : this(null, decorations)
        {
        }

        public Font(Color? color, Decorations decorations)
        {
            Color = color;
            Decorations = decorations;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out Color? color, out Decorations decorations)
        {
            color = Color;
            decorations = Decorations;
        }

        public static bool operator ==(Font left, Font right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Font left, Font right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Font other)
        {
            return Color == other.Color && Decorations == other.Decorations;
        }

        public override bool Equals(object? obj)
        {
            return obj is Font f && Equals(f);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Decorations);
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
                .Append("{Color=")
                .Append(Color?.ToString(format, formatProvider) ?? "null")
                .Append(" Decorations=(")
                .Append(Decorations.ToString(format))
                .Append(")}")
                .ToString();
        }
    }
}
