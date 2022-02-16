using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{84732946-5b28-4f05-882e-9340b767c79e}</MetaDataID>
    public class CloudNotificationManager
    {

        static FirebaseApp FirebaseApp;
        static CloudNotificationManager()
        {
            Credential credential = new Credential()
            {
                type = "service_account",
                project_id = "demomicroneme",
                private_key_id = "def7e2ba0e082086400e2953cf65fc6dfe5216d0",
                private_key = "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDDvR7eRsEgyHRh\nJbSzbygIwbjGbEfAKb3gyicjiceyZuaV7ss/IGOCFhmR1f9rrjZmjOt642s/81hk\nYKLni2ap7VgT+GWIaMF7g4kUu87plk4d3zV89Tih52s2ppSsj7nLmVdqsE4QjBmN\nVpKQ2nZEYxPJfefHE8B5V+EO7/Tiv3Q3qNDAbWVlpNfb1LTq79YDU7iLD9U2PUIL\nwdN3/fU6SChjz6cyb+3kWwtTaarUPBN54bCAG1ka5w8ol3guINOb9U4hmCvE+Cvz\ngPbmgiHxgNoRMzHmMTGM9Ycr+uC0Zja5KhHUPLKTaF/CHIOOE+eHdfSpngduRoW1\n8InGK1WlAgMBAAECggEAMmNyGdhvCShxRTz2qqZ30OFF1tazFdXpCoAf2Tcz0EpL\nG9fQPJzy4N8dj/xd93Nuj7HBQO5ggqL7Y0O5TBAHysDNxr5QLPCCtnAjDtJWLq3B\nyFDYrSVXgd5YLEZvyYhqVO5RoaZnQj0+qrLZoi6K+Ynj4x/lVctQ5ivoRPcivGgH\njXkEERmIsZsSvmuLx6RwncMAdkZAyLRbz+mPGGWFrZR4b0IegBGItAR9LbZE4PPl\npgRGy78uNxqXZP0zsZfPTpWoPKBiivIcuIsyM9+N6zMAK/x5HBkK6mg1E8EBHqFb\nRvqQs3nvBtcY34QkWP1WRGq/cgrna5XiVQ27rEGsFQKBgQDm1grkihaEQeFYChF5\nNlSdbseUTKpRfUJ2VdVdh0GG6FaJ+79zTmO4ogg8u8dMWkav5XWDA4N9q3HFlaSe\nHOD0IUCK1U9zBvYFJ3GeDaQlM4AFcqEYlO296+OYMjZRPVpMoJ5G9nojV89OHJv3\nxY+WPWNhJXNdEJ4AJSCXGvefXwKBgQDZE50zixePxldRm294StNFDNo5Md3+tU4v\nHHa16hxf8uRPnLlGUweRemNrZaT4UmdGkYaFxLsH77I/s77mplN6sBayyM6wpkQx\nl00jqUzpgV6KEeXy42bSbebNPph/Q8gadySZYiDIPq0mACqWaxCwIWAxczoqkDK6\nPEbyQlwdewKBgQCPnJjYSITrsaUFxfXLCJ8p9xLZ07yeyCRCRPJiptSAnym/3Mzm\nat2lr8EaL+U1PnD92+75HIWA+NnmiEwLRoI5wDpMZZtxP+JtoHWSVIBL2LeMLB3H\nklg6sXg+ZvbeIiJ8y+zMz2l7dZT2ztvGEbZcTUL33Hnia4UxJ+gXumJWVwKBgD7G\nNUeaiY3CRa4LzQh0WvQ060Zu7UujEqD9Ejc5JEt66hs7rzhu+llPk0CTfElzSvpV\nSxmT8qIw5tMVH7eDkdCA6494Eo1zB3Vv05bkdqFwD+7NjjnXGPzxWzUvTNpAt7Uv\njx3sCp7dwSSkF6y3+XN1s2OZdtCoMoM4uyuDlS/RAoGAUvjqDDD/3GWiYDtQdY9Y\nEh2fzyf8C0nsNngkseWOmsPFPxZ66tJTxxTqCihAD6CaXPFnEUiSdlrgG8KG+lO6\nrebFITC6riiripgKO9Ss2yx7wfpbCAYzpysbY0vVjPYS28lhrq3gk1eg5pQ1Ol1j\nnpwIXbvgpr83/rjNxP9VM9E=\n-----END PRIVATE KEY-----\n",
                client_email= "firebase-adminsdk-ggc6a@demomicroneme.iam.gserviceaccount.com",
                client_id = "112667768908373030776",
                auth_uri = "https://accounts.google.com/o/oauth2/auth",
                token_uri = "https://oauth2.googleapis.com/token",
                auth_provider_x509_cert_url = "https://www.googleapis.com/oauth2/v1/certs",
                client_x509_cert_url = "https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-ggc6a%40demomicroneme.iam.gserviceaccount.com"
            };

            FirebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(credential))
            });
//                FirebaseApp = FirebaseApp.Create(new AppOptions()
//            {
//                Credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromJson(@"{ ""type"": ""service_account"",
//  ""project_id"": ""demomicroneme"",
//  ""private_key_id"": ""def7e2ba0e082086400e2953cf65fc6dfe5216d0"",
//  ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDDvR7eRsEgyHRh\nJbSzbygIwbjGbEfAKb3gyicjiceyZuaV7ss/IGOCFhmR1f9rrjZmjOt642s/81hk\nYKLni2ap7VgT+GWIaMF7g4kUu87plk4d3zV89Tih52s2ppSsj7nLmVdqsE4QjBmN\nVpKQ2nZEYxPJfefHE8B5V+EO7/Tiv3Q3qNDAbWVlpNfb1LTq79YDU7iLD9U2PUIL\nwdN3/fU6SChjz6cyb+3kWwtTaarUPBN54bCAG1ka5w8ol3guINOb9U4hmCvE+Cvz\ngPbmgiHxgNoRMzHmMTGM9Ycr+uC0Zja5KhHUPLKTaF/CHIOOE+eHdfSpngduRoW1\n8InGK1WlAgMBAAECggEAMmNyGdhvCShxRTz2qqZ30OFF1tazFdXpCoAf2Tcz0EpL\nG9fQPJzy4N8dj/xd93Nuj7HBQO5ggqL7Y0O5TBAHysDNxr5QLPCCtnAjDtJWLq3B\nyFDYrSVXgd5YLEZvyYhqVO5RoaZnQj0+qrLZoi6K+Ynj4x/lVctQ5ivoRPcivGgH\njXkEERmIsZsSvmuLx6RwncMAdkZAyLRbz+mPGGWFrZR4b0IegBGItAR9LbZE4PPl\npgRGy78uNxqXZP0zsZfPTpWoPKBiivIcuIsyM9+N6zMAK/x5HBkK6mg1E8EBHqFb\nRvqQs3nvBtcY34QkWP1WRGq/cgrna5XiVQ27rEGsFQKBgQDm1grkihaEQeFYChF5\nNlSdbseUTKpRfUJ2VdVdh0GG6FaJ+79zTmO4ogg8u8dMWkav5XWDA4N9q3HFlaSe\nHOD0IUCK1U9zBvYFJ3GeDaQlM4AFcqEYlO296+OYMjZRPVpMoJ5G9nojV89OHJv3\nxY+WPWNhJXNdEJ4AJSCXGvefXwKBgQDZE50zixePxldRm294StNFDNo5Md3+tU4v\nHHa16hxf8uRPnLlGUweRemNrZaT4UmdGkYaFxLsH77I/s77mplN6sBayyM6wpkQx\nl00jqUzpgV6KEeXy42bSbebNPph/Q8gadySZYiDIPq0mACqWaxCwIWAxczoqkDK6\nPEbyQlwdewKBgQCPnJjYSITrsaUFxfXLCJ8p9xLZ07yeyCRCRPJiptSAnym/3Mzm\nat2lr8EaL+U1PnD92+75HIWA+NnmiEwLRoI5wDpMZZtxP+JtoHWSVIBL2LeMLB3H\nklg6sXg+ZvbeIiJ8y+zMz2l7dZT2ztvGEbZcTUL33Hnia4UxJ+gXumJWVwKBgD7G\nNUeaiY3CRa4LzQh0WvQ060Zu7UujEqD9Ejc5JEt66hs7rzhu+llPk0CTfElzSvpV\nSxmT8qIw5tMVH7eDkdCA6494Eo1zB3Vv05bkdqFwD+7NjjnXGPzxWzUvTNpAt7Uv\njx3sCp7dwSSkF6y3+XN1s2OZdtCoMoM4uyuDlS/RAoGAUvjqDDD/3GWiYDtQdY9Y\nEh2fzyf8C0nsNngkseWOmsPFPxZ66tJTxxTqCihAD6CaXPFnEUiSdlrgG8KG+lO6\nrebFITC6riiripgKO9Ss2yx7wfpbCAYzpysbY0vVjPYS28lhrq3gk1eg5pQ1Ol1j\nnpwIXbvgpr83/rjNxP9VM9E=\n-----END PRIVATE KEY-----\n"",
//  ""client_email"": ""firebase-adminsdk-ggc6a@demomicroneme.iam.gserviceaccount.com"",
//  ""client_id"": ""112667768908373030776"",
//  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
//  ""token_uri"": ""https://oauth2.googleapis.com/token"",
//  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
//  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-ggc6a%40demomicroneme.iam.gserviceaccount.com""
//}"),
//            });

        }


        public static async void SendMessage(FlavourBusinessFacade.EndUsers.Message message, string deviceFirebaseToken)
        {
            var fireBaseMessage = new Message()
            {
                Data = new Dictionary<string, string>()
                {
                 
                    {"MessageID",message.MessageID }
                },
                Notification = new Notification() { Body = message.Notification.Body, Title = message.Notification.Title },
                Token = deviceFirebaseToken,
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
                //string response = await FirebaseMessaging.DefaultInstance.SendAsync(fireBaseMessage);
            }
            catch (Exception error)
            {

                
            }


        }


    }

    /// <MetaDataID>{63f01ed1-bf17-46d4-b609-77aa77b80063}</MetaDataID>
    public class Credential
    {
        /// <MetaDataID>{6eae306e-ef3f-4920-9653-712131468bdd}</MetaDataID>
        public string type { get; set; }
        /// <MetaDataID>{39929589-4f73-4159-81d1-bacadf769070}</MetaDataID>
        public string project_id { get; set; }
        /// <MetaDataID>{621c5962-f0bc-4e28-9d4f-96824848b3d3}</MetaDataID>
        public string private_key_id { get; set; }
        /// <MetaDataID>{35bfc236-dce6-487e-adf0-6989be80fae9}</MetaDataID>
        public string private_key { get; set; }
        /// <MetaDataID>{2925cc24-c3a3-416f-ae34-b6a20554df94}</MetaDataID>
        public string client_email { get; set; }
        /// <MetaDataID>{19de72f2-b0ec-439c-bbdd-8d47e3a71546}</MetaDataID>
        public string client_id { get; set; }
        /// <MetaDataID>{75c19603-6e9d-4fea-9abe-9b32924ac2a1}</MetaDataID>
        public string auth_uri { get; set; }
        /// <MetaDataID>{465c8d11-3873-4acc-b079-9646b6b8e5f7}</MetaDataID>
        public string token_uri { get; set; }
        /// <MetaDataID>{70f62c71-6995-40c9-8ce3-68087782742a}</MetaDataID>
        public string auth_provider_x509_cert_url { get; set; }
        /// <MetaDataID>{005a2bed-71ad-461f-b71f-ff837a230ae6}</MetaDataID>
        public string client_x509_cert_url { get; set; }
    }
}
