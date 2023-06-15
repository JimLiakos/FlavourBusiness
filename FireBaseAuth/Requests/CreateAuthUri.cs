using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{7097d8e1-4197-46ab-ba86-7bfb3078374b}</MetaDataID>
    public class CreateAuthUriResponse
    {
        public string AuthUri { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public FirebaseProviderType ProviderId { get; set; }

        public string SessionId { get; set; }

        public bool Registered { get; set; }

        public List<FirebaseProviderType> SigninMethods { get; set; }

        public List<FirebaseProviderType> AllProviders { get; set; }
    }

    /// <MetaDataID>{aeff11e2-425f-45a0-9537-29212189da19}</MetaDataID>
    public class CreateAuthUriRequest
    {
        public FirebaseProviderType? ProviderId { get; set; }

        public string ContinueUri { get; set; }

        [JsonProperty("customParameter")]
        public Dictionary<string, string> CustomParameters { get; set; }

        public string OauthScope { get; set; }

        public string Identifier { get; set; }
    }

    /// <summary>
    /// Creates oauth authentication uri that user needs to navigate to in order to authenticate.
    /// </summary>
    /// <MetaDataID>{c4fad135-b0d3-49f8-bb82-d8ad067ce832}</MetaDataID>
    public class CreateAuthUri : FirebaseRequestBase<CreateAuthUriRequest, CreateAuthUriResponse>
    {
        public CreateAuthUri(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleCreateAuthUrl;
    }
}
