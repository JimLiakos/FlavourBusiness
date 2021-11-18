using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{1a7e7782-86f1-474f-9a85-d87a3067b01a}</MetaDataID>
    public interface IServingBatch
    {
        [Association("", Roles.RoleA, "48d7a02e-c7e1-4272-af2e-fb0ef5ee917b")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1, 1)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        ServicesContextResources.IServicePoint ServicePoint { get; }
        [Association("PreparedItemsToServe", Roles.RoleA, "2b36e0e0-e305-45c5-9b91-4f13b7048c84")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.List<IItemPreparation> PreparedItems { get; }

        /// <MetaDataID>{eb3120b1-beb4-4c33-bd96-5b861484b691}</MetaDataID>
        string Description { get; }

        /// <MetaDataID>{46f36229-fd4d-4d54-b0c2-751685130bdc}</MetaDataID>
        string ServicesPointIdentity { get; set; }
        /// <MetaDataID>{ffac02fc-d79b-4238-90aa-1a9cae4ca48b}</MetaDataID>
        ServicePointType ServicePointType { get; set; }
    }

    /// <MetaDataID>{bb7eff35-3ac4-4ac7-90a1-5fb3a4a8f122}</MetaDataID>
    public class ServingBatchUpdates
    {
        [OOAdvantech.Json.JsonConstructor]
        public ServingBatchUpdates(List<IServingBatch> servingBatches, List<ItemPreparationAbbreviation> removedServingItems)
        {
            RemovedServingItems = removedServingItems;
            ServingBatches = servingBatches;
        }
        public List<ItemPreparationAbbreviation> RemovedServingItems { get; }
        public List<IServingBatch> ServingBatches { get; }

    }
}