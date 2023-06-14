using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{7573a5ba-307d-4436-9962-cc0dc9267dd1}</MetaDataID>
    public interface INativeAuthUser
    {
        /// <MetaDataID>{6d5c8d2b-fd97-489e-9f3e-5f9754bb4282}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string UserFullName { get; set; }

        /// <MetaDataID>{7bfd6eb6-58ee-4d41-b34e-81389c578efa}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string UserName { get; }
    }
}