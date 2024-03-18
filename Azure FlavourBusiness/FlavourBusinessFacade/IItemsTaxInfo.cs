using FinanceFacade;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.PriceList
{
    /// <MetaDataID>{f4763532-c2fc-4f61-a6d4-5d6b4b2844df}</MetaDataID>
    public interface IItemsTaxInfo
    {
        /// <MetaDataID>{c24149b7-1de0-4202-9a49-9e0bf49fc59c}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        MenuModel.IClassified MenuModelObject { get; }

        /// <MetaDataID>{319de3c7-53f4-4ed5-9be7-28b33373ae70}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        string ItemsInfoObjectUri { get; set; }

        /// <MetaDataID>{a46721de-f28f-4d02-bc5c-d161f9c7ea73}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        ItemsPriceInfoType ItemsPriceInfoType { get; set; }

        /// <MetaDataID>{9b7e011f-a90f-4ecf-a7a5-333093e01fb0}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        ITaxableType TaxableType { get; }

        /// <MetaDataID>{bb1e3b11-b4f6-4fee-8b4f-a95793700d87}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+5")]
        string Description { get; set; }
    }
}