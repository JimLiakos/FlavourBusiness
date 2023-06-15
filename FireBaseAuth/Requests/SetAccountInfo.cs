namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{22742af8-faff-46de-89cf-3d2bd3ddffc3}</MetaDataID>
    public abstract class SetAccountInfoRequest : IdTokenRequest
    {
        public bool ReturnSecureToken { get; set; }
    }

    /// <MetaDataID>{bba95e0e-fd33-4af8-87da-77894aa4e37a}</MetaDataID>
    public class SetAccountDisplayName : SetAccountInfoRequest
    {
        public string DisplayName { get; set; }
    }

    /// <MetaDataID>{e3a355e0-8a17-49bc-a70c-e7d4afc28617}</MetaDataID>
    public class SetAccountInfoResponse
    {
        public string LocalId { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public ProviderUserInfo[] ProviderUserInfo { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailVerified { get; set; }
    }

    /// <summary>
    /// Updates specified fields for the user's account.
    /// </summary>
    /// <MetaDataID>{cff83015-9898-4ee4-87c9-7be2b34eb10a}</MetaDataID>
    public class SetAccountInfo : FirebaseRequestBase<SetAccountInfoRequest, SetAccountInfoResponse>
    {
        public SetAccountInfo(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleSetAccountUrl;
    }
}
