using System.Threading.Tasks;

namespace Firebase.Auth.Repository
{
    /// <summary>
    ///     /// Repository abstraction for <see cref="User" />.
    ///     /// </summary>
    /// <MetaDataID>{74d3f619-7360-483f-9b8a-5934a5a661a0}</MetaDataID>
    public interface IUserRepository
    {
        bool UserExists();

        UserData ReadUser();

        void SaveUser(User user);

        void DeleteUser();


    }

    /// <MetaDataID>{64e9d1cf-cfe8-403b-bc81-c4f33cbf0a07}</MetaDataID>
    public struct UserData
    {
        public UserInfo info;
        public FirebaseCredential credential;

        public UserData(UserInfo userInfo, FirebaseCredential credential)
        {
            this.info=userInfo;
            this.credential=credential;
        }
    }
}
