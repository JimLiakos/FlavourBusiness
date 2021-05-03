using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;

namespace MenuItemsEditor.JsonViewModel
{
    /// <MetaDataID>{2cdd4709-a7f8-4189-82f1-8e5b23765be1}</MetaDataID>
    public class Option : TypedObject,MenuModel.IPricedSubject
    {
        public bool Quantitative { get; set; }
        public string Name { get; set; }
        public ScaleType LevelType { get; set; }
        public int InitialLevelIndex { get; set; }

        public decimal Price { get; set; }

        public IList<ICustomizedPrice> PricingContexts
        {
            get; set;
        }

        internal static Option GetOption(MenuModel.IPreparationScaledOption orgOption, Dictionary<object, object> mappedObject, IMenuItem menuItem)
        {
            if (mappedObject.ContainsKey(orgOption))
            {
                if (orgOption is MenuModel.ItemSelectorOption)
                {
                    JsonViewModel.ItemSelectorOption itemSelectorOption = mappedObject[orgOption] as JsonViewModel.ItemSelectorOption;
                    foreach (var orgMenuItemPriceEntry in (orgOption as MenuModel.ItemSelectorOption).MenuItemPrices)
                    {
                        JsonViewModel.MenuItemPrice menuItemPrice = null;
                        if (mappedObject.ContainsKey(orgMenuItemPriceEntry.Key) && mappedObject.ContainsKey(orgMenuItemPriceEntry.Value))
                        {
                            menuItemPrice = mappedObject[orgMenuItemPriceEntry.Value] as JsonViewModel.MenuItemPrice;
                            itemSelectorOption.MenuItemPrices.Add(menuItemPrice);
                        }
                    }
                    itemSelectorOption.InitialLevelIndex = orgOption.LevelType.Levels.IndexOf(orgOption.GetInitialFor(menuItem));
                    return itemSelectorOption;
                }
                else if (orgOption is MenuModel.IPreparationScaledOption)
                {
                    JsonViewModel.Option option = mappedObject[orgOption] as Option;
                    option.InitialLevelIndex = orgOption.LevelType.Levels.IndexOf(orgOption.GetInitialFor(menuItem));
                    foreach (var customPrice in orgOption.PricingContexts)
                    {

                    }
                    return option;
                }
                else return mappedObject[orgOption] as Option;
            }
            else
            {
                Option option = new Option();
                option.PricingContexts = new List<ICustomizedPrice>();
                option.Price = orgOption.Price;
                mappedObject[orgOption] = option;
                option.Name = orgOption.Name; option.Quantitative = orgOption.Quantitative;
                option.LevelType = ScaleType.GetScaleTypeFor(orgOption.LevelType, mappedObject);

                option.InitialLevelIndex = orgOption.LevelType.Levels.IndexOf(orgOption.GetInitialFor(menuItem));
                foreach (var customPrice in orgOption.PricingContexts)
                {
                    IPricingContext jsonPricingContext = mappedObject[customPrice.PricingContext] as IPricingContext;
                    IPricedSubject jsonPricedSubject = option;
                    ICustomizedPrice customazedPrice = jsonPricingContext.GetCustomazedPrice(jsonPricedSubject); 
                    customazedPrice.Price = customPrice.Price;
                }


                return option;
            }
        }

        public decimal GetPrice(IPricingContext pricicingContext)
        {
            throw new NotImplementedException();
        }

        public void SetPrice(IPricingContext pricicingContext, decimal price)
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomazedPrice(ICustomizedPrice customazedPrice)
        {
            throw new NotImplementedException();
        }
    }


    /// <MetaDataID>{a84587dd-3413-410a-af29-73797fda7dd7}</MetaDataID>
    public class ScaleType
    {
        public string Name { get; set; }

        static Dictionary<MenuModel.IScaleType, JsonViewModel.ScaleType> jsonScaleTypeDictionary = new Dictionary<MenuModel.IScaleType, ScaleType>();


        public ScaleType(MenuModel.IScaleType scaleType)
        {
            Levels = (from level in scaleType.Levels select new Level() { Name = level.Name, UncheckOption = level.UncheckOption }).ToList();
        }
        public List<Level> Levels = new List<Level>();

        internal static ScaleType GetScaleTypeFor(MenuModel.IScaleType scaleType)
        {
            ScaleType jsonScaleType = null;
            if (!jsonScaleTypeDictionary.TryGetValue(scaleType, out jsonScaleType))
            {
                jsonScaleType = new ScaleType(scaleType);
                jsonScaleTypeDictionary[scaleType] = jsonScaleType;
            }
            return jsonScaleType;
        }

        internal static ScaleType GetScaleTypeFor(MenuModel.IScaleType scaleType, Dictionary<object, object> mappedObject)
        {
            ScaleType jsonScaleType = null;

            if (!mappedObject.ContainsKey(scaleType))
            {
                jsonScaleType = new ScaleType(scaleType);
                mappedObject[scaleType] = jsonScaleType;
                return jsonScaleType;
            }
            else
                return mappedObject[scaleType] as ScaleType;

        }
    }

    /// <MetaDataID>{07988325-98d9-4392-9135-01a0aee1af00}</MetaDataID>
    public class Level
    {
        public string Name { get; set; }

        public bool UncheckOption { get; set; }
    }
}
