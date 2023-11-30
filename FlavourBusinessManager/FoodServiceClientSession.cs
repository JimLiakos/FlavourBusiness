using System;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using OOAdvantech.Transactions;
using FlavourBusinessFacade;
using System.Security.Authentication;
using OOAdvantech.PersistenceLayer;
using FlavourBusinessFacade.RoomService;
using Microsoft.WindowsAzure.ServiceRuntime;
using FlavourBusinessManager.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using FlavourBusinessManager.ServicePointRunTime;
using System.Globalization;
using FlavourBusinessFacade.HumanResources;
using System.Threading.Tasks;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Microsoft.Azure.Documents.Spatial;
using MenuPresentationModel;
using FlavourBusinessManager.HumanResources;

namespace FlavourBusinessManager.EndUsers
{





    /// <MetaDataID>{174fc4ef-b90f-4a30-892d-7cc9c28f8fe0}</MetaDataID>
    [BackwardCompatibilityID("{174fc4ef-b90f-4a30-892d-7cc9c28f8fe0}")]
    [Persistent()]
    public class FoodServiceClientSession : MarshalByRefObject, IFoodServiceClientSession, OOAdvantech.Remoting.IExtMarshalByRefObject, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer, FinanceFacade.IPaymentGateway
    {

        /// <exclude>Excluded</exclude>
        string _UserLanguageCode;
        /// <MetaDataID>{91d4ad32-e902-4f40-94d8-25b67f2611d2}</MetaDataID>
        [PersistentMember(nameof(_UserLanguageCode))]
        [BackwardCompatibilityID("+29")]
        public string UserLanguageCode
        {
            get => _UserLanguageCode;
            set
            {
                if (_UserLanguageCode != value)
                {

                    string neutralLang = OOAdvantech.CultureContext.GetNeutralCultureInfo(value)?.Name;
                    if (neutralLang != null)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _UserLanguageCode = neutralLang;
                            stateTransition.Consistent = true;
                        }
                    }
                }

            }
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<HumanResources.ServingShiftWork> _SessionCreator = new OOAdvantech.Member<HumanResources.ServingShiftWork>();

        [Association("FoodServiceClientSessionInTheShiftWork", Roles.RoleB, "2076b2c6-2c5e-415a-93ab-187654f7c04a")]
        [PersistentMember(nameof(_SessionCreator))]
        public HumanResources.ServingShiftWork SessionCreator => _SessionCreator.Value;



        /// <MetaDataID>{c04687dc-cdf1-4dfc-96c8-b56e9808f003}</MetaDataID>
        public ChangeDeliveryPlaceResponse CanChangeDeliveryPlace(Coordinate location)
        {
            if (ServicesContextRunTime.Current.DeliveryServicePoint == null)
                return ChangeDeliveryPlaceResponse.InvalidPlace;
            var polyGon = new MapPolyGon(ServicesContextRunTime.Current.DeliveryServicePoint.ServiceAreaMap);
            if (polyGon.FindPoint(location.Latitude, location.Longitude))
            {
                //if(MainSession.SessionState==FlavourBusinessFacade.ServicesContextResources.SessionState.MealValidationDelay
                return ChangeDeliveryPlaceResponse.OK;
            }
            else
                return ChangeDeliveryPlaceResponse.OutOfServiceAreaMap;
        }
        /// <exclude>Excluded</exclude>
        bool _ImplicitMealParticipation;

        /// <MetaDataID>{d7711e10-32f3-43d6-bc37-0005e7a4f1ed}</MetaDataID>
        [PersistentMember(nameof(_ImplicitMealParticipation))]
        [BackwardCompatibilityID("+24")]
        public bool ImplicitMealParticipation
        {
            get => _ImplicitMealParticipation;
            set
            {

                if (_ImplicitMealParticipation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ImplicitMealParticipation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <MetaDataID>{93fe70c7-0ac7-4265-b46a-6b889127b1a2}</MetaDataID>
        public ClientSessionData ClientSessionData
        {
            get
            {
                if (this.ServicePoint == null)
                {

                }
                var defaultMealTypeUri = this.ServicePoint.ServesMealTypesUris.FirstOrDefault();
                var servedMealTypesUris = this.ServicePoint.ServesMealTypesUris.ToList();

                if (defaultMealTypeUri == null && ServicePoint is IHallServicePoint)
                {
                    defaultMealTypeUri = (this.ServicePoint as IHallServicePoint).ServiceArea.ServesMealTypesUris.FirstOrDefault();
                    servedMealTypesUris = (this.ServicePoint as IHallServicePoint).ServiceArea.ServesMealTypesUris.ToList();
                }

                if (defaultMealTypeUri == null && ServicePoint is IHomeDeliveryServicePoint)
                {
                    defaultMealTypeUri = ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri;
                    servedMealTypesUris = new List<string>() { ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri };
                }
                if (defaultMealTypeUri == null && ServicePoint is ITakeAwayStation)
                {
                    defaultMealTypeUri = ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri;
                    servedMealTypesUris = new List<string>() { ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri };
                }

                //"sc=7f9bde62e6da45dc8c5661ee2220a7b0&sp=50886542db964edf8dec5734e3f89395"

                //Uri myUri = new Uri("http://" + ServicePoint.ServicePointUrl);
                //string param1 =HttpUtility.ParseQueryString(myUri.Query).Get("sc");

                //"sc=7f9bde62e6da45dc8c5661ee2220a7b0&sp=50886542db964edf8dec5734e3f89395"
                return new ClientSessionData() { ServicesContextLogo = "Pizza Hut", ServicesPointName = ServicePoint.Description, SessionType = SessionType, ServicePointIdentity = ServicePoint.ServicesContextIdentity + ";" + ServicePoint.ServicesPointIdentity, Token = ServicesContextRunTime.GetToken(this), FoodServiceClientSession = this, Menu = Menu, ServedMealTypesUris = servedMealTypesUris, DefaultMealTypeUri = defaultMealTypeUri, ServicePointState = ServicePoint.State, UserLanguageCode = UserLanguageCode, DeliveryPlace = GetSessionDeliveryPlace() };

            }
        }

        /// <exclude>Excluded</exclude>
        string _DeviceFirebaseToken;

        /// <MetaDataID>{0e2fa77e-b85e-4a32-b609-648f957abe94}</MetaDataID>
        /// <summary>This token is the identity of device for push notification mechanism</summary>
        [PersistentMember(nameof(_DeviceFirebaseToken))]
        [BackwardCompatibilityID("+10")]
        public string DeviceFirebaseToken
        {
            get => _DeviceFirebaseToken;
            set
            {
                if (_DeviceFirebaseToken != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceFirebaseToken = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _IsWaiterSession;

        /// <MetaDataID>{93acba25-648a-45a6-9326-18b83386e592}</MetaDataID>
        [PersistentMember(nameof(_IsWaiterSession))]
        [BackwardCompatibilityID("+20")]
        public bool IsWaiterSession
        {
            get => _IsWaiterSession;
            set
            {
                if (_IsWaiterSession != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsWaiterSession = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <exclude>Excluded</exclude>
        int _YouMustDecideMessagesNumber;

        /// <MetaDataID>{a5872276-63d8-4b8c-8706-7741f1c27c2d}</MetaDataID>
        [PersistentMember(nameof(_YouMustDecideMessagesNumber))]
        [BackwardCompatibilityID("+19")]
        public int YouMustDecideMessagesNumber
        {
            get => _YouMustDecideMessagesNumber;
            set
            {
                if (_YouMustDecideMessagesNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _YouMustDecideMessagesNumber = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{62cbfab3-8ea9-492f-954e-f798e772c30e}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+18")]
        public DateTime DeviceAppActivationTime;

        /// <MetaDataID>{fa430d24-c820-4251-8d0e-dc76ad315257}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+17")]
        public DateTime DeviceAppSleepTime;

        /// <MetaDataID>{4c36cce8-fe2e-4c31-99fb-cfc1e95e300f}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+16")]
        public DeviceAppLifecycle DeviceAppState;


        /// <exclude>Excluded</exclude>
        DateTime _ModificationTime;

        /// <MetaDataID>{9366a845-cf06-492d-89d6-862ec27aaa3f}</MetaDataID>
        [PersistentMember(nameof(_ModificationTime))]
        [BackwardCompatibilityID("+15")]
        public DateTime ModificationTime
        {
            get => _ModificationTime;
            set
            {
                if (_ModificationTime != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ModificationTime = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }






        /// <MetaDataID>{f0df519b-0f4b-4647-9031-7f59614e69df}</MetaDataID>
        object CaregiversLock = new object();

        /// <MetaDataID>{afe80e14-7b7f-45a3-a8ac-3ef3850740b2}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+26")]
        string WillTakeCareWorkersJson;



        /// <exclude>Excluded</exclude>
        DateTime _WillTakeCareTimestamp;

        /// <MetaDataID>{941cf7d4-3c5e-4589-87cf-a01362e26dc0}</MetaDataID>
        [PersistentMember(nameof(_WillTakeCareTimestamp))]
        [BackwardCompatibilityID("+25")]
        public System.DateTime WillTakeCareTimestamp
        {
            get => _WillTakeCareTimestamp;
            set
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



        /// <MetaDataID>{b3869d58-92cd-4522-bf59-32c60cc7844c}</MetaDataID>
        public void AddCaregiver(IServicesContextWorker caregiver, Caregiver.CareGivingType caregivingType)
        {
            lock (CaregiversLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Caregivers.Add(new Caregiver() { Worker = caregiver, CareGiving = caregivingType, WillTakeCareTimestamp = DateTime.UtcNow });
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        List<Caregiver> _Caregivers = new List<Caregiver>();

        /// <MetaDataID>{a981dbd8-72be-48b0-90ba-606940b4f3e5}</MetaDataID>
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

        /// <MetaDataID>{56afdc1c-d6ee-42f0-9b81-44125cc8a57a}</MetaDataID>
        public void AcceptMealInvitation(string clientSessionToken, IFoodServiceClientSession messmateClientSesion)
        {
            bool mainSessionUpdate = MainSession != null;
            string token = null;
            ServicePointRunTime.ServicesContextRunTime.FoodServiceClientSessionsTokens.TryGetValue(this, out token);
            if (string.IsNullOrWhiteSpace(token) || token != clientSessionToken)
                throw new AuthenticationException("invalid token or token expired");

            MakePartOfMeal(messmateClientSesion);

            if (mainSessionUpdate)
                ObjectChangeState?.Invoke(this, nameof(MainSession));

            (messmateClientSesion as FoodServiceClientSession).MealInvitationAccepted(this);

            foreach (var waiterFoodServiceClientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession))
            {
                try
                {
                    waiterFoodServiceClientSession.RaiseMainSessionChange();
                }
                catch (Exception error)
                {
                }
            }

        }

        /// <MetaDataID>{8e3f84e8-e9aa-4590-972f-053a864069be}</MetaDataID>
        private void MealInvitationAccepted(FoodServiceClientSession foodServiceClientSession)
        {
            ObjectChangeState?.Invoke(this, nameof(MainSession));
        }


        /// <summary>
        /// Defines Main session which defined explicitly from meal invitation
        /// </summary>
        /// <MetaDataID>{32b57809-617f-4da5-90cc-4b4b95278fbf}</MetaDataID>
        private IFoodServiceSession ExplicitMainSession
        {
            get
            {
                if (!ImplicitMealParticipation)
                    return _MainSession.Value;
                else
                    return null;
            }
        }



        /// <summary>
        /// This method makes messmateClientSesion part of the explicitly defined MainSession
        /// </summary>
        /// <param name="messmateClientSesion">
        /// Defines the client session which will be part of the explicitly defined MainSession
        /// </param>
        /// <MetaDataID>{bce5f9e3-648f-4e89-8d76-fae5e3b3d0bd}</MetaDataID>
        internal void MakePartOfMeal(IFoodServiceClientSession messmateClientSesion)
        {

            if (messmateClientSesion.ServicePoint != ServicePoint)
                throw new Exception("Invalid part of meal request. messmate " + messmateClientSesion + " is connected to other service point");

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (ExplicitMainSession == null)
                {
                    var implicitMainSession = _MainSession.Value;
                    if ((messmateClientSesion as FoodServiceClientSession).ExplicitMainSession != null)
                    {

                        // Makes this partial session with implicit main session part of messmate session explicitly
                        if (_MainSession.Value != null)
                            _MainSession.Value.RemovePartialSession(this);

                        _MainSession.Value = null;
                        ImplicitMealParticipation = false;

                        (messmateClientSesion as FoodServiceClientSession).ExplicitMainSession.AddPartialSession(this);
                    }
                    else
                    {
                        var implicitMainSessionMainSession = messmateClientSesion.MainSession;
                        FoodServiceSession foodServiceSession = null;
                        //Tries to find a session which can host  both this and the messmate, partial sessions

                        //Session where all partial session participate  implicitly can used to  host both this and messmate, partial session explicitly

                        if (implicitMainSession != null && implicitMainSession.PartialClientSessions.All(x => x.ImplicitMealParticipation))
                            foodServiceSession = implicitMainSession as FoodServiceSession;
                        else if (implicitMainSessionMainSession != null && implicitMainSessionMainSession.PartialClientSessions.All(x => x.ImplicitMealParticipation))
                            foodServiceSession = implicitMainSessionMainSession as FoodServiceSession;

                        if (foodServiceSession != null)
                        {
                            // if all partial session are participate to main session implicitly then keep the main session and
                            // add messmate and this partial session to main session explicitly

                            if (implicitMainSession != null && implicitMainSession != foodServiceSession)
                            {
                                implicitMainSession.RemovePartialSession(this);
                                if (implicitMainSession != null && implicitMainSession.Meal == null && implicitMainSession.PartialClientSessions.Count == 0)
                                    ObjectStorage.DeleteObject(implicitMainSession);
                            }

                            if (implicitMainSessionMainSession != null && implicitMainSessionMainSession != foodServiceSession)
                            {
                                implicitMainSessionMainSession.RemovePartialSession(messmateClientSesion);
                                if (implicitMainSessionMainSession != null && implicitMainSessionMainSession.Meal == null && implicitMainSessionMainSession.PartialClientSessions.Count == 0)
                                    ObjectStorage.DeleteObject(implicitMainSessionMainSession);
                            }
                            ImplicitMealParticipation = false;
                            messmateClientSesion.ImplicitMealParticipation = false;

                            if (foodServiceSession != MainSession)
                                foodServiceSession.AddPartialSession(this);

                            if (foodServiceSession != messmateClientSesion.MainSession)
                                foodServiceSession.AddPartialSession(messmateClientSesion);
                        }
                        else
                        {
                            //there isn't  main session which can host both this and messmate partial session
                            //new session must be created to host this and messmate partial session

                            ImplicitMealParticipation = false;
                            if (_MainSession.Value != null)
                                _MainSession.Value.RemovePartialSession(this);

                            _MainSession.Value = null;

                            foodServiceSession = ServicePoint.NewFoodServiceSession() as FoodServiceSession;
                            ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodServiceSession);
                            if (this.Menu != null)
                                foodServiceSession.MenuStorageIdentity = this.Menu.StorageIdentity;
                            foodServiceSession.AddPartialSession(this);

                            if (implicitMainSessionMainSession != null)
                                implicitMainSessionMainSession.RemovePartialSession(messmateClientSesion);

                            if (implicitMainSessionMainSession != null && implicitMainSessionMainSession.Meal == null && implicitMainSessionMainSession.PartialClientSessions.Count == 0)
                                ObjectStorage.DeleteObject(implicitMainSessionMainSession);
                            foodServiceSession.AddPartialSession(messmateClientSesion);
                        }
                    }


                    if (implicitMainSession != null && implicitMainSession.Meal == null && implicitMainSession.PartialClientSessions.Count == 0)
                        ObjectStorage.DeleteObject(implicitMainSession);


                }
                else
                {
                    if (ExplicitMainSession != (messmateClientSesion as FoodServiceClientSession).ExplicitMainSession)
                    {
                        if (messmateClientSesion.MainSession != null)
                        {
                            (MainSession as FoodServiceSession).Merge(messmateClientSesion.MainSession);
                        }
                        else
                        {
                            _MainSession.Value.AddPartialSession(messmateClientSesion);
                        }
                    }

                }
                if (_MainSession.Value != null)
                {
                    var partialSession = _MainSession.Value.PartialClientSessions.OrderBy(x => x.SessionStarts).FirstOrDefault();
                    if (partialSession != null)
                        _MainSession.Value.SessionStarts = partialSession.SessionStarts;

                }

                messmateClientSesion.ImplicitMealParticipation = false;
                ImplicitMealParticipation = false;
                if (SessionType == SessionType.HomeDelivery)
                    (messmateClientSesion as FoodServiceClientSession).SessionType = SessionType.HomeDeliveryGuest;

                if (SessionType == SessionType.HomeDelivery)
                {
                    (messmateClientSesion as FoodServiceClientSession).SessionType = SessionType.HomeDeliveryGuest;

                }

                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{9d7c7058-1582-4dbf-8727-14540094ec56}</MetaDataID>
        internal void MonitorTick()
        {
            if (MainSession == null)
                CheckForMealConversationTimeout();

        }


        ///// <MetaDataID>{56afdc1c-d6ee-42f0-9b81-44125cc8a57a}</MetaDataID>
        //public void AcceptMealInvitation(string sessionID, FlavourBusinessFacade.EndUsers.IFoodServiceClientSession messmateClientSesion)
        //{

        //}

        /// <exclude>Excluded</exclude>
        string _ClientDeviceID;

        /// <exclude>Excluded</exclude>
        DeviceType _ClientDeviceType;

        /// <MetaDataID>{1fb58963-f51c-43ed-a7b5-0b8ccb8c163a}</MetaDataID>
        [PersistentMember(nameof(_ClientDeviceType))]
        [BackwardCompatibilityID("+30")]
        public DeviceType ClientDeviceType
        {
            get => _ClientDeviceType;
            set
            {
                if (_ClientDeviceType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ClientDeviceType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{de12b069-1aee-4452-a778-d278c7bea787}</MetaDataID>
        [PersistentMember(nameof(_ClientDeviceID))]
        [BackwardCompatibilityID("+12")]
        public string ClientDeviceID
        {
            get
            {
                return _ClientDeviceID;
            }

            set
            {
                //if (string.IsNullOrEmpty(SessionID))
                //{

                if (_ClientDeviceID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ClientDeviceID = value;

                        //_SessionID = Guid.NewGuid().ToString("N") + _ClientDeviceID;
                        stateTransition.Consistent = true;
                    }
                }
                //}
                //else
                //    throw new NotSupportedException("Changing ClientDeviceID is not supported");

            }
        }

        //Previous
        /// <MetaDataID>{202d1d18-93ff-4baa-9888-b0f50544a24e}</MetaDataID>
        internal DateTime? PreviousYouMustDecideMessageTime;


        /// <MetaDataID>{08eb2650-5915-43b8-a130-374992f0985a}</MetaDataID>
        bool DeviceConnectionStatusCheckActive;
        /// <MetaDataID>{b448d9b4-451c-4d2c-a064-71a9b59a635a}</MetaDataID>
        int DeviceConnectionStatusChecksNumber;
        /// <MetaDataID>{b80540b1-2b19-4fbd-887f-58039a11d111}</MetaDataID>
        void StartDeviceConnectionStatusCheck()
        {
            bool deviceConnectionStatusCheckActive = false;
            lock (StateMachineLock)
            {
                deviceConnectionStatusCheckActive = DeviceConnectionStatusCheckActive;
            }
            if (deviceConnectionStatusCheckActive)
                System.Threading.Thread.Sleep(200);

            if (!deviceConnectionStatusCheckActive)
            {
                Task.Run(() =>
                {

                    try
                    {
                        lock (StateMachineLock)
                        {
                            DeviceConnectionStatusCheckActive = true;
                        }
                        ClientSessionState sessionState;

                        lock (StateMachineLock)
                        {
                            sessionState = SessionState;
                        }

                        while (sessionState != ClientSessionState.Closed)
                        {
                            try
                            {
                                #region DeviceAppLifecycle
                                if (DeviceAppState == DeviceAppLifecycle.InUse && _MessageReceived == null)
                                {
                                    //with _MessageReceived system knows indirectly when there is active connection with client device
                                    //when communication session with client device closed the server session part drops all event consumers
                                    if (DeviceConnectionStatusChecksNumber > 5)
                                    {
                                        DeviceSleep();
                                        DeviceConnectionStatusChecksNumber = 0;
                                    }
                                    else
                                        DeviceConnectionStatusChecksNumber++;
                                }
                                else
                                    DeviceConnectionStatusChecksNumber = 0;
                                #endregion

                                CatchStateEvents();


                                lock (StateMachineLock)
                                {
                                    sessionState = SessionState;
                                }

                                if (sessionState == ClientSessionState.UrgesToDecide)
                                    UrgesToDecideRun();

                            }
                            catch (Exception error)
                            {


                            }
                            System.Threading.Thread.Sleep(5000);
                        }
                    }
                    finally
                    {
                        lock (StateMachineLock)
                        {
                            DeviceConnectionStatusCheckActive = false;
                        }
                    }
                });
            }
        }

        /// <MetaDataID>{46b3cc48-30d4-4776-99ef-a8359bfdddb8}</MetaDataID>
        object StateMachineLock = new object();
        /// <MetaDataID>{24a297e4-0669-4061-8ec1-a8d781982ace}</MetaDataID>
        private void CatchStateEvents()
        {
            lock (StateMachineLock)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    var flavourItems = FlavourItems;
                    bool allItemsCommitted = flavourItems.Count != 0 && flavourItems.Where(x => x.State.IsInPreviousState(ItemPreparationState.Committed)).Count() == 0;

                    //state machine must be run out of transaction scoop

                    #region ClientSessionState.Conversation
                    if (SessionState == ClientSessionState.Conversation && DeviceAppState == DeviceAppLifecycle.InUse && allItemsCommitted)
                    {
                        //Commit items  event
                        SessionState = ClientSessionState.ItemsCommited;
                    }

                    if (SessionState == ClientSessionState.Conversation && DeviceAppState != DeviceAppLifecycle.InUse)
                    {
                        if (this.FlavourItems.Count > 0)
                        {
                            //Device standby event
                            SessionState = ClientSessionState.ConversationStandy;
                        }
                        else
                        {
                            SessionState = ClientSessionState.Inactive;
                        }
                    }

                    if (SessionState == ClientSessionState.Conversation && MainSession != null && !ImplicitMealParticipation &&
                                    MainSession.SessionState == FlavourBusinessFacade.ServicesContextResources.SessionState.UrgesToDecide &&
                                    DeviceAppState != DeviceAppLifecycle.InUse)
                    {
                        //Standby on partial commit meal event
                        SessionState = ClientSessionState.UrgesToDecide;
                        //StandbyOnPartialCommitMeal();
                    }
                    #endregion


                    #region  ClientSessionState.ConversationStandy
                    if (SessionState == ClientSessionState.ConversationStandy && MainSession != null && !ImplicitMealParticipation &&
                      MainSession.SessionState == FlavourBusinessFacade.ServicesContextResources.SessionState.UrgesToDecide &&
                      DeviceAppState != DeviceAppLifecycle.InUse)
                    {
                        //One of the messmates commits event
                        SessionState = ClientSessionState.UrgesToDecide;
                        //StandbyOnPartialCommitMeal();
                    }

                    if (SessionState == ClientSessionState.ConversationStandy && DeviceAppState == DeviceAppLifecycle.InUse)
                    {
                        //Device resume event
                        SessionState = ClientSessionState.Conversation;
                        //StandbyOnPartialCommitMeal();
                    }

                    #endregion

                    #region  ClientSessionState.UrgesToDecide

                    if (SessionState == ClientSessionState.UrgesToDecide && DeviceAppState == DeviceAppLifecycle.InUse)
                    {
                        //Device resume event
                        SessionState = ClientSessionState.Conversation;
                    }

                    if (SessionState == ClientSessionState.UrgesToDecide &&
                         MainSession != null && MainSession.SessionState != FlavourBusinessFacade.ServicesContextResources.SessionState.UrgesToDecide &&
                         DeviceAppState != DeviceAppLifecycle.InUse)
                    {
                        //Device resume event
                        SessionState = ClientSessionState.ConversationStandy;

                    }
                    #endregion

                    if (SessionState == ClientSessionState.Inactive && DeviceAppState == DeviceAppLifecycle.InUse)
                    {
                        //Device resume event
                        SessionState = ClientSessionState.Conversation;
                        //StandbyOnPartialCommitMeal();
                    }


                    if (SessionState == ClientSessionState.ItemsCommited && !allItemsCommitted)
                    {
                        //Item change event
                        SessionState = ClientSessionState.Conversation;
                    }

                }



            }



        }

        /// <summary>
        /// Checks for long time meal conversation and update the waiters
        /// Meal conversation timeout check, occurs only when there isn't main session
        /// </summary>
        /// <MetaDataID>{49299fee-7bcb-4053-92c1-73eab19a4d1f}</MetaDataID>
        private bool CheckForMealConversationTimeout()
        {
            if (MainSession == null || SessionState == ClientSessionState.ConversationStandy)
            {
                var firstItemPreparation = (from itemPreparation in FlavourItems.OfType<ItemPreparation>()
                                            orderby itemPreparation.StateTimestamp
                                            select itemPreparation).FirstOrDefault();
                if (firstItemPreparation != null &&
                    (SessionState == ClientSessionState.ConversationStandy) &&
                    (DateTime.UtcNow - firstItemPreparation.StateTimestamp.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.MealConversationTimeoutInMin))
                {
                    if (ServicePoint.State == ServicePointState.Conversation)
                        (ServicePoint as ServicePoint).ChangeServicePointState(ServicePointState.ConversationTimeout);

                    if (Caregivers.Where(x => x.CareGiving == Caregiver.CareGivingType.ConversationCheck).Count() > 0)
                    {
                        //When there is care giving the reminding message is sent after extra time interval from care giving time stamp   
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
                    return true;
                }
                return false;
            }
            else
                return SessionState == ClientSessionState.UrgesToDecide;
        }

        /// <summary>
        /// Urges client to decide when session is in this state and
        /// time intervals prerequisites is met
        /// </summary>
        /// <MetaDataID>{5d67c1ab-dc97-4fd4-9685-d9a485f81642}</MetaDataID>
        internal void UrgesToDecideRun()
        {

            if (SessionState == ClientSessionState.UrgesToDecide)
            {
                List<IFoodServiceClientSession> commitedItemsSessions = new List<IFoodServiceClientSession>();

                if (MainSession != null)
                    commitedItemsSessions = MainSession.PartialClientSessions.Where(x => x.SessionState == ClientSessionState.ItemsCommited).ToList();

                if (commitedItemsSessions.Count == 0) // the state of other sessions changes asynchronously 
                    return;

                double fromFirstCommitedItemsSessionInMin = (DateTime.UtcNow - (from session in commitedItemsSessions
                                                                                orderby session.ModificationTime
                                                                                select session).First().ModificationTime.ToUniversalTime()).TotalMinutes;

                double fromLastCommitedItemsSessionInMin = (DateTime.UtcNow - (from session in commitedItemsSessions
                                                                               orderby session.ModificationTime
                                                                               select session).Last().ModificationTime.ToUniversalTime()).TotalMinutes;

                double fromDeviceSleep = 0;
                if (DeviceAppState != DeviceAppLifecycle.InUse)
                    fromDeviceSleep = (DateTime.UtcNow - DeviceAppSleepTime.ToUniversalTime()).TotalMinutes;

                double fromLastModificationMin = (DateTime.UtcNow - ModificationTime.ToUniversalTime()).TotalMinutes;

                bool fromLastCommitedItemsSessionExpired = fromLastCommitedItemsSessionInMin > 3;
                bool fromDeviceSleepExpired = fromDeviceSleep > 1.5;

                TimeSpan mealConversetionTime = DateTime.UtcNow - MainSession.SessionStarts.ToUniversalTime();
                TimeSpan fromStartOfSession = DateTime.UtcNow - SessionStarts.ToUniversalTime();
                TimeSpan fromLastRequest = DateTime.UtcNow - DateTimeOfLastRequest.ToUniversalTime();

                if (fromLastCommitedItemsSessionExpired && fromDeviceSleepExpired)
                {

                    if (!PreviousYouMustDecideMessageTime.HasValue || (DateTime.UtcNow - PreviousYouMustDecideMessageTime.Value).TotalMinutes > 2)
                    {

                        var clientMessage = Messages.Where(x => ((ClientMessages)(int)x.Data["ClientMessageType"]) == ClientMessages.YouMustDecide).FirstOrDefault();


                        if (clientMessage == null)
                        {
                            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                            {

                                clientMessage = new Message();
                                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(clientMessage);
                                clientMessage.Data["ClientMessageType"] = ClientMessages.YouMustDecide;
                                clientMessage.Data["ClientSessionID"] = SessionID;
                                clientMessage.Notification = new Notification() { Title = "Don't wait the waiter", Body = "You must press send button to send order" };
                                PushMessage(clientMessage);
                                if (!string.IsNullOrWhiteSpace(DeviceFirebaseToken))
                                    YouMustDecideMessagesNumber += 1;
                                stateTransition.Consistent = true;
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(DeviceFirebaseToken))
                                YouMustDecideMessagesNumber += 1;
                        }
                        if (!string.IsNullOrWhiteSpace(DeviceFirebaseToken))
                            CloudNotificationManager.SendMessage(clientMessage, DeviceFirebaseToken);
                        _MessageReceived?.Invoke(this);
                        PreviousYouMustDecideMessageTime = System.DateTime.UtcNow;

                    }
                }
            }

        }

        /// <MetaDataID>{e0560b3d-7c5c-47d4-84de-2deb81334af0}</MetaDataID>
        internal void Merge(ItemPreparation item)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                (item.ClientSession as FoodServiceClientSession).RemoveItemForMerge(item);
                AddItemForMerge(item);

                if (_MainSession.Value == null)
                    AutoMealParticipation();
                stateTransition.Consistent = true;
            }

        }
        /// <summary>
        /// In case where the prerequisites fulfilled, assign partial session to a meal session 
        /// </summary>
        /// <MetaDataID>{e320ae17-5fee-481e-a6b6-4dec45d09e90}</MetaDataID>
        private void AutoMealParticipation()
        {
            if (_MainSession.Value == null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    FoodServiceSession foodServiceSession = null;
                    if (this.FlavourItems.All(x => x.State.IsIntheSameOrFollowingState(ItemPreparationState.Committed)))
                    {
                        //if (IsWaiterSession)
                        //{
                        //    foodServiceSession = (ServicePoint as ServicePoint).OpenSessions.OrderBy(x => x.SessionStarts).LastOrDefault();

                        //    if (foodServiceSession == null)
                        //        foodServiceSession = ServicePoint.NewFoodServiceSession() as FoodServiceSession;
                        //    foodServiceSession.AddPartialSession(this);
                        //    if (this.Menu != null)
                        //        foodServiceSession.MenuStorageIdentity = this.Menu.StorageIdentity;

                        //    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodServiceSession);
                        //}
                        (ServicesContextRunTime.Current.MealsController as MealsController).AutoMealParticipation(this);
                    }

                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{fa89d751-ac9e-44c4-81e1-2a7664a59419}</MetaDataID>
        internal void Merge(FoodServiceClientSession partialSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                partialSession.SessionState = ClientSessionState.Closed;
                foreach (var itemPreparation in partialSession.FlavourItems.OfType<ItemPreparation>())
                {
                    partialSession.RemoveItemForMerge(itemPreparation);
                    AddItemForMerge(itemPreparation);

                }

                foreach (var itemPreparation in partialSession.SharedItems.OfType<ItemPreparation>())
                {
                    partialSession.RemoveSharedItemForMerge(itemPreparation);
                    AddSharedItemForMerge(itemPreparation);
                }
                if (ModificationTime < partialSession.ModificationTime)
                    ModificationTime = partialSession.ModificationTime;

                if (SessionEnds < partialSession.SessionEnds)
                    SessionEnds = partialSession.SessionEnds;

                if (DateTimeOfLastRequest < partialSession.DateTimeOfLastRequest)
                    DateTimeOfLastRequest = partialSession.DateTimeOfLastRequest;


                if (WillTakeCareTimestamp < partialSession.WillTakeCareTimestamp)
                    WillTakeCareTimestamp = partialSession.WillTakeCareTimestamp;


                if (UrgesToDecideToWaiterTimeStamp < partialSession.UrgesToDecideToWaiterTimeStamp)
                    UrgesToDecideToWaiterTimeStamp = partialSession.UrgesToDecideToWaiterTimeStamp;



                foreach (var message in partialSession.Messages)
                {
                    partialSession.RemoveMessage(message.MessageID);
                    _Messages.Add(message);
                }


                if (_MainSession.Value == null)
                    AutoMealParticipation();


                stateTransition.Consistent = true;
            }


        }



        /// <exclude>Excluded</exclude>
        string _SessionID;

        /// <MetaDataID>{f264a5cd-5525-4b5e-9e47-d4af35fee08d}</MetaDataID>
        [PersistentMember(nameof(_SessionID))]
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+11")]
        public string SessionID => _SessionID;





        /// <MetaDataID>{b9282b8c-95a4-4d24-a711-dcc4abf87df6}</MetaDataID>
        public Message PeekMessage()
        {



            var message = Messages.OrderBy(x => x.MessageTimestamp).FirstOrDefault();
            if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.YouMustDecide)
                PreviousYouMustDecideMessageTime = System.DateTime.UtcNow;
            return message;
        }

        /// <MetaDataID>{dcd4f574-ae07-4e76-9543-6f9f8ea38ef6}</MetaDataID>
        public Message PopMessage()
        {
            var message = Messages.OrderBy(x => x.MessageTimestamp).FirstOrDefault();
            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Messages.Remove(message);
                    stateTransition.Consistent = true;
                }
            }
            return message;
        }

        /// <MetaDataID>{4ca6e903-31fb-498c-8764-70ac73994758}</MetaDataID>
        public Message GetMessage(string messageId)
        {

            var message = Messages.Where(x => x.MessageID == messageId).FirstOrDefault();
            if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.YouMustDecide)
                PreviousYouMustDecideMessageTime = System.DateTime.UtcNow;

            return message;
        }

        /// <MetaDataID>{1daf8848-0e41-4631-9a44-d59f2338df29}</MetaDataID>
        public void RemoveMessage(string messageId)
        {
            var message = Messages.Where(x => x.MessageID == messageId).FirstOrDefault();
            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Messages.Remove(message);
                    stateTransition.Consistent = true;
                }
            }
        }




        /// <MetaDataID>{be22a961-fcc2-4687-8ffe-2f88939cbfbb}</MetaDataID>
        public void PushMessage(Message message)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                message.MessageTimestamp = DateTime.UtcNow;
                _Messages.Add(message);

                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Message> _Messages;

        /// <MetaDataID>{174fc4ef-b90f-4a30-892d-7cc9c28f8fe0}.Mesages</MetaDataID>
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [PersistentMember(nameof(_Messages))]
        [BackwardCompatibilityID("+9")]
        public IList<Message> Messages => _Messages.ToThreadSafeList();

        /// <MetaDataID>{43da29bd-ea42-4020-9278-9e27db289e48}</MetaDataID>
        public FoodServiceClientSession()
        {
            _SessionID = Guid.NewGuid().ToString("N");
        }

        /// <MetaDataID>{981f30bb-1479-4652-8ec2-ce0ca2b7ef76}</MetaDataID>
        public FoodServiceClientSession(OrganizationStorageRef menu = null)
        {

            _Menu = menu;
            _SessionID = Guid.NewGuid().ToString("N");
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _UserIdentity;

        /// <MetaDataID>{0daa969e-0db4-4687-94e3-814449b6a897}</MetaDataID>
        [PersistentMember("_UserIdentity")]
        [BackwardCompatibilityID("+1")]
        internal string UserIdentity
        {
            get => _UserIdentity;
            set
            {
                if (_UserIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{88294e65-17b3-470c-a8d5-dcfde3d6389b}</MetaDataID>
        internal void RaiseMainSessionChange()
        {


            ObjectChangeState?.Invoke(this, nameof(MainSession));
        }



        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{dc32b13e-155b-4df0-8997-c1f4b6aa5a62}</MetaDataID>
        IFoodServiceClient _Client;

        /// <MetaDataID>{a8221475-4f6d-4d7a-8b0b-7993fb0fc651}</MetaDataID>
        public FlavourBusinessFacade.EndUsers.IFoodServiceClient Client
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(UserIdentity) && _Client == null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(FlavoursServicesContext.OpenFlavourBusinessesStorage());

                    if (UserIdentity?.IndexOf("org_client_") == 0)
                    {
                        var organizationObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage(ServicesContextRunTime.Current.OrganizationStorageIdentity);
                        storage = new OOAdvantech.Linq.Storage(organizationObjectStorage);
                    }
                    _Client = (from client in storage.GetObjectCollection<IFoodServiceClient>()
                               where client.Identity == UserIdentity
                               select client).FirstOrDefault();
                }
                return _Client;
            }

            set
            {
                if (_Client != value)
                {
                    _Client = value;
                    if (_Client != null)
                        UserIdentity = _Client.Identity;
                    else
                        UserIdentity = null;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IFoodServiceSession> _MainSession = new OOAdvantech.Member<IFoodServiceSession>();

        /// <MetaDataID>{02db7f63-0f70-476f-b1cc-d72700e10181}</MetaDataID>
        [PersistentMember(nameof(_MainSession))]
        [CachingDataOnClientSide]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public IFoodServiceSession MainSession
        {
            get
            {
                if (_MainSession.Value == null)
                {

                }

                return _MainSession.Value;
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IServicePoint> _ServicePoint = new OOAdvantech.Member<IServicePoint>();
        /// <MetaDataID>{b8a58af5-a44e-4c7e-986a-f55fa05cff3c}</MetaDataID>
        [PersistentMember(nameof(_ServicePoint))]
        [BackwardCompatibilityID("+3")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IServicePoint ServicePoint
        {
            get
            {
                return _ServicePoint.Value;
            }
            set
            {

                if (_ServicePoint != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicePoint.Value = value;

                        Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                        {
                            ObjectChangeState?.Invoke(this, nameof(ServicePoint));
                        };
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <exclude>Excluded</exclude>
        string _ClientName;
        /// <MetaDataID>{b93c2b0a-5306-4030-b345-87e57d3bb287}</MetaDataID>
        [PersistentMember(nameof(_ClientName))]
        [BackwardCompatibilityID("+5")]
        public string ClientName
        {
            get
            {
                return _ClientName;
            }

            set
            {


                if (_ClientName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ClientName = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
        }

        /// <exclude>Excluded</exclude>
        DateTime _SessionStarts;
        /// <MetaDataID>{8219cdda-f8c2-410c-a7f7-3442cae92163}</MetaDataID>
        [PersistentMember(nameof(_SessionStarts))]
        [BackwardCompatibilityID("+4")]
        public System.DateTime SessionStarts
        {
            get
            {
                return _SessionStarts;
            }
            set
            {
                if (_SessionStarts.ToUniversalTime() != value.ToUniversalTime())
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionStarts = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime _SessionEnds = DateTime.MaxValue;

        /// <MetaDataID>{f36dec2e-ac6f-4a58-bb3b-86ea77dc1efe}</MetaDataID>
        [PersistentMember(nameof(_SessionEnds))]
        [BackwardCompatibilityID("+6")]
        public System.DateTime SessionEnds
        {
            get
            {
                return _SessionEnds;
            }

            set
            {

                if (_SessionEnds != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionEnds = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        event MessageReceivedHandle _MessageReceived;
        public event MessageReceivedHandle MessageReceived
        {
            add
            {

                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");

                _MessageReceived += value;

                //with _MessageReceived system knows indirectly when there is active connection with client device
                //when communication session with client device closed the server session part drops all event consumers

                DeviceResume();
            }
            remove
            {
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");

                _MessageReceived -= value;
            }
        }

        event ItemStateChangedHandle _ItemStateChanged;
        public event ItemStateChangedHandle ItemStateChanged
        {
            add
            {
                _ItemStateChanged += value;

                var datetime = DateTime.Now;
                string timestamp = DateTime.Now.ToLongTimeString() + ":" + datetime.Millisecond.ToString();
                System.Diagnostics.Debug.WriteLine(string.Format("RestApp channel ItemStateChanged event add {0} ", timestamp));

            }
            remove
            {
                _ItemStateChanged -= value;
            }
        }


        public event ItemsStateChangedHandle ItemsStateChanged;


        /// <exclude>Excluded</exclude>
        DateTime _DateTimeOfLastRequest;

        /// <MetaDataID>{4110bc57-e4dd-427c-a304-44fdfdb62663}</MetaDataID>
        [PersistentMember(nameof(_DateTimeOfLastRequest))]
        [BackwardCompatibilityID("+7")]
        public System.DateTime DateTimeOfLastRequest
        {
            get
            {
                return _DateTimeOfLastRequest;
            }

            set
            {

                if (_DateTimeOfLastRequest != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DateTimeOfLastRequest = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }



        /// <exclude>Excluded</exclude>
        OrganizationStorageRef _Menu;
        /// <MetaDataID>{de05c789-52e9-4334-a959-f0b5556cb01d}</MetaDataID>
        public OrganizationStorageRef Menu
        {
            get
            {
                if (_Menu == null)
                {
                    OrganizationStorageRef graphicMenu = null;
                    if (ServicesContextRunTime.GraphicMenus.Count == 1)
                        graphicMenu = ServicesContextRunTime.GraphicMenus.FirstOrDefault();
                    else
                    {
                        //var Portrait = null;
                        //var Landscape = null;

                        if (ClientDeviceType == DeviceType.Phone)
                            graphicMenu = ServicesContextRunTime.GraphicMenus.Where(x => RestaurantMenu.IsPortrait(x)).FirstOrDefault();

                        if (ClientDeviceType == DeviceType.Desktop)
                            graphicMenu = ServicesContextRunTime.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (ClientDeviceType == DeviceType.Tablet)
                            graphicMenu = ServicesContextRunTime.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (ClientDeviceType == DeviceType.TV)
                            graphicMenu = ServicesContextRunTime.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (graphicMenu == null)
                            graphicMenu = ServicesContextRunTime.GraphicMenus.FirstOrDefault();


                    }


                    string versionSuffix = "";
                    if (!string.IsNullOrWhiteSpace(graphicMenu.Version))
                        versionSuffix = "/" + graphicMenu.Version;
                    else
                        versionSuffix = "";
                    graphicMenu.StorageUrl = RawStorageCloudBlob.RootUri + string.Format("/usersfolder/{0}/Menus/{1}{3}/{2}.json", ServicesContextRunTime.OrganizationIdentity, graphicMenu.StorageIdentity, graphicMenu.Name, versionSuffix);
                    _Menu = graphicMenu;
                }
                return _Menu;
            }

        }
        ///// <MetaDataID>{7b52ff92-c72e-4af4-b442-b411796f73dd}</MetaDataID>
        //private bool IsLandscape(OrganizationStorageRef graphicMenu)
        //{

        //    string menuPageHeightAsString = null;
        //    string menuPageWidthAsString = null;
        //    if (graphicMenu.PropertiesValues.TryGetValue("MenuPageHeight", out menuPageHeightAsString)&&graphicMenu.PropertiesValues.TryGetValue("MenuPageWidth", out menuPageWidthAsString))
        //    {
        //        double height = 0;
        //        double.TryParse(menuPageHeightAsString, NumberStyles.Float, CultureInfo.GetCultureInfo(1033), out height);
        //        double width = 0;
        //        double.TryParse(menuPageWidthAsString, NumberStyles.Float, CultureInfo.GetCultureInfo(1033), out width);
        //        return width>height;
        //    }

        //    return false;
        //}
        ///// <MetaDataID>{c40451a6-e3ae-4e91-8999-bda8d857ab9a}</MetaDataID>
        //private bool IsPortrait(OrganizationStorageRef graphicMenu)
        //{
        //    string menuPageHeightAsString = null;
        //    string menuPageWidthAsString = null;
        //    if (graphicMenu.PropertiesValues.TryGetValue("MenuPageHeight", out menuPageHeightAsString)&&graphicMenu.PropertiesValues.TryGetValue("MenuPageWidth", out menuPageWidthAsString))
        //    {
        //        double height = 0;
        //        double.TryParse(menuPageHeightAsString, NumberStyles.Float, CultureInfo.GetCultureInfo(1033), out height);
        //        double width = 0;
        //        double.TryParse(menuPageWidthAsString, NumberStyles.Float, CultureInfo.GetCultureInfo(1033), out width);
        //        return height>width;
        //    }

        //    return false;
        //}

        /// <MetaDataID>{a31148d7-08cf-459f-bc06-f355081edb15}</MetaDataID>
        internal MenuPresentationModel.MenuCanvas.IRestaurantMenu GraphicMenu
        {
            get
            {
                string typedJsonurl = Menu.StorageUrl.Replace(".json", "_t.json");
                return ServicesContextRunTime.Current.GetGraphicMenuVersion(typedJsonurl);
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _FlavourItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{f32258af-4e61-4d3b-b952-58e19df96770}</MetaDataID>
        [PersistentMember(nameof(_FlavourItems))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.CascadeDelete | PersistencyFlag.ReferentialIntegrity)]
        [BackwardCompatibilityID("+13")]
        [CachingDataOnClientSide]
        public System.Collections.Generic.IList<FlavourBusinessFacade.RoomService.IItemPreparation> FlavourItems
        {
            get
            {
                return _FlavourItems.ToThreadSafeList();
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _SharedItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{ec6bbded-86a2-4248-8726-bc11385af338}</MetaDataID>
        [PersistentMember(nameof(_SharedItems))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.CascadeDelete | PersistencyFlag.ReferentialIntegrity)]
        [BackwardCompatibilityID("+14")]
        [CachingDataOnClientSide]
        [IgnoreErrorCheck]
        public IList<IItemPreparation> SharedItems => _SharedItems.ToThreadSafeList();

        /// <MetaDataID>{c2cb0b6a-35b3-4191-b60c-92b793e1229e}</MetaDataID>
        public ServicesContextRunTime ServicesContextRunTime { get; internal set; }


        /// <exclude>Excluded</exclude>;


        /// <MetaDataID>{efecab6b-0f79-4b59-8137-e18550333365}</MetaDataID>

        [BackwardCompatibilityID("+21")]
        public FlavourBusinessFacade.HumanResources.IServicesContextWorker Worker
        {
            get
            {
                return SessionCreator?.Worker;
            }
        }

        /// <exclude>Excluded</exclude>
        ClientSessionState _SessionState;

        /// <MetaDataID>{d3a7b762-52a5-4926-8f7a-4e6389581d2f}</MetaDataID>
        [PersistentMember(nameof(_SessionState))]
        [BackwardCompatibilityID("+22")]
        public ClientSessionState SessionState
        {
            get => _SessionState;
            internal set
            {
                if (_SessionState != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionState = value;
                        stateTransition.Consistent = true;
                    }
                    if (_SessionState == ClientSessionState.Closed && Transaction.Current != null)
                    {
                        Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                        {

                            if (transaction.Status == TransactionStatus.Aborted)
                                StartDeviceConnectionStatusCheck();

                        };
                    }
                    ObjectChangeState?.Invoke(this, nameof(SessionState));
                }
            }
        }




        /// <MetaDataID>{c90103fc-2f5a-452e-bc5e-0a5c452d20c4}</MetaDataID>
        public IList<IFoodServiceClientSession> GetPeopleNearMe()
        {
            var peopole = ServicePoint.GetServicePointOtherPeople(this);

            foreach (var clientSession in GetMealParticipants())
                peopole.Remove(clientSession);

            return peopole;
        }
        /// <MetaDataID>{ea91d597-b0a0-40fe-a413-b1307b56ff8d}</MetaDataID>
        public IList<IFoodServiceClientSession> GetServicePointParticipants()
        {
            var peopole = ServicePoint.GetServicePointOtherPeople(this);
            peopole.Insert(0, this);
            return peopole;
        }

        /// <summary>
        /// This method returns the service point meal participants
        /// </summary>
        /// <returns></returns>
        /// <MetaDataID>{091f9ca8-3349-4530-b0a8-33624ca619a1}</MetaDataID>
        public IList<IFoodServiceClientSession> GetMealParticipants()
        {
            if (MainSession != null)
            {
                if (IsWaiterSession) //For waiter returns all implicit meal participants and meal invitation participants
                    return MainSession.PartialClientSessions.Where(x => x != this).ToList();
                else
                    return MainSession.PartialClientSessions.Where(x => x != this && !x.ImplicitMealParticipation).ToList();// All participant which participate with meal invitation

            }
            else
                return new List<IFoodServiceClientSession>();
        }

        /// <MetaDataID>{5b49a97e-4dc4-4610-9044-f06061993866}</MetaDataID>
        public void MealInvitation(IFoodServiceClientSession messmateClientSesion)
        {

            var clientMessage = (from message in Messages
                                 where message.Data.ContainsKey("ClientMessageType") && (ClientMessages)Enum.ToObject(typeof(ClientMessages), message.Data["ClientMessageType"]) == ClientMessages.PartOfMealRequest
                                 && message.Data.ContainsKey("ClientSessionID") && (message.Data["ClientSessionID"] as string) == (messmateClientSesion as FoodServiceClientSession).SessionID
                                 select message).FirstOrDefault();

            if (clientMessage == null)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    clientMessage = new Message();
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(clientMessage);
                    clientMessage.Data["ClientMessageType"] = ClientMessages.PartOfMealRequest;
                    clientMessage.Data["ClientSessionID"] = (messmateClientSesion as FoodServiceClientSession).SessionID;
                    clientMessage.Notification = new Notification() { Title = "Make me part of meal" };

                    PushMessage(clientMessage);

                    stateTransition.Consistent = true;
                }

            }

            _MessageReceived?.Invoke(this);
            if (!string.IsNullOrWhiteSpace(DeviceFirebaseToken))
                CloudNotificationManager.SendMessage(clientMessage, DeviceFirebaseToken);

            //message.
        }

        /// <MetaDataID>{a11434ec-c2da-4d7e-8474-7d9b4633f71c}</MetaDataID>
        public void CancelMealInvitation(IFoodServiceClientSession messmateClientSesion)
        {
            var clientMessage = (from message in Messages
                                 where message.Data.ContainsKey("ClientMessageType") && (ClientMessages)Enum.ToObject(typeof(ClientMessages), message.Data["ClientMessageType"]) == ClientMessages.PartOfMealRequest
                                 && message.Data.ContainsKey("ClientSessionID") && (message.Data["ClientSessionID"] as string) == (messmateClientSesion as FoodServiceClientSession).SessionID
                                 select message).FirstOrDefault();
            if (clientMessage != null)
                RemoveMessage(clientMessage.MessageID);

        }






        /// <MetaDataID>{e17bb585-505f-49f7-a5eb-e6ee9acf9d2e}</MetaDataID>
        public void MealInvitationDenied(IFoodServiceClientSession messmateClientSesion)
        {

            ObjectChangeState?.Invoke(this, nameof(MainSession));
        }


        /// <MetaDataID>{580d9ff4-318b-4051-89d0-abf3e9c9a774}</MetaDataID>
        public void MenuItemProposal(IFoodServiceClientSession messmateClientSesion, string menuItemUri)
        {

            var clientMessage = (from message in Messages
                                 where message.Data.ContainsKey("ClientMessageType") && (ClientMessages)Enum.ToObject(typeof(ClientMessages), message.Data["ClientMessageType"]) == ClientMessages.PartOfMealRequest
                                 && message.Data.ContainsKey("ClientSessionID") && (message.Data["ClientSessionID"] as string) == (messmateClientSesion as FoodServiceClientSession).SessionID
                                 select message).FirstOrDefault();

            if (clientMessage == null)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    clientMessage = new Message();
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(clientMessage);
                    clientMessage.Data["ClientMessageType"] = ClientMessages.MenuItemProposal;
                    clientMessage.Data["ClientSessionID"] = (messmateClientSesion as FoodServiceClientSession).SessionID;
                    clientMessage.Data["MenuItemUri"] = menuItemUri;
                    clientMessage.Notification = new Notification() { Title = "Proposal" };

                    PushMessage(clientMessage);

                    stateTransition.Consistent = true;
                }

            }

            _MessageReceived?.Invoke(this);
            if (!string.IsNullOrWhiteSpace(DeviceFirebaseToken))
                CloudNotificationManager.SendMessage(clientMessage, DeviceFirebaseToken);

        }

        /// <MetaDataID>{783d18d2-0918-46bc-ba2e-42b6ef0a0ef7}</MetaDataID>

        void IObjectStateEventsConsumer.OnCommitObjectState()
        {
        }

        /// <MetaDataID>{7c6021ec-9897-4ba8-834b-78a4000f0e21}</MetaDataID>
        void IObjectStateEventsConsumer.OnActivate()
        {
            var ssd = OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(this)?.ObjectID;
            if (SessionState != ClientSessionState.Closed)
                StartDeviceConnectionStatusCheck();
            var servicPoint = this.ServicePoint?.Description;
            var forgotten = Forgotten;

            lock (CaregiversLock)
            {
                if (!string.IsNullOrWhiteSpace(WillTakeCareWorkersJson))
                    _Caregivers = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Caregiver>>(WillTakeCareWorkersJson);
            }
        }

        /// <MetaDataID>{35842787-d9d9-428e-b02b-4c1c668082b3}</MetaDataID>
        void IObjectStateEventsConsumer.OnDeleting()
        {
        }
        //Personal access tokens github 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
        /// <MetaDataID>{9e4705b6-b564-4c69-bf7a-aa4a668ee624}</MetaDataID>
        void IObjectStateEventsConsumer.LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
            //MainSession
            if (associationEnd.Association.Identity.ToString().ToLower() == "93808acd-1c78-45da-8c44-dd7666ae0128".ToLower())
            {
                if (OOAdvantech.Transactions.Transaction.Current != null)
                {
                    OOAdvantech.Transactions.Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(MainSession));
                    };
                }
                else
                {
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(MainSession));
                    });
                }
            }
        }

        /// <MetaDataID>{1840dd0b-75f1-4914-aa6e-75a825900083}</MetaDataID>
        void IObjectStateEventsConsumer.LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
        }

        /// <MetaDataID>{5308ec61-d82b-47fc-bd9c-df622e3a86a6}</MetaDataID>
        void IObjectStateEventsConsumer.BeforeCommitObjectState()
        {
            lock (CaregiversLock)
            {
                WillTakeCareWorkersJson = OOAdvantech.Json.JsonConvert.SerializeObject(_Caregivers);
            }
        }



        /// <MetaDataID>{30273f52-641c-45ec-b2ac-150c6fbad211}</MetaDataID>
        private void RemoveItemForMerge(ItemPreparation flavourItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FlavourItems.Remove(flavourItem);
                flavourItem.SessionID = null;
                Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    ObjectChangeState?.Invoke(this, nameof(FlavourItems));
                };

                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{c8e68341-2f4e-4105-a4c2-8263f98c422f}</MetaDataID>
        private void AddItemForMerge(ItemPreparation flavourItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FlavourItems.Add(flavourItem);
                flavourItem.SessionID = this.SessionID;

                Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    ObjectChangeState?.Invoke(this, nameof(FlavourItems));
                };

                stateTransition.Consistent = true;
            }
        }


        /// <MetaDataID>{c52be7e9-d08f-43fa-9cba-a4215dd74f35}</MetaDataID>
        private void RemoveSharedItemForMerge(ItemPreparation flavourItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _SharedItems.Remove(flavourItem);

                Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    ObjectChangeState?.Invoke(this, nameof(FlavourItems));
                };
                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{f6cba177-607f-4994-b008-4fe4c013f249}</MetaDataID>
        private void AddSharedItemForMerge(ItemPreparation flavourItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _SharedItems.Add(flavourItem);
                Transaction.Current.TransactionCompleted += (Transaction transaction) =>
                {
                    ObjectChangeState?.Invoke(this, nameof(FlavourItems));
                };
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{6f13d1e2-be82-48fc-a0d6-c7404bdb448f}</MetaDataID>
        public void RemoveItem(IItemPreparation item)
        {
            var flavourItem = (from storedItem in _FlavourItems.OfType<RoomService.ItemPreparation>()
                               where storedItem.uid == (item as RoomService.ItemPreparation).uid
                               select storedItem).FirstOrDefault();

            if (flavourItem != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FlavourItems.Remove(flavourItem);
                    flavourItem.SessionID = null;
                    ModificationTime = DateTime.UtcNow;

                    if (flavourItem.SharedInSessions.Count == 0)
                    {
                        if (MainSession != null && MainSession.Meal != null)
                        {
                            var flavourItemMealCourse = (from mealCourse in MainSession.Meal.Courses
                                                         from mealCourseItem in mealCourse.FoodItems
                                                         where mealCourseItem.uid == flavourItem.uid
                                                         select mealCourse).FirstOrDefault();

                            flavourItemMealCourse?.RemoveItem(flavourItem);
                            //if(flavourItemMealCourse.FoodItems.Count==0)

                        }
                        if (flavourItem.PreparationStation != null)
                            (flavourItem.PreparationStation as PreparationStation).RemoveItemPreparation(flavourItem);
                    }

                    stateTransition.Consistent = true;
                }
                CatchStateEvents();

                if (MainSession != null && flavourItem.SharedInSessions.Count != 0)
                {
                    (MainSession as FoodServiceSession).ReassignSharedItem(flavourItem);
                }
                else
                {
                    if (/*flavourItem.IsShared &&*/ MainSession != null)
                    {

                        foreach (var clientSession in MainSession.PartialClientSessions.Where(x => x != this))
                            clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);
                    }
                    foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                        clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);

                    flavourItem.State = ItemPreparationState.Canceled;
                    //foreach (var preparationStation in ServicesContextRunTime.PreparationStationRuntimes.Values.OfType<PreparationStationRuntime>())
                    //    preparationStation.OnPreparationItemChangeState(flavourItem);
                }
                ObjectChangeState?.Invoke(this, nameof(FlavourItems));

            }

            flavourItem = (from storedItem in _SharedItems.OfType<RoomService.ItemPreparation>()
                           where storedItem.uid == (item as RoomService.ItemPreparation).uid
                           select storedItem).FirstOrDefault();
            if (flavourItem != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _SharedItems.Remove(flavourItem);
                    stateTransition.Consistent = true;
                }
                if (flavourItem.IsShared && MainSession != null)
                {
                    foreach (var clientSession in MainSession.PartialClientSessions.Where(x => x != this))
                        clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);
                }
                foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                    clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);

            }

        }


        /// <MetaDataID>{18d66316-cbc0-4673-8b25-5e0af87b32a0}</MetaDataID>
        public void ItemChanged(IItemPreparation item)
        {
            try
            {

                ItemPreparation existingItem = this._FlavourItems.OfType<ItemPreparation>().Union(this._SharedItems.OfType<ItemPreparation>()).Where(x => x.uid == item.uid).FirstOrDefault();

                //if ((item as RoomService.ItemPreparation).SessionID == this.SessionID)
                //    existingItem = (from flavourItem in _FlavourItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();
                //else
                //    existingItem = (from flavourItem in _SharedItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();

                if (existingItem == null)
                    return;
                var itemSharingChanged = existingItem.IsShared != item.IsShared;
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    if (existingItem.Update(item as RoomService.ItemPreparation))
                    {

                        if (!string.IsNullOrWhiteSpace(this.UserLanguageCode))
                            existingItem.EnsurePresentationFor(CultureInfo.GetCultureInfo(this.UserLanguageCode));


                        (existingItem as ItemPreparation).StateTimestamp = DateTime.UtcNow;
                        if (_FlavourItems.Where(x => x.State.IsInPreviousState(ItemPreparationState.Committed)).Count() > 0)
                            CatchStateEvents();
                    }


                    ModificationTime = DateTime.UtcNow;
                    stateTransition.Consistent = true;
                }


                if (MainSession != null)
                {
                    foreach (var clientSession in MainSession.PartialClientSessions.Where(x => x != this))
                        clientSession.RaiseItemStateChanged(item.uid, existingItem.SessionID, SessionID, existingItem.IsShared, existingItem.SharedInSessions);
                }

                foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && x != this && (MainSession != x.MainSession || MainSession == null)))
                    clientSession.RaiseItemStateChanged(item.uid, existingItem.SessionID, SessionID, existingItem.IsShared, existingItem.SharedInSessions);


            }
            catch (AbortException error)
            {
                if (error.AbortReasons.Count > 0)
                    throw error.AbortReasons[0];

                throw;
            }

        }
        /// <MetaDataID>{fe6d766e-7a01-4f68-9163-fcab7bdbfc21}</MetaDataID>
        public void CancelLastPreparationStep(List<IItemPreparation> flavourItems)
        {
            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x => GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                {
                    if (flavourItem.State == ItemPreparationState.InPreparation)
                        flavourItem.State = ItemPreparationState.PendingPreparation;
                    else if (flavourItem.State == ItemPreparationState.IsPrepared && !flavourItem.IsCooked)
                        flavourItem.State = ItemPreparationState.InPreparation;
                    else if (flavourItem.State == ItemPreparationState.IsPrepared && flavourItem.IsCooked)
                        flavourItem.State = ItemPreparationState.IsRoasting;
                    else if (flavourItem.State == ItemPreparationState.IsRoasting)
                        flavourItem.State = ItemPreparationState.InPreparation;
                    else if (flavourItem.State == ItemPreparationState.Serving)
                        flavourItem.State = ItemPreparationState.IsPrepared;

                }

                stateTransition.Consistent = true;
            }

            foreach (var clientSession in MainSession.PartialClientSessions)
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

            foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));


            foreach (var mealCourse in MainSession.Meal.Courses.OfType<MealCourse>())
                mealCourse.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));


        }

        /// <MetaDataID>{6b965aed-f7e7-42c3-b155-51b8013f2cca}</MetaDataID>
        public bool Forgotten
        {
            get
            {
                bool forgotten = ServicePoint != null && ServicePoint.ServicePointType == ServicePointType.HallServicePoint && FlavourItems.All(x => x.State == ItemPreparationState.New) &&
                    (DateTime.UtcNow - SessionStarts.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.ForgottenSessionLifeTimeSpanInMin) &&
                    (DateTime.UtcNow - ModificationTime.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.ForgottenSessionDeviceSleepTimeSpanInMin) &&
                    (DateTime.UtcNow - DeviceAppSleepTime.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.ForgottenSessionDeviceSleepTimeSpanInMin);

                return forgotten;
            }
        }

        /// <MetaDataID>{a585165a-d688-49f1-8813-2bf0870e2247}</MetaDataID>
        public bool LongTimeForgotten
        {
            get
            {
                bool forgotten = ServicePoint != null && ServicePoint.ServicePointType == ServicePointType.HallServicePoint && FlavourItems.All(x => x.State == ItemPreparationState.New) &&
                    (DateTime.UtcNow - SessionStarts.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.ForgottenSessionLifeTimeSpanInMin * 3) &&
                    (DateTime.UtcNow - ModificationTime.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.ForgottenSessionDeviceSleepTimeSpanInMin * 3) &&
                    (DateTime.UtcNow - DeviceAppSleepTime.ToUniversalTime()) > TimeSpan.FromMinutes(ServicesContextRunTime.Current.Settings.ForgottenSessionDeviceSleepTimeSpanInMin * 3);

                //return forgotten;
                return false;
            }
        }

        /// <MetaDataID>{aa278977-7c30-42d9-a365-31cdf6dd8fbb}</MetaDataID>
        public DateTime UrgesToDecideToWaiterTimeStamp { get; private set; }

        /// <MetaDataID>{5cf89fdd-c567-4d62-8037-c06f3a83cf97}</MetaDataID>
        public bool MealConversationTimeout
        {
            get
            {
                return CheckForMealConversationTimeout();
            }
        }
        /// <exclude>Excluded</exclude>
        SessionType _SessionType;
        /// <MetaDataID>{62f96036-7ea5-484d-8463-c4b4ac8c7a04}</MetaDataID>
        [PersistentMember(nameof(_SessionType))]
        [BackwardCompatibilityID("+27")]
        [CachingDataOnClientSide]
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
        /// <summary></summary>
        /// <MetaDataID>{63419fc0-9ea8-4327-8c32-b3cdfc89d33b}</MetaDataID>
        public string MealInvitationUrl
        {
            get
            {

                return string.Format("http://{0}:4300/#/launch-app?mealInvitation={1}&sc={2}&sp={3}&cs={4}", FlavourBusinessFacade.ComputingResources.EndPoint.Server, true, ServicePoint.ServicesContextIdentity, ServicePoint.ServicesPointIdentity, SessionID);

                //return "";
            }
        }


        /// <MetaDataID>{a0e181ef-e2e4-462c-b28b-227ad1ebbf3f}</MetaDataID>
        public string MealInvitationUri
        {
            get
            {

                return string.Format("MealInvitation;{0};{1};{2}", ServicePoint.ServicesContextIdentity, ServicePoint.ServicesPointIdentity, SessionID);
            }
        }

        /// <exclude>Excluded</exclude>
        string _DeliveryComment;


        /// <MetaDataID>{93888b74-6b9f-4250-80d1-0be33073184a}</MetaDataID>
        [PersistentMember(nameof(_DeliveryComment))]
        [BackwardCompatibilityID("+28")]
        [CachingDataOnClientSide]
        public string DeliveryComment
        {
            get => _DeliveryComment;
            set
            {
                if (_DeliveryComment != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeliveryComment = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <MetaDataID>{407f40f2-a77e-4c4e-b445-3a5e1f155639}</MetaDataID>
        public void ItemsInPreparation(List<IItemPreparation> flavourItems)
        {
            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x => GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                {
                    //flavourItem.MealCourse.StartsAt = DateTime.UtcNow;
                    flavourItem.State = ItemPreparationState.InPreparation;
                }

                stateTransition.Consistent = true;
            }

            foreach (var clientSession in MainSession.PartialClientSessions)
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

            foreach (var mealCourse in MainSession.Meal.Courses.OfType<MealCourse>())
                mealCourse.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));


        }

        /// <MetaDataID>{93d66d3c-7ca4-4a3b-88a1-3aec137f028f}</MetaDataID>
        public void ItemsPrepared(List<IItemPreparation> flavourItems)
        {
            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x => GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                {
                    flavourItem.State = ItemPreparationState.IsPrepared;
                    flavourItem.StateTimestamp = DateTime.UtcNow;
                }

                stateTransition.Consistent = true;
            }

            foreach (var clientSession in MainSession.PartialClientSessions)
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

            foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));


            foreach (var mealCourse in MainSession.Meal.Courses.OfType<MealCourse>())
                mealCourse.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

        }

        /// <MetaDataID>{34f48801-e58a-4f3b-9308-5b3b690be5f4}</MetaDataID>
        public void ItemsRoasting(List<IItemPreparation> flavourItems)
        {
            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x => GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                {
                    flavourItem.State = ItemPreparationState.IsRoasting;
                    flavourItem.StateTimestamp = DateTime.UtcNow;

                }

                stateTransition.Consistent = true;
            }

            foreach (var clientSession in MainSession.PartialClientSessions)
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

            foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));



            foreach (var mealCourse in MainSession.Meal.Courses.OfType<MealCourse>())
                mealCourse.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

        }

        /// <MetaDataID>{cd81e265-e605-4a8b-8582-590fe342cb5f}</MetaDataID>
        public void ItemsServing(List<IItemPreparation> flavourItems)
        {

            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x => GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                {
                    flavourItem.State = ItemPreparationState.Serving;
                    flavourItem.StateTimestamp = DateTime.UtcNow;
                }

                stateTransition.Consistent = true;
            }

            Transaction.RunOnTransactionCompleted(() =>
             {
                 foreach (var clientSession in MainSession.PartialClientSessions)
                     clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));


                 foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                     clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

                 foreach (var mealCourse in MainSession.Meal.Courses.OfType<MealCourse>())
                     mealCourse.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));
             });

        }



        /// <MetaDataID>{1bd18663-e4f7-45a1-9233-6edf3a462ed1}</MetaDataID>
        private ItemPreparation GetSessionItem(IItemPreparation item)
        {
            ItemPreparation existingItem = this._FlavourItems.OfType<ItemPreparation>().Union(this._SharedItems.OfType<ItemPreparation>()).Where(x => x.uid == item.uid).FirstOrDefault();

            //ItemPreparation existingItem;
            //if ((item as RoomService.ItemPreparation).SessionID == this.SessionID)
            //    existingItem = (from flavourItem in _FlavourItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();
            //else
            //    existingItem = (from flavourItem in _SharedItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();
            return existingItem;
        }

        /// <MetaDataID>{6f3aecf2-dbfb-4493-a33a-88a5f34578f4}</MetaDataID>
        public void AddItem(IItemPreparation item)
        {
            try
            {
                var flavourItem = (from storedItem in _FlavourItems.OfType<RoomService.ItemPreparation>()
                                   where storedItem.uid == item.uid
                                   select storedItem).FirstOrDefault();



                if (flavourItem == null)
                {
                    var sharingFlavourItem = (from storedItem in _SharedItems.OfType<RoomService.ItemPreparation>()
                                              where storedItem.uid == item.uid
                                              select storedItem).FirstOrDefault();


                    flavourItem = item as RoomService.ItemPreparation;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (sharingFlavourItem != null)
                        {
                            //if(flavourItem.SharedInSessions.Count==1)
                            //    sharingFlavourItem.IsShared = false;
                            _SharedItems.Remove(sharingFlavourItem);
                            item = sharingFlavourItem;
                        }
                        ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(item);
                        _FlavourItems.Add(item);
                        (item as ItemPreparation).StateTimestamp = DateTime.UtcNow;
                        ModificationTime = DateTime.UtcNow;
                        MealCourse.AssignMealCourseToItem(flavourItem);
                        if (!string.IsNullOrWhiteSpace(this.UserLanguageCode))
                            flavourItem.EnsurePresentationFor(CultureInfo.GetCultureInfo(this.UserLanguageCode));

                        if (item.State == ItemPreparationState.New && SessionType == SessionType.HomeDelivery || SessionType == SessionType.HomeDeliveryGuest || SessionType == SessionType.Takeaway)
                            item.State = ItemPreparationState.AwaitingPaymentToCommit;


                        stateTransition.Consistent = true;
                    }

                    CatchStateEvents();

                    //foreach (var preparationStation in ServicesContextRunTime.PreparationStationRuntimes.Values.OfType<PreparationStationRuntime>())
                    //    preparationStation.OnPreparationItemChangeState(flavourItem);



                    if (/*flavourItem.IsShared &&*/ MainSession != null)
                    {
                        foreach (var clientSession in MainSession.PartialClientSessions.Where(x => x != this))
                            clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);
                    }
                    foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && x != this && (MainSession != x.MainSession || MainSession == null)))
                        clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);

                    if (sharingFlavourItem != null)
                    {
                        RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);
                        //ObjectChangeState?.Invoke(this, nameof(FlavourItems));
                    }
                    ObjectChangeState?.Invoke(this, nameof(FlavourItems));

                    if (MainSession == null && (ServicePoint as ServicePoint).OpenSessions.Count > 0)
                        (ServicesContextRunTime.Current.MealsController as MealsController).AutoMealParticipation(this);

                    if (!string.IsNullOrWhiteSpace(this.UserLanguageCode))
                        flavourItem.EnsurePresentationFor(CultureInfo.GetCultureInfo(this.UserLanguageCode));

                }
            }
            catch (Exception error)
            {

                throw;
            }


        }



        /// <MetaDataID>{2a63b9bd-1109-4e07-ba37-850f108f8f82}</MetaDataID>
        public IItemPreparation GetItem(string itemUid)
        {
            var flavourItem = this.FlavourItems.ToList().Find(x => x.uid == itemUid);
            if (flavourItem == null && MainSession != null)
                flavourItem = (from clientSession in MainSession.PartialClientSessions
                               where clientSession != this
                               from item in clientSession.FlavourItems
                               where item.uid == itemUid
                               select item).FirstOrDefault();

            return flavourItem;
        }

        /// <MetaDataID>{b050cbd5-497a-48cc-bc73-436e31e3a80e}</MetaDataID>
        public void RemoveSharingItem(IItemPreparation item)
        {
            RoomService.ItemPreparation flavourItem = null;
            if (MainSession != null)
            {
                flavourItem = (from clientSession in MainSession.PartialClientSessions
                               where clientSession != this
                               from storedItem in clientSession.FlavourItems
                               where storedItem.uid == item.uid
                               select storedItem).OfType<RoomService.ItemPreparation>().FirstOrDefault();
            }
            if (flavourItem != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _SharedItems.Remove(flavourItem);
                    ModificationTime = System.DateTime.UtcNow;
                    stateTransition.Consistent = true;
                }

                if (MainSession != null)
                {
                    foreach (var clientSession in MainSession.PartialClientSessions.Where(x => x != this))
                        clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);
                }
                foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                    clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);

            }

        }

        /// <MetaDataID>{2698628e-3a4a-4675-b9a6-60ba115c5422}</MetaDataID>
        public void AddSharingItem(IItemPreparation item)
        {


            var flavourItem = (from clientSession in MainSession.PartialClientSessions
                               where clientSession != this
                               from storedItem in clientSession.FlavourItems
                               where storedItem.uid == item.uid
                               select storedItem).OfType<RoomService.ItemPreparation>().FirstOrDefault();

            if (flavourItem != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _SharedItems.Add(flavourItem);
                    ModificationTime = DateTime.UtcNow;
                    stateTransition.Consistent = true;
                }

                if (MainSession != null)
                {
                    foreach (var clientSession in MainSession.PartialClientSessions.Where(x => x != this))
                        clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);
                }
                foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                    clientSession.RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);

                if (!string.IsNullOrWhiteSpace(this.UserLanguageCode))
                    flavourItem.EnsurePresentationFor(CultureInfo.GetCultureInfo(this.UserLanguageCode));
            }

            //using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //{
            //    _SharedItems.Add(item); 
            //    stateTransition.Consistent = true;
            //}

        }

        /// <MetaDataID>{4f70c893-6aff-4eab-93f6-45eeec273615}</MetaDataID>
        public void RaiseItemStateChanged(string uid, string itemOwningSession, string itemChangeSession, bool isShared, List<string> shareInSessions)
        {

            if (itemOwningSession == this.SessionID && (from flavourItem in _FlavourItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == uid select flavourItem).FirstOrDefault() != null)
            {
                var sharedItem = (from flavourItem in _FlavourItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == uid select flavourItem).FirstOrDefault();

                var clientMessage = Messages.Where(x => ((ClientMessages)(int)x.Data["ClientMessageType"]) == ClientMessages.ShareItemHasChange).FirstOrDefault();
                {
                    if (clientMessage == null)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            PreviousYouMustDecideMessageTime = System.DateTime.UtcNow;
                            clientMessage = new Message();
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(clientMessage);
                            clientMessage.Data["ClientMessageType"] = ClientMessages.ShareItemHasChange;
                            clientMessage.Data["ClientSessionID"] = SessionID;
                            clientMessage.Data["SharedItemUid"] = uid;
                            clientMessage.Data["ItemOwningSession"] = itemOwningSession;
                            clientMessage.Data["itemChangeSession"] = itemChangeSession;
                            clientMessage.Data["IsShared"] = isShared;
                            clientMessage.Data["ShareInSessions"] = shareInSessions;

                            clientMessage.Notification = new Notification() { Title = "Don't wait the waiter", Body = string.Format("The shared item '{0}' has been changed", sharedItem.Name) };
                            PushMessage(clientMessage);
                            stateTransition.Consistent = true;
                        }

                        if (!string.IsNullOrWhiteSpace(DeviceFirebaseToken))
                            CloudNotificationManager.SendMessage(clientMessage, DeviceFirebaseToken);
                    }
                    else
                    {

                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(clientMessage))
                        {
                            clientMessage.Data["SharedItemUid"] = uid;
                            clientMessage.Data["ItemOwningSession"] = itemOwningSession;
                            clientMessage.Data["itemChangeSession"] = itemChangeSession;
                            clientMessage.Data["IsShared"] = isShared;
                            clientMessage.Data["ShareInSessions"] = shareInSessions;
                            stateTransition.Consistent = true;
                        }
                    }
                    _MessageReceived?.Invoke(this);

                }
            }
            else if (itemOwningSession != this.SessionID && (from sharedItem in _SharedItems.OfType<RoomService.ItemPreparation>() where sharedItem.uid == uid select sharedItem).FirstOrDefault() != null)
            {
                var sharedItem = (from theSharedItem in _SharedItems.OfType<RoomService.ItemPreparation>() where theSharedItem.uid == uid select theSharedItem).FirstOrDefault();

                var clientMessage = Messages.Where(x => ((ClientMessages)(int)x.Data["ClientMessageType"]) == ClientMessages.ShareItemHasChange).FirstOrDefault();
                {
                    if (clientMessage == null)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            PreviousYouMustDecideMessageTime = System.DateTime.UtcNow;
                            clientMessage = new Message();
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(clientMessage);
                            clientMessage.Data["ClientMessageType"] = ClientMessages.ShareItemHasChange;
                            clientMessage.Data["ClientSessionID"] = SessionID;
                            clientMessage.Data["SharedItemUid"] = uid;
                            clientMessage.Data["ItemOwningSession"] = itemOwningSession;
                            clientMessage.Data["itemChangeSession"] = itemChangeSession;
                            clientMessage.Data["IsShared"] = isShared;
                            clientMessage.Data["ShareInSessions"] = shareInSessions;

                            clientMessage.Notification = new Notification() { Title = "Don't wait the waiter", Body = string.Format("The shared item '{0}' has been changed", sharedItem.Name) };

                            //clientMessage.Notification = new Notification() { Title = "Don't wait the waiter", Body = "ItemStateChanged" };
                            PushMessage(clientMessage);
                            stateTransition.Consistent = true;
                        }

                        if (!string.IsNullOrWhiteSpace(DeviceFirebaseToken))
                            CloudNotificationManager.SendMessage(clientMessage, DeviceFirebaseToken);
                    }
                    else
                    {

                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(clientMessage))
                        {
                            clientMessage.Data["SharedItemUid"] = uid;
                            clientMessage.Data["ItemOwningSession"] = itemOwningSession;
                            clientMessage.Data["itemChangeSession"] = itemChangeSession;
                            clientMessage.Data["IsShared"] = isShared;
                            clientMessage.Data["ShareInSessions"] = shareInSessions;
                            stateTransition.Consistent = true;
                        }
                    }

                    _MessageReceived?.Invoke(this);

                }
            }
            else
                _ItemStateChanged?.Invoke(uid, itemOwningSession, isShared, shareInSessions);
        }
        /// <MetaDataID>{3a21e1ed-277a-499b-850f-6a21073db0f8}</MetaDataID>
        public void RaiseItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {
            ItemsStateChanged?.Invoke(newItemsState);

        }

        ///// <MetaDataID>{fc8a4f16-49d7-43d7-af13-fe9049a196a7}</MetaDataID>
        //public Dictionary<string, ItemPreparationState> Prepare(List<IItemPreparation> itemPreparations)
        //{


        //    var itemsNewState = new Dictionary<string, ItemPreparationState>();

        //    var itemsIDS = itemPreparations.Select(x => x.uid).ToList();
        //    var flavourItems = (from storedItem in _FlavourItems.OfType<RoomService.ItemPreparation>()
        //                        where itemsIDS.Contains(storedItem.uid)
        //                        select storedItem).ToList();

        //    List<RoomService.ItemPreparation> changeStateFlavourItems = new List<RoomService.ItemPreparation>();



        //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //    {
        //        foreach (var item in flavourItems)
        //        {
        //            if (item.State == ItemPreparationState.New || item.State == ItemPreparationState.Committed)
        //            {
        //                item.State = ItemPreparationState.PendingPreparation;
        //                itemsNewState[item.uid] = item.State;
        //                changeStateFlavourItems.Add(item);
        //            }
        //        }
        //        stateTransition.Consistent = true;
        //    }




        //    if (MainSession != null)
        //    {
        //        foreach (var clientSession in MainSession.PartialClientSessions)
        //            clientSession.RaiseItemsStateChanged(changeStateFlavourItems.ToDictionary(x => x.uid, x => x.State));
        //    }

        //    return itemsNewState;
        //}

        /// <MetaDataID>{239c856f-008a-4e7e-9b1c-fdf7899ea5d6}</MetaDataID>
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
                    stateTransition.Consistent = true;
                }
            }

        }




        /// <MetaDataID>{e91f2250-533d-4aab-b78c-bb88fd0c2106}</MetaDataID>
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
        /// <summary>
        /// Creates a pay order with assigned payment gateway 
        /// In case where payment can be completed from refunds amount, the payment completed without pay order.  
        /// </summary>
        /// <param name="payment">
        /// Payment has all data that needed for the creation of pay order   
        /// </param>
        /// <param name="tipAmount">
        /// Defines the extra amount for tipping
        /// </param>
        /// <param name="paramsJson">
        /// Defines some extra parameters which are necessary for payment gateway
        /// </param>
        /// <MetaDataID>{8cbfdfe4-c18e-407c-863f-074e5268a9c8}</MetaDataID>
        public void CreatePaymentOrder(FinanceFacade.IPayment payment, decimal tipAmount, string paramsJson)
        {
            if (payment.State != FinanceFacade.PaymentState.Completed)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    if (!(payment as FinanceFacade.Payment).TryToCompletePaymentWithRefundAmount(tipAmount))
                    {
                        PaymentProviders.VivaWallet.CreatePaymentOrder(payment as FinanceFacade.Payment, tipAmount, paramsJson);
                        (payment as FinanceFacade.Payment).TipsAmount = tipAmount;
                    }

                    stateTransition.Consistent = true;
                }

            }
        }

        /// <MetaDataID>{f50229d0-2dfa-46c4-9173-09e98ec19a6d}</MetaDataID>
        void CreatePaymentToCommitOrder(FinanceFacade.IPayment payment, decimal tipAmount, string paramsJson)
        {
            if (payment.State != FinanceFacade.PaymentState.Completed)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    foreach (var paymentItem in payment.Items)
                    {
                        var flavourItem = _FlavourItems.Where(x => x.uid == paymentItem.uid && x.State == ItemPreparationState.New).FirstOrDefault();
                        if (flavourItem != null)
                            flavourItem.State = ItemPreparationState.AwaitingPaymentToCommit;
                    }

                    if (!(payment as FinanceFacade.Payment).TryToCompletePaymentWithRefundAmount(tipAmount))
                    {
                        PaymentProviders.VivaWallet.CreatePaymentOrder(payment as FinanceFacade.Payment, tipAmount, paramsJson);
                        (payment as FinanceFacade.Payment).TipsAmount = tipAmount;
                    }
                    stateTransition.Consistent = true;
                }
            }

        }

        /// <MetaDataID>{de284aed-075a-4c1b-877f-ee5a70fa3b3a}</MetaDataID>
        public IBill GetBill()
        {

            IBill bill = Bill.GetBillFor(this);

            return bill;

        }

        public IBill GetBill(List<SessionItemPreparationAbbreviation> itemPreparations)
        {
            return Bill.GetBillFor(itemPreparations, this);
        }



        /// <MetaDataID>{391fee95-b5be-4c9a-9118-67949d13b1fd}</MetaDataID>
        public Dictionary<string, ItemPreparationState> FlavourItemsPreparationState
        {
            get
            {
                var itemsState = new Dictionary<string, ItemPreparationState>();
                foreach (var item in FlavourItems)
                    itemsState[item.uid] = item.State;
                return itemsState;
            }
        }
        /// <exclude>Excluded</exclude>
        string _OrderComment;
        /// <MetaDataID>{8ada8ca2-18aa-4c6b-b18a-69825fc8e010}</MetaDataID>
        [PersistentMember(nameof(_OrderComment))]
        [BackwardCompatibilityID("+31")]
        public string OrderComment
        {
            get => _OrderComment; set
            {
                if (_OrderComment != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OrderComment = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }





        /// <MetaDataID>{2c628c7e-9219-4b2e-9c46-ca7610b14b7f}</MetaDataID>
        public Dictionary<string, ItemPreparationState> Commit(List<IItemPreparation> itemPreparations)
        {
            var itemsNewState = new Dictionary<string, ItemPreparationState>();

            var itemsIDS = itemPreparations.Select(x => x.uid).ToList();
            var flavourItems = (from storedItem in _FlavourItems.OfType<RoomService.ItemPreparation>()
                                where itemsIDS.Contains(storedItem.uid)
                                select storedItem).ToList();

            if (itemsIDS.Count != flavourItems.Count)
                throw new Exception("there is an item that does not belong to this session");

            List<RoomService.ItemPreparation> changeStateFlavourItems = new List<RoomService.ItemPreparation>();

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var item in flavourItems)
                {
                    if (item.State.IsInPreviousState(ItemPreparationState.Committed))
                    {
                        item.State = ItemPreparationState.Committed;
                        itemsNewState[item.uid] = item.State;
                        changeStateFlavourItems.Add(item);
                    }
                }

                if (_MainSession.Value == null)
                    AutoMealParticipation();

                AllItemsCommited();
                stateTransition.Consistent = true;
            }

            if (MainSession != null)
            {
                foreach (var clientSession in MainSession.PartialClientSessions)
                    clientSession.RaiseItemsStateChanged(changeStateFlavourItems.ToDictionary(x => x.uid, x => x.State));
            }
            if (ServicePoint is HallServicePoint)
            {
                foreach (var clientSession in (ServicePoint as ServicePoint).OpenClientSessions.Where(x => x.IsWaiterSession && (MainSession != x.MainSession || MainSession == null)))
                    clientSession.RaiseItemsStateChanged(changeStateFlavourItems.ToDictionary(x => x.uid, x => x.State));
            }

            return itemsNewState;
        }

        /// <MetaDataID>{9db073fb-148b-4efe-99c6-2cb6f4e68f42}</MetaDataID>
        private void AllItemsCommited()
        {
            if (SessionState == ClientSessionState.Conversation)
                SessionState = ClientSessionState.ItemsCommited;
        }
        /// <summary></summary>
        /// <param name="deliveryPlace"></param>
        /// <MetaDataID>{ab2d01da-b855-4cd6-b1c1-2fae7fb00906}</MetaDataID>
        public void SetSessionDeliveryPlace(IPlace deliveryPlace)
        {
            if (SessionType == SessionType.HomeDelivery)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    if (_MainSession.Value == null)
                        (ServicesContextRunTime.Current.MealsController as MealsController).AutoMealParticipation(this);

                    if (Client?.IsPlatformClient == false)
                    {

                        bool outOfDeliveryRange = CanChangeDeliveryPlace(deliveryPlace.Location) != ChangeDeliveryPlaceResponse.OK;
                        deliveryPlace.SetExtensionProperty("OutOfDeliveryRange", outOfDeliveryRange.ToString().ToLower());
                        this.MainSession.DeliveryPlace = deliveryPlace;
                    }
                    else
                    {
                        if (CanChangeDeliveryPlace(deliveryPlace.Location) == ChangeDeliveryPlaceResponse.OK)
                        {
                            deliveryPlace.SetExtensionProperty("OutOfDeliveryRange", true.ToString().ToLower());
                            this.MainSession.DeliveryPlace = deliveryPlace;
                        }
                        else
                            throw new Exception("Δεν μπορείτε να αλλάξετε την διεύθυνσης παράδοσης. Η παραγγελία είναι καθ'οδον.");
                    }
                    stateTransition.Consistent = true;
                }
            }
        }
        public IPlace GetSessionDeliveryPlace()
        {
            return (_MainSession.Value as FoodServiceSession)?.DeliveryPlace;
        }

        /// <MetaDataID>{f88bff3e-2884-440d-8d4d-12209daca5e9}</MetaDataID>
        public void SetSessionServiceTime(DateTime? value)
        {
            if (SessionType == SessionType.HomeDelivery)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    if (_MainSession.Value == null)
                        (ServicesContextRunTime.Current.MealsController as MealsController).AutoMealParticipation(this);

                    MainSession.ServiceTime = value;

                    stateTransition.Consistent = true;
                }
            }

        }

        /// <MetaDataID>{e1482ae5-b23a-4f23-a3bd-4ae21f383dc7}</MetaDataID>
        public void UpdateSessionUser(string userLanguageCode)
        {
            AuthUserRef authUserRef = AuthUserRef.GetCallContextAuthUserRef(false);

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                if (Worker == null)
                {
                    var foodServiceClient = authUserRef?.GetContextRoleObject<FoodServiceClient>();
                    if (foodServiceClient != null)
                    {
                        if (Client != null && Client != foodServiceClient)
                            throw new AuthenticationException("User hasn't access right for this action");

                        Client = foodServiceClient;
                    }
                }
                this.UserLanguageCode = userLanguageCode;
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{647d84e3-f212-4bdc-902c-d54bc57eb9cb}</MetaDataID>
        public Dictionary<string, ItemPreparationState> CommitAll()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{94e00e71-9da0-4e0f-bf45-9d421e9b84cf}</MetaDataID>

    }

    /// <MetaDataID>{3ed1fc71-2eb7-4fc5-a41e-656823ffd9a1}</MetaDataID>
    public class Caregiver
    {
        public Caregiver() { }

        /// <MetaDataID>{68f09a1a-3a1b-4ec8-915c-49679b0496de}</MetaDataID>
        [OOAdvantech.Json.JsonIgnore]
        IServicesContextWorker _Worker;

        /// <MetaDataID>{ca467d17-af5d-4e42-b3c1-71cdd8c98921}</MetaDataID>
        [OOAdvantech.Json.JsonProperty]
        string WorkerUri;

        /// <MetaDataID>{369c5b18-c3bd-4c61-bb9d-14be5b87caaa}</MetaDataID>
        [OOAdvantech.Json.JsonIgnore]
        public IServicesContextWorker Worker
        {
            get
            {
                if (_Worker == null)
                    _Worker = ObjectStorage.GetObjectFromUri<IServicesContextWorker>(WorkerUri);
                return _Worker;
            }
            set
            {
                _Worker = value;
                WorkerUri = ObjectStorage.GetStorageOfObject(_Worker).GetPersistentObjectUri(_Worker);
            }
        }
        [OOAdvantech.Json.JsonProperty]
        public System.DateTime? WillTakeCareTimestamp { get; set; }



        /// <MetaDataID>{074975bc-5a6c-4890-a835-6a7c7876504f}</MetaDataID>
        public CareGivingType CareGiving;

        public enum CareGivingType
        {
            Lay,
            ConversationCheck,
            MealConsulting,
            DelayedMealAtTheCounter
        }
    }
    /// <MetaDataID>{b870fcd0-671e-4ef8-abd5-d8bb26cef2fd}</MetaDataID>
    public enum DeviceAppLifecycle
    {
        InUse,
        Sleep,
        Shutdown

    }

}



