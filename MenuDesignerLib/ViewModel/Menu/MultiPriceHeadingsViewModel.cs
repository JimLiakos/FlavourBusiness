using MenuPresentationModel.MenuCanvas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{f607d41f-9f23-4636-acfa-f5caae0e6442}</MetaDataID>
    public class MultiPriceHeadingsViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        IItemMultiPriceHeading ItemMultiPriceHeading;
        BookViewModel BookViewModel;
        public MultiPriceHeadingsViewModel(BookViewModel bookViewModel,
                                         IItemMultiPriceHeading itemMultiPriceHeading)
        {
            ItemMultiPriceHeading = itemMultiPriceHeading;
            BookViewModel = bookViewModel;


            double AngleDegree = ItemMultiPriceHeading.PriceHeadingsAngle;
            if (AngleDegree > 180)
            {
                double pos = 360 - AngleDegree;
                pos = (pos * 180.0) / 180.0;
                _AngePos = 180.0 + pos;
            }
            else
            {
                double pos = AngleDegree;
                pos = (pos * 180.0) / 180.0;
                _AngePos = 180.0 - pos;
            }
            _PriceHeadingHorizontalPos = ItemMultiPriceHeading.PriceHeadingsHorizontalPos;

            _MultiPriceHeadingsBottomMargin = PixelToMM(ItemMultiPriceHeading.PriceHeadingsBottomMargin);
            _MultiPriceHeadingsTopMargin = ItemMultiPriceHeading.PriceHeadingsTopMargin;
            //if (string.IsNullOrWhiteSpace(_SelectedTransformOrigin))
            //    _SelectedTransformOrigin = "/MenuDesigner;component/Resources/Images/Metro/RotateBottomLeft.png";



            UseStyleDefaultsCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
              {
                  ItemMultiPriceHeading.ResetValuesToStyleDefaults();
              });

        }

        //Dictionary<string, TransformOrigin> _TransformOriginImages = new Dictionary<string, TransformOrigin> { { "pack://application:,,,/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomLeft.png",new TransformOrigin("left","bottom" ) },
        //                                                            { "/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomRight.png" ,new TransformOrigin("right","bottom" )},
        //                                                            {"/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomCenter.png" ,new TransformOrigin("left","bottom" )},
        //                                                            {"/MenuDesignerLib;component/Resources/Images/Metro/RotateTopLeft.png" ,new TransformOrigin("left","Top" )},
        //                                                            {"/MenuDesignerLib;component/Resources/Images/Metro/RotateTopRight.png",new TransformOrigin("right","Top" ) },
        //                                                            {"/MenuDesignerLib;component/Resources/Images/Metro/RotateTopCenter.png" ,new TransformOrigin("center","Top" )},
        //                                                            { "/MenuDesignerLib;component/Resources/Images/Metro/RotateCenterLeft.png" ,new TransformOrigin("left","center" )},
        //                                                            {"/MenuDesignerLib;component/Resources/Images/Metro/RotateCenterRight.png",new TransformOrigin("right","center" )},
        //                                                            { "/MenuDesignerLib;component/Resources/Images/Metro/RotateCenter.png" ,new TransformOrigin("center","center" )} };
        //public List<string> TransformOriginImages
        //{
        //    get
        //    {

        //        return _TransformOriginImages.Keys.ToList();
        //    }
        //}

        //string _SelectedTransformOrigin = "/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomLeft.png";
        //public string SelectedTransformOrigin
        //{
        //    get
        //    {
        //        return _SelectedTransformOrigin;
        //    }
        //    set
        //    {
        //        _SelectedTransformOrigin = value;
        //        ItemMultiPriceHeading.TransformOrigin = _TransformOriginImages[_SelectedTransformOrigin];
        //    }
        //}

        public WPFUIElementObjectBind.RelayCommand UseStyleDefaultsCommand { get; protected set; }


        public string AngleDescription
        {
            get
            {
                if (AngePos < 180)
                    return ((int)(((180.0 - AngePos) / 180.0) * 180)).ToString() + "°";
                else
                    return ((int)(((AngePos - 180.0) / 180.0) * 180)).ToString() + "°";

            }
        }

        double _PriceHeadingHorizontalPos;
        public double PriceHeadingHorizontalPos
        {
            get
            {
                return _PriceHeadingHorizontalPos;
            }
            set
            {
                _PriceHeadingHorizontalPos = value;
                ItemMultiPriceHeading.PriceHeadingsHorizontalPos= _PriceHeadingHorizontalPos;
            }
        }
        public string MarginUnit
        {
            get
            {
                return "mm";
            }
        }

        double _MultiPriceHeadingsTopMargin;
        public double MultiPriceHeadingsTopMargin
        {
            get
            {
                return _MultiPriceHeadingsTopMargin;
            }
            set
            {
                _MultiPriceHeadingsTopMargin = value;

                ItemMultiPriceHeading.PriceHeadingsTopMargin = value;
               
            }
        }

        double _MultiPriceHeadingsBottomMargin;
        public double MultiPriceHeadingsBottomMargin
        {
            get
            {
                return _MultiPriceHeadingsBottomMargin;
            }
            set
            {
                _MultiPriceHeadingsBottomMargin = value;
                ItemMultiPriceHeading.PriceHeadingsBottomMargin = mmToPixel(_MultiPriceHeadingsBottomMargin);
            }
        }
        double PixelToMM(double px)
        {
            var pixpermm = (double)new System.Windows.LengthConverter().ConvertFromString("1in") / 25.4;
            return px / pixpermm;

            //return (int)Math.Round(px / pixpermm, 1);
        }
        double mmToPixel(double mm)
        {
            var pixpermm = (double)new System.Windows.LengthConverter().ConvertFromString("1in") / 25.4;
            return mm * pixpermm;

            //return (int)Math.Round(mm * pixpermm, 1);
        }
        double _AngePos = 180;

        public double AngePos
        {
            get
            {
                return _AngePos;
            }
            set
            {

                _AngePos = value;

                if (_AngePos > 180)
                {
                    double val = 360 - _AngePos + 180;
                    ItemMultiPriceHeading.PriceHeadingsAngle = val;
                }
                else
                {
                    double val = 180 - _AngePos;
                    ItemMultiPriceHeading.PriceHeadingsAngle= val;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AngleDescription)));
            }
        }

    }
}
