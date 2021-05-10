using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SvgAccentModifier
{
    /// <MetaDataID>{654984d2-5293-4ce7-b59b-6025163cad08}</MetaDataID>
    public class BracketSvgModifier : ISvgModifier
    {
        public static string GetLeftPath(double width, double height)//, double strokeWidth)
        {

            string pathData = "M 8,0 L 0 0 L 0 {0} L 8 {0}";
            //width -= strokeWidth / 2;
            //height -= strokeWidth / 2;
            //cornerRadius -= strokeWidth / 2;
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), pathData, height);
        }

        public static string GetRightPath(double width, double height)
        {

            string pathData = "M {2},0 L {0} 0 L {0} {1} L {2} {1}";
            //width -= strokeWidth / 2;
            //height -= strokeWidth / 2;
            //cornerRadius -= strokeWidth / 2;
            return string.Format(System.Globalization.CultureInfo.GetCultureInfo(1033), pathData, width, height, width - 8);
        }


        SharpVectors.Dom.Svg.SvgSvgElement SvgElement;
        public BracketSvgModifier(SharpVectors.Dom.Svg.SvgSvgElement svgElement)
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
                SvgElement.Elements("path")[0].SetAttribute("d", GetLeftPath(value.Width, value.Height));
                SvgElement.Elements("path")[1].SetAttribute("d", GetRightPath(value.Width, value.Height));
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
