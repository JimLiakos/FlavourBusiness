using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Firebase.Messaging;
using Android.Gms.Extensions;
using Firebase;

namespace TakeAwayApp.Droid
{
    [Activity(Label = "TakeAwayApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::OOAdvantech.Droid.HybridWebViewRenderer.Init();
            global::OOAdvantech.Droid.DeviceInstantiator.Init();
            UserDialogs.Init(this);
            FirebaseApp.InitializeApp(this);

            //var token = (await FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>()).ToString(); 


            ////var token = FirebaseInstanceId.Instance.Token;

            ////OOAdvantech.Droid.DeviceOOAdvantechCore.PrintHashKey(this);
            //string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";
            //OOAdvantech.Droid.DeviceOOAdvantechCore.InitFirebase(this, token, webClientID);
            //OOAdvantech.Droid.DeviceOOAdvantechCore.PrintHashKey(this);


            LoadApplication(new App());

            //Keyboard-overlapping
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}