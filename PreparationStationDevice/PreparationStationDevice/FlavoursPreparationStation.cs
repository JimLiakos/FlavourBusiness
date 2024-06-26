﻿using System;
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
using MenuModel;
using MenuModel.JsonViewModel;
using System.IO;
using System.Xml;
using System.Net.Sockets;
using FlavourBusinessFacade.Print;
using System.IO.Ports;
using static Google.Apis.Requests.BatchRequest;
using static FlavourBusinessFacade.Printing.Printer;
using FlavourBusinessFacade.Printing;
using PreparationStationDevice.Printing;



#if DeviceDotNet
using ThermalDotNet;
using OOAdvantech.AudioManager;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
#endif

namespace PreparationStationDevice
{
    /// <MetaDataID>{293c7b92-a89a-4179-a8ff-616948355d82}</MetaDataID>
    public class FlavoursPreparationStation : MarshalByRefObject, IFlavoursPreparationStation, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ViewModel.ILocalization
    {
        /// <MetaDataID>{2af62a83-38f3-4b6e-86db-0d68fd81a54c}</MetaDataID>
        public FlavoursPreparationStation()
        {
            var communicationCredentialKey = this.CommunicationCredentialKey;
            PreparationVelocity = 0;

        }



        //static string AzureServerUrl = "http://localhost:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.5:8090/api/";
        //static strinb AzureServerUrl = "http://192.168.2.10:8090/api/";

        //static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work
        /// <MetaDataID>{4db24938-b8ff-4a2b-b39e-3f1fb62bdc79}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);


        /// <MetaDataID>{df0874e4-2da9-4fb0-9aa4-b9afc9d10975}</MetaDataID>
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



        /// <MetaDataID>{d4bde2dc-678e-4902-b585-31626c7b26d5}</MetaDataID>
        IPreparationStationRuntime _PreparationStation;
        /// <MetaDataID>{97918366-fc8f-40e0-8ae0-050b408b7b6a}</MetaDataID>
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

        /// <MetaDataID>{2f4d59b4-2f15-49ac-bf0a-7842f64c0276}</MetaDataID>
        private void PreparationStation_Reconnected(object sender)
        {


        }



        /// <MetaDataID>{c109d672-0fb4-4aff-867d-70e329fd0496}</MetaDataID>
        Dictionary<string, MenuModel.JsonViewModel.MenuFoodItem> MenuItems;

        /// <MetaDataID>{073297e1-b8c8-4f8c-948b-77ff8992884a}</MetaDataID>
        List<FlavourBusinessFacade.RoomService.ItemsPreparationContext> ItemsPreparationContexts = new List<FlavourBusinessFacade.RoomService.ItemsPreparationContext>();
        List<ItemsPreparationContextPresentation> ItemsPreparationContextPresentations;
        /// <MetaDataID>{a0fcee0a-6b90-42e8-8d55-531586e74dd6}</MetaDataID>
        [HttpVisible]
        public Task<List<ItemsPreparationContextPresentation>> WebUIAttached()
        {


            return Task<List<ItemsPreparationContextPresentation>>.Run(() =>
           {
               int tries = 30;
               while (tries > 0)
               {
                   try
                   {

                       if (PreparationStation == null && !string.IsNullOrWhiteSpace(CommunicationCredentialKey))
                       {
                           var start = DateTime.UtcNow;

                           string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                           string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                           string serverUrl = AzureServerUrl;
                           IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                           var preparationStation = servicesContextManagment.GetPreparationStationRuntime(CommunicationCredentialKey);

                           if (!String.IsNullOrWhiteSpace(PrintManagerDeviceCredentialKey))
                           {

                             var printManager = servicesContextManagment.GetPrintManager(PrintManagerDeviceCredentialKey);

                               if (printManager != null)
                               {
                                   PrintingService = new Printing.PrintingLocalService(printManager);
                                   PrintingService.StartPrintingService();
                               }

                           }


                           if (preparationStation != null)
                           {

                               PreparationStationStatus preparationStationStatus = preparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>(), null);

                               ItemsPreparationTags = preparationStation.ItemsPreparationTags;
                               preparationStation.ObjectChangeState += PreparationStation_ObjectChangeState;


                               //StartPrintingService();

                               var restaurantMenuDataSharedUri = preparationStation.RestaurantMenuDataSharedUri;
                               HttpClient httpClient = new HttpClient();
                               var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
                               getJsonTask.Wait();
                               var json = getJsonTask.Result;
                               var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JSonSerializeSettings.TypeRefDeserializeSettings;
                               MenuItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<MenuModel.JsonViewModel.MenuFoodItem>>(json, jSetttings).ToDictionary(x => x.Uri);

                               GetMenuLanguages(MenuItems.Values.ToList());
                               // servicesContextManagment.
                               Title = preparationStation.Description;

                               preparationStation.PreparationItemsChangeState += PreparationStation_PreparationItemsChangeState;
                               PreparationStation = preparationStation;
                               ItemsPreparationContexts = preparationStationStatus.NewItemsUnderPreparationControl.ToList();
                               ServingTimeSpanPredictions = preparationStationStatus.ServingTimespanPredictions;
                               PreparationVelocity = PreparationStation.PreparationVelocity;
                           }

                           var timeSpan = DateTime.UtcNow - start;
                           //var menuItems = PreparationStation.GetNewerRestaurandMenuData(DateTime.MinValue);
                       }

                       var itemsPreparationContexts = (from itemsPreparationContext in ItemsPreparationContexts
                                                       select new ItemsPreparationContextPresentation()
                                                       {
                                                           Description = itemsPreparationContext.MealCourseDescription,
                                                           StartsAt = itemsPreparationContext.MealCourseStartsAt,
                                                           MustBeServedAt = itemsPreparationContext.ServedAtForecast,
                                                           PreparationOrder = itemsPreparationContext.PreparationOrder,
                                                           ServicesContextIdentity = itemsPreparationContext.ServicePoint.ServicesContextIdentity,
                                                           ServicesPointIdentity = itemsPreparationContext.ServicePoint.ServicesPointIdentity,
                                                           Uri = itemsPreparationContext.Uri,
                                                           PreparationItems = itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().OrderByDescending(x => x.CookingTimeSpanInMin).Select(x => new PreparationStationItem(x, itemsPreparationContext, MenuItems, ItemsPreparationTags)).OrderBy(x => x.AppearanceOrder).ToList()
                                                       }).ToList();

                       if (ItemsPreparationContextPresentations == null || ItemsPreparationContextPresentations.Count == 0)
                       {
                           List<ItemPreparation> preparationItems = (from preparationSection in itemsPreparationContexts
                                                                     from preparationItem in preparationSection.PreparationItems
                                                                     select preparationItem.ItemPreparation).ToList();

                           if (preparationItems.All(x => x.State.IsInTheSameOrPreviousState(ItemPreparationState.PendingPreparation)) && preparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation))
                           {
                               Task.Run(() =>
                               {
                                   TimeSpan delay = TimeSpan.FromMinutes(1);
                                   int count = 0;
                                   while (true)
                                   {
                                       System.Threading.Thread.Sleep(delay);
                                       preparationItems = (from preparationSection in ItemsPreparationContextPresentations
                                                           from preparationItem in preparationSection.PreparationItems
                                                           select preparationItem.ItemPreparation).ToList();
                                       if (count < 3)
                                           delay = TimeSpan.FromMinutes(0.5);
                                       else
                                           delay = TimeSpan.FromSeconds(10);
                                       count++;
                                       if (preparationItems.All(x => x.State.IsInTheSameOrPreviousState(ItemPreparationState.PendingPreparation)) && preparationItems.Any(x => x.State == ItemPreparationState.PendingPreparation))
                                           PlayNotificationSound();
                                       else
                                           break;
                                   }

                               });
                           }
                       }

                       ItemsPreparationContextPresentations = itemsPreparationContexts;

                       return ItemsPreparationContextPresentations;
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

        public async void PlayNotificationSound()
        {

#if DeviceDotNet
            await Audio.Manager.PlaySound("NotificationSound.mp3");
#else
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(FlavourBusinessApps.Properties.Resources.NotificationSound);
            player.Play();
#endif

        }

        /// <MetaDataID>{4fbed955-14b7-4406-a861-844e2e9398d8}</MetaDataID>
        [HttpVisible]
        public Dictionary<string, ItemPreparationPlan> ServingTimeSpanPredictions { get; private set; }

        /// <MetaDataID>{5d39c9da-1f97-4c79-9da3-91164954e065}</MetaDataID>
        [HttpVisible]
        public List<Language> MenuLanguages { get; set; }
        /// <MetaDataID>{2aea780c-1323-4875-b3ce-e4b8c1c4335c}</MetaDataID>
        private void GetMenuLanguages(List<MenuFoodItem> menuItems)
        {
            List<Language> menuLanguages = new List<Language>();
            foreach (string langCode in (from menuItem in menuItems
                                         from multiigualNameValue in menuItem.MultilingualName.Values
                                         select multiigualNameValue.Key).Distinct())
            {
                menuLanguages.Add(new Language()
                {
                    Code = langCode,
                    Name = System.Globalization.CultureInfo.GetCultureInfo(langCode).DisplayName
                });
            }
            MenuLanguages = menuLanguages;
        }

        /// <MetaDataID>{c9a6ca4c-d3a5-40a3-b327-75995353dd5e}</MetaDataID>
        private void PreparationStation_ObjectChangeState(object _object, string member)
        {

            if (member == nameof(IPreparationStationRuntime.ItemsPreparationTags))
            {
                ItemsPreparationTags = PreparationStation.ItemsPreparationTags;

                PreparationItemsLoaded?.Invoke(this);
            }
        }

        /// <MetaDataID>{ee48802e-749d-440f-b1fb-ec854149343c}</MetaDataID>
        [HttpVisible]
        public void CancelLastPreparationStep(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ItemsPreparationContexts
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
                        ServingTimeSpanPredictions = PreparationStation.CancelLastPreparationStep(itemPreparations.Select(x => x.uid).ToList());
                        PreparationVelocity = PreparationStation.PreparationVelocity;
                        ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));
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
        /// <MetaDataID>{979ce1c7-d464-4952-87a3-cefa62ee6e32}</MetaDataID>
        [HttpVisible]
        public void ItemsPrepared(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ItemsPreparationContexts
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
                        ServingTimeSpanPredictions = PreparationStation.ItemsPrepared(itemPreparations.Select(x => x.uid).ToList());
                        PreparationVelocity = PreparationStation.PreparationVelocity;

                        UpdatePreparationItems(itemPreparations);

                        ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));
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

        object PreparationItemsLock = new object();

        private void UpdatePreparationItems(List<ItemPreparation> itemPreparations)
        {
            lock (PreparationItemsLock)
            {
                List<ItemPreparation> preparationItems = (from preparationSection in ItemsPreparationContextPresentations
                                                          from preparationItem in preparationSection.PreparationItems
                                                          select preparationItem.ItemPreparation).ToList();
                foreach (var itemPreparation in itemPreparations)
                    preparationItems.Where(x => x.uid == itemPreparation.uid).FirstOrDefault().Update(itemPreparation);
            }
        }

        /// <MetaDataID>{c92e564e-3b00-4b10-a079-6a55c5eb464a}</MetaDataID>
        [HttpVisible]
        public void ItemsServing(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ItemsPreparationContexts
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
                        ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));
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
        public bool IsPrintingsAgentDevice
        {
            get
            {
                return PrintingService != null;
            }
        }


        /// <MetaDataID>{61a28e77-843c-4ce1-b1eb-ebe2ad554237}</MetaDataID>
        [HttpVisible]
        public void ItemsΙnPreparation(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ItemsPreparationContexts
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
                        ServingTimeSpanPredictions = PreparationStation.ItemsInPreparation(itemPreparations.Select(x => x.uid).ToList());
                        PreparationVelocity = PreparationStation.PreparationVelocity;

                        UpdatePreparationItems(itemPreparations);

                        ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));
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

        /// <MetaDataID>{ba6f5bfb-e8ba-4f3e-a038-1f61b7397060}</MetaDataID>
        [HttpVisible]
        public double PreparationVelocity
        {
            get;
            private set;
        }


        /// <MetaDataID>{546fe7e2-28ec-4719-836b-9219920982d7}</MetaDataID>
        [HttpVisible]
        public void ItemsRoasting(List<ItemPreparation> itemPreparations)
        {
            var itemPreparationsDictionary = (from servicePointPreparationItems in ItemsPreparationContexts
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
                        ServingTimeSpanPredictions = PreparationStation.ItemsRoasting(itemPreparations.Select(x => x.uid).ToList());
                        PreparationVelocity = PreparationStation.PreparationVelocity;

                        UpdatePreparationItems(itemPreparations);

                        ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));
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





        /// <MetaDataID>{a5bdfc4f-d74e-43ce-b533-0bea4c977f34}</MetaDataID>
        private void PreparationStation_PreparationItemsChangeState(IPreparationStationRuntime sender, string deviceUpdateEtag)
        {
            var itemsOnDevice = (from servicePointPreparationItems in ItemsPreparationContexts
                                 from preparationItem in servicePointPreparationItems.PreparationItems
                                 select new ItemPreparationAbbreviation() { uid = preparationItem.uid, StateTimestamp = preparationItem.StateTimestamp }).ToList();


            PreparationStationStatus preparationStationStatus = null;
            int tries = 30;
            while (tries > 0)
            {
                try
                {

                    preparationStationStatus = PreparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>(), deviceUpdateEtag);
                    break;
                }

                catch (Exception error)
                {
                    tries--;
                    if (tries == 0)
                        throw;
                }
            }
            var servicePointsPreparationItems = preparationStationStatus.NewItemsUnderPreparationControl.ToList();
            ServingTimeSpanPredictions = preparationStationStatus.ServingTimespanPredictions;
            PreparationVelocity = PreparationStation.PreparationVelocity;

            var existingPreparationItems = (from servicePointPreparationItems in ItemsPreparationContexts
                                            from itemPreparation in servicePointPreparationItems.PreparationItems
                                            select itemPreparation).ToList();

            foreach (var updatedItemPreparation in (from servicePointPreparationItems in servicePointsPreparationItems
                                                    from itemPreparation in servicePointPreparationItems.PreparationItems
                                                    select itemPreparation))
            {
                var existingPreparationItem = existingPreparationItems.Where(x => x.uid == updatedItemPreparation.uid).FirstOrDefault();
                if (existingPreparationItem == null)
                {
                    ItemsPreparationContexts = servicePointsPreparationItems;
                    PreparationItemsLoaded?.Invoke(this);
                    ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));
                    break;
                }
                else if ((existingPreparationItem as ItemPreparation).Update(updatedItemPreparation as ItemPreparation))
                {
                    ItemsPreparationContexts = servicePointsPreparationItems;
                    PreparationItemsLoaded?.Invoke(this);
                    ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));
                    break;
                }
            }
            var updatedPreparationItems = (from servicePointPreparationItems in servicePointsPreparationItems
                                           from itemPreparation in servicePointPreparationItems.PreparationItems
                                           select itemPreparation).ToList();


            foreach (var existingPreparationItem in existingPreparationItems)
            {
                var updatedPreparationItem = updatedPreparationItems.Where(x => x.uid == existingPreparationItem.uid).FirstOrDefault();
                if (updatedPreparationItem == null)
                    (from itemsPreparationContext in ItemsPreparationContexts
                     from preparationItem in itemsPreparationContext.PreparationItems
                     where preparationItem == existingPreparationItem
                     select itemsPreparationContext).FirstOrDefault()?.RemovePreparationItem(existingPreparationItem);
            }
            PreparationItemsLoaded?.Invoke(this);
            ObjectChangeState?.Invoke(this, nameof(ServingTimeSpanPredictions));

        }

        /// <MetaDataID>{e21edb31-648a-4b63-b5a6-7dec0572b963}</MetaDataID>
        [HttpVisible]
        public string Title
        {

            get => ApplicationSettings.Current.PreparationStationTitle;
            set => ApplicationSettings.Current.PreparationStationTitle = value;
        }

        /// <summary>
        /// Defines the time interval between the first item forecast preparation time and the last item of group
        /// Food preparer can filter the product where displayed on screen to decide if worth to prepare all in session.
        /// If the food preparer uses the tags to group products, 
        /// the time interval between the preparation time of the first prediction and the last product must not exceed the GroupingTimeSpan.
        /// Products with a predicted preparation time that is outside the GroupingTimeSpan are not displayed.
        /// </summary>
        /// <MetaDataID>{01b0f950-c529-4779-aa83-756893c3a356}</MetaDataID>
        [HttpVisible]
        public double GroupingTimeSpan
        {


            get
            {
                if (PreparationStation == null)
                    return 0;
                return RemotingServices.CastTransparentProxy<IPreparationStation>(PreparationStation).GroupingTimeSpan;
            }
            set
            {
                if (PreparationStation != null)
                    RemotingServices.CastTransparentProxy<IPreparationStation>(PreparationStation).GroupingTimeSpan = value;
            }
        }


        [HttpVisible]
        [GenerateEventConsumerProxy]
        public event PreparationItemsLoadedHandle PreparationItemsLoaded;

        [HttpVisible]
        [GenerateEventConsumerProxy]
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{81a445b5-d491-4c55-a274-d89478cf0cf8}</MetaDataID>
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

        /// <MetaDataID>{e0095b8c-2e90-48e0-a6b7-f14dbf63e6f9}</MetaDataID>
        [HttpVisible]
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
        public string PrintManagerDeviceCredentialKey
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.PrintManagerDeviceCredentialKey))
                {

                }
                return ApplicationSettings.Current.PrintManagerDeviceCredentialKey;
            }
            set
            {
                ApplicationSettings.Current.PrintManagerDeviceCredentialKey = value;
            }
        }


#if DeviceDotNet
        public DeviceUtilities.NetStandard.ScanCode ScanCode = new DeviceUtilities.NetStandard.ScanCode();
#endif
        /// <MetaDataID>{a25fca78-e27e-48c3-ba70-e7f548de56dc}</MetaDataID>
        [HttpVisible]
        public Task<bool> AssignPreparationStation(bool useFrontCameraIfAvailable)
        {
            return Task<bool>.Run(async () =>
            {
#if DeviceDotNet
                var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically", useFrontCameraIfAvailable);

                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return false;
                string communicationCredentialKey = "7f9bde62e6da45dc8c5661ee2220a7b0_fff069bc4ede44d9a1f08b5f998e02ad";
                //communicationCredentialKey =result.Text;


               return await AssignCommunicationCredentialKey(communicationCredentialKey);
                //string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                //string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                //string serverUrl = AzureServerUrl;
                //IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
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
                //                                                PreparationOrder = itemsPreparationContext.PreparationOrder,
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



        /// <MetaDataID>{e15cc82a-c28a-4d01-93d8-1355926bc49b}</MetaDataID>
        [HttpVisible]
        public async void AssignCodeCardsToSessions()
        {
#if DeviceDotNet
            var result = await ScanCode.Scan("Hold your phone up to the place Identity", "Scanning will happen automatically", true);
            if (PreparationStation != null)
            {
                if (result == null || string.IsNullOrWhiteSpace(result.Text))
                    return;
                PreparationStation.AssignCodeCardsToSessions(new List<string>() { result.Text });
            }
#endif

        }



        /// <MetaDataID>{cc507cb9-818f-499e-af37-000c110c38ce}</MetaDataID>
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
                        IFlavoursServicesContextManagment servicesContextManagement = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                        PreparationStation = servicesContextManagement.GetPreparationStationRuntime(CommunicationCredentialKey);
                        if (PreparationStation != null)
                        {

                            Title = PreparationStation.Description;
                            ItemsPreparationTags = PreparationStation.ItemsPreparationTags;
                            PreparationStation.ObjectChangeState += PreparationStation_ObjectChangeState;

#if DeviceDotNet
                            StartPrintingService();
#endif


                            var restaurantMenuDataSharedUri = PreparationStation.RestaurantMenuDataSharedUri;
                            HttpClient httpClient = new HttpClient();
                            var getJsonTask = httpClient.GetStringAsync(restaurantMenuDataSharedUri);
                            getJsonTask.Wait();
                            var json = getJsonTask.Result;
                            var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JSonSerializeSettings.TypeRefDeserializeSettings;
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
                                                                        PreparationOrder = itemsPreparationContext.PreparationOrder,
                                                                        ServicesContextIdentity = itemsPreparationContext.ServicePoint.ServicesContextIdentity,
                                                                        ServicesPointIdentity = itemsPreparationContext.ServicePoint.ServicesPointIdentity,
                                                                        Uri = itemsPreparationContext.Uri,
                                                                        PreparationItems = itemsPreparationContext.PreparationItems.OfType<ItemPreparation>().OrderByDescending(x => x.CookingTimeSpanInMin).Select(x => new PreparationStationItem(x, itemsPreparationContext, MenuItems, ItemsPreparationTags)).OrderBy(x => x.AppearanceOrder).ToList()
                                                                    }).ToList();

                            return true;
                        }
                        else
                        {

                            try
                            {
                                string printManagerDeviceCredentialKey = servicesContextManagement.AssignPrintManagerDevice(CommunicationCredentialKey);
                                if (!string.IsNullOrWhiteSpace(printManagerDeviceCredentialKey))
                                {
                                    PrintManagerDeviceCredentialKey = printManagerDeviceCredentialKey;

                                    var printManager = servicesContextManagement.GetPrintManager(PrintManagerDeviceCredentialKey);
                                    if (printManager != null)
                                    {
                                        PrintingService = new Printing.PrintingLocalService(printManager);
                                        PrintingService.StartPrintingService();
                                    }



                                    Title = "Printers capture";
                                    return true;
                                }
                            }
                            catch (Exception error)
                            {

                            }


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


        /// <MetaDataID>{92f61aff-068a-4fc6-b225-a1c89f54fc35}</MetaDataID>
        string lan = "en";// OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
        /// <MetaDataID>{29cef568-8c44-4623-a159-5c81d7ca6510}</MetaDataID>
        public string Language { get { return lan; } }

        /// <MetaDataID>{15bd554d-dd21-4bb4-b64a-98f9c0212193}</MetaDataID>
        string deflan = "en";
        /// <MetaDataID>{af952168-63e2-44f8-8af6-644791c4990d}</MetaDataID>
        public string DefaultLanguage { get { return deflan; } }




        /// <MetaDataID>{1194b00f-e17d-4fca-939c-d1f68047a881}</MetaDataID>
        Dictionary<string, JObject> Translations = new Dictionary<string, JObject>();
        /// <MetaDataID>{0c6e1a0f-857f-4140-af23-c0e67941d2d1}</MetaDataID>
        private Dictionary<string, List<ITag>> ItemsPreparationTags;

        /// <MetaDataID>{c877da51-bd70-481e-9ccf-faf73b0beea9}</MetaDataID>
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


        /// <MetaDataID>{91bdbecf-c1d6-483b-b3ac-08880134ac1c}</MetaDataID>
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


        /// <MetaDataID>{77fa91cc-1678-4a1d-a4af-09d9557447d4}</MetaDataID>
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

        /// <MetaDataID>{543f6e31-794b-4b02-b698-64a8eaabce1c}</MetaDataID>
        public string AppIdentity => "com.microneme.preparationstationdevice";

        public bool RoastingAudibleAlert { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <exclude>Excluded</exclude>
        bool _RoastingAlert;
        public bool RoastingAlert
        {
            get => _RoastingAlert;
            set
            {
                _RoastingAlert = value;
                if (_RoastingAlert)
                {
                    Task.Run(() =>
                    {
                        TimeSpan delay = TimeSpan.FromMinutes(1);
                        int count = 0;
                        while (true)
                        {

                            delay = TimeSpan.FromSeconds(2);
                            PlayNotificationSound();
                            count++;
                            if (count >= 2)
                                return;

                            System.Threading.Thread.Sleep(delay);
                        }

                    });
                }
            }
        }

        public static bool TerminatePrintingWatcher { get; private set; }
        //public IPrintManager PrintManager { get; private set; }
        public Task PrintingServiceTask { get; private set; }
        internal PrintingLocalService PrintingService { get; private set; }
    }

    public delegate void PreparationItemsLoadedHandle(FlavoursPreparationStation sender);


    /// <MetaDataID>{7b4504d8-bb4c-4b28-b953-1f01d996289d}</MetaDataID>
    public class Language
    {  
        /// <MetaDataID>{6b6675fa-7d7a-46b6-923c-7cd0a0756207}</MetaDataID>
        public string Code { get; set; }

        /// <MetaDataID>{b79763c7-a6dc-4eb8-9420-f62ac7ae3b56}</MetaDataID>
        public string Name { get; set; }
    }

    /// <MetaDataID>{1a4833b0-ba26-4250-9c97-c71d2594dfe0}</MetaDataID>
    [HttpVisible]
    public interface IFlavoursPreparationStation
    {
        /// <MetaDataID>{3941cdf8-78c4-4204-97e0-45b8cf59be34}</MetaDataID>
        Task<List<ItemsPreparationContextPresentation>> WebUIAttached();
        /// <MetaDataID>{c80cb0b1-f3cc-4450-94b5-950aae5c26d1}</MetaDataID>
        Dictionary<string, ItemPreparationPlan> ServingTimeSpanPredictions { get; }
        /// <MetaDataID>{2d081f08-3e89-470a-be72-fc94d4968672}</MetaDataID>
        List<Language> MenuLanguages { get; set; }
        /// <MetaDataID>{e43808f3-55e7-447e-87fb-7c2e0912345a}</MetaDataID>
        void CancelLastPreparationStep(List<ItemPreparation> itemPreparations);
        /// <MetaDataID>{ba7351f9-1934-4595-858e-6e8e9488325c}</MetaDataID>
        void ItemsPrepared(List<ItemPreparation> itemPreparations);
        /// <MetaDataID>{5b82fa46-65d9-4fbe-b641-255a35d67d2c}</MetaDataID>
        void ItemsServing(List<ItemPreparation> itemPreparations);
        /// <MetaDataID>{f4048772-fc00-4bb3-9d59-afcf10436db5}</MetaDataID>
        void ItemsΙnPreparation(List<ItemPreparation> itemPreparations);


        bool IsPrintingsAgentDevice { get; }


        bool RoastingAudibleAlert { get; set; }

        bool RoastingAlert { get; set; }
        [GenerateEventConsumerProxy]
        event PreparationItemsLoadedHandle PreparationItemsLoaded;

        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{47cebf11-34d5-4dad-b77d-d6afea1d8fe2}</MetaDataID>
        bool IsTagsBarOpen { get; set; }
        /// <MetaDataID>{593ec5d1-3d26-472e-8f5e-388692fcc5b5}</MetaDataID>
        string CommunicationCredentialKey { get; set; }
        /// <MetaDataID>{cadaed7d-eb35-46fe-beee-565ee3917007}</MetaDataID>
        void AssignCodeCardsToSessions();
        /// <MetaDataID>{a0102c12-a590-4227-9038-8fe337493b94}</MetaDataID>
        Task<bool> AssignCommunicationCredentialKey(string credentialKey);

        /// <MetaDataID>{bd482c6d-8458-43cb-b050-87dcab4da02c}</MetaDataID>
        double PreparationVelocity { get; }

        void PlayNotificationSound();
    }


}
