using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MenuDesigner.MenuPresentetion
{
    /// <MetaDataID>{09334970-25da-49ae-85ad-433063f0528d}</MetaDataID>
    public class MenuItemsPresentationViewModel : MarshalByRefObject
    {

        public readonly MenuPresentationModel.MenuItemsPresentation MenuItemsPresentation;

        public MenuItemsPresentationViewModel(MenuPresentationModel.MenuItemsPresentation menuItemsPresentation)
        {
            this.MenuItemsPresentation = menuItemsPresentation;
        }

        public MenuItemsPresentationViewModel()
        {

        }


        public List<MenuItemPresentationViewModel> MenuItems
        {
            get
            {
                List<MenuItemPresentationViewModel> menuItems = new List<MenuItemPresentationViewModel>();
                foreach (var menuItem in MenuItemsPresentation.MenuItems)
                {
                    MenuItemPresentationViewModel menuItemPresentationViewModel = null;
                    if(!_MenuItems.TryGetValue(menuItem, out menuItemPresentationViewModel))
                    {
                        menuItemPresentationViewModel = new MenuItemPresentationViewModel(this, menuItem);
                        menuItemPresentationViewModel.PriceTextWidthChanged += MenuItemPresentationViewModel_PriceTextWidthChanged;
                        _MenuItems[menuItem] = menuItemPresentationViewModel;
                    }
                    menuItems.Add(menuItemPresentationViewModel);
                }

                return menuItems;
            }
        }

        private void MenuItemPresentationViewModel_PriceTextWidthChanged(MenuItemPresentationViewModel sender)
        {
            double maxPriceTextWidth = 0;
            foreach (var menuItem in MenuItems)
            {
                if (maxPriceTextWidth < menuItem.PriceTextWidth)
                    maxPriceTextWidth = menuItem.PriceTextWidth;
            }
            foreach (var menuItem in MenuItems)
            {
                menuItem.PriceColumntWidth = new System.Windows.GridLength(maxPriceTextWidth);

            }
        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        Dictionary<MenuModel.MenuItem, MenuItemPresentationViewModel> _MenuItems = new Dictionary<MenuModel.MenuItem, MenuItemPresentationViewModel>();
        public void RemoveMenuItem(MenuItemPresentationViewModel menuItemPresentationViewModel)
        {
            menuItemPresentationViewModel.PriceTextWidthChanged -= MenuItemPresentationViewModel_PriceTextWidthChanged;
            MenuItemsPresentation.RemoveMenuItem(menuItemPresentationViewModel.MenuItem);
            _MenuItems.Remove(menuItemPresentationViewModel.MenuItem);
            ObjectChangeState?.Invoke(this, "MenuItems");
        }

        public void MoveMenuItem(MenuItemPresentationViewModel menuItemPresentationViewModel, int pos)
        {
            MenuItemsPresentation.MoveMenuItem(menuItemPresentationViewModel.MenuItem, pos);
            ObjectChangeState?.Invoke(this, "MenuItems");
        }
        public void AddMenuItem(MenuItemPresentationViewModel menuItemPresentationViewModel)
        {
            if (!_MenuItems.ContainsKey(menuItemPresentationViewModel.MenuItem))
            {
                menuItemPresentationViewModel.PriceTextWidthChanged += MenuItemPresentationViewModel_PriceTextWidthChanged;
                MenuItemsPresentation.AddMenuItem(menuItemPresentationViewModel.MenuItem);
                _MenuItems[menuItemPresentationViewModel.MenuItem] = menuItemPresentationViewModel;
                ObjectChangeState?.Invoke(this, "MenuItems");
            }
        }
    }
}
