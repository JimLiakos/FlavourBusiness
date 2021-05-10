using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{525957bb-6671-4999-a6a6-5c3ac9a2b05a}</MetaDataID>
    public class DragCanvasItem
    {
        public readonly MenuPresentationModel.MenuCanvas.IMenuCanvasItem MenuCanvasItem;

        public DragCanvasItem(MenuPresentationModel.MenuCanvas.IMenuCanvasItem menuCanvasItem)
        {
            MenuCanvasItem = menuCanvasItem;
        }
    }

}
