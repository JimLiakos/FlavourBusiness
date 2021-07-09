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
            //url = @"http://192.168.2.5:4301/";//Braxati
            //url = @"http://10.0.0.13:4301/";//work
            //url = "local://index.html";

            
            hybridWebView.Uri = url;
        }
    }
}
