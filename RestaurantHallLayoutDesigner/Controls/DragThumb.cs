using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using FloorLayoutDesigner;

namespace FloorLayoutDesigner.Controls
{
    /// <MetaDataID>{e1f1b7df-2708-4053-948a-86031c66d1af}</MetaDataID>
    public class DragThumb : Thumb
    {
        public DragThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(DragThumb_DragDelta);
            base.DragCompleted += DragThumb_DragCompleted;
            base.DragStarted += DragThumb_DragStarted;
        }
        Point DragClickPos;
        DesignerCanvas canvas;
        private void DragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            canvas = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);

            DesignerItem designerItem = this.DataContext as DesignerItem;

            while (designerItem.ParentID != Guid.Empty)
                designerItem = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerItem>(designerItem) as DesignerItem;


            DragClickPos = Mouse.GetPosition(canvas);
            Left = Canvas.GetLeft(designerItem);
            Top = Canvas.GetTop(designerItem);

        }
        double Left = 0;
        double Top = 0;

        private void DragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {

            var undoRedoCommand = UndoRedoManager.NewCommand();
            DesignerItem designerItem = this.DataContext as DesignerItem;
            while (designerItem.ParentID != Guid.Empty)
                designerItem = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerItem>(designerItem) as DesignerItem;

            DesignerCanvas designer = VisualTreeHelper.GetParent(designerItem) as DesignerCanvas;

            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {

                if (designerItem.HasChanges)
                {
                    designerItem.UpdateShape(undoRedoCommand);
                    var canvas = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(designerItem);
                    canvas.ReCalculateHallSize();
                }

                //var designerItems = designer.SelectionService.CurrentSelection.OfType<DesignerItem>();

                //foreach (DesignerItem item in designerItems)
                //{
                //    if(item.HasChanges)
                //        item.UpdateShape(undoRedoCommand);
                //}
            }));
        }

        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var mousePos = Mouse.GetPosition(canvas);

            double deltaH = mousePos.X - DragClickPos.X;
            double deltaV = mousePos.Y - DragClickPos.Y;

            DesignerItem designerItem = this.DataContext as DesignerItem;

            while (designerItem.ParentID != Guid.Empty)
                designerItem = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerItem>(designerItem) as DesignerItem;

            DesignerCanvas designer = VisualTreeHelper.GetParent(designerItem) as DesignerCanvas;
            if (designerItem != null && designer != null && designerItem.IsSelected)
            {

                double left = Canvas.GetLeft(designerItem);
                double top = Canvas.GetTop(designerItem);

                double deltaHorizontal = Math.Max(-left, e.HorizontalChange);
                double deltaVertical = Math.Max(-top, e.VerticalChange);


                Canvas.SetLeft(designerItem, Left + deltaH);
                Canvas.SetTop(designerItem, Top + deltaV);


                //Canvas.SetLeft(designerItem, left + deltaHorizontal);
                //Canvas.SetTop(designerItem, top + deltaVertical);

                //double minLeft = double.MaxValue;
                //double minTop = double.MaxValue;


                //// we only move DesignerItems
                //var designerItems = designer.SelectionService.CurrentSelection.OfType<DesignerItem>();

                //foreach (DesignerItem item in designerItems)
                //{
                //    double left = Canvas.GetLeft(item);
                //    double top = Canvas.GetTop(item);

                //    minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                //    minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
                //}

                //double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                //double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                //foreach (DesignerItem item in designerItems)
                //{
                //    double left = Canvas.GetLeft(item);
                //    double top = Canvas.GetTop(item);

                //    if (double.IsNaN(left)) left = 0;
                //    if (double.IsNaN(top)) top = 0;

                //    Canvas.SetLeft(item, left + deltaHorizontal);
                //    Canvas.SetTop(item, top + deltaVertical);
                //}

                designer.InvalidateMeasure();
                e.Handled = true;


            }
        }
    }
}
