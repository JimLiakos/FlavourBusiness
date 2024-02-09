using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using FlavourBusinessFacade;
using FlavourBusinessToolKit;
using FloorLayoutDesigner.ViewModel;
using MenuDesigner.ViewModel.MenuCanvas;
using FLBManager.Properties;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;
using FLBAuthentication.ViewModel;
using MenuDesigner.ViewModel;
using System.Windows.Media.Imaging;
using OOAdvantech.PersistenceLayer;
using MenuItemsEditor;
using FLBManager.ViewModel.Taxes;

using MenuItemsEditor.Views;
using MenuPresentationModel.MenuStyles;

namespace FLBManager.ViewModel
{



    /// <MetaDataID>{9754d9d6-effc-4d7b-8aef-53d891afe61b}</MetaDataID>
    public class FlavourBusinessManagerViewModel : MarshalByRefObject, INotifyPropertyChanged, MenuItemsEditor.ViewModel.IMenusStyleSheets
    {


        public Visibility LastRowOfResourcesAreaVisibility
        {
            get
            {
                if (ActivePageGraphicMenu != null || ActivePageHallLayout != null)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

        }
        public Visibility ToolBarVisibility
        {
            get
            {
                if (MenuDesignerToolBarVisibility == Visibility.Visible ||
                    HallLayoutDesignerToolBarVisibility == Visibility.Visible ||
                    ZoomPercentageVisibility == Visibility.Visible)
                {
                    return Visibility.Visible;
                }
                else
                    return Visibility.Collapsed;
            }
        }
        public Visibility MenuDesignerToolBarVisibility
        {
            get
            {
                MenuDesigner.Views.MenuDesignerPage menuDesignerPage = null;
                if (PageDialogFrame != null)
                {
                    var pages = PageDialogFrame.Pages;
                    if (pages.Count > 0 && pages.Last() is MenuDesigner.Views.MenuDesignerPage)
                        menuDesignerPage = pages.Last() as MenuDesigner.Views.MenuDesignerPage;
                }


                if (menuDesignerPage != null)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility HallLayoutDesignerToolBarVisibility
        {
            get
            {
                if (ActivePageHallLayout != null)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }


        readonly MenuCommand RestaurantMenusMenu;

        List<MenuCommand> _MenuItems;
        public List<MenuCommand> MenuItems
        {
            get
            {
                if (_MenuItems == null)
                {
                    _MenuItems = new List<MenuCommand>();


                    var ss = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/OpenFolder.png"));
                    var sa = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/GenericDocument.png"));

                    var cmd = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.A, ModifierKeys.Control) }, (object seneder) =>
                   {
                   });

                    cmd.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));
                    if (RestaurantMenusMenu != null)
                        _MenuItems.Add(RestaurantMenusMenu);


                    ProductAndServicesCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => this.ShowProductAndServices());
                    var menuItem = new WPFUIElementObjectBind.MenuCommand()
                    {
                        Header = Properties.Resources.OrganizationMenuItemHeader,
                        SubMenuCommands = new List<MenuCommand>()
                        {
                            new MenuCommand(){
                                Header=Properties.Resources.RestaurandProductsMenuItemHeader,
                                Command=ProductAndServicesCommand

                            },

                        }

                    };
                    _MenuItems.Add(menuItem);

                    if (MenuDesignerToolBarVisibility == Visibility.Visible)
                    {
                        foreach (var menuDesignerMenuItem in ActivePageGraphicMenu.MenuItems)
                            _MenuItems.Add(menuDesignerMenuItem);

                    }
                }
                return _MenuItems;

            }
        }
        public void UpdateCurrentMenuItemEditorMenus()
        {

        }

        private void ShowProductAndServices()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                Window win = Window.GetWindow(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
                var window = new MenuItemsEditor.Views.MenuItemsWindow(RestaurantMenus);
                window.Owner = win;
                window.ShowDialog();
                stateTransition.Consistent = true;
            }

        }




        //private void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        //{
        //    if (SelectionDesignerCanvas != null)
        //        SelectionDesignerCanvas.AlignBottom_Executed(sender, e);
        //}

        public static FlavourBusinessManagerViewModel Current;
        public FlavourBusinessManagerViewModel()
        {
            _SignInUserPopup = new SignInUserPopupViewModel(RoleType.Organization);
            _SignInUserPopup.SignedIn += SignInUserPopup_SignedIn;
            _SignInUserPopup.SignedOut += SignInUserPopup_SignedOut;
            Current = this;
            //InitCommands();
            ClickPseudoCommand = new RelayCommand((object sender) => { });

            MenuDesigner = new MenuDesignerHost(this);
            HallLayoutDesigner = new HallLayoutDesignerHost(this);
        }
        public FlavourBusinessManagerViewModel(MenuCommand restaurantMenusMenu) : this()
        {
            RestaurantMenusMenu = restaurantMenusMenu;
        }
        MenuDesignerHost MenuDesigner;
        HallLayoutDesignerHost HallLayoutDesigner;
        public RelayCommand ClickPseudoCommand { get; protected set; }

        string _DesignAreaHeader = Properties.Resources.GraphicMenuTitle;
        public string DesignAreaHeader
        {
            get
            {
                return _DesignAreaHeader;
            }
            set
            {
                _DesignAreaHeader = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesignAreaHeader)));
            }
        }


        //HallLayoutViewModel _HallLayout;
        //public HallLayoutViewModel HallLayout
        //{
        //    get
        //    {
        //        return _HallLayout;
        //    }
        //    set
        //    {
        //        if (_HallLayout != value)
        //        {
        //            if (_HallLayout != null)
        //            {
        //                _HallLayout.PropertyChanged -= HallLayout_PropertyChanged;
        //                foreach (var hallLayoutDesignerMenuItem in _HallLayout.MenuItems)
        //                {
        //                    if (_MenuItems.Contains(hallLayoutDesignerMenuItem))
        //                        _MenuItems.Remove(hallLayoutDesignerMenuItem);
        //                }
        //            }

        //            _HallLayout = value;
        //            if (_HallLayout != null)
        //            {

        //                _HallLayout.PropertyChanged += HallLayout_PropertyChanged;
        //                DesignAreaHeader = _HallLayout.HallLayout.Name;
        //            }
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayout)));
        //        }
        //    }
        //}

        internal void ShowHallLayout(ServiceAreaPresentation serviceAreaPresentation)//HallLayout restaurantHallLayout)
        {


            System.Windows.Window win = System.Windows.Window.GetWindow(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            var pageDialogFrame = WPFUIElementObjectBind.ObjectContext.FindChilds<StyleableWindow.PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                MenuItemsEditor.ViewModel.MealTypesViewModel mealTypesViewModel = new MenuItemsEditor.ViewModel.MealTypesViewModel(ObjectStorage.GetStorageOfObject(RestaurantMenus.Menus[0]));

                //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(RestaurantMenus.Menus[0]));

                //var mealTypes = (from mealType in storage.GetObjectCollection<MenuModel.IMealType>()
                //                 select mealType).ToList();


                serviceAreaPresentation.MealTypesViewModel = mealTypesViewModel;

                var hallLayout = new HallLayoutViewModel(serviceAreaPresentation);

                var hallLayoutDesignerPage = new FloorLayoutDesigner.HallLayoutDesignerPage();
                hallLayoutDesignerPage.GetObjectContext().SetContextInstance(hallLayout);
                pageDialogFrame.ShowDialogPage(hallLayoutDesignerPage);
                stateTransition.Consistent = true;
            }


            //var mainWindow = System.Windows.Window.GetWindow(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject) as MainWindow;
            //(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.FrameworkElement).GetObjectContext().RunUnderContextTransaction(new Action(() =>
            //{
            //    if (serviceAreaPresentation.RestaurantHallLayout == null)
            //        HallLayout = null;
            //    if (HallLayout == null || HallLayout.HallLayout != serviceAreaPresentation.RestaurantHallLayout)
            //        HallLayout = new HallLayoutViewModel(serviceAreaPresentation);

            //    HallLayoutVisibility = Visibility.Visible;
            //    MenuDesignerVisibility = Visibility.Hidden;
            //    UpdateMenuAndToolBar();
            //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutCanvas)));
            //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutDesignerToolBar)));
            //    ////PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesignerToolBar)));
            //}));
        }

        StyleableWindow.PageDialogFrame _PageDialogFrame;
        StyleableWindow.PageDialogFrame PageDialogFrame
        {
            get
            {
                if (_PageDialogFrame == null)
                {
                    if (ClickPseudoCommand?.UserInterfaceObjectConnection?.ContainerControl is System.Windows.DependencyObject)
                    {
                        System.Windows.Window win = System.Windows.Window.GetWindow(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                        var pageDialogFrame = WPFUIElementObjectBind.ObjectContext.FindChilds<StyleableWindow.PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                        if (pageDialogFrame != null)
                        {
                            _PageDialogFrame = pageDialogFrame;
                            _PageDialogFrame.ObjectChange += PageDialogFrame_ObjectChange;
                        }
                    }
                }
                return _PageDialogFrame;

            }
        }

        private void PageDialogFrame_ObjectChange(object _object, string member)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {


                UpdateMenuAndToolBar();
                if (RootPageGraphicMenu == null)
                    MenuData.GraphicMenu = null;
                else
                    MenuData.GraphicMenu = OOAdvantech.UserInterface.Runtime.UIProxy.GetRealObject<BookViewModel>(RootPageGraphicMenu).RealObject;


                stateTransition.Consistent = true;
            }


            //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutDesignerToolBar)));
            //    ////PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesignerToolBar)));

        }


        List<MenuCommand> ActivePageMenuItems = new List<MenuCommand>();
        /// <summary>
        /// Refresh menu bar and tool bar
        /// </summary>
        internal void UpdateMenuAndToolBar()
        {

            if (ActivePageMenuItemViewModel != null)
                ActivePageMenuItemViewModel.PropertyChanged -= MenuItemViewModel_PropertyChanged;

            if (_MenuItems != null)
            {
                foreach (var menuItem in ActivePageMenuItems)
                {
                    if (_MenuItems.Contains(menuItem))
                        _MenuItems.Remove(menuItem);
                }
            }
            ActivePageMenuItems.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutDesignerToolBarVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToolBarVisibility)));

            var pages = PageDialogFrame.Pages;
        
      
            #region graphic menu page menu command


            MenuDesigner.Views.MenuDesignerPage menuDesignerPage = null;
            if (pages.Count > 0 && pages.Last() is MenuDesigner.Views.MenuDesignerPage)
                menuDesignerPage = pages.Last() as MenuDesigner.Views.MenuDesignerPage;


            if (MenuDesignerToolBarVisibility == Visibility.Visible)
            {
                if (ActivePageGraphicMenu != null)
                {
                    // add menu designer menu items 
                    foreach (var menuItem in ActivePageGraphicMenu.MenuItems)
                    {
                        if (!_MenuItems.Contains(menuItem))
                        {
                            _MenuItems.Add(menuItem);
                            ActivePageMenuItems.Add(menuItem);
                        }
                    }
                }
            }
            #endregion


            #region Menu Item Page menu command

            MenuItemPage menuItemPage = null;
            if (pages.Count > 0 && pages.Last() is MenuItemPage)
                menuItemPage = pages.Last() as MenuItemPage;


            if (menuItemPage != null)
            {
                ActivePageMenuItemViewModel = menuItemPage.MenuItemView.GetDataContextObject() as MenuItemsEditor.ViewModel.MenuItemViewModel;


                if (ActivePageMenuItemViewModel != null)
                {
                    ActivePageMenuItemViewModel.PropertyChanged += MenuItemViewModel_PropertyChanged;
                    var menuItem = ActivePageMenuItemViewModel.FontsMenu;
                    if (menuItem != null)
                    {
                        _MenuItems.Add(menuItem);
                        ActivePageMenuItems.Add(menuItem);
                    }
                     menuItem = ActivePageMenuItemViewModel.DesignMenu;
                    if (menuItem != null)
                    {
                        _MenuItems.Add(menuItem);
                        ActivePageMenuItems.Add(menuItem);
                    }

                }
            }
            #endregion



            #region hall layout page menu command
            if (HallLayoutDesignerToolBarVisibility == Visibility.Visible)
            {
                if (ActivePageHallLayout != null)
                {
                    // add hallLayout designer menu items 
                    foreach (var menuItem in ActivePageHallLayout.MenuItems)
                    {
                        if (!_MenuItems.Contains(menuItem))
                        {
                            _MenuItems.Add(menuItem);
                            ActivePageMenuItems.Add(menuItem);
                        }
                    }
                }
            }

            #endregion



            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItems)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuDesignerToolBarVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToolBarVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastRowOfResourcesAreaVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivePageGraphicMenu)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivePageHallLayout)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaledArea)));



        }

        private void MenuItemViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FontsMenu")
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    UpdateMenuAndToolBar();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItems))); 
                    stateTransition.Consistent = true;
                }

            }
        }

        Visibility _MenuDesignerVisibility = Visibility.Visible;
        public System.Windows.Visibility MenuDesignerVisibility
        {
            get
            {
                return _MenuDesignerVisibility;
            }
            set
            {
                _MenuDesignerVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuDesignerVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuDesignerToolBarVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToolBarVisibility)));
            }
        }


        System.Windows.Visibility _HallLayoutVisibility = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility HallLayoutVisibility
        {
            get
            {
                return _HallLayoutVisibility;
            }
            set
            {
                _HallLayoutVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HallLayoutDesignerToolBarVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToolBarVisibility)));
            }
        }


        /// <exclude>Excluded</exclude>
        RestaurantMenuItemsPresentation _MenuData;
        public RestaurantMenuItemsPresentation MenuData
        {
            get
            {
                return _MenuData;
            }
            set
            {
                _MenuData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuData)));
            }
        }

        public MenuDesigner.Views.MenuDesignerPage ActivemenuDesignerPage
        {
            get
            {
                MenuDesigner.Views.MenuDesignerPage menuDesignerPage = null;
                if (PageDialogFrame != null)
                {
                    var pages = PageDialogFrame.Pages;
                    if (pages.Count > 0 && pages.Last() is MenuDesigner.Views.MenuDesignerPage)
                        menuDesignerPage = pages.Last() as MenuDesigner.Views.MenuDesignerPage;
                }
                return menuDesignerPage;

            }
        }

        public BookViewModel ActivePageGraphicMenu
        {
            get
            {
                MenuDesigner.Views.MenuDesignerPage menuDesignerPage = null;
                if (PageDialogFrame != null)
                {
                    var pages = PageDialogFrame.Pages;
                    if (pages.Count > 0 && pages.Last() is MenuDesigner.Views.MenuDesignerPage)
                        menuDesignerPage = pages.Last() as MenuDesigner.Views.MenuDesignerPage;
                }
                if (menuDesignerPage != null)
                {
                    var menu = menuDesignerPage.GetObjectContext().CrossSessionValue as BookViewModel;
                    return menu;
                }
                else
                    return null;
            }
        }
        public BookViewModel RootPageGraphicMenu
        {
            get
            {
                MenuDesigner.Views.MenuDesignerPage menuDesignerPage = null;
                if (PageDialogFrame != null)
                {
                    var pages = PageDialogFrame.Pages;
                    if (pages.Count > 0 && pages.First() is MenuDesigner.Views.MenuDesignerPage)
                        menuDesignerPage = pages.First() as MenuDesigner.Views.MenuDesignerPage;
                }
                if (menuDesignerPage != null)
                {
                    var menu = menuDesignerPage.GetObjectContext().CrossSessionValue as BookViewModel;
                    return menu;
                }
                else
                    return null;
            }

        }

        public HallLayoutViewModel ActivePageHallLayout
        {
            get
            {
                FloorLayoutDesigner.HallLayoutDesignerPage hallLayoutDesignerPage = null;
                if (PageDialogFrame != null)
                {
                    var pages = PageDialogFrame.Pages;
                    if (pages.Count > 0 && pages[0] is FloorLayoutDesigner.HallLayoutDesignerPage)
                        hallLayoutDesignerPage = pages[0] as FloorLayoutDesigner.HallLayoutDesignerPage;
                }
                if (hallLayoutDesignerPage != null)
                {
                    var crossSessionHallLayout = hallLayoutDesignerPage.GetObjectContext().CrossSessionValue as HallLayoutViewModel;
                    return crossSessionHallLayout;
                }
                else
                    return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        ///// <exclude>Excluded</exclude>
        //BookViewModel _GraphicMenu;
        //public BookViewModel GraphicMenu
        //{
        //    get
        //    {
        //        MenuDesigner.Views.MenuDesignerPage menuDesignerPage = null;
        //        if (PageDialogFrame != null)
        //        {
        //            var pages = PageDialogFrame.Pages;
        //            if (pages.Count > 0 && pages[0] is MenuDesigner.Views.MenuDesignerPage)
        //                menuDesignerPage = pages[0] as MenuDesigner.Views.MenuDesignerPage;
        //        }
        //        if (menuDesignerPage != null && menuDesignerPage.GetDataContextObject<BookViewModel>() !=null)
        //            GraphicMenu = menuDesignerPage.GetDataContextObject<BookViewModel>();
        //        else
        //            GraphicMenu = null;


        //        return _GraphicMenu;
        //    }
        //    set
        //    {
        //        if (_GraphicMenu != value)
        //        {
        //            if (_GraphicMenu != null)
        //            {
        //                foreach (var menuDesignerMenuItem in _GraphicMenu.MenuItems)
        //                {
        //                    if (MenuItems.Contains(menuDesignerMenuItem))
        //                        _MenuItems.Remove(menuDesignerMenuItem);
        //                }
        //            }
        //            if (_GraphicMenu != null)
        //                _GraphicMenu.PropertyChanged -= Menu_PropertyChanged;

        //            _GraphicMenu = value;
        //            if (_GraphicMenu != null)
        //                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItems)));

        //            if (_GraphicMenu != null)
        //                _GraphicMenu.PropertyChanged += Menu_PropertyChanged;


        //            if (_GraphicMenu != null)
        //                _MenuData.GraphicMenu =  value.RealObject;
        //            else
        //                _MenuData.GraphicMenu = null;

        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GraphicMenu)));
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageLabel)));
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentage)));
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageVisibility)));
        //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStyleName)));
        //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageAttributeVisibility)));
        //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuDesignerToolBar)));
        //            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesignerToolBar)));
        //        }
        //    }
        //}

        public MenuItemsEditor.RestaurantMenus RestaurantMenus;






        #region Designer zoom
        public Visibility ZoomPercentageVisibility
        {
            get
            {
                if ((ActivePageGraphicMenu != null && MenuDesignerToolBarVisibility == Visibility.Visible) || (ActivePageHallLayout != null && HallLayoutDesignerToolBarVisibility == Visibility.Visible))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public FlavourBusinessUI.ViewModel.IScaledArea ScaledArea
        {
            get
            {
                if (ActivePageGraphicMenu != null)
                    return ActivePageGraphicMenu;
                if (ActivePageHallLayout != null)
                    return ActivePageHallLayout;

                return null;
            }
        }

        #endregion

        #region Authentication
        /// <exclude>Excluded</exclude>
        SignInUserPopupViewModel _SignInUserPopup;
        public SignInUserPopupViewModel SignInUser
        {
            get
            {
                return _SignInUserPopup;
            }
            set
            {
                if (_SignInUserPopup != null)
                    _SignInUserPopup.SignedIn -= SignInUserPopup_SignedIn;
                _SignInUserPopup = value;

                if (_SignInUserPopup != null)
                    _SignInUserPopup.SignedIn += SignInUserPopup_SignedIn;
            }
        }
        public void GetOrgenizationRestMenus(IResourceManager resourceManager)
        {
            string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
            if (!System.IO.Directory.Exists(appDataPath))
                System.IO.Directory.CreateDirectory(appDataPath);
            appDataPath += "\\DontWaitWater";
            if (!System.IO.Directory.Exists(appDataPath))
                System.IO.Directory.CreateDirectory(appDataPath);


            OrganizationStorageRef storageRef = resourceManager.GetStorage(OrganizationStorages.RestaurantMenus);
            string temporaryStorageLocation = appDataPath + string.Format("\\{0}RestaurantMenuData.xml", storageRef.StorageIdentity.Replace("-", ""));
            HttpClient httpClient = new HttpClient();
            var dataStreamTask = httpClient.GetStreamAsync(storageRef.StorageUrl);
            dataStreamTask.Wait();
            var dataStream = dataStreamTask.Result;
            RawStorageData storageData = new RawStorageData(XDocument.Load(dataStream), temporaryStorageLocation, storageRef, resourceManager as IUploadService);

            RestaurantMenus = new MenuItemsEditor.RestaurantMenus(storageData);
            _BusinessResources.RestaurantMenus = RestaurantMenus;


            MenuData = new RestaurantMenuItemsPresentation((RestaurantMenus.Members[0] as MenuItemsEditor.ViewModel.MenuViewModel).Menu, this, null);
            MenuData.ShowMenuTaxes += MenuData_ShowMenuTaxes;
            OrganizationStorageRef styleSeetStorageRef = resourceManager.GetStorage(OrganizationStorages.StyleSheets);
            temporaryStorageLocation = appDataPath + "\\StyleSheets.xml";
            RawStorageData styleSeetStorageData = new RawStorageData(temporaryStorageLocation, styleSeetStorageRef, null);
            var styleSeetObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("StyleSheets", styleSeetStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
            MenuPresentationModel.MenuStyles.StyleSheet.ObjectStorage = styleSeetObjectStorage;


            OrganizationStorageRef backgroundImagesStorageRef = resourceManager.GetStorage(OrganizationStorages.BackgroundImages);
            temporaryStorageLocation = appDataPath + "\\BackgroundImages.xml";
            RawStorageData backgroundImagesStorageData = new RawStorageData(temporaryStorageLocation, backgroundImagesStorageRef, null);
            var backgroundImagesObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("BackgroundImages", backgroundImagesStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

            DateTime dateTimeNow = DateTime.UtcNow;

            BackgroundSelectionViewModel.BackgroundImagesStorage = backgroundImagesObjectStorage;

            OrganizationStorageRef bordersStorageRef = resourceManager.GetStorage(OrganizationStorages.Borders);
            temporaryStorageLocation = appDataPath + "\\Borders.xml";
            RawStorageData bordersStorageData = new RawStorageData(temporaryStorageLocation, bordersStorageRef, null);
            var BordersObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("Borders", bordersStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

            BorderSelectionViewModel.BordersStorage = BordersObjectStorage;


            OrganizationStorageRef headingAccentsStorageRef = resourceManager.GetStorage(OrganizationStorages.HeadingAccents);
            temporaryStorageLocation = appDataPath + "\\HeadingAccents.xml";
            RawStorageData headingAccentsStorageData = new RawStorageData(temporaryStorageLocation, headingAccentsStorageRef, null);
            var headingAccentsObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("HeadingAccents", headingAccentsStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(headingAccentsObjectStorage);




        }

        private void MenuData_ShowMenuTaxes(MenuModel.IMenu menu)
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                try
                {
                    System.Windows.Window win = System.Windows.Window.GetWindow(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                    var window = new Views.Taxes.TaxesWindow();
                    window.Owner = win;


                    window.GetObjectContext().SetContextInstance(new TaxesPresentation(menu.TaxAuthority, this.RestaurantMenus.Menus[0]));
                    if (window.ShowDialog().Value)
                    {

                    }
                    stateTransition.Consistent = true;
                }
                catch (Exception error)
                {
                }
            }
        }
        private async void SignInUserPopup_SignedIn(SignInUserPopupViewModel authedication, IUser user)
        {

            var role = user.Roles.Where(x => x.RoleType == RoleType.Organization).FirstOrDefault();
            FlavourBusinessManager.Organization organization = null;
            if (role.RoleType == RoleType.Organization)
            {
                organization = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<FlavourBusinessManager.Organization>(role.User);
                FlavourBusinessManager.Organization.CurrentOrganization = organization;
            }

            var resourceManager = FlavourBusinessManager.Organization.CurrentOrganization as IResourceManager;
            _GraphicMenus = new GraphicMenusPresentation(resourceManager.GraphicMenus, resourceManager);

            foreach (var graphicMenu in _GraphicMenus.GraphicMenus)
                graphicMenu.StorageRef.UploadService = organization;

            _BusinessResources = new FlavourBusinessResourcesPresentation(FlavourBusinessManager.Organization.CurrentOrganization, GraphicMenus, RestaurantMenus);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BusinessResources)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivePageGraphicMenu)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaledArea)));

            await Task.Run(() =>
            {
                GetOrgenizationRestMenus(user as IResourceManager);
            });


            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                UIBaseEx.FontData.HtmlView = (Window.GetWindow(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject) as MainWindow).SignInToolBarItem.Browser;
            }));



        }
        private void SignInUserPopup_SignedOut(SignInUserPopupViewModel authedication, IUser user)
        {
            _BusinessResources?.SignOut();
            _BusinessResources = null;// new FlavourBusinessResourcesPresentation(FlavourBusinessManager.Organization.CurrentOrganization);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BusinessResources)));
        }
        #endregion

        GraphicMenusPresentation _GraphicMenus;
        public GraphicMenusPresentation GraphicMenus
        {
            get
            {
                return _GraphicMenus;
            }
        }


        FlavourBusinessResourcesPresentation _BusinessResources;
        public FlavourBusinessResourcesPresentation BusinessResources
        {
            get
            {
                //tax authority
                return _BusinessResources;
            }
        }

        public List<CultureInfo> Cultures
        {
            get
            {

                return CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
            }
        }
        public CultureInfo SelectedCulture
        {
            get
            {
                return OOAdvantech.CultureContext.CurrentCultureInfo;
            }
            set
            {
                OOAdvantech.CultureContext.CurrentCultureInfo = value;
            }
        }

        public WPFUIElementObjectBind.RoutedCommand ProductAndServicesCommand { get; private set; }

        public List<MenuItemsEditor.ViewModel.IMenuStyleSheet> StyleSheets
        {
            get
            {
                return GraphicMenus.GraphicMenus.OfType<MenuItemsEditor.ViewModel.IMenuStyleSheet>().ToList();
            }
        }

        public MenuItemsEditor.ViewModel.MenuItemViewModel ActivePageMenuItemViewModel { get; private set; }

        public class MenuDesignerHost : MenuDesigner.ViewModel.Menu.MenuDesignerHost
        {
            FlavourBusinessManagerViewModel FlavourBusinessManagerViewModel;
            public MenuDesignerHost(FlavourBusinessManagerViewModel flavourBusinessManagerViewModel)
            {
                FlavourBusinessManagerViewModel = flavourBusinessManagerViewModel;
                global::MenuDesigner.ViewModel.Menu.MenuDesignerHost.Current = this;
            }
            public override BookViewModel Menu { get => OOAdvantech.UserInterface.Runtime.UIProxy.GetRealObject<BookViewModel>(FlavourBusinessManagerViewModel.ActivePageGraphicMenu); set { } }

            public override GraphicMenusPresentation GraphicMenus => FlavourBusinessManagerViewModel.GraphicMenus;

            public override Task SaveAndPublish(OrganizationStorageRef graphicMenuStorageRef)
            {
                return Task.Run(() =>
                {
                    if (graphicMenuStorageRef.UploadService is IResourceManager)
                    {
                        if (FlavourBusinessManagerViewModel.ActivemenuDesignerPage != null)
                            FlavourBusinessManagerViewModel.ActivemenuDesignerPage.GetObjectContextConnection().Save();
                        (graphicMenuStorageRef.UploadService as IResourceManager).PublishMenu(graphicMenuStorageRef);
                    }


                });
            }
            public override void ShowOnGrpaphicMenuDesigner(BookViewModel bookViewModel, MenuItemsEditor.RestaurantMenus restaurantMenuData = null)
            {

                bookViewModel.PublishAllowed = true;
                System.Windows.Window win = System.Windows.Window.GetWindow(FlavourBusinessManagerViewModel.ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                var pageDialogFrame = WPFUIElementObjectBind.ObjectContext.FindChilds<StyleableWindow.PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();

                var menuDesignerPage = new MenuDesigner.Views.MenuDesignerPage();
                menuDesignerPage.GetObjectContext().SetContextInstance(bookViewModel);
                //bookViewModel.RunUnderTransaction = menuDesignerPage.GetObjectContextConnection().Transaction;


                pageDialogFrame.ShowDialogPage(menuDesignerPage);
                FlavourBusinessManagerViewModel.MenuData.GraphicMenu = bookViewModel.RealObject;



                //FlavourBusinessManagerViewModel.GraphicMenu = bookViewModel;
                //FlavourBusinessManagerViewModel.HallLayoutVisibility = Visibility.Hidden;
                //FlavourBusinessManagerViewModel.MenuDesignerVisibility = Visibility.Visible;
                // FlavourBusinessManagerViewModel.UpdateMenuAndToolBar();
            }
        }


        public class HallLayoutDesignerHost : FloorLayoutDesigner.HallLayoutDesignerHost
        {

            FlavourBusinessManagerViewModel FlavourBusinessManagerViewModel;
            public HallLayoutDesignerHost(FlavourBusinessManagerViewModel flavourBusinessManagerViewModel)
            {
                FlavourBusinessManagerViewModel = flavourBusinessManagerViewModel;
                FloorLayoutDesigner.HallLayoutDesignerHost.Current = this;
            }

            public override RestaurantMenus RestaurantMenus => FlavourBusinessManagerViewModel.RestaurantMenus;

            public override void ShowHallLayout(ServiceAreaPresentation serviceAreaPresentation)
            {
                FlavourBusinessManagerViewModel.ShowHallLayout(serviceAreaPresentation);
            }
        }
    }
}

