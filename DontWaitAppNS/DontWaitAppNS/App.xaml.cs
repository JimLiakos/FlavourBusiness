using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech;
using Plugin.Connectivity;
using OOAdvantech.Pay;
using System.Linq;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

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
            MainPage = new NavigationPage(new DontWaitApp.HybridWebViewPage());
            //MainPage = new NavigationPage(new PaymentPage());










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

            SerializationBinder.NamesTypesDictionary["FinanceFacade.IPayment"] = typeof(FinanceFacade.IPayment);
            SerializationBinder.NamesTypesDictionary["FinanceFacade.IItem"] = typeof(FinanceFacade.IItem);
            SerializationBinder.NamesTypesDictionary["FinanceFacade.Item"] = typeof(FinanceFacade.Item);



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

            SerializationBinder.TypesNamesDictionary[typeof(FinanceFacade.IPayment)] = "FinanceFacade.IPayment";
            SerializationBinder.TypesNamesDictionary[typeof(FinanceFacade.IItem)] = "FinanceFacade.IItem";
            SerializationBinder.TypesNamesDictionary[typeof(FinanceFacade.Item)] = "FinanceFacade.Item";


        }


        protected override async void OnStart()
        {
            await CrossConnectivity.Current.IsRemoteReachable("10.0.0.13");
            SerializeTaskScheduler.RunAsync();
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.IsinSleepMode = false;
            //OnAppLinkRequestReceived(new Uri("http://192.168.2.8:4300/#/launch-app?mealInvitation=True&sc=7f9bde62e6da45dc8c5661ee2220a7b0&sp=fe51ba7e30954ee08209bd89a03469a8&cs=6126a9565db94a88ade1e604172a683b"));


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
                try
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current?.MainPage?.DisplayAlert("App", "Application Resume", "")
                        // Code to run on the main thread
                    });

                }
                catch (Exception error)
                {
                    
                }
                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                device.IsinSleepMode = false;

                ApplicationResuming?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception error)
            {
            }

            // Handle when your app resumes
        }

        public async void iOSOnAppLinkRequestReceived(Uri uri)
        {
            OnAppLinkRequestReceived(uri);
        }
        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);


            int queryStartPos = uri.OriginalString.IndexOf("?");
            if (queryStartPos != -1)
            {
                string query = uri.OriginalString.Substring(queryStartPos + 1);
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var parameters = System.Web.HttpUtility.ParseQueryString(query);

                    if (parameters.Get("mealInvitation") != null && parameters.Get("mealInvitation").ToLower() == "true")
                    {
                        string serviceContextIdentity = parameters.Get("sc");
                        string servicePointIdentity = parameters.Get("sp");
                        string clientSessionIdentity = parameters.Get("cs");

                        //string message= string.Format("GetFriendlyNameCalled:{0}  PartOfMealRequestEventAdded:{1}", (((MainPage as NavigationPage)?.CurrentPage as HybridWebViewPage).BindingContext as FlavoursOrderServer).GetFriendlyNameCalled, (((MainPage as NavigationPage)?.CurrentPage as HybridWebViewPage).BindingContext as FlavoursOrderServer).PartOfMealRequestEventAdded);
                        //MainPage.DisplayAlert("message", "mealInvitation", "OK");
                        //if((((MainPage as NavigationPage)?.CurrentPage as HybridWebViewPage).BindingContext as FlavoursOrderServer)==null)
                        //    MainPage.DisplayAlert("message", "FlavoursOrderServer==Null", "OK");
                        //else
                        //    MainPage.DisplayAlert("message", "FlavoursOrderServer","OK");

                        try
                        {
                            await Task.Run(async () =>
                            {
                                bool invitationDelevered = false;
                                do
                                {
                                    invitationDelevered = await Xamarin.Essentials.MainThread.InvokeOnMainThreadAsync<bool>(async () =>
                                    {
                                        try
                                        {
                                            if ((((MainPage as NavigationPage)?.CurrentPage as HybridWebViewPage)?.BindingContext as FlavoursOrderServer) != null&&
                                            (((MainPage as NavigationPage)?.CurrentPage as HybridWebViewPage)?.BindingContext as FlavoursOrderServer).HasPartOfMealRequestSubscribers)
                                            {
                                                invitationDelevered = true;
                                                (((MainPage as NavigationPage)?.CurrentPage as HybridWebViewPage).BindingContext as FlavoursOrderServer).ImplicitMealInvitation(serviceContextIdentity, servicePointIdentity, clientSessionIdentity);
                                                return true;
                                            }
                                            return false;
                                        }
                                        catch (Exception error)
                                        {
                                            
                                            return false;
                                        }
                                    });
                                    if (!invitationDelevered)
                                        System.Threading.Thread.Sleep(1000);

                                } while (!invitationDelevered);
                            });

                        }
                        catch (Exception error)
                        {

                            MainPage.DisplayAlert("message", error.Message, "OK");
                        }

                    }
                }

            }




            //System.Web.HttpUtility.ParseQueryString()
            //MainPage.DisplayAlert("message", uri.AbsoluteUri, "OK");

        }

    }

    public class FuzzySearchItem
    {
        public string Description { get; set; }

        public int Ration { get; set; }

        public string Id { get; set; }

        public object Tag { get; set; }

    }
}
