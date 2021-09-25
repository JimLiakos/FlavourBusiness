using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{19537242-5674-4988-a59b-8bd8c1e697ca}</MetaDataID>
    public class OptionsTypeSelctionViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        MenuModel.IItemsCategory ItemsCategory;
        public OptionsTypeSelctionViewModel(MenuModel.IItemsCategory itemsCategory)
        {
            ItemsCategory = itemsCategory;
            EditableMenuItemTypes.OnNewViewModelWrapper += EditableMenuItemTypes_OnNewViewModelWrapper;
        }

        private void EditableMenuItemTypes_OnNewViewModelWrapper(ViewModelWrappers<MenuModel.IMenuItemType, MenuItemTypeViewModel> sender, MenuModel.IMenuItemType key, MenuItemTypeViewModel value)
        {
            value.IsCheckedChanged += Value_IsCheckedChanged;
        }

        bool CheckedChanged;
        private void Value_IsCheckedChanged(object sender, EventArgs e)
        {
            if (CheckedChanged)
                return;
            try
            {
                CheckedChanged = true;
                foreach (var optionsTypeViewModel in MenuItemTypes)
                {
                    if (optionsTypeViewModel.IsSelected)
                        optionsTypeViewModel.IsChecked = (sender as MenuItemTypeViewModel).IsChecked;
                }
            }
            finally
            {
                CheckedChanged = false;
            }
        }

        public OptionsTypeSelctionViewModel()
        {

        }


        ViewModelWrappers<MenuModel.IMenuItemType, MenuItemTypeViewModel> EditableMenuItemTypes = new ViewModelWrappers<MenuModel.IMenuItemType, MenuItemTypeViewModel>();
        public List<MenuItemTypeViewModel> MenuItemTypes
        {
            get
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ItemsCategory));
                var types = (from category in storage.GetObjectCollection<MenuModel.IItemsCategory>()
                        from menuItemType in category.ClassifiedItems.OfType<MenuModel.IMenuItemType>()
                        select menuItemType).ToList();

                
                return (from menuItemType in types
                        select EditableMenuItemTypes.GetViewModelFor(menuItemType, menuItemType,false)).ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        MenuItemTypeViewModel _SelectedMenuType;
        public MenuItemTypeViewModel SelectedMenuType
        {
            get
            {
                return _SelectedMenuType;
            }
            set
            {
                _SelectedMenuType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenuType)));
            }
        }


    }
}
