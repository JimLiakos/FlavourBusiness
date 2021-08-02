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
using MenuItemsEditor;
using StyleableWindow;
using MenuPresentationModel.MenuCanvas;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{37f6a5d6-c650-4bf2-9bf3-55a03518f1ba}</MetaDataID>
    public class ItemsCategoryViewModel : MarshalByRefObject, IMenusTreeNode, INotifyPropertyChanged
    {



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

        internal MenuModel.IItemsCategory ItemsCategory;
        /// <exclude>Excluded</exclude>
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

                foreach (var subCategory in SubCategories.Values)
                {
                    subCategory.GraphicMenu = value;
                }
                foreach (var menuItem in MenuItems.Values)
                {
                    menuItem.GraphicMenu = value;
                }
                //RootCategory.gr
            }
        }
        public ItemsCategoryViewModel(MenuModel.IItemsCategory itemsCategory, MenuItemsEditor.IMenusTreeNode parent, MenuPresentationModel.RestaurantMenu graphicMenu)
        {
            _GraphicMenu = graphicMenu;
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

                   //System.Windows.Window owner = System.Windows.Window.GetWindow(EditOptionsTypesCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                   //var menuItemWindow = new MenuItemsEditor.Views.CategoryMenuItemTypesWindow();
                   //menuItemWindow.Owner = owner;

                   //MenuItemsEditor.ViewModel.OptionsTypesViewModel optionsTypesViewModel = new MenuItemsEditor.ViewModel.OptionsTypesViewModel(ItemsCategory);
                   //menuItemWindow.GetObjectContext().SetContextInstance(optionsTypesViewModel);
                   //menuItemWindow.ShowDialog();

                   System.Windows.Window win = System.Windows.Window.GetWindow(EditOptionsTypesCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                   var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                   MenuItemsEditor.ViewModel.OptionsTypesViewModel optionsTypesViewModel = new MenuItemsEditor.ViewModel.OptionsTypesViewModel(ItemsCategory);
                   var menuItemTypesPage = new MenuItemsEditor.Views.MenuItemTypesPage();
                   menuItemTypesPage.GetObjectContext().SetContextInstance(optionsTypesViewModel);
                   frame.ShowDialogPage(menuItemTypesPage);
                   stateTransition.Consistent = true;
               }
           });


        }


        internal void RemoveMenuItem(TreeFoodItemViewModel treeViewMenuItemViewModel)
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
                var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(owner).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                MenuModel.MenuItem menuItem = new MenuModel.MenuItem();
                var menuItemPage = new MenuItemsEditor.Views.MenuItemPage();
                MenuItemsEditor.ViewModel.MenuItemViewModel itemViewModel = new MenuItemsEditor.ViewModel.MenuItemViewModel(ItemsCategory, menuItem);
                menuItemPage.GetObjectContext().SetContextInstance(itemViewModel);

                menuItemPage.GetObjectContextConnection().Transaction.TransactionCompleted += NewMenuItemTransactionCompleted;
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


            //using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            //{
            //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //    {
            //        var menuItemWindow = new MenuItemsEditor.Views.MenuItemWindow();
            //        menuItemWindow.Owner = owner;

            //        ObjectStorage objectStorage = ObjectStorage.GetStorageOfObject(ItemsCategory);

            //        MenuModel.MenuItem menuItem = new MenuModel.MenuItem();
            //        objectStorage.CommitTransientObjectState(menuItem);
            //        ItemsCategory.AddClassifiedItem(menuItem);

            //        MenuItemsEditor.ViewModel.MenuItemViewModel menuItemViewModel = new MenuItemsEditor.ViewModel.MenuItemViewModel(menuItem);
            //        menuItemWindow.GetObjectContext().SetContextInstance(menuItemViewModel);

            //        if (menuItemWindow.ShowDialog().Value)
            //            stateTransition.Consistent = true;
            //    }
            //}
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));

        }

        private void NewMenuItemTransactionCompleted(Transaction transaction)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private void AddCategory()
        {
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var itemsCategory = ItemsCategory.NewSubCategory(MenuItemsEditor.Properties.Resources.NewItemsCategoryName);

                    ItemsCategoryViewModel itemsCategoryViewModel = SubCategories.GetViewModelFor(itemsCategory, itemsCategory, this, _GraphicMenu);

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
                    IsSelected = false;
                    itemsCategoryViewModel.IsSelected = true;
                    itemsCategoryViewModel.Edit = true;
                    stateTransition.Consistent = true;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }


        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteMenuCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand NewCategoryCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand NewMenuItemCommand { get; protected set; }



        public WPFUIElementObjectBind.RelayCommand EditOptionsTypesCommand { get; protected set; }

        void Delete()
        {
            if (Parent != null)
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
            //if (Parent is MenuItemsEditor.ViewModel.MenuViewModel)
            //    return false;
            //else 
            if (Parent is ItemsCategoryViewModel)
                return (Parent as ItemsCategoryViewModel).ItemsCategory.CanDeleteSubCategory(ItemsCategory);
            else
                return true;

        }




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
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
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
                    menuItem.Header = MenuItemsEditor.Properties.Resources.AddCategoryMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewCategoryCommand;

                    _ContextMenuItems.Add(menuItem);




                    menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem-add16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.NewItemMenuItemHeader;
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewMenuItemCommand;

                    _ContextMenuItems.Add(menuItem);


                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Type16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.ShowCategoryOptionTypesMenuItemHeader;
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
            if (_Edit == true)
            {
                _Edit = !_Edit;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        }

        ViewModelWrappers<MenuModel.IItemsCategory, ItemsCategoryViewModel> SubCategories = new ViewModelWrappers<MenuModel.IItemsCategory, ItemsCategoryViewModel>();
        ViewModelWrappers<MenuModel.IMenuItem, TreeFoodItemViewModel> MenuItems = new ViewModelWrappers<MenuModel.IMenuItem, TreeFoodItemViewModel>();



        /// TreeViewOptionsTypesViewModel MenuItemTypesViewModel;

        public List<MenuItemsEditor.IMenusTreeNode> Members
        {
            get
            {
                List<MenuItemsEditor.IMenusTreeNode> members = (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                select SubCategories.GetViewModelFor(subCategory, subCategory, this, _GraphicMenu)).OfType<MenuItemsEditor.IMenusTreeNode>().Union(
                    (from menuItem in ItemsCategory.ClassifiedItems.OfType<MenuModel.MenuItem>()
                     select MenuItems.GetViewModelFor(menuItem, menuItem, this, _GraphicMenu)).OfType<MenuItemsEditor.IMenusTreeNode>()).ToList();

                if (Parent == null)//Root Category
                    members.Insert(0, new TreeBlankItemViewModel(null, _GraphicMenu));
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
                using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    ItemsCategory.Name = value;
                    suppressStateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }


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

        public MenuItemsEditor.IMenusTreeNode Parent { get; set; }

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

        internal void MoveItemTo(MenuCanvasFoodItem menuCanvasItem, TreeFoodItemViewModel targetTreeFoodItemViewModel)
        {
            //TreeFoodItemViewModel movingMenuCanvasItem= MenuItems.Where(x => x.Key == menuCanvasItem.MenuItem).Select(x => x.Value).FirstOrDefault();

            TreeFoodItemViewModel movingMenuCanvasItem = GetTreeFoodItemViewModel(menuCanvasItem);



            using (SystemStateTransition SuppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    (movingMenuCanvasItem.Parent as ItemsCategoryViewModel).ItemsCategory.RemoveClassifiedItem(movingMenuCanvasItem.MenuItem as MenuModel.IClassified);
                    (targetTreeFoodItemViewModel.Parent as ItemsCategoryViewModel).InsertBefore(targetTreeFoodItemViewModel, movingMenuCanvasItem);

                    stateTransition.Consistent = true;
                } 
                SuppressStateTransition.Consistent = true;
            }


            (movingMenuCanvasItem.Parent as ItemsCategoryViewModel).MenuItems.Remove(movingMenuCanvasItem.MenuItem);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));

        }

        private void InsertBefore(TreeFoodItemViewModel targetTreeFoodItemViewModel, TreeFoodItemViewModel movingMenuCanvasItem)
        {
            var pos = ItemsCategory.ClassifiedItems.IndexOf(targetTreeFoodItemViewModel.MenuItem  as MenuModel.IClassified);
            ItemsCategory.InsertClassifiedItem(pos, movingMenuCanvasItem.MenuItem as MenuModel.IClassified);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        TreeFoodItemViewModel GetTreeFoodItemViewModel(MenuCanvasFoodItem menuCanvasItem)
        {
            TreeFoodItemViewModel movingMenuCanvasItem = MenuItems.Where(x => x.Key == menuCanvasItem.MenuItem).Select(x => x.Value).FirstOrDefault();
            if (movingMenuCanvasItem != null)
                return movingMenuCanvasItem;

            movingMenuCanvasItem = SubCategories.Values.Where(x => x.GetTreeFoodItemViewModel(menuCanvasItem) != null).Select(x => x.GetTreeFoodItemViewModel(menuCanvasItem)).FirstOrDefault();

            return movingMenuCanvasItem;
        }

    }
}
