using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{01b817a1-b353-43a7-b27f-85c3a0d43008}</MetaDataID>
    public interface IMenuCanvasLine
    {
        /// <MetaDataID>{6a95403e-dc00-4538-a6a3-e84823929025}</MetaDataID>
        MenuStyles.LineType LineType { get; set; }

        /// <MetaDataID>{1684f6ea-25b8-4fd9-87f6-789a6a9d9de8}</MetaDataID>
        [Association("PageSeparationLine", Roles.RoleB, "813a2e85-5c02-437e-9c15-0704aa476ac8")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        IMenuPageCanvas Page { get; }

        /// <MetaDataID>{4fa0cc40-2e32-432d-85e0-1ccfbeff6823}</MetaDataID>
        double StrokeThickness
        {
            set;
            get;
        }

        /// <MetaDataID>{771ffd47-9f78-4b69-b3b6-93f3d9d89dfc}</MetaDataID>
        string Stroke
        {
            set;
            get;
        }

        /// <MetaDataID>{5826dcc0-da0b-4a18-b76d-00446ebbf16d}</MetaDataID>
        double Y2 { get; set; }

        /// <MetaDataID>{03190c7d-081e-4fe6-95a0-414c1a23e5ba}</MetaDataID>
        double X2 { get; set; }

        /// <MetaDataID>{fe2ffdf3-eab8-464c-9224-b51f99a2eadf}</MetaDataID>
        double Y1 { get; set; }

        /// <MetaDataID>{615d2a5d-8396-4b09-9ac5-59c54b73b02a}</MetaDataID>
        double X1 { get; set; }
    }
}