using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel
{
    /// <MetaDataID>{9b36a7a9-8d83-4ecd-8bf7-4715b01af545}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{9b36a7a9-8d83-4ecd-8bf7-4715b01af545}")]
    public interface IItemExtraInfoStyleSheet
    {
        /// <MetaDataID>{8fc8209a-2e27-4b55-8dda-82afdb105a29}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        UIBaseEx.FontData ParagraphFont { get; set; }

        /// <MetaDataID>{7f227e0d-fce9-473c-a5ad-37c85936a8ff}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        UIBaseEx.FontData? ParagraphFirstLetterFont { get; set; }

        /// <MetaDataID>{9d6a87bf-84d8-4f83-9e8f-1d807f6b626f}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        UIBaseEx.FontData HeadingFont { get; set; }

        /// <MetaDataID>{97513c59-c1fe-4a54-891b-147c0e5c78f1}</MetaDataID>
        int? ItemInfoFirstLetterLeftIndent { get; set; }
        /// <MetaDataID>{211d7e6a-562b-49c8-bc1b-ef7d32d62e61}</MetaDataID>
        int? ItemInfoFirstLetterRightIndent { get; set; }
        /// <MetaDataID>{ba3d4244-5e26-4ec6-b6d0-3ac3226505e8}</MetaDataID>
        int? ItemInfoFirstLetterLinesSpan { get; set; }



        /// <MetaDataID>{c6ce4167-58d2-4a1f-adf8-89819a7c4f54}</MetaDataID>
        Multilingual MultilingualHeadingFont { get; }

        /// <MetaDataID>{f218c9e6-d88b-4858-af2e-dbae1a1e2f6d}</MetaDataID>
        Multilingual MultilingualParagraphFont { get; }
        /// <MetaDataID>{c42b2fcd-9edf-4129-8fc5-0e14d4a374d7}</MetaDataID>
        Multilingual MultilingualParagraphFirstLetterFont { get; }

        /// <MetaDataID>{9970094a-0655-4cc6-8ed3-6086d04f951e}</MetaDataID>
        Multilingual MultilingualItemInfoFirstLetterLeftIndent { get; }
        /// <MetaDataID>{850f111c-4d74-4c97-864d-94e69c31e020}</MetaDataID>
        Multilingual MultilingualItemInfoFirstLetterRightIndent { get; }
        /// <MetaDataID>{cb8ef636-67d8-4c35-acfa-ca5502ebb4c5}</MetaDataID>
        Multilingual MultilingualItemInfoFirstLetterLinesSpan { get; }


    }
}