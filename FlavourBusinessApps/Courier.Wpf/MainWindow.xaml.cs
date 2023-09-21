using GenWebBrowser;
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

namespace CourierApp.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <MetaDataID>{357e3eb1-7a09-47b7-9724-e8b6d1c5d82a}</MetaDataID>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // DataContext = new ViewModel.FlavoursServiceOrderTakingStation();
            string url = @"http://192.168.2.8:4306/";//org
            url = @"http://192.168.2.5:4306/";//Braxati
            //url = @"http://10.0.0.13:4306/";//work
            url = @"http://localhost:4306/";
            //url = "https://angularhost.z16.web.core.windows.net/4303/";



            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.Navigate(new Uri(url));
             
        }

        public WebBrowserOverlay Browser { get; }
    }
}
