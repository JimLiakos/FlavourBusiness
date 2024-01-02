
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.ViewModel;

using OOAdvantech;
using OOAdvantech.Json.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System;
using FlavourBusinessFacade.HumanResources;

using FlavourBusinessFacade.EndUsers;
using Xamarin.Forms;

using FlavourBusinessFacade.RoomService;
using DontWaitApp;
using FlavourBusinessFacade.HomeDelivery;


#if DeviceDotNet

using Acr.UserDialogs;
using Xamarin.Essentials;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using FlavourBusinessManager.HumanResources;
using FlavourBusinessManager.ServicesContextResources;
using MarshalByRefObject = System.MarshalByRefObject;
#endif


namespace TakeAwayApp.ViewModel
{
    /// <MetaDataID>{1dca0d48-ef5c-41ac-a964-127385b2e256}</MetaDataID>
    [HttpVisible]
    public interface IFlavoursServiceOrderTakingStation
    {
        [Association("", Roles.RoleA, "0c702768-e0d2-45f5-8473-677b5cd739fb")]
        [RoleBMultiplicityRange(1, 1)]
        List<IHomeDeliverySession> HomeDeliverySessions { get; }

        /// <MetaDataID>{16541ed5-9f09-45e2-a16e-cb1e661a7a15}</MetaDataID>
        Task<bool> AssignDeliveryCallCenterCredentialKey(string credentialKey);

        /// <MetaDataID>{961e59f9-4c92-47af-a975-4fa28c8eb917}</MetaDataID>
        Task<bool> AssignDeliveryCallCenterStation(bool useFrontCameraIfAvailable);

        /// <MetaDataID>{9caf87cf-a117-4483-aba5-584de37e1337}</MetaDataID>
        string DeliveryCallCenterCredentialKey { get; set; }

        /// <MetaDataID>{d665ceb1-47b3-4a6a-9671-c12e4bf7737d}</MetaDataID>
        DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; }
        /// <MetaDataID>{0cc4170b-f2a9-47e0-a93c-813f6ec8abd1}</MetaDataID>
        string TakeAwayStationCredentialKey { get; set; }
        /// <MetaDataID>{0a4ccfb0-2a60-41cc-8cce-2577a15b84ea}</MetaDataID>
        Task<bool> AssignTakeAwayStationCredentialKey(string credentialKey);
        /// <MetaDataID>{0d0668da-c0ce-49ed-9a74-c6a574409a1c}</MetaDataID>
        Task<bool> AssignTakeAwayStation(bool useFrontCameraIfAvailable);


        /// <MetaDataID>{30510e04-e498-47df-8334-83573a2d20c0}</MetaDataID>
        Task<bool> IsTakeAwayStationActive { get; }

        /// <MetaDataID>{f3e59a9a-7d80-4113-b0a7-fddc88db81d1}</MetaDataID>
        Task<bool> IsDeliveryCallCenterStationActive { get; }

        /// <MetaDataID>{7be464a8-5863-4e81-8e0a-1d1f3e3e5aef}</MetaDataID>
        List<WatchingOrderPresentation> WatchingOrders { get; }



        //IPlace HomeDeliveryPlaceOfDistribution { get; }

        /// <summary>
        /// Check if application is granted to access infrastructure for QR code scanning 
        /// </summary>
        /// <returns>
        /// for granted  return true
        /// else return false
        /// </returns>
        /// <MetaDataID>{f5ade777-4b64-49ed-8a59-248d2140ec49}</MetaDataID>
        Task<bool> CheckPermissionsForQRCodeScan();

        /// <summary>
        /// Request Permission to access infrastructure for QR code scanning 
        /// </summary>
        /// <returns>
        /// for granted  return true
        /// else return false
        /// </returns>
        /// <MetaDataID>{ba8d35eb-61c2-4e41-9464-66a58f9e7e7b}</MetaDataID>
        Task<bool> RequestPermissionsForQRCodeScan();


        /// <MetaDataID>{6dfec609-25e3-40e1-b5ae-8e8ca1d74993}</MetaDataID>
        bool InActiveShiftWork { get; }

        /// <MetaDataID>{872cf9a1-885b-48ee-b71c-19abceda805f}</MetaDataID>
        System.DateTime ActiveShiftWorkStartedAt { get; }

        /// <MetaDataID>{3c03cff2-dd6b-43e3-8436-12093d1a91f0}</MetaDataID>
        System.DateTime ActiveShiftWorkEndsAt { get; }



        //period
        /// <MetaDataID>{f10b2dc3-edf7-449e-b7da-b11abce01e4d}</MetaDataID>
        void ShiftWorkStart(DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{ef901fe7-db80-466c-9826-742e38f6623f}</MetaDataID>
        void TakeAwayOrderCommitted();

        [GenerateEventConsumerProxy]
        event ObjectChangeStateHandle ObjectChangeState;



        /// <MetaDataID>{67460b8d-e957-43c3-b964-e03155b16fda}</MetaDataID>
        Task<IHomeDeliverySession> NewHomeDeliverySession();



        /// <MetaDataID>{fa6aaa89-c9c7-469b-934c-4b690d736f49}</MetaDataID>
        Task<IHomeDeliverySession> GetHomeDeliverSession(string sessionID);

        /// <MetaDataID>{06fd4791-60ad-4ab6-9788-d28a2fc13e29}</MetaDataID>
        void CancelHomeDeliverySession(IHomeDeliverySession homeDeliverySession);




        /// <MetaDataID>{71c41ce6-7b00-4091-ad5a-c9e454272c2a}</MetaDataID>
        IFoodServicesClientSessionViewModel TakeAwaySession { get; }


        List<HomeDeliveryServicePointAbbreviation> HomeDeliveryServicePoints { get; }




    }







    /// <MetaDataID>{efe40e2f-68a3-4ee7-afde-5cf1ffd4c62e}</MetaDataID>
    public class FlavoursServiceOrderTakingStation : MarshalByRefObject, IFlavoursServiceOrderTakingStation, OOAdvantech.Remoting.IExtMarshalByRefObject, ILocalization, ISecureUser, IBoundObject
    {

        /// <MetaDataID>{67d25e6d-5d8c-498a-bced-8522e4e9ac08}</MetaDataID>
        public FlavoursServiceOrderTakingStation()
        {

            VivaWalletPos.IPos sds = null;
            var vivaWalletPos = Xamarin.Forms.DependencyService.Get<VivaWalletPos.IPos>();

            FlavoursOrderServer = new FlavoursOrderServer(AppType.OrderTaking, vivaWalletPos != null) { EndUser = this };
            var appSettings = ApplicationSettings.Current;

            if (vivaWalletPos != null)
                vivaWalletPos.Confing(VivaWalletPos.POSType.TerminalPos, "127.0.0.1", 6000, 120);
#if DeviceDotNet



#endif

            _MealCoursesInProgress.OnNewViewModelWrapper+=MealCoursesInProgress_OnNewViewModelWrapper;

            //string channelUri = string.Format("{0}({1})", AzureServerUrl, "0470e076603e47b6a82556fe4c1bf335");
            // TakeawayCashier=OOAdvantech.Remoting.RestApi.RemotingServices.GetPersistentObject(channelUri, "3bdea2dc-3185-4331-bdb9-f17c535f2965\\49\\8413280b-a2d0-43d1-8194-59aaa001de3d") as FlavourBusinessFacade.HumanResources.ITakeawayCashier;


            //[{ "TypeFullName":"FlavourBusinessManager.HumanResources.TakeawayCashier","ObjectUri":"3bdea2dc-3185-4331-bdb9-f17c535f2965\\49\\8413280b-a2d0-43d1-8194-59aaa001de3d","ComputingContextID":"0470e076603e47b6a82556fe4c1bf335"}]
            //var dd = DeviceDisplay.MainDisplayInfo;

            //var ewr = Xamarin.Essentials.DeviceInfo.Platform;
            //var ss = Xamarin.Forms.Device.Idiom;
        }

        /// <MetaDataID>{a1a793ed-5fb7-44e6-9200-9e64bce46ac4}</MetaDataID>
        private void MealCoursesInProgress_OnNewViewModelWrapper(UIBaseEx.ViewModelWrappers<IMealCourse, FlavourBusinessManager.RoomService.ViewModel.MealCourse> sender, IMealCourse key, FlavourBusinessManager.RoomService.ViewModel.MealCourse value)
        {
            value.MealCourseUpdated += OnMealCourseUpdated;
        }

        /// <MetaDataID>{0b41ad20-2fa2-4b05-b03e-82c566120f96}</MetaDataID>
        private void OnMealCourseUpdated(FlavourBusinessManager.RoomService.ViewModel.MealCourse mealCourse)
        {
            ObjectChangeState?.Invoke(this, nameof(WatchingOrders));
        }

        //FlavourBusinessFacade.HumanResources.ITakeawayCashier TakeawayCashier;

        /// <MetaDataID>{f39575a9-0c7a-4e09-80e8-415fffdee64f}</MetaDataID>
        string lan = "el";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
        /// <MetaDataID>{362081c4-4989-4625-bccf-44153a06cd57}</MetaDataID>
        public string Language { get { return lan; } }

        /// <MetaDataID>{710eccb8-b8dc-4400-9424-554c418d57ba}</MetaDataID>
        string deflan = "en";
        /// <MetaDataID>{240e0b50-29ac-44e0-8e35-3562144e0304}</MetaDataID>
        public string DefaultLanguage { get { return deflan; } }

        /// <MetaDataID>{e3131f8c-278c-4224-b20d-2ed5e3b0795a}</MetaDataID>
        public string AppIdentity => "com.microneme.takeawaystationapp";

        /// <MetaDataID>{81096eb8-b5e2-4ea2-81ac-67fdc2d29693}</MetaDataID>
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




        /// <MetaDataID>{de6cdc5e-fc2a-41a1-bcce-4bc34047e827}</MetaDataID>
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

        /// <MetaDataID>{4a4a6e54-ab17-4cb1-aa6f-50194ebad799}</MetaDataID>
        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{91dd53a2-af8c-4454-819e-905c386c4647}</MetaDataID>
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

        /// <MetaDataID>{ed01239e-f11b-4c45-a163-a5111c93d476}</MetaDataID>
        public void SignOut()
        {
            UserData = new UserData();
            AuthUser = null;
            if (TakeAwayCashier != null)
            {
                TakeAwayCashier.ObjectChangeState -= TakeAwayCashier_ObjectChangeState;
                TakeAwayCashier.MessageReceived -= MessageReceived;
                //TakeAwayCashier.ServingBatchesChanged -= ServingBatchesChanged;
                if (TakeAwayCashier is ITransparentProxy)
                    (TakeAwayCashier as ITransparentProxy).Reconnected -= TakeAwayCashierPresentation_Reconnected;
            }

            TakeAwayCashier = null;

            ActiveShiftWork = null;
        }

        /// <MetaDataID>{3f9b769c-7a99-4487-a68e-6f9d077ea4be}</MetaDataID>
        public Task<bool> SignUp()
        {
            throw new System.NotImplementedException();
        }
        /// <MetaDataID>{8e848725-332e-4583-9aa4-6a5b5c35842e}</MetaDataID>
        AuthUser AuthUser;

        /// <MetaDataID>{773b4ca5-ca98-4c0c-9823-f4530a5cc86c}</MetaDataID>
        ITakeawayCashier TakeAwayCashier;
        /// <MetaDataID>{be1ed9fc-1aae-47b3-a923-dbfca2217bb8}</MetaDataID>
        IShiftWork ActiveShiftWork;

        /// <MetaDataID>{d7005bd3-dec7-4101-a42c-8ad172b92829}</MetaDataID>
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public async Task<bool> SignIn()
        {

            //System.IO.File.AppendAllLines(App.storage_path, new string[] { "SignIn " });
            System.Diagnostics.Debug.WriteLine("public async Task< bool> SignIn()");
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser == null)
                authUser = DeviceAuthentication.AuthUser;
            if (DeviceAuthentication.AuthUser == null)
            {
            }
            if (authUser == null)
                return false;

            if (AuthUser != null && authUser.User_ID == AuthUser.User_ID)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    GetMessages();
                });

                ObjectChangeState?.Invoke(this, null);
                if (TakeAwayCashier != null)
                    OAuthUserIdentity = TakeAwayCashier.OAuthUserIdentity;
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
                        if (authUser != null && !string.IsNullOrWhiteSpace(ApplicationSettings.Current.WaiterObjectRef))
                        {
                            if (TakeAwayCashier != null)
                            {
                                TakeAwayCashier.ObjectChangeState -= TakeAwayCashier_ObjectChangeState;
                                TakeAwayCashier.MessageReceived -= MessageReceived;
                                //TakeAwayCashier.ServingBatchesChanged -= ServingBatchesChanged;
                                if (TakeAwayCashier is ITransparentProxy)
                                    (TakeAwayCashier as ITransparentProxy).Reconnected -= TakeAwayCashierPresentation_Reconnected;
                            }
                            if (TakeAwayCashier != null && TakeAwayCashier.OAuthUserIdentity == authUser.User_ID)
                            {
                                AuthUser = authUser;
                                ActiveShiftWork = TakeAwayCashier.ShiftWork;
                                //UpdateServingBatches(TakeAwayCashier.GetServingBatches());
                                TakeAwayCashier.ObjectChangeState += TakeAwayCashier_ObjectChangeState;
                                TakeAwayCashier.MessageReceived += MessageReceived;
                                //TakeAwayCashier.ServingBatchesChanged += ServingBatchesChanged;
                                if (TakeAwayCashier is ITransparentProxy)
                                    (TakeAwayCashier as ITransparentProxy).Reconnected += TakeAwayCashierPresentation_Reconnected;
                                IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
#if DeviceDotNet
                                TakeAwayCashier.DeviceFirebaseToken = device.FirebaseToken;
#endif
                                (this.FlavoursOrderServer as DontWaitApp.FlavoursOrderServer).SignedInFlavourBusinessUser = TakeAwayCashier;
                                //ApplicationSettings.Current.FriendlyName = TakeAwayCashier.FullName;
                                GetMessages();

                                OAuthUserIdentity = TakeAwayCashier.OAuthUserIdentity;
                                var sdds = DeviceAuthentication.AuthUser;


                                if (ActiveShiftWork != null)
                                    _TakeAwaySession = await FlavoursOrderServer.GetFoodServicesClientSessionViewModel(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));

                                return true;

                            }
                        }
                        IAuthFlavourBusiness pAuthFlavourBusiness = null;

                        try
                        {
                            pAuthFlavourBusiness = FlavoursServiceOrderTakingStation.GetFlavourBusinessAuth();
                        }
                        catch (System.Net.WebException error)
                        {
                            throw;
                        }
                        catch (Exception error)
                        {
                            throw;
                        }

                        this.UserData = pAuthFlavourBusiness.SignIn();
                        if (UserData != null)
                        {
                            FullName = UserData.FullName;
                            UserName = UserData.UserName;
                            PhoneNumber = UserData.PhoneNumber;
                            Email = UserData.Email;


                            foreach (var role in UserData.Roles.Where(x => x.RoleType == RoleType.TakeAwayCashier))
                            {
                                if (role.RoleType == RoleType.TakeAwayCashier)
                                {
                                    if (TakeAwayCashier != null)
                                    {
                                        TakeAwayCashier.ObjectChangeState -= TakeAwayCashier_ObjectChangeState;
                                        TakeAwayCashier.MessageReceived -= MessageReceived;
                                        //TakeAwayCashier.ServingBatchesChanged -= ServingBatchesChanged;
                                        if (TakeAwayCashier is ITransparentProxy)
                                            (TakeAwayCashier as ITransparentProxy).Reconnected -= TakeAwayCashierPresentation_Reconnected;
                                    }
                                    TakeAwayCashier = RemotingServices.CastTransparentProxy<ITakeawayCashier>(role.User);
                                    if (TakeAwayCashier == null)
                                        continue;

                                    OAuthUserIdentity = TakeAwayCashier.OAuthUserIdentity;
                                    string objectRef = RemotingServices.SerializeObjectRef(TakeAwayCashier);
                                    ApplicationSettings.Current.WaiterObjectRef = objectRef;
                                    TakeAwayCashier.ObjectChangeState += TakeAwayCashier_ObjectChangeState;
                                    TakeAwayCashier.MessageReceived += MessageReceived;
                                    //TakeAwayCashier.ServingBatchesChanged += ServingBatchesChanged;
                                    if (TakeAwayCashier is ITransparentProxy)
                                        (TakeAwayCashier as ITransparentProxy).Reconnected += TakeAwayCashierPresentation_Reconnected;


#if DeviceDotNet
                                    IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                    TakeAwayCashier.DeviceFirebaseToken = device.FirebaseToken;
                                    if (!device.IsBackgroundServiceStarted)
                                    {
                                        BackgroundServiceState serviceState = new BackgroundServiceState();
                                        device.RunInBackground(new Action(async () =>
                                        {
                                            var message = TakeAwayCashier.PeekMessage();
                                            TakeAwayCashier.MessageReceived += MessageReceived;
                                            do
                                            {
                                                System.Threading.Thread.Sleep(1000);

                                            } while (!serviceState.Terminate);

                                            TakeAwayCashier.MessageReceived -= MessageReceived;
                                            //if (Waiter is ITransparentProxy)
                                            //    (Waiter as ITransparentProxy).Reconnected -= WaiterPresentation_Reconnected;
                                        }), serviceState);
                                    }
#endif
                                    ActiveShiftWork = TakeAwayCashier.ShiftWork;
                                    //UpdateServingBatches(TakeAwayCashier.GetServingBatches());
                                    (this.FlavoursOrderServer as DontWaitApp.FlavoursOrderServer).SignedInFlavourBusinessUser = TakeAwayCashier;

                                    GetMessages();
                                }
                            }
                            //https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/DiningTableChairs020.svg


                            AuthUser = authUser;
                            if (TakeAwayCashier != null)
                            {
                                OAuthUserIdentity = TakeAwayCashier.OAuthUserIdentity;
                                var sdds = DeviceAuthentication.AuthUser;

                                if (ActiveShiftWork != null)
                                {
                                    OOAdvantech.IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

                                    _TakeAwaySession = await FlavoursOrderServer.GetFoodServicesClientSessionViewModel(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));
                                }


                            }
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

                var result = await SignInTask;
                SignInTask = null;
                return result;

            }

        }



        /// <MetaDataID>{90d96b7e-007e-4998-a9f5-b83ec28479a7}</MetaDataID>
        private void TakeAwayCashierPresentation_Reconnected(object sender)
        {

        }

        /// <MetaDataID>{ba8924d0-9809-446c-92f3-db05d2126605}</MetaDataID>
        private void MessageReceived(IMessageConsumer sender)
        {

        }

        /// <MetaDataID>{0989d879-8309-46fc-ba3a-947448f9bfb4}</MetaDataID>
        private void TakeAwayCashier_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IServicesContextWorker.ShiftWork))
            {
                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWorkStartedAt));
                GetMessages();
            }
        }

        /// <MetaDataID>{4ee6f4c1-6fbf-41ab-9499-ea5f181cb690}</MetaDataID>
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
        /// <MetaDataID>{e4cfa63c-605f-4470-9395-82855d711722}</MetaDataID>
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

        /// <MetaDataID>{a8c2825a-e99b-41fd-a47a-9e19dedb93a3}</MetaDataID>
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


        /// <MetaDataID>{544700dd-b3ab-48f8-9fb9-a92603c17d3b}</MetaDataID>
        public async void ShiftWorkStart(DateTime startedAt, double timespanInHours)
        {
            ActiveShiftWork = TakeAwayCashier.NewShiftWork(startedAt, timespanInHours);

            if (ActiveShiftWork != null)
            {
                IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(IDeviceOOAdvantechCore)) as IDeviceOOAdvantechCore;
                _TakeAwaySession = await FlavoursOrderServer.GetFoodServicesClientSessionViewModel(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));


                ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));
            }


        }

        /// <MetaDataID>{e9398c9a-4676-4c56-95e3-8a70d3050ae7}</MetaDataID>
        private void GetMessages()
        {

        }

        /// <MetaDataID>{5c8b0442-2bf1-4943-9704-7b38befd3908}</MetaDataID>
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

        /// <MetaDataID>{3396825a-53ec-425b-9fc3-6e50ca351fdd}</MetaDataID>
        public Task<IList<UserData>> GetNativeUsers()
        {
            return Task<IList<UserData>>.Run(() =>
            {
                return this.TakeAwayStation.GetNativeUsers();
            });
        }

        /// <MetaDataID>{17df1e47-7f36-4cbf-aa07-928059b6ec0f}</MetaDataID>
        public UserData SignInNativeUser(string userName, string password)
        {
            return this.TakeAwayStation.SignInNativeUser(userName, password);
        }
        /// <MetaDataID>{cd75f775-036f-40a0-b1df-9ec6ad410980}</MetaDataID>
        public static IAuthFlavourBusiness GetFlavourBusinessAuth()
        {
            IAuthFlavourBusiness pAuthFlavourBusiness;
            OOAdvantech.Remoting.RestApi.AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as OOAdvantech.Remoting.RestApi.AuthUser;
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.AuthFlavourBusiness";// typeof(FlavourBusinessManager.AuthFlavourBusiness).FullName;
            System.Runtime.Remoting.Messaging.CallContext.SetData("AutUser", authUser);
            string serverUrl = AzureServerUrl;
            var remoteObject = RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData);
            pAuthFlavourBusiness = RemotingServices.CastTransparentProxy<IAuthFlavourBusiness>(remoteObject);
            return pAuthFlavourBusiness;
        }

        public static IFlavoursServicesContextManagment GetServicesContextManagment()
        {

            IAuthFlavourBusiness pAuthFlavourBusiness;
            OOAdvantech.Remoting.RestApi.AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as OOAdvantech.Remoting.RestApi.AuthUser;
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;
            IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            return servicesContextManagment;
        }

        /// <MetaDataID>{56425520-6845-48c1-9d95-ca1d08642eec}</MetaDataID>
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
        /// <MetaDataID>{8eb726f0-1621-4018-be9a-a07df03d5d9b}</MetaDataID>
        public void CreateUserWithEmailAndPassword(string emailVerificationCode)
        {
            IAuthFlavourBusiness pAuthFlavourBusiness = null;


            try
            {
                UserData userData = new UserData() { UserName = UserName, Email = Email, FullName = FullName, PhoneNumber = PhoneNumber };
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

        /// <MetaDataID>{c73714ed-3114-486e-ad8a-cbc9308ab2ee}</MetaDataID>
        public void SaveUserProfile()
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{4e633d11-713d-459a-8516-4b488f2694c2}</MetaDataID>
        public Task<bool> AssignTakeAwayStationCredentialKey(string credentialKey)
        {
            if (TakeAwayStationCredentialKey != credentialKey)
            {

                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                string serverUrl = AzureServerUrl;
                IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));

                TakeAwayStation = servicesContextManagment.GetTakeAwayStation(credentialKey);
                if (TakeAwayStation != null)
                {
                    TakeAwayStationCredentialKey = credentialKey;

                }

                return Task.FromResult(TakeAwayStation != null);

            }
            return Task.FromResult(true);
        }

        /// <MetaDataID>{644e3027-eddf-448e-8b60-9cb055b47487}</MetaDataID>
        public Task<bool> IsTakeAwayStationActive
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TakeAwayStationCredentialKey))
                {
                    TakeAwayStationCredentialKey = "7f9bde62e6da45dc8c5661ee2220a7b0_21204920cc31421685ebe82753f61a2b";
                }
                if (TakeAwayStation == null && !string.IsNullOrEmpty(TakeAwayStationCredentialKey))
                {
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    TakeAwayStation = servicesContextManagment.GetTakeAwayStation(TakeAwayStationCredentialKey);
                    if (TakeAwayStation != null)
                    {
                        var sdds = DeviceAuthentication.AuthUser;
                        //IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                        //FlavoursOrderServer.OpenFoodServicesClientSession(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));
                    }

                }
                return Task.FromResult(TakeAwayStation != null);
            }
        }



        public Task<bool> IsDeliveryCallCenterStationActive
        {
            get
            {


                if (string.IsNullOrWhiteSpace(DeliveryCallCenterCredentialKey))
                {
                    DeliveryCallCenterCredentialKey = "7f9bde62e6da45dc8c5661ee2220a7b0_37c2a132289d42cd94083ce402cd2f3f";
                }
                if (HomeDeliveryCallCenterStation == null && !string.IsNullOrEmpty(DeliveryCallCenterCredentialKey))
                {
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    HomeDeliveryCallCenterStation = servicesContextManagment.GetHomeDeliveryCallCenterStation(DeliveryCallCenterCredentialKey);

                }
                return Task.FromResult(HomeDeliveryCallCenterStation != null);
            }
        }



#if DeviceDotNet
        /// <MetaDataID>{4d76efeb-d730-4e95-ac86-753f1f59d612}</MetaDataID>
        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        /// <MetaDataID>{4dc92e1a-0ad3-4ea3-a9d3-b629e653f1cc}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);
        /// <MetaDataID>{46a39da8-3d73-45ad-9c6e-c64766fb6359}</MetaDataID>
        private bool OnSignIn;
        /// <MetaDataID>{6783baf4-0261-4de5-bb1f-bc87a3224ff8}</MetaDataID>
        private Task<bool> SignInTask;
        /// <MetaDataID>{c6f564b3-49b0-403b-867f-b519b17e5bae}</MetaDataID>
        private UserData UserData;


        /// <MetaDataID>{5427ba17-39b5-4067-bb96-925c49e549fe}</MetaDataID>
        public async Task<bool> AssignTakeAwayStation(bool useFrontCameraIfAvailable)
        {



            //UserDialogs.Instance.Prompt(("Hello world", "Take away");
            return await Task<bool>.Run(async () =>
            {
#if DeviceDotNet
                try
                {
                    var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically", useFrontCameraIfAvailable);

                    if (result == null || string.IsNullOrWhiteSpace(result.Text))
                        return false;

                    //string communicationCredentialKey = "7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad";
                    string communicationCredentialKey = result.Text;

                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));


                    return await AssignTakeAwayStationCredentialKey(communicationCredentialKey);
                    //var takeAwayStation = servicesContextManagment.GetTakeAwayStation(communicationCredentialKey);
                    //if (takeAwayStation!=null)
                    //    CommunicationCredentialKey = communicationCredentialKey;


                    //return TakeAwayStation!=null;
                }
                catch (System.Exception error)
                {

                    throw;
                }
                //PreparationStation = servicesContextManagment.GetPreparationStationRuntime(communicationCredentialKey);
                //if (PreparationStation != null)
                //{
                //    Title = PreparationStation.Description;
                //    ItemsPreparationTags = PreparationStation.ItemsPreparationTags;
                //    CommunicationCredentialKey = communicationCredentialKey;
                //    var restaurantMenuDataSharedUri = PreparationStation.RestaurantMenuDataSharedUri;
                //    HttpClient httpClient = new HttpClient();
                //    var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
                //    getJsonTask.Wait();
                //    var json = getJsonTask.Result;
                //    var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
                //    MenuItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.JsonViewModel.MenuFoodItem>>(json, jSetttings).ToDictionary(x => x.Uri);
                //    GetMenuLanguages(MenuItems.Values.ToList());
                //    PreparationStationStatus preparationStationStatus = PreparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>(), null);
                //    ItemsPreparationContexts = preparationStationStatus.NewItemsUnderPreparationControl.ToList();
                //    ServingTimeSpanPredictions = preparationStationStatus.ServingTimespanPredictions;
                //    PreparationVelocity = PreparationStation.PreparationVelocity;
                //    ItemsPreparationContextPresentations = (from itemsPreparationContext in ItemsPreparationContexts
                //                                            select new ItemsPreparationContextPresentation()
                //                                            {
                //                                                Description = itemsPreparationContext.MealCourseDescription,
                //                                                StartsAt = itemsPreparationContext.MealCourseStartsAt,
                //                                                MustBeServedAt = itemsPreparationContext.ServedAtForecast,
                //                                                PreparationOrder = itemsPreparationContext.PreparatioOrder,
                //                                                ServicesContextIdentity = itemsPreparationContext.ServicePoint.ServicesContextIdentity,
                //                                                ServicesPointIdentity = itemsPreparationContext.ServicePoint.ServicesPointIdentity,
                //                                                Uri = itemsPreparationContext.Uri,
                //                                                PreparationItems = itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().OrderByDescending(x => x.CookingTimeSpanInMin).Select(x => new PreparationStationItem(x, itemsPreparationContext, MenuItems, ItemsPreparationTags)).OrderBy(x => x.AppearanceOrder).ToList()
                //                                            }).ToList();


                //    return true;
                //}
                //else
                //{
                //    Title = "";
                //    return false;
                //}
#else
                return false;
#endif
            });
        }



        /// <MetaDataID>{4eeb4a74-20b3-4968-99e6-d0b241f0d401}</MetaDataID>
        public async Task<bool> RequestPermissionsForQRCodeScan()
        {
#if DeviceDotNet


            var cameraPermissionRequest = await Permissions.RequestAsync<Permissions.Camera>();

            return await CheckPermissionsForQRCodeScan();

            //var locationInUsePermisions = await Permissions.RequestAsync<Permissions.Camera>();
            //return locationInUsePermisions == PermissionStatus.Granted;

#else
            return await Task<bool>.FromResult(true);
#endif
        }

        /// <MetaDataID>{40d62a0a-1971-4c1a-8f6a-5b59041d90de}</MetaDataID>
        public async Task<bool> CheckPermissionsForQRCodeScan()
        {
#if DeviceDotNet
            var locationInUsePermisions = await Permissions.CheckStatusAsync<Permissions.Camera>();
            return locationInUsePermisions == PermissionStatus.Granted;
#else
            return await Task<bool>.FromResult(false);
#endif
        }

        /// <MetaDataID>{9a2c5181-e187-425b-8e49-51fa4b7363c2}</MetaDataID>
        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            if (uri == "./TakeAwayStation")
                return this;
            return FlavoursOrderServer as MarshalByRefObject;
        }

        /// <MetaDataID>{77cc5b05-dca8-4c49-a7e7-4a0a0c7c16d6}</MetaDataID>
        public async void TakeAwayOrderCommitted()
        {
            if (_TakeAwaySession?.FoodServicesClientSession?.SessionState == ClientSessionState.ItemsCommited)
            {
                IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                _TakeAwaySession = await FlavoursOrderServer.GetFoodServicesClientSessionViewModel(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));
            }

        }





        /// <MetaDataID>{115be581-0ca0-47e1-8580-f74b9aa5019a}</MetaDataID>
        IPlace HomeDeliveryPlaceOfDistribution { get; }

        /// <MetaDataID>{0f27ba87-430d-45b2-95fb-b950d1f90482}</MetaDataID>
        public DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; private set; }
        /// <MetaDataID>{a1576909-e02c-40f2-8798-fba2f670f73f}</MetaDataID>
        public string SignInProvider { get; set; }
        /// <MetaDataID>{5b0b7857-a392-4f65-a4ff-10b5ba317161}</MetaDataID>
        public string OAuthUserIdentity { get; set; }
        /// <MetaDataID>{fb7c60d1-b5be-4e11-8978-f151eaeeaabd}</MetaDataID>
        public string FullName { get; set; }
        /// <MetaDataID>{9a92b74f-0ae1-41aa-8765-737412444e19}</MetaDataID>
        public string UserName { get; set; }
        /// <MetaDataID>{d2c7c515-fb53-414c-8f08-39fd0018e22f}</MetaDataID>
        public string Email { get; set; }
        /// <MetaDataID>{e180b0f4-5e33-419a-8679-69ce4bbfb225}</MetaDataID>
        public string Password { get; set; }
        /// <MetaDataID>{50cc9ba4-9777-4bfe-a9ee-4f6f3a2e94a0}</MetaDataID>
        public string ConfirmPassword { get; set; }
        /// <MetaDataID>{96870bba-3747-4b7a-ae7e-27cb503f28ff}</MetaDataID>
        public string PhoneNumber { get; set; }
        /// <MetaDataID>{f46e93f1-6ec1-4507-964e-1efab28b06c2}</MetaDataID>
        public string TakeAwayStationCredentialKey
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.TakeAwayStationCredentialKey))
                {

                }
                return ApplicationSettings.Current.TakeAwayStationCredentialKey;
            }
            set
            {
                ApplicationSettings.Current.TakeAwayStationCredentialKey = value;
            }
        }



        /// <MetaDataID>{4fdf451e-b550-45bf-aabe-aa7de6c3bb94}</MetaDataID>
        public ITakeAwayStation TakeAwayStation { get; private set; }
        /// <MetaDataID>{1fb7bf4b-c6a4-4320-af85-cec3ca9fccd1}</MetaDataID>
        public string DeliveryCallCenterCredentialKey
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.DeliveryCallCenterCredentialKey))
                {

                }
                return ApplicationSettings.Current.DeliveryCallCenterCredentialKey;
            }
            set
            {
                ApplicationSettings.Current.DeliveryCallCenterCredentialKey = value;
            }
        }


        IHomeDeliveryCallCenterStation _HomeDeliveryCallCenterStation;
        public IHomeDeliveryCallCenterStation HomeDeliveryCallCenterStation
        {
            get => _HomeDeliveryCallCenterStation;
            private set
            {
                if (_HomeDeliveryCallCenterStation != value)
                {

                    if (_HomeDeliveryCallCenterStation!=null)
                        _HomeDeliveryCallCenterStation.ObjectChangeState-=HomeDeliveryCallCenterStation_ObjectChangeState;
                    _HomeDeliveryCallCenterStation = value;
                    if (_HomeDeliveryCallCenterStation!=null)
                        _HomeDeliveryCallCenterStation.ObjectChangeState+=HomeDeliveryCallCenterStation_ObjectChangeState;


                }
            }
        }

        private void HomeDeliveryCallCenterStation_ObjectChangeState(object _object, string member)
        {
            if (_WatchingOrders!=null)
            {
                var watchingOrders = _WatchingOrders.ToList();
                var stationWatchingOrders = watchingOrders.Select(x => new WatchingOrderAbbreviation() { SessionID=x.SessionID, TimeStamp=x.TimeStamp }).ToList();
                var callCenterStationWatchingOrders = _HomeDeliveryCallCenterStation.GetWatchingOrders(stationWatchingOrders);

                watchingOrders=watchingOrders.Where(x => !callCenterStationWatchingOrders.MissingWatchingOrders.Any(removedWatchingOrder => removedWatchingOrder.SessionID==x.SessionID)).ToList();
                watchingOrders=watchingOrders.Where(x => !callCenterStationWatchingOrders.WatchingOrders.Any(removedWatchingOrder => removedWatchingOrder.SessionID==x.SessionID)).ToList();
                watchingOrders.AddRange(callCenterStationWatchingOrders.WatchingOrders.Select(watchingOrder => new WatchingOrderPresentation(watchingOrder, watchingOrder.MealCourses.Select(x => _MealCoursesInProgress.GetViewModelFor(x, x)).ToList())).ToList());
                _WatchingOrders=watchingOrders;



            }
            ObjectChangeState?.Invoke(this, nameof(WatchingOrders));
        }



        /// <exclude>Excluded</exclude>
        List<IHomeDeliverySession> _HomeDeliverySessions = new List<IHomeDeliverySession>();

        public List<IHomeDeliverySession> HomeDeliverySessions => _HomeDeliverySessions;

        /// <exclude>Excluded</exclude>
        IFoodServicesClientSessionViewModel _TakeAwaySession;
        public IFoodServicesClientSessionViewModel TakeAwaySession => _TakeAwaySession;


        List<IHomeDeliveryServicePoint> _HomeDeliveryServicePoints;


        public List<HomeDeliveryServicePointAbbreviation> HomeDeliveryServicePoints
        {
            get
            {
                if (this.HomeDeliveryCallCenterStation != null)
                {
                    if (_HomeDeliveryServicePoints == null)
                        _HomeDeliveryServicePoints = this.HomeDeliveryCallCenterStation.HomeDeliveryServicePoints;

                    return _HomeDeliveryServicePoints.Select(x => new HomeDeliveryServicePointAbbreviation() { Description = x.Description, ServicesContextIdentity = x.ServicesContextIdentity, ServicesPointIdentity = x.ServicesPointIdentity }).ToList();
                }
                return new List<HomeDeliveryServicePointAbbreviation>();
            }
        }

        UIBaseEx.ViewModelWrappers<IMealCourse, FlavourBusinessManager.RoomService.ViewModel.MealCourse> _MealCoursesInProgress = new UIBaseEx.ViewModelWrappers<IMealCourse, FlavourBusinessManager.RoomService.ViewModel.MealCourse>();

        List<WatchingOrderPresentation> _WatchingOrders = null;
        public List<WatchingOrderPresentation> WatchingOrders
        {
            get
            {
                if (_WatchingOrders == null)
                {
                    if (_HomeDeliveryCallCenterStation != null)
                    {
                        var callCenterStationWatchingOrders = _HomeDeliveryCallCenterStation.GetWatchingOrders();


                        _WatchingOrders = callCenterStationWatchingOrders.WatchingOrders.Select(watchingOrder => new WatchingOrderPresentation(watchingOrder, watchingOrder.MealCourses.Select(x => _MealCoursesInProgress.GetViewModelFor(x, x)).ToList())).ToList();

                        //_WatchingOrders.AddRange(_WatchingOrders.ToList());
                        //_WatchingOrders.AddRange(_WatchingOrders.ToList());
                        //_WatchingOrders.AddRange(_WatchingOrders.ToList());
                        //_WatchingOrders.AddRange(_WatchingOrders.ToList());
                    }
                }
                return _WatchingOrders;
            }
        }

        /// <MetaDataID>{7f7af574-b2e1-47b3-9e7d-4a6773840adb}</MetaDataID>
        public Task<bool> AssignDeliveryCallCenterCredentialKey(string credentialKey)
        {
            if (DeliveryCallCenterCredentialKey != credentialKey)
            {

                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                string serverUrl = AzureServerUrl;
                IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));

                HomeDeliveryCallCenterStation = servicesContextManagment.GetHomeDeliveryCallCenterStation(credentialKey);
                if (HomeDeliveryCallCenterStation != null)
                {
                    DeliveryCallCenterCredentialKey = credentialKey;

                }

                return Task.FromResult(HomeDeliveryCallCenterStation != null);

            }
            return Task.FromResult(true);
        }

        public async Task<bool> AssignDeliveryCallCenterStation(bool useFrontCameraIfAvailable)
        {
            //UserDialogs.Instance.Prompt(("Hello world", "Take away");
            return await Task<bool>.Run(async () =>
            {
#if DeviceDotNet
                try
                {
                    var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically", useFrontCameraIfAvailable);

                    if (result == null || string.IsNullOrWhiteSpace(result.Text))
                        return false;

                    //string communicationCredentialKey = "7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad";
                    string communicationCredentialKey = result.Text;

                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));


                    return await AssignDeliveryCallCenterCredentialKey(communicationCredentialKey);

                }
                catch (System.Exception error)
                {

                    throw;
                }

#else
                return false;
#endif
            });

        }

        public Task<IHomeDeliverySession> GetHomeDeliverSession(string sessionID)
        {
            return Task<IHomeDeliverySession>.Run(() =>
            {
                var watchingOrder = this.WatchingOrders.Where(x => x.SessionID == sessionID).FirstOrDefault();
                IHomeDeliverySession homeDeliverySession = HomeDeliverySessions.Where(x => x.FoodServiceClientSession.MainSessionID == sessionID).FirstOrDefault();

                if (homeDeliverySession != null)
                    return homeDeliverySession;

                var foodServicesClientSessionViewModel = this.FlavoursOrderServer.GetFoodServicesClientSessionViewModel(HomeDeliveryCallCenterStation.Menu);
                homeDeliverySession = HomeDeliverySession.GetHomeDeliverySession(this, watchingOrder.HomeDeliveryServicePoint, HomeDeliveryServicePoints, foodServicesClientSessionViewModel, sessionID);
                this.HomeDeliverySessions.Add(homeDeliverySession);

                return homeDeliverySession;
            });


        }

        public async Task<IHomeDeliverySession> NewHomeDeliverySession()
        {

            var foodServicesClientSessionViewModel = this.FlavoursOrderServer.GetFoodServicesClientSessionViewModel(HomeDeliveryCallCenterStation.Menu);
            //IFoodServiceClientSession foodServiceClientSession=HomeDeliveryCallCenterStation.NewHomeDeliverFoodServicesClientSession();
            //var foodServicesClientSessionViewModel = await this.FlavoursOrderServer.GetFoodServicesClientSessionViewModel(foodServiceClientSession);
            var homeDeliverySession = new HomeDeliverySession(this, foodServicesClientSessionViewModel);
            this.HomeDeliverySessions.Add(homeDeliverySession);
            homeDeliverySession.HomeDeliveryServicePoints = HomeDeliveryServicePoints;
            return homeDeliverySession;

        }

        public void CancelHomeDeliverySession(IHomeDeliverySession homeDeliverySession)
        {
            this.HomeDeliveryCallCenterStation.CancelHomeDeliverFoodServicesClientSession(homeDeliverySession.FoodServiceClientSession.FoodServicesClientSession);
            FlavoursOrderServer.SessionIsNoLongerActive(homeDeliverySession.FoodServiceClientSession);
        }

        internal void UpdateWatchingOrder(WatchingOrder watchingOrder)
        {

            if (watchingOrder!=null)
            {

                var existingWatchingOrder = this.WatchingOrders.Where(x => x.SessionID == watchingOrder.SessionID).FirstOrDefault();
                if (existingWatchingOrder != null)
                {
                    var watchingOrderPresentation = new WatchingOrderPresentation(watchingOrder, watchingOrder.MealCourses.Select(x => _MealCoursesInProgress.GetViewModelFor(x, x)).ToList());

                    this._WatchingOrders.Insert(_WatchingOrders.IndexOf(existingWatchingOrder), watchingOrderPresentation);
                    this._WatchingOrders.Remove(existingWatchingOrder);
                }
                else
                {
                    var watchingOrderPresentation = new WatchingOrderPresentation(watchingOrder, watchingOrder.MealCourses.Select(x => _MealCoursesInProgress.GetViewModelFor(x, x)).ToList());
                    this._WatchingOrders.Add(existingWatchingOrder);
                }

                ObjectChangeState?.Invoke(this, nameof(WatchingOrders));

            }
        }
    }
}