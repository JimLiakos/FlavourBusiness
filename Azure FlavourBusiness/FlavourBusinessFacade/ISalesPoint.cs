using System.Collections.Generic;

namespace FlavourBusinessFacade.PriceList
{
    /// <MetaDataID>{2afdee4f-4844-4c43-afc4-824b023cda58}</MetaDataID>
    public interface ISalesPoint
    {
        /// <MetaDataID>{679b8a46-aafe-4b25-b526-bf756eb7220b}</MetaDataID>
        List<OrganizationStorageRef> PriceLists { get; }

        /// <MetaDataID>{7dbed63a-5aff-44aa-9a09-af5bfcbfcff8}</MetaDataID>
        void AssignPriceList(OrganizationStorageRef priceListStorageRef);

        /// <MetaDataID>{0525e226-59d1-4cfe-9bec-f95ce9fe40a5}</MetaDataID>
        void RemovePriceList(OrganizationStorageRef priceListStorageRef);
    }
}