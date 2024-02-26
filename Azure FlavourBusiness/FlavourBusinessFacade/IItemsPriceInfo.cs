using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.PriceList
{
    /// <MetaDataID>{7086923d-a949-4e1e-9bec-bbbc0430f181}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{7086923d-a949-4e1e-9bec-bbbc0430f181}")]
    public interface IItemsPriceInfo
    {
        /// <MetaDataID>{672fa328-3c4c-4a35-bd8b-3bc3c97409d1}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        double? OverridenPrice { get; set; }

        /// <MetaDataID>{e3d5df84-1222-4050-8576-c05471857bf0}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        double? AmountDiscount { get; set; }

        /// <MetaDataID>{ce678a17-0d59-4d88-89d4-35cc9de06524}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        double? Pricerounding { get; set; }

        /// <MetaDataID>{2b732863-faa1-4024-8bfb-5abff5af891d}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        double? PercentageDiscount { get; set; }

        /// <MetaDataID>{5767c6cb-c6d3-44c1-bd5f-96b7fc3cc395}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }

        /// <MetaDataID>{58e165c5-e9af-4c4d-854e-96cd6b6c145c}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        IClassified MenuModelObject { get; }

        /// <MetaDataID>{3bb37c90-f505-4f75-90e1-484a9cedf2b7}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        ItemsPriceInfoType ItemsPriceInfoType { get; set; }


    }


    /// <MetaDataID>{69317d8d-a74c-4983-b880-d2fee973f01f}</MetaDataID>
    public enum ItemsPriceInfoType
    {
        Include = 1,
        Exclude = 2,
    }
}