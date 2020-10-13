using System.ComponentModel;
using System.Text;

namespace System.Views
{
    [Serializable]
    public readonly struct Vector : IEquatable<Vector>, IFormattable
    {
        public static Vector Zero { get; } = new(0, 0);

        public static Vector One { get; } = new(1, 1);

        public static Vector UnitX { get; } = new(1, 0);

        public static Vector UnitY { get; } = new(0, 1);

        public int X { get; }

        public int Y { get; }

        public bool IsZero => this == Zero;

        public Vector(int value)
            : this(value, value)
        {
        }

        public Vector(int x, int y)
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

        public static Vector operator +(Vector operand)
        {
            return operand;
        }

        public static Vector operator -(Vector operand)
        {
            return Zero - operand;
        }

        public static Vector operator +(Vector left, Vector right)
        {
            return new(left.X + right.X, left.Y + right.Y);
        }

        public static Vector operator -(Vector left, Vector right)
        {
            return new(left.X - right.X, left.Y - right.Y);
        }

        public static Vector operator *(Vector left, int right)
        {
            return right * left;
        }

        public static Vector operator *(int left, Vector right)
        {
            return new Vector(left) * right;
        }

        public static Vector operator *(Vector left, Vector right)
        {
            return new(left.X * right.X, left.Y * right.Y);
        }

        public static Vector operator /(Vector left, int right)
        {
            return left / new Vector(right);
        }

        public static Vector operator /(Vector left, Vector right)
        {
            return new(left.X / right.X, left.Y / right.Y);
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Vector other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Vector v && Equals(v);
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

        public static int Dot(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        public static int DistanceSquared(Vector vector1, Vector vector2)
        {
            var diff = vector1 - vector2;

            return Dot(diff, diff);
        }

        public static int LengthSquared(Vector vector)
        {
            return Dot(vector, vector);
        }

        public static Vector Abs(Vector vector)
        {
            return new(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        public static Vector Min(Vector vector1, Vector vector2)
        {
            return new(Math.Min(vector1.X, vector2.X), Math.Min(vector1.Y, vector2.Y));
        }

        public static Vector Max(Vector vector1, Vector vector2)
        {
            return new(Math.Max(vector1.X, vector2.X), Math.Max(vector1.Y, vector2.Y));
        }

        public static Vector Clamp(Vector vector, Vector min, Vector max)
        {
            return new(Math.Clamp(vector.X, min.X, max.X), Math.Clamp(vector.Y, min.Y, max.Y));
        }

        // TODO: More operations?
    }
}
