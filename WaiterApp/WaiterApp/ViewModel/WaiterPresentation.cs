using FlavourBusinessFacade;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using DontWaitApp;
using FlavourBusinessFacade.EndUsers;
using System.Reflection;
using OOAdvantech.Json.Linq;
using FlavourBusinessFacade.RoomService;
using UIBaseEx;
using RestaurantHallLayoutModel;
using FlavourBusinessManager;



#if DeviceDotNet
using Xamarin.Essentials;
using DeviceUtilities.NetStandard;
using Xamarin.Forms;
using ZXing;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using QRCoder;
//using System.Drawing.Imaging;
using MarshalByRefObject = System.MarshalByRefObject;
#endif

namespace WaiterApp.ViewModel
{




    /// <MetaDataID>{5ec91d09-d693-4c4e-9dca-858a9e09a233}</MetaDataID>
    public class WaiterPresentation : MarshalByRefObject, INotifyPropertyChanged, IWaiterPresentation, FlavourBusinessFacade.ViewModel.ISecureUser, IServicePointSupervisor, FlavourBusinessFacade.ViewModel.ILocalization, OOAdvantech.Remoting.IExtMarshalByRefObject, IBoundObject
    {


        /// <MetaDataID>{0b94610a-c3b2-4e2f-b06b-02b4a261bd32}</MetaDataID>
        protected WaiterPresentation()
        {
            this.FlavoursOrderServer = new DontWaitApp.FlavoursOrderServer() { WaiterView = true, Halls = _Halls, EndUser = this };
        }
        /// <MetaDataID>{94887bc2-fef8-4fcc-af96-24e9ef4e71c3}</MetaDataID>
        static WaiterPresentation _Current;

        /// <MetaDataID>{ab4f306a-4e5e-4d88-b7d5-926cd6e2a15e}</MetaDataID>
        public static WaiterPresentation Current
        {
            get
            {
                if (_Current == null)
                    _Current = new WaiterPresentation();
                return _Current;
            }
        }

        /// <MetaDataID>{d52da524-96a8-4f0b-ae0f-703be4348ff2}</MetaDataID>
        string lan = "el";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;

        /// <MetaDataID>{0cff47a2-3b96-4019-bfab-e15d448b603f}</MetaDataID>
        public string Language { get { return lan; } }

        /// <MetaDataID>{aa111635-723d-4de6-a65b-7bf679f73f1a}</MetaDataID>
        string deflan = "en";
        /// <MetaDataID>{6a86b8c5-a1df-4b2d-8e67-93e04145bffe}</MetaDataID>
        public string DefaultLanguage { get { return deflan; } }

        /// <MetaDataID>{4304272a-c50e-4870-b451-40af03eeb405}</MetaDataID>
        static IAppLifeTime AppLifeTime
        {
            get
            {
#if DeviceDotNet
                return Application.Current as IAppLifeTime;
#else
                return System.Windows.Application.Current as IAppLifeTime;
#endif
            }
        }

        /// <MetaDataID>{33f97ad3-db86-4b6d-9492-aa4a4fedd141}</MetaDataID>
        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler
        {
            get
            {
                return AppLifeTime.SerializeTaskScheduler;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public event ObjectChangeStateHandle ObjectChangeState;

        public event LaytheTableRequestHandle LayTheTableRequest;

        public event ItemsReadyToServeRequesttHandle ItemsReadyToServeRequest;

        public event ServicePointChangeStateHandle ServicePointChangeState;

        public event MealConversationTimeoutHandle MealConversationTimeout;

        /// <MetaDataID>{4d17380a-0570-480d-a1ec-b2140d0ca76a}</MetaDataID>
        internal void ServingBatchUpdated(ServingBatchPresentation servingBatchPresentation)
        {
            if (servingBatchPresentation.ContextsOfPreparedItems == null || servingBatchPresentation.ContextsOfPreparedItems.Count == 0)
            {
                _ServingBatches.Remove(servingBatchPresentation.ServingBatch);
                servingBatchPresentation.Dispose();
            }

            ObjectChangeState?.Invoke(this, nameof(ServingBatches));
        }
        public void WillTakeCareMealConversationTimeout(string servicePointIdentity, string sessionIdentity)
        {
            this.Waiter.WillTakeCareMealConversationTimeout(servicePointIdentity, sessionIdentity);
        }

        /// <MetaDataID>{ad37da77-4203-47d2-af7e-094cd499c17a}</MetaDataID>
        string _SignInProvider;
        /// <MetaDataID>{44230b90-69cb-4ff5-9841-bcb77b77de42}</MetaDataID>
        public string SignInProvider
        {
            get
            {
                return _SignInProvider;// ApplicationSettings.Current.SignInProvider;
            }
            set
            {
                _SignInProvider = value;// ApplicationSettings.Current.SignInProvider = value;
            }
        }
        /// <MetaDataID>{6eedef83-ab80-40e4-9185-212aad63e241}</MetaDataID>
        string _Email;
        /// <MetaDataID>{d26cb029-ed3f-4379-9722-7206c981bc32}</MetaDataID>
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

        /// <MetaDataID>{58575c2e-ccc3-41be-bc9f-c27f33b0813b}</MetaDataID>
        string _FullName;
        /// <MetaDataID>{44faeb89-a15a-4a43-a992-71bed4bbc4da}</MetaDataID>
        public string FullName
        {
            get
            {
                return _FullName;
            }

            set
            {
                _FullName = value;
            }
        }

        /// <MetaDataID>{01d2f1e0-5e56-4bde-82b6-d92468e60a58}</MetaDataID>
        string _Address;
        /// <MetaDataID>{929b37eb-b260-496f-ae3c-9bcbca8b9a31}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
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

        /// <MetaDataID>{028887bc-c73a-433c-9cdb-fa58744c7321}</MetaDataID>
        string _Password;
        /// <MetaDataID>{bfc5b50d-9b83-4b4f-8a02-4d6cccb906cf}</MetaDataID>
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







        /// <MetaDataID>{32de8669-c820-4d42-b1b6-513d2e268af6}</MetaDataID>
        string _ConfirmPassword;
        /// <MetaDataID>{80ba6caf-cdfd-4f0d-bfb3-2aeeaf3c83e7}</MetaDataID>
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

        /// <MetaDataID>{57b07489-fa2b-4fce-94a6-dbb348583514}</MetaDataID>
        string _UserName;
        /// <MetaDataID>{d9aa7cb2-c431-409e-8a4b-860e48be357b}</MetaDataID>
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;

            }
        }





        /// <MetaDataID>{207007a7-cf74-4413-b852-e0e780f36ada}</MetaDataID>
        string _OAuthUserIdentity;
        /// <MetaDataID>{06310bcd-8a04-480a-a42c-fdcd16e6be97}</MetaDataID>
        public string OAuthUserIdentity
        {
            get
            {
                return _OAuthUserIdentity;// ApplicationSettings.Current.SignInUserIdentity;
            }
            set
            {
                _OAuthUserIdentity = value;// ApplicationSettings.Current.SignInUserIdentity = value;
            }
        }

        /// <MetaDataID>{e034ad7c-99e5-4ef5-b1e6-9b920f95cf86}</MetaDataID>
        UserData UserData;
        /// <MetaDataID>{c025d0d0-aac4-40e2-b9cf-71dfac467296}</MetaDataID>
        AuthUser AuthUser;
        /// <MetaDataID>{a92261fd-5478-4e86-bd09-4c3ada5a47f2}</MetaDataID>
        public bool OnSignIn;

        //static string _AzureServerUrl = "http://localhost:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.5:8090/api/";

        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string _AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string _AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        /// <MetaDataID>{b066e79f-6707-428e-ad3e-f96d17ae45c6}</MetaDataID>
        static string _AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


        /// <MetaDataID>{23fa1561-877f-4ca5-966a-f2bf40742e9f}</MetaDataID>
        static string AzureServerUrl
        {
            get
            {
                string azureStorageUrl = null;// OOAdvantech.Remoting.RestApi.RemotingServices.GetLocalIPAddress();
                if (azureStorageUrl == null)
                    azureStorageUrl = _AzureServerUrl;
                else
                    azureStorageUrl = "http://" + azureStorageUrl + ":8090/api/";

                return azureStorageUrl;
            }
        }


        /// <MetaDataID>{f55963f0-34db-4fb9-b9ef-14d064de47ec}</MetaDataID>
        string _PhoneNumber;
        /// <MetaDataID>{809b179f-f0b6-4679-8896-53c71591a182}</MetaDataID>
        internal IWaiter Waiter;


        /// <MetaDataID>{75146911-f7c0-4e07-8ebb-da97efb23cb4}</MetaDataID>
        IList<IHallLayout> _Halls;
        /// <MetaDataID>{a7e5913c-4175-4744-a4a6-3ed768cbe1a5}</MetaDataID>
        public IList<IHallLayout> Halls
        {
            get
            {
                return _Halls;
                //if (this.InActiveShiftWork)
                //    return _Halls;
                //else
                //    return new List<IHallLayout>();
            }
        }

        /// <MetaDataID>{fac71003-9d13-430a-880e-956874e37449}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        [OOAdvantech.MetaDataRepository.CachingDataOnClientSide]
        public string PhoneNumber
        {
            get
            {
                return _PhoneNumber;
            }
            set
            {
                _PhoneNumber = value;
            }
        }

        /// <MetaDataID>{4d660847-77de-4c66-bf0c-fe41b5da3cdc}</MetaDataID>
        public bool IsActiveWaiter => Waiter != null;


        /// <MetaDataID>{f21f1462-a47e-4783-a5ca-270d265d27bf}</MetaDataID>
        Task<bool> SignInTask;

        /// <MetaDataID>{0b631e28-fc5c-46ab-85c1-944ce7ead3eb}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {
                authUser = DeviceAuthentication.AuthUser;
            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }
            if (authUser == null)
            {
#if DeviceDotNet
#if DEBUG
                if (Device.RuntimePlatform == Device.iOS)
                {
                    //iOS stuff
                    //return await IOSPseudoSignIn();
                }
#endif
#endif
                return false;
            }
            if (AuthUser != null && authUser.User_ID == AuthUser.User_ID)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    GetMessages();
                });

                ObjectChangeState?.Invoke(this, null);
                OAuthUserIdentity = Waiter.OAuthUserIdentity;
                return true;
            }

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


                    if (authUser != null && !string.IsNullOrWhiteSpace(ApplicationSettings.Current.WaiterObjectRef))
                    {
                        if (Waiter != null)
                        {
                            Waiter.ObjectChangeState -= Waiter_ObjectChangeState;
                            Waiter.MessageReceived -= MessageReceived;
                            Waiter.ServingBatchesChanged -= ServingBatchesChanged;
                            if (Waiter is ITransparentProxy)
                                (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
                        }

                        if (Waiter != null && Waiter.OAuthUserIdentity == authUser.User_ID)
                        {
                            AuthUser = authUser;
                            ActiveShiftWork = Waiter.ActiveShiftWork;
                            UpdateServingBatches(Waiter.GetServingBatches());
                            if (this._Halls != null)
                            {
                                foreach (var hall in this._Halls)
                                    (hall as RestaurantHallLayoutModel.HallLayout).ServiceArea.ServicePointChangeState -= ServiceArea_ServicePointChangeState;
                            }
                            this._Halls = Waiter.GetServiceHalls();
                            this._Halls = this._Halls.Where(x => x != null).ToList();
                            foreach (var hall in this._Halls)
                            {

                                hall.FontsLink = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";
                                (hall as RestaurantHallLayoutModel.HallLayout).SetShapesImagesRoot("https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/");
                                (hall as RestaurantHallLayoutModel.HallLayout).ServiceArea.ServicePointChangeState += ServiceArea_ServicePointChangeState;
                            }
                            this.FlavoursOrderServer.Halls = _Halls;

                            Waiter.ObjectChangeState += Waiter_ObjectChangeState;
                            Waiter.MessageReceived += MessageReceived;
                            Waiter.ServingBatchesChanged += ServingBatchesChanged;
                            if (Waiter is ITransparentProxy)
                                (Waiter as ITransparentProxy).Reconnected += WaiterPresentation_Reconnected;
#if DeviceDotNet
                            IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                            Waiter.DeviceFirebaseToken = device.FirebaseToken;
#endif
                            (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser = Waiter;
                            //ApplicationSettings.Current.FriendlyName = Waiter.FullName;
                            GetMessages();

                            OAuthUserIdentity = Waiter.OAuthUserIdentity;
                            return true;

                        }

                    }

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
                    catch (Exception error)
                    {
                        throw;
                    }


                    //sds.SendTimeout
                    //OOAdvantech.Remoting.RestApi.RemotingServices.T
                    authUser = DeviceAuthentication.AuthUser;
                    if (DeviceAuthentication.AuthUser == null)
                    {

                    }
                    if (authUser == null)
                    {

                    }

                    //New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname www.mywebsite.com
                    authUser = DeviceAuthentication.AuthUser;
                    UserData = pAuthFlavourBusiness.SignIn();
                    if (UserData != null)
                    {
                        _FullName = UserData.FullName;
                        _UserName = UserData.UserName;
                        _PhoneNumber = UserData.PhoneNumber;
                        _Address = UserData.Address;
                        _OAuthUserIdentity = UserData.OAuthUserIdentity;

                        foreach (var role in UserData.Roles.Where(x => x.RoleType == UserData.RoleType.Waiter))
                        {
                            if (role.RoleType == UserData.RoleType.Waiter)
                            {
                                if (Waiter != null)
                                {
                                    Waiter.ObjectChangeState -= Waiter_ObjectChangeState;
                                    Waiter.MessageReceived -= MessageReceived;
                                    Waiter.ServingBatchesChanged -= ServingBatchesChanged;
                                    if (Waiter is ITransparentProxy)
                                        (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
                                }
                                Waiter = RemotingServices.CastTransparentProxy<IWaiter>(role.User);
                                if (Waiter==null)
                                    continue;
                                string objectRef = RemotingServices.SerializeObjectRef(Waiter);
                                ApplicationSettings.Current.WaiterObjectRef = objectRef;
                                Waiter.ObjectChangeState += Waiter_ObjectChangeState;
                                Waiter.MessageReceived += MessageReceived;
                                Waiter.ServingBatchesChanged += ServingBatchesChanged;
                                if (Waiter is ITransparentProxy)
                                    (Waiter as ITransparentProxy).Reconnected += WaiterPresentation_Reconnected;

                                HallsServicePointsState = Waiter.HallsServicePointsState;
#if DeviceDotNet
                                IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                Waiter.DeviceFirebaseToken = device.FirebaseToken;

                                if (!device.IsBackgroundServiceStarted)
                                {
                                    BackgroundServiceState serviceState = new BackgroundServiceState();
                                    device.RunInBackground(new Action(async () =>
                                    {
                                        var message = Waiter.PeekMessage();
                                        Waiter.MessageReceived += Waiter_MessageReceived;
                                        do
                                        {
                                            System.Threading.Thread.Sleep(1000);

                                        } while (!serviceState.Terminate);

                                        Waiter.MessageReceived -= Waiter_MessageReceived;
                                        //if (Waiter is ITransparentProxy)
                                        //    (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
                                    }), serviceState);
                                }
#endif
                                ActiveShiftWork = Waiter.ActiveShiftWork;
                                UpdateServingBatches(Waiter.GetServingBatches());
                                (this.FlavoursOrderServer as FlavoursOrderServer).CurrentUser = Waiter;
                                //ApplicationSettings.Current.FriendlyName = Waiter.FullName;
                                if (this._Halls != null)
                                {
                                    foreach (var hall in this._Halls)
                                        (hall as RestaurantHallLayoutModel.HallLayout).ServiceArea.ServicePointChangeState -= ServiceArea_ServicePointChangeState;
                                }


                                this._Halls = Waiter.GetServiceHalls();
                                foreach (var hall in this._Halls)
                                {
                                    hall.FontsLink = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";
                                    (hall as RestaurantHallLayoutModel.HallLayout).SetShapesImagesRoot("https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/");
                                    (hall as RestaurantHallLayoutModel.HallLayout).ServiceArea.ServicePointChangeState += ServiceArea_ServicePointChangeState;

                                }
                                this.FlavoursOrderServer.Halls = _Halls;
                                GetMessages();
                            }
                        }
                        //https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/DiningTableChairs020.svg

                 
                        AuthUser = authUser;
                        if (Waiter!=null)
                        {
                            OAuthUserIdentity = Waiter.OAuthUserIdentity;
                            ObjectChangeState?.Invoke(this, null);
                        }
                        return true;
                    }
                    else
                        return false;


                }
                catch (Exception error)
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
            pAuthFlavourBusiness =RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
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


        public void CreateUserWithEmailAndPassword(string emailVerificationCode)
        {


            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {
                UserData userData = new UserData() { Email=Email, FullName=FullName, Address=Address, UserName=UserName };
                pAuthFlavourBusiness = GetFlavourBusinessAuth();
                pAuthFlavourBusiness.SignUpUserWithEmailAndPassword(Email, Password, userData, emailVerificationCode);
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

        /// <MetaDataID>{a2faf413-7b2b-4136-91c4-8ac9771b842d}</MetaDataID>
        private void ServingBatchesChanged()
        {
            var nn = Waiter.Name;
            if (Waiter != null)
            {
                if (ActiveShiftWork != null)
                    GetServingUpdates();
            }
        }

        /// <MetaDataID>{71eb9279-699c-45d1-a7d2-e767d2729291}</MetaDataID>
        private void GetServingUpdates()
        {
            List<ItemPreparationAbbreviation> servingItemsOnDevice = (from servingBatch in this.ServingBatches
                                                                      from itemsContext in servingBatch.AllContextsOfPreparedItems
                                                                      from itemPreparation in itemsContext.PreparationItems
                                                                      select new ItemPreparationAbbreviation() { uid = itemPreparation.uid, StateTimestamp = itemPreparation.StateTimestamp }).ToList();

            ServingBatchUpdates servingBatchUpdates = Waiter.GetServingUpdates(servingItemsOnDevice);

            var servingBatches = servingBatchUpdates.ServingBatches.Where(x => !x.IsAssigned).ToList();
            foreach (var servingBatch in servingBatches)
            {
                var servingBatchPresentation = _ServingBatches.GetViewModelFor(servingBatch, servingBatch, this);
                servingBatchPresentation.Update();
            }

            var asignedServingBatches = servingBatchUpdates.ServingBatches.Where(x => x.IsAssigned).ToList();

            foreach (var assignedServingBatch in asignedServingBatches)
            {
                var servingBatchPresentation = _AssignedServingBatches.GetViewModelFor(assignedServingBatch, assignedServingBatch, this);
                servingBatchPresentation.Update();
            }


            if (servingBatchUpdates.RemovedServingItems.Count > 0)
            {
                foreach (var servingBatch in ServingBatches.ToList())
                {
                    bool allItemsRemoved = true;
                    foreach (var servingItem in from servingItemContext in servingBatch.AllContextsOfPreparedItems
                                                from servingItem in servingItemContext.PreparationItems
                                                select servingItem)
                    {
                        if (servingBatchUpdates.RemovedServingItems.Where(x => x.uid == servingItem.uid).Count() == 0)
                        {
                            allItemsRemoved = false;
                            break;
                        }
                    }
                    if (allItemsRemoved)
                    {
                        _ServingBatches.Remove(servingBatch.ServingBatch);
                        servingBatch.Dispose();
                    }

                }
            }
            ObjectChangeState?.Invoke(this, "ServingBatches");
        }

        /// <MetaDataID>{940f1f9b-d176-4955-aed9-195adf50fc55}</MetaDataID>
        private void UpdateServingBatches(IList<IServingBatch> allServingBatches)
        {

            var servingBatches = allServingBatches.Where(x => !x.IsAssigned).ToList();
            foreach (var servingBatch in servingBatches)
                _ServingBatches.GetViewModelFor(servingBatch, servingBatch, this);

            var asignedServingBatches = allServingBatches.Where(x => x.IsAssigned).ToList();

            foreach (var assignedServingBatch in asignedServingBatches)
                _AssignedServingBatches.GetViewModelFor(assignedServingBatch, assignedServingBatch, this);


            foreach (var servingBatch in _ServingBatches.Keys.Where(x => !servingBatches.Contains(x)).ToList())
            {
                _ServingBatches[servingBatch].Dispose();
                _ServingBatches.Remove(servingBatch);
            }

            foreach (var assignedServingBatch in _AssignedServingBatches.Keys.Where(x => !asignedServingBatches.Contains(x)).ToList())
            {
                _AssignedServingBatches[assignedServingBatch].Dispose();
                _AssignedServingBatches.Remove(assignedServingBatch);
            }
        }

        /// <MetaDataID>{e87cdab3-0026-4ab4-8939-1f23c921e269}</MetaDataID>
        private void WaiterPresentation_Reconnected(object sender)
        {
            if (Waiter != null)
            {
                List<ItemPreparationAbbreviation> servingItemsOnDevice = (from servingBatch in this.ServingBatches
                                                                          from itemsContext in servingBatch.AllContextsOfPreparedItems
                                                                          from itemPreparation in itemsContext.PreparationItems
                                                                          select new ItemPreparationAbbreviation() { uid = itemPreparation.uid, StateTimestamp = itemPreparation.StateTimestamp }).ToList();
                if (ActiveShiftWork != null)
                {
                    ServingBatchUpdates servingBatchUpdates = Waiter.GetServingUpdates(servingItemsOnDevice);
                }
            }
        }

        /// <MetaDataID>{bb7ffd5e-7adb-466d-a1b8-217bd785ee91}</MetaDataID>
        private void Waiter_MessageReceived(IMessageConsumer sender)
        {


        }

        /// <MetaDataID>{b4d7c3bf-4a36-4111-aa8d-b33709d3f35e}</MetaDataID>
        private void ServiceArea_ServicePointChangeState(object _object, IServicePoint servicePoint, ServicePointState newState)
        {

            this.ServicePointChangeState?.Invoke(this, servicePoint.ServicesPointIdentity, newState);
            HallsServicePointsState[servicePoint.ServicesPointIdentity] = newState;
            foreach (var hall in Halls.OfType<RestaurantHallLayoutModel.HallLayout>())
            {
                foreach (var shape in hall.Shapes)
                {
                    if (!string.IsNullOrWhiteSpace(shape.ServicesPointIdentity) && HallsServicePointsState.ContainsKey(shape.ServicesPointIdentity))
                        shape.ServicesPointState = HallsServicePointsState[shape.ServicesPointIdentity];
                }
            }
            if (this.FlavoursOrderServer != null)
                this.FlavoursOrderServer.UpdateHallsServicePointStates(HallsServicePointsState);

            ObjectChangeState?.Invoke(this, nameof(HallsServicePointsState));


        }


        /// <MetaDataID>{527b5bdb-7653-40f3-8370-e619aa9d216a}</MetaDataID>
        private void MessageReceived(IMessageConsumer sender)
        {
#if DeviceDotNet
            try
            {
                IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(IDeviceOOAdvantechCore)) as IDeviceOOAdvantechCore;
                IRingtoneService ringtoneService = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(IRingtoneService)) as IRingtoneService;
                var isInSleepMode = device.IsinSleepMode;

                Task.Run(() =>
                {

                    ringtoneService.Play();

                    int count = 4;
                    if (!isInSleepMode)
                        count = 1;

                    var duration = TimeSpan.FromSeconds(2);
                    while (count > 0)
                    {
                        count--;
                        Vibration.Vibrate(duration);
                        System.Threading.Thread.Sleep(3000);
                        Vibration.Cancel();
                        System.Threading.Thread.Sleep(3000);
                    }
                    ringtoneService.Stop();

                });
            }
            catch (Exception error)
            {
            }
#endif
            try
            {
                GetMessages();
            }
            catch (Exception error)
            {
            }
        }
        public Dictionary<string, ServicePointState> HallsServicePointsState { get; set; }
        /// <MetaDataID>{d3502416-55ad-482e-875b-f19d27f520a4}</MetaDataID>
        private void Waiter_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IServicesContextWorker.ActiveShiftWork))
            {
                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWorkStartedAt));

                GetMessages();
            }

            //if (member == nameof(IWaiter.HallsServicePointsState))
            //{
            //    HallsServicePointsState = this.Waiter.HallsServicePointsState;

            //    foreach (var hall in Halls.OfType<RestaurantHallLayoutModel.HallLayout>())
            //    {
            //        foreach (var shape in hall.Shapes)
            //        {
            //            if (!string.IsNullOrWhiteSpace(shape.ServicesPointIdentity) && HallsServicePointsState.ContainsKey(shape.ServicesPointIdentity))
            //                shape.ServicesPointState = HallsServicePointsState[shape.ServicesPointIdentity];
            //        }
            //    }
            //    if (this.FlavoursOrderServer != null)
            //        this.FlavoursOrderServer.UpdateHallsServicePointStates(HallsServicePointsState);

            //    ObjectChangeState?.Invoke(this, nameof(HallsServicePointsState));
            //}
        }

        /// <MetaDataID>{d38d4827-a9a7-48bb-b272-1f897c86cf1b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public void SignOut()
        {
            UserData = new UserData();
            AuthUser = null;
            if (Waiter != null)
            {
                Waiter.ObjectChangeState -= Waiter_ObjectChangeState;
                Waiter.MessageReceived -= MessageReceived;
                Waiter.ServingBatchesChanged -= ServingBatchesChanged;
                if (Waiter is ITransparentProxy)
                    (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
            }

            Waiter = null;
            this._Halls = null;
            ActiveShiftWork = null;
            //Organization = null;
            //ServiceContextSupervisor = null;
            //_ServicesContexts.Clear();
        }


        /// <MetaDataID>{8bb16524-ef68-4a38-94f4-71776a04d4d5}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignUp()
        {
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
            {
                authUser = DeviceAuthentication.AuthUser;
            }
            if (DeviceAuthentication.AuthUser == null)
            {

            }
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
                        pAuthFlavourBusiness = remoteObject as IAuthFlavourBusiness;

                    }
                    catch (System.Net.WebException error)
                    {
                        throw;
                    }
                    catch (Exception error)
                    {
                        throw;
                    }

                    if (authUser == null)
                    {

                    }
                    UserData = new UserData() { Email = this.Email, FullName = this.FullName, PhoneNumber = this.PhoneNumber, Address = this.Address };
                    UserData = pAuthFlavourBusiness.SignUp(UserData);

                    if (UserData != null)
                    {
                        FullName = UserData.FullName;
                        _UserName = UserData.UserName;
                        _PhoneNumber = UserData.PhoneNumber;
                        _Address = UserData.Address;
                        _OAuthUserIdentity = UserData.OAuthUserIdentity;
                        AuthUser=authUser;
                        ObjectChangeState?.Invoke(this, null);



                        return true;
                    }
                    else
                        return false;

                }
                catch (Exception error)
                {

                    throw;
                }
                finally
                {
                    OnSignIn = false;
                }
            });
        }

        public void SaveUserProfile()
        {
            Task<bool>.Run(() =>
            {
                //string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                //string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
                //AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
                //string serverUrl = "http://localhost/FlavourBusinessWebApiRole/api/";
                //serverUrl = "http://localhost:8090/api/";
                //serverUrl = AzureServerUrl;
                IAuthFlavourBusiness pAuthFlavourBusiness = GetFlavourBusinessAuth();// OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData) as IAuthFlavourBusiness;
                UserData = new UserData() { Email = this.Email, FullName = this.FullName, PhoneNumber = this.PhoneNumber, Address = this.Address };
                pAuthFlavourBusiness.UpdateUserProfile(UserData, UserData.RoleType.Waiter);
            });

        }

        /// <MetaDataID>{187d772f-a845-4262-9f4a-1efed97515fe}</MetaDataID>
        public async Task<bool> AssignWaiter()
        {
            return await Assign();
        }
#if DeviceDotNet
        public ScanCode ScanCode = new ScanCode();
#endif
        /// <MetaDataID>{9921c079-439e-4f42-8b4d-44130647d4b1}</MetaDataID>
        public async Task<bool> Assign()
        {
#if DeviceDotNet
            string waiterAssignKey = null;
            try
            {
                var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically");

                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return false;
                waiterAssignKey = result.Text;
            }
            catch (Exception error)
            {
                return false;
            }

            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;

            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;
            try
            {
                IFlavoursServicesContextManagment servicesContextManagment = RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                this.Waiter = servicesContextManagment.AssignWaiterUser(waiterAssignKey);
                type = "FlavourBusinessManager.AuthFlavourBusiness";
                var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
                var pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
                UserData = pAuthFlavourBusiness.SignIn();
                return true;
            }
            catch (Exception error)
            {
                return false;
            }



            //lock (this)
            //{
            //    if (OnScan && ConnectToServicePointTask != null)
            //        return ConnectToServicePointTask.Task;

            //    OnScan = true;
            //    ConnectToServicePointTask = new TaskCompletionSource<bool>();
            //}
            //Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await (App.Current.MainPage as NavigationPage).CurrentPage.Navigation.PushAsync(ScanPage);
            //});


            //return ConnectToServicePointTask.Task;
#else
            return true;
#endif

        }


        /// <MetaDataID>{3a868f16-c77b-4316-b6c0-c1a2bc4cf6b8}</MetaDataID>
        public DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; private set; }


        /// <MetaDataID>{12e40d3a-0505-40a7-ac75-e728e24dd157}</MetaDataID>
        IShiftWork ActiveShiftWork;

        /// <MetaDataID>{4d1af4d2-ecbb-4ef9-a0ee-8a43a3a7d137}</MetaDataID>
        ViewModelWrappers<IServingBatch, ServingBatchPresentation> _ServingBatches = new ViewModelWrappers<IServingBatch, ServingBatchPresentation>();


        /// <MetaDataID>{dd252805-6fb8-4285-9393-644a55911ee3}</MetaDataID>
        public List<ServingBatchPresentation> ServingBatches => _ServingBatches.Values.OrderBy(x => x.ServingBatch.SortID).ToList();

        /// <MetaDataID>{d4df3e7e-34d8-4d28-8cc1-98d2217c9c4b}</MetaDataID>
        ViewModelWrappers<IServingBatch, ServingBatchPresentation> _AssignedServingBatches = new ViewModelWrappers<IServingBatch, ServingBatchPresentation>();

        /// <MetaDataID>{83a88d0b-ec9d-4025-8727-410a3e52b700}</MetaDataID>
        public List<ServingBatchPresentation> AssignedServingBatches => _AssignedServingBatches.Values.OrderBy(x => x.ServingBatch.SortID).ToList();

        /// <MetaDataID>{9f501ecd-d882-47f3-b7ca-55e37b4ecf36}</MetaDataID>
        public bool AssignServingBatch(string serviceBatchIdentity)
        {
            var servingBatch = ServingBatches.Where(x => x.ServiceBatchIdentity == serviceBatchIdentity).FirstOrDefault();

            if (servingBatch != null)
            {

                _AssignedServingBatches[servingBatch.ServingBatch] = _ServingBatches[servingBatch.ServingBatch];
                _ServingBatches.Remove(servingBatch.ServingBatch);


                SerializeTaskScheduler.AddTask(async () =>
                {
                    int tries = 30;
                    while (tries > 0)
                    {
                        try
                        {
                            this.Waiter.AssignServingBatch(servingBatch.ServingBatch);
                            return true;
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
                return true;
            }

            return false;

        }
        public void PrintServingBatchReceipt(string serviceBatchIdentity)
        {
            var servingBatch = AssignedServingBatches.Where(x => x.ServiceBatchIdentity == serviceBatchIdentity).FirstOrDefault();

            if (servingBatch != null)
            {
                servingBatch.ServingBatch.PrintReceiptAgain();
            }
        }

        /// <MetaDataID>{8675f074-c752-4808-a9bb-1d1feb84db6d}</MetaDataID>
        public bool CommitServingBatches()
        {
            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        Waiter.CommitServingBatches();
                        return true;
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
            return true;
        }
        /// <MetaDataID>{76ef59a3-cbfb-4908-84c9-4578f906f5d8}</MetaDataID>
        public bool DeassignServingBatch(string serviceBatchIdentity)
        {
            var servingBatch = AssignedServingBatches.Where(x => x.ServiceBatchIdentity == serviceBatchIdentity).FirstOrDefault();
            if (servingBatch != null)
            {

                _ServingBatches[servingBatch.ServingBatch] = _AssignedServingBatches[servingBatch.ServingBatch];
                _AssignedServingBatches.Remove(servingBatch.ServingBatch);



                SerializeTaskScheduler.AddTask(async () =>
                {
                    int tries = 30;
                    while (tries > 0)
                    {
                        try
                        {
                            this.Waiter.DeassignServingBatch(servingBatch.ServingBatch);
                            return true;
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
                return true;
            }

            return false;
        }

        /// <MetaDataID>{e88857c6-f7c0-4805-933b-ad8e3a4a55e6}</MetaDataID>
        public bool InActiveShiftWork
        {
            get
            {
                if (ActiveShiftWork != null)
                {
                    var startedAt = ActiveShiftWork.StartsAt;
                    var workingHours = ActiveShiftWork.PeriodInHours;

                    var billingPayments = (ActiveShiftWork as IDebtCollection)?.BillingPayments;

                    var hour = System.DateTime.UtcNow.Hour + (((double)System.DateTime.UtcNow.Minute) / 60);
                    hour = Math.Round((hour * 2)) / 2;
                    var utcNow = DateTime.UtcNow.Date + TimeSpan.FromHours(hour);
                    if (utcNow >= startedAt.ToUniversalTime() && utcNow <= startedAt.ToUniversalTime() + TimeSpan.FromHours(workingHours))
                    {
                        return true;
                    }
                    else
                    {
                        ActiveShiftWork = null;
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        /// <MetaDataID>{6f36b650-3b5e-44e3-85fa-42b869925240}</MetaDataID>
        public DateTime ActiveShiftWorkStartedAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ActiveShiftWork.StartsAt;
                else
                    return DateTime.MinValue;
            }
        }

        /// <MetaDataID>{993f9cef-4758-4957-92bc-a524fe03c3b8}</MetaDataID>
        public DateTime ActiveShiftWorkEndsAt
        {
            get
            {
                if (InActiveShiftWork)
                    return ActiveShiftWork.StartsAt + TimeSpan.FromHours(ActiveShiftWork.PeriodInHours);
                else
                    return DateTime.MinValue;
            }
        }

        /// <MetaDataID>{4adb01c1-c93a-47dc-ad82-c5f8e696aab5}</MetaDataID>
        public string AppIdentity => "com.microneme.dontwaitwaiterapp";

        /// <MetaDataID>{66534587-dc7c-4f65-8b94-4e14471f0437}</MetaDataID>
        public void SiftWorkStart(DateTime startedAt, double timespanInHours)
        {
            ActiveShiftWork = Waiter.NewShiftWork(startedAt, timespanInHours);

            UpdateServingBatches(Waiter.GetServingBatches());


            GetMessages();
        }

        /// <MetaDataID>{36574b28-ba8d-4c4a-bc63-873836f9d4eb}</MetaDataID>
        object MessagesLock = new object();
        /// <MetaDataID>{7764a3ac-8238-42dd-81b2-bb9bf1832cae}</MetaDataID>
        private void GetMessages()
        {
            lock (MessagesLock)
            {
                if (Waiter != null)
                {
                    var message = Waiter.PeekMessage();
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.LaytheTable)
                    {
                        var tmp = (DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes;
                        if ((DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes > 40)
                            Waiter.RemoveMessage(message.MessageID);
                        else
                        {
                            if (message != null && InActiveShiftWork)
                            {

                                string servicesPointIdentity = message.GetDataValue<string>("ServicesPointIdentity");
                                if (HallsServicePointsState[servicesPointIdentity] == ServicePointState.Laying)
                                    LayTheTableRequest?.Invoke(this, message.MessageID, servicesPointIdentity);
                                else
                                    Waiter.RemoveMessage(message.MessageID);
                                //PartOfMealRequestMessageForward(message);

                                return;
                            }
                        }
                    }
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.ItemsReadyToServe)
                    {
                        if ((DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes > 20)
                            Waiter.RemoveMessage(message.MessageID);
                        else
                        {
                            if (message != null && InActiveShiftWork)
                            {
                                GetServingUpdates();

                                string servicesPointIdentity = message.GetDataValue<string>("ServicesPointIdentity");
                                ItemsReadyToServeRequest?.Invoke(this, message.MessageID, servicesPointIdentity);
                                //PartOfMealRequestMessageForward(message);
                                return;
                            }
                        }
                    }
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MealConversationTimeout)
                    {
                        if ((DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes > 20)
                            Waiter.RemoveMessage(message.MessageID);
                        else
                        {
                            if (message != null && InActiveShiftWork)
                            {


                                string servicesPointIdentity = message.GetDataValue<string>("ServicesPointIdentity");
                                string sessionIdentity = message.GetDataValue<string>("SessionIdentity");
                                MealConversationTimeout?.Invoke(this, message.MessageID, servicesPointIdentity, sessionIdentity);
                                //PartOfMealRequestMessageForward(message);
                                return;
                            }
                        }
                    }
                }
            }
        }


        private async Task GenerateLocalNotificationAsync()
        {

            //List<Location> _locList = JsonHandler.getLocationsFromReports(data);

            //for (int i = 0; i < _locList.Count; i++)
            //{
            //    double Distance = LocationExtensions.CalculateDistance(_locList[i], _location, DistanceUnits.Kilometers);
            //    if (Distance < 5)
            //    {
            //        try
            //        {
            //            await ReadTripFile("LocalNotificationTime.txt");
            //            if (FileData != "")
            //            {
            //                var Data = FileData.Split('!');
            //                var loc = Data[1].Split('*');
            //                double latitude = Convert.ToDouble(loc[0]);
            //                double longitude = Convert.ToDouble(loc[1]);

            //                DateTime t1 = DateTime.ParseExact(Data[0], "yyyy|MM|dd|hh:mm:ss", CultureInfo.InvariantCulture);


            //                string dt = DateTime.Now.ToString("yyyy|MM|dd|hh:mm:ss");

            //                DateTime t2 = DateTime.ParseExact(dt, "yyyy|MM|dd|hh:mm:ss", CultureInfo.InvariantCulture);

            //                TimeSpan ts = t2.TimeOfDay - t1.TimeOfDay;
            //                double Dis = LocationExtensions.CalculateDistance(new Location(latitude, longitude), _location, DistanceUnits.Kilometers);
            //                if (ts.TotalHours > 24 || (Dis > 20))
            //                {
            //                    await BuildTripFile(_location);
            //                    //Generate Local Notification

            //                    JsonHandler.ShowNotification(StringResource.LocalNotificationTitle, StringResource.LocalNotificationDec);

            //                    return;


            //                }
            //            }
            //            else
            //            {
            //                JsonHandler.ShowNotification(StringResource.LocalNotificationTitle, StringResource.LocalNotificationDec);
            //                await BuildTripFile(_location);
            //                return;
            //            }
            //        }
            //        catch (Exception ex)

            //        {
            //        }

            //    }
            //}
        }

        public void ItemsReadyToServeMessageReceived(string messageID)
        {
            Waiter.RemoveMessage(messageID);
        }

        public void MealConversationTimeoutReceived(string messageID)
        {
            Waiter.RemoveMessage(messageID);
        }

        public void TableIsLay(string servicesPointIdentity)
        {
            Waiter.TableIsLay(servicesPointIdentity);
        }
        public void LayTheTableMessageReceived(string messageID)
        {
            Waiter.RemoveMessage(messageID);
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
                        //if (this.FoodServiceClientSession != null)
                        //    this.FoodServiceClientSession.DeviceResume();
                        GetMessages();
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
                        //if (this.FoodServiceClientSession != null)
                        //    this.FoodServiceClientSession.DeviceSleep();
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



        /// <MetaDataID>{c4aab28c-46f3-45ae-9b95-fa4cd6fd1577}</MetaDataID>
        public void ExtendSiftWorkStart(double timespanInHours)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{269fc5a7-5b40-4aac-80a2-b55d55e41dc4}</MetaDataID>
        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            if (uri == "./ServicePointSupervisor")
                return this;
            return FlavoursOrderServer as MarshalByRefObject;
        }
        /// <MetaDataID>{07acf9b5-7184-48c8-ae68-99b9073851ca}</MetaDataID>
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


        /// <MetaDataID>{b9aa7b69-ee7c-4c79-b9c9-f44bbfcfcff9}</MetaDataID>
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

        /// <MetaDataID>{78e3b85a-cefd-4a92-aef3-66ad79121d13}</MetaDataID>
        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();

        /// <MetaDataID>{daa58ef0-acd5-443f-8fde-ebd20b3d3ec0}</MetaDataID>
        public string GetTranslation(string langCountry)
        {
            if (Translations.ContainsKey(langCountry))
                return Translations[langCountry].ToString();
            string json = "{}";
            var assembly = Assembly.GetExecutingAssembly();

#if DeviceDotNet
            string path = "WaiterApp.i18n";
#else
            string path = "WaiterApp.WPF.i18n";
#endif

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



        //public async void TransferSession(string targetServicePointIdentity)
        //{


        //    string targetFullServicePointIdentity = (from hall in Halls.OfType<HallLayout>()
        //                                             from shape in hall.Shapes
        //                                             where shape.ServicesPointIdentity == targetServicePointIdentity
        //                                             select hall).FirstOrDefault().ServicesContextIdentity + ";" + targetServicePointIdentity;

        //    this.Waiter.TransferSession(this.FlavoursOrderServer.MainSession, targetServicePointIdentity);

        //    await this.FlavoursOrderServer.GetServicePointDataEx(this.FlavoursOrderServer.MenuData.FoodServiceClientSessionUri);


        //}

        //public async void TransferSession(string sourceServicePointIdentity, string targetServicePointIdentity)
        //{
        //    if(this.FlavoursOrderServer.MenuData.ServicePointIdentity== sourceServicePointIdentity)
        //    {
        //        string targetFullServicePointIdentity = (from hall in Halls.OfType<HallLayout>()
        //                                                 from shape in hall.Shapes
        //                                                 where shape.ServicesPointIdentity == targetServicePointIdentity
        //                                                 select hall).FirstOrDefault().ServicesContextIdentity + ";" + targetServicePointIdentity;

        //        this.Waiter.TransferSession(this.FlavoursOrderServer.MainSession, targetServicePointIdentity);
        //        await this.FlavoursOrderServer.GetServicePointDataEx(this.FlavoursOrderServer.MenuData.FoodServiceClientSessionUri);

        //        //await this.FlavoursOrderServer.GetServicePointData(targetFullServicePointIdentity);
        //    }
        //}

        public bool TransferPartialSession(string partialSessionID, string targetSessionID)
        {
            if (targetSessionID == "WaiterSession")
            {
                if (string.IsNullOrWhiteSpace(FlavoursOrderServer.CurrentFoodServicesClientSession.MenuData.MainSessionID))
                {
                    var messmate = FlavoursOrderServer.CurrentFoodServicesClientSession.GetCandidateMessmates().Where(x => x.ClientSessionID == partialSessionID).FirstOrDefault();
                    if (messmate == null)
                        messmate = FlavoursOrderServer.CurrentFoodServicesClientSession.GetMessmates().Where(x => x.ClientSessionID == partialSessionID).FirstOrDefault();
                    FlavoursOrderServer.AcceptInvitation(messmate, null);
                    return true;
                }
                else
                {
                    Waiter.TransferPartialSession(partialSessionID, this.FlavoursOrderServer.CurrentFoodServicesClientSession.MenuData.MainSessionID);
                    return true;
                }
            }
            else if (!string.IsNullOrWhiteSpace(targetSessionID))
            {
                Waiter.TransferPartialSession(partialSessionID, targetSessionID);
                return true;
            }
            return false;




        }

        public void TransferItems(List<SessionItemPreparationAbbreviation> itemPreparations, string targetServicePointIdentity)
        {
            this.Waiter.TransferItems(itemPreparations, targetServicePointIdentity);
        }

        public IBill GetBill(List<SessionItemPreparationAbbreviation> itemPreparations, IFoodServicesClientSessionViewModel foodServicesClientSessionPresentation)
        {
            return this.Waiter.GetBill(itemPreparations, (foodServicesClientSessionPresentation as FoodServicesClientSessionViewModel).FoodServicesClientSession);
        }

    }
}
