using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{036bb72e-744e-48a0-b74b-317b6cc2563d}</MetaDataID>
    /// <summary>gt430</summary>
    public interface IMenuCanvasPageColumn: IMenuCanvasColumn
    {
      

        [Association("CanvasColumns", Roles.RoleB, "5a14aabe-41b9-45a0-b47b-5454df7c7bf0")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuPageCanvas Page { get; set; }

        /// <MetaDataID>{949989c5-b23d-4f7a-9226-20542ce8acfb}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [Association("ColumnFoodItemsGroups", Roles.RoleA, "878bf0cb-63ec-4140-85aa-b31f2c51e8c7")]
        [RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.IList<IMenuCanvasFoodItemsGroup> FoodItemsGroup { get; }

        /// <MetaDataID>{2f36a7e4-8af5-4a1c-9be6-330b8c725d6c}</MetaDataID>
        void RenderMenuCanvasItems(System.Collections.Generic.IList<IMenuCanvasItem> menuCanvasItems, IMenuCanvasHeading lastFoodItemsHeading);


    }

}