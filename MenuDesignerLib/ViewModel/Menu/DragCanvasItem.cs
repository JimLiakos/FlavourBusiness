using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{525957bb-6671-4999-a6a6-5c3ac9a2b05a}</MetaDataID>
    public class DragCanvasItems
    {
        public readonly List<MenuPresentationModel.MenuCanvas.IMenuCanvasItem> MenuCanvasItems;

        public DragCanvasItems(List<MenuPresentationModel.MenuCanvas.IMenuCanvasItem> menuCanvasItems)
        {
            if (menuCanvasItems.Count == 0)
                throw new ArgumentException("Parameter menuCanvasItems value is empty list");
            MenuCanvasItems = menuCanvasItems;
        }
        public DragCanvasItems(MenuPresentationModel.MenuCanvas.IMenuCanvasItem menuCanvasItem)
        {
            MenuCanvasItems = new List<MenuPresentationModel.MenuCanvas.IMenuCanvasItem>() { menuCanvasItem };
        }

    }

}
