using System;
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
using System.Net;

using static System.Net.WebRequestMethods;
using FinanceFacade;








#if DeviceDotNet
using ZXing.QrCode.Internal;
using Xamarin.Forms;
using Xamarin.Essentials;
using ZXing.Net.Mobile.Forms;
using ZXing;
using ZXing.QrCode;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
using OOAdvantech.Pay;
#else
using QRCoder;
using System.IO;
using System.Drawing.Imaging;
#endif

namespace DontWaitApp
{






    /// <MetaDataID>{cab2cac1-0d34-4bcd-b2c4-81e4a9f915c3}</MetaDataID>
    public class FlavoursOrderServer : MarshalByRefObject, IFlavoursOrderServer, FlavourBusinessFacade.ViewModel.ILocalization, OOAdvantech.Remoting.IExtMarshalByRefObject, IBoundObject
    {

        /// <MetaDataID>{03115271-880a-448a-8d34-e29ab8586c17}</MetaDataID>
        public int Age = 12;
        /// <MetaDataID>{5f360b37-d769-4114-a29c-43bbcbfeffd1}</MetaDataID>
        Dictionary<Coordinate, List<HomeDeliveryServicePointInfo>> NeighborhoodFoodServers = new Dictionary<Coordinate, List<HomeDeliveryServicePointInfo>>();

        /// <MetaDataID>{1f438d1f-dd05-4cac-ac59-4c1a7d3e9b96}</MetaDataID>
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



        /// <MetaDataID>{aa0d2f51-3139-4cf2-8abf-7b6d91896e72}</MetaDataID>
        public async Task<bool> OpenFoodServicesClientSession(string clientSessionID)
        {
            FoodServicesClientSessionViewModel foodServicesClientSessionViewModel = ApplicationSettings.Current.ActiveSessions.Where(x => x.ClientSessionID == clientSessionID).FirstOrDefault();
            if (foodServicesClientSessionViewModel == null || (await foodServicesClientSessionViewModel.IsActive()) != true)
            {
                FoodServicesClientSessionViewModel = null;
                return false;
            }
            ApplicationSettings.Current.DisplayedFoodServicesClientSession = foodServicesClientSessionViewModel;
            FoodServicesClientSessionViewModel = foodServicesClientSessionViewModel;
            return true;
        }

        public Task<bool> OpenFoodServicesClientSession(IFoodServiceClientSession foodServiceClientSession)
        {
            FoodServicesClientSessionViewModel foodServicesClientSessionViewModel = ApplicationSettings.Current.ActiveSessions.Where(x => x.ClientSessionID == foodServiceClientSession.SessionID).FirstOrDefault();
            var clientSessionData = foodServiceClientSession.ClientSessionData;
            if (foodServicesClientSessionViewModel==null)
            {
                
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    foodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel();
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ApplicationSettings.Current).CommitTransientObjectState(foodServicesClientSessionViewModel);
                    foodServicesClientSessionViewModel.Init(clientSessionData, this);
                    ApplicationSettings.Current.AddClientSession(foodServicesClientSessionViewModel);
                    stateTransition.Consistent = true;
                }
                ApplicationSettings.Current.DisplayedFoodServicesClientSession = foodServicesClientSessionViewModel;
                FoodServicesClientSessionViewModel = foodServicesClientSessionViewModel;
            }
            else
            {
                ApplicationSettings.Current.DisplayedFoodServicesClientSession = foodServicesClientSessionViewModel;
                foodServicesClientSessionViewModel.Init(clientSessionData, this);
                FoodServicesClientSessionViewModel = foodServicesClientSessionViewModel;
            }
            return Task<bool>.FromResult(true);
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

#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "FlavoursOrderServer Resuming" });

            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
#endif
            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (this.FoodServicesClientSessionViewModel?.FoodServicesClientSession != null)
                        {
                            try
                            {
#if DeviceDotNet
                                OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "FlavoursOrderServer after DeviceResume" });


                                if (!device.IsinSleepMode)
                                    this.FoodServicesClientSessionViewModel.FoodServicesClientSession.DeviceResume();
#else
                                this.FoodServicesClientSessionViewModel.FoodServicesClientSession.DeviceResume();
#endif
                            }
                            catch (OOAdvantech.Remoting.MissingServerObjectException error)
                            {
#if DeviceDotNet
                                OOAdvantech.DeviceApplication.Current.Log(new List<string>() { "DeviceResume :"+ error.Message, error.StackTrace });
#endif
                                return await ConnectToServicePoint(this.FoodServicesClientSessionViewModel.MenuData.ServicePointIdentity);
                            }
                        }
                        if (FoodServicesClientSessionViewModel != null)
                            FoodServicesClientSessionViewModel.GetMessages();
                        break;
                    }
                    catch (System.Net.WebException commError)
                    {
#if DeviceDotNet
                        OOAdvantech.DeviceApplication.Current.Log(new List<string>() { $"2 ApplicationResuming error tries {tries} :{commError.Message}" });
#endif
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    catch (Exception error)
                    {
#if DeviceDotNet
                        OOAdvantech.DeviceApplication.Current.Log(new List<string>() { $"3 ApplicationResuming error tries {tries} :{error.Message}" });
#endif
                        var er = error;
                        await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    tries--;
                }


                return true;
            });



        }
        /// <exclude>Excluded</exclude>
        IList<IHallLayout> _Halls;

        /// <MetaDataID>{281fb596-ef84-4289-8537-2b05d1f11518}</MetaDataID>
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

        /// <MetaDataID>{989d4cce-c466-48ca-84ee-815df8a3f74f}</MetaDataID>
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
            //var run = SerializeTaskScheduler.Runs;
            //var remainingTasks = SerializeTaskScheduler.RemainingTasks;



            SerializeTaskScheduler.AddTask(async () =>
            {

#if DeviceDotNet
                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
#endif
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
#if DeviceDotNet
                        if (device.IsinSleepMode)
                            FoodServicesClientSessionViewModel?.FoodServicesClientSession?.DeviceSleep();
#else
                        FoodServicesClientSessionViewModel?.FoodServicesClientSession?.DeviceSleep();
#endif

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
                    tries--;
                }
                return true;
            });

        }


#if DeviceDotNet
        private void Device_MessageReceived(OOAdvantech.IRemoteMessage remoteMessage)
        {
            if (remoteMessage.Data.ContainsKey("MessageID") && FoodServicesClientSessionViewModel != null)
                FoodServicesClientSessionViewModel.Device_MessageReceived(remoteMessage);
        }
#endif

         

        public event WebViewLoadedHandle OnWebViewLoaded;

        /// <MetaDataID>{d52da524-96a8-4f0b-ae0f-703be4348ff2}</MetaDataID>
        string lan = "el";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;


        /// <MetaDataID>{0cff47a2-3b96-4019-bfab-e15d448b603f}</MetaDataID>
        public string Language { get { return lan; } }

        /// <MetaDataID>{a265f74d-25a6-4304-aef3-7653e47bb721}</MetaDataID>
        string deflan = "en";
        /// <MetaDataID>{37345ef4-1bfa-44c3-82ce-5539786fcb18}</MetaDataID>
        public string DefaultLanguage { get { return lan; } }

        /// <MetaDataID>{01c697c1-fd73-454c-89cd-1c72d7c39116}</MetaDataID>
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
                    if (ApplicationSettings.Current.DisplayedFoodServicesClientSession?.ServicePointIdentity != path.Split('/')[0])
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
            this.FoodServicesClientSessionViewModel?.GetMessages();

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
                if (!WaiterView)
                {
                    if (_EndUser is FoodServiceClientVM)
                        return (_EndUser as FoodServiceClientVM).GetFriendlyName();
                    else
                        return _EndUser.FullName;
                }
                else
                    return _EndUser.FullName;



            });


        }


        /// <MetaDataID>{60462cb1-1349-470d-87c9-923b2e7fe825}</MetaDataID>
        public void SetFriendlyName(string friendlyName)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                if (!WaiterView)
                    (_EndUser as FoodServiceClientVM).SetFriendlyName(friendlyName);
                else
                    _EndUser.FullName = friendlyName;

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
        FlavourBusinessFacade.ViewModel.ISecureUser _EndUser;
        /// <MetaDataID>{ec560ede-ff68-4507-993e-b06ec3c912ed}</MetaDataID>
        public FlavourBusinessFacade.ViewModel.ISecureUser EndUser
        {
            get
            {
                return _EndUser;
            }
            set
            {
                if (_EndUser!=null)
                    _EndUser.ObjectChangeState-=EndUser_ObjectChangeState;
                _EndUser = value;
                if (_EndUser!=null)
                    _EndUser.ObjectChangeState+=EndUser_ObjectChangeState;
            }
        }

        private void EndUser_ObjectChangeState(object _object, string member)
        {
            this._ObjectChangeState?.Invoke(this, member);
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
        internal static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);



#if DeviceDotNet
        public DeviceUtilities.NetStandard.ScanCode ScanCode;
#endif

        /// <MetaDataID>{bd654e13-a483-4d4b-960b-1640a04b9181}</MetaDataID>
        Task InitializationTask;
        /// <MetaDataID>{504eca6e-cf99-45c5-8d67-21c5f4968f31}</MetaDataID>
        bool Initialized;
        /// <MetaDataID>{7c812852-1690-4bdb-bbb4-2605f03476ab}</MetaDataID>
        public async Task Initialize()
        {

            if (Initialized)
                return;



            Initialized = true;
#if DeviceDotNet
            ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
            if (_EndUser == null)
            {
                _EndUser = new FoodServiceClientVM(this);
                _EndUser.ObjectChangeState += EndUser_ObjectChangeState;
            }

#if DeviceDotNet
            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
#endif
            InitializationTask = Task.Run(async () =>
            {

                int? forceLoad = (_EndUser as IGeocodingPlaces)?.Places?.Count;

#if DeviceDotNet

                (Application.Current as IAppLifeTime).ApplicationSleeping += ApplicationSleeping;
                (Application.Current as IAppLifeTime).ApplicationResuming += ApplicationResuming;

                device.MessageReceived += Device_MessageReceived;
#endif

                foreach (var foodServicesClientSession in ApplicationSettings.Current.ActiveSessions)
                    foodServicesClientSession.FlavoursOrderServer = this;

                var displayedFoodServicesClientSession = ApplicationSettings.Current.DisplayedFoodServicesClientSession;
                FoodServicesClientSessionViewModel = displayedFoodServicesClientSession;


                string path = ApplicationSettings.Current.Path;
                if (!string.IsNullOrWhiteSpace(path) && path.Split('/').Length > 0)
                {

                    if (displayedFoodServicesClientSession?.ServicePointIdentity != path.Split('/')[0])
                        Path = "";
                }

                if (displayedFoodServicesClientSession != null && (await displayedFoodServicesClientSession.IsActive()) != null && (await displayedFoodServicesClientSession.IsActive()).Value)
                {

                    displayedFoodServicesClientSession.GetMessages();
                }
                else
                {

                    ApplicationSettings.Current.DisplayedFoodServicesClientSession = null;
                    Path = "";
                    _ObjectChangeState?.Invoke(this, nameof(FoodServicesClientSessionViewModel));

                }
            });

            await InitializationTask;

            await InitializationTask;
        }

        /// <MetaDataID>{5886d9fe-ee65-4c38-9293-2890c236c474}</MetaDataID>
        IList<Messmate> _CandidateMessmates = new List<Messmate>();

        /// <MetaDataID>{02fbb0b8-00bb-4a08-af4e-27f777b4d93a}</MetaDataID>
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
        /// <MetaDataID>{b057d6d1-fafe-4a29-9912-89864b0c029d}</MetaDataID>
        internal void ImplicitMealInvitation(string servicesContextIdentity, string servicePointIdentity, string clientSessionID)
        {
            //"http://192.168.2.8:4300/#/launch-app?mealInvitation=True&sc=7f9bde62e6da45dc8c5661ee2220a7b0&sp=fe51ba7e30954ee08209bd89a03469a8&cs=827a9ed57dac4786a923cd27d0b52444"

            string invitationUri = GetMealInvitationUri(servicesContextIdentity, servicePointIdentity, clientSessionID);
            var messmate = CandidateMessmates.Where(x => x.ClientSessionID == clientSessionID).FirstOrDefault();
            if (messmate == null)
            {
                var theInviter = ServicesContextManagment.GetMealInvitationInviter(invitationUri);
                if (theInviter != null)
                {
                    messmate = new Messmate(theInviter, new List<ItemPreparation>());
                    CandidateMessmates.Add(messmate);
                }
                else
                    return;
            }

            var message = new Message();
            message.Data["ClientMessageType"] = ClientMessages.PartOfMealRequest;
            message.Data["ClientSessionID"] = clientSessionID;
            message.Notification = new Notification() { Title = "Make me part of meal" };
            message.Data["MealInvitationUri"] = invitationUri;

            PartOfMealRequestMessageForward(message);
        }

        /// <MetaDataID>{61e17863-b7ff-4963-9d5b-12ab551a9369}</MetaDataID>
        internal void PartOfMealRequestMessageForward(Message message)
        {
            FoodServicesClientSessionViewModel?.FoodServicesClientSession?.RemoveMessage("A1");
            if (PendingPartOfMealMessage != null && PendingPartOfMealMessage.MessageID == message.MessageID)
                return;
            var messmate = (from theMessmate in this.CandidateMessmates
                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                            select theMessmate).FirstOrDefault();
            FoodServicesClientSessionViewModel?.FoodServicesClientSession?.RemoveMessage("A2");
            if (messmate != null && messmate.ClientSession.ServicePoint != FoodServicesClientSessionViewModel?.FoodServicesClientSession?.ServicePoint)
            {
                //invitation from service point other than current session service point.
                PendingPartOfMealMessage = message;
                if (_PartOfMealRequest != null)
                    _PartOfMealRequest?.Invoke(this, messmate, message.MessageID);
                else
                    PartOfMealMessage = message;
                FoodServicesClientSessionViewModel?.FoodServicesClientSession?.RemoveMessage("A3");
            }
            else
            {
                FoodServicesClientSessionViewModel?.FoodServicesClientSession?.RemoveMessage("A4");
                if (_PartOfMealRequest != null)
                {


                    if (FoodServicesClientSessionViewModel != null && FoodServicesClientSessionViewModel.MessmatesLoaded)
                    {
                        PendingPartOfMealMessage = message;

                        if (messmate == null)
                        {

                            messmate = (from theMessmate in FoodServicesClientSessionViewModel.GetCandidateMessmates()
                                        where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                        select theMessmate).FirstOrDefault();

                        }
                        if (messmate == null)
                        {
                            messmate = (from theMessmate in FoodServicesClientSessionViewModel.GetMessmates()
                                        where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                        select theMessmate).FirstOrDefault();
                            if (messmate != null)
                            {
                                FoodServicesClientSessionViewModel.FoodServicesClientSession.RemoveMessage(message.MessageID);
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

        /// <MetaDataID>{a02f8ab0-6c5a-49af-b84c-5fee1271991d}</MetaDataID>
        internal void SessionIsNoLongerActive(FoodServicesClientSessionViewModel foodServicesClientSessionViewModel)
        {
            ApplicationSettings.Current.RemoveClientSession(foodServicesClientSessionViewModel);
            if (ApplicationSettings.Current.DisplayedFoodServicesClientSession == null)
            {
                Path = "";
                _ObjectChangeState?.Invoke(this, nameof(FoodServicesClientSessionViewModel));
#if DeviceDotNet
                DeviceApplication.Current.Log(new List<string>() { " _ObjectChangeState?.Invoke(this, nameof(FoodServicesClientSessionViewModel))" });
#endif
            }




        }


        /// <MetaDataID>{636f2c8c-bead-4c32-abbe-36fb33977ac0}</MetaDataID>
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
            if (servicePointIdentity != ApplicationSettings.Current.DisplayedFoodServicesClientSession?.ServicePointIdentity)
            {
                FoodServicesClientSessionViewModel = null;
                ApplicationSettings.Current.DisplayedFoodServicesClientSession = null;
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
                //if (string.IsNullOrWhiteSpace(servicePointIdentity))
                //{
                //    var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically");
                //    servicePointIdentity = result.Text;
                //}
#endif
                HomeDeliveryServicePointInfo homeDeliveryServicePointInfo;
                lock (NeighborhoodFoodServers)
                {
                    homeDeliveryServicePointInfo=NeighborhoodFoodServers.SelectMany(x => x.Value).Where(x => x.ServicePointIdentity== servicePointIdentity).FirstOrDefault();
                }
                FoodServicesClientSessionViewModel = await GetFoodServiceSession(servicePointIdentity, homeDeliveryServicePointInfo?.FlavoursServices);

                if (FoodServicesClientSessionViewModel != null)
                    ApplicationSettings.Current.DisplayedFoodServicesClientSession = FoodServicesClientSessionViewModel;

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


                clientSession.MealInvitation(this.FoodServicesClientSessionViewModel.FoodServicesClientSession);
            });
        }


        /// <MetaDataID>{46a51d00-d27d-425c-b434-a3c747d8f35e}</MetaDataID>
        public async Task<Contact> PickContact()
        {
            Contact contuct = null;

#if DeviceDotNet
            var x_contact = await Xamarin.Essentials.Contacts.PickContactAsync();
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

        /// <MetaDataID>{5f2ac279-5750-4c55-bca8-5518207a9d70}</MetaDataID>
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

                string htmlBody = String.Format(@"<a style=""color:red;"" href=""{0}""  >click meal invitation link.</a>", mealInvitationUri);// + Environment.NewLine + mealInvitationUri;
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
            await Xamarin.Essentials.Email.ComposeAsync(message);

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
                    clientSession.CancelMealInvitation(this.FoodServicesClientSessionViewModel.FoodServicesClientSession);
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

            if (ApplicationSettings.Current.DisplayedFoodServicesClientSession == null)
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

        /// <MetaDataID>{94762c9c-6a70-4d24-886e-39e408f5911a}</MetaDataID>
        private string GetMealInvitationUri()
        {
            string codeValue = "MealInvitation;" + FoodServicesClientSessionViewModel.ServicePointIdentity + ";" + FoodServicesClientSessionViewModel.ClientSessionID;

            //var lastServicePoinMenuData = ApplicationSettings.Current.LastServicePoinMenuData;
            //if (lastServicePoinMenuData.ClientSessionID == null)
            //{
            //    lastServicePoinMenuData = ApplicationSettings.Current.LastServicePoinMenuData;
            //}

            //string codeValue = "MealInvitation;" + lastServicePoinMenuData.ServicePointIdentity + ";" + lastServicePoinMenuData.ClientSessionID;
            return codeValue;
        }

        /// <MetaDataID>{fe19b1dc-15a0-4ed9-bbe6-9546f1c5b9e9}</MetaDataID>
        internal static string GetMealInvitationUri(string serviceContextIdentity, string servicePointIdentity, string clientSessionID)
        {
            string codeValue = "MealInvitation;" + serviceContextIdentity + ";" + servicePointIdentity + ";" + clientSessionID;
            return codeValue;

        }

        /// <MetaDataID>{5c603aef-20da-4d50-b547-15baddaca9eb}</MetaDataID>
        private string GetMealInvitationUrl()
        {
            return FoodServicesClientSessionViewModel?.FoodServicesClientSession?.MealInvitationUrl;
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

                    FoodServicesClientSessionViewModel?.FoodServicesClientSession?.AcceptMealInvitation(FoodServicesClientSessionViewModel?.ClientSessionToken, clientSession);
                    var ss = FoodServicesClientSessionViewModel?.FoodServicesClientSession?.MainSession;
                    var ss1 = FoodServicesClientSessionViewModel?.FoodServicesClientSession?.MainSession?.SessionID;
                    //if (ApplicationSettings.Current.DisplayedFoodServicesClientSession==null.MainSessionID != FoodServicesClientSessionViewModel?.MainSession?.SessionID)
                    //{

                    //    //ApplicationSettings.Current.LastServicePoinMenuData.MainSessionID = FoodServicesClientSessionViewModel.MainSession?.SessionID;
                    //    var menuData = ApplicationSettings.Current.LastServicePoinMenuData;
                    //    menuData.MainSessionID = FoodServicesClientSessionViewModel.MainSession?.SessionID;

                    //}
                    FoodServicesClientSessionViewModel?.GetMessages();

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
            else
            {
                if (PendingPartOfMealMessage != null && PendingPartOfMealMessage.MessageID == messageID)
                {

                    string mealInvitationUri = PendingPartOfMealMessage.Data["MealInvitationUri"] as string;
                    PendingPartOfMealMessage = null;

                    var connected = await ConnectToServicePoint(mealInvitationUri);
                    if (connected)
                        Path = this.FoodServicesClientSessionViewModel?.MenuData.ServicePointIdentity;

                    return connected;
                }
            }
            return false;

        }

        /// <MetaDataID>{a4bed371-81f6-45e5-bd80-e825438ff80e}</MetaDataID>
        public void DenyInvitation(Messmate messmate, string messageID)
        {
            if (PendingPartOfMealMessage != null && PendingPartOfMealMessage.MessageID == messageID)
                PendingPartOfMealMessage = null;

            if (FoodServicesClientSessionViewModel?.FoodServicesClientSession != null)
            {
                FoodServicesClientSessionViewModel?.FoodServicesClientSession.RemoveMessage(messageID);
                var clientSession = (from theMessmate in this.CandidateMessmates
                                     where theMessmate.ClientSessionID == messmate.ClientSessionID
                                     select theMessmate.ClientSession).FirstOrDefault();
                clientSession.MealInvitationDenied(FoodServicesClientSessionViewModel.FoodServicesClientSession);
                FoodServicesClientSessionViewModel.GetMessages();
            }
        }


        [HttpInVisible]
        event PartOfMealRequestHandle _PartOfMealRequest;

        /// <MetaDataID>{313be19c-251c-4351-8992-17cb367df6ec}</MetaDataID>
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
                                messmate = (from theMessmate in FoodServicesClientSessionViewModel.GetMessmates()
                                            where theMessmate.ClientSessionID == message.GetDataValue("ClientSessionID") as string
                                            select theMessmate).FirstOrDefault();
                                if (messmate != null)
                                    FoodServicesClientSessionViewModel.FoodServicesClientSession.RemoveMessage(message.MessageID);

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







        /// <MetaDataID>{1507e7fe-4972-41be-a996-b8e3ccee29d3}</MetaDataID>
        Dictionary<string, Task<bool>> GetServicePointDataTasks = new Dictionary<string, Task<bool>>();

        /// <MetaDataID>{70edd2db-3600-4604-b5a7-321c7e901e0a}</MetaDataID>
        IFlavoursServicesContextManagment _ServicesContextManagment;

        /// <MetaDataID>{4ef6c5b0-5d53-4851-b2ed-6cd516c865a8}</MetaDataID>
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

        /// <MetaDataID>{1e094cea-d0f9-47fb-ba32-9ac745bfcaa9}</MetaDataID>
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
        /// <MetaDataID>{c0940399-45e2-4b51-b66e-b0845718f098}</MetaDataID>
        /// <MetaDataID>{c0940399-45e2-4b51-b66e-b0845718f098}</MetaDataID>
        public string GetString(string langCountry, string key)
        {
            JObject jObject = null;
            if (!Translations.TryGetValue(langCountry, out jObject))
            {
                GetTranslation(langCountry);
                jObject = Translations[langCountry];

            }

            var keyParts = key.Split('.');
            int i = 0;
            foreach (string member in keyParts)
            {
                if (jObject == null)
                    return null;
                JToken jToken = null;
                if (i == keyParts.Length - 1)
                {
                    if (jObject.TryGetValue(member, out jToken))
                    {
                        if (jToken is JValue)
                            return (jToken as JValue).Value as string;
                    }
                    return null;
                }

                if (jObject.TryGetValue(member, out jToken))
                {
                    jObject = jToken as JObject;

                }
                else
                    return null;
                i++;
            }

            return null;
        }

        /// <MetaDataID>{d57f870a-f7e7-4b0c-8449-264b9ec2ca73}</MetaDataID>
        public void SetString(string langCountry, string key, string newValue)
        {
            JObject jObject = null;
            if (!Translations.TryGetValue(langCountry, out jObject))
            {
                GetTranslation(langCountry);
                jObject = Translations[langCountry];

            }

            var keyParts = key.Split('.');
            int i = 0;
            foreach (string member in keyParts)
            {
                if (jObject == null)
                    return;
                JToken jToken = null;
                if (i == keyParts.Length - 1)
                {
                    if (jObject.TryGetValue(member, out jToken))
                    {
                        if (jToken is JValue)
                            (jToken as JValue).Value = newValue;
                    }
                    else
                        jObject.Add(member, new JValue(newValue));
                }
                else
                {
                    if (jObject.TryGetValue(member, out jToken))
                        jObject = jToken as JObject;
                    else
                    {
                        jObject.Add(member, new JObject());
                        jObject = jObject[member] as JObject;
                    }
                }
                i++;
            }

        }
        /// <MetaDataID>{dcdc6be7-cdbd-4326-804b-333dfdac6e16}</MetaDataID>
        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();
        /// <MetaDataID>{b31f0c24-9ff1-4feb-ae9f-bf38f4ad022c}</MetaDataID>
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

        /// <MetaDataID>{476a7ac1-8847-481e-b1cd-b2b9874137d1}</MetaDataID>
        public string AppIdentity => throw new NotImplementedException();

        /// <MetaDataID>{df563e71-7bde-477c-82a6-b97a6b8d1f72}</MetaDataID>
        IFoodServicesClientSessionViewModel IFlavoursOrderServer.CurrentFoodServicesClientSession
        {
            get
            {
#if DeviceDotNet
                DeviceApplication.Current.Log(new List<string>() { "get_CurrentFoodServicesClientSession" });
#endif
                if(FoodServicesClientSessionViewModel==null)
                {

                }
                return FoodServicesClientSessionViewModel;
            }
        }


        /// <MetaDataID>{e3c37591-9eb6-44c0-8470-d9c0ac8491ad}</MetaDataID>
        public Task<List<IFoodServicesClientSessionViewModel>> ActiveSessions
        {
            get
            {
                return Task<List<IFoodServicesClientSessionViewModel>>.Run(async () =>
                   {

                       await InitializationTask;

                       var areNoLongerActiveSesions = new List<FoodServicesClientSessionViewModel>();

                       foreach (var foodServicesClientSession in ApplicationSettings.Current.ActiveSessions)
                       {

                           var isActive = await foodServicesClientSession.IsActive();

                           if (isActive.HasValue && isActive.Value == false)
                               areNoLongerActiveSesions.Add(foodServicesClientSession);
                       }
                       if (areNoLongerActiveSesions.Count > 0)
                       {
                           using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                           {
                               foreach (var foodServicesClientSession in areNoLongerActiveSesions)
                                   ApplicationSettings.Current.RemoveClientSession(foodServicesClientSession);

                               stateTransition.Consistent = true;
                           }
                       }

                       return ApplicationSettings.Current.ActiveSessions.OfType<IFoodServicesClientSessionViewModel>().ToList();
                   });
            }
        }
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
        Task<FoodServicesClientSessionViewModel> GetFoodServiceSession(string servicePointIdentity, IFlavoursServicesContextRuntime flavoursServices = null, bool create = true)
        {
            return Task<IFoodServiceClientSession>.Run(async () =>
            {
                try
                {
                    string defaultServicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7";
                    defaultServicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;50886542db964edf8dec5734e3f89395";
                    if (string.IsNullOrWhiteSpace(servicePointIdentity)||servicePointIdentity.Split(';').Length < 2)
                        servicePointIdentity = defaultServicePoint;

                    //string servicePoint = "ca33b38f5c634fd49c50af60b042f910;8dedb45522ad479480e113c59d4bbdd0";
                    //servicePoint = "7f9bde62e6da45dc8c5661ee2220a7b0;8dedb45522ad479480e113c59d4bbdd0";
                    //// servicePoint = "6746e4178dd041f09a7b4130af0edacf;6171631179bf4c26aeb99546fdce6a7a";
                    //servicePoint = "b5ec4ed264c142adb26b73c95b185544;9967813ee9d943db823ca97779eb9fd7";

                    OOAdvantech.IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    ClientSessionData? clientSessionData = null;
                    do
                    {
                        try
                        {
                            if (flavoursServices!=null)
                                clientSessionData = flavoursServices.GetClientSession(servicePointIdentity, null, await GetFriendlyName(), device.DeviceID, FlavourBusinessFacade.DeviceType.Phone, device.FirebaseToken, null, null, !WaiterView, create);
                            else
                                clientSessionData = ServicesContextManagment.GetClientSession(servicePointIdentity, await GetFriendlyName(), device.DeviceID, FlavourBusinessFacade.DeviceType.Phone, device.FirebaseToken, !WaiterView, create);

                        }
                        catch (System.Net.WebException connectionError)
                        {
                            if (connectionError.Status != System.Net.WebExceptionStatus.ConnectFailure)
                                throw connectionError;
                        }
                        catch (TimeoutException timeoutError)





                        {
                        }
                    } while (clientSessionData == null);

                    //menuData.MenuRoot = "http://192.168.2.3/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                    //menuData.MenuFile = "Marzano Phone.json";
                    //menuData.MenuName = "Marzano Phone";
                    //menuData.MenuRoot = "http://192.168.2.8/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/";
                    //menuData.MenuFile = "Marzano Phones.json";
                    //menuData.MenuName = "Marzano Phone";
                    if (clientSessionData == null)
                        return null;
                    var foodServiceClientSessionUri = RemotingServices.GetComputingContextPersistentUri(clientSessionData.Value.FoodServiceClientSession);
                    var foodServicesClientSessionViewModel = ApplicationSettings.Current.ActiveSessions.Where(x => x.FoodServiceClientSessionUri == foodServiceClientSessionUri).FirstOrDefault();
                    if (foodServicesClientSessionViewModel != null)
                    {
                        foodServicesClientSessionViewModel.FlavoursOrderServer=this;
                        foodServicesClientSessionViewModel.FoodServicesClientSession = clientSessionData.Value.FoodServiceClientSession;
                    }
                    else
                    {


                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel();
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ApplicationSettings.Current).CommitTransientObjectState(foodServicesClientSessionViewModel);
                            foodServicesClientSessionViewModel.Init(clientSessionData.Value, this);
                            ApplicationSettings.Current.AddClientSession(foodServicesClientSessionViewModel);
                            stateTransition.Consistent = true;
                        }
                    }

                    foodServicesClientSessionViewModel.FlavoursOrderServer = this;
                    return foodServicesClientSessionViewModel;
                }
                catch (Exception error)
                {
                    throw;
                }
            });

        }



        ///// <summary>
        ///// this method gets food services client session  data and synchronize caching data 
        ///// </summary>
        ///// <param name="foodServicesClientSessionUri">
        ///// Defines the Uri of food services client session necessary to access the  FoodServicesClientSession from server
        ///// </param>
        ///// <returns>
        ///// true when device connected to server successfully 
        ///// otherwise return false
        ///// </returns>
        ///// <MetaDataID>{88cec399-a0b4-4578-ae18-7e76c75d977f}</MetaDataID>
        //public Task<bool> GetFoodServicesClientSessionData(string foodServicesClientSessionUri)
        //{

        //    lock (ClientSessionLock)
        //    {
        //        if (foodServicesClientSessionUri != DontWaitApp.ApplicationSettings.Current.LastServicePoinMenuData.FoodServiceClientSessionUri)
        //        {
        //            //ApplicationSettings.Current.LastServicePoinMenuData = null;
        //            //OrderItems.Clear();
        //            FoodServicesClientSessionViewModel = null;
        //        }


        //        Task<bool> getServicePointDataTask = null;
        //        GetServicePointDataTasks.TryGetValue(foodServicesClientSessionUri, out getServicePointDataTask);
        //        if (getServicePointDataTask != null && !getServicePointDataTask.IsCompleted)
        //            return getServicePointDataTask; // returns the active task to get service point data

        //        //There isn't active task.
        //        //Starts task to get service point data
        //        getServicePointDataTask = Task<bool>.Run(async () =>
        //        {

        //            try
        //            {
        //                DateTime start = DateTime.UtcNow;
        //                var foodServiceClientSession = RemotingServices.GetPersistentObject<IFoodServiceClientSession>(AzureServerUrl, foodServicesClientSessionUri);
        //                if (foodServiceClientSession != null && foodServiceClientSession.SessionState != ClientSessionState.Closed)
        //                {
        //                    var clientSessionData = foodServiceClientSession.ClientSessionData;
        //                    FoodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel(clientSessionData, this);
        //                }
        //                else
        //                {
        //                    FoodServicesClientSessionViewModel = null;
        //                    return true;
        //                }
        //            }
        //            catch (Exception error)
        //            {

        //                return false;
        //            }
        //            return true;

        //        });
        //        GetServicePointDataTasks[foodServicesClientSessionUri] = getServicePointDataTask;
        //        return getServicePointDataTask;
        //    }


        //}


        /// <MetaDataID>{69e24f40-d506-4e2e-b839-9d288ab735f1}</MetaDataID>
        public Task<bool> IsSessionActive()
        {
            return Task<bool>.Run(async () =>
            {
                if (FoodServicesClientSessionViewModel != null && (await FoodServicesClientSessionViewModel.IsActive()) != null && (await FoodServicesClientSessionViewModel.IsActive()).Value)
                    return true;
                else if (FoodServicesClientSessionViewModel != null && (await FoodServicesClientSessionViewModel.IsActive()) != null && !(await FoodServicesClientSessionViewModel.IsActive()).Value)
                {
                    FoodServicesClientSessionViewModel = null;
                    ApplicationSettings.Current.RemoveClientSession(ApplicationSettings.Current.DisplayedFoodServicesClientSession);
                    Path = "";
                }
                return false;
                //{

                //    if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePoinMenuData.FoodServiceClientSessionUri))
                //    {
                //        var foodServicesClientSessionViewModel = new FoodServicesClientSessionViewModel(ApplicationSettings.Current.LastServicePoinMenuData, this);

                //        while (!await foodServicesClientSessionViewModel.GetOrUpdateFoodServicesClientSessionData())//!await GetServicePointData(MenuData.ServicePointIdentity))
                //        {
                //            if (string.IsNullOrWhiteSpace(Path) || Path.Split('/')[0] != ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity)
                //                break;
                //        }
                //        if (foodServicesClientSessionViewModel.FoodServicesClientSession != null)
                //            FoodServicesClientSessionViewModel = foodServicesClientSessionViewModel;

                //        if (FoodServicesClientSessionViewModel != null)
                //            return true;
                //        else
                //        {
                //            //ApplicationSettings.Current.LastServicePoinMenuData = new MenuData() { OrderItems = new List<ItemPreparation>() };
                //            ApplicationSettings.Current.LastClientSessionID = "";
                //            Path = "";

                //            return false;
                //        }
                //    }
                //    return false;
                //}
            });
        }

        /// <MetaDataID>{8e393af4-d3d0-4dc5-b9c5-8043d5950566}</MetaDataID>
        private void ServicesContextManagment_ObjectChangeState(object _object, string member)
        {
            var obj = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(_object);

        }
#if DeviceDotNet
        PaymentService PaymentService = new PaymentService();
#endif
        /// <MetaDataID>{a34440a6-11d3-4378-8cdc-58bc87f87269}</MetaDataID>
        internal async Task<bool> Pay(IPayment payment)
        {
            if (payment.State==PaymentState.Completed)
                return true;
#if DeviceDotNet


            var paymentService = new PaymentService();
            return await paymentService.Pay(payment, FlavourBusinessFacade.ComputingResources.EndPoint.Server, Device.RuntimePlatform == "iOS");
#else
            return true;
#endif
        }







        /// <MetaDataID>{2121552a-8aa5-4ff3-9d14-e6b8aeeee28e}</MetaDataID>
        internal AuthUser AuthUser;









    }
}
