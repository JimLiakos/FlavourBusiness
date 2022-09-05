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
using Firebase.Iid;

namespace DontWaitAppNS.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService //: FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";

        //public override void OnTokenRefresh()
        //{
            
        //    var refreshedToken = FirebaseInstanceId.Instance.Token;
        //    Log.Debug(TAG, "Refreshed token: " + refreshedToken);
        //    SendRegistrationToServer(refreshedToken);
            
        //    OOAdvantech.Droid.DeviceOOAdvantechCore.SetFirebaseToken(refreshedToken);


        //    //dJOrurHBnCQ:APA91bG5QmbDJkEaQmP05B1k0sCnmf15bRAQyt8vJufFEsiYfIZdvhpzVAmvXt5C1KQdJllo2AEJ4i2mb5e-494NQ0mi0euo8Oiboo2vRvWNvHUZPUGxmHR9E_Xjrc7YCq82VY6Ld_Qn
        //}

        void SendRegistrationToServer(string token)
        {
            // Add custom implementation, as needed.
        }
    }
}