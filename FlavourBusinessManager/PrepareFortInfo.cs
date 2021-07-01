namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{5a7f0187-b1e8-4b95-9fe8-e79da0833aff}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{5a7f0187-b1e8-4b95-9fe8-e79da0833aff}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class PrepareFortInfo : FlavourBusinessFacade.ServicesContextResources.IPrepareFortInfo
    {
        /// <MetaDataID>{3e9af4db-dd7c-4c3f-8cae-30833a99aad4}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_PrepareFortInfoType))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
        public FlavourBusinessFacade.ServicesContextResources.PrepareFortInfoType PrepareFortInfoType
        {
            get => default;
            set
            {
            }
        }

        /// <MetaDataID>{a4a0166a-3480-4ea9-9713-f73f6abb1a18}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Description))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public string Description
        {
            get => default;
            set
            {
            }
        }

        /// <MetaDataID>{c33b309b-8ccf-4023-824d-d8c951ca7e2a}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_ServicePointsInfoObjectUri))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        public string ServicePointsInfoObjectUri
        {
            get => default;
            set
            {
            }
        }
    }
}