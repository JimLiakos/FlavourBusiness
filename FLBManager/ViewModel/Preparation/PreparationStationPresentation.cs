using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using MenuItemsEditor.ViewModel;
using MenuModel;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech.Transactions;
using StyleableWindow;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Preparation
{
    /// <MetaDataID>{8c2727a3-6d5f-4ad3-b771-0cfd38a663a8}</MetaDataID>
    public class PreparationStationPresentation : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget
    {

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{49b5c4dc-49d7-467f-8ea3-71f736302ce1}</MetaDataID>
        internal void ExcludeServicePoint(IServicePoint servicePoint)
        {

            if (StationPreparesForServicePoint(servicePoint))
            {

                var includedPreparationForInfo = (from preparationForInfo in PreparationForInfos
                                                  where preparationForInfo.ServicePoint == servicePoint
                                                  select preparationForInfo).FirstOrDefault();

                if (includedPreparationForInfo != null)
                {
                    this.PreparationStation.RemovePreparationForInfo(includedPreparationForInfo);
                    PreparationForInfos.Remove(includedPreparationForInfo);
                }
                if (StationPreparesForServicePoint(servicePoint))
                {

                    var preparationForInfo = this.PreparationStation.NewServicePointPreparationForInfo(servicePoint, PreparationForInfoType.Exclude);
                    this.PreparationForInfos.Add(preparationForInfo);
                }

                if (PreparationStationItems != null)
                    PreparationStationItems.Refresh();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }

        /// <MetaDataID>{aa0a5218-1aa5-44c1-9817-fe43628a60a9}</MetaDataID>
        internal void IncludeServicePoint(IServicePoint servicePoint)
        {

            var excludedpreparationForInfo = (from preparationForInfo in this.PreparationForInfos
                                              where preparationForInfo.ServicePoint == servicePoint && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Exclude
                                              select preparationForInfo).FirstOrDefault();

            if (excludedpreparationForInfo != null)
            {
                PreparationStation.RemovePreparationForInfo(excludedpreparationForInfo);
                PreparationForInfos.Remove(excludedpreparationForInfo);
            }

            if (!StationPreparesForServicePoint(servicePoint))
            {
                var preparationForInfo = this.PreparationStation.NewServicePointPreparationForInfo(servicePoint, PreparationForInfoType.Include);
                this.PreparationForInfos.Add(preparationForInfo);
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{df0d9a71-473e-4cae-a73d-ff4ab506dbd6}</MetaDataID>
        internal void ExcludeServicePoints(IServiceArea serviceArea)
        {
            var preparationForInfo = (from a_preparationForInfo in this.PreparationForInfos
                                      where a_preparationForInfo.ServiceArea == serviceArea
                                      select a_preparationForInfo).FirstOrDefault();
            if (preparationForInfo != null)
            {
                PreparationStation.RemovePreparationForInfo(preparationForInfo);
                PreparationForInfos.Remove(preparationForInfo);

                if ((from a_preparationForInfo in this.PreparationForInfos
                     where a_preparationForInfo.PreparationForInfoType == PreparationForInfoType.Include
                     select a_preparationForInfo).FirstOrDefault() == null)
                {
                    foreach (var servicePointpreparationForInfo in PreparationForInfos.ToList())
                    {
                        PreparationStation.RemovePreparationForInfo(servicePointpreparationForInfo);
                        PreparationForInfos.Remove(servicePointpreparationForInfo);
                    }
                }

                if (PreparationForInfos.Where(x => x.PreparationForInfoType == PreparationForInfoType.Include).FirstOrDefault() == null)
                {
                    var servicePointsPreparationInfoPresentation = _PreparationStationSubjects.OfType<ServicePointsPreparationInfoPresentation>().Where(x => x.ServiceArea == serviceArea).FirstOrDefault();
                    if (servicePointsPreparationInfoPresentation != null)
                    {
                        _PreparationStationSubjects.Remove(servicePointsPreparationInfoPresentation);
                        _PreparationStationSubjects = _PreparationStationSubjects.ToList();
                        RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationStationSubjects)));
                    }
                }
            }
        }

        /// <MetaDataID>{ded50a28-fddc-4ad2-bbdd-0611274f5ff3}</MetaDataID>
        internal void IncludeServicePoints(IServiceArea serviceArea)
        {
            var preparationForInfo = (from a_preparationForInfo in this.PreparationForInfos
                                      where a_preparationForInfo.ServiceArea == serviceArea
                                      select a_preparationForInfo).FirstOrDefault();
            if (preparationForInfo != null)
            {
                PreparationStation.RemovePreparationForInfo(preparationForInfo);
                PreparationForInfos.Remove(preparationForInfo);
            }
            PreparationStation.NewServiceAreaPreparationForInfo(serviceArea, PreparationForInfoType.Include);
        }

        internal void ClearItemsPreparationInfo(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == itemsCategory
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (includedItemsPreparationInfo != null)
            {
                this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);

                ClearSubCategoryItemsPreparationInfo(itemsCategory);

             


            }
        }

        private void ClearSubCategoryItemsPreparationInfo(IItemsCategory itemsCategory)
        {
            foreach (var menuitem in itemsCategory.MenuItems)
            {
                if (!PreparationStation.StationPrepareItem(menuitem))
                    ClearItemsPreparationInfo(menuitem);
            }


            foreach (var subCategory in itemsCategory.SubCategories)
            {
                if (!PreparationStation.StationPrepareItems(subCategory))
                    ClearSubCategoryItemsPreparationInfo(subCategory);
            }
        }


        /// <MetaDataID>{a517dbc7-ab28-48cd-b5a0-a837abed283f}</MetaDataID>
        internal double GetPreparationTimeSpanInMin_old(IMenuItem menuItem)
        {
            //var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
            //                             select new
            //                             {
            //                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
            //                                 ItemsPreparationInfo = itemsInfo
            //                             }).ToList();

            //foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            //{
            //    if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
            //        return itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTimeSpanInMin;

            //}

            //foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            //{
            //    if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
            //    {
            //        MenuModel.IItemsCategory itemsCategory = null;
            //        var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
            //        if (menuItem is MenuModel.IClassified)
            //        {
            //            itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;
            //            return GetPreparationTimeSpanInMin(itemsCategory);
            //        }
            //    }
            //}
            return 1;
        }

        internal void ClearItemsPreparationInfo(IMenuItem menuItem)
        {
            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == menuItem
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (includedItemsPreparationInfo != null)
            {
                this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);
                
            }
        }


        /// <MetaDataID>{220670a2-1825-44a5-9f02-4e7606ca5cad}</MetaDataID>
        internal double GetPreparationTimeSpanInMin_old(IItemsCategory itemsCategory)
        {

            //var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
            //                             select new
            //                             {
            //                                 ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
            //                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
            //                                 ItemsPreparationInfo = itemsInfo
            //                             }).ToList();

            //foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            //{
            //    if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
            //    {
            //        if (itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTime())
            //            return itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTimeSpanInMin;
            //    }
            //}

            //foreach (var itemsPreparationInfo in PreparationStation.ItemsPreparationInfos)
            //{
            //    MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
            //    var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
            //    if (@object is MenuModel.IItemsCategory)
            //    {
            //        var itemsPreparationInfoCategory = (@object as MenuModel.IItemsCategory);
            //        while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsPreparationInfoCategory)
            //            itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;
            //        if (itemsCategoryOrParent == itemsPreparationInfoCategory && itemsPreparationInfo.PreparationTime())
            //            return itemsPreparationInfo.PreparationTimeSpanInMin;
            //    }
            //}
            return 1;
        }

        /// <MetaDataID>{6b9f91bf-5517-41e8-917f-d69e56fda1ce}</MetaDataID>
        public void SetPreparationTimeSpanInMin_old(MenuModel.IItemsCategory itemsCategory, double timeSpanInMinutes)
        {
            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == itemsCategory && (itemsInfoEntry.ItemsPreparationInfo.Included() || itemsInfoEntry.ItemsPreparationInfo.PreparationTime())
                                        select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                itemsPreparationInfo.ItemsPreparationInfoType |= ItemsPreparationInfoType.PreparationTime;
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
            }
            else
            {


                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.PreparationTime);
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
                //this.AddItemsPreparationInfos(itemsPreparationInfo);
            }
            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            // updates infrastacture vew 

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));



        }


        /// <MetaDataID>{006e4e7a-323a-4fe4-a482-2686e3161a60}</MetaDataID>
        public void SetPreparationTimeSpanInMin_old(MenuModel.IMenuItem menuItem, double timeSpanInMinutes)
        {

            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == menuItem
                                        select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                itemsPreparationInfo.ItemsPreparationInfoType |= ItemsPreparationInfoType.PreparationTime;
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
            }
            else
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

                itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.PreparationTime);
                itemsPreparationInfo.PreparationTimeSpanInMin = timeSpanInMinutes;
                //this.AddItemsPreparationInfos(itemsPreparationInfo);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();
            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }


        /// <MetaDataID>{98413614-4726-4b54-856e-c02e67c640e1}</MetaDataID>
        internal double GetPreparationTimeSpanInMin(IMenuItem menuItem)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);


            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                var des = itemsPreparationInfo.Description;
                if (itemsPreparationInfo.PreparationTimeSpanInMin != null)
                    return itemsPreparationInfo.PreparationTimeSpanInMin.Value;
            }
            return 1;
        }


        internal double GetCookingTimeSpanInMin(IMenuItem menuItem)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);


            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                var des = itemsPreparationInfo.Description;
                if (itemsPreparationInfo.CookingTimeSpanInMin != null)
                {

                    var sss = itemsPreparationInfo.CookingTimeSpanInMin;
                    return itemsPreparationInfo.CookingTimeSpanInMin.Value;
                }
            }
            return 1;
        }




        internal bool PreparationTimeSpanInMinIsDefinedFor(IItemsCategory itemsCategory)
        {
            string itemsCategoryUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            var isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == itemsCategoryUri && x.PreparationTimeSpanInMin != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal bool PreparationTimeSpanInMinIsDefinedFor(IMenuItem menuItem)
        {
            string menuItemUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            List<IItemsPreparationInfo> itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);
            bool isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == menuItemUri && x.PreparationTimeSpanInMin != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal bool AppearanceOrderIsDefinedFor(IItemsCategory itemsCategory)
        {
            string itemsCategoryUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            var isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == itemsCategoryUri && x.AppearanceOrder != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal bool AppearanceOrderIsDefinedFor(IMenuItem menuItem)
        {
            string menuItemUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            List<IItemsPreparationInfo> itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);
            bool isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == menuItemUri && x.AppearanceOrder != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal bool CookingTimeSpanInMinIsDefinedFor(IItemsCategory itemsCategory)
        {
            string itemsCategoryUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            var isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == itemsCategoryUri && x.PreparationTimeSpanInMin != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal bool CookingTimeSpanInMinIsDefinedFor(IMenuItem menuItem)
        {
            string menuItemUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            List<IItemsPreparationInfo> itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);
            bool isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == menuItemUri && x.CookingTimeSpanInMin != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal bool TagsAreDefinedFor(IItemsCategory itemsCategory)
        {
            string itemsCategoryUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            var isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == itemsCategoryUri && x.PreparationTags != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal bool TagsAreDefinedFor(IMenuItem menuItem)
        {
            string menuItemUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            List<IItemsPreparationInfo> itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);
            bool isDefined = itemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == menuItemUri && x.PreparationTags != null).FirstOrDefault() != null;
            return isDefined;
        }

        internal double GetPreparationTimeSpanInMin(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);


            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {

                if (itemsPreparationInfo.PreparationTimeSpanInMin != null)
                    return itemsPreparationInfo.PreparationTimeSpanInMin.Value;
            }
            return 1;
        }

        internal double GetCookingTimeSpanInMin(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.CookingTimeSpanInMin != null)
                    return itemsPreparationInfo.CookingTimeSpanInMin.Value;
            }
            return 1;
        }



        /// <MetaDataID>{3e3ed675-1337-4698-8101-ed0a7a87e08b}</MetaDataID>
        internal void SetPreparationTimeSpanInMin(IMenuItem menuItem, double timeSpanInMinutes)
        {
            GetOrCreateItemsPreparationInfo(menuItem).PreparationTimeSpanInMin = timeSpanInMinutes;


            //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
            //    itemsPreparationInfoPresentation.Refresh();
            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }


        internal void SetCookingTimeSpanInMin(MenuModel.IMenuItem menuItem, double timeSpanInMinutes)
        {
            GetOrCreateItemsPreparationInfo(menuItem).CookingTimeSpanInMin = timeSpanInMinutes;
        }

        Dictionary<string, TagViewModel> Tags = new Dictionary<string, TagViewModel>();
        TagViewModel GetTagViewModel(ITag tag, IItemsPreparationInfo itemsPreparationInfo, IMenuItem menuItem)
        {
            string menuItemUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            if (!Tags.TryGetValue(tag.Uid + menuItemUri, out TagViewModel tagViewModel))
            {
                tagViewModel = new TagViewModel(tag, itemsPreparationInfo, menuItemUri);
                Tags[tag.Uid + menuItemUri] = tagViewModel;
            }
            return tagViewModel;
        }

        TagViewModel GetTagViewModel(ITag tag, IItemsPreparationInfo itemsPreparationInfo, IItemsCategory itemsCategory)
        {
            string itemsCategoryUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            if (!Tags.TryGetValue(tag.Uid + itemsCategoryUri, out TagViewModel tagViewModel))
            {
                tagViewModel = new TagViewModel(tag, itemsPreparationInfo, itemsCategoryUri);
                Tags[tag.Uid + itemsCategoryUri] = tagViewModel;
            }
            return tagViewModel;
        }



        internal TagViewModel NewTagFor(IMenuItem menuItem)
        {
            var itemsPreparationInfo = GetOrCreateItemsPreparationInfo(menuItem);
            return GetTagViewModel(itemsPreparationInfo.NewPrepatationTag(), itemsPreparationInfo, menuItem);
        }

        internal List<TagViewModel> RemoveTagFrom(IMenuItem menuItem, TagViewModel tagPresentation)
        {
            string menuItemUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            var itemsPreparationInfos = PreparationStation.ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == menuItemUri).FirstOrDefault();

            Tags.Remove(tagPresentation.Tag.Uid + tagPresentation.RefersΤoUri);

            if (itemsPreparationInfos == tagPresentation.ItemsPreparationInfo)
            {
                itemsPreparationInfos.RemovePreparationTag(tagPresentation.Tag);
                List<TagViewModel> tagsPresentations = Tags.Values.Where(x => x.ItemsPreparationInfo == tagPresentation.ItemsPreparationInfo && x != tagPresentation).ToList();
                return tagsPresentations;
            }
            else
            {
                var itemsInfoObjectUri = tagPresentation.ItemsPreparationInfo.ItemsInfoObjectUri;
                var tags = Tags.Values.Where(x => x.ItemsPreparationInfo.ItemsInfoObjectUri == x.RefersΤoUri && x.ItemsPreparationInfo == tagPresentation.ItemsPreparationInfo && x.Tag.Uid != tagPresentation.Tag.Uid).Select(x => x.Tag).Distinct().ToList();
                if (itemsPreparationInfos == null)
                    itemsPreparationInfos = GetOrCreateItemsPreparationInfo(menuItem);

                return (from tag in itemsPreparationInfos.Copy(tags)
                        select GetTagViewModel(tag, itemsPreparationInfos, menuItem)).ToList();
            }
        }
        public List<TagViewModel> GetTagsFor(IMenuItem menuItem)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.PreparationTags != null)
                {
                    return (from tag in itemsPreparationInfo.PreparationTags
                            select GetTagViewModel(tag, itemsPreparationInfo, menuItem)).ToList();
                }
            }
            return new List<TagViewModel>();

            //var itemsPreparationInfo = GetOrCreateItemsPreparationInfo(menuItem);
            //return (from tag in itemsPreparationInfo.PreparationTags
            //        select GetTagViewModel(tag, itemsPreparationInfo)).ToList();
        }

        internal TagViewModel NewTagFor(MenuModel.IItemsCategory itemsCategory)
        {
            var itemsPreparationInfo = GetOrCreateItemsPreparationInfo(itemsCategory);
            return GetTagViewModel(itemsPreparationInfo.NewPrepatationTag(), itemsPreparationInfo, itemsCategory);
        }

        internal List<TagViewModel> RemoveTagFrom(IItemsCategory itemsCategory, TagViewModel tagPresentation)
        {

            string itemsCategoryUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfos = PreparationStation.ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == itemsCategoryUri).FirstOrDefault();

            Tags.Remove(tagPresentation.Tag.Uid + tagPresentation.RefersΤoUri);

            if (itemsPreparationInfos == tagPresentation.ItemsPreparationInfo)
            {
                itemsPreparationInfos.RemovePreparationTag(tagPresentation.Tag);
                List<TagViewModel> tagsPresentations = Tags.Values.Where(x => x.ItemsPreparationInfo == tagPresentation.ItemsPreparationInfo && x != tagPresentation).ToList();
                return tagsPresentations;
            }
            else
            {
                var tags = Tags.Values.Where(x => x.ItemsPreparationInfo == tagPresentation.ItemsPreparationInfo && x != tagPresentation).Select(x => x.Tag).ToList();
                if (itemsPreparationInfos == null)
                    itemsPreparationInfos = GetOrCreateItemsPreparationInfo(itemsCategory);

                return (from tag in itemsPreparationInfos.Copy(tags)
                        select GetTagViewModel(tag, itemsPreparationInfos, itemsCategory)).ToList();
            }
        }
        public List<TagViewModel> GetTagsFor(MenuModel.IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.PreparationTags != null)
                {
                    return (from tag in itemsPreparationInfo.PreparationTags
                            select GetTagViewModel(tag, itemsPreparationInfo, itemsCategory)).ToList();
                }
            }
            return new List<TagViewModel>();
        }


        /// <MetaDataID>{7de79e26-376c-47dd-8df7-88508d2bc2e0}</MetaDataID>
        internal void SetPreparationTimeSpanInMin(IItemsCategory itemsCategory, double timeSpanInMinutes)
        {
            GetOrCreateItemsPreparationInfo(itemsCategory).PreparationTimeSpanInMin = timeSpanInMinutes;
            //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
            //    itemsPreparationInfoPresentation.Refresh();
            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        internal void SetCookingTimeSpanInMin(IItemsCategory itemsCategory, double timeSpanInMinutes)
        {
            GetOrCreateItemsPreparationInfo(itemsCategory).CookingTimeSpanInMin = timeSpanInMinutes;
        }


        internal void SetAppearanceOrder(IItemsCategory itemsCategory, int appearanceOrder)
        {
            GetOrCreateItemsPreparationInfo(itemsCategory).AppearanceOrder = appearanceOrder;
        }

        internal void SetAppearanceOrder(IMenuItem menuItem, int appearanceOrder)
        {
            GetOrCreateItemsPreparationInfo(menuItem).AppearanceOrder = appearanceOrder;
        }


        internal int? GetAppearanceOrder(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.AppearanceOrder != null)
                {
                    var value= itemsPreparationInfo.AppearanceOrder.Value;
                    return value;
                }
            }
            return null;
        }

        internal int? GetAppearanceOrder(IMenuItem menuItem)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.AppearanceOrder != null)
                    return itemsPreparationInfo.AppearanceOrder.Value;
            }
            return null;
        }


        /// <MetaDataID>{da0eb088-99e4-4619-afbc-ac6df013787d}</MetaDataID>
        internal bool IsCooked(IMenuItem menuItem)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.IsCooked != null)
                    return itemsPreparationInfo.IsCooked.Value;
            }
            return false;
        }

        /// <MetaDataID>{bca3f325-83e1-4775-9cd3-b982e3ffb54d}</MetaDataID>
        internal bool IsCooked(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = PreparationStation.GetItemsPreparationInfo(itemsCategory);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.IsCooked != null)
                    return itemsPreparationInfo.IsCooked.Value;
            }
            return false;
        }

        /// <MetaDataID>{e65951cc-cfc0-47ff-83a7-64b08fde6326}</MetaDataID>
        internal void SetIsCooked(MenuModel.IMenuItem menuItem, bool isCooked)
        {
            GetOrCreateItemsPreparationInfo(menuItem).IsCooked = isCooked;
            //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
            //    itemsPreparationInfoPresentation.Refresh();

            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{f3cd4dba-9e86-44dc-ab51-d9dc8c7bc07c}</MetaDataID>
        internal void SetIsCooked(IItemsCategory itemsCategory, bool isCooked)
        {

            GetOrCreateItemsPreparationInfo(itemsCategory).IsCooked = isCooked;

            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }


        ///// <MetaDataID>{35dca994-a50e-4582-9136-9ad245edd11b}</MetaDataID>
        //internal List<IItemsPreparationInfo> GetItemsPreparationInfo(IMenuItem menuItem)
        //{
        //    List<IItemsPreparationInfo> itemsPreparationInfoHierarchy = new List<IItemsPreparationInfo>();

        //    var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
        //                                 select new
        //                                 {
        //                                     @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
        //                                     ItemsPreparationInfo = itemsInfo
        //                                 }).ToList();

        //    foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
        //    {
        //        if (itemsPreparationInfoEntry.@object is IMenuItem && (itemsPreparationInfoEntry.@object as IMenuItem) == menuItem)
        //        {
        //            if (StationPrepareItem(itemsPreparationInfoEntry.@object as IMenuItem))
        //                itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
        //            break;
        //        }

        //    }

        //    foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
        //    {
        //        if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
        //        {
        //            IItemsCategory itemsCategory = null;
        //            var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
        //            if (menuItem is IClassified)
        //            {
        //                itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;
        //                itemsPreparationInfoHierarchy.AddRange(GetItemsPreparationInfo(itemsCategory));
        //            }
        //        }
        //    }
        //    return itemsPreparationInfoHierarchy;
        //}


        ///// <MetaDataID>{b3200fb3-30f9-492c-afab-de4c31bcd06c}</MetaDataID>
        //internal List<IItemsPreparationInfo> GetItemsPreparationInfo(IItemsCategory itemsCategory)
        //{
        //    List<IItemsPreparationInfo> itemsPreparationInfoHierarchy = new List<IItemsPreparationInfo>();
        //    var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
        //                                 select new
        //                                 {
        //                                     ItemsInfoObjectUri = itemsInfo.ItemsInfoObjectUri,
        //                                     @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
        //                                     ItemsPreparationInfo = itemsInfo
        //                                 }).ToList();

        //    foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
        //    {
        //        if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory && (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory) == itemsCategory)
        //        {
        //            if (PreparationStation.StationPrepareItems(itemsPreparationInfoEntry.@object as IItemsCategory))
        //                itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
        //        }
        //    }
        //    itemsCategory = itemsCategory.Parent;
        //    while (itemsCategory != null)
        //    {
        //        foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
        //        {
        //            if (itemsPreparationInfoEntry.@object == itemsCategory && PreparationStation.StationPrepareItems(itemsCategory))
        //                itemsPreparationInfoHierarchy.Add(itemsPreparationInfoEntry.ItemsPreparationInfo);
        //        }
        //       itemsCategory = itemsCategory.Parent;
        //    }


        //    return itemsPreparationInfoHierarchy;
        //}

        /// <MetaDataID>{893f1560-7eb5-4ef5-bd8b-61ad65f2019b}</MetaDataID>
        public IItemsPreparationInfo GetOrCreateItemsPreparationInfo(MenuModel.IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == itemsCategory && (itemsInfoEntry.ItemsPreparationInfo.Included() || itemsInfoEntry.ItemsPreparationInfo.PreparationTime())
                                        select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                itemsPreparationInfo.ItemsPreparationInfoType |= ItemsPreparationInfoType.PreparationTime;
                return itemsPreparationInfo;
            }
            else
            {


                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                itemsPreparationInfo = PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.PreparationTime);
                return itemsPreparationInfo;
            }


            //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
            //    itemsPreparationInfoPresentation.Refresh();


            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }


        /// <MetaDataID>{0d56eda5-2150-4e4c-ae5d-3e3abf3447ef}</MetaDataID>
        public IItemsPreparationInfo GetOrCreateItemsPreparationInfo(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var itemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                        where itemsInfoEntry.@object == menuItem
                                        select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (itemsPreparationInfo != null)
            {
                itemsPreparationInfo.ItemsPreparationInfoType |= ItemsPreparationInfoType.PreparationTime;
                return itemsPreparationInfo;
            }
            else
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.PreparationTime);
                return itemsPreparationInfo;
            }
        }



        /// <MetaDataID>{c7fd45e3-9319-4c1d-b527-681c49be5379}</MetaDataID>
        public void IncludeItems(MenuModel.IItemsCategory itemsCategory)
        {

            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var excludedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == itemsCategory && itemsInfoEntry.ItemsPreparationInfo.Excluded()
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();
            if (excludedItemsPreparationInfo != null)
            {
                PreparationStation.RemovePreparationInfo(excludedItemsPreparationInfo);
                //RemoveItemsPreparationInfos(excludedItemsPreparationInfo);

            }

            if (!PreparationStation.StationPrepareItems(itemsCategory))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);

                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Include);

                //this.ItemsPreparationInfos = this.PreparationStation.ItemsPreparationInfos.ToList();
            }

            List<IItemsPreparationInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPreparationInfos(itemsCategory);
            if (uselessDescendantItemsPreparationInfos.Count > 0)
            {
                // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                PreparationStation.RemovePreparationInfos(uselessDescendantItemsPreparationInfos);
                //ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
            }
            //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
            //    itemsPreparationInfoPresentation.Refresh();

            // updates infrastacture vew 

            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{15d22889-37d7-4007-b556-ecee6bbf7a82}</MetaDataID>
        private List<IItemsPreparationInfo> GetUselessDescendantItemsPreparationInfos(IItemsCategory itemsCategory)
        {

            List<IItemsPreparationInfo> itemsPreparationInfos = new List<IItemsPreparationInfo>();

            //_Members.Add(new ItemsPreparationInfoPresentation(this, itemsPreparationInfo));
            var itemsPreparationInfosEntry = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                              select new
                                              {
                                                  @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                  ItemsPreparationInfo = itemsInfo
                                              }).ToList();

            //foreach (var itemsPreparationInfo in itemsPreparationInfosEntry)
            //{
            //    if (itemsPreparationInfo.ItemsPreparationInfo.Included())
            //    {
            //        if (itemsPreparationInfo.@object is IItemsCategory)
            //            if (IsDescendantOfCategory(itemsCategory, itemsPreparationInfo.@object as IItemsCategory))
            //            {
            //               // AddItemsPreparationInfos(itemsPreparationInfo.ItemsPreparationInfo);
            //            }

            //        if (itemsPreparationInfo.@object is IMenuItem)
            //            if (((itemsPreparationInfo.@object as MenuItem).Class == itemsCategory ||
            //                IsDescendantOfCategory(itemsCategory, (itemsPreparationInfo.@object as MenuItem).Class as IItemsCategory)))
            //            {
            //                //AddItemsPreparationInfos(itemsPreparationInfo.ItemsPreparationInfo);
            //            }
            //    }

            //}
            return itemsPreparationInfos;
        }

        /// <MetaDataID>{2c778db9-f296-413c-bbc7-e1ae893b191c}</MetaDataID>
        internal bool IsMenuItemAssigned(MenuItem menuItem)
        {
            if (Parent is ItemsPreparationInfoPresentation)
            {
                if ((Parent as ItemsPreparationInfoPresentation).PreparationStationPresentation.PreparationStation.StationPrepareItem(menuItem))
                    return true;

                foreach (var subPresentaionStation in Parent.Members.OfType<PreparationStationPresentation>().Where(X => X != this))
                {
                    if (subPresentaionStation.PreparationStation.StationPrepareItem(menuItem))
                        return true;
                }
                return false;
            }
            else
            {
                List<PreparationStationPresentation> preparationSubStations = PreparationSubStations;
                foreach (var subPresentaionStation in preparationSubStations.Where(X => X != this))
                {
                    if (subPresentaionStation.PreparationStation.StationPrepareItem(menuItem))
                        return true;
                }
                return false;
            }
        }

        /// <MetaDataID>{bd24c2ff-6ce9-4002-9d0b-4d9825e8e928}</MetaDataID>
        internal bool IsCategoryAssigned(IItemsCategory itemCategory)
        {
            if (Parent is ItemsPreparationInfoPresentation)
            {

                if ((Parent as ItemsPreparationInfoPresentation).PreparationStationPresentation.StationPrepareAllItems(itemCategory))
                    return true;

                foreach (var subPresentaionStation in Parent.Members.OfType<PreparationStationPresentation>().Where(X => X != this))
                {
                    if (subPresentaionStation.StationPrepareAllItems(itemCategory))
                        return true;
                }

                return false;
            }
            else
            {

                List<PreparationStationPresentation> preparationSubStations = PreparationSubStations;
                foreach (var subPresentaionStation in preparationSubStations.Where(X => X != this))
                {
                    if (subPresentaionStation.StationPrepareAllItems(itemCategory))
                        return true;
                }
                return false;
            }
        }


        /// <MetaDataID>{dcb357c6-c1a2-44fe-a69d-68bc06b726c1}</MetaDataID>
        private bool IsDescendantOfCategory(IItemsCategory itemsCategory, IItemsCategory descendantItemsCategory)
        {
            //if (itemsCategory != null && itemsCategory == descendantItemsCategory)
            //    return true;

            descendantItemsCategory = descendantItemsCategory.Class as ItemsCategory;
            while (descendantItemsCategory != null && descendantItemsCategory != itemsCategory)
            {
                descendantItemsCategory = descendantItemsCategory.Class as ItemsCategory;
                if (descendantItemsCategory == itemsCategory)
                    return true;
            }
            return false;
        }



        /// <MetaDataID>{63377565-81e0-438c-b990-1c3924b9c7c4}</MetaDataID>
        public void IncludeItem(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            var excludedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                where itemsInfoEntry.@object == menuItem && itemsInfoEntry.ItemsPreparationInfo.Excluded()
                                                select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

            if (excludedItemsPreparationInfo != null)
            {
                if (excludedItemsPreparationInfo.PreparationTime())
                {
                    excludedItemsPreparationInfo.ItemsPreparationInfoType = ItemsPreparationInfoType.PreparationTime;
                    if (!PreparationStation.StationPrepareItem(menuItem))
                        excludedItemsPreparationInfo.ItemsPreparationInfoType = ItemsPreparationInfoType.Include | ItemsPreparationInfoType.PreparationTime;
                }
                else
                {
                    PreparationStation.RemovePreparationInfo(excludedItemsPreparationInfo);
                    //RemoveItemsPreparationInfos(excludedItemsPreparationInfo);
                }
            }


            if (!PreparationStation.StationPrepareItem(menuItem))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                var dd = this.PreparationStation.ItemsPreparationInfos;
                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Include);
                //this.AddItemsPreparationInfos(itemsPreparationInfo);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();
            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));



        }

        /// <MetaDataID>{0f93a279-93d6-41e3-99e9-b629a96b1c39}</MetaDataID>
        public void ExcludeItems(MenuModel.IItemsCategory itemsCategory)
        {


            if (PreparationStation.StationPrepareItems(itemsCategory))
            {


                var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                             select new
                                             {
                                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                 ItemsPreparationInfo = itemsInfo
                                             }).ToList();

                var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                    where itemsInfoEntry.@object == itemsCategory && itemsInfoEntry.ItemsPreparationInfo.Included()
                                                    select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

                if (includedItemsPreparationInfo != null)
                {
                    this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);
                    //RemoveItemsPreparationInfos(includedItemsPreparationInfo);
                }
                else
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                    var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);

                    //AddItemsPreparationInfos(itemsPreparationInfo);
                    //var sdd = itemsPreparationInfo.Exclude;
                }

                //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                //    itemsPreparationInfoPresentation.Refresh();

            }
            else
            {
                List<IItemsPreparationInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPreparationInfos(itemsCategory);
                if (uselessDescendantItemsPreparationInfos.Count > 0)
                {
                    // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                    PreparationStation.RemovePreparationInfos(uselessDescendantItemsPreparationInfos);
                    //ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
                }
                //foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                //    itemsPreparationInfoPresentation.Refresh();


            }

            //if (PreparationStationItems != null)
            //    PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            //foreach (var member in _Members.OfType<ItemsPreparationInfoPresentation>())
            //    member.Refresh();
        }


        /// <MetaDataID>{b2e31bfa-a809-496e-9613-d0b3c166183f}</MetaDataID>
        public void ExcludeItem(MenuModel.IMenuItem menuItem)
        {

            if (PreparationStation.StationPrepareItem(menuItem))
            {


                var itemsPreparationInfos = (from itemsInfo in PreparationStation.ItemsPreparationInfos
                                             select new
                                             {
                                                 @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                 ItemsPreparationInfo = itemsInfo
                                             }).ToList();

                var includedItemsPreparationInfo = (from itemsInfoEntry in itemsPreparationInfos
                                                    where itemsInfoEntry.@object == menuItem
                                                    select itemsInfoEntry.ItemsPreparationInfo).FirstOrDefault();

                if (includedItemsPreparationInfo != null)
                {
                    this.PreparationStation.RemovePreparationInfo(includedItemsPreparationInfo);
                    // RemoveItemsPreparationInfos(includedItemsPreparationInfo);
                }
                if (PreparationStation.StationPrepareItem(menuItem))
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                    var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);
                    //this.AddItemsPreparationInfos(itemsPreparationInfo);
                }

                if (PreparationStationItems != null)
                    PreparationStationItems.Refresh();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }


        /// <MetaDataID>{8354b66c-0c6a-451f-bb01-dbce66d0ac92}</MetaDataID>
        public bool StationPrepareAllItems(MenuModel.IItemsCategory itemsCategory)
        {


            var menuItems = itemsCategory.GetAllMenuItems();
            foreach (var menuItem in menuItems)
            {
                if (!PreparationStation.StationPrepareItem(menuItem))
                    return false;
            }
            return true;
        }









        /// <MetaDataID>{8330bb7d-3b92-415f-af7d-b40630550969}</MetaDataID>
        internal bool StationPreparesForServicePoint(IServicePoint servicePoint)
        {
            var preparationForInfo = PreparationForInfos.Where(x => x.ServicePoint == servicePoint).FirstOrDefault();
            if (preparationForInfo != null && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Include)
                return true;

            if (preparationForInfo != null && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Exclude)
                return false;

            preparationForInfo = PreparationForInfos.Where(x => x.ServiceArea == (servicePoint as IHallServicePoint).ServiceArea).FirstOrDefault();

            if (preparationForInfo != null && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Include)
                return true;

            return false;



        }

        /// <MetaDataID>{e2105090-b826-4f02-bbf0-6e48eb1c0c83}</MetaDataID>
        internal bool StationPreparesForServicePoints(IServiceArea serviceArea)
        {
            var preparationForInfo = PreparationForInfos.Where(x => x.ServiceArea == serviceArea).FirstOrDefault();

            if (preparationForInfo != null && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Include)
                return true;

            return false;
        }


        /// <MetaDataID>{d7142fdf-dab1-499f-915a-c7ba090641d4}</MetaDataID>
        public readonly MenuViewModel MenuViewModel;
        /// <MetaDataID>{5ba1cb84-f93d-4d8f-926d-6555579f1880}</MetaDataID>
        public readonly IPreparationStation PreparationStation;
        /// <MetaDataID>{bbc45666-8ce6-4082-aa83-15c6ee71d3a3}</MetaDataID>
        PreparationSationsTreeNode PreparationSations;
        /// <MetaDataID>{79641c07-48c0-4b20-a96c-c8d526e6703c}</MetaDataID>
        public PreparationStationPresentation(PreparationSationsTreeNode parent, IPreparationStation preparationStation, MenuViewModel menuViewModel) : base(parent)
        {
            MenuViewModel = menuViewModel;
            PreparationSations = parent;
            PreparationStation = preparationStation;
            //ItemsPreparationInfos = preparationStation.ItemsPreparationInfos.ToList();
            PreparationForInfos = preparationStation.PreparationForInfos.ToList();



            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                EditMenuItem(win);
            });

            AssignCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                var QRCodePopup = new Views.HumanResources.NewUserQRCodePopup("Preparation Station", "Scan to register as preparation station") { CodeValue = this.PreparationStationIdentity };
                QRCodePopup.Owner = win;
                QRCodePopup.ShowDialog();

            });
        }

        /// <MetaDataID>{1d7a52a8-e26d-4a22-8fd0-61ae0d1ddb1f}</MetaDataID>
        public PreparationStationPresentation() : base(null)
        {

        }



        /// <MetaDataID>{8249fed5-f9d5-4737-89b9-02369059d24e}</MetaDataID>
        public List<IPreparationForInfo> PreparationForInfos;

        /// <MetaDataID>{708a550f-aa89-45c5-86e6-90ca5c37f0ff}</MetaDataID>
        public PreparationStationPresentation(IPreparationStation preparationStation, MenuViewModel menuViewModel) : base(null)
        {
            MenuViewModel = menuViewModel;
            PreparationStation = preparationStation;

            PreparationForInfos = preparationStation.PreparationForInfos.ToList();

            CheckBoxVisibility = Visibility.Collapsed;
            this.IsNodeExpanded = true;
        }

        /// <MetaDataID>{cdc1ea13-0793-4d80-847e-ead8b1abea7c}</MetaDataID>
        bool _SelectionCheckBox;
        /// <MetaDataID>{ed4f4072-feb6-4c29-8209-b39112782f76}</MetaDataID>
        bool SelectionCheckBox
        {
            get
            {
                return _SelectionCheckBox;
            }
            set
            {
                _SelectionCheckBox = value;
            }
        }
        /// <MetaDataID>{e5a3b4bb-663a-4a87-bfc7-b8d7d17fcf45}</MetaDataID>
        public PreparationStationPresentation(FBResourceTreeNode parent, IPreparationStation preparationStation, MenuViewModel menuViewModel, bool selectionCheckBox) : base(parent)
        {
            SelectionCheckBox = selectionCheckBox;
            MenuViewModel = menuViewModel;
            PreparationStation = preparationStation;

            PreparationForInfos = preparationStation.PreparationForInfos.ToList();

            CheckBoxVisibility = Visibility.Collapsed;
            this.IsNodeExpanded = true;
        }


        /// <MetaDataID>{7cf76e87-7a21-44ea-b315-37547c7d9d00}</MetaDataID>
        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }


        /// <MetaDataID>{b9e2a72a-299d-4b15-9236-349d077b4122}</MetaDataID>
        private void EditMenuItem(System.Windows.Window owner)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                Views.Preparation.PreparationStationItemsPage preparationStationItemsPage = new Views.Preparation.PreparationStationItemsPage();
                preparationStationItemsPage.GetObjectContext().SetContextInstance(this);

                frame.ShowDialogPage(preparationStationItemsPage);
                stateTransition.Consistent = true;
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        /// <MetaDataID>{25d5dfb4-441e-425c-bfb6-e3c610ba2b76}</MetaDataID>
        List<FBResourceTreeNode> _ItemsToChoose;
        /// <MetaDataID>{81287596-6671-46ba-80fa-133bd488c72c}</MetaDataID>
        public List<FBResourceTreeNode> ItemsToChoose
        {
            get
            {
                if (_ItemsToChoose == null)
                {

                    var rootCategory = new ItemsPreparationInfoPresentation(this, MenuViewModel.Menu, true);
                    rootCategory.CheckBoxVisibility = Visibility.Collapsed;
                    rootCategory.IsNodeExpanded = true;
                    _ItemsToChoose = new List<FBResourceTreeNode>() { rootCategory };
                }
                return _ItemsToChoose;
            }
        }
        /// <exclude>Excluded</exclude> 
        List<FBResourceTreeNode> _PreparationStationSubjects;
        /// <MetaDataID>{d9f6370c-1757-4080-9148-20beb7c70bba}</MetaDataID>
        public List<FBResourceTreeNode> PreparationStationSubjects
        {
            get
            {
                if (_PreparationStationSubjects == null)
                {
                    _PreparationStationSubjects = ItemsToChoose.ToList();

                    _PreparationStationSubjects.AddRange(PreparationForInfos.Where(x => x.ServiceArea is IServiceArea).Select(x => new ServicePointsPreparationInfoPresentation(this, x, true)).OfType<FBResourceTreeNode>().ToList());
                }


                return _PreparationStationSubjects;
            }
        }
        /// <MetaDataID>{dbeb311b-b63f-4ef4-8c4a-3fcb07045b5e}</MetaDataID>
        private void Delete()
        {
            PreparationSations.RemovePreparationStation(this);
        }

        /// <MetaDataID>{cc8b09bf-a57c-4cf6-a491-f56c3a1bc04f}</MetaDataID>
        public void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
        }



        /// <MetaDataID>{f775d80c-3dd3-4f40-b49b-aaf62b8cbbf3}</MetaDataID>
        public RelayCommand RenameCommand { get; protected set; }


        /// <MetaDataID>{7589f21d-888b-48cf-a525-fa7281492cf6}</MetaDataID>
        public RelayCommand AssignCommand { get; protected set; }

        /// <MetaDataID>{cd56dd83-901c-45a2-8ccb-24e491b60183}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }
        /// <MetaDataID>{60131975-e7b1-429f-86fe-891130a696a1}</MetaDataID>
        public RelayCommand EditCommand { get; protected set; }



        /// <MetaDataID>{e22a5a95-2eae-455b-88f0-4912a07901e4}</MetaDataID>
        public override string Name
        {
            get
            {
                return PreparationStation.Description;
            }
            set
            {
                PreparationStation.Description = value;
            }
        }

        /// <MetaDataID>{8c4b8fea-bd90-4f18-b84b-55c15686d403}</MetaDataID>
        public string PreparationStationIdentity
        {
            get
            {
                return PreparationStation.PreparationStationIdentity;
            }
        }


        /// <MetaDataID>{8396eac2-f2bf-4877-acd5-bc137079a59b}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{b6ada176-8f1e-4e6b-9ccb-3524976614a2}</MetaDataID>
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{ab86324e-b227-4c12-9630-0c039ecfd11a}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;

        /// <MetaDataID>{79a073ac-d9a3-4924-85a3-06056a95be73}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;
                    _ContextMenuItems.Add(menuItem);



                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;

                    _ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);
                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/Key16.png"));
                    menuItem.Header = Properties.Resources.AssignPreparationDevicePrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = AssignCommand;

                    _ContextMenuItems.Add(menuItem);

                }

                return _ContextMenuItems;
            }
        }


        /// <MetaDataID>{bd5502e0-4d23-4970-a90e-1beda9d85362}</MetaDataID>
        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return ContextMenuItems;
                else
                    foreach (var treeNode in Members)
                    {
                        var contextMenuItems = treeNode.SelectedItemContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }

                return null;
            }
        }


        /// <MetaDataID>{820c406d-3493-459e-acca-29221edecf2b}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/chef16.png"));
            }
        }

        /// <MetaDataID>{ae0e7b6d-4bd4-442e-935d-37b2c8eae0ae}</MetaDataID>
        public override void SelectionChange()
        {

        }

        /// <MetaDataID>{985e3e73-ed89-406f-a16c-a5118542ed05}</MetaDataID>
        ItemsPreparationInfoPresentation PreparationStationItems;

        //List<FBResourceTreeNode> _Members;// = new List<FBResourceTreeNode>();
        /// <MetaDataID>{0d6d1423-673b-4642-a7eb-568cbdeb6dd1}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                if (PreparationStationItems == null)
                {
                    PreparationStationItems = new ItemsPreparationInfoPresentation(this, MenuViewModel.Menu, SelectionCheckBox);
                    PreparationStationItems.IsNodeExpanded = true;
                    PreparationStationItems.CheckBoxVisibility = Visibility.Collapsed;
                }
                return PreparationStationItems.Members;
            }
        }

        /// <MetaDataID>{04ff4178-ea70-46f9-8885-1fee68e2d2d9}</MetaDataID>
        bool _PreparationTimeVisible;
        /// <MetaDataID>{e4b0b586-d269-42d3-8c3d-52a2ee580ff0}</MetaDataID>
        public bool PreparationTimeIsVisible
        {
            get => _PreparationTimeVisible;
            set => _PreparationTimeVisible = value;
        }
        /// <MetaDataID>{d31cb531-1926-4d78-b8fa-8728189d7d46}</MetaDataID>
        public List<PreparationStationPresentation> PreparationSubStations
        {
            get
            {
                return ItemsToChoose.OfType<ItemsPreparationInfoPresentation>().First().PreparationSubStations.Values.ToList();
            }
        }

        /// <MetaDataID>{b26cc531-aaf0-49de-bb75-0302332678b5}</MetaDataID>
        DateTime DragEnterStartTime;
        /// <MetaDataID>{f8441639-29c4-462d-993e-502d7e85de10}</MetaDataID>
        public void DragEnter(object sender, DragEventArgs e)
        {
            DragEnterStartTime = DateTime.Now;

            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;

            FloorLayoutDesigner.ViewModel.ServiceAreaPresentation serviceAreaPresentation = e.Data.GetData(typeof(FloorLayoutDesigner.ViewModel.ServiceAreaPresentation)) as FloorLayoutDesigner.ViewModel.ServiceAreaPresentation;

            if (dragItemsCategory != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else if (serviceAreaPresentation != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else
                e.Effects = DragDropEffects.None;


        }

        /// <MetaDataID>{731d5dce-49e0-4e5a-84ac-f87c81082721}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {

            if (SelectedOnDragOver)
            {
                IsSelected = false;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));

            }
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{3b365351-d715-4958-b722-6428142c9a01}</MetaDataID>
        bool SelectedOnDragOver = false;
        /// <MetaDataID>{64ae17e5-bdb3-4371-a6c0-87a44bf46595}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {

            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            FloorLayoutDesigner.ViewModel.ServiceAreaPresentation serviceAreaPresentation = e.Data.GetData(typeof(FloorLayoutDesigner.ViewModel.ServiceAreaPresentation)) as FloorLayoutDesigner.ViewModel.ServiceAreaPresentation;

            if (dragItemsCategory != null)
            {
                if (!IsSelected)
                    SelectedOnDragOver = true;
                IsSelected = true;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                if ((DateTime.Now - DragEnterStartTime).TotalSeconds > 2)
                {
                    IsNodeExpanded = true;
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
                }
            }
            else if (serviceAreaPresentation != null)
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
                e.Effects = DragDropEffects.None;


        }

        /// <MetaDataID>{f1e896f8-2f1f-4ffe-8d76-44dea97a7546}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
            DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(DragItemsCategory)) as DragItemsCategory;
            FloorLayoutDesigner.ViewModel.ServiceAreaPresentation serviceAreaPresentation = e.Data.GetData(typeof(FloorLayoutDesigner.ViewModel.ServiceAreaPresentation)) as FloorLayoutDesigner.ViewModel.ServiceAreaPresentation;
            if (dragItemsCategory != null)
            {

                IncludeItems(dragItemsCategory.ItemsCategory);
                IsNodeExpanded = true;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));

            }
            if (serviceAreaPresentation != null)
                IncludeServiceArea(serviceAreaPresentation.ServiceArea);

        }

        /// <MetaDataID>{7d557d8e-685b-46cf-a5da-242883150e6d}</MetaDataID>
        private void IncludeServiceArea(IServiceArea serviceArea)
        {
            var serviceAreaPreparationForInfo = PreparationForInfos.Where(x => x.ServiceArea == serviceArea).FirstOrDefault();

            if (serviceAreaPreparationForInfo == null)
            {
                serviceAreaPreparationForInfo = this.PreparationStation.NewServiceAreaPreparationForInfo(serviceArea, PreparationForInfoType.Include);
                PreparationForInfos.Add(serviceAreaPreparationForInfo);
                _PreparationStationSubjects.AddRange(PreparationForInfos.Where(x => x.ServiceArea is IServiceArea).Select(x => new ServicePointsPreparationInfoPresentation(this, x, true)).OfType<FBResourceTreeNode>().ToList());
                _PreparationStationSubjects = _PreparationStationSubjects.ToList();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationStationSubjects)));
            }
        }

        /// <MetaDataID>{298e8f88-ff0a-4d71-b63e-ea19e702f45a}</MetaDataID>
        public void Refresh(MenuItem menuItemWithChanges)
        {

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh(menuItemWithChanges);

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));

        }
        /// <MetaDataID>{22aee00e-c034-4c1a-adc0-8d981e8167b7}</MetaDataID>
        public void Refresh(ItemsCategory itemsCategoryWithChanges)
        {
            if (PreparationStationItems != null)
                PreparationStationItems.Refresh(itemsCategoryWithChanges);
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
        }
    }




    /// <MetaDataID>{215add4f-0e25-40ad-b716-c9b967a93b11}</MetaDataID>
    static class ItemsCategoryExtention
    {
        /// <MetaDataID>{e78b2a9d-d9b4-4f90-8a52-e5bb03227c45}</MetaDataID>
        public static List<MenuItem> GetAllMenuItems(this IItemsCategory itemsCategory)
        {
            var menuItems = itemsCategory.ClassifiedItems.OfType<MenuItem>().ToList();

            menuItems.AddRange((from subCutegory in itemsCategory.ClassifiedItems.OfType<IItemsCategory>()
                                from subCutegoriesItem in subCutegory.GetAllMenuItems()
                                select subCutegoriesItem));
            return menuItems;

        }

        /// <MetaDataID>{97965af5-b8c5-433e-bdf7-6d6c6206c72a}</MetaDataID>
        public static bool IsAncestor(this MenuItem menuItem, ItemsCategory itemsCategory)
        {
            var category = menuItem.Category;
            while (category != null)
            {
                if (category == itemsCategory)
                    return true;
                category = category.Parent;
            }
            return false;

        }

        /// <MetaDataID>{792215bf-6d4b-4804-9378-197f51d0aea9}</MetaDataID>
        public static bool IsAncestor(this ItemsCategory thisItemsCategory, ItemsCategory itemsCategory)
        {
            var category = thisItemsCategory.Parent;
            if (category == itemsCategory)
                return true;
            while (category != null)
            {
                if (category == itemsCategory)
                    return true;
                category = category.Parent;
            }
            return false;

        }


    }
}