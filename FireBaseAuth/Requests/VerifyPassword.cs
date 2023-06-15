namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{6e8a3220-9d3f-4a16-b672-0bc0e1bfa51d}</MetaDataID>
    public class VerifyPasswordRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool ReturnSecureToken { get; set; }
    }

    /// <MetaDataID>{6a52fcaf-e568-40b1-a1bc-bc2b947216c7}</MetaDataID>
    public class VerifyPasswordResponse
    {
        public string LocalId { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        public string IdToken { get; set; }

        public bool Registered { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }

    /// <summary>
    /// Verifies specified password matches the user's actual password.
    /// </summary>
    /// <MetaDataID>{de540efe-5c1e-48de-b165-9087e8f12fca}</MetaDataID>
    public class VerifyPassword : FirebaseRequestBase<VerifyPasswordRequest, VerifyPasswordResponse>
    {
        public VerifyPassword(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GooglePasswordUrl;
    }
}
