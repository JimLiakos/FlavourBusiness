using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SvgAccentModifier
{
    /// <MetaDataID>{ef4fdc4b-06c0-4168-b126-a9f3fce88e57}</MetaDataID>
    public class SvgOppositeRoundCornerBox : ISvgModifier
    {
        public static string GetPath(double cornerRadius, double width, double height, double strokeWidth)
        {

            string pathData = "M {0} {3} L {4} {3}  A {0} {0} 0 0 0 {1} {0} L {1} {5} A {0} {0} 0 0 0 {4} {2} L {0} {2} A {0} {0} 0 0 0 {3} {5} L {3} {0} A {0} {0} 0 0 0 {0} {3} z";
            width -= strokeWidth / 2;
            height -= strokeWidth / 2;
            cornerRadius -= strokeWidth / 2;
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), pathData, cornerRadius, width, height, strokeWidth / 2, width - cornerRadius, height - cornerRadius);
        }

        SharpVectors.Dom.Svg.SvgSvgElement SvgElement;
        public SvgOppositeRoundCornerBox(SharpVectors.Dom.Svg.SvgSvgElement svgElement)
        {
            SvgElement = svgElement;
        }
        public Size Size
        {
            get
            {
                return new Size(SvgElement.CurrentView.ViewBox.BaseVal.Width, SvgElement.CurrentView.ViewBox.BaseVal.Height);
            }
            set
            {
                double widthDif = value.Width - Size.Width;
                double heightDif = value.Height - Size.Height;

                SvgElement.SetSvgSize(value);
                SvgElement.Element("path").SetAttribute("d", GetPath(15, value.Width, value.Height, 3));



            }
        }


        public string Color
        {
            get
            {
                SharpVectors.Dom.Svg.SvgDocument doc = SvgElement.OwnerDocument as SharpVectors.Dom.Svg.SvgDocument;

                for (ulong i = 0; i != doc.StyleSheets.Length; i++)
                {
                    var staleSheet = doc.StyleSheets[i];
                    for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                    {
                        var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                        if (cssStyleRule.SelectorText == ".st0")
                        {
                            if (cssStyleRule.Style.GetPropertyCssValue("stroke") != null)
                                return cssStyleRule.Style.GetPropertyValue("stroke");//, value, cssStyleRule.Style.GetPropertyPriority("stroke"));
                            else
                                cssStyleRule.Style.GetPropertyValue("fill");//, value, cssStyleRule.Style.GetPropertyPriority("fill"));
                        }
                    }
                }
                return "";
            }
            set
            {

                SharpVectors.Dom.Svg.SvgDocument doc = SvgElement.OwnerDocument as SharpVectors.Dom.Svg.SvgDocument;

                for (ulong i = 0; i != doc.StyleSheets.Length; i++)
                {
                    var staleSheet = doc.StyleSheets[i];
                    for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                    {
                        var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                        if (cssStyleRule.SelectorText == ".st0")
                        {
                            if (cssStyleRule.Style.GetPropertyCssValue("stroke") != null)
                                cssStyleRule.Style.SetProperty("stroke", value, cssStyleRule.Style.GetPropertyPriority("stroke"));
                            else
                                cssStyleRule.Style.SetProperty("fill", value, cssStyleRule.Style.GetPropertyPriority("fill"));
                        }
                    }
                    string newCssText = "\n\n";
                    for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                    {
                        var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                        newCssText += "\n" + cssStyleRule.CssText;
                    }
                    newCssText += "\n\n";
                    staleSheet.OwnerNode.InnerText = newCssText;
                }

            }
        }
    }
}
