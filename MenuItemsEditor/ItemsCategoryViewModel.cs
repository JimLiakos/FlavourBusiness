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

namespace MenuItemsEditor
{
    /// <MetaDataID>{37f6a5d6-c650-4bf2-9bf3-55a03518f1ba}</MetaDataID>
    public class ItemsCategoryViewModel : MarshalByRefObject,IMenusTreeNode,INotifyPropertyChanged
    {

        MenuModel.IItemsCategory ItemsCategory;
        
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


        }

        private void AddCategory()
        {

            ItemsCategory.NewSubCategory(Properties.Resources.NewItemsCategoryName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }


        public WPFUIElementObjectBind.RelayCommand RenameCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand DeleteMenuCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand NewCategoryCommand { get; protected set; }

        void Delete()
        {
            (Parent as ItemsCategoryViewModel).RemoveSubCategory(this);// .ItemsCategory.RemoveClassifiedItem(ItemsCategory)
        }

        private void RemoveSubCategory(ItemsCategoryViewModel itemsCategoryViewModel)
        {


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                ItemsCategory.RemoveClassifiedItem(itemsCategoryViewModel.ItemsCategory);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(itemsCategoryViewModel.ItemsCategory);

                stateTransition.Consistent = true;
            }
            int trt = ItemsCategory.ClassifiedItems.Count;
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

        

        public System.Windows.Controls.ContextMenu ContextMenu
        {
            get
            {
                return null;
            }
        }

        bool _Edit;

        public event PropertyChangedEventHandler PropertyChanged;
      

        public bool Edit
        {
            get
            {
                return _Edit;
            }

            //set
            //{
            //    if(_Edit == value)
            //    {
            //        _Edit = !_Edit;
            //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            //    }
            //    _Edit = value;
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            //}
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


       


        public List<IMenusTreeNode> Members
        {
            get
            {
                return (from subCategory in ItemsCategory.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                 select SubCategories.GetViewModelFor(subCategory,subCategory,this)).OfType<IMenusTreeNode>().ToList();
             
            }
        }

        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
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
    }
}
