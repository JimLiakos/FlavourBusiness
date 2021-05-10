using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Iid;
using OOAdvantech.Authentication.Droid;
using Firebase;
using Firebase.Auth;
using Android.Gms.Tasks;
using Android.Gms.Extensions;

namespace ServiceContextManagerApp.Droid
{
    [Activity(Label = "ServiceContextManagerApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, Android.Gms.Tasks.IOnCompleteListener, IOnFailureListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //var options = new FirebaseOptions.Builder()
            //      .SetProjectId("demomicroneme")
            //      .SetApplicationId("1:881594421690:android:83178f67d29ec09d")
            //      .SetApiKey("AIzaSyD8rMRJMQaDZob0bW4QFY2rOxW2s6D2a1Q")
            //      .SetDatabaseUrl("https://demomicroneme.firebaseio.com")
            //      .SetStorageBucket("demomicroneme.appspot.com")
            //      .Build();



            global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::OOAdvantech.Droid.HybridWebViewRenderer.Init();
            global::OOAdvantech.Droid.DeviceInstantiator.Init();

            //App.storage_path = System.IO.Path.Combine((string)global::Android.OS.Environment.ExternalStorageDirectory, "test.txt");
            //try
            //{
            //    System.IO.File.Delete(App.storage_path);
            //}
            //catch (Exception error)
            //{

            //    //throw;
            //}

            //var instanceIdResultTask = FirebaseInstanceId.Instance.GetInstanceId().AsAsync<IInstanceIdResult>();
            //instanceIdResultTask.Wait(3000);
            //var token = instanceIdResultTask.Result.Token;
            var token = FirebaseInstanceId.Instance.Token;

            //OOAdvantech.Droid.DeviceOOAdvantechCore.PrintHashKey(this);
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

        ///Facebook Login button
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            FirebaseAuthentication.OnActivityResult(requestCode, resultCode, data);
        }

        public void OnAuthStateChanged(FirebaseAuth auth)
        {
            var tt = FirebaseAuth.Instance.CurrentUser;
        }

        public void OnComplete(Task task)
        {
            if (task.IsSuccessful)
            {
                // Sign in success, update UI with the signed-in user's information

                FirebaseUser user = FirebaseAuth.Instance.CurrentUser;

            }
            else
            {
                // If sign in fails, display a message to the user.

                Toast.MakeText(this, "Authentication failed.", ToastLength.Short).Show();


            }
        }

        public void OnFailure(Java.Lang.Exception e)
        {

        }

        public override void OnBackPressed()
        {
            OOAdvantech.Droid.HybridWebViewRenderer.BackPressed();

        }

    }
}