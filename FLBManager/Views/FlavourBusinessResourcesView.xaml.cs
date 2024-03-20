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
using FLBManager.ViewModel;
using MenuDesigner.ViewModel;
using MenuItemsEditor.ViewModel.PriceList;
using WPFUIElementObjectBind;

namespace FLBManager.Views
{
    /// <summary>
    /// Interaction logic for FlavourBusinessResourcesView.xaml
    /// </summary>
    /// <MetaDataID>{606f01d7-7f4a-4884-9ee8-261425f9f810}</MetaDataID>
    public partial class FlavourBusinessResourcesView : UserControl
    {
        
        public FlavourBusinessResourcesView()
        {
            InitializeComponent();
        }

        /// <MetaDataID>{c97b6d64-764f-4282-9bb8-81d42981c185}</MetaDataID>
        private Point? dragStartPoint = null;


        private void ItemMouseMove(object sender, MouseEventArgs e)
        {
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

        private void ItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            if (this.dragStartPoint.HasValue)
            {

                var graphicMenuTreeNode = (sender as FrameworkElement).GetDataContextObject<GraphicMenuTreeNode>();

                if (graphicMenuTreeNode != null)
                {

                    DragDrop.DoDragDrop(this, graphicMenuTreeNode, DragDropEffects.Copy);
                }

                var priceListPresentation = (sender as FrameworkElement).GetDataContextObject<PriceListPresentation>();

                if (priceListPresentation != null)
                    DragDrop.DoDragDrop(this, priceListPresentation, DragDropEffects.Copy);

            }

        }


        private void TreeNode_DragEnter(object sender, DragEventArgs e)
        {
            
            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
            {
                dragDropTarget.DragEnter(sender, e);
            }
            else
                e.Effects = DragDropEffects.None;
        }

        private void TreeNode_DragLeave(object sender, DragEventArgs e)
        {

            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
                dragDropTarget.DragLeave(sender, e);
            else
                e.Effects = DragDropEffects.None;





        }

        private void TreeNode_DragOver(object sender, DragEventArgs e)
        {
            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
                dragDropTarget.DragOver(sender, e);
            else
                e.Effects = DragDropEffects.None;
        }

        private void TreeNode_Drop(object sender, DragEventArgs e)
        {
            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
                dragDropTarget.Drop(sender, e);
            else
                e.Effects = DragDropEffects.None;

        }

        private void DragMenuItemObject(object sender)
        {

            var serviceAreaViewModel = (sender as FrameworkElement).GetDataContextObject<FloorLayoutDesigner.ViewModel.ServiceAreaPresentation>();


            //var treeBlankItemViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.TreeBlankItemViewModel>();
            //var itemsCategoryViewModel = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.ItemsCategoryViewModel>();

            if (serviceAreaViewModel != null)
                DragDrop.DoDragDrop(this, serviceAreaViewModel, DragDropEffects.Copy);
            
            //if (treeBlankItemViewModel != null)
            //    DragDrop.DoDragDrop(this, new ViewModel.MenuCanvas.DragCanvasItem(treeBlankItemViewModel.MenuCanvasFoodItem), DragDropEffects.Copy);


            //if (itemsCategoryViewModel != null)
            //    DragDrop.DoDragDrop(this, new MenuItemsEditor.ViewModel.DragItemsCategory(itemsCategoryViewModel.ItemsCategory), DragDropEffects.Copy);
        }
    }
}
