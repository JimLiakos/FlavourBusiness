using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace DontWaitApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <MetaDataID>{871e04a9-8b91-464a-a62e-a2ef2e92f7c6}</MetaDataID>
    public partial class MainWindow : Window
    {
        public WebBrowserOverlay Browser;

        FlavoursOrderServer FlavoursOrderServer = new FlavoursOrderServer();

        public MainWindow()
        {

            InitializeComponent();



            //using (WebClient wc = new WebClient())
            //{
            //    var json = wc.DownloadString(@"http://localhost/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/Marzano%20Phone.json");

            //   var sds=  Newtonsoft.Json.JsonConvert.DeserializeObject<MenuPresentationModel.JsonMenuPresentation.RestaurantMenu>(json, new Newtonsoft.Json.JsonSerializerSettings { PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All });

            //}

            //http://localhost/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/Marzano%20Phone.json


            this.DataContext = FlavoursOrderServer;
            FlavoursOrderServer.OnWebViewLoaded += FlavoursOrderServer_OnWebViewLoaded;
            

            string url = @"http://192.168.2.8:4303/";//org
            //url = @"http://192.168.2.4:4300/";//Braxati
            //url = @"http://10.0.0.8:4300/";//work
            url = @"http://localhost:4300/";

            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true); 

            Browser.Navigated += Browser_Navigated;
            
            

            if (!string.IsNullOrWhiteSpace(FlavoursOrderServer.Path) && FlavoursOrderServer.Path.Split('/').Length > 0)
            {
                //FlavoursOrderServer.GetServicePointData(FlavoursOrderServer.Path.Split('/')[0]);

                FlavoursOrderServer_OnWebViewLoaded();

                if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity))
                {
                    Browser.Navigate(new Uri(url + "/#/room-service;orderServerPath=.%2FEndUser"));
                    //http://192.168.2.8:4300/#/room-service
                }
                else
                    Browser.Navigate(new Uri(url));
                FlavoursOrderServer.Initialize();

            }
            else
                Browser.Navigate(new Uri(url));

            FlavoursOrderServer_OnWebViewLoaded();

        }


        string address;
        private void Browser_Navigated(object sender, NavigatedEventArgs e)
        {
            if (address != e.Address)
                address = e.Address;

        }

        private void FlavoursOrderServer_OnWebViewLoaded()
        {
            WebBrowserHost.HorizontalAlignment = HorizontalAlignment.Stretch;
            WebBrowserHost.VerticalAlignment = VerticalAlignment.Stretch;
            WaitLoading.Visibility = Visibility.Collapsed;
        }


    }
}
