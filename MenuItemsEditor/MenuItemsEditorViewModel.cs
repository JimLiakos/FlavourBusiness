using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MenuModel;


namespace MenuItemsEditor
{
    /// <MetaDataID>{251a7b87-52b3-4c71-8650-013c08fa4d16}</MetaDataID>
    public class RestauranConfigViewModel : OOAdvantech.UserInterface.Runtime.PresentationObject<RestaurantMenus>, INotifyPropertyChanged
    {

        public RestauranConfigViewModel(RestaurantMenus restaurantMenus)
            :base(restaurantMenus)
        {

        }

        public List<IMenusTreeNode> RestauranAttributes
        {
            get
            {
                return new List<IMenusTreeNode> { RealObject };
            }
        }

     
    }
}
