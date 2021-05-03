using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{f329b2da-debf-4ec5-88aa-261b78ca8b78}</MetaDataID>
    public class DragItemsCategory
    {

        public readonly MenuModel.IItemsCategory ItemsCategory;

        public DragItemsCategory(MenuModel.IItemsCategory itemsCategory)
        {
            ItemsCategory = itemsCategory;
        }



    }
}
