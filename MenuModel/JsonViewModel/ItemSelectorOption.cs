using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{896cff14-5a02-4b57-af05-e91d4d36ad51}</MetaDataID>
    public class ItemSelectorOption : Option, IPricingContext
    {

        public ItemSelectorOption()
        {

        }

        public ItemSelectorOption(IPreparationScaledOption orgItemSelector) : base(orgItemSelector)
        {

            Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(orgItemSelector).GetPersistentObjectUri(orgItemSelector);
            MenuItemPrices = new List<MenuItemPrice>();
            PricedSubjects = new List<ICustomizedPrice>();
            Price = orgItemSelector.Price;
            _Name = new OOAdvantech.Multilingual(orgItemSelector.MultilingualName);
            Quantitative = orgItemSelector.Quantitative;
            InitialLevelIndex = orgItemSelector.LevelType.Levels.IndexOf(orgItemSelector.Initial);// (orgOption.GetInitialFor(menuItem));

        }

        /// <MetaDataID>{4550b279-f8a8-4681-9d2c-00b488423121}</MetaDataID>
        public List<MenuItemPrice> MenuItemPrices { get; set; }

        /// <MetaDataID>{fced13c3-bb48-4c5c-900e-1eaaacd29e45}</MetaDataID>
        public IList<ICustomizedPrice> PricedSubjects { get; set; }


#if MenuModel
        /// <MetaDataID>{e311bd12-5c64-43d8-9774-5d4b0090b2e4}</MetaDataID>
        internal static ItemSelectorOption GetItemSelectorOption(MenuModel.ItemSelectorOption orgItemSelector, Dictionary<object, object> mappedObject)
        {
            if (mappedObject.ContainsKey(orgItemSelector))
                return mappedObject[orgItemSelector] as ItemSelectorOption;
            else
            {
                ItemSelectorOption itemSelector = new ItemSelectorOption(orgItemSelector);

                mappedObject[orgItemSelector] = itemSelector;

                itemSelector.LevelType = ScaleType.GetScaleTypeFor(orgItemSelector.LevelType, mappedObject);


                return itemSelector;
            }
        }
#endif

        /// <MetaDataID>{77bb30e5-8016-4524-8ca7-8ae02672f9f9}</MetaDataID>
        public ICustomizedPrice GetCustomizedPrice(IPricedSubject pricedSubject)
        {
            CustomizedPrice customizedPrice = (from aCustomazedPrice in PricedSubjects.OfType<CustomizedPrice>()
                                               where aCustomazedPrice.PricedSubject == pricedSubject
                                               select aCustomazedPrice).FirstOrDefault();
            if (customizedPrice == null)
            {
                customizedPrice = new CustomizedPrice();
                customizedPrice.PricedSubject = pricedSubject;
                customizedPrice.PricingContext = this;

                PricedSubjects.Add(customizedPrice);
                pricedSubject.PricingContexts.Add(customizedPrice);
            }
            return customizedPrice;
        }

        public decimal GetDeafultPrice(IPricedSubject pricedSubject)
        {
            return pricedSubject.Price;
        }
    }


}
