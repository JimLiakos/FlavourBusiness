using Newtonsoft.Json;

namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{0c79146b-1209-4ec1-b957-1ef8bf63e642}</MetaDataID>
    public class SetAccountUnlinkRequest : IdTokenRequest
    {
        [JsonProperty("deleteProvider")]
        public FirebaseProviderType[] DeleteProviders { get; set; }
    }

    /// <summary>
    /// Unlink accounts.
    /// </summary>
    /// <MetaDataID>{088c4c64-b407-415e-9107-7ff8179e2982}</MetaDataID>
    public class SetAccountUnlink : FirebaseRequestBase<SetAccountUnlinkRequest, SetAccountInfoResponse>
    {
        public SetAccountUnlink(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleSetAccountUrl;
    }
}
