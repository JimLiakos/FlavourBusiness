namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{41de6e27-364d-4e1c-8ef3-b961e7e7b34c}</MetaDataID>
    public class ResetPasswordRequest
    {
        public const string PasswordResetType = "PASSWORD_RESET";

        public ResetPasswordRequest()
        {
            this.RequestType = PasswordResetType;
        }

        public string Email { get; set; }

        public string RequestType { get; set; }
    }

    /// <MetaDataID>{8c73ecd1-53fa-4104-9068-bb0d1083487e}</MetaDataID>
    public class ResetPasswordResponse
    {
        public string Email { get; set; }
    }

    /// <summary>
    /// Resets user's password for given email by sending a reset email.
    /// </summary>
    /// <MetaDataID>{979ea072-6355-4fdc-bb84-e919dcd9d34d}</MetaDataID>
    public class ResetPassword : FirebaseRequestBase<ResetPasswordRequest, ResetPasswordResponse>
    {
        public ResetPassword(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleGetConfirmationCodeUrl;
    }
}
