using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;
using SharpVectors.Converters;
using RestaurantHallLayoutModel;
using OOAdvantech.Transactions;
using FloorLayoutDesigner;
using OOAdvantech.PersistenceLayer;
using WPFUIElementObjectBind;
using System.Windows.Media;
using System.ComponentModel;
using FloorLayoutDesigner.ViewModel;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FloorLayoutDesigner
{
    /// <MetaDataID>FloorLayoutDesigner.DesignerCanvas</MetaDataID>
    public partial class DesignerCanvas : Canvas, ICommandTargetObject, INotifyPropertyChanged
    {
        /// <MetaDataID>{a654e6b3-fd0b-4527-8924-382c879092f0}</MetaDataID>
        private Point? rubberbandSelectionStartPoint = null;

        /// <MetaDataID>{b1715d72-23b9-46e1-a1e3-8e18e33ca56c}</MetaDataID>
        private SelectionService selectionService;
        /// <MetaDataID>{45583f37-1800-4e95-923f-348e5700a4a6}</MetaDataID>
        internal SelectionService SelectionService
        {
            get
            {
                if (selectionService == null)
                {
                    selectionService = new SelectionService(this);
                    selectionService.SelectionsChanged += SelectionService_SelectionsChanged;
                }

                return selectionService;
            }
        }

        private void SelectionService_SelectionsChanged(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShape)));
        }

        Guid _ID = Guid.NewGuid();

        public Guid ID => _ID;

        /// <MetaDataID>{e5a1c6a0-3db7-418c-a68b-626f26690de6}</MetaDataID>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                // in case that this click is the start of a 
                // drag operation we cache the start point
                this.rubberbandSelectionStartPoint = new Point?(e.GetPosition(this));

                // if you click directly on the canvas all 
                // selected items are 'de-selected'
                SelectionService.ClearSelection();
                Focus();
                e.Handled = true;
            }
        }

        /// <MetaDataID>{ddc34fd3-2686-4b1f-9d78-348bc07a6e3b}</MetaDataID>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
                this.rubberbandSelectionStartPoint = null;

            // ... but if mouse button is pressed and start
            // point value is set we do have one
            if (this.rubberbandSelectionStartPoint.HasValue)
            {
                // create rubberband adorner
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this, rubberbandSelectionStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }
            }
            e.Handled = true;
        }

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{abfe64a8-9a3a-4066-bd68-960d50ab1171}</MetaDataID>
        public List<MenuCommand> ContextMenuItems
        {
            get
            {

                if (_ContextMenuItems == null)
                {
                    _ContextMenuItems = new List<MenuCommand>();
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/font16.png"));
                    menuItem.Header = Properties.Resources.HallLayoutLabelFontMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    // menuItem.Command = LabelFontCommand;
                    _ContextMenuItems.Add(menuItem);
                    menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/textBackground16.png"));
                    menuItem.Header = Properties.Resources.HallLayoutLabelBkMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = LabelBackgroundCommand;
                    _ContextMenuItems.Add(menuItem);
                    //Images/Metro/ServicePoint.png
                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServicePoint.png"));
                    menuItem.Header = "ShapeServicepointToggle 3";
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = ShapeServicepointToggleCommand;
                    _ContextMenuItems.Add(menuItem);
                    _ContextMenuItems.Add(null);
                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServicePoint.png"));
                    menuItem.Header = "Cut";
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //  menuItem.Command = CutCommand;
                    _ContextMenuItems.Add(menuItem);
                }
                return _ContextMenuItems;
            }
        }

        /// <MetaDataID>{d9935be4-e097-42bf-aa2e-bdc303796719}</MetaDataID>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            DragObject dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
            if (dragObject != null && (!String.IsNullOrEmpty(dragObject.Xaml) || !String.IsNullOrEmpty(dragObject.SVG_Uri)))
            {

                this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                {
                    DesignerItem newItem = null;
                    Object content = null;
                    if (!string.IsNullOrWhiteSpace(dragObject.Xaml))
                        content = XamlReader.Load(XmlReader.Create(new StringReader(dragObject.Xaml)));

                    Uri sourceUri = null;
                    if (content is SvgViewbox)
                        sourceUri = (content as SvgViewbox).Source;
                    else if (!string.IsNullOrWhiteSpace(dragObject.SVG_Uri))
                        sourceUri = new Uri(dragObject.SVG_Uri);

                    if (sourceUri != null)
                    {
                        var svgHostControl = new SvgHostControl();
                        svgHostControl.svgHost.Source = sourceUri;
                        ServicePointShape servicePointShape = new ServicePointShape();
                        servicePointShape.ShapeImageUrl = sourceUri.ToString();
                        servicePointShape.Identity = Guid.NewGuid().ToString("N");
                        newItem = new DesignerItem(servicePointShape, HallLayoutPresentation);
                        newItem.ItemContent = svgHostControl;
                        Point position = e.GetPosition(this);
                        if (dragObject.DesiredSize.HasValue)
                        {
                            Size desiredSize = dragObject.DesiredSize.Value;
                            newItem.Width = desiredSize.Width;
                            newItem.Height = desiredSize.Height;
                            DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X - newItem.Width / 2));
                            DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y - newItem.Height / 2));
                        }
                        else
                        {
                            DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X));
                            DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y));
                        }

                        Canvas.SetZIndex(newItem, this.Children.Count);
                        this.Children.Add(newItem);
                        SetConnectorDecoratorTemplate(newItem);
                        //update selection
                        this.SelectionService.SelectItem(newItem);
                        newItem.Focus();
                        this.InvalidateVisual();

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            var undoRedoCommand = UndoRedoManager.NewCommand();
                            this.MarkUndo(undoRedoCommand);
                            OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(HallLayoutPresentation.HallLayout).CommitTransientObjectState(servicePointShape);

                            if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(servicePointShape))
                                ObjectStorage.GetStorageOfObject(HallLayoutPresentation.HallLayout).CommitTransientObjectState(servicePointShape);

                            HallLayoutPresentation.HallLayout.AddShape(servicePointShape);
                            newItem.UpdateShape(null);

                            this.MarkRedo(undoRedoCommand);
                            stateTransition.Consistent = true;
                        }
                    }
                }));
                return;

                //if (newItem.Content is SharpVectors.Converters.SvgViewbox)
                //{
                //    bool _textAsGeometry = false;
                //    bool _includeRuntime = true;
                //    bool _optimizePath = true;

                //    SharpVectors.Renderers.Wpf.WpfDrawingSettings settings = new SharpVectors.Renderers.Wpf.WpfDrawingSettings();
                //    settings.IncludeRuntime = _includeRuntime;
                //    settings.TextAsGeometry = _textAsGeometry;
                //    settings.OptimizePath = _optimizePath;

                //    System.Windows.Media.DrawingGroup drawing = null;
                //    using (FileStream ms = new FileStream("E:/MyWindowProfileData/Downloads/kitchen.svg", FileMode.Open, System.IO.FileAccess.Read))
                //    {
                //        using (SharpVectors.Converters.FileSvgReader reader = new SharpVectors.Converters.FileSvgReader(settings))
                //        {
                //            drawing = reader.Read(ms);
                //        }
                //    }
                //    if (drawing != null)
                //        (content as SharpVectors.Converters.SvgViewbox).Drawing = drawing;


                //}


                e.Handled = true;
            }
        }

        void UpdateHallLayout(UndoRedoCommand undoRedoCommand = null)
        {
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                if (undoRedoCommand == null)
                    undoRedoCommand = UndoRedoManager.NewCommand();
                foreach (var designItem in this.Children.OfType<DesignerItem>())
                {
                    designItem.UpdateShape(undoRedoCommand);
                }

            }));
        }

        List<MenuModel.IMealType> MealTypes;

        /// <MetaDataID>{33b03057-f4cf-45cf-af5e-8e55c5df8a3b}</MetaDataID>
        ViewModel.HallLayoutViewModel HallLayoutPresentation;
        /// <MetaDataID>{4ebf64bd-f547-4e7e-b801-0332dc8e2ff3}</MetaDataID>
        public void Load(ViewModel.HallLayoutViewModel hallLayoutPresentation)
        {
            if (hallLayoutPresentation == null)
                return;
            hallLayoutPresentation.DesignerCanvas = this;
            HallLayout hallLayout = hallLayoutPresentation.HallLayout;

            if (HallLayoutPresentation != null)
                HallLayoutPresentation.HallLayoutSizeChanged -= HallLayoutSizeChanged;

            HallLayoutPresentation = hallLayoutPresentation;
            HallLayoutPresentation.HallLayoutSizeChanged += HallLayoutSizeChanged;




            Width = hallLayout.Width;
            Height = hallLayout.Height;

            UndoRedoManager.Clear();
            this.Children.Clear();
            this.SelectionService.ClearSelection();

            var servicePoints = hallLayout.ServiceArea.ServicePoints;
            foreach (var shape in hallLayout.Shapes)
            {
                if (!string.IsNullOrWhiteSpace(shape.ServicesPointIdentity))
                {
                    var servicePoint = servicePoints.Where(x => x.ServicesPointIdentity == shape.ServicesPointIdentity).FirstOrDefault();
                    if (servicePoint != null)
                    {
                        shape.Label = servicePoint.Description;
                    }
                    else
                    {
                        shape.ServicesPointIdentity = null;
                        shape.Label = null;
                    }

                }

                if (shape is ShapesGroup)
                    LoadShape(shape as ShapesGroup, this);
                else
                    LoadShape(shape, this);
            }

            this.InvalidateVisual();



            //IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            //foreach (XElement connectionXML in connectionsXML)
            //{
            //    Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
            //    Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

            //    String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
            //    String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

            //    Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
            //    Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

            //    Connection connection = new Connection(sourceConnector, sinkConnector);
            //    Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
            //    this.Children.Add(connection);
            //}
        }

        private void HallLayoutSizeChanged(HallLayout hallLayout, PaperSize newSize)
        {
            Width = hallLayout.Width;
            Height = hallLayout.Height;

        }

        private DesignerItem LoadShape(Shape shape, Canvas canvas)
        {

            Guid id = new Guid(shape.Identity);
            DesignerItem item = new DesignerItem(shape, HallLayoutPresentation); // DeserializeDesignerItem(itemXML, id, 0, 0);
            canvas.Children.Add(item);
            SvgHostControl content = new SvgHostControl();
            (content as SvgHostControl).svgHost.Source = new Uri(shape.ShapeImageUrl);
            item.ItemContent = content;

            //Task.Run(() =>
            //{
            //    (content as SvgHostControl).svgHost.Source = new Uri(shape.ShapeImageUrl);
            //    item.ItemContent = content;
            //});
            SetConnectorDecoratorTemplate(item);

            UndoRedoManager.ReplaceCommandTarget(item);
            return item;
        }

        private DesignerItem LoadShape(ShapesGroup shapesGroup, Canvas canvas)
        {
            Guid id = new Guid(shapesGroup.Identity);
            DesignerItem item = new DesignerItem(shapesGroup, HallLayoutPresentation); // DeserializeDesignerItem(itemXML, id, 0, 0);


            canvas.Children.Add(item);
            Canvas groupCanvas = CreateGroupCanvas(item, shapesGroup);


            item.IsGroup = true;
            foreach (var groupedShape in shapesGroup.Shapes)
            {
                DesignerItem grouped_item = (from designerItem in Children.OfType<DesignerItem>()
                                             where designerItem.Shape == groupedShape
                                             select designerItem).FirstOrDefault();
                if (grouped_item == null)
                {
                    if (groupedShape is ShapesGroup)
                        grouped_item = LoadShape(groupedShape as ShapesGroup, groupCanvas);
                    else
                        grouped_item = LoadShape(groupedShape, groupCanvas);
                }
                grouped_item.ParentID = item.ID;

                //DesignerItem grouped_item = new DesignerItem(groupedShape);
                //grouped_item.ParentID = item.ID;
                //SvgHostControl content = new SvgHostControl();
                //(content as SvgHostControl).svgHost.Source = new Uri(groupedShape.ShapeImageUrl);
                //grouped_item.Content = content;
                //this.Children.Add(grouped_item);
            }
            Canvas.SetLeft(item, item.Shape.Left);
            Canvas.SetTop(item, item.Shape.Top);
            SetConnectorDecoratorTemplate(item);
            UndoRedoManager.ReplaceCommandTarget(item);
            return item;
        }

        /// <MetaDataID>{d14b4785-24a0-4683-8493-0f18f87d6fbf}</MetaDataID>
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();

            foreach (UIElement element in this.InternalChildren)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                //measure desired size for each child
                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        /// <MetaDataID>{8fe245b3-c0b4-4bfe-9080-a4c2c9ac4bed}</MetaDataID>
        private void SetConnectorDecoratorTemplate(DesignerItem item)
        {
            if (item.ApplyTemplate() && item.ItemContent is UIElement)
            {
                ControlTemplate template = DesignerItem.GetConnectorDecoratorTemplate(item.ItemContent as UIElement);
                Control decorator = item.Template.FindName("PART_ConnectorDecorator", item) as Control;
                if (decorator != null && template != null)
                    decorator.Template = template;
            }
        }

        public void UpdateState(Dictionary<string, object> state)
        {
            var shapes = state["HallSaphes"] as System.Collections.Generic.List<Shape>;
            List<Shape> removeShapes = HallLayoutPresentation.HallLayout.Shapes.Where(hallShape => !shapes.Contains(hallShape)).ToList();
            List<Shape> addShapes = shapes.Where(shape => !HallLayoutPresentation.HallLayout.Shapes.Contains(shape)).ToList();



            foreach (var addShape in addShapes)
            {
                if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(addShape))
                    ObjectStorage.GetStorageOfObject(HallLayoutPresentation.HallLayout).CommitTransientObjectState(addShape);

                HallLayoutPresentation.HallLayout.AddShape(addShape);

                var designerItem = ObjectContext.FindChilds<DesignerItem>(this).OfType<DesignerItem>().Where(existingDesignerItem => existingDesignerItem.Shape == addShape).FirstOrDefault();

                if (designerItem == null)
                {
                    if (addShape is ShapesGroup)
                        designerItem = LoadShape(addShape as ShapesGroup, this);
                    else
                        designerItem = LoadShape(addShape, this);
                }
                else
                {
                    var ownerCanvas = WPFUIElementObjectBind.ObjectContext.FindParent<Canvas>(designerItem);
                    if (ownerCanvas != this)
                    {
                        if (ownerCanvas != null)
                            ownerCanvas.Children.Remove(designerItem);
                        this.Children.Add(designerItem);
                    }

                    if (designerItem.ParentID != Guid.Empty)
                        SelectionService.AddToSelection(designerItem);
                    designerItem.ParentID = Guid.Empty;
                }
                if (addShape is ShapesGroup)
                {
                    var shapesGroup = addShape as ShapesGroup;
                    Canvas groupCanvas = null;
                    if (designerItem.ItemContent is Grid && (designerItem.ItemContent as Grid).Children.OfType<Viewbox>().FirstOrDefault() != null && (designerItem.ItemContent as Grid).Children.OfType<Viewbox>().FirstOrDefault().Child is Canvas)
                        groupCanvas = (designerItem.ItemContent as Grid).Children.OfType<Viewbox>().FirstOrDefault().Child as Canvas;

                    if (groupCanvas == null)
                        groupCanvas = CreateGroupCanvas(designerItem, shapesGroup);


                    foreach (var groupedShape in (addShape as ShapesGroup).Shapes)
                    {
                        var groupedItem = (from existingDesignerItem in Children.OfType<DesignerItem>()
                                           where existingDesignerItem.Shape == groupedShape
                                           select existingDesignerItem).FirstOrDefault();
                        //if (groupedItem == null)
                        //{
                        //    groupedItem = LoadShape(groupedShape, groupCanvas);
                        //    Canvas.SetLeft(groupedItem, groupedItem.Shape.Left);
                        //    Canvas.SetTop(groupedItem, groupedItem.Shape.Top);
                        //    groupedItem.ParentID = dItem.ID;
                        //}
                        if (groupedItem != null)
                        {
                            Canvas ownerCanvas = ObjectContext.FindParent<Canvas>(groupedItem);
                            if (ownerCanvas != null)
                                ownerCanvas.Children.Remove(groupedItem);

                            groupCanvas.Children.Add(groupedItem);
                            Canvas.SetLeft(groupedItem, groupedItem.Shape.Left);
                            Canvas.SetTop(groupedItem, groupedItem.Shape.Top);
                            groupedItem.ParentID = designerItem.ID;
                        }
                    }
                }
                if (addShapes.Count == 1 && addShape is ShapesGroup)
                    SelectionService.SelectItem(designerItem);
            }
            foreach (var removeShape in removeShapes)
            {
                var dItem = (from designerItem in Children.OfType<DesignerItem>()
                             where designerItem.Shape == removeShape
                             select designerItem).FirstOrDefault();
                if (dItem != null && dItem.ParentID == Guid.Empty)
                {
                    foreach (var groupedItem in (from designerItem in Children.OfType<DesignerItem>()
                                                 where designerItem.ParentID == dItem.ID
                                                 select designerItem).ToList())
                        Children.Remove(groupedItem);

                    Children.Remove(dItem);
                }
                HallLayoutPresentation.HallLayout.RemoveShape(removeShape);
            }
            this.InvalidateVisual();
        }

        private static Canvas CreateGroupCanvas(DesignerItem designerItem, ShapesGroup shapesGroup)
        {
            Canvas groupCanvas = new Canvas();
            Grid grid = new Grid();
            Viewbox viewbox = new Viewbox();
            viewbox.Stretch = System.Windows.Media.Stretch.Uniform;
            viewbox.HorizontalAlignment = HorizontalAlignment.Stretch;
            viewbox.VerticalAlignment = VerticalAlignment.Stretch;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Children.Add(viewbox);
            designerItem.ItemContent = grid;
            viewbox.Child = groupCanvas;
            var rect = GetBoundingRectangle(shapesGroup.Shapes);
            groupCanvas.Width = shapesGroup.ClientAreaWidth;
            groupCanvas.Height = shapesGroup.ClientAreaHeight;
            //groupCanvas.Background = new SolidColorBrush(Colors.Coral);
            return groupCanvas;
        }

        public void Undo(UndoRedoCommand command)
        {
            SelectionService.ClearSelection();

            var state = command.UndoData.Where(x => x.CommandTargetObject == this).Select(x => x.Data).FirstOrDefault() as System.Collections.Generic.Dictionary<string, object>;
            UpdateState(state);

            //var shapes = state["HallSaphes"] as System.Collections.Generic.List<Shape>;

            //List<Shape> removeShapes = new List<Shape>();
            //foreach (var shape in HallLayout.Shapes)
            //{
            //    if (!shapes.Contains(shape))
            //        removeShapes.Add(shape);
            //}

            //List<Shape> addShapes = new List<Shape>();
            //foreach (var shape in shapes)
            //{
            //    if (!HallLayout.Shapes.Contains(shape))
            //        addShapes.Add(shape);
            //}

            //foreach (var addShape in addShapes)
            //{
            //    if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(addShape))
            //        ObjectStorage.GetStorageOfObject(HallLayout).CommitTransientObjectState(addShape);

            //    HallLayout.AddShape(addShape);

            //    var dItem = (from designerItem in Children.OfType<DesignerItem>()
            //                 where designerItem.Shape == addShape
            //                 select designerItem).FirstOrDefault();
            //    if (dItem == null)
            //    {
            //        if (addShape is ShapesGroup)
            //            dItem = LoadShape(addShape as ShapesGroup);
            //        else
            //            dItem = LoadShape(addShape);
            //    }
            //    else
            //    {
            //        if (dItem.ParentID != Guid.Empty)
            //            SelectionService.AddToSelection(dItem);
            //        dItem.ParentID = Guid.Empty;
            //    }
            //    if (addShape is ShapesGroup)
            //    {
            //        foreach (var groupedShape in (addShape as ShapesGroup).Shapes)
            //        {
            //            var groupedItem = (from designerItem in Children.OfType<DesignerItem>()
            //                               where designerItem.Shape == groupedShape
            //                               select designerItem).FirstOrDefault();
            //            groupedItem.ParentID = dItem.ID;
            //        }
            //    }
            //    if (addShapes.Count == 1 && addShape is ShapesGroup)
            //        SelectionService.SelectItem(dItem);
            //}
            //foreach (var removeShape in removeShapes)
            //{
            //    var dItem = (from designerItem in Children.OfType<DesignerItem>()
            //                 where designerItem.Shape == removeShape
            //                 select designerItem).FirstOrDefault();
            //    if (dItem != null && dItem.ParentID == Guid.Empty)
            //    {
            //        foreach (var groupedItem in (from designerItem in Children.OfType<DesignerItem>()
            //                                     where designerItem.ParentID == dItem.ID
            //                                     select designerItem).ToList())
            //            Children.Remove(groupedItem);

            //        Children.Remove(dItem);
            //    }
            //    HallLayout.RemoveShape(removeShape);
            //}
            //this.InvalidateVisual();
        }

        public void Redo(UndoRedoCommand command)
        {
            SelectionService.ClearSelection();

            var state = command.RedoData.Where(x => x.CommandTargetObject == this).Select(x => x.Data).FirstOrDefault() as System.Collections.Generic.Dictionary<string, object>;
            UpdateState(state);

            //var shapes = state["HallSaphes"] as System.Collections.Generic.List<Shape>;

            //List<Shape> removeShapes = new List<Shape>();
            //foreach (var shape in HallLayout.Shapes)
            //{
            //    if (!shapes.Contains(shape))
            //        removeShapes.Add(shape);
            //}

            //List<Shape> addShapes = new List<Shape>();
            //foreach (var shape in shapes)
            //{
            //    if (!HallLayout.Shapes.Contains(shape))
            //        addShapes.Add(shape);
            //}

            //foreach (var addShape in addShapes)
            //{

            //    if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(addShape))
            //        ObjectStorage.GetStorageOfObject(HallLayout).CommitTransientObjectState(addShape);
            //    HallLayout.AddShape(addShape);

            //    var dItem = (from designerItem in Children.OfType<DesignerItem>()
            //                 where designerItem.Shape == addShape
            //                 select designerItem).FirstOrDefault();

            //    if (dItem == null)
            //    {
            //        if (addShape is ShapesGroup)
            //            dItem = LoadShape(addShape as ShapesGroup);
            //        else
            //            dItem = LoadShape(addShape);
            //    }
            //    else
            //    {
            //        if (dItem.ParentID != Guid.Empty)
            //            SelectionService.AddToSelection(dItem);
            //        dItem.ParentID = Guid.Empty;
            //    }
            //    if (addShape is ShapesGroup)
            //    {
            //        foreach (var groupedShape in (addShape as ShapesGroup).Shapes)
            //        {
            //            var groupedItem = (from designerItem in Children.OfType<DesignerItem>()
            //                               where designerItem.Shape == groupedShape
            //                               select designerItem).FirstOrDefault();
            //            groupedItem.ParentID = dItem.ID;
            //        }
            //    }
            //    if (addShapes.Count == 1 && addShape is ShapesGroup)
            //        SelectionService.SelectItem(dItem);
            //}
            //foreach (var removeShape in removeShapes)
            //{
            //    var dItem = (from designerItem in Children.OfType<DesignerItem>()
            //                 where designerItem.Shape == removeShape
            //                 select designerItem).FirstOrDefault();
            //    if (dItem != null && dItem.ParentID == Guid.Empty)
            //        Children.Remove(dItem);
            //    HallLayout.RemoveShape(removeShape);
            //}


            //this.InvalidateVisual();

        }

        public void MarkUndo(UndoRedoCommand command)
        {
            System.Collections.Generic.Dictionary<string, object> state = new System.Collections.Generic.Dictionary<string, object>();
            state["HallSaphes"] = HallLayoutPresentation.HallLayout.Shapes.ToList();
            command.UndoData.Insert(0, new CommandTarget() { CommandTargetObject = this, Data = state });

        }

        public void MarkRedo(UndoRedoCommand command)
        {
            System.Collections.Generic.Dictionary<string, object> state = new System.Collections.Generic.Dictionary<string, object>();
            state["HallSaphes"] = HallLayoutPresentation.HallLayout.Shapes.ToList();

            command.RedoData.Add(new CommandTarget() { CommandTargetObject = this, Data = state });
        }

        internal void ReCalculateHallSize()
        {
            foreach (var designerItem in Children.OfType<DesignerItem>())
            {
                try
                {
                    var rect = designerItem.TransformToAncestor(this).TransformBounds(new Rect(designerItem.RenderSize));

                    if (rect.Right > HallLayoutPresentation.HallLayout.Width)
                        HallLayoutPresentation.HallLayout.Width = rect.Right;

                    if (rect.Bottom > HallLayoutPresentation.HallLayout.Height)
                        HallLayoutPresentation.HallLayout.Height = rect.Bottom;
                }
                catch (Exception error)
                {
                }
            }
        }
    }
}
