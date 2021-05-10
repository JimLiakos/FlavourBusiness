using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{b6c9f092-b1ae-470c-bbef-2472c44f6aa0}</MetaDataID>
    public class PriceOptionsViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public PriceOptionsViewModel()
        {

        }
        PriceStyle PriceStyle;
        BookViewModel BookViewModel;
        public PriceOptionsViewModel(BookViewModel bookViewModel,
                                         //LayoutStyle layoutStyle,
                                         //PageStyle pageStyle,
                                         //MenuItemStyle menuItemStyle,
                                         PriceStyle priceStyle)
        {
            PriceStyle = priceStyle;
            BookViewModel = bookViewModel;
            _PriceLocationOption = (from priceLayout in _PriceLocationOptions
                                    where PriceStyle.Layout == priceLayout.Value
                                    select priceLayout.Key).FirstOrDefault();

            _SelectedTransformOrigin = (from imagePair in _TransformOriginImages
                                        where imagePair.Value == PriceStyle.PriceHeadingTransformOrigin
                                        select imagePair.Key).FirstOrDefault();

            double AngleDegree = PriceStyle.PriceHeadingAngle;
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
            _PriceHeadingHorizontalPos = PriceStyle.PriceHeadingHorizontalPos;


            _MultiPriceHeadingsBottomMargin = Math.Round(PixelToMM(PriceStyle.PriceHeadingsBottomMargin), 2);
            _MultiPriceSpacing = Math.Round(PixelToMM(PriceStyle.MultiPriceSpacing),2);
            if (string.IsNullOrWhiteSpace(_SelectedTransformOrigin))
                _SelectedTransformOrigin = "/MenuDesigner;component/Resources/Images/Metro/RotateBottomLeft.png";
        }

        static Dictionary<string, PriceLayout> _PriceLocationOptions = new Dictionary<string, PriceLayout>() { { Properties.Resources.PriceLayoutNormalPrompt, PriceLayout.Normal },
                                                                                                                { Properties.Resources.PriceLayoutWithNamePrompt,PriceLayout.WithName },
                                                                                                                { Properties.Resources.PriceLayoutWithDescriptionPrompt,PriceLayout.WithDescription },
                                                                                                                { Properties.Resources.PriceLayoutFollowDescriptionPrompt,PriceLayout.FollowDescription },
                                                                                                                { Properties.Resources.PriceLayoutDoNotDisplayPrompt,PriceLayout.DoNotDisplay } };


        Dictionary<string, TransformOrigin> _TransformOriginImages = new Dictionary<string, TransformOrigin> { { "/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomLeft.png",new TransformOrigin("left","bottom" ) },
                                                                    { "/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomRight.png" ,new TransformOrigin("right","bottom" )},
                                                                    {"/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomCenter.png" ,new TransformOrigin("left","bottom" )},
                                                                    {"/MenuDesignerLib;component/Resources/Images/Metro/RotateTopLeft.png" ,new TransformOrigin("left","Top" )},
                                                                    {"/MenuDesignerLib;component/Resources/Images/Metro/RotateTopRight.png",new TransformOrigin("right","Top" ) },
                                                                    {"/MenuDesignerLib;component/Resources/Images/Metro/RotateTopCenter.png" ,new TransformOrigin("center","Top" )},
                                                                    { "/MenuDesignerLib;component/Resources/Images/Metro/RotateCenterLeft.png" ,new TransformOrigin("left","center" )},
                                                                    {"/MenuDesignerLib;component/Resources/Images/Metro/RotateCenterRight.png",new TransformOrigin("right","center" )},
                                                                    { "/MenuDesignerLib;component/Resources/Images/Metro/RotateCenter.png" ,new TransformOrigin("center","center" )} };
        public List<string> TransformOriginImages
        {
            get
            {

                return _TransformOriginImages.Keys.ToList();
            }
        }

        string _SelectedTransformOrigin = "/MenuDesignerLib;component/Resources/Images/Metro/RotateBottomLeft.png";
        public string SelectedTransformOrigin
        {
            get
            {
                return _SelectedTransformOrigin;
            }
            set
            {
                _SelectedTransformOrigin = value;
                PriceStyle.PriceHeadingTransformOrigin = _TransformOriginImages[_SelectedTransformOrigin];
            }
        }

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
                PriceStyle.PriceHeadingHorizontalPos = _PriceHeadingHorizontalPos;
            }
        }
        public string MarginUnit
        {
            get
            {
                return "mm";
            }
        }

        double _MultiPriceSpacing;

        public double MultiPriceSpacing
        {
            get
            {
                return _MultiPriceSpacing;
            }
            set
            {
                _MultiPriceSpacing = value;
                PriceStyle.MultiPriceSpacing = mmToPixel(_MultiPriceSpacing);
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
                PriceStyle.PriceHeadingsBottomMargin = mmToPixel(_MultiPriceHeadingsBottomMargin);
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
                    PriceStyle.PriceHeadingAngle = val;
                }
                else
                {
                    double val = 180 - _AngePos;
                    PriceStyle.PriceHeadingAngle = val;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AngleDescription)));
            }
        }


        static List<string> _PriceLeaderOptions = new List<string>() { "Dots", "–", "—", "•", ":", "/", "|", "…", "none", };
        public IList<string> PriceLeaderOptions
        {
            get
            {
                return _PriceLeaderOptions.AsReadOnly();
            }
        }



        /// <exclude>Excluded</exclude>
        static List<string> _PriceLeaderExtraSpaceOptions = new List<string>() { "none","1 dot", "2 dots" };
        public IList<string> PriceLeaderExtraSpaceOptions
        {
            get
            {
                return _PriceLeaderExtraSpaceOptions.AsReadOnly();
            }
        }

        ///// <exclude>Excluded</exclude>
        //string _PriceLeaderExtraSpaceOption;
        public string PriceLeaderExtraSpaceOption
        {
            get
            {
                return _PriceLeaderExtraSpaceOptions[PriceStyle.BetweenDotsSpace];
            }
            set
            {
                PriceStyle.BetweenDotsSpace = _PriceLeaderExtraSpaceOptions.IndexOf(value);
            }
        }

        
        public bool PriceLeaderExtraSpaceNearItem
        {
            get
            {
                return PriceStyle.DotsSpaceFromItem == 2;
            }
            set
            {
                if(value)
                    PriceStyle.DotsSpaceFromItem = 2;
                else
                    PriceStyle.DotsSpaceFromItem = 1;
            }
        }


        public bool PriceLeaderExtraSpaceNearPrice
        {
            get
            {
                return PriceStyle.DotsSpaceFromPrice == 2;
            }
            set
            {
                if (value)
                    PriceStyle.DotsSpaceFromPrice = 2;
                else
                    PriceStyle.DotsSpaceFromPrice = 1;
            }
        }


        public bool PriceLeaderMatchNameColor
        {
            get
            {
                return PriceStyle.DotsMatchNameColor;
            }
            set
            {
                PriceStyle.DotsMatchNameColor = value;
            }
        }

        
            
        public bool ShowMultiplePrices
        {
            get
            {
                return PriceStyle.ShowMultiplePrices;
            }
            set
            {
        
                if(value)
                {
                    PriceLocationOption = Properties.Resources.PriceLayoutNormalPrompt;
                    PriceStyle.ShowMultiplePrices = value;
                    PropertyChanged?.Invoke(this,new PropertyChangedEventArgs( nameof(PriceLocationOption)));
                }
                else
                    PriceStyle.ShowMultiplePrices = value;
            }
        }
        public string SelectedPriceLeaderOption
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PriceStyle.PriceLeader))
                    return "none";

                if (PriceStyle.PriceLeader== "Dots".ToLower())
                    return "Dots";

                return PriceStyle.PriceLeader;
            }
            set
            {
                if (value == "none")
                    PriceStyle.PriceLeader = "";
                else
                    PriceStyle.PriceLeader = value;
            }
        }
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
    }
}
