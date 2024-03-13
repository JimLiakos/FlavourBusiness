using OOAdvantech.UserInterface.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuItemsEditor.Views
{
    /// <summary>
    /// Interaction logic for ContextPriceTextBox.xaml
    /// </summary>
    /// <MetaDataID>{6e36fead-7cd5-4c40-bb9e-27e4488b1891}</MetaDataID>
    public partial class ContextPriceTextBox : ContentControl, INotifyPropertyChanged
    {
        public ContextPriceTextBox()
        {
            InitializeComponent();

            ContextPrice.DataContext = this;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (OverridePrice != -1)
                RemovePriceBtn.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            RemovePriceBtn.Visibility = Visibility.Collapsed;
        }


        ToolTip _ContextPriceToolTip;

        public ToolTip ContextPriceToolTip
        {
            get
            {

                return _ContextPriceToolTip;
            }
        }

        public decimal OverridePrice
        {
            get
            {
                object value = GetValue(OverridePriceProperty);
                if (value is decimal)
                    return (decimal)value;
                else
                    return default(decimal);
            }
            set
            {
                SetValue(OverridePriceProperty, value);
            }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty OverridePriceProperty =
                    DependencyProperty.Register(
                    "OverridePrice",
                    typeof(decimal),
                    typeof(ContextPriceTextBox),
                    new PropertyMetadata((decimal)-1, new PropertyChangedCallback(OverridePricePropertyChangedCallback)));

        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void OverridePricePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is ContextPriceTextBox)
                (d as ContextPriceTextBox).OverridePricePropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void OverridePricePropertyChanged()
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayedValue)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceFontWeight)));

            if (OverridePrice == -1)
            {
                _ContextPriceToolTip = null;
            }
            else
            {
                _ContextPriceToolTip = new ToolTip();
                _ContextPriceToolTip.Content = OrgPriceToolTip;
                _ContextPriceToolTip.FontWeight = FontWeights.Normal;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContextPriceToolTip)));
        }






        public FontWeight PriceFontWeight
        {
            get
            {
                if (OverridePrice == -1)
                    return FontWeights.Normal;
                else
                    return FontWeights.Bold;
            }
        }

        decimal? _DisplayedValue;
        public decimal DisplayedValue
        {
            get
            {
                if (_DisplayedValue == null)
                {
                    if (OverridePrice != -1)
                        _DisplayedValue = OverridePrice;
                    else
                        _DisplayedValue = Price;
                }

                return _DisplayedValue.Value;
            }
            set
            {
                _DisplayedValue = value;

            }

        }



        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextHorizontalAlignmentProperty); }
            set { SetValue(TextHorizontalAlignmentProperty, value); }
        }


        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
                    DependencyProperty.Register(
                    "TextAlignment",
                    typeof(TextAlignment),
                    typeof(ContextPriceTextBox),
                    new PropertyMetadata(TextAlignment.Left, new PropertyChangedCallback(TextHorizontalAlignmentPropertyChangedCallback)));

        public static void TextHorizontalAlignmentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContextPriceTextBox)
                (d as ContextPriceTextBox).TextHorizontalAlignmentPropertyChanged();

        }
        private void TextHorizontalAlignmentPropertyChanged()
        {
            PriceTextBox.TextAlignment = TextAlignment;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextAlignment)));
        }





        //public bool OverrideAllowed
        //{
        //    get { return (bool)GetValue(OverrideAllowedProperty); }
        //    set { SetValue(OverrideAllowedProperty, value); }
        //}


        //public static readonly DependencyProperty OverrideAllowedProperty =
        //            DependencyProperty.Register(
        //            "OverrideAllowed",
        //            typeof(bool),
        //            typeof(ContextPriceTextBox),
        //            new PropertyMetadata(false, new PropertyChangedCallback(TextHorizontalAlignmentPropertyChangedCallback)));

        //public static void OverrideAllowedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is ContextPriceTextBox)
        //        (d as ContextPriceTextBox).OverrideAllowedPropertyChanged();

        //}
        //private void OverrideAllowedPropertyChanged()
        //{

        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverrideAllowed)));
        //}


        public decimal Price
        {
            get { return (decimal)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty PriceProperty =
                    DependencyProperty.Register(
                    "Price",
                    typeof(decimal),
                    typeof(ContextPriceTextBox),
                    new PropertyMetadata(default(decimal), new PropertyChangedCallback(PricePropertyChangedCallback)));

        public event PropertyChangedEventHandler PropertyChanged;


        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void PricePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is ContextPriceTextBox)
                (d as ContextPriceTextBox).PricePropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void PricePropertyChanged()
        {
            _DisplayedValue = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayedValue)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceFontWeight)));
        }

        private void RemovePriceBtn_Click(object sender, RoutedEventArgs e)
        {


            OverridePrice = -1;
            _DisplayedValue = null;
            (this).GetBindingExpression(OverridePriceProperty).UpdateSource();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayedValue)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceFontWeight)));

        }





        public string OrgPriceToolTip
        {
            get
            {
                object value = GetValue(OrgPriceToolTipProperty);
                if (value is string)
                    return (string)value;
                else
                    return default(string);
            }
            set
            {
                SetValue(OrgPriceToolTipProperty, value);
            }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty OrgPriceToolTipProperty =
                    DependencyProperty.Register(
                    "OrgPriceToolTip",
                    typeof(string),
                    typeof(ContextPriceTextBox),
                    new PropertyMetadata("", new PropertyChangedCallback(OrgPriceToolTipPropertyChangedCallback)));

        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void OrgPriceToolTipPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is ContextPriceTextBox)
                (d as ContextPriceTextBox).OrgPriceToolTipPropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void OrgPriceToolTipPropertyChanged()
        {

            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayedValue)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PriceFontWeight)));

            //if (OverridePrice == -1)
            //{
            //    _ToolTip = null;
            //}
            //else
            //{
            //    _ToolTip = new ToolTip();
            //    _ToolTip.Content = "Org Price " + Price.ToString("C");
            //}

        }

        internal void UpdateProperty()
        {
            if (_DisplayedValue != null)
            {
                if (OverridePrice == -1)
                {
                    if (Price != _DisplayedValue.Value)
                    {
                        Price = _DisplayedValue.Value;
                       GetBindingExpression(PriceProperty)?.UpdateSource();
                    }
                }
                else
                {
                    OverridePrice = _DisplayedValue.Value;
                    GetBindingExpression(OverridePriceProperty)?.UpdateSource();
                }
                
                



            }
        }

        private void PriceTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateProperty();
            //if (_DisplayedValue != null)
            //{
            //    if (OverridePrice == -1)
            //        Price = _DisplayedValue.Value;
            //    else
            //        OverridePrice = _DisplayedValue.Value;
            //}

        }
    }
}
