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
	public static class Constants
	{

		public const int DELAY_BETWEEN_LOG_MESSAGES = 5000; // milliseconds
		public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
		public const string SERVICE_STARTED_KEY = "has_service_been_started";
		public const string BROADCAST_MESSAGE_KEY = "broadcast_message";
		public const string NOTIFICATION_BROADCAST_ACTION = "DontWaitAppNS.Notification.Action";

		public const string ACTION_START_SERVICE = "DontWaitAppNS.action.START_SERVICE";
		public const string ACTION_STOP_SERVICE = "DontWaitAppNS.action.STOP_SERVICE";
		public const string ACTION_RESTART_TIMER = "DontWaitAppNS.action.RESTART_TIMER";
		public const string ACTION_MAIN_ACTIVITY = "DontWaitAppNS.action.MAIN_ACTIVITY";

	}
}