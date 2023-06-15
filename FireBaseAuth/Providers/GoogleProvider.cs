namespace Firebase.Auth.Providers
{
    /// <MetaDataID>{7a7c9d07-7409-4b93-a3e9-3e2faf8487f9}</MetaDataID>
    public class GoogleProvider : OAuthProvider
    {
        public const string DefaultProfileScope = "profile";
        public const string DefaultEmailScope = "email";

        public GoogleProvider()
        {
            this.AddScopes(DefaultProfileScope, DefaultEmailScope);
        }

        public static AuthCredential GetCredential(string token, OAuthCredentialTokenType tokenType = OAuthCredentialTokenType.AccessToken) => GetCredential(FirebaseProviderType.Google, token, tokenType);

        public override FirebaseProviderType ProviderType => FirebaseProviderType.Google;

        protected override string LocaleParameterName => "hl";
    }
}
