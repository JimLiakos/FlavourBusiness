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

    [Application]
    public class DontWaitApplication : Android.App.Application
    {
        public static Context AppContext;

        public DontWaitApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            AppContext = this.ApplicationContext;



            StartPushService();
        }

        public static void StartPushService()
        {
            AppContext.StartService(new Intent(AppContext, typeof(NotificationsService)));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(NotificationsService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public static void StopPushService()
        {
            AppContext.StopService(new Intent(AppContext, typeof(NotificationsService)));

            PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(NotificationsService)), 0);
            AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
            alarm.Cancel(pintent);
        }

    }
}