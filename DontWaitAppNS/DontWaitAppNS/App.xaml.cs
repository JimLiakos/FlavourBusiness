using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech;
using Plugin.Connectivity;
using OOAdvantech.Pay;
using System.Linq;
using Plugin.Permissions.Abstractions;

namespace DontWaitApp
{
    public partial class App : Application, IAppLifeTime
    {


        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;
        SerializeTaskScheduler IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;

        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();

        public App()
        {
            InitializeComponent();


            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            InitializeRemoteTypes();

            ApplicationSettings.GetCurrent();
            //MainPage = new NavigationPage(new DontWaitApp.HybridWebViewPage());
            MainPage = new NavigationPage(new PaymentPage());










            //MainPage = new MainPage();
        }

        private static void InitializeRemoteTypes()
        {
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.EndUsers.Coordinate"] = typeof(FlavourBusinessFacade.EndUsers.Coordinate);


            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);


            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.EndUsers.Coordinate)] = "FlavourBusinessFacade.EndUsers.Coordinate";


            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";

            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
        }


        protected override async void OnStart()
        {
            await CrossConnectivity.Current.IsRemoteReachable("10.0.0.13");
            SerializeTaskScheduler.RunAsync();
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.IsinSleepMode = false;

            bool permissionsGranted = false;
            if ((await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.ContactsRead>()) != Xamarin.Essentials.PermissionStatus.Granted)
            {
                if ((await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.ContactsRead>()) == Xamarin.Essentials.PermissionStatus.Granted)
                    permissionsGranted = true;
            }
            else
                permissionsGranted = true;

            if (permissionsGranted)
            {
                var contacts = (await Xamarin.Essentials.Contacts.GetAllAsync()).ToList();

               string name = contacts.FirstOrDefault()?.DisplayName;

                var searchingItems = contacts.Select(x => new FuzzySearchItem() { Description = x.DisplayName, Tag = x }).ToList();
                foreach (var searchingItem in searchingItems)
                    searchingItem.Ration = FuzzySharp.Fuzz.PartialRatio(searchingItem.Description.ToUpper(), "Liak".ToUpper());

                searchingItems = searchingItems.OrderByDescending(x => x.Ration).ToList();

                 name = contacts.FirstOrDefault()?.DisplayName;

            }
        }







        protected override void OnSleep()
        {
            try
            {
                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                device.IsinSleepMode = true;
                ApplicationSleeping?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception error)
            {
            }
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            try
            {
                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                device.IsinSleepMode = false;

                ApplicationResuming?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception error)
            {
            }

            // Handle when your app resumes
        }



    }

    public class FuzzySearchItem
    {
        public string Description { get; set; }

        public int Ration { get; set; }

        public object Tag { get; set; }
    }
}
