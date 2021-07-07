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

namespace MenuDesigner.Views
{
    /// <summary>
    /// Interaction logic for HeadingListView.xaml
    /// </summary>
    /// <MetaDataID>{0b3177c5-a8ff-4715-b0e4-281b8d5d4ba5}</MetaDataID>
    public partial class HeadingListView : UserControl
    {
        public HeadingListView()
        {
            InitializeComponent();
        }

        private void Headings_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var selectedItem in (sender as ListView).SelectedItems)
            {
                var heading = (sender as ListView).GetObjectContext().GetRealObject<ViewModel.MenuCanvas.ListMenuHeadingPresentation>(selectedItem);
                if (heading != null)
                {
                    heading.Edit = false;
                }
            }
            foreach (var heading in (sender as ListView).GetDataContextObject<ViewModel.MenuCanvas.MenuHeadingsPresentation>().Headings)
            {
                heading.Edit = false;
            }
             (sender as ListView).UnselectAll();
        }
        // caches the start point of the drag operation
        private Point? dragStartPoint = null;

        private void OnHeadingMouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;
            else
            {
                if(!this.dragStartPoint.HasValue)
                    this.dragStartPoint = new Point?(e.GetPosition(this));

                Vector diff = e.GetPosition(this) - this.dragStartPoint.Value;
                if (diff.Y > 3 || diff.Y < 3 || diff.X > 3 || diff.X < 3)
                {
                    var heading = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.ListMenuHeadingPresentation>().Heading;
                    if (heading != null)
                    {
                        try
                        {
                            DragDrop.DoDragDrop(this, new ViewModel.MenuCanvas.DragCanvasItems(heading), DragDropEffects.Copy);
                        }
                        catch (Exception error)
                        {
                        }
                    }
                }
            }


        }
        
        private void OnHeadingMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.dragStartPoint = new Point?(e.GetPosition(this));
            


            //if (this.dragStartPoint.HasValue)
            //{
            //    var heading= (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuHeadingPresentation>().Heading;
            //    if(heading!=null)
            //        DragDrop.DoDragDrop(this,new ViewModel.DragCanvasItem( heading), DragDropEffects.Copy);

            //    e.Handled = true;
            //}
        }

        private void DragHeading(object sender)
        {

            var heading = (sender as FrameworkElement).GetDataContextObject<ViewModel.MenuCanvas.ListMenuHeadingPresentation>().Heading;
            if (heading != null)
            {
                try
                {
                    DragDrop.DoDragDrop(this, new ViewModel.MenuCanvas.DragCanvasItems(heading), DragDropEffects.Copy);
                }
                catch (Exception error)
                {
                }
            }

        }
    }
}
