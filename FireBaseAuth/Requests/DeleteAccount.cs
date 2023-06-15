namespace Firebase.Auth.Requests
{
    /// <summary>
    /// Deletes user's account.
    /// </summary>
    /// <MetaDataID>{1655e762-9ca6-4ae4-bbf4-197e33a9f236}</MetaDataID>
    public class DeleteAccount : FirebaseRequestBase<IdTokenRequest, object>
    {
        public DeleteAccount(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleDeleteUserUrl;
    }
}
