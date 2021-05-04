using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{e2cf1dbc-d96f-44a5-be60-3722f6b46a29}</MetaDataID>
    [BackwardCompatibilityID("{e2cf1dbc-d96f-44a5-be60-3722f6b46a29}")]
    public interface ICallerIDLine
    {
        /// <MetaDataID>{22c9ea09-7e51-4de2-8737-ae8b3a6d7601}</MetaDataID>
        [CachingDataOnClientSide]
        string LineDescription { get; set; }
    }
}