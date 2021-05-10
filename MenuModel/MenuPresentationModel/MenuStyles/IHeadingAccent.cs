using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{14ed9fe7-f4ae-4ec2-a80d-fdce5b7dc714}</MetaDataID>
    public interface IHeadingAccent
    {
        /// <MetaDataID>{1d7c1fc2-791c-4d08-b775-e8016c7af650}</MetaDataID>
        string SelectionAccentImageUri { get; set; }

        /// <MetaDataID>{9140a1b6-e9b3-4ad0-b376-378b134d93d3}</MetaDataID>
        [Association("HeadingAccentImages", Roles.RoleA, "3a455243-9391-453c-92ce-9e9a8f16387a")]
        System.Collections.Generic.List<IAccentImage> AccentImages { get; }

        /// <MetaDataID>{ff75092e-bdfb-41fa-a340-362d09bcb325}</MetaDataID>
        string Name { get; set; }


        /// <MetaDataID>{f079f7f9-9acf-4bbf-9977-875cf70fb39b}</MetaDataID>
        void AddAccentImage(MenuPresentationModel.MenuStyles.IAccentImage accentImage);
        /// <MetaDataID>{39e00055-1664-4e71-8d40-58afba8ff961}</MetaDataID>
        void DeleteAccentImage(MenuPresentationModel.MenuStyles.IAccentImage accentImage);

        /// <MetaDataID>{c4be45aa-c69c-4960-9579-a5e73ee64449}</MetaDataID>
        bool UnderlineImage { get; set; }
        /// <MetaDataID>{ea07a159-6007-42d2-b4ac-e91af4a22220}</MetaDataID>
        bool OverlineImage { get; set; }

        /// <MetaDataID>{40cc9a00-5829-4ccb-b9fc-6f034cd27444}</MetaDataID>
        bool TextBackgroundImage { get; set; }

        /// <MetaDataID>{e4bb1a19-0fda-477c-90ab-36312bbd6fd6}</MetaDataID>
        bool FullRowImage { get; set; }

        /// <MetaDataID>{874d3d0b-3e85-4c5c-8488-0ae4d9f6cc83}</MetaDataID>
        bool DoubleImage { get; set; }

        /// <MetaDataID>{d060d3aa-ad50-4736-a8f1-fcdd69414e57}</MetaDataID>
        double MarginLeft { get; set; }

        /// <MetaDataID>{3a0dfe06-f16d-4c5f-9c0e-b9ba729eb843}</MetaDataID>
        double MarginTop { get; set; }

        /// <MetaDataID>{e3cd2075-036c-49c7-8811-bad76dfad63b}</MetaDataID>
        double MarginRight { get; set; }
        /// <MetaDataID>{47e0658d-b7da-43b2-81c8-863c944281c1}</MetaDataID>
        double MarginBottom { get; set; }
        /// <MetaDataID>{1290177d-9965-46f1-bb21-495307aa9715}</MetaDataID>
        double Height { get; set; }
    }
}