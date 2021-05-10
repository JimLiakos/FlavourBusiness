using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLBManager.ViewModel;
using OOAdvantech.Transactions;

namespace MenuDesigner.ViewModel
{
    /// <MetaDataID>{4442d7e1-c1d6-4fca-abb7-5d89f7ca9d2c}</MetaDataID>
    public class RestaurantMenuItemsPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        MenuModel.IMenu Menu;

        /// <exclude>Excluded</exclude>
        MenuPresentationModel.RestaurantMenu _GraphicMenu;
        public MenuPresentationModel.RestaurantMenu GraphicMenu
        {
            get
            {
                return _GraphicMenu;
            }
            set
            {
                if (_GraphicMenu != value)
                {
                    _GraphicMenu = value;

                    using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            RootCategory.GraphicMenu = value;
                            stateTransition.Consistent = true;
                        }
                        suppressStateTransition.Consistent = true;
                    }
                }
            }
        }
        public MenuCanvas.ItemsCategoryViewModel RootCategory { get; set; }
        public RestaurantMenuItemsPresentation(MenuModel.IMenu menu, MenuPresentationModel.RestaurantMenu graphicMenu)
        {
            _GraphicMenu = graphicMenu;
            Menu = menu;

            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    RootCategory = new MenuCanvas.ItemsCategoryViewModel(Menu.RootCategory, null, graphicMenu);
                    stateTransition.Consistent = true;
                }
                suppressStateTransition.Consistent = true;
            }
            

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

        List<WPFUIElementObjectBind.MenuCommand> _ContextMenuItems;
        public List<WPFUIElementObjectBind.MenuCommand> ContextMenuItems
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
