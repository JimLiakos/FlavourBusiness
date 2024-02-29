using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessFacade.PriceList
{
    /// <MetaDataID>{7086923d-a949-4e1e-9bec-bbbc0430f181}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{7086923d-a949-4e1e-9bec-bbbc0430f181}")]
    public interface IItemsPriceInfo
    {
        /// <MetaDataID>{672fa328-3c4c-4a35-bd8b-3bc3c97409d1}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        double? OverridenPrice { get; set; }

        /// <MetaDataID>{e3d5df84-1222-4050-8576-c05471857bf0}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        double? AmountDiscount { get; set; }

        /// <MetaDataID>{ce678a17-0d59-4d88-89d4-35cc9de06524}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        double? Pricerounding { get; set; }

        /// <MetaDataID>{2b732863-faa1-4024-8bfb-5abff5af891d}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        double? PercentageDiscount { get; set; }

        /// <MetaDataID>{5767c6cb-c6d3-44c1-bd5f-96b7fc3cc395}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }

        /// <MetaDataID>{58e165c5-e9af-4c4d-854e-96cd6b6c145c}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        IClassified MenuModelObject { get; }

        /// <MetaDataID>{3bb37c90-f505-4f75-90e1-484a9cedf2b7}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        ItemsPriceInfoType ItemsPriceInfoType { get; set; }

        /// <MetaDataID>{60ff5e9f-feab-4c74-98ae-7de6f348e305}</MetaDataID>
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
        public static bool Included(this IItemsPriceInfo itemsPriceInfo)
        {
            return (itemsPriceInfo.ItemsPriceInfoType & ItemsPriceInfoType.Include) == ItemsPriceInfoType.Include;
        }

        /// <MetaDataID>{4c2454fe-eade-4cf3-8857-c0fe7704dadd}</MetaDataID>
        public static bool Excluded(this IItemsPriceInfo itemsPriceInfo)
        {
            return (itemsPriceInfo.ItemsPriceInfoType & ItemsPriceInfoType.Exclude) == ItemsPriceInfoType.Exclude;
         
        }

    


        /// <MetaDataID>{363235af-9772-4e3b-9c43-31d48fdf75cd}</MetaDataID>
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
            System.Collections.Generic.List<IItemsPriceInfo> itemsPreparationInfoHierarchy = new  System.Collections.Generic.List<IItemsPriceInfo>();
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
            itemsCategory = itemsCategory.Parent;
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
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
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
                        return true;

                }
            }
            return false;
        }


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
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Included())
                        return true;
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.Excluded())
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
                        else
                            return false;

             
                    }
                }
            }
            return false;

        }



    }


}