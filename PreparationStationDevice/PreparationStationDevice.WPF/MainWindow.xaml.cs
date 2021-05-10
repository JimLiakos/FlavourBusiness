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

namespace PreparationStationDevice.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        WebBrowserOverlay Browser;
        public MainWindow()
        {
            InitializeComponent();

            
            DataContext = new FlavoursPreparationStation();
            string url = @"http://192.168.2.8:4301/";//org
            url = @"http://192.168.2.4:4301/";//Braxati
            //url = @"http://10.0.0.8:4301/";//work


            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.Navigate(new Uri(url));
        }
    }
}
