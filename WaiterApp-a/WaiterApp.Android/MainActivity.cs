using System;

using Android.App;
using Android.Content.PM;
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

namespace WaiterApp.Droid
{
    [Activity(Label = "WaiterApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
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
                    msgText= "This device is not supported";
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
        protected  override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
       

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::OOAdvantech.Droid.HybridWebViewRenderer.Init();
            global::OOAdvantech.Droid.DeviceInstantiator.Init();


            IsPlayServicesAvailable();
            CreateNotificationChannel();

            var token = FirebaseInstanceId.Instance.Token;// FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();

            string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";
            OOAdvantech.Droid.DeviceOOAdvantechCore.InitFirebase(this, token, webClientID);
            OOAdvantech.Droid.DeviceOOAdvantechCore.PrintHashKey(this);



         

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

  

        public override void OnBackPressed()
        {
            OOAdvantech.Droid.HybridWebViewRenderer.BackPressed();
            base.OnBackPressed();
        }
    }
}