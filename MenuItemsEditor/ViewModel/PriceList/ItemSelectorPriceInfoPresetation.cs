using FlavourBusinessFacade.PriceList;
using FLBManager.ViewModel;
using MenuModel;

using OOAdvantech.MetaDataRepository;
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
    public class ItemSelectorPriceInfoPresetation : FBResourceTreeNode, INotifyPropertyChanged
    {
        public ItemSelectorPriceInfoPresetation(FBResourceTreeNode parent, PriceListPresentation priceListPresentation, IMenuItem menuItem, MenuItemPrice itemPrice, bool editMode) : base(parent)
        {

            ItemPriceInfoPresentation = parent as ItemsPriceInfoPresentation;
            ItemPrice = itemPrice;
            PriceListPresentation = priceListPresentation;



            var optionsGroupsPriceInfos = (from menuItemType in menuItem.Types
                                           from optionGroup in menuItemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                                           where optionGroup.HasOptionWithPrice(menuItem)
                                           select new ItemOptionsPriceInfo(this, menuItem, optionGroup, itemPrice as IPricingContext)).OfType<FBResourceTreeNode>().ToList();

            var unGroupedScaledOptionsPriceInfos = (from menuItemType in menuItem.Types
                                                    from option in menuItemType.Options.OfType<MenuModel.IPreparationScaledOption>()
                                                    select new ItemOptionsPriceInfo(this, menuItem, option, itemPrice as IPricingContext)).OfType<FBResourceTreeNode>().ToList();



            optionsGroupsPriceInfos.AddRange(unGroupedScaledOptionsPriceInfos);

            Members = optionsGroupsPriceInfos;

            EditMode = editMode;
            RemoveItemsPreparationInfoCommand = new RelayCommand((object sender) =>
            {
                RemoveItemsPreparationInfo();
            });

        }

        internal readonly ItemsPriceInfoPresentation ItemPriceInfoPresentation;

        public MenuItemPrice ItemPrice { get; }
        public PriceListPresentation PriceListPresentation { get; }


        public void Refresh()
        {


            foreach (var member in Members.OfType<ItemOptionsPriceInfo>())
            {
                member.Refresh();
            }
            ObjectPropertiesChanged();
        }

        private void ObjectPropertiesChanged()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Price)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(OverridePrice)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceText)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceHasChanged)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideText)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasDedicatedItemPriceInfo)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));

        }

        /// <summary>
        /// Command to removes the itemsPriceInfo from price list, the price information comes from the previous object in the hierarchy
        /// </summary>

        public RelayCommand RemoveItemsPreparationInfoCommand { get; protected set; }

        /// <summary>
        /// Removes the itemsPriceInfo from price list, the price information comes from the previous object in the hierarchy
        /// </summary>
        private void RemoveItemsPreparationInfo()
        {
            if (EditMode)
            {
                if (ItemPrice != null)
                    PriceListPresentation.ClearItemsPriceInfo(ItemPrice);
                _OverridePrice = null;
                Refresh();
            }
        }


        public override string Name { get => ItemPrice.ItemSelector.Name; set { } }

        public override ImageSource TreeImage => null;

        public override List<FBResourceTreeNode> Members { get; }



        /// <summary>
        /// When EditMode is true you can include item in pricelist, exclude or edit item pricelist info.
        /// </summary>
        /// <MetaDataID>{27d5229c-bc53-42bd-9d47-6ceea9487d1f}</MetaDataID>
        readonly bool EditMode;

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;

        /// <summary>
        /// Defines the context menu for item selector tree view node
        /// </summary>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                HeaderPriceInfoNode?.ClearSelected();
                IsSelected = true;
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png")); //blank icon image source
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    MenuCommand menuItem = new MenuCommand();
                    if (EditMode)
                    {
                        menuItem = new MenuCommand();
                        imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                        menuItem.Header = Properties.Resources.ClearItemsPriceInfoMenuHeader;
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = RemoveItemsPreparationInfoCommand;
                        _ContextMenuItems.Add(menuItem);

                    }
                }
                return _ContextMenuItems;
            }
        }


        FBResourceTreeNode HeaderPriceInfoNode
        {
            get
            {
                FBResourceTreeNode treeNode = this;
                while (treeNode.Parent is ItemsPriceInfoPresentation || treeNode.Parent is ItemSelectorPriceInfoPresetation)
                {
                    treeNode = treeNode.Parent;
                }
                return treeNode;
            }
        }
        public override List<MenuCommand> SelectedItemContextMenuItems => ContextMenuItems;




        public ImageSource PriceOverrideTypeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
            }
        }

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        public override void SelectionChange()
        {
            throw new NotImplementedException();
        }


        public Visibility PercentageDiscountVisibility
        {
            get
            {
                if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

        }

        public Visibility AmountDiscountVisibility
        {
            get
            {
                if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility PriceOverrideVisibility
        {
            get
            {
                if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        PriceOverrideTypeViewModel SelectedPriceOverrideType = new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.PriceOverride);


        //public Visibility PriceVisibility
        //{
        //    get
        //    {
        //        if (!Edit)
        //            return Visibility.Visible;

        //        if (MenuItem != null && SelectedPriceOverrideType.PriceOverrideType != PriceList.PriceOverrideTypes.PriceOverride)
        //            return Visibility.Visible;
        //        else
        //            return Visibility.Hidden;


        //    }
        //    set { }
        //}




        public Visibility PriceVisibility
        {
            get
            {

                if (!Edit)
                    return Visibility.Visible;
                return Visibility.Hidden;
            }
            set
            {
            }
        }

        public override bool HasContextMenu => true;

        public override bool Edit
        {
            get => base.Edit;
            set
            {
                base.Edit = value;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceVisibility)));
                ObjectPropertiesChanged();

            }
        }

        public string PriceText
        {
            get
            {
                decimal? price = GetFinalPrice();



                //decimal? price = ItemPrice?.Price;
                //if (ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
                //{
                //    price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
                //    var pricerounding = ItemPriceInfoPresentation.Pricerounding;
                //    if (pricerounding.HasValue)
                //        price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
                //}

                return price?.ToString("C");
            }
            set { }
        }

        private decimal? GetFinalPrice()
        {

            decimal? m_price = this.PriceListPresentation.PriceList.GetFinalPrice(ItemPrice);
            decimal? m__price = (PriceListPresentation.PriceList as IPricingContext).GetCustomizedPrice(ItemPrice)?.Price;
            if (!m__price.HasValue)
                m__price = ItemPrice.Price;


            decimal? price = ItemPrice?.Price;
            if (PriceListPresentation.GetItemsPriceInfo(ItemPrice) != null)
            {
                if (OverridePrice != -1)
                    price = OverridePrice;
            }
            else if (ItemPriceInfoPresentation.SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount && ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
            {
                price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
                var pricerounding = ItemPriceInfoPresentation.Pricerounding;
                if (pricerounding.HasValue)
                    price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
            }
            else if (price != null && ItemPriceInfoPresentation.SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount && ItemPriceInfoPresentation.DefinesNewPrice && ItemPriceInfoPresentation.AmountDiscount != null)
                price = price.Value - (decimal)ItemPriceInfoPresentation.AmountDiscount.Value;



            if (price != m_price)
            {

            }
            else if(price != m__price)
            {

            }
            else
            {

            }


            if (OrgPrice >= 0 && m_price < 0)
                m_price = 0;
            return m__price;
        }

        /// <summary>
        /// Tells the user what the original price was
        /// </summary>
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

        /// <summary>
        /// Defines the original price
        /// </summary>

        decimal? OrgPrice
        {
            get
            {
                return ItemPrice?.Price;
            }
        }


        /// <summary>
        /// Defines the price which produced from dedicated itemsPriceInfo
        /// </summary>
        public decimal? Price
        {
            get
            {

                decimal? price = ItemPrice?.Price;
                if (PriceListPresentation.GetItemsPriceInfo(ItemPrice) != null && OverridePrice != -1)
                    return OverridePrice;

                //else if (ItemPriceInfoPresentation.SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount && ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
                //{
                //    price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
                //    var pricerounding = ItemPriceInfoPresentation.Pricerounding;
                //    if (pricerounding.HasValue)
                //        price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
                //}
                //else if (price != null && ItemPriceInfoPresentation.SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount && ItemPriceInfoPresentation.DefinesNewPrice && ItemPriceInfoPresentation.AmountDiscount != null)
                //    price = price.Value - (decimal)ItemPriceInfoPresentation.AmountDiscount.Value;



                //if (ItemPriceInfoPresentation.DefinesNewPrice && price != null && ItemPriceInfoPresentation.PercentageDiscount > 0)
                //{
                //    price = price.Value * (decimal)(1 - ItemPriceInfoPresentation.PercentageDiscount.Value / 100);
                //    var pricerounding = ItemPriceInfoPresentation.Pricerounding;
                //    if (pricerounding.HasValue)
                //        price = ItemPriceInfoPresentation.PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
                //}

                return price;
            }
            set
            {
                OverridePrice = value;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(OverridePrice)));
            }
        }


        public bool HasDedicatedItemPriceInfo
        {
            get
            {

                if (ItemPrice != null && PriceListPresentation.GetItemsPriceInfo(ItemPrice)?.ItemsPriceInfoType == ItemsPriceInfoType.Include)
                    return true;

                return false;
            }
        }



        /// <exclude>Excluded</exclude>
        decimal? _OverridePrice;

        public decimal? OverridePrice
        {
            get
            {
                decimal? OverridenPrice = -1;

                OverridenPrice = this.PriceListPresentation.GetOverridenPrice(ItemPrice);

                if (OverridenPrice.HasValue)
                    _OverridePrice = OverridenPrice.Value;
                if (_OverridePrice == null)
                    return -1;
                return _OverridePrice;
            }
            set
            {
                if (value.HasValue)
                {
                    if (value != -1)
                        _OverridePrice = value.Value;
                    else
                        _OverridePrice = null;
                }
                else
                    _OverridePrice = null;


                PriceListPresentation.SetOverridenPrice(ItemPrice, _OverridePrice);

                ObjectPropertiesChanged();
                ItemPriceInfoPresentation?.MenuItemPriceHasChanged();

            }
        }

        public double? PercentageDiscount
        {
            get
            {
                double? percentageDiscount = 0;
                percentageDiscount = PriceListPresentation.GetPercentageDiscount(ItemPrice);
                if (percentageDiscount == null)
                    return 0;
                return 100 * percentageDiscount;
            }
            set
            {

            }
        }

        public bool IsDefinesNewPriceEnabled { get; } = true;
        public bool DefinesNewPrice
        {
            get
            {
                return PriceListPresentation.PriceList.HasOverriddenPrice(ItemPrice);
            }
            set
            {
                if (value)
                    PriceListPresentation.IncludeItemSelector(ItemPrice);
                if (!value)
                {
                    this.PriceListPresentation.ExcludeItemSelector(ItemPrice);
                    OverridePrice = -1;
                }
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
                ItemPriceInfoPresentation.Refresh();
            }
        }


        public bool PriceHasChanged
        {
            get
            {
                decimal? price = ItemPrice?.Price;
                return ItemPriceInfoPresentation.DefinesNewPrice && price != GetFinalPrice();
            }
        }

        public string PriceOverrideText
        {
            get
            {
                if (PriceListPresentation.GetItemsPriceInfo(ItemPrice) != null)
                    return "";
                if (ItemPriceInfoPresentation.DefinesNewPrice && ItemPriceInfoPresentation.SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount)
                    return $"{ItemPriceInfoPresentation.PercentageDiscount}%";
                return "";
            }
        }
    }
}
