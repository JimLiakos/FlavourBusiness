using FlavourBusinessFacade;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.RoomService;
using FlavourBusinessManager.ServicePointRunTime;
using MenuModel;
using Microsoft.ServiceBus.Messaging;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{67391f6f-1285-492e-adaf-f7656edfe276}</MetaDataID>
    [BackwardCompatibilityID("{67391f6f-1285-492e-adaf-f7656edfe276}")]
    [Persistent()]
    public class PreparationStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IPreparationStation, IPreparationStationRuntime
    {
        [Association("PrepStationPrintManager", Roles.RoleA, "a1eae389-91a2-4af8-909f-8d1a3ef31b29")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        public Printing.PreparationStationPrintManager PrintManager;




        /// <exclude>Excluded</exclude>
        DeviceAppLifecycle _DeviceAppState = DeviceAppLifecycle.Shutdown;
        /// <MetaDataID>{d53dfc8e-8c06-49d1-8222-18f6c2c2a3a9}</MetaDataID>
        //[PersistentMember(nameof(_DeviceAppState))]
        [BackwardCompatibilityID("+10")]
        public DeviceAppLifecycle DeviceAppState
        {
            get => _DeviceAppState;
            set
            {
                if (_DeviceAppState != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceAppState = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{b56dedf6-280e-4fab-96f9-b3124f957433}</MetaDataID>
        public bool IsActive
        {
            get
            {
                if (_MainStation == null)
                    return true;

                if (DeviceAppState == DeviceAppLifecycle.InUse)
                    return true;

                return false;
            }
        }


        /// <MetaDataID>{3b09f083-8182-4f23-a959-5d79631c5664}</MetaDataID>
        public int AttachedDevices
        {
            get
            {
                if (_PreparationItemsChangeState != null)
                    return _PreparationItemsChangeState.GetInvocationList().Length;
                else
                    return 0;
            }
        }

        /// <MetaDataID>{bc544eec-852a-4e37-86cc-46b01705c8bb}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+12")]
        public DateTime DeviceAppActivationTime;

        /// <MetaDataID>{53ac56f3-c990-4efc-ba96-ed7c3ef5a48f}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+11")]
        public DateTime DeviceAppSleepTime;

        /// <MetaDataID>{bd097583-6e84-45b4-a299-9e9abc66ae03}</MetaDataID>
        public List<ItemPreparationTimeSpan> GetPreparationTimeSpans(DateTime fromDate, DateTime toDate)
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));


            var preparationTimeSpans = (from itemPreparationTimeSpan in storage.GetObjectCollection<ItemPreparationTimeSpan>()
                                        where itemPreparationTimeSpan.PreparationStation == this && itemPreparationTimeSpan.StartsAt > fromDate && itemPreparationTimeSpan.StartsAt < fromDate
                                        select itemPreparationTimeSpan).ToList();
            return preparationTimeSpans;
        }

        [PersistentMember()]
        [Association("ItemPreparationStatistics", Roles.RoleA, "7038c775-433e-4e6d-8400-ed23de8f8eb6")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        private OOAdvantech.Collections.Generic.Set<ItemPreparationTimeSpan> PreparationTimeSpans = new OOAdvantech.Collections.Generic.Set<ItemPreparationTimeSpan>();




        /// <exclude>Excluded</exclude>
        double _GroupingTimeSpan = 5;

        /// <summary>
        /// Defines the time interval between the first item forecast preparation time and the last item of group
        /// Food preparer can filter the product where displayed on screen to decide if worth to prepare all in session.
        /// If the food preparer uses the tags to group products, 
        /// the time interval between the preparation time of the first prediction and the last product must not exceed the GroupingTimeSpan.
        /// Products with a predicted preparation time that is outside the GroupingTimeSpan are not displayed.
        /// </summary>
        /// <MetaDataID>{341dba31-809e-4e82-9ffb-e68e6953f8d0}</MetaDataID>
        [PersistentMember(nameof(_GroupingTimeSpan))]
        [BackwardCompatibilityID("+8")]
        public double GroupingTimeSpan
        {
            get => _GroupingTimeSpan;
            set
            {
                if (_GroupingTimeSpan != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _GroupingTimeSpan = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{3b24e42d-0356-4c26-a8b1-febce64ea3ec}</MetaDataID>
        protected PreparationStation()
        {

        }

        /// <MetaDataID>{11f57521-338b-4ca3-966a-ba467eaafb79}</MetaDataID>
        public PreparationStation(string servicesContextIdentity)
        {
            _ServicesContextIdentity = servicesContextIdentity;
            _PreparationStationIdentity = servicesContextIdentity + "_" + Guid.NewGuid().ToString("N");

        }

        /// <MetaDataID>{278f1061-f5ca-40c2-9df7-39b4c5f1a0d9}</MetaDataID>
        public PreparationStation(ServicesContextRunTime servicesContextRunTime)
        {

            _PreparationStationIdentity = servicesContextRunTime.ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            _ServicesContextIdentity = servicesContextRunTime.ServicesContextIdentity;
        }

        /// <MetaDataID>{c0c5c15b-9f3d-43b3-a176-57a165523002}</MetaDataID>
        object DeviceUpdateEtagLock = new object();
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
            get
            {
                lock (DeviceUpdateEtagLock)
                    return _DeviceUpdateEtag;
            }
            set
            {
                lock (DeviceUpdateEtagLock)
                {
                    if (_Description == "Pasta & Rice")
                    {

                    }
                    if (value != null && string.IsNullOrWhiteSpace(value))
                    {

                    }

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
        internal double GetPreparationTimeInMin(IMenuItem menuItem)
        {

            var itemsPreparationInfos = this.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.PreparationTimeSpanInMin != null)
                    return itemsPreparationInfo.PreparationTimeSpanInMin.Value / 2;
            }
            return 0;
        }

        /// <MetaDataID>{81e7a4d0-f597-4555-a69c-5f71cf7ecbca}</MetaDataID>
        public IItemsPreparationInfo GetPreparationTimeItemsPreparationInfo(IMenuItem menuItem)
        {

            var itemsPreparationInfos = this.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.PreparationTimeSpanInMin != null)
                    return itemsPreparationInfo;
            }
            return itemsPreparationInfos.FirstOrDefault();
        }

        /// <MetaDataID>{2bdbae01-4664-4da5-8528-a1d7ee402c7b}</MetaDataID>
        public double GetCookingTimeSpanInMin(IMenuItem menuItem)
        {
            var itemsPreparationInfos = this.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.CookingTimeSpanInMin != null)
                    return itemsPreparationInfo.CookingTimeSpanInMin.Value / 2;
            }
            return 0;
        }



        /// <MetaDataID>{32cdc1cf-346e-41af-8a03-c664597a52c4}</MetaDataID>
        public int GeAppearanceOrder(IMenuItem menuItem)
        {
            var itemsPreparationInfos = this.GetItemsPreparationInfo(menuItem);
            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.AppearanceOrder != null)
                    return itemsPreparationInfo.AppearanceOrder.Value;
            }
            return 0;
        }


        /// <MetaDataID>{73462d69-7cf4-431d-aaec-49a611ad4f35}</MetaDataID>
        public bool CanPrepareItemFor(MenuModel.IMenuItem menuItem, IServicePoint servicePoint)
        {
            if (CanPrepareItem(menuItem))
            {
                string servicePointObjectUri = ObjectStorage.GetStorageOfObject(servicePoint).GetPersistentObjectUri(servicePoint);
                string serviceAreaObjectUri = ObjectStorage.GetStorageOfObject((servicePoint as IHallServicePoint).ServiceArea).GetPersistentObjectUri((servicePoint as IHallServicePoint).ServiceArea);
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
        public double GetPreparationTimeInMinForCategoryItems(IItemsCategory itemsCategory)
        {
            var itemsPreparationInfos = this.GetItemsPreparationInfo(itemsCategory);

            foreach (var itemsPreparationInfo in itemsPreparationInfos)
            {
                if (itemsPreparationInfo.IsCooked != null)
                    return itemsPreparationInfo.PreparationTimeSpanInMin.Value;
            }

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
        //[CachingDataOnClientSide]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete | PersistencyFlag.OnConstruction)]
        public IList<IItemsPreparationInfo> ItemsPreparationInfos
        {
            get
            {

                return _ItemsPreparationInfos.ToThreadSafeList();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemsPreparationInfo> _ItemsPreparationInfos = new OOAdvantech.Collections.Generic.Set<IItemsPreparationInfo>();



        /// <exclude>Excluded</exclude>
        ObjectStateManagerLink StateManagerLink;

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
        string _ShortIdentity;

        /// <MetaDataID>{8f3e7ffa-4ce7-45db-9628-9893f371502e}</MetaDataID>
        [PersistentMember(nameof(_ShortIdentity))]
        [BackwardCompatibilityID("+9")]
        public string ShortIdentity
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ShortIdentity))
                    GetShortIdentity();
                return _ShortIdentity;
            }
            set
            {
                if (_ShortIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ShortIdentity = value;
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
        //[CachingDataOnClientSide]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete | PersistencyFlag.OnConstruction)]
        public List<IPreparationForInfo> PreparationForInfos => _PreparationForInfos.ToThreadSafeList();

        /// <MetaDataID>{1593b236-b246-4099-981f-ace2d6b584bf}</MetaDataID>
        internal void OnPreparationItemsChangeState()
        {
            Transaction.RunOnTransactionCompleted(() =>
            {
                PrintManager?.OnPreparationItemsChangeState(this);
                lock (DeviceUpdateEtagLock)
                {
                    if (string.IsNullOrWhiteSpace(DeviceUpdateEtag))
                        DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();
                }
            });
        }

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

        /// <exclude>Excluded</exclude>
        string _Printer;
        /// <MetaDataID>{71f3f215-0707-4453-8d1d-c775ac59679c}</MetaDataID>
        [PersistentMember(nameof(_Printer))]
        [BackwardCompatibilityID("+14")]
        public string Printer
        {
            get => _Printer; 
            set
            {
                if (_Printer != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Printer = value;
                        stateTransition.Consistent = true;
                    }
                    Transaction.RunOnTransactionCompleted(() =>
                    {
                        PrintManager?.OnPreparationItemsChangeState(this);
                    });
                }
            }
        }

        /// <MetaDataID>{a3427e2c-66a7-4010-a1e1-b0a8e8daa54e}</MetaDataID>
        public string RestaurantMenuDataSharedUri => ServicePointRunTime.ServicesContextRunTime.Current.RestaurantMenuDataSharedUri;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IPreparationStation> _SubStations = new OOAdvantech.Collections.Generic.Set<IPreparationStation>();

        /// <MetaDataID>{65f5d480-8bad-4707-a063-a552eda431e0}</MetaDataID>
        [PersistentMember(nameof(_SubStations))]
        [BackwardCompatibilityID("+7")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public List<IPreparationStation> SubStations => _SubStations.ToThreadSafeList();


        /// <exclude>Excluded</exclude>
        Member<IPreparationStation> _MainStation = new Member<IPreparationStation>();

        /// <MetaDataID>{63799a62-82dd-43a8-8dab-f2b6b9f10bc5}</MetaDataID>
        [PersistentMember(nameof(_MainStation))]
        [BackwardCompatibilityID("+13")]

        public IPreparationStation MainStation => _MainStation.Value;


        ///// <MetaDataID>{63799a62-82dd-43a8-8dab-f2b6b9f10bc5}</MetaDataID>
        //[BackwardCompatibilityID("+13")]
        //public IPreparationStation MainStation { get => null; }

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

        /// <MetaDataID>{fb2d4b5b-30b5-4c63-8a11-dce1b6175baf}</MetaDataID>
        public void RemovePreparationForInfos(List<IPreparationForInfo> preparationForInfos)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var preparationForInfo in preparationForInfos)
                {
                    _PreparationForInfos.Remove(preparationForInfo);
                    ObjectStorage.DeleteObject(preparationForInfo);
                }
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{17d272fa-ea15-4a97-a448-3cd33f624866}</MetaDataID>
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

        /// <MetaDataID>{6c64712f-bba6-4803-acb8-28b5df0871a0}</MetaDataID>
        internal double GetPreparationTimeSpanInMin(ItemPreparation itemPreparation)
        {
            if (itemPreparation.PreparationTimeSpanInMin == -1)
            {
                var itemsPreparationInfos = this.GetItemsPreparationInfo(itemPreparation.MenuItem);
                foreach (var itemsPreparationInfo in itemsPreparationInfos)
                {
                    if (itemsPreparationInfo.PreparationTimeSpanInMin != null)
                    {
                        itemPreparation.PreparationTimeSpanInMin = itemsPreparationInfo.PreparationTimeSpanInMin.Value / 2;
                        break;
                    }
                }
                if (itemPreparation.PreparationTimeSpanInMin == -1)
                    itemPreparation.PreparationTimeSpanInMin = 0;
            }
            return itemPreparation.PreparationTimeSpanInMin;
        }
        /// <MetaDataID>{66b85fba-d3e9-430a-a510-488ecac9adad}</MetaDataID>
        internal double GetCookingTimeSpanInMin(ItemPreparation itemPreparation)
        {
            if (itemPreparation.CookingTimeSpanInMin == -1)
            {
                var itemsPreparationInfos = this.GetItemsPreparationInfo(itemPreparation.MenuItem);
                foreach (var itemsPreparationInfo in itemsPreparationInfos)
                {
                    if (itemsPreparationInfo.CookingTimeSpanInMin != null)
                    {
                        itemPreparation.CookingTimeSpanInMin = itemsPreparationInfo.CookingTimeSpanInMin.Value / 2;
                        break;
                    }
                }

            }
            if (itemPreparation.CookingTimeSpanInMin == -1)
                return 0;

            return itemPreparation.CookingTimeSpanInMin;
        }

        /// <MetaDataID>{39ef62a3-cbec-4713-9edd-16f70a9a43c8}</MetaDataID>
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

        /// <MetaDataID>{96af9e2b-3822-49d2-be45-0da2df603ce0}</MetaDataID>
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

        /// <MetaDataID>{22e79fdc-5ec6-40c7-947d-ea86376e5388}</MetaDataID>
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


        /// <exclude>Excluded</exclude>
        //List<ItemsPreparationContext> _PreparationSessions = new List<ItemsPreparationContext>();

        /// <MetaDataID>{7d50b4e9-05fb-460e-82e1-5e97fc810164}</MetaDataID>
        internal List<ItemsPreparationContext> FoodItemsInProgress
        {
            get
            {
                ObjectActivated.Task.Wait();



                //GetItemToServingtimespanPredictions();
                List<ItemsPreparationContext> itemsPreparationContexts = null;
                lock (DeviceUpdateLock)
                {
                    itemsPreparationContexts = (ServicesContextRunTime.Current.MealsController as MealsController).MealCoursesInProgress.SelectMany(x => x.FoodItemsInProgress).
                            Where(x => x.PreparationStationIdentity == this.PreparationStationIdentity && x.PreparationState.IsInPreviousState(ItemPreparationState.OnRoad)).
                            OrderBy(x => x.MealCourseStartsAt).ToList();

                }

                return itemsPreparationContexts;
            }
        }



        /// <MetaDataID>{3fca2ab5-8bfd-4eb0-ba49-1366ad0c909a}</MetaDataID>
        TaskCompletionSource<bool> ObjectActivated = new TaskCompletionSource<bool>();

        /// <MetaDataID>{f65b0df7-3300-41f4-8b0f-8700a0ee2d24}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {

            Task.Run(() =>
            {
                lock (DeviceUpdateLock)
                {

                    if (_ShortIdentity == null)
                    {
                        GetShortIdentity();
                    }
                    try
                    {
                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));

                        var servicesContextRunTime = ServicesContextRunTime.Current;
                        servicesContextRunTime.ObjectChangeState += ServicesContextRunTime_ObjectChangeState;
                        DateTime timeStamp = DateTime.UtcNow;

                        var serviceSessionsPreparationItems = (from openSession in servicesContextRunTime.OpenSessions
                                                               where openSession.Meal != null
                                                               from mealCourse in openSession.Meal.Courses
                                                               from itemPreparation in mealCourse.FoodItems
                                                               orderby itemPreparation.PreparedAtForecast
                                                               //where itemPreparation.State == ItemPreparationState.PreparationDelay|| itemPreparation.State == ItemPreparationState.PendingPreparation || itemPreparation.State == ItemPreparationState.OnPreparation
                                                               group itemPreparation by mealCourse into ServicePointItems
                                                               select ServicePointItems).OrderBy(x => x.Key.StartsAt).ToList();

                        var sss = (DateTime.UtcNow - timeStamp);

                        foreach (var servicePointPreparationItems in serviceSessionsPreparationItems)
                        {
                            var preparationItems = new System.Collections.Generic.List<IItemPreparation>();
                            foreach (var item in servicePointPreparationItems.OfType<RoomService.ItemPreparation>())
                            {
                                if (item.MenuItem == null)
                                    item.LoadMenuItem();

                                if (CanPrepareItem(item.MenuItem))
                                {
                                    //item.PreparationTimeSpanInMin = GetPreparationTimeSpanInMin(item.MenuItem);
                                    item.IsCooked = this.IsCooked(item.MenuItem);
                                    //item.CookingTimeSpanInMin = GetCookingTimeSpanInMin(item.MenuItem);

                                    //RoomService.ItemPreparation itemPreparation = new RoomService.ItemPreparation(item.uid, item.MenuItemUri, item.Name);
                                    //itemPreparation.Update(item);
                                    preparationItems.Add(item);

                                    item.ObjectChangeState += FlavourItem_ObjectChangeState;
                                }
                            }
                            //if (preparationItems.Count > 0)
                            //{
                            //    var preparationSection = new ItemsPreparationContext(servicePointPreparationItems.Key, this, preparationItems);

                            //    _PreparationSessions.Add(preparationSection);

                            //    preparationSection.ObjectChangeState += PreparationSessionChangeState;
                            //}
                        }
                    }
                    finally
                    {
                        ObjectActivated.SetResult(true);
                    }
                }

                int attachedDevices = 0;

                while (true)
                {
                    lock (DeviceUpdateLock)
                    {


                        long numberOfTicks = 0;
                        if (long.TryParse(DeviceUpdateEtag, out numberOfTicks))
                        {
                            if (_Description == "Pasta & Rice")
                            {

                            }
                            DateTime myDate = new DateTime(numberOfTicks);
                            if ((DateTime.Now - myDate).TotalSeconds > 3)
                            {
                                if (RaiseEventTimeStamp == null || (DateTime.UtcNow - RaiseEventTimeStamp.Value).TotalSeconds > 10)
                                {
                                    _PreparationItemsChangeState?.Invoke(this, DeviceUpdateEtag);
                                    RaiseEventTimeStamp = DateTime.UtcNow;
                                }
                            }
                        }
                    }

                    #region DeviceAppLifecycle
                    if (DeviceAppState == DeviceAppLifecycle.InUse && (_PreparationItemsChangeState == null || attachedDevices != AttachedDevices))
                    {

                        //with _PreparationItemsChangeState system knows indirectly when there is active connection with client device
                        //when communication session with client device closed the server session part drops all event consumers
                        if (attachedDevices < AttachedDevices)
                            attachedDevices = AttachedDevices;

                        if (DeviceConnectionStatusChecksNumber > 50) //25 seconds (50*500 ms)
                        {
                            if (_PreparationItemsChangeState == null)
                                DeviceShutdown();
                            if (attachedDevices != AttachedDevices)
                            {
                                Task.Run(() =>
                                {
                                    ObjectChangeState?.Invoke(this, nameof(AttachedDevices));
                                });
                                attachedDevices = AttachedDevices;
                            }
                            DeviceConnectionStatusChecksNumber = 0;
                        }
                        else
                            DeviceConnectionStatusChecksNumber++;
                    }
                    else
                        DeviceConnectionStatusChecksNumber = 0;
                    #endregion


                    System.Threading.Thread.Sleep(500);
                }

            });
        }

        /// <MetaDataID>{caed0947-adbb-4f1d-96a9-9ed3f975cfc4}</MetaDataID>
        object StateMachineLock = new object();

        /// <MetaDataID>{4976a965-8662-450e-92cc-f857dbe7d629}</MetaDataID>
        public void DeviceSleep()
        {
            DeviceAppLifecycle deviceAppState = DeviceAppLifecycle.InUse;
            lock (StateMachineLock)
                deviceAppState = DeviceAppState;
            if (deviceAppState != DeviceAppLifecycle.Sleep)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    DeviceAppSleepTime = DateTime.UtcNow;
                    lock (StateMachineLock)
                        DeviceAppState = DeviceAppLifecycle.Sleep;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{816503d8-233d-4779-bbef-e6c556291e59}</MetaDataID>
        private void DeviceShutdown()
        {
            DeviceAppLifecycle deviceAppState = DeviceAppLifecycle.InUse;
            lock (StateMachineLock)
                deviceAppState = DeviceAppState;
            if (deviceAppState != DeviceAppLifecycle.Shutdown)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    DeviceAppSleepTime = DateTime.UtcNow;
                    lock (StateMachineLock)
                        DeviceAppState = DeviceAppLifecycle.Shutdown;

                    OOAdvantech.Transactions.Transaction.RunOnTransactionCompleted(() =>
                    {
                        (ServicesContextRunTime.Current.MealsController as MealsController).MealPrepStationsRedistribution();
                    });
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{35ced85b-49dd-49b9-b043-481a5de5b6c7}</MetaDataID>
        public void DeviceResume()
        {
            DeviceAppLifecycle deviceAppState = DeviceAppLifecycle.InUse;
            lock (StateMachineLock)
                deviceAppState = DeviceAppState;
            if (deviceAppState != DeviceAppLifecycle.InUse)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    DeviceAppActivationTime = DateTime.UtcNow;
                    lock (StateMachineLock)
                        DeviceAppState = DeviceAppLifecycle.InUse;


                    OOAdvantech.Transactions.Transaction.RunOnTransactionCompleted(() =>
                    {
                        (ServicesContextRunTime.Current.MealsController as MealsController).MealPrepStationsRedistribution();
                    });

                    stateTransition.Consistent = true;
                }
            }


        }


        /// <MetaDataID>{13f9e1eb-2625-4576-826a-b80f2c1532fe}</MetaDataID>
        private void GetShortIdentity()
        {
            string uniqueIdsUrl = string.Format("http://{0}:8090/api/UniqueId/{1}", FlavourBusinessFacade.ComputingResources.EndPoint.Server, this.PreparationStationIdentity);
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                var json = wc.DownloadString(uniqueIdsUrl);

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ShortIdentity = OOAdvantech.Json.JsonConvert.DeserializeObject<string>(json);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{a16a4790-f7f7-471a-aed1-5fcf53ebe59c}</MetaDataID>
        private void ServicesContextRunTime_ObjectChangeState(object _object, string member)
        {
            UpdateItemsPreparationTags();
        }

        /// <MetaDataID>{8d71bc56-c666-41a0-910e-d7a8ca3b2618}</MetaDataID>
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

        ///// <MetaDataID>{ba4cf6b6-0989-49f1-adb0-b9b976ad8324}</MetaDataID>
        //private void PreparationSessionChangeState(object _object, string member)
        //{
        //    OnPreparationItemsChangeState();
        //}
        /// <exclude>Excluded</exclude>
        public event PreparationItemsChangeStateHandled _PreparationItemsChangeState;

        /// <summary>
        /// Used from devices to update its state.
        /// The number of subscribers are the number of attached devices
        /// </summary>
        public event PreparationItemsChangeStateHandled PreparationItemsChangeState
        {
            add
            {
                _PreparationItemsChangeState += value;

                DeviceResume();

                Task.Run(() =>
                {
                    ObjectChangeState?.Invoke(this, nameof(AttachedDevices));
                });
            }
            remove
            {
                _PreparationItemsChangeState -= value;



            }
        }

        /// <MetaDataID>{dab03fa5-a035-4c91-a2ac-c70b12865a93}</MetaDataID>
        DateTime? RaiseEventTimeStamp;


        /// <MetaDataID>{8ae5f7dd-9136-4cee-a183-060bf7120ae7}</MetaDataID>
        DateTime? PreparationVelocityMilestone;

        /// <MetaDataID>{397cadbc-bbeb-48f4-a6b8-8a4bbbc7c9ca}</MetaDataID>
        public PreparationStationStatus GetPreparationItems(List<ItemPreparationAbbreviation> itemsOnDevice, string deviceUpdateEtag)
        {

            ObjectActivated.Task.Wait();

            if (deviceUpdateEtag == DeviceUpdateEtag)
            {
                lock (DeviceUpdateEtagLock)
                {
                    DeviceUpdateEtag = null;
                    RaiseEventTimeStamp = null;
                }
            }

            var predictions = GetItemToServingTimespanPredictions();
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var preparationItem in (from prepartionSession in FoodItemsInProgress
                                                 from prepartionItem in prepartionSession.PreparationItems
                                                 where prepartionItem.PreparationStation == null
                                                 select prepartionItem))
                {
                    preparationItem.PreparationStation = this;
                }

                stateTransition.Consistent = true;
            }


            PreparationStationStatus preparationStationStatus = null;


            lock (DeviceUpdateLock)
            {
                preparationStationStatus = new PreparationStationStatus()
                {
                    NewItemsUnderPreparationControl = FoodItemsInProgress.Where(x => x.PreparationItems != null && x.PreparationItems.Count > 0).ToList(),
                    ServingTimespanPredictions = predictions
                };

                int numberOfPendingToPrepareItems = (from preparationSection in preparationStationStatus.NewItemsUnderPreparationControl
                                                     from itemPreparation in preparationSection.PreparationItems
                                                     where itemPreparation.State == ItemPreparationState.PendingPreparation || itemPreparation.State == ItemPreparationState.InPreparation
                                                     select itemPreparation).Count();
                if (numberOfPendingToPrepareItems == 0)
                    PreparationVelocityMilestone = null;
                else if (PreparationVelocityMilestone == null)
                {
                    PreparationVelocityMilestone = DateTime.UtcNow;
                }
            }


            foreach (var servingSession in preparationStationStatus.NewItemsUnderPreparationControl)
            {
                servingSession.PreparationItems = servingSession.PreparationItems.OrderBy(x => this.GeAppearanceOrder((x as ItemPreparation).MenuItem)).ToList();
                if (servingSession.ServedAtForecast == null)
                {
                    foreach (var preparationItem in servingSession.PreparationItems)
                    {
                        if (preparationStationStatus.ServingTimespanPredictions.ContainsKey(preparationItem.uid))
                        {
                            DateTime itemReadyToServe = preparationStationStatus.ServingTimespanPredictions[preparationItem.uid].PreparationStart + TimeSpan.FromMinutes(preparationItem.PreparationTimeSpanInMin + preparationItem.CookingTimeSpanInMin);
                            if (servingSession.ServedAtForecast == null || servingSession.ServedAtForecast < itemReadyToServe)
                                servingSession.ServedAtForecast = itemReadyToServe;
                        }
                    }
                }

            }
            //bool Web = false;

            //OOAdvantech.Remoting.RestApi.RequestData request = System.Runtime.Remoting.Messaging.CallContext.GetData("RestApiRequest" ) as OOAdvantech.Remoting.RestApi.RequestData;

            //var jSetttings = new OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings(JsonContractType.Serialize, Web ? JsonSerializationFormat.TypeScriptJsonSerialization : JsonSerializationFormat.NetTypedValuesJsonSerialization, request.ChannelUri, request.InternalChannelUri, request.ServerSession);// { TypeNameHandling = ServerSession.Web ? TypeNameHandling.None : TypeNameHandling.All, Binder = new OOAdvantech.Remoting.RestApi.SerializationBinder(Web), ContractResolver = new JsonContractResolver(JsonContractType.Serialize, ChannelUri, InternalChannelUri, ServerSession,Web) };


            //var ReturnObjectJson = OOAdvantech.Json.JsonConvert.SerializeObject(preparationStationStatus, jSetttings);
            if (preparationStationStatus.NewItemsUnderPreparationControl.Count == 0)
            {

            }
            return preparationStationStatus;
        }

        /// <MetaDataID>{8287dfb4-1de6-4bca-8e57-b72df3d7121f}</MetaDataID>
        internal void RemoveItemPreparation(ItemPreparation flavourItem)
        {
            if (flavourItem.PreparationStation == this)
            {
                lock (DeviceUpdateLock)
                {
                    flavourItem.PreparationStation = null;
                    flavourItem.ObjectChangeState -= FlavourItem_ObjectChangeState;

                    //var servicePointPreparationItems = PreparationSessions.Where(x => x.MealCourse == flavourItem.MealCourse).FirstOrDefault();
                    //if (servicePointPreparationItems != null)
                    //    servicePointPreparationItems.RemovePreparationItem(flavourItem);
                }
                OnPreparationItemsChangeState();
            }
        }
        /// <MetaDataID>{d65435e4-edc6-4442-aa7d-72b3e8a13cee}</MetaDataID>
        internal void AssignItemPreparation(ItemPreparation flavourItem)
        {
            ObjectActivated.Task.Wait();
            if (flavourItem.PreparationStation != this)
            {
                lock (DeviceUpdateLock)
                {
                    flavourItem.PreparationStation = this;
                    flavourItem.PreparationTimeSpanInMin = GetPreparationTimeInMin(flavourItem.MenuItem);
                    flavourItem.IsCooked = this.IsCooked(flavourItem.MenuItem);
                    flavourItem.CookingTimeSpanInMin = GetCookingTimeSpanInMin(flavourItem.MenuItem);
                    flavourItem.ObjectChangeState += FlavourItem_ObjectChangeState;

                    //var servicePointPreparationItems = _PreparationSessions.Where(x => x.MealCourse == flavourItem.MealCourse).FirstOrDefault();
                    //if (servicePointPreparationItems == null)
                    //{
                    //    var preparationSection = new ItemsPreparationContext(flavourItem.MealCourse, this, new List<IItemPreparation>() { flavourItem });


                    //    _PreparationSessions.Add(preparationSection);

                    //    preparationSection.ObjectChangeState += PreparationSessionChangeState;
                    //}
                    //else
                    //    servicePointPreparationItems.AddPreparationItem(flavourItem);


                    OnPreparationItemsChangeState();
                }

            }

        }


        /// <MetaDataID>{0c5474b2-cf35-496a-b5de-32808319d6f9}</MetaDataID>
        internal static IPreparationStation GetPreparationStationFor(ItemPreparation itemPreparation)
        {
            itemPreparation.LoadMenuItem();
            //PreparationData preparationData = null;
            if (itemPreparation.PreparationStation != null)
            {
                return itemPreparation.PreparationStation;
            }
            else
            {
                foreach (var preparationStation in ServicesContextRunTime.Current.PreparationStationRunTimes.Values.OfType<PreparationStation>().Where(x => x.HasServicePointsPreparationInfos))
                {
                    //Look for the dedicated for service point  prep station to prepare the item 
                    if (preparationStation.CanPrepareItemFor(itemPreparation.MenuItem, itemPreparation.ClientSession.ServicePoint))
                        return preparationStation;
                }

                //Look for the general  prep station to prepare the item 
                foreach (var preparationStation in ServicesContextRunTime.Current.PreparationStationRunTimes.Values.OfType<PreparationStation>().Where(x => !x.HasServicePointsPreparationInfos))
                {
                    if (preparationStation.CanPrepareItem(itemPreparation.MenuItem))
                        return preparationStation;
                }


            }
            return null;
        }


        /// <MetaDataID>{ef4809ba-3b6f-4644-887c-7d94d9cf3807}</MetaDataID>
        internal static IPreparationStation GetActivePreparationStationFor(ItemPreparation itemPreparation)
        {
            itemPreparation.LoadMenuItem();
            //PreparationData preparationData = null;
            if (itemPreparation.ActivePreparationStation != null)
            {
                return itemPreparation.ActivePreparationStation;
            }
            else
            {
                foreach (var preparationStation in ServicesContextRunTime.Current.PreparationStationRunTimes.Values.OfType<PreparationStation>().Where(x => x.HasServicePointsPreparationInfos))
                {
                    //Look for the dedicated for service point  prep station to prepare the item 
                    if (preparationStation.CanPrepareItemFor(itemPreparation.MenuItem, itemPreparation.ClientSession.ServicePoint))
                    {

                        if (preparationStation?.MainStation != null && !(preparationStation as PreparationStation).IsActive)
                            return preparationStation?.MainStation;
                        else
                            return preparationStation;
                    }
                }

                //Look for the general  prep station to prepare the item 
                foreach (var preparationStation in ServicesContextRunTime.Current.PreparationStationRunTimes.Values.OfType<PreparationStation>().Where(x => !x.HasServicePointsPreparationInfos))
                {
                    if (preparationStation.CanPrepareItem(itemPreparation.MenuItem))
                    {

                        if (preparationStation?.MainStation != null && !(preparationStation as PreparationStation).IsActive)
                            return preparationStation?.MainStation;
                        else
                            return preparationStation;

                    }
                }


            }
            return null;
        }


        /// <MetaDataID>{02fa2443-01b0-409d-bb9f-5a9d9e257129}</MetaDataID>
        private void FlavourItem_ObjectChangeState(object _object, string member)
        {
            OnPreparationItemsChangeState();

            //if (member == null && !SuspendsObjectChangeStateEvents)
            //    _PreparationItemsChangeState?.Invoke(this, DeviceUpdateEtag);
        }


        ///// <MetaDataID>{5a788c1a-199e-44b4-b0b0-bbe07baf672d}</MetaDataID>
        //internal Dictionary<string, ItemPreparationPlan> predictions = new Dictionary<string, ItemPreparationPlan>();

        /// <MetaDataID>{1ab40191-6416-4db1-8e60-72018f6f2945}</MetaDataID>
        object PreparationPlanStartTimeLock = new object();

        /// <exclude>Excluded</exclude>
        DateTime? _PreparationPlanStartTime;
        /// <MetaDataID>{092f57c6-5b4c-4104-bafb-286c1be565da}</MetaDataID>
        internal DateTime? PreparationPlanStartTime
        {
            get
            {
                lock (PreparationPlanStartTimeLock)
                {
                    return _PreparationPlanStartTime;
                }
            }
            set
            {
                lock (PreparationPlanStartTimeLock)
                {
                    _PreparationPlanStartTime = value;
                }
            }
        }

        ///// <MetaDataID>{099ce3b8-0473-4526-b3bd-c114990d4fd2}</MetaDataID>
        //List<IItemPreparation> ItemsInPreparation;
        ///// <MetaDataID>{e8f2d2d9-d017-4c46-b82b-9ab788e46b0c}</MetaDataID>
        //Dictionary<string, ItemPreparationPlan> GetItemToServingtimespanPredictions()
        //{

        //    try
        //    {
        //        SuspendsObjectChangeStateEvents = true;
        //        List<ItemsPreparationContext> preparationSections = null;
        //        lock (DeviceUpdateLock)
        //        {
        //            preparationSections = this._PreparationSessions.ToList();
        //        }
        //        var preparationStationItems = (from serviceSession in preparationSections
        //                                       from preparationItem in serviceSession.PreparationItems
        //                                       select preparationItem).ToList();



        //        Predictions = GetItemToServingTimespanPredictions(preparationStationItems.OfType<ItemPreparation>().ToList());
        //        return Predictions;
        //    }
        //    finally
        //    {
        //        SuspendsObjectChangeStateEvents = false;
        //    }


        //    //try
        //    //{
        //    //    lock (predictions)
        //    //    {
        //    //        List<ItemsPreparationContext> preparationSections = null;
        //    //        lock (DeviceUpdateLock)
        //    //        {
        //    //            preparationSections = this._PreparationSessions;
        //    //        }
        //    //        var preparationStationItems = (from serviceSession in preparationSections
        //    //                                       from preparationItem in serviceSession.PreparationItems.OrderByDescending(x => x.CookingTimeSpanInMin).OrderBy(x => this.GeAppearanceOrder((x as ItemPreparation).MenuItem))
        //    //                                       select preparationItem).ToList();

        //    //        var itemsInPreparation = preparationStationItems.Where(x => x.State == ItemPreparationState.…nPreparation).OrderBy(x => x.PreparationStartsAt).ToList();

        //    //        if (ItemsInPreparation == null)
        //    //            ItemsInPreparation = itemsInPreparation;
        //    //        else
        //    //        {
        //    //            #region Remove the items in preparation to recalculate preparation time
        //    //            var removedItem = ItemsInPreparation.Where(x => !itemsInPreparation.Contains(x)).FirstOrDefault();
        //    //            if (removedItem != null && predictions.ContainsKey(removedItem.uid) && predictions[removedItem.uid].PreparationStart > DateTime.UtcNow)
        //    //            {
        //    //                for (int i = ItemsInPreparation.IndexOf(removedItem); i < ItemsInPreparation.Count; i++)
        //    //                {
        //    //                    if (predictions.ContainsKey(ItemsInPreparation[i].uid))
        //    //                        predictions.Remove(ItemsInPreparation[i].uid);
        //    //                }
        //    //            }
        //    //            #endregion
        //    //        }

        //    //        DateTime previousePreparationEndsAt = DateTime.UtcNow;
        //    //        foreach (var itemInPreparation in itemsInPreparation.Where(x => ItemsInPreparation.Contains(x) && predictions.ContainsKey(x.uid)))
        //    //        {
        //    //            if (predictions[itemInPreparation.uid].PreparationStart + TimeSpan.FromMinutes(predictions[itemInPreparation.uid].Duration) > previousePreparationEndsAt)
        //    //                previousePreparationEndsAt = predictions[itemInPreparation.uid].PreparationStart + TimeSpan.FromMinutes(predictions[itemInPreparation.uid].Duration);
        //    //        }
        //    //        foreach (var itemInPreparation in itemsInPreparation.Where(x => !ItemsInPreparation.Contains(x) || !predictions.ContainsKey(x.uid)))
        //    //        {

        //    //            //if (!ItemsInPreparation.Contains(itemInPreparation) || !predictions.ContainsKey(itemInPreparation.uid))
        //    //            //{
        //    //            //item was not in preparation mode in the previous calculation or the preparation time must be recalculated

        //    //            if (previousePreparationEndsAt > DateTime.UtcNow)
        //    //                predictions[itemInPreparation.uid] = new ItemPreparationPlan() { PreparationStart = previousePreparationEndsAt, Duration = itemInPreparation.PreparationTimeSpanInMin };
        //    //            else
        //    //                predictions[itemInPreparation.uid] = new ItemPreparationPlan() { PreparationStart = DateTime.UtcNow, Duration = itemInPreparation.PreparationTimeSpanInMin };
        //    //            //}
        //    //            previousePreparationEndsAt = predictions[itemInPreparation.uid].PreparationStart + TimeSpan.FromMinutes(predictions[itemInPreparation.uid].Duration);
        //    //        }

        //    //        ItemsInPreparation = itemsInPreparation;

        //    //        var itemsPendingToPrepare = preparationStationItems.Where(x => x.State == ItemPreparationState.PendingPreparation).ToList();
        //    //        foreach (var itemPendingToPrepare in itemsPendingToPrepare)
        //    //        {
        //    //            predictions[itemPendingToPrepare.uid] = new ItemPreparationPlan() { PreparationStart = previousePreparationEndsAt, Duration = itemPendingToPrepare.PreparationTimeSpanInMin };
        //    //            previousePreparationEndsAt = previousePreparationEndsAt + TimeSpan.FromMinutes(itemPendingToPrepare.PreparationTimeSpanInMin);
        //    //        }


        //    //        var roasting…tems = preparationStationItems.Where(x => x.State == ItemPreparationState.IsRoasting).ToList();

        //    //        foreach (var roasting…tem in roasting…tems)
        //    //            predictions[roasting…tem.uid] = new ItemPreparationPlan() { PreparationStart = roasting…tem.CookingStartsAt.Value, Duration = roasting…tem.CookingTimeSpanInMin };

        //    //        foreach (var prepared…tem in preparationStationItems.Where(x => x.State == ItemPreparationState.IsPrepared))
        //    //            if (predictions.ContainsKey(prepared…tem.uid))
        //    //            {
        //    //                if (predictions[prepared…tem.uid].Duration > 0)
        //    //                {
        //    //                    predictions[prepared…tem.uid].PreparationStart = DateTime.UtcNow;
        //    //                    predictions[prepared…tem.uid].Duration = 0;
        //    //                }
        //    //            }

        //    //        return predictions;
        //    //    }
        //    //}
        //    //catch (Exception error)
        //    //{

        //    //    throw;
        //    //}
        //}

        /// <MetaDataID>{3e5166da-a888-46e4-9778-4c4acfe31411}</MetaDataID>
        internal Dictionary<string, ItemPreparationPlan> GetItemToServingTimespanPredictions()
        {

            try
            {
                ObjectActivated.Task.Wait();
                SuspendsObjectChangeStateEvents = true;
                List<ItemsPreparationContext> preparationSections = null;
                lock (DeviceUpdateLock)
                {
                    preparationSections = this.FoodItemsInProgress.ToList();
                }
                var preparationStationItems = (from serviceSession in preparationSections
                                               from preparationItem in serviceSession.PreparationItems.OrderBy(x => x.AppearanceOrder)
                                               select preparationItem).OfType<ItemPreparation>().ToList();



                //Predictions = GetItemToServingTimespanPredictions(preparationStationItems.OfType<ItemPreparation>().ToList());
                //return Predictions;

                DateTime startTime = DateTime.UtcNow;

                PreparationPlan actionContext = (ServicesContextRunTime.Current.MealsController as MealsController).RebuildPreparationPlan();
                Dictionary<string, ItemPreparationPlan> predictions = new Dictionary<string, ItemPreparationPlan>();


                foreach (var itemPreparation in preparationStationItems)
                {
                    if (itemPreparation.ActivePreparationStation == null)
                    {

                    }
                    if (itemPreparation.State == ItemPreparationState.IsRoasting && itemPreparation.CookingStartsAt.Value != null)
                    {
                        ItemPreparationPlan itemPreparationPlan = new ItemPreparationPlan()
                        {
                            PreparationStart = itemPreparation.CookingStartsAt.Value,
                            Duration = TimeSpanEx.FromMinutes(this.GetCookingTimeSpanInMin(itemPreparation)).TotalMinutes,
                            CookingDuration = 0//TimeSpanEx.FromMinutes(this.GetCookingTimeSpanInMin(itemPreparation)).TotalMinutes
                        };

                        predictions[itemPreparation.uid] = itemPreparationPlan;
                    }
                    else if (itemPreparation.State.IsInFollowingState(ItemPreparationState.IsRoasting))
                    {
                        ItemPreparationPlan itemPreparationPlan = new ItemPreparationPlan()
                        {
                            PreparationStart = itemPreparation.StateTimestamp,
                            Duration = 0,
                            CookingDuration = 0

                        };

                        predictions[itemPreparation.uid] = itemPreparationPlan;
                    }
                    else if (!actionContext.ItemPreparationsStartsAt.ContainsKey(itemPreparation))
                    {
                        ItemPreparationPlan itemPreparationPlan = new ItemPreparationPlan()
                        {
                            PreparationStart = DateTime.UtcNow,
                            Duration = 0,
                            CookingDuration = 0
                        };

                        predictions[itemPreparation.uid] = itemPreparationPlan;

                    }
                    else
                    {
                        ItemPreparationPlan itemPreparationPlan = new ItemPreparationPlan()
                        {
                            PreparationStart = actionContext.ItemPreparationsStartsAt[itemPreparation].Starts,
                            Duration = TimeSpanEx.FromMinutes(this.GetPreparationTimeSpanInMin(itemPreparation)).TotalMinutes,
                            CookingDuration = TimeSpanEx.FromMinutes(this.GetCookingTimeSpanInMin(itemPreparation)).TotalMinutes
                        };

                        predictions[itemPreparation.uid] = itemPreparationPlan;
                    }
                }
                var timeSpan = DateTime.UtcNow - startTime;
                return predictions;
            }
            finally
            {
                SuspendsObjectChangeStateEvents = false;
            }


        }
        /// <MetaDataID>{cf0856b6-a2cf-43a0-98e8-055fc4c070c0}</MetaDataID>
        public Dictionary<string, ItemPreparationPlan> ItemsInPreparation(List<string> itemPreparationUris)
        {



            var clientSessionsItems = (from servicePointPreparationItems in FoodItemsInProgress
                                       from itemPreparation in servicePointPreparationItems.PreparationItems
                                       where itemPreparationUris.Contains(itemPreparation.uid)
                                       group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

            foreach (var clientSessionItems in clientSessionsItems)
                clientSessionItems.clientSession.ItemsInPreparation(clientSessionItems.ClientSessionItems);

            return GetItemToServingTimespanPredictions();
        }


        /// <MetaDataID>{318ebf1b-cb44-4ae3-9f15-e4bb4403199a}</MetaDataID>
        public double PreparationVelocity
        {
            get
            {
                return _PreparationVelocity;
                //if (ItemsPreparationHistory.Count == 0)
                //    return 0;

                //var averageDif = ItemsPreparationHistory.Sum(x => x.DurationDif) / ItemsPreparationHistory.Count;
                //var averagePreparationTimeSpanInMin = this.ItemsPreparationHistory.Sum(x => x.PreparationTimeSpanInMin) / this.ItemsPreparationHistory.Count;

                //string json = OOAdvantech.Json.JsonConvert.SerializeObject(ItemsPreparationHistory);

                //return (averageDif / averagePreparationTimeSpanInMin) * 100;
            }
        }

        /// <MetaDataID>{2579a2ec-c9ae-46a2-bd1f-0f782af292b3}</MetaDataID>
        public Dictionary<string, ItemPreparationPlan> Predictions { get; private set; }
        /// <MetaDataID>{3d5d41d8-2f32-4789-b85c-b2deb44c07ff}</MetaDataID>
        double NormalizeTimeSpanInSec = 15;

        /// <MetaDataID>{baa4b223-d63e-4981-b948-b4be0a8b8dae}</MetaDataID>
        public Dictionary<string, ItemPreparationPlan> ItemsRoasting(List<string> itemPreparationUris)
        {


            var preparationTimeSpan = DateTime.UtcNow - PreparationVelocityMilestone.Value;

            var preparedItems = (from servicePointPreparationItems in FoodItemsInProgress
                                 from itemPreparation in servicePointPreparationItems.PreparationItems
                                 where itemPreparationUris.Contains(itemPreparation.uid)
                                 select itemPreparation).ToList();

            UpdateItemPreparationStatistics(preparedItems, preparationTimeSpan);


            PreparationVelocityMilestone = DateTime.UtcNow;

            var clientSessionsItems = (from preparedItem in preparedItems
                                       group preparedItem by preparedItem.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var clientSessionItems in clientSessionsItems)
                    clientSessionItems.clientSession.ItemsRoasting(clientSessionItems.ClientSessionItems);

                stateTransition.Consistent = true;
            }


            return GetItemToServingTimespanPredictions();
        }
        /// <exclude>Excluded</exclude>
        int _PreparationVelocity = 0;
        /// <MetaDataID>{7c7f7b5b-6d47-4c63-b20b-67988fd41c61}</MetaDataID>
        int PreviousAveragePerc;
        /// <MetaDataID>{b2f4de11-bcf7-499f-84e0-dc8d2b19297f}</MetaDataID>
        List<ItemPreparationTimeSpan> SmoothingItemsPreparationHistory = new List<ItemPreparationTimeSpan>();



        /// <summary>
        /// This method update preparation station statistics
        /// </summary>
        /// <param name="preparedItems">
        /// Defines the items where prepared in timespan
        /// </param>
        /// <param name="preparationTimeSpan">
        /// Defines the timespan where items prepared
        /// </param>
        /// <remarks>
        /// sdfdds d
        /// </remarks>
        /// <MetaDataID>{9992e845-e2d9-4d6c-864c-de2952762af6}</MetaDataID>
        private void UpdateItemPreparationStatistics(List<IItemPreparation> preparedItems, TimeSpan preparationTimeSpan)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                // calculate the default time for all items
                var totalPreparationTimeSpanInMin = preparedItems.Sum(x => x.PreparationTimeSpanInMin);
                // calculate the ratio between the default item preparation time and the default total preparation time
                var normalizedItemsRations = preparedItems.Select(x => x.PreparationTimeSpanInMin / totalPreparationTimeSpanInMin).ToList();
                var previousItemsPreparationUpdate = DateTime.UtcNow - preparationTimeSpan;

                int i = 0;
                foreach (var preparedItem in preparedItems.OfType<ItemPreparation>())
                {
                    var normalizedTimeSpan = TimeSpan.FromMinutes(preparationTimeSpan.TotalMinutes * normalizedItemsRations[i++]);
                    ItemPreparationTimeSpan itemPreparationTimeSpan = new ItemPreparationTimeSpan()
                    {
                        StartsAt = previousItemsPreparationUpdate,
                        EndsAt = previousItemsPreparationUpdate + normalizedTimeSpan,
                        DurationDif = normalizedTimeSpan.TotalMinutes - preparedItem.PreparationTimeSpanInMin,
                        OrgDurationDif = normalizedTimeSpan.TotalMinutes - preparedItem.PreparationTimeSpanInMin,
                        DefaultTimeSpanInMin = preparedItem.PreparationTimeSpanInMin,
                        ItemsPreparationInfo = GetPreparationTimeItemsPreparationInfo(preparedItem.MenuItem),
                        InformationValue = ((double)preparedItems.OfType<ItemPreparation>().Where(x => this.GetPreparationTimeItemsPreparationInfo(x.MenuItem) == GetPreparationTimeItemsPreparationInfo(preparedItem.MenuItem)).Count()) / preparedItems.Count,
                        ActualTimeSpanInMin = normalizedTimeSpan.TotalMinutes

                    };
                    itemPreparationTimeSpan.DurationDifPerc = (itemPreparationTimeSpan.DurationDif / itemPreparationTimeSpan.DefaultTimeSpanInMin) * 100;
                    itemPreparationTimeSpan.OrgDurationDifPerc = (itemPreparationTimeSpan.DurationDif / itemPreparationTimeSpan.DefaultTimeSpanInMin) * 100;





                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemPreparationTimeSpan);
                    PreparationTimeSpans.Add(itemPreparationTimeSpan);

                    previousItemsPreparationUpdate = itemPreparationTimeSpan.EndsAt;

                    if (preparationTimeSpan.TotalSeconds > NormalizeTimeSpanInSec)
                    {
                        // Normalize is necessary when the timespan between now and previous preparation complete is less than NormalizeTimeSpanInSec

                        ItemsPreparationHistory.Clear();
                        ItemsPreparationHistory.Enqueue(itemPreparationTimeSpan);
                    }
                    else
                    {
                        ItemsPreparationHistory.Enqueue(itemPreparationTimeSpan);
                        List<ItemPreparationTimeSpan> normalizedItems = ItemsPreparationHistory.ToList();
                        NormalizeItems(normalizedItems);

                        //NormalizePreparationHistory();
                    }

                    CalculatePreparationVelocity(itemPreparationTimeSpan);
                }
                stateTransition.Consistent = true;
            }

            //else
            //    PreviousAveragePerc = avargePerc;
        }

        /// <MetaDataID>{74243f9d-8326-4598-99d4-e7a289f1a137}</MetaDataID>
        private void CalculatePreparationVelocity(ItemPreparationTimeSpan itemPreparationTimeSpan)
        {
            SmoothingItemsPreparationHistory.Add(itemPreparationTimeSpan);

            var smoothingItemsPreparationHistory = SmoothingItemsPreparationHistory.Where(x => (itemPreparationTimeSpan.EndsAt - x.StartsAt).TotalMinutes < 15).ToList();
            if (smoothingItemsPreparationHistory.Count > 0)
                SmoothingItemsPreparationHistory = smoothingItemsPreparationHistory;

            // Duration difference is 
            var averageDif = SmoothingItemsPreparationHistory.Sum(x => x.DurationDif) / SmoothingItemsPreparationHistory.Count;

            var averagePreparationTimeSpanInMin = SmoothingItemsPreparationHistory.Sum(x => x.DefaultTimeSpanInMin) / SmoothingItemsPreparationHistory.Count;

            var avargePerc = (int)Math.Ceiling((averageDif / averagePreparationTimeSpanInMin) * 100);

            if (Math.Abs(avargePerc - PreviousAveragePerc) < 15 || Math.Abs(avargePerc - _PreparationVelocity) < 15)
            {
                _PreparationVelocity = avargePerc;
                PreviousAveragePerc = _PreparationVelocity;
                SmoothingItemsPreparationHistory.Clear();
            }
        }



        /// <summary>
        /// This method normalize the preparation duration difs starts and end of preparation time of items where prepared as group 
        /// </summary>
        /// <MetaDataID>{c4cac93a-366a-4794-b34d-17aa6f2d0184}</MetaDataID>
        private void NormalizePreparationHistory()
        {

            List<ItemPreparationTimeSpan> itemsPreparationHistory = ItemsPreparationHistory.ToList();

            List<ItemPreparationTimeSpan> normalizedItems = new List<ItemPreparationTimeSpan>();
            double availableTimeInMin = 0;
            foreach (var itemPreparationTimeSpan in itemsPreparationHistory)
            {


                if (itemPreparationTimeSpan.OrgDurationDifPerc < -60)//look like as items prepared as group
                {

                    if (normalizedItems.Count == 0)
                    {
                        int itemIndex = itemsPreparationHistory.IndexOf(itemPreparationTimeSpan);
                        if (itemIndex > 0)
                        {
                            availableTimeInMin = itemsPreparationHistory[itemIndex - 1].OrgDurationDif;
                            if (availableTimeInMin > Math.Abs(itemPreparationTimeSpan.OrgDurationDif))
                            {
                                availableTimeInMin -= Math.Abs(itemPreparationTimeSpan.OrgDurationDif);
                                ItemPreparationTimeSpan delayedItemPreparation = itemsPreparationHistory[itemsPreparationHistory.IndexOf(itemPreparationTimeSpan) - 1];
                                if (!normalizedItems.Contains(delayedItemPreparation))
                                    normalizedItems.Add(delayedItemPreparation);

                                normalizedItems.Add(itemPreparationTimeSpan);
                            }
                        }
                    }
                    else
                    {
                        if (availableTimeInMin > Math.Abs(itemPreparationTimeSpan.OrgDurationDif))
                        {
                            availableTimeInMin -= Math.Abs(itemPreparationTimeSpan.OrgDurationDif);
                            normalizedItems.Add(itemPreparationTimeSpan);
                        }
                    }
                }
                else
                {
                    if (normalizedItems.Count > 0)
                    {
                        NormalizeItems(normalizedItems);
                        normalizedItems.Clear();
                        availableTimeInMin = 0;
                    }
                }


                //ItemPreparationTimeSpan delayedItemPreparation = itemsPreparationHistory.Take(itemsPreparationHistory.IndexOf(itemPreparationTimeSpan)).Reverse().Where(x => x.OrgDurationDifPerc > 50).FirstOrDefault();

                //if (delayedItemPreparation != null && delayedItemPreparation.DurationDif > Math.Abs(itemPreparationTimeSpan.DurationDif))
                //{
                //    delayedItemPreparation.DurationDif = delayedItemPreparation.DurationDif - Math.Abs(itemPreparationTimeSpan.DurationDif);
                //    delayedItemPreparation.PreparationEndsAt -= TimeSpan.FromMinutes(Math.Abs(itemPreparationTimeSpan.DurationDif));
                //    delayedItemPreparation.DurationDifPerc = (delayedItemPreparation.DurationDif / delayedItemPreparation.PreparationTimeSpanInMin) * 100;
                //    itemPreparationTimeSpan.PreviousItemsPreparationUpdate -= TimeSpan.FromMinutes(Math.Abs(itemPreparationTimeSpan.DurationDif));

                //    for (int i = itemsPreparationHistory.IndexOf(delayedItemPreparation) + 1; i < itemsPreparationHistory.IndexOf(itemPreparationTimeSpan); i++)
                //    {
                //        //corrects preparation start and ends time of items between current item and delayed item. 
                //        itemsPreparationHistory[i].PreviousItemsPreparationUpdate -= TimeSpan.FromMinutes(Math.Abs(itemPreparationTimeSpan.DurationDif));
                //        itemsPreparationHistory[i].PreparationEndsAt -= TimeSpan.FromMinutes(Math.Abs(itemPreparationTimeSpan.DurationDif));
                //    }

                //    itemPreparationTimeSpan.DurationDif = 0;
                //    itemPreparationTimeSpan.DurationDifPerc = 0;
                //}
            }
            if (normalizedItems.Count > 0)
            {
                NormalizeItems(normalizedItems);
                normalizedItems.Clear();
            }
        }


        /// <summary>
        /// This method normalize the preparation time of items
        /// in case where user set in prepared state two or three items in same time, the first item seems to have been prepared late
        /// and the next items prepared in very short time
        /// the next code try to canonicalize this action.
        /// Calculates the total time for normalized items and share this time to the normalized items
        /// </summary>
        /// <param name="normalizedItems"></param>
        /// <MetaDataID>{88eee950-a77c-4d38-971b-bf4af3abd949}</MetaDataID>
        // <MetaDataID>{41936091-16f7-4e49-ae10-21bbdd3057f6}</MetaDataID>
        private void NormalizeItems(List<ItemPreparationTimeSpan> normalizedItems)
        {


            ItemPreparationTimeSpan delayedItemPreparation = normalizedItems[0];

            var timespan = (normalizedItems.Last().EndsAt - delayedItemPreparation.StartsAt).TotalMinutes;
            var totalPreparationTimeSpanInMin = normalizedItems.Sum(x => x.DefaultTimeSpanInMin); // calculate the default time for all items
            var normalizedItemsRations = normalizedItems.Select(x => x.DefaultTimeSpanInMin / totalPreparationTimeSpanInMin).ToList(); // calculate the ratio between the default item preparation time and the default total preparation time



            for (int i = 0; i < normalizedItems.Count; i++)
            {
                var normalizedItemPreparationTime = timespan * normalizedItemsRations[i];

                normalizedItems[i].DurationDif = normalizedItemPreparationTime - normalizedItems[i].DefaultTimeSpanInMin;
                normalizedItems[i].DurationDifPerc = (normalizedItems[i].DurationDif / normalizedItems[i].DefaultTimeSpanInMin) * 100;
                normalizedItems[i].EndsAt = normalizedItems[i].StartsAt + TimeSpan.FromMinutes(normalizedItemPreparationTime);
                normalizedItems[i].InformationValue = ((double)normalizedItems.Where(x => x.ItemsPreparationInfo == normalizedItems[i].ItemsPreparationInfo).Count()) / normalizedItems.Count;
                normalizedItems[i].ActualTimeSpanInMin = normalizedItemPreparationTime;

                //the item PreparationEndsAt time is the PreviousItemsPreparationUpdate  for next item
                if (i + 1 < normalizedItems.Count)
                    normalizedItems[i + 1].StartsAt = normalizedItems[i].EndsAt;
            }
        }


        /// <MetaDataID>{818687a0-bd81-48e6-8018-e6a99351c9eb}</MetaDataID>
        public Dictionary<string, ItemPreparationPlan> ItemsServing(List<string> itemPreparationUris)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                var clientSessionsItems = (from servicePointPreparationItems in FoodItemsInProgress
                                           from itemPreparation in servicePointPreparationItems.PreparationItems
                                           where itemPreparationUris.Contains(itemPreparation.uid)
                                           group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                           select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

                foreach (var clientSessionItems in clientSessionsItems)
                    clientSessionItems.clientSession.ItemsServing(clientSessionItems.ClientSessionItems);
                stateTransition.Consistent = true;
            }

            return GetItemToServingTimespanPredictions();
        }


        /// <MetaDataID>{a7dbebb6-dce3-49ac-993f-ca5bb5cd8023}</MetaDataID>
        Queue<ItemPreparationTimeSpan> ItemsPreparationHistory = new Queue<ItemPreparationTimeSpan>();
        /// <MetaDataID>{b2502860-c9af-44cf-8f10-d0a221986c7b}</MetaDataID>
        public Dictionary<string, ItemPreparationPlan> ItemsPrepared(List<string> itemPreparationUris)
        {

            //ServicesContextRunTime.Current.MealsController.RebuildPreparationPlanLastTime=DateTime.UtcNow+TimeSpan.FromMinutes()
            var preparedItems = (from servicePointPreparationItems in FoodItemsInProgress
                                 from itemPreparation in servicePointPreparationItems.PreparationItems
                                 where itemPreparationUris.Contains(itemPreparation.uid) && itemPreparation.State.IsInTheSameOrPreviousState(ItemPreparationState.InPreparation)
                                 select itemPreparation).ToList();

            if (preparedItems.Count > 0)
            {
                var preparationTimeSpan = DateTime.UtcNow - PreparationVelocityMilestone.Value;

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    UpdateItemPreparationStatistics(preparedItems, preparationTimeSpan);
                    PreparationVelocityMilestone = DateTime.UtcNow;
                    stateTransition.Consistent = true;
                }

            }

            preparedItems = (from servicePointPreparationItems in FoodItemsInProgress
                             from itemPreparation in servicePointPreparationItems.PreparationItems
                             where itemPreparationUris.Contains(itemPreparation.uid)
                             select itemPreparation).ToList();

            var clientSessionsItems = (from preparedItem in preparedItems
                                       group preparedItem by preparedItem.ClientSession into ClientSessionItems
                                       select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var clientSessionItems in clientSessionsItems)
                    clientSessionItems.clientSession.ItemsPrepared(clientSessionItems.ClientSessionItems);

                stateTransition.Consistent = true;
            }


            return GetItemToServingTimespanPredictions();


        }
        /// <MetaDataID>{4175b752-9249-4adf-86a0-dd5bf2f0d069}</MetaDataID>
        volatile bool SuspendsObjectChangeStateEvents;
        /// <MetaDataID>{11867ed0-6ab1-4df8-aa65-8d36a2e1cc7d}</MetaDataID>
        private int DeviceConnectionStatusChecksNumber;

        /// <MetaDataID>{70023458-eb6b-4245-8003-3668bc9253ec}</MetaDataID>
        public Dictionary<string, ItemPreparationPlan> CancelLastPreparationStep(List<string> itemPreparationUris)
        {

            try
            {
                SuspendsObjectChangeStateEvents = true;
                var clientSessionsItems = (from servicePointPreparationItems in FoodItemsInProgress
                                           from itemPreparation in servicePointPreparationItems.PreparationItems
                                           where itemPreparationUris.Contains(itemPreparation.uid)
                                           group itemPreparation by itemPreparation.ClientSession into ClientSessionItems
                                           select new { clientSession = ClientSessionItems.Key, ClientSessionItems = ClientSessionItems.ToList() }).ToList();

                foreach (var clientSessionItems in clientSessionsItems)
                    clientSessionItems.clientSession.CancelLastPreparationStep(clientSessionItems.ClientSessionItems);
                return GetItemToServingTimespanPredictions();
            }
            finally
            {
                SuspendsObjectChangeStateEvents = false;
            }

        }

        /// <MetaDataID>{623dfb4d-50fe-47cc-be27-d8cd809ad99a}</MetaDataID>
        public void AssignCodeCardsToSessions(List<string> codeCards)
        {
            foreach (var codeCard in codeCards)
            {
                foreach (var servicePointPreparationItems in FoodItemsInProgress)
                {
                    if (string.IsNullOrWhiteSpace(servicePointPreparationItems.CodeCard))
                        servicePointPreparationItems.CodeCard = codeCard;
                }
            }
            OnPreparationItemsChangeState();



        }

        /// <MetaDataID>{3083f4f5-9be3-4a22-9725-f571f8b2264a}</MetaDataID>
        public IPreparationStation NewSubStation()
        {
            IPreparationStation preparationStation = null;

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                preparationStation = new PreparationStation(_ServicesContextIdentity);
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





    ///// <MetaDataID>{0241f6f2-d035-4ec2-91ee-f2b41613abe3}</MetaDataID>
    //[BackwardCompatibilityID("{0241f6f2-d035-4ec2-91ee-f2b41613abe3}")]
    //class PreparationData
    //{
    //    /// <MetaDataID>{b28846c4-32c3-4318-b3ad-03536e7cf5ee}</MetaDataID>
    //    public PreparationData()
    //    {

    //    }
    //    /// <MetaDataID>{a3a3dd1c-2b8f-494e-a91b-7662e37b40b8}</MetaDataID>
    //    public ItemPreparation ItemPreparation;

    //    /// <MetaDataID>{12f62d94-bd1a-4c95-8bdb-4e58145b4469}</MetaDataID>
    //    public IPreparationStationRuntime PreparationStationRuntime;

    //    ///// <MetaDataID>{644e17a2-c58b-413c-bc65-b9b8d39c0729}</MetaDataID>
    //    //public TimeSpan Duration;

    //    /// <MetaDataID>{587c172a-2068-4dbc-bcf6-3a0039d17506}</MetaDataID>
    //    //public double CookingTimeSpanInMin { get; internal set; }
    //    ///// <MetaDataID>{e580c37e-f0b6-4f71-b0b6-0aa31e35e730}</MetaDataID>
    //    //public double PreparationTimeSpanInMin { get; internal set; }
    //}


}