using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{6fccc84a-054d-4b3a-b310-e746fee8a003}</MetaDataID>
    public interface IMenuCanvasHeadingAccent : IMenuCanvasItem
    {
        [Association("CanvasHeadingAccent", Roles.RoleB, "177981cb-c031-4d9e-bdf3-240883497e35")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasHeading Heading
        {
            set;
            get;
        }



        [Association("HeadingAccent", Roles.RoleA, "b7d1e6bb-87a0-4cb2-a7f7-f92088ae8a96")]
        MenuStyles.IHeadingAccent Accent { get; set; }

        /// <MetaDataID>{147e9533-cd17-46e3-aa67-deb8b9376b2c}</MetaDataID>
        double FullRowWidth { get; set; }
        /// <MetaDataID>{fc21bad4-f0da-4387-bce2-e97cd2a18a97}</MetaDataID>
        bool FullRowImage { get; set; }

        /// <MetaDataID>{c9fc659c-5de8-478f-9eac-a02860af93d3}</MetaDataID>
        Rect GetAccentImageRect(int accentImageIndex);
    }
}