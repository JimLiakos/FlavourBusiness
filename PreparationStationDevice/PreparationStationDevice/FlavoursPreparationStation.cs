using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using FlavourBusinessManager.RoomService;
using System.Net.Http;
using FlavourBusinessFacade.RoomService;
using OOAdvantech;
using Xamarin.Forms;
using System.Reflection;
using OOAdvantech.Json.Linq;

#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#endif

namespace PreparationStationDevice
{
    /// <MetaDataID>{293c7b92-a89a-4179-a8ff-616948355d82}</MetaDataID>
    public class FlavoursPreparationStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ViewModel.ILocalization
    {
        public FlavoursPreparationStation()
        {
            var communicationCredentialKey = this.CommunicationCredentialKey;
        }



        //static string AzureServerUrl = "http://localhost:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.5:8090/api/";
        //static strinb AzureServerUrl = "http://192.168.2.10:8090/api/";

        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


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



        IPreparationStationRuntime _PreparationStation;
        IPreparationStationRuntime PreparationStation
        {
            get => _PreparationStation;
            set
            {

                if (_PreparationStation != value)
                {
                    if (_PreparationStation is ITransparentProxy)
                        (_PreparationStation as ITransparentProxy).Reconnected -= PreparationStation_Reconnected;

                    _PreparationStation = value;

                    if (_PreparationStation is ITransparentProxy)
                        (_PreparationStation as ITransparentProxy).Reconnected += PreparationStation_Reconnected;
                }
            }
        }

        private void PreparationStation_Reconnected(object sender)
        {


        }
        Dictionary<string, MenuModel.JsonViewModel.MenuFoodItem> MenuItems;

        List<ServicePointPreparationItems> ServicePointsPreparationItems = new List<ServicePointPreparationItems>();

        [HttpVisible]
        public Task<List<PreparationItemsPerServicePoint>> WebUIAttached()
        {


            return Task<List<PreparationItemsPerServicePoint>>.Run(() =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        if (PreparationStation == null && !string.IsNullOrWhiteSpace(CommunicationCredentialKey))
                        {

                            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                            string serverUrl = AzureServerUrl;
                            IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                            PreparationStation = servicesContextManagment.GetPreparationStationRuntime(CommunicationCredentialKey);
                            if (PreparationStation != null)
                            {

                                var restaurantMenuDataSharedUri = PreparationStation.RestaurantMenuDataSharedUri;
                                HttpClient httpClient = new HttpClient();
                                var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
                                getJsonTask.Wait();
                                var json = getJsonTask.Result;
                                var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
                                MenuItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.JsonViewModel.MenuFoodItem>>(json, jSetttings).ToDictionary(x => x.Uri);

                                // servicesContextManagment.
                                Title = PreparationStation.Description;

                                PreparationStation.PreparationItemsChangeState += PreparationStation_PreparationItemsChangeState;
                                ServicePointsPreparationItems = PreparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>(), null).ToList();
                            }

                            //var menuItems = PreparationStation.GetNewerRestaurandMenuData(DateTime.MinValue);
                        }
                        var preparationItemsPerServicePoint = (from servicePointItems in ServicePointsPreparationItems
                                                               select new PreparationItemsPerServicePoint()
                                                               {
                                                                   Description = servicePointItems.Description,
                                                                   ServicesContextIdentity = servicePointItems.ServicePoint.ServicesContextIdentity,
                                                                   ServicesPointIdentity = servicePointItems.ServicePoint.ServicesPointIdentity,
                                                                   Uri = servicePointItems.Uri,
                                                                   PreparationItems = servicePointItems.PreparationItems.OfType<ItemPreparation>().Select(x => new PreparationStationItem(x, servicePointItems, MenuItems)).ToList()
                                                               }).ToList();

                        return preparationItemsPerServicePoint;
                    }
                    catch (Exception error)
                    {
                        tries--;
                        if (tries == 0)
                            throw;
                        
                    }
                }
                return null;
                
            });
        }



        [HttpVisible]
        public void CancelLastPreparationStep(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ServicePointsPreparationItems
                                              from preparationStationItem in servicePointPreparationItems.PreparationItems.OfType<ItemPreparation>()
                                              select preparationStationItem).ToDictionary(x => x.uid);

            foreach (var itemPreparation in itemPreparations)
                itemPreparationsDictionary[itemPreparation.uid].Update(itemPreparation);

            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        PreparationStation.CancelLastPreparationStep(itemPreparations.Select(x => x.uid).ToList());
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
        [HttpVisible]
        public void ItemsPrepared(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ServicePointsPreparationItems
                                              from preparationStationItem in servicePointPreparationItems.PreparationItems.OfType<ItemPreparation>()
                                              select preparationStationItem).ToDictionary(x => x.uid);
            foreach (var itemPreparation in itemPreparations)
                itemPreparationsDictionary[itemPreparation.uid].Update(itemPreparation);

            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        PreparationStation.ItemsPrepared(itemPreparations.Select(x => x.uid).ToList());
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

        [HttpVisible]
        public void ItemsServing(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ServicePointsPreparationItems
                                              from preparationStationItem in servicePointPreparationItems.PreparationItems.OfType<ItemPreparation>()
                                              select preparationStationItem).ToDictionary(x => x.uid);
            foreach (var itemPreparation in itemPreparations)
                itemPreparationsDictionary[itemPreparation.uid].Update(itemPreparation);

            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        PreparationStation.ItemsServing(itemPreparations.Select(x => x.uid).ToList());
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

        [HttpVisible]
        public void ItemsΙnPreparation(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ServicePointsPreparationItems
                                              from preparationStationItem in servicePointPreparationItems.PreparationItems.OfType<ItemPreparation>()
                                              select preparationStationItem).ToDictionary(x => x.uid);
            foreach (var itemPreparation in itemPreparations)
                itemPreparationsDictionary[itemPreparation.uid].Update(itemPreparation);

            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        PreparationStation.ItemsΙnPreparation(itemPreparations.Select(x => x.uid).ToList());
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




        [HttpVisible]
        public void ItemsRoasting(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ServicePointsPreparationItems
                                              from preparationStationItem in servicePointPreparationItems.PreparationItems.OfType<ItemPreparation>()
                                              select preparationStationItem).ToDictionary(x => x.uid);
            foreach (var itemPreparation in itemPreparations)
                itemPreparationsDictionary[itemPreparation.uid].Update(itemPreparation);

            SerializeTaskScheduler.AddTask(async () =>
            {
                int tries = 30;
                while (tries > 0)
                {
                    try
                    {
                        PreparationStation.ItemsRoasting(itemPreparations.Select(x => x.uid).ToList());
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





        private void PreparationStation_PreparationItemsChangeState(IPreparationStationRuntime sender, string deviceUpdateEtag)
        {
            var itemsOnDevice = (from servicePointPreparationItems in ServicePointsPreparationItems
                                 from preparationItem in servicePointPreparationItems.PreparationItems
                                 select new ItemPreparationAbbreviation() { uid = preparationItem.uid, StateTimestamp = preparationItem.StateTimestamp }).ToList();

            var servicePointsPreparationItems = sender.GetPreparationItems(itemsOnDevice, deviceUpdateEtag).ToList();

            var existingPreparationItems = (from servicePointPreparationItems in ServicePointsPreparationItems
                                            from itemPreparation in servicePointPreparationItems.PreparationItems
                                            select itemPreparation).ToList();

            foreach (var updatedItemPreparation in (from servicePointPreparationItems in servicePointsPreparationItems
                                                    from itemPreparation in servicePointPreparationItems.PreparationItems
                                                    select itemPreparation))
            {
                var existingPreparationItem = existingPreparationItems.Where(x => x.uid == updatedItemPreparation.uid).FirstOrDefault();
                if (existingPreparationItem == null)
                {
                    ServicePointsPreparationItems = servicePointsPreparationItems;
                    PreparationItemsLoaded?.Invoke(this);
                    break;
                }
                else if ((existingPreparationItem as ItemPreparation).Update(updatedItemPreparation as ItemPreparation))
                {
                    ServicePointsPreparationItems = servicePointsPreparationItems;
                    PreparationItemsLoaded?.Invoke(this);
                    break;

                }

            }

        }

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string Title
        {

            get => ApplicationSettings.Current.PreparationStationTitle;
            set => ApplicationSettings.Current.PreparationStationTitle = value;
        }



        [HttpVisible]
        [GenerateEventConsumerProxy]
        public event PreparationItemsLoadedHandle PreparationItemsLoaded;


        [HttpVisible]
        public bool IsTagsBarOpen
        {
            get
            {
                return ApplicationSettings.Current.IsTagsBarOpen;
            }
            set
            {
                ApplicationSettings.Current.IsTagsBarOpen = value;
            }
        }

        [HttpVisible]
        public string CommunicationCredentialKey
        {
            get
            {
                return ApplicationSettings.Current.CommunicationCredentialKey;
            }
            set
            {
                ApplicationSettings.Current.CommunicationCredentialKey = value;
            }
        }
#if DeviceDotNet
        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        [HttpVisible]
        public Task<bool> AssignPreparationStation()
        {
            return Task<bool>.Run(async () =>
            {
#if DeviceDotNet
                var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically",true);

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
                    var restaurantMenuDataSharedUri = PreparationStation.RestaurantMenuDataSharedUri;
                    HttpClient httpClient = new HttpClient();
                    var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
                    getJsonTask.Wait();
                    var json = getJsonTask.Result;
                    var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
                    MenuItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.JsonViewModel.MenuFoodItem>>(json, jSetttings).ToDictionary(x => x.Uri);

                    ServicePointsPreparationItems = PreparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>(), null).ToList();
                    CommunicationCredentialKey = communicationCredentialKey;
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



        [HttpVisible]
        public async void AssignCodeCardsToSessions()
        {
#if DeviceDotNet
            var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically",true);
            if (PreparationStation != null)
            {
                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return;
                PreparationStation.AssignCodeCardsToSessions(new List<string>() { result.Text });
            }
#endif

        }



        [HttpVisible]
        public Task<bool> AssignCommunicationCredentialKey(string credentialKey)
        {
            if (CommunicationCredentialKey != credentialKey)
            {
                CommunicationCredentialKey = credentialKey;
                PreparationStation = null;
                return Task<bool>.Run(() =>
                {
                    if (PreparationStation == null && !string.IsNullOrWhiteSpace(CommunicationCredentialKey))
                    {

                        string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                        string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                        string serverUrl = AzureServerUrl;
                        IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                        PreparationStation = servicesContextManagment.GetPreparationStationRuntime(CommunicationCredentialKey);
                        if (PreparationStation != null)
                        {
                            Title = PreparationStation.Description;
                            var restaurantMenuDataSharedUri = PreparationStation.RestaurantMenuDataSharedUri;
                            HttpClient httpClient = new HttpClient();
                            var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
                            getJsonTask.Wait();
                            var json = getJsonTask.Result;
                            var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings;
                            MenuItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.JsonViewModel.MenuFoodItem>>(json, jSetttings).ToDictionary(x => x.Uri);

                            ServicePointsPreparationItems = PreparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>(), null).ToList();
                            return true;
                        }
                        else
                        {
                            Title = "";
                            return false;
                        }


                        //var menuItems = PreparationStation.GetNewerRestaurandMenuData(DateTime.MinValue);
                    }
                    return false;

                });

            }
            return Task<bool>.FromResult(true);
        }


        string lan = "en";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
        public string Language { get { return lan; } }

        string deflan = "en";
        public string DefaultLanguage { get { return deflan; } }



        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();

        public string GetTranslation(string langCountry)
        {
            if (Translations.ContainsKey(langCountry))
                return Translations[langCountry].ToString();
            string json = "{}";
            var assembly = Assembly.GetExecutingAssembly();
            string path = "";


#if DeviceDotNet   
            path = "PreparationStationDevice.i18n";
#else
            path = "PreparationStationDevice.WPF.i18n";
#endif

            string jsonName = assembly.GetManifestResourceNames().Where(x => x.Contains(path) && x.Contains(langCountry + ".json")).FirstOrDefault();
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

        public string AppIdentity => "com.microneme.preparationstationdevice";
    }

    public delegate void PreparationItemsLoadedHandle(FlavoursPreparationStation sender);


}
