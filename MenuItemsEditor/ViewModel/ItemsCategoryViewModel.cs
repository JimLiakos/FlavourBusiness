using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;
using System.Windows;
using OOAdvantech.PersistenceLayer;
using UIBaseEx;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{37f6a5d6-c650-4bf2-9bf3-55a03518f1ba}</MetaDataID>
    public class ItemsCategoryViewModel : MarshalByRefObject,IMenusTreeNode,INotifyPropertyChanged
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
                        var contextMenuItems = subItem.SelectedItemContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }
                }
                return null;
            }
        }

      public readonly  MenuModel.IItemsCategory ItemsCategory;
        
        public ItemsCategoryViewModel(MenuModel.IItemsCategory itemsCategory,IMenusTreeNode parent)
        {
            Parent = parent;
            ItemsCategory = itemsCategory;
            _Name = itemsCategory.Name;

            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });

            DeleteMenuCommand = new RelayCommand((object sender) =>
            {
                Delete();
            }, (object sender) => CanDelete(sender));

            NewCategoryCommand = new RelayCommand((object sender) =>
            {
                AddCategory();
            });
            NewMenuItemCommand = new RelayCommand((object sender) =>
            {
                
                System.Windows.Window win = System.Windows.Window.GetWindow(NewMenuItemCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                NewMenuItem(win);
            });

            EditOptionsTypesCommand = new RelayCommand((object sender) =>
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    System.Windows.Window owner = System.Windows.Window.GetWindow(EditOptionsTypesCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                    var menuItemWindow = new Views.CategoryMenuItemTypesWindow();
                    menuItemWindow.Owner = owner;
                    OptionsTypesViewModel optionsTypesViewModel = new OptionsTypesViewModel(ItemsCategory);
                    menuItemWindow.GetObjectContext().SetContextInstance(optionsTypesViewModel);
                    menuItemWindow.ShowDialog();

                    stateTransition.Consistent = true;
                }
            });

            
        }

        internal void RemoveMenuItem(TreeViewMenuItemViewModel treeViewMenuItemViewModel)
        {
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    ItemsCategory.RemoveClassifiedItem(treeViewMenuItemViewModel.MenuItem as MenuModel.MenuItem);
                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(treeViewMenuItemViewModel.MenuItem);
                    stateTransition.Consistent = true;
                }
                MenuItems.Remove(treeViewMenuItemViewModel.MenuItem);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private void NewMenuItem(System.Windows.Window owner)
        {
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var menuItemWindow = new Views.MenuItemWindow();
                    menuItemWindow.Owner = owner;

                    ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(ItemsCategory);

                    MenuModel.MenuItem menuItem = new MenuModel.MenuItem();
                    objectStorage.CommitTransientObjectState(menuItem);
                    ItemsCategory.AddClassifiedItem(menuItem);

                    MenuItemViewModel menuItemViewModel = new MenuItemViewModel(menuItem);
                    menuItemWindow.GetObjectContext().SetContextInstance(menuItemViewModel);

                    if (menuItemWindow.ShowDialog().Value)
                        stateTransition.Consistent = true;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));

        }
        private void AddCategory()
        {
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var itemsCategory = ItemsCategory.NewSubCategory(Properties.Resources.NewItemsCategoryName);

                    ItemsCategoryViewModel itemsCategoryViewModel = SubCategories.GetViewModelFor(itemsCategory, itemsCategory, this);

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
                    IsSelected = false;
                    itemsCategoryViewModel.IsSelected = true;
                    itemsCategoryViewModel.Edit = true;
                    stateTransition.Consistent = true;
                }
            }
        }


        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteMenuCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand NewCategoryCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand NewMenuItemCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand EditOptionsTypesCommand { get; protected set; }

        void Delete()
        {
            (Parent as ItemsCategoryViewModel).RemoveSubCategory(this);// .ItemsCategory.RemoveClassifiedItem(ItemsCategory)
        }

        private void RemoveSubCategory(ItemsCategoryViewModel itemsCategoryViewModel)
        {

            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    ItemsCategory.RemoveClassifiedItem(itemsCategoryViewModel.ItemsCategory);
                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(itemsCategoryViewModel.ItemsCategory);

                    stateTransition.Consistent = true;
                }
            }
            SubCategories.Remove(itemsCategoryViewModel.ItemsCategory);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));

        }

        private bool CanDelete(object sender)
        {
            if (Parent is MenuViewModel)
                return false;
            else if (Parent is ItemsCategoryViewModel)
                return (Parent as ItemsCategoryViewModel).ItemsCategory.CanDeleteSubCategory(ItemsCategory);
            else
                return true;

        }



        /// <exclude>Excluded</exclude>
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



        //            menuItem = new System.Windows.Controls.MenuItem();
        //            imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem-add16.png"));
        //            menuItem.Header = Properties.Resources.NewItemMenuItemHeader;
        //            menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
        //            menuItem.Command = NewMenuItemCommand;

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

                    //menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    ////menuItem.Header = Properties.Resources.TreeNodeRenameMenuItemHeader;
                    //menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = RenameCommand;

                    _ContextMenuItems.Add(null);


                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category-add16.png"));
                    menuItem.Header = Properties.Resources.AddCategoryMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewCategoryCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem-add16.png"));
                    menuItem.Header = Properties.Resources.NewItemMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewMenuItemCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Type16.png"));
                    menuItem.Header = Properties.Resources.ShowCategoryOptionTypesMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditOptionsTypesCommand;

                    _ContextMenuItems.Add(menuItem);

                }
                return _ContextMenuItems;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

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
            if(_Edit==true)
            {
                _Edit = !_Edit;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        }



        ViewModelWrappers<MenuModel.IItemsCategory, ItemsCategoryViewModel> SubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsCategoryViewModel>();
        ViewModelWrappers<MenuModel.IMenuItem, TreeViewMenuItemViewModel> MenuItems = new ViewModelWrappers<MenuModel.IMenuItem, TreeViewMenuItemViewModel>();



       /// TreeViewOptionsTypesViewModel MenuItemTypesViewModel;

        public List<IMenusTreeNode> Members
        {
            get
            {
                List<IMenusTreeNode> members= (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                        select SubCategories.GetViewModelFor(subCategory, subCategory, this)).OfType<IMenusTreeNode>().Union(
                    (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                    select MenuItems.GetViewModelFor(menuItem, menuItem, this)).OfType<IMenusTreeNode>()).ToList();
                return members;

            }
        }

        string _Name;
        public string Name
        {
            get
            {
                return ItemsCategory.Name;
            }

            set
            {
                ItemsCategory.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public IMenusTreeNode Parent{get;set;}

        public ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category16.png"));
            }
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

        bool _IsNodeExpanded;
        public bool IsNodeExpanded
        {
            get
            {
                return _IsNodeExpanded;
            }
            set
            {
                _IsNodeExpanded = value;
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
    }
}
