using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OOAdvantech.Transactions;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{5f2530de-7654-44c5-9950-ce7c8afb18ae}</MetaDataID>
    public class OptionsTypesViewModel : MarshalByRefObject, INotifyPropertyChanged
    {


        public string WindowsTitle
        {
            get
            {
                return ItemsCategory.Name + "  Option Types";
            }
        }

        MenuModel.IItemsCategory ItemsCategory;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get
            {
                return "Item Types";
            }
            set
            {
            }
        }

        ViewModelWrappers<MenuModel.IMenuItemType, MenuItemTypeViewModel> EditableMenuItemTypes = new ViewModelWrappers<MenuModel.IMenuItemType, MenuItemTypeViewModel>();

        public List<MenuItemTypeViewModel> MenuItemTypes
        {
            get
            {
                var types = ItemsCategory.MenuItemTypes;
                var menuItemTypes=(from menuItemType in types
                        select EditableMenuItemTypes.GetViewModelFor(menuItemType, menuItemType,true)).ToList();

                var selectedMenuType=menuItemTypes.Where(x => x.RealObject.Options.Count > 0).FirstOrDefault();
                SelectedMenuType = selectedMenuType;

                return menuItemTypes;
            }
        }



        public WPFUIElementObjectBind.RelayCommand DeleteSelectedOptionsTypeCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand RenameSelectedOptionsTypeCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand AddOptionsTypeCommand { get; protected set; }




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
                if (_SelectedMenuType != value)
                {
                    _SelectedMenuType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenuType)));
                }
            }
        }
        public bool IsEnableEditMenuType
        {
            get
            {
                if (SelectedMenuType != null)
                    return true;
                else
                    return false;
            }
        }

        public WPFUIElementObjectBind.RelayCommand EditCommand { get; protected set; }
        public OptionsTypesViewModel()
        {
        }

            public OptionsTypesViewModel(MenuModel.IItemsCategory itemsCategory)
        {
            ItemsCategory = itemsCategory;


            AddOptionsTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var newMenuItemType = new MenuModel.MenuItemType(Properties.Resources.MenuItemTypeDefaultName);
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ItemsCategory).CommitTransientObjectState(newMenuItemType);
                    ItemsCategory.AddClassifiedItem(newMenuItemType); 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItemTypes)));


            });

            RenameSelectedOptionsTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                SelectedMenuType.Edit = true;
            }, (object sender) => SelectedMenuType!=null);

            DeleteSelectedOptionsTypeCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(ItemsCategory));
                var menuItems = (from menuItem in storage.GetObjectCollection<MenuModel.IMenuItem>()
                             from menuItemType in menuItem.Types
                             where menuItemType==SelectedMenuType.RealObject
                             select menuItem).ToList();


                //ItemsCategory.RemoveClassifiedItem(SelectedMenuType)
            }, (object sender) => CanDeleteSelectedOptionsType());

        }

        bool CanDeleteSelectedOptionsType()
        {
            if (SelectedMenuType == null)
                return false;
            return true;
        }

    }
}
