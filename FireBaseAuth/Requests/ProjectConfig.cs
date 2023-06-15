using System.Net.Http;

namespace Firebase.Auth.Requests
{
    /// <MetaDataID>{af634538-b456-43ca-b053-b1f8267e9f44}</MetaDataID>
    public class ProjectConfigResponse
    {
        public string ProjectId { get; set; }

        public string[] AuthorizedDomains { get; set; }
    }

    /// <summary>
    /// Get basic config info about the firebase project.
    /// </summary>
    /// <MetaDataID>{e2866bea-98cb-4736-a542-04d139a92645}</MetaDataID>
    public class ProjectConfig : FirebaseRequestBase<object, ProjectConfigResponse>
    {
        public ProjectConfig(FirebaseAuthConfig config) : base(config)
        {
        }

        protected override string UrlFormat => Endpoints.GoogleProjectConfighUrl;

        protected override HttpMethod Method => HttpMethod.Get;
    }
}
