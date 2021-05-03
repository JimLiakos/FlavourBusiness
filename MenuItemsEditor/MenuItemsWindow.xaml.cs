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
namespace MenuItemsEditor
{
    /// <summary>
    /// Interaction logic for MenuItemsWindow.xaml
    /// </summary>
    public partial class MenuItemsWindow : Window
    {
        /// <MetaDataID>{61edb364-7ad2-4cf1-9efb-480faee21541}</MetaDataID>
        public MenuItemsWindow()
        {
            InitializeComponent();
            this.GetObjectContext().Initialize(this);
            MenuItemsEditorViewModel = new MenuItemsEditorViewModel(new RestaurantMenus());
            this.GetObjectContext().SetContextInstance(MenuItemsEditorViewModel);
        }
        MenuItemsEditorViewModel MenuItemsEditorViewModel;

        

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
