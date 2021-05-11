﻿using GenWebBrowser;
using MenuDesigner.ViewModel.Preview;
using StyleableWindow;
using System;
using System.Collections.Generic;
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
                GraphickMenuResources = new Dictionary<string, MemoryStream>();
                BookViewModel.CreateMenuPreview(FlavoursOrderServer.MenuData.MenuRoot, FlavoursOrderServer.MenuData.MenuName, GraphickMenuResources);

            }));
            string url = @"http://localhost:4300/";

            Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
            Browser.ProcessRequest += this.Browser_ProcessRequest;

            Browser.Navigate(new Uri(url + "/#/room-service;orderServerPath=.%2FEndUser"));
            FlavoursOrderServer.Initialize();
        }

        Dictionary<string, MemoryStream> GraphickMenuResources = null;

        private void Browser_ProcessRequest(Uri requestUri, CustomProtocolResponse response)
        {
            response.Stream = GraphickMenuResources[requestUri.ToString().ToLower()];
        }



        FlavoursOrderServer FlavoursOrderServer = new FlavoursOrderServer();
        public WebBrowserOverlay Browser { get; private set; }


    }
}