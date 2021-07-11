using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PreparationStationDevice
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    /// <MetaDataID>{be053cda-95f8-40e5-aad5-07b7b1c8497a}</MetaDataID>
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


            BindingContext = new FlavoursPreparationStation();
            
            string url = @"http://192.168.2.8:4301/";//org
            url = @"http://192.168.2.12:4301/";//Braxati
            //url = @"http://10.0.0.13:4301/";//work
            //url = "local://index.html";

            url =string.Format( @"http://{0}:4301/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);//work
            hybridWebView.Uri = url;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var cameraPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Camera>();
            if (cameraPermission != Xamarin.Essentials.PermissionStatus.Granted)
                cameraPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>();

        }
    }
}
