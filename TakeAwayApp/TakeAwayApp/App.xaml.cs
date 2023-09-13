using OOAdvantech;
using OOAdvantech.Remoting.RestApi.Serialization;
using System;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TakeAwayApp
{
    /// <MetaDataID>{50f35b6d-bba8-4dea-bda3-c9a6898e0a0a}</MetaDataID>
    public partial class App : Application, OOAdvantech.IAppLifeTime
    {
        public App()
        {
            InitializeComponent();

            string storageMetadataGetFullUrl = string.Format("http://{0}:8090/api/Storages", FlavourBusinessFacade.ComputingResources.EndPoint.Server);
            // OOAdvantech.PersistenceLayer.StorageServerInstanceLocatorEx.SetStorageInstanceLocationServerUrl(storageMetadataGetFullUrl);

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuFoodItem"] = typeof(MenuModel.JsonViewModel.MenuFoodItem);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MenuItemPrice"] = typeof(MenuModel.JsonViewModel.MenuItemPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionMenuItemSpecific"] = typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Option"] = typeof(MenuModel.JsonViewModel.Option);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.OptionGroup"] = typeof(MenuModel.JsonViewModel.OptionGroup);

            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.ScaleType"] = typeof(MenuModel.JsonViewModel.ScaleType);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Level"] = typeof(MenuModel.JsonViewModel.Level);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.CustomizedPrice"] = typeof(MenuModel.JsonViewModel.CustomizedPrice);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.ItemSelectorOption"] = typeof(MenuModel.JsonViewModel.ItemSelectorOption);


            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.PartofMeal"] = typeof(MenuModel.JsonViewModel.PartofMeal);
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MealType"] = typeof(MenuModel.JsonViewModel.MealType);
            //SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.MealCourseType"] = typeof(MenuModel.JsonViewModel.MealCourseType);
            SerializationBinder.NamesTypesDictionary["MenuModel.JsonViewModel.Tag"] = typeof(MenuModel.JsonViewModel.Tag);
            //SerializationBinder.NamesTypesDictionary["MenuModel.ITag"] = typeof(MenuModel.ITag);
            SerializationBinder.NamesTypesDictionary["MenuModel.Tag"] = typeof(MenuModel.JsonViewModel.Tag);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.EndUsers.Place"] = typeof(FlavourBusinessManager.EndUsers.Place);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.EndUsers.Coordinate"] = typeof(FlavourBusinessFacade.EndUsers.Coordinate);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.HomeDeliveryServicePointInfo"] = typeof(FlavourBusinessFacade.HomeDeliveryServicePointInfo);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation"] = typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation);
            SerializationBinder.NamesTypesDictionary["TakeAwayApp.ViewModel.WatchingOrderPresentation"] = typeof(global::TakeAwayApp.ViewModel.WatchingOrderPresentation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ViewModel.MealCourse"] = typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessFacade.RoomService.PayAmount"] = typeof(FlavourBusinessFacade.RoomService.PayAmount);
            
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.ItemPreparation"] = typeof(FlavourBusinessManager.RoomService.ItemPreparation);
            SerializationBinder.NamesTypesDictionary["FlavourBusinessManager.RoomService.OptionChange"] = typeof(FlavourBusinessManager.RoomService.OptionChange);

            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.HallLayout"] = typeof(RestaurantHallLayoutModel.HallLayout);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ServicePointShape"] = typeof(RestaurantHallLayoutModel.ServicePointShape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.Shape"] = typeof(RestaurantHallLayoutModel.Shape);
            SerializationBinder.NamesTypesDictionary["RestaurantHallLayoutModel.ShapesGroup"] = typeof(RestaurantHallLayoutModel.ShapesGroup);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.Margin"] = typeof(UIBaseEx.Margin);
            SerializationBinder.NamesTypesDictionary["UIBaseEx.FontData"] = typeof(UIBaseEx.FontData);



            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MenuFoodItem)] = "MenuModel.JsonViewModel.MenuFoodItem, MenuModel";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MenuItemPrice)] = "MenuModel.JsonViewModel.MenuItemPrice";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.OptionMenuItemSpecific)] = "MenuModel.JsonViewModel.OptionMenuItemSpecific";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Option)] = "MenuModel.JsonViewModel.Option";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.OptionGroup)] = "MenuModel.JsonViewModel.OptionGroup";

            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.ScaleType)]= "MenuModel.JsonViewModel.ScaleType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Level)] = "MenuModel.JsonViewModel.Level";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.CustomizedPrice)] = "MenuModel.JsonViewModel.CustomizedPrice";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.PartofMeal)] = "MenuModel.JsonViewModel.PartofMeal";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MealCourseType)] = "MenuModel.JsonViewModel.MealCourseType";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.MealType)] = "MenuModel.JsonViewModel.MealType";
            SerializationBinder.TypesNamesDictionary[typeof(MenuModel.JsonViewModel.Tag)] = "MenuModel.JsonViewModel.Tag";
            //SerializationBinder.TypesNamesDictionary[typeof(MenuModel.ITag)] = "MenuModel.ITag";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.EndUsers.Place)] = "FlavourBusinessManager.EndUsers.Place";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.EndUsers.Coordinate)] = "FlavourBusinessFacade.EndUsers.Coordinate";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.RoomService.PayAmount)] = "FlavourBusinessFacade.RoomService.PayAmount";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.HomeDeliveryServicePointInfo)] = "FlavourBusinessFacade.HomeDeliveryServicePointInfo";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation)] = "FlavourBusinessFacade.HomeDeliveryServicePointAbbreviation";
            SerializationBinder.TypesNamesDictionary[typeof(global::TakeAwayApp.ViewModel.WatchingOrderPresentation)] = "TakeAwayApp.ViewModel.WatchingOrderPresentation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ViewModel.MealCourse)] = "FlavourBusinessManager.RoomService.ViewModel.MealCourse";




            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.ItemPreparation)] = "FlavourBusinessManager.RoomService.ItemPreparation";
            SerializationBinder.TypesNamesDictionary[typeof(FlavourBusinessManager.RoomService.OptionChange)] = "FlavourBusinessManager.RoomService.OptionChange";

            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.HallLayout)] = "RestaurantHallLayoutModel.HallLayout";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ServicePointShape)] = "RestaurantHallLayoutModel.ServicePointShape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.Shape)] = "RestaurantHallLayoutModel.Shape";
            SerializationBinder.TypesNamesDictionary[typeof(RestaurantHallLayoutModel.ShapesGroup)] = "RestaurantHallLayoutModel.ShapesGroup";



            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.Margin)] = "UIBaseEx.Margin";
            SerializationBinder.TypesNamesDictionary[typeof(UIBaseEx.FontData)] = "UIBaseEx.FontData";
            MainPage = new NavigationPage(new MainPage());
        }

        protected  override async void OnStart()
        {

            //bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(FlavourBusinessFacade.ComputingResources.EndPoint.Server);
            //if (isRemoteReachable)
            //{
            //    System.Diagnostics.Debug.WriteLine("Yes");
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Yes");
            //}
            using (HttpClient wc = new HttpClient())
            {
                try
                {
                    //bool IsRemoteReachable=await CrossConnectivity.Current.IsRemoteReachable(FlavourBusinessFacade.ComputingResources.EndPoint.Server);
                    string url = $"http://{FlavourBusinessFacade.ComputingResources.EndPoint.Server}:8090/api/values";
                    var resp = await wc.GetStringAsync(url);
                    System.Diagnostics.Debug.WriteLine("sds");
                }
                catch (Exception error)
                {
                    //                    { System.Net.Http.HttpRequestException: The Internet connection appears to be offline. --->Foundation.NSErrorException: Error Domain = NSURLErrorDomain Code = -1009 "The Internet connection appears to be offline." UserInfo ={ _kCFStreamErrorCodeKey = 50, NSUnderlyingError = 0x281d35470 { Error Domain = kCFErrorDomainCFNetwork Code = -1009 "(null)" UserInfo ={ _NSURLErrorNWPathKey = unsatisfied(Local network prohibited), interface: en0, ipv4, _kCFStreamErrorCodeKey=50, _kCFStreamErrorDomainKey=1}
                    //}, _NSURLErrorFailingURLSessionTaskErrorKey = LocalDataTask < 6CF91F8F - 33F5 - 4F6B - B500 - D05CBEB3404D >.< 1 >, _NSURLErrorRelatedURLSessionTaskErrorKey = (\n "LocalDataTask <6CF91F8F-33F5-4F6B-B500-D05CBEB3404D>.<1>"\n), NSLocalizedDescription = The Internet connection appears to be offline., NSErrorFailingURLStringKey = http://10.0.0.13:8090/api/values, NSErrorFailingURLKey=http://10.0.0.13:8090/api/values, _kCFStreamErrorDomainKey=1}\n   --- End of inner exception stack trace ---\n  at System.Net.Http.NSUrlSessionHandler.SendAsync (System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) [0x001ad] in /Library/Frameworks/Xamarin.iOS.framework/Versions/16.0.0.75/src/Xamarin.iOS/Foundation/NSUrlSessionHandler.cs:529 \n  at System.Net.Http.HttpClient.FinishSendAsyncUnbuffered (System.Threading.Tasks.Task`1[TResult] sendTask, System.Net.Http.HttpRequestMessage request, System.Threading.CancellationTokenSource cts, System.Boolean disposeCts) [0x000b3] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.Net.Http/src/System/Net/Http/HttpClient.cs:531 \n  at System.Net.Http.HttpClient.GetStringAsyncCore (System.Threading.Tasks.Task`1[TResult] getTask) [0x0002e] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.Net.Http/src/System/Net/Http/HttpClient.cs:147 \n  at DontWaitApp.FlavoursOrderServer.Initialize () [0x0007d] in F:\myproject\terpo\OpenVersions\FlavourBusiness\DontWaitAppNS\DontWaitAppNS\FlavoursOrderServer.cs:674 }
                }
            }
            //"Error Domain=NSURLErrorDomain Code=-1009 \"The Internet connection appears to be offline.\" UserInfo={_kCFStreamErrorCodeKey=50, NSUnderlyingError=0x28057c0c0 {Error Domain=kCFErrorDomainCFNetwork Code=-1009 \"(null)\" UserInfo={_NSURLErrorNWPathKey=satisfied (Path is satisfied), viable, interface: en0, ipv4, dns, _kCFStreamErrorCodeKey=50, _kCFStreamErrorDomainKey=1}}, _NSURLErrorFailingURLSessionTaskErrorKey=LocalDataTask <63194186-2F2B-4936-B2E1-FE537D9C3863>.<1>, _NSURLErrorRelatedURLSessionTaskErrorKey=(\n    \"LocalDataTask <63194186-2F2B-4936-B2E1-FE537D9C3863>.<1>\"\n), NSLocalizedDescription=The Internet connection appears to be offline., NSErrorFailingURLStringKey=http://192.168.1.8:8090/api/values, NSErrorFailingURLKey=http://192.168.1.8:8090/api/values, _kCFStreamErrorDomainKey=1}"

            SerializeTaskScheduler.RunAsync();
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.IsinSleepMode = false;
            //OnAppLinkRequestReceived(new Uri("http://192.168.2.8:4300/#/launch-app?mealInvitation=True&sc=7f9bde62e6da45dc8c5661ee2220a7b0&sp=fe51ba7e30954ee08209bd89a03469a8&cs=6126a9565db94a88ade1e604172a683b"));


            bool delete = false;

            try
            {
                string text = DeviceApplication.Current.ReadLog();
                System.Diagnostics.Debug.Write(text);

            }
            catch
            {
                // just suppress any error logging exceptions
            }

            if (delete)
                DeviceApplication.Current.ClearLog();


#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string> { "OnStart" });
#endif
            try
            {
                string text = DeviceApplication.Current.ReadLog();

            }
            catch
            {
                // just suppress any error logging exceptions
            }

        }


        public event EventHandler ApplicationResuming;
        public event EventHandler ApplicationSleeping;


        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler = new OOAdvantech.SerializeTaskScheduler();
        OOAdvantech.SerializeTaskScheduler OOAdvantech.IAppLifeTime.SerializeTaskScheduler => SerializeTaskScheduler;

    
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
