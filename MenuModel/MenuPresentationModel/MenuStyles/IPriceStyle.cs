namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{a88b5361-418e-4bf3-a4bb-a7711ffd9788}</MetaDataID>
    public interface IPriceStyle : IStyleRule
    {
        /// <MetaDataID>{b8e4c163-e0c0-4874-8f42-757c3e8fee70}</MetaDataID>
        PriceLayout Layout { get; set; }

        /// <MetaDataID>{d4d25ffb-36ca-4329-8727-94174e487bd4}</MetaDataID>
        string PriceLeader { get; set; }

        /// <MetaDataID>{3b6c8559-0e2c-401d-bc77-ec9cdd35d9b5}</MetaDataID>
        bool DotsMatchNameColor { get; set; }

        /// <MetaDataID>{f02a2631-1f31-455a-af98-7aeb04bba9e1}</MetaDataID>
        bool DisplayCurrencySymbol { get; set; }

        /// <MetaDataID>{3e5885d2-1d85-41bd-be3b-a3ac34e738a1}</MetaDataID>
        int BetweenDotsSpace { get; set; }

        /// <MetaDataID>{ae759e31-b9fe-480c-a77c-173d6c362306}</MetaDataID>
        double DotsSpaceFromPrice { get; set; }
        /// <MetaDataID>{ead1b2a8-f97b-4067-85be-80b66a7dc551}</MetaDataID>
        double DotsSpaceFromItem { get; set; }

        /// <MetaDataID>{78929366-041d-4b63-b84c-01181e11da94}</MetaDataID>
        FontData Font { get; set; }

        /// <MetaDataID>{1c3b4d58-91b9-4bb6-854a-f15d6a5c3e7e}</MetaDataID>
        void RestFont();
    }


    /// <MetaDataID>{913f997d-0b63-4bd9-8695-3de55317b4fe}</MetaDataID>
    public enum PriceLayout
    {
        Normal,
        FollowDescription,
        WithName,
        WithDescription,
        DoNotDisplay
    }
}