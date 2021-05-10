using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MenuPresentationModel.MenuCanvas;
using OOAdvantech.Transactions;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using FLBManager.Misc;
using SvgAccentModifier;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{7c7194f1-e5e9-40e2-8d19-f260bed5e294}</MetaDataID>
    [Transactional]
    public class AccentImagePresentation : MarshalByRefObject, INotifyPropertyChanged, ICanvasItem
    {

        IMenuCanvasAccent _HeadingAccent;

        public IMenuCanvasAccent HeadingAccent
        {
            get
            {
                return _HeadingAccent;
            }
        }

        int _AccentIndex;

        public int AccentIndex
        {
            get
            {
                return _AccentIndex;
            }
        }
        public AccentImagePresentation(IMenuCanvasAccent headingAccent, int accentIndex)
        {

            _HeadingAccent = headingAccent;
            _AccentIndex = accentIndex;

            if (_HeadingAccent != null)
            {
                _Top = _HeadingAccent.GetAccentImageRect(_AccentIndex).Y;
                _Left = _HeadingAccent.GetAccentImageRect(_AccentIndex).X;
                _Height = _HeadingAccent.GetAccentImageRect(_AccentIndex).Height;
                _Width = _HeadingAccent.GetAccentImageRect(_AccentIndex).Width;
            }

            _AccentUri = _HeadingAccent.Accent.AccentImages[_AccentIndex].Image.Uri;
            _HeadingAccent.ObjectChangeState += HeadingAccent_ObjectChangeState;
        }
        public AccentImagePresentation()
        {
            Visibility = Visibility.Collapsed;
        }
        public void ChangeAccent(IMenuCanvasAccent headingAccent, int accentIndex)
        {
            if (_HeadingAccent != headingAccent || _AccentIndex != accentIndex)
            {


                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _HeadingAccent.ObjectChangeState -= HeadingAccent_ObjectChangeState;
                    _HeadingAccent = headingAccent;
                    _AccentIndex = accentIndex;
                    _HeadingAccent.ObjectChangeState += HeadingAccent_ObjectChangeState;

                    if (_HeadingAccent != null)
                    {
                        _Top = _HeadingAccent.GetAccentImageRect(_AccentIndex).Y;
                        _Left = _HeadingAccent.GetAccentImageRect(_AccentIndex).X;
                        _Height = _HeadingAccent.GetAccentImageRect(_AccentIndex).Height;
                        _Width = _HeadingAccent.GetAccentImageRect(_AccentIndex).Width;
                    }
                    stateTransition.Consistent = true;
                }


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentStream)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentUri)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentDrawing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
            }
            else
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _AccentIndex = accentIndex;
                    if (_AccentUri != _HeadingAccent.Accent.AccentImages[_AccentIndex].Image.Uri)
                    {
                        _AccentUri = _HeadingAccent.Accent.AccentImages[_AccentIndex].Image.Uri;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentStream)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentUri)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentDrawing)));
                    }
                    bool sizeChange = false;
                    if (_Top != _HeadingAccent.GetAccentImageRect(_AccentIndex).Y)
                    {
                        _Top = _HeadingAccent.GetAccentImageRect(_AccentIndex).Y;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));
                    }
                    if (_Left != _HeadingAccent.GetAccentImageRect(_AccentIndex).X)
                    {
                        _Left = _HeadingAccent.GetAccentImageRect(_AccentIndex).X;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
                    }
                    if (_Height != _HeadingAccent.GetAccentImageRect(_AccentIndex).Height)
                    {
                        _Height = _HeadingAccent.GetAccentImageRect(_AccentIndex).Height;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
                        sizeChange = true;
                    }
                    if (_Width != _HeadingAccent.GetAccentImageRect(_AccentIndex).Width)
                    {
                        _Width = _HeadingAccent.GetAccentImageRect(_AccentIndex).Width;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
                        sizeChange = true;
                    }
                    if (sizeChange)
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentStream)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentUri)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentDrawing)));

                    }
                    stateTransition.Consistent = true;
                }



            }
        }

        private void HeadingAccent_ObjectChangeState(object _object, string member)
        {
            if (Visibility == Visibility.Visible)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentStream)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentDrawing)));
            }
        }

        double _Height;
        public double Height
        {
            get
            {
                if (_HeadingAccent.Accent.UnderlineImage || _HeadingAccent.Accent.OverlineImage)
                {
                    return (Width / AccentDrawing.Bounds.Width) * _Height;
                }
                return _Height;
                //if (HeadingAccent == null)
                //    return 0;
                //return HeadingAccent.GetAccentImageRect(AccentIndex).Height;
            }
            set
            {
            }
        }


        double _Left;
        public double Left
        {
            get
            {
                return _Left;
                //if (HeadingAccent == null)
                //    return 0;
                //return HeadingAccent.GetAccentImageRect(AccentIndex).X;
            }
            set
            {

            }
        }

        double _Top;
        public double Top
        {
            get
            {
                return _Top;
            }

            set
            {
            }
        }

        double _Width;
        public double Width
        {
            get
            {
                if (Visibility != Visibility.Visible)
                    return 0;
                if (_HeadingAccent == null || !IsValid)
                    return 0;
                return _HeadingAccent.GetAccentImageRect(_AccentIndex).Width;
            }
            set
            {
            }
        }

        string _AccentUri;

        public string AccentUri
        {
            get
            {
                return _AccentUri;
            }
        }

        public struct ImageKey
        {

            public ImageKey(string accentImageUri, string accentColor, Size size)
            {
                AccentImageUri = accentImageUri;
                AccentColor = accentColor;
                AccentImageSize = size;
            }
            public string AccentImageUri;
            public string AccentColor;
            public Size AccentImageSize;

            public static bool operator ==(ImageKey left, ImageKey right)
            {
                return left.AccentColor == right.AccentColor && left.AccentImageUri == right.AccentImageUri && left.AccentImageSize == right.AccentImageSize;
            }
            public static bool operator !=(ImageKey left, ImageKey right)
            {
                return !(left == right);
            }
        }
        public static Dictionary<ImageKey, System.Windows.Media.DrawingGroup> AccentDrawings = new Dictionary<ImageKey, System.Windows.Media.DrawingGroup>();

        public System.Windows.Media.DrawingGroup AccentDrawing
        {
            get
            {
                if (!IsValid)
                    return null;

                System.Windows.Media.DrawingGroup drawing = null;
                Size imagesize = default(Size);
                if (_HeadingAccent.Accent.MultipleItemsAccent&&!(_HeadingAccent.Accent.OverlineImage|| _HeadingAccent.Accent.UnderlineImage))
                    imagesize = new Size(Width, Height);

                if (!AccentDrawings.TryGetValue(new ImageKey(_HeadingAccent.Accent.AccentImages[_AccentIndex].Image.Uri, _HeadingAccent.AccentColor, imagesize), out drawing))
                {

                    MemoryStream ms = new MemoryStream();

                    using (FileStream file = new FileStream(MenuPresentationModel.MenuStyles.Accent.ResourcesRootPath + _HeadingAccent.Accent.AccentImages[_AccentIndex].Image.Uri, FileMode.Open, System.IO.FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);
                        ms.Position = 0;
                        string color = _HeadingAccent.AccentColor;

                        
                        if ((color != "none" && color != null) || imagesize != default(Size))
                        {
                            SharpVectors.Dom.Svg.SvgDocument doc = new SharpVectors.Dom.Svg.SvgDocument(null);
                            doc.Load(ms);
                            ISvgModifier svgModifier = SvgModifier.GetModifier(doc.DocumentElement as SharpVectors.Dom.Svg.SvgSvgElement);
                              
                            if (imagesize != default(Size))
                                svgModifier.Size = imagesize;
                            if (color != "none" && color != null)
                                svgModifier.Color = color;
                            ms = new MemoryStream();
                            doc.Save(ms);
                            ms.Position = 0;
                        }


                        //if (!string.IsNullOrWhiteSpace(color))
                        //    ms = SetColor(ms, color);

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

                       

                        }


                    }
                    AccentDrawings[new ImageKey(_HeadingAccent.Accent.AccentImages[_AccentIndex].Image.Uri, _HeadingAccent.AccentColor, imagesize)] = drawing;
                }
                return drawing;
            }
        }
        public System.IO.Stream AccentStream
        {
            get
            {
                if (!IsValid)
                    return null;

                MemoryStream ms = new MemoryStream();

                using (FileStream file = new FileStream(MenuPresentationModel.MenuStyles.Accent.ResourcesRootPath + _HeadingAccent.Accent.AccentImages[_AccentIndex].Image.Uri, FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];



                    file.Read(bytes, 0, (int)file.Length);
                    ms.Write(bytes, 0, (int)file.Length);
                    ms.Position = 0;
                    string color = _HeadingAccent.AccentColor;
                    if (!string.IsNullOrWhiteSpace(color))
                        ms = SetColor(ms, color);

                    return ms;
                }
            }
        }

        Visibility _Visibility;
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
                    _Visibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visibility)));
                }
            }
        }

        public bool IsValid
        {
            get
            {
                if (_HeadingAccent != null && _HeadingAccent.Accent != null && _HeadingAccent.Accent.AccentImages.Count > _AccentIndex)
                    return true;
                else
                    return false;
            }
        }

        private static MemoryStream SetColor(MemoryStream ms, string newColor)
        {
            if (newColor == "none")
                return ms;


            SharpVectors.Dom.Svg.SvgDocument doc = new SharpVectors.Dom.Svg.SvgDocument(null);

            doc.Load(ms);
            ms.Close();


            for (ulong i = 0; i != doc.StyleSheets.Length; i++)
            {
                var staleSheet = doc.StyleSheets[i];
                for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                {
                    var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                    if (cssStyleRule.SelectorText == ".st0")
                    {
                        if (cssStyleRule.Style.GetPropertyCssValue("stroke") != null)
                            cssStyleRule.Style.SetProperty("stroke", newColor, cssStyleRule.Style.GetPropertyPriority("stroke"));
                        else
                            cssStyleRule.Style.SetProperty("fill", newColor, cssStyleRule.Style.GetPropertyPriority("fill"));
                    }
                }
                string newCssText = "\n\n";
                for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                {
                    var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                    newCssText += "\n" + cssStyleRule.CssText;
                }
                newCssText += "\n\n";
                staleSheet.OwnerNode.InnerText = newCssText;
            }

            ms = new MemoryStream();
            doc.Save(ms);
            ms.Position = 0;
            return ms;
        }

        public void Release()
        {
            _HeadingAccent.ObjectChangeState -= HeadingAccent_ObjectChangeState;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
