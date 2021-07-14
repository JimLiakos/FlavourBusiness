using OOAdvantech.Speech;
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

            try
            {
                _speechRecongnitionInstance = DependencyService.Get<ISpeechText>();
                _speechRecongnitionInstance.SpeechRecognized += _speechRecongnitionInstance_SpeechRecognized;
            }
            catch (Exception ex)
            {

            }
        }

        private void _speechRecongnitionInstance_SpeechRecognized(List<string> speechTexts)
        {
            //SpeechText = "";
            //foreach (string text in speechTexts)
            //    SpeechText = text;


            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpeechText)));
        }

        private ISpeechText _speechRecongnitionInstance;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var cameraPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Camera>();
            if (cameraPermission != Xamarin.Essentials.PermissionStatus.Granted)
                cameraPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Camera>();

            var micPermission = await Xamarin.Essentials.Permissions.CheckStatusAsync<Xamarin.Essentials.Permissions.Microphone>();
            if (micPermission != Xamarin.Essentials.PermissionStatus.Granted)
                micPermission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.Microphone>();

        }
    }
}
