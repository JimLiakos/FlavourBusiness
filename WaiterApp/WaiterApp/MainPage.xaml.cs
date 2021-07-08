using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WaiterApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var appSettings = DontWaitApp.ApplicationSettings.Current;
            BindingContext = new WaiterApp.ViewModel.WaiterPresentation();

            if (string.IsNullOrWhiteSpace(hybridWebView.Uri))
            {
                string url = @"http://192.168.2.8:4303/";//org
                url = @"http://192.168.2.5:4303/";//Braxati
                url = @"http://10.0.0.13:4303/";//work
                //url = "local://index.html";

                hybridWebView.Uri = url;
            }
        }

        bool check=true;
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //var storageWritePermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.StorageWrite>();
            //if (storageWritePermission == Xamarin.Essentials.PermissionStatus.Denied)
            //    storageWritePermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.StorageWrite>();

            var cameraPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Camera>();
            if (cameraPermission != Xamarin.Essentials.PermissionStatus.Granted)
                cameraPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>();

            if (!check)
            {
                check = true;
                var deviceInstantiator = Xamarin.Forms.DependencyService.Get<OOAdvantech.IDeviceInstantiator>();
                OOAdvantech.IBatteryInfo batteryInfo = deviceInstantiator.GetDeviceSpecific(typeof(OOAdvantech.IBatteryInfo)) as OOAdvantech.IBatteryInfo;

                if (batteryInfo.CheckIsEnableBatteryOptimizations())
                {


                }
                else
                {
                    bool yes= await Application.Current.MainPage.DisplayAlert("open battery settings", "please close battery optimization", "yes", "No");
                    if(yes)
                        batteryInfo.StartSetting();

                }
            }


        }
    }
}
