using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OOAdvantech;
using OOAdvantech.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DontWaitAppNS.Droid
{
    /// <MetaDataID>{52453757-2621-49c4-a293-43e18ae14e6a}</MetaDataID>

    [Service(Name = "com.microneme.dontwait.android.MyForeGroundService")]
    public class MyForeGroundService : ForegroundService, IBackgroundService
    {
        public bool IsServiceStarted
        {
            get
            {
                return isStarted;
            }
        }

        public bool Run(Action action, BackgroundServiceState backgroundServiceState)
        {
            ForegroundService.ServiceState serviceState = new ForegroundService.ServiceState()
            {
                NotificationTitle = "Waiter app",
                NotificationContentText = "The started service is running.",
                NotificationSmallIcon = Resource.Drawable.com_facebook_button_icon,
                StopServiceCommandTitle = "Stop Service",
                StopServiceCommandIcon = Resource.Drawable.com_facebook_button_icon,
                StopActionID = "dontwaitwaiterapp.action.STOP_SERVICE",
                DelayBetweenLogMessage = 5000, // milliseconds,
                ServiceRunningNotificationID = 10000,
                ActionsMainActivity = "dontwaitwaiterapp.action.MAIN_ACTIVITY",
                ServiceStartedKey = "has_service_been_started",
                BackgroundServiceState = backgroundServiceState,
                Terminate = false

            };
            serviceState.Runnable = action;

            StartForegroundService(this, "dontwaitwaiterapp.action.START_SERVICE", serviceState);
            return true;
        }

        public void Stop()
        {

        }
    }
}
