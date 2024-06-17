using FinanceFacade;
using FlavourBusinessFacade.PriceList;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicesContextResources;
using MenuModel;

using OOAdvantech;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.PriceList
{
    /// <MetaDataID>{089749b1-083b-4348-b598-6fd512b853a5}</MetaDataID>
    [BackwardCompatibilityID("{089749b1-083b-4348-b598-6fd512b853a5}")]
    [Persistent()]
    public class PriceList : IPriceList, IPricingContext
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemsPriceInfo> _ItemsPrices = new OOAdvantech.Collections.Generic.Set<IItemsPriceInfo>();
        /// <MetaDataID>{5bc8adba-d38c-4992-98a6-6a3ce5881759}</MetaDataID>
        [PersistentMember(nameof(_ItemsPrices))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete | PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+1")]
        public System.Collections.Generic.List<FlavourBusinessFacade.PriceList.IItemsPriceInfo> ItemsPrices
        {
            get
            {
                var itemsPrices = _ItemsPrices.ToThreadSafeList();
                FlavourBusinessFacade.PriceList.IItemsPriceInfo itemsPriceInfo = itemsPrices.Where(x => x.ItemsInfoObjectUri == null).FirstOrDefault();
                //if (itemsPriceInfo == null)
                //{
                //    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));
                //    itemsPriceInfo=(from the_itemsPriceInfo in storage.GetObjectCollection<IItemsPriceInfo>()
                //                    select the_itemsPriceInfo).ToList().Where(x => string.IsNullOrWhiteSpace(x.ItemsInfoObjectUri)).FirstOrDefault();
                //    if (itemsPriceInfo != null)
                //    {

                //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                //        {
                //            _ItemsPrices.Add(itemsPriceInfo);
                //            stateTransition.Consistent = true;
                //        }

                //    }

                //}
                if (itemsPriceInfo != null)
                    itemsPriceInfo.ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).GetPersistentObjectUri(this);
                return itemsPrices;

            }
        }

        /// <exclude>Excluded</exclude>
        IItemsPriceInfo _PriceListMainItemsPriceInfo;
        /// <MetaDataID>{a810e92f-6441-42c5-8c28-975cbdac8593}</MetaDataID>
        public IItemsPriceInfo PriceListMainItemsPriceInfo
        {
            get
            {
                if (_PriceListMainItemsPriceInfo == null)
                {
                    var priceListUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).GetPersistentObjectUri(this);
                    FlavourBusinessFacade.PriceList.IItemsPriceInfo itemsPriceInfo = ItemsPrices.Where(x => x.ItemsInfoObjectUri == priceListUri).FirstOrDefault();
                    if (itemsPriceInfo != null)
                        _PriceListMainItemsPriceInfo = itemsPriceInfo;
                }
                return _PriceListMainItemsPriceInfo;
            }
        }


        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{c99d37ac-f822-4c25-b668-f7bf7cf9f527}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get => _Description; set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{610d0ffe-619e-4b14-81e4-5463bada60c9}</MetaDataID>
        public string Name { get =>""; set { }   }

        /// <MetaDataID>{fa0af410-ee21-428e-93c3-75bc654874fc}</MetaDataID>
        public IList<ICustomizedPrice> PricedSubjects => new List<ICustomizedPrice>();


        /// <exclude>Excluded</exclude>
        [OOAdvantech.Json.JsonIgnore]
        Member<ITaxesContext> _TaxesContext=new Member<ITaxesContext>();

        /// <MetaDataID>{fb27de47-6bf5-4a7c-af11-3197ad91d7ed}</MetaDataID>
        [PersistentMember(nameof(_TaxesContext))]
        [BackwardCompatibilityID("+4")]
        [OOAdvantech.Json.JsonIgnore]
        public ITaxesContext TaxesContext
        {
            get => _TaxesContext.Value;
            set
            {
                if (_TaxesContext != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TaxesContext.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        ///// <exclude>Excluded</exclude>
        //OOAdvantech.Collections.Generic.Set<IItemsTaxInfo> _ItemsTaxes = new OOAdvantech.Collections.Generic.Set<IItemsTaxInfo>();

        ///// <MetaDataID>{2f321a14-72bd-49ed-816c-6ad28f8cfec6}</MetaDataID>
        //[PersistentMember(nameof(_ItemsTaxes))]
        //[BackwardCompatibilityID("+3")]
        //public List<IItemsTaxInfo> ItemsTaxes => _ItemsTaxes.ToThreadSafeList();

        /// <MetaDataID>{0131fa91-7249-4e84-b030-a40b49940389}</MetaDataID>
        public void AddItemsPriceInfos(IItemsPriceInfo itemsPriceInfo)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ItemsPrices.Add(itemsPriceInfo);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{68e9146a-561a-49c4-935c-c83d784aa782}</MetaDataID>
        public void RemoveItemsPriceInfos(IItemsPriceInfo itemsPriceInfo)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ItemsPrices.Remove(itemsPriceInfo);
                stateTransition.Consistent = true;
            }
        }

        //public void RemoveItemsTaxInfos(IItemsTaxInfo itemsTaxInfo)
        //{
        //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //    {
        //        _ItemsTaxes.Remove(itemsTaxInfo);
        //        stateTransition.Consistent = true;
        //    }
        //}

        //public void RemoveItemsTaxInfos(List<IItemsTaxInfo> itemsTaxInfos)
        //{
        //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //    {
        //        foreach (IItemsTaxInfo itemsTaxInfo in itemsTaxInfos)
        //            this._ItemsTaxes.Remove(itemsTaxInfo);
        //        stateTransition.Consistent = true;
        //    }
        //    ObjectChangeState?.Invoke(this, null);
        //}


        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{2cfb3a5d-e099-48a6-917a-848ac5fae692}</MetaDataID>
        public void RemoveItemsPriceInfos(List<IItemsPriceInfo> itemsPriceInfos)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var itemsPreparationInfo in itemsPriceInfos)
                    this._ItemsPrices.Remove(itemsPreparationInfo);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, null);

        }

        /// <MetaDataID>{a205c02f-4c7e-47c5-996f-ae9a9341b264}</MetaDataID>
        public double? GetPercentageDiscount(object priceListSubject)
        {

            var the_itemsPriceInfo = this.GetItemPriceInfo(priceListSubject);
            if (the_itemsPriceInfo != null && the_itemsPriceInfo.IsIncluded())
                return the_itemsPriceInfo.PercentageDiscount;

            var itemsPriceInfo = this.GetItemsPriceInfo(priceListSubject).FirstOrDefault();

            if (itemsPriceInfo != null)
                return itemsPriceInfo.PercentageDiscount;

            return PriceListMainItemsPriceInfo.PercentageDiscount;
        }
        /// <MetaDataID>{af69c6b9-727b-4a71-af37-eb1b03b8a3e9}</MetaDataID>
        public double? GetPriceRounding(object priceListSubject)
        {
            var the_itemsPriceInfo = this.GetItemPriceInfo(priceListSubject);
            if (the_itemsPriceInfo != null && the_itemsPriceInfo.IsIncluded())
                return the_itemsPriceInfo.PriceRounding;

            var itemsPriceInfo = this.GetItemsPriceInfo(priceListSubject).FirstOrDefault();
            if (itemsPriceInfo != null)
                return itemsPriceInfo.PriceRounding;

            if (PriceListMainItemsPriceInfo.PercentageDiscount != null && PriceListMainItemsPriceInfo.PriceRounding != null)
                return PriceListMainItemsPriceInfo.PriceRounding;
            return null;
        }

        /// <MetaDataID>{43575e9d-c145-4fed-83ed-142de2aa289f}</MetaDataID>
        public bool IsOptionsPricesDiscountEnabled(object priceListSubject)
        {
            var the_itemsPriceInfo = this.GetItemPriceInfo(priceListSubject);
            if (the_itemsPriceInfo != null && the_itemsPriceInfo.IsIncluded())
                return the_itemsPriceInfo.IsOptionsPricesDiscountEnabled == true;

            var itemsPriceInfo = this.GetItemsPriceInfo(priceListSubject).FirstOrDefault();
            if (itemsPriceInfo != null)
                return itemsPriceInfo.IsOptionsPricesDiscountEnabled == true;

            if (PriceListMainItemsPriceInfo.IsOptionsPricesDiscountEnabled == true)
                return true;
            else
                return false;
        }

        /// <MetaDataID>{262d8279-c095-41f9-8bc9-c3248ab81a68}</MetaDataID>
        public double? GetOptionsPricesRounding(object priceListSubject)
        {

            var the_itemsPriceInfo = this.GetItemPriceInfo(priceListSubject);
            if (the_itemsPriceInfo != null && the_itemsPriceInfo.IsIncluded())
                return the_itemsPriceInfo.OptionsPricesRounding;

            var itemsPriceInfo = this.GetItemsPriceInfo(priceListSubject).FirstOrDefault();
            if (itemsPriceInfo != null)
                return itemsPriceInfo.OptionsPricesRounding;


            if (this.PriceListMainItemsPriceInfo.PercentageDiscount != null && this.PriceListMainItemsPriceInfo.OptionsPricesRounding != null)
                return this.PriceListMainItemsPriceInfo.OptionsPricesRounding;
            return null;
        }




        /// <MetaDataID>{8d0ecbae-2d32-4514-83f5-14896d0511a5}</MetaDataID>
        public decimal? GetOverridePrice(object priceListSubject)
        {
            var the_itemsPriceInfo = this.GetItemPriceInfo(priceListSubject);
            if (the_itemsPriceInfo != null && the_itemsPriceInfo.IsIncluded())
                return the_itemsPriceInfo.OverriddenPrice;
            return null;
        }

        /// <MetaDataID>{8b331deb-3ab0-49ee-98f1-6c89595b7fa8}</MetaDataID>
        public double? GetAmountDiscount(object priceListSubject)
        {

            var the_itemsPriceInfo = this.GetItemPriceInfo(priceListSubject);
            if (the_itemsPriceInfo != null && the_itemsPriceInfo.IsIncluded())
                return the_itemsPriceInfo.AmountDiscount;

            var itemsPriceInfo = this.GetItemsPriceInfo(priceListSubject).FirstOrDefault();

            if (itemsPriceInfo != null)
                return itemsPriceInfo.AmountDiscount;

            return PriceListMainItemsPriceInfo.AmountDiscount;
        }

        /// <MetaDataID>{cbbb4db7-0a70-4867-a942-e58184352c3a}</MetaDataID>
        public IItemsPriceInfo NewPriceInfo(string itemsInfoObjectUri, ItemsPriceInfoType itemsPriceInfoType)
        {
            try
            {
                var obj = ObjectStorage.GetObjectFromUri(itemsInfoObjectUri);

                if (obj is MenuModel.ItemsCategory)
                {
                    ItemsPriceInfo itemsPriceInfo = new ItemsPriceInfo(obj as ItemsCategory);

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPriceInfo);
                        this._ItemsPrices.Add(itemsPriceInfo);
                        itemsPriceInfo.ItemsPriceInfoType = itemsPriceInfoType;

                        stateTransition.Consistent = true;
                    }

                    return itemsPriceInfo;
                }

                if (obj is MenuModel.IMenuItem)
                {
                    ItemsPriceInfo itemsPiceInfo = new ItemsPriceInfo(obj as MenuModel.IMenuItem);

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPiceInfo);
                        this._ItemsPrices.Add(itemsPiceInfo);
                        itemsPiceInfo.ItemsPriceInfoType = itemsPriceInfoType;
                        stateTransition.Consistent = true;
                    }
                    return itemsPiceInfo;
                }

                if (obj is IMenuItemPrice)
                {
                    ItemsPriceInfo itemsPiceInfo = new ItemsPriceInfo(obj as MenuModel.IMenuItemPrice);

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsPiceInfo);
                        this._ItemsPrices.Add(itemsPiceInfo);
                        itemsPiceInfo.ItemsPriceInfoType = itemsPriceInfoType;
                        stateTransition.Consistent = true;
                    }
                    return itemsPiceInfo;
                }
            }
            finally
            {
                ObjectChangeState?.Invoke(this, null);
            }

            return null;

        }



        /// <MetaDataID>{b66cd981-a4ba-4ba4-ab74-848e2a195d63}</MetaDataID>
        internal string Json
        {
            get
            {
                string json = JsonConvert.SerializeObject(this, Formatting.None, OOAdvantech.Remoting.RestApi.Serialization.JSonSerializeSettings.TypeRefSerializeSettings);
                return json;
            }
        }

        //public IItemsTaxInfo NewTaxInfo(string itemsInfoObjectUri, ItemsPriceInfoType itemsPriceInfoType)
        //{
        //    try
        //    {
        //        var obj = ObjectStorage.GetObjectFromUri(itemsInfoObjectUri);

        //        if (obj is MenuModel.ItemsCategory)
        //        {
        //            ItemsTaxInfo itemsTaxInfo = new ItemsTaxInfo(obj as ItemsCategory);

        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsTaxInfo);
        //                _ItemsTaxes.Add(itemsTaxInfo);
        //                itemsTaxInfo.ItemsPriceInfoType = itemsPriceInfoType;

        //                stateTransition.Consistent = true;
        //            }

        //            return itemsTaxInfo;
        //        }

        //        if (obj is MenuModel.IMenuItem)
        //        {
        //            ItemsTaxInfo itemsTaxInfo = new ItemsTaxInfo(obj as IMenuItem);

        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(itemsTaxInfo);
        //                _ItemsTaxes.Add(itemsTaxInfo);
        //                itemsTaxInfo.ItemsPriceInfoType = itemsPriceInfoType;
        //                stateTransition.Consistent = true;
        //            }
        //            return itemsTaxInfo;
        //        }


        //    }
        //    finally
        //    {
        //        ObjectChangeState?.Invoke(this, null);
        //    }

        //    return null;
        //}

        //public decimal? GetFinalPrice(IMenuItem menuItem)
        //{

        //    double? percentageDiscount = GetPercentageDiscount(menuItem);
        //    double? amountDiscount = GetAmountDiscount(menuItem);
        //    decimal? overridePrice = GetOverridePrice(menuItem);

        //    if (percentageDiscount != null && amountDiscount != null)
        //    {

        //    }
        //    if (percentageDiscount != null && overridePrice != null)
        //    {

        //    }
        //    if (amountDiscount != null && overridePrice != null)
        //    {

        //    }
        //    decimal? price = menuItem?.MenuItemPrice?.Price;
        //    if (price == null || price.Value == 0)
        //        return price;
        //    if (percentageDiscount != null)
        //    {

        //        var priceRounding = GetPriceRounding(menuItem);


        //        price = price.Value * (decimal)(1 - percentageDiscount.Value);
        //        if (priceRounding.HasValue)
        //            price = this.RoundPriceToNearest(price.Value, priceRounding.Value);
        //    }
        //    if (overridePrice != null)
        //        return overridePrice;

        //    if (amountDiscount != null)
        //        return price.Value - (decimal)amountDiscount;

        //    return price;

        //}

        //public decimal? GetFinalPrice(IMenuItemPrice menuItemPrice)
        //{

        //    double? percentageDiscount = GetPercentageDiscount(menuItemPrice);
        //    double? amountDiscount = GetAmountDiscount(menuItemPrice);
        //    decimal? overridePrice = GetOverridePrice(menuItemPrice);

        //    if (percentageDiscount != null && amountDiscount != null)
        //    {

        //    }
        //    if (percentageDiscount != null && overridePrice != null)
        //    {

        //    }
        //    if (amountDiscount != null && overridePrice != null)
        //    {

        //    }
        //    decimal? price = menuItemPrice?.Price;
        //    if (price == null || price.Value == 0)
        //        return price;


        //    if (this.GetItemPriceInfo(menuItemPrice)?.Excluded() == true)
        //        return price;
        //    if (percentageDiscount != null)
        //    {

        //        var priceRounding = GetPriceRounding(menuItemPrice);


        //        price = price.Value * (decimal)(1 - percentageDiscount.Value);
        //        if (priceRounding.HasValue)
        //            price = this.RoundPriceToNearest(price.Value, priceRounding.Value);
        //    }
        //    if (overridePrice != null)
        //        return overridePrice;

        //    if (amountDiscount != null)
        //        return price.Value - (decimal)amountDiscount;

        //    return price;
        //}

        //public decimal? GetFinalPrice(IPreparationScaledOption option, IMenuItemPrice menuItemPrice)
        //{
        //    double? percentageDiscount = GetPercentageDiscount(menuItemPrice);
        //    double? amountDiscount = GetAmountDiscount(menuItemPrice);
        //    decimal? overridePrice = GetOverridePrice(menuItemPrice);
        //    double? optionsPricesRounding = GetOptionsPricesRounding(menuItemPrice);
        //    var isOptionsPricesDiscountEnabled = IsOptionsPricesDiscountEnabled(menuItemPrice);


        //    if (percentageDiscount != null && amountDiscount != null)
        //    {
        //    }
        //    if (percentageDiscount != null && overridePrice != null)
        //    {
        //    }
        //    if (amountDiscount != null && overridePrice != null)
        //    {
        //    }

        //    decimal? price = null;
        //    if (option != null)
        //    {
        //        #region Gets option price
        //        if (menuItemPrice != null)
        //        {
        //            var customizedPrice = menuItemPrice.GetCustomizedPrice(option as IPricedSubject);
        //            if (customizedPrice != null)
        //                price = customizedPrice.Price;
        //            else
        //            {
        //                customizedPrice = (menuItemPrice as MenuModel.MenuItemPrice).ItemSelector?.GetCustomizedPrice(option as IPricedSubject);
        //                if (customizedPrice != null)
        //                    price = customizedPrice.Price;
        //                else
        //                    price = option.Price;

        //                // customizedPrice = ItemPrice.i.GetCustomizedPrice(Option as IPricedSubject);
        //            }

        //        }
        //        else
        //            price = option.Price;

        //        #endregion


        //        if (isOptionsPricesDiscountEnabled && this.GetItemPriceInfo(menuItemPrice)?.Excluded() != true)
        //        {
        //            // Apply discount
        //            price = price.Value * (decimal)(1 - percentageDiscount.Value);
        //            if (optionsPricesRounding.HasValue)
        //                price = this.RoundPriceToNearest(price.Value, optionsPricesRounding.Value);
        //        }

        //    }
        //    return price;

        //}




        /// <MetaDataID>{dd0742ab-217b-47cd-90eb-12dd4c3ca690}</MetaDataID>
        public IPricingContext GetDerivedPriceContext(IPricingContext pricingContext)
        {
            if (pricingContext is IMenuItemPrice)
                return new PriceListMenuItemPrice(pricingContext as IMenuItemPrice, this);

            return this;
        }

        /// <MetaDataID>{db66132e-139c-4596-b670-71d301e47ddb}</MetaDataID>
        public void RemoveCustomizedPrice(ICustomizedPrice customizedPrice)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{07a06f93-ff34-4edf-888b-28ce44a64127}</MetaDataID>
        public ICustomizedPrice GetCustomizedPrice(IPricedSubject pricedSubject)
        {

            decimal? price = pricedSubject?.Price;
            if (price == null || price.Value == 0)
                return null;

            double? percentageDiscount = GetPercentageDiscount(pricedSubject);
            double? amountDiscount = GetAmountDiscount(pricedSubject);
            decimal? overridePrice = GetOverridePrice(pricedSubject);

            if (percentageDiscount != null && amountDiscount != null)
            {

            }
            if (percentageDiscount != null && overridePrice != null)
            {

            }
            if (amountDiscount != null && overridePrice != null)
            {

            }



            if (this.GetItemPriceInfo(pricedSubject)?.IsExcluded() == true)
                return new PriceListPrice(pricedSubject, this, price.Value);
            if (percentageDiscount != null)
            {

                var priceRounding = GetPriceRounding(pricedSubject);


                price = price.Value * (decimal)(1 - percentageDiscount.Value);
                if (priceRounding.HasValue)
                    price = this.RoundPriceToNearest(price.Value, priceRounding.Value);
                if (price != null)
                    return new PriceListPrice(pricedSubject, this, price.Value);
                else
                    return null;

            }
            if (overridePrice != null)

                return new PriceListPrice(pricedSubject, this, overridePrice.Value);



            if (amountDiscount != null)
                return new PriceListPrice(pricedSubject, this, price.Value - (decimal)amountDiscount);
            return null;
        }


        /// <MetaDataID>{ca2cf21e-d820-4e04-9b6c-2432f0e65ffb}</MetaDataID>
        public decimal GetDefaultPrice(IPricedSubject pricedSubject)
        {
            return pricedSubject.Price;
        }

        class PriceListMenuItemPrice : IPricingContext
        {
            public PriceListMenuItemPrice(IMenuItemPrice menuItemPrice, PriceList priceList)
            {
                MenuItemPrice = menuItemPrice;
                PriceList = priceList;
            }

            public string Name
            {
                get => PriceList.Name;
                set => throw new System.NotImplementedException();
            }

            public IList<ICustomizedPrice> PricedSubjects => throw new System.NotImplementedException();

            IMenuItemPrice MenuItemPrice { get; }
            PriceList PriceList { get; }

            public ICustomizedPrice GetCustomizedPrice(IPricedSubject pricedSubject)
            {


                double? percentageDiscount = PriceList.GetPercentageDiscount(MenuItemPrice);
                double? amountDiscount = PriceList.GetAmountDiscount(MenuItemPrice);
                decimal? overridePrice = PriceList.GetOverridePrice(MenuItemPrice);
                double? optionsPricesRounding = PriceList.GetOptionsPricesRounding(MenuItemPrice);
                var isOptionsPricesDiscountEnabled = PriceList.IsOptionsPricesDiscountEnabled(MenuItemPrice);


                if (percentageDiscount != null && amountDiscount != null)
                {
                }
                if (percentageDiscount != null && overridePrice != null)
                {
                }
                if (amountDiscount != null && overridePrice != null)
                {
                }

                decimal? price = null;
                if (pricedSubject != null)
                {
                    #region Gets option price
                    if (MenuItemPrice != null)
                    {
                        var customizedPrice = MenuItemPrice.GetCustomizedPrice(pricedSubject);
                        if (customizedPrice != null)
                            price = customizedPrice.Price;
                        else
                        {
                            customizedPrice = (MenuItemPrice as MenuModel.MenuItemPrice).ItemSelector?.GetCustomizedPrice(pricedSubject);
                            if (customizedPrice != null)
                                price = customizedPrice.Price;
                            else
                                price = pricedSubject.Price;

                            // customizedPrice = ItemPrice.i.GetCustomizedPrice(Option as IPricedSubject);
                        }

                    }
                    else
                        price = pricedSubject.Price;

                    #endregion


                    if (isOptionsPricesDiscountEnabled && PriceList.GetItemPriceInfo(MenuItemPrice)?.IsExcluded() != true)
                    {
                        // Apply discount
                        price = price.Value * (decimal)(1 - percentageDiscount.Value);
                        if (optionsPricesRounding.HasValue)
                            price = PriceList.RoundPriceToNearest(price.Value, optionsPricesRounding.Value);
                    }

                }
                if (price == null)
                    return null;

                return new PriceListPrice(pricedSubject, this, price.Value);
            }
            public decimal GetDefaultPrice(IPricedSubject pricedSubject)
            {
                decimal price = 0;
                if (MenuItemPrice != null)
                {
                    var customizedPrice = MenuItemPrice.GetCustomizedPrice(pricedSubject);
                    if (customizedPrice != null)
                        price = customizedPrice.Price;
                    else
                    {
                        customizedPrice = (MenuItemPrice as MenuModel.MenuItemPrice).ItemSelector?.GetCustomizedPrice(pricedSubject);
                        if (customizedPrice != null)
                            price = customizedPrice.Price;
                        else
                            price = pricedSubject.Price;
                    }
                }
                else
                    price = pricedSubject.Price;

                return price;
            }

            public void RemoveCustomizedPrice(ICustomizedPrice customizedPrice)
            {

            }


        }

        class PriceListPrice : ICustomizedPrice
        {

            public PriceListPrice(IPricedSubject pricedSubject, IPricingContext pricingContext, decimal price)
            {
                _Price = price;
                PricedSubject = pricedSubject;
                PricingContext = pricingContext;
            }
            public IPricedSubject PricedSubject { get; }

            public IPricingContext PricingContext { get; }

            /// <exclude>Excluded</exclude>
            decimal _Price;
            public decimal Price { get { return _Price; } set { } }
        }
    }



}