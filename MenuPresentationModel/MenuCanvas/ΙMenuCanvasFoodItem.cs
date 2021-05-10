using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{b471b59e-d3e6-4e1b-8ec6-8fc793b10fb6}</MetaDataID>
    public interface IMenuCanvasFoodItem : IGroupedItem, IHighlightedMenuCanvasItem
    {
        /// <MetaDataID>{fde909fc-33cf-40a5-a2be-462ea8880e3b}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        bool PriceInvisible { get; set; }

        /// <MetaDataID>{4ae9a1b8-44de-49c4-b2da-1997dec02873}</MetaDataID>
        string FullDescription { get; set; }

        /// <MetaDataID>{32d69013-f89f-4947-b54b-74189b8bb679}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        bool CustomSpacing { get; set; }

        /// <MetaDataID>{7a278d84-a93c-45e8-9b68-3c605ee9dd1a}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double AfterSpacing { get; set; }

        /// <MetaDataID>{ac8e6139-56bd-449f-80f5-4a3a05b6003b}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        double BeforeSpacing { get; set; }


        /// <MetaDataID>{b9fce9bc-0606-4671-8d56-28b2db74a83a}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        bool Span { get; set; }

        /// <MetaDataID>{71f38310-6ada-4e3e-a584-dfe237108607}</MetaDataID>
        double MaxHeight { get; set; }



        /// <MetaDataID>{258fcb25-5c39-44a9-8156-fd070f3b25c2}</MetaDataID>
        string ExtraDescription { get; set; }



        [Association("FoodItemsMultiPriceHeading", Roles.RoleA, "cd60ac6b-0cb2-442c-95fe-37801f2138f9")]
        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        IItemMultiPriceHeading MultiPriceHeading { set; get; }

        [Association("FoodItemMainPrice", Roles.RoleA, "278de439-3a48-47d8-ba79-5d0069285a73")]
        IMenuCanvasFoodItemPrice MainPrice { get; set; }

        [Association("ActualMenuItem", Roles.RoleA, "8295bf31-a830-4551-9b9a-d02615f1ead5")]
        [RoleAMultiplicityRange(1, 1)]
        MenuModel.IMenuItem MenuItem { get; set; }

#if MenuPresentationModel
        /// <MetaDataID>{c4253ba0-4ca1-4a7b-a71e-6d553b0f2a76}</MetaDataID>
        MenuStyles.IMenuItemStyle Style { get; }
#endif



        /// <MetaDataID>{8bd9c1be-fe35-4ec1-b3ea-43921ecc569e}</MetaDataID>
        string Extras { get; set; }


        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [Association("FoodItemSubTexts", Roles.RoleA, "6848fa01-6ad3-40c0-8812-21e3248a2a91")]
        System.Collections.Generic.IList<IMenuCanvasFoodItemText> SubTexts { get; }

        /// <MetaDataID>{de26b51c-1531-4fcf-ad2d-1269d3f79774}</MetaDataID>
        void AddSubText(IMenuCanvasFoodItemText subText);

        /// <MetaDataID>{7035b7e5-2e34-49cb-a2dc-841c65921fb5}</MetaDataID>
        void RemoveSubText(IMenuCanvasFoodItemText subText);

        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [Association("FoodItemPriceLeader", Roles.RoleA, "9e0b48c5-d77e-4e97-b510-76e56d8c9266")]
        IMenuCanvasPriceLeader PriceLeader { get; set; }

        /// <MetaDataID>{50c8ca68-ba25-4d8f-b69b-981b4f22fddd}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [Association("FoodItemPrices", Roles.RoleA, true, "9af888d3-e504-4524-9994-389228a2c2e0")]
        System.Collections.Generic.List<IMenuCanvasFoodItemPrice> Prices { get; }
        /// <MetaDataID>{abfd30b3-7edb-4e9a-b4d9-6246bc8459c8}</MetaDataID>
        Rect CanvasFrameArea { get; }



        /// <MetaDataID>{aa6c806f-9353-45fc-8289-ba6278ae4a09}</MetaDataID>
        void AddFoodItemPrice(IMenuCanvasFoodItemPrice foodItemPrice);


        /// <MetaDataID>{e6a4bd0a-2f24-4240-a71f-28831136cbc5}</MetaDataID>
        void RemoveFoodItemPrice(IMenuCanvasFoodItemPrice foodItemPrice);


    }
}
