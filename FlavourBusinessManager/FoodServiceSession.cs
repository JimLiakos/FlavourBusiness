using System;
using System.Collections.Generic;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.EndUsers;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using OOAdvantech;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessFacade.HumanResources;


namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{a44c2bf7-1fd4-4e5a-b831-f9f7a1c381ac}</MetaDataID>
    [BackwardCompatibilityID("{a44c2bf7-1fd4-4e5a-b831-f9f7a1c381ac}")]
    [Persistent()]
    public class FoodServiceSession : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IFoodServiceSession, FinanceFacade.IPaymentSubject
    {


        /// <exclude>Excluded</exclude>
        string _SessionID = Guid.NewGuid().ToString("N");

        /// <MetaDataID>{978b61ff-99f3-4e93-9923-f88005ed6315}</MetaDataID>
        [PersistentMember(nameof(_SessionID))]
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+11")]
        public string SessionID => _SessionID;

        /// <MetaDataID>{d6ea50e8-e300-41c7-8a97-7ff69f0bc9e3}</MetaDataID>
        public FoodServiceSession()
        {

        }
        /// <MetaDataID>{cee5f973-18ba-4776-95d8-3ddd2915d3d1}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
        public string Description => ServicePoint?.Description;

        /// <MetaDataID>{37452f52-1155-4bbb-87c5-f621b27c624c}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        DateTime _SessionEnds;

        /// <MetaDataID>{40f071e7-8255-4f31-b2b2-8a774e1fa835}</MetaDataID>
        [PersistentMember(nameof(_SessionEnds))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public System.DateTime SessionEnds
        {
            get
            {
                return _SessionEnds;
            }

            set
            {

                if (_SessionEnds.ToUniversalTime() != value.ToUniversalTime())
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionEnds = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <exclude>Excluded</exclude>
        IServicePoint _ServicePoint;

        /// <MetaDataID>{f1464bf3-fe2a-4468-b8b1-0cd936988636}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [CachingDataOnClientSide]
        public FlavourBusinessFacade.ServicesContextResources.IServicePoint ServicePoint
        {
            get
            {
                if (_ServicePoint == null)
                    _ServicePoint = _PartialClientSessions.FirstOrDefault()?.ServicePoint;
                return _ServicePoint;
            }
            internal set
            {


                if (_ServicePoint != value)
                {
                    var oldServicePoint = _ServicePoint;

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {

                        if (_ServicePoint != null)
                        {
                            _ServicePoint.RemoveFoodServiceSession(this);
                        }
                        _ServicePoint = value;

                        foreach (var sessionPart in this.PartialClientSessions)
                            sessionPart.ServicePoint = value;


                        foreach (var itemPreparation in (from sessionPart in this.PartialClientSessions
                                                         from itemPreparation in sessionPart.FlavourItems.OfType<ItemPreparation>()
                                                         select itemPreparation))
                        {
                            itemPreparation.StateTimestamp = DateTime.UtcNow;
                        }

                        stateTransition.Consistent = true;
                    }

                    if (oldServicePoint != null)
                        (oldServicePoint as ServicePoint).UpdateState();
                }
                Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    ObjectChangeState?.Invoke(this, nameof(ServicePoint));
                };

            }
        }

        /// <MetaDataID>{0d03f56d-2d6a-4e05-b3d3-4dec75667aee}</MetaDataID>
        internal bool CanIncludeAsPart(FoodServiceClientSession referencClientSession)
        {
            if (referencClientSession.MainSession != null && referencClientSession.MainSession != this)
                return false;


            if (Meal != null)
            {

                var mealCourse = Meal.Courses.Where(x => (x as MealCourse).MealCourseType.AutoStart && x.ServedAtForecast != null).OrderBy(x => x.ServedAtForecast).LastOrDefault();
                if (mealCourse == null)
                    return true;

                var mealStartsAt = Meal.Courses.Where(x => x.StartsAt != null && (x as MealCourse).MealCourseType.AutoStart).OrderBy(x => x.StartsAt).FirstOrDefault()?.StartsAt;
                if (mealStartsAt == null)
                    mealStartsAt = SessionStarts;


                var durationInMin = (mealCourse.ServedAtForecast - mealStartsAt).Value.TotalMinutes;

                var progressInMin = (DateTime.UtcNow - mealStartsAt).Value.TotalMinutes;
                if (durationInMin > 0 && progressInMin > 0)
                {

                    var progressPercentage = (100 * progressInMin) / durationInMin;
                    if (progressPercentage < ServicesContextRunTime.Current.Settings.AutoAssignMaxMealProgress)
                        return true;
                }
                return false;


            }

            return true;

        }

        /// <exclude>Excluded</exclude>
        DateTime _SessionStarts;
        /// <MetaDataID>{4bb0bd71-bead-41de-a054-9e612487691e}</MetaDataID>
        [PersistentMember(nameof(_SessionStarts))]
        [BackwardCompatibilityID("+3")]
        public System.DateTime SessionStarts
        {
            get
            {
                return _SessionStarts;
            }

            set
            {
                _SessionStarts = value;
            }
        }

        /// <exclude>Excluded</exclude> 
        OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession> _PartialClientSessions = new OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession>();


        /// <MetaDataID>{0aa1ebef-0011-45f0-9176-eba5e8afd63a}</MetaDataID>
        [PersistentMember(nameof(_PartialClientSessions))]
        [BackwardCompatibilityID("+4")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IList<IFoodServiceClientSession> PartialClientSessions => _PartialClientSessions.AsReadOnly();


        /// <MetaDataID>{51a1551b-e290-414a-87be-c8c5096ca437}</MetaDataID>
        public ICashiersStationRuntime CashierStation
        {
            get
            {
                return ServicePointRunTime.ServicesContextRunTime.Current.CashierStations.OfType<ICashiersStationRuntime>().FirstOrDefault();
            }
        }



        ///// <MetaDataID>{0aa1ebef-0011-45f0-9176-eba5e8afd63a}</MetaDataID>
        //[BackwardCompatibilityID("+4")]
        //public System.Collections.Generic.IList<IFoodServiceClientSession> PartialClientSessions
        //{
        //    get
        //    {
        //        return _PartialClientSessions.AsReadOnly();
        //    }
        //}

        /// <MetaDataID>{40eff473-e07d-4486-97f2-f44bcd37c877}</MetaDataID>
        public void AddPartialSession(IFoodServiceClientSession partialSession)
        {
            if (!PartialClientSessions.Contains(partialSession))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _PartialClientSessions.Add(partialSession);

                    stateTransition.Consistent = true;
                }
            }
            if (Transaction.Current != null)
            {
                Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    if (transaction.Status == TransactionStatus.Committed)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            StateMachineMonitoring();
                            partialSession.ObjectChangeState -= PartialSession_ObjectChangeState;
                        }
                    }
                };
            }
            else
            {
                StateMachineMonitoring();
                partialSession.ObjectChangeState -= PartialSession_ObjectChangeState;
            }

        }

        /// <MetaDataID>{93821822-e7d6-4ef6-b28c-07ca8f1fa291}</MetaDataID>
        private void PartialSession_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IFoodServiceClientSession.SessionState))
                StateMachineMonitoring();
        }

        /// <MetaDataID>{fbe827bb-85e5-4ac5-a527-028149b7e116}</MetaDataID>
        object StateMachineLock = new object();

        object PaymentLock = new object();

        /// <MetaDataID>{dd202b7b-1e56-45fe-a4a9-f3f26dcc645a}</MetaDataID>
        private void StateMachineMonitoring()
        {

            var partialClientSessions = PartialClientSessions;
            if (partialClientSessions.Count > 0)
            {
                //One of the messmates commits event
                if (SessionState == SessionState.Conversation && partialClientSessions.Where(
                    x => x.SessionState == ClientSessionState.Conversation || x.SessionState == ClientSessionState.ConversationStandy || x.SessionState == ClientSessionState.UrgesToDecide).Count() > 0
                    && partialClientSessions.Where(x => x.SessionState == ClientSessionState.ItemsCommited).Count() > 0)
                {
                    SessionState = SessionState.UrgesToDecide;
                    //UrgesToDecideStateRun();
                }

                lock (StateMachineLock)
                {
                    if (SessionState == SessionState.MealValidationDelay && partialClientSessions.Where(x => x.SessionState != ClientSessionState.ItemsCommited).Count() > 0)
                        SessionState = SessionState.UrgesToDecide;
                }

                lock (StateMachineLock)
                {
                    //There aren't messmates in the ItemsCommit state
                    if (SessionState == SessionState.UrgesToDecide && partialClientSessions.Where(x => x.SessionState == ClientSessionState.ItemsCommited).Count() == 0)
                        SessionState = SessionState.Conversation;
                }


                //All messmates are in committed state for specific timespan event
                if (partialClientSessions.Where(x => x.SessionState == ClientSessionState.ItemsCommited || x.SessionState == ClientSessionState.Inactive).Count() == partialClientSessions.Count)
                {
                    var itemsToPrepare = (from clientSession in PartialClientSessions
                                          from itemPreparation in clientSession.FlavourItems
                                          select itemPreparation).OfType<ItemPreparation>().ToList();

                    if (itemsToPrepare.Count > 0)
                    {
                        bool mealValidationDelaySessionState = false;

                        lock (StateMachineLock)
                        {
                            if (SessionState != SessionState.MealValidationDelay && SessionState != SessionState.MealMonitoring)
                            {
                                mealValidationDelaySessionState = true;
                                SessionState = SessionState.MealValidationDelay;
                            }
                        }

                        if (mealValidationDelaySessionState)
                            MealValidationDelayRun();
                    }
                }
            }
            else
                SessionState = SessionState.Conversation;

        }

        /// <MetaDataID>{7c0f023a-f58e-469b-be99-b65142dfc698}</MetaDataID>
        Task MealValidationDelayTask;
        /// <MetaDataID>{598f4be7-bfaf-40ae-b1e2-dc80d367a54f}</MetaDataID>
        private void MealValidationDelayRun()
        {
            lock (StateMachineLock)
            {
                if (MealValidationDelayTask != null && !MealValidationDelayTask.IsCompleted)
                    return;
                MealValidationDelayTask = Task.Run(() =>
                 {
                     var allMessmetesCommitedTimeSpanInSeconds = FlavourBusinessManager.ServicePointRunTime.ServicesContextRunTime.Current.AllMessmetesCommitedTimeSpan;
                     allMessmetesCommitedTimeSpanInSeconds = 12;
                     while (allMessmetesCommitedTimeSpanInSeconds > 0)
                     {
                         System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                         allMessmetesCommitedTimeSpanInSeconds--;
                         if (SessionState != SessionState.MealValidationDelay)
                             return;
                     }

                     if (SessionState == SessionState.MealValidationDelay)
                     {

                         using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                         {
                             SessionState = SessionState.MealMonitoring;
                             CreateAndInitMeal();
                             stateTransition.Consistent = true;
                         }
                         (FlavourBusinessManager.ServicePointRunTime.ServicesContextRunTime.Current.MealsController as MealsController).OnNewMealCoursesInrogress(_Meal.Value.Courses);
                         _Meal.Value.MonitoringRun();
                     }
                 });
            }
        }


        /// <MetaDataID>{0d5343f3-0c56-4e22-b66d-e4357dbd8a75}</MetaDataID>
        MenuModel.MealType MealType
        {
            get
            {
                return null;
            }
        }

        /// <MetaDataID>{1b995301-4937-4889-a8b1-be54049ff16e}</MetaDataID>
        private void CreateAndInitMeal()
        {

            if (_Meal.Value != null)
                throw new Exception("Meal already exist");

            var itemsToPrepare = (from clientSession in PartialClientSessions
                                  from itemPreparation in clientSession.FlavourItems
                                  select itemPreparation).OfType<ItemPreparation>().ToList();

            var mealType = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri<MenuModel.IMealCourseType>((itemsToPrepare[0] as ItemPreparation).SelectedMealCourseTypeUri).Meal as MenuModel.MealType;



            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Meal.Value = new Meal(mealType, itemsToPrepare, this);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(_Meal.Value);
                stateTransition.Consistent = true;
            }



        }




        /// <MetaDataID>{115bab53-101d-444f-b364-e8f27d160e71}</MetaDataID>
        public static TimeSpan MealConversetionTime
        {
            get
            {
                return TimeSpan.FromMinutes(10);
            }
        }

        /// <exclude>Excluded</exclude>
        SessionState _SessionState;

        /// <MetaDataID>{992bbbb9-bddc-4b7d-89f9-90360d6fe4d1}</MetaDataID>
        [PersistentMember(nameof(_SessionState))]
        [BackwardCompatibilityID("+5")]
        public SessionState SessionState
        {
            get => _SessionState;
            set
            {
                if (_SessionState != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionState = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>     
        OOAdvantech.Member<Meal> _Meal = new OOAdvantech.Member<Meal>();

        /// <MetaDataID>{8d538032-0365-42f6-a71a-176cf8a85f38}</MetaDataID>
        [PersistentMember(nameof(_Meal))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.CascadeDelete)]
        [BackwardCompatibilityID("+6")]
        public IMeal Meal
        {
            get => _Meal.Value;

        }


        /// <exclude>Excluded</exclude>
        string _MenuStorageIdentity;

        /// <MetaDataID>{6fa82f00-eaa4-41f5-a356-66a8b1862924}</MetaDataID>
        [PersistentMember(nameof(_MenuStorageIdentity))]
        [BackwardCompatibilityID("+8")]
        internal string MenuStorageIdentity
        {
            get => _MenuStorageIdentity;
            set
            {
                if (_MenuStorageIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuStorageIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{083c5981-bcc1-4a67-b17e-75600727da9a}</MetaDataID>
        public OrganizationStorageRef Menu { get; internal set; }

        /// <MetaDataID>{ce6a2e61-2404-47a8-a373-8059c48001b8}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        public IFlavoursServicesContextRuntime ServicesContextRuntime => FlavourBusinessManager.ServicePointRunTime.ServicesContextRunTime.Current;

        /// <MetaDataID>{3dac55f5-abb7-4017-b370-16eefdbd91a0}</MetaDataID>
        public double Progresses { get; internal set; }

        /// <MetaDataID>{ce6107e5-bd9c-4f9f-bc82-e2070d163bf6}</MetaDataID>
        public void RemovePartialSession(IFoodServiceClientSession partialSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PartialClientSessions.Remove(partialSession);
                stateTransition.Consistent = true;
            }
            if (OOAdvantech.Transactions.Transaction.Current != null)
            {
                OOAdvantech.Transactions.Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    if (transaction.Status == TransactionStatus.Committed)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                        {
                            StateMachineMonitoring();
                            partialSession.ObjectChangeState -= PartialSession_ObjectChangeState;
                        }
                    }
                };
            }
            else
            {
                StateMachineMonitoring();
                partialSession.ObjectChangeState -= PartialSession_ObjectChangeState;
            }
        }
        /// <MetaDataID>{9c60f720-9c83-4a06-b80d-687ff2517dc1}</MetaDataID>
        [ObjectActivationCall]
        internal void OnActivated()
        {

            foreach (var partialSession in PartialClientSessions)
                partialSession.ObjectChangeState += PartialSession_ObjectChangeState;

            if (this.SessionState == SessionState.MealValidationDelay)
                MealValidationDelayRun();
            if (this.SessionState == SessionState.MealMonitoring)
            {
                var courses = _Meal.Value.Courses;
                _Meal.Value.MonitoringRun();
            }
            else
                StateMachineMonitoring();

            lock (CaregiversLock)
            {
                if (!string.IsNullOrWhiteSpace(WillTakeCareWorkersJson))
                    _Caregivers = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Caregiver>>(WillTakeCareWorkersJson);

            }


        }
        /// <MetaDataID>{44c96e4d-408e-495e-80f5-59cc2927ca67}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        internal void OnBeforeObjectStateCommited()
        {
            lock (CaregiversLock)
            {
                WillTakeCareWorkersJson = OOAdvantech.Json.JsonConvert.SerializeObject(_Caregivers);

            }
        }
        /// <MetaDataID>{a9c1e448-ba0d-4491-8520-0dc47dfdc530}</MetaDataID>
        public void MonitorTick()
        {
            #region check for long time meal conversation and update the waiters
            var firstItemPreparation = (from partialClientSession in PartialClientSessions
                                        from itemPreparation in partialClientSession.FlavourItems.OfType<ItemPreparation>()
                                        orderby itemPreparation.StateTimestamp
                                        select itemPreparation).FirstOrDefault();
            if (firstItemPreparation != null &&
                (SessionState == SessionState.Conversation || SessionState == SessionState.UrgesToDecide) && Meal == null &&
                (DateTime.UtcNow - firstItemPreparation.StateTimestamp.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.MealConversationTimeoutInMin))
            {
                if (ServicePoint.State == ServicePointState.Conversation)
                    (ServicePoint as ServicePoint).ChangeServicePointState(ServicePointState.ConversationTimeout);

                if (Caregivers.Where(x => x.Caregiving == Caregiver.CaregivingType.ConversationCheck).Count() > 0)
                {
                    if (ServicePoint.State == ServicePointState.ConversationTimeout && (DateTime.UtcNow - WillTakeCareTimestamp.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.MealConversationTimeoutWaitersUpdateTimeSpanInMin * 3))
                    {
                        if (ServicePoint.State == ServicePointState.ConversationTimeout && (DateTime.UtcNow - UrgesToDecideToWaiterTimeStamp.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.MealConversationTimeoutWaitersUpdateTimeSpanInMin))
                        {
                            UrgesToDecideToWaiterTimeStamp = DateTime.UtcNow;
                            ServicesContextRunTime.Current.MealConversationTimeout(ServicePoint as ServicePoint, SessionID, Caregivers);
                        }
                    }

                }
                else if (ServicePoint.State == ServicePointState.ConversationTimeout && (DateTime.UtcNow - UrgesToDecideToWaiterTimeStamp.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.MealConversationTimeoutWaitersUpdateTimeSpanInMin))
                {
                    UrgesToDecideToWaiterTimeStamp = DateTime.UtcNow;
                    ServicesContextRunTime.Current.MealConversationTimeout(ServicePoint as ServicePoint, SessionID, Caregivers);
                }
            }
            #endregion
            if (firstItemPreparation != null &&SessionState == SessionState.MealValidationDelay&&MealValidationDelayTask!=null&& MealValidationDelayTask.Status==TaskStatus.Faulted)
            {
                MealValidationDelayTask = null;
                MealValidationDelayRun();
            }

        }


        /// <MetaDataID>{09474811-aa43-4545-88a7-ffb0285b2fcd}</MetaDataID>
        DateTime UrgesToDecideToWaiterTimeStamp = DateTime.MinValue;



        /// <MetaDataID>{90895d63-996c-4aaa-a041-d9621efcf018}</MetaDataID>
        object CaregiversLock = new object();


        /// <MetaDataID>{3a9025c3-c195-4acb-be73-340396fa65c4}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        [PersistentMember()]

        string WillTakeCareWorkersJson;



        /// <exclude>Excluded</exclude>
        DateTime _WillTakeCareTimestamp;


        /// <MetaDataID>{11154b0f-0798-4d5d-af09-b2ea1690f07b}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        [PersistentMember(nameof(_WillTakeCareTimestamp))]

        public System.DateTime WillTakeCareTimestamp
        {
            get => _WillTakeCareTimestamp; set
            {
                if (_WillTakeCareTimestamp != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _WillTakeCareTimestamp = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }




        /// <MetaDataID>{5bb24d64-6784-4f87-948f-e37b638d423c}</MetaDataID>
        public void AddCaregiver(IServicesContextWorker caregiver, Caregiver.CaregivingType caregivingType)
        {
            lock (CaregiversLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Caregivers.Add(new Caregiver() { Worker = caregiver, Caregiving = caregivingType });
                    if (caregivingType == Caregiver.CaregivingType.ConversationCheck)
                        WillTakeCareTimestamp = DateTime.UtcNow;

                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        List<Caregiver> _Caregivers = new List<Caregiver>();


        /// <MetaDataID>{2a450ea3-c23a-42dd-9eac-3a8e1fcb327d}</MetaDataID>
        public List<Caregiver> Caregivers
        {
            get
            {
                lock (CaregiversLock)
                {
                    return _Caregivers.ToList();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        SessionType _SessionType;
        /// <MetaDataID>{ed6ac391-1de5-4a8b-935f-c7260a784eda}</MetaDataID>
        [PersistentMember(nameof(_SessionType))]
        [BackwardCompatibilityID("+14")]
        public SessionType SessionType
        {
            get => _SessionType;
            set
            {
                if (_SessionType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        public OOAdvantech.Collections.Generic.Set<FinanceFacade.IPayment> _BillingPayments = new OOAdvantech.Collections.Generic.Set<FinanceFacade.IPayment>();


        /// <MetaDataID>{f2b50824-7139-47a4-83ac-4e5a4a01fa9e}</MetaDataID>
        [PersistentMember(nameof(_BillingPayments))]
        [BackwardCompatibilityID("+15")]
        public List<FinanceFacade.IPayment> BillingPayments => _BillingPayments.ToThreadSafeList();

        /// <MetaDataID>{ba60826c-7385-4e09-a87e-7952c74f69e0}</MetaDataID>
        [BackwardCompatibilityID("+16")]
        public IPlace DeleiveryPlace
        {
            get => Place.GetPlace(DeleiveryPlaceData);
            set
            {
                if (DeleiveryPlaceData!=PlaceData.GetPlaceData(value))
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        DeleiveryPlaceData=PlaceData.GetPlaceData(value);
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        DateTime? _ServiceTime;
        /// <MetaDataID>{5e44e5a2-62c1-4943-affe-112fb2bc8063}</MetaDataID>
        [PersistentMember(nameof(_ServiceTime))]
        [BackwardCompatibilityID("+18")]
        public DateTime? ServiceTime
        {
            get => _ServiceTime;
            set
            {
                if (_ServiceTime!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServiceTime=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{d47efdba-03b6-4e84-83ba-82f194341ebb}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+17")]
        private PlaceData DeleiveryPlaceData;



        /// <MetaDataID>{1b4da94a-7178-4595-a8d4-5084488ee46b}</MetaDataID>
        internal void ReassignSharedItem(ItemPreparation flavourItem)
        {
            if (flavourItem.SharedInSessions.Count != 0)
            {
                var sessionID = flavourItem.SharedInSessions[0];
                var clientSession = this.PartialClientSessions.Where(x => x.SessionID == sessionID).FirstOrDefault();
                clientSession.AddItem(flavourItem);

            }
        }

        /// <MetaDataID>{598b3be1-30b6-493c-be76-2328a110a602}</MetaDataID>
        object PartialSessionsLock = new object();
        /// <MetaDataID>{129d63d1-6ade-452e-b2e9-7e0601d6b728}</MetaDataID>
        internal void Merge(IFoodServiceSession foodServiceSession)
        {
            lock (PartialSessionsLock)
            {
                var partialClientSessions = PartialClientSessions;
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    foreach (var partialSession in foodServiceSession.PartialClientSessions.OfType<FoodServiceClientSession>())
                    {
                        var mergeInPartialSession = partialClientSessions.OfType<FoodServiceClientSession>().Where(x => x.ClientDeviceID == partialSession.ClientDeviceID && x.UserIdentity == partialSession.UserIdentity).FirstOrDefault();
                        if (mergeInPartialSession != null)
                        {
                            mergeInPartialSession.Merge(partialSession);
                            var flavourItems = partialSession.FlavourItems;
                            foodServiceSession.RemovePartialSession(partialSession);
                        }
                        else
                        {
                            foodServiceSession.RemovePartialSession(partialSession);
                            AddPartialSession(partialSession);
                            partialSession.ServicePoint = ServicePoint;
                        }
                    }

                    foreach (var sharedItem in (from partialSesion in PartialClientSessions
                                                from itemPreration in partialSesion.SharedItems
                                                select itemPreration))
                    {
                        if (sharedItem.ClientSession.MainSession != this)
                            throw new FlavourBusinessFacade.Exceptions.TransferException("There are shared items left in the old session", 801);
                    }
                    var sessions = foodServiceSession.PartialClientSessions;
                    if (Meal != null)
                        (Meal as Meal).Merge(foodServiceSession.Meal as Meal);

                    stateTransition.Consistent = true;
                }
            }


        }

        /// <MetaDataID>{888779e0-26b1-4fda-95a9-2bb47b7a9efd}</MetaDataID>
        public void AddPayment(FinanceFacade.IPayment payment)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _BillingPayments.Add(payment);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{4a963a47-520a-4a30-811b-ce78fe4834a1}</MetaDataID>
        public void RemovePayment(FinanceFacade.IPayment payment)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _BillingPayments.Remove(payment);
                stateTransition.Consistent = true;
            }
        }

  

        internal void CardPaymentCompleted(FinanceFacade.IPayment payment, string bankId, string cardNumber, bool isDebit, string transactionId, int tipAmount)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                lock (PaymentLock)
                {
                    if (payment.State==FinanceFacade.PaymentState.Completed)
                        return;
                    payment.CardPaymentCompleted(bankId, cardNumber, isDebit, transactionId, tipAmount);
                }
                var sessionFlavourItems = (from foodServiceClientSession in PartialClientSessions
                                           from flavourItem in foodServiceClientSession.FlavourItems
                                           select flavourItem).ToList();
                Dictionary<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession, List<FlavourBusinessFacade.RoomService.IItemPreparation>> itemsToCommit = new Dictionary<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession, List<FlavourBusinessFacade.RoomService.IItemPreparation>>();

                foreach (var paymentItem in payment.Items)
                {
                    var flavourItem = sessionFlavourItems.Where(x => x.uid==paymentItem.uid&&x.State==FlavourBusinessFacade.RoomService.ItemPreparationState.AwaitingPaymentToCommit).FirstOrDefault();
                    if (flavourItem!=null)
                    {
                        List<FlavourBusinessFacade.RoomService.IItemPreparation> flavoursItem = null;

                        if (!itemsToCommit.TryGetValue(flavourItem.ClientSession, out flavoursItem))
                        {
                            flavoursItem=new List<FlavourBusinessFacade.RoomService.IItemPreparation>();
                            itemsToCommit[flavourItem.ClientSession]=flavoursItem;
                        }
                        flavoursItem.Add(flavourItem);
                    }
                }
                foreach (var itemsToCommitEntry in itemsToCommit)
                    itemsToCommitEntry.Key.Commit(itemsToCommitEntry.Value); 
                stateTransition.Consistent = true;
            }

        }

        public void PaymentCompleted(FinanceFacade.IPayment payment)
        {
            var sessionFlavourItems = (from foodServiceClientSession in PartialClientSessions
                                       from flavourItem in foodServiceClientSession.FlavourItems
                                       select flavourItem).ToList();
            Dictionary<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession, List<FlavourBusinessFacade.RoomService.IItemPreparation>> itemsToCommit = new Dictionary<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession, List<FlavourBusinessFacade.RoomService.IItemPreparation>>();

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {

                foreach (var paymentItem in payment.Items)
                {
                    var flavourItem = sessionFlavourItems.Where(x => x.uid==paymentItem.uid&&x.State==FlavourBusinessFacade.RoomService.ItemPreparationState.AwaitingPaymentToCommit).FirstOrDefault();
                    if (flavourItem!=null)
                    {
                        List<FlavourBusinessFacade.RoomService.IItemPreparation> flavoursItem = null;

                        if (!itemsToCommit.TryGetValue(flavourItem.ClientSession, out flavoursItem))
                        {
                            flavoursItem=new List<FlavourBusinessFacade.RoomService.IItemPreparation>();
                            itemsToCommit[flavourItem.ClientSession]=flavoursItem;
                        }
                        flavoursItem.Add(flavourItem);
                    }
                }
                foreach (var itemsToCommitEntry in itemsToCommit)
                    itemsToCommitEntry.Key.Commit(itemsToCommitEntry.Value); 
                stateTransition.Consistent = true;
            }


        }
    }
}