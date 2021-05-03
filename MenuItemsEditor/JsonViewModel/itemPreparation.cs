using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;

namespace MenuItemsEditor.JsonViewModel
{
    /// <MetaDataID>{dcf114f9-d0ef-4301-bec1-273571f309bd}</MetaDataID>
    public class ItemPreparation : TypedObject, IMenuItem
    {
        public ItemPreparation()
        {

        }

        public ItemPreparation(MenuModel.IMenuItem menuItem, Dictionary<object, object> mappedObject)
        {
           
            Name = menuItem.Name;
            Description = menuItem.Description;
            ExtrasDescription = menuItem.ExtrasDescription;

            ISOCurrencySymbol = RegionInfo.CurrentRegion.ISOCurrencySymbol;
            CurrencySymbol = RegionInfo.CurrentRegion.CurrencySymbol;
            Quantity = 1;
            Prices = new List<IMenuItemPrice>();

            mappedObject[menuItem] = this;
            foreach (var menuItemPrice in menuItem.Prices)
                 Prices.Add(JsonViewModel.MenuItemPrice.GetMenuItemPrice(menuItemPrice, mappedObject));
            if (menuItem.MenuItemPrice != null)
                MenuItemPrice = mappedObject[menuItem.MenuItemPrice] as IMenuItemPrice;

            ItemOptions = (from menuItemType in menuItem.Types
                       from optionGroup in menuItemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                       select new JsonViewModel.OptionGroup()
                       {
                           ItemSelectorOptionsGroup = optionGroup is MenuModel.ItemSelectorOptionsGroup,
                           CheckUncheck = optionGroup.SelectionType != MenuModel.SelectionType.SimpleGroup,
                           Name = optionGroup.Name,
                           Options = (from scaledOption in optionGroup.GroupedOptions
                                      select JsonViewModel.Option.GetOption(scaledOption, mappedObject, menuItem)).ToList()

                       }).ToList();


        }
        public string Name { get; set; }

        public List<OptionGroup> ItemOptions { set; get; }

        public IList<IMenuItemPrice> Prices { set; get; }
        public string ISOCurrencySymbol { set; get; }
        public string CurrencySymbol { set; get; }

        public decimal Quantity { set; get; }

        public string Description { set; get; }
    

        public string ExtrasDescription { set; get; }

        public IMenuItemPrice MenuItemPrice { get; set; }
 

        public IList<IOptionMenuItemSpecific> OptionsMenuItemSpecifics
        {
            get
            {
                return new List<IOptionMenuItemSpecific>();
            }
        }

     

        public IMenuItemType DedicatedType { get; set; }
     

        public IList<IMenuItemType> Types { get; set; }
       
        public void AddMenuItemPrice(IMenuItemPrice price)
        {
            throw new NotImplementedException();
        }

        public void RemoveMenuItemPrice(IMenuItemPrice price)
        {
            throw new NotImplementedException();
        }

        public void AddType(IMenuItemType type)
        {
            throw new NotImplementedException();
        }

        public void RemoveType(IMenuItemType type)
        {
            throw new NotImplementedException();
        }
    }

    /// <MetaDataID>{f47f8b44-6b40-4ed7-bfea-998bb404d21e}</MetaDataID>
    public class MenuItemPrice : TypedObject,MenuModel.IPricingContext, MenuModel.IMenuItemPrice

         
    {
        public decimal Price { set; get; }

        public ItemSelectorOption ItemSelector { set; get; }

        public string Name
        {
            get;set;
        }

        public IList<ICustomizedPrice> PricedSubjects
        {
            get;set;
        }
       public IMenuItem MenuItem { get; set; }

        public bool IsDefaultPrice { get; set; }
  

        public IList<ICustomizedPrice> PricingContexts
        {
            get
            {
                return new List<ICustomizedPrice>();
            }
        }

        public static MenuItemPrice GetMenuItemPrice(MenuModel.IMenuItemPrice orgMenutemPrice, Dictionary<object, object> mappedObject)
        {
            if (mappedObject.ContainsKey(orgMenutemPrice))
                return mappedObject[orgMenutemPrice] as MenuItemPrice;
            else
            {
                ItemPreparation itemPreparation = mappedObject[orgMenutemPrice.MenuItem] as ItemPreparation;
                MenuItemPrice menutemPrice = new MenuItemPrice();

                menutemPrice.IsDefaultPrice = orgMenutemPrice.IsDefaultPrice;
                menutemPrice.PricedSubjects = new List<ICustomizedPrice>();
                menutemPrice.MenuItem = itemPreparation;
                menutemPrice.Price = orgMenutemPrice.Price;
                mappedObject[orgMenutemPrice] = menutemPrice;
                if (orgMenutemPrice is MenuModel.MenuItemPrice && (orgMenutemPrice as MenuModel.MenuItemPrice).ItemSelector != null)
                    menutemPrice.ItemSelector = ItemSelectorOption.GetItemSelectorOption((orgMenutemPrice as MenuModel.MenuItemPrice).ItemSelector, mappedObject);
                return menutemPrice;
            }
        }

        public void RemoveCustomazedPrice(ICustomizedPrice customazedPrice)
        {
            throw new NotImplementedException();
        }

        public ICustomizedPrice GetCustomazedPrice(IPricedSubject pricedSubject)
        {
            CustomizedPrice customazedPrice = (from aCustomazedPrice in PricedSubjects.OfType<CustomizedPrice>()
                                               where aCustomazedPrice.PricedSubject == pricedSubject
                                               select aCustomazedPrice).FirstOrDefault();
            if (customazedPrice == null)
            {
                customazedPrice = new CustomizedPrice();
                customazedPrice.PricedSubject = pricedSubject;
                customazedPrice.PricingContext = this;

                PricedSubjects.Add(customazedPrice);
                pricedSubject.PricingContexts.Add(customazedPrice);
            }
            return customazedPrice;
        }

        public decimal GetPrice(IPricingContext pricicingContext)
        {
            throw new NotImplementedException();
        }

        public void SetPrice(IPricingContext pricicingContext, decimal price)
        {
            throw new NotImplementedException();
        }
    }

}
