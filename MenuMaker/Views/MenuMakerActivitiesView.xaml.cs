using FLBManager.ViewModel;
using MenuDesigner.ViewModel;
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

namespace MenuMaker.Views
{
    /// <summary>
    /// Interaction logic for MenuMakerActivitiesView.xaml
    /// </summary>
    public partial class MenuMakerActivitiesView : UserControl
    {
        public MenuMakerActivitiesView()
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


            }

        }


        private void TreeNode_DragEnter(object sender, DragEventArgs e)
        {

            var dragDropTarget = (sender as FrameworkElement).GetDataContextObject<IDragDropTarget>();
            if (dragDropTarget != null)
                dragDropTarget.DragEnter(sender, e);
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
    }
}
