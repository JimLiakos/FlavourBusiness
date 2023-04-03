using System;
using FlavourBusinessFacade;

using OOAdvantech.Remoting.RestApi;
using OOAdvantech.PersistenceLayer;
using System.Linq;
using OOAdvantech.Transactions;
using OOAdvantech.Remoting;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessFacade.HumanResources;
using System.Threading.Tasks;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{7e0db2af-8db5-41a5-b7a7-68c767a4615e}</MetaDataID>
    public class AuthFlavourBusiness : MonoStateClass, IAuthFlavourBusiness, IExtMarshalByRefObject
    {
        /// <MetaDataID>{5e35f804-9b98-4c34-beea-1c00d4676c53}</MetaDataID>
        public IFoodServiceClient SignUpEndUser(EndUserData endUser)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            string userId = authUser.User_ID;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var foodServiceClient = (from _organization in storage.GetObjectCollection<FoodServiceClient>()
                                     where _organization.Identity == userId
                                     select _organization).FirstOrDefault();
            if (foodServiceClient == null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    foodServiceClient = new FoodServiceClient(userId);
                    foodServiceClient.Name = endUser.Name;
                    foodServiceClient.FriendlyName=endUser.FriendlyName;
                    foodServiceClient.SIMCardData = new SIMCardData() { SIMCardDescription = endUser.SIMCard.SIMCardDescription, SIMCardIdentity = endUser.SIMCard.SIMCardIdentity, SIMCardPhoneNumber = endUser.SIMCard.SIMCardPhoneNumber };
                    foodServiceClient.Email = authUser.Email;
                    foreach (var deliveryPlace in endUser.DeliveryPlaces)
                        foodServiceClient.AddDeliveryPlace(deliveryPlace);

                    objectStorage.CommitTransientObjectState(foodServiceClient);
                    if (string.IsNullOrWhiteSpace(foodServiceClient.Email))
                        throw new System.Exception("Sign up error: 'Sign up only users with email'");
                    //TODO:Να ελεχθεί όταν δεν υπάρχει Email
                    stateTransition.Consistent = true;
                }
            }
            return foodServiceClient;
        }

        /// <MetaDataID>{ed5a1afd-aa57-404a-ba84-10b0548bb57c}</MetaDataID>
        public AuthFlavourBusiness()
        {
            var time = System.DateTime.Now;

            var objectStorage = OpenFlavourBusinessesStorage();
            var ss = System.DateTime.Now - time;
            if (!System.Diagnostics.EventLog.SourceExists("PersistencySystem", "."))
                System.Diagnostics.EventLog.CreateEventSource("PersistencySystem", "OOAdvance");
            System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
            myLog.Source = "PersistencySystem";
            myLog.WriteEntry(string.Format("Open '{0}' storage timespan {1}", objectStorage.StorageMetaData.StorageName, ss.TotalSeconds), System.Diagnostics.EventLogEntryType.Information);

            lock (objectStorage)
            {
                try
                {
                    if (objectStorage.StorageMetaData.CheckForVersionUpgrate(typeof(IOrganization).Assembly.FullName))
                    {
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

        }


        public IUser SignUp(UserData userData, UserData.RoleType roleType)
        {
            if (roleType == UserData.RoleType.Organization)
            {
                OrganizationData organizationData = new OrganizationData()
                {
                    Address = userData.Address,
                    Email = userData.Email,
                    FullName = userData.FullName,
                    PhoneNumber = userData.PhoneNumber,
                    Trademark = userData.Trademark
                };

                return SignUpFounder(organizationData);
            }
            else if (roleType == UserData.RoleType.MenuMaker)
            {
                return SignUpMenuMaker(userData);
            }

            else
                return SignUp(userData);

        }

        private IUser SignUpMenuMaker(UserData userData)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            string userId = authUser.User_ID;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var menuMaker = (from _menuMaker in storage.GetObjectCollection<HumanResources.MenuMaker>()
                             where _menuMaker.Identity == userId
                             select _menuMaker).FirstOrDefault();

            if (menuMaker == null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    menuMaker = new HumanResources.MenuMaker(userId);
                    menuMaker.OAuthUserIdentity = authUser.User_ID;
                    menuMaker.Name = userData.FullName;
                    menuMaker.Email = userData.Email;
                    menuMaker.PhoneNumber = userData.PhoneNumber;
                    menuMaker.PhotoUrl = userData.PhotoUrl;
                    //organization.Trademark = userData.Trademark;
                    //organization.Address = userData.Address;

                    objectStorage.CommitTransientObjectState(menuMaker);

                    if (string.IsNullOrWhiteSpace(menuMaker.Email))
                        throw new System.Exception("Sign up error: 'Sign up only users with email'");

                    //TODO:Να ελεχθεί όταν δεν υπάρχει Email
                    stateTransition.Consistent = true;
                }
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
                authUserRef.FullName = userData.FullName;
                authUserRef.PhoneNumber = userData.PhoneNumber;
                authUserRef.Address = userData.Address;
                authUserRef.Email = authUser.Email;
                authUserRef.AddRole(menuMaker);
            }
            return menuMaker;
        }

        /// <MetaDataID>{32a5f717-59cb-43bc-afd5-cafad9826a42}</MetaDataID>
        public IOrganization SignUpFounder(OrganizationData organizationData)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            string userId = authUser.User_ID;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var organization = (from _organization in storage.GetObjectCollection<Organization>()
                                where _organization.Identity == userId
                                select _organization).FirstOrDefault();

            if (organization == null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    organization = new Organization(authUser.User_ID);
                    organization.OAuthUserIdentity = authUser.User_ID;
                    organization.Name = organizationData.FullName;
                    organization.Trademark = organizationData.Trademark;
                    organization.PhoneNumber = organizationData.PhoneNumber;
                    organization.Address = organizationData.Address;
                    organization.Email = authUser.Email;
                    objectStorage.CommitTransientObjectState(organization);

                    if (string.IsNullOrWhiteSpace(organization.Email))
                        throw new System.Exception("Sign up error: 'Sign up only users with email'");

                    //TODO:Να ελεχθεί όταν δεν υπάρχει Email

                    stateTransition.Consistent = true;
                }
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
                authUserRef.FullName = organizationData.FullName;
                authUserRef.PhoneNumber = organizationData.PhoneNumber;
                authUserRef.Address = organizationData.Address;
                authUserRef.Email = authUser.Email;
                authUserRef.AddRole(organization);
            }
            return organization;
        }

        /// <MetaDataID>{4d4c3b27-e4f0-444c-a22f-167003e2e14d}</MetaDataID>
        static string lockObj = "lock";

        /// <MetaDataID>{9af4d2e0-c30c-451c-a932-5fff125e1467}</MetaDataID>
        public static ObjectStorage OpenFlavourBusinessesStorage()
        {
            lock (lockObj)
            {
                ObjectStorage storageSession = null;
                string storageName = "FlavourBusinesses";
                string storageLocation = "DevStorage";
                string storageType = "OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider";

                try
                {
                    storageSession = ObjectStorage.OpenStorage(storageName,
                                                                storageLocation,
                                                                storageType, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey);


                    System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                    string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
                    var storagesClient = new OOAdvantech.PersistenceLayer.StoragesClient(httpClient);
                    storagesClient.BaseUrl = serverUrl;
                    var task = storagesClient.GetAsync(storageSession.StorageMetaData.StorageIdentity);
                    task.Wait();
                    var storageMetaData = task.Result;
                    if (storageMetaData == null || storageMetaData.StorageIdentity == null)
                        storagesClient.PostAsync(storageSession.StorageMetaData, true);

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
                        storageSession.StorageMetaData.RegisterComponent(typeof(ComputationalResources.IsolatedComputingContext).Assembly.FullName);
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
        }

        /// <MetaDataID>{0855ee4f-a7c2-4cf5-b96e-6c24399a38b1}</MetaDataID>
        public IOrganization SignUpWorker(WorkerData organizationData)
        {
            return null;
        }

        /// <MetaDataID>{a49ae82c-4b36-46f4-8b0c-50c808cf2dcf}</MetaDataID>
        public IServiceContextSupervisor SignInServiceContextSupervisor()
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();
            string userId = authUser.User_ID;
            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
            if (authUserRef == null)
                return null;
            IServiceContextSupervisor supervisor = authUserRef.GetRoleObject<IServiceContextSupervisor>();
            if (supervisor != null)
            {
                var sds = supervisor.Identity;
            }
            return supervisor;
        }



        public IUser SignIn(UserData.RoleType roleType)
        {

            if (roleType == UserData.RoleType.Organization)
                return SignInFounder();
            else if (roleType == UserData.RoleType.MenuMaker)
                return SignInManuMaker();
            else
            {
                var userData = SignIn();
                if (userData != null)
                {
                    var role = userData.Roles.Where(x => x.RoleType == UserData.RoleType.Organization).FirstOrDefault();
                    return role.User;
                }
                else
                    return null;
            }
        }

        private IUser SignInManuMaker()
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();
            string userId = authUser.User_ID;

            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
            if (authUserRef.Email == null || authUserRef.FullName == null)
            {
                if (authUserRef.Email == null)
                    authUserRef.Email = authUser.Email;
                if (authUserRef.FullName == null)
                    authUserRef.FullName = authUser.Name;

                authUserRef.Save();
            }
            HumanResources.MenuMaker menuMaker = authUserRef.GetRoleObject<HumanResources.MenuMaker>();
            if (menuMaker == null)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                menuMaker = (from _menuMaker in storage.GetObjectCollection<HumanResources.MenuMaker>()
                             where _menuMaker.OAuthUserIdentity == userId
                             select _menuMaker).FirstOrDefault();
            }
            return menuMaker;
        }

        /// <MetaDataID>{b51154a5-4bb7-4a7f-ad42-e58ef459c296}</MetaDataID>
        public IOrganization SignInFounder()
        {


            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();
            string userId = authUser.User_ID;
            IOrganization organization = null;



            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
            if (authUserRef != null)
            {
                if (authUserRef.Email == null || authUserRef.FullName == null)
                {
                    if (authUserRef.Email == null)
                        authUserRef.Email = authUser.Email;
                    if (authUserRef.FullName == null)
                        authUserRef.FullName = authUser.Name;

                    authUserRef.Save();
                }
                organization = authUserRef.GetRoleObject<Organization>();
            }
            if (organization == null)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                organization = (from _organization in storage.GetObjectCollection<Organization>()
                                where _organization.OAuthUserIdentity == userId
                                select _organization).FirstOrDefault();
                if (organization != null)
                    authUserRef.AddRole(organization);

            }
            return organization;



            //var computingResource = ComputingResources.ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext);
            //if (ComputingResources.ComputingCluster.CurrentComputingResource == computingResource)
            //{
            //    AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            //    ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            //    string userId = authUser.User_ID;
            //    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            //    var organization = (from _organization in storage.GetObjectCollection<Organization>()
            //                        where _organization.Identity == userId
            //                        select _organization).FirstOrDefault();
            //    organization.Email = authUser.Email;



            //    return organization;
            //}
            //else
            //{
            //    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            //    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;


            //    string channelUri = System.Runtime.Remoting.Messaging.CallContext.GetData("PublicChannelUri") as string;
            //    string publicChannelUri = null;
            //    string internalchannelUri = null;
            //    ByRef.GetChannelUriParts(channelUri, out publicChannelUri, out internalchannelUri);
            //    string serverUrl = publicChannelUri + "(" + computingResource.ResourceID + ")";

            //    IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;

            //    var organization = pAuthFlavourBusines.SignIn();


            //    return organization;

            //}

        }

#if DEBUG
        /// <MetaDataID>{cbafb371-aae4-4f31-9c94-287cb6193e30}</MetaDataID>
        public UserData SignInUser(string userID)
        {
            AuthUser authUser = new AuthUser() { User_ID = userID };//


            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);

            if (authUserRef != null)
            {
                if (authUserRef.Email == null)
                    authUserRef.Email = authUser.Email;

                if (authUserRef.FullName == null)
                    authUserRef.FullName = authUser.Name;



                UserData userData = new UserData()
                {
                    Identity = authUserRef.GetIdentity(),
                    Email = authUserRef.Email,
                    FullName = authUserRef.FullName,
                    PhoneNumber = authUserRef.PhoneNumber,
                    PhotoUrl = authUserRef.PhotoUrl,
                    Address = authUserRef.Address,
                    Roles = authUserRef.GetRoles().Where(x => x.RoleObject is IUser).Select(x => new UserData.UserRole() { User = x.RoleObject as IUser, RoleType = UserData.UserRole.GetRoleType(x.TypeFullName) }).ToList()
                };

                return userData;
            }
            else
                return null;


        }
#endif
        bool tested= false;
        void ObjectStorageMTTest()
        {
            lock(this)
            {
                if(tested) return;

                tested = true;
            }
            var task1 = Task.Run(() =>
            {
                FlavourBusinessManager.AuthUserRef.ObjectStorageMTTest();
            });
            var task2 = Task.Run(() =>
            {
                FlavourBusinessManager.AuthUserRef.ObjectStorageMTTest();
            });

            var task3 = Task.Run(() =>
            {
                FlavourBusinessManager.AuthUserRef.ObjectStorageMTTest();
            });

            var task4 = Task.Run(() =>
            {
                FlavourBusinessManager.AuthUserRef.ObjectStorageMTTest();
            });

            var task5 = Task.Run(() =>
            {
                FlavourBusinessManager.AuthUserRef.ObjectStorageMTTest();
            });

            var task6 = Task.Run(() =>
            {
                FlavourBusinessManager.AuthUserRef.ObjectStorageMTTest();
            });

            task1.Wait();
            task2.Wait();
            task3.Wait();
            task4.Wait();
            task5.Wait();
            task6.Wait();


        }


        /// <MetaDataID>{943535ac-2335-4f04-94cd-94ce17a0d256}</MetaDataID>
        public UserData SignIn()
        {
            try
            {



                //ObjectStorageMTTest();

            }
            catch (Exception error)
            {


            }

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;


            if (authUser == null)
                throw new System.Security.Authentication.AuthenticationException();


            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);

            if (authUserRef != null)
            {
                if (authUserRef.Email == null)
                    authUserRef.Email = authUser.Email;

                if (authUserRef.FullName == null)
                    authUserRef.FullName = authUser.Name;



                UserData userData = new UserData()
                {
                    Identity = authUserRef.GetIdentity(),
                    Email = authUserRef.Email,
                    FullName = authUserRef.FullName,
                    PhoneNumber = authUserRef.PhoneNumber,
                    PhotoUrl = authUserRef.PhotoUrl,
                    Address = authUserRef.Address,
                    //OAuthUserIdentity=authUserRef.GetIdentity(),
                    Roles = authUserRef.GetRoles().Where(x => x.RoleObject is IUser).Select(x => new UserData.UserRole() { User = x.RoleObject as IUser, RoleType = UserData.UserRole.GetRoleType(x.TypeFullName) }).ToList()
                };

                foreach( var role in userData.Roles)
                {
                    var thuser = role.User;
                }

                return userData;
            }
            else
                return null;

        }


        public void UpdateUserProfile(UserData userData, UserData.RoleType roleType)
        {

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                throw new System.Security.Authentication.AuthenticationException();

            string userId = authUser.User_ID;
            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
            {
                authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
                authUserRef.FullName = userData.FullName;
                authUserRef.PhoneNumber = userData.PhoneNumber;
                authUserRef.Address = userData.Address;
                authUserRef.Email = userData.Email;
                authUserRef.Save();
            }


            if (roleType == UserData.RoleType.Organization)
            {
                OrganizationData organizationData = new OrganizationData()
                {
                    Address = userData.Address,
                    Email = userData.Email,
                    FullName = userData.FullName,
                    PhoneNumber = userData.PhoneNumber,
                    Trademark = userData.Trademark
                };

                UpdateFounderUserProfile(organizationData);
            }

        }

        /// <MetaDataID>{b739e6aa-6a6b-4b7d-b337-1b7c99e0e737}</MetaDataID>
        public void UpdateFounderUserProfile(OrganizationData organizationData)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            string userId = authUser.User_ID;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var organization = (from _organization in storage.GetObjectCollection<Organization>()
                                where _organization.OAuthUserIdentity == userId
                                select _organization).FirstOrDefault();
            if (organization != null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    organization.Name = organizationData.FullName;
                    organization.Trademark = organizationData.Trademark;
                    organization.PhoneNumber = organizationData.PhoneNumber;
                    organization.Address = organizationData.Address;
                    objectStorage.CommitTransientObjectState(organization);
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <MetaDataID>{0c33c1c9-7a75-4a12-af5e-d8df44fc1f90}</MetaDataID>
        public void UpdateEndUserProfile(EndUserData endUserDataData)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            string userId = authUser.User_ID;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var endUser = (from _endUser in storage.GetObjectCollection<IFoodServiceClient>()
                           where _endUser.Identity == userId
                           select _endUser).FirstOrDefault();
            if (endUser != null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    endUser.Name = endUserDataData.Name;
                    endUser.Email = endUserDataData.Email;
                    endUser.SIMCardData = endUserDataData.SIMCard;
                    objectStorage.CommitTransientObjectState(endUser);
                    stateTransition.Consistent = true;
                }
            }

        }
        /// <MetaDataID>{d31537aa-d77f-487d-8e09-94712eb5fc36}</MetaDataID>
        public string GetMessage(string name, int age, IOrganization pok)
        {
            string message = "Hello World";
            return message;
        }


        /// <MetaDataID>{dcc1d501-7c4f-4346-b24f-baeabcc9256b}</MetaDataID>
        public IFoodServiceClient SignInEndUser()
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            string userId = authUser.User_ID;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var endUser = (from _organization in storage.GetObjectCollection<FoodServiceClient>()
                           where _organization.Identity == userId
                           select _organization).FirstOrDefault();
            if (endUser != null)
                endUser.Email = authUser.Email;

            return endUser;


            //var computingResource = ComputingResources.ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext);
            //if (ComputingResources.ComputingCluster.CurrentComputingResource == computingResource)
            //{
            //    AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            //    ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            //    string userId = authUser.User_ID;
            //    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            //    var endUser = (from _organization in storage.GetObjectCollection<FoodServiceClient>()
            //                   where _organization.Identity == userId
            //                   select _organization).FirstOrDefault();
            //    if(endUser!=null)
            //        endUser.Email = authUser.Email;



            //    return endUser;
            //}
            //else
            //{
            //    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            //    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;


            //    string channelUri = System.Runtime.Remoting.Messaging.CallContext.GetData("PublicChannelUri") as string;
            //    string publicChannelUri = null;
            //    string internalchannelUri = null;
            //    ByRef.GetChannelUriParts(channelUri, out publicChannelUri, out internalchannelUri);
            //    string serverUrl = publicChannelUri + "(" + computingResource.ResourceID + ")";

            //    IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;

            //    var endUser = pAuthFlavourBusines.SignInEndUser();


            //    return endUser;

            //}
        }

        /// <MetaDataID>{d6cb18a6-8735-4af1-ba11-e620457d366e}</MetaDataID>
        public IFoodServiceClient SIMCardSignInEndUser(string simCardIdentity)
        {

            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            var endUser = (from _endUser in storage.GetObjectCollection<FoodServiceClient>()
                           where _endUser.SIMCardData.SIMCardIdentity == simCardIdentity
                           select _endUser).FirstOrDefault();
            return endUser;


            //var computingResource = ComputingResources.ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext);
            //if (ComputingResources.ComputingCluster.CurrentComputingResource == computingResource)
            //{
            //    ObjectStorage objectStorage = OpenFlavourBusinessesStorage();
            //    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
            //    var endUser = (from _endUser in storage.GetObjectCollection<FoodServiceClient>()
            //                   where _endUser.SIMCardData.SIMCardIdentity == simCardIdentity
            //                   select _endUser).FirstOrDefault();
            //    return endUser;
            //}
            //else
            //{
            //    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            //    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            //    string channelUri = System.Runtime.Remoting.Messaging.CallContext.GetData("PublicChannelUri") as string;
            //    string publicChannelUri = null;
            //    string internalchannelUri = null;
            //    ByRef.GetChannelUriParts(channelUri, out publicChannelUri, out internalchannelUri);
            //    string serverUrl = publicChannelUri + "(" + computingResource.ResourceID + ")";
            //    IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
            //    var endUser = pAuthFlavourBusines.SIMCardSignInEndUser(simCardIdentity);
            //    return endUser;
            //}
        }

        /// <MetaDataID>{99a2191d-21cf-48ee-a82b-7b67c3bc6bcd}</MetaDataID>
        public UserData SignUp(UserData userData)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            ObjectStorage objectStorage = OpenFlavourBusinessesStorage();

            string userId = authUser.User_ID;
            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
            {
                authUserRef = AuthUserRef.GetAuthUserRef(authUser, true);
                authUserRef.FullName = userData.FullName;
                authUserRef.PhoneNumber = userData.PhoneNumber;
                authUserRef.Address = userData.Address;
                authUserRef.Email = userData.Email;
                authUserRef.Save();
            }
            userData = new UserData()
            {
                Identity = authUserRef.GetIdentity(),
                Email = authUserRef.Email,
                FullName = authUserRef.FullName,
                PhoneNumber = authUserRef.PhoneNumber,
                Address = authUserRef.Address,
                PhotoUrl = authUserRef.PhotoUrl,
                Roles = authUserRef.GetRoles().Where(x => x.RoleObject is IUser).Select(x => new UserData.UserRole() { User = x.RoleObject as IUser, RoleType = UserData.UserRole.GetRoleType(x.TypeFullName) }).ToList()
            };

            return userData;
        }

        /// <MetaDataID>{7619f4eb-43ae-4cf2-b186-95c4171b0e77}</MetaDataID>
        public UserData GetUser(string userName)
        {
            AuthUserRef authUserRef = AuthUserRef.GetAuthUserRefByUserName(userName);
            if (authUserRef != null)
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
                return userData;
            }
            else
                return null;
        }
    }
}