namespace Firebase.Auth
{
    /// <summary>
    ///     /// Basic information about the signed in user.
    ///     /// </summary>
    /// <MetaDataID>{c800b98a-94c2-45b5-9197-f419f05914a5}</MetaDataID>
    public class UserInfo
    {
        public string Uid { get; set; }

        public string FederatedId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsAnonymous { get; set; }
    }
}
