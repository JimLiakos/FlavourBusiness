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

namespace WaiterApp.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <MetaDataID>{844e71f3-f363-429e-8bb6-2ce71d64487a}</MetaDataID>
    public partial class MainWindow : Window
    {
        WebBrowserOverlay Browser;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel.WaiterPresentation.Current;
            string url = @"http://192.168.2.8:4303/";//org
            url = @"http://192.168.2.5:4303/";//Braxati
            //url = @"http://10.0.0.13:4303/";//work
            url = @"http://localhost:4303/";
            //url = "https://angularhost.z16.web.core.windows.net/4303/";



            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.Navigate(new Uri(url));
            ViewModel.WaiterPresentation.Current.FlavoursOrderServer.Initialize();
        }
    }
}
