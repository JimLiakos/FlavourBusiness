namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{0b5db3c3-e3e5-4e19-ab79-67735262cfce}</MetaDataID>
    public interface IFontData
    {
        /// <MetaDataID>{ce7510d5-ec6b-4eb1-a745-024e8a77821b}</MetaDataID>
        double FontSize { get; set; }

        /// <MetaDataID>{f82f13eb-ae83-41bf-bebc-c0834e3ad694}</MetaDataID>
        string FontStyle { get; set; }

        /// <MetaDataID>{e2ad3e2c-4ff6-4b1e-a40e-c5f89a9dc834}</MetaDataID>
        string FontWeight { get; set; }


        /// <MetaDataID>{fd64519e-75aa-490e-a14d-4722fc83ef42}</MetaDataID>
        string FontFamilyName { get; set; }


        /// <MetaDataID>{9c2f8646-23f3-4a83-be65-594a3a8bafaa}</MetaDataID>
        string Foreground { get; set; }

        /// <MetaDataID>{92539082-ce81-45ac-bba5-7517452061a1}</MetaDataID>
        bool Stroke { get; set; }

        /// <MetaDataID>{dc463f2a-c80e-407c-b6af-c1300f5836f1}</MetaDataID>
        bool AllCaps { get; set; }

        /// <MetaDataID>{b9334b15-c817-4d57-bb64-95ce32ed9e54}</MetaDataID>
        bool Shadow { get; set; }
    }
}