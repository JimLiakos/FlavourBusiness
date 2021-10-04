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
    /// Interaction logic for SignInToolBar.xaml
    /// </summary>
    /// <MetaDataID>{76819ec8-6268-4f25-a2c0-5cc3db4f05f5}</MetaDataID>
    public partial class SignInToolBar : UserControl
    {

        public WebBrowserOverlay Browser;
        public SignInToolBar()
        {
            InitializeComponent();

            Loaded += SignInToolBar_Loaded;
            //Browser.SetVisibility(Visibility.Visible);

            DataContextChanged += SignInToolBar_DataContextChanged;

        }

        double orgWidth;
        ViewModel.SignInUserPopupViewModel SignInUser;
        public SignInWindow SignInWindow { get; private set; }

        //"{\"__type\":\"OOAdvantech.Remoting.RestApi.TypesMetadataCommunicationSession\",\"$value\":{\"$type\":\"OOAdvantech.Remoting.RestApi.TypesMetadataCommunicationSession\",\"ServerSessionPartRef\":{\"__type\":\"OOAdvantech.Remoting.RestApi.ByRef\",\"$value\":{\"$type\":\"OOAdvantech.Remoting.RestApi.ByRef\",\"MembersValues\":{\"__type\":\"Map\",\"$values\":[]},\"Uri\":{\"__type\":\"String\",\"$value\":\"/dd9bc4e5_1b3d_4a09_af2d_a9276d77ed86/wtuoj9okbk2cb5xaemnqafdr_1.rem\"},\"ChannelUri\":{\"__type\":\"String\",\"$value\":\"local-device\"},\"InternalChannelUri\":null,\"TypeName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.ServerSessionPart, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"},\"ReturnTypeMetaData\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"ServerSessionPart\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.ServerSessionPart\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IWeakRefernceEventHandler\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IWeakRefernceEventHandler\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IWeakRefernceEventHandler, OOAdvantech, Version=1.0.2.0, Culture=neutral, PublicKeyToken=f3f71c39187ac643\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IServerSessionPart\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IServerSessionPart\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"ClientProcessTerminates\"},{\"__type\":\"String\",\"$value\":\"GetLifetimeService\"},{\"__type\":\"String\",\"$value\":\"Update\"},{\"__type\":\"String\",\"$value\":\"Subscribe\"},{\"__type\":\"String\",\"$value\":\"GetPendingEvents\"},{\"__type\":\"String\",\"$value\":\"Unsubscribe\"},{\"__type\":\"String\",\"$value\":\"GetObject\"}]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"ServerProcessIdentity\"}]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IServerSessionPart, OOAdvantechFacade, Version=1.0.2.0, Culture=neutral, PublicKeyToken=3586e6bd5f986239\"}}}]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.ServerSessionPart, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"}}}}},\"MarshaledTypes\":{\"__type\":\"Map\",\"$values\":[{\"key\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.ServerSessionPart, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"},\"value\":{\"__type\":\"ref\",\"$value\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"index\":2}}},{\"key\":{\"__type\":\"String\",\"$value\":\"MenuDesigner.ViewModel.SignInUserPopupViewModel, MenuDesigner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"},\"value\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"SignInUserPopupViewModel\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"MenuDesigner.ViewModel.SignInUserPopupViewModel\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"INotifyPropertyChanged\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.ComponentModel.INotifyPropertyChanged\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.ComponentModel.INotifyPropertyChanged, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IUser\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"FlavourBusinessFacade.IUser\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"FullName\"}]},\"Properties\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"ConfirmPassword\"},{\"__type\":\"String\",\"$value\":\"Email\"},{\"__type\":\"String\",\"$value\":\"FullName\"},{\"__type\":\"String\",\"$value\":\"Password\"},{\"__type\":\"String\",\"$value\":\"UserName\"}]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"FlavourBusinessFacade.IUser, FlavourBusinessFacade, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IExtMarshalByRefObject\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IExtMarshalByRefObject\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IExtMarshalByRefObject, OOAdvantechFacade, Version=1.0.2.0, Culture=neutral, PublicKeyToken=3586e6bd5f986239\"}}}]},\"Methods\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"OnPageLoaded\"},{\"__type\":\"String\",\"$value\":\"OnPageSizeChanged\"},{\"__type\":\"String\",\"$value\":\"TonglePopupView\"},{\"__type\":\"String\",\"$value\":\"GetValue\"},{\"__type\":\"String\",\"$value\":\"SetMessage\"},{\"__type\":\"String\",\"$value\":\"SignOut\"},{\"__type\":\"String\",\"$value\":\"SignIn\"},{\"__type\":\"String\",\"$value\":\"SignUp\"},{\"__type\":\"String\",\"$value\":\"SaveUserProfile\"}]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"Address\"},{\"__type\":\"String\",\"$value\":\"PhoneNumber\"},{\"__type\":\"String\",\"$value\":\"UserIdentity\"},{\"__type\":\"String\",\"$value\":\"SignInProvider\"}]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"MenuDesigner.ViewModel.SignInUserPopupViewModel, MenuDesigner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}}}},{\"key\":{\"__type\":\"String\",\"$value\":\"System.Void, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"},\"value\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"Void\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.Void\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.Void, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}}},{\"key\":{\"__type\":\"String\",\"$value\":\"System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"},\"value\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"String\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.String\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IComparable\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.IComparable\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.IComparable, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"ICloneable\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.ICloneable\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.ICloneable, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IConvertible\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.IConvertible\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.IConvertible, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IComparable`1\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.IComparable`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.IComparable`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IEnumerable`1\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.Collections.Generic.IEnumerable`1[[System.Char, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.Collections.Generic.IEnumerable`1[[System.Char, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IEnumerable\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.Collections.IEnumerable\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.Collections.IEnumerable, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IEquatable`1\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.IEquatable`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.IEquatable`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}}]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}}},{\"key\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.RemotingServicesServer, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"},\"value\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"RemotingServicesServer\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.RemotingServicesServer\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IExtMarshalByRefObject\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IExtMarshalByRefObject\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IExtMarshalByRefObject, OOAdvantechFacade, Version=1.0.2.0, Culture=neutral, PublicKeyToken=3586e6bd5f986239\"}}},{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IRomotingServer\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.IRomotingServer\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"CreateInstance\"},{\"__type\":\"String\",\"$value\":\"GetServerSession\"},{\"__type\":\"String\",\"$value\":\"RefreshCacheData\"}]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.IRomotingServer, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"}}}]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.RemotingServicesServer, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"}}}},{\"key\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.DeviceAuthentication, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"},\"value\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"DeviceAuthentication\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.DeviceAuthentication\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"IExtMarshalByRefObject\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IExtMarshalByRefObject\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.IExtMarshalByRefObject, OOAdvantechFacade, Version=1.0.2.0, Culture=neutral, PublicKeyToken=3586e6bd5f986239\"}}}]},\"Methods\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"AuthIDTokenChanged\"},{\"__type\":\"String\",\"$value\":\"GetAuthData\"},{\"__type\":\"String\",\"$value\":\"FromUnixTime\"}]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[{\"__type\":\"String\",\"$value\":\"IDToken\"},{\"__type\":\"String\",\"$value\":\"UnInitialized\"},{\"__type\":\"String\",\"$value\":\"AuthUser\"}]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"OOAdvantech.Remoting.RestApi.DeviceAuthentication, RestApiRemoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=60cdd3bea764f110\"}}}},{\"key\":{\"__type\":\"String\",\"$value\":\"System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"},\"value\":{\"__type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"$value\":{\"$type\":\"OOAdvantech.MetaDataRepository.ProxyType\",\"Name\":{\"__type\":\"String\",\"$value\":\"Object\"},\"FullName\":{\"__type\":\"String\",\"$value\":\"System.Object\"},\"Interfaces\":{\"__type\":\"Array\",\"$values\":[]},\"Methods\":{\"__type\":\"Array\",\"$values\":[]},\"CachingClientSidePropertiesNames\":{\"__type\":\"Array\",\"$values\":[]},\"Properties\":{\"__type\":\"Array\",\"$values\":[]},\"AssemblyQualifiedName\":{\"__type\":\"String\",\"$value\":\"System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"}}}}]}}}"
        private void SignInToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this/*this user control*/))
            {
                Browser = new WebBrowserOverlay(WebBrowserHost, BrowserType.Chrome, true);


                string url = "file:///C:/Users/jim/AppData/Local/Microsoft/Windows/INetCache/SignInHtml/index.html#/signintoolbar";// String.Format("file:///{0}/index.html#/signin", App.SignInHtmlPath);

                url = "http://localhost:4200/#/signintoolbar";
                Browser.Navigate(new Uri(url));

                Browser.DataContexRetrieving += Browser_DataContexRetrieving;
                Browser.SetVisibility(Visibility.Hidden);
                orgWidth = WebBrowserHost.ActualWidth;
                //WebBrowserHost.Width = 1;



                System.Windows.Window win = System.Windows.Window.GetWindow(this);

                if (SignInUser == null)
                {
                    SignInUser = DataContext as ViewModel.SignInUserPopupViewModel;
                    SignInUser.SwitchOnOffPopupView += SignInUser_SwitchOnOffPopupView;
                }

                if (SignInUser != null && SignInWindow == null)
                {
                    SignInWindow = new Views.SignInWindow(this.GetDataContextObject<ViewModel.SignInUserPopupViewModel>());
                    SignInWindow.Owner = win;
                    SignInWindow.PreLoad(PointToScreen(new Point(ActualWidth / 2, ActualHeight + 4)));
                }
            }

            //SignInWindow = new Views.SignInWindow(this.GetDataContextObject<ViewModel.FlavourBusinessManagerViewModel>().SignInUser);
            //SignInWindow.Owner = win;

            //SignInWindow.PreLoad(SignInToolBarItem.PointToScreen(new Point(SignInToolBarItem.ActualWidth / 2, SignInToolBarItem.ActualHeight + 4)));

        }

        private void SignInUser_SwitchOnOffPopupView(object sender, EventArgs e)
        {
            if (SignInWindow.IsVisible)
                SignInWindow.Hide();
            else
                SignInWindow.Show(PointToScreenWithScale(new Point(ActualWidth / 2, ActualHeight + 4)));



        }

        private Point PointToScreenWithScale(Point point)
        {

            double WitdhOnscreen = PointToScreen(new Point(ActualWidth, ActualHeight)).X - PointToScreen(new Point(0, 0)).X;
            double scalex =    ActualWidth/ WitdhOnscreen;
            double HeightonScreen = PointToScreen(new Point(ActualWidth, ActualHeight)).Y - PointToScreen(new Point(0, 0)).Y;
            double scaley = ActualHeight/HeightonScreen  ;

            var newPoint= new Point(PointToScreen(point).X* scalex, PointToScreen(point).Y * scaley);
            return newPoint;
        }

        private void SignInToolBar_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Window win = System.Windows.Window.GetWindow(this);
            ViewModel.SignInUserPopupViewModel signInUser = DataContext as ViewModel.SignInUserPopupViewModel;
        }

        private void Browser_DataContexRetrieving(object sender, object e)
        {
            Browser.SetVisibility(Visibility.Visible);
            //WebBrowserHost.Width = orgWidth;
        }

        public void Redraw()
        {
            Browser.Hide();
            UpdateLayout();
            Browser.Show();
        }

    }
}
