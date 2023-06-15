namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{9ac93a43-5188-4774-a6ad-3d717d7b2b2e}</MetaDataID>
    public class SignupNewUserRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool ReturnSecureToken { get; set; }
    }

    /// <MetaDataID>{11908372-1a43-4ff2-9258-42071d513d7c}</MetaDataID>
    public class SignupNewUserResponse
    {
        public string IdToken { get; set; }

        public string Email { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }

        public string LocalId { get; set; }
    }

    /// <summary>
    /// Creates a new user account.
    /// </summary>
    /// <MetaDataID>{802bb5b9-d020-4b56-9901-51743773dc50}</MetaDataID>
    public class SignupNewUser : FirebaseRequestBase<SignupNewUserRequest, SignupNewUserResponse>
    {
        public SignupNewUser(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleSignUpUrl;
    }
}
