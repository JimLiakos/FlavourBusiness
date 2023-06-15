using System;
using System.Threading.Tasks;

namespace Firebase.Auth.Repository
{
    /// <inherit />
    /// <MetaDataID>{7570f604-569d-4b24-99b6-4e6f427eee55}</MetaDataID>
    public class InMemoryRepository : IUserRepository
    {
        private static InMemoryRepository instance;

        private User user;

        private InMemoryRepository()
        {
        }

        public static InMemoryRepository Instance => instance ?? (instance = new InMemoryRepository());

        public bool UserExists()
        {
            return this.user != null;
        }

        public UserData ReadUser()
        {
            return new UserData(this.user?.Info, this.user?.Credential);
        }

        public void SaveUser(User user)
        {
            this.user = user;
        }

        public void DeleteUser()
        {
            this.user = null;
        }
    }
}
