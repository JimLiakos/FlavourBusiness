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

namespace MenuItemsEditor
{
    /// <MetaDataID>{1fc16510-b8d3-42a6-9b42-9039a599aa23}</MetaDataID>
    public class MenuViewModel :MarshalByRefObject, IMenusTreeNode, INotifyPropertyChanged
    {

       internal readonly MenuModel.IMenu Menu;
        public MenuViewModel(MenuModel.IMenu menu,IMenusTreeNode parent)
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

       
        bool CanDelete(object sender)
        {
            var objectSotarage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(Menu);
            if (objectSotarage != null)
                return objectSotarage.CanDeleteObject(Menu);
            else
                return true;

        }
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

        System.Windows.Controls.ContextMenu _ContextMenu;
        public System.Windows.Controls.ContextMenu ContextMenu
        {
            get
            {
                if (_ContextMenu == null)
                {

                    _ContextMenu = new System.Windows.Controls.ContextMenu();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new Image() { Source = imageSource, Width = 16, Height = 16 };

                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Delete.png"));
                    System.Windows.Controls.MenuItem menuItem = new System.Windows.Controls.MenuItem();
                    menuItem.Header = "Delete";
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteMenuCommand;
                    _ContextMenu.Items.Add(menuItem);


                    menuItem = new System.Windows.Controls.MenuItem();
                    menuItem.Header = "Rename";
                    menuItem.Icon = emptyImage;
                    menuItem.Command = RenameCommand;
                    _ContextMenu.Items.Add(menuItem);

                    _ContextMenu.Items.Add(new Separator());

                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category-add16.png"));
                    menuItem = new System.Windows.Controls.MenuItem();
                    menuItem.Header = "Add Category";
                    menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewCategoryCommand;
                    _ContextMenu.Items.Add(menuItem);

                }
                return _ContextMenu;

            }
        }

        ///// <exclude>Excluded</exclude>
        //WPFUIElementObjectBind.RelayCommand _NewCategoryCommand;
        public WPFUIElementObjectBind.RelayCommand NewCategoryCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand DeleteMenuCommand { get; protected set; }


        List<IMenusTreeNode> _Members;
        public List<IMenusTreeNode> Members
        {
            get
            {
                if (_Members == null)
                    _Members = new List<IMenusTreeNode>() { new ItemsCategoryViewModel(Menu.RootCategory,this) };

                return _Members;
            }
        }

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

        public ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Menu16.png"));
            }
        }
        bool _Edit = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Edit
        {
            get
            {
                return _Edit;
            }
            //set
            //{
            //    _Edit = value;

            //    if (_Edit == value)
            //    {
            //        _Edit = !_Edit;
            //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            //    }

            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            //}
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

        public IMenusTreeNode Parent
        {
            get;
            set;
        }
    }
}
