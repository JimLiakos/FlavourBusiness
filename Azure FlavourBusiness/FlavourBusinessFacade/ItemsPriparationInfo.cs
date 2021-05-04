using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{ccf5778b-f8d7-4a74-9a29-6e633b64f90b}</MetaDataID>
    [BackwardCompatibilityID("{ccf5778b-f8d7-4a74-9a29-6e633b64f90b}")]
    public interface IItemsPreparationInfo
    {
        /// <MetaDataID>{af070276-0111-4c31-a073-48493d0a2ea8}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string ItemsInfoObjectUri { get; set; }

        /// <MetaDataID>{976b3981-7755-487f-bcbc-90752e890884}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }


        ///// <MetaDataID>{edf34b1e-b6bb-45f1-a720-9d3242bf857c}</MetaDataID>
        //[BackwardCompatibilityID("+3")]
        //bool Exclude { get; }


        /// <MetaDataID>{9296deb8-b788-4321-bda7-d76eb74d0bfc}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        double PreparationTimeSpanInMin { get; set; }

        /// <MetaDataID>{1b78b048-2f35-4fcf-a227-1794e032c31d}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        int LearningCurveCount { get; set; }
        [BackwardCompatibilityID("+6")]
        ItemsPreparationInfoType ItemsPreparationInfoType { get; set; }

    }

    /// <MetaDataID>{62f6ca31-ec5e-4a3a-a1c8-c124b7adc8eb}</MetaDataID>
    public enum ItemsPreparationInfoType
    {
        Include = 1,
        Exclude = 2,
        PreparationTime = 4

    }
}