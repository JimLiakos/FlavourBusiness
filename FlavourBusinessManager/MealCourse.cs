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

namespace FlavourBusinessManager.RoomService
{



    /// <summary>
    ///Defines a service section with time constraints and includes the preparation of the food-coffee items, the serving - takeaway - home delivery.
    /// </summary>
    ///<MetaDataID>{986d1f45-2bef-4302-9d5c-b98141c24555}</MetaDataID>
    [BackwardCompatibilityID("{986d1f45-2bef-4302-9d5c-b98141c24555}")]
    [Persistent()]
    public class MealCourse : System.MarshalByRefObject, IMealCourse
    {

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


        /// <MetaDataID>{716e1a7a-9abf-43a1-9d52-e49af11c518c}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMeal> _Meal = new OOAdvantech.Member<IMeal>();


        /// <MetaDataID>{65995fe0-c0a3-47e3-90a1-58523ea31b41}</MetaDataID>
        [PersistentMember(nameof(_Meal))]
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+9")]
        public FlavourBusinessFacade.RoomService.IMeal Meal => _Meal.Value;


        /// <MetaDataID>{95a3e0b7-a301-429b-a8fe-023518cad466}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> FoodItemsInProgress
        {
            get
            {

                List<ItemsPreparationContext> foodItemsInProgress = (from itemPreparation in FoodItems
                                                                     where itemPreparation.State == ItemPreparationState.PendingPreparation ||
                                                                     itemPreparation.State == ItemPreparationState.PreparationDelay ||
                                                                     itemPreparation.State == ItemPreparationState.ÉnPreparation ||
                                                                     itemPreparation.State == ItemPreparationState.IsRoasting ||
                                                                     itemPreparation.State == ItemPreparationState.IsPrepared ||
                                                                     itemPreparation.State == ItemPreparationState.Serving||
                                                                     itemPreparation.State == ItemPreparationState.OnRoad
                                                                     group itemPreparation by itemPreparation.PreparationStation into itemsUnderPreparation
                                                                     select new ItemsPreparationContext(this, itemsUnderPreparation.Key, itemsUnderPreparation.ToList())).ToList();
                return foodItemsInProgress;

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
                    IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity);
                    string organizationIdentity = flavoursServicesContext.Owner.Identity;
                    string versionSuffix = "";
                    if (!string.IsNullOrWhiteSpace(fbstorage.Version))
                        versionSuffix = "/" + fbstorage.Version;
                    else
                        versionSuffix = "";

                    var storageUrl = RawStorageCloudBlob.RootUri + string.Format("/usersfolder/{0}/Menus/{1}{3}/{2}.json", organizationIdentity, fbstorage.StorageIdentity, fbstorage.Name, versionSuffix);
                    var lastModified = RawStorageCloudBlob.GetBlobLastModified( fbstorage.Url);
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

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <MetaDataID>{7b6700b2-c8a4-4548-be19-657614416518}</MetaDataID>
        public readonly MealCourseType mealCourseType;

        /// <MetaDataID>{e6899983-8b85-4e28-84f4-8a8199511318}</MetaDataID>
        public MealCourse(MealCourseType mealCourseType, List<ItemPreparation> itemPreparations)
        {
            this.mealCourseType = mealCourseType;
            _MealCourseTypeUri = ObjectStorage.GetStorageOfObject(mealCourseType).GetPersistentObjectUri(mealCourseType);
            _DurationInMinutes = mealCourseType.DurationInMinutes;
            _Name = mealCourseType.Name;
            foreach (var flavourItem in itemPreparations)
                AddItem(flavourItem);

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
        }
        public event ItemsStateChangedHandle ItemsStateChanged;

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{fa1a0f37-108e-478c-83d4-4f095498cef6}</MetaDataID>
        public void AddItem(IItemPreparation itemPreparation)
        {
            ItemPreparation flavourItem = itemPreparation as ItemPreparation;
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

                    var preparationData = PreparationStation.GetPreparationData(flavourItem);
                    if (preparationData.PreparationStationRuntime != null)
                    {
                        (preparationData.PreparationStationRuntime as PreparationStation).AssignItemPreparation(flavourItem);
                        flavourItem.State = ItemPreparationState.PreparationDelay;
                    }
                    else
                        flavourItem.State = ItemPreparationState.Serving;

                    CashierStation cashierStation = (flavourItem.ClientSession.MainSession as FoodServiceSession).CashierStation as CashierStation;
                    cashierStation.AssignItemPreparation(flavourItem);

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

            foreach (var foodItem in _FoodItems.OfType<ItemPreparation>())
            {
                foodItem.ObjectChangeState += FlavourItem_ObjectChangeState;
            }

        }



        /// <MetaDataID>{72860a38-cffd-4a68-807b-4c2ece8cddc5}</MetaDataID>
        internal static void AssignMealCourseToItem(ItemPreparation flavourItem)
        {
            //flavourItem.ClientSession.ServicePoint.
        }
    }
}