using FlavourBusinessFacade.PriceList;
using FLBManager.ViewModel;
using MenuModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel.PriceList
{
    /// <MetaDataID>{330d4ee9-3ed3-4fef-ab04-5778b4b571ab}</MetaDataID>
    public class ItemOptionsPriceInfo : FBResourceTreeNode, INotifyPropertyChanged
    {

        public ItemOptionsPriceInfo(FBResourceTreeNode parent, IMenuItem menuItem, IPreparationOptionsGroup optionsGroup, IPricingContext pricingContext) : base(parent)
        {
            //if (parent is ItemsPriceInfoPresentation)
            //    this.ItemPriceInfoPresentation = parent as ItemsPriceInfoPresentation;

            //if (parent is ItemOptionsPriceInfo)
            //    this.ItemPriceInfoPresentation = (parent as ItemOptionsPriceInfo).ItemPriceInfoPresentation;

            if (parent is ItemSelectorPriceInfoPresentation)
            {
                //this.ItemPriceInfoPresentation = (parent as ItemSelectorPriceInfoPresentation).Parent as ItemsPriceInfoPresentation;
                ItemPrice = (parent as ItemSelectorPriceInfoPresentation).ItemPrice;
            }


            //if (parent is ItemOptionsPriceInfo)
            //    this.ItemPriceInfoPresentation = (parent as ItemOptionsPriceInfo).ItemPriceInfoPresentation as ItemsPriceInfoPresentation;

            if (ItemPriceInfoPresentation == null)
            {

            }

            OptionsGroup = optionsGroup;



            Members = optionsGroup.GroupedOptions.Where(x => x.HasOptionWithPrice(menuItem)).Select(x => new ItemOptionsPriceInfo(this, menuItem, x, pricingContext)).OfType<FBResourceTreeNode>().ToList();

            //ItemPriceInfoPresentation = parent as ItemsPriceInfoPresentation;
            //ItemPrice =itemPrice;
        }

        public ItemOptionsPriceInfo(FBResourceTreeNode parent, IMenuItem menuItem, IPreparationScaledOption option, IPricingContext pricingContext) : base(parent)
        {

            Option = option;
            if (parent is ItemOptionsPriceInfo)
                ItemPrice = (parent as ItemOptionsPriceInfo).ItemPrice;

            if (parent is ItemSelectorPriceInfoPresentation)
                ItemPrice = (parent as ItemSelectorPriceInfoPresentation).ItemPrice;


            Members = new List<FBResourceTreeNode>();
        }
        public void Refresh()
        {
            foreach (var member in Members.OfType<ItemOptionsPriceInfo>())
            {
                member.Refresh();
            }

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceText)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceHasChanged)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideText)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(OrgPriceToolTip)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDefinesTaxesVisibility)));

            




        }


        public override string Name
        {
            get
            {
                if (Option != null)
                    return Option.Name;
                if (OptionsGroup != null)
                    return OptionsGroup.Name;

                return "";
            }
            set
            {

            }
        }

        public override ImageSource TreeImage => null;

        public override List<FBResourceTreeNode> Members { get; }


        FBResourceTreeNode HeaderPriceInfoNode
        {
            get
            {
                HeaderPriceInfoNode?.ClearSelected();
                IsSelected = true;

                FBResourceTreeNode treeNode = this;
                while (treeNode.Parent is ItemsPriceInfoPresentation || treeNode.Parent is ItemSelectorPriceInfoPresentation || treeNode.Parent is ItemOptionsPriceInfo)
                {
                    treeNode = treeNode.Parent;
                }
                return treeNode;
            }
        }
        public ImageSource PriceOverrideTypeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
            }
        }


        public override List<MenuCommand> ContextMenuItems => new List<MenuCommand>();

        public override List<MenuCommand> SelectedItemContextMenuItems => ContextMenuItems;

        public ItemsPriceInfoPresentation ItemPriceInfoPresentation
        {
            get
            {

                FBResourceTreeNode treeNode = null;
                treeNode = Parent;
                while (treeNode != null)
                {
                    if (treeNode is ItemsPriceInfoPresentation)
                        return treeNode as ItemsPriceInfoPresentation;

                    if (treeNode == null)
                        break;
                    treeNode = treeNode.Parent;
                }
                return null;

            }
        }

        public PriceListPresentation PriceListPresentation
        {
            get
            {
                return ItemPriceInfoPresentation.PriceListPresentation;
            }
        }

        public ItemSelectorPriceInfoPresentation ItemSelectorPriceInfoPresentation
        {
            get
            {

                FBResourceTreeNode treeNode = null;
                treeNode = Parent;
                while (treeNode != null)
                {
                    if (treeNode is ItemSelectorPriceInfoPresentation)
                        return treeNode as ItemSelectorPriceInfoPresentation;

                    if (treeNode == null)
                        break;
                    treeNode = treeNode.Parent;
                }
                return null;

            }
        }

        public MenuItemPrice ItemPrice { get; }
        public IPreparationOptionsGroup OptionsGroup { get; }


        IPreparationScaledOption Option { get; }


        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        public override void SelectionChange()
        {
            throw new NotImplementedException();
        }

        public Visibility PriceVisibility
        {
            get
            {

                return Visibility.Visible;



            }
            set { }
        }
        public string PriceText
        {
            get
            {

                decimal? price = GetFinalPrice(); //Price;

                return price?.ToString("C");
            }
            set { }
        }




        public decimal? OrgPrice
        {
            get
            {


                decimal? price = null;
                if (Option != null)
                    price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.GetDerivedPriceContext(ItemPrice)?.GetDefaultPrice(Option);
                //{
                //    if (ItemPrice != null)
                //    {
                //        var customizedPrice = ItemPrice.GetCustomizedPrice(Option as IPricedSubject);
                //        if (customizedPrice != null)
                //            price = customizedPrice.Price;
                //        else
                //        {
                //            customizedPrice = ItemPrice.ItemSelector?.GetCustomizedPrice(Option as IPricedSubject);
                //            if (customizedPrice != null)
                //                price = customizedPrice.Price;
                //            else
                //                price = Option.Price;
                //        }
                //    }
                //    else
                //        price = Option.Price;
                //}
                return price;
            }
        }


        public decimal? GetFinalPrice()
        {

            //decimal? m_price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.GetFinalPrice(Option, ItemPrice);

            decimal? m__price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.GetDerivedPriceContext(ItemPrice)?.GetCustomizedPrice(Option)?.Price;
            if (!m__price.HasValue)
                m__price = Option?.Price;
            //decimal? price = OrgPrice;
            //decimal? t_price = ItemPriceInfoPresentation.PriceListPresentation.GetPrice(Option, ItemPrice);


            //if (ItemPriceInfoPresentation.IsOptionsPricesDiscountEnabled && ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
            //{
            //    if (ItemSelectorPriceInfoPresentation != null)
            //    {
            //        if (ItemSelectorPriceInfoPresentation.DefinesNewPrice)
            //        {
            //            price = price.Value * (decimal)(1 - ItemSelectorPriceInfoPresentation.PercentageDiscount.Value / 100);
            //            var pricerounding = ItemPriceInfoPresentation.OptionsPricesRounding;
            //            if (pricerounding.HasValue)
            //                price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
            //        }
            //    }
            //    else
            //    {
            //        price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
            //        var pricerounding = ItemPriceInfoPresentation.OptionsPricesRounding;
            //        if (pricerounding.HasValue)
            //            price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
            //    }
            //}
            //if (price != m_price)
            //{

            //}
            //else if (price != m__price)
            //{

            //}
            //else
            //{

            //}


            return m__price;
        }

        //public decimal? Price
        //{
        //    get
        //    {
        //        decimal? price = OrgPrice;
        //        decimal? t_price = ItemPriceInfoPresentation.PriceListPresentation.GetPrice(Option, ItemPrice);

        //        if (ItemPriceInfoPresentation.IsOptionsPricesDiscountEnabled && ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
        //        {
        //            if (ItemSelectorPriceInfoPresentation != null)
        //            {
        //                if (ItemSelectorPriceInfoPresentation.DefinesNewPrice)
        //                {
        //                    price = price.Value * (decimal)(1 - ItemSelectorPriceInfoPresentation.PercentageDiscount.Value / 100);
        //                    var priceRounding = ItemPriceInfoPresentation.OptionsPricesRounding;
        //                    if (priceRounding.HasValue)
        //                        price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, priceRounding.Value);
        //                }
        //            }
        //            else
        //            {
        //                price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
        //                var priceRounding = ItemPriceInfoPresentation.OptionsPricesRounding;
        //                if (priceRounding.HasValue)
        //                    price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, priceRounding.Value);
        //            }
        //        }


        //        return price;
        //    }
        //    set { }
        //}

        public bool IsDefinesNewPriceEnabled { get; } = false;
        public bool DefinesNewPrice
        {
            get
            {
                if (ItemPriceInfoPresentation.IsOptionsPricesDiscountEnabled)
                {
                    if (ItemSelectorPriceInfoPresentation != null)
                        return ItemSelectorPriceInfoPresentation.DefinesNewPrice;

                    return ItemPriceInfoPresentation.DefinesNewPrice;
                }
                else
                    return false;
            }
            set
            {
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
            }
        }


        public bool PriceHasChanged
        {
            get
            {
                decimal? price = OrgPrice;
                return ItemPriceInfoPresentation.DefinesNewPrice && price != GetFinalPrice();
            }
        }

        public Visibility IsDefinesTaxesVisibility
        {
            get
            {
                return Visibility.Collapsed;
            }
        }


        public string OrgPriceToolTip
        {
            get
            {
                if (PriceHasChanged && OrgPrice.HasValue)
                    return Properties.Resources.OrgPriceLabel + " " + OrgPrice.Value.ToString("C");
                else
                    return null;
            }
        }
        public string PriceOverrideText
        {
            get
            {
               

                if (ItemPriceInfoPresentation.IsOptionsPricesDiscountEnabled && ItemPriceInfoPresentation.DefinesNewPrice && ItemPriceInfoPresentation.SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount)
                {

                    if (ItemSelectorPriceInfoPresentation != null)
                    {
                        if (ItemSelectorPriceInfoPresentation.DefinesNewPrice)
                            return $"{ItemPriceInfoPresentation.PercentageDiscount}%";
                    }
                    else
                        return $"{ItemPriceInfoPresentation.PercentageDiscount}%";

                }
                return "";
            }
        }
    }
}
