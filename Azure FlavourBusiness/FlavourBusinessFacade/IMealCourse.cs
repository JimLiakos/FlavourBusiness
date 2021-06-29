using OOAdvantech.MetaDataRepository;
using System;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{c8e552d4-3923-475d-9d77-71536de91ecf}</MetaDataID>
    public interface IMealCourse
    {
        /// <MetaDataID>{77edf270-755d-4621-9ebe-2bbeb6f1f80f}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Name { get; set; }

        /// <MetaDataID>{e6ec52a4-8462-46d7-adea-493cf75c3a4a}</MetaDataID>
        void RemoveItem(IItemPreparation itemPreparation);

        [RoleAMultiplicityRange(1)]
        [Association("ServiceSectionFoodItems", Roles.RoleA, true, "170a4e1d-1241-4efd-a037-01a2c2a3456b")]
        System.Collections.Generic.IList<IItemPreparation> FoodItems { get; }

        /// <MetaDataID>{f8309b8a-8d4b-491a-9af9-bab1fc04fb67}</MetaDataID>
        void AddItem(IItemPreparation itemPreparation);

        /// <MetaDataID>{7999416a-5ea6-44d2-bb01-953986e1d9b9}</MetaDataID>
        ItemPreparationState PreparationState { get; set; }

        /// <MetaDataID>{3de93cf9-61ad-4dda-b02a-99d2e810f125}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        DateTime StartsAt { get; set; }

        /// <MetaDataID>{ec3023f2-0cb5-4472-9a52-c171fdd905fe}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        DateTime ServedAt{ get; set; }


    }
}