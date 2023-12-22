using OOAdvantech.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CourierApp
{
    /// <MetaDataID>{86a3a8b4-2d1b-4621-a111-25305b60b1e7}</MetaDataID>
    public partial class MainPage : ContentPage
    {
        /// <MetaDataID>{cf8d310d-7926-490d-81f9-2e9032f450f8}</MetaDataID>
        public MainPage()
        {
            InitializeComponent();

            this.SizeChanged += MainPage_SizeChanged;
            BindingContext = new ViewModel.CourierActivityPresentation(false);

            if (string.IsNullOrWhiteSpace(hybridWebView.Uri))
            {
                string url = @"http://192.168.2.8:4306/";//org
                url = @"http://192.168.2.12:4306/";//Braxati
                //url = @"http://10.0.0.13:4306/";//work
                //url = "local://index.html";

                url = string.Format(@"http://{0}:4306/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

                hybridWebView.Uri = url;
            }
            hybridWebView.Navigated += HybridWebView_Navigated;
        }

        private void HybridWebView_Navigated(object sender, NavigatedEventArgs e)
        {
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.SetStatusBarColor(Color.BlueViolet);
        }

        private void MainPage_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
