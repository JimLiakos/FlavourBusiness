using MenuModel;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using System.Linq;
namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{ccf5778b-f8d7-4a74-9a29-6e633b64f90b}</MetaDataID>
    [BackwardCompatibilityID("{ccf5778b-f8d7-4a74-9a29-6e633b64f90b}")]
    [GenerateFacadeProxy]
    public interface IItemsPreparationInfo
    {

        /// <MetaDataID>{ede86e3a-e789-4e0e-86a3-64629ec624bc}</MetaDataID>
        ITag NewPrepatationTag();

        /// <MetaDataID>{abd4769c-e51a-4590-bc26-d22416de5b84}</MetaDataID>
        void RemovePreparationTag(ITag tag);

        [RoleAMultiplicityRange(0)]
        [Association("PreparationTag", Roles.RoleA, "9b2ce580-1a0f-4ee2-8a50-18c4dc6f49df")]
        List<ITag> PreparationTags { get; }

        /// <MetaDataID>{b6ea89d7-7bb8-4efd-be66-886628f19dbf}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        double? CookingTimeSpanInMin { get; set; }

        /// <MetaDataID>{af070276-0111-4c31-a073-48493d0a2ea8}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string ItemsInfoObjectUri { get; set; }

        /// <MetaDataID>{976b3981-7755-487f-bcbc-90752e890884}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }


        ///// <MetaDataID>{edf34b1e-b6bb-45f1-a720-9d3242bf857c}</MetaDataID>
        //[BackwardCompatibilityID("+3")]
        //bool Exclude { get; }


        /// <MetaDataID>{9296deb8-b788-4321-bda7-d76eb74d0bfc}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        double? PreparationTimeSpanInMin { get; set; }

        /// <MetaDataID>{1650b10f-bc6e-4034-ab83-fc2a947f759b}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        bool? IsCooked { get; set; }

        /// <MetaDataID>{1b78b048-2f35-4fcf-a227-1794e032c31d}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        int LearningCurveCount { get; set; }
        /// <MetaDataID>{3ce058c1-96e9-436d-b7fa-2b510bac9427}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        ItemsPreparationInfoType ItemsPreparationInfoType { get; set; }

    }

    /// <MetaDataID>{62f6ca31-ec5e-4a3a-a1c8-c124b7adc8eb}</MetaDataID>
    public enum ItemsPreparationInfoType
    {
        Include = 1,
        Exclude = 2,
        PreparationTime = 4,
        IsCooked = 8
    }

    //Extension

#if !FlavourBusinessDevice    
    /// <MetaDataID>{15225b5f-29c1-422f-a877-130c45aa7ce7}</MetaDataID>
    public static class ItemsPreparationInfoTypeExtension
    {
        /// <MetaDataID>{9d6be088-acf3-4837-838d-54d95cc7c501}</MetaDataID>
        public static bool Included(this IItemsPreparationInfo itemsPreparationInfo)
        {
            return (itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Include) == ItemsPreparationInfoType.Include;
        }

        /// <MetaDataID>{4c2454fe-eade-4cf3-8857-c0fe7704dadd}</MetaDataID>
        public static bool Excluded(this IItemsPreparationInfo itemsPreparationInfo)
        {
            return (itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Exclude) == ItemsPreparationInfoType.Exclude;
        }

        /// <MetaDataID>{c9bea570-5248-4ec6-9850-19487fc65bef}</MetaDataID>
        public static bool PreparationTime(this IItemsPreparationInfo itemsPreparationInfo)
        {
            return (itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.PreparationTime) == ItemsPreparationInfoType.PreparationTime;
        }


        /// <MetaDataID>{363235af-9772-4e3b-9c43-31d48fdf75cd}</MetaDataID>
        public static List<IItemsPreparationInfo> GetItemsPreparationInfo(this IPreparationStation preparationStation, IMenuItem menuItem)
        {
            List<IItemsPreparationInfo> itemsPreparationInfoHierarchy = new List<IItemsPreparationInfo>();

            var itemsPreparationInfos = (from itemsInfo in preparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                {
                    itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
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
                        itemsPreparationInfoHierarchy.AddRange(preparationStation.GetItemsPreparationInfo(itemsCategory));
                    }
                }
            }
            return itemsPreparationInfoHierarchy;
        }

        /// <MetaDataID>{abcf3581-0079-4d07-8429-f8779b711a94}</MetaDataID>
        public static bool IsCooked(this IPreparationStation preparationStation, IMenuItem menuItem)
        {
            var itemsPreparationInfos = preparationStation.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.IsCooked != null)
                    return itemsPreparationInfo.IsCooked.Value;
            }
            return false;
        }

        /// <MetaDataID>{b3200fb3-30f9-492c-afab-de4c31bcd06c}</MetaDataID>
        public static List<IItemsPreparationInfo> GetItemsPreparationInfo(this IPreparationStation preparationStation, IItemsCategory itemsCategory)
        {
            List<IItemsPreparationInfo> itemsPreparationInfoHierarchy = new List<IItemsPreparationInfo>();
            var itemsPreparationInfos = (from itemsInfo in preparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
                {
                    if (preparationStation.StationPrepareItems(itemsPreparationInfoEntry.@object as IItemsCategory))
                        itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
                }
            }
            itemsCategory = itemsCategory.Parent;
            while (itemsCategory != null)
            {
                foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
                {
                    if (itemsPreparationInfoEntry.@object == itemsCategory && preparationStation.StationPrepareItems(itemsCategory))
                        itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
                }
                itemsCategory = itemsCategory.Parent;
            }


            return itemsPreparationInfoHierarchy;
        }


        /// <MetaDataID>{112fd3c1-e25f-4e72-9fef-db94ea560622}</MetaDataID>
        public static bool StationPrepareItems(this IPreparationStation preparationStation, IItemsCategory itemsCategory)
        {


            var itemsPreparationInfos = (from itemsInfo in preparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
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

            foreach (var itemsPreparationInfo in preparationStation.ItemsPreparationInfos)
            {
                MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
                var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
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
        public static bool StationPrepareItem(this IPreparationStation preparationStation, IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in preparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
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

                        if (preparationStation.StationPrepareItems(itemsCategory))
                            return true;
                        else
                            return false;

                        //while (itemsCategory != null && itemsCategory != itemsPreparationInfoCategory)
                        //    itemsCategory = itemsCategory.Class as MenuModel.IItemsCategory;

                        //if (itemsCategory == itemsPreparationInfoCategory)
                        //    return true;
                    }
                }
            }
            return false;

        }



    }

#endif
}