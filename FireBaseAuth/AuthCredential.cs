namespace Firebase.Auth
{
    /// <summary>
    /// Base class for provider-specific AuthCredentials.
    /// </summary>
    /// <MetaDataID>{61138d6f-4a22-4683-84bf-b6958375b3fa}</MetaDataID>
    public abstract class AuthCredential
    {
        public FirebaseProviderType ProviderType { get; set; }
    }
}
