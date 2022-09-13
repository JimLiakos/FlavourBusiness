using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Firebase.InstanceID;
using Foundation;
using UIKit;

namespace WaiterApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            global::OOAdvantech.iOS.HybridWebViewRenderer.Init();
            global::OOAdvantech.iOS.DeviceInstantiator.Init();


            var coreType = typeof(Firebase.Core.App);
            var authType = typeof(Firebase.Auth.Auth);
            var sdhs = authType.FullName;


            Firebase.Core.App.Configure();
            

            //var token = await Task<string>.Run(() =>
            //{
            //    return FirebaseInstanceId.Instance.GetToken("881594421690", "FCM");
            //});
            //Firebase.InstanceID.InstanceId.SharedInstance.GetToken("881594421690","FCM",default(Dictionary<object, object>),new InstanceIdTokenHandler(InstanceIdToken));
            ////var token = FirebaseInstanceId.Instance.Token;// FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();
            //string token = "";
            //string webClientID = "881594421690-a1j78aqdr924gb82btoboblipfjur9i5.apps.googleusercontent.com";
            //OOAdvantech.iOS.DeviceOOAdvantechCore.InitFirebase( token, webClientID);

            LoadApplication(new App());

            var sds = Firebase.Auth.Auth.DefaultInstance;
            return base.FinishedLaunching(app, options);
        }

        void InstanceIdToken(string token, NSError error)
        {

        }
    }
}
