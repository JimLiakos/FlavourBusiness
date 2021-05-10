using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{5b3d373f-b5be-4c4c-b72d-22d0628f577b}</MetaDataID>
    public interface IMenuCanvasHeading : IMenuCanvasItem
    {
        [Association("FoodItemsGroupFormatHeading", Roles.RoleB, "5e6c99e8-6025-483c-b87f-aa315ce10f98")]
        IMenuCanvasFoodItemsGroup HostingArea { get; set; }

        /// <MetaDataID>{b710b0bc-e3a2-497f-8d6d-f9695b0de3c6}</MetaDataID>
        MenuStyles.IHeadingStyle Style { get; }

        [Association("CanvasHeadingAccent", Roles.RoleA, "177981cb-c031-4d9e-bdf3-240883497e35")]
        IMenuCanvasHeadingAccent Accent { get; set; }


        /// <MetaDataID>{e60d70dd-e95f-4895-ad65-76323a9afc83}</MetaDataID>
        HeadingType HeadingType
        {
            get; set;
        }

        /// <MetaDataID>{c53eb61c-d497-499b-b2ca-353b38555314}</MetaDataID>
        MenuStyles.FontData Font { get; set; }


        /// <MetaDataID>{e372b19d-b0a8-476f-b784-e233c4f89442}</MetaDataID>
        Rect CanvasFrameArea { get; }

    }
}