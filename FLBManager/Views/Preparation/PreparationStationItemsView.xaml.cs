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

namespace FLBManager.Views.Preparation
{
    /// <summary>
    /// Interaction logic for PreparationStationItemsView.xaml
    /// </summary>
    /// <MetaDataID>{e120e8a6-1e10-4dd7-ba73-8da0943ff7bf}</MetaDataID>
    public partial class PreparationStationItemsView : UserControl
    {
        public PreparationStationItemsView()
        {
            InitializeComponent();
        }

        private void DragEnter(object sender, DragEventArgs e)
        {

            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<ViewModel.IDragDropTarget>();
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

            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<ViewModel.IDragDropTarget>();
            if (dragDropTarget != null)
                dragDropTarget.Drop(sender, e);
            else
                e.Effects = DragDropEffects.None;

        }

        private void DragLeave(object sender, DragEventArgs e)
        {
            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<ViewModel.IDragDropTarget>();
            if (dragDropTarget != null)
            {
                dragDropTarget.DragLeave(sender, e);
            }
            else
                e.Effects = DragDropEffects.None;
            
        }

        private void DragOver(object sender, DragEventArgs e)
        {
            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<ViewModel.IDragDropTarget>();
            if (dragDropTarget != null)
            {
                dragDropTarget.DragOver(sender, e);
            }
            else
                e.Effects = DragDropEffects.None;

            
        }

        private void ContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //ButtonCommand
        }
    }
}
