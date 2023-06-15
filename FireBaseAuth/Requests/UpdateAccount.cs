namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{fac13266-4718-4e79-b26b-6a9f3fcba20b}</MetaDataID>
    public class UpdateAccountRequest : IdTokenRequest
    {
        public string Password { get; set; }

        public bool ReturnSecureToken { get; set; }
    }

    /// <MetaDataID>{169ff987-9b42-412c-a8a5-37b88ffa5a2b}</MetaDataID>
    public class UpdateAccountResponse
    {
        public string LocalId { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string IdToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }

    /// <MetaDataID>{bc7e012d-ee77-4e6e-984a-8f9d655725ad}</MetaDataID>
    public class UpdateAccount : FirebaseRequestBase<UpdateAccountRequest, UpdateAccountResponse>
    {
        public UpdateAccount(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleUpdateUserPassword;
    }
}
