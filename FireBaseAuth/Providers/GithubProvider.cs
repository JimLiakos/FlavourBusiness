namespace Firebase.Auth.Providers
{
    /// <MetaDataID>{7d21d03c-c4ec-4bbb-9750-4b4743256533}</MetaDataID>
    public class GithubProvider : OAuthProvider
    {
        public static AuthCredential GetCredential(string accessToken) => GetCredential(FirebaseProviderType.Github, accessToken, OAuthCredentialTokenType.AccessToken);

        public override FirebaseProviderType ProviderType => FirebaseProviderType.Github;
    }
}
