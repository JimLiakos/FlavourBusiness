using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel
{
    /// <MetaDataID>{40411631-07a8-4734-a0f3-3c5cf9ad5530}</MetaDataID>
    public interface IStyle
    {
        [Association("StyleFont", Roles.RoleA, "1f1f59ae-e3a2-4e36-85af-0b3e2a597f0f")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1, 1)]
        IFontData Font { get; set; }

        /// <MetaDataID>{d060853b-b8ea-446a-ba78-3994b52f90b7}</MetaDataID>
        double BeforeSpacing { get; set; }

        /// <MetaDataID>{55e2fa01-0f4a-4174-8670-13feff6a1d4c}</MetaDataID>
        double AfterSpacing { get; set; }

        /// <MetaDataID>{9903486f-550a-4d4c-9fa7-23f114903a20}</MetaDataID>
        Alignment Alignment { get; set; }
    }
}