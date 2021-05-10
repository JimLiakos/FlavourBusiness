using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{ae67aeb5-ca67-4e6d-bc81-22b47e7a2c41}</MetaDataID>
    public interface IHeadingStyle : IStyleRule
    {
        [Association("StyleFont", Roles.RoleA, "1f1f59ae-e3a2-4e36-85af-0b3e2a597f0f")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1, 1)]
        IFontData Font { get; set; }

        /// <MetaDataID>{a76f3b71-641f-4348-9f05-9fc6e49433f5}</MetaDataID>
        Alignment Alignment { get; set; }

        /// <MetaDataID>{425ee6de-4936-4e76-8b96-eede88806255}</MetaDataID>
        double AfterSpacing { get; set; }

        /// <MetaDataID>{cd9bda3c-798a-4a52-be52-d9752d1cb33d}</MetaDataID>
        double BeforeSpacing { get; set; }
    }
}