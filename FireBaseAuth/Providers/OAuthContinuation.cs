using Firebase.Auth.Requests;
using System;
using System.Threading.Tasks;

namespace Firebase.Auth.Providers
{
    /// <summary>
    /// Continuation of OAuth sign in. This class processes the redirect uri user is navigated to and signs the user in.
    /// </summary>
    /// <MetaDataID>{f31d3914-ba75-4dbd-9a0c-858cda26000b}</MetaDataID>
    public class OAuthContinuation
    {
        private readonly VerifyAssertion verifyAssertion;
        private readonly FirebaseAuthConfig config;
        private readonly string sessionId;
        private readonly FirebaseProviderType providerType;

        internal OAuthContinuation(FirebaseAuthConfig config, string uri, string sessionId, FirebaseProviderType providerType)
        {
            this.verifyAssertion = new VerifyAssertion(config);
            this.config = config;
            this.Uri = uri;
            this.sessionId = sessionId;
            this.providerType = providerType;
        }

        /// <summary>
        /// The uri user should be initially navigated to in browser.
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Finishes OAuth sign in after user signs in in browser.
        /// </summary>
        /// <param name="redirectUri"> Final uri that user lands on after completing sign in in browser. </param>
        /// <param name="idToken"> Optional id token  of an existing Firebase user. If set, it will effectivelly perform account linking. </param>
        /// <returns></returns>
        public async Task<UserCredential> ContinueSignInAsync(string redirectUri, string idToken = null)
        {
            var userData = await this.verifyAssertion.ExecuteAndParseAsync(
                this.providerType,
                new VerifyAssertionRequest
                {
                    IdToken = idToken,
                    RequestUri = redirectUri,
                    SessionId = this.sessionId,
                    ReturnIdpCredential = true,
                    ReturnSecureToken = true
                }).ConfigureAwait(false);

            var provider = this.config.GetAuthProvider(this.providerType) as OAuthProvider ?? throw new InvalidOperationException($"{this.providerType} is not a OAuthProvider");
            var credential = provider.GetCredential(userData.response);

            userData.response.Validate(credential);

            return new UserCredential(userData.user, credential, OperationType.SignIn);
        }
    }
}
