using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{1a7e7782-86f1-474f-9a85-d87a3067b01a}</MetaDataID>
    public interface IServingBatch
    {

        bool IsAssigned { get; }

        string MealCourseUri { get; }

        [Association("ServicePointServingBatch", Roles.RoleA, "48d7a02e-c7e1-4272-af2e-fb0ef5ee917b")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        IServicePoint ServicePoint { get; }


        [Association("PreparedItemsToServe", Roles.RoleA, true, "2b36e0e0-e305-45c5-9b91-4f13b7048c84")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        List<IItemPreparation> PreparedItems { get; }

        /// <MetaDataID>{1e78d099-87cc-4807-99a5-d27fead0c5bb}</MetaDataID>
        IMealCourse MealCourse { get; }
        /// <MetaDataID>{1c82234b-1265-4f06-9c61-7915cbae32c4}</MetaDataID>
        IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; }


        /// <MetaDataID>{04be4909-5947-4ed7-a47f-6b881f1cb9ee}</MetaDataID>
        IList<ItemsPreparationContext> ContextsOfPreparedItems { get; }


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
        /// <MetaDataID>{4f6671e5-41c4-4e8c-a1a2-fd47440009ac}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public ServingBatchUpdates(List<IServingBatch> servingBatches, List<ItemPreparationAbbreviation> removedServingItems)
        {
            RemovedServingItems = removedServingItems;
            ServingBatches = servingBatches;
        }
        /// <MetaDataID>{0fd1a264-342b-40d2-96d0-7f156710889a}</MetaDataID>
        public List<ItemPreparationAbbreviation> RemovedServingItems { get; }
        /// <MetaDataID>{4dd157ca-5593-404c-9318-f7164fd270c0}</MetaDataID>
        public List<IServingBatch> ServingBatches { get; }

    }
}