using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FLBManager.ViewModel;
using MenuItemsEditor.ViewModel;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel
{

    public delegate void ShowMenuTaxesHandle(MenuModel.IMenu menu);
    /// <MetaDataID>{4442d7e1-c1d6-4fca-abb7-5d89f7ca9d2c}</MetaDataID>
    public class RestaurantMenuItemsPresentation : MarshalByRefObject, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public event ShowMenuTaxesHandle ShowMenuTaxes;
        MenuModel.IMenu Menu;

        /// <exclude>Excluded</exclude>
        MenuPresentationModel.RestaurantMenu _GraphicMenu;
        public MenuPresentationModel.RestaurantMenu GraphicMenu
        {
            get
            {
                return _GraphicMenu;
            }
            set
            {
                if (_GraphicMenu != value)
                {
                    _GraphicMenu = value;

                    using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            RootCategory.GraphicMenu = value;
                            stateTransition.Consistent = true;
                        }
                        suppressStateTransition.Consistent = true;
                    }
                }
            }
        }
        public MenuCanvas.ItemsCategoryViewModel RootCategory { get; set; }
        public RestaurantMenuItemsPresentation(MenuModel.IMenu menu,IMenusStyleSheets menuStyleSheets, MenuPresentationModel.RestaurantMenu graphicMenu)
        {
            _GraphicMenu = graphicMenu;
            Menu = menu;

            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    RootCategory = new MenuCanvas.ItemsCategoryViewModel(Menu.RootCategory, null, graphicMenu, menuStyleSheets);
                    stateTransition.Consistent = true;
                }
                suppressStateTransition.Consistent = true;
            }


            RootCategory.IsNodeExpanded = true;
            foreach (MenuItemsEditor.IMenusTreeNode treeNode in RootCategory.Members)
                treeNode.IsNodeExpanded = true;

            SelectedCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));
            });

            ActionsCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                ActionsPopupOpen = !ActionsPopupOpen;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActionsPopupOpen)));
            });

            ShowMenuTaxesCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                ShowMenuTaxes?.Invoke(this.Menu);

            });
        }



        public bool ActionsPopupOpen { get; set; }


        public WPFUIElementObjectBind.RelayCommand SelectedCommand { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand ActionsCommand { get; protected set; }


        public List<MenuItemsEditor.IMenusTreeNode> MenuItems
        {
            get
            {
                return RootCategory.Members;
            }
        }

        public WPFUIElementObjectBind.RelayCommand ShowMenuTaxesCommand { get; protected set; }


        List<WPFUIElementObjectBind.MenuCommand> _ContextMenuItems;
        public List<WPFUIElementObjectBind.MenuCommand> ContextMenuItems
        {
            get
            {
                var selectedItemContextMenuItems = RootCategory.SelectedItemContextMenuItems;
                if (selectedItemContextMenuItems != null)
                {
                    if (selectedItemContextMenuItems.Where(x => x != null && x.Command == ShowMenuTaxesCommand).FirstOrDefault() == null)
                    {
                        var menuItem = new MenuCommand(); ;
                        var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Taxes16.png"));
                        menuItem.Header = MenuItemsEditor.Properties.Resources.TaxesMenuItemHeader;
                        menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = ShowMenuTaxesCommand;

                        selectedItemContextMenuItems.Add(menuItem);
                    }
                    return selectedItemContextMenuItems;
                }
                else
                {
                    if (RootCategory.ContextMenuItems.Where(x => x != null && x.Command == ShowMenuTaxesCommand).FirstOrDefault() == null)
                    {
                        var menuItem = new MenuCommand(); ;
                        var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Taxes16.png"));
                        menuItem.Header = MenuItemsEditor.Properties.Resources.TaxesMenuItemHeader;
                        menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = ShowMenuTaxesCommand;

                        RootCategory.ContextMenuItems.Add(menuItem);
                    }
                    return RootCategory.ContextMenuItems;


                }
            }
        }
    }
}
