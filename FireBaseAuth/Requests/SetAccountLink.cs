namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{3d239174-478a-4320-901e-79a32c629fd0}</MetaDataID>
    public class SetAccountLinkRequest : SetAccountInfoRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    /// <MetaDataID>{714459f8-9702-4559-8a85-7ba055814f3f}</MetaDataID>
    public class SetAccountLinkResponse : SetAccountInfoResponse
    {
        public string IdToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }

    /// <summary>
    /// Link two accounts.
    /// </summary>
    /// <MetaDataID>{7ec99357-4006-4d29-8b7e-46bf468bceac}</MetaDataID>
    public class SetAccountLink : FirebaseRequestBase<SetAccountLinkRequest, SetAccountLinkResponse>
    {
        public SetAccountLink(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleSetAccountUrl;
    }
}
