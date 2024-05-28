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

namespace PreparationStationDevice.Droid
{

    [Service(Name = "com.microneme.preparationstationdevice.ForeGroundService", ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
    public class ForeGroundService : ForegroundService, IBackgroundService
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
                NotificationTitle = "Items preparation app",
                NotificationContentText = "The started service is running.",
                NotificationSmallIcon = Resource.Drawable.com_facebook_button_icon,
                StopServiceCommandTitle = "Stop Service",
                StopServiceCommandIcon = Resource.Drawable.com_facebook_button_icon,
                StopActionID = "preparationstationdevice.action.STOP_SERVICE",
                DelayBetweenLogMessage = 5000, // milliseconds,
                ServiceRunningNotificationID = 10000,
                ActionsMainActivity = "preparationstationdevice.action.MAIN_ACTIVITY",
                ServiceStartedKey = "has_service_been_started",
                BackgroundServiceState = backgroundServiceState,
                Terminate = false

            };
            serviceState.Runnable = action;

            try
            {
                StartForegroundService(this, "preparationstationdevice.action.START_SERVICE", serviceState);
            }
            catch (Exception error)
            {

                throw;
            }
            return true;
        }

        public void Stop()
        {

        }
    }
}