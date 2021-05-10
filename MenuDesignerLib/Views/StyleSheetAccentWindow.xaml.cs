using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using MenuDesigner.ViewModel.MenuCanvas;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.Transactions;

namespace MenuDesigner.Views
{
    /// <summary>
    /// Interaction logic for StyleSheetAccentWindow.xaml
    /// </summary>
    /// <MetaDataID>{d34122ac-e970-4357-afb6-d6ff59ee1469}</MetaDataID>
    public partial class StyleSheetAccentsWindow : StyleableWindow.Window
    {
        /// <MetaDataID>{57809844-3abd-42a6-9889-f491279ed47f}</MetaDataID>
        public StyleSheetAccentsWindow()
        {
            InitializeComponent();

            ZoomViewBox.DataContextChanged += ZoomViewBox_DataContextChanged;

        }
        /// <MetaDataID>{6f03aedc-4b3e-47ae-bf31-2fdd084520c1}</MetaDataID>
        double? Zoom;
        /// <MetaDataID>{a19a77dc-12c1-4480-9102-be6c39481a55}</MetaDataID>
        private void ZoomViewBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StyleSheetAccentsAccentViewModel viewModel = ZoomViewBox.GetDataContextObject<StyleSheetAccentsAccentViewModel>();
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
            {
                Zoom = (ZoomViewBox.ActualHeight / (ZoomViewBox.Child as FrameworkElement).ActualHeight) * 100;
                viewModel.ZoomPercentage = Zoom.Value;
            }
        }

        /// <MetaDataID>{2d7a828c-5d24-4f5e-8980-a36c190a497e}</MetaDataID>
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StyleSheetAccentsAccentViewModel.ZoomPercentage))
            {
                StyleSheetAccentsAccentViewModel BookViewModel = ZoomViewBox.GetDataContextObject<StyleSheetAccentsAccentViewModel>();
                //if (!Zoom.HasValue)
                //{
                //    if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                //    {
                //        Zoom = (ZoomViewBox.ActualHeight / (ZoomViewBox.Child as FrameworkElement).ActualHeight) * 100;
                //        BookViewModel.ZoomPercentage = Zoom.Value;
                //    }
                //}

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


        /// <MetaDataID>{9ee7f88a-3d4f-4e9e-b738-a0f9a1a39914}</MetaDataID>
        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            StyleSheetAccentsAccentViewModel viewModel = ZoomViewBox.GetDataContextObject<StyleSheetAccentsAccentViewModel>();
            if (!Zoom.HasValue)
            {
                if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                {
                    if (ViewBoxHost.ViewportWidth != 0)
                    {
                        Zoom = ((ViewBoxHost.ViewportWidth - 20) / (ZoomViewBox.Child as FrameworkElement).ActualWidth) * 100;
                        ZoomViewBox.Height = (Zoom.Value / 100) * (ZoomViewBox.Child as FrameworkElement).ActualHeight;
                        viewModel.ZoomPercentage = Zoom.Value;
                    }
                }
            }
            else
            {
                if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                {
                    Zoom = ((ViewBoxHost.ViewportWidth - 20) / (ZoomViewBox.Child as FrameworkElement).ActualWidth) * 100;
                    ZoomViewBox.Height = (Zoom.Value / 100) * (ZoomViewBox.Child as FrameworkElement).ActualHeight;
                    viewModel.ZoomPercentage = Zoom.Value;
                }
            }
        }

        /// <MetaDataID>{99d100d1-d0f1-4327-bd2f-c6d26d8b7206}</MetaDataID>
        private void ViewBoxHost_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            StyleSheetAccentsAccentViewModel viewModel = ZoomViewBox.GetDataContextObject<StyleSheetAccentsAccentViewModel>();
            if (!Zoom.HasValue)
            {
                if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                {
                    if (ViewBoxHost.ViewportWidth != 0)
                    {
                        Zoom = ((ViewBoxHost.ViewportWidth - 20) / (ZoomViewBox.Child as FrameworkElement).ActualWidth) * 100;
                        ZoomViewBox.Height = (Zoom.Value / 100) * (ZoomViewBox.Child as FrameworkElement).ActualHeight;
                        viewModel.ZoomPercentage = Zoom.Value;
                    }
                }
            }


        }
    }
}


