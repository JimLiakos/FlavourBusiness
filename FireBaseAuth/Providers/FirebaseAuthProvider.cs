using Firebase.Auth.Requests;
using System.Threading.Tasks;

namespace Firebase.Auth.Providers
{
    /// <summary>
    /// Base class for Firebase auth providers.
    /// </summary>
    /// <MetaDataID>{7cedd338-3764-4b0f-bb9d-8a07f00e9aad}</MetaDataID>
    public abstract class FirebaseAuthProvider
    {
        protected FirebaseAuthConfig config;
        protected CreateAuthUri createAuthUri;

        public abstract FirebaseProviderType ProviderType { get; }

        internal virtual void Initialize(FirebaseAuthConfig config)
        {
            this.config = config;
            this.createAuthUri = new CreateAuthUri(config);
        }

        protected internal abstract Task<UserCredential> SignInWithCredentialAsync(AuthCredential credential);

        protected internal abstract Task<UserCredential> LinkWithCredentialAsync(string idToken, AuthCredential credential);
    }
}
