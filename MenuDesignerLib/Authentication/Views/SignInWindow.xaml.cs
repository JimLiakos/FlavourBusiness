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
using System.Windows.Shapes;
using GenWebBrowser;
using FlavourBusinessFacade;
using System.ComponentModel;
using OOAdvantech.Remoting.RestApi;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace FLBAuthentication.Views
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    /// <MetaDataID>{7c1f367e-ada1-4cc9-977a-eca1f189e6a3}</MetaDataID>
    public partial class SignInWindow : Window
    {
        IAuthFlavourBusiness pAuthFlavourBusines;
        WebBrowserOverlay Browser;
        public static SignInWindow Current;
        Point OrgLocation;
        public SignInWindow(ViewModel.SignInUserPopupViewModel signInUserViewModel)
        {
            InitializeComponent();
            Current = this;
            SignInUserViewModel = signInUserViewModel;// new ViewModel.SignInUserPopupViewModel();
            DataContext = SignInUserViewModel;
            SignInUserViewModel.SetHostWindowSize(new Size(this.Width, this.Height));
            SignInUserViewModel.PageLoaded += SignInWindow_PageLoaded;
            SignInUserViewModel.PageSizeChanged += SignInWindow_PageSizeChanged;
            //SignInUserViewModel.SwitchOnOffPopupView += SignInUserViewModel_SwitchOnOffPopupView;
            SizeChanged += SignInWindow_SizeChanged;
            Loaded += SignInWindow_Loaded;
            
        }

        private void SignInUserViewModel_SwitchOnOffPopupView(object sender, EventArgs e)
        {
            if (IsVisible)
                Hide();
        }

        ViewModel.SignInUserPopupViewModel SignInUserViewModel;

        private void SignInWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SignInUserViewModel.SetHostWindowSize(e.NewSize);
        }

        public void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
                Point p = e.GetPosition(this);
                if(p.X<0|| p.X>Width||
                    p.Y<0||p.Y>Height)
                {

                    Hide();
                }
            
        }
        public new void Hide()
        {
            base.Hide();
            SignInUserView.ReloadOrigin();
        }
        

        


        internal void PreLoad(Point location)
        {
            OrgLocation = location;
            if (this.ActualHeight == 0)
            {
                Point arrowOffset = (DataContext as ViewModel.SignInUserPopupViewModel).ArrowOffset;
                this.Left = location.X - arrowOffset.X;
                this.Top = location.Y - arrowOffset.Y-MainGrid.Margin.Top;
                this.Width = (DataContext as ViewModel.SignInUserPopupViewModel).PopupWitdh;
                this.Height = (DataContext as ViewModel.SignInUserPopupViewModel).PopupHeight + 50;
                Visibility = Visibility.Hidden;
                this.Show();
            }
        }
        public void Show(Point location)
        {
            OrgLocation = location;
            var pageSize = new Size((DataContext as ViewModel.SignInUserPopupViewModel).PopupWitdh, (DataContext as ViewModel.SignInUserPopupViewModel).PopupHeight);
            RecalculateWindowPos(pageSize);
            Show();
            Activate();
            UpdateLayout();
            pageSize = new Size(pageSize.Width, pageSize.Height + 1);//force repaint
            RecalculateWindowPos(pageSize);

            //CefSharp.Cef.GetGlobalCookieManager().
        }
   
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hwnd, IntPtr hwndNewParent);

        private const int HWND_MESSAGE = -3;

        private IntPtr hwnd;
        private IntPtr oldParent;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

            if (hwndSource != null)
            {
                hwnd = hwndSource.Handle;
                oldParent = SetParent(hwnd, (IntPtr)HWND_MESSAGE);
                Visibility = Visibility.Hidden;
            }
        }

        private void SignInWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var pageSize = new Size((DataContext as ViewModel.SignInUserPopupViewModel).PopupWitdh, (DataContext as ViewModel.SignInUserPopupViewModel).PopupHeight);
            RecalculateWindowPos(pageSize);
        }
        void RecalculateWindowPos(Size pageSize)
        {
            
            this.Height = (this.ActualHeight - SignInUserView.ActualHeight) + pageSize.Height + 50;
            this.Width = (this.ActualWidth - SignInUserView.ActualWidth) + pageSize.Width;
            Point arrowOffset = (DataContext as ViewModel.SignInUserPopupViewModel).ArrowOffset;
            this.Left = OrgLocation.X - arrowOffset.X;
            this.Top = OrgLocation.Y - arrowOffset.Y - MainGrid.Margin.Top;
        }

       // string url = "http://localhost:4200/#/signin";

        private void SignInWindow_PageSizeChanged(object sender, Size newSize)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (newSize.Height > 100)
                    RecalculateWindowPos(newSize);

            }));
        }
        bool PageLoaded;
        private void SignInWindow_PageLoaded(object sender, EventArgs e)
        {
            if (!PageLoaded)
            {
                Visibility = Visibility.Hidden;
                SetParent(hwnd, oldParent);
                PageLoaded = true;
            }
        }
    }


}
