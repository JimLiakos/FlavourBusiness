using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{f3d1695a-f156-4737-bd0b-2a4bbf204d48}</MetaDataID>
    [HttpVisible]
    public interface IFontsResolver
    {
        FontData GetFont(string fontUri);
    }
}
