using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{276e0657-e957-48cb-8a03-8b3980d810a6}</MetaDataID>
    public interface IMenuItemStyle : IStyleRule
    {
        /// <MetaDataID>{7cf01b8b-5777-4977-8809-fc61cdff6133}</MetaDataID>
        FontData ItemInfoParagraphFont { get; set; }

        /// <MetaDataID>{fa42585b-2a63-4570-b692-3e1b838a9487}</MetaDataID>
        FontData ItemInfoHeadingFont { get; set; }

        /// <MetaDataID>{cfcb02a4-933a-4853-9b63-7c28209cacd5}</MetaDataID>
        FontData? ItemInfoParagraphFirstLetterFont { get; set; }
        /// <MetaDataID>{003de667-6bd7-4c63-901a-74cced8d870a}</MetaDataID>
        int? ItemInfoFirstLetterLeftIndent { get; set; }
        /// <MetaDataID>{7cf01a2b-a34f-4cc4-b511-8e8e618e942f}</MetaDataID>
        int? ItemInfoFirstLetterRightIndent { get; set; }
        /// <MetaDataID>{1aacd165-acb4-4234-ab08-37b3805e243e}</MetaDataID>
        int? ItemInfoFirstLetterLinesSpan { get; set; }

        /// <MetaDataID>{2d6618c8-bcbf-48b7-9ea5-4b7f7cb5ab5e}</MetaDataID>
        bool NewLineForDescription { get; set; }

        /// <MetaDataID>{5b3c4812-1474-485c-bd03-b1dbf695c1df}</MetaDataID>
        double Indent { get; set; }

        /// <MetaDataID>{d38f1e0b-17bf-4928-b065-8a7d0f976176}</MetaDataID>
        FontData Font { get; set; }

        /// <MetaDataID>{979e35dd-4b86-4ce8-ac5d-690c9af54b83}</MetaDataID>
        FontData ExtrasFont { get; set; }

        /// <MetaDataID>{7e73fbdc-f2fd-4ec9-bce6-1139ad7c1ea6}</MetaDataID>
        FontData DescriptionFont { get; set; }

        /// <MetaDataID>{d5a08ae6-91a9-4aa6-bdf7-23630ba29803}</MetaDataID>
        Alignment Alignment { get; set; }

        /// <MetaDataID>{5be9674e-0eeb-41c3-943c-9997a4991dcd}</MetaDataID>
        double AfterSpacing { get; set; }

        /// <MetaDataID>{5bd5690d-7b05-4668-ac50-a3ef8da1dd7f}</MetaDataID>
        double BeforeSpacing { get; set; }

        /// <MetaDataID>{2ff558a4-e673-44cf-80fa-171d649ffd3b}</MetaDataID>
        void RestFont();
    }
}