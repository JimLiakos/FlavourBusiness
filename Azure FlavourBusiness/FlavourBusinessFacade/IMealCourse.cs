using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{c8e552d4-3923-475d-9d77-71536de91ecf}</MetaDataID>
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IMealCourse
    {
        [Association("MealCourseSequence", Roles.RoleA, "3f8daafd-1296-452b-a9d6-1d9cd00f242e")]
        [RoleBMultiplicityRange(1, 1)]
        IMealCourse Previous { get;set; }

        /// <MetaDataID>{81761951-6699-42f8-aef3-19c407cf14e2}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        IMealCourse HeaderCourse { get; }

        [RoleAMultiplicityRange(0)]
        [Association("MealCourseServingBatches", Roles.RoleA, "be1d6d0b-5778-416c-b68f-18f019d34479")]
        List<IServingBatch> ServingBatches { get; }
        [Association("MealCourses", Roles.RoleB, "3c1213a5-f6e9-4d34-8802-72a4f051472b")]
        [RoleBMultiplicityRange(1, 1)]
        IMeal Meal { get; }

        /// <MetaDataID>{77edf270-755d-4621-9ebe-2bbeb6f1f80f}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Name { get; set; }

        /// <MetaDataID>{e6ec52a4-8462-46d7-adea-493cf75c3a4a}</MetaDataID>
        void RemoveItem(IItemPreparation itemPreparation);

        [RoleAMultiplicityRange(1)]
        [Association("ServiceSectionFoodItems", Roles.RoleA, true, "170a4e1d-1241-4efd-a037-01a2c2a3456b")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        System.Collections.Generic.IList<IItemPreparation> FoodItems { get; }

        /// <MetaDataID>{f8309b8a-8d4b-491a-9af9-bab1fc04fb67}</MetaDataID>
        void AddItem(IItemPreparation itemPreparation);

        /// <MetaDataID>{7999416a-5ea6-44d2-bb01-953986e1d9b9}</MetaDataID>
        ItemPreparationState PreparationState { get; set; }

        /// <MetaDataID>{3de93cf9-61ad-4dda-b02a-99d2e810f125}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        DateTime? StartsAt { get; set; }

        /// <MetaDataID>{ec3023f2-0cb5-4472-9a52-c171fdd905fe}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        DateTime? ServedAtForecast { get; set; }


        /// <summary>
        /// Defines the meal course items grouped by state of items
        /// </summary>
        /// <MetaDataID>{b5f5344d-c7f5-4d9e-81a7-ed3559031101}</MetaDataID>
        IList<ItemsPreparationContext> FoodItemsInProgress { get; }


        /// <MetaDataID>{314d0217-3182-440d-8da4-5405eb86c0bd}</MetaDataID>
        EndUsers.SessionData SessionData { get; }

        /// <MetaDataID>{5e6117fe-6aad-4b88-8aea-5ca123480287}</MetaDataID>
        void RaiseItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState);


        event ItemsStateChangedHandle ItemsStateChanged;

        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
    }
}