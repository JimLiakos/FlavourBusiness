using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessToolKit;
using FLBAuthentication.ViewModel;
using FLBManager.ViewModel;
using MenuDesigner.ViewModel;
using MenuDesigner.ViewModel.MenuCanvas;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFUIElementObjectBind;


namespace MenuMaker.ViewModel
{
    /// <MetaDataID>{36d46a3d-8e39-4c1b-b466-ca7218cb52f1}</MetaDataID>
    public class MenuMakerViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        public MenuItemsEditor.RestaurantMenus RestaurantMenus;

        public event PropertyChangedEventHandler PropertyChanged;
        readonly MenuCommand RestaurantMenusMenu;
        public MenuMakerViewModel()
        {
            _SignInUserPopup = new SignInUserPopupViewModel(RoleType.MenuMaker);
            _SignInUserPopup.SignedIn += SignInUserPopup_SignedIn;
            _SignInUserPopup.SignedOut += SignInUserPopup_SignedOut;
            RestaurantMenusMenu = global::MenuDesigner.Views.MenuDesignerControl.RestaurantMenusMenu;
            ClickPseudoCommand = new RelayCommand((object sender) => { });
            MenuDesigner = new MenuDesignerHost(this);
        }

        public RelayCommand ClickPseudoCommand { get; protected set; }
        public MenuDesignerHost MenuDesigner { get; }

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
        IMenuMaker MenuMaker;
        private async void SignInUserPopup_SignedIn(SignInUserPopupViewModel authedication, IUser user)
        {
            IMenuMaker menuMaker = user as IMenuMaker;
            MenuMaker = menuMaker;
            Resources = new List<FBResourceTreeNode>() { new MenuMakerTreeNode(menuMaker) };

            await Task.Run(() =>
            {
                GetMenuMakingResources();
            });



        }
        public void GetMenuMakingResources()
        {

            string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
            if (!System.IO.Directory.Exists(appDataPath))
                System.IO.Directory.CreateDirectory(appDataPath);
            appDataPath += "\\DontWaitWater";
            if (!System.IO.Directory.Exists(appDataPath))
                System.IO.Directory.CreateDirectory(appDataPath);

            OrganizationStorageRef styleSeetStorageRef = MenuMaker.GetStorage(OrganizationStorages.StyleSheets);
            string temporaryStorageLocation = appDataPath + "\\StyleSheets.xml";
            RawStorageData styleSeetStorageData = new RawStorageData(temporaryStorageLocation, styleSeetStorageRef, null);
            var styleSeetObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("StyleSheets", styleSeetStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
            MenuPresentationModel.MenuStyles.StyleSheet.ObjectStorage = styleSeetObjectStorage;


            OrganizationStorageRef backgroundImagesStorageRef = MenuMaker.GetStorage(OrganizationStorages.BackgroundImages);
            temporaryStorageLocation = appDataPath + "\\BackgroundImages.xml";
            RawStorageData backgroundImagesStorageData = new RawStorageData(temporaryStorageLocation, backgroundImagesStorageRef, null);
            var backgroundImagesObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("BackgroundImages", backgroundImagesStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

            DateTime dateTimeNow = DateTime.UtcNow;

            BackgroundSelectionViewModel.BackgroundImagesStorage = backgroundImagesObjectStorage;

            OrganizationStorageRef bordersStorageRef = MenuMaker.GetStorage(OrganizationStorages.Borders);
            temporaryStorageLocation = appDataPath + "\\Borders.xml";
            RawStorageData bordersStorageData = new RawStorageData(temporaryStorageLocation, bordersStorageRef, null);
            var BordersObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("Borders", bordersStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");

            BorderSelectionViewModel.BordersStorage = BordersObjectStorage;


            OrganizationStorageRef headingAccentsStorageRef = MenuMaker.GetStorage(OrganizationStorages.HeadingAccents);
            temporaryStorageLocation = appDataPath + "\\HeadingAccents.xml";
            RawStorageData headingAccentsStorageData = new RawStorageData(temporaryStorageLocation, headingAccentsStorageRef, null);
            var headingAccentsObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("HeadingAccents", headingAccentsStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");





        }

        private void SignInUserPopup_SignedOut(SignInUserPopupViewModel authedication, IUser user)
        {

        }

        List<FBResourceTreeNode> _Resources = new List<FBResourceTreeNode>();
        public List<FBResourceTreeNode> Resources
        {
            get => _Resources;
            set
            {
                if (_Resources != value)
                {
                    _Resources = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Resources)));
                }
            }
        }
        #endregion


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
                if (ActivePageGraphicMenu == null)
                    MenuData.GraphicMenu = null;

                stateTransition.Consistent = true;
            }
        }


        public Visibility ZoomPercentageVisibility
        {
            get
            {
                if (ActivePageGraphicMenu != null)
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
                //if (ActivePageHallLayout != null)
                //    return ActivePageHallLayout;

                return null;
            }
        }

        List<MenuCommand> ActivePageMenuItems = new List<MenuCommand>();

        List<MenuCommand> _MenuItems;


        public List<MenuCommand> MenuItems
        {
            get
            {
                if (_MenuItems == null)
                {
                    _MenuItems = new List<MenuCommand>();


                    if (RestaurantMenusMenu != null)
                        _MenuItems.Add(RestaurantMenusMenu);


      

                    if (MenuDesignerToolBarVisibility == Visibility.Visible)
                    {
                        foreach (var menuDesignerMenuItem in ActivePageGraphicMenu.MenuItems)
                            _MenuItems.Add(menuDesignerMenuItem);

                    }
                }
                return _MenuItems;

            }
        }

        internal void UpdateMenuAndToolBar()
        {
            if (_MenuItems != null)
            {
                foreach (var menuItem in ActivePageMenuItems)
                {
                    if (_MenuItems.Contains(menuItem))
                        _MenuItems.Remove(menuItem);
                }
            }
            ActivePageMenuItems.Clear();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageVisibility)));

            var pages = PageDialogFrame.Pages;
            MenuDesigner.Views.MenuDesignerPage menuDesignerPage = null;
            if (pages.Count > 0 && pages.Last() is MenuDesigner.Views.MenuDesignerPage)
                menuDesignerPage = pages.Last() as MenuDesigner.Views.MenuDesignerPage;

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

    
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItems)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuDesignerToolBarVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActivePageGraphicMenu)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaledArea)));



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

    }


    public class MenuDesignerHost : MenuDesigner.ViewModel.Menu.MenuDesignerHost, MenuItemsEditor.ViewModel.IMenusStyleSheets
    {
        MenuMakerViewModel MenuMakerViewModel;
        public MenuDesignerHost(MenuMakerViewModel menuMakerViewModel)
        {
            MenuMakerViewModel = menuMakerViewModel;
            global::MenuDesigner.ViewModel.Menu.MenuDesignerHost.Current = this;
        }
        public override BookViewModel Menu { get => OOAdvantech.UserInterface.Runtime.UIProxy.GetRealObject<BookViewModel>(MenuMakerViewModel.ActivePageGraphicMenu); set { } }

        public override GraphicMenusPresentation GraphicMenus => null;

        public List<MenuItemsEditor.ViewModel.IMenuStyleSheet> StyleSheets
        {
            get
            {
                return GraphicMenus.GraphicMenus.OfType<MenuItemsEditor.ViewModel.IMenuStyleSheet>().ToList();
            }
        }

        public override Task SaveAndPublish(OrganizationStorageRef graphicMenuStorageRef)
        {
            throw new NotImplementedException();
        }

        public override void ShowOnGrpaphicMenuDesigner(BookViewModel bookViewModel, MenuItemsEditor.RestaurantMenus restaurantMenuData = null)
        {
            System.Windows.Window win = System.Windows.Window.GetWindow(MenuMakerViewModel.ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            var pageDialogFrame = WPFUIElementObjectBind.ObjectContext.FindChilds<StyleableWindow.PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();

            var menuDesignerPage = new MenuDesigner.Views.MenuDesignerPage();
            menuDesignerPage.GetObjectContext().SetContextInstance(bookViewModel);
            //bookViewModel.RunUnderTransaction = menuDesignerPage.GetObjectContextConnection().Transaction;
            MenuMakerViewModel.RestaurantMenus = restaurantMenuData;
            MenuMakerViewModel.MenuData = new RestaurantMenuItemsPresentation((restaurantMenuData.Members[0] as MenuItemsEditor.ViewModel.MenuViewModel).Menu,this, null);
            pageDialogFrame.ShowDialogPage(menuDesignerPage);
            MenuMakerViewModel.MenuData.GraphicMenu = bookViewModel.RealObject;



            //FlavourBusinessManagerViewModel.GraphicMenu = bookViewModel;
            //FlavourBusinessManagerViewModel.HallLayoutVisibility = Visibility.Hidden;
            //FlavourBusinessManagerViewModel.MenuDesignerVisibility = Visibility.Visible;
            // FlavourBusinessManagerViewModel.UpdateMenuAndToolBar();
        }
    }


}
