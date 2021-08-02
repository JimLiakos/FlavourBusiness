using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OOAdvantech.Transactions;
using System.Windows;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{006ff8f1-37ea-41c0-8b02-0347403777f0}</MetaDataID>
    public class TreeViewMenuItemViewModel : MarshalByRefObject, IMenusTreeNode, INotifyPropertyChanged
    {

        public readonly MenuModel.IMenuItem MenuItem;
        public TreeViewMenuItemViewModel(MenuModel.IMenuItem menuItem, IMenusTreeNode parent)
        {
            MenuItem = menuItem;
            Parent = parent;

            RenameCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                EditMode();
            });

            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                EditMenuItem(win);
            });

            DeleteMenuCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
             {
                 (Parent as ItemsCategoryViewModel).RemoveMenuItem(this);
             });

        }

        private void EditMenuItem(System.Windows.Window owner)
        {



            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var menuItemWindow = new Views.MenuItemWindow();
                    menuItemWindow.Owner = owner;

                    var types = MenuItem.Types.ToList();

                    MenuItemViewModel menuItemViewModel = new MenuItemViewModel(MenuItem);
                    menuItemWindow.GetObjectContext().SetContextInstance(menuItemViewModel);

                    if (menuItemWindow.ShowDialog().Value)
                        stateTransition.Consistent = true;
                } 
                suppressStateTransition.Consistent = true;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));

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

        public List<IMenusTreeNode> Members
        {
            get
            {
                return new List<IMenusTreeNode>();
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
                MenuItem.Name = value;
            }
        }

        public IMenusTreeNode Parent { get; set; }

        public ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem16.png"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;




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

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = Properties.Resources.EditObject;
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
            get;set;
        }

        public List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return this.ContextMenuItems;

                else
                {
                    foreach(var subItem in this.Members)
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
