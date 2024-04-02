using GenWebBrowser;
using MenuDesigner.ViewModel.Preview;
using StyleableWindow;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace MenuDesigner.Views
{
    /// <summary>
    /// Interaction logic for GraphicMenuPreviewPage.xaml
    /// </summary>
    /// <MetaDataID>{4ebd8d6f-be9a-48fc-905c-622f466a1d87}</MetaDataID>
    public partial class GraphicMenuPreviewPage : PageDialogViewEmulator
    {
        public GraphicMenuPreviewPage()
        {
            InitializeComponent();
            Loaded += GraphicMenuPreviewPage_Loaded;
        }
        ViewModel.MenuCanvas.BookViewModel BookViewModel;
        private void GraphicMenuPreviewPage_Loaded(object sender, RoutedEventArgs e)
        {
            BookViewModel = this.GetDataContextObject<ViewModel.MenuCanvas.BookViewModel>();
            WebBrowserHost.DataContext = FlavoursOrderServer;
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                GraphicMenuResources = new Dictionary<string, MemoryStream>();
                
                MenuModel.IMenuItem firstMenuItemOfGraphicMenu = BookViewModel.RestaurantMenu.MenuCanvasItems.OfType<MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem>().FirstOrDefault()?.MenuItem;
                if(firstMenuItemOfGraphicMenu!=null)
                {
                    OOAdvantech.Linq.Storage storage =new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(firstMenuItemOfGraphicMenu));

                    string defaultMealTypeUri=(from mealType in storage.GetObjectCollection<MenuModel.FixedMealType>()
                     select mealType).ToList().Where(x => x.Courses.Count == 2).FirstOrDefault()?.MealTypeUri;

                    string priceListUrl = null;
                    if (BookViewModel.PriceList != null && BookViewModel.PriceList.PriceListStorageRef != null)
                        priceListUrl = "customscheme://PriceList/pricelist.json";
                    FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData = new DontWaitApp.MenuData()
                    {
                        ServicesPointName = "",
                        ServicesContextLogo = "",
                        ServicePointIdentity = "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7",
                        MenuName = "Marzano Phone",
                        MenuFile = "Marzano Phone.json",
                        MenuRoot = "customscheme://Menu/",
                        PriceListUrl= priceListUrl,
                        ClientSessionID = "6ac1ac9751274733a6554312621b09a591000000296",
                        DefaultMealTypeUri = defaultMealTypeUri
                    };
                    //MenuModel.FixedMealType
                }



                BookViewModel.CreateMenuPreview(FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.MenuRoot, FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.MenuName, GraphicMenuResources);
                BookViewModel.PropertyChanged += BookViewModel_PropertyChanged;

            }));
            string url = @"https://localhost:4400/";

            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.ProcessRequest += this.Browser_ProcessRequest;

            Browser.Navigate(new Uri(url + "#/room-service;orderServerPath=.%2FEndUser"));
            FlavoursOrderServer.Initialize();
        }

        private void BookViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.MenuCanvas.BookViewModel.PriceList))
            {
                string priceListUrl = null;
                if (BookViewModel.PriceList != null && BookViewModel.PriceList.PriceListStorageRef != null)
                    priceListUrl = "customscheme://PriceList/pricelist.json";
                FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData = new DontWaitApp.MenuData()
                {
                    ServicesPointName = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.ServicesPointName,
                    ServicesContextLogo = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.ServicesContextLogo,
                    ServicePointIdentity = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.ServicePointIdentity,
                    MenuName = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.MenuName,
                    MenuFile = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.MenuFile,
                    MenuRoot = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.MenuRoot,
                    PriceListUrl = priceListUrl,
                    ClientSessionID = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.ClientSessionID,
                    DefaultMealTypeUri = FlavoursOrderServer.FoodServicesClientSessionViewModel.MenuData.DefaultMealTypeUri
                };


                BookViewModel.CreatePriceListPreview(GraphicMenuResources);

                Browser.Reload(ignoreCache: false);
            }
        }

        Dictionary<string, MemoryStream> GraphicMenuResources = null;

        private void Browser_ProcessRequest(Uri requestUri, CustomProtocolResponse response)
        {
            response.Stream = GraphicMenuResources[requestUri.ToString().ToLower()];
        }
        public void SetLanguage(CultureInfo culture)
        {
            FlavoursOrderServer.Language=culture.Name;
        }



        FlavoursOrderServer FlavoursOrderServer = new FlavoursOrderServer();
        public WebBrowserOverlay Browser { get; private set; }


    }
}
