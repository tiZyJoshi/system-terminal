using System.ComponentModel;
using System.Text;

namespace System.Views
{
    [Serializable]
    public readonly struct Rectangle : IEquatable<Rectangle>, IFormattable
    {
        public static Rectangle Empty { get; }

        public Point Location { get; }

        public Size Size { get; }

        public int Width => (int)Size.Width!;

        public int Height => (int)Size.Height!;

        public int Left => Location.X;

        public int Top => Location.Y;

        public int Right => Left + Width;

        public int Bottom => Top + Height;

        public Point TopLeft => new(Left, Top);

        public Point TopRight => new(Right, Top);

        public Point BottomLeft => new(Left, Bottom);

        public Point BottomRight => new(Right, Bottom);

        public Point Center => new(Left + Width / 2, Top + Height / 2);

        public bool IsEmpty => this == Empty;

        public Rectangle(Point location, Size size)
        {
            if (size.Width == null || size.Height == null)
                throw new ArgumentException("Specified size is invalid.", nameof(size));

            Location = location;
            Size = size;

            // Normalize empty rectangles so comparisons to Empty work as expected.
            if (size.IsEmpty)
                this = Empty;
        }

        public Rectangle(Point point1, Point point2)
        {
            Location = new(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            Size = new(Math.Max(point1.X, point2.X) - Location.X, Math.Max(point1.Y, point2.Y) - Location.Y);

            // Normalize empty rectangles so comparisons to Empty work as expected.
            if (Size.IsEmpty)
                this = Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out Point location, out Size size)
        {
            location = Location;
            size = Size;
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Rectangle other)
        {
            return Location == other.Location && Size == other.Size;
        }

        public override bool Equals(object? obj)
        {
            return obj is Rectangle r && Equals(r);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Location, Size);
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
                .Append("{Location=")
                .Append(Location.ToString(format, formatProvider))
                .Append(" Size=")
                .Append(Size.ToString(format, formatProvider))
                .Append('}')
                .ToString();
        }

        public bool Contains(Point point)
        {
            return !IsEmpty &&
                Left <= point.X && Right > point.X &&
                Top <= point.Y && Bottom > point.Y;
        }

        public bool Contains(Rectangle rectangle)
        {
            return !IsEmpty && !rectangle.IsEmpty &&
                Left <= rectangle.Left && Right >= rectangle.Right &&
                Top <= rectangle.Top && Bottom >= rectangle.Bottom;
        }

        public bool IntersectsWith(Rectangle rectangle)
        {
            return !IsEmpty && !rectangle.IsEmpty &&
                Left < rectangle.Right &&
                Right >= rectangle.Left &&
                Top < rectangle.Bottom &&
                Bottom >= rectangle.Top;
        }

        // TODO: More operations?
    }
}
