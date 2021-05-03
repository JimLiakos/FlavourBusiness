using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{f5b464b8-d129-44f2-b91d-1715c9462c9e}</MetaDataID>
    public class PreparationoOptionPriceContextViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        MenuModel.IPricingContext _PricingContext;
        public readonly PreparationScaledOptionViewModel PreparationScaledOptionViewModel;
        public readonly PreparationOptionsGroupViewModel PreparationOptionsGroupViewModel;
        public readonly MenuItemViewModel MenuItemViewModel;

        readonly MenuModel.ItemSelectorOption ItemSelectorOption;
        MenuModel.IPricedSubject _PricedSubject;

        public MenuModel.IPricedSubject PricedSubject
        {
            get
            {
                return _PricedSubject;
            }
        }
        public MenuModel.IPricingContext PricingContext
        {
            get
            {
              
                return _PricingContext;
            }
        }


        public PreparationoOptionPriceContextViewModel(MenuModel.IPricingContext pricingContext, PreparationOptionViewModel pricedSubject)
        {

            _PricingContext = pricingContext;
            PreparationScaledOptionViewModel = pricedSubject as PreparationScaledOptionViewModel;
            PreparationOptionsGroupViewModel = pricedSubject as PreparationOptionsGroupViewModel;
            ItemSelectorOption = pricingContext as MenuModel.ItemSelectorOption;
            if (PreparationScaledOptionViewModel != null)
            {
                MenuItemViewModel = PreparationScaledOptionViewModel.MenuItemViewModel;
                _PricedSubject = PreparationScaledOptionViewModel.PreparationScaledOption;
            }
            if(PreparationOptionsGroupViewModel!=null)
                MenuItemViewModel = PreparationOptionsGroupViewModel.MenuItemViewModel;


            if (MenuItemViewModel != null && pricingContext is MenuModel.ItemSelectorOption)
            {
                MenuModel.ItemSelectorOption itemSelectorOption = PricingContext as MenuModel.ItemSelectorOption;
                var menuItemPrices = itemSelectorOption.MenuItemPrices;
                MenuModel.MenuItemPrice menuItemPrice = null;
                if (menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                    _PricingContext = menuItemPrice;
            }
        }

        void GetPriceContextForItemSelector()
        {
            if (MenuItemViewModel != null && PricingContext is MenuModel.ItemSelectorOption)
            {
                MenuModel.ItemSelectorOption itemSelectorOption = PricingContext as MenuModel.ItemSelectorOption;
                var menuItemPrices = itemSelectorOption.MenuItemPrices;
                MenuModel.MenuItemPrice menuItemPrice = null;
                if (!menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                {
                    menuItemPrice = new MenuModel.MenuItemPrice();
                    menuItemPrice.Price = -1;
                    menuItemPrice.Name = itemSelectorOption.Name;
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemSelectorOption).CommitTransientObjectState(menuItemPrice);
                    MenuItemViewModel.MenuItem.AddMenuItemPrice(menuItemPrice);
                    itemSelectorOption.AddMenuItemPrice(menuItemPrice);
                }
                _PricingContext = menuItemPrice;
            }
        }

        //public PreparationoOptionPriceContextViewModel(MenuModel.ItemSelectorOption itemSelectorOption, MenuItemViewModel pricedSubject)
        //{
        //    PricingContext = pricingContext;
        //    MenuItemViewModel = pricedSubject;
        //    ItemSelectorOption = itemSelectorOption;
        //    //PricedSubject = pricedSubject.MenuItem as MenuModel.MenuItem;
        //    //PreparationScaledOptionViewModel = pricedSubject as PreparationScaledOptionViewModel;
        //    //PreparationOptionsGroupViewModel = pricedSubject as PreparationOptionsGroupViewModel;
        //}

        public string Name
        {
            get
            {
                return PricingContext.Name;
            }
        }


        public decimal OverridePrice
        {
            get
            {

                if (MenuItemViewModel == null)
                    return -1;
                //if (MenuItemViewModel != null && PricingContext is MenuModel.ItemSelectorOption)
                //{
                //    MenuModel.ItemSelectorOption itemSelectorOption = PricingContext as MenuModel.ItemSelectorOption;
                //    var menuItemPrices = itemSelectorOption.MenuItemPrices;
                //    MenuModel.MenuItemPrice menuItemPrice = null;
                //    if (menuItemPrices.TryGetValue(PreparationScaledOptionViewModel.MenuItemViewModel.MenuItem, out menuItemPrice))
                //    {
                //        var customazedPrice = menuItemPrice.GetCustomazedPrice(PricedSubject);
                //        if (customazedPrice != null)
                //            return customazedPrice.Price;
                //    }
                //}
                //else
                if (PricingContext != null && PricedSubject != null)
                {
                    var customazedPrice = PricingContext.GetCustomazedPrice(PricedSubject);
                    if (customazedPrice != null)
                        return customazedPrice.Price;
                }
                return -1;
            }
            set
            {
                if (MenuItemViewModel != null)
                {
                    if (value == -1)
                    {
                        if (PricingContext != null)
                        {
                            var customazedPrice = PricingContext.GetCustomazedPrice(PricedSubject);
                            if (customazedPrice != null)
                                PricingContext.RemoveCustomazedPrice(customazedPrice);

                        }
                        //if (MenuItemViewModel != null && PricingContext is MenuModel.ItemSelectorOption)
                        //{
                           
                        //    MenuModel.ItemSelectorOption itemSelectorOption = PricingContext as MenuModel.ItemSelectorOption;
                        //    var menuItemPrices = itemSelectorOption.MenuItemPrices;
                        //    MenuModel.MenuItemPrice menuItemPrice = null;
                        //    if (menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                        //    {
                        //        var customazedPrice = menuItemPrice.GetCustomazedPrice(PricedSubject);
                        //        if (customazedPrice != null)
                        //            menuItemPrice.RemoveCustomazedPrice(customazedPrice);
                        //    }
                        //}
                        //else if (PricingContext != null && PricedSubject != null)
                        //{
                        //    var customazedPrice = PricingContext.GetCustomazedPrice(PricedSubject);
                        //    if (customazedPrice != null)
                        //        PricingContext.RemoveCustomazedPrice(customazedPrice);
                        //}
                    }
                    else
                        Price = value;
                }


            }

        }

        public decimal Price
        {
            get
            {
                if (PreparationScaledOptionViewModel == null)
                    return 0;

                //if (MenuItemViewModel != null && PricingContext is MenuModel.ItemSelectorOption)
                //{
                //    MenuModel.ItemSelectorOption itemSelectorOption = PricingContext as MenuModel.ItemSelectorOption;
                //    var menuItemPrices = itemSelectorOption.MenuItemPrices;
                //    MenuModel.MenuItemPrice menuItemPrice = null;
                //    if (menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                //    {
                //        var customazedPrice = menuItemPrice.GetCustomazedPrice(PricedSubject);
                //        if (customazedPrice != null)
                //            return customazedPrice.Price;
                //    }
                //}
                if(PricingContext!=null)
                {
                    var customazedPrice = PricingContext.GetCustomazedPrice(PricedSubject);
                    if (customazedPrice != null)
                        return customazedPrice.Price;
                }
                if(ItemSelectorOption !=null)
                {
                    var customazedPrice = ItemSelectorOption.GetCustomazedPrice(PricedSubject);
                    if (customazedPrice != null)
                        return customazedPrice.Price;
                }
                return PricedSubject.Price;
            }
            set
            {
                if (PricedSubject != null)
                {
                    if (Price != value)
                    {

                        if (PricingContext == null && ItemSelectorOption != null)
                            GetPriceContextForItemSelector();
                        PricedSubject.SetPrice(PricingContext, value);
                        //if (MenuItemViewModel != null && PricingContext is MenuModel.ItemSelectorOption)
                        //{
                        //    MenuModel.ItemSelectorOption itemSelectorOption = PricingContext as MenuModel.ItemSelectorOption;
                        //    var menuItemPrices = itemSelectorOption.MenuItemPrices;
                        //    MenuModel.MenuItemPrice menuItemPrice = null;
                        //    if (!menuItemPrices.TryGetValue(MenuItemViewModel.MenuItem, out menuItemPrice))
                        //    {
                        //        menuItemPrice = new MenuModel.MenuItemPrice();
                        //        menuItemPrice.Price = -1;
                        //        menuItemPrice.Name = itemSelectorOption.Name;
                        //        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemSelectorOption).CommitTransientObjectState(menuItemPrice);
                        //        MenuItemViewModel.MenuItem.AddMenuItemPrice(menuItemPrice);
                        //        itemSelectorOption.AddMenuItemPrice(menuItemPrice);
                        //    }
                        //    PricedSubject.SetPrice(menuItemPrice, value);

                        //}
                        //else
                        //    PricedSubject.SetPrice(PricingContext, value);
                    }
                }
            }
        }

        public decimal OrgPrice
        {
            get
            {
                if (MenuItemViewModel != null && ItemSelectorOption!=null)
                    return PricedSubject.GetPrice(ItemSelectorOption);
                    //MenuModel.ItemSelectorOption itemSelectorOption = PricingContext as MenuModel.ItemSelectorOption;
                    //var menuItemPrices = itemSelectorOption.MenuItemPrices;
                    //MenuModel.MenuItemPrice menuItemPrice = null;
                    //if (menuItemPrices.TryGetValue(PreparationScaledOptionViewModel.MenuItemViewModel.MenuItem, out menuItemPrice))
                    //{
                    //    var customazedPrice = menuItemPrice.GetCustomazedPrice(PreparationScaledOptionViewModel.PreparationScaledOption);
                    //    if (customazedPrice != null)
                    //        return customazedPrice.Price;
                    //}
                
                return PricedSubject.Price;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
