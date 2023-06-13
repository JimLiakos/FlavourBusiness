using Firebase.Auth.Repository;
using System;
using System.Threading.Tasks;

namespace Firebase.Auth
{
    /// <MetaDataID>{7e1e0d21-6133-4bc9-892c-7f2ace52f0e4}</MetaDataID>
    public class UserManager
    {
        private readonly IUserRepository fs;

        private UserData? cache;

        public event EventHandler<UserEventArgs> UserChanged;

        /// <summary>
        /// Creates a new instance of <see cref="UserManager"/> with custom repository.
        /// </summary>
        /// <param name="fileSystem"> Proxy to the filesystem operations. </param>
        public UserManager(IUserRepository fileSystem)
        {
            this.fs = fileSystem;
        }

        public UserData GetUser()
        {
            if (!this.cache.HasValue)
            {
                if (!this.fs.UserExists())
                {
                    this.cache =new UserData (null, null);
                    return this.cache.Value;
                }

                this.cache = this.fs.ReadUser();
            }

            return this.cache.Value;
        }

        public void SaveNewUser(User user)
        {
            this.cache =new UserData(user.Info, user.Credential);
            this.fs.SaveUser(user);
            this.UserChanged?.Invoke(this, new UserEventArgs(user));
        }

        public void UpdateExistingUser(User user)
        {
            if (user.Uid != this.cache?.info.Uid)
            {
                // if updating a user, it needs to be the active one, otherwise don't do anything
                return;
            }

            // continue updating current user
            this.SaveNewUser(user);
        }

        public void DeleteExistingUser(string uid)
        {
            if (cache?.info.Uid != uid)
            {
                // deleting a user which is not an active user
                return;
            }

            this.cache =new UserData(default(UserInfo), default(FirebaseCredential));
            this.fs.DeleteUser();
            this.UserChanged?.Invoke(this, new UserEventArgs(null));
        }
      
    }
}
