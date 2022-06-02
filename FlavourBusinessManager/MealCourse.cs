using FlavourBusinessFacade.RoomService;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Linq;
using System.Collections.Generic;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessManager.ServicesContextResources;
using FlavourBusinessFacade.ServicesContextResources;

namespace FlavourBusinessManager.RoomService
{



  
    /// <summary>
    /// A meal consisting of multiple dishes (meal courses)
    /// Most Western-world multicourse meals follow a standard sequence.
    /// MealCourse class defines the food items where belongs to the same course 
    /// for instance hors d'oeuvre or appetizer,main dish , dessert
    /// </summary>
    ///<MetaDataID>{986d1f45-2bef-4302-9d5c-b98141c24555}</MetaDataID>
    [BackwardCompatibilityID("{986d1f45-2bef-4302-9d5c-b98141c24555}")]
    [Persistent()]
    public class MealCourse : System.MarshalByRefObject, IMealCourse
    {

        /// <exclude>Excluded</exclude>
        bool _Synchronized;
        /// <MetaDataID>{18314fb4-00f4-46d1-a89f-93b54bb013d5}</MetaDataID>
        [PersistentMember(nameof(_Synchronized))]
        [BackwardCompatibilityID("+11")]
        public bool Synchronized
        {
            get => _Synchronized;
            set
            {
                if (_Synchronized != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Synchronized = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _DurationInMinutes;

        /// <MetaDataID>{73175278-0c8f-45cd-a78f-3c49557633cb}</MetaDataID>
        [PersistentMember(nameof(_DurationInMinutes))]
        [BackwardCompatibilityID("+7")]
        public double DurationInMinutes
        {
            get => _DurationInMinutes;
            set
            {
                if (_DurationInMinutes != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DurationInMinutes = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        string _MealCourseTypeUri;

        /// <MetaDataID>{dbf7beba-2251-433e-80cc-9052b24a0189}</MetaDataID>
        [PersistentMember(nameof(_MealCourseTypeUri))]
        [BackwardCompatibilityID("+6")]
        public string MealCourseTypeUri
        {
            get => _MealCourseTypeUri;
            set
            {

                if (_MealCourseTypeUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealCourseTypeUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{8b2a7043-450a-4ff3-825a-f505a07ffd2d}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+5")]
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{22f12ba9-4c26-4197-ad51-92b7981d601a}</MetaDataID>
        protected MealCourse()
        {
            //_FoodItemsInProgress = new List<ItemsPreparationContext>();
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _FoodItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{1f941000-ce6f-4ec6-85f6-736ad57cf9a6}</MetaDataID>
        [PersistentMember(nameof(_FoodItems))]
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        public IList<IItemPreparation> FoodItems => _FoodItems.ToThreadSafeList();


        /// <exclude>Excluded</exclude>
        DateTime? _StartsAt;

        /// <MetaDataID>{963bb0ae-3234-4bf5-b7e4-9a313dcb9531}</MetaDataID>
        [PersistentMember(nameof(_StartsAt))]
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        public System.DateTime? StartsAt
        {
            get => _StartsAt;
            set
            {
                if (_StartsAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StartsAt = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        System.DateTime? _ServedAtForecast;

        /// <MetaDataID>{19242092-38cd-4f7d-b7dc-77abc4dfb56e}</MetaDataID>
        [PersistentMember(nameof(_ServedAtForecast))]
        [BackwardCompatibilityID("+8")]
        public System.DateTime? ServedAtForecast
        {
            get => _ServedAtForecast;
            set
            {
                if (_ServedAtForecast != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServedAtForecast = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2b29e1c8-9c66-4412-96f3-cf276f528e44}</MetaDataID>
        DateTime? _PreparedAt;

        /// <MetaDataID>{f5802cfc-5c51-4da8-891f-c48fa3abd3b3}</MetaDataID>
        [PersistentMember(nameof(_PreparedAt))]
        [BackwardCompatibilityID("+3")]
        public System.DateTime? ServedAt
        {
            get => _PreparedAt;
            set => throw new NotImplementedException();
        }


        /// <exclude>Excluded</exclude>
        int? _MealCourseTypeOrder;
        /// <MetaDataID>{b5c8f968-c936-4c77-aef2-a4d107446aaf}</MetaDataID>
        public int MealCourseTypeOrder
        {
            get
            {
                if (_MealCourseTypeOrder == null)
                    _MealCourseTypeOrder = (Meal as Meal).MealType.Courses.OfType<MenuModel.MealCourseType>().Select(x => ObjectStorage.GetStorageOfObject(x).GetPersistentObjectUri(x)).ToList().IndexOf(_MealCourseTypeUri);
                return _MealCourseTypeOrder.Value;
            }

        }

        /// <MetaDataID>{a5d66845-0435-4312-9423-6f482b945eb8}</MetaDataID>
        public MealCourseType MealCourseType
        {
            get
            {
                return (Meal as Meal).MealType.Courses.OfType<MenuModel.MealCourseType>().Where(x => ObjectStorage.GetStorageOfObject(x).GetPersistentObjectUri(x) == _MealCourseTypeUri).FirstOrDefault();
            }
        }

        /// <exclude>Excluded</exclude>
        ItemPreparationState _PreparationState;

        /// <MetaDataID>{fdf1dea6-88ed-41fa-8302-6d5392ea0359}</MetaDataID>
        [PersistentMember(nameof(_PreparationState))]
        [BackwardCompatibilityID("+4")]
        public ItemPreparationState PreparationState
        {
            get => _PreparationState;
            set
            {

                if (_PreparationState != value)
                {
                    var activeWaiters = ServicesContextRunTime.Current.GetActiveShiftWorks().Where(x => x.EndsAt > System.DateTime.UtcNow).Select(x => x.Worker).OfType<IWaiter>().ToList();
                    if (activeWaiters.Count > 0)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _PreparationState = value;
                            stateTransition.Consistent = true;
                        }

                        if (value == ItemPreparationState.Serving)
                            ServicesContextRunTime.Current.MealItemsReadyToServe(Meal.Session.ServicePoint as ServicePoint);
                    }
                }

            }
        }

        /// <MetaDataID>{a071224d-7336-4f53-a34f-69e48fc81c43}</MetaDataID>
        object MealLock = new object();

        /// <MetaDataID>{c89e44cf-2f2a-4632-a055-bb05232b5c23}</MetaDataID>
        internal void ChangeMeal(Meal meal)
        {
            lock (MealLock)
            {
                if (_Meal.Value != null)
                    _Meal.Value = meal;
            }

            if (Transaction.Current != null)
            {
                Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    if (transaction.Status == TransactionStatus.Committed)
                    {
                        ObjectChangeState?.Invoke(this, nameof(Meal));
                    }
                };
            }
            else
            {
                ObjectChangeState?.Invoke(this, nameof(Meal));
            }

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMeal> _Meal = new OOAdvantech.Member<IMeal>();


        /// <MetaDataID>{65995fe0-c0a3-47e3-90a1-58523ea31b41}</MetaDataID>
        [PersistentMember(nameof(_Meal))]
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+9")]
        public IMeal Meal => _Meal.Value;

     
        /// <summary>
        /// Multi thread synchronization object
        /// </summary>
        /// <MetaDataID>{e1c88e32-243d-4257-82a0-5a8119bf34e7}</MetaDataID>
        object FoodItemsInProgressLock = new object();


        /// <exclude>Excluded</exclude>
        List<ItemsPreparationContext> _FoodItemsInProgress;

        /// <summary>
        /// Defines the meal course items grouped by preparation station where it prepares them.
        /// </summary>
        // <MetaDataID>{95a3e0b7-a301-429b-a8fe-023518cad466}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> FoodItemsInProgress
        {
            get
            {
                lock (FoodItemsInProgressLock)
                {
                    if (_FoodItemsInProgress == null)
                    {
                        _FoodItemsInProgress = new List<ItemsPreparationContext>();
                        foreach (var itemPreparation in _FoodItems)
                        {
                            var itemsPreparationContext = itemPreparation.FindItemsPreparationContext();
                            if (itemsPreparationContext == null)
                            {
                                itemsPreparationContext = new ItemsPreparationContext(this, itemPreparation.PreparationStation, new List<IItemPreparation>() { itemPreparation });
                                _FoodItemsInProgress.Add(itemsPreparationContext);
                            }
                            else
                                itemsPreparationContext.AddPreparationItem(itemPreparation);
                            if (!_FoodItemsInProgress.Contains(itemsPreparationContext))
                                _FoodItemsInProgress.Add(itemsPreparationContext);
                        }

                        foreach (var itemsPreparationContext in this._FoodItemsInProgress)
                        {
                            var commonItemPreparationState = itemsPreparationContext.PreparationItems.GetMinimumCommonItemPreparationState();
                            itemsPreparationContext.PreparationState = commonItemPreparationState;
                        }

                    }
                }
                return _FoodItemsInProgress;

                //List<ItemsPreparationContext> foodItemsInProgress = (from itemPreparation in FoodItems
                //                                                     where itemPreparation.State == ItemPreparationState.PendingPreparation ||
                //                                                     itemPreparation.State == ItemPreparationState.PreparationDelay ||
                //                                                     itemPreparation.State == ItemPreparationState.ÉnPreparation ||
                //                                                     itemPreparation.State == ItemPreparationState.IsRoasting ||
                //                                                     itemPreparation.State == ItemPreparationState.IsPrepared ||
                //                                                     itemPreparation.State == ItemPreparationState.Serving ||
                //                                                     itemPreparation.State == ItemPreparationState.OnRoad
                //                                                     group itemPreparation by itemPreparation.PreparationStation into itemsUnderPreparation
                //                                                     select new ItemsPreparationContext(this, itemsUnderPreparation.Key, itemsUnderPreparation.ToList())).ToList();
                //return foodItemsInProgress;

            }
        }

        /// <MetaDataID>{d5bb9342-16b0-4c93-8b7b-07b0c5c5eb36}</MetaDataID>
        internal void Monitoring()
        {
            if (FoodItems.Where(x => x.State == ItemPreparationState.Serving).Count() == FoodItems.Count)
            {
                if ((System.DateTime.Now - FoodItems.OrderBy(x => x.StateTimestamp).Last().StateTimestamp).TotalMinutes > 1)
                {
                    if (PreparationState != ItemPreparationState.Serving)
                        PreparationState = ItemPreparationState.Serving;
                }
            }
        }



        /// <MetaDataID>{e146e6e4-9b86-429f-b4d2-171b340d8937}</MetaDataID>
        [CachingDataOnClientSide]
        public FlavourBusinessFacade.EndUsers.SessionData SessionData
        {
            get
            {
                var fbstorage = ServicePointRunTime.ServicesContextRunTime.Current.Storages.Where(x => x.StorageIdentity == (Meal.Session as ServicesContextResources.FoodServiceSession).MenuStorageIdentity).FirstOrDefault();
                if (fbstorage != null && (Meal.Session as ServicesContextResources.FoodServiceSession).Menu == null)
                {
                    //IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity);
                    string organizationIdentity = ServicePointRunTime.ServicesContextRunTime.Current.OrganizationIdentity;
                    string versionSuffix = "";
                    if (!string.IsNullOrWhiteSpace(fbstorage.Version))
                        versionSuffix = "/" + fbstorage.Version;
                    else
                        versionSuffix = "";

                    var storageUrl = RawStorageCloudBlob.RootUri + string.Format("/usersfolder/{0}/Menus/{1}{3}/{2}.json", organizationIdentity, fbstorage.StorageIdentity, fbstorage.Name, versionSuffix);
                    var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);
                    var storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };
                    (Meal.Session as ServicesContextResources.FoodServiceSession).Menu = storageRef;
                }

                var defaultMealTypeUri = Meal.Session.ServicePoint.ServesMealTypesUris.FirstOrDefault();
                var servedMealTypesUris = Meal.Session.ServicePoint.ServesMealTypesUris.ToList();
                if (defaultMealTypeUri == null)
                {
                    defaultMealTypeUri = Meal.Session.ServicePoint.ServiceArea.ServesMealTypesUris.FirstOrDefault();
                    servedMealTypesUris = Meal.Session.ServicePoint.ServiceArea.ServesMealTypesUris.ToList();
                }

                FlavourBusinessFacade.EndUsers.SessionData sessionData = new FlavourBusinessFacade.EndUsers.SessionData() { DefaultMealTypeUri = defaultMealTypeUri, ServedMealTypesUris = servedMealTypesUris, FoodServiceSession = Meal.Session, ServicePointIdentity = Meal.Session.ServicePoint.ServicesPointIdentity, Menu = (Meal.Session as ServicesContextResources.FoodServiceSession).Menu, ServicesPointName = Meal.Session.ServicePoint.Description, ServicesContextLogo = "Pizza Hut" };
                return sessionData;
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IServingBatch> _ServingBatches = new OOAdvantech.Collections.Generic.Set<IServingBatch>();

        /// <MetaDataID>{6ad7a9db-e78f-4fe6-baf6-151bb81a730f}</MetaDataID>
        [PersistentMember(nameof(_ServingBatches))]
        [BackwardCompatibilityID("+10")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public List<IServingBatch> ServingBatches => _ServingBatches.ToThreadSafeList();

        /// <MetaDataID>{f98cfc40-f73f-4f0a-9868-566afbc8ff71}</MetaDataID>
        public bool ItsTooLateForChange(ItemPreparation itemPreparation)
        {
            if (itemPreparation.SelectedMealCourseTypeUri == MealCourseTypeUri)
            {
                var preparationStationRuntime = PreparationStation.GetPreparationStationFor(itemPreparation);

                if (preparationStationRuntime != null)
                {
                    var itemsPreparationContext = _FoodItemsInProgress.Where(x => x.PreparationStationIdentity == (preparationStationRuntime as IPreparationStation).PreparationStationIdentity).FirstOrDefault();

                    if (itemsPreparationContext != null)
                    {
                        if (itemsPreparationContext.PreparationItems.Any(x => x.State.IsInFollowingState(ItemPreparationState.PendingPreparation)))
                        {
                            //ServedAtForecast
                        }
                    }

                    //PreparationStation.GetPreparedAtForecast(itemPreparation, this);
                    //itemPreparation.PreparedAtForecast
                    //PreparationStation.GetPreparationData(itemPreparation).Duration


                    //this.FoodItems.Any(x => x.State.IsInTheSameOrPreviousState(ItemPreparationState.PendingPreparation))
                }


                //foreach (var itemPreparation in _FoodItems)
                //{
                //    var itemsPreparationContext = itemPreparation.FindItemsPreparationContext();
                //    if (itemsPreparationContext == null)
                //    {
                //        itemsPreparationContext = new ItemsPreparationContext(this, itemPreparation.PreparationStation, new List<IItemPreparation>() { itemPreparation });
                //        _FoodItemsInProgress.Add(itemsPreparationContext);
                //    }
                //    else
                //        itemsPreparationContext.PreparationItems.Add(itemPreparation);
                //}

                //foreach (var itemsPreparationContext in this.FoodItemsInProgress)
                //{
                //    var commonItemPreparationState = itemsPreparationContext.PreparationItems.GetMinimumCommonItemPreparationState();
                //    itemsPreparationContext.PreparationState = commonItemPreparationState;
                //}


                // if there is at least on item with state above  ÉnPreparation the course is closed to changes
                // return this._FoodItems.ToThreadSafeList().Any(x => x.State.IsInFollowingState(ItemPreparationState.PendingPreparation));
            }
            return false;

        }

        /// <exclude>Excluded</exclude>
        IMealCourse _Previous;
        /// <MetaDataID>{1135b8be-43f3-465a-ba4a-7d1861e7b3d7}</MetaDataID>
        [PersistentMember(nameof(_Previous))]
        [BackwardCompatibilityID("+12")]
        public IMealCourse Previous
        {
            get => _Previous;
            set
            {
                if (_Previous != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Previous = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        IMealCourse _Next;

        /// <MetaDataID>{d6b41929-cd83-497d-9b01-f0af04be952e}</MetaDataID>
        [PersistentMember(nameof(_Next))]
        [BackwardCompatibilityID("+14")]
        public IMealCourse Next { get => _Next; }


        /// <MetaDataID>{d91b18f4-9d77-4881-8cf6-1777b356c2c6}</MetaDataID>
        public IMealCourse HeaderCourse
        {
            get
            {
                if (Previous == null)
                    return this;
                else
                    return Previous.HeaderCourse;
            }
        }

        /// <exclude>Excluded</exclude>
        int _SortID;

        /// <MetaDataID>{4d905191-7c6c-48a3-902d-5596c9a92a67}</MetaDataID>
        [PersistentMember(nameof(_SortID))]
        [BackwardCompatibilityID("+13")]
        public int SortID
        {
            get => _SortID;
            set
            {
                if (_SortID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SortID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <MetaDataID>{7b6700b2-c8a4-4548-be19-657614416518}</MetaDataID>
        public readonly MealCourseType mealCourseType;

        /// <MetaDataID>{e6899983-8b85-4e28-84f4-8a8199511318}</MetaDataID>
        public MealCourse(MealCourseType mealCourseType, List<ItemPreparation> itemPreparations, Meal meal)
        {
            this.mealCourseType = mealCourseType;
            _MealCourseTypeUri = ObjectStorage.GetStorageOfObject(mealCourseType).GetPersistentObjectUri(mealCourseType);
            _DurationInMinutes = mealCourseType.DurationInMinutes;
            _Name = mealCourseType.Name;
            _Meal.Value = meal;
            _FoodItemsInProgress = new List<ItemsPreparationContext>();
            foreach (var flavourItem in itemPreparations)
                AddItem(flavourItem);


            foreach (var itemsPreparationContext in this.FoodItemsInProgress)
            {
                var commonItemPreparationState = itemsPreparationContext.PreparationItems.GetMinimumCommonItemPreparationState();
                itemsPreparationContext.PreparationState = commonItemPreparationState;
            }

            (meal.Session as FoodServiceSession).ObjectChangeState += MealSession_ObjectChangeState;
            _SortID = (ServicePointRunTime.ServicesContextRunTime.Current.MealsController as MealsController).GetNextSortingID();
        }

        /// <MetaDataID>{cebbd65d-3108-47bf-933a-371f815eacbe}</MetaDataID>
        private void MealSession_ObjectChangeState(object _object, string member)
        {
            ObjectChangeState?.Invoke(this, member);
        }



        /// <MetaDataID>{ed457de3-cf46-443e-a9cc-73340c1a1294}</MetaDataID>
        private void FlavourItem_ObjectChangeState(object _object, string member)
        {
            ObjectChangeState?.Invoke(this, nameof(FoodItems));
        }

        /// <MetaDataID>{b75dd0be-1b93-4c0d-8c13-f368e2c5a980}</MetaDataID>
        public void RaiseItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {
            ItemsStateChanged?.Invoke(newItemsState);

            foreach (var itemsPreparationContext in this.FoodItemsInProgress)
            {
                var commonItemPreparationState = itemsPreparationContext.PreparationItems.GetMinimumCommonItemPreparationState();
                if (commonItemPreparationState == ItemPreparationState.Serving && itemsPreparationContext.PreparationState < ItemPreparationState.Serving)
                {
                    //itemsPreparationContext.PreparationState = commonItemPreparationState;
                    ServicePointRunTime.ServicesContextRunTime.Current.MealItemsReadyToServe(Meal.Session.ServicePoint as ServicePoint);
                }
                itemsPreparationContext.PreparationState = commonItemPreparationState;
            }

            //foreach(var itemsPreparationContext in this.FoodItemsInProgress)
            //{
            //    itemsPreparationContext.PreparationState
            //}
        }
        public event ItemsStateChangedHandle ItemsStateChanged;

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{fa1a0f37-108e-478c-83d4-4f095498cef6}</MetaDataID>
        public void AddItem(IItemPreparation itemPreparation)
        {
            ItemPreparation flavourItem = itemPreparation as ItemPreparation;
            lock (FoodItemsInProgressLock)
            {
                if (!_FoodItems.Contains(flavourItem))
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FoodItems.Add(flavourItem);
                        stateTransition.Consistent = true;
                    }
                    flavourItem.ObjectChangeState += FlavourItem_ObjectChangeState;
                    if (flavourItem.State == ItemPreparationState.Committed)
                    {
                        if (flavourItem.MenuItem == null)
                            flavourItem.LoadMenuItem();

                        var preparationStationRuntime = PreparationStation.GetPreparationStationFor(flavourItem);
                        if (preparationStationRuntime != null)
                        {
                            (preparationStationRuntime as PreparationStation).AssignItemPreparation(flavourItem);
                            flavourItem.State = ItemPreparationState.PreparationDelay;
                        }
                        else
                        {
                            flavourItem.State = ItemPreparationState.Serving;
                            flavourItem.PreparedAtForecast = DateTime.UtcNow;
                        }

                        CashierStation cashierStation = (Meal.Session as FoodServiceSession).CashierStation as CashierStation;
                        cashierStation.AssignItemPreparation(flavourItem);

                    }


                    var itemsPreparationContext = itemPreparation.FindItemsPreparationContext();

                    if (itemsPreparationContext == null)
                    {

                        itemsPreparationContext = new ItemsPreparationContext(this, itemPreparation.PreparationStation, new List<IItemPreparation>() { itemPreparation });
                        _FoodItemsInProgress.Add(itemsPreparationContext);

                    }
                    else
                    {
                        if (!_FoodItemsInProgress.Contains(itemsPreparationContext))
                            FoodItemsInProgress.Add(itemsPreparationContext);
                        itemsPreparationContext.AddPreparationItem(itemPreparation);
                    }
                }

                ObjectChangeState?.Invoke(this, nameof(FoodItems));
            }
        }

        /// <MetaDataID>{9b5fa430-11c3-4ad7-98c5-749b4d06c186}</MetaDataID>
        public void RemoveItem(IItemPreparation itemPreparation)
        {
            if (_FoodItems.Contains(itemPreparation))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {

                    lock (FoodItemsInProgressLock)
                    {
                        var itemsPreparationContext = itemPreparation.FindItemsPreparationContext();
                        if (itemsPreparationContext.PreparationItems.Contains(itemPreparation))
                            itemsPreparationContext.PreparationItems.Remove(itemPreparation);
                    }

                    _FoodItems.Remove(itemPreparation);
                    (itemPreparation as ItemPreparation).ObjectChangeState -= FlavourItem_ObjectChangeState;



                    stateTransition.Consistent = true;
                }




                ObjectChangeState?.Invoke(this, nameof(FoodItems));
            }

        }
        /// <MetaDataID>{ce36f3b9-7c45-48df-b766-2f622eb00589}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {

            ServicePointRunTime.ServicesContextRunTime.Current.ObjectChangeState += ServicesContextRunTime_ObjectChangeState;
            foreach (var foodItem in _FoodItems.OfType<ItemPreparation>())
                foodItem.ObjectChangeState += FlavourItem_ObjectChangeState;

         

            (_Meal.Value.Session as FoodServiceSession).ObjectChangeState += MealSession_ObjectChangeState;
        }

        private void ServicesContextRunTime_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(ServicesContextRunTime.OperativeRestaurantMenu))
                ObjectChangeState?.Invoke(this, nameof(SessionData));

        }





        /// <MetaDataID>{72860a38-cffd-4a68-807b-4c2ece8cddc5}</MetaDataID>
        internal static void AssignMealCourseToItem(ItemPreparation flavourItem)
        {
            //flavourItem.ClientSession.ServicePoint.
        }


    }

    /// <MetaDataID>{a4f7a4df-8b25-4fe0-9ed1-046806c3ee5d}</MetaDataID>
    public static class ItemPreparationContextHelper
    {
        /// <MetaDataID>{2ff27f48-6c0e-463b-993b-34fbb35cc52a}</MetaDataID>
        public static ItemsPreparationContext FindItemsPreparationContext(this IItemPreparation itemPreparation)
        {
          
            if (itemPreparation.PreparationStation != null)
                return (itemPreparation.PreparationStation as PreparationStation).PreparationSessions.Where(x => x.PreparationStationIdentity == itemPreparation.PreparationStation.PreparationStationIdentity&&x.MealCourse== itemPreparation.MealCourse).FirstOrDefault();
            else
                return itemPreparation.MealCourse?.FoodItemsInProgress?.Where(x => x.PreparationStationIdentity == ItemsPreparationContext.TradeProductsStationIdentity).FirstOrDefault();
        }

        /// <MetaDataID>{e8a414af-4141-45ef-bc7a-5c7081052f7b}</MetaDataID>
        public static ItemPreparationState GetMinimumCommonItemPreparationState(this List<IItemPreparation> foodItems)
        {
            var ss = Enum.GetValues(typeof(ItemPreparationState)).OfType<ItemPreparationState>().OrderByDescending(x => (int)x).ToArray();
            ItemPreparationState commonState = ItemPreparationState.New;
            foreach (int i in Enum.GetValues(typeof(ItemPreparationState)).OfType<ItemPreparationState>().OrderBy(x => (int)x))
            {
                if (foodItems.OfType<ItemPreparation>().All(x => x.IsIntheSameOrFollowingState((ItemPreparationState)i)))
                    commonState = (ItemPreparationState)i;
                else
                    break;
            }
            return commonState;
        }

        ///// <MetaDataID>{4e384189-294f-4979-87dc-0a2a22256286}</MetaDataID>
        //public static TimeSpan getPreparationDuration(this List<IItemPreparation> foodItems)
        //{
        //    var foodItemPreparationData = foodItems.OfType<ItemPreparation>().Select(x => new { foodItem = x, duration = ServicesContextResources.PreparationStation.GetPreparationData(x).Duration }).OrderBy(x => x.duration).LastOrDefault();
        //    if (foodItemPreparationData == null)
        //        return TimeSpan.FromSeconds(0);

        //    return foodItemPreparationData.duration;

        //}

        //public static DateTime GetPreparedAtForecast(this ItemsPreparationContext itemsPreparationContext)
        //{
        //    var firstPreparationItem = itemsPreparationContext.PreparationItems.FirstOrDefault();

        //    DateTime preparedAtForecast = DateTime.UtcNow;
        //    if (firstPreparationItem != null && firstPreparationItem.PreparationStation != null)
        //    {

        //        PreparationStation preparationStation = firstPreparationItem.PreparationStation as PreparationStation;
        //        foreach (var preparationItem in itemsPreparationContext.PreparationItems)
        //        {
        //            if (preparationStation.predictions.ContainsKey(preparationItem.uid)&& preparationStation.predictions[preparationItem.uid].PreparationStart + TimeSpan.FromMinutes(preparationStation.predictions[preparationItem.uid].Duration) > preparedAtForecast)
        //                preparedAtForecast = preparationStation.predictions[preparationItem.uid].PreparationStart + TimeSpan.FromMinutes(preparationStation.predictions[preparationItem.uid].Duration);

        //        }
        //    }
        //    itemsPreparationContext.PreparedAtForecast = preparedAtForecast;
        //    return preparedAtForecast;
        //}
    }


}