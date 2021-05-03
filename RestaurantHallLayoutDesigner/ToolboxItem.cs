using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace FloorLayoutDesigner
{
    // Represents a selectable item in the Toolbox/>.
    /// <MetaDataID>{bac8e87d-e8c5-4c4d-9ae2-ec14e7bc92f0}</MetaDataID>
    public class ToolboxItem : ContentControl
    {
        // caches the start point of the drag operation
        private Point? dragStartPoint = null;

        static ToolboxItem()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ToolboxItem), new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            if (this.dragStartPoint.HasValue)
            {

                DragObject dataObject = new DragObject();

                // XamlWriter.Save() has limitations in exactly what is serialized,
                // see SDK documentation; short term solution only;
                if (this.Content is SharpVectors.Converters.SvgViewbox)
                {
                    dataObject.SVG_Uri = (this.Content as SharpVectors.Converters.SvgViewbox).Source.ToString();
                }
                else
                {
                    string xamlString = XamlWriter.Save(this.Content);
                    dataObject.Xaml = xamlString;
                }

                WrapPanel panel = VisualTreeHelper.GetParent(this) as WrapPanel;
                if (panel != null)
                {
                    // desired size for DesignerCanvas is the stretched Toolbox item size
                    double scale = 1.3;
                    dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
                }

                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);

                e.Handled = true;
            }
        }
    }

    // Wraps info of the dragged object into a class
    /// <MetaDataID>{3ebc0dc8-ccf5-43ae-8799-254a056b9f28}</MetaDataID>
    public class DragObject
    {
        // Xaml string that represents the serialized content
        public String Xaml { get; set; }


        public String SVG_Uri { get; set; }

        // Defines width and height of the DesignerItem
        // when this DragObject is dropped on the DesignerCanvas
        public Size? DesiredSize { get; set; }
    }
}
