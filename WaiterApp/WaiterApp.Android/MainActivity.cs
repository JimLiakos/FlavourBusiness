using System;

using Android.App;

using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Iid;
using OOAdvantech.Authentication.Droid;
using Android.Gms.Common;
using Android.Gms.Extensions;
using Android.Support.V4.App;
using Android.Content;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Media;
using Android.Gms.Tasks;
using Firebase.Messaging;
using OOAdvantech.Authentication;
using System.Linq;

namespace WaiterApp.Droid
{
    [Activity(Label = "WaiterApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IOnSuccessListener
    {

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

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
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::OOAdvantech.Droid.HybridWebViewRenderer.Init();
            global::OOAdvantech.Droid.DeviceInstantiator.Init();

            await FirebaseMessaging.Instance.GetToken().AddOnSuccessListener(this, this);


            VivaWalletPos.Android.VivaWalletAppPos.callback = "deliveriescallbackscheme://result";

            IsPlayServicesAvailable();
            CreateNotificationChannel();



            var token = await Task<string>.Run(() =>
            {
                return FirebaseInstanceId.Instance.GetToken("881594421690", "FCM");
            });



            string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";

            var providers = new System.Collections.Generic.List<SignInProvider> { SignInProvider.NativeUser, SignInProvider.Google, SignInProvider.Facebook, SignInProvider.Twitter, SignInProvider.Email };
            OOAdvantech.Droid.DeviceOOAdvantechCore.InitFirebase(this, token, webClientID, providers);


            OOAdvantech.Droid.DeviceOOAdvantechCore.PrintHashKey(this);
            OOAdvantech.Droid.DeviceOOAdvantechCore.ForegroundServiceManager = new Droid.MyForeGroundService();



            LoadApplication(new App());


        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (permissions.Contains(Android.Manifest.Permission.PostNotifications))
                OOAdvantech.Droid.DeviceOOAdvantechCore.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            else
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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

        public override void OnBackPressed()
        {
            OOAdvantech.Droid.HybridWebViewRenderer.BackPressed();
            //base.OnBackPressed();
        }
    }
}