using System.Linq;
using UnityEngine;

namespace Core
{
    public interface IRichTextData
    {
        string Opener { get; }
        string Closer { get; }
    }

    public static class RichTextFormatExtensions
    {
        public static string Format(this string message, params IRichTextData[] rtfData)
        {
            foreach (var formatData in rtfData)
                message = formatData.Apply(message);

            return message;
        }

        public static string Apply(this IRichTextData rtfData, string message)
        {
            return $"{rtfData.Opener}{message}{rtfData.Closer}";
        }

        public static IRichTextData Rtf(this Color color)
        {
            string hexColor = '#' + ColorUtility.ToHtmlStringRGBA(color);
            return Core.Rtf.Color(hexColor);
        }

        #region Single-Tag Shortcuts

        public static string Bold(this string message)
        {
            return message.Format(Core.Rtf.Bold);
        }

        public static string Italic(this string message)
        {
            return message.Format(Core.Rtf.Italic);
        }

        public static string Color(this string message, Color color)
        {
            return message.Format(color.Rtf());
        }

        public static string Size(this string message, int size)
        {
            return message.Format(Core.Rtf.Size(size));
        }

        public static string Material(this string message, ushort id)
        {
            return message.Format(Core.Rtf.Material(id));
        }

        #region Colors

        public static string Aqua(this string message)      { return message.Format(Core.Rtf.AquaColor); }
        public static string Black(this string message)     { return message.Format(Core.Rtf.BlackColor); }
        public static string Blue(this string message)      { return message.Format(Core.Rtf.BlueColor); }
        public static string Brown(this string message)     { return message.Format(Core.Rtf.BrownColor); }
        public static string Cyan(this string message)      { return message.Format(Core.Rtf.CyanColor); }
        public static string DarkBlue(this string message)  { return message.Format(Core.Rtf.DarkBlueColor); }
        public static string Fuchsia(this string message)   { return message.Format(Core.Rtf.FuchsiaColor); }
        public static string Green(this string message)     { return message.Format(Core.Rtf.GreenColor); }
        public static string Grey(this string message)      { return message.Format(Core.Rtf.GreyColor); }
        public static string LightBlue(this string message) { return message.Format(Core.Rtf.LightBlueColor); }
        public static string Lime(this string message)      { return message.Format(Core.Rtf.LimeColor); }
        public static string Magenta(this string message)   { return message.Format(Core.Rtf.MagentaColor); }
        public static string Maroon(this string message)    { return message.Format(Core.Rtf.MaroonColor); }
        public static string Navy(this string message)      { return message.Format(Core.Rtf.NavyColor); }
        public static string Olive(this string message)     { return message.Format(Core.Rtf.OliveColor); }
        public static string Orange(this string message)    { return message.Format(Core.Rtf.OrangeColor); }
        public static string Purple(this string message)    { return message.Format(Core.Rtf.PurpleColor); }
        public static string Red(this string message)       { return message.Format(Core.Rtf.RedColor); }
        public static string Silver(this string message)    { return message.Format(Core.Rtf.SilverColor); }
        public static string Teal(this string message)      { return message.Format(Core.Rtf.TealColor); }
        public static string White(this string message)     { return message.Format(Core.Rtf.WhiteColor); }
        public static string Yellow(this string message)    { return message.Format(Core.Rtf.YellowColor); }

        #endregion

        #endregion
    }

    public static class Rtf
    {
        // Bold and Italic are the easiest RTF tags to handle because they have no parameters.
        public static readonly IRichTextData Bold = new BoldFormat();
        public static readonly IRichTextData Italic = new ItalicFormat();

        // Color is difficult because it is so common, and needs to be expressed tersely
        public static IRichTextData Color(string hexColor)
        {
            return new ColorFormat(hexColor);
        }

        public static IRichTextData Color(Color color)
        {
            return color.Rtf();
        }

        // Size is less common, but ints are pretty ease to represent
        public static IRichTextData Size(int pixels)
        {
            return new SizeFormat(pixels);
        }

        // The most un-used RTF tag. Who knows, maybe someone will use it!
        public static IRichTextData Material(ushort index)
        {
            return new MaterialFormat(index);
        }

        // If you are combining certain tags together all the time, consider making it a composite!
        public static IRichTextData Composite(params IRichTextData[] rtfData)
        {
            return new CompositeFormat(rtfData);
        }

        #region Formats

        private readonly struct BoldFormat : IRichTextData
        {
            public string Opener => "<b>";
            public string Closer => "</b>";
        }

        private readonly struct ItalicFormat : IRichTextData
        {
            public string Opener => "<i>";
            public string Closer => "</i>";
        }

        private readonly struct ColorFormat : IRichTextData
        {
            private readonly string _hexColor;

            public ColorFormat(string hexColor)
            {
                _hexColor = hexColor;
            }

            public string Opener => $"<color={_hexColor}>";
            public string Closer => "</color>";
        }

        private readonly struct SizeFormat : IRichTextData
        {
            private readonly int _pixels;

            public SizeFormat(int pixels)
            {
                _pixels = pixels;
            }

            public string Opener => $"<size={_pixels}>";
            public string Closer => "</size>";
        }

        private readonly struct MaterialFormat : IRichTextData
        {
            private readonly ushort _index;

            public MaterialFormat(ushort index)
            {
                _index = index;
            }

            public string Opener => $"<material={_index}>";
            public string Closer => "</material>";
        }

        private readonly struct CompositeFormat : IRichTextData
        {
            public CompositeFormat(params IRichTextData[] data)
            {
                Opener = "";
                Closer = "";

                foreach (var textData in data)
                    Opener += textData.Opener;

                foreach (var textData in data.Reverse())
                    Closer += textData.Closer;
            }

            public string Opener { get; }
            public string Closer { get; }
        }

        #endregion

        #region Colors

        // Essentially a copy-paste of the Unity default colors, ported into our custom data-type.
        public static IRichTextData AquaColor      = new ColorFormat("#00ffffff");
        public static IRichTextData BlackColor     = new ColorFormat("#000000ff");
        public static IRichTextData BlueColor      = new ColorFormat("#0000ffff");
        public static IRichTextData BrownColor     = new ColorFormat("#a52a2aff");
        public static IRichTextData CyanColor      = new ColorFormat("#00ffffff");
        public static IRichTextData DarkBlueColor  = new ColorFormat("#0000a0ff");
        public static IRichTextData FuchsiaColor   = new ColorFormat("#ff00ffff");
        public static IRichTextData GreenColor     = new ColorFormat("#008000ff");
        public static IRichTextData GreyColor      = new ColorFormat("#808080ff");
        public static IRichTextData LightBlueColor = new ColorFormat("#add8e6ff");
        public static IRichTextData LimeColor      = new ColorFormat("#00ff00ff");
        public static IRichTextData MagentaColor   = new ColorFormat("#ff00ffff");
        public static IRichTextData MaroonColor    = new ColorFormat("#800000ff");
        public static IRichTextData NavyColor      = new ColorFormat("#000080ff");
        public static IRichTextData OliveColor     = new ColorFormat("#808000ff");
        public static IRichTextData OrangeColor    = new ColorFormat("#ffa500ff");
        public static IRichTextData PurpleColor    = new ColorFormat("#800080ff");
        public static IRichTextData RedColor       = new ColorFormat("#ff0000ff");
        public static IRichTextData SilverColor    = new ColorFormat("#c0c0c0ff");
        public static IRichTextData TealColor      = new ColorFormat("#008080ff");
        public static IRichTextData WhiteColor     = new ColorFormat("#ffffffff");
        public static IRichTextData YellowColor    = new ColorFormat("#ffff00ff");

        #endregion

        // Common composite groups.
        public static readonly IRichTextData Error   = Composite(Bold,   RedColor);
        public static readonly IRichTextData Warning = Composite(Bold,   YellowColor);
        public static readonly IRichTextData Hint    = Composite(Italic, GreyColor);
        public static readonly IRichTextData Success = Composite(Bold,   GreenColor);
        public static readonly IRichTextData Failure = Composite(Bold,   RedColor);
    }
}
