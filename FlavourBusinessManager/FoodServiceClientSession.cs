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

namespace FlavourBusinessManager.EndUsers
{

    /// <MetaDataID>{b870fcd0-671e-4ef8-abd5-d8bb26cef2fd}</MetaDataID>
    public enum DeviceAppLifecycle
    {
        InUse,
        Sleep,
        Shutdown

    }



    /// <MetaDataID>{174fc4ef-b90f-4a30-892d-7cc9c28f8fe0}</MetaDataID>
    [BackwardCompatibilityID("{174fc4ef-b90f-4a30-892d-7cc9c28f8fe0}")]
    [Persistent()]
    public class FoodServiceClientSession : MarshalByRefObject, IFoodServiceClientSession, OOAdvantech.Remoting.IExtMarshalByRefObject, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {


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

        /// <MetaDataID>{56afdc1c-d6ee-42f0-9b81-44125cc8a57a}</MetaDataID>
        public void AcceptMealInvitation(string clientSessionToken, IFoodServiceClientSession messmateClientSesion)
        {

            string token = null;
            ServicePointRunTime.ServicesContextRunTime.FoodServiceClientSessionsTokens.TryGetValue(this, out token);
            if (string.IsNullOrWhiteSpace(token) || token != clientSessionToken)
                throw new AuthenticationException("invalid token or token expired");

            MakePartOfMeal(messmateClientSesion);

            (messmateClientSesion as FoodServiceClientSession).MealInvitationAccepted(this);
            ObjectChangeState?.Invoke(this, nameof(MainSession));
        }

        /// <MetaDataID>{bce5f9e3-648f-4e89-8d76-fae5e3b3d0bd}</MetaDataID>
        internal void MakePartOfMeal(IFoodServiceClientSession messmateClientSesion)
        {

            if (messmateClientSesion.ServicePoint != ServicePoint)
                throw new Exception("Invalid part of meal request. messmate " + messmateClientSesion + " is connected to other service point");

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (_MainSession.Value == null)
                {
                    if (messmateClientSesion.MainSession != null)
                    {
                        // _MainSession.Value = messmateClientSesion.MainSession;
                        messmateClientSesion.MainSession.AddPartialSession(this);
                    }
                    else
                    {
                        var foodServiceSession = ServicePoint.NewFoodServiceSession();
                        foodServiceSession.AddPartialSession(this);
                        //_MainSession.Value = ServicePoint.NewFoodServiceSession();
                        ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodServiceSession);
                    }
                    _MainSession.Value.AddPartialSession(messmateClientSesion);
                }
                else
                {
                    if (_MainSession.Value != messmateClientSesion.MainSession)
                    {

                        if (messmateClientSesion.MainSession != null)
                        {
                            var partialClientSessions = messmateClientSesion.MainSession.PartialClientSessions.ToList();
                            IFoodServiceSession foodServiceSession = messmateClientSesion.MainSession;
                            foreach (var clientSession in partialClientSessions)
                            {
                                foodServiceSession.RemovePartialSession(clientSession);
                                _MainSession.Value.AddPartialSession(clientSession);
                            }

                            ObjectStorage.DeleteObject(foodServiceSession);
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

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{8e3f84e8-e9aa-4590-972f-053a864069be}</MetaDataID>
        private void MealInvitationAccepted(FoodServiceClientSession foodServiceClientSession)
        {
            ObjectChangeState?.Invoke(this, nameof(MainSession));
        }

        /// <MetaDataID>{9d7c7058-1582-4dbf-8727-14540094ec56}</MetaDataID>
        internal void MonitorTick()
        {
        }

        ///// <MetaDataID>{56afdc1c-d6ee-42f0-9b81-44125cc8a57a}</MetaDataID>
        //public void AcceptMealInvitation(string sessionID, FlavourBusinessFacade.EndUsers.IFoodServiceClientSession messmateClientSesion)
        //{

        //}

        /// <exclude>Excluded</exclude>
        string _ClientDeviceID;


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
                if (string.IsNullOrEmpty(SessionID))
                {

                    if (_ClientDeviceID != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _ClientDeviceID = value;

                            _SessionID = Guid.NewGuid().ToString("N") + _ClientDeviceID;
                            stateTransition.Consistent = true;
                        }
                    }
                }
                else
                    throw new NotSupportedException("Changing ClientDeviceID is not supported");

            }
        }

        //Previous
        /// <MetaDataID>{202d1d18-93ff-4baa-9888-b0f50544a24e}</MetaDataID>
        internal DateTime? PreviousYouMustDecideMessageTime;


        /// <MetaDataID>{b448d9b4-451c-4d2c-a064-71a9b59a635a}</MetaDataID>
        int DeviceConnectionStatusChecksNumber;
        /// <MetaDataID>{b80540b1-2b19-4fbd-887f-58039a11d111}</MetaDataID>
        void StartDeviceConnectionStatusCheck()
        {
            Task.Run(() =>
            {
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
            });
        }

        object StateMachineLock = new object();
        private void CatchStateEvents()
        {
            lock (StateMachineLock)
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    var flavourItems = FlavourItems;
                    bool allItemsCommitted = flavourItems.Count != 0 && flavourItems.Where(x => x.State == ItemPreparationState.New).Count() == 0;

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

                    if (SessionState == ClientSessionState.Conversation && MainSession != null &&
                                    MainSession.SessionState == FlavourBusinessFacade.ServicesContextResources.SessionState.UrgesToDecide &&
                                    DeviceAppState != DeviceAppLifecycle.InUse)
                    {
                        //Standby on partial commit meal event
                        SessionState = ClientSessionState.UrgesToDecide;
                        //StandbyOnPartialCommitMeal();
                    }
                    #endregion


                    #region  ClientSessionState.ConversationStandy
                    if (SessionState == ClientSessionState.ConversationStandy && MainSession != null &&
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


        /// <MetaDataID>{2a6c5a7e-109b-4483-8e38-87ef7843ad5a}</MetaDataID>
        internal void YouMustDecide(List<IFoodServiceClientSession> commitedItemsSessions)
        {





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

        /// <exclude>Excluded</exclude>
        string _SessionID;

        /// <MetaDataID>{f264a5cd-5525-4b5e-9e47-d4af35fee08d}</MetaDataID>
        [PersistentMember(nameof(_SessionID))]
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



        /// <MetaDataID>{981f30bb-1479-4652-8ec2-ce0ca2b7ef76}</MetaDataID>
        public FoodServiceClientSession()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{0daa969e-0db4-4687-94e3-814449b6a897}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember()]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string ClientIdentity;


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{dc32b13e-155b-4df0-8997-c1f4b6aa5a62}</MetaDataID>
        IFoodServiceClient _Client;

        /// <MetaDataID>{a8221475-4f6d-4d7a-8b0b-7993fb0fc651}</MetaDataID>
        public FlavourBusinessFacade.EndUsers.IFoodServiceClient Client
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ClientIdentity) && _Client == null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(FlavoursServicesContext.OpenFlavourBusinessesStorage());
                    _Client = (from client in storage.GetObjectCollection<IFoodServiceClient>()
                               where client.Identity == ClientIdentity
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
                        ClientIdentity = _Client.Identity;
                    else
                        ClientIdentity = null;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IFoodServiceSession> _MainSession = new OOAdvantech.Member<IFoodServiceSession>();

        /// <MetaDataID>{02db7f63-0f70-476f-b1cc-d72700e10181}</MetaDataID>
        [PersistentMember(nameof(_MainSession))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public IFoodServiceSession MainSession
        {
            get
            {
                return _MainSession.Value;
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IServicePoint> _ServicePoint = new OOAdvantech.Member<IServicePoint>();
        /// <MetaDataID>{b8a58af5-a44e-4c7e-986a-f55fa05cff3c}</MetaDataID>
        [PersistentMember(nameof(_ServicePoint))]
        [BackwardCompatibilityID("+3")]
        public FlavourBusinessFacade.ServicesContextResources.IServicePoint ServicePoint
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

        /// <MetaDataID>{de05c789-52e9-4334-a959-f0b5556cb01d}</MetaDataID>
        public OrganizationStorageRef Menu { get; set; }


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
                foreach (var item in _FlavourItems)
                {
                    if (item.ClientSession == null)
                    {

                    }

                }
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
        OOAdvantech.Member<FlavourBusinessFacade.HumanResources.IWaiter> _Waiter = new OOAdvantech.Member<IWaiter>();

        /// <MetaDataID>{efecab6b-0f79-4b59-8137-e18550333365}</MetaDataID>
        [PersistentMember(nameof(_Waiter))]
        [BackwardCompatibilityID("+21")]
        public FlavourBusinessFacade.HumanResources.IWaiter Waiter
        {
            get
            {
                return _Waiter.Value;
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
            set
            {
                if (_SessionState != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionState = value;
                        stateTransition.Consistent = true;
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

        /// <MetaDataID>{091f9ca8-3349-4530-b0a8-33624ca619a1}</MetaDataID>
        public IList<IFoodServiceClientSession> GetMealParticipants()
        {
            if (MainSession != null)
                return MainSession.PartialClientSessions.Where(x => x != this).ToList();
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
            if (SessionState != ClientSessionState.Closed)
                StartDeviceConnectionStatusCheck();
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
                    
                    if (flavourItem.PreparationStation != null)
                        (flavourItem.PreparationStation as PreparationStation).RemoveItemPreparation(flavourItem);
                    _FlavourItems.Remove(flavourItem);
                    flavourItem.SessionID = null;
                    ModificationTime = DateTime.UtcNow;
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
            }

        }


        /// <MetaDataID>{18d66316-cbc0-4673-8b25-5e0af87b32a0}</MetaDataID>
        public void ItemChanged(IItemPreparation item)
        {
            try
            {
                ItemPreparation existingItem;
                if ((item as RoomService.ItemPreparation).SessionID == this.SessionID)
                    existingItem = (from flavourItem in _FlavourItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();
                else
                    existingItem = (from flavourItem in _SharedItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();

                if (existingItem == null)
                    return;
                var itemSharingChanged = existingItem.IsShared != item.IsShared;
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    if (existingItem.Update(item as RoomService.ItemPreparation))
                    {
                        (existingItem as ItemPreparation).StateTimestamp = DateTime.UtcNow;
                        if (_FlavourItems.Where(x => x.State == ItemPreparationState.New).Count() > 0)
                            CatchStateEvents();

                        //foreach (var preparationStation in ServicesContextRunTime.PreparationStationRuntimes.Values.OfType<PreparationStationRuntime>())
                        //    preparationStation.OnPreparationItemChangeState(existingItem);
                    }
                    ModificationTime = DateTime.UtcNow;
                    stateTransition.Consistent = true;
                }


                if (MainSession != null)
                {
                    if (MainSession != null)
                    {
                        foreach (var clientSession in MainSession.PartialClientSessions.Where(x => x != this))
                            clientSession.RaiseItemStateChanged(item.uid, existingItem.SessionID, SessionID, existingItem.IsShared, existingItem.SharedInSessions);
                    }
                }
            }
            catch (AbortException error)
            {
                if (error.AbortReasons.Count > 0)
                    throw error.AbortReasons[0];

                throw;
            }

        }
        public void CancelLastPreparationStep(List<IItemPreparation> flavourItems)
        {
            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x => GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                {
                    if(flavourItem.State == ItemPreparationState.…nPreparation)
                        flavourItem.State = ItemPreparationState.PendingPreparation;

                    if (flavourItem.State == ItemPreparationState.Prepared)
                        flavourItem.State = ItemPreparationState.…nPreparation;

                }

                stateTransition.Consistent = true;
            }

            foreach (var clientSession in MainSession.PartialClientSessions)
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

        }

        

        public void Items…nPreparation(List<IItemPreparation> flavourItems)
        {
            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x=>GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                    flavourItem.State = ItemPreparationState.…nPreparation;

                stateTransition.Consistent = true;
            }

            foreach (var clientSession in MainSession.PartialClientSessions)
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

        }

        public void ItemsPrepared(List<IItemPreparation> flavourItems)
        {
            CatchStateEvents();
            var clientSessionItems = flavourItems.Select(x => GetSessionItem(x));


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var flavourItem in clientSessionItems)
                    flavourItem.State = ItemPreparationState.Prepared;

                stateTransition.Consistent = true;
            }

            foreach (var clientSession in MainSession.PartialClientSessions)
                clientSession.RaiseItemsStateChanged(clientSessionItems.ToDictionary(x => x.uid, x => x.State));

        }

        private ItemPreparation GetSessionItem(IItemPreparation item)
        {
            ItemPreparation existingItem;
            if ((item as RoomService.ItemPreparation).SessionID == this.SessionID)
                existingItem = (from flavourItem in _FlavourItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();
            else
                existingItem = (from flavourItem in _SharedItems.OfType<RoomService.ItemPreparation>() where flavourItem.uid == item.uid select flavourItem).FirstOrDefault();
            return existingItem;
        }

        /// <MetaDataID>{6f3aecf2-dbfb-4493-a33a-88a5f34578f4}</MetaDataID>
        public void AddItem(IItemPreparation item)
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
                if (sharingFlavourItem != null)
                {
                    RaiseItemStateChanged(flavourItem.uid, flavourItem.SessionID, SessionID, flavourItem.IsShared, flavourItem.SharedInSessions);
                    //ObjectChangeState?.Invoke(this, nameof(FlavourItems));
                }
                ObjectChangeState?.Invoke(this, nameof(FlavourItems));
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
            var flavourItem = (from clientSession in MainSession.PartialClientSessions
                               where clientSession != this
                               from storedItem in clientSession.FlavourItems
                               where storedItem.uid == item.uid
                               select storedItem).OfType<RoomService.ItemPreparation>().FirstOrDefault();
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
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                DeviceAppActivationTime = DateTime.UtcNow;
                DeviceAppState = DeviceAppLifecycle.InUse;
                stateTransition.Consistent = true;
            }
        }




        /// <MetaDataID>{e91f2250-533d-4aab-b78c-bb88fd0c2106}</MetaDataID>
        public void DeviceSleep()
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                DeviceAppSleepTime = DateTime.UtcNow;
                DeviceAppState = DeviceAppLifecycle.Sleep;
                stateTransition.Consistent = true;
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

            List<RoomService.ItemPreparation> changeStateFlavourItems = new List<RoomService.ItemPreparation>();

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var item in flavourItems)
                {
                    if (item.State == ItemPreparationState.New)
                    {
                        item.State = ItemPreparationState.Committed;
                        itemsNewState[item.uid] = item.State;
                        changeStateFlavourItems.Add(item);
                    }
                }

                if (_MainSession.Value == null)
                {
                    var foodServiceSession = ServicePoint.NewFoodServiceSession();
                    foodServiceSession.AddPartialSession(this);
                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodServiceSession);
                }

                AllItemsCommited();

                stateTransition.Consistent = true;
            }

            if (MainSession != null)
            {
                foreach (var clientSession in MainSession.PartialClientSessions)
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

       

        /// <MetaDataID>{94e00e71-9da0-4e0f-bf45-9d421e9b84cf}</MetaDataID>

    }


}
