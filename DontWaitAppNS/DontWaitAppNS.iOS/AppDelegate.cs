using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DontWaitApp;
using Firebase.CloudMessaging;
using Foundation;
using ObjCRuntime;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;

namespace DontWaitAppNS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;


            global::Xamarin.Forms.Forms.Init();

            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            global::OOAdvantech.iOS.HybridWebViewRenderer.Init();
            global::OOAdvantech.iOS.DeviceInstantiator.Init();
            //global::OOAdvantech.Pay.iOS.PayService.Init();


            //Firebase.Core.App.Configure();


            LoadApplication(new App());
            RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
        }
        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var error = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unobserved Task Exception:"+ error.Message, error.StackTrace });
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var error = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            OOAdvantech.DeviceApplication.Current.Log(new System.Collections.Generic.List<string>() { "Unhandled Exception:"+ error.Message, error.StackTrace });
        }
        private void RegisterForRemoteNotifications()
        {
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });

                if (Messaging.SharedInstance != null)
                {
                    // For iOS 10 display notification (sent via APNS)
                    UNUserNotificationCenter.Current.Delegate = this;

                    // For iOS 10 data message (sent via FCM)
                    Messaging.SharedInstance.Delegate = this;
                }
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine($"Firebase registration token: {fcmToken}");



            string webClientID = "881594421690-5d71hln1mvtmirn68rdle07p8v45httq.apps.googleusercontent.com";

            //"apps.googleusercontent.com.241222885422-bquei744e1i8q3h0r82k7fm31fbuej7m"

            //
            OOAdvantech.iOS.DeviceOOAdvantechCore.InitFirebase(fcmToken, webClientID);

            // TODO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
        }



        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
        }
        public override void DidEnterBackground(UIApplication uiApplication)
        {
            base.DidEnterBackground(uiApplication);
        }
        [Export("application:didReceiveRemoteNotification:fetchCompletionHandler:")]
        public void DidReceiveRemoteNotification(UIKit.UIApplication application, NSDictionary userInfo, Action<UIKit.UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                OOAdvantech.DeviceApplication.Current.Log(new List<string> { "DidReceiveRemoteNotification DidReceiveRemoteNotification" });

                try
                {
                    // Use default vibration length
                    //Vibration.Vibrate();

                    // Or use specified time
                    var duration = TimeSpan.FromSeconds(7);
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }


            }
            catch (Exception error)
            {


            }
        }

        //[Export("application:didRegisterForRemoteNotificationsWithDeviceToken:")]
        //public void RegisteredForRemoteNotifications(UIKit.UIApplication application, NSData deviceToken)
        //{
        //}

        //[Export("application:didFailToRegisterForRemoteNotificationsWithError:")]
        //public void FailedToRegisterForRemoteNotifications(UIKit.UIApplication application, NSError error)
        //{
        //}

        //public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo,  Action<UIBackgroundFetchResult> completionHandler)
        //{

        //    try
        //    {
        //        OOAdvantech.DeviceApplication.Current.Log(new List<string> { "DidReceiveRemoteNotification DidReceiveRemoteNotification" });
        //        base.DidReceiveRemoteNotification(application, userInfo, completionHandler);
        //    }
        //    catch (Exception error)
        //    {


        //    }
        //}


    }
}
