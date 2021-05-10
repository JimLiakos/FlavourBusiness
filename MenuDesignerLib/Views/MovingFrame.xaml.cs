using MenuDesigner.Views.MenuCanvas;
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
    /// Interaction logic for MovingFrame.xaml
    /// </summary>
    /// <MetaDataID>{2a81a3a5-7e40-4ef6-a69c-0ee59aef9426}</MetaDataID>
    public partial class MovingFrame : UserControl
    {
        public MovingFrame()
        {
            InitializeComponent();

            this.AllowDrop = true;
            MouseLeftButtonUp += MovingFrame_MouseLeftButtonUp;

            //DispatcherTimer = new System.Windows.Threading.DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500), IsEnabled = true };
            //DispatcherTimer.Tick += TimerTick;

        }

        private void TimerTick(object sender, EventArgs e)
        {
            var frame = this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>();
            if (frame != null)
            {
                if (IsMouseOver)
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = true;
                else
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = false;
            }
        }

        //System.Windows.Threading.DispatcherTimer DispatcherTimer;


        private void MovingFrame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid dragArea = DragArea;
            if (dragArea.IsMouseCaptured)
            {
                dragArea.CaptureMouse();
                dragArea.Visibility = Visibility.Collapsed;
                dragAreaArrowYpos += transform.Y;
                var canvasFrameViewModel = this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>();
                if (canvasFrameViewModel != null)
                {
                    using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(this.GetObjectContext().Culture, this.GetObjectContext().UseDefaultCultureWhenValueMissing))
                    {
                        var page = canvasFrameViewModel.MenuCanvasItem.Page;

                        if (!canvasFrameViewModel.Pinned)
                        {
                            
                            this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                            {
                                var pointerPos = e.GetPosition(WPFUIElementObjectBind.ObjectContext.FindParent<MenuCanvas.DesignerCanvas>(this as DependencyObject));
                                var pointerPos2 = WPFUIElementObjectBind.ObjectContext.FindParent<MenuCanvas.DesignerCanvas>(this as DependencyObject).PointFromScreen(this.PointToScreen(new Point(this.ActualWidth/2 + transform.X, this.ActualHeight/2 + transform.Y)));
                                var point = new Point(canvasFrameViewModel.Left + canvasFrameViewModel.Width / 2 + transform.X, canvasFrameViewModel.MenuCanvasItem.YPos + transform.Y);
                                if (page.MoveCanvasItemTo(canvasFrameViewModel.MenuCanvasItem, pointerPos2))
                                    canvasFrameViewModel.BookPageViewModel.BookViewModel.MenuItemMoveOnPage(canvasFrameViewModel.BookPageViewModel.MenuPage, canvasFrameViewModel.MenuCanvasItem);
                            }));
                        }
                    }
                }


                //int count = VisualTreeHelper.GetChildrenCount(MenuItemsControl);
                //MenuPresentetion.MenuItemPresentationViewModel menuItem = null;
                //foreach (var item in MenuItemsControl.Items)
                //{
                //    FrameworkElement uiElement = MenuItemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                //    Point itemControlPos = uiElement.TranslatePoint(new Point(0, 0), this);
                //    double itemCenterY = itemControlPos.Y + (uiElement.ActualHeight / 2);
                //    if (dragAreaArrowYpos > itemCenterY)
                //        menuItem = uiElement.GetDataContextObject<MenuPresentetion.MenuItemPresentationViewModel>();
                //}
                //this.GetObjectContext().ExecuteUnderContextTransaction(new Action(() =>
                //{
                //    dragMenuItem.MoveItemAfter(menuItem);
                //}));

                e.Handled = true;
            }
        }

        private Point elementStartPosition;
        private Point mouseStartPosition;
        private Point mouseStartPositionOnCanvas;
        private double dragAreaArrowYpos;
        MenuPresentetion.MenuItemPresentationViewModel dragMenuItem;
        private TranslateTransform transform = new TranslateTransform();

        bool OnDragArea;
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().AllowDragDrop)
                OnDragArea = true;
            base.OnMouseLeftButtonDown(e);
        }


        protected override void OnMouseLeave(MouseEventArgs e)
        {

            var frame = this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>();
            if (frame != null)
            {
                if (IsMouseOver)
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = true;
                else
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = false;
            }
            base.OnMouseLeave(e);
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            var frame = this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>();
            if (frame != null)
            {
                if (IsMouseOver)
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = true;
                else
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = false;
            }
            base.OnMouseEnter(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {




            if (e.LeftButton == MouseButtonState.Released)
                OnDragArea = false;

            if ((OnDragArea && e.LeftButton == MouseButtonState.Pressed) || DragArea.IsMouseCaptured)
            {
                var canvasFrameViewModel = this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>();
                if (canvasFrameViewModel == null || canvasFrameViewModel.DropArea)
                    return;

                Vector diff = e.GetPosition(this) - mouseStartPosition;

                if (diff.Y > 3 || diff.Y < 3 || DragArea.IsMouseCaptured)
                {
                    this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                    {
                        var pointerPos = e.GetPosition(WPFUIElementObjectBind.ObjectContext.FindParent<MenuCanvas.DesignerCanvas>(this as DependencyObject));

                        pointerPos = new Point(mouseStartPositionOnCanvas.X + diff.X, mouseStartPositionOnCanvas.Y + diff.Y); // new Point(pointerPos.X - diff.X, pointerPos.Y - diff.Y);
                        
                        var frameCenterPoint = new Point(canvasFrameViewModel.MenuCanvasItem.XPos + diff.X, canvasFrameViewModel.MenuCanvasItem.YPos + diff.Y);

                        var dropRectangle = canvasFrameViewModel.BookPageViewModel.GetDropRectangle(pointerPos);

                        double upperYpos = canvasFrameViewModel.MenuCanvasItem.Page.Margin.MarginTop - canvasFrameViewModel.MenuCanvasItem.YPos;
                        double bottomYpos = (canvasFrameViewModel.MenuCanvasItem.Page.Height - canvasFrameViewModel.MenuCanvasItem.Page.Margin.MarginBottom) - canvasFrameViewModel.MenuCanvasItem.YPos;
                        Point mousetPosition = e.GetPosition(this);

                        if (diff.Y > upperYpos && diff.Y < bottomYpos)
                        {


                            dragMenuItem = DragArea.GetDataContextObject<MenuPresentetion.MenuItemPresentationViewModel>();
                            if (!DragArea.IsMouseCaptured)
                            {
                                if (canvasFrameViewModel.MenuCanvasItem is MenuPresentationModel.MenuCanvas.IMenuCanvasHeading &&
                                (canvasFrameViewModel.MenuCanvasItem as MenuPresentationModel.MenuCanvas.IMenuCanvasHeading).NextColumnOrPage)
                                    canvasFrameViewModel.Pinned = true;
                                else
                                    canvasFrameViewModel.Pinned = false;

                                DragArea.Visibility = Visibility.Visible;
                                DragArea.RenderTransform = transform;
                                elementStartPosition = DragArea.TranslatePoint(new Point(0, 0), this);
                                mouseStartPosition = e.GetPosition(this);
                                mouseStartPositionOnCanvas= e.GetPosition(WPFUIElementObjectBind.ObjectContext.FindParent<DesignerCanvas>(this as DependencyObject));
                                DragArea.CaptureMouse();
                                DragArea.Margin = new Thickness(0, 0, 0, 0);
                                UpdateLayout();
                                dragAreaArrowYpos = elementStartPosition.Y + (DragArea.ActualHeight / 2);
                            }


                            double rightMargin = canvasFrameViewModel.Width - dropRectangle.Width;
                            if (DragArea.Margin==null)
                                DragArea.Margin = new Thickness(0, 0, rightMargin, 0);
                            else
                            {
                                double marginDif = DragArea.Margin.Right - rightMargin;
                                if (marginDif < 0)
                                    marginDif = -marginDif;
                                if(marginDif>10)
                                    DragArea.Margin = new Thickness(0, 0, rightMargin, 0);

                            }
                          
                            transform.Y = diff.Y;
                            //var pointerPos = WPFUIElementObjectBind.ObjectContext.FindParent<MenuDesigner.DesignerCanvas>(this as DependencyObject).PointFromScreen(this.PointToScreen(new Point(this.ActualWidth / 2 + transform.X, this.ActualHeight / 2 + transform.Y)));
                            //var page = canvasFrameViewModel.MenuCanvasItem.Page as MenuPresentationModel.MenuPage;
                            //var mouseOverMenuCanvasItem = page.GetMenuCanvasItemAt(pointerPos) as MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem;
                            //try
                            //{
                            //    if (mouseOverMenuCanvasItem != null)
                            //        transform.X = mouseOverMenuCanvasItem.CanvasFrameArea.X - canvasFrameViewModel.Left;
                            //}
                            //catch (Exception error)
                            //{

                                
                            //}                                

                            transform.X = dropRectangle.X - canvasFrameViewModel.Left;

                        }
                    }));
                }
            }
            base.OnMouseMove(e);
        }

        private void FrameGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            var frame = this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>();
            if (frame != null)
            {
                if (IsMouseOver)
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = true;
                else
                    this.GetDataContextObject<ViewModel.MenuCanvas.CanvasFrameViewModel>().IsMouseOver = false;
            }

            //FrameGrid.Opacity = 0.05;
        }

        private void FrameGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            //FrameGrid.Opacity = 0;

        }

        private void FrameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
