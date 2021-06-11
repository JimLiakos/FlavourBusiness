using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;
using FloorLayoutDesigner.Controls;
using OOAdvantech.Transactions;
using FloorLayoutDesigner;
using RestaurantHallLayoutModel;
using System.Collections.Generic;
using OOAdvantech.PersistenceLayer;
using WPFUIElementObjectBind;
using System.ComponentModel;
using MenuPresentationModel.MenuStyles;
using UIBaseEx;
using FloorLayoutDesigner.ViewModel;
using FlavourBusinessFacade.ServicesContextResources;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using MenuModel;
using MenuItemsEditor.ViewModel;

namespace FloorLayoutDesigner
{
    //These attributes identify the types of the named parts that are used for templating
    /// <MetaDataID>{99f297d6-bc59-4c78-b2c1-2da41a72c000}</MetaDataID>
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class DesignerItem : ContentControl, ISelectable, IGroupable, ICommandTargetObject, INotifyPropertyChanged
    {
        #region ID
        private Guid id;
        internal Shape Shape;

        public Guid ID
        {
            get { return id; }
        }
        #endregion

        #region ParentID
        public Guid ParentID
        {
            get { return (Guid)GetValue(ParentIDProperty); }
            set { SetValue(ParentIDProperty, value); }
        }
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(DesignerItem));
        #endregion

        #region IsGroup
        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }

        int _RotationDeegrees = 0;
        public int RotationDeegrees
        {
            get
            {

                return _RotationDeegrees;
            }
            set
            {
                _RotationDeegrees = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RotationDeegrees)));
                RotateTransform.Angle = value;
                RotateTransform.CenterX = Width / 2;
                RotateTransform.CenterY = Height / 2;


            }
        }


        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(DesignerItem));
        #endregion

        #region IsSelected Property

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set
            {

                if ((bool)GetValue(IsSelectedProperty) != value)
                {

                    if (!value && HasChanges)
                    {
                        if (this.GetObjectContext() != null)
                        {
                            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                            {
                                var undoRedoCommand = UndoRedoManager.NewCommand();
                                UpdateShape(undoRedoCommand);
                            }));
                        }
                    }
                    SetValue(IsSelectedProperty, value);
                }
            }
        }
        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected",
                                       typeof(bool),
                                       typeof(DesignerItem),
                                       new FrameworkPropertyMetadata(false));

        #endregion

        #region DragThumbTemplate Property

        // can be used to replace the default template for the DragThumb
        public static readonly DependencyProperty DragThumbTemplateProperty =
            DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetDragThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(DragThumbTemplateProperty);
        }

        public static void SetDragThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragThumbTemplateProperty, value);
        }

        #endregion

        #region ConnectorDecoratorTemplate Property

        // can be used to replace the default template for the ConnectorDecorator
        public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached("ConnectorDecoratorTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(ConnectorDecoratorTemplateProperty);
        }

        public static void SetConnectorDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectorDecoratorTemplateProperty, value);
        }

        #endregion

        #region IsDragConnectionOver

        // while drag connection procedure is ongoing and the mouse moves over 
        // this item this value is true; if true the ConnectorDecorator is triggered
        // to be visible, see template
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }
        public object ItemContent
        {
            get
            {
                return Content;
                //return ContentGrid.Children.OfType<UIElement>().FirstOrDefault();
            }
            set
            {
                Content = value;
                //if (value is UIElement)
                //{
                //    if (ContentGrid.Children.OfType<UIElement>().FirstOrDefault() != null)
                //        ContentGrid.Children.RemoveAt(0);

                //    ContentGrid.Children.Add(value as UIElement);
                //}
            }
        }

        internal bool HasChanges
        {
            get
            {
                bool hasChanges = false;
                if (ItemContent is SvgHostControl)
                {
                    hasChanges |= ((int)Shape.Left) != ((int)Canvas.GetLeft(this) + ((int)((ItemContent as SvgHostControl).svgHost).TranslatePoint(new Point(0, 0), this).X));
                    hasChanges |= ((int)Shape.Top) != ((int)Canvas.GetTop(this) + ((int)((ItemContent as SvgHostControl).svgHost).TranslatePoint(new Point(0, 0), this).Y));
                    if ((ItemContent as SvgHostControl).svgHost.ActualWidth == 0 && (ItemContent as SvgHostControl).svgHost.ActualHeight == 0)
                    {
                        hasChanges |= ((int)Shape.Width) != ((int)Width);
                        hasChanges |= ((int)Shape.Height) != ((int)Height);
                    }
                    else
                    {
                        hasChanges |= ((int)Shape.Width) != ((int)(ItemContent as SvgHostControl).svgHost.ActualWidth);
                        hasChanges |= ((int)Shape.Height) != ((int)(ItemContent as SvgHostControl).svgHost.ActualHeight);
                    }
                    hasChanges |= Shape.ZIndex != Canvas.GetZIndex(this);
                    hasChanges |= Shape.RotateAngle != RotationDeegrees;
                }
                else
                {
                    hasChanges |= ((int)Shape.Left) != ((int)Canvas.GetLeft(this));
                    hasChanges |= ((int)Shape.Top) != ((int)Canvas.GetTop(this));
                    hasChanges |= ((int)Shape.Width) != ((int)Width);
                    hasChanges |= ((int)Shape.Height) != ((int)Height);
                    hasChanges |= Shape.ZIndex != Canvas.GetZIndex(this);
                    hasChanges |= Shape.RotateAngle != RotationDeegrees;

                }
                return hasChanges;
            }
        }
        internal void UpdateShape(FloorLayoutDesigner.UndoRedoCommand undoRedoCommand)
        {
            if (Shape != null)
            {
                if (undoRedoCommand != null)
                    this.MarkUndo(undoRedoCommand);

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    if (ItemContent is SvgHostControl)
                    {
                        Shape.Left = Canvas.GetLeft(this) + ((ItemContent as SvgHostControl).svgHost).TranslatePoint(new Point(0, 0), this).X;
                        Shape.Top = Canvas.GetTop(this) + ((ItemContent as SvgHostControl).svgHost).TranslatePoint(new Point(0, 0), this).Y;
                        if ((ItemContent as SvgHostControl).svgHost.ActualWidth == 0 && (ItemContent as SvgHostControl).svgHost.ActualHeight == 0)
                        {
                            Shape.Width = Width;
                            Shape.Height = Height;
                        }
                        else
                        {
                            Shape.Width = (ItemContent as SvgHostControl).svgHost.ActualWidth;
                            Shape.Height = (ItemContent as SvgHostControl).svgHost.ActualHeight;
                        }
                        Shape.ZIndex = Canvas.GetZIndex(this);
                        Shape.RotateAngle = RotationDeegrees;
                    }
                    else
                    {
                        Shape.Left = Canvas.GetLeft(this);
                        Shape.Top = Canvas.GetTop(this);
                        Shape.Width = this.ActualWidth;
                        Shape.Height = this.ActualHeight;
                        if (Shape.Width == 0 && Shape.Height == 0)
                        {
                            Shape.Width = Width;
                            Shape.Height = Height;
                        }
                        Shape.ZIndex = Canvas.GetZIndex(this);
                        Shape.RotateAngle = RotationDeegrees;
                    }
                    stateTransition.Consistent = true;
                }
                if (undoRedoCommand != null)
                    this.MarkRedo(undoRedoCommand);

                else
                {
                }
            }
        }

        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register("IsDragConnectionOver",
                                         typeof(bool),
                                         typeof(DesignerItem),
                                         new FrameworkPropertyMetadata(false));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        static DesignerItem()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));

            var img = LabelBkgImage;
            img = FontImage;
            img = RenameImage;
            img = ServicePointImage;
            img = CopyImage;
            img = CutImage;
            img = PasteImage;
        }



        internal Grid ContentGrid;
        RotateTransform RotateTransform = new RotateTransform(0, 0, 0);
        public DesignerItem(Guid id)
        {
            this.id = id;
            this.Loaded += new RoutedEventHandler(DesignerItem_Loaded);
            this.Unloaded += DesignerItem_Unloaded;
            this.SizeChanged += DesignerItem_SizeChanged;

            ResourceDictionary resourceDictionary = new ResourceDictionary();

            Resources.Source = new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;component/RestaurantHallLayoutDesigner.xaml");
            ContentGrid = new Grid();

            InitContextMenuCommands();

        }

        private void InitContextMenuCommands()
        {
            LabelFontCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    designer.LabelFont_Executed(sender, null);

            });
            LabelBackgroundCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    designer.LabelBackground_Executed(sender, null);
            });

            EditLabelCommand= new RelayCommand((object sender) =>
            {
                ServicePointLabelEditPopupIsOpen = true;

            });

            ShapeServicepointToggleCommand = new RelayCommand((object sender) =>
            {
                //var objectContext = (ShapeServicepointToggleCommand.UserInterfaceObjectConnection.ContainerControl as FrameworkElement).GetObjectContext();
                this.GetObjectContext().RunUnderContextTransaction(new Action(async () =>
               {
                   await ShapeServicepointToggle();

                   if (string.IsNullOrWhiteSpace(Shape.ServicesPointIdentity))
                   {
                       ServicePointAssignment.Header = Properties.Resources.AssignServicePointMenuItemHeader;

                       if (_ContextMenuItems.Contains(EditMenuCommand))
                           _ContextMenuItems.Remove(EditMenuCommand);
                   }
                   else
                   {
                       ServicePointAssignment.Header = Properties.Resources.DeassignServicePointMenuItemHeader;
                       if (!_ContextMenuItems.Contains(EditMenuCommand))
                           _ContextMenuItems.Insert(1, EditMenuCommand);

                   }
                   var index = _ContextMenuItems.IndexOf(ServicePointAssignment);
                   _ContextMenuItems.Remove(ServicePointAssignment);
                   _ContextMenuItems.Insert(index, ServicePointAssignment);

               }));

            });
            CutCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    designer.Cut_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    return designer.CutEnabled();
                return false;

            });
            CopyCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    designer.Copy_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    return designer.CopyEnabled();
                return false;

            });
            PasteCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    designer.Paste_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    return designer.PasteEnabled();
                return false;

            });
            GroupCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    designer.Group_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    return designer.Group_CanExecute(sender);
                return false;

            });
            UngroupCommand = new RelayCommand((object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    designer.Ungroup_Executed(sender, null);

            }, (object sender) =>
            {
                DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this);
                if (designer != null)
                    return designer.Ungroup_CanExecute(sender);
                return false;

            });
        }

        private async Task ShapeServicepointToggle()
        {
            var serviceArea = HallLayoutViewModel.HallLayout.ServiceArea;

            if (!string.IsNullOrWhiteSpace(Shape.ServicesPointIdentity))
            {
                Shape.ServicesPointIdentity = null;
                Shape.Label = null;
                ServicesPoints = new List<IServicePoint>();
            }
            else
            {
                var hallServicesPoints = HallLayoutViewModel.HallLayout.Shapes.Select(x => x.ServicesPointIdentity).ToList();
                List<IServicePoint> unassignedServicePoints = HallLayoutViewModel.ServiceAreaViewModel.ServicePoints.Where(x => !hallServicesPoints.Contains(x.ServicePoint.ServicesPointIdentity)).OrderBy(x=>x.ServicePoint.Description).Select(x => x.ServicePoint).ToList();
                //List<IServicePoint> unassignedServicePoints = serviceArea.GetUnassignedServicePoints(hallServicesPoints);


                ServicesPoints = unassignedServicePoints;
                if (unassignedServicePoints.Count > 0)
                {
                    if(unassignedServicePoints.Count > 1)
                        ServicePointsPopupIsOpen = true;
                    else
                    {
                        this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                        {
                            AssignServicePoint(unassignedServicePoints[0]);
                        }));
                    }
                }
                else
                {
                    IServicePointViewModel newServicePoint = await HallLayoutViewModel.ServiceAreaViewModel.NewServicePoint();
                    this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                    {
                        AssignServicePoint(newServicePoint.ServicePoint);
                    }));

                }
            }
        }

        DesignerItem RootDesignerItem
        {
            get
            {
                var designerItem = this;
                while (designerItem.ParentID != Guid.Empty)
                {
                    DesignerCanvas designer = WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(designerItem);
                    designerItem = designer.Children.OfType<DesignerItem>().Where(x => x.ID == designerItem.ParentID).FirstOrDefault();
                }
                return designerItem;
            }
        }
        public RelayCommand LabelFontCommand { get; protected set; }
        public RelayCommand EditLabelCommand { get; protected set; }
        public RelayCommand LabelBackgroundCommand { get; protected set; }
        public RelayCommand ShapeServicepointToggleCommand { get; protected set; }
        public RelayCommand CutCommand { get; protected set; }
        public RelayCommand CopyCommand { get; protected set; }
        public RelayCommand PasteCommand { get; protected set; }
        public RelayCommand GroupCommand { get; protected set; }
        public RelayCommand UngroupCommand { get; protected set; }



        MenuCommand ServicePointAssignment;

        MenuCommand EditMenuCommand;
        /// <exclude>Excluded</exclude>
        System.Collections.ObjectModel.ObservableCollection<MenuCommand> _ContextMenuItems = new System.Collections.ObjectModel.ObservableCollection<MenuCommand>();
        /// <MetaDataID>{abfe64a8-9a3a-4066-bd68-960d50ab1171}</MetaDataID>
        public System.Collections.ObjectModel.ObservableCollection<MenuCommand> ContextMenuItems
        {
            get
            {

                if (RootDesignerItem != this)
                    return RootDesignerItem.ContextMenuItems;

                if (_ContextMenuItems.Count == 0)
                {

                    //  _ContextMenuItems = new System.Collections.ObjectModel.ObservableCollection<MenuComamnd>()

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    MenuCommand menuItem = new MenuCommand();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/font16.png"));
                    //FontImage= new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    menuItem.Header = Properties.Resources.HallLayoutLabelFontMenuItemHeader;
                    menuItem.Icon = FontImage;// new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = LabelFontCommand;
                    _ContextMenuItems.Add(menuItem);


                    menuItem = new MenuCommand();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Rename16.png"));
                    //RenameImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    menuItem.Header = Properties.Resources.EditLabelMenuItemHeader;
                    menuItem.Icon = RenameImage;// new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditLabelCommand;
                    if (!string.IsNullOrWhiteSpace(Shape.ServicesPointIdentity))
                        _ContextMenuItems.Add(menuItem);
                    EditMenuCommand = menuItem;



                    menuItem = new MenuCommand(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/textBackground16.png"));
                    //LabelBkgImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    menuItem.Header = Properties.Resources.HallLayoutLabelBkMenuItemHeader;
                    menuItem.Icon = LabelBkgImage;// new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = LabelBackgroundCommand;


                    _ContextMenuItems.Add(menuItem);
                    //Images/Metro/ServicePoint.png
                    menuItem = new MenuCommand();
                    ServicePointAssignment = menuItem;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServicePoint.png"));
                    //ServicePointImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    if (string.IsNullOrWhiteSpace(Shape.ServicesPointIdentity))
                        ServicePointAssignment.Header = Properties.Resources.AssignServicePointMenuItemHeader;
                    else
                        ServicePointAssignment.Header = Properties.Resources.DeassignServicePointMenuItemHeader;

                    menuItem.Icon = ServicePointImage;// new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = ShapeServicepointToggleCommand;




                    _ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Cut.png"));
                    //CutImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    menuItem.Header = "Cut";
                    menuItem.Icon = CutImage;// new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = CutCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Copy.png"));
                    //CopyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    menuItem.Header = "Copy";
                    menuItem.Icon = CopyImage;// new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = CopyCommand;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Paste.png"));
                    //PasteImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    menuItem.Header = "Paste";
                    menuItem.Icon = PasteImage;// new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = PasteCommand;
                    _ContextMenuItems.Add(menuItem);


                    menuItem = new MenuCommand();
                    menuItem.Header = "Group";
                    menuItem.Icon = emptyImage;
                    var groupSubMenuCommands = new List<MenuCommand>();
                    menuItem.SubMenuCommands = groupSubMenuCommands;
                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Group.png"));
                    menuItem.Header = "Group";
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = GroupCommand;
                    groupSubMenuCommands.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Ungroup.png"));
                    menuItem.Header = "Ungroup";
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = UngroupCommand;
                    groupSubMenuCommands.Add(menuItem);


                    // menuItem.Command = CutCommand;











                }

                return _ContextMenuItems;
            }
        }

        private void DesignerItem_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Shape != null)
                Shape.ObjectStateChanged -= Shape_ObjectStateChanged;
        }

        private void Shape_ObjectStateChanged(object _object, string member)
        {

            if (member == nameof(Shape.Label))
            {
                _Label = Shape.Label;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Label)));
            }
        }

        private void DesignerItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Background = new SolidColorBrush(Colors.Coral);
                ContentPresenter contentPresenter =
                         this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                if (contentPresenter != null)
                {
                    UserControl contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UserControl;
                    if (contentVisual != null)
                    {
                        contentVisual.Height = e.NewSize.Height;
                        contentVisual.Width = e.NewSize.Width;
                    }
                }


                // UpdateShape(null);
            }
            catch (Exception error)
            {
            }
        }

        public DesignerItem()
            : this(Guid.NewGuid())
        {
        }
        public HallLayoutViewModel HallLayoutViewModel { get; set; }
        public DesignerItem(Shape shape, HallLayoutViewModel hallLayoutViewModel) : this(new Guid(shape.Identity))
        {
            this.Shape = shape;
            HallLayoutViewModel = hallLayoutViewModel;
            var servicePointPresentation = hallLayoutViewModel.ServiceAreaViewModel.ServicePoints.Where(x => x.ServicePoint.ServicesPointIdentity == shape.ServicesPointIdentity).FirstOrDefault();
            if (servicePointPresentation != null)
                _Seats = servicePointPresentation.ServicePoint.Seats;


            Width = shape.Width;
            Height = shape.Height;
            Canvas.SetLeft(this, shape.Left);
            Canvas.SetTop(this, shape.Top);

            Canvas.SetZIndex(this, shape.ZIndex);
            RotationDeegrees = (int)shape.RotateAngle;

            _Label = shape.Label;
        }


        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

            // update selection
            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                    if (this.IsSelected)
                    {
                        designer.SelectionService.RemoveFromSelection(this);
                    }
                    else
                    {
                        designer.SelectionService.AddToSelection(this);
                    }
                else if (!this.IsSelected)
                {
                    designer.SelectionService.SelectItem(this);


                }
                Focus();
            }

            e.Handled = false;
        }

        string _Label;
        public string Label
        {
            get
            {
                return _Label;
            }
            set
            {
                _Label = value;
                this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                {
                    HallLayoutViewModel.ServiceAreaViewModel.SetServicePointName(Shape.ServicesPointIdentity, _Label);
                }));


                //this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                //{

                //    //if (LabelTextBlock != null)
                //    //    LabelTextBlock.Text = value;

                //    Shape.Label = value;
                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelVisibility)));
                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Label)));
                //}));
            }
        }


        int  _Seats;
        public int Seats
        {
            get
            {
                return _Seats;
            }
            set
            {
                MealTypeViewModel rr = null;
                _Seats = value;
                this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                {
                    HallLayoutViewModel.ServiceAreaViewModel.SetServicePointSeats(Shape.ServicesPointIdentity, _Seats);
                }));
            }
        }

        public List<MealTypeViewModel> MealTypes
        {
            get
            {
                return null;
            }

        }


        bool _ServicePointsPopupIsOpen;
        public bool ServicePointsPopupIsOpen
        {
            get => _ServicePointsPopupIsOpen;
            set
            {
                _ServicePointsPopupIsOpen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServicePointsPopupIsOpen)));
            }
        }

        bool _ServicePointLabelEditPopupIsOpen;
        public bool ServicePointLabelEditPopupIsOpen
        {
            get => _ServicePointLabelEditPopupIsOpen;

            set
            {
                _ServicePointLabelEditPopupIsOpen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServicePointLabelEditPopupIsOpen)));
            }
        }


        List<IServicePoint> _ServicesPoints;
        public List<IServicePoint> ServicesPoints
        {
            get
            {
                return _ServicesPoints;
            }
            set
            {
                var sds = value.Select(x => x.Description).ToArray();
                _ServicesPoints = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServicesPoints)));
            }
        }

        /// <exclude>Excluded</exclude> 
        IServicePoint _SelectedServicesPoint;
        public IServicePoint SelectedServicesPoint
        {
            get
            {
                return _SelectedServicesPoint;
            }
            set
            {
                _SelectedServicesPoint = value;
                if (_SelectedServicesPoint != null)
                {
                    this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                    {
                        AssignServicePoint(_SelectedServicesPoint);
                    }));
                }
            }
        }

        private void AssignServicePoint(IServicePoint servicePoint)
        {
            if (servicePoint != null)
            {
                Shape.ServicesPointIdentity = servicePoint.ServicesPointIdentity;
                Shape.Label = servicePoint.Description;
                ServicePointsPopupIsOpen = false;
            }
            else
            {
                Shape.ServicesPointIdentity = null;
                Shape.Label = null;
                ServicePointsPopupIsOpen = false;
            }


            //_ContextMenuItems.Clear();// = null;
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));


            if (string.IsNullOrWhiteSpace(Shape.ServicesPointIdentity))
                ServicePointAssignment.Header = Properties.Resources.AssignServicePointMenuItemHeader;
            else
                ServicePointAssignment.Header = Properties.Resources.DeassignServicePointMenuItemHeader;
            var index = _ContextMenuItems.IndexOf(ServicePointAssignment);
            _ContextMenuItems.Remove(ServicePointAssignment);
            _ContextMenuItems.Insert(index, ServicePointAssignment);
        }

        public Visibility LabelVisibility
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Label))
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        /// <exclude>Excluded</exclude>
        static Image _FontImage;
        public static Image FontImage
        {
            get
            {
                if (_FontImage == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/font16.png"));
                    _FontImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                }
                return _FontImage;
            }
        }

        /// <exclude>Excluded</exclude>
        static Image _LabelBkgImage;
        public static Image LabelBkgImage
        {
            get
            {
                if (_LabelBkgImage == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/textBackground16.png"));
                    _LabelBkgImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                }
                return _LabelBkgImage;
            }
        }

        /// <exclude>Excluded</exclude>
        static Image _RenameImage;
        public static Image RenameImage
        {
            get
            {
                if (_RenameImage == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Rename16.png"));
                    _RenameImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                }
                return _RenameImage;
            }
        }
        /// <exclude>Excluded</exclude>
        static Image _ServicePointImage;
        public static Image ServicePointImage
        {
            get
            {
                if (_ServicePointImage == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServicePoint.png"));
                    _ServicePointImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                }
                return _ServicePointImage;
            }
        }

        /// <exclude>Excluded</exclude>
        static Image _CopyImage;
        public static Image CopyImage
        {
            get
            {
                if (_CopyImage == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Copy.png"));
                    _CopyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                }
                return _CopyImage;
            }
        }

        /// <exclude>Excluded</exclude>
        static Image _CutImage;
        public static Image CutImage
        {
            get
            {
                if (_CutImage == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Cut.png"));
                    _CutImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                }
                return _CutImage;
            }
        }

        /// <exclude>Excluded</exclude>
        static Image _PasteImage;
        public static Image PasteImage
        {
            get
            {
                if (_PasteImage == null)
                {
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/Paste.png"));
                    _PasteImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                }
                return _PasteImage;
            }
        }

        Grid Main;
        TextBlock LabelTextBlock;
        void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Shape != null)
                    Shape.ObjectStateChanged += Shape_ObjectStateChanged;

                if (base.Template != null)
                {
                    if (Main == null)
                    {
                        Main = this.Template.FindName("PART_Main", this) as Grid;
                        var ss = (Main.Parent as Grid).DataContext;
                        Main.RenderTransform = RotateTransform;
                        LabelTextBlock = this.Template.FindName("PART_Label", this) as TextBlock;
                        if (LabelTextBlock != null)
                            LabelTextBlock.Text = _Label;
                    }
                    ContentPresenter contentPresenter =
                        this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;

                    if (contentPresenter != null)
                    {
                        UIElement contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
                        if (contentVisual != null)
                        {
                            DragThumb thumb = this.Template.FindName("PART_DragThumb", this) as DragThumb;
                            if (thumb != null)
                            {
                                ControlTemplate template =
                                    DesignerItem.GetDragThumbTemplate(contentVisual) as ControlTemplate;
                                if (template != null)
                                    thumb.Template = template;
                            }

                        }
                    }
                }
            }
            catch (Exception error)
            {
            }

        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }
        public void Undo(UndoRedoCommand command)
        {
            var state = command.UndoData.Where(x => x.CommandTargetObject == this).Select(x => x.Data).FirstOrDefault() as System.Collections.Generic.Dictionary<string, object>;
            Shape.Left = (double)state["Left"];
            Shape.Top = (double)state["Top"];
            Shape.Width = (double)state["Width"];
            Shape.Height = (double)state["Height"];
            Shape.ZIndex = (int)state["ZIndex"];
            Shape.RotateAngle = (double)state["RotateAngle"];
            RotationDeegrees = (int)Shape.RotateAngle;


            if (Shape is ShapesGroup)
            {
                (Shape as ShapesGroup).ClientAreaWidth = (double)state["ClientAreaWidth"];
                (Shape as ShapesGroup).ClientAreaHeight = (double)state["ClientAreaHeight"];
            }

            Width = Shape.Width;
            Height = Shape.Height;
            Canvas.SetLeft(this, Shape.Left);
            Canvas.SetTop(this, Shape.Top);
            Canvas.SetZIndex(this, Shape.ZIndex);
            DesignerCanvas rootCanvas = ObjectContext.FindParent<DesignerCanvas>(this);
            if (Shape is ShapesGroup)
            {
                var shapes = state["GroupedSaphes"] as System.Collections.Generic.List<Shape>;
                List<Shape> removeShapes = new List<Shape>();
                foreach (var shape in (Shape as ShapesGroup).Shapes)
                {
                    if (!shapes.Contains(shape))
                        removeShapes.Add(shape);
                }
                List<Shape> addShapes = new List<Shape>();
                foreach (var shape in shapes)
                {
                    if (!(Shape as ShapesGroup).Shapes.Contains(shape))
                        addShapes.Add(shape);
                }

                foreach (var removeShape in removeShapes)
                {
                    if (Parent != null)
                    {
                        var dItem = (from designerItem in ObjectContext.FindChilds<DesignerItem>(rootCanvas).OfType<DesignerItem>()
                                     where designerItem.Shape == removeShape
                                     select designerItem).FirstOrDefault();
                        dItem.ParentID = Guid.Empty;
                        Canvas groupCanvas = null;
                        if (ItemContent is Grid && (ItemContent as Grid).Children.OfType<Viewbox>().FirstOrDefault() != null && (ItemContent as Grid).Children.OfType<Viewbox>().FirstOrDefault().Child is Canvas)
                        {
                            groupCanvas = (ItemContent as Grid).Children.OfType<Viewbox>().FirstOrDefault().Child as Canvas;
                            groupCanvas.Children.Remove(dItem);
                        }
                    }
                    (Shape as ShapesGroup).RemoveGroupedShape(removeShape);
                }

                foreach (var addShape in addShapes)
                {
                    if (Parent != null)
                    {
                        var dItem = (from designerItem in ObjectContext.FindChilds<DesignerItem>(rootCanvas).OfType<DesignerItem>()
                                     where designerItem.Shape == addShape
                                     select designerItem).FirstOrDefault();
                        dItem.ParentID = this.id;
                    }
                    if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(addShape))
                        ObjectStorage.GetStorageOfObject(Shape).CommitTransientObjectState(addShape);

                    (Shape as ShapesGroup).AddGroupedShape(addShape);
                }

                this.InvalidateVisual();
            }


        }

        public void Redo(UndoRedoCommand command)
        {

            DesignerCanvas rootCanvas = ObjectContext.FindParent<DesignerCanvas>(this);
            var state = command.RedoData.Where(x => x.CommandTargetObject == this).Select(x => x.Data).FirstOrDefault() as System.Collections.Generic.Dictionary<string, object>;
            //var state = command.RedoData[this] as System.Collections.Generic.Dictionary<string, object>;
            Shape.Left = (double)state["Left"];
            Shape.Top = (double)state["Top"];
            Shape.Width = (double)state["Width"];
            Shape.Height = (double)state["Height"];
            Shape.ZIndex = (int)state["ZIndex"];
            Shape.RotateAngle = (double)state["RotateAngle"];
            RotationDeegrees = (int)Shape.RotateAngle;
            if (Shape is ShapesGroup)
            {
                (Shape as ShapesGroup).ClientAreaWidth = (double)state["ClientAreaWidth"];
                (Shape as ShapesGroup).ClientAreaHeight = (double)state["ClientAreaHeight"];
            }


            Width = Shape.Width;
            Height = Shape.Height;
            Canvas.SetLeft(this, Shape.Left);
            Canvas.SetTop(this, Shape.Top);
            Canvas.SetZIndex(this, Shape.ZIndex);
            if (Shape is ShapesGroup)
            {
                var shapes = state["GroupedSaphes"] as System.Collections.Generic.List<Shape>;
                List<Shape> removeShapes = new List<Shape>();
                foreach (var shape in (Shape as ShapesGroup).Shapes)
                {
                    if (!shapes.Contains(shape))
                        removeShapes.Add(shape);
                }
                List<Shape> addShapes = new List<Shape>();
                foreach (var shape in shapes)
                {
                    if (!(Shape as ShapesGroup).Shapes.Contains(shape))
                        addShapes.Add(shape);
                }

                foreach (var removeShape in removeShapes)
                {
                    if (Parent != null)
                    {
                        var dItem = (from designerItem in ObjectContext.FindChilds<DesignerItem>(rootCanvas).OfType<DesignerItem>()
                                     where designerItem.Shape == removeShape
                                     select designerItem).FirstOrDefault();
                        dItem.ParentID = Guid.Empty;

                        if (ItemContent is Canvas && (ItemContent as Canvas).Children.Contains(dItem))
                            (ItemContent as Canvas).Children.Remove(dItem);
                    }
                    (Shape as ShapesGroup).RemoveGroupedShape(removeShape);
                }

                foreach (var addShape in addShapes)
                {
                    if (Parent != null)
                    {
                        var dItem = (from designerItem in ObjectContext.FindChilds<DesignerItem>(rootCanvas).OfType<DesignerItem>()
                                     where designerItem.Shape == addShape
                                     select designerItem).FirstOrDefault();
                        dItem.ParentID = this.id;
                    }

                    if (!OOAdvantech.PersistenceLayer.ObjectStorage.IsPersistent(addShape))
                        ObjectStorage.GetStorageOfObject(Shape).CommitTransientObjectState(addShape);

                    (Shape as ShapesGroup).AddGroupedShape(addShape);
                }

                this.InvalidateVisual();
            }
        }

        public void MarkUndo(UndoRedoCommand command)
        {
            System.Collections.Generic.Dictionary<string, object> state = new System.Collections.Generic.Dictionary<string, object>();
            state["Left"] = Shape.Left;
            state["Top"] = Shape.Top;
            state["Width"] = Shape.Width;
            state["Height"] = Shape.Height;
            state["ZIndex"] = Shape.ZIndex;
            state["RotateAngle"] = Shape.RotateAngle;

            if (Shape is ShapesGroup)
            {
                state["GroupedSaphes"] = (Shape as ShapesGroup).Shapes.ToList();
                state["ClientAreaWidth"] = (Shape as ShapesGroup).ClientAreaWidth;
                state["ClientAreaHeight"] = (Shape as ShapesGroup).ClientAreaHeight;

            }


            command.UndoData.Insert(0, new CommandTarget() { CommandTargetObject = this, Data = state });
            //command.UndoData[this] = state;



        }

        public void MarkRedo(UndoRedoCommand command)
        {
            System.Collections.Generic.Dictionary<string, object> state = new System.Collections.Generic.Dictionary<string, object>();
            state["Left"] = Shape.Left;
            state["Top"] = Shape.Top;
            state["Width"] = Shape.Width;
            state["Height"] = Shape.Height;
            state["ZIndex"] = Shape.ZIndex;
            state["RotateAngle"] = Shape.RotateAngle;

            if (Shape is ShapesGroup)
            {
                state["GroupedSaphes"] = (Shape as ShapesGroup).Shapes.ToList();

                state["ClientAreaWidth"] = (Shape as ShapesGroup).ClientAreaWidth;
                state["ClientAreaHeight"] = (Shape as ShapesGroup).ClientAreaHeight;
            }

            command.RedoData.Insert(0, new CommandTarget() { CommandTargetObject = this, Data = state });
            //command.RedoData[this] = state;



        }


    }
}
