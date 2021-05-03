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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RestaurantHallLayoutModel;

namespace FloorLayoutDesigner
{
    /// <summary>
    /// Interaction logic for FloorLayoutDisigner.xaml
    /// </summary>
    /// <MetaDataID>{35c22103-a843-446a-90bc-55daba6daa0c}</MetaDataID>
    public partial class HallLayoutDesigner : UserControl
    {
        public HallLayoutDesigner()
        {
            InitializeComponent();

            ZoomViewBox.SizeChanged += ZoomViewBox_SizeChanged;
            ZoomViewBox.DataContextChanged += ZoomViewBox_DataContextChanged;


        }
        public DesignerCanvas Canvas = new DesignerCanvas();
        private void ZoomViewBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {

                try
                {

                    if (Application.Current != null && Application.Current?.MainWindow != null)
                        Application.Current.MainWindow.Cursor = Cursors.Wait;

                    Canvas = new DesignerCanvas
                    {
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Background = new SolidColorBrush(Colors.WhiteSmoke),
                        Name = "FloorCanvas"


                    };
                    if (HallLayoutViewPresentation != null)
                        HallLayoutViewPresentation.PropertyChanged -= HallLayout_PropertyChanged;

                    HallLayoutViewPresentation = ZoomViewBox.GetDataContextObject<ViewModel.HallLayoutViewModel>();
                    if (HallLayoutViewPresentation != null)
                    {

                        Canvas.Load(HallLayoutViewPresentation);
                        if (HallLayoutViewPresentation != null)
                        {
                            HallLayoutViewPresentation.PropertyChanged += HallLayout_PropertyChanged;
                            if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                            {
                                Zoom = (ZoomViewBox.ActualHeight / (ZoomViewBox.Child as FrameworkElement).ActualHeight) * 100;
                                HallLayoutViewPresentation.ZoomPercentage = Zoom.Value;
                            }
                            ZoomViewBox.Visibility = Visibility.Visible;
                        }
                        else
                            ZoomViewBox.Visibility = Visibility.Hidden;
                        DesignerCanvasHost.Child = Canvas;
                    }
                }
                finally
                {
                    if (Application.Current != null && Application.Current?.MainWindow != null)
                        Application.Current.MainWindow.Cursor = Cursors.Arrow;
                }
            }
        }

        private void HallLayoutViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(ViewModel.HallLayoutViewModel.ZoomPercentage))
            {
                ViewModel.HallLayoutViewModel hallLayoutViewModel = ZoomViewBox.GetDataContextObject<ViewModel.HallLayoutViewModel>();
                if (hallLayoutViewModel != null)
                {
                    if (Zoom.HasValue && Zoom.Value != hallLayoutViewModel.ZoomPercentage)
                    {
                        Zoom = hallLayoutViewModel.ZoomPercentage;
                        ZoomViewBox.Height = (Zoom.Value / 100) * (ZoomViewBox.Child as FrameworkElement).ActualHeight;
                    }
                }
            }
        }

        double? Zoom;
        private void ZoomViewBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            ViewModel.HallLayoutViewModel hallLayoutViewModel = DataContext as ViewModel.HallLayoutViewModel;
            if (hallLayoutViewModel != null)
            {
                if ((ZoomViewBox.Child as FrameworkElement).ActualHeight > 0)
                {
                    Zoom = (ZoomViewBox.ActualHeight / (ZoomViewBox.Child as FrameworkElement).ActualHeight) * 100;
                    hallLayoutViewModel.ZoomPercentage = Zoom.Value;
                }
            }
        }


        ViewModel.HallLayoutViewModel HallLayoutViewPresentation;

        private void HallLayout_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.HallLayoutViewModel.ZoomPercentage))
            {

                if (HallLayoutViewPresentation != null)
                {
                    if (Zoom.HasValue && Zoom.Value != HallLayoutViewPresentation.ZoomPercentage)
                    {
                        Zoom = HallLayoutViewPresentation.ZoomPercentage;
                        ZoomViewBox.Height = (Zoom.Value / 100) * (ZoomViewBox.Child as FrameworkElement).ActualHeight;
                    }
                }
            }
        }

        private void ViewBoxHost_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ZoomViewBox.ActualHeight + (e.Delta / 2) > 0)
                ZoomViewBox.Height = ZoomViewBox.ActualHeight + (e.Delta / 2);
        }




    }
}
