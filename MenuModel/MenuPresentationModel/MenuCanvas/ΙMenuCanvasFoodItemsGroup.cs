using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{987d01ea-d392-496d-b600-fa370ca7abfe}</MetaDataID>
    public interface IMenuCanvasFoodItemsGroup
    {
        /// <MetaDataID>{d4af39d3-9877-485f-b503-b3a854f326e4}</MetaDataID>
        double MaxHeight { get; set; }

        /// <MetaDataID>{1e36e0f0-ffbc-408f-a03d-dd7eb90cd106}</MetaDataID>
        double Width { get; set; }

        /// <MetaDataID>{f755fcfd-ce8a-4ecc-9c3f-4f7100d22cd8}</MetaDataID>
        double YPos { get; set; }

        /// <MetaDataID>{2ee82584-e2a7-410f-88ee-6f6671ef8796}</MetaDataID>
        double XPos { get; set; }


        /// <MetaDataID>{68339e8a-9bf5-4c88-ac36-4c86bfdfcaea}</MetaDataID>
        void AddGroupedItem(IGroupedItem Item);


        /// <MetaDataID>{569b3454-e983-4cae-b650-e52a44347c42}</MetaDataID>
        void RemoveGroupedItem(IGroupedItem Item);

        [Association("ColumnFoodItemsGroups", Roles.RoleB, "878bf0cb-63ec-4140-85aa-b31f2c51e8c7")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasColumn Column { get; }


        [Association("FoodItemsGroupFormatHeading", Roles.RoleA, "5e6c99e8-6025-483c-b87f-aa315ce10f98")]
        [RoleAMultiplicityRange(1, 1)]
        IMenuCanvasHeading ItemsGroupHeading { get; }


        [RoleAMultiplicityRange(0)]
        [Association("GroupedItem", Roles.RoleA, true, "320ef910-eea5-48db-9f18-73aa0f181e8f")]
        [RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.IList<IGroupedItem> GroupedItems { get; }



    }
}