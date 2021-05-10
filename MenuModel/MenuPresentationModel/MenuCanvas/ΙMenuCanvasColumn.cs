using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{036bb72e-744e-48a0-b74b-317b6cc2563d}</MetaDataID>
    /// <summary>gt430</summary>
    public interface IMenuCanvasColumn
    {
        /// <MetaDataID>{ff9191fd-85b0-4bc0-b91a-de6ddf8a01ca}</MetaDataID>
        double XPos { get; set; }

        /// <MetaDataID>{0a645b31-b2da-46a5-ae19-d4974f186632}</MetaDataID>
        double YPos { get; set; }

        /// <MetaDataID>{79bfb0c5-b663-4a3b-87b1-fa760cfe82d4}</MetaDataID>
        double Width { get; set; }

        /// <MetaDataID>{bf37e26c-fc4a-462e-81c8-764ce590df57}</MetaDataID>
        double MaxHeight { get; set; }
        [Association("CanvasColumns", Roles.RoleB, "5a14aabe-41b9-45a0-b47b-5454df7c7bf0")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuPageCanvas Page { get; set; }

        /// <MetaDataID>{949989c5-b23d-4f7a-9226-20542ce8acfb}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [Association("ColumnFoodItemsGroups", Roles.RoleA, "878bf0cb-63ec-4140-85aa-b31f2c51e8c7")]
        [RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.IList<IMenuCanvasFoodItemsGroup> FoodItemsGroup { get; }

        /// <MetaDataID>{2f36a7e4-8af5-4a1c-9be6-330b8c725d6c}</MetaDataID>
        void RenderMenuCanvasItems(System.Collections.Generic.IList<IMenuCanvasItem> menuCanvasItems, MenuCanvas.IMenuCanvasItem previousMenuCanvasItem);
    }
}