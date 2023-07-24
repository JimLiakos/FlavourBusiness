using GenWebBrowser;
using StyleableWindow;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FLBManager.Views
{
    /// <summary>
    /// Interaction logic for HomeDeliveryServicePage.xaml
    /// </summary>
    /// <MetaDataID>{1c48811b-23cc-4a1e-92e4-927675230f17}</MetaDataID>
    public partial class HomeDeliveryServicePage : PageDialogViewEmulator
    {
        public HomeDeliveryServicePage()
        {
            InitializeComponent();


            Loaded += HomeDeliveryServicePage_Loaded;
            Unloaded += HomeDeliveryServicePage_Unloaded;


        }

        private void HomeDeliveryServicePage_Unloaded(object sender, RoutedEventArgs e)
        {
            Browser?.DisposeBrowser();
        }

        protected override bool CheckOnlyPersistentClassInstancesForChanges => false;

        private void HomeDeliveryServicePage_Loaded(object sender, RoutedEventArgs e)
        {

            WebBrowserHost.DataContext = this.GetDataContextObject();
            string url = @"http://localhost:4300/";
            url = @"https://localhost:4300/";
            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.Navigate(new Uri(url + "#/service-area-map"));
        }

        private void Browser_ProcessRequest(Uri requestUri, CustomProtocolResponse response)
        {

        }

        public WebBrowserOverlay Browser { get; private set; }
    }
}
