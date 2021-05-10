using MenuPresentationModel.MenuCanvas;
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

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{a23d85b6-bb11-4fa5-ac31-a232317d9063}</MetaDataID>
    [Transactional]
    public class PriceHeadingViewModel : MarshalByRefObject, ICanvasItem, INotifyPropertyChanged
    {
        internal IMenuCanvasItem MenuCanvasItem;

        internal IPriceHeading PriceHeading;
        public PriceHeadingViewModel(IPriceHeading priceHeading)
        {
            PriceHeading = priceHeading;
            MenuCanvasItem = priceHeading;
            _Visibility = Visibility.Visible;
            UpdateCanvasItemValues();
        }

        /// <exclude>Excluded</exclude>
        double _CenterX;
        public double CenterX
        {
            get
            {
                return _CenterX;
            }
        }

        /// <exclude>Excluded</exclude>
        double _CenterY;

        public double CenterY
        {
            get
            {
                return _CenterY;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Angle;
        public double Angle
        {
            get
            {
                return _Angle;
            }
        }

        /// <exclude>Excluded</exclude>
        Visibility _Visibility= Visibility.Collapsed;
        public Visibility Visibility
        {
            get
            {
                return _Visibility;
            }
            set
            {
                if (_Visibility != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Visibility = value;
                        stateTransition.Consistent = true;
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
                }
            }
        }
        public void ChangeCanvasItem(IPriceHeading priceHeading)
        {
            if (MenuCanvasItem != priceHeading)
            {
                //if (MenuCanvasItem is MenuCanvasFoodItem)
                //    (MenuCanvasItem as MenuCanvasFoodItem).ObjectChangeState -= ObjectChangeState;

                MenuCanvasItem = priceHeading;
                PriceHeading = priceHeading;
                //if (MenuCanvasItem is MenuCanvasFoodItem)
                //    (MenuCanvasItem as MenuCanvasFoodItem).ObjectChangeState += ObjectChangeState;

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
                ////PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Font)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Foreground)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontFamily)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontStyle)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontWeight)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCaps)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeFill)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Overline)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Underline)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSize)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSpacing)));

                UpdateCanvasItemValues();
            }

        }

        private void UpdateCanvasItemValues()
        {

            //if (Transaction.Current != null)
            //    Transaction.Current.TransactionCompleted += OnTransactionCompleted; 

            var transformOrigin = PriceHeading.TransformOrigin;
            double x = 0;
            double y = 0;
            if (transformOrigin.xAxis == "center")
                x = PriceHeading.PriceHeadingTextWitdh / 2;
            if (transformOrigin.xAxis == "left" || transformOrigin.xAxis == "start")
                x = 0;
            if (transformOrigin.xAxis == "right" || transformOrigin.xAxis == "end")
                x = PriceHeading.PriceHeadingTextWitdh;
            if (transformOrigin.yAxis == "center")
                y = PriceHeading.Height / 2;
            if (transformOrigin.yAxis == "top" || transformOrigin.yAxis == "start")
                y = 0;
            if (transformOrigin.yAxis == "bottom" || transformOrigin.yAxis == "end")
                y = PriceHeading.Height;

            if (_CenterX != x)
            {
                _CenterX = x;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CenterX)));
            }

            if (_CenterY != y)
            {
                _CenterY = y;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CenterY)));
            }


            if (_Angle!=PriceHeading.Angle)
            {
                _Angle = PriceHeading.Angle;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Angle)));
            }


            if (_Top != this.MenuCanvasItem.YPos)
            {
                _Top = this.MenuCanvasItem.YPos;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            }

            if (_Left != PriceHeading.PriceHeadinTextXPos)
            {
                _Left = PriceHeading.PriceHeadinTextXPos;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
            }

            if (MenuCanvasItem.Font.AllCaps)
            {
                if (_Text != MenuCanvasItem.Description.ToUpper())
                {
                    _Text = MenuCanvasItem.Description.ToUpper();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }
            else
            {
                if (_Text != MenuCanvasItem.Description)
                {
                    _Text = MenuCanvasItem.Description;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }

            FontFamily fontFamily = null;
            if (Font != null)
                fontFamily = FontData.FontFamilies[Font.Value.FontFamilyName];
            else
                fontFamily = null;

            if (fontFamily != _FontFamily)
            {
                _FontFamily = fontFamily;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontFamily)));
            }

            FontStyle fontStyle = default(FontStyle);

            if (Font != null)
                fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(Font.Value.FontStyle);
            else
                fontStyle = default(FontStyle);
            if (fontStyle != _FontStyle)
            {
                _FontStyle = fontStyle;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontStyle)));
            }

            FontWeight fontWeight = default(FontWeight);
            if (Font != null)
                fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(Font.Value.FontWeight);
            else
                fontWeight = default(FontWeight);

            if (fontWeight != _FontWeight)
            {
                _FontWeight = fontWeight;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontWeight)));

            }

            double strokeThickness = 0;
            if (Font != null && Font.Value.Stroke)
                strokeThickness = Font.Value.StrokeThickness;
            else
                strokeThickness = 0;

            if (strokeThickness != _StrokeThickness)
            {
                _StrokeThickness = strokeThickness;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
            }

            bool allCaps = false;
            if (Font != null)
                allCaps = Font.Value.AllCaps;
            else
                allCaps = false;

            if (allCaps != _AllCaps)
            {
                _AllCaps = allCaps;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCaps)));
            }

            Color strokeFill = default(Color);

            if (Font != null)
            {
                if (Font.Value.StrokeFill != null && Font.Value.Stroke)
                    strokeFill = (Color)ColorConverter.ConvertFromString(Font.Value.StrokeFill);
                else
                    strokeFill = _Foreground;
            }
            else
                strokeFill = _Foreground;

            if (strokeFill != _StrokeFill)
            {
                _StrokeFill = strokeFill;
                _StrokeFillBrush = new SolidColorBrush(strokeFill);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeFill)));
            }


            System.Windows.Media.Effects.DropShadowEffect dropShadowEffect = null;

            if (Font != null)
            {
                double deltaX = Font.Value.ShadowXOffset;
                double deltaY = Font.Value.ShadowXOffset;

                if (Font.Value.ShadowColor == null && deltaX == 0 && deltaY == 0)
                    dropShadowEffect = null;
                else
                {

                    deltaY = -deltaY;
                    var rad = Math.Atan2(deltaY, deltaX);

                    var deg = rad * (180 / Math.PI);

                    if (deg < 0)
                        deg = 360 + deg;

                    double a = deltaX;
                    double b = deltaY;
                    if (a < 0)
                        a = -a;
                    if (b < 0)
                        b = -b;
                    double depth = Math.Sqrt(a * a + b * b);

                    var shaddow = new System.Windows.Media.Effects.DropShadowEffect();
                    shaddow.Direction = deg;
                    shaddow.ShadowDepth = depth;

                    shaddow.Opacity = 1;
                    shaddow.BlurRadius = Font.Value.BlurRadius;
                    shaddow.Color = (Color)ColorConverter.ConvertFromString(Font.Value.ShadowColor);
                    dropShadowEffect = shaddow;
                }
            }
            else
                dropShadowEffect = null;

            if (dropShadowEffect != _DropShadowEffect)
            {
                _DropShadowEffect = dropShadowEffect;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
            }

            Color foreground = default(Color);

            if (Font != null)
                foreground = (Color)ColorConverter.ConvertFromString(Font.Value.Foreground);
            else
                foreground = default(Color);

            if (foreground != _Foreground)
            {
                _Foreground = foreground;
                _ForegroundBrush = new SolidColorBrush(foreground);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Foreground)));
            }

            double fontSize = 0;
            if (Font != null)
                fontSize = Font.Value.FontSize;
            else
                fontSize = 0;

            if (fontSize != _FontSize)
            {
                _FontSize = fontSize;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSize)));
            }

            double fontSpacing = 0;
            if (Font != null)
                fontSpacing = Font.Value.FontSpacing;
            else
                fontSpacing = 0;

            if (fontSpacing != _FontSpacing)
            {
                _FontSpacing = fontSpacing;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSpacing)));
            }
        }

        private void OnTransactionCompleted(Transaction transaction)
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
        }

        /// <exclude>Excluded</exclude>
        double _Top;
        /// <MetaDataID>{fdf0ff7d-ec50-402d-967e-ed9699e53281}</MetaDataID>
        public double Top
        {
            get
            {
                return _Top;
            }
        }
        /// <exclude>Excluded</exclude>
        double _Left;
        /// <MetaDataID>{c591ecc0-f544-4fcf-9440-8176db2484ba}</MetaDataID>
        public double Left
        {
            get
            {
                return _Left;
            }
        }
        /// <exclude>Excluded</exclude> 
        string _Text;
        /// <MetaDataID>{09379650-7810-4b92-bdd1-c4b6a0e287db}</MetaDataID>
        public string Text
        {
            get
            {
                return _Text;
            }
        }

        /// <exclude>Excluded</exclude>
        System.Windows.Media.FontFamily _FontFamily;
        /// <MetaDataID>{55ca289c-24b5-4ace-843c-ef0199a69411}</MetaDataID>
        public System.Windows.Media.FontFamily FontFamily
        {
            get
            {
                return _FontFamily;

            }
        }

        /// <exclude>Excluded</exclude>
        FontStyle _FontStyle;
        /// <MetaDataID>{b9f2f208-7f90-4a42-9bc5-c8a8c780147e}</MetaDataID>
        public FontStyle FontStyle
        {
            get
            {
                return _FontStyle;

            }
        }

        /// <exclude>Excluded</exclude>
        FontWeight _FontWeight;
        /// <MetaDataID>{c88a77b9-fcbe-4bb6-9f4b-bf81f2fe6b3e}</MetaDataID>
        public FontWeight FontWeight
        {
            get
            {
                return _FontWeight;
            }
        }


        /// <exclude>Excluded</exclude>
        double _StrokeThickness;
        public double StrokeThickness
        {
            get
            {
                return _StrokeThickness;
            }
        }


        /// <exclude>Excluded</exclude>
        bool _AllCaps;
        public bool AllCaps
        {
            get
            {
                return _AllCaps;
            }
        }

        /// <exclude>Excluded</exclude>
        Color _StrokeFill;
        Brush _StrokeFillBrush;
        public Brush StrokeFill
        {
            get
            {
                return _StrokeFillBrush;
            }
        }

        public FontData? Font
        {
            get
            {
                return MenuCanvasItem.Font;
                //if (MenuCanvasItem is IMenuCanvasHeading)
                //    return (MenuCanvasItem as IMenuCanvasHeading).Font;
                //if (MenuCanvasItem is IMenuCanvasFoodItem)
                //    return (MenuCanvasItem as IMenuCanvasFoodItem).Font;
                //if (MenuCanvasItem is IMenuCanvasFoodItemPrice)
                //    return (MenuCanvasItem as IMenuCanvasFoodItemPrice).Font;

                //if (MenuCanvasItem is IMenuCanvasPriceLeader )
                //    return (MenuCanvasItem as IMenuCanvasPriceLeader).Font;


                //return null;
            }
        }

        /// <exclude>Excluded</exclude>
        System.Windows.Media.Effects.DropShadowEffect _DropShadowEffect;
        public System.Windows.Media.Effects.DropShadowEffect DropShadowEffect
        {
            get
            {
                return _DropShadowEffect;
            }
        }

        /// <exclude>Excluded</exclude>
        Color _Foreground;

        /// <exclude>Excluded</exclude>
        Brush _ForegroundBrush;

        /// <MetaDataID>{47626bc6-45e7-4c24-ba13-ac1112bc0345}</MetaDataID>
        public Brush Foreground
        {
            get
            {
                return _ForegroundBrush;
            }
        }

        public bool Overline
        {
            get
            {
                return false;
            }
        }
        public bool Underline
        {
            get
            {
                return false;
            }
        }

        /// <exclude>Excluded</exclude>
        double _FontSize;
        /// <MetaDataID>{e6273d48-ed55-4313-9ee7-8803ba40ecd1}</MetaDataID>
        public double FontSize
        {
            get
            {
                return _FontSize;

            }
        }

        /// <exclude>Excluded</exclude>
        double _FontSpacing;

        public event PropertyChangedEventHandler PropertyChanged;

        public double FontSpacing
        {
            get
            {
                return _FontSpacing;
            }
        }

        public double Height
        {
            get
            {
                return 0;
            }
        }

        public double Width
        {
            get
            {
                return 0;
            }
        }

        internal void Refresh()
        {
            UpdateCanvasItemValues();
        }
        public void Release()
        {

        }
    }
}
