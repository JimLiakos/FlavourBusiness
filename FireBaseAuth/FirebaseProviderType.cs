using System.Runtime.Serialization;

namespace Firebase.Auth
{
    /// <summary>
    /// Type of authentication provider.
    /// </summary>
    /// <MetaDataID>{be00f5b8-8a71-48e8-99e4-4661f3de2fc4}</MetaDataID>
    public enum FirebaseProviderType
    {
        Unknown,

        [EnumMember(Value = "facebook.com")]
        Facebook,

        [EnumMember(Value = "google.com")]
        Google,

        [EnumMember(Value = "github.com")]
        Github,

        [EnumMember(Value = "twitter.com")]
        Twitter,

        [EnumMember(Value = "microsoft.com")]
        Microsoft,

        [EnumMember(Value = "apple.com")]
        Apple,

        [EnumMember(Value = "password")]
        EmailAndPassword,

        [EnumMember(Value = "phone")]
        Phone,

        Anonymous
    }
}
