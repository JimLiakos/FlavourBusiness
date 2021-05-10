using System;//
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SharpVectors.Dom.Svg;

namespace SvgAccentModifier
{
    /// <MetaDataID>{0ca190b3-5806-4e51-87b0-ff6caa1f1301}</MetaDataID>
    public class RegularBoxModifier : ISvgModifier
    {
        SharpVectors.Dom.Svg.SvgSvgElement SvgElement;
        public RegularBoxModifier(SharpVectors.Dom.Svg.SvgSvgElement svgElement)
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

                foreach (SvgRectElement rect in SvgElement.Elements("rect").OfType<SvgRectElement>())
                {
                    if (rect != null)
                    {
                        double newWidtht = rect.Width.BaseVal.Value + widthDif;
                        double newHeight = rect.Height.BaseVal.Value + heightDif;
                        rect.SetSvgRectSize(newWidtht, newHeight);
                        var sds = rect.GetType();
                    }
                }



                //cssStyleRule.Style.SetProperty("fill", newColor, cssStyleRule.Style.GetPropertyPriority("fill"));



                var sss = this.Size;
                //SvgElement.CurrentView.ViewBox.BaseVal.Height = value.Height;
                //SvgElement.CurrentView.ViewBox.BaseVal.Width = value.Width;
                //SvgElement.CurrentView.ViewBox.AnimVal.Height = value.Height;
                //SvgElement.CurrentView.ViewBox.AnimVal.Width = value.Width;


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
                                return cssStyleRule.Style.GetPropertyValue("fill");//, value, cssStyleRule.Style.GetPropertyPriority("fill"));
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
