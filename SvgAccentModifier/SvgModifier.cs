using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvgAccentModifier
{
    /// <MetaDataID>{35ecbde9-1fe9-4eef-855d-16ce4dea2469}</MetaDataID>
    public class SvgModifier
    {
        public static ISvgModifier GetModifier(SharpVectors.Dom.Svg.SvgSvgElement svgElement)
        {

            if (svgElement.GetAttribute("class") == "regularbox")
            {
                return new RegularBoxModifier(svgElement);
            }
            if (svgElement.GetAttribute("class") == "sizeablebox")
            {
                return new SizeableSvgModifier(svgElement);
            }
            if (svgElement.GetAttribute("class") == "oppositeroundcornerBox")
            {
                return new SvgOppositeRoundCornerBox(svgElement);
            }

            if (svgElement.GetAttribute("class") == "bracket")
            {
                return new BracketSvgModifier(svgElement);
            }





            return new ColorSvgModifier(svgElement);
        }
    }
}
