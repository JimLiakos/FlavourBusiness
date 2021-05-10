using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using MenuDesigner.ViewModel.MenuCanvas;
using MenuPresentationModel.MenuCanvas;
using OOAdvantech.Transactions;
using SharpVectors.Converters;


namespace MenuDesigner.Views.MenuCanvas
{
    /// <MetaDataID>MenuDesigner.DesignerCanvas</MetaDataID>
    public partial class DesignerCanvas : System.Windows.Controls.Canvas
    {



        /// <MetaDataID>{7cb37773-9eae-4d6d-ad90-b4c40f91e5e9}</MetaDataID>
        public DesignerCanvas()
        {
            this.CommandBindings.Add(new WPFUIElementObjectBind.CommandBinding(DesignerCanvas.SelectAll, SelectAll_Executed));
            SelectAll.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));

            this.AllowDrop = true;
            Clipboard.Clear();

            DataContextChanged += DesignerCanvas_DataContextChanged;


            ClickPseudoCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { });

        }
        internal void SelectAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // SelectionService.SelectAll();
        }

        /// <MetaDataID>{d74d6464-c817-4c41-97bd-4dfac20ef0e6}</MetaDataID>
        public static RoutedCommand SelectAll = new RoutedCommand();

        /// <MetaDataID>{c472c361-b886-4b7e-a2d1-ca923e6b2e4d}</MetaDataID>
        private void DesignerCanvas_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <MetaDataID>{cd3ae040-3b41-4d65-a723-e3d7668350cf}</MetaDataID>
        static System.Drawing.Text.PrivateFontCollection privateFonts;
        /// <MetaDataID>{94e30237-fdd0-459a-a03a-f906b03f2488}</MetaDataID>



        /// <summary>
        /// Get the required height and width of the specified text. Uses FortammedText
        /// </summary>
        /// <MetaDataID>{103361e7-2e33-48d1-a6a2-4a98a3c86621}</MetaDataID>
        public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                                 CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                 fontSize,
                                                 Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
        /// <MetaDataID>{439f4f43-2fa6-4ad3-bf98-36098710972b}</MetaDataID>
        static DesignerCanvas _SelectedDesignerCanvas;
        /// <MetaDataID>{ac16038c-5f4c-414d-9dbd-860b3ed6d926}</MetaDataID>
        public static DesignerCanvas SelectedDesignerCanvas
        {
            set
            {
                if (_SelectedDesignerCanvas != value)
                {
                    //if (_SelectedDesignerCanvas != null)
                    //    _SelectedDesignerCanvas.selectionService.ClearSelection();
                    _SelectedDesignerCanvas = value;
                    if (_SelectedDesignerCanvas != null)
                        (_SelectedDesignerCanvas.DataContext as BookPageViewModel).Select();
                }
            }
            get
            {
                return _SelectedDesignerCanvas;
            }
        }

        public WPFUIElementObjectBind.RelayCommand ClickPseudoCommand { get; }

        /// <MetaDataID>{aaeb7dbf-1120-45d2-bb3a-197f478780de}</MetaDataID>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                Focus();
                SelectedDesignerCanvas = this;
                e.Handled = true;
            }
        }


        /// <MetaDataID>{14a86e3d-05a1-4896-9ee9-e10d8e25dae5}</MetaDataID>
        Dictionary<FrameworkElement, bool> CanvasChilds;
        /// <MetaDataID>{7ad0fded-c0f6-4b84-abe4-50f7b79f0bf5}</MetaDataID>
        protected override void OnDragEnter(DragEventArgs e)
        {
            DragEnter(e);
            base.OnDragEnter(e);
        }

        /// <MetaDataID>{a576105d-6b00-40be-b61a-ae1a61e03b8f}</MetaDataID>
        public void DragEnter(DragEventArgs e)
        {
            var bookPageViewModel = this.GetDataContextObject<BookPageViewModel>();
            if (bookPageViewModel.IsReadonly)
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            CanvasChilds = WPFUIElementObjectBind.ObjectContext.FindChilds<FrameworkElement>(this).ToDictionary(x => x, x => x.IsHitTestVisible);
            foreach (var child in CanvasChilds)
                child.Key.IsHitTestVisible = false;
        }

        /// <MetaDataID>{cce46fed-49c2-42c3-b9af-e0664acee304}</MetaDataID>
        int i = 0;



        /// <MetaDataID>{b49e7d2b-0bb4-4539-a5f1-aa3e0bdd3552}</MetaDataID>
        protected override void OnDragLeave(DragEventArgs e)
        {
            DragLeave(e);
            base.OnDragLeave(e);
        }

        /// <MetaDataID>{3686a35d-0696-4ffc-a817-5905be6b3f0e}</MetaDataID>
        public void DragLeave(DragEventArgs e)
        {
            foreach (var child in CanvasChilds)
                child.Key.IsHitTestVisible = child.Value;


            var bookPageViewModel = this.GetDataContextObject<BookPageViewModel>();
            if (bookPageViewModel == null)
            {
                var canvasFrameViewModel = this.GetDataContextObject<CanvasFrameViewModel>();
                if (canvasFrameViewModel != null)
                    bookPageViewModel = canvasFrameViewModel.BookPageViewModel;
            }
            Point screenPoint = this.PointToScreen(e.GetPosition(this));
            bookPageViewModel.DragDropArea.Opacity = 0;
            bookPageViewModel.DragDropArea.Visibility = Visibility.Collapsed;
            i++;
        }

        /// <MetaDataID>{89badf75-9d93-4451-aac3-15237ff5fa5d}</MetaDataID>
        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            DragOver(e);
        }

        /// <MetaDataID>{58de779d-702c-422f-bbd1-e02405faa8e6}</MetaDataID>
        public void DragOver(DragEventArgs e)
        {

            var bookPageViewModel = this.GetDataContextObject<BookPageViewModel>();
            if (bookPageViewModel.IsReadonly)
            {
                e.Effects = DragDropEffects.None;
                return;
            }
            ViewModel.MenuCanvas.DragCanvasItem canvasItem = e.Data.GetData(typeof(ViewModel.MenuCanvas.DragCanvasItem)) as ViewModel.MenuCanvas.DragCanvasItem;
            if (canvasItem != null)
            {
                this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                {


                    
                    if (!bookPageViewModel.IsReadonly)
                    {



                        Point screenPoint = PointToScreen(e.GetPosition(this));


                        var canvasPoint = PointFromScreen(screenPoint);
                        var dropRectangle = bookPageViewModel.GetDropRectangle(canvasPoint);


                        bookPageViewModel.DragDropArea.Opacity = 1;
                        bookPageViewModel.DragDropArea.Visibility = Visibility.Visible;

                        if (bookPageViewModel.DragDropArea.MenuItem == null || bookPageViewModel.DragDropArea.MenuItem.MenuCanvasItem != canvasItem.MenuCanvasItem)
                            bookPageViewModel.DragDropArea.MenuItem = new MenuCanvasItemTextViewModel(canvasItem.MenuCanvasItem);

                        bookPageViewModel.DragDropArea.Top = canvasPoint.Y;
                        bookPageViewModel.DragDropArea.Left = dropRectangle.X;
                        bookPageViewModel.DragDropArea.Width = dropRectangle.Width;
                    }

                }));
            }
        }




        /// <MetaDataID>{42d4d8d5-c130-4b71-a54e-b16bbe6c397c}</MetaDataID>
        protected override void OnDrop(DragEventArgs e)
        {
            Drop(e);
            base.OnDrop(e);
        }

        /// <MetaDataID>{0ef1d36e-f15a-430a-ad46-66c77c95599b}</MetaDataID>
        public void Drop(DragEventArgs e)
        {
            var bookPageViewModel = this.GetDataContextObject<BookPageViewModel>();
            if (bookPageViewModel.IsReadonly)
            {
                e.Effects = DragDropEffects.None;
                return;
            }
            try
            {
                foreach (var child in CanvasChilds)
                    child.Key.IsHitTestVisible = child.Value;


                this.GetObjectContext().RunUnderContextTransaction(new Action(() =>
                {
                    try
                    {
                        Point point = e.GetPosition(this);
                        DragCanvasItem dragCanvasItem = e.Data.GetData(typeof(DragCanvasItem)) as DragCanvasItem;
                         bookPageViewModel = this.GetDataContextObject<BookPageViewModel>();

                        if (dragCanvasItem.MenuCanvasItem.Page != null)
                            bookPageViewModel.BookViewModel.ShowPopUpMessage = true;
                        else
                        {
                            if (dragCanvasItem.MenuCanvasItem is MenuPresentationModel.MenuCanvas.IMenuCanvasHeading)
                                (dragCanvasItem.MenuCanvasItem as MenuPresentationModel.MenuCanvas.IMenuCanvasHeading).NextColumnOrPage = false;

                            IMenuCanvasItem dropedMenuCanvasItem = dragCanvasItem.MenuCanvasItem;

                            if (dropedMenuCanvasItem is IMenuCanvasFoodItem && (dropedMenuCanvasItem as IMenuCanvasFoodItem).MenuItem == null)
                            {
                                dropedMenuCanvasItem = new MenuCanvasFoodItem();
                                dropedMenuCanvasItem.Description = "";
                            }

                       
                            
                            bookPageViewModel.MenuPage.InsertCanvasItemTo(dropedMenuCanvasItem, point);

                            var menuCanvasItems = (from page in bookPageViewModel.BookViewModel.RestaurantMenu.Pages
                                                   from menuCanvasItem in page.MenuCanvasItems
                                                   select menuCanvasItem).ToList();
                            var indexofMenuCanvasItem = menuCanvasItems.IndexOf(dropedMenuCanvasItem);
                            if (indexofMenuCanvasItem > 0)
                                bookPageViewModel.BookViewModel.RestaurantMenu.InsertMenuItemAfter(menuCanvasItems[indexofMenuCanvasItem - 1], dropedMenuCanvasItem);
                            else
                                bookPageViewModel.BookViewModel.RestaurantMenu.InsertMenuItemAfter(null, dropedMenuCanvasItem);


                            bookPageViewModel.BookViewModel.MenuItemDropOnPage(this.GetDataContextObject<BookPageViewModel>().MenuPage);

                            // this.GetDataContextObject<BookPageViewModel>().Refresh();
                        }
                        bookPageViewModel.DragDropArea.Opacity = 0;
                        bookPageViewModel.DragDropArea.Visibility = Visibility.Collapsed;
                    }
                    catch (Exception error)
                    {

                        throw;
                    }





                    //DragObject dragObject = e.Data.GetData(typeof(DragObject)) as DragObject;
                    //Point position = e.GetPosition(this);

                    //var objectContextConnection = this.GetObjectContextConnection();
                    //if (objectContextConnection.Transaction != null)
                    //{
                    //    using (SystemStateTransition stateTransition = new SystemStateTransition(objectContextConnection.Transaction))
                    //    {
                    //        DrobDraggedObject(position, dragObject);
                    //        e.Handled = true;
                    //        stateTransition.Consistent = true;
                    //    }
                    //}
                    //else
                    //{
                    //    DrobDraggedObject(position, dragObject);
                    //    e.Handled = true;
                    //}
                }));
            }
            catch (Exception error)
            {

                throw;
            }
        }





        ///// <MetaDataID>{f0a11605-d612-4b80-b085-bff0134698f0}</MetaDataID>
        //protected override Size MeasureOverride(Size constraint)
        //{
        //    Size size = new Size();

        //    foreach (UIElement element in this.InternalChildren)
        //    {
        //        double left = System.Windows.Controls.Canvas.GetLeft(element);
        //        double top = System.Windows.Controls.Canvas.GetTop(element);
        //        left = double.IsNaN(left) ? 0 : left;
        //        top = double.IsNaN(top) ? 0 : top;

        //        //measure desired size for each child
        //        element.Measure(constraint);

        //        Size desiredSize = element.DesiredSize;
        //        if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
        //        {
        //            size.Width = Math.Max(size.Width, left + desiredSize.Width);
        //            size.Height = Math.Max(size.Height, top + desiredSize.Height);
        //        }
        //    }
        //    // add margin 
        //    size.Width += 10;
        //    size.Height += 10;
        //    return size;
        //}

    }
}
