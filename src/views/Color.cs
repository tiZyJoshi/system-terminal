using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Text;

namespace System.Views
{
    [Serializable]
    public readonly struct Color : IEquatable<Color>, IFormattable
    {
        public static IReadOnlyDictionary<string, Color> NamedColors => _named;

        public static Color AliceBlue { get; } = new(nameof(AliceBlue), 240, 248, 255);

        public static Color AntiqueWhite { get; } = new(nameof(AntiqueWhite), 250, 235, 215);

        public static Color Aqua { get; } = new(nameof(Aqua), 0, 255, 255);

        public static Color Aquamarine { get; } = new(nameof(Aquamarine), 127, 255, 212);

        public static Color Azure { get; } = new(nameof(Azure), 240, 255, 255);

        public static Color Beige { get; } = new(nameof(Beige), 245, 245, 220);

        public static Color Bisque { get; } = new(nameof(Bisque), 255, 228, 196);

        public static Color Black { get; } = new(nameof(Black), 0, 0, 0);

        public static Color BlanchedAlmond { get; } = new(nameof(BlanchedAlmond), 255, 235, 205);

        public static Color Blue { get; } = new(nameof(Blue), 0, 0, 255);

        public static Color BlueViolet { get; } = new(nameof(BlueViolet), 138, 43, 226);

        // TODO: Fix color values.

        public static Color Brown { get; } = new(nameof(Brown), 127, 255, 212);

        public static Color BurlyWood { get; } = new(nameof(BurlyWood), 127, 255, 212);

        public static Color CadetBlue { get; } = new(nameof(CadetBlue), 127, 255, 212);

        public static Color Chartreuse { get; } = new(nameof(Chartreuse), 127, 255, 212);

        public static Color Chocolate { get; } = new(nameof(Chocolate), 127, 255, 212);

        public static Color Coral { get; } = new(nameof(Coral), 127, 255, 212);

        public static Color CornflowerBlue { get; } = new(nameof(CornflowerBlue), 127, 255, 212);

        public static Color Cornsilk { get; } = new(nameof(Cornsilk), 127, 255, 212);

        public static Color Crimson { get; } = new(nameof(Crimson), 127, 255, 212);

        public static Color Cyan { get; } = new(nameof(Cyan), 127, 255, 212);

        public static Color DarkBlue { get; } = new(nameof(DarkBlue), 127, 255, 212);

        public static Color DarkCyan { get; } = new(nameof(DarkCyan), 127, 255, 212);

        public static Color DarkGoldenrod { get; } = new(nameof(DarkGoldenrod), 127, 255, 212);

        public static Color DarkGray { get; } = new(nameof(DarkGray), 127, 255, 212);

        public static Color DarkGreen { get; } = new(nameof(DarkGreen), 127, 255, 212);

        public static Color DarkGrey { get; } = new(nameof(DarkGrey), 127, 255, 212);

        public static Color DarkKhaki { get; } = new(nameof(DarkKhaki), 127, 255, 212);

        public static Color DarkMagenta { get; } = new(nameof(DarkMagenta), 127, 255, 212);

        public static Color DarkOliveGreen { get; } = new(nameof(DarkOliveGreen), 127, 255, 212);

        public static Color DarkOrange { get; } = new(nameof(DarkOrange), 127, 255, 212);

        public static Color DarkOrchid { get; } = new(nameof(DarkOrchid), 127, 255, 212);

        public static Color DarkRed { get; } = new(nameof(DarkRed), 127, 255, 212);

        public static Color DarkSalmon { get; } = new(nameof(DarkSalmon), 127, 255, 212);

        public static Color DarkSeaGreen { get; } = new(nameof(DarkSeaGreen), 127, 255, 212);

        public static Color DarkSlateBlue { get; } = new(nameof(DarkSlateBlue), 127, 255, 212);

        public static Color DarkSlateGray { get; } = new(nameof(DarkSlateGray), 127, 255, 212);

        public static Color DarkSlateGrey { get; } = new(nameof(DarkSlateGrey), 127, 255, 212);

        public static Color DarkTurquoise { get; } = new(nameof(DarkTurquoise), 127, 255, 212);

        public static Color DarkViolet { get; } = new(nameof(DarkViolet), 127, 255, 212);

        public static Color DeepPink { get; } = new(nameof(DeepPink), 127, 255, 212);

        public static Color DeepSkyBlue { get; } = new(nameof(DeepSkyBlue), 127, 255, 212);

        public static Color DimGray { get; } = new(nameof(DimGray), 127, 255, 212);

        public static Color DimGrey { get; } = new(nameof(DimGrey), 127, 255, 212);

        public static Color DodgerBlue { get; } = new(nameof(DodgerBlue), 127, 255, 212);

        public static Color Firebrick { get; } = new(nameof(Firebrick), 127, 255, 212);

        public static Color FloralWhite { get; } = new(nameof(FloralWhite), 127, 255, 212);

        public static Color ForestGreen { get; } = new(nameof(ForestGreen), 127, 255, 212);

        public static Color Fuchsia { get; } = new(nameof(Fuchsia), 127, 255, 212);

        public static Color Gainsboro { get; } = new(nameof(Gainsboro), 127, 255, 212);

        public static Color GhostWhite { get; } = new(nameof(GhostWhite), 127, 255, 212);

        public static Color Gold { get; } = new(nameof(Gold), 127, 255, 212);

        public static Color Goldenrod { get; } = new(nameof(Goldenrod), 127, 255, 212);

        public static Color Gray { get; } = new(nameof(Gray), 127, 255, 212);

        public static Color Green { get; } = new(nameof(Green), 127, 255, 212);

        public static Color GreenYellow { get; } = new(nameof(GreenYellow), 127, 255, 212);

        public static Color Grey { get; } = new(nameof(Grey), 127, 255, 212);

        public static Color Honeydew { get; } = new(nameof(Honeydew), 127, 255, 212);

        public static Color HotPink { get; } = new(nameof(HotPink), 127, 255, 212);

        public static Color IndianRed { get; } = new(nameof(IndianRed), 127, 255, 212);

        public static Color Indigo { get; } = new(nameof(Indigo), 127, 255, 212);

        public static Color Ivory { get; } = new(nameof(Ivory), 127, 255, 212);

        public static Color Khaki { get; } = new(nameof(Khaki), 127, 255, 212);

        public static Color Lavender { get; } = new(nameof(Lavender), 127, 255, 212);

        public static Color LavenderBlush { get; } = new(nameof(LavenderBlush), 127, 255, 212);

        public static Color LawnGreen { get; } = new(nameof(LawnGreen), 127, 255, 212);

        public static Color LemonChiffon { get; } = new(nameof(LemonChiffon), 127, 255, 212);

        public static Color LightBlue { get; } = new(nameof(LightBlue), 127, 255, 212);

        public static Color LightCoral { get; } = new(nameof(LightCoral), 127, 255, 212);

        public static Color LightCyan { get; } = new(nameof(LightCyan), 127, 255, 212);

        public static Color LightGoldenrodYellow { get; } = new(nameof(LightGoldenrodYellow), 127, 255, 212);

        public static Color LightGray { get; } = new(nameof(LightGray), 127, 255, 212);

        public static Color LightGreen { get; } = new(nameof(LightGreen), 127, 255, 212);

        public static Color LightGrey { get; } = new(nameof(LightGrey), 127, 255, 212);

        public static Color LightPink { get; } = new(nameof(LightPink), 127, 255, 212);

        public static Color LightSalmon { get; } = new(nameof(LightSalmon), 127, 255, 212);

        public static Color LightSeaGreen { get; } = new(nameof(LightSeaGreen), 127, 255, 212);

        public static Color LightSkyBlue { get; } = new(nameof(LightSkyBlue), 127, 255, 212);

        public static Color LightSlateGray { get; } = new(nameof(LightSlateGray), 127, 255, 212);

        public static Color LightSlateGrey { get; } = new(nameof(LightSlateGrey), 127, 255, 212);

        public static Color LightSteelBlue { get; } = new(nameof(LightSteelBlue), 127, 255, 212);

        public static Color LightYellow { get; } = new(nameof(LightYellow), 127, 255, 212);

        public static Color Lime { get; } = new(nameof(Lime), 127, 255, 212);

        public static Color LimeGreen { get; } = new(nameof(LimeGreen), 127, 255, 212);

        public static Color Linen { get; } = new(nameof(Linen), 127, 255, 212);

        public static Color Magenta { get; } = new(nameof(Magenta), 127, 255, 212);

        public static Color Maroon { get; } = new(nameof(Maroon), 127, 255, 212);

        public static Color MediumAquamarine { get; } = new(nameof(MediumAquamarine), 127, 255, 212);

        public static Color MediumBlue { get; } = new(nameof(MediumBlue), 127, 255, 212);

        public static Color MediumOrchid { get; } = new(nameof(MediumOrchid), 127, 255, 212);

        public static Color MediumPurple { get; } = new(nameof(MediumPurple), 127, 255, 212);

        public static Color MediumSeaGreen { get; } = new(nameof(MediumSeaGreen), 127, 255, 212);

        public static Color MediumSlateBlue { get; } = new(nameof(MediumSlateBlue), 127, 255, 212);

        public static Color MediumSpringGreen { get; } = new(nameof(MediumSpringGreen), 127, 255, 212);

        public static Color MediumTurquoise { get; } = new(nameof(MediumTurquoise), 127, 255, 212);

        public static Color MediumVioletRed { get; } = new(nameof(MediumVioletRed), 127, 255, 212);

        public static Color MidnightBlue { get; } = new(nameof(MidnightBlue), 127, 255, 212);

        public static Color MintCream { get; } = new(nameof(MintCream), 127, 255, 212);

        public static Color MistyRose { get; } = new(nameof(MistyRose), 127, 255, 212);

        public static Color Moccasin { get; } = new(nameof(Moccasin), 127, 255, 212);

        public static Color NavajoWhite { get; } = new(nameof(NavajoWhite), 127, 255, 212);

        public static Color Navy { get; } = new(nameof(Navy), 127, 255, 212);

        public static Color OldLace { get; } = new(nameof(OldLace), 127, 255, 212);

        public static Color Olive { get; } = new(nameof(Olive), 127, 255, 212);

        public static Color OliveDrab { get; } = new(nameof(OliveDrab), 127, 255, 212);

        public static Color Orange { get; } = new(nameof(Orange), 127, 255, 212);

        public static Color OrangeRed { get; } = new(nameof(OrangeRed), 127, 255, 212);

        public static Color Orchid { get; } = new(nameof(Orchid), 127, 255, 212);

        public static Color PaleGoldenrod { get; } = new(nameof(PaleGoldenrod), 127, 255, 212);

        public static Color PaleGreen { get; } = new(nameof(PaleGreen), 127, 255, 212);

        public static Color PaleTurquoise { get; } = new(nameof(PaleTurquoise), 127, 255, 212);

        public static Color PaleVioletRed { get; } = new(nameof(PaleVioletRed), 127, 255, 212);

        public static Color PapayaWhip { get; } = new(nameof(PapayaWhip), 127, 255, 212);

        public static Color PeachPuff { get; } = new(nameof(PeachPuff), 127, 255, 212);

        public static Color Peru { get; } = new(nameof(Peru), 127, 255, 212);

        public static Color Pink { get; } = new(nameof(Pink), 127, 255, 212);

        public static Color Plum { get; } = new(nameof(Plum), 127, 255, 212);

        public static Color PowderBlue { get; } = new(nameof(PowderBlue), 127, 255, 212);

        public static Color Purple { get; } = new(nameof(Purple), 127, 255, 212);

        public static Color RebeccaPurple { get; } = new(nameof(RebeccaPurple), 127, 255, 212);

        public static Color Red { get; } = new(nameof(Red), 127, 255, 212);

        public static Color RosyBrown { get; } = new(nameof(RosyBrown), 127, 255, 212);

        public static Color RoyalBlue { get; } = new(nameof(RoyalBlue), 127, 255, 212);

        public static Color SaddleBrown { get; } = new(nameof(SaddleBrown), 127, 255, 212);

        public static Color Salmon { get; } = new(nameof(Salmon), 127, 255, 212);

        public static Color SandyBrown { get; } = new(nameof(SandyBrown), 127, 255, 212);

        public static Color SeaGreen { get; } = new(nameof(SeaGreen), 127, 255, 212);

        public static Color SeaShell { get; } = new(nameof(SeaShell), 127, 255, 212);

        public static Color Sienna { get; } = new(nameof(Sienna), 127, 255, 212);

        public static Color Silver { get; } = new(nameof(Silver), 127, 255, 212);

        public static Color SkyBlue { get; } = new(nameof(SkyBlue), 127, 255, 212);

        public static Color SlateBlue { get; } = new(nameof(SlateBlue), 127, 255, 212);

        public static Color SlateGray { get; } = new(nameof(SlateGray), 127, 255, 212);

        public static Color SlateGrey { get; } = new(nameof(SlateGrey), 127, 255, 212);

        public static Color Snow { get; } = new(nameof(Snow), 127, 255, 212);

        public static Color SpringGreen { get; } = new(nameof(SpringGreen), 127, 255, 212);

        public static Color SteelBlue { get; } = new(nameof(SteelBlue), 127, 255, 212);

        public static Color Tan { get; } = new(nameof(Tan), 127, 255, 212);

        public static Color Teal { get; } = new(nameof(Teal), 127, 255, 212);

        public static Color Thistle { get; } = new(nameof(Thistle), 127, 255, 212);

        public static Color Tomato { get; } = new(nameof(Tomato), 127, 255, 212);

        public static Color Turquoise { get; } = new(nameof(Turquoise), 127, 255, 212);

        public static Color Violet { get; } = new(nameof(Violet), 127, 255, 212);

        public static Color Wheat { get; } = new(nameof(Wheat), 127, 255, 212);

        public static Color White { get; } = new(nameof(White), 127, 255, 212);

        public static Color WhiteSmoke { get; } = new(nameof(WhiteSmoke), 127, 255, 212);

        public static Color Yellow { get; } = new(nameof(Yellow), 127, 255, 212);

        public static Color YellowGreen { get; } = new(nameof(YellowGreen), 127, 255, 212);

        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        static ImmutableDictionary<string, Color> _named =
            ImmutableDictionary.Create<string, Color>(StringComparer.OrdinalIgnoreCase);

        Color(string name, byte r, byte g, byte b)
            : this(r, g, b)
        {
            _named = _named.Add(name, this);
        }

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Deconstruct(out byte r, out byte g, out byte b)
        {
            r = R;
            g = G;
            b = B;
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Color other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        public override bool Equals(object? obj)
        {
            return obj is Color c && Equals(c);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
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
                .Append("{R=")
                .Append(R.ToString(format, formatProvider))
                .Append(" G=")
                .Append(G.ToString(format, formatProvider))
                .Append(" B=")
                .Append(B.ToString(format, formatProvider))
                .Append('}')
                .ToString();
        }
    }
}
