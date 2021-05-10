using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FLBManager.ViewModel;
using MenuPresentationModel;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.Transactions;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{66b4d6f1-3f62-4582-bf36-ea366dd044e9}</MetaDataID>
    public class MenuHeadingViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public readonly IMenuCanvasHeading MenuCanvasHeading;
        public readonly RestaurantMenu RestaurantMenu;

        public MenuHeadingViewModel()
        {

        }

        public MenuHeadingViewModel(IMenuCanvasHeading menuCanvasHeading, RestaurantMenu restaurantMenu)
        {
            this.MenuCanvasHeading = menuCanvasHeading;
            this.RestaurantMenu = restaurantMenu;

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(DownloadStylesWindow.HeadingAccentStorage);
            var accents = (from accent in storage.GetObjectCollection<Accent>() select accent).ToList();


            //AccentImage
            _AccentImages = (from accent in accents select new AccentViewModel(accent)).ToList();

            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            //{
            //    foreach (var accentImage in MenuPresentationModel.MenuStyles.AccentImage.AvailableAccents)
            //    {
            //        var headingAccent = (from accent in storage.GetObjectCollection<MenuPresentationModel.MenuStyles.HeadingAccent>()
            //                             where accent.Name == accentImage.Image.Name
            //                             select accent).FirstOrDefault();
            //        if (headingAccent != null)
            //            headingAccent.SelectionAccentImageUri = accentImage.Uri;
            //        else
            //        {

            //        }

            //    }
            //    stateTransition.Consistent = true;
            //}

            _AccentImages.Insert(0, new AccentViewModel(AccentViewModel.AccentViewModelType.StyleSheet));
            _AccentImages.Insert(0, new AccentViewModel(AccentViewModel.AccentViewModelType.None));
            _SelectedAccent = _AccentImages[1];
            if ((MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent != null)
            {
                foreach (var accentViewModel in _AccentImages)
                {
                    if ((MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent.IsTheSameWith(accentViewModel.Accent))
                    {
                        _SelectedAccent = accentViewModel;

                        if (!string.IsNullOrWhiteSpace((MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent.AccentColor))
                        {
                            _HeadingsAccentSelectedColor = (Color)ColorConverter.ConvertFromString((MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent.AccentColor);
                            _HeadingsAccentColorize = true;
                        }

                    }
                }

            }
        }

        public bool CustomSpacing
        {
            get
            {
                return MenuCanvasHeading.CustomSpacing;
            }
            set
            {
                MenuCanvasHeading.CustomSpacing = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BeforeSpacing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AfterSpacing)));
            }
        }
        public bool StyleSpacing
        {
            get
            {
                return !MenuCanvasHeading.CustomSpacing;
            }
        }
        public string SpacingUnit
        {
            get
            {
                return "mm";
            }
        }
        public double BeforeSpacing
        {
            get
            {
                return Math.Round(LayoutOptionsPresentation.PixelToMM(MenuCanvasHeading.BeforeSpacing), 2);

            }
            set
            {
                MenuCanvasHeading.BeforeSpacing = Math.Round(LayoutOptionsPresentation.mmToPixel(value), 2);
            }
        }
        public double AfterSpacing
        {
            get
            {
                return Math.Round(LayoutOptionsPresentation.PixelToMM(MenuCanvasHeading.AfterSpacing), 2);
            }
            set
            {
                MenuCanvasHeading.AfterSpacing = Math.Round(LayoutOptionsPresentation.mmToPixel(value), 2);
            }
        }

        public HeadingType HeadingType
        {
            get
            {
                return MenuCanvasHeading.HeadingType;
            }
            set
            {
                MenuCanvasHeading.HeadingType = value;
            }
        }

        public AlignmentViewModel Alignment
        {
            get
            {
                if (!MenuCanvasHeading.IsStyleAlignmentOverridden)
                    return _HeadingAlignments.Where(x => x.Alignment == null).FirstOrDefault();
                else
                    return _HeadingAlignments.Where(x => x.Alignment == MenuCanvasHeading.Alignment).FirstOrDefault();
            }
            set
            {
                if (value.Alignment == null)
                    MenuCanvasHeading.ClearAlignment();
                else
                    MenuCanvasHeading.Alignment = value.Alignment.Value;
            }
        }

        List<AlignmentViewModel> _HeadingAlignments = new List<AlignmentViewModel>() { new AlignmentViewModel(null), new AlignmentViewModel(MenuPresentationModel.MenuStyles.Alignment.Left), new AlignmentViewModel(MenuPresentationModel.MenuStyles.Alignment.Center), new AlignmentViewModel(MenuPresentationModel.MenuStyles.Alignment.Right) };
        public List<AlignmentViewModel> HeadingAlignments
        {
            get
            {
                return _HeadingAlignments;
            }
        }

        List<HeadingType> _HeadingTypes = null;
        public List<HeadingType> HeadingTypes
        {
            get
            {
                if (_HeadingTypes == null)
                {
                    _HeadingTypes = new List<HeadingType>();
                    _HeadingTypes.Add(HeadingType.Normal);
                    _HeadingTypes.Add(HeadingType.Title);
                    _HeadingTypes.Add(HeadingType.AltFont);
                    _HeadingTypes.Add(HeadingType.SubHeading);
                }
                return _HeadingTypes;
            }
        }


        AccentViewModel _SelectedAccent;
        public AccentViewModel SelectedAccent
        {
            get
            {
                return _SelectedAccent;
            }
            set
            {
                _SelectedAccent = value;


                (MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent = value.Accent;
                if ((MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent != null)
                {
                    (MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent.AccentColor = new ColorConverter().ConvertToString(_HeadingsAccentSelectedColor);
                }
                else
                {
                    _HeadingsAccentSelectedColor = Colors.LightGray;
                    _HeadingsAccentColorize = false;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingsAccentColorize)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingsAccentSelectedColor)));

                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomAccent)));
            }
        }
        public string HeadingTitle
        {
            get
            {
                return MenuCanvasHeading.Description;
            }
            set
            {
                MenuCanvasHeading.Description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslatedHeadingTitle)));

            }
        }
        public bool UnTranslatedHeadingTitle
        {
            get
            {
                string headingTitle = HeadingTitle;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return HeadingTitle==null;
                }
            }
        }

        WPFUIElementObjectBind.ITranslator _Translator;
        public WPFUIElementObjectBind.ITranslator Translator
        {
            get
            {
                if (_Translator == null)
                    _Translator = new MenuItemsEditor.Translator();

                return _Translator;
            }
        }


        List<AccentViewModel> _AccentImages;
        public List<AccentViewModel> AccentImages
        {
            get
            {
                return _AccentImages;
            }
        }

        public bool CustomAccent
        {
            get
            {
                return SelectedAccent.AccentType == AccentViewModel.AccentViewModelType.Image;
            }
        }

        bool _HeadingsAccentColorize;
        public bool HeadingsAccentColorize
        {
            get
            {
                return _HeadingsAccentColorize;
            }
            set
            {
                _HeadingsAccentColorize = value;
                if (!_HeadingsAccentColorize)
                {
                    _HeadingsAccentSelectedColor = Colors.LightGray;
                    SelectedAccent.Accent.AccentColor = null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingsAccentSelectedColor)));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Color _HeadingsAccentSelectedColor = Colors.LightGray;
        public Color HeadingsAccentSelectedColor
        {
            get
            {
                return _HeadingsAccentSelectedColor;
            }
            set
            {
                _HeadingsAccentSelectedColor = value;
                SelectedAccent.Accent.AccentColor = new ColorConverter().ConvertToString(_HeadingsAccentSelectedColor);

                (MenuCanvasHeading as FoodItemsHeading).CustomHeadingAccent.AccentColor = SelectedAccent.Accent.AccentColor;
            }
        }


        public Uri ImageUri
        {
            get
            {
                if (MenuCanvasHeading.Page != null)
                    return new Uri(@"pack://application:,,,/MenuDesigner;Component/Resources/Images/Metro/DocumentHeader.png");
                else
                    return new Uri(@"pack://application:,,,/MenuDesigner;Component/Resources/Images/Metro/DocumentHeaderAdd.png");
            }
        }


        public bool NextColumnOrPage
        {
            get
            {
                return MenuCanvasHeading.NextColumnOrPage;
            }
            set
            {
                MenuCanvasHeading.NextColumnOrPage = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public int SelectedNumOfColumns
        {
            get
            {
                return MenuCanvasHeading.NumberOfFoodColumns;
            }
            set
            {
                MenuCanvasHeading.NumberOfFoodColumns = value;
                // UpdateColumnsWidths();
            }
        }

    }

    /// <MetaDataID>{2d7d8b10-4233-470b-8b42-7e32f36f21dd}</MetaDataID>
    public class AlignmentViewModel : MarshalByRefObject
    {
        public readonly Alignment? Alignment;
        public AlignmentViewModel(Alignment? alignment)
        {
            Alignment = alignment;
        }
        public override string ToString()
        {
            if (Alignment == null)
                return Properties.Resources.StyleSheetValuePrompt;
            if (Alignment == MenuPresentationModel.MenuStyles.Alignment.Left)
                return Properties.Resources.AlignmentLeftPrompt;

            if (Alignment == MenuPresentationModel.MenuStyles.Alignment.Center)
                return Properties.Resources.AlignmentCenterPrompt;
            if (Alignment == MenuPresentationModel.MenuStyles.Alignment.Right)
                return Properties.Resources.AlignmentRightPrompt;
            return base.ToString();

        }
    }

    /// <MetaDataID>{60394389-e18b-4ab8-8f34-eaf5fb06df0b}</MetaDataID>
    public class AccentViewModel : MarshalByRefObject
    {
        public enum AccentViewModelType
        {
            None,
            StyleSheet,
            Image
        }

        public AccentViewModelType AccentType { get; }
        public AccentViewModel()
        {

        }


        public IAccent Accent { get; }
        public AccentViewModel(IAccent accent)
        {
            Accent = accent;
            if (accent.AccentImages.Count > 0)
                AccentType = AccentViewModelType.Image;
            else
                AccentType = AccentViewModelType.None;

        }



     




        public AccentViewModel(AccentViewModelType accentViewModelType)
        {
            if (accentViewModelType == AccentViewModelType.StyleSheet)
            {
                AccentType = AccentViewModelType.StyleSheet;
                _Description = Properties.Resources.StyleSheetValuePrompt;
            }
            else
            {
                _Description = "(None)";
                AccentType = AccentViewModelType.None;
            }
        }



        /// <exclude>Excluded</exclude>
        string _Description;
        public string Description
        {
            get
            {
                return _Description;
            }
        }

        public Visibility ImageVisibility
        {
            get
            {
                if (AccentType == AccentViewModelType.Image)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
        public Visibility TextVisibility
        {
            get
            {
                if (AccentType == AccentViewModelType.Image)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public string Uri
        {
            get
            {
                if (Accent != null)
                    return Accent.SelectionAccentImageUri;
                else
                    return "";
            }
        }

        static Dictionary<string, byte[]> SvgStreams = new Dictionary<string, byte[]>();
        static Dictionary<string, DrawingGroup> SvgDrawings = new Dictionary<string, DrawingGroup>();

        public Stream Stream
        {
            get
            {
                if (Accent == null || string.IsNullOrWhiteSpace(Accent.SelectionAccentImageUri))
                    return null;


                byte[] bytes = null;
                if (!SvgStreams.TryGetValue(Accent.SelectionAccentImageUri, out bytes))
                {
                    MemoryStream ms = new MemoryStream();
                    using (FileStream file = new FileStream(MenuPresentationModel.MenuStyles.Accent.ResourcesRootPath + Accent.SelectionAccentImageUri, FileMode.Open, System.IO.FileAccess.Read))
                    {
                        bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        SvgStreams[Accent.SelectionAccentImageUri] = bytes;
                        ms.Write(bytes, 0, (int)file.Length);
                        ms.Position = 0;
                        return ms;
                    }
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    ms.Write(bytes, 0, (int)bytes.Length);
                    ms.Position = 0;
                    return ms;
                }

            }
        }


        public DrawingGroup Drawing
        {
            get
            {
                if (Accent == null || string.IsNullOrWhiteSpace(Accent.SelectionAccentImageUri))
                    return null;


                DrawingGroup drawing = null;
                if (!SvgDrawings.TryGetValue(Accent.SelectionAccentImageUri, out drawing))
                {
                    MemoryStream ms = new MemoryStream();
                    using (FileStream file = new FileStream(MenuPresentationModel.MenuStyles.Accent.ResourcesRootPath + Accent.SelectionAccentImageUri, FileMode.Open, System.IO.FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        SvgStreams[Accent.SelectionAccentImageUri] = bytes;
                        ms.Write(bytes, 0, (int)file.Length);
                        ms.Position = 0;

                    }

                    bool _textAsGeometry = false;
                    bool _includeRuntime = true;
                    bool _optimizePath = true;

                    WpfDrawingSettings settings = new WpfDrawingSettings();
                    settings.IncludeRuntime = _includeRuntime;
                    settings.TextAsGeometry = _textAsGeometry;
                    settings.OptimizePath = _optimizePath;
                    ////  if (_culture != null)
                    //      settings.CultureInfo = _culture;

                    using (FileSvgReader reader =
                              new FileSvgReader(settings))
                    {
                        drawing = reader.Read(ms);

                        
                        //Size   orgSize = drawing.Bounds.Size;

                        //Accent.Height = orgSize.Height;
                        //Accent.Width = orgSize.Width;
                    }
                    SvgDrawings[Accent.SelectionAccentImageUri] = drawing;

                }
                return drawing;
            }
        }

    }
}


