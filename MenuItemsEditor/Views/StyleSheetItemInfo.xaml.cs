using GenWebBrowser;
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
using System.Windows.Shapes;

namespace MenuItemsEditor.Views
{
    /// <summary>
    /// Interaction logic for StyleSheetItemInfo.xaml
    /// </summary>
    public partial class StyleSheetItemInfo : StyleableWindow.Window
    {
        public StyleSheetItemInfo()
        {
            InitializeComponent();
            Loaded += StyleSheetItemInfo_Loaded;
        }

        WebBrowserOverlay ItemInfoStyleWebBrowser;

        private void StyleSheetItemInfo_Loaded(object sender, RoutedEventArgs e)
        {
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                ItemInfoStyleWebBrowser = new WebBrowserOverlay(ItemInfoStyleWebBrowserHost, BrowserType.Chrome, true);
                ItemInfoStyleWebBrowser.Navigate("http://localhost:4300/#/EditItemExtraInfo");
            }));

        }

    }
}
