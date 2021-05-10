namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{14ed9fe7-f4ae-4ec2-a80d-fdce5b7dc714}</MetaDataID>
    public interface IHeadingAccent
    {
        /// <MetaDataID>{ff75092e-bdfb-41fa-a340-362d09bcb325}</MetaDataID>
        string Name { get; set; }

        /// <MetaDataID>{577992ee-931c-42f2-824b-248b271dcfb1}</MetaDataID>
        System.Collections.Generic.List<Resource> AccentImages { get; }

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

    }
}