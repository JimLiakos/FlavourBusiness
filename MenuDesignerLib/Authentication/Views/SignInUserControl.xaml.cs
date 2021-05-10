using GenWebBrowser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace FLBAuthentication.Views
{
    /// <summary>
    /// Interaction logic for SignInUserControl.xaml
    /// </summary>
    /// <MetaDataID>{146d6493-035f-4dd9-9f86-6e390991ca40}</MetaDataID>
    public partial class SignInUserControl : UserControl
    {
        //string url = "http://localhost:4200/#/signin";
        string url;//= String.Format("file:///{0}/index.html#/signin", App.SignInHtmlPath);

        public WebBrowserOverlay Browser;
        public SignInUserControl()
        {
            url = "file:///C:/Users/jim/AppData/Local/Microsoft/Windows/INetCache/SignInHtml/index.html#/signin";// String.Format("file:///{0}/index.html#/signin", App.SignInHtmlPath);
            url = "http://localhost:4200/#/signin";
            //url = "https://angularhost.z16.web.core.windows.net/SignInApp/#/signin";

            //url = "http://localhost/SignInApp/#/signin";
            InitializeComponent();
            DataContextChanged += SignInUserControl_DataContextChanged;
            SizeChanged += SignInUserControl_SizeChanged;
        }
        private void SignInUserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsVisible)
            {
                if (Browser != null)
                {
                    Browser.Hide();
                    Browser.Show();
                }
            }
        }
        private void SignInUserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (DataContext as ViewModel.SignInUserPopupViewModel).PageLoaded += SignInWindow_PageLoaded;
        }
        internal void ReloadOrigin()
        {
            Browser.Navigate(new Uri(url));
        }
        private void SignInWindow_PageLoaded(object sender, EventArgs e)
        {
            //this.SizeChanged
            WebBrowserHost.HorizontalAlignment = HorizontalAlignment.Stretch;
            WebBrowserHost.VerticalAlignment = VerticalAlignment.Stretch;
            WaitLoading.Visibility = Visibility.Collapsed;

        }
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            if (!DesignerProperties.GetIsInDesignMode(this/*this user control*/))
            {
                Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);
                Browser.Navigate(new Uri(url));
            }
        }
    }
}
