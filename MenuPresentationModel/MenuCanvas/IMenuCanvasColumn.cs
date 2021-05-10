using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{f87279f5-4d26-4ea5-ac11-ec9da7217706}</MetaDataID>
    public interface IMenuCanvasColumn
    {

        [Association("ColumnItems", Roles.RoleA, "2ae6275c-9dfb-4623-b27b-220e657aae4a")]
        [RoleBMultiplicityRange(0, 1)]
        System.Collections.Generic.IList<IMenuCanvasItem> MenuCanvasItems { get; }

        /// <MetaDataID>{6186cc7d-a138-4edc-9db4-b4f7b078adcd}</MetaDataID>
        System.Collections.Generic.IList<IMenuCanvasItem> GetDeepMenuCanvasItems();


        /// <MetaDataID>{42fd0d28-20bd-4489-9fbf-937aec6c4df8}</MetaDataID>
        double Height { get; }

        /// <MetaDataID>{ff9191fd-85b0-4bc0-b91a-de6ddf8a01ca}</MetaDataID>
        double XPos { get; set; }

        /// <MetaDataID>{0a645b31-b2da-46a5-ae19-d4974f186632}</MetaDataID>
        double YPos { get; set; }

        /// <MetaDataID>{79bfb0c5-b663-4a3b-87b1-fa760cfe82d4}</MetaDataID>
        double Width { get; set; }

        /// <MetaDataID>{bf37e26c-fc4a-462e-81c8-764ce590df57}</MetaDataID>
        double MaxHeight { get; set; }

        /// <MetaDataID>{ed40bb73-1db2-4e2e-befc-31843b2ecd7e}</MetaDataID>
        void RemoveMenuCanvasItem(IMenuCanvasItem menuCanvasItem);
        /// <MetaDataID>{d012d956-6257-48b2-a074-c8600f6fe086}</MetaDataID>
        void AddMenuCanvasItem(IMenuCanvasItem menuCanvasItem);
    }
}