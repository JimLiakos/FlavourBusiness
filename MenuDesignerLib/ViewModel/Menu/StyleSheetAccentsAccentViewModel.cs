using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{90b4402a-8550-4e97-92c0-26ecc4878da8}</MetaDataID>
    public class StyleSheetAccentsAccentViewModel : System.MarshalByRefObject, INotifyPropertyChanged
    {
        /// <MetaDataID>{46bd21df-830f-4486-948c-22d3c88e54ad}</MetaDataID>
        List<MenuCanvasItemTextViewModel> FoodItemTextCanvasItemsSlots = new List<MenuCanvasItemTextViewModel>();
        /// <MetaDataID>{5e34ba44-cd30-4d28-ad46-35624c21cbfa}</MetaDataID>
        List<AccentImagePresentation> FoodItemAccentImagesSlots = new List<AccentImagePresentation>();


        /// <MetaDataID>{1fe07ad9-15b7-4eb2-b63e-125b331182c3}</MetaDataID>
        List<MenuCanvasItemTextViewModel> HeadingTextCanvasItemsSlots = new List<MenuCanvasItemTextViewModel>();
        /// <MetaDataID>{86d6524d-bb55-4968-953a-eba88f2ca1de}</MetaDataID>
        List<AccentImagePresentation> HeadingAccentImagesSlots = new List<AccentImagePresentation>();

        /// <MetaDataID>{20732ea1-7817-407b-8449-5a849ca3697b}</MetaDataID>
        public List<ICanvasItem> HeadingCanvasItems
        {
            get
            {
                List<ICanvasItem> canvasItems = HeadingAccentImagesSlots.OfType<ICanvasItem>().ToList();
                canvasItems.AddRange(HeadingTextCanvasItemsSlots.OfType<ICanvasItem>().ToList());
                return canvasItems;
            }
        }

        /// <exclude>Excluded</exclude>
        double _ZoomPercentage;
        /// <MetaDataID>{7e2b14d0-58d7-4ca3-86d7-627156047f3d}</MetaDataID>
        public double ZoomPercentage
        {
            get
            {
                return _ZoomPercentage;
            }
            set
            {
                _ZoomPercentage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomPercentageLabel)));
            }
        }

        /// <MetaDataID>{4001637b-b1b6-40c7-a58d-ee320c20954f}</MetaDataID>
        public string ZoomPercentageLabel
        {
            get
            {
                return ZoomPercentage.ToString("N2") + "%";
            }
        }



        /// <MetaDataID>{afbdd6a3-c116-4a9b-a7ef-3ffd20339773}</MetaDataID>
        public List<ICanvasItem> CanvasItems
        {
            get
            {
                List<ICanvasItem> canvasItems = FoodItemAccentImagesSlots.OfType<ICanvasItem>().ToList();
                canvasItems.AddRange(FoodItemTextCanvasItemsSlots.OfType<ICanvasItem>().ToList());
                return canvasItems;
            }
        }

        /// <exclude>Excluded</exclude>
        UIBaseEx.Unit _AccentMarginUnit = UIBaseEx.Unit.em;

        /// <MetaDataID>{629e806c-0497-416e-bdb1-1f1d2ee29d58}</MetaDataID>
        public UIBaseEx.Unit AccentMarginUnit
        {
            get
            {
                if (PaddingAccent != null)
                    return PaddingAccent.Accent.MarginUnit;
                else
                    return _AccentMarginUnit;
            }
            set
            {
                if (PaddingAccent != null)
                    PaddingAccent.Accent.MarginUnit = value;

                _AccentMarginUnit = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MarginUnit)));
                RedrawHeading();
            }
        }
        /// <MetaDataID>{d45cf8c2-e56c-47c9-bcd2-48e92e1816a1}</MetaDataID>
        public List<UIBaseEx.Unit> Units
        {
            get
            {
                return Enum.GetValues(typeof(UIBaseEx.Unit)).Cast<UIBaseEx.Unit>().ToList();
            }
        }
        public List<AccentPosition> Positions
        {
            get
            {
                return Enum.GetValues(typeof(AccentPosition)).Cast<AccentPosition>().ToList();
            }
        }

        public AccentPosition Position
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent.UnderlineImage)
                    return AccentPosition.Underline;
                if (PaddingAccent != null && PaddingAccent.Accent.OverlineImage)
                    return AccentPosition.OverLine;

                return AccentPosition.Background;

            }
            set
            {
                if (value == AccentPosition.Background)
                {
                    PaddingAccent.Accent.OverlineImage = false;
                    PaddingAccent.Accent.UnderlineImage = false;
                }
                if (value == AccentPosition.Underline)
                {
                    PaddingAccent.Accent.OverlineImage = false;
                    PaddingAccent.Accent.UnderlineImage = true;
                }
                if (value == AccentPosition.OverLine)
                {
                    PaddingAccent.Accent.OverlineImage = true;
                    PaddingAccent.Accent.UnderlineImage = false;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOverlineUnderlineAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBackgroundRelativeSizeAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Margin)));


                RedrawHeading();


            }
        }


        /// <MetaDataID>{58272dbd-9896-47d0-8464-64a94d605113}</MetaDataID>
        MenuCanvasFoodItem MenuCanvasFoodItem = null;
        /// <MetaDataID>{727f5993-c83d-4e9d-91e5-30e597091eac}</MetaDataID>
        FoodItemsHeading FoodItemsHeading = null;
        /// <MetaDataID>{c3ba3311-d842-4834-958a-645eb9a01a58}</MetaDataID>
        public double FoodItemAreaHeight { get; set; }
        /// <MetaDataID>{acd02719-cdee-462b-89bd-3edb24d055fc}</MetaDataID>
        public double FoodItemAreaWidth { get; set; }

        /// <MetaDataID>{028d61c0-6627-4847-8bd7-85dc5969ae33}</MetaDataID>
        public double HeadingAreaHeight { get; set; }
        /// <MetaDataID>{d2f547cd-1a48-4000-879c-2375cc07026e}</MetaDataID>
        public double HeadingAreaWidth { get; set; }
        /// <MetaDataID>{03249b48-8db0-4046-b057-c6934b339e2f}</MetaDataID>
        MenuPresentationModel.MenuPage FoodItemMenuPage;

        /// <MetaDataID>{fab45570-6c90-48a8-b616-417b32d7131e}</MetaDataID>
        MenuPresentationModel.MenuPage HeadingMenuPage;
        /// <MetaDataID>{c89b3f85-ee58-4dad-8c33-5005503b093d}</MetaDataID>
        public StyleSheetAccentsAccentViewModel()
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(DownloadStylesWindow.HeadingAccentStorage);
            var accents = (from accent in storage.GetObjectCollection<Accent>() where accent.MultipleItemsAccent select accent).ToList();
            _StretchAccentImages = (from accent in accents select new AccentViewModel(accent)).ToList();

            accents = (from accent in storage.GetObjectCollection<Accent>() where !accents.Contains(accent) select accent).ToList();
            _Accents = (from accent in accents select new AccentViewModel(accent)).ToList();



            try
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    HeadingMenuPage = new MenuPresentationModel.MenuPage();
                    //FoodItemMenuPage = new MenuPresentationModel.MenuPage();
                    FoodItemMenuPage = HeadingMenuPage;
                    //HeadingMenuPage = FoodItemMenuPage;
                    StyleSheet styleSheet = new StyleSheet();
                    string json = System.Text.UTF8Encoding.UTF8.GetString(Properties.Resources.StylePrototype);
                    try
                    {

                        System.Collections.Generic.Dictionary<string, MenuPresentationModel.MenuStyles.IStyleRule> styles = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, MenuPresentationModel.MenuStyles.IStyleRule>>(json, new OOAdvantech.Json.JsonSerializerSettings
                        {
                            PreserveReferencesHandling = OOAdvantech.Json.PreserveReferencesHandling.All,
                            ReferenceLoopHandling = OOAdvantech.Json.ReferenceLoopHandling.Ignore,
                            TypeNameHandling = OOAdvantech.Json.TypeNameHandling.Objects
                        });

                        foreach (var styleEntry in styles)
                            styleSheet.AddStyle(styleEntry.Value);
                        MenuPresentationModel.RestaurantMenu restaurantMenu = new MenuPresentationModel.RestaurantMenu();
                        restaurantMenu.Style = styleSheet;
                        restaurantMenu.AddPage(FoodItemMenuPage);
                        restaurantMenu.AddPage(HeadingMenuPage);
                    }
                    catch (Exception error)
                    {
                    }

                    FoodItemAreaWidth = FoodItemMenuPage.Width;
                    HeadingAreaWidth = HeadingMenuPage.Width;

                    MenuCanvasFoodItem = new MenuCanvasFoodItem();
                    MenuModel.MenuItem menuItem = new MenuModel.MenuItem();
                    menuItem.Name = "Favorita";
                    menuItem.Description = "Santos tomatoes, fior di latter mozzarella, prosciutto di parma ham, rocket, olives and tomato sauce.";
                    var menuItemPrice = menuItem.MenuItemPrice;
                    menuItemPrice.Price = 10;
                    MenuCanvasFoodItem.MenuItem = menuItem;
                    FoodItemMenuPage.AddMenuItem(MenuCanvasFoodItem);

                    FoodItemsHeading = new FoodItemsHeading();
                    FoodItemsHeading.Description = "Appetizers";
                    HeadingMenuPage.AddMenuItem(FoodItemsHeading);


                    RedrawHeading();

                    stateTransition.Consistent = true;
                }


            }
            catch (Exception error)
            {
            }
        }


        /// <MetaDataID>{bed7f4cb-999b-4674-a8ba-054db414ad42}</MetaDataID>
        public Color TextColor
        {
            get
            {
                if (FoodItemMenuPage.Style != null)
                    return (Color)ColorConverter.ConvertFromString((FoodItemMenuPage.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).Font.Foreground);
                else
                    return Colors.Gray;
            }
            set
            {
                if (FoodItemMenuPage.Style != null)
                {
                    var fontData = (FoodItemMenuPage.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).Font;
                    fontData.Foreground = new ColorConverter().ConvertToString(value);
                    (FoodItemMenuPage.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).Font = fontData;

                    fontData = (FoodItemMenuPage.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).DescriptionFont;
                    fontData.Foreground = new ColorConverter().ConvertToString(value);
                    (FoodItemMenuPage.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).DescriptionFont = fontData;

                    fontData = (FoodItemMenuPage.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).ExtrasFont;
                    fontData.Foreground = new ColorConverter().ConvertToString(value);
                    (FoodItemMenuPage.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).ExtrasFont = fontData;

                    fontData = (FoodItemMenuPage.Style.Styles["heading"] as MenuPresentationModel.MenuStyles.HeadingStyle).Font;
                    fontData.Foreground = new ColorConverter().ConvertToString(value);
                    (FoodItemMenuPage.Style.Styles["heading"] as MenuPresentationModel.MenuStyles.HeadingStyle).Font = fontData;

                    fontData = (FoodItemMenuPage.Style.Styles["price-options"] as MenuPresentationModel.MenuStyles.PriceStyle).Font;
                    fontData.Foreground = new ColorConverter().ConvertToString(value);
                    (FoodItemMenuPage.Style.Styles["price-options"] as MenuPresentationModel.MenuStyles.PriceStyle).Font = fontData;







                    RedrawHeading();
                }
            }
        }

        /// <exclude>Excluded</exclude>
        AccentViewModel _SelectedAccent;
        /// <MetaDataID>{937b9bb4-72bd-413e-97c0-8640bdee7c37}</MetaDataID>
        public AccentViewModel SelectedAccent
        {
            get
            {

                return _SelectedAccent;
            }
            set
            {
                _SelectedAccent = value;
                _SelectedStretchAccent = null;
                PaddingAccent = value;
                //this.FoodItemsHeading.CustomHeadingAccent = _SelectedAccent.Accent;
                this.MenuCanvasFoodItem.AccentType = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingTop)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingLeft)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingBottom)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingRight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinHeight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullRowWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RelativeSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBackgroundRelativeSizeAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOverlineUnderlineAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WidthLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeightLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));




                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentMarginUnit)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MarginUnit)));


                RedrawHeading();
            }
        }

        /// <MetaDataID>{ac60371a-6b05-4168-9511-79553f32f00d}</MetaDataID>
        public bool RelativeSize
        {
            get
            {
                if (PaddingAccent == null)
                    return false;
                return !ImageSize;
            }
        }

        public bool IsBackgroundRelativeSizeAccent
        {
            get
            {
                if (PaddingAccent == null)
                    return false;

                return RelativeSize && (Position == AccentPosition.Background);
            }
        }
        public bool IsOverlineUnderlineAccent
        {
            get
            {
                if (PaddingAccent == null)
                    return false;

                return (Position != AccentPosition.Background);
            }
        }

        /// <MetaDataID>{9c66c4bf-7cfe-4a5d-a7fc-ae0039008a79}</MetaDataID>
        AccentViewModel PaddingAccent;
        /// <exclude>Excluded</exclude>
        AccentViewModel _SelectedStretchAccent;
        /// <MetaDataID>{317d77da-b500-4bb8-b6be-bbb0339fae91}</MetaDataID>
        public AccentViewModel SelectedStretchAccent
        {
            get
            {

                return _SelectedStretchAccent;
            }
            set
            {
                _SelectedStretchAccent = value;
                PaddingAccent = value;
                //this.MenuCanvasFoodItem.AccentType = _SelectedStretchAccent.Accent;
                //this.FoodItemsHeading.CustomHeadingAccent = _SelectedStretchAccent.Accent;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingTop)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingLeft)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingBottom)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PaddingRight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccentMarginUnit)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MarginUnit)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullRowWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RelativeSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBackgroundRelativeSizeAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOverlineUnderlineAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WidthLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeightLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));

                RedrawHeading();
            }
        }



        /// <MetaDataID>{5b392f73-07d5-4d44-b5b6-1ab9a0a3e216}</MetaDataID>
        public double PaddingLeft
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                    return Math.Round(PaddingAccent.Accent.MarginLeft, 2);
                else
                    return 0;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    PaddingAccent.Accent.MarginLeft = value;

                    RedrawHeading();
                }
            }
        }

        /// <MetaDataID>{35408227-0546-4782-89b1-7f18c444b8a7}</MetaDataID>
        public double PaddingTop
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                    return Math.Round(PaddingAccent.Accent.MarginTop, 2);
                else
                    return 0;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    PaddingAccent.Accent.MarginTop = value;

                    RedrawHeading();
                }
            }
        }
        /// <MetaDataID>{bf698804-35ee-4b3a-bc69-923805e2f013}</MetaDataID>
        public bool ImageSize
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                    return PaddingAccent.Accent.OrgSize;
                else
                    return false;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    PaddingAccent.Accent.OrgSize = value;
                    if (value)
                        FullRowWidth = false;
                    RedrawHeading();
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RelativeSize)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBackgroundRelativeSizeAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOverlineUnderlineAccent)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WidthLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeightLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinHeight)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullRowWidth)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSize)));


            }
        }
        /// <MetaDataID>{ffd1e530-5c26-4963-bc92-707a348f66f9}</MetaDataID>
        public bool FullRowWidth
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                    return PaddingAccent.Accent.FullRowImage;
                else
                    return false;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    PaddingAccent.Accent.FullRowImage = value;
                    if (value)
                        ImageSize = false;
                    RedrawHeading();
                }
            }
        }

        /// <MetaDataID>{2f3b2dfb-39c8-4b0c-ab51-42e5f4881636}</MetaDataID>
        public double MinWidth
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    if (ImageSize)
                        return Math.Round(UIBaseEx.SizeUtil.PixelTomm(PaddingAccent.Accent.Width), 2);
                    else
                        return Math.Round(UIBaseEx.SizeUtil.PixelTomm(PaddingAccent.Accent.MinWidth), 2);
                }
                else
                    return 0;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    if (ImageSize)
                        PaddingAccent.Accent.Width = Math.Round(UIBaseEx.SizeUtil.mmToPixel(value), 2);
                    else
                        PaddingAccent.Accent.MinWidth = Math.Round(UIBaseEx.SizeUtil.mmToPixel(value), 2);

                    //RedrawFoodItem();
                    RedrawHeading();
                }
            }
        }

        /// <MetaDataID>{dbb73192-053e-4813-80ae-b4908edc88fb}</MetaDataID>
        public string SizeUnit
        {
            get
            {
                return UIBaseEx.Unit.mm.ToString();
            }
        }

        /// <MetaDataID>{69fa4db0-5c3c-4c1d-90a7-ed0d8e3d5470}</MetaDataID>
        public double MinHeight
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    if (ImageSize)
                        return Math.Round(UIBaseEx.SizeUtil.PixelTomm(PaddingAccent.Accent.Height), 2);
                    else
                        return Math.Round(UIBaseEx.SizeUtil.PixelTomm(PaddingAccent.Accent.MinHeight), 2);

                }
                else
                    return 0;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    if (ImageSize)
                        PaddingAccent.Accent.Height = UIBaseEx.SizeUtil.mmToPixel(value);
                    else
                        PaddingAccent.Accent.MinHeight = UIBaseEx.SizeUtil.mmToPixel(value);

                    RedrawHeading();
                }
            }
        }

        public double Margin
        {
            get
            {
                if (Position == AccentPosition.Underline)
                    return PaddingBottom;

                if (Position == AccentPosition.OverLine)
                    return PaddingTop;

                return 0;

            }
            set
            {
                if (Position == AccentPosition.Underline)
                    PaddingBottom = value;

                if (Position == AccentPosition.OverLine)
                    PaddingTop = value;

            }

        }

        /// <MetaDataID>{67684d71-84f3-47c6-8124-9d71e8a46f78}</MetaDataID>
        public double PaddingRight
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                    return Math.Round(PaddingAccent.Accent.MarginRight, 2);
                else
                    return 0;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    PaddingAccent.Accent.MarginRight = value;

                    RedrawHeading();
                }
            }
        }

        /// <MetaDataID>{9d9dd037-9390-4eb3-a26f-3afcc746ac80}</MetaDataID>
        public double PaddingBottom
        {
            get
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                    return Math.Round(PaddingAccent.Accent.MarginBottom, 2);
                else
                    return 0;
            }
            set
            {
                if (PaddingAccent != null && PaddingAccent.Accent != null)
                {
                    PaddingAccent.Accent.MarginBottom = value;

                    RedrawHeading();
                }
            }
        }




        /// <MetaDataID>{508c4f72-5b5e-435f-be8d-a9f830bda05b}</MetaDataID>
        public string MarginUnit
        {
            get
            {
                return AccentMarginUnit.ToString();
            }
        }

        /// <MetaDataID>{da545869-a8d7-4494-ba13-94a5ef5a4c5e}</MetaDataID>
        private void RedrawHeading()
        {
            this.MenuCanvasFoodItem.AccentType = null;
            this.FoodItemsHeading.CustomHeadingAccent = null;


            if (_SelectedStretchAccent != null)
            {
                this.MenuCanvasFoodItem.AccentType = _SelectedStretchAccent.Accent;
                this.FoodItemsHeading.CustomHeadingAccent = _SelectedStretchAccent.Accent;
            }

            if (_SelectedAccent != null)
            {
                this.FoodItemsHeading.CustomHeadingAccent = _SelectedAccent.Accent;
            }
            if (FoodItemsHeading.Page == null)
                HeadingMenuPage.AddMenuItem(FoodItemsHeading);

            if (MenuCanvasFoodItem.Page == null)
                HeadingMenuPage.AddMenuItem(MenuCanvasFoodItem);

            FoodItemsHeading.HeadingType = HeadingType.Normal;

            HeadingMenuPage.RenderMenuCanvasItems(new List<IMenuCanvasItem>() { FoodItemsHeading, MenuCanvasFoodItem });

            if (FoodItemsHeading.CustomHeadingAccent != null)
                FoodItemsHeading.MenuCanvasAccent = new MenuCanvasAccent((FoodItemsHeading as FoodItemsHeading).CustomHeadingAccent);

            if (MenuCanvasFoodItem.AccentType != null)
                MenuCanvasFoodItem.MenuCanvasAccent = new MenuCanvasAccent((MenuCanvasFoodItem as MenuCanvasFoodItem).AccentType);


            HeadingAreaHeight = 250;

            if (FoodItemsHeading.Page == null)
                return;



            List<IMenuCanvasItem> menuCanvasTextItems = (from menuCanvasItem in MenuCanvasFoodItem.Page.MenuCanvasItems
                                                         where menuCanvasItem is IMenuCanvasHeading
                                                         select menuCanvasItem).OfType<IMenuCanvasItem>().ToList();

            List<IMenuCanvasFoodItem> menuCanvasFoodItems = (from menuCanvasItem in MenuCanvasFoodItem.Page.MenuCanvasItems
                                                             where menuCanvasItem is IMenuCanvasFoodItem
                                                             select menuCanvasItem).OfType<IMenuCanvasFoodItem>().ToList();

            menuCanvasTextItems.AddRange((from menuCanvasItem in MenuCanvasFoodItem.Page.MenuCanvasItems.OfType<IMenuCanvasFoodItem>()
                                          from menuCanvasFoodItemText in menuCanvasItem.SubTexts
                                          select menuCanvasFoodItemText).OfType<IMenuCanvasItem>().ToList());

            menuCanvasTextItems.AddRange((from menuCanvasItem in MenuCanvasFoodItem.Page.MenuCanvasItems.OfType<IMenuCanvasFoodItem>()
                                          from priceText in menuCanvasItem.Prices
                                          where priceText.Visisble
                                          select priceText).OfType<IMenuCanvasItem>().ToList());

            menuCanvasTextItems.AddRange((from menuCanvasItem in MenuCanvasFoodItem.Page.MenuCanvasItems.OfType<IMenuCanvasFoodItem>()
                                          select menuCanvasItem.PriceLeader).OfType<IMenuCanvasItem>().ToList());





            Dictionary<IMenuCanvasItem, MenuCanvasItemTextViewModel> textCanvasItems = (from menuCanvasItem in HeadingTextCanvasItemsSlots
                                                                                        where menuCanvasTextItems.Contains(menuCanvasItem.MenuCanvasItem)
                                                                                        select menuCanvasItem).ToDictionary(x => x.MenuCanvasItem);

            List<MenuCanvasItemTextViewModel> freeTextCanvasItems = (from menuCanvasItem in HeadingTextCanvasItemsSlots
                                                                     where !menuCanvasTextItems.Contains(menuCanvasItem.MenuCanvasItem)
                                                                     select menuCanvasItem).ToList();


            foreach (var menuCanvasHeadingText in menuCanvasTextItems)
            {
                if (textCanvasItems.ContainsKey(menuCanvasHeadingText))
                {
                    MenuCanvasItemTextViewModel textCanvasItem = textCanvasItems[menuCanvasHeadingText];
                    textCanvasItem.Visibility = Visibility.Visible;
                    textCanvasItem.Refresh();
                }
                else if (freeTextCanvasItems.Count > 0)
                {
                    MenuCanvasItemTextViewModel textCanvasItem = freeTextCanvasItems[0];
                    freeTextCanvasItems.RemoveAt(0);
                    textCanvasItem.Visibility = Visibility.Visible;
                    textCanvasItem.ChangeCanvasItem(menuCanvasHeadingText);
                }
                else
                {
                    MenuCanvasItemTextViewModel textCanvasItem = new MenuCanvasItemTextViewModel(menuCanvasHeadingText);
                    HeadingTextCanvasItemsSlots.Add(textCanvasItem);

                }
            }

            foreach (var textCanvasItem in freeTextCanvasItems)
                textCanvasItem.Visibility = Visibility.Collapsed;


            List<AccentImageKey> menuCanvasAccentImages = new List<AccentImageKey>();
            foreach (var menuCanvasHeadingAccent in FoodItemsHeading.Page.MenuCanvasItems.OfType<IHighlightedMenuCanvasItem>())
            {
                if (menuCanvasHeadingAccent.MenuCanvasAccent != null)
                {
                    int i = 0;
                    foreach (var accentImage in menuCanvasHeadingAccent.MenuCanvasAccent.Accent.AccentImages)
                        menuCanvasAccentImages.Add(new AccentImageKey(menuCanvasHeadingAccent.MenuCanvasAccent, i++));
                }
            }

            Dictionary<AccentImageKey, AccentImagePresentation> accentImagesDictionary = (from headingAccent in HeadingAccentImagesSlots
                                                                                          where menuCanvasAccentImages.Contains(new AccentImageKey(headingAccent.HeadingAccent, headingAccent.AccentIndex))
                                                                                          select headingAccent).ToDictionary(x => new AccentImageKey() { HeadingAccent = x.HeadingAccent, AccentIndex = x.AccentIndex });

            List<AccentImagePresentation> freeAccentImages = (from headingAccent in HeadingAccentImagesSlots
                                                              where !menuCanvasAccentImages.Contains(new AccentImageKey(headingAccent.HeadingAccent, headingAccent.AccentIndex))
                                                              select headingAccent).ToList();


            foreach (var menuCanvasAccentImage in menuCanvasAccentImages)
            {
                AccentImagePresentation accentImageVM = null;
                if (accentImagesDictionary.TryGetValue(menuCanvasAccentImage, out accentImageVM))
                {
                    accentImageVM.Visibility = Visibility.Visible;
                    accentImageVM.ChangeAccent(menuCanvasAccentImage.HeadingAccent, menuCanvasAccentImage.AccentIndex);
                }
                else
                {
                    if (freeAccentImages.Count > 0)
                    {
                        AccentImagePresentation freeAccentImageVM = freeAccentImages[0];
                        freeAccentImages.RemoveAt(0);
                        freeAccentImageVM.Visibility = Visibility.Visible;
                        freeAccentImageVM.ChangeAccent(menuCanvasAccentImage.HeadingAccent, menuCanvasAccentImage.AccentIndex);
                    }
                    else
                    {
                        AccentImagePresentation newAccentImageVM = new AccentImagePresentation(menuCanvasAccentImage.HeadingAccent, menuCanvasAccentImage.AccentIndex);
                        HeadingAccentImagesSlots.Add(newAccentImageVM);
                    }
                }
            }
            foreach (var freeAccentImageVM in freeAccentImages)
                freeAccentImageVM.Visibility = Visibility.Hidden;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingCanvasItems)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAreaHeight)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeadingAreaWidth)));
        }


        public string WidthLabel
        {
            get
            {
                if (ImageSize)
                    return "Width:";
                else
                    return "Min width:";

            }
        }

        public string HeightLabel
        {
            get
            {
                if (ImageSize)
                    return "Height:";
                else
                    return "Min height:";

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;



        /// <exclude>Excluded</exclude>
        List<AccentViewModel> _StretchAccentImages;
        /// <MetaDataID>{f9d33931-bc99-4080-8685-c32fe31c41fc}</MetaDataID>
        public List<AccentViewModel> StretchAccentImages
        {
            get
            {
                return _StretchAccentImages;
            }
        }

        /// <exclude>Excluded</exclude>
        List<AccentViewModel> _Accents;
        /// <MetaDataID>{a3899da7-1a03-4d6c-8aae-9282ab8e7ac2}</MetaDataID>
        public List<AccentViewModel> Accents
        {
            get
            {
                return _Accents;
            }
        }
    }


    /// <MetaDataID>{dbe7bcb9-b78b-495a-b3aa-df82320ea77a}</MetaDataID>
    public enum AccentPosition
    {
        Background,
        Underline,
        OverLine,
    }
}
