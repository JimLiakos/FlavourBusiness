using MenuPresentationModel.MenuStyles;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.ViewModel
{
    public interface IMenusStyleSheets
    {

        List<IMenuStyleSheet> StyleSheets { get; }
    }

    [HttpVisible]
    public interface IMenuStyleSheet
    {

        
        string Name { get; }
        Task<IStyleSheet> StyleSheet { get; }

        WPFUIElementObjectBind.MenuCommand ItemFontsMenu { get; }

        WPFUIElementObjectBind.MenuCommand DesignItemInfoViewMenu { get; }

    }
}
