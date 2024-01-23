using GenWebBrowser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MenuItemsEditor.Views
{
    /// <summary>
    /// Interaction logic for MenuItemView.xaml
    /// </summary>
    /// <MetaDataID>{9fb34322-c956-48a9-8ed1-e6b70c462ff1}</MetaDataID>
    public partial class MenuItemView : UserControl, INotifyPropertyChanged
    {
        public MenuItemView()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {

                //browser.RegisterJsObject("cscallbackObj", new CallbackObjectForJs(browser));
                Loaded += MenuItemWindow_Loaded;
            }

        }

        const int BrowserExtraWidth=370;


        WebBrowserOverlay Browser;

        WebBrowserOverlay ExtraInfoWebView;
        private void Browser_Navigated(object sender, EventArgs e)
        {

        }

        private void Wb_ScriptNotify(object sender, string data)
        {



            //object res = Browser.InvockeJSMethod("logA", new[] { "Hello mama" });
            //MessageBox.Show(res as string, "Alert" , MessageBoxButton.OK);

        }

        protected void RunPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }
        bool Expanded;

        //const string JavaScriptFunction = "function invokeCSharpAction(data){ return 1 + 1;}";

        private void MenuItemWindow_Loaded(object sender, RoutedEventArgs e)
        {

            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {

                
                Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
                Browser.Navigated += Browser_Navigated;

                 
                ExtraInfoWebView= new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
                ExtraInfoWebView.Navigated+=ExtraInfoWebView_Navigated;


                ExpandBtn.Checked += ExpandBtn_Checked;
                ExpandBtn.Unchecked += ExpandBtn_Unchecked;

                SizeChanged += MenuItemWindow_SizeChanged;
                WebBrowser.SizeChanged += WebBrowser_SizeChanged;
                OrgWidth = Width;

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(WindowExpandedWith)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(WindowOrgWith)));



                //wb.NavigateToString(MenuItemsEditor.Properties.Resources.Default);

                string curDir = System.IO.Directory.GetCurrentDirectory();
                string internetCache = System.Environment.GetFolderPath(System.Environment.SpecialFolder.InternetCache);
                string sourcePath = String.Format(@"{0}\MenuItemsEditorHtml", curDir);
                string destinationPath = String.Format(@"{0}\MenuItemsEditorHtml", internetCache);

                //Copy(sourcePath, destinationPath);

                // WebBrowser.DataContext = new ViewModel.MenuItemTSViewModel(this.GetDataContextObject<MenuItemsEditor.ViewModel.MenuItemViewModel>());

                //string uri = String.Format("file:///{0}/index.html", destinationPath);
                //string uri = String.Format("file:///{0}/PublicMenu/TransitionHtmlPage.html", destinationPath);

                string uri = String.Format("http://localhost:4300/#/ItemPreview");

                // uri = "http://localhost/FlavourBusinessWebRole/PublicMenu/TransitionHtmlPage.html";
                Browser.Navigate(uri);
                Browser.SuppressScriptErrors = true;
                ExtraInfoWebView.Navigate("http://localhost:4300/#/EditItemExtraInfo");

                //this.GetDataContextObject<ViewModel.MenuItemViewModel>().SetHtmlView(Browser);
            }));



        }

        private void ExtraInfoWebView_Navigated(object sender, NavigatedEventArgs e)
        {
            
        }

        private void WebBrowser_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OrgWidth = Width - WebBrowser.Width;

            if (WebBrowser.Width == BrowserExtraWidth || WebBrowser.Width < 10)
            {
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(WindowExpandedWith)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(WindowOrgWith)));
            }
        }

        private void MenuItemWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            OrgWidth = Width - WebBrowser.Width;

            if (WebBrowser.Width == BrowserExtraWidth || WebBrowser.Width < 10)
            {
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(WindowExpandedWith)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(WindowOrgWith)));
            }

        }

        double _OrgWidth;

        public event PropertyChangedEventHandler PropertyChanged;

        public double OrgWidth
        {
            set
            {
                if (_OrgWidth != value)
                {
                    _OrgWidth = value;
                }
            }
            get
            {
                return _OrgWidth;
            }
        }
        public double WindowExpandedWith
        {
            get
            {
                return OrgWidth + BrowserExtraWidth;
            }
        }

        public double WindowOrgWith
        {
            get
            {
                return OrgWidth;
            }
        }


        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {


            // browser.EvaluateScriptAsync(JavaScriptFunction);
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(System.IO.Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            List.ScrollIntoView(List.SelectedItem);

        }

        private void ExpandBtn_Checked(object sender, RoutedEventArgs e)
        {
            Expanded = true;
            WebBrowser.Width = BrowserExtraWidth;
            DialogContentControl.InitialWidth = DialogContentControl.InitialWidth + WebBrowser.Width;
            WebBrowser.Visibility = Visibility.Visible;
        }

        private void ExpandBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            Expanded = false;
            var newWidth = DialogContentControl.InitialWidth - WebBrowser.Width;
            WebBrowser.Visibility = Visibility.Hidden;
            WebBrowser.Width = 0;
            DialogContentControl.InitialWidth = newWidth;

        }
    }
}
