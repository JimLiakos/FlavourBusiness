using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using MenuModel;
using OOAdvantech.Transactions;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{109b578e-d78d-46a1-a7a1-527067bc82ba}</MetaDataID>
    public class PreparationOptionsGroupViewModel : PreparationOptionViewModel, IPreparationOptionsListView
    {

        public bool HideName
        {
            get => PreparationOptionsGroup.HideName;
            set => PreparationOptionsGroup.HideName = value;
        }
        /// <MetaDataID>{8681a8af-2578-42aa-b475-3f61ba98d6f1}</MetaDataID>
        internal MenuModel.IPreparationOptionsGroup PreparationOptionsGroup;

        public PreparationOptionsGroupViewModel()
        {

        }

        /// <MetaDataID>{7f45823e-b789-4a94-ba77-2c20b9043b69}</MetaDataID>
        public PreparationOptionsGroupViewModel(MenuModel.IPreparationOptionsGroup preparationOptionsGroup, IPreparationOptionsListView preparationOptionsListView, bool isEditable)
                    : base(preparationOptionsGroup, preparationOptionsListView, isEditable)
        {
            PreparationOptionsGroup = preparationOptionsGroup;


            AddOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(PreparationOptionsGroup);
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

                var checkUncheckScaleType = (from scaleType in storage.GetObjectCollection<MenuModel.FixedScaleType>()
                                             where scaleType.UniqueIdentifier == MenuModel.FixedScaleTypes.CheckUncheck
                                             select scaleType).FirstOrDefault();
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var preparationOption = PreparationOptionsGroup.NewPreparationOption();
                    preparationOption.Name = Properties.Resources.PreparationScaledOptionDefaultName;

                    //var preparationOption = new MenuModel.PreparationScaledOption(Properties.Resources.PreparationScaledOptionDefaultName);

                    //if (objectStorage != null)
                    //    objectStorage.CommitTransientObjectState(preparationOption);

                    preparationOption.LevelType = checkUncheckScaleType;
                    preparationOption.Initial = checkUncheckScaleType.Levels[0];

                    //PreparationOptionsGroup.AddPreparationOption(preparationOption);
                    stateTransition.Consistent = true;
                }
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));
            });



            AddOptionsGroupCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var preparationOption = (PreparationOptionsGroup as StepperOptionsGroup).NewNestedOptionsGroups();
                    OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(PreparationOptionsGroup);
                    if (objectStorage != null)
                        objectStorage.CommitTransientObjectState(preparationOption);

                    stateTransition.Consistent = true;
                }
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));

            });


            RenameSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedOption.Edit = true;
            }, (object sender) => SelectedOption != null);

            EditSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                if (SelectedOption.ViewType == PreparationOptionViewType.Minimize)
                {
                    foreach (var option in GroupedOptions)
                        option.Minimize();
                    SelectedOption.Maximaze();
                }
                else
                    SelectedOption.Minimize();

            }, (object sender) => SelectedOption != null);

            DeleteSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {

                    if (isEditable)
                        PreparationOptionsGroup.RemovePreparationOption(SelectedOption.PreparationOption as MenuModel.IPreparationScaledOption);
                    else
                    {
                        if (SelectedOption != null && SelectedScaledOption.IsItemOptionSpecific && SelectedScaledOption.PreparationScaledOption != null)
                        {
                            SelectedScaledOption.RemoveOptionSpecificFor(SelectedOption.MenuItemViewModel.MenuItem);
                            PreparationOptionChanged(SelectedOption);
                        }

                    }
                    stateTransition.Consistent = true;
                }

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));
            }, (object sender) =>
            {
                if (IsEditable)
                    return SelectedOption != null;
                else
                    return SelectedOption != null && SelectedScaledOption.IsItemOptionSpecific;
            });


            MoveUpSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { MoveUpSelectedOption(); }, (object sender) => SelectedOption != null);
            MoveDownSelectedOptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { MoveDownSelectedOption(); }, (object sender) => SelectedOption != null);
            SetSelectedOptionCheckUncheckCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { SetSelectedOptionCheckUncheck(); }, (object sender) => SelectedOption != null);

            SetSelectedOptionHideShowCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { SetSelectedOptionHideShow(); }, (object sender) => SelectedOption != null);
            if (PreparationOptionsGroup is ItemSelectorOptionsGroup)
            {
                _SelectionTypes = new List<SelectionTypeViewModel>()
                {
                    new SelectionTypeViewModel (PreparationOptionsGroup,  MenuModel.SelectionType.AtLeastOneSelected),
                };
            }
            else
            {
                _SelectionTypes = new List<SelectionTypeViewModel>()
                {
                    new SelectionTypeViewModel(PreparationOptionsGroup, MenuModel.SelectionType.SimpleGroup ),
                    new SelectionTypeViewModel (PreparationOptionsGroup, MenuModel.SelectionType.SingleSelection ),
                    new SelectionTypeViewModel (PreparationOptionsGroup,  MenuModel.SelectionType.AtLeastOneSelected),
                    new SelectionTypeViewModel (PreparationOptionsGroup,   MenuModel.SelectionType.MultiSelection)
                };
            }


            ClearSelectorAlwaysInDescriptionCommand = new WPFUIElementObjectBind.RelayCommand((object sender) => { 
                (MenuItemViewModel.MenuItem as MenuModel.MenuItem).ClearSelectorAlwaysInDescription();
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlwaysInDescriptionFontWeight)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectorAlwaysInDescription)));
            }, (object sender) => MenuItemViewModel != null && (MenuItemViewModel.MenuItem as MenuModel.MenuItem).SelectorAlwaysInDescriptionOverriden);
            _SelectionType = new ViewModel.SelectionTypeViewModel(PreparationOptionsGroup);
        }

        ViewModelWrappers<MenuModel.IPricingContext, PreparationoOptionPriceContextViewModel> PriceContextDitionary = new ViewModelWrappers<MenuModel.IPricingContext, PreparationoOptionPriceContextViewModel>();
        public virtual List<PreparationoOptionPriceContextViewModel> PricingContexts
        {
            get
            {
                if (PreparationOptionsGroup is MenuModel.ItemSelectorOptionsGroup)
                    return new List<PreparationoOptionPriceContextViewModel>();

                if (_SelectedPriceContext == null)
                {
                    var pricingContext = (from itemSelectorOption in PreparationOptionsGroup.MenuItemType.PricingContexts.OfType<MenuModel.ItemSelectorOption>()
                                          where itemSelectorOption.LevelType != null && itemSelectorOption.LevelType.Levels[0].UncheckOption && itemSelectorOption.Initial != null && !itemSelectorOption.Initial.UncheckOption
                                          select itemSelectorOption).FirstOrDefault();
                    if (pricingContext != null)
                        SelectedPriceContext = PriceContextDitionary.GetViewModelFor(pricingContext, pricingContext, this);
                }

                return (from pricingContext in PreparationOptionsGroup.MenuItemType.PricingContexts
                        select PriceContextDitionary.GetViewModelFor(pricingContext, pricingContext, this)).ToList();
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

        internal void IsRecipeIngredientPropertyChanged()
        {
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRecipeIngredient)));
        }
        public bool? IsRecipeIngredient
        {
            get
            {
                if (GroupedOptions.OfType<PreparationScaledOptionViewModel>().Where(x => !x.IsRecipeIngredient).Count() == 0)
                    return true;

                if (GroupedOptions.OfType<PreparationScaledOptionViewModel>().Where(x => x.IsRecipeIngredient).Count() == 0)
                    return false;
                return null;
            }
            set
            {
                if (value.HasValue)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        foreach (var option in GroupedOptions.OfType<PreparationScaledOptionViewModel>())
                            option.IsRecipeIngredient = value.Value;

                        stateTransition.Consistent = true;
                    }
                }
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

                if (_SelectedPriceContext != null)
                {
                    var sds = _SelectedPriceContext.Name;
                }

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPriceContext)));

                foreach (var preparationScaledOptionViewModel in GroupedOptions.OfType<PreparationScaledOptionViewModel>())
                    preparationScaledOptionViewModel.SetSelectedPriceContext(_SelectedPriceContext.PricingContext);

                if (this.PreparationOptionsGroup is StepperOptionsGroup)
                {
                    foreach (var preparationOptionsGroupViewModel in GroupedOptions.OfType<PreparationOptionsGroupViewModel>())
                        preparationOptionsGroupViewModel.SelectedPriceContext = _SelectedPriceContext;
                }


            }
        }

        /// <MetaDataID>{fa5f38d3-2157-4c7f-bbe7-1c328394c8b9}</MetaDataID>
        public bool IsSelectionTypeEditable
        {
            get
            {
                if (PreparationOptionsGroup is ItemSelectorOptionsGroup)
                    return false;
                else
                    return IsEditable;
            }
        }


        /// <MetaDataID>{44c90dd2-b933-46a7-810d-6ac16025555e}</MetaDataID>
        List<SelectionTypeViewModel> _SelectionTypes;
        /// <MetaDataID>{25012363-24fb-40f1-aa28-2139450764a6}</MetaDataID>
        public List<SelectionTypeViewModel> SelectionTypes
        {
            get
            {

                return _SelectionTypes;
            }
        }
        /// <MetaDataID>{0712d2e2-2df5-4f8f-8e9c-e73771e434a1}</MetaDataID>
        SelectionTypeViewModel _SelectionType;
        /// <MetaDataID>{78a755df-c4a6-4225-a894-4a4cc490a6d1}</MetaDataID>
        public SelectionTypeViewModel SelectionType
        {
            get
            {
                return _SelectionType;
            }
            set
            {
                // PreparationOptionsGroup.SelectionType = value;
            }
        }
        public Visibility SelectorAlwaysInDescriptionIsVisible
        {
            get
            {
                if (PreparationOptionsGroup is ItemSelectorOptionsGroup)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public  FontWeight AlwaysInDescriptionFontWeight => MenuItemViewModel!=null&& (MenuItemViewModel.MenuItem as  MenuModel.MenuItem).SelectorAlwaysInDescriptionOverriden ? FontWeights.Bold : base.FontWeight;


        public bool SelectorAlwaysInDescription
        {
            get
            {


                if (MenuItemViewModel != null)
                    return MenuItemViewModel.MenuItem.SelectorAlwaysInDescription;

                if (PreparationOptionsGroup is ItemSelectorOptionsGroup)
                    return (PreparationOptionsGroup as ItemSelectorOptionsGroup).AlwaysInDescription;
                return false;
            }
            set
            {
                if (MenuItemViewModel != null)
                    MenuItemViewModel.MenuItem.SelectorAlwaysInDescription = value;
                else if (PreparationOptionsGroup is ItemSelectorOptionsGroup)
                    (PreparationOptionsGroup as ItemSelectorOptionsGroup).AlwaysInDescription = value; ;

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AlwaysInDescriptionFontWeight)));

            }

        }

        /// <MetaDataID>{b4fb5205-60e7-4072-ae1a-590f14ea7def}</MetaDataID>
        private void SetSelectedOptionCheckUncheck()
        {
            if (SelectedScaledOption != null && SelectedScaledOption.PreparationScaledOption.Initial != null)
            {
                SelectedScaledOption.ToggleUncheckOtption();
                PreparationOptionChanged(SelectedOption);
            }
        }
        /// <MetaDataID>{b6947651-f5e3-4d83-9a84-77065675234a}</MetaDataID>
        private void SetSelectedOptionHideShow()
        {
            if (SelectedScaledOption != null)
            {
                SelectedScaledOption.TonggleShowHide();
                PreparationOptionChanged(SelectedOption);
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionShowImagePath)));

            }
        }


        /// <MetaDataID>{484a5c8b-3a50-4cec-a13a-1ff6806892cb}</MetaDataID>
        private void MoveDownSelectedOption()
        {
            if (SelectedScaledOption != null)
            {
                int pos = PreparationOptionsGroup.GroupedOptions.IndexOf(SelectedScaledOption.PreparationScaledOption);
                if (pos < PreparationOptionsGroup.GroupedOptions.Count - 1)
                    PreparationOptionsGroup.MovePreparationOption(SelectedScaledOption.PreparationScaledOption, pos + 1);
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));
            }
            if (SelectedOption is PreparationOptionsGroupViewModel && (PreparationOptionsGroup is StepperOptionsGroup))
            {
                var stepperOptionsGroup = PreparationOptionsGroup as StepperOptionsGroup;
                var selectedOptionsGroup = (SelectedOption as PreparationOptionsGroupViewModel).PreparationOptionsGroup as PreparationOptionsGroup;
                int pos = stepperOptionsGroup.NestedOptionsGroups.IndexOf(selectedOptionsGroup);
                if (pos < stepperOptionsGroup.NestedOptionsGroups.Count - 1)
                    stepperOptionsGroup.MoveNestedOptionsGroup(selectedOptionsGroup, pos + 1);
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));
            }
        }

        /// <MetaDataID>{12635194-9306-42ea-a7a9-b68a7a135fe7}</MetaDataID>
        private void MoveUpSelectedOption()
        {
            if (SelectedScaledOption != null)
            {
                int pos = PreparationOptionsGroup.GroupedOptions.IndexOf(SelectedScaledOption.PreparationScaledOption);
                if (pos > 0)
                    PreparationOptionsGroup.MovePreparationOption(SelectedScaledOption.PreparationScaledOption, pos - 1);

                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));
            }
            if (SelectedOption is PreparationOptionsGroupViewModel && (PreparationOptionsGroup is StepperOptionsGroup))
            {
                var stepperOptionsGroup = PreparationOptionsGroup as StepperOptionsGroup;
                var selectedOptionsGroup = (SelectedOption as PreparationOptionsGroupViewModel).PreparationOptionsGroup as PreparationOptionsGroup;
                int pos = stepperOptionsGroup.NestedOptionsGroups.IndexOf(selectedOptionsGroup);
                if (pos > 0)
                    stepperOptionsGroup.MoveNestedOptionsGroup(selectedOptionsGroup, pos - 1);
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupedOptions)));
            }

        }
        public override bool IsHidden => base.IsHidden;

        //ViewModelWrappers<MenuModel.IPreparationOptionsGroup, PreparationOptionsGroupViewModel> PreparationOptionsWrappers = new ViewModelWrappers<MenuModel.IPreparationOptionsGroup, PreparationOptionsGroupViewModel>();
        /// <MetaDataID>{3e9dd827-b403-49e1-abb1-5c31ab9dae1f}</MetaDataID>
        ViewModelWrappers<MenuModel.IPreparationScaledOption, PreparationScaledOptionViewModel> PreparationScaledOptionsWrappers = new ViewModelWrappers<MenuModel.IPreparationScaledOption, PreparationScaledOptionViewModel>();

        /// <MetaDataID>{d0ba3d1a-b032-4bc4-8305-c64fe6f4026a}</MetaDataID>
        ViewModelWrappers<MenuModel.ItemSelectorOption, ItemSelectorViewModel> ItemSelectorsWrappers = new ViewModelWrappers<ItemSelectorOption, ItemSelectorViewModel>();


        ViewModelWrappers<MenuModel.IPreparationOptionsGroup, PreparationOptionsGroupViewModel> OptionsGroupWrappers = new ViewModelWrappers<MenuModel.IPreparationOptionsGroup, PreparationOptionsGroupViewModel>();

        /// <MetaDataID>{0a8e04dd-df76-4379-9d88-7e19e7d3fc6c}</MetaDataID>
        public List<PreparationOptionViewModel> GroupedOptions
        {
            get
            {

                //return (from scaledOption in RealObject.Options.OfType<MenuModel.IPreparationScaledOption>()
                //        select PreparationScaledOptionsWrappers.GetViewModelFor(scaledOption, scaledOption, this)).OfType<PreparationOptionViewModel>().Union(
                //from optionsGroup in RealObject.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                //select PreparationOptionsWrappers.GetViewModelFor(optionsGroup, optionsGroup, this, _IsEditable)).ToList();

                var optionsViewModels = (from scaledOption in PreparationOptionsGroup.GroupedOptions
                                         where scaledOption.GetType() == typeof(MenuModel.PreparationScaledOption)
                                         select PreparationScaledOptionsWrappers.GetViewModelFor(scaledOption, scaledOption, this, IsEditable)).OfType<PreparationOptionViewModel>().Union(
                                    (from itemSelectorOption in PreparationOptionsGroup.GroupedOptions.OfType<MenuModel.ItemSelectorOption>()
                                     select ItemSelectorsWrappers.GetViewModelFor(itemSelectorOption, itemSelectorOption, this, IsEditable)).OfType<PreparationOptionViewModel>()).ToList();



                if (PreparationOptionsGroup is StepperOptionsGroup)
                    optionsViewModels = optionsViewModels.Union((from optionGroup in (PreparationOptionsGroup as StepperOptionsGroup).NestedOptionsGroups
                                                                 select OptionsGroupWrappers.GetViewModelFor(optionGroup, optionGroup, this, IsEditable)).OfType<PreparationOptionViewModel>()).ToList();



                return (from optionViewModel in optionsViewModels
                        where !optionViewModel.IsHidden
                        select optionViewModel).Union(
                    from optionViewModel in optionsViewModels
                    where optionViewModel.IsHidden
                    select optionViewModel
                    ).ToList();



            }
        }

        public PreparationScaledOptionViewModel SelectedScaledOption
        {
            get
            {
                return SelectedOption as PreparationScaledOptionViewModel;
            }
        }
        /// <exclude>Excluded</exclude>
        PreparationOptionViewModel _SelectedOption;
        /// <MetaDataID>{b069f31b-1db7-4824-b44d-f9e92fb33c28}</MetaDataID>
        public PreparationOptionViewModel SelectedOption
        {
            get
            {
                return _SelectedOption;
            }
            set
            {
                _SelectedOption = value;
                _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedOption)));
            }
        }



        /// <MetaDataID>{00433e35-d488-41c5-9c46-6167f8427ee3}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand DeleteSelectedOptionCommand { get; protected set; }

        /// <MetaDataID>{c4738f75-1460-46a4-bd56-4a272a9baceb}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand RenameSelectedOptionCommand { get; protected set; }

        /// <MetaDataID>{48409cd0-cdbf-4efb-9922-77bf84d8fde2}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand AddOptionCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand ClearSelectorAlwaysInDescriptionCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand AddOptionsGroupCommand { get; protected set; }

        public Visibility AddOptionsGroupVisibility
        {
            get
            {
                if (this.PreparationOptionsGroup is StepperOptionsGroup)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;

            }
        }
        /// <MetaDataID>{6f355586-0473-4104-9804-6e92dcd34b41}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand EditSelectedOptionCommand { get; protected set; }

        /// <MetaDataID>{ebc55c75-d0c4-42e0-a6e5-39de0f5c3a38}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand MoveDownSelectedOptionCommand { get; protected set; }
        /// <MetaDataID>{11f9e524-c411-4ea7-bc0d-0a0f12f47769}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand MoveUpSelectedOptionCommand { get; protected set; }

        /// <MetaDataID>{4dfdbe75-bf0c-40fd-83f8-46bb28d8dbe0}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SetSelectedOptionCheckUncheckCommand { get; protected set; }

        /// <MetaDataID>{5d5c4384-4c0b-424b-9886-a4c0acc1470a}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SetSelectedOptionHideShowCommand { get; protected set; }



        string _OptionShowImagePath = @"pack://application:,,,/MenuItemsEditor;Component/Image/view16.png";
        string _OptionHideImagePath = @"pack://application:,,,/MenuItemsEditor;Component/Image/restriction16.png";
        public string OptionShowImagePath
        {
            get
            {
                if (SelectedScaledOption != null && SelectedScaledOption.IsHidden)
                    return _OptionShowImagePath;
                return _OptionHideImagePath;
            }
        }



        /// <MetaDataID>{d92138da-15ef-4b95-b670-76efa6b805c2}</MetaDataID>
        public override void Maximaze()
        {
            _ViewType = PreparationOptionViewType.OptionsGroup;
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewType)));
            PreparationOptionsListView.Maximazed(this);
        }


        /// <MetaDataID>{b2a5016d-fa39-43a4-9617-2c4a46347644}</MetaDataID>
        public void Maximazed(PreparationOptionViewModel preparationOptionViewModel)
        {
            foreach (var option in GroupedOptions)
            {
                if (option != preparationOptionViewModel)
                    option.ViewType = PreparationOptionViewType.Minimize;
            }

            SelectedOption = GroupedOptions[0];
            if (preparationOptionViewModel is PreparationScaledOptionViewModel)
                SelectedOption = preparationOptionViewModel as PreparationScaledOptionViewModel;
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionsListViewButtonsVisible)));
        }
        public void Minimized(PreparationOptionViewModel preparationOptionViewModel)
        {
            _PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionsListViewButtonsVisible)));
        }

        public Visibility OptionsListViewButtonsVisible
        {
            get
            {
                foreach (var option in GroupedOptions)
                {

                    if (option.ViewType != PreparationOptionViewType.Minimize)
                        return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        /// <MetaDataID>{27bbe703-421c-428a-9ed3-51a454df455f}</MetaDataID>
        public void PreparationOptionChanged(PreparationOptionViewModel preparationOptionViewModel)
        {
            if (preparationOptionViewModel.PreparationOption is MenuModel.IPreparationScaledOption)
            {
                var scaledOption = preparationOptionViewModel.PreparationOption as MenuModel.IPreparationScaledOption;

                IMenuItem menuItem = null;
                if (MenuItemViewModel != null)
                    menuItem = MenuItemViewModel.MenuItem;
                PreparationOptionsGroup.GroupedOptionChanged(scaledOption, menuItem);

                foreach (var selectionTypeViewModel in SelectionTypes)
                    selectionTypeViewModel.Refresh();


                foreach (var groupedOptionViewModel in GroupedOptions)
                    groupedOptionViewModel.Refresh();



            }
        }

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



        /// <MetaDataID>{f0a15226-232d-4df8-8b2d-7f372cb25d11}</MetaDataID>
        public override Visibility HideOptionIsVisible
        {
            get
            {
                if (this.MenuItemViewModel != null)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

    }

    /// <MetaDataID>{729a6844-fba1-43e1-b6d9-0d4ef5520c44}</MetaDataID>
    public class SelectionTypeViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        /// <MetaDataID>{607eba98-3a3f-4aa6-b3ad-e4fef85b4e18}</MetaDataID>
        MenuModel.IPreparationOptionsGroup PreparationOptionsGroup;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{03f57d19-8961-41ed-b76c-0eddf3a2a059}</MetaDataID>
        public SelectionTypeViewModel(MenuModel.IPreparationOptionsGroup preparationOptionsGroup, MenuModel.SelectionType selectionType)
        {
            PreparationOptionsGroup = preparationOptionsGroup;

            PreparationOptionsGroup.ObjectChangeState += PreparationOptionsGroup_ObjectChangeState;
            SelectionType = selectionType;
        }

        /// <MetaDataID>{81b89a74-c8eb-42bc-ae9b-43b2d42b5b22}</MetaDataID>
        public SelectionTypeViewModel(IPreparationOptionsGroup preparationOptionsGroup)
        {
            PreparationOptionsGroup = preparationOptionsGroup;
            PreparationOptionsGroup.ObjectChangeState += PreparationOptionsGroup_ObjectChangeState;
        }

        /// <MetaDataID>{d2a6b5f0-4f7b-4710-b05a-b60bf64d1f29}</MetaDataID>
        private void PreparationOptionsGroup_ObjectChangeState(object _object, string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CheckBoxIsEnable)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectionTypeDescription)));
        }

        /// <MetaDataID>{c18a9c2e-d815-4dee-9031-e96a2c1d8af5}</MetaDataID>
        internal void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CheckBoxIsEnable)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectionTypeDescription)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CheckBoxTooltip)));
        }

        /// <MetaDataID>{f145589e-2691-48de-a388-e73f59d82edf}</MetaDataID>
        MenuModel.SelectionType? SelectionType;

        /// <MetaDataID>{f760bb97-c6b8-415f-8475-40f0d86c9603}</MetaDataID>
        public bool CheckBoxIsEnable
        {
            get
            {
                if (PreparationOptionsGroup is ItemSelectorOptionsGroup)
                    return false;

                if (SelectionType == MenuModel.SelectionType.AtLeastOneSelected ||
                    SelectionType == MenuModel.SelectionType.MultiSelection ||
                    SelectionType == MenuModel.SelectionType.SingleSelection)
                {
                    foreach (MenuModel.IPreparationScaledOption preparationScaledOption in PreparationOptionsGroup.GroupedOptions)
                    {
                        if (preparationScaledOption.LevelType == null)
                            return false;
                        if (!preparationScaledOption.LevelType.Levels[0].UncheckOption)
                            return false;
                    }
                    return true;
                }
                return true;
            }
        }


        /// <MetaDataID>{28f7b04a-6113-49f8-bf7f-d45fcf50d868}</MetaDataID>
        System.Windows.Controls.ToolTip _CheckBoxTooltip = new System.Windows.Controls.ToolTip();
        /// <MetaDataID>{be77599b-68c9-4658-8cbe-f8c855168a06}</MetaDataID>
        public System.Windows.Controls.ToolTip CheckBoxTooltip
        {
            get
            {


                if (!CheckBoxIsEnable)
                {
                    _CheckBoxTooltip.Content = string.Format(Properties.Resources.SelectionTypeViewModelCheckBoxPrompt, SelectionTypeDescription);
                    return _CheckBoxTooltip;
                }
                else
                    return null;


            }
        }



        /// <MetaDataID>{48446029-efac-4580-b020-551ccf7835d4}</MetaDataID>
        public bool IsChecked
        {
            get
            {

                if (PreparationOptionsGroup.SelectionType == SelectionType)
                    return true;
                else
                    if ((PreparationOptionsGroup.SelectionType & SelectionType) != 0)
                    return true;
                return false;
            }
            set
            {
                if (SelectionType.HasValue)
                {
                    if (value)
                    {
                        if ((PreparationOptionsGroup.SelectionType & MenuModel.SelectionType.AtLeastOneSelected) > 0 && SelectionType != MenuModel.SelectionType.SimpleGroup)
                            PreparationOptionsGroup.SelectionType = SelectionType.Value | MenuModel.SelectionType.AtLeastOneSelected;
                        else if (SelectionType == MenuModel.SelectionType.AtLeastOneSelected && PreparationOptionsGroup.SelectionType != MenuModel.SelectionType.SimpleGroup)
                            PreparationOptionsGroup.SelectionType = PreparationOptionsGroup.SelectionType | MenuModel.SelectionType.AtLeastOneSelected;
                        else
                            PreparationOptionsGroup.SelectionType = SelectionType.Value;
                        if (PreparationOptionsGroup.SelectionType == MenuModel.SelectionType.AtLeastOneSelected)
                            PreparationOptionsGroup.SelectionType = MenuModel.SelectionType.SingleSelection | MenuModel.SelectionType.AtLeastOneSelected;
                    }
                    else
                    {
                        PreparationOptionsGroup.SelectionType = PreparationOptionsGroup.SelectionType & (~SelectionType.Value);
                        Refresh();
                    }
                }



            }
        }
        /// <MetaDataID>{fd29fd2d-692b-4bff-a78b-b81ab03479e5}</MetaDataID>
        public string SelectionTypeDescription
        {
            get
            {
                if (PreparationOptionsGroup is ItemSelectorOptionsGroup)
                    return Properties.Resources.SelectionTypeItemSelectionDescription;

                if (SelectionType.HasValue)
                {
                    if (SelectionType == MenuModel.SelectionType.MultiSelection)
                        return Properties.Resources.SelectionTypeMultiSelectionDescription;
                    if (SelectionType == MenuModel.SelectionType.SingleSelection)
                        return Properties.Resources.SelectionTypeSingleSelectionDescription;

                    if (SelectionType == MenuModel.SelectionType.AtLeastOneSelected)
                        return Properties.Resources.SelectionTypeAtLeastOneSelectedDescription;
                    return Properties.Resources.SelectionTypeSimpleGroupDescription;
                }
                else
                {
                    if ((PreparationOptionsGroup.SelectionType & MenuModel.SelectionType.MultiSelection) != 0)
                        return Properties.Resources.SelectionTypeMultiSelectionDescription;
                    if ((PreparationOptionsGroup.SelectionType & MenuModel.SelectionType.SingleSelection) != 0)
                        return Properties.Resources.SelectionTypeSingleSelectionDescription;
                    if (PreparationOptionsGroup.SelectionType == MenuModel.SelectionType.SimpleGroup)
                        return Properties.Resources.SelectionTypeSimpleGroupDescription;

                    if (PreparationOptionsGroup.SelectionType == MenuModel.SelectionType.AtLeastOneSelected)
                        return Properties.Resources.SelectionTypeAtLeastOneSelectedDescription;
                    return Properties.Resources.SelectionTypeSimpleGroupDescription;
                }
            }
        }
    }
}
