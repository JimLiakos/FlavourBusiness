using System;

namespace Firebase.Auth
{
    /// <MetaDataID>{66c2090c-5d6b-408a-94b2-e9c830a572d8}</MetaDataID>
    public class UserEventArgs : EventArgs
    {
        public UserEventArgs(User user)
        {
            this.User = user;
        }

        /// <summary>
        /// Currently signed in user. Null if no user is signed in.
        /// </summary>
        public User User { get; }
    }
}
