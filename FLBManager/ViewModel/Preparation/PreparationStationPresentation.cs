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
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech.Transactions;
using StyleableWindow;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Preparation
{
    /// <MetaDataID>{8c2727a3-6d5f-4ad3-b771-0cfd38a663a8}</MetaDataID>
    public class PreparationStationPresentation : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget
    {


        /// <MetaDataID>{6b9f91bf-5517-41e8-917f-d69e56fda1ce}</MetaDataID>
        public void SetPreparationTimeSpanInMin(MenuModel.IItemsCategory itemsCategory, double timeSpanInMinutes)
        {
            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                this.ItemsPreparationInfos.Add(itemsPreparationInfo);
            }
            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            // updates infrastacture vew 

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));



        }


        /// <MetaDataID>{006e4e7a-323a-4fe4-a482-2686e3161a60}</MetaDataID>
        public void SetPreparationTimeSpanInMin(MenuModel.IMenuItem menuItem, double timeSpanInMinutes)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                this.ItemsPreparationInfos.Add(itemsPreparationInfo);
                //_Members.Add(new ItemsPreparationInfoPresentation(this, itemsPreparationInfo));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();
            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }

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

        internal void ExcludeServicePoints(IServiceArea serviceArea)
        {
            var preparationForInfo = (from a_preparationForInfo in this.PreparationForInfos
                                      where a_preparationForInfo.ServiceArea == serviceArea
                                      select a_preparationForInfo).FirstOrDefault();
            if (preparationForInfo != null)
            {
                PreparationStation.RemovePreparationForInfo(preparationForInfo);
                PreparationForInfos.Remove(preparationForInfo);
            }
        }

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

     
        /// <MetaDataID>{a517dbc7-ab28-48cd-b5a0-a837abed283f}</MetaDataID>
        internal double GetPreparationTimeSpanInMin(IMenuItem menuItem)
        {
            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
                                         select new
                                         {
                                             @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                             ItemsPreparationInfo = itemsInfo
                                         }).ToList();

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IMenuItem && (itemsPreparationInfoEntry.@object as MenuModel.IMenuItem) == menuItem)
                    return itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTimeSpanInMin;

            }

            foreach (var itemsPreparationInfoEntry in itemsPreparationInfos)
            {
                if (itemsPreparationInfoEntry.@object is MenuModel.IItemsCategory)
                {
                    MenuModel.IItemsCategory itemsCategory = null;
                    var itemsPreparationInfoCategory = (itemsPreparationInfoEntry.@object as MenuModel.IItemsCategory);
                    if (menuItem is MenuModel.IClassified)
                    {
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;
                        return GetPreparationTimeSpanInMin(itemsCategory);
                    }
                }
            }
            return 1;
        }

        /// <MetaDataID>{220670a2-1825-44a5-9f02-4e7606ca5cad}</MetaDataID>
        internal double GetPreparationTimeSpanInMin(IItemsCategory itemsCategory)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                    if (itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTime())
                        return itemsPreparationInfoEntry.ItemsPreparationInfo.PreparationTimeSpanInMin;
                }
            }

            foreach (var itemsPreparationInfo in ItemsPreparationInfos)
            {
                MenuModel.IItemsCategory itemsCategoryOrParent = itemsCategory;
                var @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsPreparationInfo.ItemsInfoObjectUri);
                if (@object is MenuModel.IItemsCategory)
                {
                    var itemsPreparationInfoCategory = (@object as MenuModel.IItemsCategory);
                    while (itemsCategoryOrParent != null && itemsCategoryOrParent != itemsPreparationInfoCategory)
                        itemsCategoryOrParent = itemsCategoryOrParent.Class as MenuModel.IItemsCategory;
                    if (itemsCategoryOrParent == itemsPreparationInfoCategory && itemsPreparationInfo.PreparationTime())
                        return itemsPreparationInfo.PreparationTimeSpanInMin;
                }
            }
            return 1;
        }

        /// <MetaDataID>{c7fd45e3-9319-4c1d-b527-681c49be5379}</MetaDataID>
        public void IncludeItems(MenuModel.IItemsCategory itemsCategory)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                ItemsPreparationInfos.Remove(excludedItemsPreparationInfo);

            }

            if (!StationPrepareItems(itemsCategory))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);

                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Include);

                this.ItemsPreparationInfos = this.PreparationStation.ItemsPreparationInfos.ToList();
            }

            List<IItemsPreparationInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPreparationInfos(itemsCategory);
            if (uselessDescendantItemsPreparationInfos.Count > 0)
            {
                // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                PreparationStation.RemovePreparationInfos(uselessDescendantItemsPreparationInfos);
                ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
            }
            foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            // updates infrastacture vew 

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{15d22889-37d7-4007-b556-ecee6bbf7a82}</MetaDataID>
        private List<IItemsPreparationInfo> GetUselessDescendantItemsPreparationInfos(IItemsCategory itemsCategory)
        {

            List<IItemsPreparationInfo> itemsPreparationInfos = new List<IItemsPreparationInfo>();

            var itemsPreparationInfosEntry = (from itemsInfo in ItemsPreparationInfos
                                              select new
                                              {
                                                  @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
                                                  ItemsPreparationInfo = itemsInfo
                                              }).ToList();

            foreach (var itemsPreparationInfo in itemsPreparationInfosEntry)
            {
                if (itemsPreparationInfo.ItemsPreparationInfo.Included())
                {
                    if (itemsPreparationInfo.@object is IItemsCategory)
                        if (IsDescendantOfCategory(itemsCategory, itemsPreparationInfo.@object as IItemsCategory))
                        {
                            itemsPreparationInfos.Add(itemsPreparationInfo.ItemsPreparationInfo);
                        }

                    if (itemsPreparationInfo.@object is IMenuItem)
                        if (((itemsPreparationInfo.@object as MenuItem).Class == itemsCategory ||
                            IsDescendantOfCategory(itemsCategory, (itemsPreparationInfo.@object as MenuItem).Class as IItemsCategory)))
                        {
                            itemsPreparationInfos.Add(itemsPreparationInfo.ItemsPreparationInfo);
                        }
                }

            }
            return itemsPreparationInfos;
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

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                    if (!StationPrepareItem(menuItem))
                        excludedItemsPreparationInfo.ItemsPreparationInfoType = ItemsPreparationInfoType.Include | ItemsPreparationInfoType.PreparationTime;
                }
                else
                {
                    PreparationStation.RemovePreparationInfo(excludedItemsPreparationInfo);
                    ItemsPreparationInfos.Remove(excludedItemsPreparationInfo);
                }
            }


            if (!StationPrepareItem(menuItem))
            {
                string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

                var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Include);
                this.ItemsPreparationInfos.Add(itemsPreparationInfo);
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


            if (StationPrepareItems(itemsCategory))
            {


                var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                    ItemsPreparationInfos.Remove(includedItemsPreparationInfo);
                }
                else
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
                    var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);

                    ItemsPreparationInfos.Add(itemsPreparationInfo);
                    //var sdd = itemsPreparationInfo.Exclude;
                }

                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();

            }
            else
            {
                List<IItemsPreparationInfo> uselessDescendantItemsPreparationInfos = GetUselessDescendantItemsPreparationInfos(itemsCategory);
                if (uselessDescendantItemsPreparationInfos.Count > 0)
                {
                    // the item preparation infos which refer to items or category which contained in included category and are useless must be removed
                    PreparationStation.RemovePreparationInfos(uselessDescendantItemsPreparationInfos);
                    ItemsPreparationInfos = PreparationStation.ItemsPreparationInfos.ToList();
                }
                foreach (var itemsPreparationInfoPresentation in ItemsToChoose.OfType<ItemsPreparationInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh();


            }

            if (PreparationStationItems != null)
                PreparationStationItems.Refresh();
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            //foreach (var member in _Members.OfType<ItemsPreparationInfoPresentation>())
            //    member.Refresh();
        }
        /// <MetaDataID>{b2e31bfa-a809-496e-9613-d0b3c166183f}</MetaDataID>
        public void ExcludeItem(MenuModel.IMenuItem menuItem)
        {

            if (StationPrepareItem(menuItem))
            {


                var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                    ItemsPreparationInfos.Remove(includedItemsPreparationInfo);
                }
                if (StationPrepareItem(menuItem))
                {

                    string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                    var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri, ItemsPreparationInfoType.Exclude);
                    this.ItemsPreparationInfos.Add(itemsPreparationInfo);
                }

                if (PreparationStationItems != null)
                    PreparationStationItems.Refresh();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }

        /// <MetaDataID>{112fd3c1-e25f-4e72-9fef-db94ea560622}</MetaDataID>
        public bool StationPrepareItems(MenuModel.IItemsCategory itemsCategory)
        {


            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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



            foreach (var itemsPreparationInfo in ItemsPreparationInfos)
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
        public bool StationPrepareItem(MenuModel.IMenuItem menuItem)
        {

            var itemsPreparationInfos = (from itemsInfo in ItemsPreparationInfos
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
                        itemsCategory = (menuItem as MenuModel.IClassified).Class as MenuModel.ItemsCategory;

                        if (StationPrepareItems(itemsCategory))
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




        internal bool StationPreparesForServicePoint(IServicePoint servicePoint)
        {
            var preparationForInfo = PreparationForInfos.Where(x => x.ServicePoint == servicePoint).FirstOrDefault();
            if (preparationForInfo != null && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Include)
                return true;

            if (preparationForInfo != null && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Exclude)
                return false;

            preparationForInfo = PreparationForInfos.Where(x => x.ServiceArea == servicePoint.ServiceArea).FirstOrDefault();

            if (preparationForInfo != null && preparationForInfo.PreparationForInfoType == PreparationForInfoType.Include)
                return true;

            return false;



        }

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
            ItemsPreparationInfos = preparationStation.ItemsPreparationInfos.ToList();
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
        }

        /// <MetaDataID>{1d7a52a8-e26d-4a22-8fd0-61ae0d1ddb1f}</MetaDataID>
        public PreparationStationPresentation() : base(null)
        {

        }

        /// <MetaDataID>{fa4e0f57-1adf-4fd1-bb36-9186da361586}</MetaDataID>
        List<IItemsPreparationInfo> ItemsPreparationInfos;


        /// <MetaDataID>{8249fed5-f9d5-4737-89b9-02369059d24e}</MetaDataID>
        MenuModel.IMenu Menu;

        public List<IPreparationForInfo> PreparationForInfos;

        /// <MetaDataID>{708a550f-aa89-45c5-86e6-90ca5c37f0ff}</MetaDataID>
        public PreparationStationPresentation(IPreparationStation preparationStation, MenuModel.IMenu menu, MenuViewModel menuViewModel) : base(null)
        {
            MenuViewModel = menuViewModel;
            PreparationStation = preparationStation;
            Menu = menu;
            PreparationForInfos = preparationStation.PreparationForInfos.ToList();
            ItemsPreparationInfos = preparationStation.ItemsPreparationInfos.ToList();

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
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            //{
            //    var SelectPreparationStationItemsWindow = new Views.Preparation.SelectPreparationStationItemsWindow();

            //    SelectPreparationStationItemsWindow.Owner = owner;

            //    SelectPreparationStationItemsWindow.GetObjectContext().RollbackOnExitWithoutAnswer = false;
            //    SelectPreparationStationItemsWindow.GetObjectContext().RollbackOnNegativeAnswer = false;

            //    SelectPreparationStationItemsWindow.GetObjectContext().SetContextInstance(this);

            //    SelectPreparationStationItemsWindow.ShowDialog();
            //    stateTransition.Consistent = true;
            //}


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                //System.Windows.Window owner = System.Windows.Window.GetWindow(EditOptionsTypesCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                //var menuItemWindow = new MenuItemsEditor.Views.CategoryMenuItemTypesWindow();
                //menuItemWindow.Owner = owner;
                //MenuItemsEditor.ViewModel.OptionsTypesViewModel optionsTypesViewModel = new MenuItemsEditor.ViewModel.OptionsTypesViewModel(ItemsCategory);
                //menuItemWindow.GetObjectContext().SetContextInstance(optionsTypesViewModel);
                //menuItemWindow.ShowDialog();

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
                    var rootCategory = new ItemsPreparationInfoPresentation(this, new FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo(MenuViewModel.Menu.RootCategory), true);
                    rootCategory.CheckBoxVisibility = Visibility.Collapsed;
                    rootCategory.IsNodeExpanded = true;
                    _ItemsToChoose = new List<FBResourceTreeNode>() { rootCategory };
                }
                return _ItemsToChoose;
            }
        }
        /// <exclude>Excluded</exclude> 
        List<FBResourceTreeNode> _PreparationStationSubjects;
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



                }
                //if (_ContextMenuItems == null)
                //{
                //    _ContextMenuItems = new List<MenuComamnd>();
                //}
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
                    PreparationStationItems = new ItemsPreparationInfoPresentation(this, new FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo(MenuViewModel.Menu.RootCategory), false);
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

                //string uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(dragItemsCategory.ItemsCategory).GetPersistentObjectUri(dragItemsCategory.ItemsCategory);
                //var itemsPreparationInfo = this.PreparationStation.NewPreparationInfo(uri);
                //_Members.Add(new ItemsPreparationInfoPresentation(this, itemsPreparationInfo));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                //IsNodeExpanded = true;
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
            }
            if (serviceAreaPresentation != null)
                IncludeServiceArea(serviceAreaPresentation.ServiceArea);
            //DragItemsCategory

        }

        private void IncludeServiceArea(IServiceArea serviceArea)
        {
            var serviceAreaPreparationForInfo = PreparationForInfos.Where(x => x.ServiceArea == serviceArea).FirstOrDefault();

            if(serviceAreaPreparationForInfo==null)
            {
                serviceAreaPreparationForInfo=this.PreparationStation.NewServiceAreaPreparationForInfo(serviceArea, PreparationForInfoType.Include);
                PreparationForInfos.Add(serviceAreaPreparationForInfo);
            }

        }

        //public List<FBResourceTreeNode> PreparationItemsInfoSelections
        //{
        //    get
        //    {
        //        return _Members.ToList();
        //    }
        //}

    }

    //Extension

    /// <MetaDataID>{15225b5f-29c1-422f-a877-130c45aa7ce7}</MetaDataID>
    static class ItemsPreparationInfoTypeExtension
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


    }
}