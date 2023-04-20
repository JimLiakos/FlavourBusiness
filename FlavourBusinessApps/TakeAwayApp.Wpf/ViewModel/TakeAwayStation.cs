using FlavourBusinessFacade;
using FlavourBusinessFacade.ViewModel;
using OOAdvantech;
using OOAdvantech.Json.Linq;
using OOAdvantech.Remoting;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#else
using MarshalByRefObject = System.MarshalByRefObject;
#endif


namespace TakeAwayApp
{
    /// <MetaDataID>{1dca0d48-ef5c-41ac-a964-127385b2e256}</MetaDataID>
    public interface IFlavoursTakeAwayStation
    {
        DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; }
        string CommunicationCredentialKey { get; set; }
        Task<bool> AssignCommunicationCredentialKey(string credentialKey);
        Task<bool> AssignTakeAwayStation(bool useFrontCameraIfAvailable);
    }
    /// <MetaDataID>{efe40e2f-68a3-4ee7-afde-5cf1ffd4c62e}</MetaDataID>
    public class TakeAwayStation : MarshalByRefObject, IFlavoursTakeAwayStation, IExtMarshalByRefObject, ILocalization,ISecureUser
    {
        
        public TakeAwayStation()
        {

            FlavoursOrderServer = new DontWaitApp.FlavoursOrderServer() { EndUser = this };
        }

        string lan = "el";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
        public string Language { get { return lan; } }

        string deflan = "en";
        public string DefaultLanguage { get { return deflan; } }

        public string AppIdentity => "com.microneme.takeawaystationapp";

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

        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();

        public event ObjectChangeStateHandle ObjectChangeState;

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

        public void SignOut()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SignUp()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SignIn()
        {
            throw new System.NotImplementedException();
        }

        public void SaveUserProfile()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AssignCommunicationCredentialKey(string credentialKey)
        {
            throw new System.NotImplementedException();
        }

        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

        public Task<bool> AssignTakeAwayStation(bool useFrontCameraIfAvailable)
        {
            return Task<bool>.Run(async () =>
            {
#if DeviceDotNet
                var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically", useFrontCameraIfAvailable);

                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return false;
                string communicationCredentialKey = "7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad";
                //communicationCredentialKey =result.Text;

                string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                string serverUrl = AzureServerUrl;
                IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                PreparationStation = servicesContextManagment.GetPreparationStationRuntime(communicationCredentialKey);
                if (PreparationStation != null)
                {
                    Title = PreparationStation.Description;
                    ItemsPreparationTags = PreparationStation.ItemsPreparationTags;
                    CommunicationCredentialKey = communicationCredentialKey;
                    var restaurantMenuDataSharedUri = PreparationStation.RestaurantMenuDataSharedUri;
                    HttpClient httpClient = new HttpClient();
                    var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
                    getJsonTask.Wait();
                    var json = getJsonTask.Result;
                    var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
                    MenuItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.JsonViewModel.MenuFoodItem>>(json, jSetttings).ToDictionary(x => x.Uri);
                    GetMenuLanguages(MenuItems.Values.ToList());
                    PreparationStationStatus preparationStationStatus = PreparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>(), null);
                    ItemsPreparationContexts = preparationStationStatus.NewItemsUnderPreparationControl.ToList();
                    ServingTimeSpanPredictions = preparationStationStatus.ServingTimespanPredictions;
                    PreparationVelocity = PreparationStation.PreparationVelocity;
                    ItemsPreparationContextPresentations = (from itemsPreparationContext in ItemsPreparationContexts
                                                            select new ItemsPreparationContextPresentation()
                                                            {
                                                                Description = itemsPreparationContext.MealCourseDescription,
                                                                StartsAt = itemsPreparationContext.MealCourseStartsAt,
                                                                MustBeServedAt = itemsPreparationContext.ServedAtForecast,
                                                                PreparationOrder = itemsPreparationContext.PreparatioOrder,
                                                                ServicesContextIdentity = itemsPreparationContext.ServicePoint.ServicesContextIdentity,
                                                                ServicesPointIdentity = itemsPreparationContext.ServicePoint.ServicesPointIdentity,
                                                                Uri = itemsPreparationContext.Uri,
                                                                PreparationItems = itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().OrderByDescending(x => x.CookingTimeSpanInMin).Select(x => new PreparationStationItem(x, itemsPreparationContext, MenuItems, ItemsPreparationTags)).OrderBy(x => x.AppearanceOrder).ToList()
                                                            }).ToList();


                    return true;
                }
                else
                {
                    Title = "";
                    return false;
                }
#else
                return false;
#endif
            });
        }

        public DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; private set; }
        public string SignInProvider { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string OAuthUserIdentity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string FullName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string UserName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Email { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Password { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string ConfirmPassword { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string PhoneNumber { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string CommunicationCredentialKey { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
