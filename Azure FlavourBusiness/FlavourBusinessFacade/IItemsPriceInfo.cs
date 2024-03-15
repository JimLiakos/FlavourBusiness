using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FlavourBusinessFacade.PriceList
{
    /// <MetaDataID>{7086923d-a949-4e1e-9bec-bbbc0430f181}</MetaDataID>
    [BackwardCompatibilityID("{7086923d-a949-4e1e-9bec-bbbc0430f181}")]
    public interface IItemsPriceInfo
    {
        /// <MetaDataID>{32b87239-ef84-4483-ad05-7d0a7f28e9cd}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        double? OptionsPricesRounding { get; set; }
        /// <MetaDataID>{f1eb4d26-ffe6-4065-a42c-2b772398909b}</MetaDataID>
        bool? IsOptionsPricesDiscountEnabled { get; set; }

        /// <MetaDataID>{672fa328-3c4c-4a35-bd8b-3bc3c97409d1}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        decimal? OverridenPrice { get; set; }


        /// <MetaDataID>{e3d5df84-1222-4050-8576-c05471857bf0}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        double? AmountDiscount { get; set; }

        /// <MetaDataID>{ce678a17-0d59-4d88-89d4-35cc9de06524}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double? Pricerounding { get; set; }

        /// <MetaDataID>{2b732863-faa1-4024-8bfb-5abff5af891d}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        double? PercentageDiscount { get; set; }

        /// <MetaDataID>{5767c6cb-c6d3-44c1-bd5f-96b7fc3cc395}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string Description { get; set; }

        /// <MetaDataID>{58e165c5-e9af-4c4d-854e-96cd6b6c145c}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        object MenuModelObject { get; }

        /// <MetaDataID>{3bb37c90-f505-4f75-90e1-484a9cedf2b7}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        ItemsPriceInfoType ItemsPriceInfoType { get; set; }

        /// <MetaDataID>{60ff5e9f-feab-4c74-98ae-7de6f348e305}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        string ItemsInfoObjectUri { get; set; }
    }


    /// <MetaDataID>{69317d8d-a74c-4983-b880-d2fee973f01f}</MetaDataID>
    public enum ItemsPriceInfoType
    {
        Include = 1,
        Exclude = 2,
    }


    /// <MetaDataID>{15225b5f-29c1-422f-a877-130c45aa7ce7}</MetaDataID>
    public static class ItemsPriceInfoTypeExtension
    {
        /// <MetaDataID>{9d6be088-acf3-4837-838d-54d95cc7c501}</MetaDataID>
        public static bool IsIncluded(this IItemsPriceInfo itemsPriceInfo)
        {
            return (itemsPriceInfo.ItemsPriceInfoType & ItemsPriceInfoType.Include) == ItemsPriceInfoType.Include;
        }

        /// <MetaDataID>{4c2454fe-eade-4cf3-8857-c0fe7704dadd}</MetaDataID>
        public static bool IsExcluded(this IItemsPriceInfo itemsPriceInfo)
        {
            return (itemsPriceInfo.ItemsPriceInfoType & ItemsPriceInfoType.Exclude) == ItemsPriceInfoType.Exclude;
        }


        
        public static bool IsIncluded(this IItemsTaxInfo itemsTaxInfo)
        {
            return (itemsTaxInfo.ItemsPriceInfoType & ItemsPriceInfoType.Include) == ItemsPriceInfoType.Include;
        }

        
        public static bool IsExcluded(this IItemsTaxInfo itemsTaxInfo)
        {
            return (itemsTaxInfo.ItemsPriceInfoType & ItemsPriceInfoType.Exclude) == ItemsPriceInfoType.Exclude;
        }

        public static IItemsPriceInfo GetItemPriceInfo(this IPriceList priceList, MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == menuItem
                                        select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                return itemsPreparationInfo;
            }
            else
            {
                return null;
            }
        }

        public static IItemsPriceInfo GetItemPriceInfo(this IPriceList priceList, object priceListSubject)
        {

            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == priceListSubject
                                        select itemsInfoEntry.ItemsPriceInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
                return itemsPreparationInfo;
            else
                return null;
        }



        /// <MetaDataID>{363235af-9772-4e3b-9c43-31d48fdf75cd}</MetaDataID>
        public static System.Collections.Generic.List<IItemsPriceInfo> GetItemsPriceInfo(this IPriceList priceList, object priceListSubject)
        {

            if (priceListSubject is IMenuItemPrice)
            {

            }
            System.Collections.Generic.List<IItemsPriceInfo> itemsPreparationInfoHierarchy = new System.Collections.Generic.List<IItemsPriceInfo>();

            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         });// ;//.ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is IMenuItem && itemsPreparationInfoEntry.@object == priceListSubject)
                {
                    itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPriceInfo);
                    break;
                }
                if (priceListSubject is IMenuItemPrice && itemsPreparationInfoEntry.@object == (priceListSubject as IMenuItemPrice).MenuItem)
                {
                    itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPriceInfo);
                    break;
                }

            }

            IClassified classifiedObject = priceListSubject as IClassified;
            if (priceListSubject is IMenuItemPrice)
                classifiedObject = (priceListSubject as IMenuItemPrice).MenuItem as IClassified;

            if (classifiedObject is IClassified)
            {
                foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
                {
                    if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                    {
                        IItemsCategory itemsCategory = null;
                        var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                        itemsCategory = (classifiedObject as MenuModel.IClassified).Class as IItemsCategory;
                        itemsPreparationInfoHierarchy.AddRange((System.Collections.Generic.IEnumerable<IItemsPriceInfo>)priceList.GetItemsPriceInfo(itemsCategory));
                    }
                }
            }
            return itemsPreparationInfoHierarchy;
        }

        public static System.Collections.Generic.List<IItemsPriceInfo> GetItemsPriceInfo(this IPriceList priceList, IMenuItem menuItem)
        {
            System.Collections.Generic.List<IItemsPriceInfo> itemsPreparationInfoHierarchy = new System.Collections.Generic.List<IItemsPriceInfo>();

            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPriceInfo = itemsInfo
                                         });// ;//.ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                {
                    itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPriceInfo);
                    break;
                }

            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItem is IClassified)
                    {
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as IItemsCategory;
                        itemsPreparationInfoHierarchy.AddRange((System.Collections.Generic.IEnumerable<IItemsPriceInfo>)priceList.GetItemsPriceInfo(itemsCategory));
                    }
                }
            }
            return itemsPreparationInfoHierarchy;
        }


        /// <MetaDataID>{b3200fb3-30f9-492c-afab-de4c31bcd06c}</MetaDataID>
        public static System.Collections.Generic.List<IItemsPriceInfo> GetItemsPriceInfo(this IPriceList priceList, IItemsCategory itemsCategory)
        {
            System.Collections.Generic.List<IItemsPriceInfo> itemsPreparationInfoHierarchy = new System.Collections.Generic.List<IItemsPriceInfo>();
            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
                {
                    if (priceList.HasOverriddenPrice(itemsPreparationInfoEntry.@object as IItemsCategory))
                        itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
                }
            }

            while (itemsCategory != null)
            {
                foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
                {
                    if (itemsPreparationInfoEntry.@object == itemsCategory && priceList.HasOverriddenPrice(itemsCategory))
                        itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
                }
                itemsCategory = itemsCategory.Parent;
            }


            return itemsPreparationInfoHierarchy;
        }

        /// <MetaDataID>{d6d430fc-fb4c-419b-8334-d90442ad0c87}</MetaDataID>
        public static decimal RoundPriceToNearest(this IPriceList priceList, decimal price, double nearTo)
        {
            if (nearTo == 0 || nearTo > 1)
                return price;

            return (decimal)(Math.Round((double)price * (1 / nearTo)) / (1 / nearTo));

        }


        public static bool HasOverriddenTaxes(this IPriceList priceList, IItemsCategory itemsCategory)
        {


            var itemsTaxesInfos = (from itemsInfo in priceList.ItemsTaxes
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsTaxesInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsTaxesInfoEntry in itemsTaxesInfos)
            {
                if (itemsTaxesInfoEntry.@object is MenuModel.IItemsCategory && (itemsTaxesInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
                {
                    if (itemsTaxesInfoEntry.ItemsTaxesInfo.IsIncluded())
                        return true;
                    else
                        return false;

                }
            }

            foreach (var itemsTaxInfo in priceList.ItemsTaxes)
            {
                MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
                var @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsTaxInfo as System.MarshalByRefObject) ? itemsTaxInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsTaxInfo.ItemsInfoObjectUri);
                if (@object is MenuModel.IItemsCategory)
                {
                    var itemsTaxInfoCategory = (@object as MenuModel.IItemsCategory);
                    while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsTaxInfoCategory)
                        itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;

                    if (itemsCategoryOrParent == itemsTaxInfoCategory)
                    {
                        if (itemsTaxInfo.IsExcluded())
                            return false;
                        if (itemsTaxInfo.IsIncluded())
                            return true;
                    }

                }
            }

            return false;
        }

        public static bool HasOverriddenTaxes(this IPriceList priceList, IMenuItem menuItem)
        {
            var itemsTaxesInfos = (from itemsInfo in priceList.ItemsTaxes
                                   select new
                                   {
                                       ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                       @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                       ItemsTaxesInfo = itemsInfo
                                   }).ToList();



            foreach (var itemsTaxInfoEntry in itemsTaxesInfos)
            {
                if (itemsTaxInfoEntry.@object is MenuModel.IMenuItem && (itemsTaxInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                {
                    if (itemsTaxInfoEntry.ItemsTaxesInfo.IsIncluded())
                        return true;
                    if (itemsTaxInfoEntry.ItemsTaxesInfo.IsExcluded())
                        return false;

                }
            }

            foreach (var itemsTaxInfoEntry in itemsTaxesInfos)
            {
                if (itemsTaxInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    MenuModel.IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsTaxInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItem is MenuModel.IClassified)
                    {
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as IItemsCategory;

                        if (priceList.HasOverriddenTaxes(itemsCategory))
                            return true;
                        return false;
                    }
                }
            }

            return false;
        }

        /// <MetaDataID>{112fd3c1-e25f-4e72-9fef-db94ea560622}</MetaDataID>
        public static bool HasOverriddenPrice(this IPriceList priceList, IItemsCategory itemsCategory)
        {


            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.IsIncluded())
                        return true;
                    else
                        return false;

                }
            }

            foreach (var itemsPreparationInfo in priceList.ItemsPrices)
            {
                MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
                var @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsPreparationInfo as System.MarshalByRefObject) ? itemsPreparationInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
                //var @object = itemsPreparationInfo.MenuModelObject;// OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
                if (@object is MenuModel.IItemsCategory)
                {
                    var itemsPreparationInfoCategory = (@object as MenuModel.IItemsCategory);
                    while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsPreparationInfoCategory)
                        itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;

                    if (itemsCategoryOrParent == itemsPreparationInfoCategory)
                    {
                        if (itemsPreparationInfo.IsExcluded())
                            return false;
                        if (itemsPreparationInfo.IsIncluded())
                            return true;

                    }

                }
            }
            if (priceList.PriceListMainItemsPriceInfo.PercentageDiscount != null)
                return true;

            if (priceList.PriceListMainItemsPriceInfo.AmountDiscount != null)
                return true;

            return false;
        }




        public static bool HasOptionWithPrice(this IPreparationOptionsGroup optionGroup, IMenuItem menuItem)
        {
            return optionGroup.GroupedOptions.Any(x => x.HasOptionWithPrice(menuItem));
        }

        public static bool HasOptionWithPrice(this IPreparationScaledOption option, IMenuItem menuItem)
        {
            if (option is IPricingContext)
                return false;
            else
                return true;
        }


        //public static double? GetPercentageDiscount(this IPriceList priceList, IItemsCategory itemsCategory)
        //{
        //    itemsCategory
        //}
        //public static double? GetPercentageDiscount(this IPriceList priceList, IMenuItem menuItem)
        //{

        //}

        //public static void SetPercentageDiscount(this IPriceList priceList, IItemsCategory itemsCategory, double percentageDiscount)
        //{

        //}
        //public static void SetPercentageDiscount(this IPriceList priceList, IMenuItem menuItem, double percentageDiscount)
        //{

        //}
        /// <MetaDataID>{63fd57ce-f588-45fe-86fd-7625ad1ead3e}</MetaDataID>
        public static bool HasOverriddenPrice(this IPriceList priceList, IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();


            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.IsIncluded())
                        return true;
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.IsExcluded())
                        return false;

                }
            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    MenuModel.IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItem is MenuModel.IClassified)
                    {
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as IItemsCategory;

                        if (priceList.HasOverriddenPrice(itemsCategory))
                            return true;



                        return false;


                    }
                }
            }
            if (priceList.PriceListMainItemsPriceInfo.PercentageDiscount != null)
                return true;
            if (priceList.PriceListMainItemsPriceInfo.AmountDiscount != null)
                return true;
            return false;

        }


        public static bool HasOverriddenPrice(this IPriceList priceList, IMenuItemPrice menuItemPrice)
        {

            var itemsPreparationInfos = (from itemsInfo in priceList.ItemsPrices
                                         select new
                                         {
                                             @object = !OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(itemsInfo as System.MarshalByRefObject) ? itemsInfo.MenuModelObject : OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();


            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItemPrice && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItemPrice) == menuItemPrice)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.IsIncluded())
                        return true;
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.IsExcluded())
                        return false;
                }
            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem)
                {
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.IsIncluded())
                        return true;
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.IsExcluded())
                        return false;

                }
            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    MenuModel.IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItemPrice.MenuItem is MenuModel.IClassified)
                    {
                        itemsCategory = (menuItemPrice.MenuItem as MenuModel.IClassified).Class as IItemsCategory;

                        if (priceList.HasOverriddenPrice(itemsCategory))
                            return true;



                        return false;


                    }
                }
            }
            if (priceList.PriceListMainItemsPriceInfo.PercentageDiscount != null)
                return true;
            if (priceList.PriceListMainItemsPriceInfo.AmountDiscount != null)
                return true;
            return false;

        }

    }


}