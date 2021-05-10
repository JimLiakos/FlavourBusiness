using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using SharpVectors.Dom.Css;

namespace SvgAccentModifier
{


    /// <MetaDataID>{ce603632-6a84-4ea9-8097-5ebb95c7901d}</MetaDataID>
    public static class SvgDocumentExtender
    {
        /// <MetaDataID>{89e59bc2-4da3-49c2-b3ff-1d55ca39357a}</MetaDataID>
        public static System.Xml.XmlElement Element(this XmlElement xmlElement, string elementName)
        {
            return xmlElement.ChildNodes.OfType<XmlElement>().Where(x => x.Name == elementName).FirstOrDefault();
        }

        public static List<XmlElement> Elements(this XmlElement xmlElement, string elementName)
        {
            return xmlElement.ChildNodes.OfType<XmlElement>().Where(x => x.Name == elementName).ToList();
        }

        public static List<string> GetCssProperties(this ICssStyleDeclaration style)
        {
            List<string> properties = new List<string>();
            for (ulong i = 0; i != style.Length; i++)
            {
                properties.Add(style[i]);
            }
            return properties;
        }

        public static void SetSvgRectSize(this SharpVectors.Dom.Svg.SvgRectElement svgElement, Size newSize)
        {
            svgElement.SetSvgRectSize(newSize.Width, newSize.Height);
        }
        public static void SetSvgRectSize(this SharpVectors.Dom.Svg.SvgRectElement svgElement, double width, double height)
        {
            svgElement.SetAttribute("width", width.ToString(CultureInfo.GetCultureInfo(1033)));
            svgElement.SetAttribute("height", height.ToString(CultureInfo.GetCultureInfo(1033)));
        }


        public static void SetSvgSize(this SharpVectors.Dom.Svg.SvgSvgElement svgElement, Size newSize)
        {
            string viewBoxValue = string.Format(CultureInfo.GetCultureInfo(1033), "0 0 {0} {1}", newSize.Width, newSize.Height);
            string widthValue = string.Format(CultureInfo.GetCultureInfo(1033), "{0}px", newSize.Width);
            string heightValue = string.Format(CultureInfo.GetCultureInfo(1033), "{0}px", newSize.Height);
            svgElement.SetAttribute("viewBox", viewBoxValue);
            svgElement.SetAttribute("width", widthValue);
            svgElement.SetAttribute("height", heightValue);

            var cssProperties = svgElement.Style.GetCssProperties();

            if (!string.IsNullOrWhiteSpace(svgElement.Style.GetPropertyValue("enable-background")))
            {
                svgElement.Style.SetProperty("enable-background", "new " + viewBoxValue, svgElement.Style.GetPropertyPriority("enable-background"));
                string styleValue = null;
                foreach (var cssProperty in cssProperties)
                {
                    string propertyValue = svgElement.Style.GetPropertyValue(cssProperty);
                    styleValue += cssProperty + ":" + propertyValue + ";";
                }
                svgElement.SetAttribute("style", styleValue);
            }
        }




    }


}
