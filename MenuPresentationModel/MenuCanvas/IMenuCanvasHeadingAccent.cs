using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{6fccc84a-054d-4b3a-b310-e746fee8a003}</MetaDataID>
    public interface IMenuCanvasAccent : IMenuCanvasItem
    {
        [Association("MenuCanvasItemAccent", Roles.RoleB, "2a070e47-80ea-4cf6-8213-f19b69db3526")]
        [RoleBMultiplicityRange(1)]
        System.Collections.Generic.List<IHighlightedMenuCanvasItem> HighlightedItems { get; }



        [Association("CanvasHeadingAccent", Roles.RoleB, "177981cb-c031-4d9e-bdf3-240883497e35")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasHeading Heading
        {
            set;
            get;
        }
        /// <MetaDataID>{436b1d70-a564-4043-8d47-ea29b4aad6f6}</MetaDataID>
        string AccentColor { get; }

#if MenuPresentationModel

        event ObjectChangeStateHandle ObjectChangeState;
        
        /// <MetaDataID>{c9fc659c-5de8-478f-9eac-a02860af93d3}</MetaDataID>
        Rect GetAccentImageRect(int accentImageIndex);

#endif

        [Association("HeadingAccent", Roles.RoleA, "b7d1e6bb-87a0-4cb2-a7f7-f92088ae8a96")]
        MenuStyles.IAccent Accent { get; set; }

        /// <MetaDataID>{fc21bad4-f0da-4387-bce2-e97cd2a18a97}</MetaDataID>
        bool FullRowImage { get; set; }



    }
}