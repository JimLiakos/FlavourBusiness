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
using FlavourBusinessFacade;
using System.Data;
using System.Security.Authentication;
using OOAdvantech.Remoting;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{680d7ca1-c381-4be1-890b-e1b21451126e}</MetaDataID>
    [BackwardCompatibilityID("{680d7ca1-c381-4be1-890b-e1b21451126e}")]
    [Persistent()]
    public class ServicePoint : ExtMarshalByRefObject, IServicePoint
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
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


        /// <MetaDataID>{6fea060f-d976-4e87-91d0-47732725d99f}</MetaDataID>
        public ServicePoint()
        {
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

        /// <exclude>Excluded</exclude>
        protected string _Description;
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

        /// <MetaDataID>{6a624ba0-8a41-422c-bf35-d52185e0bbc6}</MetaDataID>
        public string ServicePointUrl
        {
            get
            {
                return string.Format("{0}:4300/#/launch-app?sc={1}&sp={2}", FlavourBusinessFacade.ComputingResources.EndPoint.Server, ServicesContextIdentity, ServicesPointIdentity);
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

        /// <MetaDataID>{d79628e5-95b4-4abf-b51c-f65fe2a29806}</MetaDataID>
        internal void UpdateState()
        {


            if (OpenClientSessions.Count == 0)
                ChangeServicePointState(ServicePointState.Free);
            else
            {



                var mealCourses = (from servicePointOpenSession in OpenSessions
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
                    var servicePointMealCourses = (from servicePointOpenSession in OpenSessions
                                                   where servicePointOpenSession.Meal != null
                                                   from mealCourse in servicePointOpenSession.Meal.Courses
                                                   select mealCourse).ToList();
                    if (servicePointMealCourses.Count > 0 && servicePointMealCourses.All(x => x.PreparationState == FlavourBusinessFacade.RoomService.ItemPreparationState.Served))
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

        /// <MetaDataID>{e85c6108-a826-45f4-a2b9-bd7b288d9073}</MetaDataID>
        internal void RunObjectChangeState(HomeDeliveryServicePoint homeDeliveryServicePoint, string member)
        {
            _ObjectChangeState?.Invoke(this, member);
        }

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
        public IList<IFoodServiceClientSession> ActiveFoodServiceClientSessions
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
        public virtual IFoodServiceClientSession NewFoodServiceClientSession(string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken)
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
        public virtual IFoodServiceClientSession GetFoodServiceClientSession(string clientName, string mealInvitationSessionID, string clientDeviceID, DeviceType deviceType, DeviceOS deviceOS, string deviceFirebaseToken, bool endUser, bool create = false)
        {
            AuthUserRef authUserRef = AuthUserRef.GetCallContextAuthUserRef(false);
            FlavourBusinessFacade.IUser user = null;

            Waiter waiter = null;
            if (authUserRef != null)
                user = waiter = authUserRef.GetContextRoleObject<Waiter>();

            if (!endUser && waiter == null)
                throw new AuthenticationException("User hasn't access right for this action");

            if (endUser)
                waiter = null;


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
https://play.google.com/store/apps/details?id=com.arion.deliveries;sp=servicepointid_Mega 

//fsClientSession = ServicesContextRunTime.NewFoodServiceClientSession(fsClientSession);
                        fsClientSession = new EndUsers.FoodServiceClientSession();
                        fsClientSession.ClientName = clientName;
                        fsClientSession.ClientDeviceID = clientDeviceID;
                        fsClientSession.ClientDeviceType = deviceType;
                        fsClientSession.DeviceFirebaseToken = deviceFirebaseToken;
                        fsClientSession.SessionStarts = DateTime.UtcNow;
                        fsClientSession.ModificationTime = DateTime.UtcNow;
                        fsClientSession.PreviousYouMustDecideMessageTime = DateTime.UtcNow;

                        if (this is HallServicePoint)
                            fsClientSession.SessionType = SessionType.Hall;

                        if (this is HomeDeliveryServicePoint)
                            fsClientSession.SessionType = SessionType.HomeDelivery;

                        if (this is TakeAwayStation)
                            fsClientSession.SessionType = SessionType.Takeaway;


                        if (user != null && fsClientSession != null && user.Identity != fsClientSession.UserIdentity)
                            fsClientSession.UserIdentity = user.Identity;

                        if (waiter != null)
                        {
                            fsClientSession.IsWaiterSession = true;
                            ((user as HumanResources.Waiter).ShiftWork as ServingShiftWork).AddClientSession(fsClientSession);
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

                    if (!fsClientSession.IsWaiterSession && State == ServicePointState.Free)
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


                        if (waiter != null && fsClientSession.Worker == null)
                        {
                            fsClientSession.IsWaiterSession = true;

                            ((user as HumanResources.Waiter).ShiftWork as ServingShiftWork).AddClientSession(fsClientSession);

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
            fsClientSession.ClientDeviceType = deviceType;
            fsClientSession.ClientDeviceOS = deviceOS;
            return fsClientSession;
        }



        /// <exclude>Excluded</exclude>
        ServicePointRunTime.ServicesContextRunTime _ServicesContextRunTime;
        /// <MetaDataID>{54737567-c31c-42ab-8d40-8334f928483a}</MetaDataID>
        ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime
        {
            get
            {
                if (_ServicesContextRunTime == null)
                    _ServicesContextRunTime = ServicePointRunTime.ServicesContextRunTime.Current;
                //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));
                //_ServicesContextRunTime = (from runTime in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>() select runTime).FirstOrDefault();

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




        /// <exclude>Excluded</exclude>
        ServicePointType _ServicePointType = ServicePointType.HallServicePoint;
        /// <MetaDataID>{95d60c6b-ecf6-42f4-b9fc-87c79c614974}</MetaDataID>
        [PersistentMember(nameof(_ServicePointType))]
        [BackwardCompatibilityID("+11")]
        public virtual ServicePointType ServicePointType
        {
            get => _ServicePointType;
            protected set
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

        /// <MetaDataID>{db3ec41f-0bf7-4c7f-8ba3-22a3d0503ec7}</MetaDataID>
        public List<FoodServiceSession> OpenSessions
        {
            get
            {
                return ServicePointRunTime.ServicesContextRunTime.Current.OpenSessions.Where(x => x.ServicePoint == this).ToList();

            }
        }

        /// <MetaDataID>{49abf041-57e1-44e9-8c00-a42c9992ba8b}</MetaDataID>
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
                _ObjectChangeState?.Invoke(this, nameof(State));
                switch (State)
                {
                    case ServicePointState.Laying:
                        {

                            this.ServicesContextRunTime.ServicePointChangeState(this, oldState, newState);
                            break;
                        }
                    default:
                        break;
                }

            }
        }
        /// <MetaDataID>{8d123473-0138-4e66-8f19-5ba61d5029f8}</MetaDataID>
        public virtual IList<IFoodServiceClientSession> GetServicePointOtherPeople(IFoodServiceClientSession serviceClientSession)
        {
            return new List<IFoodServiceClientSession>();
        }


        /// <MetaDataID>{e1599e2d-a463-4e10-8634-8b7786b64b09}</MetaDataID>
        internal void TransferPartialSession(FoodServiceClientSession partialSessionToTransfer, string targetSessionID)
        {
            lock (ServicePointLock)
            {

                var targetSession = ServicesContextRunTime.OpenSessions.Where(x => x.SessionID == targetSessionID).First();
                var sourceSession = partialSessionToTransfer.MainSession;
                if (partialSessionToTransfer.ServicePoint != this || targetSession.ServicePoint != this)
                    throw new ArgumentException("Invalid partial session transfer.");
                if (targetSession == sourceSession)
                    return;


                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    if (sourceSession != null)
                        sourceSession.RemovePartialSession(partialSessionToTransfer);
                    targetSession.AddPartialSession(partialSessionToTransfer);
                    if (sourceSession != null && sourceSession.Meal == null && sourceSession.PartialClientSessions.Count == 0)
                        ObjectStorage.DeleteObject(sourceSession);

                    stateTransition.Consistent = true;
                }
                foreach (var partialSession in targetSession.PartialClientSessions.OfType<FoodServiceClientSession>())
                {
                    partialSession.RaiseMainSessionChange();
                }


            }



        }



        /// <MetaDataID>{b1e7aef4-92d8-4acc-82fa-5a603735d8f4}</MetaDataID>
        public static void TransferSession(IFoodServiceSession foodServiceSession, string targetServicePointIdentity)
        {
            if (foodServiceSession == null)
                return;
            if (foodServiceSession.ServicePoint.ServicesPointIdentity == targetServicePointIdentity)
                return;

            ServicePoint targetServicePoint = null;
            if (ServicePointRunTime.ServicesContextRunTime.Current.DeliveryServicePoint.ServicesPointIdentity == targetServicePointIdentity)
                targetServicePoint = ServicePointRunTime.ServicesContextRunTime.Current.DeliveryServicePoint as HomeDeliveryServicePoint;
            else
                targetServicePoint = (from serviceArea in ServicePointRunTime.ServicesContextRunTime.Current.ServiceAreas
                                      from servicePoint in serviceArea.ServicePoints
                                      where servicePoint.ServicesPointIdentity == targetServicePointIdentity
                                      select servicePoint).OfType<ServicePoint>().FirstOrDefault();

            if (ServicePointRunTime.ServicesContextRunTime.Current.DeliveryServicePoint.ServicesPointIdentity == targetServicePointIdentity)
                targetServicePoint = ServicePointRunTime.ServicesContextRunTime.Current.DeliveryServicePoint as ServicePoint;


            if (ServicePointRunTime.ServicesContextRunTime.Current.TakeAwayStations.Where(x => x.ServicesPointIdentity == targetServicePointIdentity).Count() == 1)
                targetServicePoint = ServicePointRunTime.ServicesContextRunTime.Current.TakeAwayStations.Where(x => x.ServicesPointIdentity == targetServicePointIdentity).FirstOrDefault() as ServicePoint;



            if (targetServicePoint == null)
                throw new ArgumentException("There is no service with identity, the value of 'targetServicePointIdentity' parameter");
            else
            {
                FoodServiceSession servicePointLastOpenSession = null;
                if (targetServicePoint is HallServicePoint)
                    servicePointLastOpenSession = targetServicePoint.OpenSessions.OrderBy(x => x.SessionStarts).LastOrDefault();

                if (servicePointLastOpenSession == null)
                {
                    if (targetServicePoint is IHomeDeliveryServicePoint && foodServiceSession.PartialClientSessions.Count > 1)
                        throw new Exception("You cannot transform session to home delivery session");

                    if (targetServicePoint is ITakeAwayStation && foodServiceSession.PartialClientSessions.Count > 1)
                        throw new Exception("You cannot transform session to take away session");

                    (foodServiceSession as FoodServiceSession).ServicePoint = targetServicePoint;
                    targetServicePoint.UpdateState();


                    if (foodServiceSession.Meal != null)
                    {
                        //When meal has change service point, may be changed the waiters which can serve the prepared meal courses  

                        (ServicePointRunTime.ServicesContextRunTime.Current.MealsController as RoomService.MealsController).ReadyToServeMealCoursesCheck(foodServiceSession.Meal.Courses);
                    }


                }
                else
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        servicePointLastOpenSession.Merge(foodServiceSession);

                        (foodServiceSession.ServicePoint as ServicePoint).UpdateState();
                        ObjectStorage.DeleteObject(foodServiceSession);
                        //When meal has change service point, may be changed the waiters which can serve the prepared meal courses  

                        Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                        {
                            if (transaction.Status == TransactionStatus.Committed && servicePointLastOpenSession.Meal != null)
                                (ServicePointRunTime.ServicesContextRunTime.Current.MealsController as RoomService.MealsController).ReadyToServeMealCoursesCheck(servicePointLastOpenSession.Meal.Courses);
                        };

                        stateTransition.Consistent = true;
                    }

                }
            }

            //ObjectChangeState?.Invoke(this, nameof(HallsServicePointsState));

        }


        /// <MetaDataID>{f9df74dc-4dd6-4cf4-afa3-54b0ba4d9f4f}</MetaDataID>
        public static void TransferItems(List<SessionItemPreparationAbbreviation> itemPreparations, string targetServicePointIdentity)
        {

            List<string> constrainErrors = new List<string>();
            List<EndUsers.FoodServiceClientSession> partialSessionsForTransfer = new List<EndUsers.FoodServiceClientSession>();
            List<RoomService.ItemPreparation> itemsForTransfer = new List<RoomService.ItemPreparation>();
            ServicePoint sourceServicePoint = null;

            foreach (var sessionitemsEntry in (from itemPreparation in itemPreparations
                                               group itemPreparation by itemPreparation.SessionID into SessionItems
                                               select SessionItems))
            {

                var itemsUids = sessionitemsEntry.Select(x => x.uid).ToList();
                var session = ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions.OfType<EndUsers.FoodServiceClientSession>().Where(x => x.SessionID == sessionitemsEntry.Key).First();
                sourceServicePoint = session.ServicePoint as ServicePoint;

                if (session.FlavourItems.Union(session.SharedItems).Distinct().All(x => itemsUids.Contains(x.uid)))
                    partialSessionsForTransfer.Add(session);
                else if (!session.IsWaiterSession)
                {
                    constrainErrors.Add(string.Format("Partial transfer of the '{0}' FoodServiceClientSession is not possible", session.ClientName));
                    continue;
                }



                var sessionItemsForTransfer = session.FlavourItems.Union(session.SharedItems).Distinct().OfType<RoomService.ItemPreparation>().Where(sessionItem => itemPreparations.Any(x => x.uid == sessionItem.uid)).ToList();


                var itemsSharings = (from sessionItem in sessionItemsForTransfer
                                     from sessionID in sessionItem.SharedInSessions
                                     select new { sessionID, sessionItem.uid, sessionItem }).ToList();

                foreach (var itemSharing in itemsSharings)
                {
                    if (!itemPreparations.Any(x => x.SessionID == itemSharing.sessionID && x.uid == itemSharing.uid))
                    {
                        constrainErrors.Add(string.Format("Partial transfer of the sharing item {0} is not possible", itemSharing.sessionItem.Name));
                        var sessionItem = sessionItemsForTransfer.Where(x => x.uid == itemSharing.uid).FirstOrDefault();
                        if (sessionItem != null)
                            sessionItemsForTransfer.Remove(sessionItem);
                    }
                }


                foreach (var sessionItem in sessionItemsForTransfer.ToList())
                {
                    if (sessionItem.PaidAmounts.Count > 0)
                    {
                        constrainErrors.Add(string.Format("Partial transfer of the paid item {0} is not possible", sessionItem.Name));
                        sessionItemsForTransfer.Remove(sessionItem);
                    }
                    else
                    {
                        if (!partialSessionsForTransfer.Contains(sessionItem.ClientSession))
                            itemsForTransfer.Add(sessionItem);
                    }
                }
            }
            if (constrainErrors.Count > 0)
            {
                string message = null;
                foreach (var constrainError in constrainErrors)
                {
                    if (message != null)
                        message += Environment.NewLine;
                    message += constrainError;
                }
                throw new ArgumentException(message);
            }



            var targetServicePoint = (from serviceArea in ServicePointRunTime.ServicesContextRunTime.Current.ServiceAreas
                                      from servicePoint in serviceArea.ServicePoints
                                      where servicePoint.ServicesPointIdentity == targetServicePointIdentity
                                      select servicePoint).OfType<ServicePoint>().FirstOrDefault();


            if (targetServicePoint == null)
                throw new ArgumentException("There is no service with identity, the value of 'targetServicePointIdentity' parameter");
            else
            {
                List<FoodServiceSession> sessionsForTransfer = new List<FoodServiceSession>();


                var servicePointLastOpenSession = targetServicePoint.OpenSessions.OrderBy(x => x.SessionStarts).LastOrDefault();
                foreach (var session in partialSessionsForTransfer.ToList())
                {
                    if (session.MainSession != null && !sessionsForTransfer.Contains(session.MainSession))
                    {
                        var movingItemsuids = itemPreparations.Select(x => x.uid).ToList();

                        var sessionItems = (from partialSession in session.MainSession.PartialClientSessions
                                            from flavourItem in partialSession.FlavourItems
                                            select flavourItem);

                        if (sessionItems.All(x => movingItemsuids.Contains(x.uid))) //all session item will be transfered
                            sessionsForTransfer.Add(session.MainSession as FoodServiceSession);
                    }
                }
                foreach (var session in sessionsForTransfer)
                {
                    foreach (var partialSession in session.PartialClientSessions.OfType<EndUsers.FoodServiceClientSession>())
                    {
                        if (partialSessionsForTransfer.Contains(partialSession))
                            partialSessionsForTransfer.Remove(partialSession);
                    }
                }


                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {


                    foreach (var session in sessionsForTransfer)
                        TransferSession(session, targetServicePointIdentity);

                    foreach (var partialSession in partialSessionsForTransfer)
                    {

                        EndUsers.FoodServiceClientSession targetServicePointExistingPartialSession = null;
                        if (partialSession.IsWaiterSession)
                            targetServicePointExistingPartialSession = targetServicePoint.GetFoodServiceClientSession(partialSession.ClientName, null, partialSession.ClientDeviceID, partialSession.ClientDeviceType, partialSession.ClientDeviceOS, partialSession.DeviceFirebaseToken, !partialSession.IsWaiterSession, true) as EndUsers.FoodServiceClientSession;

                        if (targetServicePointExistingPartialSession != null)
                        {
                            targetServicePointExistingPartialSession.Merge(partialSession);
                        }
                        else
                        {


                            if (partialSession.MainSession != null && servicePointLastOpenSession == null)
                            {
                                servicePointLastOpenSession = targetServicePoint.NewFoodServiceSession() as FoodServiceSession;
                                ObjectStorage.GetStorageOfObject(targetServicePoint).CommitTransientObjectState(servicePointLastOpenSession);
                            }

                            partialSession.ServicePoint = targetServicePoint;
                            if (partialSession.MainSession != null)
                                partialSession.MainSession.RemovePartialSession(partialSession);

                            if (servicePointLastOpenSession != null)
                                servicePointLastOpenSession.AddPartialSession(partialSession);
                        }
                    }

                    foreach (var item in itemsForTransfer)
                    {
                        var foodServiceClientSession = targetServicePoint.GetFoodServiceClientSession((item.ClientSession as EndUsers.FoodServiceClientSession).ClientName, null, (item.ClientSession as EndUsers.FoodServiceClientSession).ClientDeviceID, (item.ClientSession as EndUsers.FoodServiceClientSession).ClientDeviceType, (item.ClientSession as EndUsers.FoodServiceClientSession).ClientDeviceOS,(item.ClientSession as EndUsers.FoodServiceClientSession).DeviceFirebaseToken, !(item.ClientSession as EndUsers.FoodServiceClientSession).IsWaiterSession, true) as EndUsers.FoodServiceClientSession;
                        foodServiceClientSession.Merge(item);
                    }

                    if ((from partialSession in sourceServicePoint.OpenClientSessions
                         from flavourItem in partialSession.FlavourItems
                         select flavourItem).Count() == 0)
                    {
                        sourceServicePoint.ChangeServicePointState(ServicePointState.Free);
                    }

                    if ((from partialSession in targetServicePoint.OpenClientSessions
                         from flavourItem in partialSession.FlavourItems
                         select flavourItem).Count() > 0 && (targetServicePoint.State == ServicePointState.Free || targetServicePoint.State == ServicePointState.Laying))
                    {
                        targetServicePoint.ChangeServicePointState(ServicePointState.Conversation);
                    }

                    Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                    {
                        foreach (var waiterFoodServiceClientSession in sourceServicePoint.OpenClientSessions.Where(x => x.IsWaiterSession))
                        {
                            try
                            {
                                waiterFoodServiceClientSession.RaiseMainSessionChange();
                            }
                            catch (Exception error)
                            {
                            }
                        }

                        foreach (var waiterFoodServiceClientSession in targetServicePoint.OpenClientSessions.Where(x => x.IsWaiterSession))
                        {
                            try
                            {
                                waiterFoodServiceClientSession.RaiseMainSessionChange();
                            }
                            catch (Exception error)
                            {
                            }
                        }
                    };
                    stateTransition.Consistent = true;
                }



            }




        }


        /// <MetaDataID>{85bd4b65-edff-4bc1-95ac-7ec4f7ac7274}</MetaDataID>
        public static void TransferPartialSession(string partialSessionID, string targetSessionID)
        {
            var partialSession = ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions.Where(x => x.SessionID == partialSessionID).First();

            (partialSession.ServicePoint as ServicePoint).TransferPartialSession(partialSession, targetSessionID);
        }

        public IFlavoursServicesContextRuntime GetServiceContextRuntime()
        {
            return ServicePointRunTime.ServicesContextRunTime.Current;
        }
    }
}

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{5a6deea9-0208-441b-9b01-77a13dc8c126}</MetaDataID>
    [BackwardCompatibilityID("{5a6deea9-0208-441b-9b01-77a13dc8c126}")]
    [Persistent()]
    public class HallServicePoint : ServicePoint, IHallServicePoint, OOAdvantech.Remoting.IExtMarshalByRefObject
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

        public override ServicePointType ServicePointType
        {
            get
            {
                base.ServicePointType = ServicePointType.HallServicePoint;
                return ServicePointType.HallServicePoint;
            }
            protected set
            {
            }
        }



        /// <MetaDataID>{4bcce2d4-e720-47bd-bafe-b5dea36afbc3}</MetaDataID>
        internal bool CanBeAssignedTo(IWaiter waiter, IShiftWork shiftWork)
        {
            return true;
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IServiceArea> _ServiceArea = new OOAdvantech.Member<IServiceArea>();

        /// <MetaDataID>{7e5c81f0-c1d2-4ea7-931a-1f6884181a3c}</MetaDataID>
        [PersistentMember(nameof(_ServiceArea))]
        [BackwardCompatibilityID("+9")]
        public IServiceArea ServiceArea => _ServiceArea.Value;


        /// <MetaDataID>{262fbc71-1863-4a2c-a440-c8fe77c00a13}</MetaDataID>
        public override IList<IFoodServiceClientSession> GetServicePointOtherPeople(IFoodServiceClientSession serviceClientSession)
        {

            var collection = (from foodServiceClient in ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions
                              where foodServiceClient.ServicePoint == this && foodServiceClient != serviceClientSession
                              select foodServiceClient).ToList();

            if (!serviceClientSession.IsWaiterSession)
            {
                collection = (from foodServiceClient in collection
                              where !foodServiceClient.IsWaiterSession && !foodServiceClient.LongTimeForgotten
                              select foodServiceClient).ToList();
            }



            return collection.OfType<IFoodServiceClientSession>().ToList();
        }



    }




}
