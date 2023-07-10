using System;
using System.Linq;
using System.Collections.Generic;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ComputingResources;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using FlavourBusinessManager.ServicesContextResources;
using ComputationalResources;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Remoting;
using System.Threading.Tasks;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;

using MarshalByRefObject =System.MarshalByRefObject;
#endif
namespace FlavourBusinessManager
{
    /// <MetaDataID>{e8fbdbb0-94fd-4108-81a6-58b46e944a68}</MetaDataID>
    [BackwardCompatibilityID("{e8fbdbb0-94fd-4108-81a6-58b46e944a68}")]
    [Persistent()]
    public class FlavoursServicesContext : MarshalByRefObject, IFlavoursServicesContext, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{093d728f-914a-47a7-aa1e-9e95d605263b}</MetaDataID>
        public void RemovePaymentTerminal(IPaymentTerminal paymentTerminal)
        {
            GetRunTime().RemovePaymentTerminal(paymentTerminal);
        }

        /// <MetaDataID>{10e5b27d-5996-4f8a-8873-07b64114d51a}</MetaDataID>
        public IPaymentTerminal NewPaymentTerminal()
        {
            return GetRunTime().NewPaymentTerminal();
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IFoodTypeTag> _FoodTypes = new OOAdvantech.Collections.Generic.Set<IFoodTypeTag>();

        /// <MetaDataID>{057cd81b-2ed5-4e0a-9a82-c905c36086f5}</MetaDataID>
        [PersistentMember(nameof(_FoodTypes))]
        [BackwardCompatibilityID("+7")]
        public List<IFoodTypeTag> FoodTypes
        {
            get => _FoodTypes.ToThreadSafeList();

        }


        /// <MetaDataID>{07e6d9ad-97ef-4bf0-a4a8-91c1f5fe9224}</MetaDataID>
        public void RemovePreparationStation(IPreparationStation prepartionStation)
        {
            GetRunTime().RemovePreparationStation(prepartionStation);
        }

        //[BeforeCommitObjectStateInStorageCall]
        //void BeforeCommitObjectState()
        //{
        //    //lock (this)
        //    //    DeliveryPlacesJson = OOAdvantech.Json.JsonConvert.SerializeObject(_DeliveryPlaces);

        //}
        //[ObjectActivationCall]
        //public void ObjectActivation()
        //{
        //    //if (DeliveryPlacesJson != null)
        //    //    _DeliveryPlaces = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Place>>(DeliveryPlacesJson);
        //    //else
        //    //    _DeliveryPlaces = new List<Place>();

        //}
        /// <MetaDataID>{52258872-5176-411c-8cea-4173e715bd41}</MetaDataID>
        public IPreparationStation NewPreparationStation()
        {
            return GetRunTime().NewPreparationStation();
        }
        /// <MetaDataID>{d5772667-b75a-425a-a963-b41ed9a64ac8}</MetaDataID>
        public ITakeAwayStation NewTakeAwayStation()
        {
            return GetRunTime().NewTakeAwayStation();
        }

        /// <MetaDataID>{68705304-f7f3-4545-84ec-88bb0ae2d614}</MetaDataID>
        public void RemoveTakeAwayStation(ITakeAwayStation takeAwayStationStation)
        {
            GetRunTime().RemoveTakeAwayStation(takeAwayStationStation);
        }
        /// <MetaDataID>{f45b81fa-f1b3-41b0-89a1-25ac56e590ea}</MetaDataID>
        public FlavoursServicesContext()
        {

        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IOrganization> _Owner = new OOAdvantech.Member<IOrganization>();

        /// <MetaDataID>{f43cc497-c5f1-4daf-a19c-39f65142e659}</MetaDataID>
        [PersistentMember(nameof(_Owner))]
        [BackwardCompatibilityID("+5")]
        public IOrganization Owner
        {
            get
            {
                return _Owner.Value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;

        /// <MetaDataID>{b14f88e5-6a86-459a-ade7-a10c82b9b4ee}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+3")]
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

        /// <MetaDataID>{02353f9a-fc1c-4365-bfca-e645692c928e}</MetaDataID>
        internal static IFlavoursServicesContext GetServicesContext(string servicesContextIdentity)
        {
            var objectStorage = OpenFlavourBusinessesStorage();

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            return (from flavoursServicesContext in storage.GetObjectCollection<IFlavoursServicesContext>()
                    where flavoursServicesContext.ServicesContextIdentity == servicesContextIdentity
                    select flavoursServicesContext).FirstOrDefault();
        }
        /// <MetaDataID>{13da411d-ba9c-4447-a2fe-cc1aa1b1315b}</MetaDataID>
        public static IFlavoursServicesContextRuntime GetServicesContextRuntime(string servicesContextIdentity)
        {
            return GetServicesContext(servicesContextIdentity).GetRunTime();
        }

        /// <MetaDataID>{0112e364-4f4b-4c70-bc60-efe9d2b23737}</MetaDataID>
        internal static List<IFlavoursServicesContext> ActiveFlavoursServicesContexts
        {
            get
            {
                var objectStorage = OpenFlavourBusinessesStorage();
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                return (from flavoursServicesContext in storage.GetObjectCollection<IFlavoursServicesContext>()
                        select flavoursServicesContext).ToList();
            }
        }

        /// <MetaDataID>{af72231d-8a19-4e2f-a752-0efc32848315}</MetaDataID>
        static ObjectStorage FlavourBusinessesStorage;
        /// <MetaDataID>{7f8e6a1b-9597-4376-8112-d75c700bddb7}</MetaDataID>
        internal static ObjectStorage OpenFlavourBusinessesStorage()
        {

            if (FlavourBusinessesStorage != null)
                return FlavourBusinessesStorage;
            ObjectStorage storageSession = null;
            string storageName = "FlavourBusinesses";
            string storageLocation = "DevStorage";
            string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

            try
            {
                storageSession = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);


                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
                var storagesClient = new OOAdvantech.PersistenceLayer.StoragesClient(httpClient);
                storagesClient.BaseUrl = serverUrl;
                Task.Run(async () =>
                {
                    var storageMetaData = await storagesClient.GetAsync(storageSession.StorageMetaData.StorageIdentity);
                    if (storageMetaData == null || storageMetaData.StorageIdentity == null)
                        await storagesClient.PostAsync(storageSession.StorageMetaData, true);

                });
                FlavourBusinessesStorage = storageSession;
                //storageSession.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    storageSession = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    storageSession.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
                }
                catch (System.Exception Errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }

            return storageSession;
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{5ff9f6f0-dc4b-4f77-8241-dd8aef2ace0a}</MetaDataID>
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
                        if (_RunAtContext.Value != null)
                            _RunAtContext.Value.Description = _Description;

                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IIsolatedComputingContext> _RunAtContext = new OOAdvantech.Member<IIsolatedComputingContext>();

        /// <MetaDataID>{49a9afce-336e-4b15-b8cb-0d75eaf1b46c}</MetaDataID>
        [PersistentMember(nameof(_RunAtContext))]
        [BackwardCompatibilityID("+2")]
        public IIsolatedComputingContext RunAtContext
        {
            get
            {
                return _RunAtContext.Value;
            }

            internal set
            {
                if (_RunAtContext != value)
                {
                    if (_RunAtContext != null)
                        throw new Exception("You can't change computing context");

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _RunAtContext.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{f7cbd48e-fe10-4508-a9cc-c20da2f9e51e}</MetaDataID>
        public IList<IServiceArea> ServiceAreas
        {
            get
            {
                return GetRunTime().ServiceAreas;
                //var objectStorage = OpenServicesContextStorageStorage();
                //OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                //var servicesContextIdentity = ServicesContextIdentity;
                //return (from serviceArea in servicesContextStorage.GetObjectCollection<IServiceArea>()
                //        where serviceArea.ServicesContextIdentity == servicesContextIdentity
                //        select serviceArea).ToList();

            }
        }



        /// <exclude>Excluded</exclude>
        string _ContextStorageName;
        /// <MetaDataID>{58a331ce-1ec2-4eb4-9cf5-878418b9c7e1}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember(nameof(_ContextStorageName))]
        public string ContextStorageName
        {
            get
            {
                return _ContextStorageName;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ContextStorageName = value;
                    stateTransition.Consistent = true;
                }
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
                    var objectStorage = OpenServicesContextStorageStorage();
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

        /// <MetaDataID>{8b9d39cf-ece6-4e0c-b030-7cc9c3540640}</MetaDataID>
        public IList<ICashierStation> CashierStations
        {
            get
            {
                return GetRunTime().CashierStations;
            }
        }

        /// <MetaDataID>{3c9c6c67-a203-420d-9cdb-782aeaf1c3d9}</MetaDataID>
        public IList<FinanceFacade.IFisicalParty> FisicalParties
        {
            get
            {
                return GetRunTime().FisicalParties;
            }
        }
        /// <MetaDataID>{9919d8d1-7601-428d-94f5-933aabf2ed42}</MetaDataID>
        public ServiceContextResources ServiceContextResources
        {
            get
            {
                return GetRunTime().ServiceContextResources;
            }
        }

        /// <MetaDataID>{201f8944-163a-41af-9110-b650a5bce25b}</MetaDataID>
        public ServiceContextHumanResources ServiceContextHumanResources
        {
            get
            {
                return GetRunTime().ServiceContextHumanResources;
            }
        }

        /// <MetaDataID>{69bf337b-02fb-4706-a312-4f212a0bf333}</MetaDataID>
        public IList<FlavourBusinessFacade.ServicesContextResources.IPreparationStation> PreparationStations
        {
            get
            {
                return GetRunTime().PreparationStations;
            }
        }


        /// <MetaDataID>{6b6fc373-9d43-4dc7-8b52-de6c7751c59d}</MetaDataID>
        object ServicesContextLock = new object();

        /// <MetaDataID>{faf51a66-19f0-492c-9981-d2e0e3d1415b}</MetaDataID>
        IFlavoursServicesContextRuntime FlavoursServicesContextRuntime;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{118c775e-adb8-4234-b3cb-9637b1bd3458}</MetaDataID>
        public IFlavoursServicesContextRuntime GetRunTime()
        {




            lock (ServicesContextLock)
            {
                if (FlavoursServicesContextRuntime != null)
                    return FlavoursServicesContextRuntime;


                try
                {
                    FlavoursServicesContextManagment.FlavoursServicesEventLog.WriteEntry("Get runtime :" + DateTime.Now.ToLongTimeString());
                }
                catch (Exception error)
                {
                }

                try
                {
                    string storageIdentity = null;
                    var fbstorage = (from storage in (Owner as Organization).Storages
                                     where storage.FlavourStorageType == OrganizationStorages.OperativeRestaurantMenu
                                     select storage).FirstOrDefault();

                    var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri + fbstorage.Url;
                    var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                    var storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };



                    if (ComputingCluster.CurrentComputingResource == ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(RunAtContext.ContextID))
                    {

                        string publicServerUrl = ComputingCluster.CurrentComputingCluster.GetRoleInstancePublicServerUrl(RunAtContext.ContextID);
                        FlavoursServicesContextManagment flavoursServicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(publicServerUrl, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).FullName, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).Assembly.FullName) as FlavourBusinessManager.FlavoursServicesContextManagment;
                        string storageName = ContextStorageName;
                        string storageLocation = "DevStorage";

                        try
                        {
                            FlavoursServicesContextManagment.FlavoursServicesEventLog.WriteEntry("Get runtime remote call:" + DateTime.Now.ToLongTimeString());
                        }
                        catch (Exception error)
                        {
                        }
                        FlavoursServicesContextRuntime = flavoursServicesContextManagment.GetServicesContextRuntime(storageName, storageLocation, this.ServicesContextIdentity, Owner.Identity, storageRef, true);
                        if (FlavoursServicesContextRuntime != null)
                            FlavoursServicesContextRuntime.Description = Description;
                        try
                        {
#if DEBUG
                            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));
                            var foodTypeTags = (from s_foodTypeTag in storage.GetObjectCollection<FoodTypeTag>()
                                                select s_foodTypeTag).ToList();

                            string foodCategories = "Καφέδες(47); Σουβλάκια(61); Pizza(48); Κινέζικη(18); Κρέπες(21); Burgers(46); Sushi(26); Γλυκά(24); Μαγειρευτά(40); Ζυμαρικά(36); Μεξικάνικη(5); Νηστίσιμα(16); Βάφλες(10); Ινδική(13); Vegan(8); Brunch(20); Vegetarian(5); Hot Dog(5); Γαλακτοκομικά(1); Ασιατική(24); Σφολιάτες(8); Θαλασσινά(21); Σαλάτες(44); Ζαχαροπλαστείο(10); Cocktails(16); Ιταλική(16); Ψητά - Grill(95); Αρτοποιήματα(2); Sandwich(33); Παγωτό(21); Kebab(12); Πεϊνιρλί(7); Ποτά(13); Ethnic(10); Cool food(6); Κοτόπουλα(5); Ανατολίτικη(8); Φαλάφελ(6); Τσέχικη(1); Κεντροευρωπαϊκή(1); Μεζεδοπωλείο(7); Μεσογειακή(9); Ελληνική(9); Πατσάς(1); Snacks(4); Donuts(2); Πιροσκί(1); Λουκουμάδες(3)";

                            var foodCategoriesNames = foodCategories.Split(';').Select(x => x.Split('(')[0]).ToList();

                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                foreach (var foodCategory in foodCategoriesNames)
                                {

                                    if (foodTypeTags.Where(x => x.Name.Trim() == foodCategory.Trim()).Count() == 0)
                                    {
                                        var foodTypeTag = new FoodTypeTag(); ;
                                        foodTypeTag.Name = foodCategory.Trim();
                                        ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodTypeTag);
                                    }

                                } 
                                stateTransition.Consistent = true;
                            }

#endif
                        }
                        catch (Exception error)
                        {
                        }
                        return FlavoursServicesContextRuntime;
                    }
                    else
                    {
                        string publicServerUrl = ComputingCluster.CurrentComputingCluster.GetRoleInstancePublicServerUrl(RunAtContext.ContextID);

                        FlavoursServicesContextManagment flavoursServicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(publicServerUrl, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).FullName, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).Assembly.FullName) as FlavourBusinessManager.FlavoursServicesContextManagment;



                        string storageName = ContextStorageName;
                        string storageLocation = "DevStorage";
                        //string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

                        try
                        {
                            FlavoursServicesContextManagment.FlavoursServicesEventLog.WriteEntry("Get runtime remote call:" + DateTime.Now.ToLongTimeString());
                        }
                        catch (Exception error)
                        {
                        }

                        OOAdvantech.Linq.Storage storage = null;

                        //storage.GetObjectCollection<FlavoursServicesContext>
                        FlavoursServicesContextRuntime = flavoursServicesContextManagment.GetServicesContextRuntime(storageName, storageLocation, this.ServicesContextIdentity, Owner.Identity, storageRef, true);
                        if (FlavoursServicesContextRuntime != null)
                            FlavoursServicesContextRuntime.ObjectChangeState += FlavoursServicesContextRuntime_ObjectChangeState;

                        if (FlavoursServicesContextRuntime != null)
                            FlavoursServicesContextRuntime.Description = Description;


                        var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(FlavoursServicesContextRuntime) as OOAdvantech.Remoting.RestApi.Proxy;

                        AuthUserRef authUserRef = AuthUserRef.GetAuthUserRefForRole(Owner.OAuthUserIdentity);


                        IServiceContextSupervisor masterSupervisor = authUserRef.GetRoles().Where(x => UserData.UserRole.GetRoleType(x.TypeFullName) == RoleType.ServiceContextSupervisor && (x.RoleObject is IServiceContextSupervisor) && (x.RoleObject as IServiceContextSupervisor).ServicesContextIdentity == this.ServicesContextIdentity).Select(x => x.RoleObject).OfType<IServiceContextSupervisor>().FirstOrDefault();

                        if (masterSupervisor == null)
                        {

                            masterSupervisor = FlavoursServicesContextRuntime.ServiceContextHumanResources.Supervisors.Where(x => x.OAuthUserIdentity == Owner.OAuthUserIdentity).FirstOrDefault();
                            if (masterSupervisor != null && !authUserRef.HasRole(masterSupervisor))
                                authUserRef.AddRole(masterSupervisor);
                            else if (masterSupervisor == null)
                            {
                                UserData userData = new UserData()
                                {
                                    Identity = authUserRef.GetIdentity(),
                                    Email = authUserRef.Email,
                                    FullName = authUserRef.FullName,
                                    PhoneNumber = authUserRef.PhoneNumber,
                                    Address = authUserRef.Address,
                                    PhotoUrl = authUserRef.PhotoUrl,
                                    Roles = authUserRef.GetRoles().Where(x => x.RoleObject is IUser).Select(x => new UserData.UserRole() { User = x.RoleObject as IUser, RoleType = UserData.UserRole.GetRoleType(x.TypeFullName) }).ToList()
                                };


                                int tries = 4;
                                while (tries > 0)
                                {
                                    tries--;
                                    string supervisorAssignKey = FlavoursServicesContextRuntime.NewSupervisor();
                                    masterSupervisor = FlavoursServicesContextRuntime.AssignSupervisorUser(supervisorAssignKey, Owner.OAuthUserIdentity, authUserRef.FullName);
                                    if (masterSupervisor != null)
                                    {
                                        masterSupervisor.Name = authUserRef.FullName;
                                        authUserRef.AddRole(masterSupervisor);
                                        break;
                                    }
                                }
                            }

                        }
                        return FlavoursServicesContextRuntime;
                    }
                }
                catch (Exception error)
                {

                    throw;
                }

            }
        }

        /// <MetaDataID>{dcac4ddf-369d-4e2d-8ebb-a730bf1683fa}</MetaDataID>
        private void FlavoursServicesContextRuntime_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(ServiceContextHumanResources))
                ObjectChangeState?.Invoke(this, "ServiceContextHumanResources");
        }



        /// <MetaDataID>{be856d2b-a161-49ef-846b-37df21c68ffd}</MetaDataID>
        private ObjectStorage OpenServicesContextStorageStorage()
        {
            ObjectStorage storageSession = null;
            string storageName = ContextStorageName;
            string storageLocation = "DevStorage";
            string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

            try
            {
                storageSession = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                // storageSession.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    storageSession = ObjectStorage.NewStorage(storageName,
                                                            storageLocation,
                                                            storageType);
                }
                else
                    throw Error;
                try
                {
                    storageSession.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
                }
                catch (System.Exception Errore)
                {
                    int sdf = 0;
                }
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }

            return storageSession;
        }





        /// <MetaDataID>{71d65deb-60c4-4cda-97d3-227ffe95f45b}</MetaDataID>
        public IServiceArea NewServiceArea()
        {
            return GetRunTime().NewServiceArea();
            //var objectStorage = OpenServicesContextStorageStorage();

            //ServicesContextResources.ServiceArea serviceArea = new ServicesContextResources.ServiceArea();
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{

            //    serviceArea.Description = Properties.Resources.DefaultServiceAreaDescription;
            //    serviceArea.ServicesContextIdentity = this.ServicesContextIdentity;
            //    objectStorage.CommitTransientObjectState(serviceArea);

            //    stateTransition.Consistent = true;
            //}
            //return serviceArea;


        }

        /// <MetaDataID>{d666e5e8-12ee-4ce4-bd69-cd45c7584e5c}</MetaDataID>
        public void RemoveServiceArea(IServiceArea serviceArea)
        {
            GetRunTime().RemoveServiceArea(serviceArea);
            //ObjectStorage.DeleteObject(serviceArea);

        }

        /// <MetaDataID>{65920937-f6f4-4472-870b-ff1b9081ed7f}</MetaDataID>
        public void LaunchCallerIDServer()
        {

            GetRunTime().LaunchCallerIDServer();

        }

        /// <MetaDataID>{64f78232-2293-4106-8cc6-a9be2ceb5dc6}</MetaDataID>
        public void RemoveCallerIDServer()
        {
            GetRunTime().RemoveCallerIDServer();
            //if (CallerIDServer != null)
            //{
            //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //    {
            //        ObjectStorage.DeleteObject(_CallerIDServer);
            //        stateTransition.Consistent = true;
            //    }
            //}
        }

        /// <MetaDataID>{0ef48d10-4577-44c9-9b44-192c3b375fae}</MetaDataID>
        public ICashierStation NewCashierStation()
        {
            return GetRunTime().NewCashierStation();
        }

        /// <MetaDataID>{4a448ec9-8101-478c-bf69-e38742edc357}</MetaDataID>
        public void RemoveCashierStation(ICashierStation cashierStation)
        {
            GetRunTime().RemoveCashierStation(cashierStation);
        }



        /// <MetaDataID>{4613e465-5c23-4a01-8e6d-8bea90712bf9}</MetaDataID>
        public FinanceFacade.IFisicalParty NewFisicalParty()
        {
            return GetRunTime().NewFisicalParty();
        }

        /// <MetaDataID>{10a844d5-f3c6-4ffd-96a5-af1261be18b0}</MetaDataID>
        public void RemoveFisicalParty(FinanceFacade.IFisicalParty fisicalParty)
        {
            GetRunTime().RemoveFisicalParty(fisicalParty);
        }
        /// <MetaDataID>{352ae357-09a4-4a78-9181-354fc66150d0}</MetaDataID>
        public void UpdateFisicalParty(FinanceFacade.IFisicalParty fisicalParty)
        {
            GetRunTime().UpdateFisicalParty(fisicalParty);
        }
        /// <MetaDataID>{f7fbc80c-50ed-436e-831b-eafc0e94b61b}</MetaDataID>
        public OrganizationStorageRef GetHallLayoutStorageForServiceArea(IServiceArea serviceArea)
        {
            return GetRunTime().GetHallLayoutStorageForServiceArea(serviceArea);
        }

        /// <MetaDataID>{09920f5b-21b3-4ba9-ae37-ecb6b90bbd8c}</MetaDataID>
        public string NewWaiter()
        {
            return GetRunTime().NewWaiter();

        }

        /// <MetaDataID>{ca9b1bc6-04b4-408a-83c6-079e836e31c7}</MetaDataID>
        public void RemoveWaiter(IWaiter waiter)
        {
            GetRunTime().RemoveWaiter(waiter);

        }

        /// <MetaDataID>{cfb2c9e3-e4d3-4c6c-9ddc-d7fee936a836}</MetaDataID>
        public void RemoveHomeDeliveryService()
        {
            GetRunTime().RemoveHomeDeliveryService();
        }

        /// <MetaDataID>{d3551fc2-7c11-4efa-96fd-aff02a9d9970}</MetaDataID>
        public void LaunchHomeDeliveryService()
        {
            GetRunTime().LaunchHomeDeliveryService();
        }

        /// <MetaDataID>{073ddaf2-3bf7-4a29-8938-6df63e777f81}</MetaDataID>
        public void RemoveFoodTypes(List<IFoodTypeTag> foodTypeTags)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var foodTypeTag in foodTypeTags)
                {
                    var serverFoodTypeTag = FlavoursServicesContextManagment.Current.FoodTypeTags.Where(x => x.Uri == foodTypeTag.Uri).FirstOrDefault();
                    _FoodTypes.Remove(serverFoodTypeTag);
                }
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{cac75434-043b-402a-9b8f-52802390a295}</MetaDataID>
        public void AddFoodTypes(List<IFoodTypeTag> foodTypeTags)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var foodTypeTag in foodTypeTags)
                {
                    var serverFoodTypeTag = FlavoursServicesContextManagment.Current.FoodTypeTags.Where(x => x.Uri == foodTypeTag.Uri).FirstOrDefault();
                    _FoodTypes.Add(serverFoodTypeTag);
                }
                stateTransition.Consistent = true;
            }
        }

     

        /// <MetaDataID>{1900ac60-fd30-4922-b07b-a93d61875010}</MetaDataID>
        public IUploadService UploadService { get => GetRunTime() as IUploadService; }

        /// <MetaDataID>{008ca56c-3e87-442e-8540-65eb36cf3837}</MetaDataID>
        public ISettings Settings
        {
            get
            {
                return GetRunTime().Settings;
            }
        }

        /// <MetaDataID>{97b2ae66-b89d-4e66-883b-e03bb1d3817a}</MetaDataID>
        public IHomeDeliveryServicePoint DeliveryServicePoint => GetRunTime().DeliveryServicePoint;

        /// <MetaDataID>{3fd3e0d5-cc05-489f-8278-ce9a00f8e851}</MetaDataID>
        public IList<ITakeAwayStation> TakeAwayStations => GetRunTime().TakeAwayStations;


        /// <MetaDataID>{7304523e-3bf9-4445-94de-dbe61fa2ee57}</MetaDataID>
        public List<IPaymentTerminal> PaymentTerminals => GetRunTime().PaymentTerminals;
    }
}
