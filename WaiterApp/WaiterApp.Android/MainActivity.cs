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
            VivaWalletPos.Android.Pos.callback = "deliveriescallbackscheme://result";

            IsPlayServicesAvailable();
            CreateNotificationChannel();

            //  var token = FirebaseInstanceId.Instance.Token;// FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();


            var token = await Task<string>.Run(() =>
            {
                return FirebaseInstanceId.Instance.GetToken("881594421690", "FCM");
            });

            //Firebase / Authentication / sign-in method / Sign-in providers / Google:Web SDK configuration


            string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";
            //Search for package name
            //"google-services.json   {client[1].oauth_client[where client_type=3].client_id "

            OOAdvantech.Droid.DeviceOOAdvantechCore.InitFirebase(this, token, webClientID);
            OOAdvantech.Droid.DeviceOOAdvantechCore.PrintHashKey(this);

            OOAdvantech.Droid.DeviceOOAdvantechCore.ForegroundServiceManager = new Droid.MyForeGroundService();

            //OOAdvantech.Droid.ForegroundService.ServiceState serviceState = new OOAdvantech.Droid.ForegroundService.ServiceState()
            //{
            //    NotificationTitle = "Δέστω πέστω",
            //    NotificationContentText = "The started service is running.",
            //    NotificationSmallIcon = Resource.Drawable.com_facebook_button_icon,
            //    StopServiceCommandTitle = "Stop Service",
            //    StopServiceCommandIcon = Resource.Drawable.com_facebook_button_icon,
            //    StopActionID = "DestoPesto.action.STOP_SERVICE",
            //    DelayBetweenLogMessage = 5000, // milliseconds,
            //    ServiceRunningNotificationID = 10000,
            //    ActionsMainActivity = "DestoPesto.action.MAIN_ACTIVITY",
            //    ServiceStartedKey = "has_service_been_started",
            //    Terminate = false

            //};
            //serviceState.Runnable = new Action(async () =>
            //{
            //    do
            //    {
            //        System.Threading.Thread.Sleep(1000);

            //    } while (!serviceState.Terminate);
            //});

            //new Droid.MyForeGroundService().StartForegroundService(this, "DestoPesto.action.START_SERVICE", serviceState);

            //Android.Net.Uri uri = RingtoneManager.GetDefaultUri(RingtoneType.Ringtone);
            //Ringtone rt = RingtoneManager.GetRingtone(this.ApplicationContext, uri);
            //rt.Play();


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