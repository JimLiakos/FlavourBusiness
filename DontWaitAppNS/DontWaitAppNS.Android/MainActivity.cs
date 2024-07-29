

using Android.App;
using Android.Runtime;
using Android.OS;
using Firebase.Iid;
using OOAdvantech.Authentication.Droid;
using Android.Gms.Common;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using System.Threading.Tasks;
using Android.Content.PM;
using Firebase.Messaging;
using OOAdvantech.Authentication;
using System;
using Acr.UserDialogs;
using Firebase;
using Xamarin.Forms.Platform.Android.AppLinks;
using System.Linq;


//using Android.App;
//using Android.Runtime;
//using Android.OS;
//using Firebase.Iid;
//using OOAdvantech.Authentication.Droid;
//using Android.Gms.Common;
//using Android.Gms.Extensions;
//using System.Threading.Tasks;
//using Android.Content.PM;
//using Android.Gms.Tasks;
//using Firebase.Messaging;
//using OOAdvantech.Authentication;

namespace DontWaitAppNS.Droid
{
    [Activity(Label = "DontWaitAppNS", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]

    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    DataScheme = "http",
    DataHost = "10.0.0.13",
    DataPathPrefix = "/",
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    DataScheme = "http",
    DataHost = "192.168.2.8",
    DataPathPrefix = "/",
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
    DataScheme = "https",
    DataHost = "dontwait.com",
    DataPathPrefix = "/",
    Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnSuccessListener
    {

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;


        string msgText;






        protected async override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;



            //this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(this);


            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::OOAdvantech.Droid.HybridWebViewRenderer.Init();
            global::OOAdvantech.Droid.DeviceInstantiator.Init();

            

            IsPlayServicesAvailable();
            CreateNotificationChannel();
            AndroidAppLinks.Init(this);
            OOAdvantech.Droid.DeviceOOAdvantechCore.ForegroundServiceManager = new Droid.MyForeGroundService();
            LoadApplication(new DontWaitApp.App());

            await FirebaseMessaging.Instance.GetToken().AddOnSuccessListener(this, this);
            var token = await Task<string>.Run(() =>
            {
                return FirebaseInstanceId.Instance.GetToken("881594421690", "FCM");
            });

            string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";
            OOAdvantech.Droid.DeviceOOAdvantechCore.InitFirebase(this, token, webClientID);

            
            
            

          

          

            


        }



        protected override void OnStart()
        {
            base.OnStart();
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))

                    msgText = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    msgText = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                msgText = "Google Play Services is available.";
                return true;
            }
        }
        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var error = new System.Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unobserved Task Exception:" + error.Message, error.StackTrace });
        }

        private static void CurrentDomainOnUnhandledException(object sender, System.UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var error = new System.Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as System.Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unhandled Exception:" + error.Message, error.StackTrace });
        }
        Xamarin.Forms.AppLinkEntry GetAppLink()
        {
            var pageType = GetType().ToString();
            var pageLink = new Xamarin.Forms.AppLinkEntry
            {
                Title = "Don't Wait",
                Description = "Don't Wait",
                AppLinkUri = new System.Uri($"http://dontwait/invitation?id=0", System.UriKind.RelativeOrAbsolute),
                IsLinkActive = true,
                //Thumbnail = ImageSource.FromFile("monkey.png")
            };

            pageLink.KeyValues.Add("contentType", "TodoItemPage");
            pageLink.KeyValues.Add("appName", "dontwait");
            pageLink.KeyValues.Add("companyName", "Xamarin");

            return pageLink;
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (permissions.Contains(Android.Manifest.Permission.PostNotifications))
            {
                OOAdvantech.Droid.DeviceOOAdvantechCore.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            }
            else
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            FirebaseAuthentication.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            OOAdvantech.Droid.DeviceOOAdvantechCore.OnDestroy();
            base.OnDestroy();
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            string token = result.ToString();
            OOAdvantech.Droid.DeviceOOAdvantechCore.SetFirebaseToken(token);
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);


        }


        public override void OnBackPressed()
        {
            OOAdvantech.Droid.HybridWebViewRenderer.BackPressed();
            //base.OnBackPressed();
        }
    }
}

