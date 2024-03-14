using StyleableWindow;
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
using WPFUIElementObjectBind;

namespace MenuItemsEditor.Views.PriceList
{
    /// <summary>
    /// Interaction logic for PrriceListPage.xaml
    /// </summary>
    public partial class PriceListPage : PageDialogViewEmulator
    {
        public PriceListPage()
        {
            InitializeComponent();

            //Title="{x:Static resx:Resources.PriceListPageTitle}" 
        }


        

        private void DragEnter(object sender, DragEventArgs e)
        {

            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
            {
                dragDropTarget.DragEnter(sender, e);
            }
            else
                e.Effects = DragDropEffects.None;

            System.Diagnostics.Debug.WriteLine("DragEnter " + e.Effects.ToString());

        }

        private void Drop(object sender, DragEventArgs e)
        {

            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
                dragDropTarget.Drop(sender, e);
            else
                e.Effects = DragDropEffects.None;

        }

        private void DragLeave(object sender, DragEventArgs e)
        {
            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
            {
                dragDropTarget.DragLeave(sender, e);
            }
            else
                e.Effects = DragDropEffects.None;

        }

        private void DragOver(object sender, DragEventArgs e)
        {
            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
            {
                dragDropTarget.DragOver(sender, e);
            }
            else
                e.Effects = DragDropEffects.None;


        }
    }
}
