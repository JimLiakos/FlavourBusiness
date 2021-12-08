using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Finance.ViewModel;
using FinanceFacade;
using GenWebBrowser;
using Newtonsoft.Json;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{685b0650-6fe3-440d-bb48-39a3c25e92b4}</MetaDataID>
    public class MenuItemViewModel : MarshalByRefObject, INotifyPropertyChanged, IPreparationOptionsListView
    {


        /// <MetaDataID>{739e628f-fdc6-47eb-a15c-414453971130}</MetaDataID>
        CultureInfo _SelectedCulture;
        /// <MetaDataID>{e49e6db8-8c8a-4633-a722-bac75f5616dd}</MetaDataID>
        public CultureInfo SelectedCulture
        {
            set
            {
                _SelectedCulture = value;
                if (_MenuItemTSViewModel != null)
                    _MenuItemTSViewModel.SelectedCulture = value;

            }
        }
        /// <exclude>Excluded</exclude>
        MenuItemTSViewModel _MenuItemTSViewModel = null;
        /// <MetaDataID>{95867009-58d7-42b3-84d9-6e935548c15e}</MetaDataID>
        public MenuItemTSViewModel MenuItemTSViewModel
        {
            get
            {
                if (_MenuItemTSViewModel == null)
                    _MenuItemTSViewModel = new ViewModel.MenuItemTSViewModel(this);

                return _MenuItemTSViewModel;
            }
        }

        //public MenuItemViewModel()
        //{
        //}


        /// <exclude>Excluded</exclude>

        Dictionary<FinanceFacade.ITaxableType, TaxableTypeViewModel> _TaxableTypes;



        /// <MetaDataID>{a611bd6a-2251-4841-a2d5-458f0343ca5f}</MetaDataID>
        public IList<TaxableTypeViewModel> TaxableTypes
        {
            get
            {
                if ((MenuItem is MenuModel.MenuItem) &&
                    (MenuItem as MenuModel.MenuItem).Menu != null
                    && (MenuItem as MenuModel.MenuItem).Menu.TaxAuthority != null)
                {
                    if (_TaxableTypes == null)
                        _TaxableTypes = (from taxableType in (MenuItem as MenuModel.MenuItem).Menu.TaxAuthority.TaxableTypes
                                         select new TaxableTypeViewModel(taxableType)).ToDictionary(x => x.TaxableType);

                    return _TaxableTypes.Values.ToList();
                }
                else
                    return new List<TaxableTypeViewModel>();
            }
        }
        /// <MetaDataID>{2d4bec53-4750-48d6-b42f-ba6e1025db7a}</MetaDataID>
        private bool CanAddTaxableType()
        {
            if ((MenuItem is MenuModel.MenuItem) && (MenuItem as MenuModel.MenuItem).Menu != null && (MenuItem as MenuModel.MenuItem).Menu.TaxAuthority != null)
                return true;
            else
                return false;
        }


        //CultureInfo _SelectedCulture= CultureInfo.CurrentCulture;
        //public CultureInfo SelectedCulture
        //{
        //    get
        //    {
        //        return _SelectedCulture;
        //        //if (_SelectedCulture == null)
        //        //{
        //        //    _SelectedCulture = Cultures.Where(x => x.CultureInfo.Name == CultureInfo.CurrentCulture.Name).FirstOrDefault();
        //        //    if (_SelectedCulture == null)
        //        //        _SelectedCulture = Cultures.Where(x => x.CultureInfo.Name == CultureInfo.CurrentCulture.Parent.Name).FirstOrDefault();
        //        //}
        //        //return _SelectedCulture;
        //    }
        //    set
        //    {
        //        _SelectedCulture = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExtrasDescription)));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        //    }
        //}


        //CulturePresentation _SelectedCulture;
        //public CulturePresentation SelectedCulture
        //{
        //    get
        //    {
        //        if (_SelectedCulture == null)
        //        {
        //            _SelectedCulture = Cultures.Where(x => x.CultureInfo.Name == CultureInfo.CurrentCulture.Name).FirstOrDefault();
        //            if(_SelectedCulture==null)
        //                _SelectedCulture = Cultures.Where(x => x.CultureInfo.Name == CultureInfo.CurrentCulture.Parent.Name).FirstOrDefault();
        //        }
        //        return _SelectedCulture;
        //    }
        //    set
        //    {
        //        _SelectedCulture = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExtrasDescription)));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        //    }
        //}

        //List<CulturePresentation> _Cultures;
        //public List<CulturePresentation> Cultures
        //{
        //    get
        //    {
        //        if(_Cultures==null)
        //            _Cultures= CultureInfo.GetCultures(CultureTypes.AllCultures).Select(x=>new CulturePresentation(x) ).ToList();

        //        return _Cultures;
        //    }
        //}
        /// <MetaDataID>{d0c540f3-2a87-4e35-ae7c-f14b47e59b37}</MetaDataID>
        public MenuItemViewModel()
        {

        }
        /// <MetaDataID>{55fab2b6-7914-489d-8473-fc906db3b8a3}</MetaDataID>
        private bool CanEditTaxableType()
        {
            return SelectedTaxableType != null;
        }
        /// <MetaDataID>{029d38c8-62db-4946-ab17-7c9299c7c6be}</MetaDataID>
        public readonly MenuModel.MenuItem _MenuItem;

        /// <MetaDataID>{a237a554-8319-4533-80e9-8dd7814b5c69}</MetaDataID>
        public MenuModel.IMenuItem MenuItem
        {
            get
            {
                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_MenuItem) == null)
                {
                    ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(ItemsCategory);
                    objectStorage.CommitTransientObjectState(_MenuItem);
                    ItemsCategory.AddClassifiedItem(_MenuItem);
                }

                return _MenuItem;
            }
        }
        MealTypesViewModel _MealTypesViewModel;
        MealTypesViewModel MealTypesViewModel
        {
            get
            {
                if (_MealTypesViewModel == null)
                {
                    _MealTypesViewModel = new MealTypesViewModel(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MenuItem));
                    _SelectedMealTypeViewModel = MealTypesViewModel.MealTypes[0];
                    _SelectedMealCourseType = _SelectedMealTypeViewModel.Courses.FirstOrDefault();
                }
                return _MealTypesViewModel;

            }
        }

        /// <MetaDataID>{ac5e20a4-5ddd-4281-8b8c-0a8c534b8ae6}</MetaDataID>
        public MenuItemViewModel(MenuModel.IMenuItem menuItem)
        {

            _MenuItem = menuItem as MenuModel.MenuItem;

            var menu = (_MenuItem as MenuModel.MenuItem).Menu;
            EditSelectedOptionsTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

            }, (object sender) => CanEditSelectedOptionsType());

            AddOptionsTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                OptionsTypeSelctionViewModel optionsTypeSelectionViewModel = new OptionsTypeSelctionViewModel((MenuItem as MenuModel.MenuItem).Class as MenuModel.IItemsCategory);

                foreach (var optionsTypepresentationObject in optionsTypeSelectionViewModel.MenuItemTypes)
                {
                    if (MenuItem.Types.Contains(optionsTypepresentationObject.RealObject))
                        optionsTypepresentationObject.IsChecked = true;

                }
                List<MenuModel.IMenuItemType> chekedOptionsTypes = null;
                List<MenuModel.IMenuItemType> unChekedOptionsTypes = null;
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    try
                    {
                        System.Windows.Window win = System.Windows.Window.GetWindow(AddOptionsTypeCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                        Views.MenuItemsTypesWindow window = new Views.MenuItemsTypesWindow();
                        window.Owner = win;


                        window.GetObjectContext().SetContextInstance(optionsTypeSelectionViewModel);
                        if (window.ShowDialog().Value)
                        {
                            chekedOptionsTypes = (from OptionsTypesViewModel in optionsTypeSelectionViewModel.MenuItemTypes
                                                  where OptionsTypesViewModel.IsChecked
                                                  select OptionsTypesViewModel.RealObject).ToList();
                            unChekedOptionsTypes = (from OptionsTypesViewModel in optionsTypeSelectionViewModel.MenuItemTypes
                                                    where !OptionsTypesViewModel.IsChecked
                                                    select OptionsTypesViewModel.RealObject).ToList();

                        }
                        stateTransition.Consistent = true;
                    }
                    catch (Exception error)
                    {
                    }
                }
                if (unChekedOptionsTypes != null)
                {
                    foreach (var optionsType in menuItem.Types)
                    {
                        if (unChekedOptionsTypes.Contains(optionsType))
                            menuItem.RemoveType(optionsType);
                    }
                }

                if (chekedOptionsTypes != null)
                {
                    foreach (var optionsType in chekedOptionsTypes)
                    {
                        if (!menuItem.Types.Contains(optionsType))
                            menuItem.AddType(optionsType);
                    }
                }




                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItemTypes)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceContextsVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PricingContexts)));
                if (PricingContexts.Count > 0 && _SelectedPriceContext == null)
                {
                    var priceContext = (from itemSelectorVM in PricingContexts
                                        where itemSelectorVM.IsChecked
                                        select itemSelectorVM).FirstOrDefault();
                    if (priceContext == null)
                        priceContext = PricingContexts[0];
                    SelectedPriceContext = priceContext;

                    var menuItemsPrices = menuItem.Prices.ToList();

                    foreach (var itemSelector in from itemSelectorVM in PricingContexts select itemSelectorVM.ItemSelectorOption)
                    {
                        var menuItemPrice = itemSelector.GetMenuItemPrice(menuItem);
                        if (menuItemPrice == null)
                        {
                            menuItemPrice = new MenuModel.MenuItemPrice();
                            menuItemPrice.Name = itemSelector.Name;
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).CommitTransientObjectState(menuItemPrice);
                            menuItem.AddMenuItemPrice(menuItemPrice);
                            itemSelector.AddMenuItemPrice(menuItemPrice);
                        }
                        else if (menuItemsPrices.Contains(menuItemPrice))
                            menuItemsPrices.Remove(menuItemPrice);
                    }
                    foreach (var menuItemPrice in menuItemsPrices)
                        menuItem.RemoveMenuItemPrice(menuItemPrice);

                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPriceContext)));
            });

            RenameSelectedOptionsTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

            });

            RefreshWebItemViewCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                MenuItemTSViewModel.ItemChanged();
            });

            DeleteSelectedOptionsTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                if (SelectedMenuType != MenuItem.DedicatedType)
                    MenuItem.RemoveType(SelectedMenuType);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItemTypes)));
            }, (object sender) => CanDeleteSelectedOptionsType());

            //LoadPricingContexts();





            AddPartofMealCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                if (MealTypes.Count > 0)
                {
                    NewPartOfMeal = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewPartOfMeal)));
                }
            });

            DeletePartofMealCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
             {
                 _PartsOfMeals.Remove(SelectedPartOfMeal);
                 PartOfMealTypeDictionary.Remove(SelectedPartOfMeal.PartofMeal);
                 menuItem.RemoveMealType(SelectedPartOfMeal.PartofMeal);
                 RefreshMealTypes();


             }, (object sender) => SelectedPartOfMeal != null);

            SavePartofMealCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
        {
            NewPartOfMeal = false;

            var newPartOfMeal = menuItem.AddMealType(SelectedMealTypeViewModel.MealType, SelectedMealCourseType.MealCourseType);
            if (_PartsOfMeals == null)
                _PartsOfMeals = (from partOfMeal in MenuItem.PartofMeals select PartOfMealTypeDictionary.GetViewModelFor(partOfMeal, partOfMeal, this)).ToList();
            else
                _PartsOfMeals.Add(PartOfMealTypeDictionary.GetViewModelFor(newPartOfMeal, newPartOfMeal, this));

            RefreshMealTypes();

        });
            OpenMealTypesCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                try
                {
                    System.Windows.Window win = System.Windows.Window.GetWindow(OpenMealTypesCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                    Views.MealTypesWindow window = new Views.MealTypesWindow();
                    window.Owner = win;

                    PartsOfMealExpanded = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PartsOfMealExpanded)));

                    window.GetObjectContext().SetContextInstance(this.MealTypesViewModel);
                    window.ShowDialog();
                    PartsOfMealExpanded = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PartsOfMealExpanded)));


                    stateTransition.Consistent = true;
                }
                catch (Exception error)
                {
                }
            }

        });

            TranslateNameCommand = new RelayCommand((object sender) =>
            {
                TranslateName();
            });


            TranslateFullNameCommand = new RelayCommand((object sender) =>
            {
                TranslateFullName();
            });

            TranslateExtrasDescriptionCommand = new RelayCommand((object sender) =>
            {
                TranslateExtrasDescription();
            });

            TranslateDescriptionCommand = new RelayCommand((object sender) =>
            {
                TranslateDescription();
            });


            AddTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                var newTaxableType = (MenuItem as MenuModel.MenuItem).Menu.TaxAuthority.NewTaxableType();
                _TaxableTypes[newTaxableType] = new TaxableTypeViewModel(newTaxableType);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));


            }, (object sender) => CanAddTaxableType());
            DeleteSelectedTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                //{


                var taxableType = SelectedTaxableType.TaxableType;
                SelectedTaxableType = null;
               
                if( (MenuItem as MenuModel.MenuItem).Menu.TaxAuthority.RemoveTaxableType(taxableType))
                    _TaxableTypes.Remove(taxableType);
              

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTaxableType)));
                
                //}
            }, (object sender) => CanDeleteSelectedTaxableType());

            EditSelectedTaxableTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    try
                    {
                        System.Windows.Window win = System.Windows.Window.GetWindow(AddOptionsTypeCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                        Finance.Views.TaxableTypeWindow window = new Finance.Views.TaxableTypeWindow();
                        window.Owner = win;


                        window.GetObjectContext().SetContextInstance(SelectedTaxableType);
                        if (window.ShowDialog().Value)
                        {

                        }
                        stateTransition.Consistent = true;
                    }
                    catch (Exception error)
                    {
                    }
                }

                //(MenuItem as MenuModel.MenuItem).Menu.TaxAuthority.NewTaxableType();
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxableTypes)));


            }, (object sender) => CanEditTaxableType());

        }
        bool CanDeleteSelectedTaxableType()
        {
            if (SelectedTaxableType != null)
                return SelectedTaxableType.TaxableType.TaxableSubjects.Count == 1 && SelectedTaxableType.TaxableType.TaxableSubjects.Contains(_MenuItem)|| SelectedTaxableType.TaxableType.TaxableSubjects.Count==0;
            else
                return false;
        }
        private void RefreshMealTypes()
        {
            if (MealTypes.Count > 0)
            {
                _SelectedMealTypeViewModel = MealTypes[0];
                _SelectedMealCourseType = _SelectedMealTypeViewModel.Courses[0];
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PartsOfMeals)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewPartOfMeal)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MealTypes)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMealTypeViewModel)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMealTypeCourses)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMealCourseType)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PartsOfMealText)));
        }



        /// <MetaDataID>{b55a8afb-6a73-4507-ae0d-461b849f670a}</MetaDataID>
        private void LoadPricingContexts()
        {

            PricingContextsLoaded = true;
            if (PricingContexts.Count > 0)
            {
                _SelectedPriceContext = (from itemSelectorVM in PricingContexts
                                         where itemSelectorVM.IsChecked
                                         select itemSelectorVM).FirstOrDefault();
                if (_SelectedPriceContext == null)
                    _SelectedPriceContext = PricingContexts[0];
            }
        }

        /// <MetaDataID>{169529ac-0f58-4886-beea-a13171ba0813}</MetaDataID>
        MenuModel.IItemsCategory ItemsCategory;
        /// <MetaDataID>{7016608d-99b1-4667-8ede-cbe1c5cab725}</MetaDataID>
        public MenuItemViewModel(MenuModel.IItemsCategory itemsCategory, MenuModel.MenuItem menuItem) : this(menuItem)
        {
            ItemsCategory = itemsCategory;

            //BeforeSaveCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            //{
            //    ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(ItemsCategory);
            //    objectStorage.CommitTransientObjectState(menuItem);
            //    ItemsCategory.AddClassifiedItem(menuItem);
            //});
        }

        /// <MetaDataID>{c931d554-e783-43b4-acc2-ff26b1ef69df}</MetaDataID>
        bool PricingContextsLoaded = false;


        /// <MetaDataID>{c9a5ed8e-b026-4e04-971b-6f50db3345cc}</MetaDataID>
        private void TranslateDescription()
        {
            Description = BingAPILibrary.Translator.TranslateString(Description, OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedDescription)));
        }

        /// <MetaDataID>{c066a8de-267d-4bfe-b985-92a60317582b}</MetaDataID>
        private void TranslateExtrasDescription()
        {
            ExtrasDescription = BingAPILibrary.Translator.TranslateString(ExtrasDescription, OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExtrasDescription)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedExtrasDescription)));
        }
        /// <MetaDataID>{fe554d74-4687-4728-bfe4-89242ebe04b7}</MetaDataID>
        private void TranslateName()
        {
            Name = BingAPILibrary.Translator.TranslateString(Name, OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedName)));
        }

        /// <MetaDataID>{89e53098-b549-4dc3-8316-021578ab981c}</MetaDataID>
        private void TranslateFullName()
        {
            FullName = BingAPILibrary.Translator.TranslateString(FullName, OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedFullName)));
        }

        /// <MetaDataID>{ba4952e0-c967-4dd8-bd62-51168cb481cf}</MetaDataID>
        public bool AllowCustom
        {
            get => MenuItem != null ? MenuItem.AllowCustom : false;
            set
            {
                if (MenuItem != null)
                    MenuItem.AllowCustom = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditPromptForCustom)));

            }
        }

        /// <MetaDataID>{993991ad-f8e4-4894-8c12-50dbacfd8cfa}</MetaDataID>
        public bool EditPromptForCustom
        {
            get => MenuItem != null ? MenuItem.AllowCustom | MenuItem.Stepper : false;

        }

        /// <MetaDataID>{7c4ea8fc-8e07-480d-8fd6-2e5ec50cd76e}</MetaDataID>
        public bool Stepper
        {
            get => MenuItem != null ? MenuItem.Stepper : false;
            set
            {
                if (MenuItem != null)
                    MenuItem.Stepper = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditPromptForCustom)));
            }
        }



        /// <MetaDataID>{1de6c3c5-0f19-4d7d-8f04-ec3a6ddcfa31}</MetaDataID>
        public bool UnTranslatedName
        {
            get
            {
                string name = Name;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return name != Name;
                }
            }
        }

        /// <MetaDataID>{617bef95-81ad-4551-9e3e-76b7b1563a38}</MetaDataID>
        public bool UnTranslatedFullName
        {
            get
            {
                string fullName = FullName;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return fullName != FullName;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        ITranslator _Translator;
        /// <MetaDataID>{56097b50-8be4-4397-8a8b-8309c42108a0}</MetaDataID>
        public ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new Translator();
                return _Translator;
            }
        }

        /// <MetaDataID>{a21d52e0-2741-4b30-9cfd-69f78ca62bab}</MetaDataID>
        public bool UnTranslatedExtrasDescription
        {
            get
            {
                string extrasDescription = ExtrasDescription;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return extrasDescription != ExtrasDescription;
                }
            }
        }

        /// <MetaDataID>{f2e6aa8c-05ff-4eba-8066-f36bee8a2eb2}</MetaDataID>
        public bool UnTranslatedPromptForDefault
        {
            get
            {
                string promptForDefault = PromptForDefault;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return promptForDefault != PromptForDefault;
                }
            }
        }

        /// <MetaDataID>{35e0af0e-3685-4cb7-a64a-a5c84e78db1c}</MetaDataID>
        public bool UnTranslatedPromptForCustom
        {
            get
            {
                string promptForCustom = PromptForCustom;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return promptForCustom != PromptForCustom;
                }
            }
        }



        /// <MetaDataID>{315cccc9-5ee9-43ad-9e1d-5011c8b61409}</MetaDataID>
        public bool UnTranslatedDescription
        {
            get
            {
                string description = Description;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return description != Description;
                }
            }
        }




        /// <MetaDataID>{45dbeac1-63e3-45ea-9469-caa5ce4f4e93}</MetaDataID>
        WebBrowserOverlay HtmlView;
        /// <MetaDataID>{0f7ea432-938d-4efe-b167-e80a7ddee8c4}</MetaDataID>
        internal void SetHtmlView(WebBrowserOverlay browser)
        {
            HtmlView = browser;
        }

        /// <MetaDataID>{3b6cf208-320d-4a81-bd61-0f1e1e26abf7}</MetaDataID>
        ViewModelWrappers<MenuModel.IMenuItemType, MenuItemTypeViewModel> EditableMenuTypes = new ViewModelWrappers<MenuModel.IMenuItemType, MenuItemTypeViewModel>();


        /// <MetaDataID>{50bdb499-b72c-442b-b728-18ecd4327556}</MetaDataID>
        public MenuItemTypeViewModel EditableMenuItemType
        {
            get
            {
                if (SelectedMenuType == null)
                    return null;
                return EditableMenuTypes.GetViewModelFor(SelectedMenuType, SelectedMenuType, this, MenuItem.DedicatedType == SelectedMenuType);
            }
        }
        /// <MetaDataID>{206e274a-bcaf-462b-a2d2-6b8914eb4415}</MetaDataID>
        bool CanEditSelectedOptionsType()
        {
            return true;
        }

        /// <MetaDataID>{7b4e8cf0-0de6-46a2-970c-d0071b9107e6}</MetaDataID>
        bool CanDeleteSelectedOptionsType()
        {
            if (SelectedMenuType == null)
                return false;

            if (SelectedMenuType == MenuItem.DedicatedType)
                return false;

            else
                return true;
        }

        /// <MetaDataID>{52494023-ddde-44bb-be5e-4b5974b74f1c}</MetaDataID>
        public void Maximazed(PreparationOptionViewModel preparationOptionViewModel)
        {

        }

        /// <MetaDataID>{c3c6cb3a-ae47-46fa-b0a0-b94097ed3883}</MetaDataID>
        public void PreparationOptionChanged(PreparationOptionViewModel preparationOptionViewModel)
        {

        }
        /// <MetaDataID>{47d5ddea-0411-467c-b58c-e4f101063460}</MetaDataID>
        public void Minimized(PreparationOptionViewModel preparationOptionViewModel)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionsListViewButtonsVisible)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{a008ae69-ae7a-4fc8-bed7-2eed03ab2abc}</MetaDataID>
        string _FormalName;
        /// <MetaDataID>{374f2ed0-d1f3-416f-8709-772f029632d8}</MetaDataID>
        public string FormalName
        {
            get
            {
                return _FormalName;
            }
            set
            {
                _FormalName = value;
            }
        }

        /// <MetaDataID>{025254b4-f440-4d32-8300-cf516e9b166b}</MetaDataID>
        public string Description
        {
            get
            {
                //using (new OOAdvantech.CultureContext(SelectedCulture))
                {
                    if (MenuItem != null)
                        return MenuItem.Description;
                    else
                        return "";
                }
            }
            set
            {
                if (MenuItem != null)
                {

                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MenuItem);
                    if (string.IsNullOrEmpty(objectStorage.StorageMetaData.Culture))
                        objectStorage.StorageMetaData.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;

                    //using (new OOAdvantech.CultureContext(SelectedCulture))
                    {
                        MenuItem.Description = value;
                    }

                    var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                    var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedDescription)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                        }
                    }));
                }
            }
        }

        /// <MetaDataID>{32f4cbf8-acd9-410c-84b6-aa9ac5b466ec}</MetaDataID>
        public string ExtrasDescription
        {
            get
            {
                if (MenuItem != null)
                {
                    //using (new OOAdvantech.CultureContext(SelectedCulture))
                    {
                        return MenuItem.ExtrasDescription;
                    }
                }
                else
                    return "";
            }
            set
            {
                if (MenuItem != null)
                {
                    //using (new OOAdvantech.CultureContext(SelectedCulture))
                    {
                        MenuItem.ExtrasDescription = value;
                    }
                    var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                    var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedExtrasDescription)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExtrasDescription)));
                        }
                    }));
                }
            }
        }



        /// <MetaDataID>{d5674ace-5e6f-486a-9597-abd66098e8b3}</MetaDataID>
        public string PromptForCustom
        {
            get
            {
                if (MenuItem != null)
                    return MenuItem.PromptForCustom;
                else
                    return "";
            }
            set
            {
                if (MenuItem != null)
                {
                    MenuItem.PromptForCustom = value;
                    var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                    var useCustomCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useCustomCultureValue))
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedPromptForCustom)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PromptForCustom)));
                        }
                    }));
                }
            }
        }

        /// <MetaDataID>{16e7e0f2-67e1-48a5-8abd-0b1bbdb20556}</MetaDataID>
        public string PromptForDefault
        {
            get
            {
                if (MenuItem != null)
                    return MenuItem.PromptForDefault;
                else
                    return "";
            }
            set
            {
                if (MenuItem != null)
                {
                    MenuItem.PromptForDefault = value;
                    var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                    var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedPromptForDefault)));
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PromptForDefault)));
                        }
                    }));
                }
            }
        }


        /// <exclude>Excluded</exclude>
        bool _MenuItemWebViewVizible = true;
        /// <MetaDataID>{7a569aa4-96b3-455a-ba88-495642d2de2b}</MetaDataID>
        public bool MenuItemWebViewVizible
        {
            set
            {
                _MenuItemWebViewVizible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItemWebViewToggleText)));
            }
        }


        /// <MetaDataID>{999faf5a-5878-4d2a-b2a7-52cb485339f2}</MetaDataID>
        public string MenuItemWebViewToggleText
        {
            get
            {
                if (_MenuItemWebViewVizible)
                    return "<";
                else
                    return ">";
            }
        }
        /// <MetaDataID>{ae41067d-fa61-45c4-8c80-da48d9b3720a}</MetaDataID>
        public string Name
        {
            get
            {
                if (MenuItem != null)
                {
                    //using (new OOAdvantech.CultureContext(SelectedCulture))
                    {
                        return MenuItem.Name;
                    }
                }
                else
                    return "";
            }
            set
            {
                if (MenuItem != null)
                {
                    //using (new OOAdvantech.CultureContext(SelectedCulture))

                    if (string.IsNullOrWhiteSpace(value))
                        MenuItem.Name = null;
                    else
                        MenuItem.Name = value;

                }

                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedName)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    }
                }));




                if (_MenuItemWebViewVizible)
                {
                    //Dictionary<object, object> mappedObject = new Dictionary<object, object>();
                    //MenuModel.JsonViewModel.MenuFoodItem ItemPreparation = new MenuModel.JsonViewModel.MenuFoodItem(MenuItem, mappedObject);// { Name = this.Name, Quantity = 1 ,ISOCurrencySymbol=RegionInfo.CurrentRegion.ISOCurrencySymbol,CurrencySymbol= RegionInfo.CurrentRegion .CurrencySymbol};
                    //                                                                                                                        //MenuModel.ItemSelectorOptionsGroup
                    //optionsGroups
                    //ItemPreparation.ItemOptions = (from menuItemType in this.MenuItemTypes
                    //                               from optionGroup in EditableMenuTypes.GetViewModelFor(menuItemType, menuItemType, this, false).Options.OfType<PreparationOptionsGroupViewModel>()
                    //                               select new JsonViewModel.OptionGroup()
                    //                               {
                    //                                   ItemSelectorOptionsGroup = optionGroup.PreparationOptionsGroup is MenuModel.ItemSelectorOptionsGroup,
                    //                                   CheckUncheck = optionGroup.PreparationOptionsGroup.SelectionType != MenuModel.SelectionType.SimpleGroup,
                    //                                   Name = optionGroup.Name,
                    //                                   Options = (from option in optionGroup.GroupedOptions
                    //                                              select JsonViewModel.Option.GetOption(option.PreparationScaledOption, mappedObject, MenuItem)).ToList()

                    //                               }).ToList();


                    //var sss= (from menuItemType in this.MenuItemTypes
                    //          from optionGroup in menuItemType.Options.OfType<MenuModel.IPreparationOptionsGroup>()
                    //          select new JsonViewModel.OptionGroup()
                    //          {
                    //              ItemSelectorOptionsGroup = optionGroup is MenuModel.ItemSelectorOptionsGroup,
                    //              CheckUncheck = optionGroup.SelectionType != MenuModel.SelectionType.SimpleGroup,
                    //              Name = optionGroup.Name,
                    //              Options = (from scaledOption in optionGroup.GroupedOptions
                    //                         select JsonViewModel.Option.GetOption(scaledOption, mappedObject, MenuItem)).ToList()

                    //          }).ToList();


                    //new JsonViewModel.Option() { Name = option.Name, Quantitative = option.Quantitative, LevelType = JsonViewModel.ScaleType.GetScaleTypeFor(option.PreparationScaledOption.LevelType), InitialLevelIndex = option.PreparationScaledOption.LevelType.Levels.IndexOf(option.PreparationScaledOption.GetInitialFor(MenuItem)) }
                    //ItemPreparation.ItemOptions = new List<JsonViewModel.OptionGroup>();
                    //string json = JsonConvert.SerializeObject(ItemPreparation, Formatting.None, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });

                    //HtmlView.InvockeJSMethod("SetViewModelData", new object[] { json });
                }
            }
        }


        /// <MetaDataID>{9c5ba7aa-99f1-45f5-983b-fe3179240e87}</MetaDataID>
        public string PageTitle
        {
            get
            {
                return string.Format("Menu item details '{0}'", Name);
            }
            set { }
        }

        /// <MetaDataID>{f16422c1-e62d-4b28-b035-a3f9b49fb2ba}</MetaDataID>
        public string FullName
        {
            get
            {
                if (MenuItem != null)
                {
                    return MenuItem.FullName;
                }
                else
                    return "";
            }
            set
            {
                if (MenuItem != null)
                {
                    MenuItem.FullName = value;
                }

                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedFullName)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
                    }
                }));

            }
        }

        /// <MetaDataID>{8b8b3713-fa0b-4d2a-a89c-53f487f31afb}</MetaDataID>
        public List<MenuModel.IMenuItemType> MenuItemTypes
        {
            get
            {
                var types = MenuItem.Types.ToList();
                return types;
            }
        }


        /// <MetaDataID>{ea803f64-2f14-4731-bd85-b7d6e05263ff}</MetaDataID>
        public RelayCommand AddTaxableTypeCommand { get; protected set; }

        /// <MetaDataID>{722fc392-9049-425d-a2c6-c7866c944dfe}</MetaDataID>
        public RelayCommand OpenMealTypesCommand { get; protected set; }

        public RelayCommand AddPartofMealCommand { get; protected set; }

        public RelayCommand DeletePartofMealCommand { get; protected set; }


        public RelayCommand SavePartofMealCommand { get; protected set; }

        /// <MetaDataID>{c57fccc1-2337-4900-bed7-2c15a3f0e564}</MetaDataID>
        public RelayCommand TranslateNameCommand { get; protected set; }
        /// <MetaDataID>{12ca826b-8b2a-4a58-88fd-52e58fbb63fe}</MetaDataID>
        public RelayCommand TranslateFullNameCommand { get; protected set; }


        /// <MetaDataID>{9e51526c-165d-41d4-b15a-f211e3b0061b}</MetaDataID>
        public RelayCommand TranslateDescriptionCommand { get; protected set; }

        /// <MetaDataID>{fd009ba8-4d41-4b49-a81e-d3b9439d3325}</MetaDataID>
        public RelayCommand TranslateExtrasDescriptionCommand { get; protected set; }

        public bool NewPartOfMeal { get; set; }

        /// <MetaDataID>{9a34c0ee-7d2b-4d05-b0f4-41267e1b7ac0}</MetaDataID>
        public RelayCommand EditSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{50a25739-a329-4358-be0e-b61f130a8205}</MetaDataID>
        public RelayCommand DeleteSelectedTaxableTypeCommand { get; protected set; }
        /// <MetaDataID>{9174515a-3173-4590-a5c8-8f8b81afe4d5}</MetaDataID>
        public RelayCommand RenameSelectedTaxableTypeCommand { get; protected set; }


        /// <MetaDataID>{5603e97e-2626-4e93-b3ed-46ead4752cda}</MetaDataID>
        public RelayCommand EditSelectedOptionsTypeCommand { get; protected set; }

        /// <MetaDataID>{50d2ec9b-72bf-42b2-8d3e-bbcb74d890c4}</MetaDataID>
        public RelayCommand BeforeSaveCommand { get; protected set; }
        /// <MetaDataID>{098ea8c4-9e69-4c85-a8a3-b6d17efac12e}</MetaDataID>
        public RelayCommand DeleteSelectedOptionsTypeCommand { get; protected set; }
        /// <MetaDataID>{db36b6db-e7ba-421f-aae7-f52ed3d59a2d}</MetaDataID>
        public RelayCommand RenameSelectedOptionsTypeCommand { get; protected set; }

        /// <MetaDataID>{d1179982-57b4-4043-81be-8b35c0502a62}</MetaDataID>
        public RelayCommand RefreshWebItemViewCommand { get; protected set; }


        /// <MetaDataID>{7887b998-f72d-4cf5-ba95-8159af725578}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand AddOptionsTypeCommand { get; protected set; }



        /// <exclude>Excluded</exclude>
        MenuModel.IMenuItemType _SelectedMenuType;
        /// <MetaDataID>{9c513d19-ce7f-4121-b1aa-961c830b7155}</MetaDataID>
        public MenuModel.IMenuItemType SelectedMenuType
        {
            get
            {
                if (_SelectedMenuType == null)
                {
                    if (MenuItem.DedicatedType.Options.Count > 0)
                        return MenuItem.DedicatedType;
                    else
                    {
                        foreach (var menuItemType in MenuItemTypes)
                            if (menuItemType.Options.Count > 0)
                                return menuItemType;
                    }

                    return MenuItem.DedicatedType;
                }
                return _SelectedMenuType;
            }
            set
            {
                _SelectedMenuType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenuType)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditableMenuItemType)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnableEditMenuType)));
            }
        }


        /// <exclude>Excluded</exclude>
        TaxableTypeViewModel _SelectedTaxableType;

        /// <MetaDataID>{ff0d55fa-b8cf-4e8e-8595-6ed7d7815691}</MetaDataID>
        public TaxableTypeViewModel SelectedTaxableType
        {
            get
            {
                if (TaxableTypes != null && MenuItem is ITaxable && (MenuItem as ITaxable).TaxableType != null)
                    return _TaxableTypes[(MenuItem as ITaxable).TaxableType];
                else
                    return null;
            }
            set
            {
                if (MenuItem is ITaxable)
                {
                    if (value != null)
                        (MenuItem as ITaxable).TaxableType = value.TaxableType;
                    else
                        (MenuItem as ITaxable).TaxableType = null;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenuType)));
            }
        }




        /// <MetaDataID>{f0aba6cf-c0a5-4106-ba35-e429f7f728e7}</MetaDataID>
        ItemSelectorViewModel _SelectedPriceContext;
        /// <MetaDataID>{2c97c331-512f-4ae1-b945-73faf73a1507}</MetaDataID>
        public ItemSelectorViewModel SelectedPriceContext
        {
            get
            {
                if (!PricingContextsLoaded)
                    LoadPricingContexts();


                return _SelectedPriceContext;
            }
            set
            {
                _SelectedPriceContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridePrice)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OrgPriceToolTip)));

            }
        }




        /// <MetaDataID>{cae3a270-a3f0-4e7c-bc5a-0682d35bfc83}</MetaDataID>
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


        /// <MetaDataID>{47be2816-737b-46e9-b7e7-3f098d275254}</MetaDataID>
        ViewModelWrappers<MenuModel.ItemSelectorOption, ItemSelectorViewModel> PriceContextDitionary = new ViewModelWrappers<MenuModel.ItemSelectorOption, ItemSelectorViewModel>();
        /// <MetaDataID>{05283185-e281-4dfc-93cf-30009a2b6498}</MetaDataID>
        public virtual List<ItemSelectorViewModel> PricingContexts
        {
            get
            {
                if (!PricingContextsLoaded)
                    LoadPricingContexts();

                MenuModel.IMenuItemType menuItemTypeWithPriceContexts = (from menuItemType in MenuItem.Types where menuItemType.PricingContexts.Count > 0 select menuItemType).FirstOrDefault();
                if (menuItemTypeWithPriceContexts == null)
                    return new List<ItemSelectorViewModel>();
                return (from itemSelectorOption in menuItemTypeWithPriceContexts.PricingContexts.OfType<MenuModel.ItemSelectorOption>()
                        select PriceContextDitionary.GetViewModelFor(itemSelectorOption, itemSelectorOption, this, false)).ToList();
            }
        }




        /// <MetaDataID>{e564b919-de25-428d-be13-92ccb6babda6}</MetaDataID>
        decimal _OverridePrice = -1;
        /// <MetaDataID>{ca31bd79-e0bc-4887-b552-b260975fdaae}</MetaDataID>
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


                _OverridePrice = value;
            }
        }

        /// <exclude>Excluded</exclude>
        List<PartOfMealViewModel> _PartsOfMeals;

        readonly ViewModelWrappers<MenuModel.IPartofMeal, PartOfMealViewModel> PartOfMealTypeDictionary = new ViewModelWrappers<MenuModel.IPartofMeal, PartOfMealViewModel>();
        public List<PartOfMealViewModel> PartsOfMeals
        {
            get
            {
                if (_PartsOfMeals == null)
                    _PartsOfMeals = (from partOfMeal in MenuItem.PartofMeals select PartOfMealTypeDictionary.GetViewModelFor(partOfMeal, partOfMeal, this)).ToList();

                return _PartsOfMeals;

            }
        }

        /// <exclude>Excluded</exclude>
        bool _PartsOfMealExpanded;
        public bool PartsOfMealExpanded { get => _PartsOfMealExpanded; set => _PartsOfMealExpanded = value; }


        public string PartsOfMealText
        {
            get
            {
                string partsOfMealText = "";
                foreach (var partOfMeal in PartsOfMeals)
                {
                    if (!string.IsNullOrWhiteSpace(partsOfMealText))
                        partsOfMealText += ", ";
                    partsOfMealText += partOfMeal.PartofMeal.MealType.Name;
                }

                partsOfMealText = partsOfMealText.Trim();
                return partsOfMealText;

            }
        }


        PartOfMealViewModel _SelectedPartOfMeal;
        public PartOfMealViewModel SelectedPartOfMeal
        {
            get
            {
                return _SelectedPartOfMeal;
            }
            set
            {
                _SelectedPartOfMeal = value;
            }
        }




        public List<MealTypeViewModel> MealTypes
        {
            get
            {
                var isPartofMealsList = MenuItem.PartofMeals.Select(x => x.MealType).ToList();

                return MealTypesViewModel.MealTypes.Where(x => !isPartofMealsList.Contains(x.MealType)).ToList();
            }
        }

        MealTypeViewModel _SelectedMealTypeViewModel;
        public MealTypeViewModel SelectedMealTypeViewModel
        {
            get
            {
                return _SelectedMealTypeViewModel;
            }
            set
            {
                if (_SelectedMealTypeViewModel != value)
                {
                    _SelectedMealTypeViewModel = value;
                    _SelectedMealCourseType = _SelectedMealTypeViewModel.Courses[0];
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMealTypeCourses)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMealCourseType)));
                }
            }

        }


        public List<MealCourseTypeViewModel> SelectedMealTypeCourses
        {
            get
            {
                return SelectedMealTypeViewModel.Courses;
            }
        }

        MealCourseTypeViewModel _SelectedMealCourseType;
        public MealCourseTypeViewModel SelectedMealCourseType
        {
            get
            {
                return _SelectedMealCourseType;
            }
            set
            {
                _SelectedMealCourseType = value;
            }
        }




        /// <MetaDataID>{77430558-db6c-4427-8a82-c649dffa0a5e}</MetaDataID>
        public decimal Price
        {
            get
            {
                if (SelectedPriceContext != null)
                    return SelectedPriceContext.PreparationScaledOption.Price;
                else
                    return MenuItem.MenuItemPrice.Price;

            }
            set
            {

                if (Price == value)
                    return;


                if (SelectedPriceContext != null)
                    SelectedPriceContext.Price = value;
                else
                    MenuItem.MenuItemPrice.Price = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridePrice)));

            }
        }

        /// <MetaDataID>{9d11aadd-9d00-4570-9f7c-8151eb58624b}</MetaDataID>
        public virtual string OrgPriceToolTip
        {
            get
            {
                decimal orgPrice = 0;

                if (SelectedPriceContext != null)
                    orgPrice = SelectedPriceContext.OrgPrice;


                return Properties.Resources.OrgPriceLabel + " " + orgPrice.ToString("C");
            }
        }





        /// <exclude>Excluded</exclude>
        PreparationOptionViewModel _SelectedOption;

        /// <MetaDataID>{6cd25218-dfd5-4a35-83ba-74e9e5808958}</MetaDataID>
        public PreparationOptionViewModel SelectedOption
        {
            get
            {
                return _SelectedOption;
            }
            set
            {
                _SelectedOption = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedOption)));
            }
        }


        /// <MetaDataID>{7cdae14c-b6e8-4a12-9f0b-bed5d2e6f7ec}</MetaDataID>
        public bool IsEnableEditMenuType
        {
            get
            {
                if (EditableMenuItemType != null)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{b04717f0-aa9d-431c-b64b-ae16a0f6952b}</MetaDataID>
        public Visibility OptionsListViewButtonsVisible
        {
            get
            {
                return Visibility.Visible;
            }
        }
    }




}
