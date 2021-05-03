using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{c4799c24-5b30-4c43-9997-8b0825cd8ae2}</MetaDataID>
    public class ItemSelectorViewModel : PreparationScaledOptionViewModel
    {
        public readonly MenuModel.ItemSelectorOption ItemSelectorOption;
        public ItemSelectorViewModel(MenuModel.IPreparationScaledOption preparationOption, IPreparationOptionsListView preparationOptionsListView, bool isEditable)
            : base(preparationOption, preparationOptionsListView, isEditable)
        {
            ItemSelectorOption = preparationOption as MenuModel.ItemSelectorOption;
        }

         

        /// <exclude>Excluded</exclude>
        event PropertyChangedEventHandler _PropertyChanged;
        public override event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                base.PropertyChanged += value;
                _PropertyChanged += value;
            }
            remove
            {
                base.PropertyChanged -= value;
                _PropertyChanged -= value;
            }
        }


        public override decimal OverridePrice
        {
            get
            {
                if (MenuItemViewModel != null)
                {
                    var menuItemPrices = ItemSelectorOption.MenuItemPrices;
                    MenuModel.MenuItemPrice menuItemPrice = null;
                    if (menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                    {
                        if (menuItemPrice.IsPriceOverridden)
                            return menuItemPrice.Price;
                    }

                }
                return -1;
            }

            set
            {
                if (MenuItemViewModel != null)
                {

                    var menuItemPrices = ItemSelectorOption.MenuItemPrices;
                    MenuModel.MenuItemPrice menuItemPrice = null;
                    if (menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                    {
                        menuItemPrice.Price = value;
                        if (value == 0)
                        {
                            menuItemPrice.MenuItem.RemoveMenuItemPrice(menuItemPrice);
                            ItemSelectorOption.RemoveMenuItemPrice(menuItemPrice);
                        }
                    }
       
                }
              
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceIsVisible)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));

                foreach (var level in Levels)
                    level.Refresh();


            }
        }
        public decimal OrgPrice
        {
            get
            {
                return ItemSelectorOption.Price;
            }
        }
        public override decimal Price
        {
            get
            {
                if (MenuItemViewModel != null)
                {
                    var menuItemPrices = ItemSelectorOption.MenuItemPrices;
                    MenuModel.MenuItemPrice menuItemPrice = null;
                    if (menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                    {
                        if(menuItemPrice.Price!=-1)
                            return menuItemPrice.Price;
                    }

                }
                return base.Price;

            }

            set
            {
                if (MenuItemViewModel != null)
                {

                    var menuItemPrices = ItemSelectorOption.MenuItemPrices;
                    MenuModel.MenuItemPrice menuItemPrice = null;
                    if (menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                    {
                        menuItemPrice.Price = value;
                        if (value == 0)
                        {
                            menuItemPrice.MenuItem.RemoveMenuItemPrice(menuItemPrice);
                            ItemSelectorOption.RemoveMenuItemPrice(menuItemPrice);
                        }
                    }
                    else
                    {
                        menuItemPrice = new MenuModel.MenuItemPrice();
                        menuItemPrice.Name = ItemSelectorOption.Name;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ItemSelectorOption).CommitTransientObjectState(menuItemPrice);
                        MenuItemViewModel.MenuItem.AddMenuItemPrice(menuItemPrice);
                        ItemSelectorOption.AddMenuItemPrice(menuItemPrice);
                        menuItemPrice.Price = value;
                    }

                }
                else
                    base.Price = value;
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceIsVisible)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridePrice)));

                foreach (var level in Levels)
                    level.Refresh();
            }
        }
    }
}
