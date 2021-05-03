using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MenuModel;
using WPFUIElementObjectBind;
namespace MenuItemsEditor.Views
{
    /// <summary>
    /// Interaction logic for MenuItemsWindow.xaml
    /// </summary>
    /// <MetaDataID>{9f7e8861-c24a-42c4-abfd-7f07ba3c3e4e}</MetaDataID>
    public partial class MenuItemsWindow : Window
    {
        /// <MetaDataID>{61edb364-7ad2-4cf1-9efb-480faee21541}</MetaDataID>
        public MenuItemsWindow(RestaurantMenus restaurantMenu)
        {
            InitializeComponent();
            this.GetObjectContext().Initialize(this);
            MenuItemsEditorViewModel = new RestauranConfigViewModel(restaurantMenu);
            this.GetObjectContext().SetContextInstance(MenuItemsEditorViewModel);
        }
        private void TreeViewSelectedItemChanged(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            if (item != null)
            {
                item.BringIntoView();
                e.Handled = true;
            }
        }
        RestauranConfigViewModel MenuItemsEditorViewModel;



        /// <MetaDataID>{e1997a14-729d-4e52-b159-48a20fb723dd}</MetaDataID>
        //public System.Collections.Generic.List<IMenusTreeNode> ClassifiedItems
        //{
        //    get
        //    {
        //        return new List<IMenusTreeNode> { RestaurantMenus };
        //    }
        //}
    }


}
