namespace Firebase.Auth.Providers
{
    /// <MetaDataID>{5ff9c429-e7ae-4209-a389-895d90fb1e39}</MetaDataID>
    public class MicrosoftProvider : OAuthProvider
    {
        public static string[] DefaultScopes = new[]
        {
            "profile",
            "email",
            "openid",
            "User.Read",
        };

        public MicrosoftProvider()
        {
            this.AddScopes(DefaultScopes);
        }

        public static AuthCredential GetCredential(string accessToken) => GetCredential(FirebaseProviderType.Microsoft, accessToken, OAuthCredentialTokenType.AccessToken);

        public override FirebaseProviderType ProviderType => FirebaseProviderType.Microsoft;
    }
}
