using System;
using System.Linq;
using System.Collections.Generic;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech;
using MenuModel;
using OOAdvantech.PersistenceLayer;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{67391f6f-1285-492e-adaf-f7656edfe276}</MetaDataID>
    [BackwardCompatibilityID("{67391f6f-1285-492e-adaf-f7656edfe276}")]
    [Persistent()]
    public class PreparationStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IPreparationStation
    {
        /// <MetaDataID>{3f1486ad-b5f2-4a14-bd2a-d96245b1df97}</MetaDataID>
        public bool CanPrepareItem(IMenuItem menuItem)
        {
            string ItemsInfoObjectUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

            var itemsPreparationInfo = ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == ItemsInfoObjectUri).FirstOrDefault();
            if (itemsPreparationInfo != null)
            {
                if ((itemsPreparationInfo.ItemsPreparationInfoType&ItemsPreparationInfoType.Exclude)== ItemsPreparationInfoType.Exclude)
                    return false;
                else
                    return true;
            }
            if ((menuItem as MenuItem).Category != null)
                return CanPrepareItemsOfCategory((menuItem as MenuItem).Category);

            return false;
        }
        public double GetPreparationTimeSpanInMin(IMenuItem menuItem)
        {
            string ItemsInfoObjectUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

            var itemsPreparationInfo = ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == ItemsInfoObjectUri).FirstOrDefault();
            if (itemsPreparationInfo != null)
            {
                if ((itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Exclude) == ItemsPreparationInfoType.Exclude)
                    return 0;
                else
                    return itemsPreparationInfo.PreparationTimeSpanInMin;
            }
            if ((menuItem as MenuItem).Category != null)
                return GetPreparationTimeSpanInMinForCategoryItems((menuItem as MenuItem).Category);

            return 0;
        }


        public bool CanPrepareItemFor(MenuModel.IMenuItem menuItem, IServicePoint servicePoint)
        {
            if(CanPrepareItem(menuItem))
            {
                string servicePointObjectUri = ObjectStorage.GetStorageOfObject(servicePoint).GetPersistentObjectUri(servicePoint);
                string serviceAreaObjectUri = ObjectStorage.GetStorageOfObject(servicePoint.ServiceArea).GetPersistentObjectUri(servicePoint.ServiceArea);
                if (PreparationForInfos.OfType<PreparationForInfo>().Where(x => x.ServicePointsInfoObjectUri == servicePointObjectUri && x.PreparationForInfoType == PreparationForInfoType.Exclude).FirstOrDefault() != null)
                    return false;

                if (PreparationForInfos.OfType<PreparationForInfo>().Where(x => x.ServicePointsInfoObjectUri == servicePointObjectUri && x.PreparationForInfoType == PreparationForInfoType.Include).FirstOrDefault() != null)
                    return true;
                if (PreparationForInfos.OfType<PreparationForInfo>().Where(x => x.ServicePointsInfoObjectUri == serviceAreaObjectUri && x.PreparationForInfoType == PreparationForInfoType.Include).FirstOrDefault() != null)
                    return true;
            }
            return false;

        }

        public double GetPreparationTimeSpanInMinForCategoryItems(IItemsCategory itemsCategory)
        {
            string ItemsInfoObjectUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfo = ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == ItemsInfoObjectUri).FirstOrDefault();
            if (itemsPreparationInfo != null)
            {
                if ((itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Exclude) == ItemsPreparationInfoType.Exclude)
                    return 0;
                else
                    return itemsPreparationInfo.PreparationTimeSpanInMin;
            }
            else if (itemsCategory.Parent != null)
                return GetPreparationTimeSpanInMinForCategoryItems(itemsCategory.Parent);
            else
                return 0;
        }
        /// <MetaDataID>{da579ee2-870e-4fd5-accd-08c2ca57fa4a}</MetaDataID>
        public bool CanPrepareItemsOfCategory(IItemsCategory itemsCategory)
        {

            string ItemsInfoObjectUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfo = ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == ItemsInfoObjectUri).FirstOrDefault();
            if (itemsPreparationInfo != null)
            {
                if ((itemsPreparationInfo.ItemsPreparationInfoType&ItemsPreparationInfoType.Exclude)== ItemsPreparationInfoType.Exclude)
                    return false;
                else
                    return true;
            }
            else if (itemsCategory.Parent != null)
                return CanPrepareItemsOfCategory(itemsCategory.Parent);
            else
                return false;
        }




        /// <MetaDataID>{2b80a101-a22c-46c7-b8e0-509dd8acc4ee}</MetaDataID>
        [PersistentMember(nameof(_ItemsPreparationInfos))]
        [BackwardCompatibilityID("+3")]

        public IList<IItemsPreparationInfo> ItemsPreparationInfos
        {
            get
            {

                return _ItemsPreparationInfos.ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemsPreparationInfo> _ItemsPreparationInfos = new OOAdvantech.Collections.Generic.Set<IItemsPreparationInfo>();



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{8134e07a-fc2e-457d-9fd5-32668ccdc3d5}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {

                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude> 
        string _ServicesContextIdentity;

        /// <MetaDataID>{ea8bf40e-ac92-4b52-a0d5-f88c1ac99f47}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+2")]
        public string ServicesContextIdentity
        {
            get
            {
                return _ServicesContextIdentity;
            }
            set
            {

                if (_ServicesContextIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _PreparationStationIdentity = Guid.NewGuid().ToString("N");

        /// <MetaDataID>{22bf5e56-fdf2-4553-ad43-fbf63146eb70}</MetaDataID>
        [PersistentMember(nameof(_PreparationStationIdentity))]
        [BackwardCompatibilityID("+4")]
        public string PreparationStationIdentity
        {
            get
            {

                return _ServicesContextIdentity + "_" + _PreparationStationIdentity;
            }
            private set
            {

                if (_PreparationStationIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparationStationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IPreparationForInfo> _PreparationForInfos = new OOAdvantech.Collections.Generic.Set<IPreparationForInfo>();

        /// <MetaDataID>{2f99ee50-7744-4e5a-85cc-0e57d1c949fa}</MetaDataID>
        [PersistentMember(nameof(_PreparationForInfos))]
        [BackwardCompatibilityID("+5")]
        public List<IPreparationForInfo> PreparationForInfos => _PreparationForInfos.ToThreadSafeList();

        public bool HasServicePointsPreparationInfos
        {
            get
            {
                if (_PreparationForInfos.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;
        ///// <MetaDataID>{bbe32c62-0fef-4c53-b119-7a88a70c3277}</MetaDataID>
        //public IItemsPreparationInfo NewPreparationInfo(string itemsInfoObjectUri)
        //{
        //    return NewPreparationInfo(itemsInfoObjectUri, ItemsPreparationInfoType.Include);

        //    //var obj = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfoObjectUri);

        //    //if (obj is MenuModel.ItemsCategory)
        //    //{
        //    //    ItemsPreparationInfo itemsPreparationInfo = new ItemsPreparationInfo(obj as MenuModel.ItemsCategory);

        //    //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //    //    {
        //    //        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPreparationInfo);
        //    //        this._ItemsPreparationInfos.Add(itemsPreparationInfo);
        //    //        stateTransition.Consistent = true;
        //    //    }
        //    //    return itemsPreparationInfo;
        //    //}

        //    //if (obj is MenuModel.IMenuItem)
        //    //{
        //    //    ItemsPreparationInfo itemsPreparationInfo = new ItemsPreparationInfo(obj as MenuModel.IMenuItem);

        //    //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //    //    {
        //    //        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPreparationInfo);
        //    //        this._ItemsPreparationInfos.Add(itemsPreparationInfo);
        //    //        stateTransition.Consistent = true;
        //    //    }

        //    //    return itemsPreparationInfo;

        //    //}

        //    //return null;
        //}




        /// <MetaDataID>{9ee58e55-24bf-43c2-87c8-d0604eef6b23}</MetaDataID>
        public void RemovePreparationInfo(IItemsPreparationInfo itemsPreparationInfo)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                this._ItemsPreparationInfos.Remove(itemsPreparationInfo);

                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{5db58706-9b6c-4db0-bec6-fb3dc73bf6db}</MetaDataID>
        public IItemsPreparationInfo NewPreparationInfo(string itemsInfoObjectUri, ItemsPreparationInfoType itemsPreparationInfoType)
        {
            var obj = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfoObjectUri);

            if (obj is MenuModel.ItemsCategory)
            {
                ItemsPreparationInfo itemsPreparationInfo = new ItemsPreparationInfo(obj as MenuModel.ItemsCategory);

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPreparationInfo);
                    this._ItemsPreparationInfos.Add(itemsPreparationInfo);
                    itemsPreparationInfo.ItemsPreparationInfoType= itemsPreparationInfoType;
                    //if (exclude == false)
                    //{
                    //    RemoveDescendantItemsPreparationInfos(obj as MenuModel.ItemsCategory);
                    //}
                    stateTransition.Consistent = true;
                }

                return itemsPreparationInfo;
            }

            if (obj is MenuModel.IMenuItem)
            {
                ItemsPreparationInfo itemsPreparationInfo = new ItemsPreparationInfo(obj as MenuModel.IMenuItem);

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPreparationInfo);
                    this._ItemsPreparationInfos.Add(itemsPreparationInfo);
                    itemsPreparationInfo.ItemsPreparationInfoType= itemsPreparationInfoType;
                    stateTransition.Consistent = true;
                }

                return itemsPreparationInfo;

            }

            return null;
        }

        //private void RemoveDescendantItemsPreparationInfos(ItemsCategory itemsCategory)
        //{
        //    var itemsPreparationInfosEntry = (from itemsInfo in ItemsPreparationInfos
        //                                      select new
        //                                      {
        //                                          @object = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfo.ItemsInfoObjectUri),
        //                                          ItemsPreparationInfo = itemsInfo
        //                                      }).ToList();

        //    foreach (var itemsPreparationInfo in itemsPreparationInfosEntry)
        //    {
        //        if (itemsPreparationInfo.@object is ItemsCategory)
        //            if (IsDescendantOfCategory(itemsCategory, itemsPreparationInfo.@object as ItemsCategory))
        //            {
        //                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //                {
        //                    this._ItemsPreparationInfos.Remove(itemsPreparationInfo.ItemsPreparationInfo);

        //                    stateTransition.Consistent = true;
        //                }
        //            }
        //    }

        //}

        //private bool IsDescendantOfCategory(ItemsCategory itemsCategory, ItemsCategory descendantItemsCategory)
        //{
        //    if (itemsCategory != null && itemsCategory == descendantItemsCategory)
        //        return true;

        //    descendantItemsCategory = descendantItemsCategory.Class as ItemsCategory;
        //    while (descendantItemsCategory != null && descendantItemsCategory != itemsCategory)
        //    {
        //        descendantItemsCategory = descendantItemsCategory.Class as ItemsCategory;
        //        if (descendantItemsCategory == itemsCategory)
        //            return true;
        //    }
        //    return false;
        //}

        /// <MetaDataID>{5756a4e9-e593-4b91-b335-e7da58cc7d85}</MetaDataID>
        public void RemovePreparationInfos(List<IItemsPreparationInfo> itemsPreparationInfos)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var itemsPreparationInfo in itemsPreparationInfos)
                    this._ItemsPreparationInfos.Remove(itemsPreparationInfo);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{5dd31a9b-d087-4ba9-be06-b2272f4fe231}</MetaDataID>
        public void RemovePreparationForInfo(IPreparationForInfo preparationForInfo)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PreparationForInfos.Remove(preparationForInfo);
                ObjectStorage.DeleteObject(preparationForInfo);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{624df837-5606-4bbe-b471-5735642e9fec}</MetaDataID>
        public IPreparationForInfo NewServiceAreaPreparationForInfo(IServiceArea serviceArea, PreparationForInfoType preparationForInfoType)
        {
            var existPreparationForInfo = PreparationForInfos.Where(x => x.ServiceArea == serviceArea).FirstOrDefault();
            if (existPreparationForInfo != null)
            {
                existPreparationForInfo.PreparationForInfoType = preparationForInfoType;
                return existPreparationForInfo;
            }
            
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var preparationForInfo = new PreparationForInfo() { ServiceArea = serviceArea, PreparationForInfoType = preparationForInfoType };
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(preparationForInfo);
                this._PreparationForInfos.Add(preparationForInfo);
                stateTransition.Consistent = true;
                return preparationForInfo;
            }
            
        }

        /// <MetaDataID>{cc7b3a81-4bea-4a90-aace-5f019c0adbba}</MetaDataID>
        public IPreparationForInfo NewServicePointPreparationForInfo(IServicePoint servicePoint, PreparationForInfoType preparationForInfoType)
        {
            var existPreparationForInfo = PreparationForInfos.Where(x => x.ServicePoint == servicePoint).FirstOrDefault();
            if (existPreparationForInfo != null)
            {
                existPreparationForInfo.PreparationForInfoType = preparationForInfoType;
                return existPreparationForInfo;
            }
            
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var preparationForInfo = new PreparationForInfo() { ServicePoint = servicePoint, PreparationForInfoType = preparationForInfoType };
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(preparationForInfo);
                this._PreparationForInfos.Add(preparationForInfo);
                stateTransition.Consistent = true;
                return preparationForInfo;
            }
            
        }
    }
}