using MenuModel;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

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


        /// <MetaDataID>{2ee663b8-a65c-46fe-9f6b-a71371070f01}</MetaDataID>
        IItemsPriceInfo PriceListMainItemsPriceInfo { get; }

        /// <MetaDataID>{a78aab39-d9ad-45cd-84cf-3fb9b609610c}</MetaDataID>
        void AddItemsPriceInfos(IItemsPriceInfo itemsPriceInfo);




        /// <MetaDataID>{3ccdfdeb-48d4-4dac-b793-7b45b06bc765}</MetaDataID>
        double? GetPercentageDiscount(object priceListSubject);

        /// <MetaDataID>{3281d9b3-3196-4fc7-bb2c-d68f4458ff9b}</MetaDataID>
        double? GetPriceRounding(object priceListSubject);

        /// <MetaDataID>{8e75ce6c-0bd9-49b8-9faa-796f903796a0}</MetaDataID>
        bool IsOptionsPricesDiscountEnabled(object priceListSubject);
        /// <MetaDataID>{8c3128f9-4cf3-4d88-af2d-1895d319c048}</MetaDataID>
        double? GetOptionsPricesRounding(object priceListSubject);

        /// <MetaDataID>{75706c57-0795-43e4-9680-1758fcfa9c40}</MetaDataID>
        decimal? GetOverridePrice(object priceListSubject);

        /// <MetaDataID>{21f22e71-e1c1-4045-9b25-bf9f0285a652}</MetaDataID>
        double? GetAmountDiscount(object priceListSubject);






        /// <MetaDataID>{516aa500-46ce-4c94-9649-cff8228c611a}</MetaDataID>
        IItemsPriceInfo NewPriceInfo(string uri, ItemsPriceInfoType include);

        /// <MetaDataID>{83f3e18b-a96c-4917-a4b5-c766bd336fff}</MetaDataID>
        void RemoveItemsPriceInfos(IItemsPriceInfo itemsPriceInfo);

        /// <MetaDataID>{e01da134-72d7-4331-912d-98d67e8e4bd7}</MetaDataID>
        void RemoveItemsPriceInfos(List<IItemsPriceInfo> itemsPriceInfos);



        ///// <MetaDataID>{01882af7-d78b-437c-bfb1-eb78bd497ba8}</MetaDataID>
        //decimal? GetFinalPrice(IMenuItem menuItem);

        ///// <MetaDataID>{179aa811-4925-42aa-af89-b346eef586c8}</MetaDataID>
        //decimal? GetFinalPrice(IMenuItemPrice menuItemPrice);
        ///// <MetaDataID>{5d017c6f-631b-41c6-8cc8-0ef4a29676e0}</MetaDataID>
        //decimal? GetFinalPrice(IPreparationScaledOption option, IMenuItemPrice itemPrice);

        /// <MetaDataID>{4cd0bfe9-f963-4521-8915-1e3c56040cfd}</MetaDataID>
        IPricingContext GetDerivedPriceContext(IPricingContext pricingContext);
    }
}