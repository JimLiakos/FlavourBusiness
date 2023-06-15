namespace Firebase.Auth.Providers
{
    /// <MetaDataID>{7df271fa-a567-49e1-9eb1-80f017966784}</MetaDataID>
    public class AppleProvider : OAuthProvider
    {
        public const string DefaultEmailScope = "email";

        public AppleProvider()
        {
            this.AddScopes(DefaultEmailScope);
        }

        public static AuthCredential GetCredential(string accessToken) => GetCredential(FirebaseProviderType.Apple, accessToken, OAuthCredentialTokenType.AccessToken);

        public override FirebaseProviderType ProviderType => FirebaseProviderType.Apple;

        protected override string LocaleParameterName => "locale";
    }
}
