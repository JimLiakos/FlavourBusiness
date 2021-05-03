using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MenuItemsEditor.Views
{
    /// <summary>
    /// Interaction logic for PreparationOptionView.xaml
    /// </summary>
    /// <MetaDataID>{967878df-3363-450b-a2f9-64b33834ebc6}</MetaDataID>
    public partial class PreparationOptionView : ContentControl
    {
        public PreparationOptionView()
        {
            InitializeComponent();

            
        }
        public PreparationOptionViewType ViewType
        {
            get { return (PreparationOptionViewType)GetValue(ViewTypeProperty); }
            set { SetValue(ViewTypeProperty, value); }
        }

        public static readonly DependencyProperty ViewTypeProperty =
                 DependencyProperty.Register(
                 "ViewType",
                 typeof(PreparationOptionViewType),
                 typeof(PreparationOptionView),
                 new PropertyMetadata(PreparationOptionViewType.Minimize, new PropertyChangedCallback(ViewTypePropertyPropertyChangedCallback)));

        public static void ViewTypePropertyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            //if (d is EditableTextBlock)
            //    (d as EditableTextBlock).TextPropertyChanged();
        }

    }
  
}
namespace MenuItemsEditor
{
    /// <MetaDataID>{1bbf74f4-41d0-438f-ae3c-7d335ad42ef2}</MetaDataID>
    public enum PreparationOptionViewType
    {
        Minimize, ScaledOption, OptionsGroup
    }
}
