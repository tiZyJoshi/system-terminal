namespace System.Views
{
    [Flags]
    public enum Decorations
    {
        None = 0b0000000000,
        Bold = 0b0000000001,
        Faint = 0b0000000010,
        Italic = 0b0000000100,
        Underline = 0b0000001000,
        Blink = 0b0000010000,
        Invert = 0b0000100000,
        Invisible = 0b0001000000,
        Strike = 0b0010000000,
        DoubleUnderline = 0b0100000000,
        Overline = 0b1000000000,
    }
}
