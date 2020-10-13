using System.ComponentModel;
using System.Text;

namespace System.Views
{
    [Serializable]
    public readonly struct Point : IEquatable<Point>, IFormattable
    {
        public static Point Origin { get; }

        public int X { get; }

        public int Y { get; }

        public bool IsOrigin => this == Origin;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        public static Point operator +(Point left, Vector right)
        {
            return new(left.X + right.X, left.Y + right.Y);
        }

        public static Point operator -(Point left, Vector right)
        {
            return new(left.X - right.X, left.Y - right.Y);
        }

        public static Vector operator -(Point left, Point right)
        {
            return new(left.X - right.X, left.Y - right.Y);
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Point p && Equals(p);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
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
                .Append("{X=")
                .Append(X.ToString(format, formatProvider))
                .Append(" Y=")
                .Append(Y.ToString(format, formatProvider))
                .Append('}')
                .ToString();
        }
    }
}
