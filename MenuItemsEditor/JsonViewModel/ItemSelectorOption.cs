using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;

namespace MenuItemsEditor.JsonViewModel
{
    /// <MetaDataID>{896cff14-5a02-4b57-af05-e91d4d36ad51}</MetaDataID>
    public class ItemSelectorOption : Option, IPricingContext
    {
        public List<MenuItemPrice> MenuItemPrices { get; set; }

        public IList<ICustomizedPrice> PricedSubjects { get; set; }

        internal static ItemSelectorOption GetItemSelectorOption(MenuModel.ItemSelectorOption orgItemSelector, Dictionary<object, object> mappedObject)
        {
            if (mappedObject.ContainsKey(orgItemSelector))
                return mappedObject[orgItemSelector] as ItemSelectorOption;
            else
            {
                ItemSelectorOption itemSelector = new ItemSelectorOption();
                 
                itemSelector.MenuItemPrices = new List<MenuItemPrice>();
                itemSelector.PricedSubjects = new List<ICustomizedPrice>();
                itemSelector.Price = orgItemSelector.Price;
                mappedObject[orgItemSelector] = itemSelector;
                itemSelector.Name = orgItemSelector.Name; itemSelector.Quantitative = orgItemSelector.Quantitative;
                itemSelector.LevelType = ScaleType.GetScaleTypeFor(orgItemSelector.LevelType, mappedObject);
                  

                return itemSelector;
            }
        }

        public ICustomizedPrice GetCustomazedPrice(IPricedSubject pricedSubject)
        {
            CustomizedPrice customazedPrice = (from aCustomazedPrice in PricedSubjects.OfType< CustomizedPrice>()
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
    }
}
