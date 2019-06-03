using System.Windows.Media;
using System.Windows;

namespace NoteUtils
{
    public static class WindowsFormsHelper
    {
        /// <summary>
        /// Utils for Notepad.NET
        /// </summary>
        
        public static System.Drawing.Color Convert(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        } //Convert From Windows.Media

        public static Color Convert(this System.Drawing.Color color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        } //Convert from Windows.Drawing

        /*public static Color Convert(this System.Drawing.Color color)
        {
            return new Color()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B
            };
        }*/

        public static bool ChooseColor(ref Color color)
        {
            using (System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog())
            {
                colorDialog.Color = color.Convert();
                colorDialog.AllowFullOpen = true;

                bool result = colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK;

                if (result)
                    color = colorDialog.Color.Convert();
                return result;
            }
        }

        public struct Font
        {
            public FontFamily Family { get; set; }

            public string FamilyName
            {
                get
                {
                    return Family.ToString();
                }
            }

            public FontStyle Style { get; set; }
            public FontWeight Weight { get; set; }
            public TextDecorationCollection textDecorations { get; set; }
            public double Size { get; set; }
            public Color Color { get; set; }
            public Brush brush { get { return new SolidColorBrush(Color); } }

            public static Font Default
            {
                get
                {
                    return new Font()
                    {
                        Family = new FontFamily("Segoe UI"),
                        Style = FontStyles.Normal,
                        Weight = FontWeights.Normal,
                        textDecorations = null,
                        Size = 12,
                        Color = Colors.Black
                    };
                }
            }

            public static System.Drawing.Font defineToSystemDrawingFont(Font font)
            {
                System.Drawing.FontStyle style = (font.Style == FontStyles.Italic) ?
                    System.Drawing.FontStyle.Italic : System.Drawing.FontStyle.Regular;
                if (font.Weight == FontWeights.Bold)
                    style |= System.Drawing.FontStyle.Bold;
                if (font.textDecorations.Contains(System.Windows.TextDecorations.Underline[0]))
                    style |= System.Drawing.FontStyle.Underline;
                if (font.textDecorations.Contains(System.Windows.TextDecorations.Strikethrough[0]))
                    style |= System.Drawing.FontStyle.Strikeout;
                System.Drawing.Font _Font = new System.Drawing.Font(font.FamilyName, (int)font.Size, style);
                return _Font;
            }

            public static Font defineToWindowsMedia(System.Drawing.Font sdFont, System.Drawing.Color sdColor)
            {
                Font font = new Font();
                font.Family = new FontFamily(sdFont.FontFamily.Name);
                font.Style = sdFont.Italic ? FontStyles.Italic : FontStyles.Normal;
                font.Weight = sdFont.Bold ? FontWeights.Bold : FontWeights.Regular;
                font.textDecorations = new TextDecorationCollection();
                if (sdFont.Underline)
                    font.textDecorations.Add(System.Windows.TextDecorations.Underline);
                if (sdFont.Strikeout)
                    font.textDecorations.Add(System.Windows.TextDecorations.Strikethrough);
                font.Size = sdFont.Size;
                font.Color = sdColor.Convert();
                return font;
            }
        }
    }
}



