
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.ViewModel;

using OOAdvantech;
using OOAdvantech.Json.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


#if DeviceDotNet
using Acr.UserDialogs;
using Xamarin.Essentials;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using FlavourBusinessManager.ServicesContextResources;
using MarshalByRefObject = System.MarshalByRefObject;
#endif


namespace TakeAwayApp
{
    /// <MetaDataID>{1dca0d48-ef5c-41ac-a964-127385b2e256}</MetaDataID>
    [HttpVisible]
    public interface IFlavoursTakeAwayStation
    {
        /// <MetaDataID>{d665ceb1-47b3-4a6a-9671-c12e4bf7737d}</MetaDataID>
        DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; }
        /// <MetaDataID>{0cc4170b-f2a9-47e0-a93c-813f6ec8abd1}</MetaDataID>
        string CommunicationCredentialKey { get; set; }
        /// <MetaDataID>{0a4ccfb0-2a60-41cc-8cce-2577a15b84ea}</MetaDataID>
        Task<bool> AssignCommunicationCredentialKey(string credentialKey);
        /// <MetaDataID>{0d0668da-c0ce-49ed-9a74-c6a574409a1c}</MetaDataID>
        Task<bool> AssignTakeAwayStation(bool useFrontCameraIfAvailable);

        Task<bool> IsActive { get; }


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



    }
    /// <MetaDataID>{efe40e2f-68a3-4ee7-afde-5cf1ffd4c62e}</MetaDataID>
    public class TakeAwayStationPresentation : MarshalByRefObject, IFlavoursTakeAwayStation, IExtMarshalByRefObject, ILocalization, ISecureUser, OOAdvantech.Remoting.RestApi.IBoundObject
    {

        /// <MetaDataID>{67d25e6d-5d8c-498a-bced-8522e4e9ac08}</MetaDataID>
        public TakeAwayStationPresentation()
        {

            FlavoursOrderServer = new DontWaitApp.FlavoursOrderServer() { EndUser = this };
            var appSettings = ApplicationSettings.Current;

            //var dd = DeviceDisplay.MainDisplayInfo;

            //var ewr = Xamarin.Essentials.DeviceInfo.Platform;
            //var ss = Xamarin.Forms.Device.Idiom;
        }

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
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{3f9b769c-7a99-4487-a68e-6f9d077ea4be}</MetaDataID>
        public Task<bool> SignUp()
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{d7005bd3-dec7-4101-a42c-8ad172b92829}</MetaDataID>
        public Task<bool> SignIn()
        {
            return Task<bool>.FromResult( false);
        }

        /// <MetaDataID>{c73714ed-3114-486e-ad8a-cbc9308ab2ee}</MetaDataID>
        public void SaveUserProfile()
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{4e633d11-713d-459a-8516-4b488f2694c2}</MetaDataID>
        public Task<bool> AssignCommunicationCredentialKey(string credentialKey)
        {
            if (CommunicationCredentialKey != credentialKey)
            {

                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                string serverUrl = AzureServerUrl;
                IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));

                TakeAwayStation = servicesContextManagment.GetTakeAwayStation(credentialKey);
                if (TakeAwayStation!=null)
                {
                    CommunicationCredentialKey = credentialKey;

                    OOAdvantech.IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    FlavoursOrderServer.OpenFoodServicesClientSession( TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));

                    
                }

                return Task.FromResult(TakeAwayStation!=null);

            }
            return Task.FromResult(true);
        }

        public Task<bool> IsActive
        {
            get
            {
                //if(string.IsNullOrWhiteSpace( CommunicationCredentialKey))
                //{
                //    CommunicationCredentialKey="7f9bde62e6da45dc8c5661ee2220a7b0_66294b0d4ec04e54814c309257358ea4";
                //}
                if(TakeAwayStation==null&&!string.IsNullOrEmpty(CommunicationCredentialKey))
                {
                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    TakeAwayStation = servicesContextManagment.GetTakeAwayStation(CommunicationCredentialKey);
                    if (TakeAwayStation!=null)
                    {
                        IDeviceOOAdvantechCore device = Xamarin.Forms.DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                        FlavoursOrderServer.OpenFoodServicesClientSession(TakeAwayStation.GetUncommittedFoodServiceClientSession(TakeAwayStation.Description, device.DeviceID, FlavourBusinessFacade.DeviceType.Desktop, device.FirebaseToken));
                    }
                    
                }
                return Task.FromResult(TakeAwayStation!=null);
            }
        }


#if DeviceDotNet
        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        /// <MetaDataID>{4dc92e1a-0ad3-4ea3-a9d3-b629e653f1cc}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

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


                    return await AssignCommunicationCredentialKey(communicationCredentialKey);
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
         

            var cameraPermissionRequest =await Permissions.RequestAsync<Permissions.Camera>();
            
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

        public MarshalByRefObject GetObjectFromUri(string uri)
        {
              if (uri == "./TakeAwayStation")
                return this;
            return FlavoursOrderServer as MarshalByRefObject;
        }

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
        public string CommunicationCredentialKey
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.CommunicationCredentialKey))
                {

                }
                return ApplicationSettings.Current.CommunicationCredentialKey;
            }
            set
            {
                ApplicationSettings.Current.CommunicationCredentialKey = value;
            }
        }

        /// <MetaDataID>{4fdf451e-b550-45bf-aabe-aa7de6c3bb94}</MetaDataID>
        public ITakeAwayStation TakeAwayStation { get; private set; }
    }
}
