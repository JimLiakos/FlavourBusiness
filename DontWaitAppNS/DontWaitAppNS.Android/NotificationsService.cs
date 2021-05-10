using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DontWaitAppNS.Droid
{
    [Service]
    public class NotificationsService : Service
    {

        public override void OnCreate()
        {
            base.OnCreate();

            Toast.MakeText(this, "Notifications Service - Created", ToastLength.Long);
        }

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            Toast.MakeText(this, "Notifications Service - Started", ToastLength.Long);
            return StartCommandResult.Sticky;
        }

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            Toast.MakeText(this, "Notifications Service - Binded", ToastLength.Long);
            return null;
        }


        public override void OnDestroy()
        {
            System.Diagnostics.Debug.WriteLine("Notifications Service destroyed");
            base.OnDestroy();
        }
    }
}