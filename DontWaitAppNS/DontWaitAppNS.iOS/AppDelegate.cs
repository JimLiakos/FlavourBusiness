using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DontWaitApp;
using Foundation;
using UIKit;

namespace DontWaitAppNS.iOS
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;


            global::Xamarin.Forms.Forms.Init();

            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            global::OOAdvantech.iOS.HybridWebViewRenderer.Init();
            global::OOAdvantech.iOS.DeviceInstantiator.Init();

            LoadApplication(new App());

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

    }
}
