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
        /// <MetaDataID>{07e6d9ad-97ef-4bf0-a4a8-91c1f5fe9224}</MetaDataID>
        public void RemovePreparationStation(IPreparationStation prepartionStation)
        {
            GetRunTime().RemovePreparationStation(prepartionStation);
        }


        /// <MetaDataID>{52258872-5176-411c-8cea-4173e715bd41}</MetaDataID>
        public IPreparationStation NewPreparationStation()
        {
            return GetRunTime().NewPreparationStation();
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

            if (FlavoursServicesContextRuntime != null)
                return FlavoursServicesContextRuntime;


            lock (ServicesContextLock)
            {
                string storageIdentity = null;
                var fbstorage = (from storage in (Owner as Organization).Storages
                                 where storage.FlavourStorageType == OrganizationStorages.OperativeRestaurantMenu
                                 select storage).FirstOrDefault();

                var storageUrl = RawStorageCloudBlob.BlobsStorageHttpAbsoluteUri+  fbstorage.Url;
                var lastModified = RawStorageCloudBlob.GetBlobLastModified(fbstorage.Url);

                var storageRef = new OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, FlavourStorageType = fbstorage.FlavourStorageType, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = storageUrl, TimeStamp = lastModified.Value.UtcDateTime };



                if (ComputingCluster.CurrentComputingResource == ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(RunAtContext.ContextID))
                {

                    string publicServerUrl = ComputingCluster.CurrentComputingCluster.GetRoleInstancePublicServerUrl(RunAtContext.ContextID);
                    FlavoursServicesContextManagment flavoursServicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(publicServerUrl, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).FullName, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).Assembly.FullName) as FlavourBusinessManager.FlavoursServicesContextManagment;
                    string storageName = ContextStorageName;
                    string storageLocation = "DevStorage";

                    FlavoursServicesContextRuntime = flavoursServicesContextManagment.GetServicesContextRuntime(storageName, storageLocation, this.ServicesContextIdentity, Owner.Identity, storageRef, true);
                    if (FlavoursServicesContextRuntime != null)
                        FlavoursServicesContextRuntime.Description = Description;

                    return FlavoursServicesContextRuntime;
                }
                else
                {
                    string publicServerUrl = ComputingCluster.CurrentComputingCluster.GetRoleInstancePublicServerUrl(RunAtContext.ContextID);

                    FlavoursServicesContextManagment flavoursServicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(publicServerUrl, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).FullName, typeof(FlavourBusinessManager.FlavoursServicesContextManagment).Assembly.FullName) as FlavourBusinessManager.FlavoursServicesContextManagment;



                    string storageName = ContextStorageName;
                    string storageLocation = "DevStorage";
                    //string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

                    FlavoursServicesContextRuntime = flavoursServicesContextManagment.GetServicesContextRuntime(storageName, storageLocation, this.ServicesContextIdentity, Owner.Identity, storageRef, true);
                    if (FlavoursServicesContextRuntime != null)
                        FlavoursServicesContextRuntime.ObjectChangeState += FlavoursServicesContextRuntime_ObjectChangeState;

                    if (FlavoursServicesContextRuntime != null)
                        FlavoursServicesContextRuntime.Description = Description;


                    var proxy = System.Runtime.Remoting.RemotingServices.GetRealProxy(FlavoursServicesContextRuntime) as OOAdvantech.Remoting.RestApi.Proxy;

                    AuthUserRef authUserRef = AuthUserRef.GetAuthUserRefForRole(Owner.SignUpUserIdentity);


                    IServiceContextSupervisor masterSupervisor = authUserRef.GetRoles().Where(x => UserData.UserRole.GetRoleType(x.TypeFullName) == UserData.RoleType.ServiceContextSupervisor && (x.RoleObject is IServiceContextSupervisor) && (x.RoleObject as IServiceContextSupervisor).ServicesContextIdentity == this.ServicesContextIdentity).Select(x => x.RoleObject).OfType<IServiceContextSupervisor>().FirstOrDefault();

                    if (masterSupervisor == null)
                    {

                        masterSupervisor = FlavoursServicesContextRuntime.ServiceContextHumanResources.Supervisors.Where(x => x.SignUpUserIdentity == Owner.SignUpUserIdentity).FirstOrDefault();
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
                                masterSupervisor = FlavoursServicesContextRuntime.AssignSupervisorUser(supervisorAssignKey, Owner.SignUpUserIdentity, authUserRef.FullName);
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
            //if (CallerIDServer == null)
            //{
            //    var objectStorage = OpenServicesContextStorageStorage();

            //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //    {
            //        _CallerIDServer = new CallerIDServer();
            //        _CallerIDServer.ServicesContextIdentity = this.ServicesContextIdentity;
            //        objectStorage.CommitTransientObjectState(_CallerIDServer);

            //        stateTransition.Consistent = true;
            //    }

            //}
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



        public FinanceFacade.IFisicalParty NewFisicalParty()
        {
            return GetRunTime().NewFisicalParty();
        }

        public void RemoveFisicalParty(FinanceFacade.IFisicalParty fisicalParty)
        {
            GetRunTime().RemoveFisicalParty(fisicalParty);
        }
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

        /// <MetaDataID>{1900ac60-fd30-4922-b07b-a93d61875010}</MetaDataID>
        public IUploadService UploadService { get => GetRunTime() as IUploadService; }
    }
}
