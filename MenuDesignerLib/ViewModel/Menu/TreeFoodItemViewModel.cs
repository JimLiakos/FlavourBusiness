using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OOAdvantech.Transactions;
using System.Windows;
using WPFUIElementObjectBind;
using StyleableWindow;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{2901bd92-e9cf-4e1f-b365-a560ab9eb08d}</MetaDataID>
    public class TreeFoodItemViewModel : MarshalByRefObject, MenuItemsEditor.IMenusTreeNode, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem MenuCanvasFoodItem;

        public readonly MenuModel.IMenuItem MenuItem;

        MenuPresentationModel.RestaurantMenu _GraphicMenu;
        internal MenuPresentationModel.RestaurantMenu GraphicMenu
        {
            get
            {
                return _GraphicMenu;
            }
            set
            {
                _GraphicMenu = value;
                MenuCanvasFoodItem = null;
                if (_GraphicMenu != null)
                {

                    MenuCanvasFoodItem = _GraphicMenu.MenuCanvasItems.OfType<MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem>().Where(x => x.MenuItem == MenuItem).FirstOrDefault();

                    //var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_GraphicMenu);
                    
                    //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                    //MenuCanvasFoodItem = (from menuCanvasFoodItem in storage.GetObjectCollection<MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem>()
                    //                      where menuCanvasFoodItem.MenuItem == MenuItem
                    //                      select menuCanvasFoodItem).FirstOrDefault();
                }
                else
                {

                }
                if (MenuCanvasFoodItem == null)
                {
                    MenuCanvasFoodItem = new MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem();
                    MenuCanvasFoodItem.MenuItem = MenuItem;
                }

                MenuCanvasFoodItem.ObjectChangeState += MenuCanvasFoodItem_ObjectChangeState;
                //  MenuCanvasFoodItem.Description = MenuItem.Name;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TreeImage)));

            }
        }


        public TreeFoodItemViewModel(MenuModel.IMenuItem menuItem, MenuItemsEditor.IMenusTreeNode parent, MenuPresentationModel.RestaurantMenu graphicMenu)
        {
            _GraphicMenu = graphicMenu;


            if (_GraphicMenu != null)
            {
                var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(graphicMenu);
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                MenuCanvasFoodItem = (from menuCanvasFoodItem in storage.GetObjectCollection<MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem>()
                                      where menuCanvasFoodItem.MenuItem == menuItem
                                      select menuCanvasFoodItem).FirstOrDefault();
            }
            else
            {

            }
            if (MenuCanvasFoodItem == null)
            {
                MenuCanvasFoodItem = new MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem();
                MenuCanvasFoodItem.MenuItem = menuItem;
            }

            MenuCanvasFoodItem.ObjectChangeState += MenuCanvasFoodItem_ObjectChangeState;
            // MenuCanvasFoodItem.Description = menuItem.Name;
            MenuItem = menuItem;
            Parent = parent;

            //if (menuItem.CustomPrices.Count > 0&& MenuCanvasFoodItem.Prices.Count==0)
            //{
            //    MenuPresentationModel.MenuCanvas.MenuCanvasFoodItemPrice foodItemPrice = new MenuPresentationModel.MenuCanvas.MenuCanvasFoodItemPrice();
            //    foodItemPrice.Price = menuItem.CustomPrices[0].Price;
            //    MenuCanvasFoodItem.AddFoodItemPrice(foodItemPrice);
            //}


            RenameCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                EditMode();
            });

            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();

                EditMenuItem(frame);
            });

            DeleteMenuCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                (Parent as ItemsCategoryViewModel).RemoveMenuItem(this);
            });

        }

        private void MenuCanvasFoodItem_ObjectChangeState(object _object, string member)
        {


            if (member == nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasHeading.Page))
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TreeImage)));
                }));
            }

            if (member == nameof(MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem.Description))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        private void EditMenuItem(System.Windows.Window owner)
        {
            //using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                {
                    var menuItemWindow = new Views.FoodItemWindow();
                    //new MenuItemsEditor. Views.MenuItemWindow();
                    menuItemWindow.Owner = owner;



                    FoodItemViewModel foodItemViewModel = new FoodItemViewModel(MenuCanvasFoodItem, _GraphicMenu);
                    menuItemWindow.GetObjectContext().SetContextInstance(foodItemViewModel);

                    if (menuItemWindow.ShowDialog().Value)
                        stateTransition.Consistent = true;
                }
                // suppressStateTransition.Consistent = true;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));

        }

        private void EditMenuItem(PageDialogFrame frame)
        {
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                var menuItemPage = new MenuItemsEditor.Views.MenuItemPage();
                MenuItemsEditor.ViewModel.MenuItemViewModel itemViewModel = new MenuItemsEditor.ViewModel.MenuItemViewModel(MenuCanvasFoodItem.MenuItem);
                menuItemPage.GetObjectContext().SetContextInstance(itemViewModel);

                if (frame.RootPage is Views.MenuDesignerPage)
                {
                    frame.ShowDialogPageAfter(frame.RootPage, menuItemPage);
                }
                else
                {
                    frame.ShowDialogPage(menuItemPage);
                }
                suppressStateTransition.Consistent = true;
            }

            //var foodItemPage = new Views.FoodItemPage();
            //FoodItemViewModel foodItemViewModel = new FoodItemViewModel(MenuCanvasFoodItem, _GraphicMenu);
            //foodItemPage.GetObjectContext().SetContextInstance(foodItemViewModel);
            //frame.ShowDialogPage(foodItemPage);
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));

        }

        public WPFUIElementObjectBind.RelayCommand EditCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteMenuCommand { get; protected set; }



        /// <exclude>Excluded</exclude>
        bool _Edit;
        public bool Edit
        {
            get
            {
                return _Edit;
            }
            set
            {
                _Edit = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }

        public void EditMode()
        {
            Edit = true;
            return;
            if (_Edit == true)
            {
                _Edit = !_Edit;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        }

        /// <exclude>Excluded</exclude>
        bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));



            }
        }

        public List<MenuItemsEditor.IMenusTreeNode> Members
        {
            get
            {
                return new List<MenuItemsEditor.IMenusTreeNode>();
            }
        }

        public string Name
        {
            get
            {
                return MenuItem.Name;
            }

            set
            {
                using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        MenuItem.Name = value;
                        stateTransition.Consistent = true;
                    }
                }

                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                    }
                }));
            }
        }
        //UnTranslated
        public bool UnTranslated
        {
            get
            {
                string name = Name;

                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return name != Name;
                }

                //return true;
            }
        }


        WPFUIElementObjectBind.ITranslator _Translator;
        public WPFUIElementObjectBind.ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new MenuItemsEditor.Translator();

                return _Translator;
            }
        }

        public MenuItemsEditor.IMenusTreeNode Parent { get; set; }

        public ImageSource TreeImage
        {
            get
            {
                if (MenuCanvasFoodItem.Page != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/MenuItemOnPage16.png"));
                else
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem16.png"));

            }
        }





        ///// <exclude>Excluded</exclude>
        //System.Windows.Controls.ContextMenu _ContextMenu;
        //public System.Windows.Controls.ContextMenu ContextMenu
        //{
        //    get
        //    {
        //        if (_ContextMenu == null)
        //        {

        //            _ContextMenu = new System.Windows.Controls.ContextMenu();

        //            var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
        //            var emptyImage = new Image() { Source = imageSource, Width = 16, Height = 16 };

        //            imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Delete.png"));
        //            System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
        //            menuItem.Header = Properties.Resources.DeleteMenuItemHeader;
        //            menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
        //            menuItem.Command = DeleteMenuCommand;
        //            _ContextMenu.Items.Add(menuItem);



        //            menuItem = new System.Windows.Controls.MenuItem();
        //            imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
        //            menuItem.Header = Properties.Resources.TreeNodeRenameMenuItemHeader;
        //            menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
        //            menuItem.Command = RenameCommand;

        //            _ContextMenu.Items.Add(menuItem);

        //            _ContextMenu.Items.Add(new Separator());


        //            menuItem = new System.Windows.Controls.MenuItem();
        //            imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
        //            menuItem.Header = Properties.Resources.EditObject;
        //            menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
        //            menuItem.Command = EditCommand;

        //            _ContextMenu.Items.Add(menuItem);


        //        }
        //        return _ContextMenu;


        //    }
        //}

        public bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;
        public List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Delete.png"));
                    MenuCommand menuItem = new MenuCommand();
                    menuItem.Header = Properties.Resources.DeleteMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteMenuCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;

                    _ContextMenuItems.Add(menuItem);


                }
                return _ContextMenuItems;
            }
        }






        public string CondextMenuResourceKey
        {
            get
            {
                return "MenuItemTreeNodeMenu";
            }
        }

        public bool IsEditable
        {
            get
            {
                return true;
            }
        }

        public bool IsNodeExpanded
        {
            get; set;
        }

        public List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return this.ContextMenuItems;

                else
                {
                    foreach (var subItem in this.Members)
                    {
                        var contextMenuItems = subItem.ContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }
                }
                return null;
            }
        }
    }
}
