using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MenuPresentationModel.MenuCanvas;
using OOAdvantech.Transactions;
using UIBaseEx;

 namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{dc87d504-e2a8-4e94-b400-fdf9a6ff7027}</MetaDataID>
    [Transactional]
    public class MenuCanvasItemTextViewModel : MarshalByRefObject, ICanvasItem, INotifyPropertyChanged
    {
        /// <MetaDataID>{312c21d3-16f3-4493-af7f-ffe6692500df}</MetaDataID>
        internal IMenuCanvasItem MenuCanvasItem;

        public event PropertyChangedEventHandler PropertyChanged;


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

                    
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
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

        public System.Windows.HorizontalAlignment MenuItemAlignment
        {
            get
            {
                if (MenuCanvasItem is MenuPresentationModel.MenuCanvas.IMenuCanvasHeading)
                {
                    if ((MenuCanvasItem as MenuPresentationModel.MenuCanvas.IMenuCanvasHeading).Alignment == MenuPresentationModel.MenuStyles.Alignment.Center)
                        return System.Windows.HorizontalAlignment.Center;

                    if ((MenuCanvasItem as MenuPresentationModel.MenuCanvas.IMenuCanvasHeading).Alignment == MenuPresentationModel.MenuStyles.Alignment.Left)
                        return System.Windows.HorizontalAlignment.Left;

                    if ((MenuCanvasItem as MenuPresentationModel.MenuCanvas.IMenuCanvasHeading).Alignment == MenuPresentationModel.MenuStyles.Alignment.Right)
                        return System.Windows.HorizontalAlignment.Right;
                }

                return System.Windows.HorizontalAlignment.Center;
            }
        }

        ///// <exclude>Excluded</exclude>
        //bool _IsHitTestVisible;
        //public bool IsHitTestVisible
        //{
        //    get
        //    {
        //        return _IsHitTestVisible;
        //    }
        //    set
        //    {
        //        _IsHitTestVisible = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsHitTestVisible)));
        //    }
        //}


        /// <MetaDataID>{bad63ab7-e239-4689-a8a5-f60978a4d0f8}</MetaDataID>
        public MenuCanvasItemTextViewModel(MenuPresentationModel.MenuCanvas.IMenuCanvasItem menuCanvasItem)
        {
            MenuCanvasItem = menuCanvasItem;
            Visibility = Visibility.Visible;
            UpdateCanvasItemValues();
            if (MenuCanvasItem is MenuCanvasFoodItem)
                (MenuCanvasItem as MenuCanvasFoodItem).ObjectChangeState += ObjectChangeState;

        }

        private void MenuCanvasItemTextViewModel__PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
        }

        private void ObjectChangeState(object _object, string member)
        {

            if (member == nameof(IMenuCanvasItem.Description))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
        }

        CanvasFrameViewModel _MenuCanvasItemFrame;
        public CanvasFrameViewModel MenuCanvasItemFrame
        {
            get
            {
                //if (_MenuCanvasItemFrame == null)
                //    _MenuCanvasItemFrame = new CanvasFrameViewModel(MenuCanvasItem);

                return _MenuCanvasItemFrame;
            }
        }
        public void ChangeCanvasItem(MenuPresentationModel.MenuCanvas.IMenuCanvasItem menuCanvasItem)
        {
            if (MenuCanvasItem != menuCanvasItem)
            {
                if (MenuCanvasItem is MenuCanvasFoodItem)
                    (MenuCanvasItem as MenuCanvasFoodItem).ObjectChangeState -= ObjectChangeState;

                MenuCanvasItem = menuCanvasItem;
                if (MenuCanvasItem is MenuCanvasFoodItem)
                    (MenuCanvasItem as MenuCanvasFoodItem).ObjectChangeState += ObjectChangeState;

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

            bool untranslated = false;
            //if(Transaction.Current!=null)
            //    Transaction.Current.TransactionCompleted += OnTransactionCompleted;   

            if (OOAdvantech.CultureContext.UseDefaultCultureValue)
            {
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    string text = MenuCanvasItem.Description;
                    if(MenuCanvasItem is MenuCanvasFoodItemText)
                    {
                        text = (MenuCanvasItem as MenuCanvasFoodItemText).FoodItem.Description;
                        if (text == "...")
                            text = null;
                    }
                    untranslated = string.IsNullOrWhiteSpace(text);
                }
            }

            if (_Top != this.MenuCanvasItem.YPos)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Top = this.MenuCanvasItem.YPos; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            }
            if (_Left != MenuCanvasItem.XPos)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Left = MenuCanvasItem.XPos; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
            }
            if (_Width!= MenuCanvasItem.Width)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Width = MenuCanvasItem.Width;
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
            }
            if (_Height!= MenuCanvasItem.Height)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Height = MenuCanvasItem.Height;
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
            }

            if (MenuCanvasItem.Font.AllCaps&& MenuCanvasItem.Description!=null)
            {

                if (_Text != MenuCanvasItem.Description.ToUpper())
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        
                        _Text = MenuCanvasItem.Description.ToUpper(); 

                   
                     
                        stateTransition.Consistent = true;
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }
            else
            {
                if (_Text != MenuCanvasItem.Description)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Text = MenuCanvasItem.Description;

                    

                        stateTransition.Consistent = true;
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
                }
            }

            FontFamily fontFamily = null;
            if (Font != null)
            {
                string fontFamilyName = Font.Value.FontFamilyName+ ", Arial, SansSerif";
                
                
                fontFamily = FontData.FontFamilies[Font.Value.FontFamilyName];
            }
            else
                fontFamily = null;

            if (fontFamily != _FontFamily)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontFamily = fontFamily; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontFamily)));
            }

            FontStyle fontStyle = default(FontStyle);

            if (Font != null)
                fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(Font.Value.FontStyle);
            else
                fontStyle = default(FontStyle);
            if (fontStyle != _FontStyle)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontStyle = fontStyle; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontStyle)));
            }

            FontWeight fontWeight = default(FontWeight);
            if (Font != null)
                fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(Font.Value.FontWeight);
            else
                fontWeight = default(FontWeight);

            if (fontWeight != _FontWeight)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontWeight = fontWeight; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontWeight)));

            }

            double strokeThickness = 0;
            if (Font != null && Font.Value.Stroke)
                strokeThickness = Font.Value.StrokeThickness;
            else
                strokeThickness = 0;

            if (strokeThickness != _StrokeThickness)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StrokeThickness = strokeThickness; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
            }

            bool allCaps = false;
            if (Font != null)
                allCaps = Font.Value.AllCaps;
            else
                allCaps = false;

            if(allCaps!=_AllCaps)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _AllCaps = allCaps; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCaps)));
            }


            Color foreground = default(Color);
            if (Font != null)
                foreground = (Color)ColorConverter.ConvertFromString(Font.Value.Foreground);
            else
                foreground = default(Color);

            if (foreground != _Foreground || untranslated)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Foreground = foreground;
                    _ForegroundBrush = new SolidColorBrush(foreground);
                    stateTransition.Consistent = true;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Foreground)));
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

            if (strokeFill!=_StrokeFill|| _StrokeFillBrush==null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StrokeFill = strokeFill; 
                    stateTransition.Consistent = true;
                }

                _StrokeFillBrush =new SolidColorBrush( strokeFill);
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

            if(dropShadowEffect!=_DropShadowEffect)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _DropShadowEffect = dropShadowEffect; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
            }



            double fontSize = 0;
            if (Font != null)
                fontSize = Font.Value.FontSize;
            else
                fontSize = 0;


            //if(Font!=null&& MenuCanvasItem.Width != Font.Value.MeasureText(_Text).Width)
            //{
            //    //var font = Font.Value;
            //    //fontSize = fontSize * (MenuCanvasItem.Width / Font.Value.MeasureText(_Text).Width);
            //    //font.FontSize = fontSize;
            //    //var wi = font.MeasureText(_Text).Width;

            //    //var baseLineDif = MenuCanvasItem.BaseLine-font.GetsTextBaseLine(_Text)  ;

            //    //_Top += baseLineDif;
            //    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            //}

            if (fontSize!=_FontSize)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontSize = fontSize; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSize)));
            }

            double fontSpacing = 0;
            if (Font != null)
                fontSpacing = Font.Value.FontSpacing;
            else
                fontSpacing = 0;

            if (fontSpacing!=_FontSpacing)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontSpacing = fontSpacing; 
                    stateTransition.Consistent = true;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSpacing)));
            }


        }


        static void RgbToHls(int r, int g, int b,
        out double h, out double l, out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;

            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;

            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;

            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;

                h = h * 60;
                if (h < 0) h += 360;
            }
        }

        // Convert an HLS value into an RGB value.
        static void HlsToRgb(double h, double l, double s,
            out int r, out int g, out int b)
        {
            double p2;
            if (l <= 0.5) p2 = l * (1 + s);
            else p2 = l + s - l * s;

            double p1 = 2 * l - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = l;
                double_g = l;
                double_b = l;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            r = (int)(double_r * 255.0);
            g = (int)(double_g * 255.0);
            b = (int)(double_b * 255.0);
        }
        private static double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;

            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
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
        double _Width;
        /// <MetaDataID>{c591ecc0-f544-4fcf-9440-8176db2484ba}</MetaDataID>
        public double Width
        {
            get
            {
                return _Width;
            }
        }

        /// <exclude>Excluded</exclude>
        double _Height;
        /// <MetaDataID>{c591ecc0-f544-4fcf-9440-8176db2484ba}</MetaDataID>
        public double Height
        {
            get
            {
                return _Height;
            }
        }


        /// <exclude>Excluded</exclude> 
        string _Text;
        /// <MetaDataID>{09379650-7810-4b92-bdd1-c4b6a0e287db}</MetaDataID>
        public string Text
        {
            get
            {
                if (_Text == null)
                    return "";
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
        public double FontSpacing
        {
            get
            {
                return _FontSpacing;
            }
        }

        //public double Height
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //}

        //public double Width
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //}

        internal void Refresh()
        {
            UpdateCanvasItemValues();
        }

        public void Release()
        {

        }
    }
}
