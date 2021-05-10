
using MenuPresentationModel.MenuStyles;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{483e0295-fb8d-43be-8157-2ca6ddeabe68}</MetaDataID>
    public class LayoutOptionsPresentation : MarshalByRefObject, INotifyPropertyChanged
    {

        BookViewModel BookViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public readonly LayoutStyle LayoutStyle;
        public readonly PageStyle PageStyle;
        public readonly MenuItemStyle MenuItemStyle;
        public readonly PriceStyle PriceStyle;
        public LayoutOptionsPresentation(BookViewModel bookViewModel, 
                                            LayoutStyle layoutStyle,
                                            PageStyle pageStyle,
                                            MenuItemStyle menuItemStyle,
                                            PriceStyle priceStyle)
        {
            BookViewModel = bookViewModel;

            LayoutStyle = layoutStyle;
            PageStyle = pageStyle;
            MenuItemStyle = menuItemStyle;
            PriceStyle = priceStyle;
            PriceStyle.ObjectChangeState += PriceStyleObjectChangeState;
            LoadStyleSheetAttributes();
            UseStyleDefaultsCommand = new RelayCommand((object sender) =>
            {
                PageStyle.UseDefaultValues();

                PriceStyle.UseDefaultValues();
                MenuItemStyle.UseDefaultValues();
                LayoutStyle.UseDefaultValues();
                LoadStyleSheetAttributes();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Bottom)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.BulletsOptions)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ColumnsNums)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ColumnsWidths)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ExtraDescriptionBullet)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ExtraDescriptionPosOptions)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ExtrasBullet)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.FoodItemAlignmentOptions)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.IsCustomPaperSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ItemDescriptionLeftIndent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ItemDescriptionRightIndent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.ItemNameIndent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Landscape)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Left)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.LineBetweenColumns)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.LineSpacingOptions)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.LineThickness)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.LineTypes)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.MarginUnit)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.PageHeight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.PageWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.PaperSizes)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Portrait)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.PortraitLandscapeIsEnabled)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.PriceLocationOption)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.PriceLocationOptions)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Right)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SelectedFoodItemAlignmentOption)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SelectedLineColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SelectedLineSpacing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SelectedLineThickness)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SelectedNumOfColumns)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SelectedOtptionForExtraDescriptionPos)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SelectedPaperSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SeparationLineType)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.SpaceBetweenColumns)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Top)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Uneven)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LayoutOptionsPresentation.Unit)));


            });

            PriceOptionsDetailsCommand = new RelayCommand((object sender) =>
            {

                //System.Windows.Window window = sender as System.Windows.Window;

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                {

                    OOAdvantech.Transactions.Transaction.Current.TransactionCompleted += PriceOptionsTransactionCompleted;
                    PriceOptionsViewModel priceOptions = new PriceOptionsViewModel(bookViewModel,
                                                                                                                 //bookViewModel.RealObject.Style.Styles["layout"] as MenuPresentationModel.MenuStyles.LayoutStyle,
                                                                                                                 //bookViewModel.RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle,
                                                                                                                 //bookViewModel.RealObject.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle,
                                                                                                                 BookViewModel.RealObject.Style.Styles["price-options"] as MenuPresentationModel.MenuStyles.PriceStyle);
                    System.Windows.Window win = System.Windows.Window.GetWindow(sender as System.Windows.DependencyObject);
                    //  position = win.PointFromScreen(position);

                    Views.PriceOptionsForm priceOptionsForm = new Views.PriceOptionsForm();
                    priceOptionsForm.Owner = win;


                    priceOptionsForm.GetObjectContext().SetContextInstance(priceOptions);
                    if (priceOptionsForm.ShowDialog().Value)
                        stateTransition.Consistent = true;
                }

            });

        }

        private void LoadStyleSheetAttributes()
        {
            _ItemNameIndent = PixelToMM(LayoutStyle.NameIndent);
            _ItemDescriptionLeftIndent = PixelToMM(LayoutStyle.DescLeftIndent);
            _ItemDescriptionRightIndent = PixelToMM(LayoutStyle.DescRightIndent);
            _SpaceBetweenColumns = Math.Round(PixelToMM(LayoutStyle.SpaceBetweenColumns), 2);
            _LineBetweenColumns = LayoutStyle.LineBetweenColumns;
            _SelectedLineThickness = (int)LayoutStyle.SeparationLineThickness;

            _PriceLocationOption = (from priceLayout in _PriceLocationOptions
                                    where PriceStyle.Layout == priceLayout.Value
                                    select priceLayout.Key).FirstOrDefault();

            if (_SelectedLineThickness == 0)
                _SelectedLineThickness = 2;
            if (LayoutStyle.SeparationLineColor == null)
                _SelectedLineColor = Colors.Black;
            else
                _SelectedLineColor = (Color)ColorConverter.ConvertFromString(LayoutStyle.SeparationLineColor);

            _SeparationLineType = (from lineTypePair in _LineTypes
                                   where lineTypePair.Value == LayoutStyle.SeparationLineType
                                   select lineTypePair.Key).FirstOrDefault();

            _Top = PixelToMM(PageStyle.Margin.MarginTop);
            _Bottom = PixelToMM(PageStyle.Margin.MarginBottom);
            _Left = PixelToMM(PageStyle.Margin.MarginLeft);
            _Right = PixelToMM(PageStyle.Margin.MarginRight);

            _Top = Math.Round(_Top, 1);
            _Bottom = Math.Round(_Bottom, 1);
            _Left = Math.Round(_Left, 1);
            _Right = Math.Round(_Right, 1);

            var height = PixelToMM(PageStyle.PageHeight);
            var width = PixelToMM(PageStyle.PageWidth);


            _SelectedNumOfColumns = PageStyle.NumOfPageColumns;
            _Uneven = PageStyle.ColumnsUneven;
            UpdateColumnsWidths();


            var pageSize = (from paperSize in MenuPresentationModel.MenuStyles.PageStyle.PaperSizes
                            where AreSame(paperSize.Height, height) && AreSame(paperSize.Width, width)
                            select paperSize).FirstOrDefault();
            if (pageSize.PaperType != MenuPresentationModel.MenuStyles.PaperType.Unspecified)
            {
                _SelectedPaperSize = pageSize;
                _Portrait = pageSize.Height >= pageSize.Width;
                _Landscape = !_Portrait;
                _PageHeight = pageSize.Height;
                _PageWidth = pageSize.Width;

            }
            else
            {
                pageSize = (from paperSize in MenuPresentationModel.MenuStyles.PageStyle.PaperSizes
                            where AreSame(paperSize.Height, width) && AreSame(paperSize.Width, height)
                            select paperSize).FirstOrDefault();
                if (pageSize.PaperType != MenuPresentationModel.MenuStyles.PaperType.Unspecified)
                {
                    _SelectedPaperSize = pageSize;
                    _Portrait = false;
                    _Landscape = true;
                    _PageHeight = pageSize.Height;
                    _PageWidth = pageSize.Width;
                }
                else
                {
                    pageSize = (from thePaperSize in MenuPresentationModel.MenuStyles.PageStyle.PaperSizes
                                where thePaperSize.PaperType == MenuPresentationModel.MenuStyles.PaperType.Custom
                                select thePaperSize).FirstOrDefault();
                    _SelectedPaperSize = pageSize;
                    _Portrait = pageSize.Height >= pageSize.Width;
                    _Landscape = !_Portrait;
                    _PageHeight = Math.Round(height, 1);
                    _PageWidth = Math.Round(width, 1);
                }
            }
        }

        private void PriceOptionsTransactionCompleted(Transaction transaction)
        {
            _PriceLocationOption = _PriceLocationOption = (from priceLayout in _PriceLocationOptions
                                                           where PriceStyle.Layout == priceLayout.Value
                                                           select priceLayout.Key).FirstOrDefault();
        }

        public RelayCommand PriceOptionsDetailsCommand { get; protected set; }

        public RelayCommand UseStyleDefaultsCommand { get; protected set; }

        public bool PortraitLandscapeIsEnabled
        {
            get
            {
                if (_SelectedPaperSize.PaperType == MenuPresentationModel.MenuStyles.PaperType.Custom)
                    return false;
                else
                    return true;
            }
        }



        bool AreSame(double left, double right)
        {
            double dif = left - right;
            if (dif > 0)
            {
                if ((100 / left) * dif < 2)
                    return true;
                else
                    return false;
            }
            else
            {
                dif = right - left;
                if ((100 / right) * dif < 2)
                    return true;
                else
                    return false;
            }
        }
        internal static double PixelToMM(double px)
        {
            var pixpermm = (double)new System.Windows.LengthConverter().ConvertFromString("1in") / 25.4;
            return px / pixpermm;

            //return (int)Math.Round(px / pixpermm, 1);
        }


        internal static double mmToPixel(double mm)
        {
            var pixpermm = (double)new System.Windows.LengthConverter().ConvertFromString("1in") / 25.4;
            return mm * pixpermm;

            //return (int)Math.Round(mm * pixpermm, 1);
        }

        public List<MenuPresentationModel.MenuStyles.PaperSize> PaperSizes
        {
            get
            {
                return MenuPresentationModel.MenuStyles.PageStyle.PaperSizes;
            }
        }


        MenuPresentationModel.MenuStyles.PaperSize _SelectedPaperSize;
        public MenuPresentationModel.MenuStyles.PaperSize SelectedPaperSize
        {
            get
            {
                return _SelectedPaperSize;
            }
            set
            {
                _SelectedPaperSize = value;
                if (_SelectedPaperSize.Width != 0 && _SelectedPaperSize.Height != 0)
                {
                    PageWidth = _SelectedPaperSize.Width;
                    PageHeight = _SelectedPaperSize.Height;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCustomPaperSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPaperSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PortraitLandscapeIsEnabled)));

            }

        }

        /// <exclude>Excluded</exclude>
        double _PageWidth;
        public double PageWidth
        {
            set
            {
                _PageWidth = value;
                if (_PageWidth > 0)
                {
                    MenuPresentationModel.MenuStyles.PaperSize paperSize = new MenuPresentationModel.MenuStyles.PaperSize(
                                                                MenuPresentationModel.MenuStyles.PaperType.Custom,
                                                                MenuPresentationModel.MenuStyles.PaperType.Custom.ToString(), mmToPixel(_PageWidth), mmToPixel(PageHeight), "px");
                    PageStyle.PageSize = paperSize;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageWidth)));
            }
            get
            {
                return _PageWidth;
            }
        }

        /// <exclude>Excluded</exclude>
        double _PageHeight;
        public double PageHeight
        {
            set
            {
                _PageHeight = value;
                if (_PageHeight > 0)
                {
                    MenuPresentationModel.MenuStyles.PaperSize paperSize = new MenuPresentationModel.MenuStyles.PaperSize(
                                                              MenuPresentationModel.MenuStyles.PaperType.Custom,
                                                              MenuPresentationModel.MenuStyles.PaperType.Custom.ToString(), mmToPixel(_PageWidth), mmToPixel(PageHeight), "px");

                    PageStyle.PageSize = paperSize;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageHeight)));
            }
            get
            {

                return _PageHeight;
            }
        }


        List<double> _ColumnsWidths = new List<double>() { 35 };
        public List<double> ColumnsWidths
        {
            get
            {
                return _ColumnsWidths;
            }
        }
        /// <exclude>Excluded</exclude>
        List<int> _ColumnsNums = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        public List<int> ColumnsNums
        {
            get
            {
                return _ColumnsNums;
            }
        }

        /// <exclude>Excluded</exclude>
        int _SelectedNumOfColumns = 1;

        public int SelectedNumOfColumns
        {
            get
            {

                return _SelectedNumOfColumns;
            }
            set
            {
                _SelectedNumOfColumns = value;
                PageStyle.NumOfPageColumns = _SelectedNumOfColumns;
                UpdateColumnsWidths();
            }
        }

        /// <exclude>Excluded</exclude>
        List<int> _LineThickness = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        public List<int> LineThickness
        {
            get
            {
                return _LineThickness;
            }
        }

        System.Windows.Media.Color _SelectedLineColor = Colors.LightGray;
        public System.Windows.Media.Color SelectedLineColor
        {
            get
            {
                return _SelectedLineColor;
            }
            set
            {
                _SelectedLineColor = value;
                
                LayoutStyle.SeparationLineColor = new ColorConverter().ConvertToString(value);

            }
        }


        /// <exclude>Excluded</exclude>
        int _SelectedLineThickness = 1;
        public int SelectedLineThickness
        {
            get
            {
                return _SelectedLineThickness;
            }
            set
            {
                _SelectedLineThickness = value;
                LayoutStyle.SeparationLineThickness = value;
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
                if (_Left != value)
                {
                    _Left = value;
                    Margin margin = new Margin();
                    margin.MarginTop = mmToPixel(Top);
                    margin.MarginBottom = mmToPixel(Bottom);
                    margin.MarginLeft = mmToPixel(Left);
                    margin.MarginRight = mmToPixel(Right);
                    PageStyle.Margin = margin;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _Right;
        public double Right
        {
            get
            {
                return _Right;
            }
            set
            {
                if (_Right != value)
                {
                    //PixelFormatConverter
                    _Right = value;
                    UIBaseEx.Margin margin = new UIBaseEx.Margin();
                    margin.MarginTop = mmToPixel(Top);
                    margin.MarginBottom = mmToPixel(Bottom);
                    margin.MarginLeft = mmToPixel(Left);
                    margin.MarginRight = mmToPixel(Right);
                    PageStyle.Margin = margin;
                }
            }
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
                if (_Top != value)
                {
                    _Top = value;
                    UIBaseEx.Margin margin = new UIBaseEx.Margin();
                    margin.MarginTop = mmToPixel(Top);
                    margin.MarginBottom = mmToPixel(Bottom);
                    margin.MarginLeft = mmToPixel(Left);
                    margin.MarginRight = mmToPixel(Right);
                    PageStyle.Margin = margin;
                }
            }
        }


        /// <exclude>Excluded</exclude>
        double _Bottom;
        public double Bottom
        {
            get
            {
                return _Bottom;
            }
            set
            {
                if (_Bottom != value)
                {
                    _Bottom = value;
                    UIBaseEx.Margin margin = new UIBaseEx.Margin();
                    margin.MarginTop = mmToPixel(Top);
                    margin.MarginBottom = mmToPixel(Bottom);
                    margin.MarginLeft = mmToPixel(Left);
                    margin.MarginRight = mmToPixel(Right);
                    PageStyle.Margin = margin;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Portrait = true;
        public bool Portrait
        {
            get
            {
                return _Portrait;
            }
            set
            {
                if (value)
                    Landscape = false;

                _Portrait = value;
                //PageHeight = PageHeight;
                //PageWidth = PageWidth;
                if (_Portrait)
                {
                    MenuPresentationModel.MenuStyles.PaperSize paperSize = new MenuPresentationModel.MenuStyles.PaperSize(
                                                             MenuPresentationModel.MenuStyles.PaperType.Custom,
                                                             MenuPresentationModel.MenuStyles.PaperType.Custom.ToString(), mmToPixel(_PageWidth), mmToPixel(PageHeight), "px");
                    PageStyle.PageSize = paperSize;
                    //PageStyle.PageHeight = mmToPixel(_PageHeight);
                    //PageStyle.PageWidth = mmToPixel(_PageWidth);
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Portrait)));
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Landscape;

        public bool Landscape
        {
            get
            {
                return _Landscape;
            }
            set
            {
                if (value)
                    Portrait = false;
                _Landscape = value;

                if (_Landscape)
                {
                    MenuPresentationModel.MenuStyles.PaperSize paperSize = new MenuPresentationModel.MenuStyles.PaperSize(
                                                             MenuPresentationModel.MenuStyles.PaperType.Custom,
                                                             MenuPresentationModel.MenuStyles.PaperType.Custom.ToString(), mmToPixel(PageHeight), mmToPixel(_PageWidth), "px");
                    PageStyle.PageSize = paperSize;

                }

                //if (_Landscape)
                //{
                //    PageHeight = PageHeight;
                //    PageWidth = PageWidth;
                //}

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Landscape)));
            }
        }

        public string MarginUnit
        {
            get
            {
                return "mm";
            }
        }


        public bool IsCustomPaperSize
        {
            get
            {
                if (SelectedPaperSize.PaperType == MenuPresentationModel.MenuStyles.PaperType.Custom)
                    return true;
                else
                    return false;

            }
        }


        /// <exclude>Excluded</exclude>
        string _Unit = "mm";
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _ItemNameIndent;

        public double ItemNameIndent
        {
            get
            {
                return _ItemNameIndent;
            }
            set
            {
                _ItemNameIndent = value;
                LayoutStyle.NameIndent = mmToPixel(_ItemNameIndent);
            }
        }


        /// <exclude>Excluded</exclude>
        double _ItemDescriptionRightIndent;

        public double ItemDescriptionRightIndent
        {
            get
            {
                return _ItemDescriptionRightIndent;
            }
            set
            {
                _ItemDescriptionRightIndent = value;
                LayoutStyle.DescRightIndent = mmToPixel(_ItemDescriptionRightIndent);
            }
        }

        /// <exclude>Excluded</exclude>
        double _ItemDescriptionLeftIndent;

        public double ItemDescriptionLeftIndent
        {
            get
            {
                return _ItemDescriptionLeftIndent;
            }
            set
            {
                _ItemDescriptionLeftIndent = value;
                LayoutStyle.DescLeftIndent = mmToPixel(_ItemDescriptionLeftIndent);
            }
        }

        static List<string> _BulletsOptions = new List<string>() { "~", "-", "–", "—", ":", ">", "»", "•", "+", "*", "/", "|", "…", ",", "none", };

        public IList<string> BulletsOptions
        {
            get
            {
                return _BulletsOptions.AsReadOnly();
            }
        }

        public string ExtraDescriptionBullet
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LayoutStyle.DescSeparator))
                    return "none";
                return LayoutStyle.DescSeparator;
            }
            set
            {
                if (value == "none")
                    LayoutStyle.DescSeparator = "";
                else
                    LayoutStyle.DescSeparator = value;

            }
        }
        public string ExtrasBullet
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LayoutStyle.ExtrasSeparator))
                    return "none";

                return LayoutStyle.ExtrasSeparator;
            }
            set
            {
                if (value == "none")
                    LayoutStyle.ExtrasSeparator = "";
                else
                    LayoutStyle.ExtrasSeparator = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _SpaceBetweenColumns = 1.1;
        public double SpaceBetweenColumns
        {
            get
            {
                return _SpaceBetweenColumns;
            }
            set
            {
                _SpaceBetweenColumns = value;
                LayoutStyle.SpaceBetweenColumns =Math.Round( mmToPixel(_SpaceBetweenColumns),2);
            }
        }


        /// <exclude>Excluded</exclude>
        bool _LineBetweenColumns;
        /// <summary>
        /// 
        /// </summary>
        public bool LineBetweenColumns
        {
            get
            {
                return _LineBetweenColumns;
            }
            set
            {
                _LineBetweenColumns = value;
                LayoutStyle.LineBetweenColumns = value;
            }
        }

        /// <exclude>Excluded</exclude>
        static List<string> _ExtraDescriptionPosOptions = new List<string>() { Properties.Resources.ExtraDescriptionFollowNamePrompt, Properties.Resources.ExtraDescriptionInNewLinePrompt };
        public IList<string> ExtraDescriptionPosOptions
        {
            get
            {
                return _ExtraDescriptionPosOptions.AsReadOnly();

            }
        }

        /// <exclude>Excluded</exclude>
        string _SelectedOtptionForExtraDescriptionPos;
        public string SelectedOtptionForExtraDescriptionPos
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_SelectedOtptionForExtraDescriptionPos))
                {
                    if (MenuItemStyle.NewLineForDescription)
                        _SelectedOtptionForExtraDescriptionPos = _ExtraDescriptionPosOptions[1];
                    else
                        _SelectedOtptionForExtraDescriptionPos = _ExtraDescriptionPosOptions[0];
                }

                return _SelectedOtptionForExtraDescriptionPos;
            }
            set
            {
                _SelectedOtptionForExtraDescriptionPos = value;
                if (_SelectedOtptionForExtraDescriptionPos == _ExtraDescriptionPosOptions[1])
                    MenuItemStyle.NewLineForDescription = true;
                else
                    MenuItemStyle.NewLineForDescription = false;

            }
        }


        static Dictionary<string, PriceLayout> _PriceLocationOptions = new  Dictionary<string, PriceLayout>() { { Properties.Resources.PriceLayoutNormalPrompt, PriceLayout.Normal },
                                                                                                                { Properties.Resources.PriceLayoutWithNamePrompt,PriceLayout.WithName },
                                                                                                                { Properties.Resources.PriceLayoutWithDescriptionPrompt,PriceLayout.WithDescription },
                                                                                                                { Properties.Resources.PriceLayoutFollowDescriptionPrompt,PriceLayout.FollowDescription },
                                                                                                                { Properties.Resources.PriceLayoutDoNotDisplayPrompt,PriceLayout.DoNotDisplay } };

        public IList<string> PriceLocationOptions
        {
            get
            {
                return _PriceLocationOptions.Keys.ToList();
            }
        }

        string _PriceLocationOption;
        public string PriceLocationOption
        {
            get
            {
                return _PriceLocationOption;
            }
            set
            {
                if (_PriceLocationOption != value)
                {
                    _PriceLocationOption = value;
                    PriceStyle.Layout = _PriceLocationOptions[value];
                }
            }
        }

        private void PriceStyleObjectChangeState(object _object, string member)
        {
            if (member == nameof(IPriceStyle.Layout))
            {
                _PriceLocationOption = _PriceLocationOption = (from priceLayout in _PriceLocationOptions
                                                               where PriceStyle.Layout == priceLayout.Value
                                                               select priceLayout.Key).FirstOrDefault();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceLocationOption)));
            }
        }

        /// <exclude>Excluded</exclude>
        static List<string> _FoodItemAlignmentOptions = new List<string>() { Properties.Resources.FooItemAlignLeftPrompt, Properties.Resources.FooItemAlignCenterPrompt, Properties.Resources.FooItemAlignRightPrompt };
        public IList<string> FoodItemAlignmentOptions
        {
            get
            {
                return _FoodItemAlignmentOptions.AsReadOnly();

            }
        }

        /// <exclude>Excluded</exclude>
        string _SelectedFoodItemAlignmentOption;
        public string SelectedFoodItemAlignmentOption
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_SelectedFoodItemAlignmentOption))
                {
                    switch (MenuItemStyle.Alignment)
                    {
                        case MenuPresentationModel.MenuStyles.Alignment.Left:
                            _SelectedFoodItemAlignmentOption = _FoodItemAlignmentOptions[0];
                            break;
                        case MenuPresentationModel.MenuStyles.Alignment.Center:
                            _SelectedFoodItemAlignmentOption = _FoodItemAlignmentOptions[1];
                            break;
                        case MenuPresentationModel.MenuStyles.Alignment.Right:
                            _SelectedFoodItemAlignmentOption = _FoodItemAlignmentOptions[2];
                            break;
                    }
                }
                return _SelectedFoodItemAlignmentOption;
            }
            set
            {
                _SelectedFoodItemAlignmentOption = value;
                if (_SelectedFoodItemAlignmentOption == _FoodItemAlignmentOptions[0])
                    MenuItemStyle.Alignment = MenuPresentationModel.MenuStyles.Alignment.Left;
                if (_SelectedFoodItemAlignmentOption == _FoodItemAlignmentOptions[1])
                    MenuItemStyle.Alignment = MenuPresentationModel.MenuStyles.Alignment.Center;
                if (_SelectedFoodItemAlignmentOption == _FoodItemAlignmentOptions[2])
                    MenuItemStyle.Alignment = MenuPresentationModel.MenuStyles.Alignment.Right;

            }
        }


        List<double> _LineSpacingOptions = new List<double>() { 0.85, 0.95, 1, 1.15, 1.3, 1.5, 2 };

        public IList<double> LineSpacingOptions
        {
            get
            {
                return _LineSpacingOptions.AsReadOnly();
            }
        }


        public double SelectedLineSpacing
        {
            get
            {
                return LayoutStyle.LineSpacing;
            }
            set
            {
                LayoutStyle.LineSpacing = value;
            }
        }


        int _Uneven = 50;
        public int Uneven
        {
            get
            {
                return PageStyle.ColumnsUneven;
            }
            set
            {
                _Uneven = value;

                UpdateColumnsWidths();
            }
        }
        Dictionary<string,MenuPresentationModel.MenuStyles.LineType> _LineTypes = new Dictionary<string, MenuPresentationModel.MenuStyles.LineType>()
                                                                                    { { "|" , MenuPresentationModel.MenuStyles.LineType .Single},
                                                                                    { "||", MenuPresentationModel.MenuStyles.LineType.Double },
                                                                                    { "¦", MenuPresentationModel.MenuStyles.LineType.Dashed },
                                                                                    { "⋮", MenuPresentationModel.MenuStyles.LineType.Dotted },
                                                                                    { "↕", MenuPresentationModel.MenuStyles.LineType.Arrowend },
                                                                                    { "↕▴", MenuPresentationModel.MenuStyles.LineType.ArrowendEx },
                                                                                    { "⊡", MenuPresentationModel.MenuStyles.LineType.ColumnBox } };
        public List<string> LineTypes
        {
            get
            {
                return _LineTypes.Keys.ToList();
            }
        }

        string _SeparationLineType;
        public string SeparationLineType
        {
            get
            {
                return _SeparationLineType;

            }
            set
            {
                
                if(_SeparationLineType != value)
                {
                    _SeparationLineType = value;
                    LayoutStyle.SeparationLineType = _LineTypes[value];
                }
                
            }
        }
     

        public void UnevenSlipStopped()
        {
            
            PageStyle.ColumnsUneven = _Uneven;
        }

        private void UpdateColumnsWidths()
        {
            if (SelectedNumOfColumns == 1)
            {
                _ColumnsWidths = new List<double>() { 35 };
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColumnsWidths)));
            }
            if (SelectedNumOfColumns > 1)
            {
                //double uneven = _Uneven;
                //double ratio = 150;
                //double columnWidth = 35;
                //if (uneven < 50)
                //{
                //    _ColumnsWidths = new List<double>() { columnWidth / (1 + ((50 - uneven) / ratio)), columnWidth * (1 + ((50 - uneven) / ratio)) };
                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColumnsWidths)));
                //}
                //else
                //{
                //    _ColumnsWidths = new List<double>() { columnWidth * (1 + ((uneven - 50) / ratio)), columnWidth / (1 + ((uneven - 50) / ratio)) };
                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColumnsWidths)));
                //}

                double uneven = _Uneven;
                double ratio = 150;
                double columnWidth = 35;
                int columnsCount = SelectedNumOfColumns;
                if (columnsCount > 3)
                    columnsCount = 3;
                if (uneven < 50)
                {
                    double narrow = columnWidth / (1 + ((50 - uneven) / ratio));
                    _ColumnsWidths = new List<double>() { narrow };
                    for (int i = 0; i != columnsCount - 1; i++)
                        _ColumnsWidths.Add((columnWidth * columnsCount - narrow) / (columnsCount - 1));
                }
                else
                {
                    double narrow = columnWidth / (1 + ((uneven - 50) / ratio));
                    _ColumnsWidths = new List<double>() { narrow };
                    for (int i = 0; i != columnsCount - 1; i++)
                        _ColumnsWidths.Insert(0, (columnWidth * columnsCount - narrow) / (columnsCount - 1));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColumnsWidths)));
            }
            //if (SelectedNumOfColumns > 2)
            //{
            //    double uneven = _Uneven;
            //    double ratio = 150;
            //    double columnWidth = 35;
            //    int columnsCount = 3;
            //    if (uneven < 50)
            //    {
            //        double narrow = columnWidth / (1 + ((50 - uneven) / ratio));
            //        _ColumnsWidths = new List<double>() { narrow };

            //        for (int i = 0; i != columnsCount - 1; i++)
            //            _ColumnsWidths.Add((columnWidth * columnsCount - narrow) / (columnsCount-1));
            //    }
            //    else
            //    {
            //        double narrow=columnWidth / (1 + ((uneven - 50) / ratio));
            //        _ColumnsWidths = new List<double>() { narrow };

            //        for (int i = 0; i != columnsCount - 1; i++)
            //            _ColumnsWidths.Insert(0,(columnWidth * columnsCount - narrow) / (columnsCount -1));

            //    }
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColumnsWidths)));
            //}
        }
    }
}
