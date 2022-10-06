using System;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.Remoting;
using System.Linq;
using OOAdvantech;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessToolKit;
using FlavourBusinessManager.ServicePointRunTime;
using System.Collections.Generic;
using System.IO;
using OOAdvantech.Remoting.RestApi;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{c5aa979a-e6cc-4d31-bfb6-5029a096e714}</MetaDataID>
    public class FlavoursServicesContextManagment : MonoStateClass, IFlavoursServicesContextManagment, IExtMarshalByRefObject
    {

        /// <MetaDataID>{cda691fe-3d33-4e1d-90d9-c6946d4a4560}</MetaDataID>
        public static System.Timers.Timer FlavoursServicesContextsMonitoringTimer;
        /// <MetaDataID>{352e066e-131d-482b-8e2b-5badc8b5c738}</MetaDataID>
        public FlavoursServicesContextManagment()
        {

            if (FlavoursServicesContextsMonitoringTimer == null)
            {
                OnFlavoursServicesContextsMonitoring(this, default(System.Timers.ElapsedEventArgs));
                FlavoursServicesContextsMonitoringTimer = new System.Timers.Timer(20000);
                FlavoursServicesContextsMonitoringTimer.Enabled = true;
                FlavoursServicesContextsMonitoringTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnFlavoursServicesContextsMonitoring);
                FlavoursServicesContextsMonitoringTimer.Start();
            }

        }

        /// <MetaDataID>{b7ee9e11-5fbd-493e-bd04-6c01b99983fc}</MetaDataID>
        public static FlavoursServicesContextManagment Current
        {
            get
            {
                return GetInstance(typeof(FlavoursServicesContextManagment)) as FlavoursServicesContextManagment;
            }

        }



        /// <MetaDataID>{c486b6cc-f89a-4623-b822-27426444c14d}</MetaDataID>
        static void OnFlavoursServicesContextsMonitoring(object source, System.Timers.ElapsedEventArgs e)
        {

            //bool utd = true;
            //if (utd)
            //    return;

            foreach (var flavoursServicesContext in FlavoursServicesContext.ActiveFlavoursServicesContexts)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    var organizationIdentity = flavoursServicesContext.GetRunTime().OrganizationIdentity;
                });
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{665cfb4a-ee22-4494-9cfc-b0b6ad526bab}</MetaDataID>
        public ClientSessionData GetClientSession(string servicePointIdentity, string clientName, string clientDeviceID, string deviceFirebaseToken, bool create)
        {
            string servicesContextIdentity = null;
            string mealInvitationSessionID = null;
            string[] servicePointIdentityParts = servicePointIdentity.Split(';');
            if (servicePointIdentityParts.Length == 2)
            {
                servicesContextIdentity = servicePointIdentityParts[0];
                servicePointIdentity = servicePointIdentityParts[1];
            }
            else
            {
                servicesContextIdentity = servicePointIdentityParts[1];
                servicePointIdentity = servicePointIdentityParts[2];
                mealInvitationSessionID = servicePointIdentityParts[3];

            }



            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);

            var graphicMenus = (flavoursServicesContext.Owner as Organization).UnSafeGraphicMenus;


            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();
            string orgIdentity = flavoursServicesContext.Owner.Identity;

            var clientSession = flavoursServicesContextRunTime.GetClientSession(servicePointIdentity, mealInvitationSessionID, clientName, clientDeviceID, deviceFirebaseToken, orgIdentity, graphicMenus, create);

            return clientSession;
        }

        /// <MetaDataID>{4b6acc8b-1b62-41db-933f-9840a7eb5a2c}</MetaDataID>
        public FlavourBusinessFacade.HumanResources.IWaiter AssignWaiterUser(string waiterAssignKey)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new System.Security.Authentication.AuthenticationException();

            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);

            if (authUser == null)
                throw new System.Security.Authentication.AuthenticationException("User isn't signed up.");

            string[] servicePointIdentityParts = waiterAssignKey.Split(';');
            string servicesContextIdentity = servicePointIdentityParts[0];

            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);
            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();

            var waiter = flavoursServicesContextRunTime.AssignWaiterUser(waiterAssignKey, authUser.User_ID, authUserRef.FullName);
            authUserRef.AddRole(waiter);


            return waiter;
        }

        /// <MetaDataID>{ce23edf9-4f91-4120-9a43-7e55e8edb31b}</MetaDataID>
        public FlavourBusinessFacade.HumanResources.IServiceContextSupervisor AssignSupervisorUser(string supervisorAssignKey)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new System.Security.Authentication.AuthenticationException();

            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);

            if (authUser == null)
                throw new System.Security.Authentication.AuthenticationException("User isn't signed up.");

            string[] servicePointIdentityParts = supervisorAssignKey.Split(';');
            string servicesContextIdentity = servicePointIdentityParts[0];

            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);
            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();

            var serviceContextSupervisor = flavoursServicesContextRunTime.AssignSupervisorUser(supervisorAssignKey, authUser.User_ID, authUserRef.FullName);
            authUserRef.AddRole(serviceContextSupervisor);


            return serviceContextSupervisor;
        }


        /// <MetaDataID>{b3cb9fa2-46bc-4703-b75c-6fca388695f7}</MetaDataID>
        public IHallLayout GetHallLayout(string servicePointIdentity)
        {

            string servicesContextIdentity = servicePointIdentity.Substring(0, servicePointIdentity.IndexOf(";"));
            servicePointIdentity = servicePointIdentity.Substring(servicePointIdentity.IndexOf(";") + 1);
            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);

            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();

            IHallLayout hallLayout = flavoursServicesContextRunTime.GetHallLayout(servicePointIdentity);
            return hallLayout;
        }

        /// <MetaDataID>{eace50d6-ed5b-44f1-8527-94586ebc33f4}</MetaDataID>
        public OrganizationStorageRef GetMenu(string servicePointIdentity)
        {
            string servicesContextIdentity = servicePointIdentity.Substring(0, servicePointIdentity.IndexOf(";"));
            servicePointIdentity = servicePointIdentity.Substring(servicePointIdentity.IndexOf(";") + 1);

            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);

            //var serviceArea = (from theServiceArea in flavoursServicesContext.ServiceAreas
            //                   from servicPoint in theServiceArea.ServicePoints
            //                   where servicPoint.ServicesPointIdentity == servicePointIdentity
            //                   select theServiceArea).FirstOrDefault();
            var graphicMenus = (flavoursServicesContext.Owner as Organization).UnSafeGraphicMenus;
            var graphicMenu = graphicMenus.FirstOrDefault();

            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();

            var flavoursServicesContexGraphicMenu = flavoursServicesContextRunTime.GraphicMenus.FirstOrDefault();

            if (flavoursServicesContexGraphicMenu != null)
            {
                graphicMenu = (from gMenu in graphicMenus where gMenu.StorageIdentity == flavoursServicesContexGraphicMenu.StorageIdentity select gMenu).FirstOrDefault();
            }

            string orgIdentity = flavoursServicesContext.Owner.Identity;

            string versionSuffix = "";
            if (!string.IsNullOrWhiteSpace(graphicMenu.Version))
                versionSuffix = "/" + graphicMenu.Version;
            else
                versionSuffix = "";

            graphicMenu.StorageUrl = RawStorageCloudBlob.RootUri + string.Format("/usersfolder/{0}/Menus/{1}{3}/{2}.json", orgIdentity, graphicMenu.StorageIdentity, graphicMenu.Name, versionSuffix);

            return graphicMenu;

        }



        /// <MetaDataID>{1726fda9-eed5-4e8c-8a04-a3fc80a093bc}</MetaDataID>
        public IPreparationStationRuntime GetPreparationStationRuntime(string preparationStationCredentialKey)
        {
            string servicesContextIdentity = preparationStationCredentialKey.Substring(0, preparationStationCredentialKey.IndexOf("_"));
            string preparationStationIdentity = preparationStationCredentialKey.Substring(preparationStationCredentialKey.IndexOf("_") + 1);

            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);
            if (flavoursServicesContext == null)
                return null;

            var graphicMenus = (flavoursServicesContext.Owner as Organization).UnSafeGraphicMenus;


            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();
            string orgIdentity = flavoursServicesContext.Owner.Identity;

            var preparationStation = flavoursServicesContextRunTime.GetPreparationStationRuntime(preparationStationCredentialKey);//, clientName, clientDeviceID, deviceFirebaseToken, clientIdentity, orgIdentity, graphicMenus);

            return preparationStation;


        }




        /// <MetaDataID>{7dc4b42e-3028-4c25-811c-86eda8f65811}</MetaDataID>
        public ICashiersStationRuntime GetCashiersStationRuntime(string communicationCredentialKey)
        {
            string servicesContextIdentity = communicationCredentialKey.Substring(0, communicationCredentialKey.IndexOf("_"));
            string preparationStationIdentity = communicationCredentialKey.Substring(communicationCredentialKey.IndexOf("_") + 1);

            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);
            if (flavoursServicesContext == null)
                return null;

            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();


            ICashiersStationRuntime cashiersStation = flavoursServicesContextRunTime.GetCashiersStationRuntime(communicationCredentialKey);//, clientName, clientDeviceID, deviceFirebaseToken, clientIdentity, orgIdentity, graphicMenus);

            return cashiersStation;


        }

        /// <MetaDataID>{9797a103-75d3-4431-b2d4-593249086772}</MetaDataID>
        List<IFoodTypeTag> _FoodTypeTags;
        /// <MetaDataID>{0ca6621f-99eb-489e-9ddf-7a6a55bdf29d}</MetaDataID>
        public System.Collections.Generic.List<IFoodTypeTag> FoodTypeTags
        {
            get
            {
                if (_FoodTypeTags == null)
                {
                    var objectStorage = FlavoursServicesContext.OpenFlavourBusinessesStorage();
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                    _FoodTypeTags = (from s_foodTypeTag in storage.GetObjectCollection<IFoodTypeTag>()
                                     select s_foodTypeTag).ToList();
                }
                return _FoodTypeTags.ToList();
            }
        }
        /// <MetaDataID>{9696eb0e-cbca-4371-af36-a7093c85dc15}</MetaDataID>
        public IServicePoint GetServicePoint(string servicePointIdentity)
        {
            return null;
        }

        /// <MetaDataID>{6b140fca-b1f5-4d7c-b4ec-fd93b8565076}</MetaDataID>
        static object EventLogLock = new object();
        /// <MetaDataID>{974a39c6-8ec9-4e91-a4ef-e09cbb3bd1a5}</MetaDataID>
        static System.Diagnostics.EventLog _FlavoursServicesEventLog;
        /// <MetaDataID>{bc3d83e0-d26c-42bc-85df-697a6581316c}</MetaDataID>
        public static System.Diagnostics.EventLog FlavoursServicesEventLog
        {
            get
            {

                lock (EventLogLock)
                {
                    if (_FlavoursServicesEventLog != null)
                        return _FlavoursServicesEventLog;
                    if (!System.Diagnostics.EventLog.SourceExists("FlavoursServices", "."))
                        System.Diagnostics.EventLog.CreateEventSource("FlavoursServices", "Microneme");
                    System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                    myLog.Source = "FlavoursServices";
                    _FlavoursServicesEventLog = myLog;
                    return _FlavoursServicesEventLog;
                }
            }
        }
        /// <MetaDataID>{f20f9e9d-c06f-440b-afdc-1a8f38dd82ea}</MetaDataID>
        public IFlavoursServicesContextRuntime GetServicesContextRuntime(string storageName, string storageLocation, string servicesContextIdentity, string organizationIdentity, OrganizationStorageRef restaurantMenusDataStorageRef, bool create = false)
        {
            //7f9bde62e6da45dc8c5661ee2220a7b0_bf55f9a68e5048d0b6b1676fa18ca64a

            storageLocation = "DevStorage";
            string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";


            try
            {
                FlavoursServicesContextManagment.FlavoursServicesEventLog.WriteEntry("In GetServicesContextRuntime:" + DateTime.Now.ToLongTimeString());
            }
            catch (Exception error)
            {
            }


            ObjectStorage objectStorage = null;
            try
            {
                var time = System.DateTime.Now;

                objectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey);

                var ss = System.DateTime.Now - time;
                if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                    System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
                System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                myLog.Source = "PersistencySystem";
                myLog.WriteEntry(string.Format("Open '{0}' storage timespan {1}", storageName, ss.ToString()), System.Diagnostics.EventLogEntryType.Information);



                lock (objectStorage)
                {
                    try
                    {

                        if (objectStorage.StorageMetaData.CheckForVersionUpgrate(typeof(IOrganization).Assembly.FullName) || objectStorage.StorageMetaData.CheckForVersionUpgrate(typeof(FinanceFacade.Transaction).Assembly.FullName))
                        {
                            objectStorage.StorageMetaData.RegisterComponent(typeof(FinanceFacade.Transaction).Assembly.FullName);
                            objectStorage.StorageMetaData.RegisterComponent(typeof(IOrganization).Assembly.FullName);
                            objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
                        }
                        else if (objectStorage.StorageMetaData.CheckForVersionUpgrate(typeof(Organization).Assembly.FullName))
                        {
                            objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);
                        }
                    }
                    catch (Exception error)
                    {
                        throw;
                    }
                }




                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
                var storagesClient = new OOAdvantech.PersistenceLayer.StoragesClient(httpClient);
                storagesClient.BaseUrl = serverUrl;
                var task = storagesClient.GetAsync(objectStorage.StorageMetaData.StorageIdentity);
                task.Wait();
                var storageMetaData = task.Result;
                if (storageMetaData == null || storageMetaData.StorageIdentity == null)
                    storagesClient.PostAsync(objectStorage.StorageMetaData, true);




                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var servicesContextRunTime = (from theServicePointRun in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                                              where theServicePointRun.ServicesContextIdentity == servicesContextIdentity
                                              select theServicePointRun).FirstOrDefault();



                if (servicesContextRunTime == null && create)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        servicesContextRunTime = new ServicePointRunTime.ServicesContextRunTime();
                        servicesContextRunTime.OrganizationIdentity = organizationIdentity;
                        servicesContextRunTime.ServicesContextIdentity = servicesContextIdentity;

                        servicesContextRunTime.SetRestaurantMenusData(restaurantMenusDataStorageRef);
                        objectStorage.CommitTransientObjectState(servicesContextRunTime);

                        stateTransition.Consistent = true;
                    }
                    //PublishMenuRestaurantMenuData(servicesContextRunTime);
                }
                else
                {

                    // bool publishRestaurantMenuData = false;
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {

                        //publishRestaurantMenuData = servicesContextRunTime.RestaurantMenuDataLastModified.ToUniversalTime() != restaurantMenusDataStorageRef.TimeStamp.ToUniversalTime();

                        servicesContextRunTime.SetRestaurantMenusData(restaurantMenusDataStorageRef);

                        if (servicesContextRunTime.OrganizationIdentity != organizationIdentity)
                            servicesContextRunTime.OrganizationIdentity = organizationIdentity;

                        stateTransition.Consistent = true;
                    }
                    //if (publishRestaurantMenuData)
                    //    PublishMenuRestaurantMenuData(servicesContextRunTime);
                    //else
                    //{
                    //    var restaurantMenusData = servicesContextRunTime.Storages.Where(x => x.FlavourStorageType == OrganizationStorages.OperativeRestaurantMenu).FirstOrDefault();
                    //    string serverStorageFolder = GetVersionFolder(servicesContextRunTime, restaurantMenusData.Version);
                    //    string jsonFileName = serverStorageFolder + restaurantMenusData.Name + ".json";
                    //    IFileManager fileManager = new BlobFileManager(RawStorageCloudBlob.CloudStorageAccount);
                    //    if (!fileManager.Exist(jsonFileName))
                    //        WritePublicRestaurantMenuData(servicesContextRunTime, jsonFileName);
                    //}
                }

                return servicesContextRunTime;
            }
            catch (OOAdvantech.PersistenceLayer.StorageException Error)
            {
                if (Error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    objectStorage = ObjectStorage.NewStorage(storageName, storageLocation, storageType);
                    System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();


                    string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
                    System.Threading.Tasks.Task.Run(async () =>
                    {
                        StoragesClient storagesClient = new StoragesClient(httpClient);
                        storagesClient.BaseUrl = serverUrl;
                        string res = await storagesClient.PostAsync(objectStorage.StorageMetaData, true);
                    });
                }
                else
                    throw Error;

                objectStorage.StorageMetaData.RegisterComponent(typeof(IOrganization).Assembly.FullName);
                objectStorage.StorageMetaData.RegisterComponent(typeof(Organization).Assembly.FullName);

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var servicesContextRunTime = (from theServicePointRun in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>()
                                              where theServicePointRun.ServicesContextIdentity == servicesContextIdentity
                                              select theServicePointRun).FirstOrDefault();



                if (servicesContextRunTime == null && create)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        servicesContextRunTime = new ServicePointRunTime.ServicesContextRunTime();
                        servicesContextRunTime.ServicesContextIdentity = servicesContextIdentity;
                        servicesContextRunTime.OrganizationIdentity = organizationIdentity;
                        objectStorage.CommitTransientObjectState(servicesContextRunTime);

                        servicesContextRunTime.SetRestaurantMenusData(restaurantMenusDataStorageRef);

                        stateTransition.Consistent = true;
                    }
                    //PublishMenuRestaurantMenuData(servicesContextRunTime);
                }
                else
                {
                    //bool publishRestaurantMenuData = false;
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        servicesContextRunTime.SetRestaurantMenusData(restaurantMenusDataStorageRef);

                        //publishRestaurantMenuData = servicesContextRunTime.RestaurantMenuDataLastModified.ToUniversalTime() != restaurantMenusDataStorageRef.TimeStamp.ToUniversalTime();

                        if (servicesContextRunTime.OrganizationIdentity != organizationIdentity)
                            servicesContextRunTime.OrganizationIdentity = organizationIdentity;

                        stateTransition.Consistent = true;
                    }
                    //if (publishRestaurantMenuData)
                    //    PublishMenuRestaurantMenuData(servicesContextRunTime);
                    //else
                    //{
                    //    var restaurantMenusData = servicesContextRunTime.Storages.Where(x => x.FlavourStorageType == OrganizationStorages.RestaurantMenus).FirstOrDefault();
                    //    string serverStorageFolder = GetVersionFolder(servicesContextRunTime, restaurantMenusData.Version);
                    //    string jsonFileName = serverStorageFolder + restaurantMenusData.Name + ".json";
                    //    IFileManager fileManager = new BlobFileManager(RawStorageCloudBlob.CloudStorageAccount);
                    //    if (!fileManager.Exist(jsonFileName))
                    //        WritePublicRestaurantMenuData(servicesContextRunTime, jsonFileName);
                    //}


                }

                return servicesContextRunTime;
            }
            catch (System.Exception Error)
            {
                int tt = 0;
            }

            return null;
        }

        /// <MetaDataID>{a5dabc66-3410-40ee-ae82-84e357744b7d}</MetaDataID>
        public static void Init()
        {
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl;

            IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));

        }

        ///// <MetaDataID>{e316c772-4861-4a2a-8183-e656ff7a011a}</MetaDataID>
        //private void PublishMenuRestaurantMenuData(ServicesContextRunTime servicesContextRunTime)
        //{

        //    IFileManager fileManager = new BlobFileManager(RawStorageCloudBlob.CloudStorageAccount);
        //    var restaurantMenusData = servicesContextRunTime.Storages.Where(x => x.FlavourStorageType == OrganizationStorages.RestaurantMenus).FirstOrDefault();
        //    string version = "";
        //    string oldVersion = restaurantMenusData.Version;
        //    if (string.IsNullOrWhiteSpace(oldVersion))
        //    {
        //        version = "v1";
        //    }
        //    else
        //    {
        //        int v = int.Parse(oldVersion.Replace("v", ""));
        //        v++;
        //        version = "v" + v.ToString();
        //    }

        //    string previousVersionServerStorageFolder = GetVersionFolder(servicesContextRunTime, oldVersion);

        //    string serverStorageFolder = GetVersionFolder(servicesContextRunTime, version);
        //    string jsonFileName = serverStorageFolder + restaurantMenusData.Name + ".json";
        //    WritePublicRestaurantMenuData(servicesContextRunTime, jsonFileName);

        //    if (fileManager != null)
        //    {
        //        jsonFileName = previousVersionServerStorageFolder + restaurantMenusData.Name + ".json";
        //        fileManager.GetBlobInfo(jsonFileName).DeleteIfExists();
        //    }

        //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
        //    {
        //        restaurantMenusData.Version = version;
        //        stateTransition.Consistent = true;
        //    }


        //    //jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
        //    //var sss = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.IMenuItem>>(jsonEx, jSetttings);
        //}

        ///// <MetaDataID>{5643d261-95ed-4c57-a6ee-9bb4994f3b1c}</MetaDataID>
        //private static void WritePublicRestaurantMenuData(ServicesContextRunTime servicesContextRunTime, string jsonFileName)
        //{
        //    IFileManager fileManager = new BlobFileManager(RawStorageCloudBlob.CloudStorageAccount);

        //    var fbstorage = (from servicesContextRunTimeStorage in servicesContextRunTime.Storages
        //                     where servicesContextRunTimeStorage.FlavourStorageType == FlavourBusinessFacade.OrganizationStorages.RestaurantMenus
        //                     select servicesContextRunTimeStorage).FirstOrDefault();
        //    var storageRef = new FlavourBusinessFacade.OrganizationStorageRef { StorageIdentity = fbstorage.StorageIdentity, Name = fbstorage.Name, Description = fbstorage.Description, StorageUrl = servicesContextRunTime.RestaurantMenuDataUri, TimeStamp = servicesContextRunTime.RestaurantMenuDataLastModified };

        //    RawStorageData rawStorageData = new RawStorageData(storageRef, null);


        //    OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenusData", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));
        //    Dictionary<object, object> mappedObject = new Dictionary<object, object>();
        //    List<MenuModel.IMenuItem> menuFoodItems = (from menuItem in restMenusData.GetObjectCollection<MenuModel.IMenuItem>()
        //                                               select menuItem).ToList().Select(x => new MenuModel.JsonViewModel.MenuFoodItem(x, mappedObject)).OfType<MenuModel.IMenuItem>().ToList();

        //    var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
        //    string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSetttings);
        //    MemoryStream jsonRestaurantMenuStream = new MemoryStream();
        //    byte[] jsonBuffer = System.Text.Encoding.UTF8.GetBytes(jsonEx);
        //    jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
        //    jsonRestaurantMenuStream.Position = 0;

        //    if (fileManager != null)
        //        fileManager.Upload(jsonFileName, jsonRestaurantMenuStream, "application/json");


        //}

        ///// <MetaDataID>{a6d225d5-a273-4988-bbc2-d773cbd1ecb9}</MetaDataID>
        //private static string GetVersionFolder(ServicesContextRunTime servicesContextRunTime, string version)
        //{
        //    string versionSuffix = "";
        //    if (!string.IsNullOrWhiteSpace(version))
        //        versionSuffix = "/" + version + "/";
        //    else
        //        versionSuffix = "/";

        //    string serverStorageFolder = string.Format("usersfolder/{0}{1}", servicesContextRunTime.OrganizationIdentity, versionSuffix);
        //    return serverStorageFolder;
        //}


        /// <MetaDataID>{93c6ffc2-01a1-4898-a165-47cdba49af2f}</MetaDataID>
        public List<HomeDeliveryServicePointInfo> GetNeighborhoodFoodServers(Coordinate location)
        {

            List<IHomeDeliveryServicePoint> deliveryServicePoints = new List<IHomeDeliveryServicePoint>();

            foreach (var flavoursServicesContext in FlavoursServicesContext.ActiveFlavoursServicesContexts)
            {

                var deliveryServicePoint = flavoursServicesContext.GetRunTime().DeliveryServicePoint;
                if (deliveryServicePoint != null)
                {
                    var placeOfDistribution = deliveryServicePoint.PlaceOfDistribution;
                    double distance = CalDistance(location.Latitude, location.Longitude, placeOfDistribution.Location.Latitude, placeOfDistribution.Location.Longitude);

                    var polyGon = new MapPolyGon(deliveryServicePoint.ServiceAreaMap);

                    if (polyGon.FindPoint(location.Latitude, location.Longitude))
                        deliveryServicePoints.Add(deliveryServicePoint);
                }
            }

            var servers = deliveryServicePoints.Select(x => new HomeDeliveryServicePointInfo(x)).ToList();

            return servers;

        }

        /// <MetaDataID>{d1f5e252-7a72-430c-950f-3efabc2835e0}</MetaDataID>
        internal static double CalDistance(double Lat1, double Long1, double Lat2, double Long2)
        {
            /*
                The Haversine formula according to Dr. Math.
                http://mathforum.org/library/drmath/view/51879.html
                
                dlon = lon2 - lon1
                dlat = lat2 - lat1
                a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
                c = 2 * atan2(sqrt(a), sqrt(1-a)) 
                d = R * c
                
                Where
                    * dlon is the change in longtitude
                    * dlat is the change in latitude
                    * c is the great circle distance in Radians.
                    * R is the radius of a spherical Earth.
                    * The locations of the two points in 
                        spherical coordinates (longtitude and 
                        latitude) are lon1,lat1 and lon2, lat2.
            */
            double dDistance = Double.MinValue;
            double dLat1InRad = Lat1 * (Math.PI / 180.0);
            double dLong1InRad = Long1 * (Math.PI / 180.0);
            double dLat2InRad = Lat2 * (Math.PI / 180.0);
            double dLong2InRad = Long2 * (Math.PI / 180.0);

            double dlongtitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                       Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                       Math.Pow(Math.Sin(dlongtitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // Distance.
            // const Double kEarthRadiusMiles = 3956.0;
            const Double kEarthRadiusKms = 6376.5;
            dDistance = kEarthRadiusKms * c;

            return dDistance;
        }

        public IFoodServiceClientSession GetMealInvitationInviter(string invitationUri)
        {

            string[] servicePointIdentityParts = invitationUri.Split(';');
            string servicesContextIdentity = servicePointIdentityParts[1];
            string servicePointIdentity = servicePointIdentityParts[2];
            string mealInvitationSessionID = servicePointIdentityParts[3];

            IFlavoursServicesContext flavoursServicesContext = FlavoursServicesContext.GetServicesContext(servicesContextIdentity);

            var graphicMenus = (flavoursServicesContext.Owner as Organization).UnSafeGraphicMenus;


            var flavoursServicesContextRunTime = flavoursServicesContext.GetRunTime();
            string orgIdentity = flavoursServicesContext.Owner.Identity;

            IFoodServiceClientSession clientSession = flavoursServicesContextRunTime.GetMealInvitationInviter( mealInvitationSessionID);

            return clientSession;

        }
    }

    /// <MetaDataID>{ae4070e9-17de-4a7a-9105-3639358bd4f7}</MetaDataID>
    class MapPolyGon
    {
        /// <MetaDataID>{6d727f7b-4bdb-4569-aae4-687d657da1d4}</MetaDataID>
        List<Coordinate> Points;

        /// <MetaDataID>{00f1084e-aeff-4b69-a993-61ee5bb30c99}</MetaDataID>
        public MapPolyGon(List<Coordinate> points)
        {
            Points = points.ToList();

        }


        //  The function will return true if the point x,y is inside the polygon, or
        //  false if it is not.  If the point is exactly on the edge of the polygon,
        //  then the function may return true or false.

        /// <MetaDataID>{a429d4d6-ecb7-45b7-bc39-7fb0aba7cf2b}</MetaDataID>
        public bool FindPoint(double latitude, double longitude)
        {
            int sides = Points.Count - 1;
            int j = sides - 1;
            bool pointStatus = false;
            for (int i = 0; i < sides; i++)
            {
                if (Points[i].Longitude < longitude && Points[j].Longitude >= longitude || Points[j].Longitude < longitude && Points[i].Longitude >= longitude)
                {
                    if (Points[i].Latitude + (longitude - Points[i].Longitude) / (Points[j].Longitude - Points[i].Longitude) * (Points[j].Latitude - Points[i].Latitude) < latitude)
                        pointStatus = !pointStatus;
                }
                j = i;
            }
            return pointStatus;
        }
    }

}
