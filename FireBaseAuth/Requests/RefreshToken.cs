using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{0e826b00-d87f-4bd1-8c3d-e6f19ef308ad}</MetaDataID>
    public class RefreshTokenRequest
    {
        public string GrantType { get; set; }

        public string RefreshToken { get; set; }
    }

    /// <MetaDataID>{a1f22ab8-8327-466e-a1c4-54741b6aaea7}</MetaDataID>
    public class RefreshTokenResponse
    {
        public int ExpiresIn { get; set; }

        public string RefreshToken { get; set; }

        public string IdToken { get; set; }

        public string UserId { get; set; }
    }

    /// <summary>
    /// Refreshes IdToken using a refresh token.
    /// </summary>
    /// <MetaDataID>{5777aab6-83c3-4fa2-b7e3-512f8e9df83a}</MetaDataID>
    public class RefreshToken : FirebaseRequestBase<RefreshTokenRequest, RefreshTokenResponse>
    {
        public RefreshToken(FirebaseAuthConfig config) : base(config)
        {
            this.JsonSettingsOverride = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
        }

        protected override JsonSerializerSettings JsonSettingsOverride { get; }

        protected override string UrlFormat => Endpoints.GoogleRefreshAuth;
    }
}
