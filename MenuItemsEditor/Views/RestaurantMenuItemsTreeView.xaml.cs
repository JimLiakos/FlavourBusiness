using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuItemsEditor.Views
{
    /// <summary>
    /// Interaction logic for RestaurantMenuItemsTreeView.xaml
    /// </summary>
    /// <MetaDataID>{cd9facef-c6fa-42e8-bcfe-44334e459c92}</MetaDataID>
    public partial class RestaurantMenuItemsTreeView : UserControl
    {
        /// <MetaDataID>{ada93ecd-ad2c-4dac-afcb-8fcde1851a37}</MetaDataID>
        public RestaurantMenuItemsTreeView()
        {
            InitializeComponent();
        }

        /// <MetaDataID>{8cbc4e66-c9b3-4b58-8972-fc269178a8a1}</MetaDataID>
        private void MenuItemsTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            TreeView treeView = sender as TreeView;
            IMenusTreeNode selectedItem = treeView.GetObjectContext().GetRealObject<IMenusTreeNode>(treeView.SelectedItem);
            if (selectedItem != null)
            {
                selectedItem.IsSelected = false;
                selectedItem.Edit = false;
            }

        }

   
    }
}
