using System;
using FlavourBusinessFacade;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Linq;
using System.Linq;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using System.Collections.Generic;
using OOAdvantech;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.EndUsers;
using System.Xml.Linq;
using FlavourBusinessManager.ServicesContextResources;
using Microsoft.Azure.Storage;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessFacade.HumanResources;
using System.Threading.Tasks;
using FlavourBusinessToolKit;
using System.IO;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService;
using FinanceFacade;
using MenuModel;

namespace FlavourBusinessManager.ServicePointRunTime
{
    /// <MetaDataID>{f44b4b10-677e-461a-bbc7-bb8b1b62716a}</MetaDataID>
    [BackwardCompatibilityID("{f44b4b10-677e-461a-bbc7-bb8b1b62716a}")]
    [Persistent()]
    public class ServicesContextRunTime : MarshalByRefObject, IExtMarshalByRefObject, IFlavoursServicesContextRuntime, IUploadService
    {
        /// <MetaDataID>{1d4e96ea-00e3-43e2-a710-ed93ee808246}</MetaDataID>
        public bool RemoveSupervisor(IServiceContextSupervisor supervisor)
        {
            try
            {
                //lock (ServiceContextRTLock)
                //{
                //    supervisor.ObjectChangeState -=Supervisor_ObjectChangeState;
                //    if (_Supervisors != null && _Supervisors.Contains(supervisor))
                //        _Supervisors.Remove(supervisor);
                //}
                (supervisor as HumanResources.ServiceContextSupervisor).Suspended = true;
                return false;
            }
            catch (Exception error)
            {
                throw;
            }
        }


        /// <MetaDataID>{410b0278-7f9b-4ec0-86f6-c9725db0750a}</MetaDataID>
        public void MakeSupervisorActive(IServiceContextSupervisor supervisor)
        {
            try
            {
                (supervisor as HumanResources.ServiceContextSupervisor).Suspended = false;
            }
            catch (Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{e76644ba-d6de-49e5-9639-3debe6905449}</MetaDataID>
        public void RemoveWaiter(FlavourBusinessFacade.HumanResources.IWaiter waiter)
        {
            try
            {
                lock (ServiceContextRTLock)
                {
                    waiter.ObjectChangeState -= Waiter_ObjectChangeState;
                    if (_Waiters != null && _Waiters.Contains(waiter))
                        _Waiters.Remove(waiter);
                }

                ObjectStorage.DeleteObject(waiter);
            }
            catch (Exception error)
            {
                throw;
            }

        }



        /// <exclude>Excluded</exclude>
        List<IServiceContextSupervisor> _Supervisors;

        /// <MetaDataID>{349c7df1-c3ca-4cc1-a858-b6f80735cc4a}</MetaDataID>
        object SupervisorsLock = new object();

        /// <MetaDataID>{685b5e38-8960-486a-9b22-0bc7773bf4c2}</MetaDataID>
        public List<IServiceContextSupervisor> Supervisors
        {
            get
            {
                lock (SupervisorsLock)
                {
                    if (_Supervisors == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
                        var servicesContextIdentity = ServicesContextIdentity;
                        _Supervisors = (from supervisor in servicesContextStorage.GetObjectCollection<IServiceContextSupervisor>()
                                        where supervisor.ServicesContextIdentity == servicesContextIdentity
                                        select supervisor).ToList();

                        foreach (var supervisor in _Supervisors)
                            supervisor.ObjectChangeState += Supervisor_ObjectChangeState;

                    }
                    return _Supervisors;
                }
            }
        }

        /// <MetaDataID>{878f062d-16a9-4852-8ab0-343a213640c4}</MetaDataID>
        private void Supervisor_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IWaiter.ActiveShiftWork))
            {
                Task.Run(() =>
                {
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                });
            }
        }


        /// <exclude>Excluded</exclude>
        List<IWaiter> _Waiters;

        /// <MetaDataID>{66836f78-9f8b-4cb1-9641-21a7380fd2dc}</MetaDataID>
        public IList<IWaiter> Waiters
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_Waiters == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        _Waiters = (from waiter in servicesContextStorage.GetObjectCollection<IWaiter>()
                                    where waiter.ServicesContextIdentity == servicesContextIdentity
                                    select waiter).ToList();

                        foreach (var waiter in _Waiters)
                            waiter.ObjectChangeState += Waiter_ObjectChangeState;
                    }

                    return _Waiters.ToList();
                }

            }
        }

        /// <MetaDataID>{e9759669-6bd8-4091-b318-90229918434e}</MetaDataID>
        private void Waiter_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IWaiter.ActiveShiftWork))
            {
                Task.Run(() =>
                {
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                });
            }
        }

        /// <MetaDataID>{ee50a4b3-0173-4323-87b6-36f53b6a2459}</MetaDataID>
        public string NewWaiter()
        {


            var objectStorage = ObjectStorage.GetStorageOfObject(this);


            //OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
            var servicesContextIdentity = ServicesContextIdentity;


            lock (SupervisorsLock)
            {
                var unassignedWaiter = (from waiter in Waiters
                                        select waiter).ToList().Where((x => string.IsNullOrWhiteSpace(x.SignUpUserIdentity))).FirstOrDefault();
                string WaiterAssignKey = "";

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    if (unassignedWaiter == null)
                    {
                        unassignedWaiter = new HumanResources.Waiter();
                        unassignedWaiter.ServicesContextIdentity = servicesContextIdentity;
                        unassignedWaiter.Name = Properties.Resources.DefaultWaiterName;

                        objectStorage.CommitTransientObjectState(unassignedWaiter);
                        _Waiters.Add(unassignedWaiter);
                    }
                    WaiterAssignKey = servicesContextIdentity + ";" + unassignedWaiter.Identity + ";" + Guid.NewGuid().ToString("N");

                    unassignedWaiter.WaiterAssignKey = WaiterAssignKey;
                    stateTransition.Consistent = true;
                }

                return WaiterAssignKey;
            }

            //var objectStorage = ObjectStorage.GetStorageOfObject(this);

            //HumanResources.Waiter waiter = new HumanResources.Waiter();
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{

            //    waiter.Name = Properties.Resources.DefaultWaiterName;
            //    waiter.ServicesContextIdentity = this.ServicesContextIdentity;
            //    objectStorage.CommitTransientObjectState(waiter);

            //    stateTransition.Consistent = true;
            //}
            //return waiter;

        }

        /// <MetaDataID>{9b816e2c-6456-47a3-b95b-9ee396b80b1b}</MetaDataID>
        public System.Timers.Timer SessionsMonitoringTimer = new System.Timers.Timer(5000);

        /// <MetaDataID>{f655395d-23e6-4145-b73d-9b3e7e578fdb}</MetaDataID>
        object ServiceContextRTLock = new object();


        /// <MetaDataID>{d64ddfb9-8fee-4703-a467-6961a6568c86}</MetaDataID>
        public ServicesContextRunTime()
        {

            _Current = this;

            try
            {
                FlavoursServicesContextManagment.FlavoursServicesEventLog.WriteEntry("ServicesContextRunTime ctor:" + DateTime.Now.ToLongTimeString());
            }
            catch (Exception error)
            {
            }

        }

        /// <exclude>Excluded</exclude>
        static ServicesContextRunTime _Current;
        /// <MetaDataID>{925c577b-d2f2-457e-99ed-c29b27e24eda}</MetaDataID>
        public static ServicesContextRunTime Current
        {
            get
            {
                if (_Current != null)
                    return _Current;
                else
                {


                    var flavoursServicesContext = FlavoursServicesContext.ActiveFlavoursServicesContexts.Where(x => x.RunAtContext.ContextID == ComputationalResources.IsolatedComputingContext.CurrentContextID).FirstOrDefault();

                    if (flavoursServicesContext == null)
                        return null;
                    flavoursServicesContext.GetRunTime();

                    return _Current;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        MenuModel.Menu _OperativeRestaurantMenu;

        /// <MetaDataID>{54caa9a3-5612-44a1-a91d-91e7294825ba}</MetaDataID>
        public MenuModel.Menu OperativeRestaurantMenu
        {
            get
            {
                return _OperativeRestaurantMenu;
            }
        }

        /// <MetaDataID>{269a82d7-69e7-4e6c-9832-c50e7a1fa8b2}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {
            var fbstorage = Storages.Where(x => x.FlavourStorageType == OrganizationStorages.OperativeRestaurantMenu).FirstOrDefault();

            if (fbstorage != null)
            {
                var storageRef = new FlavourBusinessFacade.OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = RestaurantMenuDataUri, TimeStamp = RestaurantMenuDataLastModified };
                FlavourBusinessToolKit.RawStorageData rawStorageData = new FlavourBusinessToolKit.RawStorageData(storageRef, null);
                OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));

                _OperativeRestaurantMenu = (from menu in restMenusData.GetObjectCollection<MenuModel.Menu>()
                                            select menu).FirstOrDefault();
            }
            bool del = false;

            if (del)
                DeleteSimulationData();

            SessionsMonitoringTimer.Start();
            SessionsMonitoringTimer.Elapsed += SessionsMonitoringTimer_Elapsed;
            SessionsMonitoringTimer.AutoReset = false;

            Task.Run(() =>
            {
                (MealsController as MealsController).Init();
                //Load CashierStations
                var cashierStations = CashierStations;

            });

            //Task.Run(() =>
            //{
            //    var trt = GetPreparationStationRuntime("7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad").GetPreparationItems(new List<ItemPreparationAbbreviation>(), null);

            //});

            //Task.Run(() =>
            //{
            //    var trt = GetPreparationStationRuntime("7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad").GetPreparationItems(new List<ItemPreparationAbbreviation>(), null);

            //});

        }


        /// <exclude>Excluded</exclude>
        object ObjLock = new object();
        /// <exclude>Excluded</exclude>
        List<EndUsers.FoodServiceClientSession> _OpenClientSessions;
        /// <MetaDataID>{b6321911-81bf-401b-93fe-a0b4277bc301}</MetaDataID>
        public List<EndUsers.FoodServiceClientSession> OpenClientSessions
        {
            get
            {


                lock (ObjLock)
                {
                    if (_OpenClientSessions == null)
                    {
                        DateTime timeStamp = DateTime.UtcNow;



                        var mainSessions = (from session in new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this)).GetObjectCollection<FoodServiceSession>()
                                            select session).ToList();


                        _OpenClientSessions = (from session in new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this)).GetObjectCollection<EndUsers.FoodServiceClientSession>()
                                               select session).ToList();

                        foreach (var clientSession in _OpenClientSessions)
                            clientSession.ServicesContextRunTime = this;

                        TimeSpan timeSpan = (DateTime.UtcNow - timeStamp);


                    }
                    CollectGarbageClientSessions();
                    return _OpenClientSessions;
                }
            }

        }

        /// <summary>
        /// Removes all forgotten food client session.
        ///  </summary>
        /// <MetaDataID>{2375e405-a15c-4144-b361-936dca75755f}</MetaDataID>
        private void CollectGarbageClientSessions()
        {
            //try
            //{
            //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //    {
            //        List<FoodServiceClientSession> forgottenClientSessions = _OpenClientSessions.Where(x => x.LongTimeForgotten).ToList();
            //        foreach (var forgottenClientSession in forgottenClientSessions)
            //        {
            //            var mainSession = forgottenClientSession.MainSession;
            //            if (mainSession != null)
            //            {
            //                mainSession.RemovePartialSession(forgottenClientSession);
            //            }

            //            OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(forgottenClientSession);
            //            RemoveClientSession(forgottenClientSession);

            //            if (mainSession != null && mainSession.PartialClientSessions.Count == 0 && mainSession.Meal != null)
            //                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(mainSession);

            //        }

            //        stateTransition.Consistent = true;
            //    }

            //}
            //catch (Exception error)
            //{
            //}
        }



        /// <MetaDataID>{ffcc6736-cc7f-4cfe-b777-b38edd3143d4}</MetaDataID>
        internal void RemoveClientSession(FoodServiceClientSession foodServiceClientSession)
        {
            lock (ObjLock)
            {
                if (_OpenClientSessions != null && _OpenClientSessions.Contains(foodServiceClientSession))
                    _OpenClientSessions.Remove(foodServiceClientSession);

            }
        }
        /// <MetaDataID>{843f5047-930f-4a7a-8287-885b16d56317}</MetaDataID>
        internal List<FoodServiceSession> OpenSessions
        {
            get
            {
                return (from clientSession in OpenClientSessions
                        select clientSession.MainSession).Distinct().OfType<FoodServiceSession>().ToList();

            }
        }

        /// <MetaDataID>{be5c7298-abaf-4538-a45c-8cee3b38f9a1}</MetaDataID>
        private void SessionsMonitoringTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                StartSimulator();
                foreach (var clientSession in OpenClientSessions.Where(x => x.MainSession == null))
                {
                    try
                    {
                        clientSession.MonitorTick();
                    }
                    catch (Exception error)
                    {
                    }
                }
                foreach (var mealSession in OpenSessions)
                {
                    try
                    {
                        mealSession.MonitorTick();
                    }
                    catch (Exception error)
                    {
                    }
                }

                List<HumanResources.Waiter> waitersWithUnreadedMessages = null;
                lock (ServiceContextRTLock)
                {
                    if (WaitersWithUnreadedMessages == null)
                    {
                        lock (ServiceContextRTLock)
                        {
                            var activeWaiters = (from shiftWork in GetActiveShiftWorks()
                                                 where shiftWork.Worker is IWaiter
                                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

                            WaitersWithUnreadedMessages = (from activeWaiter in activeWaiters
                                                           from message in activeWaiter.Messages
                                                           where message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable && !message.MessageReaded
                                                           select activeWaiter).ToList();

                        }
                    }
                    waitersWithUnreadedMessages = WaitersWithUnreadedMessages.ToList();
                }
                foreach (var waiter in WaitersWithUnreadedMessages)
                {
                    var messages = waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable && !x.MessageReaded && x.NotificationsNum <= 5).ToList();
                    if (messages.Count > 0)
                    {
                        var layMessage = messages[0];
                        if (!string.IsNullOrWhiteSpace(waiter.DeviceFirebaseToken))
                        {
                            var last = waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable && !x.MessageReaded).OrderByDescending(x => x.NotificationTimestamp).Last();
                            if (System.DateTime.UtcNow - last.NotificationTimestamp.ToUniversalTime() > TimeSpan.FromMinutes(2))
                            {
                                CloudNotificationManager.SendMessage(layMessage, waiter.DeviceFirebaseToken);
                                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                                {

                                    foreach (var message in waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable && !x.MessageReaded))
                                    {
                                        message.NotificationTimestamp = DateTime.UtcNow;
                                        message.NotificationsNum += 1;
                                    }

                                    stateTransition.Consistent = true;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception error)
            {


            }
            SessionsMonitoringTimer.Start();
        }

        /// <MetaDataID>{3a59a851-5bfb-4a61-afd8-cca6d4bdb045}</MetaDataID>
        internal void AddOpenServiceClientSession(FoodServiceClientSession fsClientSession)
        {
            lock (ObjLock)
            {
                if (_OpenClientSessions == null)
                {
                    _OpenClientSessions = (from session in new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this)).GetObjectCollection<EndUsers.FoodServiceClientSession>()
                                           select session).ToList();

                }
                _OpenClientSessions.Add(fsClientSession);
                fsClientSession.ServicesContextRunTime = this;
            }
        }

        /// <exclude>Excluded</exclude>
        string _OrganizationIdentity;

        /// <MetaDataID>{4bd8b124-6792-425c-ae02-81f80954be42}</MetaDataID>
        [PersistentMember(nameof(_OrganizationIdentity))]
        [BackwardCompatibilityID("+5")]
        public string OrganizationIdentity
        {
            get => _OrganizationIdentity;
            set
            {
                if (_OrganizationIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OrganizationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime _RestaurantMenuDataLastModified;

        /// <MetaDataID>{729f244b-6c19-4fdd-82d1-af3017095974}</MetaDataID>
        [PersistentMember(nameof(_RestaurantMenuDataLastModified))]
        [BackwardCompatibilityID("+4")]
        public DateTime RestaurantMenuDataLastModified
        {
            get => _RestaurantMenuDataLastModified;
            set
            {

                if (_RestaurantMenuDataLastModified.ToUniversalTime() != value.ToUniversalTime())
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RestaurantMenuDataLastModified = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _RestaurantMenuDataUri;

        /// <MetaDataID>{70f53356-dbbe-4fa8-9346-739f303977c9}</MetaDataID>
        [PersistentMember(nameof(_RestaurantMenuDataUri))]
        [BackwardCompatibilityID("+3")]
        public string RestaurantMenuDataUri
        {
            get => _RestaurantMenuDataUri;
            set
            {
                if (_RestaurantMenuDataUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RestaurantMenuDataUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        object PreparationStationRuntimesLock = new object();
        /// <exclude>Excluded</exclude>
        Dictionary<string, IPreparationStationRuntime> _PreparationStationRuntimes;
        [Association("ContextPreparationStationRuntime", Roles.RoleA, "471493c1-dcec-41bb-a354-e0dc03ea7101")]
        public Dictionary<string, IPreparationStationRuntime> PreparationStationRuntimes
        {
            get
            {
                lock (this)
                {
                    if (_PreparationStationRuntimes == null)
                    {
                        _PreparationStationRuntimes = new Dictionary<string, IPreparationStationRuntime>();
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);
                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        foreach (var preparationStation in (from aPreparationStation in servicesContextStorage.GetObjectCollection<PreparationStation>()
                                                            where aPreparationStation.ServicesContextIdentity == servicesContextIdentity
                                                            select aPreparationStation))
                        {
                            if (!string.IsNullOrWhiteSpace(preparationStation.PreparationStationIdentity))
                                this._PreparationStationRuntimes[preparationStation.PreparationStationIdentity] = preparationStation;

                        }
                    }
                    return new Dictionary<string, IPreparationStationRuntime>(_PreparationStationRuntimes);
                }
            }
        }



        //internal static ServicesContextRunTime GetServicesContextRunTime(OOAdvantech.PersistenceLayer.ObjectStorage objectStorage, string servicesContextIdentity)
        //{

        //    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
        //    var servicesContextRunTime = (from theServicePointRun in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
        //                                  where theServicePointRun.ServicesContextIdentity == servicesContextIdentity
        //                                  select theServicePointRun).FirstOrDefault();

        //    return servicesContextRunTime;

        //}


        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;
        /// <MetaDataID>{479020f0-a84d-4a71-ae33-6442d0578cda}</MetaDataID>
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
        OOAdvantech.Collections.Generic.Set<FlavourBusinessStorage> _Storages = new OOAdvantech.Collections.Generic.Set<FlavourBusinessStorage>();

        [PersistentMember(nameof(_Storages))]
        [Association("ServicePointStorages", Roles.RoleA, "744ca35f-21a3-4548-92c5-e185a11ced30")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.CascadeDelete)]
        public System.Collections.Generic.IList<FlavourBusinessStorage> Storages
        {
            get
            {
                return _Storages.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        public string _Description;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{72cdeed6-b146-405e-97dd-d1ef21dd72fb}</MetaDataID>
        internal void WaiterSiftWorkUpdated(HumanResources.Waiter waiter)
        {
            if (waiter.ActiveShiftWork != null && !ActiveShiftWorks.Contains(waiter.ActiveShiftWork))
                ActiveShiftWorks.Add(waiter.ActiveShiftWork);

            var activeShiftWorks = GetActiveShiftWorks();


        }

        /// <MetaDataID>{fa22f19b-550e-4ae2-8c6c-315fa0ea2b26}</MetaDataID>
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
                    ObjectChangeState?.Invoke(this, nameof(Description));
                }


            }
        }

        /// <MetaDataID>{d71eaa49-3fce-4cf4-8cf6-bad7d22a3bb6}</MetaDataID>
        internal void MealConversationTimeout(ServicePoint servicePoint, string sessionIdentity, List<Caregiver> caregivers)
        {
            var activeWaiters = caregivers.Where(x => x.Worker is HumanResources.Waiter && x.Caregiving == Caregiver.CaregivingType.ConversationCheck).Select(x => x.Worker as HumanResources.Waiter).ToList();
            if (activeWaiters.Count == 0)
                activeWaiters = (from shiftWork in GetActiveShiftWorks()
                                 where shiftWork.Worker is IWaiter && servicePoint.IsAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();


            foreach (var waiter in activeWaiters)
            {
                var waiterActiveShiftWork = waiter.ActiveShiftWork;
                if (waiterActiveShiftWork != null && DateTime.UtcNow > waiterActiveShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < waiterActiveShiftWork.EndsAt.ToUniversalTime())
                {
                    Message clientMessage = waiter.Messages.Where(x => x.HasDataValue<ClientMessages>("ClientMessageType", ClientMessages.MealConversationTimeout) && x.HasDataValue<string>("ServicesPointIdentity", servicePoint.ServicesPointIdentity)).FirstOrDefault();
                    if (clientMessage == null)
                    {
                        clientMessage = new Message();
                        clientMessage.Data["ClientMessageType"] = ClientMessages.MealConversationTimeout;
                        clientMessage.Data["ServicesPointIdentity"] = servicePoint.ServicesPointIdentity;
                        clientMessage.Data["SessionIdentity"] = sessionIdentity;

                        clientMessage.Notification = new Notification() { Title = "Meal conversation is over time" };
                    }
                    waiter.PushMessage(clientMessage);
                    if (!string.IsNullOrWhiteSpace(waiter.DeviceFirebaseToken))
                    {
                        CloudNotificationManager.SendMessage(clientMessage, waiter.DeviceFirebaseToken);

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var message in waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MealConversationTimeout && !x.MessageReaded))
                            {
                                message.NotificationsNum += 1;
                                message.NotificationTimestamp = DateTime.UtcNow;
                            }

                            stateTransition.Consistent = true;
                        }
                        lock (ServiceContextRTLock)
                        {
                            WaitersWithUnreadedMessages = (from activeWaiter in activeWaiters
                                                           from message in activeWaiter.Messages
                                                           where message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MealConversationTimeout && !message.MessageReaded
                                                           select activeWaiter).ToList();
                        }

                    }

                }
                //}
            }

        }


        /// <MetaDataID>{c1f668e1-a630-46cf-9797-3569d2e193b2}</MetaDataID>
        List<HumanResources.Waiter> WaitersWithUnreadedMessages = null;
        /// <MetaDataID>{a8d06287-3ab2-4a1a-8858-e36d8822fcb9}</MetaDataID>
        internal void ServicePointChangeState(ServicePoint servicePoint, ServicePointState oldState, ServicePointState newState)
        {
            if (newState == ServicePointState.Laying)
            {
                //if (servicePoint.OpenClientSessions.Where(x => !x.IsWaiterSession).FirstOrDefault() != null)
                //{
                var activeWaiters = (from shiftWork in GetActiveShiftWorks()
                                     where shiftWork.Worker is IWaiter && servicePoint.IsAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                     select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

                foreach (var waiter in activeWaiters)
                {
                    var waiterActiveShiftWork = waiter.ActiveShiftWork;
                    if (waiterActiveShiftWork != null && DateTime.UtcNow > waiterActiveShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < waiterActiveShiftWork.EndsAt.ToUniversalTime())
                    {
                        Message clientMessage = waiter.Messages.Where(x => x.HasDataValue<ClientMessages>("ClientMessageType", ClientMessages.LaytheTable) && x.HasDataValue<string>("ServicesPointIdentity", servicePoint.ServicesPointIdentity)).FirstOrDefault();

                        if (clientMessage == null)
                        {
                            clientMessage = new Message();
                            clientMessage.Data["ClientMessageType"] = ClientMessages.LaytheTable;
                            clientMessage.Data["ServicesPointIdentity"] = servicePoint.ServicesPointIdentity;
                            clientMessage.Notification = new Notification() { Title = "Lay the Table" };
                        }
                        waiter.PushMessage(clientMessage);

                        if (!string.IsNullOrWhiteSpace(waiter.DeviceFirebaseToken))
                        {
                            CloudNotificationManager.SendMessage(clientMessage, waiter.DeviceFirebaseToken);

                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                foreach (var message in waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable && !x.MessageReaded))
                                {
                                    message.NotificationsNum += 1;
                                    message.NotificationTimestamp = DateTime.UtcNow;
                                }

                                stateTransition.Consistent = true;
                            }
                            lock (ServiceContextRTLock)
                            {
                                WaitersWithUnreadedMessages = (from activeWaiter in activeWaiters
                                                               from message in activeWaiter.Messages
                                                               where message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable && !message.MessageReaded
                                                               select activeWaiter).ToList();
                            }

                        }

                    }
                    //}
                }
            }
        }




        /// <MetaDataID>{d718d244-0e50-4ac9-b66a-495fc9f91b43}</MetaDataID>
        internal void MealItemsReadyToServe(ServicePoint servicePoint)
        {


            var activeWaiters = (from shiftWork in GetActiveShiftWorks()
                                 where shiftWork.Worker is IWaiter && servicePoint.IsAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

            foreach (var waiter in activeWaiters)
            {
                if (waiter.ActiveShiftWork != null && DateTime.UtcNow > waiter.ActiveShiftWork.StartsAt.ToUniversalTime() && DateTime.UtcNow < waiter.ActiveShiftWork.EndsAt.ToUniversalTime())
                {
                    var clientMessage = waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe && !x.MessageReaded).OrderBy(x => x.MessageTimestamp).FirstOrDefault();
                    if (clientMessage == null)
                    {
                        clientMessage = new Message();
                        clientMessage.Data["ClientMessageType"] = ClientMessages.ItemsReadyToServe;
                        clientMessage.Data["ServicesPointIdentity"] = servicePoint.ServicesPointIdentity;
                        clientMessage.Notification = new Notification() { Title = "There are items read to serve", Body = "Check items ready to serve List" };
                    }
                    waiter.PushMessage(clientMessage);

                    if (!string.IsNullOrWhiteSpace(waiter.DeviceFirebaseToken))
                    {
                        CloudNotificationManager.SendMessage(clientMessage, waiter.DeviceFirebaseToken);

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var message in waiter.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe && !x.MessageReaded))
                            {
                                message.NotificationsNum += 1;
                                message.NotificationTimestamp = DateTime.UtcNow;
                            }

                            stateTransition.Consistent = true;
                        }
                        lock (ServiceContextRTLock)
                        {
                            WaitersWithUnreadedMessages = (from activeWaiter in activeWaiters
                                                           from message in activeWaiter.Messages
                                                           where message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe && !message.MessageReaded
                                                           select activeWaiter).ToList();
                        }
                    }
                }
            }
        }

        /// <MetaDataID>{8828ea17-8bb9-42c6-b5e1-36270c0ae343}</MetaDataID>
        public IList<IHallLayout> Halls
        {
            get
            {
                List<IHallLayout> halls = new System.Collections.Generic.List<FlavourBusinessFacade.ServicesContextResources.IHallLayout>();

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

                var servicesContextRunTime = (from theServicePointRun in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                                              where theServicePointRun.ServicesContextIdentity == ServicesContextIdentity
                                              select theServicePointRun).FirstOrDefault();

                foreach (var serviceArea in (from aServiceArea in storage.GetObjectCollection<ServicesContextResources.ServiceArea>()
                                             select aServiceArea))
                {
                    if (!string.IsNullOrWhiteSpace(serviceArea.HallLayoutUri))
                    {
                        RestaurantHallLayoutModel.HallLayout hallLayout = ObjectStorage.GetObjectFromUri(serviceArea.HallLayoutUri) as RestaurantHallLayoutModel.HallLayout;


                        if (hallLayout == null)
                            continue;

                        hallLayout.ServiceArea = serviceArea;
                        foreach (var servicePointShape in hallLayout.Shapes.Where(x => !string.IsNullOrWhiteSpace(x.ServicesPointIdentity)))
                        {
                            var servicePoint = hallLayout.ServiceArea.ServicePoints.Where(x => x.ServicesPointIdentity == servicePointShape.ServicesPointIdentity).FirstOrDefault();
                            if (servicePoint != null)
                                servicePointShape.ServicesPointState = servicePoint.State;
                        }

                        halls.Add(hallLayout);
                    }
                }
                return halls;
            }
        }

        /// <MetaDataID>{87b7a308-7a41-4dab-bb16-71548617adda}</MetaDataID>
        public List<OrganizationStorageRef> GraphicMenus
        {
            get
            {
                List<OrganizationStorageRef> graphicMenusStorages = new List<OrganizationStorageRef>();

                var fbstorages = (from storage in this.Storages
                                  where storage.FlavourStorageType == OrganizationStorages.GraphicMenu
                                  select storage).ToList();

                string urlRoot = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri;
                foreach (var fbStorage in fbstorages)
                {
                    var storageUrl = urlRoot + fbStorage.Url;
                    var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbStorage.Url);

                    OrganizationStorageRef storageRef = new OrganizationStorageRef { StorageIdentity = fbStorage.StorageIdentity, FlavourStorageType = fbStorage.FlavourStorageType, Name = fbStorage.Name, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime, Version = fbStorage.Version };
                    graphicMenusStorages.Add(storageRef);
                }

                return graphicMenusStorages;
            }
        }




        /// <MetaDataID>{bcb050fe-a95c-4908-8fa8-9ac1b251721e}</MetaDataID>
        public void AssignGraphicMenu(OrganizationStorageRef graphicMenuStorageRef)
        {

            if ((from storage in _Storages
                 where storage.StorageIdentity == graphicMenuStorageRef.StorageIdentity
                 select storage).Count() == 0)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    var fbstorage = new FlavourBusinessStorage();
                    fbstorage.StorageIdentity = graphicMenuStorageRef.StorageIdentity;
                    fbstorage.Name = graphicMenuStorageRef.Name;

                    int npos = graphicMenuStorageRef.StorageUrl.IndexOf("usersfolder/");
                    if (npos != -1)
                        fbstorage.Url = graphicMenuStorageRef.StorageUrl.Substring(npos);
                    else
                        fbstorage.Url = graphicMenuStorageRef.StorageUrl;
                    fbstorage.FlavourStorageType = OrganizationStorages.GraphicMenu;
                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                    _Storages.Add(fbstorage);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{4afc6793-f079-4341-b3ad-c3f196ae04df}</MetaDataID>
        object RestaurantMenusDataLock = new object();
        /// <MetaDataID>{dd6fcb7a-0ce2-46cd-b11e-d205f516854b}</MetaDataID>
        public void SetRestaurantMenusData(OrganizationStorageRef restaurantMenusDataStorageRef)
        {
            lock (RestaurantMenusDataLock)
            {
                bool publishRestaurantMenuData = false;
                var fbstorage = (from storage in _Storages
                                 where storage.FlavourStorageType == OrganizationStorages.OperativeRestaurantMenu
                                 select storage).FirstOrDefault();

                if (fbstorage == null)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        fbstorage = new FlavourBusinessStorage();
                        ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                        _Storages.Add(fbstorage);
                        stateTransition.Consistent = true;
                    }
                }
                fbstorage.FlavourStorageType = OrganizationStorages.OperativeRestaurantMenu;
                fbstorage.StorageIdentity = restaurantMenusDataStorageRef.StorageIdentity;
                fbstorage.Name = restaurantMenusDataStorageRef.Name;
                int npos = restaurantMenusDataStorageRef.StorageUrl.IndexOf("usersfolder/");
                if (npos != -1)
                    fbstorage.Url = restaurantMenusDataStorageRef.StorageUrl.Substring(npos);
                else
                    fbstorage.Url = restaurantMenusDataStorageRef.StorageUrl;
                publishRestaurantMenuData = RestaurantMenuDataLastModified.ToUniversalTime() != restaurantMenusDataStorageRef.TimeStamp.ToUniversalTime();

                string restaurantMenusDataStorageUrl = restaurantMenusDataStorageRef.StorageUrl;
                RestaurantMenuDataLastModified = restaurantMenusDataStorageRef.TimeStamp;
                RestaurantMenuDataUri = restaurantMenusDataStorageUrl;


                if (publishRestaurantMenuData)
                    PublishMenuRestaurantMenuData();
            }
        }

        /// <MetaDataID>{86ae372e-b324-43c2-a142-debc7505971f}</MetaDataID>
        void PublishMenuRestaurantMenuData()
        {

            IFileManager fileManager = new BlobFileManager(FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);
            var restaurantMenusData = Storages.Where(x => x.FlavourStorageType == OrganizationStorages.OperativeRestaurantMenu).FirstOrDefault();
            string version = "";
            string oldVersion = restaurantMenusData.Version;
            if (string.IsNullOrWhiteSpace(oldVersion))
            {
                version = "v1";
            }
            else
            {
                int v = int.Parse(oldVersion.Replace("v", ""));
                v++;
                version = "v" + v.ToString();
            }

            string previousVersionServerStorageFolder = GetVersionFolder(oldVersion);

            string serverStorageFolder = GetVersionFolder(version);
            string jsonFileName = serverStorageFolder + restaurantMenusData.Name + ".json";
            WritePublicRestaurantMenuData(jsonFileName);

            if (fileManager != null)
            {
                jsonFileName = previousVersionServerStorageFolder + restaurantMenusData.Name + ".json";
                fileManager.GetBlobInfo(jsonFileName).DeleteIfExists();
            }

            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                restaurantMenusData.Version = version;
                stateTransition.Consistent = true;
            }



            //jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
            //var sss = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.IMenuItem>>(jsonEx, jSetttings);
        }
        /// <MetaDataID>{09d076c1-518e-4c4c-8c94-ad05f45ab97a}</MetaDataID>
        private void WritePublicRestaurantMenuData(string jsonFileName)
        {
            IFileManager fileManager = new BlobFileManager(FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);

            //var fbstorage = (from servicesContextRunTimeStorage in Storages
            //                 where servicesContextRunTimeStorage.FlavourStorageType == FlavourBusinessFacade.OrganizationStorages.OperativeRestaurantMenu
            //                 select servicesContextRunTimeStorage).FirstOrDefault();
            //var storageRef = new FlavourBusinessFacade.OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = RestaurantMenuDataUri, TimeStamp = RestaurantMenuDataLastModified };

            //RawStorageData rawStorageData = new RawStorageData(storageRef, null);
            //OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));

            var objectStorage = ObjectStorage.GetStorageOfObject(OperativeRestaurantMenu);
            OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(objectStorage);

            Dictionary<object, object> mappedObject = new Dictionary<object, object>();
            List<MenuModel.IMenuItem> menuFoodItems = (from menuItem in restMenusData.GetObjectCollection<MenuModel.IMenuItem>()
                                                       select menuItem).ToList().Select(x => new MenuModel.JsonViewModel.MenuFoodItem(x, mappedObject)).OfType<MenuModel.IMenuItem>().ToList();

            var meneuItemsNames = menuFoodItems.Select(X => X.Name).ToList();

            var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
            string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSetttings);
            MemoryStream jsonRestaurantMenuStream = new MemoryStream();
            byte[] jsonBuffer = System.Text.Encoding.UTF8.GetBytes(jsonEx);
            jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
            jsonRestaurantMenuStream.Position = 0;

            if (fileManager != null)
                fileManager.Upload(jsonFileName, jsonRestaurantMenuStream, "application/json");


        }

        /// <MetaDataID>{a6d225d5-a273-4988-bbc2-d773cbd1ecb9}</MetaDataID>
        private string GetVersionFolder(string version)
        {
            string versionSuffix = "";
            if (!string.IsNullOrWhiteSpace(version))
                versionSuffix = "/" + version + "/";
            else
                versionSuffix = "/";

            string serverStorageFolder = string.Format("usersfolder/{0}/{1}{2}", OrganizationIdentity, this.ServicesContextIdentity, versionSuffix);
            return serverStorageFolder;
        }


        /// <MetaDataID>{cb479cfa-63f5-4f29-ba81-f1e597d4af0b}</MetaDataID>
        public void RemoveGraphicMenu(OrganizationStorageRef graphicMenuStorageRef)
        {
            var fbstorage = (from storage in _Storages
                             where storage.StorageIdentity == graphicMenuStorageRef.StorageIdentity
                             select storage).FirstOrDefault();
            if (fbstorage != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Storages.Remove(fbstorage);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{e57f4255-89a4-44e4-8b44-c9688dab616b}</MetaDataID>
        public void GraphicMenuStorageMetaDataUpdated(OrganizationStorageRef graphicMenuStorageRef)
        {
            var fbstorage = (from storage in _Storages
                             where storage.StorageIdentity == graphicMenuStorageRef.StorageIdentity
                             select storage).FirstOrDefault();
            if (fbstorage != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    fbstorage.Name = graphicMenuStorageRef.Name;

                    int npos = graphicMenuStorageRef.StorageUrl.IndexOf("usersfolder/");
                    if (npos != -1)
                        fbstorage.Url = graphicMenuStorageRef.StorageUrl.Substring(npos);
                    else
                        fbstorage.Url = graphicMenuStorageRef.StorageUrl;

                    fbstorage.Version = graphicMenuStorageRef.Version;

                    stateTransition.Consistent = true;
                }
            }


        }

        /// <MetaDataID>{e1d0f93c-b314-44dd-a9ba-41a73faf3a51}</MetaDataID>
        public void OperativeRestaurantMenuDataUpdated(OrganizationStorageRef restaurantMenusDataStorageRef)
        {
            var menuItem = ObjectStorage.GetObjectFromUri<MenuModel.MenuItem>(@"7021ec91-37df-4417-8c1a-a6afb012fd09\3\56");
            ObjectStorage.UpdateOperativeOperativeObjects(restaurantMenusDataStorageRef.StorageIdentity);
            var menuItem2 = ObjectStorage.GetObjectFromUri<MenuModel.MenuItem>(@"7021ec91-37df-4417-8c1a-a6afb012fd09\3\56");
            SetRestaurantMenusData(restaurantMenusDataStorageRef);


            var menuItem3 = ObjectStorage.GetObjectFromUri<MenuModel.MenuItem>(@"7021ec91-37df-4417-8c1a-a6afb012fd09\3\56");

            foreach (var session in OpenSessions)
                session.Menu = null;

            ObjectChangeState?.Invoke(this, nameof(OperativeRestaurantMenu));

            //PublishMenuRestaurantMenuData();

        }
        ///Data Source=arion2017.database.windows.net;Initial Catalog=orderserver;Integrated Security=False;User ID=sotirnikol;Password=1Arionsapwd;Connect Timeout=350;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False

        /// <exclude>Excluded</exclude>
        List<IServiceArea> _ServiceAreas;
        /// <MetaDataID>{cabdf8c9-31ab-4668-9bc0-861e965e6996}</MetaDataID>
        public IList<IServiceArea> ServiceAreas
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_ServiceAreas == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        _ServiceAreas = (from serviceArea in servicesContextStorage.GetObjectCollection<IServiceArea>()
                                         where serviceArea.ServicesContextIdentity == servicesContextIdentity
                                         select serviceArea).ToList();
                    }

                    return _ServiceAreas.ToList();
                }

            }
        }




        /// <MetaDataID>{71d65deb-60c4-4cda-97d3-227ffe95f45b}</MetaDataID>
        public IServiceArea NewServiceArea()
        {

            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            ServicesContextResources.ServiceArea serviceArea = new ServicesContextResources.ServiceArea();
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {


                serviceArea.Description = Properties.Resources.DefaultServiceAreaDescription;
                serviceArea.ServicesContextIdentity = this.ServicesContextIdentity;

                MenuModel.FixedMealType twoCourseMealType = GetTowcoursesMealType();

                serviceArea.AddMealType(ObjectStorage.GetStorageOfObject(twoCourseMealType).GetPersistentObjectUri(twoCourseMealType));


                objectStorage.CommitTransientObjectState(serviceArea);

                stateTransition.Consistent = true;
            }
            lock (ServiceContextRTLock)
            {
                if (!ServiceAreas.Contains(serviceArea))
                    _ServiceAreas.Add(serviceArea);
            }
            return serviceArea;

        }

        /// <MetaDataID>{5beee6e4-0f02-4127-8e9d-679d34553969}</MetaDataID>
        internal MenuModel.FixedMealType GetTowcoursesMealType()
        {
            if (OperativeRestaurantMenu != null)
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(OperativeRestaurantMenu);
                if (objectStorage != null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                    var mealTypes = (from mealType in storage.GetObjectCollection<MenuModel.FixedMealType>()
                                     select mealType).ToList();
                    var twoCourseMealType = mealTypes.Where(x => x.Courses.Count() == 2).FirstOrDefault();
                    return twoCourseMealType;
                }

            }
            return null;
        }



        /// <MetaDataID>{d42d73e7-41a2-4abd-9466-d6e39f973e0c}</MetaDataID>
        public void RemoveServiceArea(IServiceArea serviceArea)
        {
            ObjectStorage.DeleteObject(serviceArea);
            lock (ServiceContextRTLock)
            {
                if (ServiceAreas.Contains(serviceArea))
                    _ServiceAreas.Remove(serviceArea);
            }
        }


        /// <MetaDataID>{d937fd62-64f5-4dce-8944-424f0cba01ef}</MetaDataID>
        bool CallerIDServerLoaded;

        /// <exclude>Excluded</exclude>
        ICallerIDServer _CallerIDServer;

        /// <MetaDataID>{004af86f-b675-4824-973b-0cb7ef14c57b}</MetaDataID>
        public FlavourBusinessFacade.ServicesContextResources.ICallerIDServer CallerIDServer
        {
            get
            {
                if (!CallerIDServerLoaded)
                {
                    var objectStorage = ObjectStorage.GetStorageOfObject(this);// OpenServicesContextStorageStorage();
                    OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
                    var servicesContextIdentity = ServicesContextIdentity;
                    _CallerIDServer = (from serviceArea in servicesContextStorage.GetObjectCollection<ICallerIDServer>()
                                       where serviceArea.ServicesContextIdentity == servicesContextIdentity
                                       select serviceArea).FirstOrDefault();

                    CallerIDServerLoaded = true;
                }

                return _CallerIDServer;
            }
        }

        /// <MetaDataID>{d84f89df-d312-47da-af2e-bf226a9fa832}</MetaDataID>
        public IList<ICashierStation> CashierStations
        {

            get
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                var servicesContextIdentity = ServicesContextIdentity;
                return (from cashierStation in servicesContextStorage.GetObjectCollection<ICashierStation>()
                        where cashierStation.ServicesContextIdentity == servicesContextIdentity
                        select cashierStation).ToList();

            }

        }
        /// <MetaDataID>{d134e638-dbd1-48af-8051-35d64d569422}</MetaDataID>
        public IList<FinanceFacade.IFisicalParty> FisicalParties
        {
            get
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                var servicesContextIdentity = ServicesContextIdentity;

                return (from fisicalParty in servicesContextStorage.GetObjectCollection<IFisicalParty>()
                        select fisicalParty).ToList();
            }
        }

        /// <MetaDataID>{f26b75fe-4231-4cc0-890d-618a20ed0f42}</MetaDataID>
        public ServiceContextResources ServiceContextResources
        {
            get
            {
                return new ServiceContextResources() { CallerIDServer = CallerIDServer, CashierStations = CashierStations, ServiceAreas = ServiceAreas, PreparationStations = PreparationStations };
            }
        }

        /// <MetaDataID>{fa9fc77b-4d3e-40c2-a258-aa5d6d39c9b9}</MetaDataID>
        public ServiceContextHumanResources ServiceContextHumanResources
        {
            get
            {
                var activeShiftWorks = GetActiveShiftWorks();
                return new ServiceContextHumanResources() { Waiters = Waiters.Where(x => !string.IsNullOrWhiteSpace(x.SignUpUserIdentity)).ToList(), Supervisors = Supervisors.Where(x => !string.IsNullOrWhiteSpace(x.SignUpUserIdentity)).ToList(), ActiveShiftWorks = activeShiftWorks };
            }
        }


        /// <MetaDataID>{37b1908b-a3e7-4a4e-a710-8ef5a0145ae0}</MetaDataID>
        List<IShiftWork> ActiveShiftWorks;

        /// <MetaDataID>{a82e0e64-72d8-4c6c-bc2d-6c30c4737337}</MetaDataID>
        internal IList<IShiftWork> GetActiveShiftWorks()
        {
            var mileStoneDate = System.DateTime.UtcNow - TimeSpan.FromDays(1);
            if (ActiveShiftWorks == null)
            {

                var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

                var shiftWorks = (from shiftWork in storage.GetObjectCollection<FlavourBusinessManager.HumanResources.ShiftWork>()
                                  where shiftWork.StartsAt > mileStoneDate
                                  orderby shiftWork.StartsAt
                                  group shiftWork by shiftWork.Worker into workerShiftWorks
                                  select new { worker = workerShiftWorks.Key, workerShiftWorks }).ToList();




                var activeShiftWork = shiftWorks.Select(x => x.workerShiftWorks.Last()).OfType<IShiftWork>().ToList();
                ActiveShiftWorks = activeShiftWork;
            }
            else
            {
                ActiveShiftWorks = ActiveShiftWorks.Where(x => x.StartsAt > mileStoneDate).ToList();
            }


            return ActiveShiftWorks;
        }


        /// <MetaDataID>{e9d1e0b0-cc99-4300-a7d7-8aeff007e83f}</MetaDataID>
        public IList<IPreparationStation> PreparationStations
        {
            get
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                var servicesContextIdentity = ServicesContextIdentity;

                return PreparationStationRuntimes.Values.OfType<IPreparationStation>().ToList();

                //return (from preparationStation in servicesContextStorage.GetObjectCollection<IPreparationStation>()
                //        where preparationStation.ServicesContextIdentity == servicesContextIdentity
                //        select preparationStation).ToList();

            }
        }


        /// <exclude>Excluded</exclude>
        int _AllMessmetesCommitedTimeSpan = 120;//3min

        /// <summary>
        /// Defines the timespan in seconds to wait in AllMessmetesCommited state before move to meal monitoring state and starts meal preparation. 
        /// </summary>
        /// <MetaDataID>{594d0953-06b7-46d0-953e-5efd14086a70}</MetaDataID>
        [PersistentMember(nameof(_AllMessmetesCommitedTimeSpan))]
        [BackwardCompatibilityID("+6")]
        public int AllMessmetesCommitedTimeSpan
        {
            get => _AllMessmetesCommitedTimeSpan;
            set
            {
                if (_AllMessmetesCommitedTimeSpan != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AllMessmetesCommitedTimeSpan = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{379b7e7e-2f39-4d55-aa85-6ee85a88e9fd}</MetaDataID>
        public string RestaurantMenuDataSharedUri
        {
            get
            {
                lock (RestaurantMenusDataLock)
                {
                    var restaurantMenusData = Storages.Where(x => x.FlavourStorageType == OrganizationStorages.OperativeRestaurantMenu).FirstOrDefault();
                    string version = restaurantMenusData.Version;
                    string serverStorageFolder = GetVersionFolder(version);
                    string jsonFileName = serverStorageFolder + restaurantMenusData.Name + ".json";
                    return RawStorageCloudBlob.RootUri + "/" + jsonFileName;
                }

            }
        }
        /// <exclude>Excluded</exclude>
        FlavourBusinessFacade.RoomService.IMealsController _MealsController;

        /// <MetaDataID>{80ea5e2b-4baa-4036-af72-8a91354dbe36}</MetaDataID>
        public IMealsController MealsController
        {
            get
            {
                if (_MealsController == null)
                    _MealsController = new MealsController(this);
                return _MealsController;
            }
        }

        /// <MetaDataID>{43a48158-e9c5-4be3-9813-4433f3d0c580}</MetaDataID>
        ISettings _Settings;

        /// <MetaDataID>{a219ef11-d9cb-4af1-886e-ea14f9e844d9}</MetaDataID>
        public ISettings Settings
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_Settings == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        _Settings = (from settings in servicesContextStorage.GetObjectCollection<ISettings>()
                                     select settings).FirstOrDefault();
                        if (_Settings == null)
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                _Settings = new Settings()
                                {
                                    AutoAssignMaxMealProgress = 50,
                                    ForgottenSessionDeviceSleepTimeSpanInMin = 3,
                                    ForgottenSessionLastChangeTimeSpanInMin = 10,
                                    ForgottenSessionLifeTimeSpanInMin = 20,
                                    MealConversationTimeoutInMin = 10,
                                    MealConversationTimeoutWaitersUpdateTimeSpanInMin = 4
                                };

                                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(_Settings);
                                stateTransition.Consistent = true;
                            }
                        }
                    }
                    return _Settings;
                }
            }
        }


        /// <MetaDataID>{65920937-f6f4-4472-870b-ff1b9081ed7f}</MetaDataID>
        public void LaunchCallerIDServer()
        {
            if (CallerIDServer == null)
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    _CallerIDServer = new ServicesContextResources.CallerIDServer();
                    _CallerIDServer.ServicesContextIdentity = this.ServicesContextIdentity;
                    objectStorage.CommitTransientObjectState(_CallerIDServer);

                    stateTransition.Consistent = true;
                }

            }
        }

        /// <MetaDataID>{7d26d5d7-3508-4ceb-b671-2c581af67b6f}</MetaDataID>
        public void RemoveCallerIDServer()
        {
            if (CallerIDServer != null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    ObjectStorage.DeleteObject(_CallerIDServer);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{79d3d467-5be2-47e5-9f07-957900db58cf}</MetaDataID>
        public ICashierStation NewCashierStation()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            CashierStation cashierStation = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                cashierStation = new ServicesContextResources.CashierStation(this.ServicesContextIdentity);
                cashierStation.Description = Properties.Resources.DefaultPreparationStationDescription;

                objectStorage.CommitTransientObjectState(cashierStation);

                stateTransition.Consistent = true;
            }
            return cashierStation;
        }

        /// <MetaDataID>{422eba89-0568-4bc5-af3b-ed76e133dac7}</MetaDataID>
        public void RemoveCashierStation(ICashierStation cashierStation)
        {
            try
            {
                ObjectStorage.DeleteObject(cashierStation);
            }
            catch (Exception error)
            {

                throw;
            }
        }



        /// <MetaDataID>{1513e163-7a1b-4747-97cf-161a9fc8e55a}</MetaDataID>
        static internal Dictionary<IFoodServiceClientSession, string> FoodServiceClientSessionsTokens = new Dictionary<IFoodServiceClientSession, string>();

        //clientDeviceID="81000000296"
        //clientName="clientName"

        /// <MetaDataID>{fd5b3748-a682-47e5-8c57-59022f9e4f17}</MetaDataID>
        public ClientSessionData GetClientSession(string servicePointIdentity, string mealInvitationSessionID, string clientName, string clientDeviceID, string deviceFirebaseToken, string organizationIdentity, List<OrganizationStorageRef> graphicMenus, bool create)
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

            var servicesContextIdentity = ServicesContextIdentity;
            var servicePoint = (from serviceArea in ServiceAreas
                                from aServicePoint in serviceArea.ServicePoints
                                where aServicePoint.ServicesPointIdentity == servicePointIdentity
                                select aServicePoint).FirstOrDefault();

            //(from aServicePoint in servicesContextStorage.GetObjectCollection<IServicePoint>()
            //                  where aServicePoint.ServicesPointIdentity == servicePointIdentity
            //                  select aServicePoint).FirstOrDefault();

            var clientSession = servicePoint.GetFoodServiceClientSession(clientName, mealInvitationSessionID, clientDeviceID, deviceFirebaseToken, create);

            if (clientSession == null)
                return new ClientSessionData();

            string token = null;
            var graphicMenu = graphicMenus.FirstOrDefault();

            //if ((clientSession.Menu == null || clientSession.Menu.Version != graphicMenu.Version))
            //{


            //    //graphicMenu = (from gMenu in graphicMenus where gMenu.StorageIdentity == flavoursServicesContexGraphicMenu.StorageIdentity select gMenu).FirstOrDefault();

            //    string versionSuffix = "";
            //    if (!string.IsNullOrWhiteSpace(graphicMenu.Version))
            //        versionSuffix = "/" + graphicMenu.Version;
            //    else
            //        versionSuffix = "";

            //    graphicMenu.StorageUrl = RawStorageCloudBlob.RootUri + string.Format("/usersfolder/{0}/Menus/{1}{3}/{2}.json", organizationIdentity, graphicMenu.StorageIdentity, graphicMenu.Name, versionSuffix);
            //    (clientSession as EndUsers.FoodServiceClientSession).Menu = graphicMenu;
            //    if ((clientSession as EndUsers.FoodServiceClientSession).MainSession != null)
            //        ((clientSession as EndUsers.FoodServiceClientSession).MainSession as FoodServiceSession).MenuStorageIdentity = graphicMenu.StorageIdentity;

            //}
            return clientSession.ClientSessionData;
            //token = GetToken(clientSession, token);
            //var defaultMealTypeUri = clientSession.ServicePoint.ServesMealTypesUris.FirstOrDefault();
            //var servedMealTypesUris = clientSession.ServicePoint.ServesMealTypesUris.ToList();

            //if (defaultMealTypeUri == null)
            //{
            //    defaultMealTypeUri = clientSession.ServicePoint.ServiceArea.ServesMealTypesUris.FirstOrDefault();
            //    servedMealTypesUris = clientSession.ServicePoint.ServiceArea.ServesMealTypesUris.ToList();
            //}

            //return new ClientSessionData() { ServicesContextLogo = "Pizza Hut", ServicesPointName = servicePoint.Description, ServicePointIdentity = servicesContextIdentity + ";" + servicePointIdentity, Token = token, FoodServiceClientSession = clientSession, ServedMealTypesUris = servedMealTypesUris, DefaultMealTypeUri = defaultMealTypeUri, ServicePointState = servicePoint.State };
        }

        /// <MetaDataID>{1f4f29a1-b453-4221-bdc0-dda0d3b40017}</MetaDataID>
        internal static string GetToken(IFoodServiceClientSession clientSession)
        {
            string token = null;
            lock (FoodServiceClientSessionsTokens)
            {
                if (clientSession != null)
                {
                    if (!FoodServiceClientSessionsTokens.TryGetValue(clientSession, out token))
                    {
                        token = Guid.NewGuid().ToString("N") + "_" + OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(clientSession).GetPersistentObjectUri(clientSession);
                        FoodServiceClientSessionsTokens[clientSession] = token;
                    }
                }
            }

            return token;
        }

        /// <MetaDataID>{7be35e44-04e6-418d-b29e-100f9c6f71b0}</MetaDataID>
        public void RemovePreparationStation(IPreparationStation prepartionStation)
        {
            try
            {

                if (PreparationStationRuntimes.ContainsKey(prepartionStation.PreparationStationIdentity))
                    PreparationStationRuntimes.Remove(prepartionStation.PreparationStationIdentity);




                ObjectStorage.DeleteObject(prepartionStation);
            }
            catch (Exception error)
            {

                throw;
            }
        }

        /// <MetaDataID>{0d4e50d1-2089-474c-9316-9088844fd894}</MetaDataID>
        public IPreparationStation NewPreparationStation()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            ServicesContextResources.PreparationStation preparationStation = new ServicesContextResources.PreparationStation(this);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                preparationStation.Description = Properties.Resources.DefaultPreparationStationDescription;
                preparationStation.ServicesContextIdentity = this.ServicesContextIdentity;
                objectStorage.CommitTransientObjectState(preparationStation);
                stateTransition.Consistent = true;
            }
            var count = PreparationStationRuntimes.Count;
            lock (PreparationStationRuntimesLock)
            {
                _PreparationStationRuntimes[preparationStation.PreparationStationIdentity] = preparationStation;
            }
            return preparationStation;
        }



        /// <MetaDataID>{84849525-39ef-4904-a46a-dae0f0abdf47}</MetaDataID>
        public IPreparationStationRuntime GetPreparationStationRuntime(string preparationStationIdentity)
        {
            IPreparationStationRuntime preparationStationRuntime = null;

            if (!this.PreparationStationRuntimes.TryGetValue(preparationStationIdentity, out preparationStationRuntime))
            {

                var objectStorage = ObjectStorage.GetStorageOfObject(this);
                OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                var preparationStation = (from aPreparationStation in servicesContextStorage.GetObjectCollection<PreparationStation>()
                                          where aPreparationStation.PreparationStationIdentity == preparationStationIdentity
                                          select aPreparationStation).ToList().FirstOrDefault();
                if (preparationStation != null)
                {

                    this.PreparationStationRuntimes[preparationStationIdentity] = preparationStation;
                }
            }
            //7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad
            return this.PreparationStationRuntimes[preparationStationIdentity];
        }


        /// <MetaDataID>{16f0ecc7-44cf-47b9-b628-c6ddbcc99d60}</MetaDataID>
        public ICashiersStationRuntime GetCashiersStationRuntime(string communicationCredentialKey)
        {

            return CashierStations.OfType<CashierStation>().Where(x => x.CashierStationIdentity == communicationCredentialKey).FirstOrDefault();

        }





        /// <MetaDataID>{e3638d03-da40-44de-a45b-5aa5953904d8}</MetaDataID>
        public OrganizationStorageRef GetHallLayoutStorageForServiceArea(IServiceArea serviceArea)
        {
            FlavourBusinessStorage fbstorage = null;
            if (!string.IsNullOrWhiteSpace((serviceArea as ServicesContextResources.ServiceArea).HallLayoutUri))
            {
                IHallLayout hallLayout = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri((serviceArea as ServiceArea).HallLayoutUri) as IHallLayout;
                var storageMetadata = ObjectStorage.GetStorageFromUri((serviceArea as ServiceArea).HallLayoutUri);
                fbstorage = _Storages.Where(x => x.StorageIdentity == storageMetadata.StorageIdentity).FirstOrDefault();
                var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);
                var storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };

                return storageRef;
            }
            else
            {
                string blobUrl = "usersfolder/" + OrganizationIdentity + "/" + serviceArea.Description.Replace(" ", "-") + ".xml";
                RawStorageCloudBlob rawStorage = new RawStorageCloudBlob(new XDocument(), blobUrl);
                RestaurantHallLayoutModel.HallLayout restaurantHallLayout = null;
                ObjectStorage hallLayoutStorage = null;
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    hallLayoutStorage = ObjectStorage.NewStorage(serviceArea.Description.Replace(" ", "-"),
                                    rawStorage,
                                    "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    fbstorage = new FlavourBusinessStorage();
                    fbstorage.StorageIdentity = hallLayoutStorage.StorageMetaData.StorageIdentity;
                    fbstorage.Name = hallLayoutStorage.StorageMetaData.StorageName;
                    fbstorage.Url = blobUrl;
                    fbstorage.FlavourStorageType = OrganizationStorages.HallLayout;
                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                    _Storages.Add(fbstorage);

                    restaurantHallLayout = new RestaurantHallLayoutModel.HallLayout() { Name = (serviceArea as ServicesContextResources.ServiceArea).Description };
                    restaurantHallLayout.ServicesContextIdentity = ServicesContextIdentity;
                    hallLayoutStorage.CommitTransientObjectState(restaurantHallLayout);


                    stateTransition.Consistent = true;
                }

                (serviceArea as ServicesContextResources.ServiceArea).HallLayoutUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(restaurantHallLayout).GetPersistentObjectUri(restaurantHallLayout);

                var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;


                var task = System.Threading.Tasks.Task.Run(async () =>
                {
                    string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
                    System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                    var storagesClient = new StoragesClient(httpClient);
                    storagesClient.BaseUrl = serverUrl;

                    var storageMetaData = new OOAdvantech.MetaDataRepository.StorageMetaData()
                    {
                        StorageName = hallLayoutStorage.StorageMetaData.StorageName,
                        StorageLocation = storageUrl,
                        StorageType = hallLayoutStorage.StorageMetaData.StorageType,
                        StorageIdentity = hallLayoutStorage.StorageMetaData.StorageIdentity,
                        MultipleObjectContext = true
                    };
                    string res = await storagesClient.PostAsync(storageMetaData);
                });
                task.Wait();
                if (task.Exception != null)
                    throw task.Exception;

                var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);
                var storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };
                return storageRef;
            }
        }

        /// <MetaDataID>{ea8f665e-e692-47eb-8725-42453ae08ae6}</MetaDataID>
        public IUploadSlot GetUploadSlotFor(OrganizationStorageRef storageRef)
        {
            var fbStorage = (from storage in this._Storages
                             where storage.StorageIdentity == storageRef.StorageIdentity && storage.FlavourStorageType == storage.FlavourStorageType
                             select storage).FirstOrDefault();

            if (fbStorage == null)
                return null;
            else
            {
                string blobUrl = fbStorage.Url;
                var uploadSlot = new FlavourBusinessToolKit.UploadSlot(blobUrl, FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);
                uploadSlot.FileUploaded += UploadSlot_FileUploaded;
                uploadSlot.Tag = fbStorage;
                return uploadSlot;
            }
        }


        /// <MetaDataID>{a4d1bc7c-fb23-4dbf-9b5b-bbfa69df46c4}</MetaDataID>
        private void UploadSlot_FileUploaded(object sender, EventArgs e)
        {
            (sender as FlavourBusinessToolKit.UploadSlot).FileUploaded -= UploadSlot_FileUploaded;

            FlavourBusinessStorage storageRef = (sender as FlavourBusinessToolKit.UploadSlot).Tag as FlavourBusinessStorage;
            ObjectStorage.UpdateOperativeOperativeObjects(storageRef.StorageIdentity);


        }
        /// <MetaDataID>{cfaf6fae-af4a-4ceb-a682-c39bcef3ec8c}</MetaDataID>
        public void StorageDataUpdated(string storageIdentity)
        {
            ObjectStorage.UpdateOperativeOperativeObjects(storageIdentity);
        }


        /// <MetaDataID>{cc310316-0d1f-477a-a360-e7fdc5f55a79}</MetaDataID>
        public IHallLayout GetHallLayout(string servicePointIdentity)
        {

            return (from hall in Halls.OfType<RestaurantHallLayoutModel.HallLayout>()
                    from shape in hall.Shapes
                    where shape.ServicesPointIdentity == servicePointIdentity
                    select hall).FirstOrDefault();

            //var objectStorage = ObjectStorage.GetStorageOfObject(this);

            //OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

            //var servicesContextIdentity = ServicesContextIdentity;
            //var serviceArea = (from aServiceArea in servicesContextStorage.GetObjectCollection<ServicesContextResources.ServiceArea>()
            //                   from aServicePoint in aServiceArea.ServicePoints
            //                   where aServicePoint.ServicesPointIdentity == servicePointIdentity
            //                   select aServiceArea).FirstOrDefault();

            //return Halls.Where(x => x.HallLayoutUri == serviceArea.HallLayoutUri).FirstOrDefault();
            //if (!string.IsNullOrWhiteSpace(serviceArea.HallLayoutUri))
            //{
            //    IHallLayout hallLayout = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(serviceArea.HallLayoutUri) as IHallLayout;
            //    return hallLayout;
            //}
            //else
            //    return null;

        }

        /// <MetaDataID>{92daab09-4528-4610-9317-370461819661}</MetaDataID>
        public string NewSupervisor()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);


            //OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
            var servicesContextIdentity = ServicesContextIdentity;


            lock (SupervisorsLock)
            {
                var unassignedSupervisor = (from supervisor in Supervisors
                                            select supervisor).ToList().Where((x => string.IsNullOrWhiteSpace(x.SignUpUserIdentity))).FirstOrDefault();
                string supervisorAssignKey = "";

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    if (unassignedSupervisor == null)
                    {
                        unassignedSupervisor = new HumanResources.ServiceContextSupervisor
                        {
                            ServicesContextIdentity = servicesContextIdentity,
                            Name = Properties.Resources.DefaultSupervisorName
                        };

                        objectStorage.CommitTransientObjectState(unassignedSupervisor);
                        if (_Supervisors != null)
                        {
                            _Supervisors.Add(unassignedSupervisor);
                            unassignedSupervisor.ObjectChangeState += Supervisor_ObjectChangeState;
                        }
                    }
                    supervisorAssignKey = servicesContextIdentity + ";" + unassignedSupervisor.Identity + ";" + Guid.NewGuid().ToString("N");

                    unassignedSupervisor.SupervisorAssignKey = supervisorAssignKey;
                    stateTransition.Consistent = true;
                }

                return supervisorAssignKey;
            }
        }

        /// <MetaDataID>{e2138a17-29fe-4c32-9093-b0ffe39fc9e8}</MetaDataID>
        public IServiceContextSupervisor AssignSupervisorUser(string supervisorAssignKey, string signUpUserIdentity, string userName)
        {
            lock (SupervisorsLock)
            {
                var unassignedSupervisor = (from supervisor in Supervisors
                                            where supervisor.SupervisorAssignKey == supervisorAssignKey
                                            select supervisor).FirstOrDefault();

                if (unassignedSupervisor != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        unassignedSupervisor.SupervisorAssignKey = null;
                        unassignedSupervisor.SignUpUserIdentity = signUpUserIdentity;
                        unassignedSupervisor.Name = userName;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                }

                return unassignedSupervisor;
            }
        }


        /// <MetaDataID>{234a4e8e-d5b5-4a98-9372-8719ced1568e}</MetaDataID>
        public void ChangeSiftWork(IShiftWork shiftWork, DateTime startedAt, double timespanInHours)
        {
            shiftWork.Worker.ChangeSiftWork(shiftWork, startedAt, timespanInHours);
        }

        /// <MetaDataID>{ad553593-59ac-4542-944d-b02f1ee008ac}</MetaDataID>
        public IShiftWork NewShifWork(IServicesContextWorker worker, System.DateTime startedAt, double timespanInHours)
        {
            return worker.NewShiftWork(startedAt, timespanInHours);
        }

        /// <MetaDataID>{269aa8e6-0fb9-4925-9d4e-7eba3cdcf288}</MetaDataID>
        public IWaiter AssignWaiterUser(string waiterAssignKey, string signUpUserIdentity, string userName)
        {
            lock (SupervisorsLock)
            {
                var unassignedWaiter = (from waiter in Waiters
                                        where waiter.WaiterAssignKey == waiterAssignKey
                                        select waiter).FirstOrDefault();

                if (unassignedWaiter != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        unassignedWaiter.WaiterAssignKey = null;
                        unassignedWaiter.SignUpUserIdentity = signUpUserIdentity;
                        unassignedWaiter.Name = userName;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                }

                return unassignedWaiter;
            }
        }

        /// <MetaDataID>{bc4d31f4-b44e-4d59-845e-582531cd8584}</MetaDataID>
        public bool IsGraphicMenuAssigned(string storageIdentity)
        {
            var fbstorage = (from storage in _Storages
                             where storage.StorageIdentity == storageIdentity
                             select storage).FirstOrDefault();

            return fbstorage != null;
        }


        /// <MetaDataID>{0c807eee-86a7-4c7d-b523-0ee2767e2062}</MetaDataID>
        public FinanceFacade.IFisicalParty NewFisicalParty()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);
            var fisicalParty = new FisicalParty();
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                fisicalParty.Name = Properties.Resources.DefaultFisicalPartName;
                objectStorage.CommitTransientObjectState(fisicalParty);
                stateTransition.Consistent = true;
            }
            return fisicalParty;
        }

        /// <MetaDataID>{f4b6e660-beb0-4236-828b-7836a4ab1966}</MetaDataID>
        public void RemoveFisicalParty(FinanceFacade.IFisicalParty fisicalParty)
        {
            ObjectStorage.DeleteObject(fisicalParty);
        }
        /// <MetaDataID>{d53bbd80-aea0-44bb-bc30-903daffa974f}</MetaDataID>
        public void UpdateFisicalParty(FinanceFacade.IFisicalParty fisicalParty)
        {
            ObjectStorage.GetObjectFromUri<FisicalParty>((fisicalParty as FisicalParty).FisicalPartyUri).Update(fisicalParty as FisicalParty);
        }
        static Random _R = new Random();

        Task SimulationTask;

        bool EndOfSimulation = false;

        static List<List<PSItemsPattern>> PreparationStationSimulatorItems = new List<List<PSItemsPattern>>() {
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 1 }, new PSItemsPattern() { NumberOfItems = 2 }, new PSItemsPattern() {NumberOfItems= 0 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() {NumberOfItems= 2 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 2 }, new PSItemsPattern() { NumberOfItems = 1 }, new PSItemsPattern() {NumberOfItems= 0 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 3 }, new PSItemsPattern() {NumberOfItems= 0 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 2 }, new PSItemsPattern() { NumberOfItems = 1 }, new PSItemsPattern() {NumberOfItems= 1 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 2 }, new PSItemsPattern() {NumberOfItems= 1 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 2 }, new PSItemsPattern() { NumberOfItems = 1 }, new PSItemsPattern() {NumberOfItems= 1 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 2 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() {NumberOfItems= 1 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 3 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() {NumberOfItems= 1 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 5 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() {NumberOfItems= 0 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 1 }, new PSItemsPattern() {NumberOfItems= 2 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 3 }, new PSItemsPattern() {NumberOfItems= 0 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() {NumberOfItems= 3 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 2 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() {NumberOfItems= 0 } },
            new List<PSItemsPattern> { new PSItemsPattern() {NumberOfItems= 0 } , new PSItemsPattern() { NumberOfItems = 1 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() {NumberOfItems= 2 } },

        };

        /// <MetaDataID>{8574ee8e-dac2-40f7-acf2-ff4f27bc5f1e}</MetaDataID>
        void StartSimulator()
        {

            
            if (SimulationTask == null || SimulationTask.Status != TaskStatus.Running)
            {
                SimulationTask = Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(60000);
                    DateTime? lastMealCourseAdded = null;

                    var servicePoints = (from serviceArea in ServiceAreas
                                         from ServicePoint in serviceArea.ServicePoints
                                         select ServicePoint).ToList();

                    List<IMenuItem> menuItems = GetMenuItems(this.OperativeRestaurantMenu.RootCategory);

                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(OperativeRestaurantMenu));
                    var twoCoursesMealType = (from fixedMealType in storage.GetObjectCollection<FixedMealType>()
                                              select fixedMealType).ToList().Where(x => x.Courses.Count == 2).FirstOrDefault();
                    //var clientSession = GetClientSession(servicesPointIdentity, null, clientName, clientDeviceID, null, OrganizationIdentity, GraphicMenus, true);

                    var mainMealCourseType = twoCoursesMealType.Courses.Where(x => x.IsDefault).FirstOrDefault();
                    var mainMealCourseMenuItems = menuItems.Where(x => x.PartofMeals.Any(y => y.MealCourseType == mainMealCourseType)).ToList();
                    string mainMealCourseTypeUri = ObjectStorage.GetStorageOfObject(mainMealCourseType).GetPersistentObjectUri(mainMealCourseType);
                    Dictionary<IPreparationStation, List<IMenuItem>> preparationStationsItems = new Dictionary<IPreparationStation, List<IMenuItem>>();




                    foreach (var preparationStation in SimulatorPreparationStations.ToList())
                    {
                        var preparationSationItems = mainMealCourseMenuItems.Where(x => preparationStation.CanPrepareItem(x)).ToList();
                        if (preparationSationItems.Count > 0)
                            preparationStationsItems[preparationStation] = preparationSationItems;
                    }

                    IFoodServiceClientSession clientSession = null;


                    int i = 0;
                    while (!EndOfSimulation && servicePoints.Count > 0)
                    {
                        List<List<PSItemsPattern>> preparationStationSimulatorItems = GetNextPreparationPatern(i++);
                        if (lastMealCourseAdded == null || (DateTime.UtcNow - lastMealCourseAdded.Value).TotalMinutes > 0.6)
                        {
                            servicePoints = servicePoints.Where(x => x.State == ServicePointState.Free).ToList();

                            if (servicePoints.Count > 0 && servicePoints.Where(x => x.State != ServicePointState.Free).Count() <= 2)
                            {

                                string servicesPointIdentity = servicePoints[_R.Next(servicePoints.Count - 1)].ServicesPointIdentity;
                                string clientDeviceID = "S_81000000296";
                                string clientName = "Jimmy Garson";
                                clientSession = simulateClientSession(mainMealCourseTypeUri, preparationStationsItems, preparationStationSimulatorItems, servicesPointIdentity, clientDeviceID, clientName);
                                lastMealCourseAdded = DateTime.UtcNow;
                            }
                            ////DeleteSimulationData();
                        }

                        //PreparationStations[0].
                        //clientSession.FoodServiceClientSession.AddItem
                        System.Threading.Thread.Sleep(10000);

                    }
                });
            }
            else
            {

            }
            //clientDeviceID="S_81000000296"
            //clientName="clientName"

        }


        IList<IPreparationStation> SimulatorPreparationStations
        {
            get
            {
                var simulatorPreparationStations = PreparationStationRuntimes.Values.OrderBy(x => x.Description).OfType<IPreparationStation>().ToList();
                return simulatorPreparationStations;
            }
        }
        private List<List<PSItemsPattern>> GetNextPreparationPatern(int step)
        {
            List<List<PSItemsPattern>> preparationPaterns = new List<List<PSItemsPattern>>();

            if (step == 0)
            {
                preparationPaterns.Add(new List<PSItemsPattern> { new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 0, ItemsPatterns = new List<ItemPattern> { new ItemPattern { MinDuration = 3.5, MaxDuration = 4.5 } } }, new PSItemsPattern() { NumberOfItems = 0, ItemsPatterns = new List<ItemPattern> { new ItemPattern { MinDuration = 8.5, MaxDuration = 10.5 } } } });
                return preparationPaterns;
            }
            if (step == 1)
            {
                preparationPaterns.Add(new List<PSItemsPattern> { new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 0 }, new PSItemsPattern() { NumberOfItems = 0, ItemsPatterns = new List<ItemPattern> { new ItemPattern { MinDuration = 3.5, MaxDuration = 4.5 }, new ItemPattern { MinDuration = 3.5, MaxDuration = 4.5 } } }, new PSItemsPattern() { NumberOfItems = 0 } });
                return preparationPaterns;
            }


            return PreparationStationSimulatorItems;
        }

        private void DeleteSimulationData()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);
            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

            var simulationClientSessions = (from clientSession in servicesContextStorage.GetObjectCollection<FoodServiceClientSession>()
                                            where clientSession.ClientDeviceID == "S_81000000296"
                                            select clientSession).ToList();

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var clientSession in simulationClientSessions)
                {
                    var mainSession = clientSession.MainSession;


                    if (mainSession != null)
                    {
                        ObjectStorage.DeleteObject(mainSession);
                        var meal = mainSession.Meal as Meal;
                        if (meal != null)
                        {
                            meal.StopMonitoring();
                            foreach (var mealCourse in meal.Courses)
                                ObjectStorage.DeleteObject(mealCourse);
                            ObjectStorage.DeleteObject(meal);
                        }

                    }
                    clientSession.ServicePoint.State = ServicePointState.Free;
                    foreach (var itemPreparation in clientSession.FlavourItems)
                        ObjectStorage.DeleteObject(itemPreparation);
                    ObjectStorage.DeleteObject(clientSession);

                }
                stateTransition.Consistent = true;
            }
        }

        private IFoodServiceClientSession simulateClientSession(string mainMealCourseTypeUri, Dictionary<IPreparationStation, List<IMenuItem>> preparationStationsItems, List<List<PSItemsPattern>> preparationStationSimulatorItems, string servicesPointIdentity, string clientDeviceID, string clientName)
        {



            List<IItemPreparation> itemsToPrepare = new List<IItemPreparation>();
            var patern = preparationStationSimulatorItems[_R.Next(preparationStationSimulatorItems.Count - 1)].ToList();



            foreach (var psItemsPattern in patern)
            {
                if (psItemsPattern.NumberOfItems != null)
                {
                    while (psItemsPattern.NumberOfItems > 0)
                    {
                        var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[patern.IndexOf(psItemsPattern)]];
                        var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                        ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
                        itemsToPrepare.Add(itemPreparation);
                        psItemsPattern.NumberOfItems = patern[0].NumberOfItems - 1;
                    }
                }
                if (psItemsPattern.ItemsPatterns != null)
                {
                    foreach (var itemPater in psItemsPattern.ItemsPatterns)
                    {
                        var preparationStation = preparationStationsItems.Keys.ToList()[patern.IndexOf(psItemsPattern)];
                        var preparationStationItems = preparationStationsItems[preparationStation];

                        preparationStationItems = preparationStationItems.Where(x => (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) >= itemPater.MinDuration/2 && (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) <= itemPater.MaxDuration/2).ToList();
                        var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                        ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
                        itemsToPrepare.Add(itemPreparation);
                        psItemsPattern.NumberOfItems = patern[0].NumberOfItems - 1;
                    }
                }


            }

            while (patern[0].NumberOfItems > 0)
            {
                var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[0]];
                var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
                itemsToPrepare.Add(itemPreparation);
                patern[0].NumberOfItems = patern[0].NumberOfItems - 1;
            }


            //while (patern[1].NumberOfItems > 0)
            //{
            //    var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[1]];
            //    var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
            //    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
            //    itemsToPrepare.Add(itemPreparation);
            //    patern[1].NumberOfItems = patern[1].NumberOfItems - 1;
            //}

            //while (patern[2].NumberOfItems > 0)
            //{
            //    var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[2]];
            //    var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
            //    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
            //    itemsToPrepare.Add(itemPreparation);
            //    patern[2].NumberOfItems = patern[2].NumberOfItems - 1;
            //}

            //while (patern[3].NumberOfItems > 0)
            //{
            //    var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[3]];
            //    var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
            //    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
            //    itemsToPrepare.Add(itemPreparation);
            //    patern[3].NumberOfItems = patern[3].NumberOfItems - 1;
            //}
            IFoodServiceClientSession clientSession = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                clientSession = GetClientSession(servicesPointIdentity, null, clientName, clientDeviceID, null, OrganizationIdentity, GraphicMenus, true).FoodServiceClientSession;
                foreach (var itemToPrepare in itemsToPrepare)
                    clientSession.AddItem(itemToPrepare);


                stateTransition.Consistent = true;

            }
            clientSession.Commit(itemsToPrepare);
            return clientSession;

            //   clientSession.FoodServiceClientSession.Commit(itemsToPrepare);
        }

        private List<IMenuItem> GetMenuItems(IItemsCategory rootCategory)
        {
            var menuItems = rootCategory.MenuItems.ToList();
            foreach (var subCategory in rootCategory.SubCategories)
                menuItems.AddRange(GetMenuItems(subCategory));

            return menuItems;
        }
    }

    class PSItemsPattern
    {
        public List<ItemPattern> ItemsPatterns;

        public int? NumberOfItems;

    }

    class ItemPattern
    {
        public double MinDuration;
        public double MaxDuration;
    }
}
