using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MenuItemsEditor.ViewModel;
using MenuModel;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace MenuItemsEditor
{
    /// <MetaDataID>{e6e69af5-3b96-447d-8804-6edb238c65f2}</MetaDataID>
    public class RestaurantMenus : MarshalByRefObject, IMenusTreeNode,INotifyPropertyChanged
    {
        public void CollapseAll()
        {
            if (this.IsNodeExpanded)
            {
                this.IsNodeExpanded= false;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
            }

            foreach (var subNode in Members)
                subNode.CollapseAll();
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

        /// <exclude>Excluded</exclude>
        bool _IsExpanded;
        public bool IsNodeExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                _IsExpanded = value;
            }
        }

      public readonly  ObjectStorage ObjectStorage = null;
         RestaurantMenus()
        {
            _IsExpanded = true;
            string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
            if (!System.IO.Directory.Exists(appDataPath))
                System.IO.Directory.CreateDirectory(appDataPath);
            appDataPath += "\\DontWaitWater";
            if (!System.IO.Directory.Exists(appDataPath))
                System.IO.Directory.CreateDirectory(appDataPath);
            string storageLocation = appDataPath + "\\RestaurantMenuData.xml";
            try
            {
                ObjectStorage = ObjectStorage.OpenStorage("RestaurantMenuData", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
            }
            catch (StorageException error)
            {

                if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    ObjectStorage = ObjectStorage.NewStorage("RestaurantMenuData",
                                                            storageLocation,
                                                            "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                }
                else
                    throw error;
            }
            catch (Exception error)
            {
            }

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);
            //_Menus = (from menu in storage.GetObjectCollection<MenuModel.IMenu>() select new MenuViewModel(menu, this) ).ToDictionary(menuViewModel=>menuViewModel.Menu);

            var storageMenus = (from menu in storage.GetObjectCollection<MenuModel.IMenu>() select menu).ToList();
            _Menus = (from menu in storageMenus select new MenuViewModel(menu,this)).ToDictionary(menuViewModel=>menuViewModel.Menu);


            MenuModel.FixedScaleType.UpdateStorage(ObjectStorage);


            NewMenuCommand = new RelayCommand((object sender) =>
             {
                 NewMenu();
             });
        }


        public RestaurantMenus(OOAdvantech.PersistenceLayer.IRawStorageData restaurantMenu)
        {

            _IsExpanded = true;
        
            try
            {
                ObjectStorage = ObjectStorage.OpenStorage("RestaurantMenuData", restaurantMenu, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
            }
            catch (StorageException error)
            {

                if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                {
                    ObjectStorage = ObjectStorage.NewStorage("RestaurantMenuData",
                                                            restaurantMenu,
                                                            "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                }
                else
                    throw error;
            }
            catch (Exception error)
            {
            }

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage);
            //_Menus = (from menu in storage.GetObjectCollection<MenuModel.IMenu>() select new MenuViewModel(menu, this) ).ToDictionary(menuViewModel=>menuViewModel.Menu);

            var storageMenus = (from menu in storage.GetObjectCollection<MenuModel.IMenu>() select menu).ToList();
            _Menus = (from menu in storageMenus select new MenuViewModel(menu, this)).ToDictionary(menuViewModel => menuViewModel.Menu);


            FixedScaleType.UpdateStorage(ObjectStorage);
            MealType.UpdateStorage(ObjectStorage);



            NewMenuCommand = new RelayCommand((object sender) =>
            {
                NewMenu();
            });

        }


        internal void RemoveMenu(MenuModel.IMenu menu)
        {
             ObjectStorage.DeleteObject(menu);
            _Menus.Remove(menu);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private void NewMenu()
        {
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var menu = new MenuModel.Menu() { Name = Properties.Resources.MenuDefaultName };
                    ObjectStorage.CommitTransientObjectState(menu);
                    _Menus.Add(menu, new MenuViewModel(menu, this));
                    stateTransition.Consistent = true;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));

        }

        ///// <exclude>Excluded</exclude>
        //System.Windows.Controls.ContextMenu _ContextMenu;
        //public System.Windows.Controls.ContextMenu ContextMenu
        //{
        //    get
        //    {
        //        if(_ContextMenu==null)
        //        {
        //            var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Menu-add16.png"));
        //            _ContextMenu = new System.Windows.Controls.ContextMenu();
        //            System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
        //            menuItem.Header = Properties.Resources.NewMenuMenuItemHeader;
        //            menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
        //            menuItem.Command = NewMenuCommand;
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


        List<MenuCommand> _ContextMenuItems;
        public List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Menu-add16.png"));
                    _ContextMenuItems = new List<MenuCommand>();
                    MenuCommand menuItem = new MenuCommand();
                    menuItem.Header = Properties.Resources.NewMenuMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewMenuCommand;
                    _ContextMenuItems.Add(menuItem);
                }

                return _ContextMenuItems;
            }
        }


        public bool IsEditable
        {
            get
            {
                return false;
            }
        }

        Dictionary<MenuModel.IMenu, MenuViewModel> _Menus;

        public IList<MenuModel.IMenu> Menus
        {
            get
            {
                return _Menus.Keys.ToList();
            }
        }
        public System.Collections.Generic.List<IMenusTreeNode> Members
        {
            get
            {
                //return new List<IMenusTreeNode>() { new MenuViewModel(new MenuModel.Menu() { Name = "MainMenu" }), new MenuViewModel(new MenuModel.Menu() { Name = "DeliveryMenu" }) };
                return _Menus.Values.OfType<IMenusTreeNode>().ToList();
            }
        }

        public string Name
        {
            get
            {
                return Properties.Resources.MemuEditorRootTreeNodeName;
            }
            set
            {
            }
        }

        public ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/restaurant16.png"));
            }
        }
        /// <exclude>Excluded</exclude>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <exclude>Excluded</exclude>
        WPFUIElementObjectBind.RelayCommand _NewMenuCommand;
        public WPFUIElementObjectBind.RelayCommand NewMenuCommand
        {
            get
            {
                return _NewMenuCommand;
            }
            protected set
            {
                _NewMenuCommand = value;
            }
        }

        bool _Edit=false;
        public bool Edit
        {
            get
            {
                return _Edit;
            }
            set
            {
                _Edit = value;
                //if (_Edit == value)
                //{
                //    _Edit = !_Edit;
                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
                //}

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }

        public IMenusTreeNode Parent
        {
            get;
            set;
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
        public string CondextMenuResourceKey
        {
            get
            {
                return "MenuItemTreeNodeMenu";
            }
        }

     
    }
}
