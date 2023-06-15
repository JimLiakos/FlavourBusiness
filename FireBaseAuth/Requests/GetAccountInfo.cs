using Firebase.Auth.Requests.Converters;
using Newtonsoft.Json;
using System;

namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{f8b2dca5-c33d-43be-b6b0-ffe567284d0f}</MetaDataID>
    public class GetAccountInfoResponse
    {
        public GetAccountInfoResponseUserInfo[] Users { get; set; }
    }

    /// <MetaDataID>{2d6a1516-a2ac-4750-ba0c-ac9b4ae158f7}</MetaDataID>
    public class GetAccountInfoResponseUserInfo
    {
        public string LocalId { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string PhotoUrl { get; set; }

        public bool EmailVerified { get; set; }

        public ProviderUserInfo[] ProviderUserInfo { get; set; }

        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
        public DateTime ValidSince { get; set; }

        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
        public DateTime LastLoginAt { get; set; }

        [JsonConverter(typeof(JavaScriptDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        public DateTime LastRefreshAt { get; set; }
    }

    /// <MetaDataID>{ce52ec32-3502-4b17-b662-1dbca9c7b092}</MetaDataID>
    public class ProviderUserInfo
    {
        [JsonConverter(typeof(DefaultEnumConverter))]
        public FirebaseProviderType ProviderId { get; set; }

        public string DisplayName { get; set; }

        public string PhotoUrl { get; set; }

        public string FederatedId { get; set; }

        public string Email { get; set; }

        public string RawId { get; set; }
    }

    /// <summary>
    /// Gets basic info about a user and his/her account.
    /// </summary>
    /// <MetaDataID>{efcaabae-c96a-49e2-beac-981b38d504ac}</MetaDataID>
    public class GetAccountInfo : FirebaseRequestBase<IdTokenRequest, GetAccountInfoResponse>
    {
        public GetAccountInfo(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleGetUser;
    }

}
