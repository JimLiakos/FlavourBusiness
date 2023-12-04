using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{7ef4f082-bc81-4572-b19a-dbe28f462249}</MetaDataID>
    [BackwardCompatibilityID("{7ef4f082-bc81-4572-b19a-dbe28f462249}")]
    public interface IAuditWorkerEvents
    {
        [BackwardCompatibilityID("+1")]
        string Description { get; }
    }
}