namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{6aaaca01-fdc9-49f8-aecf-8222929f73a5}</MetaDataID>
    public interface IPageStyle : IStyleRule
    {
        /// <MetaDataID>{4ddf15d7-5d5e-425a-97f2-f2a800e46ba5}</MetaDataID>
        double LineSpacing { set; get; }

        /// <MetaDataID>{ac6dd6cc-c282-40a4-b762-9c029212f8a1}</MetaDataID>
        Margin BorderMargin { get; set; }

        /// <MetaDataID>{9978da9f-7a6c-47a6-80df-f7f7f92fbb9d}</MetaDataID>
        Resource Background { get; set; }
        /// <MetaDataID>{19420685-9c0f-4017-8401-72a1ec6f0763}</MetaDataID>
        Resource Border { get; set; }

        /// <MetaDataID>{8ddcfa88-b081-4e0e-a4b4-4f26f37370d5}</MetaDataID>
        double PageWidth { get; set; }

        /// <MetaDataID>{427151c2-6ba1-4208-9881-8549288eaad9}</MetaDataID>
        double PageHeight { get; set; }

        /// <MetaDataID>{8c119a49-6a05-4c9d-b656-5a14589adde3}</MetaDataID>
        Margin Margin { get; set; }
    }
}