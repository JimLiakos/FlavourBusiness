using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.OData.UriParser;

namespace FlavourBusinessManager
{


    public class CloudNotificationManager
    {
        static CloudNotificationManager()
        {


        }


        public static async void SendMessage(FlavourBusinessFacade.EndUsers.Message message, string deviceFirebaseToken)
        {
            await FireBase.Init();
            //Documantation
            //https://firebase.google.com/docs/cloud-messaging/concept-options#setting-the-priority-of-a-message
            var fireBaseMessage = new Message()
            {
                Data = new Dictionary<string, string>()
                {

                    {"MessageID",message.MessageID },
                    {"MessageTimestamp",message.MessageTimestamp.ToString("u") },
                    {"ServicesContextIdentity",ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity }

                },
                Notification = new Notification() { Body = message.Notification.Body, Title = message.Notification.Title },
                Token = deviceFirebaseToken,
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    //Notification = new AndroidNotification
                    //{
                    //    Body = message.Notification.Body,
                    //    Title = message.Notification.Title,
                    //    DefaultSound = true
                    //}
                },

            };

            //if (message.Notification != null)
            //{
            //    AndroidConfig androidConfig = new AndroidConfig() { Notification = new AndroidNotification { Title = message.Notification.Title, Body = message.Notification.Body,ClickAction= ".MainActivity" } };
            //    fireBaseMessage.Android= androidConfig;//.Notification = new Notification() { Body = message.Notification.Body, Title = message.Notification.Title };
            //}

            // Send a message to the device corresponding to the provided
            // registration token.
            try
            {
                string response = await FireBase.FirebaseMessaging.SendAsync(fireBaseMessage);
            }
            catch (Exception error)
            {


            }


        }


    }


}
