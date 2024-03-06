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
    public class ItemSelectorPriceInfo : FBResourceTreeNode, INotifyPropertyChanged
    {
        public ItemSelectorPriceInfo(FBResourceTreeNode parent, IMenuItem menuItem, MenuItemPrice itemPrice) : base(parent)
        {
            ItemPriceInfoPresentation = parent as ItemsPriceInfoPresentation;
            ItemPrice =itemPrice;

            var itemOptionsPriceInfos = (from menuItemType in menuItem.Types
                                         from optionGroup in menuItemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                                         where optionGroup.HasOptionWithPrice(menuItem)
                                         select new ItemOptionsPriceInfo(this, menuItem, optionGroup,itemPrice as IPricingContext)).OfType<FBResourceTreeNode>().ToList();

            var unGroupedScaledOptions = (from menuItemType in menuItem.Types
                                          from option in menuItemType.Options.OfType<MenuModel.IPreparationScaledOption>()
                                          select new ItemOptionsPriceInfo(this, menuItem, option, itemPrice as IPricingContext)).OfType<FBResourceTreeNode>().ToList();

            

            itemOptionsPriceInfos.AddRange(unGroupedScaledOptions);

            Members=itemOptionsPriceInfos;

        }
        public void Refresh()
        {
            foreach(var member in Members.OfType<ItemOptionsPriceInfo>())
            {
                member.Refresh();
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceText)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceHasChanged)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideText)));
        }
        public override string Name { get => ItemPrice.ItemSelector.Name; set { } }

        public override ImageSource TreeImage => null;

        public override List<FBResourceTreeNode> Members { get; } 

        public override List<MenuCommand> ContextMenuItems => new List<MenuCommand>();

        public override List<MenuCommand> SelectedItemContextMenuItems => ContextMenuItems;

        private readonly ItemsPriceInfoPresentation ItemPriceInfoPresentation;

        public MenuItemPrice ItemPrice { get; }

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
                
                decimal? price = ItemPrice?.Price;
                if (ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
                {
                    price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
                    var pricerounding = ItemPriceInfoPresentation.Pricerounding;
                    if (pricerounding.HasValue)
                        price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
                }

                return price?.ToString("C");
            }
            set { }
        }

        public decimal? Price
        {
            get
            {

                decimal? price = ItemPrice?.Price;
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
                decimal? price = ItemPrice?.Price;
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
