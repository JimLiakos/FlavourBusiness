using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech;

using OOAdvantech.Pay;
using System.Linq;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO;
using OOAdvantech.Collections.Generic;
using System.Net.Http;
using Plugin.Connectivity;
using System.Security.Cryptography.X509Certificates;
using System.IO.Compression;
using System.Net;
using OOAdvantech.Web;

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

        

            //using (WebClient wc = new WebClient())
            //{
            //    int i = 12;
            //    try
            //    {
            //        var json = wc.DownloadString("http://dontwaitwaiter.com/img/ServerAddress.json");
            //        var webAppBundl = wc.DownloadData("http://dontwaitwaiter.com/img/DontWaitWeb.zip");

            //        System.Diagnostics.Debug.WriteLine(json);
            //    }
            //    catch (Exception error)
            //    {

            //        throw;
            //    }
            //    i++;


            //}



            HybridWebView.WebAppUpdate("http://dontwaitwaiter.com/img/DontWaitWeb.zip");




            OOAdvantech.Remoting.RestApi.Authentication.InitializeFirebase("demomicroneme");
            InitializeRemoteTypes();





            ApplicationSettings.GetCurrent();
            MainPage = new NavigationPage(new DontWaitApp.HybridWebViewPage());
            //MainPage = new NavigationPage(new MainPage());






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


            using (HttpClient wc = new HttpClient())
            {
                try
                {
                    //CrossConnectivity.Current.IsRemoteReachable("127.0.0.1", 22, 5000);
                    string server = FlavourBusinessFacade.ComputingResources.EndPoint.Server;
                    //var IsRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(FlavourBusinessFacade.ComputingResources.EndPoint.Server);
                    
                    
                    string url = $"http://{FlavourBusinessFacade.ComputingResources.EndPoint.Server}:8090/api/values";
                    var resp= await wc.GetStringAsync(url);
                    
                    
                    System.Diagnostics.Debug.WriteLine(resp);
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error.Message);
                    //                    { System.Net.Http.HttpRequestException: The Internet connection appears to be offline. --->Foundation.NSErrorException: Error Domain = NSURLErrorDomain Code = -1009 "The Internet connection appears to be offline." UserInfo ={ _kCFStreamErrorCodeKey = 50, NSUnderlyingError = 0x281d35470 { Error Domain = kCFErrorDomainCFNetwork Code = -1009 "(null)" UserInfo ={ _NSURLErrorNWPathKey = unsatisfied(Local network prohibited), interface: en0, ipv4, _kCFStreamErrorCodeKey=50, _kCFStreamErrorDomainKey=1}
                    //}, _NSURLErrorFailingURLSessionTaskErrorKey = LocalDataTask < 6CF91F8F - 33F5 - 4F6B - B500 - D05CBEB3404D >.< 1 >, _NSURLErrorRelatedURLSessionTaskErrorKey = (\n "LocalDataTask <6CF91F8F-33F5-4F6B-B500-D05CBEB3404D>.<1>"\n), NSLocalizedDescription = The Internet connection appears to be offline., NSErrorFailingURLStringKey = http://10.0.0.13:8090/api/values, NSErrorFailingURLKey=http://10.0.0.13:8090/api/values, _kCFStreamErrorDomainKey=1}\n   --- End of inner exception stack trace ---\n  at System.Net.Http.NSUrlSessionHandler.SendAsync (System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) [0x001ad] in /Library/Frameworks/Xamarin.iOS.framework/Versions/16.0.0.75/src/Xamarin.iOS/Foundation/NSUrlSessionHandler.cs:529 \n  at System.Net.Http.HttpClient.FinishSendAsyncUnbuffered (System.Threading.Tasks.Task`1[TResult] sendTask, System.Net.Http.HttpRequestMessage request, System.Threading.CancellationTokenSource cts, System.Boolean disposeCts) [0x000b3] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.Net.Http/src/System/Net/Http/HttpClient.cs:531 \n  at System.Net.Http.HttpClient.GetStringAsyncCore (System.Threading.Tasks.Task`1[TResult] getTask) [0x0002e] in /Library/Frameworks/Xamarin.iOS.framework/Versions/Current/src/Xamarin.iOS/external/corefx/src/System.Net.Http/src/System/Net/Http/HttpClient.cs:147 \n  at DontWaitApp.FlavoursOrderServer.Initialize () [0x0007d] in F:\myproject\terpo\OpenVersions\FlavourBusiness\DontWaitAppNS\DontWaitAppNS\FlavoursOrderServer.cs:674 }
                }
            }
            //try
            //{
            //    OOAdvantech.FileSystem.WriteAllText(@"\Demo.log", "hello");

            //    System.Threading.Thread.Sleep(1000);

            //    string text = OOAdvantech.FileSystem.ReadAllText(@"\Demo.log");
            //    System.Diagnostics.Debug.WriteLine(text);

            //}
            //catch (Exception error)
            //{


            //}



            //try
            //{

            //    bool isRemoteReachable = await CrossConnectivity.Current.IsRemoteReachable(FlavourBusinessFacade.ComputingResources.EndPoint.Server);
            //    if (isRemoteReachable)
            //    {
            //        System.Diagnostics.Debug.WriteLine("Yes");
            //    }
            //    else
            //    {
            //        System.Diagnostics.Debug.WriteLine("No");
            //    }
            //}
            //catch (Exception error)
            //{

            //}
            using (HttpClient wc = new HttpClient())
            {
                try
                {
                    //bool IsRemoteReachable=await CrossConnectivity.Current.IsRemoteReachable(FlavourBusinessFacade.ComputingResources.EndPoint.Server);
                    string url = $"http://{FlavourBusinessFacade.ComputingResources.EndPoint.Server}:8090/api/values";
                    var resp = await wc.GetStringAsync(url);
                    System.Diagnostics.Debug.WriteLine(resp);
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error.Message);
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
            OOAdvantech.DeviceApplication.Current.Log(new List<string> { "OnStart" });
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







        protected override void OnSleep()
        {

#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new List<string> { "OnSleep" });
#endif



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

#if DeviceDotNet
            OOAdvantech.DeviceApplication.Current.Log(new List<string> { "OnResume" });
#endif

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
                                            if ((((MainPage as NavigationPage)?.CurrentPage as HybridWebViewPage)?.BindingContext as FlavoursOrderServer) != null &&
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
