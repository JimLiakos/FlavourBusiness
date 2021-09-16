using OOAdvantech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ServiceContextManagerApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    /// <MetaDataID>{819d6d14-ecb8-4522-9b80-220d86c801cf}</MetaDataID>
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        string url;
        public MainPage()
        {
            InitializeComponent();
            try
            {
                var sdsd = GetType().GetMetaData().Assembly.GetType("FlavourBusinessFacade.Proxies.Pr_IFlavoursServicesContextRuntime");


                BindingContext = new ManagerPresentation();
                if (string.IsNullOrWhiteSpace(hybridWebView.Uri))
                {
                    url = "http://192.168.2.2/DemoAngular";//DemoNPMTypeScript/index.html";
                    url = "http://localhost/demoangular/#/";
                    url = "http://192.168.2.7/DontWaitWeb/";
                    url = "http://192.168.2.10/DontWaitWeb/";
                    url = "http://localhost/DontWaitWeb/";
                    //hybridWebView.Uri = "http://192.168.2.3/DontWaitWeb/";


                    url = "http://192.168.2.4/DontWaitWeb/#/";
                    //url = "http://192.168.2.5:4304/";
                    //url = "http://10.0.0.8:4304";
                    //url = "http://10.0.0.8:4304/";
                    //url = "http://www.google.com/";
                    url = "http://192.168.2.8:4304/";
                    //url = "https://angularhost.z16.web.core.windows.net/4304/";

                    //https://angularhost.z16.web.core.windows.net/4304/

                    url = string.Format(@"https://{0}:4304/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

                    hybridWebView.Uri = url;

                    hybridWebView.Navigated += MainPage_Navigated;
                }
            }
            catch (Exception error)
            {
            }

        }

        private void MainPage_Navigated(object sender, OOAdvantech.Web.NavigatedEventArgs e)
        {
            if (e.Address.IndexOf(url) == 0)
                SigninTitleBar.IsVisible = false;
            else
                SigninTitleBar.IsVisible = true;
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            if (hybridWebView.NativeWebBrowser != null)
                hybridWebView.NativeWebBrowser.GoBack();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //var storageWritePermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.StorageWrite>();
            //if (storageWritePermission == Xamarin.Essentials.PermissionStatus.Denied)
            //    storageWritePermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.StorageWrite>();

            var cameraPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Camera>();
            if (cameraPermission == Xamarin.Essentials.PermissionStatus.Denied)
                cameraPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>();

            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.KeyboordChangeState += Device_KeyboordChangeState;


        }

        private void Device_KeyboordChangeState(KeybordStatus keybordStatus)
        {
            
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
            OOAdvantech.IDeviceOOAdvantechCore device = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.KeyboordChangeState -= Device_KeyboordChangeState;
        }
    }
}


/*
 * "  at System.Collections.Generic.Dictionary`2[TKey,TValue].TryInsert (TKey key, TValue value, System.Collections.Generic.InsertionBehavior behavior) [0x00111] in <0381c8f952fb47759ee29160e807b17d>:0 
  at System.Collections.Generic.Dictionary`2[TKey,TValue].set_Item (TKey key, TValue value) [0x00000] in <0381c8f952fb47759ee29160e807b17d>:0 
  at OOAdvantech.Remoting.Tracker.MarshaledObject (System.Object obj, System.Runtime.Remoting.ObjRef objRef) [0x00020] in <010cb23f03074f2084d9df61f934ca92>:0 
  at System.Runtime.Remoting.RemotingServices.Marshal (System.MarshalByRefObject marshalByRefObject) [0x00048] in <010cb23f03074f2084d9df61f934ca92>:0 
  at OOAdvantech.Remoting.RestApi.ServerSessionPart.GetServerSesionObjectRef () [0x00012] in <010cb23f03074f2084d9df61f934ca92>:0 
  at (wrapper remoting-invoke-with-check) OOAdvantech.Remoting.RestApi.ServerSessionPart.GetServerSesionObjectRef()
  at OOAdvantech.Remoting.RestApi.MessageDispatcher.ProcessRestApiManagerMethodCall (OOAdvantech.Remoting.RestApi.RequestData request, OOAdvantech.Remoting.RestApi.MethodCallMessage methodCallMessage) [0x00073] in <010cb23f03074f2084d9df61f934ca92>:0 "
 * */