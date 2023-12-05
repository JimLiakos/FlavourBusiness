using OOAdvantech.MetaDataRepository;
using System;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{7ef4f082-bc81-4572-b19a-dbe28f462249}</MetaDataID>
    [BackwardCompatibilityID("{7ef4f082-bc81-4572-b19a-dbe28f462249}")]
    public interface IAuditWorkerEvents
    {
        /// <MetaDataID>{b513238b-6e6c-4043-abb5-36c7e691a724}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; }

        /// <MetaDataID>{a0fb8ea2-cb34-47e8-8c87-2ca43eae00f5}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        DateTime EventTimeStamp { get; }
    }
}