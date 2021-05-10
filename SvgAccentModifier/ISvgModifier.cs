using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SvgAccentModifier
{
    /// <MetaDataID>{59562705-cc91-42ec-82bc-bac3e1523b24}</MetaDataID>
    public interface ISvgModifier
    {
        Size Size { get; set; }

        string Color { get; set; }
    }
}
