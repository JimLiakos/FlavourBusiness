using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DontWaitApp;
using Firebase.CloudMessaging;
using Foundation;
using UIKit;
using UserNotifications;
using Xamarin.Forms.PlatformConfiguration;

namespace DontWaitAppNS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
        }

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
            global::OOAdvantech.Pay.iOS.PayService.Init();



            // Firebase.Core.App.Configure();




            LoadApplication(new App());

            RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
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



            string webClientID = "881594421690-dj2nfdnrdhhmp2bhhkc4nuqapno43kbv.apps.googleusercontent.com";
            //"apps.googleusercontent.com.241222885422-bquei744e1i8q3h0r82k7fm31fbuej7m"

            //
            OOAdvantech.iOS.DeviceOOAdvantechCore.InitFirebase(fcmToken, webClientID);

            // TODO: If necessary send token to application server.
            // Note: This callback is fired at each app startup and whenever a new token is generated.
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

        //internal static void LogUnhandledException(Exception exception)
        //{
        //    try
        //    {
        //        const string errorFileName = "Fatal.log";
        //        var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
        //        var errorFilePath = Path.Combine(libraryPath, errorFileName);
        //        var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
        //        DateTime.Now, exception.ToString());
        //        File.WriteAllText(errorFilePath, errorMessage);


        //    }
        //    catch
        //    {
        //        // just suppress any error logging exceptions
        //    }
        //}


        //iOS: Different than Android. Must be in FinishedLaunching, not in Main.
        // In AppDelegate

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private static void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            var alertView = new UIAlertView("Crash Report", errorText, null, "Close", "Clear") { UserInteractionEnabled = true };
            alertView.Clicked += (sender, args) =>
            {
                if (args.ButtonIndex != 0)
                {
                    File.Delete(errorFilePath);
                }
            };
            alertView.Show();
        }



        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            App myApp = App.Current as App;
            if (null != myApp && null != url)
            {
                myApp.iOSOnAppLinkRequestReceived(new Uri(url.AbsoluteString));
            }
            return true;
            //return base.OpenUrl(app, url, options);
        }


    }

    public class MyUNUserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        bool toggle;
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            if (toggle)
                completionHandler(UNNotificationPresentationOptions.Alert);
            else
            {
                Console.WriteLine(notification);
                completionHandler(UNNotificationPresentationOptions.None);
            }
            toggle = !toggle;
        }
    }


}
