using System;
using System.Collections.Generic;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Linq;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.HumanResources;
using FlavourBusinessFacade.HumanResources;
using MenuModel;
using OOAdvantech;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{5a6deea9-0208-441b-9b01-77a13dc8c126}</MetaDataID>
    [BackwardCompatibilityID("{5a6deea9-0208-441b-9b01-77a13dc8c126}")]
    [Persistent()]
    public class ServicePoint : MarshalByRefObject, IServicePoint, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        /// <exclude>Excluded</exclude>
        int _Seats;

        /// <MetaDataID>{bc816cfa-a904-4461-a92b-7ea5ad1bbb09}</MetaDataID>
        [PersistentMember(nameof(_Seats))]
        [BackwardCompatibilityID("+6")]
        public int Seats
        {
            get => _Seats;
            set
            {
                if (_Seats != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Seats = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{bb55702d-4be6-4339-94f5-354cf83542ec}</MetaDataID>
        public ServicePoint()
        {
        }





        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink = new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{883aabfe-47f1-45db-80f6-3c3e077b862f}</MetaDataID>
        [PersistentMember(nameof(_Description)), BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Description = value;
                    stateTransition.Consistent = true;
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;
        /// <MetaDataID>{7e7046e7-023a-4b36-b55e-898075088d56}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember(nameof(_ServicesContextIdentity))]
        public string ServicesContextIdentity
        {
            get
            {
                return _ServicesContextIdentity;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ServicesContextIdentity = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesPointIdentity;
        /// <MetaDataID>{7187498f-52cf-4f19-84a2-1fd301986c46}</MetaDataID>
        [PersistentMember(nameof(_ServicesPointIdentity))]
        [BackwardCompatibilityID("+3")]
        [CachingDataOnClientSide]
        public string ServicesPointIdentity
        {
            get
            {
                if (_ServicesPointIdentity == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesPointIdentity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }

                }

                return _ServicesPointIdentity;
            }

            set
            {
                if (_ServicesPointIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesPointIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        internal void UpdateState()
        {
            var servicePointOpenSessions = ServicePointRunTime.ServicesContextRunTime.Current.OpenSessions.Where(x => x.ServicePoint == this).ToList();

            if (servicePointOpenSessions.Count == 0)
                ChangeServicePointState(ServicePointState.Free);
            else
            {



                var mealCourses = (from servicePointOpenSession in servicePointOpenSessions
                                   where servicePointOpenSession.Meal != null
                                   from mealCourse in servicePointOpenSession.Meal.Courses
                                   where mealCourse.FoodItems.OfType<RoomService.ItemPreparation>().Any(x => x.IsInPreviousState(FlavourBusinessFacade.RoomService.ItemPreparationState.Served))
                                   select mealCourse).ToList();

                var overTimeMealCourses = mealCourses.Where(mealCourse => mealCourse.ServedAtForecast <= DateTime.UtcNow).ToList();
                var inTimeMealCourses = mealCourses.Where(mealCourse => mealCourse.ServedAtForecast > DateTime.UtcNow).ToList();
                if (overTimeMealCourses.Count > 0)
                    ChangeServicePointState(ServicePointState.MealCourseOvertime);
                else if (inTimeMealCourses.Count > 0)
                    ChangeServicePointState(ServicePointState.MealCoursePreparation);
                else
                {
                    if ((from servicePointOpenSession in servicePointOpenSessions
                         where servicePointOpenSession.Meal != null
                         from mealCourse in servicePointOpenSession.Meal.Courses
                         select mealCourse).All(x => x.PreparationState == FlavourBusinessFacade.RoomService.ItemPreparationState.Served))
                    {

                        ChangeServicePointState(ServicePointState.Served);
                    }
                    else
                        ChangeServicePointState(ServicePointState.Conversation);
                }
            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{b3a3c408-9ebf-4d2b-af0c-8cc7268f12ab}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<IFoodServiceSession> _ServiceSessions = new OOAdvantech.Collections.Generic.Set<IFoodServiceSession>();
        /// <MetaDataID>{3f818102-6573-4e5a-99cf-8f52c39c0805}</MetaDataID>
        [PersistentMember(nameof(_ServiceSessions))]
        [BackwardCompatibilityID("+4")]
        public System.Collections.Generic.IList<IFoodServiceSession> ServiceSessions
        {
            get
            {
                return _ServiceSessions.AsReadOnly();
            }
        }


        /// <MetaDataID>{950c80cc-27bb-4dcd-88e5-4dc2836c4a0a}</MetaDataID>
        object ServicePointLock = new object();


        /// <MetaDataID>{7beae4f4-7e79-476d-9f83-8c169bd6a8dd}</MetaDataID>
        public System.Collections.Generic.IList<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession> ActiveFoodServiceClientSessions
        {
            get
            {
                lock (ServicePointLock)
                {

                    var activeFoodServiceClientSessions = (from session in ServicesContextRunTime.OpenClientSessions.OfType<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession>()
                                                           where session.ServicePoint == this && session.SessionEnds > System.DateTime.UtcNow
                                                           select session).ToList();

                    return activeFoodServiceClientSessions;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        ServicePointState _State;
        /// <MetaDataID>{b9c9676f-c53d-4952-92e6-b17d22d81e18}</MetaDataID>
        [PersistentMember(nameof(_State))]
        [BackwardCompatibilityID("+7")]
        public ServicePointState State
        {
            get => _State;
            set
            {

                if (_State != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _State = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{4182b8cc-608d-460a-838e-b666eb57c83c}</MetaDataID>
        public void AddFoodServiceSession(IFoodServiceSession foodServiceSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServiceSessions.Add(foodServiceSession);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{9379a27e-c078-423d-afad-b3587d587247}</MetaDataID>
        public void RemoveFoodServiceSession(IFoodServiceSession foodServiceSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServiceSessions.Remove(foodServiceSession);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{0eac815e-114c-40bf-b5f4-a4b72d42e03a}</MetaDataID>
        public IFoodServiceSession NewFoodServiceSession()
        {
            FoodServiceSession foodServiceSession = new FoodServiceSession();
            foodServiceSession.SessionStarts = DateTime.UtcNow;
            foodServiceSession.SessionEnds = DateTime.MinValue;
            AddFoodServiceSession(foodServiceSession);
            return foodServiceSession;
        }

        ///// <MetaDataID>{ca0eaef8-e9fa-4c7f-b0e9-ae9e6b5b6d8d}</MetaDataID>
        //public void AddServicePointClientSesion(IFoodServiceClientSession foodServiceClientSession)
        //{

        //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //    {
        //        _ActiveFoodServiceClientSessions.Add(foodServiceClientSession);
        //        stateTransition.Consistent = true;
        //    }

        //}

        ///// <MetaDataID>{37354422-3d39-4c76-9c59-ba11861a1151}</MetaDataID>
        //public void RemoveServicePointClientSesion(IFoodServiceClientSession foodServiceClientSession)
        //{
        //    if (ActiveFoodServiceClientSessions != null && ActiveFoodServiceClientSessions.Contains(foodServiceClientSession))
        //    {
        //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //        {

        //            _ActiveFoodServiceClientSessions.Remove(foodServiceClientSession);
        //            stateTransition.Consistent = true;
        //        }
        //    }

        //}

        /// <MetaDataID>{f9251525-75d8-4189-b246-e25d08c268ce}</MetaDataID>
        public IFoodServiceClientSession NewFoodServiceClientSession(string clientName, string clientDeviceID)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Provides a session between client and service point.
        /// In case where there isn't session and the create flag is false return s null;
        /// </summary>
        /// <param name="clientName">
        /// Defines the client nick name
        /// </param>
        /// <param name="mealInvitationSessionID">
        /// Defines the identity which is necessary to make client session part of meal  
        /// </param>
        /// <param name="clientDeviceID">
        /// Defines the identity of device which used from the client to send its order 
        /// </param>
        /// <param name="deviceFirebaseToken">
        /// Defines the device firebase token for push notification facility 
        /// </param>
        /// <param name="clientIdentity">
        /// Defines the identity of client.
        /// Used only in case where client is signed in
        /// </param>
        /// <param name="create">
        /// In case where this flag is true, service points creates a session if there isn't.
        /// </param>
        /// <returns>
        /// return the client session
        /// </returns>
        /// <MetaDataID>{d9fdfbcc-661f-4f13-a29d-2c7e42a886aa}</MetaDataID>
        public IFoodServiceClientSession GetFoodServiceClientSession(string clientName, string mealInvitationSessionID, string clientDeviceID, string deviceFirebaseToken, bool create = false)
        {
            AuthUserRef authUserRef = AuthUserRef.GetCallContextAuthUserRef(false);
            FlavourBusinessFacade.IUser user = null;
            Waiter waiter = null;
            if (authUserRef != null)
                user = waiter = authUserRef.GetContextRoleObject<Waiter>();


            FoodServiceClientSession fsClientSession = null;
            FoodServiceClientSession messmateClientSesion = null;


            //CollectGarbageClientSessions(user, clientDeviceID);

            var objectStorage = ObjectStorage.GetStorageOfObject(this);
            if (user != null)
                fsClientSession = (from session in ActiveFoodServiceClientSessions.OfType<EndUsers.FoodServiceClientSession>()
                                   where session.ServicePoint == this && session.UserIdentity == user.Identity
                                   select session).FirstOrDefault();


            if (fsClientSession == null) //visitor client session
                fsClientSession = (from session in ActiveFoodServiceClientSessions.OfType<EndUsers.FoodServiceClientSession>()
                                   where session.ServicePoint == this && session.ClientDeviceID == clientDeviceID
                                   select session).FirstOrDefault();



            if (string.IsNullOrWhiteSpace(clientName) && user != null)
                clientName = user.FullName;



            if (!string.IsNullOrWhiteSpace(mealInvitationSessionID))

                messmateClientSesion = (from session in ActiveFoodServiceClientSessions.OfType<EndUsers.FoodServiceClientSession>()
                                        where session.ServicePoint == this && session.SessionID == mealInvitationSessionID
                                        select session).FirstOrDefault();



            if (fsClientSession == null && create)
            {
                try
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {

                        //fsClientSession = ServicesContextRunTime.NewFoodServiceClientSession(fsClientSession);
                        fsClientSession = new EndUsers.FoodServiceClientSession();
                        fsClientSession.ClientName = clientName;
                        fsClientSession.ClientDeviceID = clientDeviceID;
                        fsClientSession.DeviceFirebaseToken = deviceFirebaseToken;
                        fsClientSession.SessionStarts = DateTime.UtcNow;
                        fsClientSession.ModificationTime = DateTime.UtcNow;
                        fsClientSession.PreviousYouMustDecideMessageTime = DateTime.UtcNow;

                        if (user != null && fsClientSession != null && user.Identity != fsClientSession.UserIdentity)
                            fsClientSession.UserIdentity = user.Identity;

                        if (waiter != null)
                        {
                            fsClientSession.IsWaiterSession = true;
                            (user as HumanResources.Waiter).AddClientSession(fsClientSession);
                        }



                        fsClientSession.DateTimeOfLastRequest = DateTime.UtcNow;// DateTime.MinValue + TimeSpan.FromDays(28);
                        objectStorage.CommitTransientObjectState(fsClientSession);
                        fsClientSession.ServicePoint = this;

                        if (messmateClientSesion != null && messmateClientSesion.ServicePoint == this)
                            messmateClientSesion.MakePartOfMeal(fsClientSession);



                        stateTransition.Consistent = true;
                    }

                    ServicesContextRunTime.AddOpenServiceClientSession(fsClientSession);
                }
                catch (OOAdvantech.Transactions.TransactionException error)
                {
                    throw;
                }
                catch (System.Exception error)
                {
                    throw;
                }
                lock (ServicePointLock)
                {

                    if ( !fsClientSession.IsWaiterSession&& State==ServicePointState.Free)
                        ChangeServicePointState(ServicePointState.Laying);

                }
            }
            else
            {
                if (fsClientSession != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        if ((DateTime.UtcNow - fsClientSession.DateTimeOfLastRequest).TotalMinutes > 0.5)
                            fsClientSession.DateTimeOfLastRequest = DateTime.UtcNow;
                        fsClientSession.ClientName = clientName;
                        if (fsClientSession.DeviceFirebaseToken != deviceFirebaseToken)
                        {

                        }


                        if (waiter != null && fsClientSession.Waiter == null)
                        {
                            fsClientSession.IsWaiterSession = true;
                            (user as HumanResources.Waiter).AddClientSession(fsClientSession);

                        }
                        if (user != null && user.Identity != fsClientSession.UserIdentity)
                            fsClientSession.UserIdentity = user.Identity;


                        fsClientSession.DeviceFirebaseToken = deviceFirebaseToken;
                        if (messmateClientSesion != null && messmateClientSesion.ServicePoint == this)
                            messmateClientSesion.MakePartOfMeal(fsClientSession);
                        stateTransition.Consistent = true;
                    }
                }
            }

            return fsClientSession;
        }

        ///// <summary>
        ///// Removes all inactive food client session.
        ///// There is case where user open a session with service point but 
        ///// </summary>
        ///// <param name="user"></param>
        ///// <param name="clientDeviceID"></param>
        //private void CollectGarbageClientSessions(FlavourBusinessFacade.IUser user, string clientDeviceID)
        //{
        //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
        //    {
        //        List<FoodServiceClientSession> openClientSessions = new List<FoodServiceClientSession>();

        //        if(user!=null)
        //            OpenClientSessions.Where(x => x.UserIdentity == user.Identity).ToList();
        //        if (openClientSessions.Count > 0)
        //        {
        //            foreach (var foodServiceClientSession in openClientSessions.ToList()/*.Where(x => x.Waiter == this)*/.Where(x => x.FlavourItems.Count == 0 && x.SharedItems.Count == 0))
        //            {
        //                if (openClientSessions.Count > 1)
        //                {
        //                    //all sessions without items are garbage, except the last one 
        //                    RemoveClientSession(foodServiceClientSession);
        //                    openClientSessions.Remove(foodServiceClientSession);
        //                }
        //            }
        //        }
        //        //visitor client sessions
        //        openClientSessions = OpenClientSessions.Where(x => x.ClientDeviceID == clientDeviceID).ToList();
        //        if (openClientSessions.Count > 0)
        //        {
        //            foreach (var foodServiceClientSession in openClientSessions.ToList()/*.Where(x => x.Waiter == this)*/.Where(x => x.FlavourItems.Count == 0 && x.SharedItems.Count == 0))
        //            {
        //                if (openClientSessions.Count > 1)
        //                {
        //                    //all sessions without items are garbage except the last one 
        //                    RemoveClientSession(foodServiceClientSession);
        //                    openClientSessions.Remove(foodServiceClientSession);
        //                }
        //            }
        //        }

        //        stateTransition.Consistent = true;
        //    }
        //}

        //private void RemoveClientSession(FoodServiceClientSession foodServiceClientSession)
        //{
        //    var mainSession = foodServiceClientSession.MainSession;
        //    if (mainSession != null)
        //        mainSession.RemovePartialSession(foodServiceClientSession);


        //    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(foodServiceClientSession);
        //    ServicePointRunTime.ServicesContextRunTime.Current.RemoveClientSession(foodServiceClientSession);

        //    if (mainSession != null && mainSession.PartialClientSessions.All(x => x.Forgotten))
        //    {
        //        using (SystemStateTransition innerStateTransition = new SystemStateTransition(TransactionOption.Required))
        //        {
        //            foreach (var partialClientSession in mainSession.PartialClientSessions)
        //                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(partialClientSession);
        //            OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(mainSession);
        //            innerStateTransition.Consistent = true;
        //        }
        //    }
        //}

        /// <exclude>Excluded</exclude>
        ServicePointRunTime.ServicesContextRunTime _ServicesContextRunTime;
        /// <MetaDataID>{54737567-c31c-42ab-8d40-8334f928483a}</MetaDataID>
        ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime
        {
            get
            {
                if (_ServicesContextRunTime == null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));
                    _ServicesContextRunTime = (from runTime in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>() select runTime).FirstOrDefault();
                }
                return _ServicesContextRunTime;

            }
        }

        /// <MetaDataID>{dacf638f-ca24-4200-907a-9633288ccc3f}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+10")]
        private string MealTypesUris;

        /// <exclude>Excluded</exclude>
        List<IMealType> _ServesMealTypes;

        /// <MetaDataID>{686cd5b3-a352-4230-9143-a6cf92b45210}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        public IList<IMealType> ServesMealTypes
        {
            get
            {

                if (_ServesMealTypes == null)
                    _ServesMealTypes = (from mealTypeUri in ServesMealTypesUris
                                        select ObjectStorage.GetObjectFromUri(mealTypeUri) as IMealType).Where(x => x != null).ToList();
                return _ServesMealTypes.ToList();
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IServiceArea> _ServiceArea = new OOAdvantech.Member<IServiceArea>();


        public event ObjectChangeStateHandle _ObjectChangeState;
        public event ObjectChangeStateHandle ObjectChangeState
        {
            add
            {
                _ObjectChangeState += value;
            }
            remove
            {
                _ObjectChangeState -= value;
            }
        }


        /// <MetaDataID>{7e5c81f0-c1d2-4ea7-931a-1f6884181a3c}</MetaDataID>
        [PersistentMember(nameof(_ServiceArea))]
        [BackwardCompatibilityID("+9")]
        public IServiceArea ServiceArea => _ServiceArea.Value;

        /// <MetaDataID>{9be98e3b-defe-473e-99a6-84d3e179f6e8}</MetaDataID>
        public IList<string> ServesMealTypesUris
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MealTypesUris))
                    return new List<string>();
                return MealTypesUris.Split(';');
            }
        }

        /// <exclude>Excluded</exclude>
        ServicePointType _ServicePointType = ServicePointType.HallServicePoint;
        /// <MetaDataID>{95d60c6b-ecf6-42f4-b9fc-87c79c614974}</MetaDataID>
        [PersistentMember(nameof(_ServicePointType))]
        [BackwardCompatibilityID("+11")]
        public ServicePointType ServicePointType
        {
            get => _ServicePointType;
            set
            {
                if (_ServicePointType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicePointType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        public List<FoodServiceSession> OpenSessions
        {
            get
            {
                return ServicePointRunTime.ServicesContextRunTime.Current.OpenSessions.Where(x => x.ServicePoint == this).ToList();

            }
        }

        public List<FoodServiceClientSession> OpenClientSessions
        {
            get
            {
                return ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions.Where(x => x.ServicePoint == this).ToList();
            }
        }

        /// <MetaDataID>{181eef1f-e954-4ca8-a85a-088ceab9c8f9}</MetaDataID>
        public void AddMealType(string mealTypeUri)
        {

            int mealTypesCount = ServesMealTypes.Count;//force system to load mealTypes;
            if (ServesMealTypesUris.Contains(mealTypeUri))
                return;

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {

                IMealType mealType = ObjectStorage.GetObjectFromUri(mealTypeUri) as IMealType;
                if (mealType == null)
                    throw new System.Exception("Invalid meal type uri");
                _ServesMealTypes.Add(mealType);
                MealTypesUris = null;
                foreach (var uri in _ServesMealTypes.Select(x => ObjectStorage.GetStorageOfObject(x).GetPersistentObjectUri(x)))
                {
                    if (MealTypesUris != null)
                        MealTypesUris += ";";
                    MealTypesUris += uri;
                }
                stateTransition.Consistent = true;
            }

            _ObjectChangeState?.Invoke(this, null);
        }

        /// <MetaDataID>{c08357c9-1a0e-4edd-b5d8-0f31c8459f2c}</MetaDataID>
        public void RemoveMealType(string mealTypeUri)
        {
            if (!ServesMealTypesUris.Contains(mealTypeUri))
                return;
            int mealTypesCount = ServesMealTypes.Count;//force system to load mealTypes;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                IMealType mealType = ObjectStorage.GetObjectFromUri(mealTypeUri) as IMealType;
                if (mealType == null)
                    throw new System.Exception("Invalid meal type uri");
                _ServesMealTypes.Remove(mealType);
                MealTypesUris = null;
                foreach (var uri in _ServesMealTypes.Select(x => ObjectStorage.GetStorageOfObject(x).GetPersistentObjectUri(x)))
                {
                    if (MealTypesUris != null)
                        MealTypesUris += ";";
                    MealTypesUris += uri;
                }
                stateTransition.Consistent = true;
            }
            _ObjectChangeState?.Invoke(this, nameof(ServesMealTypesUris));
        }

        /// <MetaDataID>{c7d46722-ff23-48b3-a51e-3a7e3bbe7e3a}</MetaDataID>
        internal void ChangeServicePointState(ServicePointState newState)
        {
            if (State != newState)
            {
                var oldState = State;
                State = newState;
                switch (State)
                {
                    case ServicePointState.Laying:
                        {

                            this.ServicesContextRunTime.ServicePointChangeState(this, oldState, newState);
                            break;
                        }
                    default:
                        _ObjectChangeState?.Invoke(this, nameof(State));
                        break;
                }
            }
        }

        /// <MetaDataID>{34345d6f-bc61-494d-9814-7644ba581c12}</MetaDataID>
        public IList<IFoodServiceClientSession> GetServicePointOtherPeople(IFoodServiceClientSession serviceClientSession)
        {

            
            var collection = (from foodServiceClient in ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions
                              where foodServiceClient.ServicePoint == this && foodServiceClient != serviceClientSession
                              select foodServiceClient).ToList();

            if (!serviceClientSession.IsWaiterSession)
            {
                collection = (from foodServiceClient in collection
                              where !foodServiceClient.IsWaiterSession&&!foodServiceClient.LongTimeForgotten
                              select foodServiceClient).ToList();
            }



            return collection.OfType<IFoodServiceClientSession>().ToList();
        }

        /// <MetaDataID>{4bcce2d4-e720-47bd-bafe-b5dea36afbc3}</MetaDataID>
        internal bool IsAssignedTo(IWaiter waiter, IShiftWork shiftWork)
        {
            return true;
        }

    }




}
