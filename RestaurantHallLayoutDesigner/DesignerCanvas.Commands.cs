using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using OOAdvantech.Transactions;
using FloorLayoutDesigner;
using RestaurantHallLayoutModel;
using OOAdvantech.PersistenceLayer;
using System.ComponentModel;
using FloorLayoutDesigner.ViewModel;
using FlavourBusinessFacade.ServicesContextResources;

namespace FloorLayoutDesigner
{
    /// <MetaDataID>FloorLayoutDesigner.DesignerCanvas</MetaDataID>
    public partial class DesignerCanvas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ///// <MetaDataID>{2b29b4d9-9e82-432f-9519-769518336c43}</MetaDataID>
        //public static RoutedCommand Group = new RoutedCommand();

        //public static RoutedCommand LabelFont = new RoutedCommand();
        //public static RoutedCommand LabelBackground = new RoutedCommand();
        //public static RoutedCommand ShapeServicepointToggle = new RoutedCommand();


        ///// <MetaDataID>{2b860397-7db4-41d3-b3d4-cc5cc6e77361}</MetaDataID>
        //public static RoutedCommand Ungroup = new RoutedCommand();
        ///// <MetaDataID>{32545482-81cd-40c2-85f6-bfac46e6df25}</MetaDataID>
        //public static RoutedCommand BringForward = new RoutedCommand();
        ///// <MetaDataID>{39ef9fc5-f940-4a2e-8980-adc67b028cf9}</MetaDataID>
        //public static RoutedCommand BringToFront = new RoutedCommand();
        ///// <MetaDataID>{add5f7d1-5c7b-4d05-a455-213f1a49b223}</MetaDataID>
        //public static RoutedCommand SendBackward = new RoutedCommand();
        ///// <MetaDataID>{e6faa705-3217-4286-aa46-4086016a87d1}</MetaDataID>
        //public static RoutedCommand SendToBack = new RoutedCommand();
        ///// <MetaDataID>{0bd2ad71-5091-4509-8cba-14f035aa72d3}</MetaDataID>
        //public static RoutedCommand AlignTop = new RoutedCommand();
        ///// <MetaDataID>{3295bf6f-b1c3-4060-a99b-767bd63aef46}</MetaDataID>
        //public static RoutedCommand Rotate = new RoutedCommand();
        ///// <MetaDataID>{3f1d6ec5-c5db-49b1-a2a2-01992b4ffd8f}</MetaDataID>
        //public static RoutedCommand AlignVerticalCenters = new RoutedCommand();
        ///// <MetaDataID>{bac32713-ddc0-470d-a814-f75d856e0128}</MetaDataID>
        //public static RoutedCommand AlignBottom = new RoutedCommand();
        ///// <MetaDataID>{4229aa57-aaf4-4a39-a7ea-6562ea9796fa}</MetaDataID>
        //public static RoutedCommand AlignLeft = new RoutedCommand();
        ///// <MetaDataID>{c10e454a-d7b5-43b8-88fe-85db884523fa}</MetaDataID>
        //public static RoutedCommand AlignHorizontalCenters = new RoutedCommand();
        ///// <MetaDataID>{67c5bbcf-8e0a-4de4-b10c-15dfa613ec93}</MetaDataID>
        //public static RoutedCommand AlignRight = new RoutedCommand();
        ///// <MetaDataID>{d16f7ef6-2cf1-48d9-8391-20d160599f1a}</MetaDataID>
        //public static RoutedCommand DistributeHorizontal = new RoutedCommand();
        ///// <MetaDataID>{d030904b-98bc-42f2-8b79-e4bc77a78db2}</MetaDataID>
        //public static RoutedCommand DistributeVertical = new RoutedCommand();
        /// <MetaDataID>{7ab6cc82-a746-4ff0-9cfe-7c25de60cef3}</MetaDataID>
        public static RoutedCommand SelectAll = new RoutedCommand();


        //public WPFUIElementObjectBind.RelayCommand Cut { get; protected set; }
        //public WPFUIElementObjectBind.RelayCommand Copy { get; protected set; }
        //public WPFUIElementObjectBind.RelayCommand Paste { get; protected set; }
        //public WPFUIElementObjectBind.RelayCommand Delete { get; protected set; }

        public WPFUIElementObjectBind.RelayCommand PageSetup { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutShapeLabelFont { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand HallLayoutShapeLabelBackground { get; protected set; }



        /// <MetaDataID>{1240f832-3bae-444a-aeda-9069a10317a8}</MetaDataID>
        public DesignerCanvas()
        {
            // this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, New_Executed));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Open_Executed));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Save_Executed));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Print_Executed));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, Cut_Executed, Cut_Enabled));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, Copy_Executed, Copy_Enabled));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, Paste_Executed, Paste_Enabled));
            //this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Group, Group_Executed, Group_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.LabelFont, LabelFont_Executed));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.LabelBackground, LabelBackground_Executed));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.ShapeServicepointToggle, ShapeServicepointToggle_Executed, ShapeServicepointToggle_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Ungroup, Ungroup_Executed, Ungroup_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringForward, BringForward_Executed, Order_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.BringToFront, BringToFront_Executed, Order_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendBackward, SendBackward_Executed, Order_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SendToBack, SendToBack_Executed, Order_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.Rotate, Rotate_Executed, Rotate_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignTop, AlignTop_Executed, Align_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignVerticalCenters, AlignVerticalCenters_Executed, Align_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignBottom, AlignBottom_Executed, Align_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignLeft, AlignLeft_Executed, Align_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignHorizontalCenters, AlignHorizontalCenters_Executed, Align_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.AlignRight, AlignRight_Executed, Align_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeHorizontal, DistributeHorizontal_Executed, Distribute_Enabled));
            //this.CommandBindings.Add(new CommandBinding(DesignerCanvas.DistributeVertical, DistributeVertical_Executed, Distribute_Enabled));

            this.CommandBindings.Add(new CommandBinding(DesignerCanvas.SelectAll, SelectAll_Executed));
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            this.AllowDrop = true;
            this.Focusable = true;

            Clipboard.Clear();

            //Paste = new WPFUIElementObjectBind.RelayCommand((object sender) => Paste_Executed(sender, null), (object sender) =>
            // {
            //     return Clipboard.ContainsData(DataFormats.Xaml);
            // });


            HallLayoutShapeLabelFont = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {

                LabelFont_Executed(sender, null);

            });






            PageSetup = new WPFUIElementObjectBind.RelayCommand((object sender) =>
         {
             PageSetup_Executed(sender, null);


         }, (object sender) =>
         {
             return true;
         });

            DataContext = this;



        }


        //   public event PropertyChangedEventHandler PropertyChanged;

        #region New Command

        /// <MetaDataID>{c08232f0-b74c-4fa1-ac54-25e5d4c64179}</MetaDataID>
        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Children.Clear();
            this.SelectionService.ClearSelection();

        }

        #endregion

        #region Open Command

        /// <MetaDataID>{507e55e7-1c2e-47e1-be07-f9e6918b1d88}</MetaDataID>
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XElement root = LoadSerializedDataFromFile();

            if (root == null)
                return;

            this.Children.Clear();
            this.SelectionService.ClearSelection();

            IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");
            foreach (XElement itemXML in itemsXML)
            {
                Guid id = new Guid(itemXML.Element("ID").Value);
                DesignerItem item = DeserializeDesignerItem(itemXML, id, 0, 0);
                this.Children.Add(item);
                SetConnectorDecoratorTemplate(item);
            }

            this.InvalidateVisual();

            IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
            foreach (XElement connectionXML in connectionsXML)
            {
                Guid sourceID = new Guid(connectionXML.Element("SourceID").Value);
                Guid sinkID = new Guid(connectionXML.Element("SinkID").Value);

                String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                Connector sourceConnector = GetConnector(sourceID, sourceConnectorName);
                Connector sinkConnector = GetConnector(sinkID, sinkConnectorName);

                Connection connection = new Connection(sourceConnector, sinkConnector);
                Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                this.Children.Add(connection);
            }
        }

        #endregion

        #region Save Command

        /// <MetaDataID>{8e31f2e6-adf9-4b43-8a02-81be1b1e9bf4}</MetaDataID>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IEnumerable<DesignerItem> designerItems = this.Children.OfType<DesignerItem>();
            IEnumerable<Connection> connections = this.Children.OfType<Connection>();

            XElement designerItemsXML = SerializeDesignerItems(designerItems);
            XElement connectionsXML = SerializeConnections(connections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            SaveFile(root);
        }

        #endregion

        #region Print Command

        /// <MetaDataID>{3ff8258f-546a-45d0-ae96-b071825db82a}</MetaDataID>
        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.ClearSelection();

            PrintDialog printDialog = new PrintDialog();

            if (true == printDialog.ShowDialog())
            {
                printDialog.PrintVisual(this, "WPF Diagram");
            }
        }

        #endregion

        #region Copy Command

        /// <MetaDataID>{20b04a1d-982c-40c5-8614-677a51d3f87f}</MetaDataID>
        internal void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
        }

        /// <MetaDataID>{588923b7-e049-4e7e-829c-4086ea87a1f9}</MetaDataID>
        internal void Copy_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CopyEnabled();
        }
        internal bool CopyEnabled()
        {
            return SelectionService.CurrentSelection.Count() > 0;
        }
        #endregion


        Point MouseRightClickPos;
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            MouseRightClickPos = Mouse.GetPosition(this);
        }


        #region Paste Command

        /// <MetaDataID>{5c08e36a-5c3a-4135-88f2-3917a7578233}</MetaDataID>
        internal void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            XElement root = LoadSerializedDataFromClipBoard();

            if (root == null)
                return;
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                var undoRedoCommand = UndoRedoManager.NewCommand();
                this.MarkUndo(undoRedoCommand);
                // create DesignerItems
                Dictionary<Guid, Guid> mappingOldToNewIDs = new Dictionary<Guid, Guid>();
                List<ISelectable> newItems = new List<ISelectable>();
                IEnumerable<XElement> itemsXML = root.Elements("DesignerItems").Elements("DesignerItem");

                double offsetX = Double.Parse(root.Attribute("OffsetX").Value, CultureInfo.InvariantCulture);
                double offsetY = Double.Parse(root.Attribute("OffsetY").Value, CultureInfo.InvariantCulture);


                double left = Double.Parse(root.Attribute("Left").Value, CultureInfo.InvariantCulture);
                double top = Double.Parse(root.Attribute("Top").Value, CultureInfo.InvariantCulture);

                offsetY = MouseRightClickPos.Y - top;
                offsetX = MouseRightClickPos.X - left;
                Dictionary<Guid, DesignerItem> copyItems = new Dictionary<Guid, DesignerItem>();

                Dictionary<Guid, List<DesignerItem>> copyGroupedItems = new Dictionary<Guid, List<DesignerItem>>();

                foreach (XElement itemXML in itemsXML)
                {
                    Guid oldID = new Guid(itemXML.Element("ID").Value);
                    Guid newID = Guid.NewGuid();
                    mappingOldToNewIDs.Add(oldID, newID);
                    DesignerItem item = DeserializeDesignerItem(itemXML, newID, offsetX, offsetY);


                    if (!item.IsGroup && item.Content is SvgHostControl)
                    {

                        var uri = (item.Content as SvgHostControl).svgHost.Source;
                        item.Content = new SvgHostControl();
                        (item.Content as SvgHostControl).svgHost.Source = uri;
                        item.Shape = new ServicePointShape();

                        item.Shape.Identity = item.ID.ToString();
                        if (item.ParentID == Guid.Empty)
                        {
                            ObjectStorage.GetStorageOfObject(HallLayoutPresentation.HallLayout).CommitTransientObjectState(item.Shape);
                            HallLayoutPresentation.HallLayout.AddShape(item.Shape);
                            this.Children.Add(item);
                        }
                        else
                        {
                            DesignerItem groupDesignerItem = null;
                            if (copyItems.TryGetValue(item.ParentID, out groupDesignerItem))
                            {

                            }
                            else
                            {
                                List<DesignerItem> groupedItems = null;
                                if (!copyGroupedItems.TryGetValue(item.ParentID, out groupedItems))
                                {
                                    groupedItems = new List<DesignerItem>();
                                    copyGroupedItems[item.ParentID] = groupedItems;
                                }
                                groupedItems.Add(item);
                            }

                        }

                        item.Shape.ShapeImageUrl = uri.AbsoluteUri;
                        item.UpdateShape(null);
                    }
                    else
                    {
                        item.Shape = new ShapesGroup();
                        item.Shape.Identity = item.ID.ToString();
                        item.UpdateShape(undoRedoCommand);
                        ObjectStorage.GetStorageOfObject(HallLayoutPresentation.HallLayout).CommitTransientObjectState(item.Shape);
                        HallLayoutPresentation.HallLayout.AddShape(item.Shape);
                        this.Children.Add(item);
                        List<DesignerItem> groupedItems = null;
                        copyItems[item.ID] = item;
                        if (copyGroupedItems.TryGetValue(item.ParentID, out groupedItems))
                        {

                        }
                    }



                    SetConnectorDecoratorTemplate(item);
                    newItems.Add(item);
                }

                // update group hierarchy
                SelectionService.ClearSelection();
                foreach (DesignerItem el in newItems)
                {
                    if (el.ParentID != Guid.Empty)
                        el.ParentID = mappingOldToNewIDs[el.ParentID];
                }
                foreach (DesignerItem group in newItems.OfType<DesignerItem>().Where(x => x.IsGroup))
                {
                    var groupedItems = newItems.OfType<DesignerItem>().Where(x => x.ParentID == group.ID).ToList();
                    foreach (var groupedShape in newItems.OfType<DesignerItem>().Where(x => x.ParentID == group.ID).Select(x => x.Shape).ToList())
                    {
                        (group.Shape as ShapesGroup).AddGroupedShape(groupedShape);
                    }

                    var rect = GetBoundingRectangle((group.Shape as ShapesGroup).Shapes);
                    (group.Shape as ShapesGroup).ClientAreaWidth = rect.Width;
                    (group.Shape as ShapesGroup).ClientAreaHeight = rect.Height;

                    Canvas groupCanvas = CreateGroupCanvas(group, (group.Shape as ShapesGroup));

                    foreach (DesignerItem el in newItems.OfType<DesignerItem>().Where(x => x.ParentID == group.ID))
                    {
                        if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(el.Shape))
                            ObjectStorage.GetStorageOfObject(group.Shape).CommitTransientObjectState(el.Shape);

                        var le = Canvas.GetLeft(el);
                        var to = Canvas.GetTop(el);



                        (group.Shape as ShapesGroup).AddGroupedShape(el.Shape);
                        groupCanvas.Children.Add(el);
                    }

                    group.MarkRedo(undoRedoCommand);
                }

                foreach (DesignerItem item in newItems)
                {
                    if (item.ParentID == Guid.Empty)
                    {
                        SelectionService.AddToSelection(item);
                    }
                }

                // create Connections
                IEnumerable<XElement> connectionsXML = root.Elements("Connections").Elements("Connection");
                foreach (XElement connectionXML in connectionsXML)
                {
                    Guid oldSourceID = new Guid(connectionXML.Element("SourceID").Value);
                    Guid oldSinkID = new Guid(connectionXML.Element("SinkID").Value);

                    if (mappingOldToNewIDs.ContainsKey(oldSourceID) && mappingOldToNewIDs.ContainsKey(oldSinkID))
                    {
                        Guid newSourceID = mappingOldToNewIDs[oldSourceID];
                        Guid newSinkID = mappingOldToNewIDs[oldSinkID];

                        String sourceConnectorName = connectionXML.Element("SourceConnectorName").Value;
                        String sinkConnectorName = connectionXML.Element("SinkConnectorName").Value;

                        Connector sourceConnector = GetConnector(newSourceID, sourceConnectorName);
                        Connector sinkConnector = GetConnector(newSinkID, sinkConnectorName);

                        Connection connection = new Connection(sourceConnector, sinkConnector);
                        Canvas.SetZIndex(connection, Int32.Parse(connectionXML.Element("zIndex").Value));
                        this.Children.Add(connection);

                        SelectionService.AddToSelection(connection);
                    }
                }
                BringSelectedItemsToFront(undoRedoCommand);
                this.MarkRedo(undoRedoCommand);


                //DesignerCanvas.BringToFront.Execute(null, this);

                // update paste offset
                root.Attribute("OffsetX").Value = (offsetX + 10).ToString();
                root.Attribute("OffsetY").Value = (offsetY + 10).ToString();


                Clipboard.Clear();
                Clipboard.SetData(DataFormats.Xaml, root);
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paste)));

            }));
        }

        /// <MetaDataID>{e9dde05e-2a44-42bb-b48a-dce384166bde}</MetaDataID>
        private void Paste_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = PasteEnabled();
        }

        internal bool PasteEnabled()
        {
            return Clipboard.ContainsData(DataFormats.Xaml);
        }
        public void PageSetup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                var mainWindow = System.Windows.Window.GetWindow(this);
                Views.HallLayoutWindow HallLayoutWindow = new Views.HallLayoutWindow();
                HallLayoutWindow.Owner = mainWindow;
                HallLayoutWindow.GetObjectContext().SetContextInstance(HallLayoutPresentation);

                if (HallLayoutWindow.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }

            Width = this.HallLayoutPresentation.HallLayout.Width;
            Height = this.HallLayoutPresentation.HallLayout.Height;
        }

        public void LabelFont_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            //var hallLayoutPresentation = HallLayoutPresentation;
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                {
                    StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                    fontDialog.GetObjectContext().SetContextInstance(HallLayoutPresentation.FontPresantation);
                    fontDialog.Owner = win;
                    if (fontDialog.ShowDialog().Value)
                        stateTransition.Consistent = true;
                }
            }));
        }
        public void ShapeServicepointToggle_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                var sdsd = SelectedShape.DataContext;
                var serviceArea = SelectedShape.HallLayoutViewModel.HallLayout.ServiceArea;
                if (!string.IsNullOrWhiteSpace(SelectedShape.Shape.ServicesPointIdentity))
                {
                    SelectedShape.Shape.ServicesPointIdentity = null;
                    SelectedShape.Shape.Label = null;
                    SelectedShape.ServicesPoints = new List<IServicePoint>();
                }
                else
                {
                    var hallServicesPoints = SelectedShape.HallLayoutViewModel.HallLayout.Shapes.Select(x => x.ServicesPointIdentity).ToList();
                    List<IServicePoint> unassignedServicePoints = serviceArea.GetUnassignedServicePoints(hallServicesPoints);
                    unassignedServicePoints = unassignedServicePoints.OrderBy(x => x.Description).ToList();
                    SelectedShape.ServicesPoints = unassignedServicePoints;
                    SelectedShape.ServicePointsPopupIsOpen = true;
                }

            }));

            //  SelectedShape.Shape.ServicesPointIdentity = serviceArea.ServicePoints.FirstOrDefault().ServicesPointIdentity;

        }

        private void ShapeServicepointToggle_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SelectedShape != null;
        }
        public void LabelBackground_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            //var hallLayoutPresentation = HallLayoutPresentation;
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                {
                    try
                    {
                        Views.ShapeLabelBackgroundWindow shapeLabelBackgroundSetup = new Views.ShapeLabelBackgroundWindow();
                        shapeLabelBackgroundSetup.GetObjectContext().SetContextInstance(HallLayoutPresentation);
                        shapeLabelBackgroundSetup.Owner = win;
                        if (shapeLabelBackgroundSetup.ShowDialog().Value)
                            stateTransition.Consistent = true;
                    }
                    catch (Exception error)
                    {

                    }
                }
            }));
        }

        #endregion

        #region Delete Command

        /// <MetaDataID>{89e2d136-a61f-4479-8f6b-1f8fa57a4007}</MetaDataID>
        internal void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCurrentSelection();
            Focus();
        }

        /// <MetaDataID>{2b682900-1886-4e1f-a4b8-dcb5317d3277}</MetaDataID>
        internal void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DeleteEnabled();
        }

        internal bool DeleteEnabled()
        {
            return this.SelectionService.CurrentSelection.Count() > 0;
        }
        #endregion

        #region Cut Command

        /// <MetaDataID>{1535bb4d-8111-4309-8b84-7cda8adb0659}</MetaDataID>
        internal void Cut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CopyCurrentSelection();
            DeleteCurrentSelection();
            Focus();
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paste)));
        }

        /// <MetaDataID>{2739707f-1bb8-4b46-863d-5f136cab7a1b}</MetaDataID>
        private void Cut_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = CutEnabled();
        }

        internal bool CutEnabled()
        {
            return this.SelectionService.CurrentSelection.Count() > 0;
        }
        #endregion

        #region Group Command

        /// <MetaDataID>{8c1743d4-d229-48ad-993e-8b5873c1c57a}</MetaDataID>
        public void Group_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var items = from item in this.SelectionService.CurrentSelection.OfType<DesignerItem>()
                        where item.ParentID == Guid.Empty
                        select item;
            if (items.ToList().Count > 0)
            {



                this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                {

                    Rect rect = GetBoundingRectangle(items.Select(x => x.Shape));
                    var undoRedoCommand = UndoRedoManager.NewCommand();
                    this.MarkUndo(undoRedoCommand);
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        ShapesGroup shapesGroup = new ShapesGroup();
                        shapesGroup.Identity = Guid.NewGuid().ToString();
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(HallLayoutPresentation.HallLayout).CommitTransientObjectState(shapesGroup);
                        HallLayoutPresentation.HallLayout.AddShape(shapesGroup);

                        DesignerItem groupItem = new DesignerItem(shapesGroup, HallLayoutPresentation);

                        groupItem.IsGroup = true;
                        groupItem.Width = rect.Width;
                        groupItem.Height = rect.Height;
                        Canvas.SetLeft(groupItem, rect.Left);
                        Canvas.SetTop(groupItem, rect.Top);

                        shapesGroup.ClientAreaWidth = rect.Width;
                        shapesGroup.ClientAreaHeight = rect.Height;

                        Canvas groupCanvas = CreateGroupCanvas(groupItem, shapesGroup);

                        //new Canvas();
                        //Grid grid = new Grid();
                        //Viewbox viewbox = new Viewbox();
                        //viewbox.Stretch = System.Windows.Media.Stretch.Uniform;
                        //viewbox.HorizontalAlignment = HorizontalAlignment.Stretch;
                        //viewbox.VerticalAlignment = VerticalAlignment.Stretch;
                        //grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                        //grid.VerticalAlignment = VerticalAlignment.Stretch;
                        //grid.Children.Add(viewbox);

                        //viewbox.Child = groupCanvas;

                        //groupCanvas.Width = rect.Width;
                        //groupCanvas.Height = rect.Height;
                        //groupItem.Content = grid;

                        this.Children.Add(groupItem);

                        Canvas.SetZIndex(groupItem, this.Children.Count);
                        groupItem.UpdateShape(undoRedoCommand);

                        foreach (DesignerItem item in items)
                        {
                            var itemLeft = Canvas.GetLeft(item) - rect.Left;
                            var itemTop = Canvas.GetTop(item) - rect.Top;
                            Children.Remove(item);
                            groupCanvas.Children.Add(item);
                            Canvas.SetLeft(item, itemLeft);
                            Canvas.SetTop(item, itemTop);
                            item.ParentID = groupItem.ID;
                            item.UpdateShape(undoRedoCommand);

                            shapesGroup.AddGroupedShape(item.Shape);
                            HallLayoutPresentation.HallLayout.RemoveShape(item.Shape);
                        }
                        groupItem.MarkRedo(undoRedoCommand);

                        this.SelectionService.SelectItem(groupItem);
                        stateTransition.Consistent = true;
                    }

                    this.MarkRedo(undoRedoCommand);

                }));

            }
        }

        /// <MetaDataID>{aa552cec-7065-46f1-9f67-47464f3ca110}</MetaDataID>
        private void Group_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            int count = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                         where item.ParentID == Guid.Empty
                         select item).Count();

            e.CanExecute = count > 1;
        }
        private bool GroupEnabled()
        {
            int count = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                         where item.ParentID == Guid.Empty
                         select item).Count();

            return count > 1;
        }
        /// <MetaDataID>{6cfbd519-ad98-4b17-8d2c-98f072757f61}</MetaDataID>
        public bool Group_CanExecute(object sender)
        {
            int count = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                         where item.ParentID == Guid.Empty
                         select item).Count();

            return count > 1;
        }



        #endregion

        #region Ungroup Command

        /// <MetaDataID>{c8d6d72a-1ad6-4e54-b2e7-9cd43dc34c83}</MetaDataID>
        public void Ungroup_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var groups = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                          where item.IsGroup && item.ParentID == Guid.Empty
                          select item).ToArray();
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                var undoRedoCommand = UndoRedoManager.NewCommand();
                this.MarkUndo(undoRedoCommand);

                foreach (DesignerItem groupRoot in groups)
                {

                    groupRoot.UpdateShape(undoRedoCommand);

                    ShapesGroup shapesGroup = groupRoot.Shape as ShapesGroup;
                    var children = from child in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                   where child.ParentID == groupRoot.ID
                                   select child;

                    Canvas groupCanvas = (groupRoot.Content as Grid).Children.Cast<UIElement>().OfType<Viewbox>().FirstOrDefault().Child as Canvas;
                    foreach (DesignerItem child in children.ToList())
                    {

                        child.ParentID = Guid.Empty;
                        var left = Canvas.GetLeft(groupRoot) + Canvas.GetLeft(child);
                        var top = Canvas.GetTop(groupRoot) + Canvas.GetTop(child);

                        groupCanvas.Children.Remove(child);

                        this.Children.Add(child);
                        Canvas.SetLeft(child, left);
                        Canvas.SetTop(child, top);
                        child.UpdateShape(undoRedoCommand);
                        shapesGroup.RemoveGroupedShape(child.Shape);
                        HallLayoutPresentation.HallLayout.AddShape(child.Shape);
                    }
                    groupRoot.MarkRedo(undoRedoCommand);

                    HallLayoutPresentation.HallLayout.RemoveShape(shapesGroup);


                    this.SelectionService.RemoveFromSelection(groupRoot);
                    this.Children.Remove(groupRoot);
                    UpdateZIndex();
                }

                this.MarkRedo(undoRedoCommand);

            }));
        }

        /// <MetaDataID>{42f5b426-9eb7-467a-849b-c8b0c9872de6}</MetaDataID>
        private void Ungroup_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentID != Guid.Empty
                              select item;


            e.CanExecute = groupedItem.Count() > 0;
        }

        /// <MetaDataID>{7a969de3-58f4-4851-841c-5a2c976b9a8b}</MetaDataID>
        public bool Ungroup_CanExecute(object sender)
        {
            var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                              where item.ParentID != Guid.Empty
                              select item;
            return groupedItem.Count() > 0;
        }

        #endregion

        #region BringForward Command

        /// <MetaDataID>{0c1184fe-d0a7-499b-8385-baf5e83e7ba1}</MetaDataID>
        private void BringForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) descending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Min(count - 1 - i, currentIndex + 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }

            UpdateHallLayout();

        }
        public bool Order_Enabled()
        {
            return SelectionService.CurrentSelection.Count() > 0;
        }
        /// <MetaDataID>{e7b30469-ff97-4270-b36a-a7ce6f103b0f}</MetaDataID>
        private void Order_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = SelectionService.CurrentSelection.Count() > 0;
            e.CanExecute = true;
        }

        #endregion

        #region BringToFront Command

        /// <MetaDataID>{a1a55a19-6883-4d43-9ba9-1f24af97c367}</MetaDataID>
        public void BringToFront_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var undoRedoCommand = UndoRedoManager.NewCommand();
            BringSelectedItemsToFront(undoRedoCommand);
        }

        private void BringSelectedItemsToFront(UndoRedoCommand undoRedoCommand)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();

            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, childrenSorted.Count - selectionSorted.Count + j++);
                }
                else
                {
                    Canvas.SetZIndex(item, i++);
                }
            }
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {

                foreach (UIElement item in childrenSorted)
                {
                    if (item is DesignerItem)
                        (item as DesignerItem).UpdateShape(undoRedoCommand);
                }

            }));
            UpdateHallLayout(undoRedoCommand);
        }

        #endregion

        #region SendBackward Command

        /// <MetaDataID>{f21ea39a-e32e-445f-b229-007e6edb219a}</MetaDataID>
        private void SendBackward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> ordered = (from item in SelectionService.CurrentSelection
                                       orderby Canvas.GetZIndex(item as UIElement) ascending
                                       select item as UIElement).ToList();

            int count = this.Children.Count;

            for (int i = 0; i < ordered.Count; i++)
            {
                int currentIndex = Canvas.GetZIndex(ordered[i]);
                int newIndex = Math.Max(i, currentIndex - 1);
                if (currentIndex != newIndex)
                {
                    Canvas.SetZIndex(ordered[i], newIndex);
                    IEnumerable<UIElement> it = this.Children.OfType<UIElement>().Where(item => Canvas.GetZIndex(item) == newIndex);

                    foreach (UIElement elm in it)
                    {
                        if (elm != ordered[i])
                        {
                            Canvas.SetZIndex(elm, currentIndex);
                            break;
                        }
                    }
                }
            }
            UpdateHallLayout();
        }

        #endregion

        #region SendToBack Command

        /// <MetaDataID>{3c3029e9-8bb0-422d-badc-757486b9601d}</MetaDataID>
        public void SendToBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<UIElement> selectionSorted = (from item in SelectionService.CurrentSelection
                                               orderby Canvas.GetZIndex(item as UIElement) ascending
                                               select item as UIElement).ToList();

            List<UIElement> childrenSorted = (from UIElement item in this.Children
                                              orderby Canvas.GetZIndex(item as UIElement) ascending
                                              select item as UIElement).ToList();
            int i = 0;
            int j = 0;
            foreach (UIElement item in childrenSorted)
            {
                if (selectionSorted.Contains(item))
                {
                    int idx = Canvas.GetZIndex(item);
                    Canvas.SetZIndex(item, j++);

                }
                else
                {
                    Canvas.SetZIndex(item, selectionSorted.Count + i++);
                }
            }
            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
            {
                var undoRedoCommand = UndoRedoManager.NewCommand();
                foreach (UIElement item in childrenSorted)
                {
                    if (item is DesignerItem)
                        (item as DesignerItem).UpdateShape(undoRedoCommand);
                }

            }));
        }

        #endregion




        /// <MetaDataID>{c993e30c-28ec-4362-9270-9a10772c9203}</MetaDataID>
        private void RotateSelections(double angle)
        {
            foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                FrameworkElement element = item.Content as FrameworkElement;
                if (element != null)
                {
                    RotateTransform rotateTransform = element.LayoutTransform as RotateTransform;
                    if (rotateTransform == null)
                    {
                        rotateTransform = new RotateTransform();
                        element.LayoutTransform = rotateTransform;
                    }

                    rotateTransform.Angle = (rotateTransform.Angle + angle) % 360;
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - (item.Height - item.Width) / 2);
                    Canvas.SetTop(item, Canvas.GetTop(item) - (item.Width - item.Height) / 2);
                    double width = item.Width;
                    item.Width = item.Height;
                    item.Height = width;
                }
            }
        }


        /// <MetaDataID>{8f080ca3-886d-4431-9332-f7c66350b690}</MetaDataID>
        private void Rotate_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RotateSelections(90);
        }


        /// <MetaDataID>{3c6007b6-eb38-472e-9097-a85ec9dc68b0}</MetaDataID>
        private void Rotate_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }



        #region Same Width Command

        /// <MetaDataID>{eb874885-a76e-469d-b547-7c2cc9ca4ca1}</MetaDataID>
        public void SameWidth_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double witdth = selectedItems.First().ActualWidth;

                foreach (DesignerItem item in selectedItems)
                {
                    item.Width = witdth;
                }
            }

            UpdateHallLayout();
        }

        /// <MetaDataID>{839036fb-ce4f-4e06-b620-65476767919a}</MetaDataID>
        private void SameWidth_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion



        #region Same Height Command
        public void SameHeight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double height = selectedItems.First().ActualHeight;

                foreach (DesignerItem item in selectedItems)
                    item.Height = height;
            }
            UpdateHallLayout();
        }

        private void SameHeight_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }
        #endregion

        #region Same Size Command
        public void SameSize_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double height = selectedItems.First().ActualHeight;
                double width = selectedItems.First().ActualWidth;

                foreach (DesignerItem item in selectedItems)
                {
                    item.Height = height;
                    item.Width = width;
                }
            }
            UpdateHallLayout();
        }


        public DesignerItem SelectedShape
        {
            get
            {
                var selectedItems = (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                     where item.ParentID == Guid.Empty
                                     select item).ToArray();
                if (selectedItems.Length == 1)
                    return selectedItems[0];
                else
                    return null;

            }
        }


        public DesignerItem[] SelectedShapes
        {
            get
            {
                return (from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                        where item.ParentID == Guid.Empty
                        select item).ToArray();

            }
        }

        private void SameSize_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }
        #endregion
        public void Undo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FloorLayoutDesigner.UndoRedoManager.Undo();
        }
        public bool Undo_Enabled(object sender)
        {
            return FloorLayoutDesigner.UndoRedoManager.HasUndoCommands;
        }

        public void Redo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FloorLayoutDesigner.UndoRedoManager.Redo();
        }
        public bool Redo_Enabled(object sender)
        {
            return FloorLayoutDesigner.UndoRedoManager.HasRedoCommands;
        }

        #region AlignTop Command

        /// <MetaDataID>{eb874885-a76e-469d-b547-7c2cc9ca4ca1}</MetaDataID>
        public void AlignTop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = top - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
            UpdateHallLayout();
        }

        /// <MetaDataID>{839036fb-ce4f-4e06-b620-65476767919a}</MetaDataID>
        private void Align_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region AlignVerticalCenters Command

        /// <MetaDataID>{6fb6c79f-bd68-4485-91c0-af5d9fcb2edb}</MetaDataID>
        public void AlignVerticalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
            UpdateHallLayout();
        }

        #endregion

        #region AlignBottom Command

        /// <MetaDataID>{3cf9da59-107d-4a82-ac78-3346d63c4bb0}</MetaDataID>
        public void AlignBottom_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double bottom = Canvas.GetTop(selectedItems.First()) + selectedItems.First().Height;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = bottom - (Canvas.GetTop(item) + item.Height);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                }
            }
            UpdateHallLayout();
        }

        #endregion

        #region AlignLeft Command

        /// <MetaDataID>{710f1f95-a6a2-4cd6-afee-c56ff5d6cae1}</MetaDataID>
        public void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = left - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
            UpdateHallLayout();
        }

        #endregion

        #region AlignHorizontalCenters Command

        /// <MetaDataID>{aa365ae5-2315-410f-9982-79f841a6d37b}</MetaDataID>
        public void AlignHorizontalCenters_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double center = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width / 2;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = center - (Canvas.GetLeft(item) + item.Width / 2);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
            UpdateHallLayout();
        }

        #endregion

        #region AlignRight Command

        /// <MetaDataID>{8b686f12-28a5-4639-aad6-52f8cc75184f}</MetaDataID>
        public void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                select item;

            if (selectedItems.Count() > 1)
            {
                double right = Canvas.GetLeft(selectedItems.First()) + selectedItems.First().Width;

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = right - (Canvas.GetLeft(item) + item.Width);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                }
            }
            UpdateHallLayout();
        }

        #endregion

        #region DistributeHorizontal Command

        /// <MetaDataID>{18acd1af-fa20-44cb-9386-c76878b84ab4}</MetaDataID>
        public void DistributeHorizontal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemLeft = Canvas.GetLeft(item)
                                orderby itemLeft
                                select item;

            if (selectedItems.Count() > 1)
            {
                double left = Double.MaxValue;
                double right = Double.MinValue;
                double sumWidth = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    left = Math.Min(left, Canvas.GetLeft(item));
                    right = Math.Max(right, Canvas.GetLeft(item) + item.Width);
                    sumWidth += item.Width;
                }

                double distance = Math.Max(0, (right - left - sumWidth) / (selectedItems.Count() - 1));
                double offset = Canvas.GetLeft(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetLeft(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetLeft(di, Canvas.GetLeft(di) + delta);
                    }
                    offset = offset + item.Width + distance;
                }
            }
            UpdateHallLayout();
        }

        /// <MetaDataID>{7b1aa912-6324-40e0-91db-f0d571e0262f}</MetaDataID>
        private void Distribute_Enabled(object sender, CanExecuteRoutedEventArgs e)
        {
            //var groupedItem = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
            //                  where item.ParentID == Guid.Empty
            //                  select item;


            //e.CanExecute = groupedItem.Count() > 1;
            e.CanExecute = true;
        }

        #endregion

        #region DistributeVertical Command

        /// <MetaDataID>{a981d34a-f84f-4acf-9662-5ced93f65f3c}</MetaDataID>
        public void DistributeVertical_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedItems = from item in SelectionService.CurrentSelection.OfType<DesignerItem>()
                                where item.ParentID == Guid.Empty
                                let itemTop = Canvas.GetTop(item)
                                orderby itemTop
                                select item;

            if (selectedItems.Count() > 1)
            {
                double top = Double.MaxValue;
                double bottom = Double.MinValue;
                double sumHeight = 0;
                foreach (DesignerItem item in selectedItems)
                {
                    top = Math.Min(top, Canvas.GetTop(item));
                    bottom = Math.Max(bottom, Canvas.GetTop(item) + item.Height);
                    sumHeight += item.Height;
                }

                double distance = Math.Max(0, (bottom - top - sumHeight) / (selectedItems.Count() - 1));
                double offset = Canvas.GetTop(selectedItems.First());

                foreach (DesignerItem item in selectedItems)
                {
                    double delta = offset - Canvas.GetTop(item);
                    foreach (DesignerItem di in SelectionService.GetGroupMembers(item))
                    {
                        if (di.ParentID == Guid.Empty)
                            Canvas.SetTop(di, Canvas.GetTop(di) + delta);
                    }
                    offset = offset + item.Height + distance;
                }
            }
            UpdateHallLayout();
        }

        #endregion

        #region SelectAll Command

        /// <MetaDataID>{eae17315-a5d1-491c-8463-e3ebf621c2a6}</MetaDataID>
        private void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SelectionService.SelectAll();
        }

        #endregion

        #region Helper Methods

        /// <MetaDataID>{bdbe2ea5-3a2a-4cad-a9f3-17d4725f1329}</MetaDataID>
        private XElement LoadSerializedDataFromFile()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Designer Files (*.xml)|*.xml|All Files (*.*)|*.*";

            if (openFile.ShowDialog() == true)
            {
                try
                {
                    return XElement.Load(openFile.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        /// <MetaDataID>{d6a4cb00-5b29-41c0-acba-24ace09be9ac}</MetaDataID>
        void SaveFile(XElement xElement)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    xElement.Save(saveFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <MetaDataID>{83b58d77-da97-42b6-82f1-64dd5f1c3a80}</MetaDataID>
        private XElement LoadSerializedDataFromClipBoard()
        {
            if (Clipboard.ContainsData(DataFormats.Xaml))
            {
                String clipboardData = Clipboard.GetData(DataFormats.Xaml) as String;

                if (String.IsNullOrEmpty(clipboardData))
                    return null;
                try
                {
                    return XElement.Load(new StringReader(clipboardData));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, e.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return null;
        }

        /// <MetaDataID>{f3390670-c6d2-4289-8c21-91861f5584b0}</MetaDataID>
        private XElement SerializeDesignerItems(IEnumerable<DesignerItem> designerItems)
        {
            XElement serializedItems = new XElement("DesignerItems",
                                       from item in designerItems
                                       let contentXaml = GetContentXaml(item)
                                       select new XElement("DesignerItem",
                                                  new XElement("Left", Canvas.GetLeft(item)),
                                                  new XElement("Top", Canvas.GetTop(item)),
                                                  new XElement("Width", item.Width),
                                                  new XElement("Height", item.Height),
                                                  new XElement("ID", item.ID),
                                                  new XElement("zIndex", Canvas.GetZIndex(item)),
                                                  new XElement("IsGroup", item.IsGroup),
                                                  new XElement("ParentID", item.ParentID),
                                                  new XElement("RotationAngle", item.RotationDeegrees),
                                                  new XElement("ShapeImageUrl", GetImageUrl(item)),
                                                  new XElement("Content", contentXaml)
                                              )
                                   );

            return serializedItems;
        }

        private object GetImageUrl(DesignerItem item)
        {
            if (item.Shape != null)
                return item.Shape.ShapeImageUrl;
            else
                return "";
        }

        private string GetContentXaml(DesignerItem item)
        {
            if (item.Content is SvgHostControl)
                return "";
            object content = item.Content;
            if (item.IsGroup)
                return "";
            if (content != null)
                return XamlWriter.Save(content);
            else
                return "";
        }

        /// <MetaDataID>{66a1a283-4a01-445e-b555-492225c841f6}</MetaDataID>
        private XElement SerializeConnections(IEnumerable<Connection> connections)
        {
            var serializedConnections = new XElement("Connections",
                           from connection in connections
                           select new XElement("Connection",
                                      new XElement("SourceID", connection.Source.ParentDesignerItem.ID),
                                      new XElement("SinkID", connection.Sink.ParentDesignerItem.ID),
                                      new XElement("SourceConnectorName", connection.Source.Name),
                                      new XElement("SinkConnectorName", connection.Sink.Name),
                                      new XElement("SourceArrowSymbol", connection.SourceArrowSymbol),
                                      new XElement("SinkArrowSymbol", connection.SinkArrowSymbol),
                                      new XElement("zIndex", Canvas.GetZIndex(connection))
                                     )
                                  );

            return serializedConnections;
        }

        /// <MetaDataID>{3cce10c0-b5fa-4789-b15c-23e2d596f135}</MetaDataID>
        private static DesignerItem DeserializeDesignerItem(XElement itemXML, Guid id, double OffsetX, double OffsetY)
        {
            DesignerItem item = new DesignerItem(id);
            item.Width = Double.Parse(itemXML.Element("Width").Value, CultureInfo.InvariantCulture);
            item.Height = Double.Parse(itemXML.Element("Height").Value, CultureInfo.InvariantCulture);
            item.ParentID = new Guid(itemXML.Element("ParentID").Value);
            item.IsGroup = Boolean.Parse(itemXML.Element("IsGroup").Value);

            item.RotationDeegrees = int.Parse(itemXML.Element("RotationAngle").Value);

            if (item.ParentID == Guid.Empty)
            {
                Canvas.SetLeft(item, Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture) + OffsetX);
                Canvas.SetTop(item, Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture) + OffsetY);
            }
            else
            {
                var left = Double.Parse(itemXML.Element("Left").Value, CultureInfo.InvariantCulture);
                var top = Double.Parse(itemXML.Element("Top").Value, CultureInfo.InvariantCulture);
                if (left < 0)
                    left = 0;
                if (top < 0)
                    top = 0;


                Canvas.SetLeft(item, left);
                Canvas.SetTop(item, top);
            }

            Canvas.SetZIndex(item, Int32.Parse(itemXML.Element("zIndex").Value));
            if (!string.IsNullOrWhiteSpace(itemXML.Element("Content").Value))
            {
                Object content = XamlReader.Load(XmlReader.Create(new StringReader(itemXML.Element("Content").Value)));
                item.Content = content;
            }
            else if (!string.IsNullOrWhiteSpace(itemXML.Element("ShapeImageUrl").Value))
            {
                var svgHostControl = new SvgHostControl();
                svgHostControl.svgHost.Source = new Uri(itemXML.Element("ShapeImageUrl").Value);
                item.Content = svgHostControl;
            }

            return item;
        }

        /// <MetaDataID>{af037ac7-f339-4ab1-8931-f751ff05df3a}</MetaDataID>
        private void CopyCurrentSelection()
        {
            IEnumerable<DesignerItem> selectedDesignerItems =
                this.SelectionService.CurrentSelection.OfType<DesignerItem>();

            List<Connection> selectedConnections =
                this.SelectionService.CurrentSelection.OfType<Connection>().ToList();

            foreach (Connection connection in this.Children.OfType<Connection>())
            {
                if (!selectedConnections.Contains(connection))
                {
                    DesignerItem sourceItem = (from item in selectedDesignerItems
                                               where item.ID == connection.Source.ParentDesignerItem.ID
                                               select item).FirstOrDefault();

                    DesignerItem sinkItem = (from item in selectedDesignerItems
                                             where item.ID == connection.Sink.ParentDesignerItem.ID
                                             select item).FirstOrDefault();

                    if (sourceItem != null &&
                        sinkItem != null &&
                        BelongToSameGroup(sourceItem, sinkItem))
                    {
                        selectedConnections.Add(connection);
                    }
                }
            }
            double? left = null;
            double? top = null;

            foreach (var designerItem in selectedDesignerItems)
            {
                if (designerItem.ParentID == Guid.Empty && designerItem.Shape != null)
                {
                    if (left == null || left.Value > designerItem.Shape.Left)
                        left = designerItem.Shape.Left;

                    if (top == null || top.Value > designerItem.Shape.Top)
                        top = designerItem.Shape.Top;
                }
            }

            XElement designerItemsXML = SerializeDesignerItems(selectedDesignerItems);
            XElement connectionsXML = SerializeConnections(selectedConnections);

            XElement root = new XElement("Root");
            root.Add(designerItemsXML);
            root.Add(connectionsXML);

            root.Add(new XAttribute("OffsetX", 10));
            root.Add(new XAttribute("OffsetY", 10));

            root.Add(new XAttribute("Left", left.Value));
            root.Add(new XAttribute("Top", top.Value));


            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Xaml, root);
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paste)));

        }

        /// <MetaDataID>{5e557c51-11dc-4587-a666-7001431a4684}</MetaDataID>
        private void DeleteCurrentSelection()
        {
            foreach (Connection connection in SelectionService.CurrentSelection.OfType<Connection>())
            {
                this.Children.Remove(connection);
            }

            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
              {
                  var undoRedoCommand = UndoRedoManager.NewCommand();
                  this.MarkUndo(undoRedoCommand);

                  foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
                  {
                      Control cd = item.Template.FindName("PART_ConnectorDecorator", item) as Control;

                      List<Connector> connectors = new List<Connector>();
                      GetConnectors(cd, connectors);

                      foreach (Connector connector in connectors)
                      {
                          foreach (Connection con in connector.Connections)
                          {
                              this.Children.Remove(con);
                          }
                      }

                      this.Children.Remove(item);

                      if (item.Shape != null)
                          HallLayoutPresentation.HallLayout.RemoveShape(item.Shape);
                  }
                  this.MarkRedo(undoRedoCommand);
              }));

            SelectionService.ClearSelection();
            UpdateZIndex();
        }

        /// <MetaDataID>{f0bd2955-a0f7-4fd3-8fbf-790eb22b06c1}</MetaDataID>
        private void UpdateZIndex()
        {
            List<UIElement> ordered = (from UIElement item in this.Children
                                       orderby Canvas.GetZIndex(item as UIElement)
                                       select item as UIElement).ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                Canvas.SetZIndex(ordered[i], i);
            }
        }

        /// <MetaDataID>{509c4932-c79f-4c9f-9716-ace9365032d7}</MetaDataID>
        private static Rect GetBoundingRectangle(IEnumerable<DesignerItem> items)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (DesignerItem item in items)
            {
                x1 = Math.Min(Canvas.GetLeft(item), x1);
                y1 = Math.Min(Canvas.GetTop(item), y1);

                x2 = Math.Max(Canvas.GetLeft(item) + item.Width, x2);
                y2 = Math.Max(Canvas.GetTop(item) + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }

        private static Rect GetBoundingRectangle(IEnumerable<Shape> items)
        {
            double x1 = Double.MaxValue;
            double y1 = Double.MaxValue;
            double x2 = Double.MinValue;
            double y2 = Double.MinValue;

            foreach (Shape item in items)
            {
                x1 = Math.Min(item.Left, x1);
                y1 = Math.Min(item.Top, y1);

                x2 = Math.Max(item.Left + item.Width, x2);
                y2 = Math.Max(item.Top + item.Height, y2);
            }

            return new Rect(new Point(x1, y1), new Point(x2, y2));
        }


        /// <MetaDataID>{c52cb494-5a66-4729-87fd-1b76f1214efe}</MetaDataID>
        private void GetConnectors(DependencyObject parent, List<Connector> connectors)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is Connector)
                {
                    connectors.Add(child as Connector);
                }
                else
                    GetConnectors(child, connectors);
            }
        }

        /// <MetaDataID>{86bef08c-e263-40f7-8a3e-d8cd00c2878a}</MetaDataID>
        private Connector GetConnector(Guid itemID, String connectorName)
        {
            DesignerItem designerItem = (from item in this.Children.OfType<DesignerItem>()
                                         where item.ID == itemID
                                         select item).FirstOrDefault();

            Control connectorDecorator = designerItem.Template.FindName("PART_ConnectorDecorator", designerItem) as Control;
            connectorDecorator.ApplyTemplate();

            return connectorDecorator.Template.FindName(connectorName, connectorDecorator) as Connector;
        }

        /// <MetaDataID>{0f229f9a-ed82-4421-a1f0-b9e6e8117de7}</MetaDataID>
        private bool BelongToSameGroup(IGroupable item1, IGroupable item2)
        {
            IGroupable root1 = SelectionService.GetGroupRoot(item1);
            IGroupable root2 = SelectionService.GetGroupRoot(item2);

            return (root1.ID == root2.ID);
        }

        #endregion
    }
}
