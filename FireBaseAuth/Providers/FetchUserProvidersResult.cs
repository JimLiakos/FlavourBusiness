using System;
using System.Collections.Generic;

namespace Firebase.Auth.Providers
{
    /// <MetaDataID>{3d63b365-f2cc-46bb-b4a1-0f6038a23e3c}</MetaDataID>
    public class FetchUserProvidersResult
    {
        public FetchUserProvidersResult(string email, bool exists, IReadOnlyCollection<FirebaseProviderType> signinProviders, IReadOnlyCollection<FirebaseProviderType> allProviders)
        {
            this.Email = email;
            this.UserExists = exists;
            this.SignInProviders = signinProviders ?? Array.Empty<FirebaseProviderType>();
            this.AllProviders = allProviders ?? Array.Empty<FirebaseProviderType>();
        }

        public string Email { get; }

        /// <summary>
        /// Specifies whether a user account exists for the given <see cref="Email"/>.
        /// </summary>
        public bool UserExists { get; }

        /// <summary>
        /// Collection of allowed sign-in providers. If empty, it means any provider is allowed.
        /// </summary>
        public IReadOnlyCollection<FirebaseProviderType> SignInProviders { get; }

        /// <summary>
        /// Collection of providers given <see cref="Email"/> has an account with.
        /// </summary>
        public IReadOnlyCollection<FirebaseProviderType> AllProviders { get; }
    }
}
