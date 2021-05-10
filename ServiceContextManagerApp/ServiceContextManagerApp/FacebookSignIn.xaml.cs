using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OOAdvantech.Authentication.Facebook.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ServiceContextManagerApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacebookSignIn : ContentPage,System.ComponentModel.INotifyPropertyChanged
    {
        public FacebookSignIn()
        {
            InitializeComponent();
            OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthStateChanged += DeviceAuthentication_AuthStateChanged;

            facebookLoginService = FacebookLoginService.CurrentFacebookLoginService;
            facebookLoginService.AccessTokenChanged = (string oldToken, string newToken) => FacebookLogoutCmd.ChangeCanExecute();

            FacebookLogoutCmd = new Command(() =>
                facebookLoginService.SignOut(),
                () => !string.IsNullOrEmpty(facebookLoginService.AccessToken));

            OnFacebookLoginSuccessCmd = new Command<string>(
                (authToken) => DisplayAlert("Success", $"Authentication succeed: {authToken}"));

            OnFacebookLoginErrorCmd = new Command<string>(
                (err) => DisplayAlert("Error", $"Authentication failed: {err}"));

            OnFacebookLoginCancelCmd = new Command(
                () => DisplayAlert("Cancel", "Authentication cancelled by the user."));

            BindingContext = this;
        }

        private void DeviceAuthentication_AuthStateChanged(object sender, OOAdvantech.Remoting.RestApi.AuthUser e)
        {
            this.OnPropertyChanged(nameof(StateTitle));
        }

        public string StateTitle
        {
            get
            {
                if (OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser != null)
                    return "SignIn with " + OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser.Name + " " + OOAdvantech.Remoting.RestApi.DeviceAuthentication.AuthUser.Firebase_Sign_in_Provider;
                else
                    return "SignOut";
            }

        }


        readonly FacebookLoginService facebookLoginService;

        public ICommand OnFacebookLoginSuccessCmd { get; }
        public ICommand OnFacebookLoginErrorCmd { get; }
        public ICommand OnFacebookLoginCancelCmd { get; }
        public Command FacebookLogoutCmd { get; }

    

        void DisplayAlert(string title, string msg) =>
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "OK");


        
        private void Facebook_Clicked(object sender, EventArgs e)
        {
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.Signin(OOAdvantech.AuthProvider.Facebook);
            
        }


        private void Google_Clicked(object sender, EventArgs e)
        {
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.Signin(OOAdvantech.AuthProvider.Google);
        }

        private void SignOutAll_Clicked(object sender, EventArgs e)
        {
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
            device.SignOut();
        }
    }
}