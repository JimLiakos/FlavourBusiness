using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Firebase.Messaging;

namespace WaiterApp.Droid
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {

        public MyFirebaseMessagingService()
        {
        }
        const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                //App.Current.MainPage.DisplayAlert(message.GetNotification().Title, message.GetNotification().Body,"OK");
                // Code to run on the main thread
            });







            //Log.Debug(TAG, "From: " + message.From);

            //if (message.GetNotification() != null)
            //{
            //    var body = message.GetNotification().Body;
            //    Log.Debug(TAG, "Notification Message Body: " + body);
            //    SendNotification(body, message.Data);
            //}

            var remoteMessage = new OOAdvantech.Droid.RemoteMessage
            {
                MessageId = message.MessageId,
                MessageType = message.MessageType,
                From = message.From,
                To = message.To,
                Data = message.Data,
                SentTime = DateTime.FromFileTime(message.SentTime)
            };

            OOAdvantech.Droid.DeviceOOAdvantechCore.MessageReceived(remoteMessage);
        }

        void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            var pendingIntent = PendingIntent.GetActivity(this, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.Immutable);

            //var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
            //                          .SetSmallIcon(Resource.Drawable.ic_stat_ic_notification)
            //                          .SetContentTitle("FCM Message")
            //                          .SetContentText(messageBody)
            //                          .SetAutoCancel(true)
            //                          .SetContentIntent(pendingIntent);

            //var notificationManager = NotificationManagerCompat.From(this);
            //  notificationManager.Notify(MainActivity.NOTIFICATION_ID, notificationBuilder.Build());
        }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
            OOAdvantech.Droid.DeviceOOAdvantechCore.SetFirebaseToken(p0);
        }




    }

}