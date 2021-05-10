//using System.Web.UI.WebControls;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{14ed9fe7-f4ae-4ec2-a80d-fdce5b7dc714}</MetaDataID>
    [BackwardCompatibilityID("{14ed9fe7-f4ae-4ec2-a80d-fdce5b7dc714}")]
    public interface IAccent
    {
        /// <MetaDataID>{279becaf-392f-4046-9081-2147c938f616}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        double Width { get; set; }

        /// <MetaDataID>{ed1f650f-dd00-42d1-b2ec-6fa545593427}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        bool OrgSize { get; set; }

        /// <MetaDataID>{58283f3e-3a6b-49d7-aef4-cead7b4a78c7}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        double MinHeight { get; set; }

        /// <MetaDataID>{5a8a203d-e1cd-4fe9-9d8f-167457652801}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double MinWidth { get; set; }

        /// <MetaDataID>{35a7d8ee-82fb-42c2-b811-dd6862c71ec1}</MetaDataID>
        [BackwardCompatibilityID("+18")]
        Unit MarginUnit { get; set; }

        /// <MetaDataID>{d6e7ee24-4dc1-4fae-b3bf-c648f71a8022}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        bool MultipleItemsAccent { get; set; }

     

        event ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{1d7c1fc2-791c-4d08-b775-e8016c7af650}</MetaDataID>
        string SelectionAccentImageUri { get; set; }

        /// <MetaDataID>{9140a1b6-e9b3-4ad0-b376-378b134d93d3}</MetaDataID>
        [Association("HeadingAccentImages", Roles.RoleA, "3a455243-9391-453c-92ce-9e9a8f16387a")]
        System.Collections.Generic.IList<IImage> AccentImages { get; }

        /// <MetaDataID>{ff75092e-bdfb-41fa-a340-362d09bcb325}</MetaDataID>
        string Name { get; set; }


        /// <MetaDataID>{f079f7f9-9acf-4bbf-9977-875cf70fb39b}</MetaDataID>
        void AddAccentImage(MenuPresentationModel.MenuStyles.IImage accentImage);
        /// <MetaDataID>{39e00055-1664-4e71-8d40-58afba8ff961}</MetaDataID>
        void DeleteAccentImage(MenuPresentationModel.MenuStyles.IImage accentImage);

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

        /// <MetaDataID>{3dce7e67-feda-4a10-85a4-2ea260d3f0c9}</MetaDataID>
        string AccentColor { get; set; }

        /// <MetaDataID>{b980985e-d216-49e9-b463-82b69ebc7463}</MetaDataID>
        void ChangeOrgStyle(IAccent orgHeadingAccent);
        /// <MetaDataID>{85d15a0d-fea5-4067-a4cb-b6ed6acbba50}</MetaDataID>
        bool IsTheSameWith(IAccent accent);
    }
}