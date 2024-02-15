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
        UIBaseEx.FontData ParagraphFirstLetterFont { get; set; }

        /// <MetaDataID>{9d6a87bf-84d8-4f83-9e8f-1d807f6b626f}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        UIBaseEx.FontData HeadingFont { get; set; }

        /// <MetaDataID>{c6ce4167-58d2-4a1f-adf8-89819a7c4f54}</MetaDataID>
        Multilingual MultilingualHeadingFont { get; }

        /// <MetaDataID>{f218c9e6-d88b-4858-af2e-dbae1a1e2f6d}</MetaDataID>
        Multilingual MultilingualParagraphFont { get; }
        /// <MetaDataID>{c42b2fcd-9edf-4129-8fc5-0e14d4a374d7}</MetaDataID>
        Multilingual MultilingualParagraphFirstLetterFont { get; }
    }
}