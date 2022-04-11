using System;
using System.Linq;
using System.Collections.Generic;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech;
using MenuModel;
using OOAdvantech.PersistenceLayer;
using FlavourBusinessFacade;
using FlavourBusinessFacade.RoomService;
using System.Threading.Tasks;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessManager.RoomService;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{67391f6f-1285-492e-adaf-f7656edfe276}</MetaDataID>
    [BackwardCompatibilityID("{67391f6f-1285-492e-adaf-f7656edfe276}")]
    [Persistent()]
    public class PreparationStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IPreparationStation, IPreparationStationRuntime
    {
        /// <MetaDataID>{11f57521-338b-4ca3-966a-ba467eaafb79}</MetaDataID>
        protected PreparationStation()
        {

        }

        /// <MetaDataID>{278f1061-f5ca-40c2-9df7-39b4c5f1a0d9}</MetaDataID>
        public PreparationStation(ServicesContextRunTime servicesContextRunTime)
        {

            _PreparationStationIdentity = servicesContextRunTime.ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            _ServicesContextIdentity = servicesContextRunTime.ServicesContextIdentity;
        }
        /// <exclude>Excluded</exclude>
        string _DeviceUpdateEtag;
        /// <MetaDataID>{c78c2bbe-02f2-4094-9f11-6b1660e1f47c}</MetaDataID>
        /// <summary>
        /// Device update mechanism operates asynchronously
        /// When the state of preparation station change the change marked as timestamp
        /// The device update mechanism raise event after 3 seconds.
        /// The device catch the event end gets the changes for timestamp (DeviceUpdateEtag) 
        /// the PreparationStationRuntime clear DeviceUpdateEtag 
        /// </summary>
        [PersistentMember(nameof(_DeviceUpdateEtag))]
        [BackwardCompatibilityID("+6")]
        public string DeviceUpdateEtag
        {
            get => _DeviceUpdateEtag;
            set
            {
                if (_DeviceUpdateEtag != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceUpdateEtag = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{3f1486ad-b5f2-4a14-bd2a-d96245b1df97}</MetaDataID>
        public bool CanPrepareItem(IMenuItem menuItem)
        {
            string ItemsInfoObjectUri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

            var itemsPreparationInfo = ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == ItemsInfoObjectUri).FirstOrDefault();
            if (itemsPreparationInfo != null)
            {
                if ((itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Exclude) == ItemsPreparationInfoType.Exclude)
                    return false;
                else
                    return true;
            }
            if ((menuItem as MenuItem).Category != null)
                return CanPrepareItemsOfCategory((menuItem as MenuItem).Category);

            return false;
        }
        /// <MetaDataID>{7dbd63b3-bf54-4af7-8f79-090016be250b}</MetaDataID>
        public double GetPreparationTimeSpanInMin(IMenuItem menuItem)
        {

            var itemsPreparationInfos = this.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.PreparationTimeSpanInMin != null)
                    return itemsPreparationInfo.PreparationTimeSpanInMin.Value;
            }


            return 0;
        }
        public double GetCookingTimeSpanInMin(IMenuItem menuItem)
        {

            var itemsPreparationInfos = this.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.CookingTimeSpanInMin != null)
                    return itemsPreparationInfo.CookingTimeSpanInMin.Value;
            }


            return 0;
        }

        /// <MetaDataID>{73462d69-7cf4-431d-aaec-49a611ad4f35}</MetaDataID>
        public bool CanPrepareItemFor(MenuModel.IMenuItem menuItem, IServicePoint servicePoint)
        {
            if (CanPrepareItem(menuItem))
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

        /// <MetaDataID>{fdf4d688-7439-48b0-b7d7-87df67b6c965}</MetaDataID>
        public double GetPreparationTimeSpanInMinForCategoryItems(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = this.GetItemsPreparationInfo(itemsCategory);

            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.IsCooked != null)
                    return itemsPreparationInfo.PreparationTimeSpanInMin.Value;
            }

            //string ItemsInfoObjectUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            //var itemsPreparationInfo = ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == ItemsInfoObjectUri).FirstOrDefault();
            //if (itemsPreparationInfo != null)
            //{
            //    if ((itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Exclude) == ItemsPreparationInfoType.Exclude)
            //        return 0;
            //    else
            //        return itemsPreparationInfo.PreparationTimeSpanInMin;
            //}
            //else if (itemsCategory.Parent != null)
            //    return GetPreparationTimeSpanInMinForCategoryItems(itemsCategory.Parent);
            //else
            return 0;
        }
        /// <MetaDataID>{da579ee2-870e-4fd5-accd-08c2ca57fa4a}</MetaDataID>
        public bool CanPrepareItemsOfCategory(IItemsCategory itemsCategory)
        {

            string ItemsInfoObjectUri = ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            var itemsPreparationInfo = ItemsPreparationInfos.Where(x => x.ItemsInfoObjectUri == ItemsInfoObjectUri).FirstOrDefault();
            if (itemsPreparationInfo != null)
            {
                if ((itemsPreparationInfo.ItemsPreparationInfoType & ItemsPreparationInfoType.Exclude) == ItemsPreparationInfoType.Exclude)
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
        [CachingDataOnClientSide]

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
        string _PreparationStationIdentity;

        /// <MetaDataID>{22bf5e56-fdf2-4553-ad43-fbf63146eb70}</MetaDataID>
        [PersistentMember(nameof(_PreparationStationIdentity))]
        [BackwardCompatibilityID("+4")]
        public string PreparationStationIdentity
        {
            get
            {

                return _PreparationStationIdentity;
            }
            //private set
            //{

            //    if (_PreparationStationIdentity != value)
            //    {
            //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //        {
            //            _PreparationStationIdentity = value;
            //            stateTransition.Consistent = true;
            //        }
            //    }
            //}
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IPreparationForInfo> _PreparationForInfos = new OOAdvantech.Collections.Generic.Set<IPreparationForInfo>();

        /// <MetaDataID>{2f99ee50-7744-4e5a-85cc-0e57d1c949fa}</MetaDataID>
        [PersistentMember(nameof(_PreparationForInfos))]
        [BackwardCompatibilityID("+5")]
        public List<IPreparationForInfo> PreparationForInfos => _PreparationForInfos.ToThreadSafeList();

        /// <MetaDataID>{0bc40c3c-0676-4b92-a7c2-71921ac5dced}</MetaDataID>
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


        /// <MetaDataID>{a3427e2c-66a7-4010-a1e1-b0a8e8daa54e}</MetaDataID>
        public string RestaurantMenuDataSharedUri => ServicePointRunTime.ServicesContextRunTime.Current.RestaurantMenuDataSharedUri;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IPreparationStation> _SubStations = new OOAdvantech.Collections.Generic.Set<IPreparationStation>();

        /// <MetaDataID>{65f5d480-8bad-4707-a063-a552eda431e0}</MetaDataID>
        [PersistentMember(nameof(_SubStations))]
        [BackwardCompatibilityID("+7")]
        public List<IPreparationStation> SubStations => _SubStations.ToThreadSafeList();

        public event ObjectChangeStateHandle ObjectChangeState;



        /// <MetaDataID>{9ee58e55-24bf-43c2-87c8-d0604eef6b23}</MetaDataID>
        public void RemovePreparationInfo(IItemsPreparationInfo itemsPreparationInfo)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                this._ItemsPreparationInfos.Remove(itemsPreparationInfo);

                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, null);
            UpdateItemsPreparationTags();
        }

        /// <MetaDataID>{5db58706-9b6c-4db0-bec6-fb3dc73bf6db}</MetaDataID>
        public IItemsPreparationInfo NewPreparationInfo(string itemsInfoObjectUri, ItemsPreparationInfoType itemsPreparationInfoType)
        {
            try
            {
                var obj = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(itemsInfoObjectUri);

                if (obj is MenuModel.ItemsCategory)
                {
                    ItemsPreparationInfo itemsPreparationInfo = new ItemsPreparationInfo(obj as MenuModel.ItemsCategory);

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPreparationInfo);
                        this._ItemsPreparationInfos.Add(itemsPreparationInfo);
                        itemsPreparationInfo.ItemsPreparationInfoType = itemsPreparationInfoType;
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
                        itemsPreparationInfo.ItemsPreparationInfoType = itemsPreparationInfoType;
                        stateTransition.Consistent = true;
                    }

                    return itemsPreparationInfo;

                }
            }
            finally
            {
                ObjectChangeState?.Invoke(this, null);
            }

            return null;
        }


        /// <MetaDataID>{5756a4e9-e593-4b91-b335-e7da58cc7d85}</MetaDataID>
        public void RemovePreparationInfos(List<IItemsPreparationInfo> itemsPreparationInfos)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var itemsPreparationInfo in itemsPreparationInfos)
                    this._ItemsPreparationInfos.Remove(itemsPreparationInfo);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, null);
            UpdateItemsPreparationTags();
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

        internal static DateTime GetPreparedAtForecast(ItemPreparation itemPreparation, MealCourse mealCourse)
        {
            return DateTime.UtcNow;
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
        /// <exclude>Excluded</exclude>
        Dictionary<string, List<ITag>> _ItemsPreparationTags;
        public Dictionary<string, List<ITag>> ItemsPreparationTags
        {
            get
            {

                lock (DeviceUpdateLock)
                {
                    if (_ItemsPreparationTags == null)
                    {
                        _ItemsPreparationTags = GetitemsPreparationTags();
                    }
                }
                return _ItemsPreparationTags;
            }
        }

        private Dictionary<string, List<ITag>> GetitemsPreparationTags()
        {
            var itemsPreparationTags = new Dictionary<string, List<ITag>>();
            IList<IMenuItem> menuItems = GetPreparationStationMenuItems(ServicesContextRunTime.Current.OperativeRestaurantMenu.RootCategory);
            foreach (var menuItem in menuItems)
            {
                foreach (var itemsPreparationInfo in this.GetItemsPreparationInfo(menuItem))
                {
                    if (itemsPreparationInfo.PreparationTags != null)
                    {
                        string uri = ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
                        itemsPreparationTags[uri] = itemsPreparationInfo.PreparationTags;
                        break;
                    }
                }
            }

            return itemsPreparationTags;
        }

        private IList<IMenuItem> GetPreparationStationMenuItems(IItemsCategory category)
        {
            List<IMenuItem> menuItems = category.MenuItems.Where(x => CanPrepareItem(x)).ToList();
            foreach (var subCategory in category.SubCategories)
                menuItems.AddRange(GetPreparationStationMenuItems(subCategory));

            return menuItems;
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

        /// <MetaDataID>{a32e2c1f-9f35-4f67-868e-8d6a3b13fb20}</MetaDataID>
        public object DeviceUpdateLock = new object();

        /// <MetaDataID>{7d50b4e9-05fb-460e-82e1-5e97fc810164}</MetaDataID>
        List<ServicePointPreparationItems> ServicePointsPreparationItems = new List<ServicePointPreparationItems>();

        /// <MetaDataID>{f65b0df7-3300-41f4-8b0f-8700a0ee2d24}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {
            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));

            var servicesContextRunTime = ServicesContextRunTime.Current;
            servicesContextRunTime.ObjectChangeState += ServicesContextRunTime_ObjectChangeState;
            var sdsd = servicesContextRunTime.OpenSessions[0].Meal.Courses.ToList();
            foreach (var servicePointPreparationItems in (from openSession in servicesContextRunTime.OpenSessions
                                                          where openSession.Meal != null
                                                          from mealCourse in openSession.Meal.Courses
                                                          from itemPreparation in mealCourse.FoodItems
                                                          orderby itemPreparation.PreparedAtForecast
                                                          //where itemPreparation.State == ItemPreparationState.PreparationDelay|| itemPreparation.State == ItemPreparationState.PendingPreparation || itemPreparation.State == ItemPreparationState.OnPreparation
                                                          group itemPreparation by mealCourse into ServicePointItems
                                                          select ServicePointItems).ToList())

            //foreach (var servicePointPreparationItems in (from itemPreparation in (from item in servicesContextStorage.GetObjectCollection<IItemPreparation>()
            //                                                                           //where item.State == ItemPreparationState.PreparationDelay|| item.State == ItemPreparationState.PendingPreparation || item.State == ItemPreparationState.OnPreparation
            //                                                                       select item.Fetching(item.ClientSession)).ToArray()
            //                                              group itemPreparation by itemPreparation.ClientSession.MainSession into ServicePointItems
            //                                              select ServicePointItems))
            {
                var preparationItems = new System.Collections.Generic.List<IItemPreparation>();
                foreach (var item in servicePointPreparationItems.OfType<RoomService.ItemPreparation>())
                {
                    if (item.MenuItem == null)
                        item.LoadMenuItem();

                    if (CanPrepareItem(item.MenuItem))
                    {
                        item.PreparationTimeSpanInMin = GetPreparationTimeSpanInMin(item.MenuItem);
                        item.IsCooked = this.IsCooked(item.MenuItem);
                        item.CookingTimeSpanInMin = GetCookingTimeSpanInMin(item.MenuItem);

                        //RoomService.ItemPreparation itemPreparation = new RoomService.ItemPreparation(item.uid, item.MenuItemUri, item.Name);
                        //itemPreparation.Update(item);
                        preparationItems.Add(item);

                        item.ObjectChangeState += FlavourItem_ObjectChangeState;
                    }
                }

                var preparationSession = new ServicePointPreparationItems(servicePointPreparationItems.Key, preparationItems);
                ServicePointsPreparationItems.Add(preparationSession);
                preparationSession.ObjectChangeState += PreparationSessionChangeState;
            }

            Task.Run(() =>
            {

                while (true)
                {
                    lock (DeviceUpdateLock)
                    {
                        if (DeviceUpdateEtag != null)
                        {
                            long numberOfTicks = 0;
                            if (long.TryParse(DeviceUpdateEtag, out numberOfTicks))
                            {
                                DateTime myDate = new DateTime(numberOfTicks);
                                if ((DateTime.Now - myDate).TotalSeconds > 3)
                                {
                                    if (RaiseEventTimeStamp == null || (DateTime.UtcNow - RaiseEventTimeStamp.Value).TotalSeconds > 30)
                                    {
                                        _PreparationItemsChangeState?.Invoke(this, DeviceUpdateEtag);
                                        RaiseEventTimeStamp = DateTime.UtcNow;
                                    }
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(500);
                }

            });
        }

        private void ServicesContextRunTime_ObjectChangeState(object _object, string member)
        {
            UpdateItemsPreparationTags();
        }

        private void UpdateItemsPreparationTags()
        {
            Dictionary<string, List<ITag>> itemsPreparationTags = GetitemsPreparationTags();
            if (_ItemsPreparationTags != null)
            {
                lock (DeviceUpdateLock)
                {
                    if (!(itemsPreparationTags.Keys.All(x => _ItemsPreparationTags.ContainsKey(x)) && _ItemsPreparationTags.Keys.All(x => itemsPreparationTags.ContainsKey(x))))
                    {
                        _ItemsPreparationTags = null;
                    }
                    else
                    {
                        foreach (var key in _ItemsPreparationTags.Keys)
                        {
                            if (_ItemsPreparationTags[key].Count != itemsPreparationTags[key].Count)
                            {
                                _ItemsPreparationTags = null;
                                break;
                            }
                            for (int i = 0; i < _ItemsPreparationTags[key].Count; i++)
                            {
                                if (_ItemsPreparationTags[key] != itemsPreparationTags[key])
                                {
                                    _ItemsPreparationTags = null;
                                    break;
                                }
                            }
                            if (_ItemsPreparationTags == null)
                                break;
                        }
                    }
                }
                if (_ItemsPreparationTags != null)
                    ObjectChangeState?.Invoke(this, nameof(ItemsPreparationTags));
            }
        }

        private void PreparationSessionChangeState(object _object, string member)
        {

            lock (DeviceUpdateLock)
                DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();

        }

        public event PreparationItemsChangeStateHandled _PreparationItemsChangeState;

        public event PreparationItemsChangeStateHandled PreparationItemsChangeState
        {
            add
            {
                _PreparationItemsChangeState += value;
            }
            remove
            {
                _PreparationItemsChangeState -= value;
            }
        }

        /// <MetaDataID>{dab03fa5-a035-4c91-a2ac-c70b12865a93}</MetaDataID>
        DateTime? RaiseEventTimeStamp;

        /// <MetaDataID>{397cadbc-bbeb-48f4-a6b8-8a4bbbc7c9ca}</MetaDataID>
        public IList<ServicePointPreparationItems> GetPreparationItems(List<ItemPreparationAbbreviation> itemsOnDevice, string deviceUpdateEtag)
        {
            if (deviceUpdateEtag == DeviceUpdateEtag)
            {
                lock (DeviceUpdateLock)
                {
                    DeviceUpdateEtag = null;
                    RaiseEventTimeStamp = null;
                }
            }


            return ServicePointsPreparationItems.Where(x => x.PreparationItems != null && x.PreparationItems.Count > 0).ToList();
        }

        /// <MetaDataID>{8287dfb4-1de6-4bca-8e57-b72df3d7121f}</MetaDataID>
        internal void RemoveItemPreparation(ItemPreparation flavourItem)
        {

            lock (DeviceUpdateLock)
            {
                if (flavourItem.PreparationStation == this)
                {
                    flavourItem.PreparationStation = null;
                    flavourItem.ObjectChangeState -= FlavourItem_ObjectChangeState;

                    var servicePointPreparationItems = ServicePointsPreparationItems.Where(x => x.ServicePoint == flavourItem.ClientSession.ServicePoint).FirstOrDefault();
                    if (servicePointPreparationItems != null)
                        servicePointPreparationItems.RemovePreparationItem(flavourItem);

                    if (DeviceUpdateEtag == null)
                        DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();
                }

            }
        }
        /// <MetaDataID>{d65435e4-edc6-4442-aa7d-72b3e8a13cee}</MetaDataID>
        internal void AssignItemPreparation(ItemPreparation flavourItem)
        {

            lock (DeviceUpdateLock)
            {
                if (flavourItem.PreparationStation != this)
                {
                    flavourItem.PreparationStation = this;

                    flavourItem.PreparationTimeSpanInMin = GetPreparationTimeSpanInMin(flavourItem.MenuItem);
                    flavourItem.IsCooked = this.IsCooked(flavourItem.MenuItem);
                    flavourItem.CookingTimeSpanInMin = GetCookingTimeSpanInMin(flavourItem.MenuItem);

                    flavourItem.ObjectChangeState += FlavourItem_ObjectChangeState;

                    var servicePointPreparationItems = ServicePointsPreparationItems.Where(x => x.MealCourse == flavourItem.MealCourse).FirstOrDefault();
                    if (servicePointPreparationItems == null)
                    {
                        var preparationSession = new ServicePointPreparationItems(flavourItem.MealCourse, new List<IItemPreparation>() { flavourItem });
                        ServicePointsPreparationItems.Add(preparationSession);
                        preparationSession.ObjectChangeState += PreparationSessionChangeState;
                    }
                    else
                        servicePointPreparationItems.AddPreparationItem(flavourItem);

                    if (DeviceUpdateEtag == null)
                        DeviceUpdateEtag = DateTime.Now.Ticks.ToString();
                }

            }

        }


        /// <MetaDataID>{0c5474b2-cf35-496a-b5de-32808319d6f9}</MetaDataID>
        internal static PreparationData GetPreparationData(ItemPreparation itemPreparation)
        {
            itemPreparation.LoadMenuItem();
            PreparationData preparationData = null;
            if (itemPreparation.PreparationStation != null)
            {
                //the item has already been assigned to the prep station
                preparationData = new PreparationData()
                {
                    ItemPreparation = itemPreparation,
                    PreparationStationRuntime = itemPreparation.PreparationStation as IPreparationStationRuntime,
                    PreparationTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem),
                    CookingTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetCookingTimeSpanInMin(itemPreparation.MenuItem)
                };
            }
            else
            {

                foreach (var preparationStation in ServicesContextRunTime.Current.PreparationStationRuntimes.Values.OfType<PreparationStation>().Where(x => x.HasServicePointsPreparationInfos))
                {
                    //Look for the dedicated for service point  prep station to prepare the item 
                    if (preparationStation.CanPrepareItemFor(itemPreparation.MenuItem, itemPreparation.ClientSession.ServicePoint))
                    {
                        preparationData = new PreparationData()
                        {
                            ItemPreparation = itemPreparation,
                            PreparationStationRuntime = preparationStation,
                            PreparationTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem),
                            CookingTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetCookingTimeSpanInMin(itemPreparation.MenuItem)
                        };
                    }
                }
                if (preparationData == null || preparationData.PreparationStationRuntime == null)
                {
                    //Look for the general  prep station to prepare the item 
                    foreach (var preparationStation in ServicesContextRunTime.Current.PreparationStationRuntimes.Values.OfType<PreparationStation>().Where(x => !x.HasServicePointsPreparationInfos))
                    {
                        if (preparationStation.CanPrepareItem(itemPreparation.MenuItem))
                        {
                            preparationData = new PreparationData()
                            {
                                ItemPreparation = itemPreparation,
                                PreparationStationRuntime = preparationStation,
                                PreparationTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem),
                                CookingTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetCookingTimeSpanInMin(itemPreparation.MenuItem)
                            };
                        }
                    }
                }

            }
            if (preparationData == null)
            {
                preparationData = new PreparationData()
                {
                    ItemPreparation = itemPreparation,
                    Duration = TimeSpan.FromMinutes(0.5),
                    PreparationTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem),
                    CookingTimeSpanInMin = (itemPreparation.PreparationStation as PreparationStation).GetCookingTimeSpanInMin(itemPreparation.MenuItem)
                };
            }
            else
            {
                preparationData.Duration = TimeSpan.FromMinutes((preparationData.PreparationStationRuntime as PreparationStation).GetPreparationTimeSpanInMin(itemPreparation.MenuItem) + (preparationData.PreparationStationRuntime as PreparationStation).GetCookingTimeSpanInMin(itemPreparation.MenuItem));
            }
            return preparationData;
        }


        /// <MetaDataID>{02fa2443-01b0-409d-bb9f-5a9d9e257129}</MetaDataID>
        private void FlavourItem_ObjectChangeState(object _object, string member)
        {
            lock (DeviceUpdateLock)
            {
                DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();
            }
            if (member == null)
                _PreparationItemsChangeState?.Invoke(this, DeviceUpdateEtag);
        }
        Dictionary<string, double> GetItemToServingtimespanPredictions()
        {
            var preparationStationItems = (from serviceSession in this.ServicePointsPreparationItems
                                           from preparationItem in serviceSession.PreparationItems.OrderByDescending(x => x.CookingTimeSpanInMin)
                                           select preparationItem).ToList();

            double duration = 0;
            Dictionary<string, double> predictions = new Dictionary<string, double>();
            var itemsInPreparation = preparationStationItems.Where(x => x.State == ItemPreparationState.…nPreparation).ToList();
            var itemsPendingToPrepare = preparationStationItems.Where(x => x.State == ItemPreparationState.PendingPreparation).ToList();
            foreach (var itemInPreparation in itemsInPreparation)
            {
                duration += itemInPreparation.PreparationTimeSpanInMin;
                predictions[itemInPreparation.uid] = duration + itemInPreparation.CookingTimeSpanInMin;
            }

            foreach (var itemPendingToPrepare in itemsPendingToPrepare)
            {
                duration += itemPendingToPrepare.PreparationTimeSpanInMin;
                predictions[itemPendingToPrepare.uid] = duration + itemPendingToPrepare.CookingTimeSpanInMin;
            }
            var roasting…tems = preparationStationItems.Where(x => x.State == ItemPreparationState.IsRoasting).ToList();

            foreach (var roasting…tem in roasting…tems)
                predictions[roasting…tem.uid] = roasting…tem.CookingTimeSpanInMin;



            foreach (var prepared…tem in preparationStationItems.Where(x => x.State == ItemPreparationState.IsPrepared))
                predictions[prepared…tem.uid] = 0;



            return new Dictionary<string, double>();
        }
        /// <MetaDataID>{cf0856b6-a2cf-43a0-98e8-055fc4c070c0}</MetaDataID>
        public Dictionary<string, double> Items…nPreparation(List<string> itemPreparationUris)
        {
            var clientSessionsItems = (from servicePointPreparationItems in ServicePointsPreparationItems
                                       from itemPreparation in servicePointPreparationItems.PreparationItems
                                       where itemPreparationUris.Contains(itemPreparation.uid)
                                       group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

            foreach (var clientSessionItems in clientSessionsItems)
                clientSessionItems.clientSession.Items…nPreparation(clientSessionItems.ClientSessionItems);

            return GetItemToServingtimespanPredictions();
        }

        public Dictionary<string, double> ItemsRoasting(List<string> itemPreparationUris)
        {
            var clientSessionsItems = (from servicePointPreparationItems in ServicePointsPreparationItems
                                       from itemPreparation in servicePointPreparationItems.PreparationItems
                                       where itemPreparationUris.Contains(itemPreparation.uid)
                                       group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

            foreach (var clientSessionItems in clientSessionsItems)
                clientSessionItems.clientSession.ItemsRoasting(clientSessionItems.ClientSessionItems);
            return GetItemToServingtimespanPredictions();
        }
        public Dictionary<string, double> ItemsServing(List<string> itemPreparationUris)
        {
            var clientSessionsItems = (from servicePointPreparationItems in ServicePointsPreparationItems
                                       from itemPreparation in servicePointPreparationItems.PreparationItems
                                       where itemPreparationUris.Contains(itemPreparation.uid)
                                       group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

            foreach (var clientSessionItems in clientSessionsItems)
                clientSessionItems.clientSession.ItemsServing(clientSessionItems.ClientSessionItems);
            return GetItemToServingtimespanPredictions();
        }


        /// <MetaDataID>{b2502860-c9af-44cf-8f10-d0a221986c7b}</MetaDataID>
        public Dictionary<string, double> ItemsPrepared(List<string> itemPreparationUris)
        {
            var clientSessionsItems = (from servicePointPreparationItems in ServicePointsPreparationItems
                                       from itemPreparation in servicePointPreparationItems.PreparationItems
                                       where itemPreparationUris.Contains(itemPreparation.uid)
                                       group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

            foreach (var clientSessionItems in clientSessionsItems)
                clientSessionItems.clientSession.ItemsPrepared(clientSessionItems.ClientSessionItems);
            return GetItemToServingtimespanPredictions();


        }
        /// <MetaDataID>{70023458-eb6b-4245-8003-3668bc9253ec}</MetaDataID>
        public Dictionary<string, double> CancelLastPreparationStep(List<string> itemPreparationUris)
        {
            var clientSessionsItems = (from servicePointPreparationItems in ServicePointsPreparationItems
                                       from itemPreparation in servicePointPreparationItems.PreparationItems
                                       where itemPreparationUris.Contains(itemPreparation.uid)
                                       group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

            foreach (var clientSessionItems in clientSessionsItems)
                clientSessionItems.clientSession.CancelLastPreparationStep(clientSessionItems.ClientSessionItems);
            return GetItemToServingtimespanPredictions();

        }

        /// <MetaDataID>{623dfb4d-50fe-47cc-be27-d8cd809ad99a}</MetaDataID>
        public void AssignCodeCardsToSessions(List<string> codeCards)
        {
            foreach (var codeCard in codeCards)
            {
                foreach (var servicePointPreparationItems in ServicePointsPreparationItems)
                {
                    if (string.IsNullOrWhiteSpace(servicePointPreparationItems.CodeCard))
                        servicePointPreparationItems.CodeCard = codeCard;
                }
            }

            lock (DeviceUpdateLock)
                DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();

            _PreparationItemsChangeState?.Invoke(this, DeviceUpdateEtag);
        }

        /// <MetaDataID>{3083f4f5-9be3-4a22-9725-f571f8b2264a}</MetaDataID>
        public IPreparationStation NewSubStation()
        {
            IPreparationStation preparationStation = null;

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                preparationStation = new PreparationStation();
                preparationStation.Description = Properties.Resources.DefaultSubPreparationStationDescription;
                preparationStation.ServicesContextIdentity = ServicesContextIdentity;
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(preparationStation);
                _SubStations.Add(preparationStation);
                stateTransition.Consistent = true;
            }
            return preparationStation;


        }

        /// <MetaDataID>{f309b3c8-eb98-42e1-b365-e047bc74daa8}</MetaDataID>
        public void RemoveSubStation(IPreparationStation preparationStation)
        {
            if (_SubStations.Contains(preparationStation))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _SubStations.Remove(preparationStation);
                    stateTransition.Consistent = true;
                }
            }

        }
    }





    /// <MetaDataID>{0241f6f2-d035-4ec2-91ee-f2b41613abe3}</MetaDataID>
    [BackwardCompatibilityID("{0241f6f2-d035-4ec2-91ee-f2b41613abe3}")]
    class PreparationData
    {
        public PreparationData()
        {

        }
        /// <MetaDataID>{a3a3dd1c-2b8f-494e-a91b-7662e37b40b8}</MetaDataID>
        public ItemPreparation ItemPreparation;

        /// <MetaDataID>{12f62d94-bd1a-4c95-8bdb-4e58145b4469}</MetaDataID>
        public IPreparationStationRuntime PreparationStationRuntime;

        /// <MetaDataID>{644e17a2-c58b-413c-bc65-b9b8d39c0729}</MetaDataID>
        public TimeSpan Duration;

        public double CookingTimeSpanInMin { get; internal set; }
        public double PreparationTimeSpanInMin { get; internal set; }
    }
}