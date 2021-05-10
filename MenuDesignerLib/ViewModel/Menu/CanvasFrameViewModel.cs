using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using OOAdvantech.Transactions;
using System.Windows;
using MenuPresentationModel.MenuCanvas;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{ad446829-a7c2-4845-bea1-0e7a468124e7}</MetaDataID>
    [Transactional]
    public class CanvasFrameViewModel : MarshalByRefObject, ICanvasItem, INotifyPropertyChanged
    {

        public WPFUIElementObjectBind.RelayCommand RemoveCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand FrameClicked { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand EditCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand MoveUpCommand { get; protected set; }
        public WPFUIElementObjectBind.RelayCommand MoveDownCommand { get; protected set; }

        IMenuCanvasItem _MenuCanvasItem;
        public IMenuCanvasItem MenuCanvasItem
        {
            get
            {
                return _MenuCanvasItem;
            }
            set
            {
                _MenuCanvasItem = value;
            }
        }
        public readonly BookPageViewModel BookPageViewModel;

        MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItem MenuCanvasFoodItem;

        MenuPresentationModel.MenuCanvas.IMenuCanvasHeading MenuCanvasHeading;
        public CanvasFrameViewModel()
        {
            Visibility = Visibility.Visible;

            RemoveCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                _Opacity = 0;
                BookPageViewModel.BookViewModel.RemoveMenuCanvasItem(MenuCanvasItem, BookPageViewModel.MenuPage);
            });

            MoveUpCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                _Opacity = 0;
                BookPageViewModel.BookViewModel.MoveUpMenuCanvasItem(MenuCanvasItem, BookPageViewModel.MenuPage);

            });

            MoveDownCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                _Opacity = 0;
                BookPageViewModel.BookViewModel.MoveDownMenuCanvasItem(MenuCanvasItem, BookPageViewModel.MenuPage);

            });
            FrameClicked = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                if (MenuCanvasFoodItem != null)
                {
                    System.Windows.Window win = System.Windows.Window.GetWindow(FrameClicked.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                    {
                        var menuItemWindow = new Views.FoodItemWindow();
                        menuItemWindow.Owner = win;
                        FoodItemViewModel foodItemViewModel = new FoodItemViewModel(MenuCanvasFoodItem, BookPageViewModel.BookViewModel.RestaurantMenu);
                        menuItemWindow.GetObjectContext().SetContextInstance(foodItemViewModel);
                        if (menuItemWindow.ShowDialog().Value)
                            stateTransition.Consistent = true;
                    }
                }



                if (MenuCanvasHeading != null)
                {
                    System.Windows.Window win = System.Windows.Window.GetWindow(FrameClicked.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                    {
                        var menuItemWindow = new Views.HeadingWindow();
                        menuItemWindow.Owner = win;
                        MenuHeadingViewModel menuHeadingViewModel = new MenuHeadingViewModel(MenuCanvasHeading, BookPageViewModel.BookViewModel.RestaurantMenu);
                        menuItemWindow.GetObjectContext().SetContextInstance(menuHeadingViewModel);
                        if (menuItemWindow.ShowDialog().Value)
                            stateTransition.Consistent = true;
                    }
                }
                if (ItemMultiPriceHeading != null)
                {
                    System.Windows.Window win = System.Windows.Window.GetWindow(FrameClicked.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                    {
                        var multiPriceHeadingsForm = new Views.MultiPriceHeadingsForm();
                        multiPriceHeadingsForm.Owner = win;
                        MultiPriceHeadingsViewModel multiPriceHeadingsViewModel = new MultiPriceHeadingsViewModel(BookPageViewModel.BookViewModel, ItemMultiPriceHeading);
                        multiPriceHeadingsForm.GetObjectContext().SetContextInstance(multiPriceHeadingsViewModel);
                        if (multiPriceHeadingsForm.ShowDialog().Value)
                            stateTransition.Consistent = true;

                    }
                }

                Opacity = 0;
            });

            EditCommand = FrameClicked;
        }
        public CanvasFrameViewModel(IMenuCanvasFoodItem menuCanvasFoodItem, BookPageViewModel bookPageViewModel) : this()
        {
            BookPageViewModel = bookPageViewModel;
            MenuCanvasFoodItem = menuCanvasFoodItem;
            MenuCanvasItem = MenuCanvasFoodItem;
            MenuPresentationModel.MenuCanvas.Rect rect = menuCanvasFoodItem.CanvasFrameArea;


            _Left = rect.X;
            _Top = rect.Y;
            _Height = rect.Height;
            _Width = rect.Width;
            _FrameBackgroundColor = new SolidColorBrush(Colors.Aqua);
        }

        public System.Windows.Visibility RemoveBtnVisibility
        {
            get
            {
                if (DropArea)
                    return System.Windows.Visibility.Collapsed;
                else
                    return System.Windows.Visibility.Visible;
            }
        }

        Brush _FrameBackgroundColor;
        public System.Windows.Media.Brush FrameBackgroundColor
        {
            get
            {
                return _FrameBackgroundColor;
            }
        }




        MenuCanvasItemTextViewModel _MenuItem;
        public MenuCanvasItemTextViewModel MenuItem
        {
            get
            {

                return _MenuItem;
            }
            set
            {
                if (DropArea && value is IMenuCanvasHeading && (value as IMenuCanvasHeading).NextColumnOrPage)
                {

                }
                _MenuItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MenuItem)));
            }
        }
        ///// <exclude>Excluded</exclude>
        //bool _DragDropOn;
        //public bool DragDropOn
        //{
        //    get
        //    {
        //        return _DragDropOn;
        //    }
        //    set
        //    {
        //        _DragDropOn = value;
        //        IsHitTestVisible = !_DragDropOn;
        //    }
        //}

        /// <exclude>Excluded</exclude>
        bool _IsHitTestVisible = true;
        public bool IsHitTestVisible
        {
            get
            {
                return _IsHitTestVisible;
            }
            set
            {
                _IsHitTestVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsHitTestVisible)));
            }
        }



        public readonly bool DropArea;

        public CanvasFrameViewModel(BookPageViewModel bookPageViewModel) : this()
        {
            BookPageViewModel = bookPageViewModel;
            _Left = BookPageViewModel.MenuPage.Margin.MarginLeft;
            _Top = BookPageViewModel.MenuPage.Margin.MarginTop;
            _Width = BookPageViewModel.MenuPage.Width - (BookPageViewModel.MenuPage.Margin.MarginLeft + BookPageViewModel.MenuPage.Margin.MarginRight);
            _Height = 50;
            DropArea = true;
            //_IsHitTestVisible = false;
            _FrameBackgroundColor = new SolidColorBrush(Colors.Blue);
            _Visibility = Visibility.Collapsed;
        }


        public CanvasFrameViewModel(IMenuCanvasHeading menuCanvasHeading, BookPageViewModel bookPageViewModel) : this()
        {
            BookPageViewModel = bookPageViewModel;
            MenuCanvasHeading = menuCanvasHeading;
            MenuCanvasItem = MenuCanvasHeading;
            MenuPresentationModel.MenuCanvas.Rect rect = menuCanvasHeading.CanvasFrameArea;
            _FrameBackgroundColor = new SolidColorBrush(Colors.Aqua);
            //if (menuCanvasHeading.HostingArea != null)
            //    _Left = menuCanvasHeading.HostingArea.Column.XPos - 10;
            //else
            //    _Left = (menuCanvasHeading.Page.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).Margin.MarginLeft;

            //_Top = menuCanvasHeading.YPos-10;
            //_Height = menuCanvasHeading.Height+20;
            //if (menuCanvasHeading.HostingArea != null)
            //    _Width = menuCanvasHeading.HostingArea.Width + 20;
            //else
            //{
            //    MenuPresentationModel.MenuStyles.PageStyle pageStyle = menuCanvasHeading.Page.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle;
            //    _Width = pageStyle.PageWidth-( pageStyle.Margin.MarginLeft+ pageStyle.Margin.MarginRight);
            //}


            _Left = rect.X;
            _Top = rect.Y;
            _Height = rect.Height;
            _Width = rect.Width;

        }

        internal IItemMultiPriceHeading ItemMultiPriceHeading;

        public CanvasFrameViewModel(IItemMultiPriceHeading itemMultiPriceHeading, BookPageViewModel bookPageViewModel) : this()
        {
            BookPageViewModel = bookPageViewModel;
            ItemMultiPriceHeading = itemMultiPriceHeading;
            MenuCanvasItem = itemMultiPriceHeading;
            MenuPresentationModel.MenuCanvas.Rect rect = itemMultiPriceHeading.CanvasFrameArea;
            _FrameBackgroundColor = new SolidColorBrush(Colors.Aqua);
            _Left = rect.X;
            _Top = rect.Y;
            _Height = rect.Height;
            _Width = rect.Width;
        }

        /// <exclude>Excluded</exclude>
        double _Opacity = 0;
        public double Opacity
        {
            get
            {
                return _Opacity;
            }
            set
            {
             
                _Opacity = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opacity)));
            }
        }

        public bool IsMouseOver
        {
            set
            {
                if (!DropArea)
                {
                    if (value)
                        Opacity = 1;
                    else
                        Opacity = 0;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        double _Height;
        public double Height
        {
            get
            {
                return _Height;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Left;

        public double Left
        {
            get
            {
                return _Left;
            }
            set
            {
                _Left = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
            }
        }

        public string MovingFrameText
        {
            get
            {
                //if (Pinned)
                //{
                //    if (MenuCanvasItem != null)
                //        return string.Format("The {0} pinned to start", MenuCanvasItem.Description);
                //    else
                //        return "Pinned to start";
                //}
                //else
                return "move";
            }
        }


        static Brush PinnedDragAreaBackgroundColor = new SolidColorBrush(Colors.Red);
        static Brush UnPinnedDragAreaBackgroundColor = new SolidColorBrush(Colors.Blue);

        public Brush DragAreaBackgroundColor
        {
            get
            {
                if (Pinned)
                    return PinnedDragAreaBackgroundColor;
                else
                    return UnPinnedDragAreaBackgroundColor;


            }

        }
        public string PinnedMovingFrameText
        {
            get
            {

                return "The item pinned to start";


            }
        }
        void Clear()
        {
            MenuCanvasHeading = null;
            MenuCanvasFoodItem = null;
            MenuCanvasItem = null;
            ItemMultiPriceHeading = null;
        }

        //internal void ChangeCanvasItem(IItemMultiPriceHeading itemMultiPriceHeading)
        //{

        //    Clear();
        //    ItemMultiPriceHeading = itemMultiPriceHeading;
        //    MenuCanvasItem = itemMultiPriceHeading;
        //    Refresh();

        //}



        internal void ChangeCanvasItem(IMenuCanvasFoodItem menuCanvasFoodItem)
        {
            Clear();
            MenuCanvasHeading = null;
            MenuCanvasFoodItem = menuCanvasFoodItem;
            MenuCanvasItem = menuCanvasFoodItem;
            Refresh();
        }

        public void Refresh()
        {

            //_Left = menuCanvasFoodItem.HostingArea.Column.XPos-10;
            //_Top = MenuCanvasFoodItem.SubTexts[0].YPos -20;
            //_Height = MenuCanvasFoodItem.Height+20;
            //_Width = menuCanvasFoodItem.HostingArea.Column.Width+20;

            MenuPresentationModel.MenuCanvas.Rect rect;
            if (MenuCanvasFoodItem != null)
                rect = MenuCanvasFoodItem.CanvasFrameArea;
            else if (MenuCanvasHeading != null)
                rect = MenuCanvasHeading.CanvasFrameArea;
            else
                return;

            if (_Left != rect.X)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Left = rect.X;
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
            }
            if (_Top != rect.Y)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Top = rect.Y;
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            }
            if (_Height != rect.Height)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Height = rect.Height;
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
            }
            if (_Width != rect.Width)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Width = rect.Width;
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
            }

            _FrameBackgroundColor = new SolidColorBrush(Colors.Aqua);
            if(Visibility==Visibility.Visible)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opacity)));
        }

        public Visibility MoveVisibility
        {
            get
            {
                if (Pinned)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }
        public Visibility PinnedVisibility
        {
            get
            {
                if (Pinned)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        internal void ChangeCanvasItem(IMenuCanvasHeading menuCanvasHeading)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                Clear();
                MenuCanvasHeading = menuCanvasHeading;
                MenuCanvasFoodItem = null;
                MenuCanvasItem = menuCanvasHeading;
                stateTransition.Consistent = true;
            }

            MenuPresentationModel.MenuCanvas.Rect rect = menuCanvasHeading.CanvasFrameArea;
            if (_Left != rect.X)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Left = rect.X;
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
            }
            if (_Top != rect.Y)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Top = rect.Y;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            }
            if (_Height != rect.Height)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Height = rect.Height;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
            }
            if (_Width != rect.Width)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Width = rect.Width;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
            }

            _FrameBackgroundColor = new SolidColorBrush(Colors.Aqua);


        }

        /// <exclude>Excluded</exclude>
        double _Top;
        public double Top
        {
            get
            {
                return _Top;
            }
            set
            {
                _Top = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            }
        }

        /// <exclude>Excluded</exclude>
        double _Width;
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));

            }
        }

        Visibility _Visibility;
        public Visibility Visibility
        {
            get
            {
                //return Visibility.Visible;
                return _Visibility;
            }
            set
            {
                if (_Visibility != value)
                {
                    _Visibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opacity)));
                }
            }
        }

        bool _Pinned;
        public bool Pinned
        {
            get
            {
                return _Pinned;
            }
            set
            {
                _Pinned = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoveVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PinnedVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DragAreaBackgroundColor)));
            }
        }

        public bool AllowDragDrop
        {
            get
            {
                if (this.MenuCanvasFoodItem != null || this.MenuCanvasHeading != null)
                    return true;
                else
                    return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Release()
        {

        }
    }
}


