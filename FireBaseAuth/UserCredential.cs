namespace Firebase.Auth
{
    /// <MetaDataID>{29ed5490-428f-4c19-9b37-5c8b91089e2a}</MetaDataID>
    public enum OperationType
    {
        SignIn,
        Link,
        Reauthenticate
    }

    /// <summary>
    /// A structure containing a <see cref="User" />, an <see cref="AuthCredential" /> and <see cref="OperationType" />.
    /// </summary>
    /// <MetaDataID>{41d3ac01-9c6b-43b9-9e96-243986f8f52c}</MetaDataID>
    public class UserCredential
    {
        public UserCredential(User user, AuthCredential authCredential, OperationType op)
        {
            this.User = user;
            this.AuthCredential = authCredential;
            this.OperationType = op;
        }

        public User User { get; set; }

        public AuthCredential AuthCredential { get; set; }

        public OperationType OperationType { get; set; }
    }
}
