using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MenuPresentationModel.MenuCanvas;

using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{1658a4cf-0c9b-4ca6-a34a-a76bc6d5dcc1}</MetaDataID>
    public class HeadingStyleAccentViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        /// <MetaDataID>{a4a6e0b6-e993-497a-a6c0-bbb1b13598c7}</MetaDataID>
        Size textSize;
        /// <MetaDataID>{1813e826-bfd5-4d88-a7f7-15cc2fde567f}</MetaDataID>
        MenuCanvasItemTextViewModel MenuCanvasItemViewModel;
        /// <MetaDataID>{654e6a57-e4f5-4801-84ae-3efdd934e9f3}</MetaDataID>
        public HeadingStyleAccentViewModel()
        {
            try
            {
                MenuPresentationModel.MenuCanvas.FoodItemsHeading heading = new MenuPresentationModel.MenuCanvas.FoodItemsHeading();
                MenuCanvasItemViewModel = new MenuCanvasItemTextViewModel(heading);

                //if (Accents.Count > 0)
                //    heading.Accent = new MenuCanvasHeadingAccent(heading, Accents[0].Accent);
            }
            catch (Exception)
            {
            }
            //            Refresh();
        }

        /// <MetaDataID>{175d615e-95c9-47c8-b6e1-e1958b806d43}</MetaDataID>
        public HeadingStyleAccentViewModel(MenuCanvasItemTextViewModel menuCanvasItemViewModel)
        {
            MenuCanvasItemViewModel = menuCanvasItemViewModel;
            //Refresh();
        }



        /// <MetaDataID>{1cd1131b-8370-43a8-87d7-a835543142db}</MetaDataID>
        System.Collections.Generic.List<HeadingAccentPresentation> _Accents = null;
        /// <MetaDataID>{f90b4bff-7813-4600-b7cd-43e18687fb78}</MetaDataID>
        public System.Collections.Generic.List<HeadingAccentPresentation> Accents
        {
            get
            {
                if (_Accents == null)
                {
                    _Accents = (from headingAccent in (from styleSheet in MenuPresentationModel.MenuStyles.StyleSheet.StyleSheets
                                                       from headingStyle in styleSheet.Styles.Values.OfType<MenuPresentationModel.MenuStyles.IHeadingStyle>()
                                                       where headingStyle.Accent != null
                                                       select headingStyle.Accent).Distinct().ToList()
                                select new HeadingAccentPresentation(headingAccent)).ToList();
                }
                return _Accents;
            }
        }

        /// <MetaDataID>{d18ec01e-543e-4e28-b7db-cf7f48320a00}</MetaDataID>
        int _SelectedAccent = 0;
        /// <MetaDataID>{2058b439-d37e-46ee-b514-77c21134def7}</MetaDataID>
        public int SelectedAccent
        {
            get
            {
                return _SelectedAccent;
            }
            set
            {
                _SelectedAccent = value;
                (MenuCanvasItemViewModel.MenuCanvasItem as MenuPresentationModel.MenuCanvas.FoodItemsHeading).CustomHeadingAccent = _Accents[_SelectedAccent].Accent;
                (MenuCanvasItemViewModel.MenuCanvasItem as MenuPresentationModel.MenuCanvas.IMenuCanvasHeading).FullRowWidth = CanvasWidth;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StretchAccent)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentUri)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentWidth)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentHeight)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAccentLeft)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAccentTop)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentImages)));

            }
        }

        ///// <MetaDataID>{4e18dcc9-4a7e-4c4a-9627-6f06ef3192b6}</MetaDataID>
        //public void Refresh()
        //{
        //    try
        //    {
        //        string fontJson = "{\"$id\":\"1\",\"ShadowColor\":null,\"BlurRadius\":0.0,\"ShadowYOffset\":0.0,\"ShadowXOffset\":0.0,\"StrokeThickness\":0.0,\"StrokeFill\":null,\"FontSize\":26.6666666666667,\"FontFamilyName\":\"Oswald\",\"FontWeight\":\"Bold\",\"FontStyle\":\"Normal\",\"Foreground\":\"#FFD4C4A2\",\"Stroke\":false,\"AllCaps\":true,\"Shadow\":false}";

        //        var font = JsonConvert.DeserializeObject<FontData>(fontJson);

        //        MenuPresentationModel.MenuCanvas.FoodItemsHeading heading = MenuCanvasItemViewModel.MenuCanvasItem as MenuPresentationModel.MenuCanvas.FoodItemsHeading;

        //        heading.Font = font;
        //        heading.Description = "Appetizers".ToUpper();

        //        textSize = MenuPresentationModel.MenuCanvas.MenuCanvasColumn.MeasureText(heading.Description, font);
        //        heading.XPos = (CanvasWidth / 2) - (textSize.Width / 2);
        //        heading.YPos = (CanvasHeight / 2) - (textSize.Height / 2);
        //        MenuCanvasItemViewModel.Refresh();

        //        //MarginLeft = 1.5 * FontSize;// (textSize.Width * 1.5 - textSize.Width) / 2;
        //        //MarginRight = 1.5 * FontSize;// (textSize.Width * 1.5 - textSize.Width) / 2;
        //        //MarginTop = 0.1 * FontSize; //(textSize.Height * 1.2 - textSize.Height) / 2;
        //        //MarginBottom = 0.1 * FontSize; //(textSize.Height * 1.2 - textSize.Height) / 2;

        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));

        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingLeft)));
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingTop)));

        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentWidth)));
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentHeight)));
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAccentLeft)));
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAccentTop)));

        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentImages)));


        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Left)));
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top)));


        //        if (heading.Accent != null)
        //            heading.Accent.FullRowWidth = CanvasWidth;
        //    }
        //    catch (Exception)
        //    {


        //    }

        //}

        ///// <MetaDataID>{6ed1110a-e354-48bb-8eab-9a1a2926deb2}</MetaDataID>
        //double MarginTop;
        ///// <MetaDataID>{1b684aa9-8a9c-484a-8fa3-09bae1de926f}</MetaDataID>
        //double MarginLeft;
        ///// <MetaDataID>{733e5edf-6173-427e-967e-6a738ed7f084}</MetaDataID>
        //double MarginRight;
        ///// <MetaDataID>{8fe0e73f-c968-4044-bd79-1f870aecd1b8}</MetaDataID>
        //double MarginBottom;


        ///// <MetaDataID>{b792c210-28ff-4ed9-a782-3eea1edbc828}</MetaDataID>
        //public string AccentUri
        //{
        //    get
        //    {
        //        //return @"C:\ProgramData\Microneme\DontWaitWater\AccentImages\Box_Banner_4.svg";
        //        if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent != null)
        //            return (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.Accent.AccentImages[0].Uri;
        //        else
        //            return "";
        //    }
        //}
        /// <MetaDataID>{28d39b28-2768-498e-b29b-3c90ed303c12}</MetaDataID>
        //public double AccentWidth
        //{
        //    get
        //    {
        //        if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent == null)
        //            return 0;
        //        var rect = (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.GetAccentImageRect(1);
        //        return rect.Width;
        //        //if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.FullRowImage)
        //        //    return CanvasWidth;
        //        //else
        //        //    return textSize.Width + MarginLeft + MarginRight;
        //    }
        //}



        /// <MetaDataID>{09ef0129-32bb-49c3-8906-1c21e8fca494}</MetaDataID>
        public bool StretchAccent
        {
            get
            {
                if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).MenuCanvasAccent == null)
                    return false;
                return (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).MenuCanvasAccent.FullRowImage;
            }
            set
            {
                if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).MenuCanvasAccent != null)
                    (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).MenuCanvasAccent.FullRowImage = value;

                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentUri)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentImages)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAccentLeft)));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAccentTop)));
            }
        }
        // /// <MetaDataID>{346ea803-b233-4a0d-90a9-ddebbf2ee74a}</MetaDataID>
        //public double AccentHeight
        //{
        //    get
        //    {
        //        if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent == null)
        //            return 0;

        //        return (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.GetAccentImageRect(0).Height;
        //        //if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.UnderlineImage)
        //        //    return 10;
        //        //else
        //        //    return textSize.Height + MarginTop + MarginBottom;
        //    }
        //}



        /// <exclude>Excluded</exclude>
        double _CanvasWidth = 0;

        /// <MetaDataID>{a3f40147-4cad-4493-a28d-7077d5b02936}</MetaDataID>
        public double CanvasWidth
        {
            get
            {
                return _CanvasWidth;
            }
            set
            {
                _CanvasWidth = value;

                //if (MenuCanvasItemViewModel.MenuCanvasItem is IMenuCanvasHeading && (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent != null)
                //    (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.FullRowWidth = value;
            }
        }
        /// <exclude>Excluded</exclude>
        double _CanvasHeight;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{24a3c160-d38e-410f-82a3-b7f97ac28f18}</MetaDataID>
        public double CanvasHeight
        {
            get
            {
                return _CanvasHeight;
            }
            set
            {
                _CanvasHeight = value;
            }
        }
        ///// <MetaDataID>{5909c544-081e-4814-bef9-52785b44259a}</MetaDataID>
        //public double HeadingTop
        //{
        //    get
        //    {
        //        return MenuCanvasItemViewModel.Top;
        //    }
        //}
        ///// <MetaDataID>{56585e7c-bd6e-433c-ae50-9b8e9f795962}</MetaDataID>
        //public double HeadingLeft
        //{
        //    get
        //    {
        //        return MenuCanvasItemViewModel.Left;
        //    }
        //}


        public List<ICanvasItem> AccentImages
        {
            get
            {
                List<ICanvasItem> canvasItem = new List<ICanvasItem>();
                if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).MenuCanvasAccent != null)
                {
                    int i = 0;
                    foreach (var accentImage in (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).MenuCanvasAccent.Accent.AccentImages)
                        canvasItem.Add(new AccentImagePresentation((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).MenuCanvasAccent, i++));
                }
                canvasItem.Add(MenuCanvasItemViewModel);
                return canvasItem;
            }
        }


        ///// <MetaDataID>{ab0338c7-cb41-411e-aa65-3df9eb5b12ea}</MetaDataID>
        //public double HeadingAccentTop
        //{
        //    get
        //    {
        //        if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent == null)
        //            return 0;

        //        return (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.GetAccentImageRect(0).Y;
        //        //if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.UnderlineImage)
        //        //    return MenuCanvasItemViewModel.Top + textSize.Height;
        //        //else
        //        //    return MenuCanvasItemViewModel.Top - MarginTop;
        //    }
        //}
        ///// <MetaDataID>{1c286dd1-3456-4b40-86ee-95ab19b410d3}</MetaDataID>
        //public double HeadingAccentLeft
        //{
        //    get
        //    {
        //        if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent == null)
        //            return 0;


        //        return (MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.GetAccentImageRect(0).X;

        //        //if ((MenuCanvasItemViewModel.MenuCanvasItem as IMenuCanvasHeading).Accent.FullRowImage)
        //        //    return 0;
        //        //else
        //        //    return MenuCanvasItemViewModel.Left - MarginLeft;
        //    }
        //}

        //HeadingAccentTop
        /// <MetaDataID>{5f8e84b3-c515-47a9-88c2-91dfa57fcf8f}</MetaDataID>
        //public string Text
        //{
        //    get
        //    {
        //        return MenuCanvasItemViewModel.Text;
        //    }
        //}
        //    /// <MetaDataID>{05a217c6-c4ca-4990-8ca0-62bc73e2c20d}</MetaDataID>
        //    public System.Windows.Media.FontFamily FontFamily
        //    {
        //        get
        //        {
        //            return MenuCanvasItemViewModel.FontFamily;
        //        }
        //    }

        //    /// <MetaDataID>{db0bc3bc-09e2-440e-80f9-0187d4cecf43}</MetaDataID>
        //    public FontStyle FontStyle
        //    {
        //        get
        //        {
        //            return MenuCanvasItemViewModel.FontStyle;
        //        }
        //    }

        //    /// <MetaDataID>{00dba239-d432-42bb-9a25-e8b37346aa21}</MetaDataID>
        //    public FontWeight FontWeight
        //    {
        //        get
        //        {
        //            return MenuCanvasItemViewModel.FontWeight;
        //        }
        //    }

        //    /// <MetaDataID>{dacdf018-2f08-452c-9f58-31fdd3ab3e16}</MetaDataID>
        //    public System.Windows.Media.Brush Foreground
        //    {
        //        get
        //        {
        //            return MenuCanvasItemViewModel.Foreground;
        //        }
        //    }
        //    /// <MetaDataID>{bd71e709-2589-4205-b634-e2a47172c65e}</MetaDataID>
        //    public double FontSize
        //    {
        //        get
        //        {

        //            return MenuCanvasItemViewModel.FontSize;
        //        }
        //    }

        //    public double Top
        //    {
        //        get
        //        {
        //            return MenuCanvasItemViewModel.Top; 
        //        }

        //        set
        //        {

        //        }
        //    }

        //    public double Left
        //    {
        //        get
        //        {
        //            return MenuCanvasItemViewModel.Left;
        //        }

        //        set
        //        {

        //        }
        //    }

        //    public double Height
        //    {
        //        get
        //        {
        //            return textSize.Height;
        //        }

        //        set
        //        {
        //        }
        //    }

        //    public double Width
        //    {
        //        get
        //        {
        //            return textSize.Width;

        //        }

        //        set
        //        {

        //        }
        //    }
    }



}
