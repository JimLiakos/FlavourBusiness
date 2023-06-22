using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using OOAdvantech.Remoting.RestApi;

using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using FlavourBusinessFacade.EndUsers;
using Xamarin.Forms;
using OOAdvantech.Transactions;
using System;

#if DeviceDotNet
using Xamarin.Essentials;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace DontWaitApp
{
    /// <MetaDataID>{ceaf19ab-2b52-45d6-a7f7-5dd4e251ed92}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public class FoodServiceClientVM : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ViewModel.ISecureUser, IGeocodingPlaces
    {
        [GenerateEventConsumerProxy]
        public event ObjectChangeStateHandle ObjectChangeState;

        public string OAuthUserIdentity { get; set; }

        List<OOAdvantech.SIMCardData> SIMCards;

        IFlavoursOrderServer FlavoursOrderServer;
        public FoodServiceClientVM(IFlavoursOrderServer flavoursOrderServer)
        {

            FlavoursOrderServer = flavoursOrderServer;

#if !DeviceDotNet
            var sd = typeof(OOAdvantech.Net.DeviceInstantiator).Assembly.GetCustomAttributes(true);
#endif
            //var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            //OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            ////_LinePhoneNumber= device.GetLinePhoneNumber(0);

            //SIMCards = device.LinesPhoneNumbers.ToList();
            //_LinesPhoneNumbers = device.LinesPhoneNumbers.Select(x => x.SIMCardDescription).ToList();

            if (ApplicationSettings.Current?.ClientAsGuest!=null)
            {
                _FoodServiceClient=ApplicationSettings.Current?.ClientAsGuest;
                _PhoneNumber=ApplicationSettings.Current?.ClientAsGuest.PhoneNumber;
            }



        }
        /// <MetaDataID>{b2483c93-a9f2-41f9-b223-ea85797d1490}</MetaDataID>
        object ClientSessionLock = new object();
        /// <MetaDataID>{1737e27b-b763-48ce-b629-0f13e16c0fdd}</MetaDataID>
        private void CreateClientAsGuest()
        {
            lock (ClientSessionLock)
            {

                if (ApplicationSettings.Current.ClientAsGuest == null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        ApplicationSettings.Current.ClientAsGuest = new FlavourBusinessManager.EndUsers.FoodServiceClient("Guest");
                        ApplicationSettings.AppSettingsStorage.CommitTransientObjectState(ApplicationSettings.Current.ClientAsGuest);
                        //if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.FriendlyName))
                        //    ApplicationSettings.Current.ClientAsGuest.FriendlyName = ApplicationSettings.Current.FriendlyName;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        string _PhoneNumber;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                _PhoneNumber = value;
                FoodServiceClient.PhoneNumber= value;
                //if ((this.FlavoursOrderServer as FlavoursOrderServer)?.CurrentUser!=null)
                //    (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser.PhoneNumber=_PhoneNumber;
                //if (ApplicationSettings.Current.ClientAsGuest!=null&&ApplicationSettings.Current.ClientAsGuest!=(this.FlavoursOrderServer as FlavoursOrderServer)?.CurrentUser)
                //    ApplicationSettings.Current.ClientAsGuest.PhoneNumber=value;
            }
        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageLoaded()
        {
        }
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void OnPageSizeChanged(double width, double height)
        {
        }



        /// <MetaDataID>{310c4d44-a0ae-4508-bb67-c3a7cd859178}</MetaDataID>
        public List<IPlace> Places
        {
            get
            {
                if (FoodServiceClient != null)
                {

                    var places = FoodServiceClient.DeliveryPlaces;
                    var defaultPlace = places.Where(x => x.Default).FirstOrDefault();
                    if (defaultPlace != null)
                        this.FlavoursOrderServer.GetNeighborhoodFoodServers(defaultPlace.Location);
                    return FoodServiceClient.DeliveryPlaces;
                }
                else
                    return new List<IPlace>();
            }
        }

        /// <MetaDataID>{93306e3b-021c-4cf9-86ba-d33ec81a80d3}</MetaDataID>
        public void SavePlace(IPlace deliveryPlace)
        {
            lock (ClientSessionLock)
            {
                if (ApplicationSettings.Current.ClientAsGuest == null)
                    CreateClientAsGuest();
            }
            FoodServiceClient.AddDeliveryPlace(deliveryPlace);
        }

        /// <MetaDataID>{14220482-c7b6-492b-ac98-3a289e32ea50}</MetaDataID>
        public void RemovePlace(IPlace deliveryPlace)
        {
            lock (ClientSessionLock)
            {
                if (ApplicationSettings.Current.ClientAsGuest == null)
                    CreateClientAsGuest();
            }
            FoodServiceClient.RemoveDeliveryPlace(deliveryPlace);

        }



        /// <MetaDataID>{d7f60501-e723-4279-8051-a3669a4f1448}</MetaDataID>
        public void SetDefaultPlace(IPlace deliveryPlace)
        {
            lock (ClientSessionLock)
            {
                if (ApplicationSettings.Current.ClientAsGuest == null)
                    CreateClientAsGuest();
            }
            FoodServiceClient.SetDefaultDelivaryPlace(deliveryPlace);

        }

        /// <MetaDataID>{4e4722c8-b953-4769-a184-75f7cdd6a5bc}</MetaDataID>
        public async Task<Coordinate?> GetCurrentLocation()
        {

#if DeviceDotNet
            if (!await CheckPermissionsToAccessCurrentLocation())
            {
                if (await RequestPermissionsToAccessCurrentLocation())
                {
                    var location = await GetCurrentLocationNative();
                    if (location != null)
                        return new Coordinate() { Latitude = location.Latitude, Longitude = location.Longitude };
                }
            }
            else
            {
                var location = await GetCurrentLocationNative();
                if (location != null)
                    return new Coordinate() { Latitude = location.Latitude, Longitude = location.Longitude };
            }
#endif
#if DEBUG
            //return new Location()
            //{
            //    Latitude = 38.0002465,
            //    Longitude = 23.74731
            //};
            string strCoordinatesBrax = "37.953746, 22.801600";
            string strCoordinates = "38.000483, 23.745453";
            var coordinates = strCoordinates.Split(',').Select(x => double.Parse(x.Trim(), System.Globalization.CultureInfo.GetCultureInfo(1033))).ToArray();
            return new Coordinate()
            {
                Latitude = coordinates[0],
                Longitude = coordinates[1]
            };

            //var debugLocation = new Location()
            //{
            //    Latitude = 37.953746,
            //    Longitude = 22.801600
            //};
            //return debugLocation;

#endif
            return null;
        }


        /// <MetaDataID>{bccd130a-67c8-4f46-a550-cb9d10103eb1}</MetaDataID>
        public async Task<bool> CheckPermissionsToAccessCurrentLocation()

        {
#if DeviceDotNet
#if IOSEmulator
                    return true;
#else
            var locationInUsePermisions = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            return locationInUsePermisions == PermissionStatus.Granted;

#endif
#else
            return await Task<bool>.FromResult(false);
#endif
        }



        /// <MetaDataID>{9521189d-cb68-4f9b-90a6-b6e5fb33461d}</MetaDataID>
        public async Task<bool> RequestPermissionsToAccessCurrentLocation()
        {
#if DeviceDotNet
            var locationInUsePermisions = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return locationInUsePermisions == PermissionStatus.Granted;
#else
            return await Task<bool>.FromResult(true);
#endif
        }

#if DeviceDotNet


        public static System.Threading.CancellationTokenSource cts;
        async Task<Xamarin.Essentials.Location> GetCurrentLocationNative()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, System.TimeSpan.FromSeconds(10));
                cts = new System.Threading.CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                if (location == null)
                    location = await Geolocation.GetLastKnownLocationAsync();
                return location;


            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (System.Exception ex)
            {
                // Unable to get location
            }
            return null;
        }

#endif


        //[OOAdvantech.MetaDataRepository.HttpVisible]
        //public string AuthUserIdentity
        //{
        //    get
        //    {
        //        return ApplicationSettings.Current.SignInUserIdentity;
        //    }
        //    set
        //    {
        //        ApplicationSettings.Current.SignInUserIdentity = value;
        //    }
        //}

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string SignInProvider
        {
            get
            {
                return _FoodServiceClient?.SignInProvider;
            }
            set
            {
                _FoodServiceClient.SignInProvider = value;
            }
        }


        string _Address;
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = value;
            }
        }




        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SetMessage(string message)
        {

        }



        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignInOld()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {

            }
            if (authUser.Firebase_Sign_in_Provider.ToLower()=="google.com")
                UserName = authUser.Email;

            return await Task<bool>.Run(() =>
            {
                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
                string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                serverUrl = "http://localhost:8090/api/";

                serverUrl = AzureServerUrl;
                IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;

                var endUser = pAuthFlavourBusines.SignInEndUser();
                //Organization.CurrentOrganization = organization;

                // organization.ObjectChangeState += Organization_ObjectChangeState;
                _LinePhoneNumber = null;

                if (endUser == null)
                {
                    ObjectChangeState?.Invoke(this, null);
                    return false;
                }
                else
                {
                    _LinePhoneNumber = endUser.SIMCardData.SIMCardDescription;
                    _FullName = endUser.Name;
                    ObjectChangeState?.Invoke(this, null);


                    //_Address = organization.Address;
                    //_PhoneNumber = organization.PhoneNumber;
                    //   organization.Address= _Address;
                    //GetOrgenizationRestMenus(organization as IResourceManager);

                    //SignedIn?.Invoke(this, EventArgs.Empty);
                    return true;
                }


            });

            //pAuthFlavourBusines.SignUpOwner(new OrganizationData() { Email = "jim.liakos@gmail.com", Name = "jim", Trademark = "Liakos" });




        }

        EndUserData EndUserData;
        /// <exclude>Excluded</exclude>
        IFoodServiceClient _FoodServiceClient;
        public IFoodServiceClient FoodServiceClient
        {
            get
            {
                if (_FoodServiceClient==null)
                    _FoodServiceClient=ApplicationSettings.Current?.ClientAsGuest;

                return _FoodServiceClient;

            }

            private set
            {
                if (_FoodServiceClient!=null)
                {

                    if (_FoodServiceClient!=value&&value!=null)
                    {
                        _FoodServiceClient= value;
                        _PhoneNumber=_FoodServiceClient.PhoneNumber;
                        ApplicationSettings.Current.ClientAsGuest.Synchronize(_FoodServiceClient);
                    }
                }
                else
                    _FoodServiceClient=value;
            }
        }
        public bool OnSignIn { get; private set; }
        public Task<bool> SignInTask { get; private set; }

        /// <MetaDataID>{0b631e28-fc5c-46ab-85c1-944ce7ead3eb}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            OOAdvantech.Remoting.RestApi.AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as OOAdvantech.Remoting.RestApi.AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;

            if (authUser == null)
                return false;

            if (OOAdvantech.Remoting.RemotingServices.IsOutOfProcess(FoodServiceClient as MarshalByRefObject)&&FoodServiceClient != null && FoodServiceClient.OAuthUserIdentity == authUser.User_ID)
                return true;


            if (OnSignIn && SignInTask != null)
                return await SignInTask;
            else
            {

                SignInTask = Task<bool>.Run(async () =>
                {

                    OnSignIn = true;
                    try
                    {
                        string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                        string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                        System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
                        string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                        serverUrl = "http://localhost:8090/api/";
                        serverUrl = AzureServerUrl;

                        IAuthFlavourBusiness pAuthFlavourBusiness = null;

                        try
                        {
                            var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                            pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);

                        }
                        catch (System.Net.WebException error)
                        {
                            throw;
                        }
                        catch (System.Exception error)
                        {
                            throw;
                        }
                        authUser = DeviceAuthentication.AuthUser;

                        var foodServiceClient = pAuthFlavourBusiness.SignInEndUser();
                        if (foodServiceClient != null)
                        {
                            FoodServiceClient=foodServiceClient;
                            FullName = FoodServiceClient.FullName;
                            UserName = FoodServiceClient.UserName;
                            Email = FoodServiceClient.Email;
                            if (string.IsNullOrWhiteSpace(UserName))
                                UserName=Email;

                            //ApplicationSettings.Current.FriendlyName = FoodServiceClient.FriendlyName;

#if DeviceDotNet
                            IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                            FoodServiceClient.DeviceFirebaseToken = device.FirebaseToken;
#endif
                            (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser =  FoodServiceClient;
                            (this.FlavoursOrderServer as FlavoursOrderServer).AuthUser = authUser;
                            OAuthUserIdentity = FoodServiceClient.OAuthUserIdentity;
                            ObjectChangeState?.Invoke(this, null);

                            return true;
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(FullName))
                            {
                                //FullName = ApplicationSettings.Current.FriendlyName;
                                if (string.IsNullOrWhiteSpace(UserName))
                                    UserName = authUser.Email;
                                ObjectChangeState?.Invoke(this, null);
                            }
                            else if (string.IsNullOrWhiteSpace(UserName))
                            {
                                UserName = authUser.Email;
                                ObjectChangeState?.Invoke(this, null);
                            }


                            return false;
                        }

                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }
                    finally
                    {
                        OnSignIn = false;
                    }
                });

                var result = await SignInTask;
                SignInTask = null;
                return result;

            }

        }



        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SaveUserProfile()
        {
            Task<bool>.Run(() =>
            {
                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
                string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                serverUrl = "http://localhost:8090/api/";
                serverUrl = AzureServerUrl;
                IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
                var selectedSimCard = (from simCard in SIMCards
                                       where simCard.SIMCardDescription == _LinePhoneNumber
                                       select new FlavourBusinessFacade.EndUsers.SIMCardData()
                                       {
                                           SIMCardDescription=simCard.SIMCardDescription,
                                           SIMCardIdentity=simCard.SIMCardIdentity,
                                           SIMCardPhoneNumber=simCard.SIMCardPhoneNumber
                                       }).FirstOrDefault();


                pAuthFlavourBusines.UpdateEndUserProfile(new EndUserData() { Email = this.Email, Name = this.FullName, SIMCard=selectedSimCard });

            });
            //SwitchOnOffPopupView?.Invoke(this, EventArgs.Empty);

        }


        [OOAdvantech.MetaDataRepository.HttpVisible]
        public bool SignUpOld()
        {
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
            serverUrl = "http://localhost:8090/api/";
            serverUrl = AzureServerUrl;
            //var selectedSimCard = (from simCard in SIMCards
            //                       where simCard.SIMCardDescription == _LinePhoneNumber
            //                       select simCard).FirstOrDefault();
            var selectedSimCard = (from simCard in SIMCards
                                   where simCard.SIMCardDescription == _LinePhoneNumber
                                   select new FlavourBusinessFacade.EndUsers.SIMCardData()
                                   {
                                       SIMCardDescription = simCard.SIMCardDescription,
                                       SIMCardIdentity = simCard.SIMCardIdentity,
                                       SIMCardPhoneNumber = simCard.SIMCardPhoneNumber
                                   }).FirstOrDefault();

            IAuthFlavourBusiness pAuthFlavourBusines = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
            var endUser = pAuthFlavourBusines.SignUpEndUser(new EndUserData() { Email = this.Email, Name = this.FullName, SIMCard= selectedSimCard });
            //Organization.CurrentOrganization = organization;
            return endUser != null;

        }

        /// <MetaDataID>{8bb16524-ef68-4a38-94f4-71776a04d4d5}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignUp()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;

            return await Task<bool>.Run(async () =>
            {

                OnSignIn = true;
                try
                {
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                    System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
                    string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                    serverUrl = "http://localhost:8090/api/";
                    serverUrl = AzureServerUrl;
                    IAuthFlavourBusiness pAuthFlavourBusiness = null;
                    try
                    {
                        var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                        pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
                    }
                    catch (System.Net.WebException error)
                    {
                        throw;
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }

                    if (authUser == null)
                    {

                    }
                    EndUserData = new EndUserData() { Email = authUser.Email, Name = this.FoodServiceClient.FriendlyName, Identity = authUser.User_ID, DeliveryPlaces= this.FoodServiceClient.DeliveryPlaces, FriendlyName=this.FoodServiceClient.FriendlyName };
                    var foodServiceClient = pAuthFlavourBusiness.SignUpEndUser(EndUserData);


                    if (foodServiceClient != null)
                    {
                        FullName = FoodServiceClient.FullName;
                        UserName = FoodServiceClient.UserName;
                        Email = FoodServiceClient.Email;
                        if (string.IsNullOrWhiteSpace(UserName))
                            UserName=Email;
                        FoodServiceClient=foodServiceClient;
                        //ApplicationSettings.Current.FriendlyName = FoodServiceClient.FriendlyName;

#if DeviceDotNet
                        IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                        FoodServiceClient.DeviceFirebaseToken = device.FirebaseToken;
#endif
                        (FlavoursOrderServer as FlavoursOrderServer).CurrentUser = FoodServiceClient;
                        (FlavoursOrderServer as FlavoursOrderServer).AuthUser = authUser;
                        ObjectChangeState?.Invoke(this, null);
                        OAuthUserIdentity = FoodServiceClient.OAuthUserIdentity;
                        return true;
                    }
                    else
                        return false;

                }
                catch (System.Exception error)
                {

                    throw;
                }
                finally
                {
                    OnSignIn = false;
                }
            });
        }



        /// <MetaDataID>{d38d4827-a9a7-48bb-b272-1f897c86cf1b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SignOut()
        {
            EndUserData = new EndUserData();
            (FlavoursOrderServer as FlavoursOrderServer).AuthUser = null;
            FoodServiceClient = null;
            FullName = null;
            UserName = null;
            Email = null;
            (FlavoursOrderServer as FlavoursOrderServer).CurrentUser = null;

            ObjectChangeState?.Invoke(this, null);
        }

        internal void SetFriendlyName(string friendlyName)
        {
            lock (ClientSessionLock)
            {
                FoodServiceClient.FriendlyName= friendlyName;
            }

        }

        internal string GetFriendlyName()
        {
            lock (ClientSessionLock)
            {

                if (ApplicationSettings.Current.ClientAsGuest == null)
                    CreateClientAsGuest();
                return FoodServiceClient.FriendlyName;
            }
        }

        public bool IsUsernameInUse(string username, OOAdvantech.Authentication.SignInProvider signInProvider)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;
            

            try
            {
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                return pAuthFlavourBusiness.IsUsernameInUse(username, signInProvider);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
        }

        private static IAuthFlavourBusiness GetFlavourBusinessAuth()
        {
            IAuthFlavourBusiness pAuthFlavourBusiness;
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
            string serverUrl = AzureServerUrl;
            var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
            pAuthFlavourBusiness = remoteObject as IAuthFlavourBusiness;
            return pAuthFlavourBusiness;
        }

        public void SendVerificationEmail(string emailAddress)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                pAuthFlavourBusiness.SendVerificationEmail(emailAddress);
            }
            catch (System.Net.WebException error)
            {
                throw;
            }
            catch (Exception error)
            {
                throw;
            }
        }

        /// <exclude>Excluded</exclude>
        string _LinePhoneNumber;

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string LinePhoneNumber
        {
            get
            {

                //return "";
                return _LinePhoneNumber;
            }
            set
            {
                var selectedSimCard = (from simCard in SIMCards
                                       where simCard.SIMCardDescription == value
                                       select simCard).FirstOrDefault();

                _LinePhoneNumber = value;
            }
        }
        ///// <exclude>Excluded</exclude>
        //List<string> _LinesPhoneNumbers;

        //[OOAdvantech.MetaDataRepository.HttpVisible]
        //public List<string> LinesPhoneNumbers
        //{
        //    get
        //    {
        //        return _LinesPhoneNumbers;
        //    }
        //}

        string _ConfirmPassword;

        public string ConfirmPassword
        {
            get
            {
                return _ConfirmPassword;
            }

            set
            {
                _ConfirmPassword = value;
            }
        }


        string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                _Email = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _FullName;
        public string FullName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_FullName))
                    return _FullName;


                return FoodServiceClient.FriendlyName;
            }
            set
            {

                if (!string.IsNullOrWhiteSpace(value)||_FullName==FoodServiceClient.FriendlyName)
                    FoodServiceClient.FriendlyName = value;
                _FullName = value;
                ObjectChangeState?.Invoke(this, null);
            }
        }

        /// <exclude>Excluded</exclude>
        string _Password;
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                _Password = value;
            }
        }
        /// <exclude>Excluded</exclude>
        string _UserName;

        public string UserName
        {
            get
            {
                return _UserName;
                //return ApplicationSettings.Current.SignInUserName;
            }
            set
            {
                _UserName = value;
                //ApplicationSettings.Current.SignInUserName = value;
            }
        }
    }
}
