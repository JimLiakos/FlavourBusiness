using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.PriceList
{
    /// <MetaDataID>{f3227625-fb47-47aa-b911-5eed56e5d67b}</MetaDataID>
    public interface IPriceList
    {
        /// <MetaDataID>{9f8f9bcc-5335-49eb-bc6b-6d761457d913}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }

        /// <MetaDataID>{b7d3eff6-6330-43ff-ac44-ea681c395788}</MetaDataID>
        [Association("PriceListItems", Roles.RoleA, "712c2573-54d4-4370-9589-e51517f911b5")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.List<IItemsPriceInfo> ItemsPrices { get; }


        /// <MetaDataID>{a78aab39-d9ad-45cd-84cf-3fb9b609610c}</MetaDataID>
        void AddItemsPriceInfos(IItemsPriceInfo itemsPriceInfo);
        /// <MetaDataID>{83f3e18b-a96c-4917-a4b5-c766bd336fff}</MetaDataID>
        void RemoveItemsPriceInfos(IItemsPriceInfo itemsPriceInfo);


    }
}