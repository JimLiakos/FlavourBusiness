using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Threading.Tasks;
using Firebase.Messaging;
using Android.Gms.Extensions;
using OOAdvantech.Authentication.Droid;
using OOAdvantech.Authentication;

namespace CourierApp.Droid
{
    [Activity(Label = "CourierApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {
              //  OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string> { "OnCreate" });

                AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
                TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;


                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


                global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
                global::OOAdvantech.Droid.HybridWebViewRenderer.Init();
                global::OOAdvantech.Droid.DeviceInstantiator.Init();


                string token = (await FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>()).ToString();




                string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";
                //Search for package name
                //"google-services.json   {client[1].oauth_client[where client_type=3].client_id "
                var providers = new System.Collections.Generic.List<SignInProvider> { SignInProvider.NativeUser, SignInProvider.Google, SignInProvider.Facebook, SignInProvider.Twitter, SignInProvider.Email };

                OOAdvantech.Droid.DeviceOOAdvantechCore.InitFirebase(this, token, webClientID, providers);
                OOAdvantech.Droid.DeviceOOAdvantechCore.PrintHashKey(this);
            }
            catch (Exception error)
            {

                
            }

           // OOAdvantech.Droid.DeviceOOAdvantechCore.ForegroundServiceManager = new Droid.MyForeGroundService();
            LoadApplication(new App());
            
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            FirebaseAuthentication.OnActivityResult(requestCode, resultCode, data);
        }


        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var error = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unobserved Task Exception:" + error.Message, error.StackTrace });
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var error = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unhandled Exception:" + error.Message, error.StackTrace });
        }
    }
}