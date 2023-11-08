using FlavourBusinessManager;
using OOAdvantech.Remoting.RestApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessApps
{
    public class FirebaseAuth : IOAuth
    {
        public async Task<AuthUser> VerifyIdToken(string authToken)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            OOAdvantech.Remoting.RestApi.AuthUser authUser = new OOAdvantech.Remoting.RestApi.AuthUser();

            await FireBase.Init();//Force FirbaseI nitialization 

            var decoded = await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(authToken);
            var uid = decoded.Uid;

            //System.Diagnostics.Debug.WriteLine(OOAdvantech.Json.JsonConvert.SerializeObject(decoded));

            authUser.IssuedAt = FromUnixTime(decoded.IssuedAtTimeSeconds).ToLocalTime();


            authUser.ExpirationTime = FromUnixTime(decoded.ExpirationTimeSeconds).ToLocalTime();


            authUser.Auth_Time = FromUnixTime((long)decoded.Claims["auth_time"]).ToLocalTime();

            authUser.Audience =decoded.Audience;
            authUser.Email = (from claim in decoded.Claims where claim.Key == "email" select claim.Value as string).FirstOrDefault();

            if ((from claim in decoded.Claims where claim.Key == "email_verified" select claim.Value).FirstOrDefault() != null)
                authUser.Email_Verified = bool.Parse((from claim in decoded.Claims where claim.Key == "email_verified" select claim.Value?.ToString()).FirstOrDefault());
            authUser.Iss = decoded.Issuer;
            authUser.Name = (from claim in decoded.Claims where claim.Key == "name" select claim.Value?.ToString()).FirstOrDefault();
            authUser.Picture = (from claim in decoded.Claims where claim.Key == "picture" select claim.Value?.ToString()).FirstOrDefault();
            authUser.Subject = decoded.Subject;// (from claim in tokenS.Claims where claim.Type == "sub" select claim.Value).FirstOrDefault();
            authUser.User_ID = decoded.Uid;// (from claim in tokenS.Claims where claim.Type == "user_id" select claim.Value).FirstOrDefault();

            var firebaseAttributes = (from claim in decoded.Claims where claim.Key == "firebase" select Newtonsoft.Json.Linq.JObject.Parse(claim.Value?.ToString())).FirstOrDefault();

            authUser.Firebase_Sign_in_Provider = (from fireBaseProperty in firebaseAttributes.Properties()
                                                  where fireBaseProperty.Name == "sign_in_provider"
                                                  select (fireBaseProperty.Value as Newtonsoft.Json.Linq.JValue).Value).FirstOrDefault() as string;
            timer.Stop();
            authUser.AuthToken = authToken;

            var elapsed = timer.ElapsedMilliseconds;
            return authUser;

        }

        /// Is the number of seconds that have elapsed since January 1, 1970 (midnight UTC/GMT)
        /// </summary>
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
    }
}
