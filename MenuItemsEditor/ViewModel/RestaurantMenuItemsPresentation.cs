using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuItemsEditor.ViewModel;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{4442d7e1-c1d6-4fca-abb7-5d89f7ca9d2c}</MetaDataID>
    public class RestaurantMenuItemsPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        MenuModel.IMenu Menu;

        ItemsCategoryViewModel RootCategory;
        public RestaurantMenuItemsPresentation(MenuModel.IMenu menu)
        {
            Menu = menu;
            RootCategory = new ItemsCategoryViewModel(Menu.RootCategory, null);
            RootCategory.IsNodeExpanded = true;
            foreach (MenuItemsEditor.IMenusTreeNode treeNode in RootCategory.Members)
                treeNode.IsNodeExpanded = true;

            SelectedCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));
            });
        }
        public WPFUIElementObjectBind.RelayCommand SelectedCommand { get; protected set; }
        

        public List<MenuItemsEditor.IMenusTreeNode> MenuItems
        {
            get
            {
                return RootCategory.Members;
            }
        }
     
        List<MenuCommand> _ContextMenuItems;
        public List<MenuCommand> ContextMenuItems
        {
            get
            {
                var selectedItemContextMenuItems = RootCategory.SelectedItemContextMenuItems;
                if (selectedItemContextMenuItems != null)
                    return selectedItemContextMenuItems;
                else
                    return RootCategory.ContextMenuItems;
            }
        }
    }
}
