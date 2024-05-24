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

using FlavourBusinessManager.EndUsers;
using FlavourBusinessFacade.HumanResources;
using System.Threading.Tasks;
using FlavourBusinessToolKit;
using System.IO;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService;
using FinanceFacade;
using MenuModel;


//using WebhooksToLocalServer;

using FlavourBusinessManager.HumanResources;

using System.Net.Http;
using OOAdvantech.Remoting.RestApi.Serialization;
using System.Globalization;
using Firebase.Auth.Providers;
using Firebase.Auth;
using OOAdvantech.Remoting.RestApi;


namespace FlavourBusinessManager.ServicePointRunTime
{
    /// <MetaDataID>{f44b4b10-677e-461a-bbc7-bb8b1b62716a}</MetaDataID>
    [BackwardCompatibilityID("{f44b4b10-677e-461a-bbc7-bb8b1b62716a}")]
    [Persistent()]
    public class ServicesContextRunTime : MarshalByRefObject, IExtMarshalByRefObject, IFlavoursServicesContextRuntime, IUploadService
    {

        /// <exclude>Excluded</exclude>
        string _OrganizationStorageIdentity;

        /// <MetaDataID>{2cdb9cf0-e524-4798-8198-a31f6716d6f5}</MetaDataID>
        [PersistentMember(nameof(_OrganizationStorageIdentity))]
        [BackwardCompatibilityID("+8")]
        public string OrganizationStorageIdentity
        {
            get => _OrganizationStorageIdentity;
            set
            {
                if (_OrganizationStorageIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OrganizationStorageIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{bbfa9e26-c08a-44ca-9b67-e4d172ffecb2}</MetaDataID>
        public void RemovePaymentTerminal(IPaymentTerminal paymentTerminal)
        {
            try
            {
                ObjectStorage.DeleteObject(paymentTerminal);
                lock (ServiceContextRTLock)
                {
                    if (_PaymentTerminals != null)
                        _PaymentTerminals.Remove(paymentTerminal);
                }

            }
            catch (Exception error)
            {
                throw;
            }
        }

        /// <MetaDataID>{8e21cb9c-f2f7-4304-81f4-790bdefed874}</MetaDataID>
        public IPaymentTerminal NewPaymentTerminal()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            PaymentTerminal paymentTerminal = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                paymentTerminal = new PaymentTerminal(ServicesContextIdentity);
                paymentTerminal.Description = Properties.Resources.DefaultPaymentTerminalDescription;

                objectStorage.CommitTransientObjectState(paymentTerminal);

                lock (ServiceContextRTLock)
                {
                    _PaymentTerminals.Add(paymentTerminal);
                }


                stateTransition.Consistent = true;
            }
            return paymentTerminal;

        }

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
            if (member == nameof(IWaiter.ShiftWork))
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
                        {
                            waiter.NativeUser = NativeUsers.Where(x => x.OAuthUserIdentity == waiter.OAuthUserIdentity).FirstOrDefault() != null;
                            waiter.ObjectChangeState += Waiter_ObjectChangeState;
                        }
                    }

                    return _Waiters.ToList();
                }

            }
        }

        /// <exclude>Excluded</exclude>
        List<IPaymentTerminal> _PaymentTerminals;

        /// <MetaDataID>{c612de4d-29a7-47c5-9569-4e02a81c5580}</MetaDataID>
        public List<IPaymentTerminal> PaymentTerminals
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_PaymentTerminals == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        _PaymentTerminals = (from takeawayCashier in servicesContextStorage.GetObjectCollection<IPaymentTerminal>()
                                             where takeawayCashier.ServicesContextIdentity == servicesContextIdentity
                                             select takeawayCashier).ToList();

                        foreach (var paymentTerminal in _PaymentTerminals)
                            paymentTerminal.ObjectChangeState += PaymentTerminal_ObjectChangeState;
                    }

                    return _PaymentTerminals.ToList();
                }

            }
        }


        /// <exclude>Excluded</exclude>
        List<ITakeawayCashier> _TakeawayCashiers;
        /// <MetaDataID>{658dd45b-6685-4313-9835-b2bcebc603f3}</MetaDataID>
        public IList<ITakeawayCashier> TakeawayCashiers
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_TakeawayCashiers == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        _TakeawayCashiers = (from takeawayCashier in servicesContextStorage.GetObjectCollection<ITakeawayCashier>()
                                             where takeawayCashier.ServicesContextIdentity == servicesContextIdentity
                                             select takeawayCashier).ToList();

                        foreach (var takeawayCashier in _TakeawayCashiers)
                        {
                            takeawayCashier.NativeUser = NativeUsers.Where(x => x.OAuthUserIdentity == takeawayCashier.OAuthUserIdentity).FirstOrDefault() != null;
                            takeawayCashier.ObjectChangeState += TakeawayCashier_ObjectChangeState;

                        }
                    }

                    return _TakeawayCashiers.ToList();
                }

            }
        }



        /// <exclude>Excluded</exclude>
        List<ICourier> _Couriers;

        public IList<ICourier> Couriers
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_Couriers == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        _Couriers = (from courier in servicesContextStorage.GetObjectCollection<ICourier>()
                                     where courier.ServicesContextIdentity == servicesContextIdentity
                                     select courier).ToList();


                        foreach (var courier in _Couriers)
                        {
                            courier.NativeUser = NativeUsers.Where(x => x.OAuthUserIdentity == courier.OAuthUserIdentity).FirstOrDefault() != null;
                            courier.ObjectChangeState += Courier_ObjectChangeState;
                        }
                    }

                    return _Couriers.ToList();
                }

            }
        }


        /// <MetaDataID>{c543c675-b069-4660-a061-3fbbe7a42b0b}</MetaDataID>
        private void TakeawayCashier_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IWaiter.ShiftWork))
            {
                Task.Run(() =>
                {
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                });
            }
        }

        private void Courier_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IWaiter.ShiftWork))
            {
                Task.Run(() =>
                {
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                });
            }
        }


        /// <MetaDataID>{179a3bed-629f-4a3f-b79a-bbe334fcbed2}</MetaDataID>
        private void PaymentTerminal_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IWaiter.ShiftWork))
            {
                Task.Run(() =>
                {
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                });
            }
        }
        /// <MetaDataID>{e9759669-6bd8-4091-b318-90229918434e}</MetaDataID>
        private void Waiter_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IWaiter.ShiftWork))
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
                                        select waiter).ToList().Where((x => string.IsNullOrWhiteSpace(x.OAuthUserIdentity))).FirstOrDefault();
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

                    unassignedWaiter.WorkerAssignKey = WaiterAssignKey;
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


        /// <MetaDataID>{0fe4559a-d8a3-412d-b711-5cc3bb020bd0}</MetaDataID>
        public string NewTakeAwayCashier()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            //OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
            var servicesContextIdentity = ServicesContextIdentity;

            lock (SupervisorsLock)
            {
                //unassignedtakeawayCashier
                var unassignedtakeawayCashier = (from takeawayCashier in TakeawayCashiers
                                                 select takeawayCashier).ToList().Where((x => string.IsNullOrWhiteSpace(x.OAuthUserIdentity))).FirstOrDefault();
                string takeawayCashierKey = "";

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    if (unassignedtakeawayCashier == null)
                    {
                        unassignedtakeawayCashier = new TakeawayCashier();
                        unassignedtakeawayCashier.ServicesContextIdentity = servicesContextIdentity;
                        unassignedtakeawayCashier.Name = Properties.Resources.DefaultWaiterName;

                        objectStorage.CommitTransientObjectState(unassignedtakeawayCashier);
                        lock (ServiceContextRTLock)
                        {
                            _TakeawayCashiers.Add(unassignedtakeawayCashier);
                        }
                    }
                    takeawayCashierKey = servicesContextIdentity + ";" + unassignedtakeawayCashier.Identity + ";" + Guid.NewGuid().ToString("N");

                    unassignedtakeawayCashier.WorkerAssignKey = takeawayCashierKey;
                    stateTransition.Consistent = true;
                }

                return takeawayCashierKey;
            }
        }


        /// <MetaDataID>{d11f6e72-b8ca-4588-9d09-6f5b3f984e09}</MetaDataID>
        public string NewCourier()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            //OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
            var servicesContextIdentity = ServicesContextIdentity;

            lock (SupervisorsLock)
            {
                //unassignedtakeawayCashier
                var unassignedCourier = (from courier in Couriers
                                         select courier).ToList().Where((x => string.IsNullOrWhiteSpace(x.OAuthUserIdentity))).FirstOrDefault();
                string courierKey = "";

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    if (unassignedCourier == null)
                    {
                        unassignedCourier = new Courier();
                        unassignedCourier.ServicesContextIdentity = servicesContextIdentity;
                        unassignedCourier.Name = Properties.Resources.DefaultWaiterName;

                        objectStorage.CommitTransientObjectState(unassignedCourier);
                        lock (ServiceContextRTLock)
                        {
                            _Couriers.Add(unassignedCourier);
                        }
                    }
                    courierKey = servicesContextIdentity + ";" + unassignedCourier.Identity + ";" + Guid.NewGuid().ToString("N");

                    unassignedCourier.WorkerAssignKey = courierKey;
                    stateTransition.Consistent = true;
                }

                return courierKey;
            }
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
                lock (this)
                {
                    return _OperativeRestaurantMenu;
                }
            }
        }

        /// <MetaDataID>{41576ebd-3865-4161-988d-071e26c5b296}</MetaDataID>
        internal Simulator Simulator = new Simulator();

        /// <MetaDataID>{269a82d7-69e7-4e6c-9832-c50e7a1fa8b2}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {
            lock (this)
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
            }
            bool del = false;

            if (del)
                Simulator.DeleteSimulationData();

            SessionsMonitoringTimer.Start();
            SessionsMonitoringTimer.Elapsed += SessionsMonitoringTimer_Elapsed;
            SessionsMonitoringTimer.AutoReset = false;

            LoadPaymentProviders();

            //foreach(var twsp in TakeAwayStations )
            //{
            //    if(twsp.ServesMealTypes.Count==0)
            //    {
            //        twsp.AddMealType(GetOneCoursesMealType().MealTypeUri);

            //    }
            //}

            Task.Run(() =>
            {

                (MealsController as MealsController).Init();
                //Load CashierStations
                var cashierStations = CashierStations;

            });


#if DEBUG
            try
            {
                Webhookservice = new WebhooksToLocalServer.Webhookservice();
                Webhookservice.Start(this.ServicesContextIdentity);
            }
            catch (Exception error)
            {


            }
#endif

            // Firebase UI initialization
            var config = new Firebase.Auth.FirebaseAuthConfig
            {
                ApiKey = "AIzaSyD8rMRJMQaDZob0bW4QFY2rOxW2s6D2a1Q",
                AuthDomain = "demomicroneme.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                   {
                    new GoogleProvider(),
                    new FacebookProvider(),
                    new AppleProvider(),
                    new TwitterProvider(),
                    new GithubProvider(),
                    new MicrosoftProvider(),
                    new EmailProvider()
                   },
                //PrivacyPolicyUrl = "https://github.com/step-up-labs/firebase-authentication-dotnet",
                //TermsOfServiceUrl = "https://github.com/step-up-labs/firebase-database-dotnet",
                //IsAnonymousAllowed = true,
                //AutoUpgradeAnonymousUsers = true,
                //UserRepository = new StorageRepository(),
                // Func called when upgrade of anonymous user fails because the user already exists
                // You should grab any data created under your anonymous user, sign in with the pending credential
                // and copy the existing data to the new user
                // see details here: https://github.com/firebase/firebaseui-web#upgrading-anonymous-users
                //AnonymousUpgradeConflict = conflict => conflict.SignInWithPendingCredentialAsync(true)
            };

            FireBaseClient = new FirebaseAuthClient(config);
            //Task.Run(async () =>
            //{
            //    var User = await FireBaseClient.SignInWithEmailAndPasswordAsync("jim.liakos@hotmail.com", "astraxan");
            //    AuthUser authUser = AuthUser.GetAuthUserFromToken(User.User.Credential.IdToken);
            //    AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);



            //});

            //Task.Run(() =>
            //{
            //    var trt = GetPreparationStationRuntime("7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad").GetPreparationItems(new List<ItemPreparationAbbreviation>(), null);

            //});

            //Task.Run(() =>
            //{
            //    var trt = GetPreparationStationRuntime("7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad").GetPreparationItems(new List<ItemPreparationAbbreviation>(), null);

            //});

        }

        /// <MetaDataID>{cc75d0bf-1dda-498a-b80c-fa68b7245b42}</MetaDataID>
        private void LoadPaymentProviders()
        {
            var vivaProvider = new PaymentProviders.VivaWallet();
            if (Payment.GetPaymentProvider("Viva") == null)
                Payment.SetPaymentProvider("Viva", vivaProvider);
            if (Payment.GetPaymentProvider("VivaPayment") == null)
                Payment.SetPaymentProvider("VivaPayment", vivaProvider);
            Payment.PaymentFinder = new ServiceContextPaymentFinder();





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

                Simulator.StartSimulator(this);
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

                RemindWorkersForUnreadedMessages();

            }
            catch (Exception error)
            {
            }
            SessionsMonitoringTimer.Start();
        }

        private void RemindWorkersForUnreadedMessages()
        {
            List<IMessageConsumer> workersWithUnreadedMessages = null;
            lock (ServiceContextRTLock)
            {
                workersWithUnreadedMessages = WaitersWithUnreadedMessages.OfType<IMessageConsumer>().ToList();
                workersWithUnreadedMessages.AddRange(CouriersWithUnreadedMessages.OfType<IMessageConsumer>().ToList());
                workersWithUnreadedMessages.AddRange(SupervisorsWithUnreadedMessages.OfType<IMessageConsumer>().ToList());
            }

            foreach (var worker in workersWithUnreadedMessages)
            {
                var workerlayMessages = worker.Messages.Where(x => x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable &&
                                                    !x.MessageReaded && x.NotificationsNum <= 5).ToList();
                if (workerlayMessages.Count > 0)
                {
                    string deviceFirebaseToken = null;
                    if (worker is IWaiter)
                        deviceFirebaseToken = (worker as IWaiter).DeviceFirebaseToken;
                    if (worker is ICourier)
                        deviceFirebaseToken = (worker as ICourier).DeviceFirebaseToken;

                    var layMessage = workerlayMessages[0];
                    if (!string.IsNullOrWhiteSpace(deviceFirebaseToken))
                    {
                        var last = workerlayMessages.Where(x => !x.MessageReaded).OrderByDescending(x => x.NotificationTimestamp).Last();
                        if (System.DateTime.UtcNow - last.NotificationTimestamp.ToUniversalTime() > TimeSpan.FromMinutes(2))
                        {
                            CloudNotificationManager.SendMessage(layMessage, deviceFirebaseToken);
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {

                                foreach (var message in workerlayMessages.Where(x => !x.MessageReaded))
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


                var workerServingMessages = worker.Messages.Where(x => (x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe || x.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.DelayedMealAtTheCounter) &&
                                                !x.MessageReaded && x.NotificationsNum <= 5).ToList();
                if (workerServingMessages.Count > 0)
                {
                    string deviceFirebaseToken = null;
                    if (worker is IWaiter)
                        deviceFirebaseToken = (worker as IWaiter).DeviceFirebaseToken;
                    if (worker is ICourier)
                        deviceFirebaseToken = (worker as ICourier).DeviceFirebaseToken;

                    if (worker is IServiceContextSupervisor)
                        deviceFirebaseToken = (worker as IServiceContextSupervisor).DeviceFirebaseToken;


                    var servingMessage = workerServingMessages[0];
                    if (!string.IsNullOrWhiteSpace(deviceFirebaseToken))
                    {
                        var last = workerServingMessages.Where(x => !x.MessageReaded).OrderByDescending(x => x.NotificationTimestamp).Last();
                        if (System.DateTime.UtcNow - last.NotificationTimestamp.ToUniversalTime() > TimeSpan.FromMinutes(2))
                        {
                            CloudNotificationManager.SendMessage(servingMessage, deviceFirebaseToken);
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {

                                foreach (var message in workerServingMessages.Where(x => !x.MessageReaded))
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

        /// <MetaDataID>{1f046455-d3b7-4545-a1ea-1aebda104e94}</MetaDataID>
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
            lock (ServiceContextRTLock)
            {
                if (waiter.ShiftWork != null && !ActiveShiftWorks.Contains(waiter.ShiftWork))
                    ActiveShiftWorks.Add(waiter.ShiftWork);
            }

            var activeShiftWorks = GetActiveShiftWorks();
        }


        /// <MetaDataID>{9cae1cbf-00a7-48df-8b4b-e7fd8ad6177f}</MetaDataID>
        internal void CashierSiftWorkUpdated(HumanResources.TakeawayCashier cashier)
        {
            lock (ServiceContextRTLock)
            {
                if (cashier.ShiftWork != null && !ActiveShiftWorks.Contains(cashier.ShiftWork))
                    ActiveShiftWorks.Add(cashier.ShiftWork);
            }

            var activeShiftWorks = GetActiveShiftWorks();
        }

        internal void CourierSiftWorkUpdated(Courier courier)
        {
            lock (ServiceContextRTLock)
            {
                if (courier.ShiftWork != null && !ActiveShiftWorks.Contains(courier.ShiftWork))
                    ActiveShiftWorks.Add(courier.ShiftWork);
            }

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

                        if (DeliveryServicePoint != null)
                            DeliveryServicePoint.Description = Description;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Description));
                }


            }
        }

        /// <MetaDataID>{d71eaa49-3fce-4cf4-8cf6-bad7d22a3bb6}</MetaDataID>
        internal void MealConversationTimeout(ServicePoint servicePoint, string sessionIdentity, List<Caregiver> caregivers)
        {
            var activeWaiters = caregivers.Where(x => x.Worker is HumanResources.Waiter && x.CareGiving == Caregiver.CareGivingType.ConversationCheck).Select(x => x.Worker as HumanResources.Waiter).ToList();
            if (activeWaiters.Count == 0 && servicePoint is HallServicePoint)
                activeWaiters = (from shiftWork in GetActiveShiftWorks()
                                 where shiftWork.Worker is IWaiter && (servicePoint as HallServicePoint).CanBeAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                 select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();


            foreach (var waiter in activeWaiters)
            {
                var waiterActiveShiftWork = waiter.ShiftWork;
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
                        this.UpdateWaitersWithUnreadedMessages();


                    }

                }
                //}
            }

        }


        List<HumanResources.Waiter> _WaitersWithUnreadedMessages;

        /// <MetaDataID>{c1f668e1-a630-46cf-9797-3569d2e193b2}</MetaDataID>
        List<HumanResources.Waiter> WaitersWithUnreadedMessages
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_WaitersWithUnreadedMessages == null)
                        UpdateWaitersWithUnreadedMessages();
                    return _WaitersWithUnreadedMessages;
                }
            }
        }

        internal void UpdateWaitersWithUnreadedMessages()
        {
            lock (ServiceContextRTLock)
            {

                var activeWaiters = (from shiftWork in GetActiveShiftWorks()
                                     where shiftWork.Worker is IWaiter
                                     select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

                _WaitersWithUnreadedMessages = (from activeWaiter in activeWaiters
                                                from message in activeWaiter.Messages
                                                where
                                                (message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable ||
                                                message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MealConversationTimeout ||
                                                message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe
                                                )
                                                && !message.MessageReaded
                                                select activeWaiter).ToList();


            }
        }



        List<Courier> _CouriersWithUnreadedMessages;


        List<Courier> CouriersWithUnreadedMessages
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_CouriersWithUnreadedMessages == null)
                        UpdateCouriersWithUnreadedMessages();
                    return _CouriersWithUnreadedMessages;
                }
            }
        }

        internal void UpdateCouriersWithUnreadedMessages()
        {
            lock (ServiceContextRTLock)
            {

                var activeCouriers = (from shiftWork in GetActiveShiftWorks()
                                      where shiftWork.Worker is ICourier
                                      select shiftWork.Worker).OfType<Courier>().ToList();

                _CouriersWithUnreadedMessages = (from activeCourier in activeCouriers
                                                 from message in activeCourier.Messages
                                                 where message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe

                                                 && !message.MessageReaded
                                                 select activeCourier).ToList();
            }
        }

        List<ServiceContextSupervisor> _SupervisorsWithUnreadedMessages;


        List<ServiceContextSupervisor> SupervisorsWithUnreadedMessages
        {
            get
            {
                lock (ServiceContextRTLock)
                {
                    if (_SupervisorsWithUnreadedMessages == null)
                        UpdateSupervisorsWithUnreadedMessages();
                    return _SupervisorsWithUnreadedMessages;
                }
            }
        }

        internal void UpdateSupervisorsWithUnreadedMessages()
        {
            lock (ServiceContextRTLock)
            {

                var activeSupervisors = (from shiftWork in GetActiveShiftWorks()
                                         where shiftWork.Worker is IServiceContextSupervisor
                                         select shiftWork.Worker).OfType<ServiceContextSupervisor>().ToList();

                _SupervisorsWithUnreadedMessages = (from activeCourier in activeSupervisors
                                                    from message in activeCourier.Messages
                                                    where message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.DelayedMealAtTheCounter
                                                    && !message.MessageReaded
                                                    select activeCourier).ToList();
            }
        }





        /// <MetaDataID>{a8d06287-3ab2-4a1a-8858-e36d8822fcb9}</MetaDataID>
        internal void ServicePointChangeState(ServicePoint servicePoint, ServicePointState oldState, ServicePointState newState)
        {
            if (newState == ServicePointState.Laying && servicePoint is HallServicePoint)
            {
                //if (servicePoint.OpenClientSessions.Where(x => !x.IsWaiterSession).FirstOrDefault() != null)
                //{
                var activeWaiters = (from shiftWork in GetActiveShiftWorks()
                                     where shiftWork.Worker is IWaiter && (servicePoint as HallServicePoint).CanBeAssignedTo(shiftWork.Worker as IWaiter, shiftWork)
                                     select shiftWork.Worker).OfType<HumanResources.Waiter>().ToList();

                foreach (var waiter in activeWaiters)
                {
                    var waiterActiveShiftWork = waiter.ShiftWork;
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
                            var waitersWithUnreadedMessages = (from activeWaiter in activeWaiters
                                                               from message in activeWaiter.Messages
                                                               where message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable && !message.MessageReaded
                                                               select activeWaiter).ToList();
                        }
                    }
                    //}
                }
                this.UpdateWaitersWithUnreadedMessages();
            }
        }




        /// <exclude>Excluded</exclude>
        FlavoursServicesContext _FlavoursServicesContext;

        /// <MetaDataID>{3787eccb-d3dd-49cf-890a-0acad8f729df}</MetaDataID>
        internal FlavoursServicesContext FlavoursServicesContext
        {
            get
            {
                if (_FlavoursServicesContext != null)
                    return _FlavoursServicesContext;
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(FlavoursServicesContext.OpenFlavourBusinessesStorage());

                _FlavoursServicesContext = (from flavoursServicesContext in storage.GetObjectCollection<FlavoursServicesContext>()
                                            where flavoursServicesContext.ServicesContextIdentity == ServicesContextIdentity
                                            select flavoursServicesContext).FirstOrDefault();

                return _FlavoursServicesContext;
            }
        }

        /// <MetaDataID>{8828ea17-8bb9-42c6-b5e1-36270c0ae343}</MetaDataID>
        public IList<IHallLayout> Halls
        {
            get
            {
                List<IHallLayout> halls = new System.Collections.Generic.List<FlavourBusinessFacade.ServicesContextResources.IHallLayout>();

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));

                //var servicesContextRunTime = (from theServicePointRun in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                //                              where theServicePointRun.ServicesContextIdentity == ServicesContextIdentity
                //                              select theServicePointRun).FirstOrDefault();

                //(from aServiceArea in storage.GetObjectCollection<ServicesContextResources.ServiceArea>()
                // select aServiceArea))
                foreach (var serviceArea in ServiceAreas.OfType<ServiceArea>())
                {
                    if (!string.IsNullOrWhiteSpace(serviceArea.HallLayoutUri))
                    {
                        RestaurantHallLayoutModel.HallLayout hallLayout = ObjectStorage.GetObjectFromUri(serviceArea.HallLayoutUri) as RestaurantHallLayoutModel.HallLayout;


                        if (hallLayout == null)
                            continue;

                        hallLayout.ServiceArea = serviceArea;

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var servicePointShape in hallLayout.Shapes.Where(x => !string.IsNullOrWhiteSpace(x.ServicesPointIdentity)))
                            {
                                var servicePoint = hallLayout.ServiceArea.ServicePoints.Where(x => x.ServicesPointIdentity == servicePointShape.ServicesPointIdentity).FirstOrDefault();
                                if (servicePoint != null)
                                {

                                    if (servicePoint.State != ServicePointState.Free && OpenClientSessions.Where(x => x.ServicePoint.ServicesPointIdentity == servicePoint.ServicesContextIdentity).Count() == 0)
                                        servicePoint.State = ServicePointState.Free;
                                    servicePointShape.ServicesPointState = servicePoint.State;
                                }
                            }

                            stateTransition.Consistent = true;
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
                    try
                    {
                        var storageUrl = urlRoot + fbStorage.Url;
                        var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbStorage.Url);

                        OrganizationStorageRef storageRef = new OrganizationStorageRef { StorageIdentity = fbStorage.StorageIdentity, FlavourStorageType = fbStorage.FlavourStorageType, Name = fbStorage.Name, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime, Version = fbStorage.Version, PropertiesValues = fbStorage.PropertiesValues };
                        graphicMenusStorages.Add(storageRef);
                    }
                    catch (Exception error)
                    {
                    }
                }

                return graphicMenusStorages;
            }
        }


        public void AssignPriceList(OrganizationStorageRef priceListStorageRef)
        {

            if ((from storage in _Storages
                 where storage.StorageIdentity == priceListStorageRef.StorageIdentity
                 select storage).Count() == 0)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    var fbstorage = new FlavourBusinessStorage();
                    fbstorage.StorageIdentity = priceListStorageRef.StorageIdentity;
                    fbstorage.Name = priceListStorageRef.Name;

                    int npos = priceListStorageRef.StorageUrl.IndexOf("usersfolder/");
                    if (npos != -1)
                        fbstorage.Url = priceListStorageRef.StorageUrl.Substring(npos);
                    else
                        fbstorage.Url = priceListStorageRef.StorageUrl;
                    fbstorage.FlavourStorageType = OrganizationStorages.PriceList;
                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(fbstorage);
                    _Storages.Add(fbstorage);

                    //GraphicMenuStorageMetaDataUpdated(priceListStorageRef);
                    stateTransition.Consistent = true;
                }
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

                    GraphicMenuStorageMetaDataUpdated(graphicMenuStorageRef);
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
            string previousVersionJsonFileName = previousVersionServerStorageFolder + restaurantMenusData.Name + ".json";
            if (WritePublicRestaurantMenuDataIfChanged(jsonFileName, previousVersionJsonFileName))
            {

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
            }



            //jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
            //var sss = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.IMenuItem>>(jsonEx, jSetttings);
        }
        /// <MetaDataID>{09d076c1-518e-4c4c-8c94-ad05f45ab97a}</MetaDataID>
        private bool WritePublicRestaurantMenuDataIfChanged(string jsonFileName, string previousVersionJsonFileName)
        {
            IFileManager fileManager = new BlobFileManager(FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);



            var objectStorage = ObjectStorage.GetStorageOfObject(OperativeRestaurantMenu);
            OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(objectStorage);

            Dictionary<object, object> mappedObject = new Dictionary<object, object>();
            List<MenuModel.IMenuItem> menuFoodItems = (from menuItem in restMenusData.GetObjectCollection<MenuModel.IMenuItem>()
                                                       select menuItem).ToList().Select(x => new MenuModel.JsonViewModel.MenuFoodItem(x, mappedObject)).OfType<MenuModel.IMenuItem>().ToList();

            var meneuItemsNames = menuFoodItems.Select(X => X.Name).ToList();

            var jSettings = JsonSerializerSettings.TypeRefSerializeSettings;
            string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSettings);

            Stream previousVersionJsonStream = fileManager.GetBlobStream(previousVersionJsonFileName);

            byte[] buffer = new byte[previousVersionJsonStream.Length];
            previousVersionJsonStream.Position = 0;
            previousVersionJsonStream.Read(buffer, 0, (int)previousVersionJsonStream.Length);
            previousVersionJsonStream.Close();
            string oldJsonEx = System.Text.Encoding.UTF8.GetString(buffer);

            if (oldJsonEx.Length == jsonEx.Length && oldJsonEx == jsonEx)
                return false;


            MemoryStream jsonRestaurantMenuStream = new MemoryStream();
            byte[] jsonBuffer = System.Text.Encoding.UTF8.GetBytes(jsonEx);
            jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
            jsonRestaurantMenuStream.Position = 0;

            if (fileManager != null)
                fileManager.Upload(jsonFileName, jsonRestaurantMenuStream, "application/json");

            return true;


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


        public void RemovePriceList(OrganizationStorageRef priceListStorageRef)
        {
            var fbstorage = (from storage in _Storages
                             where storage.StorageIdentity == priceListStorageRef.StorageIdentity
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
        public void StorageMetaDataUpdated(OrganizationStorageRef storageRef)
        {
            if (storageRef.FlavourStorageType == OrganizationStorages.GraphicMenu)
            {
                GraphicMenuStorageMetaDataUpdated(storageRef);
            }
            else
            {
                var sc_storageRef = _Storages.Where(x => x.StorageIdentity == storageRef.StorageIdentity).FirstOrDefault();

                if (sc_storageRef != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        // sc_storageRef.Url = storageRef.StorageUrl;
                        sc_storageRef.Name = storageRef.Name;
                        sc_storageRef.Version= storageRef.Version;

                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{e57f4255-89a4-44e4-8b44-c9688dab616b}</MetaDataID>
        void GraphicMenuStorageMetaDataUpdated(OrganizationStorageRef graphicMenuStorageRef)
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

                    var storageRef = new FlavourBusinessFacade.OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = graphicMenuStorageRef.StorageUrl, TimeStamp = RestaurantMenuDataLastModified };
                    FlavourBusinessToolKit.RawStorageData rawStorageData = new RawStorageData(storageRef, null);
                    OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));
                    var restaurantMenu = (from menu in restMenusData.GetObjectCollection<MenuPresentationModel.RestaurantMenu>()
                                          select menu).FirstOrDefault();

                    double? pageHeight = (restaurantMenu as MenuPresentationModel.MenuCanvas.IRestaurantMenu).Pages.FirstOrDefault()?.Height;
                    double? pageWidth = (restaurantMenu as MenuPresentationModel.MenuCanvas.IRestaurantMenu).Pages.FirstOrDefault()?.Width;
                    if (pageHeight != null)
                    {
                        fbstorage.SetPropertyValue("MenuPageHeight", pageHeight.Value.ToString(CultureInfo.GetCultureInfo(1033)));
                        fbstorage.SetPropertyValue("MenuPageWidth", pageWidth.Value.ToString(CultureInfo.GetCultureInfo(1033)));
                    }
                    else
                    {
                        fbstorage.RemoveProperty("MenuPageHeight");
                        fbstorage.RemoveProperty("MenuPageWidth");

                    }

                    stateTransition.Consistent = true;
                }



            }


        }

        /// <MetaDataID>{e1d0f93c-b314-44dd-a9ba-41a73faf3a51}</MetaDataID>
        public void OperativeRestaurantMenuDataUpdated(OrganizationStorageRef restaurantMenusDataStorageRef)
        {
            var menuItem = ObjectStorage.GetObjectFromUri<MenuModel.MenuItem>(@"7021ec91-37df-4417-8c1a-a6afb012fd09\3\56");
            ObjectStorage.UpdateOperativeObjects(restaurantMenusDataStorageRef.StorageIdentity);
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

        /// <MetaDataID>{b7b84334-cdad-4344-969d-28e1ff9de176}</MetaDataID>
        internal MenuModel.FixedMealType GetOneCoursesMealType()
        {
            if (OperativeRestaurantMenu != null)
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(OperativeRestaurantMenu);
                if (objectStorage != null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                    var mealTypes = (from mealType in storage.GetObjectCollection<MenuModel.FixedMealType>()
                                     select mealType).ToList();
                    var oneCourseMealType = mealTypes.Where(x => x.Courses.Count() == 1).FirstOrDefault();
                    return oneCourseMealType;
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
                    _CallerIDServer = (from callerIDServer in servicesContextStorage.GetObjectCollection<ICallerIDServer>()
                                       where callerIDServer.ServicesContextIdentity == servicesContextIdentity
                                       select callerIDServer).FirstOrDefault();

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

      

        /// <MetaDataID>{fa9fc77b-4d3e-40c2-a258-aa5d6d39c9b9}</MetaDataID>
        public ServiceContextHumanResources ServiceContextHumanResources
        {
            get
            {
                var activeShiftWorks = GetActiveShiftWorks();
                return new ServiceContextHumanResources() { Waiters = Waiters.Where(x => !string.IsNullOrWhiteSpace(x.OAuthUserIdentity)).ToList(), TakeawayCashiers = TakeawayCashiers.Where(x => !string.IsNullOrWhiteSpace(x.OAuthUserIdentity)).ToList(), Couriers = Couriers.Where(x => !string.IsNullOrWhiteSpace(x.OAuthUserIdentity)).ToList(), Supervisors = Supervisors.Where(x => !string.IsNullOrWhiteSpace(x.OAuthUserIdentity)).ToList(), ActiveShiftWorks = activeShiftWorks };
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

                var preparationStations =PreparationStationRuntimes.Values.OfType<IPreparationStation>().ToList();

                foreach(var subPreparationStation in preparationStations.SelectMany(x=>x.SubStations ).ToList())
                {
                    if(preparationStations.Contains(subPreparationStation))
                        preparationStations.Remove(subPreparationStation);
                }

                return preparationStations;

                //return (from preparationStation in servicesContextStorage.GetObjectCollection<IPreparationStation>()
                //        where preparationStation.ServicesContextIdentity == servicesContextIdentity
                //        select preparationStation).ToList();

            }
        }

        /// <MetaDataID>{11e22a8c-34e2-4673-a526-2248b7e891de}</MetaDataID>
        public IList<ITakeAwayStation> TakeAwayStations
        {
            get
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                var servicesContextIdentity = ServicesContextIdentity;
                return (from takeAwayStation in servicesContextStorage.GetObjectCollection<ITakeAwayStation>()
                        where takeAwayStation.ServicesContextIdentity == servicesContextIdentity
                        select takeAwayStation).ToList();




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

        /// <exclude>Excluded</exclude>
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

        /// <MetaDataID>{a5152b8e-310e-4172-b646-8f6325557e8c}</MetaDataID>
        object DeliveryServicePointLock = new object();

        /// <exclude>Excluded</exclude>
        bool DeliveryServicePointLoaded;

        /// <exclude>Excluded</exclude>
        HomeDeliveryServicePoint _DeliveryServicePoint;

        /// <MetaDataID>{d0ccc8ee-e69b-455b-9ec1-40395c967522}</MetaDataID>
        public IHomeDeliveryServicePoint DeliveryServicePoint
        {
            get
            {
                lock (DeliveryServicePointLock)
                {
                    if (DeliveryServicePointLoaded)
                    {
                        if (_DeliveryServicePoint?.IsActive == true)
                        {
                            _DeliveryServicePoint.Description = Description;
                            return _DeliveryServicePoint;
                        }
                        else
                            return null;
                    }
                }


                var objectStorage = ObjectStorage.GetStorageOfObject(this);// OpenServicesContextStorageStorage();
                OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
                var servicesContextIdentity = ServicesContextIdentity;
                var deliveryServicePoint = (from homeDeliveryServicePoint in servicesContextStorage.GetObjectCollection<HomeDeliveryServicePoint>()
                                            where homeDeliveryServicePoint.ServicesContextIdentity == servicesContextIdentity
                                            select homeDeliveryServicePoint).ToList().Where(x => !x.ServicesPointIdentity.EndsWith("_test")).FirstOrDefault();

                lock (DeliveryServicePointLock)
                {
                    if (!DeliveryServicePointLoaded)
                    {
                        _DeliveryServicePoint = deliveryServicePoint;
                        DeliveryServicePointLoaded = true;
                    }
                    if (_DeliveryServicePoint?.IsActive == true)
                        return _DeliveryServicePoint;
                    else
                        return null;
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
        /// <MetaDataID>{478c7c00-001c-4ef8-b167-e5613de66c68}</MetaDataID>
        private WebhooksToLocalServer.Webhookservice Webhookservice;

        //clientDeviceID="81000000296"
        //clientName="clientName"


        public IServicePoint GetServicePoint(string servicePointIdentity)
        {

            var servicesContextIdentity = ServicesContextIdentity;
            servicePointIdentity = servicePointIdentity.Replace(servicesContextIdentity + ";", "");

            if (DeliveryServicePoint.ServicesPointIdentity == servicePointIdentity)
                return DeliveryServicePoint;


            var servicePoint = (from serviceArea in ServiceAreas
                                from aServicePoint in serviceArea.ServicePoints
                                where aServicePoint.ServicesPointIdentity == servicePointIdentity
                                select aServicePoint).OfType<IServicePoint>().FirstOrDefault();

            return servicePoint;

        }




        public void ObjectStorageUpdate(string storageIdentity, OrganizationStorages flavourStorageType)
        {

            ObjectStorage.UpdateOperativeObjects(storageIdentity);

        }



        /// <MetaDataID>{fd5b3748-a682-47e5-8c57-59022f9e4f17}</MetaDataID>
        public ClientSessionData GetClientSession(string servicePointIdentity, string mealInvitationSessionID, string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken, string organizationIdentity, List<OrganizationStorageRef> graphicMenus, bool endUser, bool create)
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);

            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

            var servicesContextIdentity = ServicesContextIdentity;
            servicePointIdentity = servicePointIdentity.Replace(servicesContextIdentity + ";", "");
            var servicePoint = (from serviceArea in ServiceAreas
                                from aServicePoint in serviceArea.ServicePoints
                                where aServicePoint.ServicesPointIdentity == servicePointIdentity
                                select aServicePoint).OfType<IServicePoint>().FirstOrDefault();

            if (servicePoint == null && DeliveryServicePoint?.ServicesPointIdentity == servicePointIdentity)
                servicePoint = DeliveryServicePoint as IServicePoint;
            if(servicePoint==null)
                servicePoint=TakeAwayStations.Where(x=>x.ServicesPointIdentity==servicePointIdentity).FirstOrDefault();

                var clientSession = servicePoint.GetFoodServiceClientSession(clientName, mealInvitationSessionID, clientDeviceID, deviceType, deviceFirebaseToken, endUser, create);

            if (clientSession == null)
                return new ClientSessionData();

            string token = null;
            //var graphicMenu = graphicMenus.FirstOrDefault();

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



        /// <MetaDataID>{783ca332-c628-47c9-8646-43841e92b78f}</MetaDataID>
        public IFoodServiceClientSession GetMealInvitationInviter(string mealInvitationSessionID)
        {
            return OpenClientSessions.Where(x => x.SessionID == mealInvitationSessionID).FirstOrDefault();
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
                lock (PreparationStationRuntimesLock)
                {
                    if (_PreparationStationRuntimes.ContainsKey(prepartionStation.PreparationStationIdentity))
                        _PreparationStationRuntimes.Remove(prepartionStation.PreparationStationIdentity);
                }




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


        /// <MetaDataID>{3a6537d5-5bc1-420d-9d43-cfccceb943f1}</MetaDataID>
        public ITakeAwayStation NewTakeAwayStation()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);
            TakeAwayStation takeAwayStation = new TakeAwayStation(this);

            takeAwayStation.AddMealType(ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                takeAwayStation.Description = Properties.Resources.DefaultTakeAwayStationDescription;
                takeAwayStation.ServicesContextIdentity = this.ServicesContextIdentity;
                objectStorage.CommitTransientObjectState(takeAwayStation);
                stateTransition.Consistent = true;
            }
            var count = TakeAwayStationsDictionary.Count;
            lock (takeAwayStationsLock)
            {
                _TakeAwayStationsDictionary[takeAwayStation.TakeAwayStationIdentity] = takeAwayStation;
            }
            //var count = PreparationStationRuntimes.Count;
            return takeAwayStation;
        }


        /// <MetaDataID>{1f046455-d3b7-4545-a1ea-1aebda104e94}</MetaDataID>
        object takeAwayStationsLock = new object();


        /// <MetaDataID>{896dab94-f734-4dda-9be5-1a8c21e0039c}</MetaDataID>
        object homeDeliveryCallCenterStationsLock = new object();
        /// <exclude>Excluded</exclude>
        Dictionary<string, ITakeAwayStation> _TakeAwayStationsDictionary;

        /// <MetaDataID>{717900a3-bd70-4e5c-814f-228ffd01fb35}</MetaDataID>
        public Dictionary<string, ITakeAwayStation> TakeAwayStationsDictionary
        {
            get
            {
                lock (takeAwayStationsLock)
                {
                    if (_TakeAwayStationsDictionary == null)
                    {
                        _TakeAwayStationsDictionary = new Dictionary<string, ITakeAwayStation>();
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);
                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        foreach (var takeAwayStation in (from aTakeAwayStation in servicesContextStorage.GetObjectCollection<ITakeAwayStation>()
                                                         where aTakeAwayStation.ServicesContextIdentity == servicesContextIdentity
                                                         select aTakeAwayStation))
                        {
                            if (!string.IsNullOrWhiteSpace(takeAwayStation.TakeAwayStationIdentity))
                                this._TakeAwayStationsDictionary[takeAwayStation.TakeAwayStationIdentity] = takeAwayStation;

                        }
                    }
                    return new Dictionary<string, ITakeAwayStation>(_TakeAwayStationsDictionary);
                }
            }
        }

        /// <MetaDataID>{6fe2bea5-c425-4733-afee-7f472727e728}</MetaDataID>
        public FirebaseAuthClient FireBaseClient { get; private set; }

        /// <MetaDataID>{e24e3ae2-eef3-4240-8237-cae30fd5dcd4}</MetaDataID>
        public ITakeAwayStation GetTakeAwayStation(string takeAwayStationCredentialKey)
        {
            lock (takeAwayStationsLock)
            {
                if (TakeAwayStationsDictionary.ContainsKey(takeAwayStationCredentialKey))
                    return _TakeAwayStationsDictionary[takeAwayStationCredentialKey];
                return null;
            }

        }
        /// <MetaDataID>{f70c687d-1a6b-4278-adf6-60692e5a422c}</MetaDataID>
        public IHomeDeliveryCallCenterStation GetHomeDeliveryCallCenterStation(string deliveryCallCenterCredentialKey)
        {
            lock (homeDeliveryCallCenterStationsLock)
            {
                if (CallCenterStationsDictionary.ContainsKey(deliveryCallCenterCredentialKey))
                {
                    var homeDeliveryCallCenterStation = CallCenterStationsDictionary[deliveryCallCenterCredentialKey];
                    if (this.DeliveryServicePoint != null && !homeDeliveryCallCenterStation.HomeDeliveryServicePoints.Contains(this.DeliveryServicePoint))
                        homeDeliveryCallCenterStation.AddHomeDeliveryServicePoint(this.DeliveryServicePoint);

                    return _CallCenterStationsDictionary[deliveryCallCenterCredentialKey];
                }
                return null;
            }
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

        public ServiceContextResources ServiceContextResources { get=>new ServiceContextResources() { CallerIDServer = CallerIDServer, CashierStations = CashierStations, ServiceAreas = ServiceAreas, PreparationStations = PreparationStations, TakeAwayStations = TakeAwayStations, PaymentTerminals = PaymentTerminals, DeliveryCallCenterStations = this.CallCenterStations };  }


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


                string hallBlobName = Properties.Resources.DefaultHallFileName;

                fbstorage = (from storage in this.Storages
                             where storage.Name == hallBlobName
                             select storage).FirstOrDefault();
                int count = 1;
                while (fbstorage != null)
                {
                    hallBlobName = Properties.Resources.DefaultHallFileName + count;
                    fbstorage = (from storage in this.Storages
                                 where storage.Name == hallBlobName
                                 select storage).FirstOrDefault();
                }



                string blobUrl = "usersfolder/" + OrganizationIdentity + "/" + hallBlobName + ".xml";
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
                var uploadSlot = new UploadSlot(blobUrl, FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer);
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
            ObjectStorage.UpdateOperativeObjects(storageRef.StorageIdentity);


        }
        /// <MetaDataID>{cfaf6fae-af4a-4ceb-a682-c39bcef3ec8c}</MetaDataID>
        public void StorageDataUpdated(string storageIdentity)
        {
            ObjectStorage.UpdateOperativeObjects(storageIdentity);
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
                                            select supervisor).ToList().Where((x => string.IsNullOrWhiteSpace(x.OAuthUserIdentity))).FirstOrDefault();
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

                    unassignedSupervisor.WorkerAssignKey = supervisorAssignKey;
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
                                            where supervisor.WorkerAssignKey == supervisorAssignKey
                                            select supervisor).FirstOrDefault();

                if (unassignedSupervisor != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        unassignedSupervisor.WorkerAssignKey = null;
                        (unassignedSupervisor as ServiceContextSupervisor).OAuthUserIdentity = signUpUserIdentity;
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

        ///// <MetaDataID>{ad553593-59ac-4542-944d-b02f1ee008ac}</MetaDataID>
        //public IShiftWork NewShifWork(IServicesContextWorker worker, System.DateTime startedAt, double timespanInHours)
        //{
        //    return worker.NewShiftWork(startedAt, timespanInHours);
        //}

        /// <MetaDataID>{269aa8e6-0fb9-4925-9d4e-7eba3cdcf288}</MetaDataID>
        public IWaiter AssignWaiterUser(string waiterAssignKey, string signUpUserIdentity, string userName)
        {
            lock (SupervisorsLock)
            {
                var unassignedWaiter = (from waiter in Waiters
                                        where waiter.WorkerAssignKey == waiterAssignKey
                                        select waiter).FirstOrDefault();

                if (unassignedWaiter != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        unassignedWaiter.WorkerAssignKey = null;
                        (unassignedWaiter as Waiter).OAuthUserIdentity = signUpUserIdentity;
                        unassignedWaiter.Name = userName;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                }

                return unassignedWaiter;
            }
        }

        public ICourier AssignCourierUser(string courierAssignKey, string signUpUserIdentity, string userName)
        {
            lock (SupervisorsLock)
            {
                var unassignedCourier = (from courier in Couriers
                                         where courier.WorkerAssignKey == courierAssignKey
                                         select courier).FirstOrDefault();

                if (unassignedCourier != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        unassignedCourier.WorkerAssignKey = null;
                        (unassignedCourier as Waiter).OAuthUserIdentity = signUpUserIdentity;
                        unassignedCourier.Name = userName;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                }

                return unassignedCourier;
            }
        }
        public ITakeawayCashier AssignTakeawayCashierUser(string takeawayCashierAssignKey, string signUpUserIdentity, string userName)
        {
            lock (SupervisorsLock)
            {
                var unassignedCashier = (from cashier in TakeawayCashiers
                                         where cashier.WorkerAssignKey == takeawayCashierAssignKey
                                         select cashier).FirstOrDefault();

                if (unassignedCashier != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        unassignedCashier.WorkerAssignKey = null;
                        (unassignedCashier as Waiter).OAuthUserIdentity = signUpUserIdentity;
                        unassignedCashier.Name = userName;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                }

                return unassignedCashier;
            }
        }

        public string AssignCourierScannerDevice(string deviceAssignKey)
        {

            NativeAuthUser nativeUser = null;
            string wokerIdentity = deviceAssignKey.Split(';')[1];
            var courier = this.Couriers.Where(x => x.Identity == wokerIdentity).FirstOrDefault();
            if (courier != null)
            {
                return DeliveryServicePoint.ServicesContextIdentity + ";" + DeliveryServicePoint.ServicesPointIdentity;
            }
            else
                throw new InvalidAssignmentQRCodeException("The QR code for assignment is not valid");
        }
        public NativeUserSignInData AssignDeviceToNativeUser(string deviceAssignKey)
        {
            NativeAuthUser nativeUser = null;
            string wokerIdentity = deviceAssignKey.Split(';')[1];
            var waiter = this.Waiters.Where(x => x.Identity == wokerIdentity).FirstOrDefault();
            if (waiter != null)
            {
                nativeUser = NativeUsers.Where(x => x.OAuthUserIdentity == waiter.OAuthUserIdentity).FirstOrDefault();
            }
            else
            {
                var courier = this.Couriers.Where(x => x.Identity == wokerIdentity).FirstOrDefault();
                if (courier != null)
                {
                    nativeUser = NativeUsers.Where(x => x.OAuthUserIdentity == courier.OAuthUserIdentity).FirstOrDefault();

                }
                else
                {
                    var cashier = this.TakeawayCashiers.Where(x => x.Identity == wokerIdentity).FirstOrDefault();
                    if (cashier != null)
                    {
                        nativeUser = NativeUsers.Where(x => x.OAuthUserIdentity == cashier.OAuthUserIdentity).FirstOrDefault();
                    }
                    else
                    {
                        var supervisor = this.Supervisors.Where(x => x.Identity == wokerIdentity).FirstOrDefault();
                        if (supervisor != null)
                        {
                            nativeUser = NativeUsers.Where(x => x.OAuthUserIdentity == supervisor.OAuthUserIdentity).FirstOrDefault();
                        }

                    }
                }
            }
            if (nativeUser != null)
                return new NativeUserSignInData() { FireBaseUserName = nativeUser.FireBaseUserName, FireBasePasword = nativeUser.FireBasePasword, ServiceContextIdentity = ServicesContextIdentity };

            return null;
        }



        /// <MetaDataID>{ca263376-0c75-4e57-abf9-2e06345ff26c}</MetaDataID>
        public IWaiter AssignWaiterNativeUser(string waiterAssignKey, string userName, string password, string userFullName)
        {
            lock (SupervisorsLock)
            {


                var unassignedWaiter = (from waiter in Waiters
                                        where waiter.WorkerAssignKey == waiterAssignKey
                                        select waiter).FirstOrDefault();


                NativeAuthUser nativeAuthUser = null;
                if (unassignedWaiter != null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {

                        nativeAuthUser = new NativeAuthUser(userName, password, userFullName);// { UserName=userName, Password=password, UserFullName=userFullName };
                        nativeAuthUser.RoleType = RoleType.Waiter;
                        nativeAuthUser.CreateFirebaseEmailUserCredential();
                        UserCredential user = null;
                        var FireBaseAcoountTask = Task.Run(async () =>
                        {
                            user = await this.FireBaseClient.CreateUserWithEmailAndPasswordAsync(nativeAuthUser.FireBaseUserName, nativeAuthUser.FireBasePasword, nativeAuthUser.UserFullName);
                        });

                        FireBaseAcoountTask.Wait(TimeSpan.FromSeconds(30));

                        AuthUser authUser = AuthUser.GetAuthUserFromToken(user.User.Credential.IdToken);
                        AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
                        authUserRef.AddRole(unassignedWaiter);
                        authUserRef.UserName = userName;
                        authUserRef.FullName = userFullName;
                        authUserRef.Save();



                        var objectStorage = ObjectStorage.GetStorageOfObject(this);
                        objectStorage.CommitTransientObjectState(nativeAuthUser);
                        unassignedWaiter.WorkerAssignKey = null;
                        (unassignedWaiter as Waiter).OAuthUserIdentity = authUser.User_ID;
                        nativeAuthUser.OAuthUserIdentity = authUser.User_ID;
                        unassignedWaiter.Name = userFullName;
                        unassignedWaiter.NativeUser = true;

                        stateTransition.Consistent = true;
                    }
                    lock (NativeUsersLock)
                    {
                        if (NativeUsers.Contains(nativeAuthUser))
                            NativeUsers.Add(nativeAuthUser);
                    }
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                }




                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {



                    stateTransition.Consistent = true;
                }
                return default(IWaiter);

                //if (unassignedWaiter != null)
                //{
                //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                //    {
                //        unassignedWaiter.WorkerAssignKey = null;
                //        (unassignedWaiter as Waiter).OAuthUserIdentity = signUpUserIdentity;
                //        unassignedWaiter.Name = userName;
                //        stateTransition.Consistent = true;
                //    }
                //    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                //}

                //return unassignedWaiter;
            }
        }

        /// <MetaDataID>{6c4a37cf-4c90-4108-be8b-c3fa0f238af9}</MetaDataID>
        public ITakeawayCashier AssignTakeAwayCashierNativeUser(string takeAwayCashierAssignKey, string userName, string password, string userFullName)
        {
            lock (SupervisorsLock)
            {


                var unassignedtakeawayCashier = (from takeawayCashier in TakeawayCashiers
                                                 where takeawayCashier.WorkerAssignKey == takeAwayCashierAssignKey
                                                 select takeawayCashier).FirstOrDefault();



                if (unassignedtakeawayCashier != null)
                {
                    NativeAuthUser nativeAuthUser = null;
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {

                        nativeAuthUser = new NativeAuthUser(userName, password, userFullName);// { UserName=userName, Password=password, UserFullName=userFullName };
                        nativeAuthUser.RoleType = RoleType.TakeAwayCashier;
                        nativeAuthUser.CreateFirebaseEmailUserCredential();
                        UserCredential user = null;
                        var FireBaseAcoountTask = Task.Run(async () =>
                        {

                            user = await this.FireBaseClient.CreateUserWithEmailAndPasswordAsync(nativeAuthUser.FireBaseUserName, nativeAuthUser.FireBasePasword, nativeAuthUser.UserFullName);
                        });

                        FireBaseAcoountTask.Wait(TimeSpan.FromSeconds(30));

                        AuthUser authUser = AuthUser.GetAuthUserFromToken(user.User.Credential.IdToken);
                        AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
                        authUserRef.AddRole(unassignedtakeawayCashier);
                        authUserRef.UserName = userName;
                        authUserRef.FullName = userFullName;
                        authUserRef.Save();



                        var objectStorage = ObjectStorage.GetStorageOfObject(this);
                        objectStorage.CommitTransientObjectState(nativeAuthUser);
                        unassignedtakeawayCashier.WorkerAssignKey = null;
                        (unassignedtakeawayCashier as TakeawayCashier).OAuthUserIdentity = authUser.User_ID;
                        nativeAuthUser.OAuthUserIdentity = authUser.User_ID;
                        unassignedtakeawayCashier.Name = userFullName;
                        unassignedtakeawayCashier.NativeUser = true;

                        stateTransition.Consistent = true;
                    }
                    lock (NativeUsersLock)
                    {
                        if (!NativeUsers.Contains(nativeAuthUser))
                            NativeUsers.Add(nativeAuthUser);
                    }
                    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                }



                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    stateTransition.Consistent = true;
                }
                return default(ITakeawayCashier);

                //if (unassignedWaiter != null)
                //{
                //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                //    {
                //        unassignedWaiter.WorkerAssignKey = null;
                //        (unassignedWaiter as Waiter).OAuthUserIdentity = signUpUserIdentity;
                //        unassignedWaiter.Name = userName;
                //        stateTransition.Consistent = true;
                //    }
                //    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                //}

                //return unassignedWaiter;
            }

        }


        /// <MetaDataID>{c916e91a-8686-41a1-bae4-6fd42e42da32}</MetaDataID>
        public ICourier AssignCourierNativeUser(string courierAssignKey, string userName, string password, string userFullName)
        {
            lock (SupervisorsLock)
            {


                var unassignedCourier = (from courier in Couriers
                                         where courier.WorkerAssignKey == courierAssignKey
                                         select courier).FirstOrDefault();



                if (unassignedCourier != null)
                {
                    NativeAuthUser nativeAuthUser = null;
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {

                        nativeAuthUser = new NativeAuthUser(userName, password, userFullName);// { UserName=userName, Password=password, UserFullName=userFullName };
                        nativeAuthUser.RoleType = RoleType.Courier;
                        nativeAuthUser.CreateFirebaseEmailUserCredential();
                        UserCredential user = null;
                        var FireBaseAcoountTask = Task.Run(async () =>
                        {
                            user = await this.FireBaseClient.CreateUserWithEmailAndPasswordAsync(nativeAuthUser.FireBaseUserName, nativeAuthUser.FireBasePasword, nativeAuthUser.UserFullName);
                        });

                        FireBaseAcoountTask.Wait(TimeSpan.FromSeconds(30));

                        AuthUser authUser = AuthUser.GetAuthUserFromToken(user.User.Credential.IdToken);
                        AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
                        authUserRef.AddRole(unassignedCourier);
                        authUserRef.UserName = userName;
                        authUserRef.FullName = userFullName;
                        authUserRef.Save();



                        var objectStorage = ObjectStorage.GetStorageOfObject(this);
                        objectStorage.CommitTransientObjectState(nativeAuthUser);
                        unassignedCourier.WorkerAssignKey = null;
                        (unassignedCourier as Courier).OAuthUserIdentity = authUser.User_ID;
                        nativeAuthUser.OAuthUserIdentity = authUser.User_ID;
                        unassignedCourier.Name = userFullName;
                        unassignedCourier.NativeUser = true;

                        stateTransition.Consistent = true;
                    }
                    lock (NativeUsersLock)
                    {
                        if (!NativeUsers.Contains(nativeAuthUser))
                            NativeUsers.Add(nativeAuthUser);
                    }

                }
                ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));



                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    stateTransition.Consistent = true;
                }
                return default(ICourier);

                //if (unassignedWaiter != null)
                //{
                //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                //    {
                //        unassignedWaiter.WorkerAssignKey = null;
                //        (unassignedWaiter as Waiter).OAuthUserIdentity = signUpUserIdentity;
                //        unassignedWaiter.Name = userName;
                //        stateTransition.Consistent = true;
                //    }
                //    ObjectChangeState?.Invoke(this, nameof(ServiceContextHumanResources));
                //}

                //return unassignedWaiter;
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

        /// <MetaDataID>{2eace06e-0e0c-4799-aa8b-7c5b931c9bfc}</MetaDataID>
        public void RemoveHomeDeliveryService()
        {
            if (_DeliveryServicePoint != null)
                _DeliveryServicePoint.IsActive = false;
        }

        /// <MetaDataID>{ac27cd5a-22b3-4080-937c-b7db324656f0}</MetaDataID>
        public void LaunchHomeDeliveryService()
        {
            if (_DeliveryServicePoint != null)
                _DeliveryServicePoint.IsActive = true;
            else
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    _DeliveryServicePoint = new HomeDeliveryServicePoint();
                    _DeliveryServicePoint.ServicesContextIdentity = this.ServicesContextIdentity;
                    _DeliveryServicePoint.IsActive = true;
                    objectStorage.CommitTransientObjectState(_DeliveryServicePoint);

                    _DeliveryServicePoint.AddMealType(ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri);
                    stateTransition.Consistent = true;
                }
            }

        }

        /// <MetaDataID>{573c4b04-625c-4470-8299-4213e41ca1cc}</MetaDataID>
        public OOAdvantech.Remoting.RestApi.HookRespnose WebHook(string method, string webHookName, Dictionary<string, string> headers, string content)
        {
            var hookRespnose = new OOAdvantech.Remoting.RestApi.HookRespnose();



            hookRespnose = Payment.GetPaymentProvider(webHookName).WebHook(method, webHookName, headers, content);

            //    return  PaymentProviders.VivaWallet.WebHook(method,webHookName,headers,content);
            return hookRespnose;
        }
        /// <MetaDataID>{4d421429-58e5-49df-b236-a056c6cf33d1}</MetaDataID>
        Dictionary<string, MenuPresentationModel.MenuCanvas.IRestaurantMenu> VersioningGraphicMenus = new Dictionary<string, MenuPresentationModel.MenuCanvas.IRestaurantMenu>();
        /// <MetaDataID>{7d77c670-4ebc-4f29-84d7-5ea530bb5b32}</MetaDataID>
        internal MenuPresentationModel.MenuCanvas.IRestaurantMenu GetGraphicMenuVersion(string storageUrl)
        {
            lock (VersioningGraphicMenus)
            {
                if (VersioningGraphicMenus.ContainsKey(storageUrl))
                    return VersioningGraphicMenus[storageUrl];
                else
                {
                    using (System.Net.WebClient wc = new System.Net.WebClient())
                    {

                        var json = wc.DownloadString(storageUrl);
                        var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
                        //new OOAdvantech.Json.JsonSerializerSettings { ReferenceLoopHandling = OOAdvantech.Json.ReferenceLoopHandling.Serialize, PreserveReferencesHandling = OOAdvantech.Json.PreserveReferencesHandling.All };


                        var restaurantMenu = OOAdvantech.Json.JsonConvert.DeserializeObject<MenuPresentationModel.JsonMenuPresentation.RestaurantMenu>(json, jSetttings);
                        VersioningGraphicMenus[storageUrl] = restaurantMenu;
                        return restaurantMenu;
                    }
                }

            }

        }

        /// <MetaDataID>{727137b8-540e-499e-91eb-afbdf0147e46}</MetaDataID>
        public void RemoveTakeAwayStation(ITakeAwayStation takeAwayStationStation)
        {
            try
            {
                ObjectStorage.DeleteObject(takeAwayStationStation);
                lock (takeAwayStationsLock)
                {

                    if (_TakeAwayStationsDictionary.ContainsKey(takeAwayStationStation.TakeAwayStationIdentity))
                        _TakeAwayStationsDictionary.Remove(takeAwayStationStation.TakeAwayStationIdentity);
                }
            }
            catch (Exception error)
            {

                throw;
            }
        }
        /// <MetaDataID>{895528db-162c-4fef-b4e0-aa2dcb0e60ec}</MetaDataID>
        object NativeUsersLock = new object();
        /// <MetaDataID>{8f937803-ac74-41c3-bc61-cf1e5af8ee2c}</MetaDataID>
        List<NativeAuthUser> _NativeUsers;
        /// <MetaDataID>{25fe9404-b580-4dc4-b373-e552ce7682db}</MetaDataID>
        List<NativeAuthUser> NativeUsers
        {
            get
            {
                lock (NativeUsersLock)
                {
                    if (_NativeUsers == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        _NativeUsers = (from nativeUser in servicesContextStorage.GetObjectCollection<NativeAuthUser>()

                                        select nativeUser).ToList();
                    }
                    return _NativeUsers;
                }
            }
        }



        /// <MetaDataID>{52248cf8-a28f-493a-82ec-fd456600df81}</MetaDataID>
        public IList<UserData> GetNativeUsers(RoleType roleType)
        {

            return NativeUsers.Where(x => (x.RoleType & roleType) != 0).Select(x => new UserData() { UserName = x.UserName, FullName = x.UserFullName }).ToList();

        }

        /// <MetaDataID>{294f1115-7a48-48a6-a5c4-58870a77a191}</MetaDataID>
        public UserData SignInNativeUser(string userName, string password)
        {
            lock (NativeUsersLock)
            {
                var nativeUser = NativeUsers.Where(x => x.UserName?.ToLower() == userName?.ToLower() && x.Password == password).FirstOrDefault();
                if (nativeUser == null)
                    return null;

                UserData userData = new UserData() { Email = nativeUser.FireBaseUserName, Password = nativeUser.FireBasePasword, UserName = nativeUser.UserName, FullName = nativeUser.UserFullName };
                return userData;

            }
        }

        /// <MetaDataID>{72e35824-8378-44ac-a8ce-768e5aa4437b}</MetaDataID>
        public IHomeDeliveryCallCenterStation NewCallCenterStation()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(this);
            HomeDeliveryCallCenterStation homeDeliveryCallCenterStation = new HomeDeliveryCallCenterStation(this);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                homeDeliveryCallCenterStation.Description = Properties.Resources.DefaultHomeDeliveryCallCenterStationDescription;
                homeDeliveryCallCenterStation.ServicesContextIdentity = this.ServicesContextIdentity;
                objectStorage.CommitTransientObjectState(homeDeliveryCallCenterStation);

                if (this.DeliveryServicePoint != null)
                    homeDeliveryCallCenterStation.AddHomeDeliveryServicePoint(this.DeliveryServicePoint);
                stateTransition.Consistent = true;
            }
            var count = CallCenterStationsDictionary.Count;
            lock (DeliveryCallcenterStationsLock)
            {
                _CallCenterStationsDictionary[homeDeliveryCallCenterStation.CallcenterStationIdentity] = homeDeliveryCallCenterStation;
            }
            //var count = PreparationStationRuntimes.Count;
            return homeDeliveryCallCenterStation;
        }


        /// <exclude>Excluded</exclude>
        object DeliveryCallcenterStationsLock = new object();

        /// <exclude>Excluded</exclude>
        Dictionary<string, IHomeDeliveryCallCenterStation> _CallCenterStationsDictionary;

        /// <MetaDataID>{717900a3-bd70-4e5c-814f-228ffd01fb35}</MetaDataID>
        public Dictionary<string, IHomeDeliveryCallCenterStation> CallCenterStationsDictionary
        {
            get
            {
                lock (DeliveryCallcenterStationsLock)
                {
                    if (_CallCenterStationsDictionary == null)
                    {
                        _CallCenterStationsDictionary = new Dictionary<string, IHomeDeliveryCallCenterStation>();
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);
                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var servicesContextIdentity = ServicesContextIdentity;
                        foreach (var takeAwayStation in (from aTakeAwayStation in servicesContextStorage.GetObjectCollection<IHomeDeliveryCallCenterStation>()
                                                         where aTakeAwayStation.ServicesContextIdentity == servicesContextIdentity
                                                         select aTakeAwayStation))
                        {
                            if (!string.IsNullOrWhiteSpace(takeAwayStation.CallcenterStationIdentity))
                                this._CallCenterStationsDictionary[takeAwayStation.CallcenterStationIdentity] = takeAwayStation;

                        }
                    }
                    return new Dictionary<string, IHomeDeliveryCallCenterStation>(_CallCenterStationsDictionary);
                }
            }
        }


        /// <MetaDataID>{273fea1f-369c-4340-9f81-8db8d3440da3}</MetaDataID>
        public void RemoveCallCenterStation(IHomeDeliveryCallCenterStation homeDeliveryCallcenterStation)
        {
            try
            {
                ObjectStorage.DeleteObject(homeDeliveryCallcenterStation);
                lock (DeliveryCallcenterStationsLock)
                {

                    if (_CallCenterStationsDictionary.ContainsKey(homeDeliveryCallcenterStation.CallcenterStationIdentity))
                        _CallCenterStationsDictionary.Remove(homeDeliveryCallcenterStation.CallcenterStationIdentity);
                }

            }
            catch (Exception error)
            {

                throw;
            }
        }

        internal void SupervisorSiftWorkUpdated(ServiceContextSupervisor serviceContextSupervisor)
        {
            lock (ServiceContextRTLock)
            {
                if (serviceContextSupervisor.ShiftWork != null && !ActiveShiftWorks.Contains(serviceContextSupervisor.ShiftWork))
                    ActiveShiftWorks.Add(serviceContextSupervisor.ShiftWork);

                var activeShiftWorks = GetActiveShiftWorks();
            }
        }

        /// <MetaDataID>{8caa1c61-4c4a-4119-8d9c-594ee8b59b6e}</MetaDataID>
        public List<IHomeDeliveryCallCenterStation> CallCenterStations
        {

            get
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                var servicesContextIdentity = ServicesContextIdentity;
                return (from homeDeliveryCallcenterStation in servicesContextStorage.GetObjectCollection<IHomeDeliveryCallCenterStation>()
                        where homeDeliveryCallcenterStation.ServicesContextIdentity == servicesContextIdentity
                        select homeDeliveryCallcenterStation).ToList();

            }

        }
    }




}