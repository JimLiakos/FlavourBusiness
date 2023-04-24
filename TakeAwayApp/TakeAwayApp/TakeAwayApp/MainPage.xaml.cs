using OOAdvantech.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TakeAwayApp
{
    /// <MetaDataID>{1568ac78-f45d-4398-b240-3312b7a6289b}</MetaDataID>
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new TakeAwayStationPresentation();

            if (string.IsNullOrWhiteSpace(hybridWebView.Uri))
            {
                string url = @"http://192.168.2.8:4303/";//org
                url = @"http://192.168.2.12:4303/";//Braxati
                //url = @"http://10.0.0.13:4303/";//work
                //url = "local://index.html";

                url = string.Format(@"http://{0}:4305/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

                hybridWebView.Uri = url;


            }
        }
    }
}
