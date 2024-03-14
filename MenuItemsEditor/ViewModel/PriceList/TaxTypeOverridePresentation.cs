using FlavourBusinessFacade.ServicesContextResources;


using MenuModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FLBManager.ViewModel;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using UIBaseEx;
using WPFUIElementObjectBind;
using FlavourBusinessFacade.PriceList;
using System.Windows;
using System.Runtime.Remoting.Messaging;
using FlavourBusinessManager.ServicesContextResources;



namespace MenuItemsEditor.ViewModel.PriceList
{



    public class ItemsPriceInfoPresentation : FBResourceTreeNode, INotifyPropertyChanged
    {

        public override void RemoveChild(FLBManager.ViewModel.FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        /// <MetaDataID>{017f504b-513b-4fc4-af42-c0c2ae4641b9}</MetaDataID>
        public readonly IItemsPreparationInfo ItemsPreparationInfo;
        /// <MetaDataID>{b369fbe0-d83b-467f-ba71-050afe49452e}</MetaDataID>
        public PriceListPresentation PriceListPresentation { get; }

        /// <summary>
        /// Preparation infos concern menu item
        /// </summary>
        /// <MetaDataID>{664db8b4-7bf9-46f2-b0e8-a18af5e23430}</MetaDataID>
        // <MetaDataID>{4e50a2ba-e4a7-498e-a316-aa888e4692bd}</MetaDataID>
        IMenuItem MenuItem;

        /// <summary>
        ///  Preparation infos concern menu items of category
        /// </summary>
        /// <MetaDataID>{b341b6ed-cd19-4c47-b3a2-9f175b91d05b}</MetaDataID>
        // <MetaDataID>{a777298f-cf21-4ccb-848d-38596df908f3}</MetaDataID>
        internal IItemsCategory ItemsCategory;




        /// <summary>
        /// When EditMode is true you can include item in preparation station, exclude or edit item preparation info.
        /// </summary>
        /// <MetaDataID>{27d5229c-bc53-42bd-9d47-6ceea9487d1f}</MetaDataID>
        readonly bool EditMode;


        /// <MetaDataID>{636b39c3-93bd-4987-988e-ebb01317fc4d}</MetaDataID>
        public readonly bool IsRootCategory;

        /// <summary>
        /// Defines constructor for root items category
        /// </summary>
        /// <param name="preparationStationPresentation">
        /// Defines the preparation station presentation of itemsPreparationInfo
        /// </param>
        /// <param name="menu">
        /// Defines the items menu
        /// </param>
        /// <param name="editMode"></param>
        /// <MetaDataID>{36657d09-72be-40de-bc82-5213ba950d4b}</MetaDataID>
        // <MetaDataID>{6d5912d3-dbd6-431c-b216-d6a4732da003}</MetaDataID>
        public ItemsPriceInfoPresentation(PriceListPresentation priceListPresentation, IMenu menu, bool editMode) : base(priceListPresentation)
        {
            PriceOverrideTypes = new List<PriceOverrideTypeViewModel>() {
            new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.PercentageDiscount),
            new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.AmountDiscount)};

            IsRootCategory = true;
            PriceListPresentation = priceListPresentation;

            SelectedPriceOverrideType = PriceOverrideTypes[0];
            EditMode = editMode;
            ItemsPreparationInfo = new FlavourBusinessManager.ServicesContextResources.ItemsPreparationInfo(menu.RootCategory);

            ItemsCategory = menu.RootCategory;




            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            RemoveItemsPreparationInfoCommand = new RelayCommand((object sender) =>
            {
                RemoveItemsPreparationInfo();
            });

            ToggleDiscoundTypeCommand = new RelayCommand((object sender) =>
            {
                ToggleDiscoundType();
            });



            //System.Windows.Input.RoutedCommand((object sender) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("");
            //});


        }
        public bool PriceOverrideTypesPopupOpen { get; set; }
        internal void ToggleDiscoundType()
        {
            PriceOverrideTypesPopupOpen = !PriceOverrideTypesPopupOpen;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideTypesPopupOpen)));
        }




        PriceOverrideTypeViewModel _SelectedPriceOverrideType;
        public PriceOverrideTypeViewModel SelectedPriceOverrideType
        {
            get
            {
                if (Edit)
                    return _SelectedPriceOverrideType;
                else
                {
                    double? percentageDiscount = 0;
                    if (this.ItemsCategory != null)
                        percentageDiscount = PriceListPresentation.GetPercentageDiscount(this.ItemsCategory);
                    else if (this.MenuItem != null)
                        percentageDiscount = this.PriceListPresentation.GetPercentageDiscount(this.MenuItem);
                    if (percentageDiscount.HasValue)
                        return PriceOverrideTypes.Where(x => x.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount).FirstOrDefault();

                    double? amountDiscount = 0;
                    if (this.ItemsCategory != null)
                        amountDiscount = PriceListPresentation.GetAmountDiscount(this.ItemsCategory);
                    else if (this.MenuItem != null)
                        amountDiscount = this.PriceListPresentation.GetAmountDiscount(this.MenuItem);

                    if (amountDiscount.HasValue)
                        return PriceOverrideTypes.Where(x => x.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount).FirstOrDefault();



                    decimal? overridenPrice = 0;
                    if (this.ItemsCategory != null)
                        overridenPrice = PriceListPresentation.GetOverridenPrice(this.ItemsCategory);
                    else if (this.MenuItem != null)
                        overridenPrice = this.PriceListPresentation.GetOverridenPrice(this.MenuItem);

                    if (overridenPrice.HasValue)
                        return PriceOverrideTypes.Where(x => x.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride).FirstOrDefault();

                    return _SelectedPriceOverrideType;

                }
            }
            set
            {
                if (_SelectedPriceOverrideType != value)
                {
                    _SelectedPriceOverrideType = value;
                    if (this.ItemsCategory != null)
                        PriceListPresentation.SetOverridenPrice(this.ItemsCategory, null);
                    else if (this.MenuItem != null)
                        this.PriceListPresentation.SetOverridenPrice(this.MenuItem, null);


                    if (this.ItemsCategory != null)
                        PriceListPresentation.SetPercentageDiscount(this.ItemsCategory, null);
                    else if (this.MenuItem != null)
                        this.PriceListPresentation.SetPercentageDiscount(this.MenuItem, null);


                    if (this.ItemsCategory != null)
                        PriceListPresentation.SetAmountDiscount(this.ItemsCategory, 0);
                    else if (this.MenuItem != null)
                        this.PriceListPresentation.SetAmountDiscount(this.MenuItem, 0);

                    //decimal? m_p = null;
                    //if (this.MenuItem != null)
                    //    m_p = this.PriceListPresentation.PriceList.GetFinalPrice(this.MenuItem);



                    switch (_SelectedPriceOverrideType.PriceOverrideType)
                    {
                        case PriceList.PriceOverrideTypes.PriceOverride:
                            {
                                if (_OverridePrice != -1 && HasDedicatedItemPriceInfo)
                                {
                                    if (this.ItemsCategory != null)
                                        PriceListPresentation.SetOverridenPrice(this.ItemsCategory, _OverridePrice);
                                    else if (this.MenuItem != null)
                                        this.PriceListPresentation.SetOverridenPrice(this.MenuItem, _OverridePrice);
                                }
                                break;
                            }
                        case PriceList.PriceOverrideTypes.AmountDiscount:
                            {
                                if (_AmountDiscount > 0 && HasDedicatedItemPriceInfo)
                                {
                                    if (this.ItemsCategory != null)
                                        PriceListPresentation.SetAmountDiscount(this.ItemsCategory, _AmountDiscount);
                                    else if (this.MenuItem != null)
                                        this.PriceListPresentation.SetAmountDiscount(this.MenuItem, _AmountDiscount);
                                }
                                break;
                            }

                        case PriceList.PriceOverrideTypes.PercentageDiscount:
                            {
                                if (_PercentageDiscount > 0 && HasDedicatedItemPriceInfo)
                                {
                                    if (this.ItemsCategory != null)
                                        PriceListPresentation.SetPercentageDiscount(this.ItemsCategory, _PercentageDiscount);
                                    else if (this.MenuItem != null)
                                        this.PriceListPresentation.SetPercentageDiscount(this.MenuItem, _PercentageDiscount);
                                }
                                break;
                            }

                        default:
                            break;
                    }
                }


                //if (_SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride)
                //{
                //     if (this.MenuItem != null)
                //        this.PriceListPresentation.SetOverridenPrice(this.MenuItem, MenuItem?.MenuItemPrice?.Price);
                //}

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedPriceOverrideType)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PercentageDiscountVisibility)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideVisibility)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AmountDiscountVisibility)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceVisibility)));
                Refresh();

            }
        }



        public ImageSource PriceOverrideTypeImage
        {
            get
            {

                if (this.ItemsCategory != null)
                {
                    if (HasDedicatedItemPriceInfo)
                    {
                        if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount && this.DefinesNewPrice && PercentageDiscount > 0)
                            return SelectedPriceOverrideType.Image;
                        if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride && this.DefinesNewPrice && OverridePrice > 0)
                            return SelectedPriceOverrideType.Image;
                        if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount && AmountDiscount != null)
                            return SelectedPriceOverrideType.Image;
                    }
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                }
                else
                {

                    if (HasDedicatedItemPriceInfo)
                    {
                        decimal? price = MenuItem?.MenuItemPrice?.Price;

                        if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount && this.DefinesNewPrice && price != null && PercentageDiscount > 0)
                            return SelectedPriceOverrideType.Image;
                        if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride && this.DefinesNewPrice && price != null && OverridePrice > 0)
                            return SelectedPriceOverrideType.Image;

                        if (price != null && SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount && AmountDiscount != null)
                            return SelectedPriceOverrideType.Image;
                    }
                }

                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
            }


        }


        public List<PriceOverrideTypeViewModel> PriceOverrideTypes { get; }
        //DiscoundTypeViewModel


        /// <MetaDataID>{81bc990f-0fb8-46d4-9096-885f628cdae6}</MetaDataID>
        public ItemsPriceInfoPresentation(ItemsPriceInfoPresentation parent, PriceListPresentation priceListPresentation, MenuModel.IItemsCategory itemsCategory, bool editMode) : base(parent)
        {
            PriceOverrideTypes = new List<PriceOverrideTypeViewModel>() {
            new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.PercentageDiscount),
            new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.AmountDiscount)};
            PriceListPresentation = priceListPresentation;

            SelectedPriceOverrideType = PriceOverrideTypes[0];
            EditMode = editMode;


            ItemsCategory = itemsCategory;



            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            RemoveItemsPreparationInfoCommand = new RelayCommand((object sender) =>
            {
                RemoveItemsPreparationInfo();
            });
            ToggleDiscoundTypeCommand = new RelayCommand((object sender) =>
            {
                ToggleDiscoundType();
            });

            CheckBoxVisibility = System.Windows.Visibility.Visible;
        }

        private void RemoveItemsPreparationInfo()
        {
            if (EditMode)
            {
                if (IsRootCategory)
                {
                    PriceListPresentation.ClearItemsPriceInfo();
                    _PercentageDiscount = 0;
                    _Pricerounding = 0;
                }
                else if (ItemsCategory != null)
                    PriceListPresentation.ClearItemsPriceInfo(ItemsCategory);
                else if (MenuItem != null)
                    PriceListPresentation.ClearItemsPriceInfo(MenuItem);
                _OverridePrice = -1;
                UpdatePresentationItems();


                //foreach (var member in Members.OfType<ItemsPriceInfoPresentation>())
                //{
                //    member.Refresh();
                //}

                _PercentageDiscount = 0;
                _AmountDiscount = 0;
                _Pricerounding = null;
                _OptionsPricesRounding = null;
                Refresh();


            }
        }

        /// <MetaDataID>{f76f2725-c4bc-48db-b680-cf20f594f218}</MetaDataID>
        public ItemsPriceInfoPresentation(ItemsPriceInfoPresentation parent, PriceListPresentation priceListPresentation, IMenuItem menuItem, bool editMode) : base(parent)
        {

            PriceOverrideTypes = new List<PriceOverrideTypeViewModel>() {
            new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.PercentageDiscount),
            new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.AmountDiscount),
            new PriceOverrideTypeViewModel(PriceList.PriceOverrideTypes.PriceOverride)};
            PriceListPresentation = priceListPresentation;

            EditMode = editMode;
            SelectedPriceOverrideType = PriceOverrideTypes[0];

            MenuItem = menuItem;



            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            RemoveItemsPreparationInfoCommand = new RelayCommand((object sender) =>
            {
                RemoveItemsPreparationInfo();
            });
            ToggleDiscoundTypeCommand = new RelayCommand((object sender) =>
            {
                ToggleDiscoundType();
            });




        }




        /// <MetaDataID>{2a48dd97-5dfd-4f46-aabb-a776aa182436}</MetaDataID>
        ViewModelWrappers<MenuModel.IItemsCategory, ItemsPriceInfoPresentation> SubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsPriceInfoPresentation>();
        /// <MetaDataID>{760c0787-688d-4974-8c42-a58092d27c33}</MetaDataID>
        ViewModelWrappers<MenuModel.IMenuItem, ItemsPriceInfoPresentation> MenuItems = new ViewModelWrappers<MenuModel.IMenuItem, ItemsPriceInfoPresentation>();



        /// <summary>
        /// Defines when ItemsPreparationInfoPresentation node has items which can prepared from preparation station 
        /// In view mode displayed, only items category node and items which prepared from preparation station
        /// </summary>
        /// <MetaDataID>{2f994810-395c-48aa-8eb6-d44938a06d5c}</MetaDataID>
        // <MetaDataID>{5166e975-649f-47b1-b98e-5f1abf42f3ad}</MetaDataID>
        public bool HasItemsWhichCanPrepared
        {
            get
            {

                if (ItemsCategory != null && PriceListPresentation.PriceList.HasOverriddenPrice(this.ItemsCategory))
                    return true;
                else if (this.MenuItem != null && PriceListPresentation.PriceList.HasOverriddenPrice(this.MenuItem))
                    return true;
                else
                {
                    if (ItemsCategory != null)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select SubCategories.GetViewModelFor(subCategory, this, PriceListPresentation, subCategory, EditMode)).Union(
                             (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                              select MenuItems.GetViewModelFor(menuItem, this, PriceListPresentation, menuItem, EditMode)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();

                        foreach (var member in members.OfType<ItemsPriceInfoPresentation>())
                        {
                            if (member.HasItemsWhichCanPrepared)
                                return true;
                        }
                    }
                    return false;
                }
            }
        }









        //public System.Windows.FontWeight CookingTimeFontWeight
        //{
        //    get
        //    {
        //        if (this.ItemsCategory != null && this.PriceListPresentation.CookingTimeSpanInMinIsDefinedFor(this.ItemsCategory))
        //            return System.Windows.FontWeights.SemiBold;
        //        if (this.MenuItem != null && this.PriceListPresentation.CookingTimeSpanInMinIsDefinedFor(this.MenuItem))
        //            return System.Windows.FontWeights.SemiBold;
        //        return System.Windows.FontWeights.Normal;
        //    }
        //}





        public override bool Edit
        {
            get => base.Edit;

            set
            {
                if (PriceListPresentation?.Taxes == true)
                {
                    base.Edit = false;
                    return;
                }

                base.Edit = value;



                double? percentageDiscount = 0;
                if (this.ItemsCategory != null)
                    percentageDiscount = PriceListPresentation.GetPercentageDiscount(this.ItemsCategory);
                else if (this.MenuItem != null)
                    percentageDiscount = this.PriceListPresentation.GetPercentageDiscount(this.MenuItem);



                double? amountDiscount = 0;
                if (this.ItemsCategory != null)
                    amountDiscount = PriceListPresentation.GetAmountDiscount(this.ItemsCategory);
                else if (this.MenuItem != null)
                    amountDiscount = this.PriceListPresentation.GetAmountDiscount(this.MenuItem);





                decimal? overridenPrice = 0;
                if (this.ItemsCategory != null)
                    overridenPrice = PriceListPresentation.GetOverridenPrice(this.ItemsCategory);
                else if (this.MenuItem != null)
                    overridenPrice = this.PriceListPresentation.GetOverridenPrice(this.MenuItem);


                if (!percentageDiscount.HasValue && !amountDiscount.HasValue && !overridenPrice.HasValue)
                {

                }


                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceVisibility)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceText)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(OverridePrice)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideText)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceHasChanged)));

            }




        }


        public bool IsOptionsPricesDiscountCheckBoxEnabled
        {
            get
            {
                IItemsPriceInfo itemsPriceInfo = null;
                if (IsRootCategory)
                    itemsPriceInfo = PriceListPresentation.PriceList.PriceListMainItemsPriceInfo;
                else if (this.ItemsCategory != null)
                    itemsPriceInfo = PriceListPresentation.GetItemsPriceInfo(this.ItemsCategory);
                else if (this.MenuItem != null)
                    itemsPriceInfo = PriceListPresentation.GetItemsPriceInfo(this.MenuItem);

                if (itemsPriceInfo?.PercentageDiscount != null && itemsPriceInfo.Included())
                    return true;
                else
                    return false;
            }
        }

        /// <exclude>Excluded</exclude>
        bool _IsOptionsPricesDiscountEnabled;

        public bool IsOptionsPricesDiscountEnabled
        {
            get
            {
                bool? IsOptionsPricesDiscountEnabled = null;
                if (this.ItemsCategory != null)
                    IsOptionsPricesDiscountEnabled = PriceListPresentation.GetsIsOptionsPricesDiscountEnabled(this.ItemsCategory);
                else if (this.MenuItem != null)
                    IsOptionsPricesDiscountEnabled = PriceListPresentation.GetIsOptionsPricesDiscountEnabled(this.MenuItem);

                if (IsRootCategory)
                    IsOptionsPricesDiscountEnabled = PriceListPresentation.PriceList.PriceListMainItemsPriceInfo.IsOptionsPricesDiscountEnabled == true;


                if (IsOptionsPricesDiscountEnabled.HasValue)
                    _IsOptionsPricesDiscountEnabled = IsOptionsPricesDiscountEnabled == true;

                return _IsOptionsPricesDiscountEnabled;
            }
            set
            {
                _IsOptionsPricesDiscountEnabled = value;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(ToppingsDiscountRoundingVisible)));

                if (IsRootCategory)
                {
                    PriceListPresentation.SetsOptionsPricesDiscountEnabled(value);
                    Refresh();
                    return;
                }
                if (this.ItemsCategory != null)
                    PriceListPresentation.SetsOptionsPricesDiscountEnabled(this.ItemsCategory, value);
                else if (this.MenuItem != null)
                    this.PriceListPresentation.SetsOptionsPricesDiscountEnabled(this.MenuItem, value);

                var isOptionsPricesDiscountEnabled = IsOptionsPricesDiscountEnabled;
                Refresh();

            }
        }

        public Visibility ToppingsDiscountRoundingVisible
        {
            get
            {
                if (_IsOptionsPricesDiscountEnabled)
                    return Visibility.Visible;
                else
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

        public string PriceOverrideTypeToolTip
        {
            get
            {
                if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount)
                    return Properties.Resources.PriceListDiscountTooltip;
                else
                    return null;
            }
        }


        public bool HasDedicatedItemPriceInfo
        {
            get
            {
                if (PriceListPresentation.Taxes)
                    return false;

                if (IsRootCategory)
                    return PriceListPresentation.PriceList.PriceListMainItemsPriceInfo.PercentageDiscount != null || PriceListPresentation.PriceList.PriceListMainItemsPriceInfo.AmountDiscount != null;

                if (ItemsCategory != null && PriceListPresentation.GetItemsPriceInfo(this.ItemsCategory)?.ItemsPriceInfoType == ItemsPriceInfoType.Include)
                    return true;

                if (MenuItem != null && PriceListPresentation.GetItemsPriceInfo(MenuItem)?.ItemsPriceInfoType == ItemsPriceInfoType.Include)
                    return true;

                return false;
            }
        }


        /// <MetaDataID>{a558dc92-8072-47f3-b669-0d4c1b763396}</MetaDataID>
        public bool PreparationTimeIsVisible
        {
            get
            {
                return DefinesNewPrice;
            }
        }

        public bool IsDefinesNewPriceEnabled { get; } = true;

        public bool DefinesTaxes { get; set; }


        public Visibility IsDefinesTaxesVisibility
        {
            get
            {
                if (PriceListPresentation.Taxes)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        /// <MetaDataID>{c0a45032-38e7-4142-ba1e-47015ff8c0dd}</MetaDataID>
        public bool DefinesNewPrice
        {
            get
            {
                if (this.ItemsCategory != null && this.PriceListPresentation.PriceList.HasOverriddenPrice(this.ItemsCategory))
                    return true;
                else if (this.MenuItem != null && this.PriceListPresentation.PriceList.HasOverriddenPrice(this.MenuItem))
                    return true;
                else if (IsRootCategory)
                    return true;
                else
                    return false;
            }
            set
            {


                if (value && ItemsCategory != null)
                    PriceListPresentation.IncludeItems(ItemsCategory);

                if (!value && ItemsCategory != null)
                    PriceListPresentation.ExcludeItems(ItemsCategory);


                if (value && MenuItem != null)
                    PriceListPresentation.IncludeItem(MenuItem);

                if (!value && MenuItem != null)
                    this.PriceListPresentation.ExcludeItem(MenuItem);


                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));

                UpdatePresentationItems();

                if (value && SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount)
                {
                    if (_PercentageDiscount > 0)
                    {
                        if (this.ItemsCategory != null)
                            PriceListPresentation.SetPercentageDiscount(this.ItemsCategory, _PercentageDiscount);
                        else if (this.MenuItem != null)
                            this.PriceListPresentation.SetPercentageDiscount(this.MenuItem, _PercentageDiscount);
                    }


                    //Pricerounding = _Pricerounding;
                }
                Refresh();


                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CookingTimeSpanInMin)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(TotalPreparationTimeSpanInMin)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsCooked)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasPreparationTimeSpanValue)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Tags)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasTags)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasAppearanceOrderValue)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrder)));
                //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrderText)));




            }
        }



        decimal? _OverridePrice;

        public decimal? OverridePrice
        {
            get
            {
                decimal? OverridenPrice = -1;
                if (this.ItemsCategory != null)
                    OverridenPrice = PriceListPresentation.GetOverridenPrice(this.ItemsCategory);
                else if (this.MenuItem != null)
                    OverridenPrice = this.PriceListPresentation.GetOverridenPrice(this.MenuItem);

                if (OverridenPrice.HasValue)
                    _OverridePrice = OverridenPrice.Value;
                if (_OverridePrice == null)
                    return -1;
                return _OverridePrice;
            }
            set
            {
                if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride)
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

                    if (this.ItemsCategory != null)
                        PriceListPresentation.SetOverridenPrice(this.ItemsCategory, _OverridePrice);
                    else if (this.MenuItem != null)
                        this.PriceListPresentation.SetOverridenPrice(this.MenuItem, _OverridePrice);
                    Refresh();
                }

            }
        }




        double _PercentageDiscount { get; set; } = 0;

        public double? PercentageDiscount
        {
            get
            {
                double? percentageDiscount = 0;
                if (this.ItemsCategory != null)
                    percentageDiscount = PriceListPresentation.GetPercentageDiscount(this.ItemsCategory);
                else if (this.MenuItem != null)
                    percentageDiscount = this.PriceListPresentation.GetPercentageDiscount(this.MenuItem);

                if (IsRootCategory)
                    percentageDiscount = PriceListPresentation.PriceList.PriceListMainItemsPriceInfo.PercentageDiscount;

                if (this.DefinesNewPrice && percentageDiscount.HasValue)
                    return 100 * percentageDiscount.Value;

                if (_PercentageDiscount == null)
                    return 0;

                return 100 * _PercentageDiscount;
            }
            set
            {
                if (value == PercentageDiscount)
                    return;
                if (value.HasValue)
                {

                    _PercentageDiscount = value.Value / 100;
                }

                if (IsRootCategory)
                {
                    PriceListPresentation.SetPercentageDiscount(_PercentageDiscount);
                    Pricerounding = _Pricerounding;
                    Refresh();

                    return;
                }

                if (value.HasValue)
                {
                    if (this.ItemsCategory != null)
                        PriceListPresentation.SetPercentageDiscount(this.ItemsCategory, _PercentageDiscount);
                    else if (this.MenuItem != null)
                        this.PriceListPresentation.SetPercentageDiscount(this.MenuItem, _PercentageDiscount);
                }
                else
                {
                    if (this.ItemsCategory != null)
                        PriceListPresentation.SetPercentageDiscount(this.ItemsCategory, null);
                    else if (this.MenuItem != null)
                        this.PriceListPresentation.SetPercentageDiscount(this.MenuItem, null);
                }

                Refresh();

            }
        }

        double? _Pricerounding;

        public double? Pricerounding
        {
            get
            {

                double? pricerounding = null;
                if (this.ItemsCategory != null)
                    pricerounding = PriceListPresentation.GetPricerounding(this.ItemsCategory);
                else if (this.MenuItem != null)
                    pricerounding = this.PriceListPresentation.GetPricerounding(this.MenuItem);

                if (IsRootCategory)
                    pricerounding = PriceListPresentation.PriceList.PriceListMainItemsPriceInfo.PercentageDiscount;


                if (pricerounding.HasValue)
                    _Pricerounding = pricerounding.Value;

                if (_Pricerounding == null)
                    return 0;

                return _Pricerounding;
            }
            set
            {
                if (value == Pricerounding)
                    return;

                if (value.HasValue)
                    _Pricerounding = value.Value;
                if (_Pricerounding == 0)
                    _Pricerounding = null;

                if (IsRootCategory)
                {
                    PriceListPresentation.SetPricerounding(_Pricerounding);
                    Refresh();
                    return;
                }

                if (this.ItemsCategory != null)
                    PriceListPresentation.SetPricerounding(this.ItemsCategory, _Pricerounding);
                else if (this.MenuItem != null)
                    this.PriceListPresentation.SetPricerounding(this.MenuItem, _Pricerounding);


                Refresh();


            }
        }


        double? _OptionsPricesRounding;

        public double? OptionsPricesRounding
        {
            get
            {

                double? optionsPricesRounding = null;

                if (this.ItemsCategory != null)
                    optionsPricesRounding = PriceListPresentation.GetOptionsPricesRounding(this.ItemsCategory);
                else if (this.MenuItem != null)
                    optionsPricesRounding = this.PriceListPresentation.GetOptionsPricesRounding(this.MenuItem);
                _OptionsPricesRounding = optionsPricesRounding;

                if (_OptionsPricesRounding == null)
                    return 0;

                return _OptionsPricesRounding;
            }
            set
            {
                if (value == OptionsPricesRounding)
                    return;

                if (value.HasValue)
                    _OptionsPricesRounding = value.Value;
                if (_OptionsPricesRounding == 0)
                    _OptionsPricesRounding = null;

                if (IsRootCategory)
                {
                    PriceListPresentation.SetOptionsPricesRounding(_OptionsPricesRounding);
                    Refresh();
                    return;
                }

                if (this.ItemsCategory != null)
                    PriceListPresentation.SetOptionsPricesRounding(this.ItemsCategory, _OptionsPricesRounding);
                else if (this.MenuItem != null)
                    this.PriceListPresentation.SetOptionsPricesRounding(this.MenuItem, _OptionsPricesRounding);


                Refresh();


            }
        }


        double _AmountDiscount = 0;

        public double? AmountDiscount
        {
            get
            {
                double? amountDiscount = 0;
                if (this.ItemsCategory != null)
                    amountDiscount = PriceListPresentation.GetAmountDiscount(this.ItemsCategory);
                else if (this.MenuItem != null)
                    amountDiscount = this.PriceListPresentation.GetAmountDiscount(this.MenuItem);

                if (amountDiscount.HasValue)
                    _AmountDiscount = amountDiscount.Value;

                if (_AmountDiscount == null)
                    return 0;
                return _AmountDiscount;
            }
            set
            {
                if (value == AmountDiscount)
                    return;
                if (value.HasValue)
                    _AmountDiscount = value.Value;
                if (IsRootCategory)
                {
                    PriceListPresentation.SetAmountDiscount(_AmountDiscount);
                    Pricerounding = _Pricerounding;
                    Refresh();
                    return;
                }



                if (this.ItemsCategory != null)
                    PriceListPresentation.SetAmountDiscount(this.ItemsCategory, _AmountDiscount);
                else if (this.MenuItem != null)
                    this.PriceListPresentation.SetAmountDiscount(this.MenuItem, _AmountDiscount);

                Refresh();


            }
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

        public Visibility PriceVisibility
        {
            get
            {
                if (!Edit)
                    return Visibility.Visible;

                if (MenuItem != null && SelectedPriceOverrideType.PriceOverrideType != PriceList.PriceOverrideTypes.PriceOverride)
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;


            }
            set { }
        }

        public string PriceOverrideText
        {
            get
            {
                if (PriceListPresentation.Taxes)
                    return "";

                if (DefinesNewPrice && SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount)
                    if (PercentageDiscount > 0)
                        return $"{PercentageDiscount}%";
                return "";
            }
        }
        public bool PriceHasChanged
        {
            get
            {
                decimal? price = MenuItem?.MenuItemPrice?.Price;
                if (Edit)
                    return DefinesNewPrice && price != Price;
                else
                    return DefinesNewPrice && price != GetFinalPrice();
            }
        }

        decimal? OrgPrice
        {
            get
            {
                return MenuItem?.MenuItemPrice?.Price;
            }
        }



        public decimal? Price
        {
            get
            {
                decimal? price = MenuItem?.MenuItemPrice?.Price;

                if (HasDedicatedItemPriceInfo)
                {
                    if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount && price != null && PercentageDiscount > 0)
                        price = price.Value * (decimal)(1 - PercentageDiscount.Value / 100);
                    if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride && _OverridePrice != null && _OverridePrice != -1)
                        return (decimal)_OverridePrice;
                    if (price != null && SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount && AmountDiscount != null)
                        price = price.Value - (decimal)AmountDiscount.Value;


                    var itemSelectorPriceInfoPresetation = Members.OfType<ItemSelectorPriceInfoPresentation>().Where(x => x.ItemPrice == MenuItem?.MenuItemPrice).FirstOrDefault();
                    if (itemSelectorPriceInfoPresetation?.HasDedicatedItemPriceInfo == true)
                        price = itemSelectorPriceInfoPresetation?.Price;
                }

                return price;
            }
            set
            {
                OverridePrice = value;

            }
        }

        /// <summary>
        /// Defines the price when presentation object isn't in edit mode
        /// </summary>
        public string PriceText
        {
            get
            {
                decimal? price = GetFinalPrice();
                if (Edit)
                    price = Price;

                return price?.ToString("C");
            }
            set { }
        }

        private decimal? GetFinalPrice()
        {

            decimal? m__price = (PriceListPresentation.PriceList as IPricingContext).GetCustomizedPrice(MenuItem?.MenuItemPrice)?.Price;
            //decimal? m_price = this.PriceListPresentation.PriceList.GetFinalPrice(MenuItem);
            //if (!m__price.HasValue)
            //    m__price = MenuItem?.MenuItemPrice.Price;

            //decimal? price = MenuItem?.MenuItemPrice?.Price;
            //if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PercentageDiscount && this.DefinesNewPrice && price != null && PercentageDiscount > 0)
            //{
            //    price = price.Value * (decimal)(1 - PercentageDiscount.Value / 100);
            //    var pricerounding = Pricerounding;
            //    if (pricerounding.HasValue)
            //        price = PriceListPresentation.PriceList.RoundPriceToNearest(price.Value, pricerounding.Value);
            //}
            //if (SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.PriceOverride && this.DefinesNewPrice && price != null && OverridePrice > 0)
            //    price = OverridePrice;

            //if (price != null && SelectedPriceOverrideType.PriceOverrideType == PriceList.PriceOverrideTypes.AmountDiscount && AmountDiscount != null)
            //    price = price.Value - (decimal)AmountDiscount.Value;

            //var itemSelectorPriceInfoPresetation = Members.OfType<ItemSelectorPriceInfoPresentation>().Where(x => x.ItemPrice == MenuItem?.MenuItemPrice).FirstOrDefault();
            //if (itemSelectorPriceInfoPresetation?.HasDedicatedItemPriceInfo == true)
            //    price = itemSelectorPriceInfoPresetation?.Price;


            //if (price != m_price)
            //{

            //}
            //else if (price != m__price)
            //{

            //}
            //else
            //{

            //}
            if (OrgPrice < 0)
            {

            }
            if (OrgPrice >= 0 && m__price < 0)
                m__price = 0;
            return m__price;
        }

        /// <summary>
        /// Some times a change in items preparation info affect nodes in preparation station infos hierarchy
        /// this method update the  hierarchy if needed
        /// </summary>
        /// <MetaDataID>{eabde86b-dbcc-439f-9118-3958c36c0e92}</MetaDataID>
        private void UpdatePresentationItems()
        {

            if (ItemsCategory != null)
            {
                Root.Refresh(ItemsCategory as ItemsCategory);
                Root.PriceListPresentation.Refresh(ItemsCategory as ItemsCategory);
            }


            if (MenuItem != null)
            {
                Root.Refresh(MenuItem as MenuItem);
                Root.PriceListPresentation.Refresh(MenuItem as MenuItem);
            }


            //Refresh();
        }
        /// <MetaDataID>{7e6b7299-dd58-402d-9094-9c51b82cb177}</MetaDataID>
        ItemsPriceInfoPresentation Root
        {
            get
            {
                ItemsPriceInfoPresentation mainPreparationStationPresentation = null;
                FBResourceTreeNode parent = this;
                if (parent is ItemsPriceInfoPresentation)
                    mainPreparationStationPresentation = parent as ItemsPriceInfoPresentation;
                while (parent.Parent != null)
                {
                    parent = parent.Parent;
                    if (parent is ItemsPriceInfoPresentation)
                        mainPreparationStationPresentation = parent as ItemsPriceInfoPresentation;
                }

                return mainPreparationStationPresentation;
            }
        }
        /// <MetaDataID>{fad3761a-9049-4cf1-8c5e-ffe25141ee69}</MetaDataID>
        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }



        /// <MetaDataID>{469e6645-b25e-49b6-8eaa-e3ce412bbf93}</MetaDataID>
        private void Delete()
        {
            if (ItemsCategory != null)
                PriceListPresentation.ExcludeItems(ItemsCategory);
            if (this.MenuItem != null)
                PriceListPresentation.ExcludeItem(MenuItem);
        }


        /// <MetaDataID>{99fd2858-5fca-4b23-8d02-7eca8f7106c5}</MetaDataID>
        public override string Name
        {
            get
            {
                if (ItemsCategory != null)
                    return ItemsCategory.Name;
                else if (this.MenuItem != null)
                    return MenuItem.Prices.OfType<MenuItemPrice>().Where(x => x.ItemSelector != null).Count() > 0 ? $"{MenuItem.Name}  {MenuItem.MenuItemPrice.Name}" : MenuItem.Name;
                else
                    return "";
            }
            set
            {
            }
        }








        /// <MetaDataID>{5b29545c-fe12-4953-ae37-58f532044a8c}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                if (ItemsCategory != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category16.png"));
                else if (this.MenuItem != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem16.png"));
                else
                    return null;
            }
        }



        /// <exclude>Excluded</exclude>
        List<FBResourceTreeNode> _Members;
        /// <MetaDataID>{d2401e62-1e30-4557-9b75-142a3f9c4690}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {

                if (_Members != null)
                    return _Members;



                List<FBResourceTreeNode> members = new List<FBResourceTreeNode>();

                if (ItemsCategory != null)
                {




                    if (EditMode)
                    {
                        var itemsPriceInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                            select SubCategories.GetViewModelFor(subCategory, this, PriceListPresentation, subCategory, EditMode)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, PriceListPresentation, menuItem, EditMode)));

                        members.AddRange((from itemsPriceInfoPresentation in itemsPriceInfosPresentations
                                          select itemsPriceInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;
                    }
                    else
                    {
                        var itemsPriceInfosPresentations = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                            select SubCategories.GetViewModelFor(subCategory, this, PriceListPresentation, subCategory, EditMode)).Union(
                           (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, PriceListPresentation, menuItem, EditMode)));

                        members.AddRange((from itemsPreparationInfoPresentation in itemsPriceInfosPresentations
                                          where itemsPreparationInfoPresentation.HasItemsWhichCanPrepared
                                          select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList());
                        _Members = members;
                        return members;

                    }
                }
                else
                {
                    if (MenuItem != null)
                    {

                        var itemOptionsPriceInfos = (from menuItemType in MenuItem.Types
                                                     from optionGroup in menuItemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                                                     where optionGroup.HasOptionWithPrice(MenuItem)
                                                     select new ItemOptionsPriceInfo(this, MenuItem, optionGroup, null)).OfType<FBResourceTreeNode>().ToList();

                        var unGroupedScaledOptions = (from menuItemType in MenuItem.Types
                                                      from option in menuItemType.Options.OfType<MenuModel.IPreparationScaledOption>()
                                                      select new ItemOptionsPriceInfo(this, MenuItem, option, null)).OfType<FBResourceTreeNode>().ToList();

                        itemOptionsPriceInfos.AddRange(unGroupedScaledOptions);

                        //MenuItem.o
                        _Members = MenuItem.Prices.OfType<MenuItemPrice>().Where(x => x.ItemSelector != null).Select(x => new ItemSelectorPriceInfoPresentation(this, PriceListPresentation, MenuItem, x, EditMode)).OfType<FBResourceTreeNode>().ToList();

                        if (_Members.Count == 0)
                            _Members.AddRange(itemOptionsPriceInfos);

                        return _Members;
                    }

                    return members;
                }
            }
        }


        /// <MetaDataID>{ff9d9100-88c5-453d-ae84-6583239e8501}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand RemoveItemsPreparationInfoCommand { get; protected set; }

        public RelayCommand ToggleDiscoundTypeCommand { get; protected set; }


        /// <MetaDataID>{261115fe-509b-4c99-bb24-709235bb932f}</MetaDataID>



        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;

        /// <MetaDataID>{83dcd867-12dc-4c2f-97d7-2a9d26a10471}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                HeaderPriceInfoNode?.ClearSelected();
                IsSelected = true;
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
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

                    if (this.Parent is PriceListPresentation)
                    {
                        _ContextMenuItems.Add(null);

                        menuItem = new MenuCommand();
                        imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                        menuItem.Header = Properties.Resources.DeleteMenuItemHeader;
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = DeleteCommand;

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
                while (treeNode.Parent is ItemsPriceInfoPresentation)
                {
                    treeNode = treeNode.Parent;
                }
                return treeNode;
            }
        }
        /// <MetaDataID>{64de1643-b464-40c2-b4cf-ab00dea48227}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{1229ec0a-0597-483d-93c5-828187c7cd0e}</MetaDataID>
        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return ContextMenuItems;
                else
                    foreach (var treeNode in Members)
                    {
                        var contextMenuItems = treeNode.SelectedItemContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }

                return null;
            }
        }

        /// <MetaDataID>{abae4496-c9a2-4ad7-a3bf-a508792fa0af}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{cae2c912-49d0-45d1-894b-57dce6c77bdd}</MetaDataID>
        public bool AllInHierarchyDefinesNewPrice
        {
            get
            {
                if (!DefinesNewPrice)
                    return false;
                foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                {
                    if (!itemsPreparationInfoPresentation.DefinesNewPrice)
                        return false;
                }
                return true;
            }
        }



        //AppearanceOrder
        /// <MetaDataID>{68294fd3-55bc-4e63-ac2d-453faacd02d9}</MetaDataID>
        public void Refresh(MenuItem menuItemWithChanges)
        {

            if (ItemsCategory is ItemsCategory && menuItemWithChanges.IsAncestor(ItemsCategory as ItemsCategory))
            {
                ItemsCategory.GetAllMenuItems();
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
                //_Members = null;

                foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                    itemsPreparationInfoPresentation.Refresh(menuItemWithChanges);






                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
            }

            if (MenuItem == menuItemWithChanges)
                Refresh();

        }
        /// <MetaDataID>{919c4b1c-7f8e-40e3-8ad7-fdcab01e6dbb}</MetaDataID>
        public void Refresh(ItemsCategory itemsCategoryWithChanges)
        {

            if (MenuItem is MenuItem)
            {
                if ((MenuItem as MenuItem).IsAncestor(itemsCategoryWithChanges))
                    Refresh();
            }

            if (ItemsCategory is ItemsCategory)
            {
                if ((ItemsCategory as ItemsCategory).IsAncestor(itemsCategoryWithChanges) || ItemsCategory == itemsCategoryWithChanges)
                    Refresh();

                if (itemsCategoryWithChanges.IsAncestor(ItemsCategory as ItemsCategory))
                {
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
                    _Members = null;

                    foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                        itemsPreparationInfoPresentation.Refresh(itemsCategoryWithChanges);

                    foreach (var preparationSubStation in Members.OfType<ItemsPriceInfoPresentation>())
                        preparationSubStation.Refresh(itemsCategoryWithChanges);

                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));
                }

            }
        }

        /// <MetaDataID>{9bbe65bf-458f-4f70-9218-4a3e24f9823d}</MetaDataID>
        public void Refresh()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Price)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(OverridePrice)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefinesNewPrice)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PercentageDiscount)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsOptionsPricesDiscountCheckBoxEnabled)));

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AmountDiscount)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Pricerounding)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(OptionsPricesRounding)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceText)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceHasChanged)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(OrgPriceToolTip)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideText)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasDedicatedItemPriceInfo)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceOverrideTypeImage)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDefinesTaxesVisibility)));




            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsCooked)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(TotalPreparationTimeSpanInMin)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasTags)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Tags)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(HasAppearanceOrderValue)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrder)));
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(AppearanceOrderText)));
            //RunPropertyChang(this, new PropertyChangedEventArgs(nameof(HasPreparationTimeSpanValue)));



            foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPriceInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            foreach (var itemSelectorPriceInfo in Members.OfType<ItemSelectorPriceInfoPresentation>())
                itemSelectorPriceInfo.Refresh();
            //_Members = null;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeIsVisible)));


        }

        internal void MenuItemPriceHasChanged()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Price)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PriceText)));
        }
    }


    public enum PriceOverrideTypes
    {
        PriceOverride,
        AmountDiscount,
        PercentageDiscount
    }

    public class PriceOverrideTypeViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PriceOverrideTypes PriceOverrideType { get; }
        public PriceOverrideTypeViewModel(PriceOverrideTypes priceOverrideType)
        {
            PriceOverrideType = priceOverrideType;
        }

        public string Description
        {
            get
            {
                switch (PriceOverrideType)
                {
                    case PriceOverrideTypes.PriceOverride:
                        return Properties.Resources.PriceOverrideLabel;

                    case PriceOverrideTypes.AmountDiscount:
                        return Properties.Resources.AmountDiscountLabel;

                    case PriceOverrideTypes.PercentageDiscount:
                        return Properties.Resources.PercentageDiscountLabel;
                    default:
                        return Properties.Resources.PriceOverrideLabel;
                }
            }
        }

        public ImageSource Image
        {
            get
            {

                switch (PriceOverrideType)
                {
                    case PriceOverrideTypes.PriceOverride:
                        return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/price24.png"));

                    case PriceOverrideTypes.AmountDiscount:
                        return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/low-price24.png"));

                    case PriceOverrideTypes.PercentageDiscount:
                        return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/percDiscount24.png"));
                    default:
                        return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/price24.png"));
                }

            }
        }
    }

    public class TaxTypeOverridePresentation
    {
        readonly IMenuItem MenuItem;
        readonly PriceListPresentation PriceListPresentation;
        readonly IItemsCategory ItemsCategory;


        public TaxTypeOverridePresentation(PriceListPresentation priceListPresentation, IItemsCategory itemsCategory)
        {
            ItemsCategory = itemsCategory;
            PriceListPresentation = priceListPresentation;
        }

        public TaxTypeOverridePresentation(PriceListPresentation priceListPresentation, IMenuItem menuItem)
        {
            MenuItem = menuItem;
            PriceListPresentation = priceListPresentation;
        }


    }
}



