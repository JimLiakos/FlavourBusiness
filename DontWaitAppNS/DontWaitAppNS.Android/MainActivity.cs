using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;
using Java.Util;
using Android;
using Plugin.Permissions;
using Android.Util;
using Firebase.Iid;
using Android.Content;
using System.Linq;
using Android.Graphics;
using Acr.UserDialogs;
using Firebase;
using Xamarin.Forms.Platform.Android.AppLinks;
using Xamarin.Forms;
using System.Threading.Tasks;
using OOAdvantech;
using Firebase.Messaging;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using OOAdvantech.Authentication.Droid;
using Android.Gms.Common;

namespace DontWaitAppNS.Droid
{
    //Keyboard-overlapping https://devlinduldulao.pro/how-to-fix-keyboard-overlapping-or-covering-entry/
    [Activity(Label = "DontWaitAppNS", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]

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

            await FirebaseMessaging.Instance.GetToken().AddOnSuccessListener(this, this);

            IsPlayServicesAvailable();
            CreateNotificationChannel();


            var token = await Task<string>.Run(() =>
            {
                return FirebaseInstanceId.Instance.GetToken("881594421690", "FCM");
            });

            string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";
            OOAdvantech.Droid.DeviceOOAdvantechCore.InitFirebase(this, token, webClientID);

            FirebaseApp.InitializeApp(this);
            AndroidAppLinks.Init(this);
            OOAdvantech.Droid.DeviceOOAdvantechCore.ForegroundServiceManager = new Droid.MyForeGroundService();


            getDeviceUniqueID();
            
            
            LoadApplication(new DontWaitApp.App());

            
    


        }
        string msgText;

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
            var error = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unobserved Task Exception:" + error.Message, error.StackTrace });
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var error = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unhandled Exception:" + error.Message, error.StackTrace });
        }
        Xamarin.Forms.AppLinkEntry GetAppLink()
        {
            var pageType = GetType().ToString();
            var pageLink = new AppLinkEntry
            {
                Title = "Don't Wait",
                Description = "Don't Wait",
                AppLinkUri = new Uri($"http://dontwait/invitation?id=0", UriKind.RelativeOrAbsolute),
                IsLinkActive = true,
                //Thumbnail = ImageSource.FromFile("monkey.png")
            };

            pageLink.KeyValues.Add("contentType", "TodoItemPage");
            pageLink.KeyValues.Add("appName", "dontwait");
            pageLink.KeyValues.Add("companyName", "Xamarin");

            return pageLink;
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
            BackPressedArgs eventArgs = new BackPressedArgs();
            OOAdvantech.Droid.DeviceOOAdvantechCore.BackPressed(eventArgs);

            base.OnBackPressed();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



        public static String getDeviceUniqueID()
        {
            // If all else fails, if the user does have lower than API 9 (lower
            // than Gingerbread), has reset their phone or 'Secure.ANDROID_ID'
            // returns 'null', then simply the ID returned will be solely based
            // off their Android device information. This is where the collisions
            // can happen.
            // Thanks http://www.pocketmagic.net/?p=1662!
            // Try not to use DISPLAY, HOST or ID - these items could change.
            // If there are collisions, there will be overlapping data
            String m_szDevIDShort = "35" +
                    (Build.Board.Length % 10)
                    + (Build.Brand.Length % 10)
                    + (Build.CpuAbi.Length % 10)
                    + (Build.Device.Length % 10)
                    + (Build.Manufacturer.Length % 10)
                    + (Build.Model.Length % 10)
                    + (Build.Product.Length % 10);

            // Thanks to @Roman SL!
            // http://stackoverflow.com/a/4789483/950427
            // Only devices with API >= 9 have android.os.Build.SERIAL
            // http://developer.android.com/reference/android/os/Build.html#SERIAL
            // If a user upgrades software or roots their phone, there will be a duplicate entry
            String serial = null;
            try
            {
                serial = Build.Serial;

                // Go ahead and return the serial for api => 9
                return new UUID(m_szDevIDShort.GetHashCode(), serial.GetHashCode()).ToString();
            }
            catch (Exception e)
            {
                // String needs to be initialized
                serial = "serial"; // some value
            }

            // Thanks @Joe!
            // http://stackoverflow.com/a/2853253/950427
            // Finally, combine the values we have found by using the UUID class to create a unique identifier

            // DebugLog..LOGE(new UUID(m_szDevIDShort.hashCode(), serial.hashCode()).toString());

            return new UUID(m_szDevIDShort.GetHashCode(), serial.GetHashCode()).ToString();
        }
        public void GetDeviceData()
        {
            //try
            //{
            //    var telephonyDeviceID = string.Empty;
            //    var telephonySIMSerialNumber = string.Empty;
            //    TelephonyManager telephonyManager = (TelephonyManager)ApplicationContext.GetSystemService(Context.TelephonyService);
            //    if (telephonyManager != null)
            //    {
            //        if (!string.IsNullOrEmpty(telephonyManager.DeviceId))
            //            telephonyDeviceID = telephonyManager.DeviceId;
            //        if (!string.IsNullOrEmpty(telephonyManager.SimSerialNumber))
            //            telephonySIMSerialNumber = telephonyManager.SimSerialNumber;
            //    }
            //    var androidID = Android.Provider.Settings.Secure.GetString(ApplicationContext.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            //    var deviceUuid = new UUID(androidID.GetHashCode(), ((long)telephonyDeviceID.GetHashCode() << 32) | telephonySIMSerialNumber.GetHashCode());
            //    var deviceID = deviceUuid.ToString();
            //}
            //catch (System.Exception error)
            //{
            //}
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    // Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                }
            }
        }
        protected override void OnStart()
        {
            base.OnStart();
            const int requestLocationId = 0;

            string[] notiPermission =
            {
                Android.Manifest.Permission.PostNotifications
            };

            if ((int)Build.VERSION.SdkInt < 33) return;

            if (this.CheckSelfPermission(Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                this.RequestPermissions(notiPermission, requestLocationId);
            }
        }
    
        public void OnSuccess(Java.Lang.Object result)
        {
            string token = result.ToString();
            OOAdvantech.Droid.DeviceOOAdvantechCore.SetFirebaseToken(token);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            FirebaseAuthentication.OnActivityResult(requestCode, resultCode, data);
        }

    }
}