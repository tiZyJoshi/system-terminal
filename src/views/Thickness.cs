using System.ComponentModel;
using System.Text;

namespace System.Views
{
    [Serializable]
    public readonly struct Thickness : IEquatable<Thickness>, IFormattable
    {
        public static Thickness Zero { get; }

        public int Left { get; }

        public int Top { get; }

        public int Right { get; }

        public int Bottom { get; }

        public bool IsZero => this == Zero;

        public bool IsUniform => Left == Top && Left == Right && Left == Bottom;

        public Thickness(int value)
            : this(value, value, value, value)
        {
        }

        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left >= 0 ? left : throw new ArgumentOutOfRangeException(nameof(left));
            Top = top >= 0 ? top : throw new ArgumentOutOfRangeException(nameof(top));
            Right = right >= 0 ? right : throw new ArgumentOutOfRangeException(nameof(right));
            Bottom = bottom >= 0 ? bottom : throw new ArgumentOutOfRangeException(nameof(bottom));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out int left, out int top, out int right, out int bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }

        public static bool operator ==(Thickness left, Thickness right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Thickness left, Thickness right)
        {
            return !left.Equals(right);
        }

        public static Thickness operator +(Thickness operand)
        {
            return operand;
        }

        public static Thickness operator ++(Thickness operand)
        {
            return operand + 1;
        }

        public static Thickness operator --(Thickness operand)
        {
            return operand - 1;
        }

        public static Thickness operator +(Thickness left, int right)
        {
            return new(left.Left + right, left.Top + right, left.Right + right, left.Bottom + right);
        }

        public static Thickness operator -(Thickness left, int right)
        {
            return new(left.Left - right, left.Top - right, left.Right - right, left.Bottom - right);
        }

        public static Thickness operator *(Thickness left, int right)
        {
            return new(left.Left * right, left.Top * right, left.Right * right, left.Bottom * right);
        }

        public static Thickness operator /(Thickness left, int right)
        {
            return new(left.Left / right, left.Top / right, left.Right / right, left.Bottom / right);
        }

        public static Thickness operator %(Thickness left, int right)
        {
            return new(left.Left % right, left.Top % right, left.Right % right, left.Bottom % right);
        }

        public bool Equals(Thickness other)
        {
            return Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
        }

        public override bool Equals(object? obj)
        {
            return obj is Thickness t && Equals(t);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Top, Right, Bottom);
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
                .Append("{Left=")
                .Append(Left.ToString(format, formatProvider))
                .Append(" Top=")
                .Append(Top.ToString(format, formatProvider))
                .Append(" Right=")
                .Append(Right.ToString(format, formatProvider))
                .Append(" Bottom=")
                .Append(Bottom.ToString(format, formatProvider))
                .Append('}')
                .ToString();
        }
    }
}
