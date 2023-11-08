using OOAdvantech;
using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WaiterApp
{
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public App()
        {
            var eee = DeviceApplication.Current.ReadLog();

            DeviceApplication.Current.ClearLog();

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);

            SerializationBinder.NamesTypesDictionary["WaiterApp.ViewModel.ServingBatchPresentation"] = typeof(global::WaiterApp.ViewModel.ServingBatchPresentation);


            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);

            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);


            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";

            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";

            SerializationBinder.TypesNamesDictionary[typeof(global::WaiterApp.ViewModel.ServingBatchPresentation)] = "WaiterApp.ViewModel.ServingBatchPresentation";


            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";

            SerializeTaskScheduler.RunAsync();
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.IsinSleepMode = false;
            //MainPage = new FacebookSignIn();
            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage());
        }

        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;


        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;
        protected override async void OnStart()
        {


            

            SerializeTaskScheduler.RunAsync();
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.IsinSleepMode = false;


            using (HttpClient wc = new HttpClient())
            {
                try
                {

                    var resp = await wc.GetStringAsync(@"http://192.168.2.8:8090/api/values");
                    System.Diagnostics.Debug.WriteLine("sds");
                }
                catch (Exception error)
                {
                    //                    { System.Net.Http.HttpRequestException: The Internet connection appears to be offline. --->Foundation.NSErrorException: Error Domain = NSURLErrorDomain Code = -1009 "The Internet connection appears to be offline." UserInfo ={ _kCFStreamErrorCodeKey = 50, NSUnderlyingError = 0x281d35470 { Error Domain = kCFErrorDomainCFNetwork Code = -1009 "(null)" UserInfo ={ _NSURLErrorNWPathKey = unsatisfied(Local network prohibited), interface: en0, ipv4, _kCFStreamErrorCodeKey=50, _kCFStreamErrorDomainKey=1}
                    //}, _NSURLErrorFailingURLSessionTaskErrorKey = LocalDataTask < 6CF91F8F - 33F5 - 4F6B - B500 - D05CBEB3404D >.< 1 >, _NSURLErrorRelatedURLSessionTaskErrorKey = (\n "LocalDataTask <6CF91F8F-33F5-4F6B-B500-D05CBEB3404D>.<1>"\n), NSLocalizedDescription = The Internet connection appears to be offline., NSErrorFailingURLStringKey = http://10.0.0.13:8090/api/values, NSErrorFailingURLKey=http://10.0.0.13:8090/api/values, _kCFStreamErrorDomainKey=1}\n   --- End of inner exception stack trace ---\n  at System.Net.Http.NSUrlSessionHandler.SendAsync (System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) [0x001ad] in /Library/Frameworks/Xamarin.iOS.framework/Versions/16.0.0.75/src/Xamarin.iOS/Foundation/NSUrlSessionHandler.cs:529 \n  at System.Net.Http.HttpClient.FinishSendAsyncUnbuffered (System.Threading.Tasks.Task`1[TResult] sendTask, System.Net.Http.HttpRequestMessage request, System.Threading.CancellationTokenSource cts, System.Boolean disposeCts) [0x000b3] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.Net.Http/src/System/Net/Http/HttpClient.cs:531 \n  at System.Net.Http.HttpClient.GetStringAsyncCore (System.Threading.Tasks.Task`1[TResult] getTask) [0x0002e] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.Net.Http/src/System/Net/Http/HttpClient.cs:147 \n  at DontWaitApp.FlavoursOrderServer.Initialize () [0x0007d] in F:\myproject\terpo\OpenVersions\FlavourBusiness\DontWaitAppNS\DontWaitAppNS\FlavoursOrderServer.cs:674 }
                }
            }
            // Handle when your app starts
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
}
