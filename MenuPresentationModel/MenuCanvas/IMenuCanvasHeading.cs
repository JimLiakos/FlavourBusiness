using OOAdvantech.MetaDataRepository;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
  

    /// <MetaDataID>{5b3d373f-b5be-4c4c-b72d-22d0628f577b}</MetaDataID>
    public interface IMenuCanvasHeading : IMenuCanvasItem, IHighlightedMenuCanvasItem
    {
        /// <MetaDataID>{828dd11a-1421-4437-b5a6-a9f6dbe3aa2e}</MetaDataID>
        [BackwardCompatibilityID("+11")]
        bool CustomSpacing { get; set; }
        /// <MetaDataID>{a3bea03f-1460-43e1-918c-a00503eef77a}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        double AfterSpacing { get; set; }
        /// <MetaDataID>{57b6eb79-bace-4c7a-97b4-3941112fff56}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        double BeforeSpacing { get; set; }

        /// <MetaDataID>{a2aab8f5-0f5f-42e1-82d1-4a52b7632ecc}</MetaDataID>
        bool IsStyleAlignmentOverridden { get; }

        /// <MetaDataID>{e73333b4-7f11-4308-8af5-be4d1dbdcc79}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        MenuStyles.Alignment Alignment { get; set; }


        [Association("FoodItemsGroupFormatHeading", Roles.RoleB, true, "5e6c99e8-6025-483c-b87f-aa315ce10f98")]
        System.Collections.Generic.IList<IMenuCanvasFoodItemsGroup> HostingAreas { get; }




        /// <MetaDataID>{964da3ea-e041-43eb-9c52-0703526b2910}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        int NumberOfFoodColumns { get; set; }

        /// <MetaDataID>{ae1d73ae-9c94-412e-91d2-3b9edbcea1d5}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        bool NextColumnOrPage { get; set; }




#if MenuPresentationModel
        /// <MetaDataID>{b4734eed-8ef5-4d85-9dbb-ed502c0b45b7}</MetaDataID>
        void AddHostingArea(IMenuCanvasFoodItemsGroup hostingArea);

        /// <MetaDataID>{204677ac-74e9-4444-8acc-249b4c013a60}</MetaDataID>
        void RemoveHostingArea(IMenuCanvasFoodItemsGroup hostingArea);

        /// <MetaDataID>{f9139102-1204-4634-acac-d932b71920e7}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        MenuPresentationModel.MenuStyles.IHeadingStyle Style { get; }

        /// <MetaDataID>{b0aa9965-84af-4136-b766-bd9764c7c5dc}</MetaDataID>
        void RemoveAllHostingAreas();

#endif

        [Association("CanvasHeadingAccent", Roles.RoleA, "177981cb-c031-4d9e-bdf3-240883497e35")]
        IMenuCanvasAccent Accent { get; }
        /// <MetaDataID>{3e1378ef-d944-427d-939b-74c2362780ab}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        HeadingType HeadingType
        {
            get; set;
        }
        /// <MetaDataID>{1799920d-30ca-44c9-b7d8-3a25bb5b4f5b}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        FontData Font { get; set; }


        /// <MetaDataID>{e372b19d-b0a8-476f-b784-e233c4f89442}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        Rect CanvasFrameArea { get; }

        /// <MetaDataID>{147e9533-cd17-46e3-aa67-deb8b9376b2c}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        double FullRowWidth { get; set; }

        /// <MetaDataID>{cc470bde-b693-49d0-8aba-7b3e00840288}</MetaDataID>
        void ClearAlignment();

        


        //string Color { get; set; }


    }
}