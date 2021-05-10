using OOAdvantech.MetaDataRepository;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{ae67aeb5-ca67-4e6d-bc81-22b47e7a2c41}</MetaDataID>
    public interface IHeadingStyle : IStyleRule
    {


        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [Association("HeadingStyleAccent", Roles.RoleA, "4dc8bf99-fff7-4ec8-8ac7-c0c96dfa97b5")]
        IAccent Accent { get; set; }

        /// <MetaDataID>{b786a5b5-5f3d-4fcb-93c6-dc2b9789f09f}</MetaDataID>
        bool OverlineImage { get; set; }

        /// <MetaDataID>{ad3b5318-7e20-4350-bca6-40eda6076f7f}</MetaDataID>
        bool UnderlineImage { get; set; }

        /// <MetaDataID>{7596fe4a-1ea9-48c3-ae5c-5c405c539223}</MetaDataID>
        FontData Font { get; set; }

        /// <MetaDataID>{a76f3b71-641f-4348-9f05-9fc6e49433f5}</MetaDataID>
        Alignment Alignment { get; set; }

        /// <MetaDataID>{425ee6de-4936-4e76-8b96-eede88806255}</MetaDataID>
        double AfterSpacing { get; set; }

        /// <MetaDataID>{cd9bda3c-798a-4a52-be52-d9752d1cb33d}</MetaDataID>
        double BeforeSpacing { get; set; }


        /// <MetaDataID>{3a0247b7-4504-42d3-992b-051e0eb0cf36}</MetaDataID>
        System.Collections.Generic.List<Resource> AccentImages { get; }

        /// <MetaDataID>{3a2aa5e0-1d93-4144-822d-44f427b22101}</MetaDataID>
        void AddAccentImage(Resource accentImage);

        /// <MetaDataID>{4dac00bf-ff43-4680-937e-be7d98874760}</MetaDataID>
        void DeleteAccentImage(Resource accentImage);
        /// <MetaDataID>{da7fa3e7-6c85-4390-b366-d06d368069c4}</MetaDataID>
        void RestFont();

      
    }
}