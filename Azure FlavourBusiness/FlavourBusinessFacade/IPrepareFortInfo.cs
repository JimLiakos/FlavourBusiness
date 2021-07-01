using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{c288b0d9-f317-4c09-9b94-1b7de9a43353}</MetaDataID>
    public interface IPreparationForInfo
    {


        /// <MetaDataID>{711b696b-9832-4c87-824b-8e4ab45ccf4b}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }


        /// <MetaDataID>{3821d45d-204b-4279-b1ef-bedaa77a1105}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        PreparationForInfoType PreparationForInfoType { get; set; }

        /// <MetaDataID>{cce994cf-adec-4111-9eb8-574bedf97071}</MetaDataID>
        IServicePoint ServicePoint { get; set; }

        /// <MetaDataID>{932ddd16-664e-4493-85c6-5c658cf71011}</MetaDataID>
        IServiceArea ServiceArea { get; set; }

    }

    /// <MetaDataID>{2475ef23-7f27-403f-b42b-e05683421dd1}</MetaDataID>
    public enum PreparationForInfoType
    {
        Include = 1,
        Exclude = 2
    }
}

