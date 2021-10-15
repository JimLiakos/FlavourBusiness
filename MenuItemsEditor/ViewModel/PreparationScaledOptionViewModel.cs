using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MenuItemsEditor.Properties;
using MenuModel;
using OOAdvantech.Transactions;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{b57c5928-bba1-4226-bf09-4a85cc2ff9b9}</MetaDataID>
    public class PreparationScaledOptionViewModel : PreparationOptionViewModel
    {
        /// <MetaDataID>{4188b432-8bcb-4f6d-b96f-248dd406ba32}</MetaDataID>
        internal readonly MenuModel.IPreparationScaledOption PreparationScaledOption;


        /// <MetaDataID>{f2ba9e51-9245-4b4d-836c-75170a2a1527}</MetaDataID>
        public PreparationScaledOptionViewModel(MenuModel.IPreparationScaledOption preparationOption, IPreparationOptionsListView preparationOptionsListView, bool isEditable)
            : base(preparationOption, preparationOptionsListView, isEditable)
        {


            PreparationScaledOption = preparationOption;

            if (PreparationOptionsListView is PreparationOptionsGroupViewModel)
                (PreparationOptionsListView as PreparationOptionsGroupViewModel).PreparationOptionsGroup.ObjectChangeState += PreparationOptionsGroup_ObjectChangeState;

            EditScaleTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    try
                    {
                        System.Windows.Window win = System.Windows.Window.GetWindow(EditScaleTypeCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                        Views.ScaleTypesWindow window = new Views.ScaleTypesWindow();
                        window.Owner = win;

                        ScaleTypesViewModel scaleTypesViewModel = new ScaleTypesViewModel(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(PreparationScaledOption));
                        window.GetObjectContext().SetContextInstance(scaleTypesViewModel);


                        window.ShowDialog();
                        stateTransition.Consistent = true;
                    }
                    catch (Exception error)
                    {
                    }
                }

                PreparationOptionsListView.PreparationOptionChanged(this);
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Levels)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleTypes)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleType)));

                foreach (var level in Levels)
                    level.Refresh();

            });


            SetSelectedLevelInitialCommand = new RelayCommand((object sender) =>
            {
                MenuModel.IMenuItem menuItem = null;
                if (MenuItemViewModel != null)
                    menuItem = MenuItemViewModel.MenuItem;
                PreparationScaledOption.SetInitialFor(menuItem, SelectedOptionLevel.Level);

                foreach (var level in Levels)
                    level.Refresh();

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UncheckOptionImage)));
                PreparationOptionsListView.PreparationOptionChanged(this);

            }, (object sender) => SelectedOptionLevel != null);


            NewTagCommand = new RelayCommand((object sender) =>
            {
                NewTag();
            });


            PreparationScaledOption.MenuItemType.PreparationOptionAdded += MenuItemType_PreparationOptionAdded;
            PreparationScaledOption.MenuItemType.PreparationOptionRemoved += MenuItemType_PreparationOptionRemoved;

        }

        private void MenuItemType_PreparationOptionRemoved(MenuModel.IMenuItemType menuType, MenuModel.IPreparationOption preparationOption)
        {
            if (preparationOption is MenuModel.ItemSelectorOption)
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PricingContexts)));

        }
        //bool _EditFullName;
        //public bool EditFullName
        //{
        //    get
        //    {
        //        if (_EditFullName != null)
        //            return _EditFullName;

        //    }
        //}


        public bool UntranslatedFullName
        {
            get
            {
                string name = FullName;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return name != FullName;
                }
            }
        }

        public override string Name
        {
            get => base.Name;
            set
            {
                base.Name = value;
                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                    {
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UntranslatedFullName)));
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                    }
                }));
            }
        }


        public bool AutoGenFullName
        {
            get
            {
                return PreparationScaledOption.AutoGenFullName;
            }
            set
            {
                PreparationScaledOption.AutoGenFullName = value;

                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                    {
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UntranslatedFullName)));
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                    }
                }));
            }
        }
        public string FullName
        {
            get
            {
                if (PreparationScaledOption != null)
                    return PreparationScaledOption.FullName;
                else
                    return null;
            }
            set
            {
                PreparationScaledOption.FullName = value;
                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                    {
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                        _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                    }
                }));
            }
        }

        internal void RemoveOptionSpecificFor(IMenuItem menuItem)
        {
            if (PreparationScaledOption != null)
            {
                PreparationScaledOption.RemoveOptionSpecificFor(menuItem);
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.FontWeight)));
            }
        }

        private void MenuItemType_PreparationOptionAdded(MenuModel.IMenuItemType menuType, MenuModel.IPreparationOption preparationOption)
        {
            if (preparationOption is MenuModel.ItemSelectorOption)
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PricingContexts)));
        }

        private void PreparationOptionsGroup_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(MenuModel.IPreparationOptionsGroup.SelectionType))
            {
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleTypes)));
            }
            if (member == nameof(MenuModel.IPreparationOptionsGroup.GroupedOptions))
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UncheckOptionImage)));
        }




        /// <MetaDataID>{61a395ad-699e-4ac5-956a-693bde543fcd}</MetaDataID>
        public override void Maximaze()
        {
            _ViewType = PreparationOptionViewType.ScaledOption;
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewType)));
            PreparationOptionsListView.Maximazed(this);
        }


        /// <exclude>Excluded</exclude>
        event PropertyChangedEventHandler _PropertyChanged;
        public override event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                base.PropertyChanged += value;
                _PropertyChanged += value;
            }
            remove
            {
                base.PropertyChanged -= value;
                _PropertyChanged -= value;
            }
        }


        /// <exclude>Excluded</exclude>
        OptionLevelViewModel _SelectedOptionLevel;
        /// <MetaDataID>{26b3ac34-d14e-4be8-8cfe-5d0987315fef}</MetaDataID>
        public OptionLevelViewModel SelectedOptionLevel
        {
            get
            {
                return _SelectedOptionLevel;
            }
            set
            {
                _SelectedOptionLevel = value;
            }

        }

        public bool IsItemOptionSpecific
        {
            get
            {
                return MenuItemViewModel != null && MenuItemViewModel.MenuItem != null && PreparationScaledOption.GetMenuItemSpecific(MenuItemViewModel.MenuItem) != null;
            }
        }

        public override FontWeight FontWeight => IsItemOptionSpecific && !IsHidden ? FontWeights.Bold : base.FontWeight;


        /// <MetaDataID>{85ac216a-c3dc-4f86-be36-55240090b256}</MetaDataID>
        ViewModelWrappers<MenuModel.ILevel, OptionLevelViewModel> OptionLevelsDitionary = new ViewModelWrappers<MenuModel.ILevel, OptionLevelViewModel>();

        internal void ToggleUncheckOtption()
        {
            if (PreparationScaledOption.LevelType != null && (PreparationScaledOption.LevelType.Levels[0].UncheckOption || PreparationScaledOption.Quantitative))
            {
                MenuModel.IMenuItem menuItem = null;
                if (MenuItemViewModel != null)
                    menuItem = MenuItemViewModel.MenuItem;

                if (PreparationScaledOption.GetInitialFor(menuItem) != PreparationScaledOption.Initial)
                {
                    PreparationScaledOption.RemoveOptionSpecificFor(menuItem);
                }
                else
                {
                    if (PreparationScaledOption.GetInitialFor(menuItem) == PreparationScaledOption.LevelType.Levels[0])
                        PreparationScaledOption.SetInitialFor(menuItem, PreparationScaledOption.LevelType.Levels[1]);
                    else
                        PreparationScaledOption.SetInitialFor(menuItem, PreparationScaledOption.LevelType.Levels[0]);
                }

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UncheckOptionImage)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontWeight)));
            }
        }

        /// <MetaDataID>{4484e120-6ccc-4726-a073-d93e7992f690}</MetaDataID>
        public List<OptionLevelViewModel> Levels
        {
            get
            {
                if (ScaleType == null)
                    return new List<OptionLevelViewModel>();
                else
                {

                    return (from level in ScaleType.Levels
                            select OptionLevelsDitionary.GetViewModelFor(level.Level, this, level.Level)).ToList();
                }
            }
        }


        ViewModelWrappers<MenuModel.IPricingContext, PreparationoOptionPriceContextViewModel> PriceContextDitionary = new ViewModelWrappers<MenuModel.IPricingContext, PreparationoOptionPriceContextViewModel>();
        public virtual List<PreparationoOptionPriceContextViewModel> PricingContexts
        {
            get
            {
                if (PreparationScaledOption is MenuModel.ItemSelectorOption)
                    return new List<PreparationoOptionPriceContextViewModel>();

                if (_SelectedPriceContext == null)
                {
                    var pricingContext = (from itemSelectorOption in PreparationScaledOption.MenuItemType.PricingContexts.OfType<MenuModel.ItemSelectorOption>()
                                          where itemSelectorOption.LevelType != null && itemSelectorOption.LevelType.Levels[0].UncheckOption && itemSelectorOption.Initial != null && !itemSelectorOption.Initial.UncheckOption
                                          select itemSelectorOption).FirstOrDefault();
                    if (pricingContext != null)
                        SelectedPriceContext = PriceContextDitionary.GetViewModelFor(pricingContext, pricingContext, this);
                    else
                    {
                        if (MenuItemViewModel != null)
                        {
                            SelectedPriceContext = PriceContextDitionary.GetViewModelFor(MenuItemViewModel.MenuItem.MenuItemPrice, MenuItemViewModel.MenuItem.MenuItemPrice, this);
                        }
                    }
                }

                return (from pricingContext in PreparationScaledOption.MenuItemType.PricingContexts
                        select PriceContextDitionary.GetViewModelFor(pricingContext, pricingContext, this)).ToList();
            }
        }

        public void SetSelectedPriceContext(MenuModel.IPricingContext pricingContext)
        {

            foreach (var pricingContextViewModel in PricingContexts)
            {
                if (pricingContextViewModel.PricingContext == pricingContext)
                    SelectedPriceContext = pricingContextViewModel;
            }

        }

        public Visibility PriceContextsVisibility
        {
            get
            {
                if (PricingContexts.Count > 0)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }


        public bool IsRecipeIngredient
        {
            get
            {
                return PreparationScaledOption.IsRecipeIngredient;
            }
            set
            {
                PreparationScaledOption.IsRecipeIngredient = value;
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRecipeIngredient)));

                if (PreparationOptionsListView is PreparationOptionsGroupViewModel)
                    (PreparationOptionsListView as PreparationOptionsGroupViewModel).IsRecipeIngredientPropertyChanged();
            }
        }

        PreparationoOptionPriceContextViewModel _SelectedPriceContext;
        public PreparationoOptionPriceContextViewModel SelectedPriceContext
        {
            get
            {
                if (_SelectedPriceContext == null)
                {
                    int load = PricingContexts.Count;
                }
                return _SelectedPriceContext;
            }
            set
            {
                _SelectedPriceContext = value;

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPriceContext)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridePrice)));
                foreach (var level in Levels)
                    level.Refresh();
            }
        }
        ViewModelWrappers<MenuModel.IScaleType, ScaleTypeViewModel> ScaleTypesDictionary = new ViewModelWrappers<MenuModel.IScaleType, ScaleTypeViewModel>();



        /// <MetaDataID>{f2fe93a1-baee-4478-8e7f-e08522e16cff}</MetaDataID>
        public List<ScaleTypeViewModel> ScaleTypes
        {
            get
            {

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(PreparationScaledOption));
                if (PreparationOptionsListView is PreparationOptionsGroupViewModel && (PreparationOptionsListView as PreparationOptionsGroupViewModel).PreparationOptionsGroup.SelectionType != MenuModel.SelectionType.SimpleGroup)
                {



                    var scaleTypes = (from scaleType in storage.GetObjectCollection<MenuModel.IScaleType>()
                                      from level in scaleType.Levels
                                      where level.UncheckOption == true
                                      select scaleType).ToList();

                    var vmScaleTypes = (from theScaleType in scaleTypes select ScaleTypesDictionary.GetViewModelFor(theScaleType, theScaleType)).ToList();



                    return vmScaleTypes;
                }
                else
                {
                    var scaleTypes = (from scaleType in storage.GetObjectCollection<MenuModel.IScaleType>() select scaleType).ToList();
                    var vmScaleTypes = (from theScaleType in scaleTypes select ScaleTypesDictionary.GetViewModelFor(theScaleType, theScaleType)).ToList();
                    return vmScaleTypes;


                }
                //_ScaleTypes = (from theScaleType in selectedTypes select ScaleTypesDictionary.GetViewModelFor(theScaleType, theScaleType)).ToList();
            }
        }

        internal void TonggleShowHide()
        {
            if (MenuItemViewModel != null)
            {
                var menuItem = MenuItemViewModel.MenuItem;
                if (!PreparationScaledOption.IsHiddenFor(menuItem) && IsChecked)
                    ToggleUncheckOtption();

                PreparationScaledOption.SetHiddenFor(menuItem, !PreparationScaledOption.IsHiddenFor(menuItem));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opacity)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontWeight)));
            }


        }



        //MenuModel.IScaleType _ScaleType;
        /// <MetaDataID>{4ad52a93-84d5-4f3e-88a0-dbf50d98806f}</MetaDataID>
        public ScaleTypeViewModel ScaleType
        {
            get
            {
                return ScaleTypesDictionary.GetViewModelFor(PreparationScaledOption.LevelType, PreparationScaledOption.LevelType);
            }
            set
            {

                PreparationScaledOption.LevelType = value.ScaleType;

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleType)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Levels)));
                PreparationOptionsListView.PreparationOptionChanged(this);
            }
        }


        /// <summary>
        /// Price is visible in minimized mode
        /// </summary>
        public Visibility PriceIsVisible
        {
            get
            {
                if (Price != 0)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public virtual string OrgPriceToolTip
        {
            get
            {
                decimal orgPrice = 0;

                if (SelectedPriceContext != null)
                    orgPrice = SelectedPriceContext.OrgPrice;
                else
                    orgPrice = (PreparationScaledOption as MenuModel.IPricedSubject).Price;

                return Resources.OrgPriceLabel + " " + orgPrice.ToString("C");
            }
        }





        decimal _OverridePrice = -1;
        public virtual decimal OverridePrice
        {
            get
            {
                if (SelectedPriceContext != null)
                    return SelectedPriceContext.OverridePrice;
                return _OverridePrice;
            }
            set
            {
                if (SelectedPriceContext != null)
                    SelectedPriceContext.OverridePrice = value;

                foreach (var level in Levels)
                    level.Refresh();
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                _OverridePrice = value;
            }
        }

        public bool Quantitative
        {
            get
            {
                return PreparationScaledOption.Quantitative;
            }
            set
            {
                PreparationScaledOption.Quantitative = value;
            }
        }




        List<TagViewModel> _Tags;

        public List<TagViewModel> Tags
        {
            get
            {
                if (_Tags == null)
                {
                    List<TagViewModel> tags = new List<TagViewModel>();
                    int index = 0;
                    if (PreparationScaledOption.PreparationTags != null)
                    {
                        foreach (var tag in PreparationScaledOption.PreparationTags.Split(';'))
                        {
                            var tagPresentation = new TagViewModel(tag, index++);
                            tagPresentation.TagDeleted += TagPresentation_TagDeleted;
                            tagPresentation.NameChanged += TagPresentation_NameChanged;
                            tags.Add(tagPresentation);
                        }
                    }
                    _Tags = tags;
                }

                return _Tags;
            }
        }
        public WPFUIElementObjectBind.RelayCommand NewTagCommand { get; protected set; }

        private void TagPresentation_TagDeleted(TagViewModel tag)
        {
            _Tags.RemoveAt(tag.Index);
            tag.TagDeleted -= TagPresentation_TagDeleted;
            tag.NameChanged -= TagPresentation_NameChanged;
            int i = 0;
            PreparationScaledOption.PreparationTags = null;
            foreach (var theTag in _Tags)
            {
                theTag.Index = _Tags.IndexOf(theTag);
                if (_Tags.IndexOf(theTag) == 0)
                    PreparationScaledOption.PreparationTags = tag.Name;
                else
                    PreparationScaledOption.PreparationTags = ";" + tag.Name;
            }
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));
        }

        private void NewTag()
        {
            var tagPresentation = new TagViewModel("new tag", Tags.Count);
            tagPresentation.TagDeleted += TagPresentation_TagDeleted;
            tagPresentation.NameChanged += TagPresentation_NameChanged;
            if (Tags.Count == 0)
                PreparationScaledOption.PreparationTags += tagPresentation.Name;
            else
                PreparationScaledOption.PreparationTags += ";" + tagPresentation.Name;

            _Tags.Add(tagPresentation);

            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tags)));

        }

        private void TagPresentation_NameChanged(TagViewModel tag)
        {
            int i = 0;
            PreparationScaledOption.PreparationTags = null;
            foreach (var theTag in _Tags)
            {
                theTag.Index = _Tags.IndexOf(theTag);
                if (_Tags.IndexOf(theTag) == 0)
                    PreparationScaledOption.PreparationTags = tag.Name;
                else
                    PreparationScaledOption.PreparationTags = ";" + tag.Name;
            }
        }



        /// <MetaDataID>{91cfab13-4383-4cae-a993-ff04ce46bcec}</MetaDataID>
        public virtual decimal Price
        {
            get
            {
                if (!(PreparationScaledOption is MenuModel.IPricedSubject))
                    return 0;

                if (SelectedPriceContext != null)
                    return SelectedPriceContext.Price;
                return (PreparationScaledOption as MenuModel.IPricedSubject).Price;
            }
            set
            {
                if (!(PreparationScaledOption is MenuModel.IPricedSubject))
                    return;
                if (Price == value)
                    return;

                if (SelectedPriceContext != null)
                    SelectedPriceContext.Price = value;
                else
                    (PreparationScaledOption as MenuModel.IPricedSubject).Price = value;

                foreach (var level in Levels)
                    level.Refresh();

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridePrice)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceIsVisible)));

            }
        }



        /// <MetaDataID>{c929f544-e753-4392-8b4e-2a177f04e1d7}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand EditScaleTypeCommand { get; protected set; }

        /// <MetaDataID>{caf32dc0-3730-482e-a4fa-a863b117a9cf}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SetSelectedLevelInitialCommand { get; protected set; }



        public bool IsChecked
        {
            get
            {
                if (PreparationOptionsListView is PreparationOptionsGroupViewModel)
                {
                    if (PreparationScaledOption.LevelType != null && PreparationScaledOption.LevelType.Levels[0].UncheckOption)
                    {
                        MenuModel.IMenuItem menuItem = null;
                        if (MenuItemViewModel != null)
                            menuItem = MenuItemViewModel.MenuItem;

                        if (PreparationScaledOption.GetInitialFor(menuItem).UncheckOption)
                            return false;
                        else
                            return true;
                    }
                    else if (PreparationScaledOption.Quantitative && PreparationScaledOption.LevelType != null)
                    {
                        MenuModel.IMenuItem menuItem = null;
                        if (MenuItemViewModel != null)
                            menuItem = MenuItemViewModel.MenuItem;

                        if (PreparationScaledOption.GetInitialFor(menuItem) == PreparationScaledOption.LevelType.Levels[0])
                            return false;
                        else
                            return true;
                    }
                    else
                        return false;
                }
                else if (PreparationOptionsListView is MenuItemTypeViewModel)
                {
                    if (PreparationScaledOption.LevelType != null && PreparationScaledOption.LevelType.Levels[0].UncheckOption)
                    {
                        MenuModel.IMenuItem menuItem = null;
                        if (MenuItemViewModel != null)
                            menuItem = MenuItemViewModel.MenuItem;

                        if (PreparationScaledOption.GetInitialFor(menuItem).UncheckOption)
                            return false;
                        else
                            return true;
                    }
                    else if (PreparationScaledOption.Quantitative && PreparationScaledOption.LevelType != null)
                    {
                        MenuModel.IMenuItem menuItem = null;
                        if (MenuItemViewModel != null)
                            menuItem = MenuItemViewModel.MenuItem;

                        if (PreparationScaledOption.GetInitialFor(menuItem) == PreparationScaledOption.LevelType.Levels[0])
                            return false;
                        else
                            return true;
                    }
                    else
                        return false;
                }
                else
                    return false;


            }
        }

        public ImageSource UncheckOptionImage
        {
            get
            {

                if (IsChecked)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/checkRed16.png"));
                else
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
            }
        }

        public override bool IsHidden
        {
            get
            {
                if (MenuItemViewModel != null)
                {
                    var menuItem = MenuItemViewModel.MenuItem;
                    return PreparationScaledOption.IsHiddenFor(menuItem);
                }
                return false;
            }
        }


        public double Opacity
        {
            get
            {
                if (IsHidden)
                    return 0.5;
                else
                    return 1;
            }
        }

        internal void Refresh()
        {
            foreach (var level in Levels)
                level.Refresh();

            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UncheckOptionImage)));
        }
    }
}
