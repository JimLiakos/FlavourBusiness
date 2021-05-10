using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MenuPresentationModel.MenuCanvas;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{e40cf3fe-738a-4065-ad7b-e4c3cdf4bac2}</MetaDataID>
    public class MenuCanvasLinePresentation: MarshalByRefObject, INotifyPropertyChanged, ICanvasItem
    {

        IMenuCanvasLine _MenuCanvasLine;
        public IMenuCanvasLine MenuCanvasLine
        {
            get
            {
                return _MenuCanvasLine;
            }
        }
        public MenuCanvasLinePresentation(MenuPresentationModel.MenuCanvas.IMenuCanvasLine menuCanvasLine)
        {
            _MenuCanvasLine = menuCanvasLine;
            _Visibility = Visibility.Visible;

            _Stroke = default(Color);

            if (_MenuCanvasLine.Stroke != null)
                _Stroke = (Color)ColorConverter.ConvertFromString(_MenuCanvasLine.Stroke);

            if (this.MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Double)
                _StrokeBrush = DoubleLineBrush;
            else
                _StrokeBrush = new SolidColorBrush(_Stroke);


        }


        public double StrokeThickness
        {
            get
            {
                return _MenuCanvasLine.StrokeThickness;
            }
        }


        public double Height
        {
            get
            {
                double height = _MenuCanvasLine.Y1 - _MenuCanvasLine.Y2; 
                if (height < 0)
                    height = -height;
                if (height < _MenuCanvasLine.StrokeThickness)
                    height = _MenuCanvasLine.StrokeThickness;
                return height;
            }
        }

        public double Left
        {
            get
            {
                return _MenuCanvasLine.X1;
            }
        }
        public double Top
        {
            get
            {
                return _MenuCanvasLine.Y1;
            }
        }

        public double X1
        {
            get
            {
                return  _MenuCanvasLine.X1-Left;
            }
        }

        public double X2
        {
            get
            {
                return  _MenuCanvasLine.X2- Left;
            }
        }
        public double Y1
        {
            get
            {
                return _MenuCanvasLine.Y1-Top;
            }
        }
        public double Y2
        {
            get
            {
                return _MenuCanvasLine.Y2-Top;
            }
        }
        public DoubleCollection LineDashes
        {
            get
            {
                if (this.MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Single)
                    return DashStyles.Solid.Dashes;

                if (this.MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Dotted)
                    return DashStyles.Dot.Dashes;
                if (this.MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Dashed)
                    return new DoubleCollection(new List<double> { 4, 3 });// DashStyles.Dash.Dashes;

                return DashStyles.Solid.Dashes;
            }
        }
        public PenLineCap StrokeDashCap
        {
            get
            {
                if (this.MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Dotted)
                    return PenLineCap.Round;


                return PenLineCap.Square;
            }
        }

        /// <exclude>Excluded</exclude>
        Color _Stroke;
        /// <exclude>Excluded</exclude>
        Brush _StrokeBrush;
        public Brush Stroke
        {
            get
            {
                return _StrokeBrush;
            }
        }


        /// <exclude>Excluded</exclude>
        Visibility _Visibility;
        public Visibility Visibility
        {
            get
            {
                return _Visibility;
            }

            set
            {
                _Visibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
            }
        }

        public double Width
        {
            get
            {

                double width = _MenuCanvasLine.X1 - _MenuCanvasLine.X2;
                if (width < 0)
                    width = -width;
                if (width < _MenuCanvasLine.StrokeThickness)
                    width = _MenuCanvasLine.StrokeThickness;
                return width;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Release()
        {
            
        }

        Brush DoubleLineBrush
        {
            get
            {
                var gradientStops = new GradientStopCollection() { new GradientStop(_Stroke, 0.3), new GradientStop(Colors.Transparent, 0.3), new GradientStop(Colors.Transparent, 0.7), new GradientStop(_Stroke, 0.7) };
                return new LinearGradientBrush(gradientStops, new Point(0, 0), new Point(1, 0));
            }
        }
        public Shapes.ArrowEnds ArrowEnds
        {
            get
            {
                if (MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Arrowend || MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.ArrowendEx)
                    return Shapes.ArrowEnds.Both;
                else
                    return Shapes.ArrowEnds.None;

            }
        }
        internal void Refresh()
        {
            _Stroke = default(Color);
            if (_MenuCanvasLine.Stroke != null)
                _Stroke = (Color)ColorConverter.ConvertFromString(_MenuCanvasLine.Stroke);
    
            if (this.MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Double)
                _StrokeBrush = DoubleLineBrush;
            else
                _StrokeBrush = new SolidColorBrush(_Stroke);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X1)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X2)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y1)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y2)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stroke)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineDashes)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeDashCap)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArrowEnds)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));

            //throw new NotImplementedException();
        }

        internal void ChangeCanvasLine(IMenuCanvasLine separationLine)
        {
            _MenuCanvasLine = separationLine;

            _Stroke = default(Color);

            Refresh();
            //if (_MenuCanvasLine.Stroke != null)
            //    _Stroke = (Color)ColorConverter.ConvertFromString(_MenuCanvasLine.Stroke);
            //if (this.MenuCanvasLine.LineType == MenuPresentationModel.MenuStyles.LineType.Double)
            //    _StrokeBrush = DoubleLineBrush;
            //else
            //    _StrokeBrush = new SolidColorBrush(Colors.Red);


            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X1)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X2)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y1)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y2)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stroke)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineDashes)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeDashCap)));
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArrowEnds)));


        }
    }

    /// <MetaDataID>{ebdcfcd6-5127-4502-aefe-768c020f929a}</MetaDataID>
    public class TranslationLinePresentation : MenuCanvasLinePresentation
    {
        public TranslationLinePresentation(IMenuCanvasLine menuCanvasLine) : base(menuCanvasLine)
        {

        }
    }
}
