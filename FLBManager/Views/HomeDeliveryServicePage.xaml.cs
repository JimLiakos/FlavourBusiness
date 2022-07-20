using GenWebBrowser;
using StyleableWindow;
using System;
using System.Collections.Generic;
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
    public partial class HomeDeliveryServicePage : PageDialogViewEmulator
    {
        public HomeDeliveryServicePage()
        {
            InitializeComponent();

            
            Loaded += HomeDeliveryServicePage_Loaded;

        
        }


        private void HomeDeliveryServicePage_Loaded(object sender, RoutedEventArgs e)
        {
            string url = @"http://localhost:4300/";
            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.Navigate(new Uri(url + "/#/room-service;orderServerPath=.%2FEndUser"));
        }

        private void Browser_ProcessRequest(Uri requestUri, CustomProtocolResponse response)
        {
            
        }

        public WebBrowserOverlay Browser { get; private set; }
    }
}
