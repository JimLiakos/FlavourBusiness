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
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel.PriceList
{
    /// <MetaDataID>{330d4ee9-3ed3-4fef-ab04-5778b4b571ab}</MetaDataID>
    public class ItemOptionsPriceInfo : FBResourceTreeNode, INotifyPropertyChanged
    {

        public ItemOptionsPriceInfo(FBResourceTreeNode parent, IMenuItem menuItem, IPreparationOptionsGroup optionsGroup, IPricingContext pricingContext) : base(parent)
        {
            if (parent is ItemsPriceInfoPresentation)
                this.ItemPriceInfoPresentation=parent as ItemsPriceInfoPresentation;

            if (parent is ItemOptionsPriceInfo)
                this.ItemPriceInfoPresentation=(parent as ItemOptionsPriceInfo).ItemPriceInfoPresentation;

            if (parent is ItemSelectorPriceInfo)
            {
                this.ItemPriceInfoPresentation= (parent as ItemSelectorPriceInfo).Parent as ItemsPriceInfoPresentation;

                ItemPrice=(parent as ItemSelectorPriceInfo).ItemPrice;
            }


            if (parent is ItemOptionsPriceInfo)
                this.ItemPriceInfoPresentation= (parent as ItemOptionsPriceInfo).ItemPriceInfoPresentation as ItemsPriceInfoPresentation;

            if (ItemPriceInfoPresentation==null)
            {

            }

            OptionsGroup=optionsGroup;



            Members= optionsGroup.GroupedOptions.Where(x => x.HasOptionWithPrice(menuItem)).Select(x => new ItemOptionsPriceInfo(this, menuItem, x, pricingContext)).OfType<FBResourceTreeNode>().ToList();

            //ItemPriceInfoPresentation = parent as ItemsPriceInfoPresentation;
            //ItemPrice =itemPrice;
        }

        public ItemOptionsPriceInfo(FBResourceTreeNode parent, IMenuItem menuItem, IPreparationScaledOption option, IPricingContext pricingContext) : base(parent)
        {

            Option=option;

            if (parent is ItemsPriceInfoPresentation)
                this.ItemPriceInfoPresentation=parent as ItemsPriceInfoPresentation;

            if (parent is ItemOptionsPriceInfo)
                this.ItemPriceInfoPresentation=(parent as ItemOptionsPriceInfo).ItemPriceInfoPresentation;



            if (parent is ItemOptionsPriceInfo)
            {
                this.ItemPriceInfoPresentation=(parent as ItemOptionsPriceInfo).ItemPriceInfoPresentation;
                ItemPrice=(parent as ItemOptionsPriceInfo).ItemPrice;
            }

            if (parent is ItemSelectorPriceInfo)
            {
                this.ItemPriceInfoPresentation= (parent as ItemSelectorPriceInfo).Parent as ItemsPriceInfoPresentation;
                ItemPrice=(parent as ItemSelectorPriceInfo).ItemPrice;
            }

            if (ItemPriceInfoPresentation==null)
            {

            }


            Members=new List<FBResourceTreeNode>();
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
             
             
        }


        public override string Name
        {
            get
            {
                if (Option!=null)
                    return Option.Name;
                if (OptionsGroup!=null)
                    return OptionsGroup.Name;

                return "";
            }
            set
            {

            }
        }

        public override ImageSource TreeImage => null;

        public override List<FBResourceTreeNode> Members { get; }

        public override List<MenuCommand> ContextMenuItems => new List<MenuCommand>();

        public override List<MenuCommand> SelectedItemContextMenuItems => ContextMenuItems;

        public readonly ItemsPriceInfoPresentation ItemPriceInfoPresentation;

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

                decimal? price = Price;
                
                return price?.ToString("C");
            }
            set { }
        }




        public decimal? OrgPrice
        {
            get
            {
                decimal? price = null;
                if (Option!=null)
                {
                    if (ItemPrice!=null)
                    {
                        var customizedPrice = ItemPrice.GetCustomazedPrice(Option as IPricedSubject);
                        if (customizedPrice != null)
                            price= customizedPrice.Price;
                        else
                        {
                            customizedPrice=ItemPrice.ItemSelector?.GetCustomazedPrice(Option as IPricedSubject);
                            if (customizedPrice != null)
                                price= customizedPrice.Price;
                            else
                                price= Option.Price;

                            // customizedPrice = ItemPrice.i.GetCustomazedPrice(Option as IPricedSubject);
                        }
                    }
                }




                return price;
            }
        }

        public decimal? Price
        {
            get
            {

                decimal? price = OrgPrice;
                if (ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
                {
                    price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
                    var pricerounding = ItemPriceInfoPresentation.Pricerounding;
                    if (pricerounding.HasValue)
                        price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
                }

       
                return price;
            }
            set { }
        }

        public bool DefinesNewPrice
        {
            get
            {
                return ItemPriceInfoPresentation.DefinesNewPrice;
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
                return ItemPriceInfoPresentation.DefinesNewPrice && price != Price;
                
            }
        }

        public string PriceOverrideText
        {
            get
            {
                if (ItemPriceInfoPresentation.DefinesNewPrice && ItemPriceInfoPresentation.SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount)
                    return $"{ItemPriceInfoPresentation.PercentageDiscount}%";
                return "";
            }
        }
    }
}
