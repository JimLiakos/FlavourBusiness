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
                FlavoursServicesContextsMonitoringTimer = new System.Timers.Timer(20000);
                FlavoursServicesContextsMonitoringTimer.Enabled = true;
                FlavoursServicesContextsMonitoringTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnFlavoursServicesContextsMonitoring);
                FlavoursServicesContextsMonitoringTimer.Start();
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

        /// <MetaDataID>{9696eb0e-cbca-4371-af36-a7093c85dc15}</MetaDataID>
        public IServicePoint GetServicePoint(string servicePointIdentity)
        {
            return null;
        }

        /// <MetaDataID>{f20f9e9d-c06f-440b-afdc-1a8f38dd82ea}</MetaDataID>
        public IFlavoursServicesContextRuntime GetServicesContextRuntime(string storageName, string storageLocation, string servicesContextIdentity, string organizationIdentity, OrganizationStorageRef restaurantMenusDataStorageRef, bool create = false)
        {
            //7f9bde62e6da45dc8c5661ee2220a7b0_bf55f9a68e5048d0b6b1676fa18ca64a

            storageLocation = "DevStorage";
            string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";


            ObjectStorage objectStorage = null;
            try
            {
                var time = System.DateTime.Now;

                objectStorage = ObjectStorage.OpenStorage(storageName,
                                                            storageLocation,
                                                            storageType, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey);

                var ss = System.DateTime.Now - time;


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


    }
}
