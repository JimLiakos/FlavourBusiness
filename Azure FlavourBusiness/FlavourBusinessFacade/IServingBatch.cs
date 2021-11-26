using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{1a7e7782-86f1-474f-9a85-d87a3067b01a}</MetaDataID>
    [BackwardCompatibilityID("{1a7e7782-86f1-474f-9a85-d87a3067b01a}")]
    [GenerateFacadeProxy]
    public interface IServingBatch
    {
        /// <MetaDataID>{7a3cacaf-eaba-4fcd-a84b-e8bc312e54f5}</MetaDataID>
        ServicePointType ServicePointType { get; set; }

        [Association("MealCourseServingBatches", Roles.RoleB, "be1d6d0b-5778-416c-b68f-18f019d34479")]
        IMealCourse MealCourse { get; }


        event ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{81d18623-47c4-4758-8c09-ab1f11cf5e5d}</MetaDataID>
        bool IsAssigned { get; }
        /// <MetaDataID>{ddc19c8a-0a40-475a-82df-2e682a382e8e}</MetaDataID>
        int SortID { get; }

        /// <MetaDataID>{ba252d9e-edb5-4fc7-b860-894e9542b431}</MetaDataID>
        string MealCourseUri { get; }

        [Association("ServicePointServingBatch", Roles.RoleA, "48d7a02e-c7e1-4272-af2e-fb0ef5ee917b")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(0)]
        IServicePoint ServicePoint { get; }


        [Association("PreparedItemsToServe", Roles.RoleA, true, "2b36e0e0-e305-45c5-9b91-4f13b7048c84")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        List<IItemPreparation> PreparedItems { get; }


        /// <MetaDataID>{1c82234b-1265-4f06-9c61-7915cbae32c4}</MetaDataID>
        IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; }


        /// <MetaDataID>{04be4909-5947-4ed7-a47f-6b881f1cb9ee}</MetaDataID>
        IList<ItemsPreparationContext> ContextsOfPreparedItems { get; }


        /// <MetaDataID>{eb3120b1-beb4-4c33-bd96-5b861484b691}</MetaDataID>
        string Description { get; }

        /// <MetaDataID>{46f36229-fd4d-4d54-b0c2-751685130bdc}</MetaDataID>
        string ServicesPointIdentity { get; set; }

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