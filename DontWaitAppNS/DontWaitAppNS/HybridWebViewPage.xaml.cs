using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using OOAdvantech;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using DontWaitApp;
using static System.Net.WebRequestMethods;
using System.Linq;

namespace DontWaitApp
{
    public partial class HybridWebViewPage : ContentPage
    {
        public HybridWebViewPage()
        {

            //"http://192.168.2.10/WebPart/index.html" 
            //"http://10.0.0.10/WebPart/index.html" 

            InitializeComponent();

            hybridWebView.RegisterAction(async (string data) =>
            {
                string res = await hybridWebView.NativeWebBrowser.InvockeJSMethod("logA", new[] { data });
                Device.BeginInvokeOnMainThread(async () =>
                {

                    await DisplayAlert("Message", res, "OK");
                });

                int rr = 0;
            });

            FlavoursOrderServer.Trademark = "Arionas";
            this.BindingContext = FlavoursOrderServer;

            EventTest eventTest = new EventTest();

            var changeEventInfo = eventTest.GetType().GetMetaData().GetEvent("Change");

            CSM_EventTest_Change eventConsumer = new CSM_EventTest_Change(eventTest, changeEventInfo);
            eventTest.Triger();

            if (string.IsNullOrWhiteSpace(hybridWebView.Uri))
            {

                string url = "http://192.168.2.2/DemoAngular";//DemoNPMTypeScript/index.html";
                url = "http://localhost/demoangular/#/";
                url = "http://192.168.2.7/DontWaitWeb/";
                url = "http://192.168.2.10/DontWaitWeb/";
                url = "http://localhost/DontWaitWeb/";
                //hybridWebView.Uri = "http://192.168.2.3/DontWaitWeb/";


                url = "http://192.168.2.4/DontWaitWeb/#/";
                url = "http://192.168.2.4:4300/#/";
                url = "http://10.0.0.13:4300/#/";

                //url = "https://angularhost.z16.web.core.windows.net/DontWaitWeb/#/";

                url = string.Format(@"http://{0}:4300/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);
                //url = "https://angularhost.z16.web.core.windows.net/DontWaitWeb/#/";
                //url = "http://dontwaitwaiter.com/DontWaitWeb/#/";
                //url = "http://dontwaitwaiter.com";
                //url = string.Format(@"https://{0}:4300/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

                //var AssemblyMetaObject =OOAdvantech.DotNetMetaDataRepository.Assembly.GetComponent(typeof(ApplicationSettings).GetMetaData().Assembly);
                // long count = AssemblyMetaObject.Residents.Count;


                //url= "local://index.html#/";
                //url = "local://m-index.html";
                //System.Reflection.DispatchProxy dispatchProxy;

                string path = FlavoursOrderServer.Path;
               // Refresh=true;
                //path = "";
                if (path != null && path.Split('/').Length > 0 &&
                path.Split('/')[0] == ApplicationSettings.Current.DisplayedFoodServicesClientSession?.ServicePointIdentity)
                {
                    if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.DisplayedFoodServicesClientSession?.ServicePointIdentity))
                    {
                        // FlavoursOrderServer.GetServicePointData(FlavoursOrderServer.MenuData.ServicePointIdentity);

                        hybridWebView.Uri = url + "#/?orderServerPath=.%2FEndUser";
                        //http://192.168.2.9:4300/#/room-service
                    }
                    else
                        hybridWebView.Uri = url;
                }
                else
                    hybridWebView.Uri = url;

                hybridWebView.Navigated += HybridWebView_Navigated;

                

            }


        }
        bool Refresh;
        private void HybridWebView_Navigated(object sender, OOAdvantech.Web.NavigatedEventArgs e)
        {
            if(Refresh)
            {
                Refresh=false;
                hybridWebView.RefreshPage();
            }
        }

        bool check = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //MyLabel.Text = "Liakos";

            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

            //if (device.ForegroundService)
            //{
            //    if (!device.IsForegroundServiceStarted)
            //        device.StartForegroundService();
            //}
            if (check)
            {
                OOAdvantech.IBatteryInfo batteryInfo = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IBatteryInfo)) as OOAdvantech.IBatteryInfo;

                if (batteryInfo.CheckIsEnableBatteryOptimizations())
                {


                }
                else
                {
                    var action = await Application.Current.MainPage.DisplayAlert("open battery settings", "please close battery optimization", "yes", "No");
                    batteryInfo.StartSetting();
                    //if (action)
                    //{

                    //    batteryInfo.StartSetting();
                    //}
                    //else
                    //{

                    //}

                }
            }
            FlavoursOrderServer.Initialize();


            //IDeviceOOAdvantechCore device = DependencyService.Get<IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
          
            //if (!device.IsBackgroundServiceStarted)
            //{
            //    BackgroundServiceState serviceState = new BackgroundServiceState();
            //    device.RunInBackground(new Action(async () =>
            //    {
            //        //var message = Waiter.PeekMessage();
            //        //Waiter.MessageReceived += Waiter_MessageReceived;
                  
            //        do
            //        {
            //            System.Threading.Thread.Sleep(3000);

            //        } while (!serviceState.Terminate);

            //        //Waiter.MessageReceived -= Waiter_MessageReceived;
                    
            //    }), serviceState);
            //}

        }

        FlavoursOrderServer FlavoursOrderServer = new FlavoursOrderServer();
        protected override bool OnBackButtonPressed()
        {
            hybridWebView.NativeWebBrowser.InvockeJSMethod("NavigationButtonPress", new object[] { 1 });
            //hybridWebView.GoBack();

            return true;
            //return base.OnBackButtonPressed();
        }

      
    }
    public class EventTest
    {
        public event EventHandler Change;

        public void Triger()
        {
            Change?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CSM_EventTest_Change
    {
        public CSM_EventTest_Change(object target, EventInfo eventInfo)
        {
            //"C:\Projects\OpenVersions\PersistenceLayer\DotNetPersistenceLayer\Xamarin\DontWaitApp\bin\Debug\DontWaitApp.dll"
            //c:\projects\openversions\persistencelayer\dotnetpersistencelayer\xamarin\dontwaitapp
            eventInfo.AddEventHandler(target, new EventHandler(this.EventTest_Change));
        }

        private void EventTest_Change(object sender, EventArgs e)
        {

        }
    }


    //public static class Utils
    //{
    //    public static async Task<PermissionStatus> CheckPermissions(Permission permission)
    //    {
    //        var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
    //        bool request = false;
    //        if (permissionStatus == PermissionStatus.Denied)
    //        {
    //            if (Device.RuntimePlatform == Device.iOS)
    //            {

    //                var title = $"{permission} Permission";
    //                var question = $"To use this plugin the {permission} permission is required. Please go into Settings and turn on {permission} for the app.";
    //                var positive = "Settings";
    //                var negative = "Maybe Later";
    //                var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
    //                if (task == null)
    //                    return permissionStatus;

    //                var result = await task;
    //                if (result)
    //                {

    //                    CrossPermissions.Current.OpenAppSettings();
    //                }

    //                return permissionStatus;
    //            }

    //            request = true;

    //        }

    //        if (request || permissionStatus != PermissionStatus.Granted)
    //        {
    //            var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);

    //            if (!newStatus.ContainsKey(permission))
    //            {
    //                return permissionStatus;
    //            }

    //            permissionStatus = newStatus[permission];

    //            if (newStatus[permission] != PermissionStatus.Granted)
    //            {
    //                permissionStatus = newStatus[permission];
    //                string title = $"{permission} Permission";

    //                System.Diagnostics.Debug.WriteLine(title);
    //                string question = $"To use the plugin the {permission} permission is required.";
    //                string positive = "Settings";
    //                string negative = "Maybe Later";
    //                var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
    //                if (task == null)
    //                    return permissionStatus;

    //                var result = await task;
    //                if (result)
    //                {
    //                    //CrossPermissions.Current.OpenAppSettings();
    //                    if (result)
    //                    {
    //                        int i = 0;
    //                        var newStaatus = await CrossPermissions.Current.RequestPermissionsAsync(new Permission[] { permission });
    //                        i++;

    //                    }
    //                }
    //                return permissionStatus;
    //            }
    //        }

    //        return permissionStatus;
    //    }
    //}
}
