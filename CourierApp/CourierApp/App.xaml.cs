using OOAdvantech;
using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourierApp
{
    /// <MetaDataID>{fb79f3c9-89fe-4efa-b3ed-73078ac2b894}</MetaDataID>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        /// <MetaDataID>{f2aa7a20-355b-4754-a588-cfb9d23250e1}</MetaDataID>
        public App()
        {
            try
            {
                string text = DeviceApplication.Current.ReadLog();
                DeviceApplication.Current.ClearLog();
                OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string> { "OnStart" });
            }
            catch
            {
                // just suppress any error logging exceptions
            }
            App.SerializeTaskScheduler.RunAsync();

            InitializeComponent();
            var navigationPage = new NavigationPage(new MainPage());
            MainPage = navigationPage;
            //navigationPage.BarBackgroundColor = Color.Black;
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            SerializationBinder.NamesTypesDictionary["CourierApp.ViewModel.FoodShippingPresentation"] = typeof(global::CourierApp.ViewModel.FoodShippingPresentation);


            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);
            //SerializationBinder.NamesTypesDictionary["MenuModel.MealType"] = typeof(MenuModel.MealType);
            //SerializationBinder.NamesTypesDictionary["MenuModel.FixedMealType"] = typeof(MenuModel.FixedMealType);
            //SerializationBinder.NamesTypesDictionary["MenuModel.MealCourseType"] = typeof(MenuModel.MealCourseType);


            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.EndUsers.Coordinate"] = typeof(FlavourBusinessFacade.EndUsers.Coordinate);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation"] = typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation);
            //SerializationBinder.NamesTypesDictionary["TakeAwayApp.ViewModel.WatchingOrderPresentation"] = typeof(global::TakeAwayApp.ViewModel.WatchingOrderPresentation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ViewModel.MealCourse"] = typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.Shipping.ReturnReason"] = typeof(FlavourBusinessFacade.Shipping.ReturnReason);






            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealType)] = "MenuModel.MealType";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.FixedMealType)] = "MenuModel.FixedMealType";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.MealCourseType)] = "MenuModel.MealCourseType";
            SerializationBinder.TypesNamesDictionary[typeof(global::CourierApp.ViewModel.FoodShippingPresentation)] = "CourierApp.ViewModel.FoodShippingPresentation";




            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.EndUsers.Coordinate)] = "FlavourBusinessFacade.EndUsers.Coordinate";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation)] = "FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.Shipping.ReturnReason)] = "FlavourBusinessFacade.Shipping.ReturnReason";


            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse)] = "FlavourBusinessManager.RoomService.ViewModel.MealCourse";




            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
        }

        /// <MetaDataID>{cafbaedf-880a-4314-971b-3a8796e365a3}</MetaDataID>
        protected override void OnStart()
        {

        }

        /// <MetaDataID>{b759c930-dff3-4bfc-a91c-038b6fb30d15}</MetaDataID>
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
        }

        /// <MetaDataID>{646d5d92-8d90-4ee3-9dae-6bc9fb5d590a}</MetaDataID>
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
        }

        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;

        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;

    }
}
