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
using GenWebBrowser;

namespace ServiceContextManagerApp.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <MetaDataID>{ca6e5961-399b-48f5-a455-f2d918df846e}</MetaDataID>
    public partial class MainWindow : Window
    {
        WebBrowserOverlay Browser;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ManagerPresentation();

            string url = @"http://192.168.2.8:4304/";//org
            //url = @"http://192.168.2.4:4304/";//Braxati
            //url = @"http://10.0.0.8:4304/";//work
            url = @"https://localhost:4304/";
            //url = "https://angularhost.z16.web.core.windows.net/4304/";



            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.Navigate(new Uri(url));

        }
    }
}

