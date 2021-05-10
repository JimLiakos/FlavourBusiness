using OOAdvantech.MetaDataRepository;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{6aaaca01-fdc9-49f8-aecf-8222929f73a5}</MetaDataID>
    public interface IPageStyle : IStyleRule
    {
        int ColumnsUneven { get; set; }

        /// <MetaDataID>{58697dae-f5dd-4091-a871-5bcc18d0ca7f}</MetaDataID>
        int NumOfPageColumns { get; set; }

        /// <MetaDataID>{0b0e5e26-33d7-4704-a5db-a7d7dc08268d}</MetaDataID>
        Margin BackgroundMargin { get; set; }


        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [Association("PageBackground", Roles.RoleA, "992fe09b-1712-4b60-aae4-4c5c478b7c40")]
        IPageImage Background { get; set; }


        /// <MetaDataID>{f8a19b42-90c3-402b-9f9d-29797a99859b}</MetaDataID>
        ImageStretch BackgroundStretch { get; set; }


        /// <MetaDataID>{e3c19ce5-0424-4f31-94ae-3f6c66145495}</MetaDataID>
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [Association("PageBorder", Roles.RoleA, "4d565e8e-efaa-4bc6-b4ee-884f422a4ebe")]
        IPageImage Border { get; set; }

        /// <MetaDataID>{4ddf15d7-5d5e-425a-97f2-f2a800e46ba5}</MetaDataID>
        double LineSpacing { set; get; }

        /// <MetaDataID>{ac6dd6cc-c282-40a4-b762-9c029212f8a1}</MetaDataID>
        Margin BorderMargin { get; set; }



        /// <MetaDataID>{8ddcfa88-b081-4e0e-a4b4-4f26f37370d5}</MetaDataID>
        double PageWidth { get; set; }

        /// <MetaDataID>{427151c2-6ba1-4208-9881-8549288eaad9}</MetaDataID>
        double PageHeight { get; set; }

        /// <MetaDataID>{ee86c4c1-819b-4e91-84d7-5d677aa239c4}</MetaDataID>
        PaperSize PageSize { get; set; }

        /// <MetaDataID>{8c119a49-6a05-4c9d-b656-5a14589adde3}</MetaDataID>
        Margin Margin { get; set; }


        /// <MetaDataID>{e50ed5ca-d3b0-47aa-b2ea-9f9becd3f7b2}</MetaDataID>
        bool IsPortrait { get; }

        /// <MetaDataID>{313a1c74-e724-4f41-86b8-38ec30adf55e}</MetaDataID>
        bool IsLandscape { get; }

        /// <MetaDataID>{8eb85afd-78d9-4063-8bfa-cddedd0b11b3}</MetaDataID>
        bool IsOrgPropertyValue(string propertyName, object value);
    }

    //Stretch="UniformToFill"   preserveAspectRatio = "xMidYMid slice"
    //Stretch="Uniform" preserveAspectRatio = "xMidYMid meet"  Stretch to fit false
    //Stretch="Fill" preserveAspectRatio = "none" Stretch to fit true


    /// <MetaDataID>{c12282a3-26c3-418d-8979-54923631502f}</MetaDataID>
    public enum ImageStretch
    {
        /// <summary>
        ///    The content preserves its original size.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The content is resized to fill the destination dimensions. The aspect ratio is
        ///     not preserved.
        /// </summary>
        Fill = 1,

        /// <summary>
        ///     The content is resized to fit in the destination dimensions while it preserves
        ///     its native aspect ratio.
        /// </summary>
        Uniform = 2,

        /// <summary>
        ///     The content is resized to fill the destination dimensions while it preserves
        ///     its native aspect ratio. If the aspect ratio of the destination rectangle differs
        ///     from the source, the source content is clipped to fit in the destination dimensions.
        /// </summary>
        UniformToFill = 3

    }
}