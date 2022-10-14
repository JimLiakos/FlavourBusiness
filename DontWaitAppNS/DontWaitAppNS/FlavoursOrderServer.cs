using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

using OOAdvantech.Remoting.RestApi;
using RestaurantHallLayoutModel;
using FlavourBusinessFacade.RoomService;
using System.Configuration;

using OOAdvantech.BinaryFormatter;
using System.Drawing;
using OOAdvantech;
using FlavourBusinessManager.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech.Json.Linq;
using System.Reflection;
using System.Threading;
using OOAdvantech.Transactions;
using FlavourBusinessManager.EndUsers;




#if DeviceDotNet
using Xamarin.Forms;
using Xamarin.Essentials;
using ZXing.Net.Mobile.Forms;
using ZXing;
using ZXing.QrCode;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using QRCoder;
using System.IO;
using System.Drawing.Imaging;
using System;
#endif

namespace DontWaitApp
{
    /// <MetaDataID>{cab2cac1-0d34-4bcd-b2c4-81e4a9f915c3}</MetaDataID>
    class FlavoursOrderServer : MarshalByRefObject, IFlavoursOrderServer, FlavourBusinessFacade.ViewModel.ILocalization, IGeocodingPlaces, OOAdvantech.Remoting.IExtMarshalByRefObject, IBoundObject
    {

        Dictionary<Coordinate, List<HomeDeliveryServicePointInfo>> NeighborhoodFoodServers = new Dictionary<Coordinate, List<HomeDeliveryServicePointInfo>>();

        public Task<List<HomeDeliveryServicePointInfo>> GetNeighborhoodFoodServers(Coordinate location)
        {
            if (NeighborhoodFoodServersTask != null && NeighborhoodFoodServersTask.Status == TaskStatus.Running)
                return NeighborhoodFoodServersTask;

            lock (NeighborhoodFoodServers)
            {
                if (NeighborhoodFoodServers.ContainsKey(location))
                    return Task.FromResult<List<HomeDeliveryServicePointInfo>>(NeighborhoodFoodServers[location]);
            }
            NeighborhoodFoodServersTask = Task<List<HomeDeliveryServicePointInfo>>.Run(() =>
            {
                System.Collections.Generic.List<HomeDeliveryServicePointInfo> servers = null;
                do
                {
                    try
                    {
                        servers = this.ServicesContextManagment.GetNeighborhoodFoodServers(location);
                        lock (NeighborhoodFoodServers)
                        {
                            NeighborhoodFoodServers[location] = servers;
                        }
                    }
                    catch (System.Net.WebException connectionError)
                    {
                        if (connectionError.Status != System.Net.WebExceptionStatus.ConnectFailure)
                            throw connectionError;
                    }
                    catch (TimeoutException timeoutError)
                    {
                    }

                } while (!NeighborhoodFoodServers.ContainsKey(location));

#if !DeviceDotNet
#if DEBUG

                foreach (var server in servers)
                {
                    if (!string.IsNullOrWhiteSpace(server.LogoBackgroundImageUrl))
                        server.LogoBackgroundImageUrl = "https://dev-localhost/" + server.LogoBackgroundImageUrl.Substring(server.LogoBackgroundImageUrl.IndexOf("devstoreaccount1"));

                    if (!string.IsNullOrWhiteSpace(server.LogoImageUrl))
                        server.LogoImageUrl = "https://dev-localhost/" + server.LogoImageUrl.Substring(server.LogoImageUrl.IndexOf("devstoreaccount1"));
                }
#endif
#endif
                return servers;
            });

            return NeighborhoodFoodServersTask;
        }


        /// <MetaDataID>{3b2f428f-f1e1-4845-b4e7-6ec49a2502fb}</MetaDataID>
        public Task<HallLayout> GetHallLayout()
        {
            return Task<HallLayout>.Run(async () =>
            {
                try
                {

                    string servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7";


                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    servicesContextManagment.ObjectChangeState += ServicesContextManagment_ObjectChangeState;

                    var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                    OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    FlavourBusinessFacade.ServicesContextResources.IHallLayout hallLayout = servicesContextManagment.GetHallLayout(servicePoint);


                    if (hallLayout is HallLayout)
                    {
                        (hallLayout as HallLayout).SetShapesImagesRoot("https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/");
                        var sds = hallLayout.Name;
                    }


                    return hallLayout as HallLayout;
                }
                catch (Exception error)
                {

                    throw;
                }
            });
        }



        /// <MetaDataID>{d87c2614-3408-428f-80d7-3d590f546a27}</MetaDataID>
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler
        {
            get
            {
#if DeviceDotNet
                return (Application.Current as IAppLifeTime).SerializeTaskScheduler;
#else
                return (System.Windows.Application.Current as IAppLifeTime).SerializeTaskScheduler;

#endif
            }
        }

        /// <MetaDataID>{cc704161-f4c2-454b-9ff6-010d1e190a4b}</MetaDataID>
        public FlavoursOrderServer()
        {

        }

        /// <MetaDataID>{01f8d08e-6e0b-434c-88f3-7da0722f7af5}</MetaDataID>
        private void ApplicationResuming(object sender, EventArgs e)
        {

            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (this.FoodServicesClientSessionViewModel.FoodServicesClientSession != null)
                        {
                            try
                            {
                                this.FoodServicesClientSessionViewModel.FoodServicesClientSession.DeviceResume();
                            }
                            catch (OOAdvantech.Remoting.MissingServerObjectException error)
                            {
                                return await ConnectToServicePoint(this.FoodServicesClientSessionViewModel.MenuData.ServicePointIdentity);
                            }
                        }
                        FoodServicesClientSessionViewModel.GetMessages();
                        break;
                    }
                    catch (System.Net.WebException commError)
                    {
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
                        var er = error;
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                return true;
            });



        }
        /// <exclude>Excluded</exclude>
        IList<IHallLayout> _Halls;

        public IList<IHallLayout> Halls
        {
            get
            {
                return _Halls;
            }
            set
            {
                _Halls = value;
            }
        }

        public void UpdateHallsServicePointStates(Dictionary<string, ServicePointState> hallsServicePointsState)
        {
            foreach (var hall in Halls.OfType<RestaurantHallLayoutModel.HallLayout>())
            {
                foreach (var shape in hall.Shapes)
                {
                    if (!string.IsNullOrWhiteSpace(shape.ServicesPointIdentity) && hallsServicePointsState.ContainsKey(shape.ServicesPointIdentity))
                        shape.ServicesPointState = hallsServicePointsState[shape.ServicesPointIdentity];
                }
            }
            _ObjectChangeState?.Invoke(this, nameof(Halls));
        }

        /// <MetaDataID>{efa7ab05-f66c-42a1-850c-c825f70ce948}</MetaDataID>
        private void ApplicationSleeping(object sender, EventArgs e)
        {
            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (FoodServicesClientSessionViewModel != null)
                            FoodServicesClientSessionViewModel.FoodServicesClientSession.DeviceSleep();
                        break;
                    }
                    catch (System.Net.WebException commError)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }
                return true;
            });

        }


#if DeviceDotNet
        private void Device_MessageReceived(OOAdvantech.IRemoteMessage remoteMessage)
        {
            if (remoteMessage.Data.ContainsKey("MessageID") && FoodServiceClientSession != null)
                MessageReceived(FoodServiceClientSession);
        }
#endif



        public event WebViewLoadedHandle OnWebViewLoaded;

        /// <MetaDataID>{d52da524-96a8-4f0b-ae0f-703be4348ff2}</MetaDataID>
        string lan = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;


        /// <MetaDataID>{0cff47a2-3b96-4019-bfab-e15d448b603f}</MetaDataID>
        public string Language { get { return lan; } }

        string deflan = "en";
        public string DefaultLanguage { get { return deflan; } }

        public string FontsLink { get; set; } = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";

        /// <MetaDataID>{08ad11d2-20bf-4763-b9ea-733efa47a3e1}</MetaDataID>
        public string Path
        {
            get
            {
                return ApplicationSettings.Current.Path;
            }
            set
            {
                string path = value;
                if (!string.IsNullOrWhiteSpace(path) && path.Split('/').Length > 0)
                {
                    if (MenuData.ServicePointIdentity != path.Split('/')[0])
                        path = "";
                }
                ApplicationSettings.Current.Path = path;
            }
        }

        /// <MetaDataID>{94695399-7052-4246-a471-e57eab60ecf3}</MetaDataID>
        public async void WebViewAttached()
        {

            //if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePointIdentity))
            //{
            //    var clientSessionData = await GetFoodServiceSession(ApplicationSettings.Current.LastServicePointIdentity, false);
            //}

        }

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

        public List<IPlace> Places
        {
            get
            {
                if (ApplicationSettings.Current.ClientAsGuest != null)
                {

                    var places = ApplicationSettings.Current.ClientAsGuest.DeliveryPlaces;
                    if (places.Count > 0 && places.Where(x => x.Default).FirstOrDefault() == null)
                        ApplicationSettings.Current.ClientAsGuest.SetDefaultDelivaryPlace(places[0]);

                    var defaultPlace = places.Where(x => x.Default).FirstOrDefault();
                    if (defaultPlace != null)
                        GetNeighborhoodFoodServers(defaultPlace.Location);
                    return ApplicationSettings.Current.ClientAsGuest.DeliveryPlaces;
                }
                else
                    return new List<IPlace>();
            }
        }

        public void SavePlace(IPlace deliveryPlace)
        {
            lock (ClientSessionLock)
            {
                if (ApplicationSettings.Current.ClientAsGuest == null)
                    CreateClientAsGuest();
            }
            ApplicationSettings.Current.ClientAsGuest.AddDeliveryPlace(deliveryPlace);
        }

        public void RemovePlace(IPlace deliveryPlace)
        {
            lock (ClientSessionLock)
            {
                if (ApplicationSettings.Current.ClientAsGuest == null)
                    CreateClientAsGuest();
            }
            ApplicationSettings.Current.ClientAsGuest.RemoveDeliveryPlace(deliveryPlace);

        }



        public void SetDefaultPlace(IPlace deliveryPlace)
        {
            lock (ClientSessionLock)
            {
                if (ApplicationSettings.Current.ClientAsGuest == null)
                    CreateClientAsGuest();
            }
            ApplicationSettings.Current.ClientAsGuest.SetDefaultDelivaryPlace(deliveryPlace);

        }
#if DeviceDotNet
        public static CancellationTokenSource cts;
        async Task<Xamarin.Essentials.Location> GetCurrentLocationNative()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
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
            catch (Exception ex)
            {
                // Unable to get location
            }
            return null;
        }

#endif

        /// <MetaDataID>{ebf9e3ce-2957-4519-82cf-8aa08b910d88}</MetaDataID>
        public void WebViewLoaded()
        {
            OnWebViewLoaded?.Invoke();
            this.FoodServicesClientSessionViewModel.GetMessages();

        }



        #region Client

        /// <MetaDataID>{3786f954-db74-4151-af46-e70e04c9c3c8}</MetaDataID>
        string _Name;
        /// <MetaDataID>{303dedc3-0c36-40b0-aab6-c5ee74dee937}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }


        /// <MetaDataID>{75fc6cad-1e29-4aa3-97fa-14462e970f67}</MetaDataID>
        public Task<string> GetFriendlyName()
        {

            //return Task<string>.FromResult(default(string));

            return Task.Run<string>(() =>
            {

                var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

                if (ApplicationSettings.Current.ClientAsGuest == null && !WaiterView)
                {
                    CreateClientAsGuest();
                    return ApplicationSettings.Current.ClientAsGuest.FriendlyName;
                }
                string friendlyName = ApplicationSettings.Current.FriendlyName;
                return friendlyName;
            });


        }

        private void CreateClientAsGuest()
        {
            lock (ClientSessionLock)
            {

                if (ApplicationSettings.Current.ClientAsGuest == null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        ApplicationSettings.Current.ClientAsGuest = new FoodServiceClient("Guest");
                        ApplicationSettings.AppSettingsStorage.CommitTransientObjectState(ApplicationSettings.Current.ClientAsGuest);
                        if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.FriendlyName))
                            ApplicationSettings.Current.ClientAsGuest.FriendlyName = ApplicationSettings.Current.FriendlyName;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{60462cb1-1349-470d-87c9-923b2e7fe825}</MetaDataID>
        public void SetFriendlyName(string friendlyName)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                if (!WaiterView)
                {
                    lock (ClientSessionLock)
                    {
                        if (ApplicationSettings.Current.ClientAsGuest == null)
                            CreateClientAsGuest();
                    }
                }
                ApplicationSettings.Current.FriendlyName = friendlyName;
                stateTransition.Consistent = true;
            }

            //if (this.FoodServiceClientSession != null && this.FoodServiceClientSession.ClientName != friendlyName)
            //{
            //    #region Sets client name of active session a sync for unstable connection 
            //    SerializeTaskScheduler.AddTask(async () =>
            //    {
            //        int tries = 30; //try for 30 time 
            //        while (tries > 0)
            //        {
            //            try
            //            {
            //                if (this.FoodServiceClientSession != null)
            //                    this.FoodServiceClientSession.ClientName = friendlyName;
            //            }
            //            catch (System.Net.WebException commError)
            //            {
            //                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2));
            //            }
            //            catch (Exception error)
            //            {
            //                await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(2));
            //            }
            //        }
            //        return true;
            //    });
            //    #endregion
            //}

        }

        /// <MetaDataID>{8854bfe2-67ed-42ca-964b-f4b37a1493c1}</MetaDataID>
        string _Trademark;
        /// <MetaDataID>{65f97bd7-2d60-4296-8453-f92181151fd2}</MetaDataID>
        public string Trademark
        {
            get
            {
                return _Trademark;
            }
            set
            {
                _Trademark = value;
            }
        }

        /// <exclude>Excluded</exclude>
        FlavourBusinessFacade.ViewModel.IUser _EndUser;
        /// <MetaDataID>{ec560ede-ff68-4507-993e-b06ec3c912ed}</MetaDataID>
        public FlavourBusinessFacade.ViewModel.IUser EndUser
        {
            get
            {
                return _EndUser;
            }
            set
            {
                _EndUser = value;
            }
        }
        #endregion

        /// <MetaDataID>{1756d916-aa99-42fa-83f8-b0d19447c13a}</MetaDataID>
        public string ISOCurrencySymbol
        {
            get
            {
                return System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol;
            }
        }

        /// <MetaDataID>{9da82eed-28ed-4ce5-8ed0-5f28130b64df}</MetaDataID>
        [CachingDataOnClientSide]
        public bool WaiterView { get; set; }








        //static string AzureServerUrl = "http://localhost:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.5:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";
        /// <MetaDataID>{ce3e3555-c6b1-4fa2-ab40-abac321bce3d}</MetaDataID>
        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";



        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);



#if DeviceDotNet
        public DeviceUtilities.NetStandard.ScanCode ScanCode;
#endif


        bool Initialized;
        /// <MetaDataID>{7c812852-1690-4bdb-bbb4-2605f03476ab}</MetaDataID>
        internal async Task Initialize()
        {

            if (Initialized)
                return;

            Initialized = true;
#if DeviceDotNet
            ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif

            _EndUser = new FoodServiceClientVM();
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            await Task.Run(async () =>
            {

                int? forceLoad = Places?.Count;


#if DeviceDotNet
                 (Application.Current as IAppLifeTime).ApplicationResuming += ApplicationResuming;
                 (Application.Current as IAppLifeTime).ApplicationSleeping += ApplicationSleeping;

                 device.MessageReceived += Device_MessageReceived;
#endif
                if (ApplicationSettings.Current.LastServicePoinMenuData != null)
                    FoodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel(ApplicationSettings.Current.LastServicePoinMenuData, this);

                string path = ApplicationSettings.Current.Path;
                if (!string.IsNullOrWhiteSpace(path) && path.Split('/').Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine(ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity);
                    if (ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity != path.Split('/')[0])
                        Path = "";
                }

                if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePoinMenuData.FoodServiceClientSessionUri))
                {
                    var tryStartigTime = DateTime.Now;
                    while (!await GetFoodServicesClientSessionData(ApplicationSettings.Current.LastServicePoinMenuData.FoodServiceClientSessionUri))    //while (!await GetServicePointData(MenuData.ServicePointIdentity))
                    {
                        if (string.IsNullOrWhiteSpace(Path) || Path.Split('/')[0] != ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity)
                            break;
                    }
                    if (this.FoodServicesClientSession != null)
                    {
                        GetMessages();
                    }
                    else
                    {
                        _ObjectChangeState?.Invoke(this, nameof(FoodServicesClientSession));
                        ApplicationSettings.Current.LastServicePoinMenuData = new MenuData() { OrderItems = new List<ItemPreparation>() };
                        ApplicationSettings.Current.LastClientSessionID = "";
                        Path = "";
                    }
                }

            });
        }

        IList<Messmate> _CandidateMessmates = new List<Messmate>();

        public IList<Messmate> CandidateMessmates
        {
            get
            {
                if (FoodServicesClientSessionViewModel == null)
                    return _CandidateMessmates;
                else
                    return FoodServicesClientSessionViewModel.GetCandidateMessmates();

            }
        }

        /// <MetaDataID>{6133970c-efe1-4c70-a5c0-2162b7b2dda9}</MetaDataID>
        Message PendingPartOfMealMessage;
        internal void ImplicitMealInvitation(string servicesContextIdentity, string servicePointIdentity, string clientSessionID)
        {
            //"http://192.168.2.8:4300/#/launch-app?mealInvitation=True&sc=7f9bde62e6da45dc8c5661ee2220a7b0&sp=fe51ba7e30954ee08209bd89a03469a8&cs=827a9ed57dac4786a923cd27d0b52444"

            string invitationUri = GetMealInvitationUri(servicesContextIdentity, servicePointIdentity, clientSessionID);
            var messmate = CandidateMessmates.Where(x => x.ClientSessionID == clientSessionID).FirstOrDefault();
            if (messmate == null)
            {
                var theInviter = ServicesContextManagment.GetMealInvitationInviter(invitationUri);
                messmate = new Messmate(theInviter, new List<ItemPreparation>());
                CandidateMessmates.Add(messmate);
            }

            var message = new Message();
            message.Data["ClientMessageType"] = ClientMessages.PartOfMealRequest;
            message.Data["ClientSessionID"] = clientSessionID;
            message.Notification = new Notification() { Title = "Make me part of meal" };
            message.Data["MealInvitationUri"] = invitationUri;

            PartOfMealRequestMessageForward(message);
        }

        /// <MetaDataID>{61e17863-b7ff-4963-9d5b-12ab551a9369}</MetaDataID>
        private void PartOfMealRequestMessageForward(Message message)
        {
            if (PendingPartOfMealMessage != null && PendingPartOfMealMessage.MessageID == message.MessageID)
                return;
            var messmate = (from theMessmate in this.CandidateMessmates
                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                            select theMessmate).FirstOrDefault();
            if (messmate != null && messmate.ClientSession.ServicePoint != FoodServicesClientSessionViewModel?.FoodServicesClientSession?.ServicePoint)
            {
                //invitation from service point other than current session service point.
                PendingPartOfMealMessage = message;
                if (_PartOfMealRequest != null)
                    _PartOfMealRequest?.Invoke(this, messmate, message.MessageID);
                else
                    PartOfMealMessage = message;
            }
            else
            {
                if (_PartOfMealRequest != null)
                {


                    if (FoodServicesClientSessionViewModel!=null&& FoodServicesClientSessionViewModel.MessmatesLoaded)
                    {
                        PendingPartOfMealMessage = message;

                        if (messmate == null)
                        {
                            var candidateMessmates = (from clientSession in FoodServicesClientSession.GetPeopleNearMe()
                                                      select new Messmate(clientSession, OrderItems)).ToList();
                            this.CandidateMessmates = candidateMessmates;
                            messmate = (from theMessmate in this.CandidateMessmates
                                        where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                        select theMessmate).FirstOrDefault();

                        }
                        if (messmate == null)
                        {
                            messmate = (from theMessmate in this.Messmates
                                        where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                        select theMessmate).FirstOrDefault();
                            if (messmate != null)
                            {
                                FoodServicesClientSession.RemoveMessage(message.MessageID);
                                return;
                            }
                        }
                        _PartOfMealRequest?.Invoke(this, messmate, message.MessageID);

#if DeviceDotNet

                        var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                        OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                        if (device.IsinSleepMode)
                        {
                            var t = Task.Run(async delegate
                            {
                                while (device.IsinSleepMode)
                                {
                                    await Task.Delay(2000);
                                    if (!device.IsinSleepMode)
                                        break;

                                    device.PlaySound();
                                    await Task.Delay(2000);
                                    if (!device.IsinSleepMode)
                                        break;

                                    var duration = TimeSpan.FromSeconds(1);
                                    Vibration.Vibrate(duration);

                                }

                            });
                        }
#endif
                    }
                }
                else
                    PartOfMealMessage = message;
            }
        }


        FoodServicesClientSessionViewModel FoodServicesClientSessionViewModel;
        /// <summary>
        /// Open a new or get existing session for service point (servicePointIdentity)
        /// This object is the proxy of server side client session 
        /// </summary>
        /// <param name="servicePointIdentity">
        /// Defines the service point identity. 
        /// Service point identity can be two part identity service context identity and service point identity
        /// or meal invitation identity with three parts service context identity and service point identity and invitation client session
        /// </param>
        /// <returns></returns>
        /// <MetaDataID>{47842b22-c95e-4a09-8f5b-a1eaaba8b014}</MetaDataID>
        public Task<bool> ConnectToServicePoint(string servicePointIdentity = "")
        {
            if (servicePointIdentity != DontWaitApp.ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity)
            {
                FoodServicesClientSessionViewModel = null;
                //DontWaitApp.ApplicationSettings.Current.LastServicePoinMenuData = null;
                //OrderItems.Clear();
            }

#if IOSEmulator
              return Task<MenuData>.Run(async () =>
           {
               var foodServiceClientSession = await GetFoodServiceSession("");

               if (FoodServiceClientSession != foodServiceClientSession)
               {
                   if (FoodServiceClientSession != null)
                       FoodServiceClientSession.MessageReceived -= MessageReceived;
                   FoodServiceClientSession = foodServiceClientSession;
                   FoodServiceClientSession.MessageReceived += MessageReceived;
               }
               var storeRef = FoodServiceClientSession.Menu;
               MenuData menuData = new MenuData() { MenuName = storeRef.Name, MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1), MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1) };
               menuData.OrderItems = OrderItems.Values.ToList();
               return menuData;
           });
#else

#if DeviceDotNet2
            lock (this)
            {
                if (OnScan && ConnectToServicePointTask != null)
                    return ConnectToServicePointTask.Task;

                OnScan = true;
                ConnectToServicePointTask = new TaskCompletionSource<bool>();
            }
            Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            {
                await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            });
            return ConnectToServicePointTask.Task;
#else
            return Task<MenuData>.Run(async () =>
            {
                //if (string.IsNullOrWhiteSpace(servicePointIdentity))
                //    servicePointIdentity = "MealInvitation;7f9bde62e6da45dc8c5661ee2220a7b0;fe51ba7e30954ee08209bd89a03469a8;38ec58d3bc5a4145b1a94851cfc43ade91000000296";
#if DeviceDotNet
                if (string.IsNullOrWhiteSpace(servicePointIdentity))
                {
                    var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically");
                    servicePointIdentity = result.Text;
                }
#endif

                var clientSessionData = await GetFoodServiceSession(servicePointIdentity);
                FoodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel(clientSessionData);


                return true;
            });
#endif
#endif
        }



        #region Meal Invitation code


        /// <summary>
        /// Makes meal invitation to candidate messmate
        /// </summary>
        /// <param name="messmate">
        /// Defines the messmate who will be invited for meal.
        /// </param>
        /// <MetaDataID>{61c4b585-5ae6-4b38-8922-b9e56fe6d335}</MetaDataID>
        public void MealInvitation(Messmate messmate)
        {
            Task.Run(() =>
            {
                var clientSession = (from theMessmate in this.CandidateMessmates
                                     where theMessmate.ClientSessionID == messmate.ClientSessionID
                                     select theMessmate.ClientSession).FirstOrDefault();


                clientSession.MealInvitation(this.FoodServicesClientSession);
            });
        }


        public async Task<Contact> PickContact()
        {
            Contact contuct = null;

#if DeviceDotNet_a
            var x_contact= await Xamarin.Essentials.Contacts.PickContactAsync();
            if (x_contact != null)
            {
                contuct = new Contact(x_contact.Id,
                                            x_contact.NameSuffix,
                                            x_contact.GivenName,
                                            x_contact.MiddleName,
                                            x_contact.FamilyName,
                                            x_contact.NameSuffix,
                                            x_contact.Phones,
                                            x_contact.Emails);
                var contact_json = OOAdvantech.Json.JsonConvert.SerializeObject(contuct);

            }
#else
            var contact_json = @"{
                          ""Id"": ""116"",
                          ""DisplayName"": ""Δημήτρης Λιάκος"",
                          ""NamePrefix"": null,
                          ""GivenName"": ""Δημήτρης"",
                          ""MiddleName"": null,
                          ""FamilyName"": ""Λιάκος"",
                          ""NameSuffix"": null,
                          ""Phones"": [
                            ""6972992632"",
                            ""2108822590""
                          ],
                          ""Emails"": [
                            ""jim.liakos@gmail.com"",
                            ""jim.liakos@hotmail.com"",
                            ""jim.dontwait@hotmail.com""
                          ]
                        }";
            contuct = OOAdvantech.Json.JsonConvert.DeserializeObject<Contact>(contact_json);
#endif

            return contuct;

        }

        public async void SendMealInvitationMessage(InvitationChannel channel, string endPoint)
        {
            string mealInvitationUri = GetMealInvitationUrl();

#if DeviceDotNet

            if (channel == InvitationChannel.SMS)
            {
                string messagePhone = endPoint;
                if (messagePhone != null)
                {
                    string message = "click meal invitation link" + Environment.NewLine + mealInvitationUri;
                    await SendSms(message, messagePhone);
                }
            }
            else if (channel == InvitationChannel.Email)
            {
                string emailAddress = endPoint;

                string htmlBody =String.Format( @"<a style=""color:red;"" href=""{0}""  >click meal invitation link.</a>", mealInvitationUri);// + Environment.NewLine + mealInvitationUri;
                string plainTextBody = "click meal invitation link." + Environment.NewLine + mealInvitationUri; 
                try
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await SendEmail(new List<string>() { emailAddress }, "Meal Invitation", htmlBody, EmailBodyFormat.Html);
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                        await SendEmail(new List<string>() { emailAddress }, "Meal Invitation", plainTextBody, EmailBodyFormat.PlainText);
                    }

                    
                }

                catch (FeatureNotSupportedException fbsEx)
                {
                    if (fbsEx.Message.IndexOf("PlainText") != -1)
                    {
                        try
                        {
                            var hResult = fbsEx.HResult;
                            await SendEmail(new List<string>() { emailAddress }, "Meal Invitation", plainTextBody, EmailBodyFormat.PlainText);

                        }
                        catch (Exception error)
                        {
                        }
                    }
                    // Email is not supported on this device  
                }
                catch (Exception ex)
                {
                    // Some other exception occurred  
                }

            }

#else
#if DEBUG
            mealInvitationUri = GetMealInvitationUrl();
#endif

#endif

        }
#if DeviceDotNet
        public async Task SendEmail(List<string> recipients, string subject, string body, EmailBodyFormat bodyFormat)
        {

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                To = recipients,
                BodyFormat = bodyFormat,
            };
            await Email.ComposeAsync(message);

        }


        public async Task SendSms(string messageText, string recipient)
        {

            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);

            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }

    }
#endif
        /// <summary>
        /// Cancel the previous invitation
        /// </summary>
        /// <param name="messmate">
        /// Defines the messmate who has been invited for meal.
        /// </param>
        /// <MetaDataID>{15847442-73d8-4818-a521-954119101cb7}</MetaDataID>
        public void CancelMealInvitation(Messmate messmate)
        {
            Task.Run(() =>
            {
                var clientSession = (from theMessmate in this.CandidateMessmates
                                     where theMessmate.ClientSessionID == messmate.ClientSessionID
                                     select theMessmate.ClientSession).FirstOrDefault();

                if (clientSession != null)
                    clientSession.CancelMealInvitation(this.FoodServicesClientSession);
            });
        }

        /// <summary>
        /// Creates and return a qr code image as base64 string with meal invitation code
        /// </summary>
        /// <param name="color">
        /// Defines the color of qr code image
        /// </param>
        /// <returns>
        /// Returns qr code image as base64 string
        /// </returns>
        /// <MetaDataID>{61a707f3-a8c3-4528-8171-0f505f779c28}</MetaDataID>
        public string GetMealInvitationQRCode(string color)
        {

            if (ApplicationSettings.Current.LastServicePoinMenuData == null)
                return null;
            string SigBase64 = "";

            string codeValue = GetMealInvitationUri();
#if DeviceDotNet
            var barcodeWriter = new BarcodeWriterGeneric()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400
                }
            };


            var bitmapMatrix = barcodeWriter.Encode(codeValue);
            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];
            SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

            SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
            if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                fgColor = SkiaSharp.SKColors.Black;

            var pixels = qrCodeImage.Pixels;
            int k = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixels[k++] = fgColor;
                    else
                        pixels[k++] = SkiaSharp.SKColors.White;
                }
            }
            qrCodeImage.Pixels = pixels;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                d.SaveTo(ms);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }



#else
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
#endif

            return SigBase64;
            //return @"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAA0QAAANEBqyQtcAAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTAw9HKhAAAAhUlEQVQ4T+2MsQ2AMAwEEQVVFmCjVAzBFMxGhcQ2bEBlHL9DCDwLIIqT7Pf7GhHhjH1QZifQjkJDf14VcdJMJY/AinjYlc1JM5VUixXK86AsTpqp5P0ZGQSYqeQqiF7AM7IiwJ4lMWdXQat0546sFiDrrOt7OTCY4AYNT37BRwSTwW6GNAdnJbxPs8oKKwAAAABJRU5ErkJggg==";

        }

        private string GetMealInvitationUri()
        {
            var lastServicePoinMenuData = ApplicationSettings.Current.LastServicePoinMenuData;
            if (lastServicePoinMenuData.ClientSessionID == null)
            {
                lastServicePoinMenuData = ApplicationSettings.Current.LastServicePoinMenuData;
            }

            string codeValue = "MealInvitation;" + lastServicePoinMenuData.ServicePointIdentity + ";" + lastServicePoinMenuData.ClientSessionID;
            return codeValue;
        }

        internal static string GetMealInvitationUri(string serviceContextIdentity, string servicePointIdentity, string clientSessionID)
        {
            string codeValue = "MealInvitation;" + serviceContextIdentity + ";" + servicePointIdentity + ";" + clientSessionID;
            return codeValue;

        }

        private string GetMealInvitationUrl()
        {
            return this.FoodServicesClientSession?.MealInvitationUrl;
        }




        /// <MetaDataID>{cebd99a0-8d4c-420a-a861-5aabc396dae5}</MetaDataID>
        Message PartOfMealMessage;



        /// <MetaDataID>{546af2d0-8204-44e9-83fc-4d0d782e30f3}</MetaDataID>
        public async Task<bool> AcceptInvitation(Messmate messmate, string messageID)
        {

            var clientSession = (from theMessmate in this.CandidateMessmates
                                 where theMessmate.ClientSessionID == messmate.ClientSessionID
                                 select theMessmate.ClientSession).FirstOrDefault();

            if (FoodServicesClientSessionViewModel?.FoodServicesClientSession?.ServicePoint == clientSession.ServicePoint)
            {
                if (PendingPartOfMealMessage != null && PendingPartOfMealMessage.MessageID == messageID)
                    PendingPartOfMealMessage = null;


                //invitation from current session service point.

                if (clientSession == null && this.FoodServicesClientSessionViewModel.GetMessmates().Where(x => x.ClientSessionID == messmate.ClientSessionID).FirstOrDefault() != null)
                    return false;

                try
                {
                    if (messageID != null)
                        FoodServicesClientSessionViewModel?.FoodServicesClientSession.RemoveMessage(messageID);

                    FoodServicesClientSessionViewModel?.FoodServicesClientSession?.AcceptMealInvitation(FoodServicesClientSessionViewModel?.FoodServicesClientSession ?.ClientSessionToken, clientSession);
                    var ss = FoodServicesClientSessionViewModel?.FoodServicesClientSession?.MainSession;
                    var ss1 = FoodServicesClientSessionViewModel?.FoodServicesClientSession?.MainSession?.SessionID;
                    if (ApplicationSettings.Current.LastServicePoinMenuData.MainSessionID != FoodServicesClientSessionViewModel?.MainSession?.SessionID)
                    {
                        
                        ApplicationSettings.Current.LastServicePoinMenuData.MainSessionID = FoodServicesClientSessionViewModel.MainSession?.SessionID;
                        
                    }
                    FoodServicesClientSessionViewModel.GetMessages();

                    return true;
                }
                catch (Exception authenticationError)
                {

                    //await Task<MenuData>.Run(async () =>
                    //{
                    //    var clientSessionData = await GetFoodServiceSession("");
                    //    this.FoodServicesClientSession = clientSessionData.FoodServiceClientSession;
                    //    RefreshMessmates();
                    //    this.ClientSessionToken = clientSessionData.Token;
                    //    this.FoodServicesClientSession.AcceptMealInvitation(ClientSessionToken, clientSession);
                    //});
                    return false;
                }

            }
            //else
            //{
            //    if (PendingPartOfMealMessage != null && PendingPartOfMealMessage.MessageID == messageID)
            //    {

            //        string mealInvitationUri = PendingPartOfMealMessage.Data["MealInvitationUri"] as string;
            //        PendingPartOfMealMessage = null;

            //        var connected = await ConnectToServicePoint(mealInvitationUri);
            //        if (connected)
            //            Path =this. MenuData.ServicePointIdentity;

            //        return connected;
            //    }
            //}
            return false;

        }

        /// <MetaDataID>{a4bed371-81f6-45e5-bd80-e825438ff80e}</MetaDataID>
        public void DenyInvitation(Messmate messmate, string messageID)
        {
            if (PendingPartOfMealMessage != null && PendingPartOfMealMessage.MessageID == messageID)
                PendingPartOfMealMessage = null;

            if (FoodServicesClientSession != null)
            {
                FoodServicesClientSession.RemoveMessage(messageID);
                var clientSession = (from theMessmate in this.CandidateMessmates
                                     where theMessmate.ClientSessionID == messmate.ClientSessionID
                                     select theMessmate.ClientSession).FirstOrDefault();
                clientSession.MealInvitationDenied(FoodServicesClientSession);
                GetMessages();
            }
        }


        [HttpInVisible]
        event PartOfMealRequestHandle _PartOfMealRequest;

        public bool HasPartOfMealRequestSubscribers
        {
            get
            {
                return _PartOfMealRequest != null;
            }
        }
        [HttpInVisible]
        public event PartOfMealRequestHandle PartOfMealRequest
        {
            add
            {
                if (PartOfMealMessage != null)
                {
                    var message = PartOfMealMessage;
                    PartOfMealMessage = null;
                    Task.Run(() =>
                    {
                        Thread.Sleep(2000);
                        if (FoodServicesClientSessionViewModel != null && FoodServicesClientSessionViewModel.MessmatesLoaded)
                        {
                            FoodServicesClientSessionViewModel.GetCandidateMessmates();
                            var messmate = (from theMessmate in this.CandidateMessmates
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();

                            if (messmate == null)
                            {
                                messmate = (from theMessmate in this.Messmates
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();
                                if (messmate != null)
                                    FoodServicesClientSession.RemoveMessage(message.MessageID);

                                return;
                            }

                            _PartOfMealRequest?.Invoke(this, messmate, message.MessageID);
                        }
                        else
                        {
                            var messmate = (from theMessmate in this.CandidateMessmates
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();
                            if (messmate != null)
                            {
                                _PartOfMealRequest?.Invoke(this, messmate, message.MessageID);
                            }
                        }
                    });
                }


                _PartOfMealRequest += value;

            }
            remove
            {
                _PartOfMealRequest -= value;
            }
        }

        #endregion





        /// <MetaDataID>{b2483c93-a9f2-41f9-b223-ea85797d1490}</MetaDataID>
        object ClientSessionLock = new object();

        /// <MetaDataID>{1507e7fe-4972-41be-a996-b8e3ccee29d3}</MetaDataID>
        Dictionary<string, Task<bool>> GetServicePointDataTasks = new Dictionary<string, Task<bool>>();

        IFlavoursServicesContextManagment _ServicesContextManagment;

        public IFlavoursServicesContextManagment ServicesContextManagment
        {
            get
            {
                if (_ServicesContextManagment == null)
                {
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;

                    _ServicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    _ServicesContextManagment.ObjectChangeState += ServicesContextManagment_ObjectChangeState;
                }
                return _ServicesContextManagment;

            }
        }
#if DeviceDotNet
        public Dictionary<string, Contact> Contacts { get; private set; }
#endif

        public Task<List<HomeDeliveryServicePointInfo>> NeighborhoodFoodServersTask;


        #region Permissions

        /// <MetaDataID>{11b11a15-ce5b-4d4f-aaaf-ff6e6427b9f1}</MetaDataID>
        public async Task<bool> CheckPermissionsForServicePointScan()
        {
#if DeviceDotNet
            //#if IOSEmulator
            //                    return true;
            //#else

            var locationInUsePermisions = await Permissions.CheckStatusAsync<Permissions.Camera>();
            return locationInUsePermisions == PermissionStatus.Granted;


            //var status = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
            //return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
            //#endif
#else
            return await Task<bool>.FromResult(false);
#endif
        }

        /// <MetaDataID>{d20a2a27-4af2-4642-a402-373c188de241}</MetaDataID>
        public async Task<bool> RequestPermissionsForServicePointScan()
        {
#if DeviceDotNet

            var locationInUsePermisions = await Permissions.RequestAsync<Permissions.Camera>();
            return locationInUsePermisions == PermissionStatus.Granted;

            //var status = (await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Camera))[Plugin.Permissions.Abstractions.Permission.Camera];
            //return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
#else
            return await Task<bool>.FromResult(true);
#endif
        }


        /// <MetaDataID>{7fd664dc-cdda-488a-ab38-6670e874af11}</MetaDataID>
        public Task<bool> CheckPermissionsPassivePushNotification()
        {

#if DeviceDotNet2
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IBatteryInfo batteryInfo = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IBatteryInfo)) as OOAdvantech.IBatteryInfo;
            return Task<bool>.Run(()=> { return batteryInfo.CheckIsEnableBatteryOptimizations(); });
#else
            return Task<bool>.FromResult(true);
#endif

        }
        /// <MetaDataID>{c6fbb6fd-26d1-4a63-bb9a-1b68e79f780c}</MetaDataID>
        public async Task<bool> RequestPermissionsPassivePushNotification()
        {
#if DeviceDotNet
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IBatteryInfo batteryInfo = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IBatteryInfo)) as OOAdvantech.IBatteryInfo;
            batteryInfo.StartSetting();
            return await CheckPermissionsPassivePushNotification();
#else
            return await Task<bool>.FromResult(true);
#endif
        }

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



        public async Task<bool> RequestPermissionsToAccessCurrentLocation()
        {
#if DeviceDotNet
            var locationInUsePermisions = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return locationInUsePermisions == PermissionStatus.Granted;
#else
            return await Task<bool>.FromResult(true);
#endif
        }

        /// <MetaDataID>{aeabec87-3214-4488-b5a6-77cba7b5bf51}</MetaDataID>
        public async Task<bool> RequestPermissionsForBatteryOptimazation()
        {
#if DeviceDotNet

            var batteryOptimazationPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Battery>();
            if (batteryOptimazationPermission == Xamarin.Essentials.PermissionStatus.Denied)
                batteryOptimazationPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Battery>();
            return batteryOptimazationPermission == Xamarin.Essentials.PermissionStatus.Granted;
#else
            return true;
#endif
        }
        #endregion


        /// <MetaDataID>{ba41dc5e-2e26-4830-963e-e9c3e5077f12}</MetaDataID>
        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            return this;
        }
        public string GetString(string langCountry, string key)
        {
            return "";
        }
        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();
        public string GetTranslation(string langCountry)
        {
            if (Translations.ContainsKey(langCountry))
                return Translations[langCountry].ToString();
            string json = "{}";
            var assembly = Assembly.GetExecutingAssembly();

#if DeviceDotNet
            string path = "DontWaitApp.i18n";
#else
            string path = "DontWaitApp.i18n";
#endif
            //var ssd = assembly.GetManifestResourceNames();
            string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains(path) && x.Contains(langCountry + ".json")).FirstOrDefault();

            //string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains("WaiterApp.WPF.i18n") && x.Contains(langCountry + ".json")).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(jsonName))
            {
                using (var reader = new System.IO.StreamReader(assembly.GetManifestResourceStream(jsonName), Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                    Translations[langCountry] = JObject.Parse(json);
                    // Do something with the value
                }
            }
            return json;
        }
        public void SetString(string langCountry, string key, string newValue)
        {
            throw new NotImplementedException();
        }


        public string AppIdentity => throw new NotImplementedException();

        /// <MetaDataID>{3b8f1bd2-0697-4d3e-81d2-e304771ac9b6}</MetaDataID>
        public void Speak(string text)
        {


            // MessmateAdded?.Invoke(this, "Liakos");


            if (TextToSpeech == null)
            {
                var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                TextToSpeech = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.Speech.ITextToSpeech)) as OOAdvantech.Speech.ITextToSpeech;
            }
            if (TextToSpeech != null)
                TextToSpeech.Speak(text);


        }




        /// <MetaDataID>{7de752f6-71be-4fb9-8c3b-789d04c70d8e}</MetaDataID>
        OOAdvantech.Speech.ITextToSpeech TextToSpeech;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event OOAdvantech.ObjectChangeStateHandle _ObjectChangeState;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState
        {
            add
            {
                _ObjectChangeState += value;
            }
            remove
            {
                _ObjectChangeState -= value;
            }
        }


        /// <MetaDataID>{afbf90c0-ee2f-4692-9ef0-8ed3c80f7bd2}</MetaDataID>
        public IUser CurrentUser;

        /// <MetaDataID>{0a4b8da3-005a-4dad-9728-12c5fb1ea1dd}</MetaDataID>
        Task<ClientSessionData> GetFoodServiceSession(string servicePointID, bool create = true)
        {
            return Task<IFoodServiceClientSession>.Run(async () =>
            {
                try
                {
                    string servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7";
                    servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;50886542db964edf8dec5734e3f89395";
                    if (servicePointID != null && servicePointID.Split(';').Length > 1)
                        servicePoint = servicePointID;

                    //string servicePoint = "ca33b38f5c634fd49c50af60b042f910;8dedb45522ad479480e113c59d4bbdd0";
                    //servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;8dedb45522ad479480e113c59d4bbdd0";
                    //// servicePoint = "6746e4178dd041f09a7b4130af0edacf;6171631179bf4c26aeb99546fdce6a7a";
                    //servicePoint = "b5ec4ed264c142adb26b73c95b185544;9967813ee9d943db823ca97779eb9fd7";

                    OOAdvantech.IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    ClientSessionData? foodServiceClientSession = null;
                    do
                    {
                        try
                        {
                            foodServiceClientSession = ServicesContextManagment.GetClientSession(servicePoint, await GetFriendlyName(), device.DeviceID, device.FirebaseToken, create);
                        }
                        catch (System.Net.WebException connectionError)
                        {
                            if (connectionError.Status != System.Net.WebExceptionStatus.ConnectFailure)
                                throw connectionError;
                        }
                        catch (TimeoutException timeoutError)
                        {
                        }
                    } while (foodServiceClientSession == null);

                    //menuData.MenuRoot = "http://192.168.2.3/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                    //menuData.MenuFile = "Marzano Phone.json";
                    //menuData.MenuName = "Marzano Phone";
                    //menuData.MenuRoot = "http://192.168.2.8/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                    //menuData.MenuFile = "Marzano Phones.json";
                    //menuData.MenuName = "Marzano Phone";
                    return foodServiceClientSession.Value;
                }
                catch (Exception error)
                {
                    throw;
                }
            });

        }



        /// <summary>
        /// this method gets food services client session  data and synchronize caching data 
        /// </summary>
        /// <param name="foodServicesClientSessionUri">
        /// Defines the Uri of food services client session necessary to access the  FoodServicesClientSession from server
        /// </param>
        /// <returns>
        /// true when device connected to server successfully 
        /// otherwise return false
        /// </returns>
        public Task<bool> GetFoodServicesClientSessionData(string foodServicesClientSessionUri)
        {

            lock (ClientSessionLock)
            {
                if (foodServicesClientSessionUri != DontWaitApp.ApplicationSettings.Current.LastServicePoinMenuData.FoodServiceClientSessionUri)
                {
                    //ApplicationSettings.Current.LastServicePoinMenuData = null;
                    //OrderItems.Clear();
                    FoodServicesClientSessionViewModel = null;
                }


                Task<bool> getServicePointDataTask = null;
                GetServicePointDataTasks.TryGetValue(foodServicesClientSessionUri, out getServicePointDataTask);
                if (getServicePointDataTask != null && !getServicePointDataTask.IsCompleted)
                    return getServicePointDataTask; // returns the active task to get service point data

                //There isn't active task.
                //Starts task to get service point data
                getServicePointDataTask = Task<bool>.Run(async () =>
                {

                    try
                    {
                        DateTime start = DateTime.UtcNow;
                        var foodServiceClientSession = RemotingServices.GetPersistentObject<IFoodServiceClientSession>(AzureServerUrl, foodServicesClientSessionUri);
                        if (foodServiceClientSession != null && foodServiceClientSession.SessionState != ClientSessionState.Closed)
                        {
                            var clientSessionData = foodServiceClientSession.ClientSessionData;
                            FoodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel(clientSessionData);
                        }
                        else
                        {
                            FoodServicesClientSessionViewModel = null;
                            return true;
                        }
                    }
                    catch (Exception error)
                    {

                        return false;
                    }
                    return true;

                });
                GetServicePointDataTasks[foodServicesClientSessionUri] = getServicePointDataTask;
                return getServicePointDataTask;
            }


        }


        /// <MetaDataID>{69e24f40-d506-4e2e-b839-9d288ab735f1}</MetaDataID>
        public Task<bool> IsSessionActive()
        {
            return Task<bool>.Run(async () =>
            {
                if (FoodServicesClientSessionViewModel != null)
                    return true;
                else
                {
                    if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePoinMenuData.FoodServiceClientSessionUri))
                    {

                        while (!await GetFoodServicesClientSessionData(ApplicationSettings.Current.LastServicePoinMenuData.FoodServiceClientSessionUri))//!await GetServicePointData(MenuData.ServicePointIdentity))
                        {
                            if (string.IsNullOrWhiteSpace(Path) || Path.Split('/')[0] != ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity)
                                break;
                        }

                        if (FoodServicesClientSessionViewModel != null)
                            return true;
                        else
                        {
                            ApplicationSettings.Current.LastServicePoinMenuData = new MenuData() { OrderItems = new List<ItemPreparation>() };
                            ApplicationSettings.Current.LastClientSessionID = "";
                            Path = "";

                            return false;
                        }
                    }
                    return false;
                }
            });
        }

        /// <MetaDataID>{8e393af4-d3d0-4dc5-b9c5-8043d5950566}</MetaDataID>
        private void ServicesContextManagment_ObjectChangeState(object _object, string member)
        {
            var obj = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(_object);

        }













    }
}
