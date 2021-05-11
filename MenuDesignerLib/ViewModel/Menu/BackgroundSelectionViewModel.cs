﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.PersistenceLayer;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{e2f74d9b-41de-4898-8d48-414c8bf9b8ef}</MetaDataID>
    public class BackgroundSelectionViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        private PageStyle PageStyle;

        public BackgroundSelectionViewModel(PageStyle pageStyle)
        {
            PageStyle = pageStyle;

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(BackgroundImagesStorage);
            _Backgrounds = (from pageImage in storage.GetObjectCollection<PageImage>()
                        select new PageBackgroundViewModel(pageImage, pageStyle) /*{PageImage=pageImage,PageStyle=pageStyle}*/).ToList();

            _Backgrounds.Insert(0, new PageBackgroundViewModel(null, pageStyle));

            if (pageStyle.Background == null)
                _SelectedBackground = _Backgrounds[0];
            else
            {
                _SelectedBackground = (from background in _Backgrounds
                                   where background.PageImage != null && background.PageImage.Name == pageStyle.Background.Name
                                   select background).FirstOrDefault();
            }

        }

        /// <exclude>Excluded</exclude>
        string _MarginUnit = "px";
        public string MarginUnit
        {
            get
            {
                return _MarginUnit;
            }
            set
            {
                _MarginUnit = value;
            }
        }

    

        public double Margin
        {
            get
            {
                return PageStyle.BackgroundMargin.MarginLeft;
            }
            set
            {
                PageStyle.BackgroundMargin = new UIBaseEx.Margin() { MarginLeft = value, MarginTop = value, MarginRight = value, MarginBottom = value };
            }
        }

        public bool StretchtoFit
        {
            get
            {
                if (PageStyle.BackgroundStretch == ImageStretch.Fill)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    PageStyle.BackgroundStretch = ImageStretch.Fill;
                else
                    PageStyle.BackgroundStretch = ImageStretch.Uniform;
            }
        }
        public Color SelectedBackgroundColor
        {
            get
            {
                if (_SelectedBackground != null)
                    return _SelectedBackground.BackgroundColor;
                else
                    return default(Color);
            }
            set
            {
                //if (PageStyle.IsDerivedStyle&&PageStyle.IsOrgPropertyValue(nameof(IPageStyle.Background), PageStyle.Background))
                //{
                //    PageImage pageImage = new PageImage(PageStyle.Background as PageImage);
                //    PageStyle.Background = pageImage;
                //}

                _SelectedBackground.BackgroundColor = value;
                //PageStyle.Background.Color = new ColorConverter().ConvertToString(value);

            }
        }


        /// <exclude>Excluded</exclude>
        List<PageBackgroundViewModel> _Backgrounds;

        public List<PageBackgroundViewModel> Backgrounds
        {
            get
            {
                return _Backgrounds;
            }
        }
        /// <exclude>Excluded</exclude>
        PageBackgroundViewModel _SelectedBackground;

        public PageBackgroundViewModel SelectedBackground
        {
            get
            {
                return _SelectedBackground;
            }
            set
            {
                if (_SelectedBackground != null)
                    _SelectedBackground.RestoreOriginalBackground();
                if (PageStyle.IsDerivedStyle)
                    PageStyle.Background = null;

                _SelectedBackground = value;
                PageStyle.Background = value.PageImage;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedBackgroundColor)));

            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        /// <exclude>Excluded</exclude>
        static ObjectStorage _BackgroundImagesStorage;
        public static ObjectStorage BackgroundImagesStorage
        {
            get
            {
                if (_BackgroundImagesStorage == null)
                {
                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\DontWaitWater";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\BackgroundImages.xml";

                    try
                    {
                        _BackgroundImagesStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("BackgroundImages", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _BackgroundImagesStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("BackgroundImages",
                                                                    storageLocation,
                                                                    "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }
                        else
                            throw error;
                    }
                    catch (Exception error)
                    {
                    }
                }
                return _BackgroundImagesStorage;
            }

             set
            {
                _BackgroundImagesStorage = value;
            }
        }


    }
    /// <MetaDataID>{c8d14664-6af3-43b4-807f-fa6d3af7ba91}</MetaDataID>
    public class PageBackgroundViewModel : MarshalByRefObject
    {
        public event ObjectChangeStateHandle ObjectChangeState;

        public readonly PageStyle PageStyle;

        readonly PageImage OrgPageImage;
        PageImage _PageImage;
        public PageImage PageImage
        {
            get
            {
                return _PageImage;
            }
        }
        public PageBackgroundViewModel(MenuPresentationModel.MenuStyles.PageImage pageImage, PageStyle pageStyle)
        {
            _PageImage = pageImage;
            OrgPageImage = pageImage;
            PageStyle = pageStyle;
        }

        public string Description
        {
            get
            {
                if (_PageImage == null)
                    return "(none)";
                else
                    return _PageImage.Name;

            }
        }



        public bool BackgroundColorize
        {
            get
            {
                if (PageImage == null)
                    return false;
                return !string.IsNullOrWhiteSpace(PageImage.Color);
            }
            set
            {
                if (!value && PageImage != null)
                    PageImage.Color = null;
            }
        }

        public bool Flip
        {
            get
            {
                if (PageImage == null)
                    return false;

                return PageImage.Flip;
            }
            set
            {
                if (_PageImage == OrgPageImage)
                {
                    _PageImage = new PageImage(PageStyle.Background as PageImage);
                    PageStyle.Background = _PageImage;
                }
                PageImage.Flip = value;
            }
        }
        int _Opacity=70;
        public int Opacity
        {
            get
            {
                if (PageImage == null)
                    return 100;
                return (int)PageImage.Opacity*100;
            }
            set
            {
                if (_PageImage == OrgPageImage)
                {
                    _PageImage = new PageImage(PageStyle.Background as PageImage);
                    PageStyle.Background = _PageImage;
                }
                PageImage.Opacity = ((double)value)/100;
            }
        }
        public bool Mirror
        {
            get
            {
                if (PageImage == null)
                    return false;

                return PageImage.Mirror;
            }
            set
            {
                if (_PageImage == OrgPageImage)
                {
                    _PageImage = new PageImage(PageStyle.Background as PageImage);
                    PageStyle.Background = _PageImage;
                }
                PageImage.Mirror = value;
            }
        }

        private static List<Color> ConvertToGrayScaleImage(List<Color> orgColors, Color newColor)
        {
            System.Drawing.Bitmap originalBitmap = Properties.Resources.TransformColor;
            // A blank bitmap is created having same size as original bitmap image.
            System.Drawing.Bitmap GrayScaleBitmap = new System.Drawing.Bitmap(originalBitmap.Width, originalBitmap.Height);

            List<Color> colors = new List<Color>();

            int i = 0;
            foreach (Color color in orgColors)
            {
                originalBitmap.SetPixel(i, 0, System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
                i++;
            }



            // Initializing a graphics object from the new image bitmap.
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(GrayScaleBitmap);

            // Creating the Grayscale ColorMatrix whose values are determined by
            // calculating the luminosity of a color, which is a weighted average of the
            // RGB color components. The average is weighted according to the sensitivity
            // of the human eye to each color component. The weights used here are as
            // given by the NTSC (North America Television Standards Committee)
            // and are widely accepted.
            //ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            //{
            //    new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
            //    new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
            //    new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
            //    new float[] { 0,      0,      0,      1, 0 },
            //    new float[] { 0,      0,      0,      0, 1 }
            //});

            //ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            //{
            //    new float[] {1f, 0.349f, 0.272f, 0, 0},
            //    new float[] {1f, 0.686f, 0.534f, 0, 0},
            //    new float[] {1f, 0.168f, 0.131f, 0, 0},
            //    new float[] { 0, 0, 0, 1, 0},
            //    new float[] { 0, 0, 0, 0, 1}
            //});
            //#0b8565
            //a11d28
            float r = ((float)newColor.R) / ((float)0xff);
            float g = ((float)newColor.G) / ((float)0xff);
            float b = ((float)newColor.B) / ((float)0xff);
            if (r > 1)
                r = 1;
            if (g > 1)
                g = 1;
            if (b > 1)
                b = 1;


            int e = 0;
            System.Drawing.Imaging.ColorMatrix orgcolorMatrix = new System.Drawing.Imaging.ColorMatrix(new float[][]
           {
                             new float[] { r, 0, 0, 0, 0 },
                             new float[] { 0, g, 0, 0, 0 },
                             new float[] { 0, 0, b, 0, 0 },
                             new float[] { 0, 0, 0, 1, 0 },
                             new float[] { 0, 0, 0, 0, 1 }
           });
            //create the grayscale ColorMatrix
            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {.6f, .6f, .6f, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            // Creating image attributes.
            System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();

            // Setting the color matrix attribute.
            attributes.SetColorMatrix(colorMatrix);

            // Drawing the original bitmap image on the new bitmap image using the
            // Grayscale color matrix.
            graphics.DrawImage(originalBitmap, new System.Drawing.Rectangle(0, 0, originalBitmap.Width,
                originalBitmap.Height), 0, 0, originalBitmap.Width,
                originalBitmap.Height, System.Drawing.GraphicsUnit.Pixel, attributes);

            graphics.Dispose();
            attributes.SetColorMatrix(orgcolorMatrix);
            originalBitmap = GrayScaleBitmap;
            GrayScaleBitmap = new System.Drawing.Bitmap(originalBitmap.Width, originalBitmap.Height);

            graphics = System.Drawing.Graphics.FromImage(GrayScaleBitmap);
            // Drawing the original bitmap image on the new bitmap image using the
            // Grayscale color matrix.
            graphics.DrawImage(originalBitmap, new System.Drawing.Rectangle(0, 0, originalBitmap.Width,
                originalBitmap.Height), 0, 0, originalBitmap.Width,
                originalBitmap.Height, System.Drawing.GraphicsUnit.Pixel, attributes);


            // Disposing the Graphics object.
            graphics.Dispose();


            i = 0;
            foreach (Color color in orgColors)
            {
                var dColor = originalBitmap.GetPixel(i, 0);
                colors.Add(Color.FromArgb(dColor.A, dColor.R, dColor.G, dColor.B));
                i++;
            }


            return colors;
        }

        internal void RestoreOriginalBackground()
        {
            //PageStyle.Background = OrgPageImage;
            _PageImage = OrgPageImage;

            ObjectChangeState?.Invoke(this, null);
        }

        public Color BackgroundColor
        {
            get
            {
                if (PageStyle.Background == null || string.IsNullOrWhiteSpace(PageStyle.Background.Color))
                    return default(Color);
                else
                {
                    object color = ColorConverter.ConvertFromString(PageStyle.Background.Color);
                    if (color is Color)
                        return (Color)color;
                    else
                        return default(Color);

                }

                if (_PageImage != null && _PageImage.IsVectorImage)
                {
                    SharpVectors.Dom.Svg.SvgDocument doc = new SharpVectors.Dom.Svg.SvgDocument(null);
                    doc.Load(_PageImage.PortraitUri);
                    List<Color> colors = new List<Color>();
                    for (ulong i = 0; i != doc.StyleSheets.Length; i++)
                    {

                        var staleSheet = doc.StyleSheets[i];
                        for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                        {
                            var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                            string sdf = cssStyleRule.SelectorText;

                            if (cssStyleRule.Style.GetPropertyCssValue("fill") != null)
                            {
                                string colorStr = cssStyleRule.Style.GetPropertyValue("fill");
                                object color = ColorConverter.ConvertFromString(colorStr);
                                if (color is Color)
                                    colors.Add((Color)color);
                            }
                        }
                    }
                    return Colors.Black;
                }
                return Colors.Black;

            }

            set
            {
                //if (_PageImage == OrgPageImage)
                //{
                //    _PageImage = new PageImage(PageStyle.Background as PageImage);
                //    PageStyle.Background = _PageImage;
                //}
                //PageStyle.Background.Color = new ColorConverter().ConvertToString(value);
            }
        }
    }
}