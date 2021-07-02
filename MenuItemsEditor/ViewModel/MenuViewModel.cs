using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OOAdvantech.PersistenceLayer;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{1fc16510-b8d3-42a6-9b42-9039a599aa23}</MetaDataID>
    public class MenuViewModel :MarshalByRefObject, IMenusTreeNode, INotifyPropertyChanged
    {

        /// <MetaDataID>{912e1447-5183-4097-b7fc-abb92066c7ae}</MetaDataID>
        public bool IsNodeExpanded
        {
            get; set;
        }

        /// <MetaDataID>{70719d75-41b7-4f44-ab09-dc4bb6e3cf30}</MetaDataID>
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

        /// <MetaDataID>{5d0bc16a-ccb4-45d9-a51f-c3ef70a5d0e2}</MetaDataID>
        public readonly MenuModel.IMenu Menu;
        /// <MetaDataID>{3e90ecdb-6d77-4041-8070-ab6baf526b62}</MetaDataID>
        public MenuViewModel(MenuModel.IMenu menu, IMenusTreeNode parent)
        {
            Parent = parent;
            Menu = menu;


            RenameCommand = new RelayCommand((object sender) =>
             {
                 EditMode();
             });

            DeleteMenuCommand = new RelayCommand((object sender) =>
             {
                 Delete();

             }, (object sender) => CanDelete(sender));

            //NewCategoryCommand = new RelayCommand((object sender) =>
            //{
            //    AddCategory();
            //});
        }


        /// <MetaDataID>{b04129d0-1f94-4f18-a268-4722d51ac944}</MetaDataID>
        bool CanDelete(object sender)
        {
            var objectSotarage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Menu);
            if (objectSotarage != null)
                return !objectSotarage.HasReferencialintegrityConstrain(Menu);
            else
                return true;

        }
        /// <MetaDataID>{7e60b944-ce8a-429b-8744-a2c99557ec80}</MetaDataID>
        void Delete()
        {
            try
            {
                (Parent as RestaurantMenus).RemoveMenu(Menu);
            }
            catch (Exception error)
            {
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
        //            imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category-add16.png"));
        //            menuItem.Header = Properties.Resources.AddCategoryMenuItemHeader;
        //            menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
        //            menuItem.Command = NewCategoryCommand;

        //            _ContextMenu.Items.Add(menuItem);



        //            //menuItem = new System.Windows.Controls.MenuItem();
        //            //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem-add16.png"));
        //            //menuItem.Header = Properties.Resources.AddCategoryMenuItemHeader;
        //            //menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
        //            //menuItem.Command = NewMenuItemCommand;

        //            //_ContextMenu.Items.Add(menuItem);
        //        }
        //        return _ContextMenu;
        //    }
        //}


        /// <MetaDataID>{ae949c24-5031-4cab-b619-a10cacaf543d}</MetaDataID>
        public bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{5f1e5ce8-0a83-4649-87e2-2a8db4af3ca9}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{a7185169-9c45-4d6a-90e7-75dc8a48e2fe}</MetaDataID>
        public List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new Image() { Source = imageSource, Width = 16, Height = 16 };

                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Delete.png"));
                    MenuCommand menuItem = new MenuCommand();
                    menuItem.Header = Properties.Resources.DeleteMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteMenuCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;

                    _ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);


                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category-add16.png"));
                    menuItem.Header = Properties.Resources.AddCategoryMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewCategoryCommand;

                    _ContextMenuItems.Add(menuItem);

                }
                return _ContextMenuItems;
            }
        }


        ///// <exclude>Excluded</exclude>
        //WPFUIElementObjectBind.RelayCommand _NewCategoryCommand;
        /// <MetaDataID>{656b7d6f-a387-42cb-96b0-bbc5c5645fc7}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand NewCategoryCommand { get; protected set; }

        /// <MetaDataID>{386cc203-375d-4ef1-b3e1-eecd92be5940}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }

        /// <MetaDataID>{881a1911-ca7e-4e79-81b1-688dc858b787}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand DeleteMenuCommand { get; protected set; }


        /// <MetaDataID>{4f47657c-c003-4c85-96a8-a5d276d81100}</MetaDataID>
        List<IMenusTreeNode> _Members;
        /// <MetaDataID>{ddb6ca2c-0984-4fd9-9238-c9e2ca76b112}</MetaDataID>
        public List<IMenusTreeNode> Members
        {
            get
            {
                if (_Members == null)
                    _Members = new List<IMenusTreeNode>() { new ItemsCategoryViewModel(Menu.RootCategory, this) };

                return _Members;
            }
        }

        /// <MetaDataID>{03ed3b28-4761-4f4a-8879-5df7e694f07f}</MetaDataID>
        public string Name
        {
            get
            {
                return Menu.Name;
            }
            set
            {
                Menu.Name = value;
            }
        }

        /// <MetaDataID>{9bcc27ad-2205-423d-bda0-67e90a276b66}</MetaDataID>
        public ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Menu16.png"));
            }
        }
        /// <MetaDataID>{ed4a1703-e424-4dee-867f-7b8947201137}</MetaDataID>
        bool _Edit = false;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{5ad1e021-91d9-4671-8683-5161a999b252}</MetaDataID>
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

        /// <MetaDataID>{638a9a1a-046e-465b-8b2a-2ae43ef04a40}</MetaDataID>
        public void EditMode()
        {

            if (_Edit == true)
            {
                _Edit = !_Edit;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        }

        /// <MetaDataID>{09000080-edcc-46a3-8d67-89deb77a9ddf}</MetaDataID>
        public IMenusTreeNode Parent
        {
            get;
            set;
        }

        /// <exclude>Excluded</exclude>
        bool _IsSelected;
        /// <MetaDataID>{0c451e71-7ec6-4738-a626-c21068fc13eb}</MetaDataID>
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

        /// <MetaDataID>{949904cd-f812-4063-8d84-9886a68ea7f4}</MetaDataID>
        public string CondextMenuResourceKey
        {
            get
            {
                return "MenuItemTreeNodeMenu";
            }
        }

        /// <MetaDataID>{7da6c473-1eef-42df-8c86-5a7d71febd45}</MetaDataID>
        public bool IsEditable
        {
            get
            {
                return true;
            }
        }
    }
}
