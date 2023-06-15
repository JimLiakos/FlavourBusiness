namespace Firebase.Auth.Providers
{
    /// <MetaDataID>{635d3373-3bab-4d11-8406-2d401d11cde4}</MetaDataID>
    public class FacebookProvider : OAuthProvider
    {
        public const string DefaultEmailScope = "email";

        public FacebookProvider()
        {
            this.AddScopes(DefaultEmailScope);
        }

        public static AuthCredential GetCredential(string accessToken) => GetCredential(FirebaseProviderType.Facebook, accessToken, OAuthCredentialTokenType.AccessToken);

        public override FirebaseProviderType ProviderType => FirebaseProviderType.Facebook;

        protected override string LocaleParameterName => "locale";
    }
}
