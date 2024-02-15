using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using FLBManager.ViewModel;
using Microsoft.Win32;
using OOAdvantech.Transactions;
using System.Net.Http;
using FlavourBusinessToolKit;
using System.Globalization;
using System.ComponentModel;
using OOAdvantech;
using MenuDesigner.ViewModel.MenuCanvas;
using MenuDesigner.Views.MenuCanvas;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace MenuDesigner.Views
{
    using RoutedCommand = WPFUIElementObjectBind.RoutedCommand;
    using MenuCommand = WPFUIElementObjectBind.MenuCommand;

    //imenupro.com / SVGMenuPro11  - menuborder

    /// <summary>
    /// Interaction logic for PageDesigner.xaml
    /// </summary>
    /// <MetaDataID>{5f832311-fd18-4366-8fe6-14b23c5315d9}</MetaDataID>
    public partial class MenuDesignerControl : UserControl, INotifyPropertyChanged
    {
        /// <MetaDataID>{e75bd865-01cb-4add-a453-0d6fba1ecf79}</MetaDataID>
        public MenuDesignerControl()
        {
            InitializeComponent();

            ZoomViewBox.SizeChanged += ZoomViewBox_SizeChanged;
            ZoomViewBox.DataContextChanged += ZoomViewBox_DataContextChanged;

            // DataContext = BookViewModel;
            BookScrollViewer.SizeChanged += BookScrollViewer_SizeChanged;
            LanguageCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                ShowLanguageSelectionPopuo();
            });
            LanguageControl.DataContext = this;
            DataContextChanged += MenuDesignerControl_DataContextChanged;

        }

        private void MenuDesignerControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (this.GetDataContextObject<BookViewModel>() != null && !this.GetDataContextObject<BookViewModel>().IsReadonly)
            {
                this.GetObjectContext()?.RunUnderContextTransaction(new Action(() =>
                {
                    //this.GetDataContextObject<BookViewModel>().RebuildAllPages();
                }));
            }
        }

        private void ShowLanguageSelectionPopuo()
        {

            LanguagePopup.VerticalOffset = 5;
            //LanguagePopup.HorizontalOffset = -LanguageButton.ActualWidth;
            LanguagePopup.IsOpen = true;
        }

        static MenuCommand _RestaurantMenusMenu;
        public static MenuCommand RestaurantMenusMenu
        {
            get
            {
                if (_RestaurantMenusMenu == null)
                {
                    _RestaurantMenusMenu = new WPFUIElementObjectBind.MenuCommand()
                    {
                        Header = Properties.Resources.RestaurantMenusMenuItemHeader,
                        SubMenuCommands = new List<MenuCommand>()
                        {
                            new MenuCommand()
                            {
                                Header=Properties.Resources.NewGraphicMenuMenuItemHeader,
                                Icon=new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/GenericDocument.png")), Width = 16, Height = 16 },
                                Command=new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) },  async (object seneder) =>{ await NewGraphicMenu(); })
                            },
                            new MenuCommand()
                            {
                                Header=Properties.Resources.OpenGraphicMenuMenuItemHeader,
                                Icon=new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/OpenFolder.png")), Width = 16, Height = 16 },
                                Command=new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.O, ModifierKeys.Control) }, async (object seneder) =>{ await OpenGraphicMenus(); })
                            },
                            //null,
                            //new MenuCommand()
                            //{
                            //    Header=Properties.Resources.SaveGraphicMenuMenuItemHeader,
                            //    Icon=new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/Save.png")), Width = 16, Height = 16 },
                            //    Command=new WPFUIElementObjectBind.RoutedCommand( new List<InputGesture>() { new KeyGesture(Key.P, ModifierKeys.Control) }, (object seneder) =>{ SaveGraphicMenu(); })
                            //}
                          }
                    };
                }
                return _RestaurantMenusMenu;
            }
        }

        private static async Task OpenGraphicMenus()
        {
            Window win = Application.Current.MainWindow;
            Views.GraphicMenusForm graphicMenusForm = new Views.GraphicMenusForm();
            graphicMenusForm.Owner = win;
            graphicMenusForm.GetObjectContext().SetContextInstance(ViewModel.Menu.MenuDesignerHost.Current.GraphicMenus);
            if (graphicMenusForm.ShowDialog().Value)
            {
                var graphicMenu = ViewModel.Menu.MenuDesignerHost.Current.GraphicMenus.SelectedMenu;
                await ViewModel.Menu.MenuDesignerHost.OpenGraphicMenu(graphicMenu.StorageRef, null);
            }
        }

        private static async Task NewGraphicMenu()
        {
            await ViewModel.Menu.MenuDesignerHost.NewGraphicMenu();
        }


        public WPFUIElementObjectBind.RelayCommand LanguageCommand { get; protected set; }

        StyleableWindow.CulturePresentation _SelectedCulturePresentation;
        public StyleableWindow.CulturePresentation SelectedCulturePresentation
        {
            get
            {
                if (_SelectedCulturePresentation == null)
                {
                    _SelectedCulturePresentation = StyleableWindow.CulturePresentation.Cultures.Where(x => x.CultureInfo.Name == CultureInfo.CurrentCulture.Name).FirstOrDefault();
                    if (_SelectedCulturePresentation == null)
                        _SelectedCulturePresentation = StyleableWindow.CulturePresentation.Cultures.Where(x => x.CultureInfo.Name == CultureInfo.CurrentCulture.Parent.Name).FirstOrDefault();

                    if (_SelectedCulturePresentation == null)
                        _SelectedCulturePresentation = StyleableWindow.CulturePresentation.Cultures.Where(x => x.CultureInfo.Parent!=null&& x.CultureInfo.Parent.Name == CultureInfo.CurrentCulture.Name).FirstOrDefault();

                    if (_SelectedCulturePresentation != null)
                    {
                        var selectedCulture = _SelectedCulturePresentation.CultureInfo;
                        this.GetObjectContextConnection().Culture = selectedCulture;
                        this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing = UseDefaultCultureWhenValueMissing;
                    }
                    else
                    {
                        this.GetObjectContextConnection().Culture = null;
                    }

                    BookViewModel menu = this.GetDataContextObject<BookViewModel>();

                }
                return _SelectedCulturePresentation;
            }
            set
            {

                _SelectedCulturePresentation = value;
                if (value != null)
                {
                    var selectedCulture = value.CultureInfo;
                    if (this.GetObjectContextConnection() != null)
                    {
                        if (this.GetObjectContextConnection().Culture != selectedCulture)
                        {

                            this.GetObjectContextConnection().Culture = selectedCulture;
                            this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing = UseDefaultCultureWhenValueMissing;

                            using (SystemStateTransition stateTransition = new SystemStateTransition(this.GetObjectContextConnection().Transaction))
                            {
                                using (CultureContext cultureContext = new CultureContext(this.GetObjectContextConnection().Culture, this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing))
                                {

                                    BookViewModel menu = ViewModel.Menu.MenuDesignerHost.Current.Menu;
                                    if (menu != null)
                                        menu.RebuildAllPages();
                                }
                                stateTransition.Consistent = true;
                            }

                        }
                    }
                }
                else
                {
                    if (this.GetObjectContextConnection() != null)
                        this.GetObjectContextConnection().Culture = null;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCulturePresentation)));
            }
        }

        bool _UseDefaultCultureWhenValueMissing;
        public bool UseDefaultCultureWhenValueMissing
        {
            get
            {
                return _UseDefaultCultureWhenValueMissing;
            }
            set
            {
                if (_UseDefaultCultureWhenValueMissing != value)
                {
                    _UseDefaultCultureWhenValueMissing = value;
                    if (this.GetObjectContextConnection() != null)
                    {
                        this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing = value;
                        if (this.GetObjectContextConnection().Culture != null)
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(this.GetObjectContextConnection().Transaction))
                            {
                                using (CultureContext cultureContext = new CultureContext(this.GetObjectContextConnection().Culture, this.GetObjectContextConnection().UseDefaultCultureWhenValueMissing))
                                {

                                    BookViewModel menu = ViewModel.Menu.MenuDesignerHost.Current.Menu;
                                    if (menu != null)
                                        menu.RebuildAllPages();
                                }
                                stateTransition.Consistent = true;
                            }
                        }
                    }
                }
            }
        }
        public List<StyleableWindow.CulturePresentation> Cultures
        {
            get => StyleableWindow.CulturePresentation.Cultures;
        }

        private void BookViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BookViewModel.ZoomPercentage))
            {
                BookViewModel BookViewModel = ZoomViewBox.GetDataContextObject<BookViewModel>();
                if (BookViewModel != null)
                {
                    if (Zoom.HasValue && Zoom.Value != BookViewModel.ZoomPercentage)
                    {
                        Zoom = BookViewModel.ZoomPercentage;
                        ZoomViewBox.Height = (Zoom.Value / 100) * (ZoomViewBox.Child as FrameworkElement).ActualHeight;
                    }
                }
            }
        }

        double? Zoom;

        private void ZoomViewBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BookViewModel BookViewModel = ZoomViewBox.GetDataContextObject<BookViewModel>();
            if (BookViewModel != null)
            {
                BookViewModel.PropertyChanged += BookViewModel_PropertyChanged;
                if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                {
                    Zoom = (ZoomViewBox.ActualHeight / (ZoomViewBox.Child as FrameworkElement).ActualHeight) * 100;
                    BookViewModel.ZoomPercentage = Zoom.Value;
                }
            }
        }

        public string DefaultLanguage
        {
            get
            {
                BookViewModel BookViewModel = ZoomViewBox.GetDataContextObject<BookViewModel>();
                if (BookViewModel != null)
                    return BookViewModel.DefaultLanguage;
                return "";

            }
        }

        private void ZoomViewBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BookViewModel BookViewModel = ZoomViewBox.GetDataContextObject<BookViewModel>();
            if (BookViewModel != null)
            {
                if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                {
                    Zoom = (ZoomViewBox.ActualHeight / (ZoomViewBox.Child as FrameworkElement).ActualHeight) * 100;
                    BookViewModel.ZoomPercentage = Zoom.Value;
                }
            }
        }
        //HeadingTypesAccents

        /// <MetaDataID>{a579ef07-ddc1-424c-ad48-9e62220b4e81}</MetaDataID>
        private void BookScrollViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            ZoomViewBox.Height = BookScrollViewer.ActualHeight - (SystemParameters.HorizontalScrollBarHeight + 10);
            UpdateLayout();
        }




        /// <MetaDataID>{794af5f8-f785-483b-89eb-c3433b9c7db0}</MetaDataID>
        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("Height:{0},Width{1}", ZoomViewBox.ActualHeight, ZoomViewBox.ActualWidth));
            if (ZoomViewBox.ActualHeight + (e.Delta / 2) > 0)
                ZoomViewBox.Height = ZoomViewBox.ActualHeight + (e.Delta / 2);
        }

        /// <MetaDataID>{be2c9838-0e35-407a-917f-399777aa06b1}</MetaDataID>
        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            //{
            //    System.Windows.Window win = System.Windows.Window.GetWindow(sender as System.Windows.DependencyObject);

            //    var window = new MenuItemsEditor.Views.MenuItemsWindow(BookViewModel.RestaurantMenus);
            //    window.Owner = win;
            //    window.ShowDialog();
            //    stateTransition.Consistent = true;
            //}
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }



        //XDocument RestaurandMenuDoc;
        RawStorageData RestaurandMenuDoc;

        public event PropertyChangedEventHandler PropertyChanged;

        //string RestaurandMenuFileName;


        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            DesignerCanvas designerCanvas = WPFUIElementObjectBind.ObjectContext.FindChilds<DesignerCanvas>(sender as DependencyObject).FirstOrDefault();
            if (designerCanvas != null)
                designerCanvas.DragEnter(e);
        }



        private void Grid_DragLeave(object sender, DragEventArgs e)
        {
            DesignerCanvas designerCanvas = WPFUIElementObjectBind.ObjectContext.FindChilds<DesignerCanvas>(sender as DependencyObject).FirstOrDefault();
            if (designerCanvas != null)
                designerCanvas.DragLeave(e);
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            DesignerCanvas designerCanvas = WPFUIElementObjectBind.ObjectContext.FindChilds<DesignerCanvas>(sender as DependencyObject).FirstOrDefault();
            if (designerCanvas != null)
                designerCanvas.DragOver(e);
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            DesignerCanvas designerCanvas = WPFUIElementObjectBind.ObjectContext.FindChilds<DesignerCanvas>(sender as DependencyObject).FirstOrDefault();
            if (designerCanvas != null)
                designerCanvas.Drop(e);
        }

        private void LanguageClicked(object sender, MouseButtonEventArgs e)
        {

        }
    }

    /// <MetaDataID>{7bedc77d-eb1e-437d-97ab-8b884161c28d}</MetaDataID>
    public static class ViewBoxExtensions
    {
        public static double GetScaleFactor(this Viewbox viewbox)
        {
            if (viewbox.Child == null ||
                (viewbox.Child is FrameworkElement) == false)
            {
                return double.NaN;
            }
            FrameworkElement child = viewbox.Child as FrameworkElement;
            return viewbox.ActualWidth / child.ActualWidth;
        }
    }

    /// <MetaDataID>{61eaf0d0-63a0-4e79-bda1-6241aa198246}</MetaDataID>
    public class StyleFontUpdater
    {
        public readonly MenuPresentationModel.MenuStyles.IStyleRule Style;
        public readonly StyleableWindow.FontPresantation FontPresentation;
        public readonly string FontProperty;
        public StyleFontUpdater(MenuPresentationModel.MenuStyles.IStyleRule style, StyleableWindow.FontPresantation fontPresentation, string fontProperty)
        {
            FontProperty = fontProperty;
            FontPresentation = fontPresentation;
            Style = style;
            fontPresentation.PropertyChanged += FontPropertyChanged;
        }

        private void FontPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Style is MenuPresentationModel.MenuStyles.IMenuItemStyle)
            {
                if (FontProperty == "DescriptionFont")
                    (Style as MenuPresentationModel.MenuStyles.IMenuItemStyle).DescriptionFont = FontPresentation.Font;
                else if (FontProperty == "ExtrasFont")
                    (Style as MenuPresentationModel.MenuStyles.IMenuItemStyle).ExtrasFont = FontPresentation.Font;
                else if (FontProperty == "ItemInfoHeadingFont")
                    (Style as MenuPresentationModel.MenuStyles.IMenuItemStyle).ItemInfoHeadingFont = FontPresentation.Font;
                else if (FontProperty == "ItemInfoParagraphFont")
                    (Style as MenuPresentationModel.MenuStyles.IMenuItemStyle).ItemInfoParagraphFont = FontPresentation.Font;
                else if (FontProperty == "ItemInfoParagraphFirstLetterFont")
                    (Style as MenuPresentationModel.MenuStyles.IMenuItemStyle).ItemInfoParagraphFirstLetterFont= FontPresentation.Font;

                else
                    (Style as MenuPresentationModel.MenuStyles.IMenuItemStyle).Font = FontPresentation.Font;
            }

            if (Style is MenuPresentationModel.MenuStyles.IHeadingStyle)
                (Style as MenuPresentationModel.MenuStyles.IHeadingStyle).Font = FontPresentation.Font;
            if (Style is MenuPresentationModel.MenuStyles.IPriceStyle)
                (Style as MenuPresentationModel.MenuStyles.IPriceStyle).Font = FontPresentation.Font;
        }
    }
}
