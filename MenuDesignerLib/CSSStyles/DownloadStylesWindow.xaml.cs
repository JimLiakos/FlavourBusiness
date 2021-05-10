using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;
using ExCSS;
using MenuPresentationModel.MenuStyles;
using mshtml;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuDesigner
{
    /// <summary>
    /// Interaction logic for DownloadStylesWindow.xaml
    /// </summary>
    /// <MetaDataID>{fa1947a3-f113-417c-8d7d-0f4c36151a12}</MetaDataID>
    public partial class DownloadStylesWindow : Window, System.ComponentModel.INotifyPropertyChanged
    {
        List<MenuProBorder> _Borders = new List<MenuProBorder>();
        public List<MenuProBorder> Borders
        {
            get
            {
                return _Borders;
            }
        }
        MenuProBorder _SelectedBorder;
        public MenuProBorder SelectedBorder
        {
            get
            {
                return _SelectedBorder;
            }
            set
            {
                _SelectedBorder = value;
            }
        }

        /// <MetaDataID>{9fc55a71-9cd2-4325-95ad-c3ae4184678c}</MetaDataID>
        public DownloadStylesWindow()
        {
            InitializeComponent();

            Web.Source = new Uri("https://imenupro.com/SVG/?access_token=demo_user&firstrun=1");
            //Web.Source = new Uri("https://imenupro.com/");
            InitFont();
            DataContext = this;


            XDocument doc = XDocument.Load(@"C:\ProgramData\Microneme\DontWaitWater\MenuPro-Borders.xml");
            foreach (var element in doc.Root.Elements("option"))
                _Borders.Add(new MenuProBorder(element));

        }
        static ObjectStorage _BordersStorage;
        public static ObjectStorage BordersStorage
        {
            get
            {
                if (_BordersStorage == null)
                {
                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\DontWaitWater";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\Borders.xml";

                    try
                    {
                        _BordersStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("Borders", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _BordersStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("Borders",
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
                return _BordersStorage;
            }
        }


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
        }

        static ObjectStorage _HeadingAccentStorage;
        public static ObjectStorage HeadingAccentStorage
        {
            get
            {
                if (_HeadingAccentStorage == null)
                {
                    string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    appDataPath += "\\DontWaitWater";
                    if (!System.IO.Directory.Exists(appDataPath))
                        System.IO.Directory.CreateDirectory(appDataPath);
                    string storageLocation = appDataPath + "\\HeadingAccents.xml";

                    try
                    {
                        _HeadingAccentStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("StyleSheets", storageLocation, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (StorageException error)
                    {

                        if (error.Reason == StorageException.ExceptionReason.StorageDoesnotExist)
                        {
                            _HeadingAccentStorage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("StyleSheets",
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
                return _HeadingAccentStorage;
            }
        }
        /// <MetaDataID>{1462f8f1-75de-44e5-981d-3c781a2b28b1}</MetaDataID>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            //DownloadAccents();

            //CopyHeadingAccent



            mshtml.IHTMLStyleSheet ssheet = (Web.Document as mshtml.HTMLDocument).styleSheets.item(0) as mshtml.IHTMLStyleSheet;
            string title = ssheet.title;
            string styleName = null;

            CSSHeadingStyle titleHeadingStyle = new CSSHeadingStyle();
            CSSHeadingStyle headingStyle = new CSSHeadingStyle();



            CSSHeadingStyle subHeadingStyle = new CSSHeadingStyle();
            CSSHeadingStyle altFontHeadingStyle = new CSSHeadingStyle();


            CSSPageStyle pageStyle = new CSSPageStyle();
            CSSPriceOptions priceOptions = null;
            CSSMenuItemOptions menuItemOptions = null;
            CSSLayoutOptions layoutOptions = null;
            ExCSS.StyleSheet cssStyleSheet = GetStyleSheet(out styleName);
            //if (cssStyleSheet != null)
            //{
            //    GetPageStyleData(pageStyle, cssStyleSheet);
            //    GetHeadingStyleDataFromStyleSheet(cssStyleSheet, headingStyle, "heading", pageStyle);
            //    if (headingStyle.AccentImages.Count > 0)
            //    {
            //        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            //        {

            //            string accentName = headingStyle.AccentName.Replace("_", " ");
            //            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(HeadingAccentStorage);

            //            var headingAccent = (from accent in storage.GetObjectCollection<MenuPresentationModel.MenuStyles.HeadingAccent>()
            //                                 where accent.Name == accentName
            //                                 select accent).FirstOrDefault();

            //            if (headingAccent == null)
            //            {
            //                headingAccent = new HeadingAccent();
            //                HeadingAccentStorage.CommitTransientObjectState(headingAccent);

            //            }
            //            string accentImagesFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
            //            if (!System.IO.Directory.Exists(accentImagesFolder))
            //                System.IO.Directory.CreateDirectory(accentImagesFolder);
            //            accentImagesFolder += "\\DontWaitWater\\AccentImages";
            //            if (!System.IO.Directory.Exists(accentImagesFolder))
            //                System.IO.Directory.CreateDirectory(accentImagesFolder);
            //            CopyHeadingAccent(headingStyle, headingAccent, accentImagesFolder);
            //            stateTransition.Consistent = true;
            //        }



            //    }
            //}
            //return;

            if (cssStyleSheet != null)
            {
                priceOptions = GetPriceStyleData(cssStyleSheet);
                menuItemOptions = GetMenuItemStyleData(cssStyleSheet);
                layoutOptions = GetLayoutStyleData(cssStyleSheet);
                GetPageStyleData(pageStyle, cssStyleSheet);

                GetHeadingStyleDataFromStyleSheet(cssStyleSheet, titleHeadingStyle, "title-heading", pageStyle);
                GetHeadingStyleDataFromStyleSheet(cssStyleSheet, headingStyle, "heading", pageStyle);
                SetFont(headingStyle.FontStyle);
                GetHeadingStyleDataFromStyleSheet(cssStyleSheet, subHeadingStyle, "small-heading", pageStyle);
                GetHeadingStyleDataFromStyleSheet(cssStyleSheet, altFontHeadingStyle, "alt-font-heading", pageStyle);
            }
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    var styleSheet = MenuPresentationModel.MenuStyles.StyleSheet.GetStyleSheet(styleName);
                    if (styleSheet == null)
                    {
                        styleSheet = MenuPresentationModel.MenuStyles.StyleSheet.NewStyleSheet(styleName);

                        //stateTransition.Consistent = true;
                    }
                    IStyleRule styleRule = null;
                    MenuPresentationModel.MenuStyles.PageStyle pageStyleRule = null;
                    if (!styleSheet.Styles.TryGetValue("page", out styleRule))
                    {
                        pageStyleRule = new MenuPresentationModel.MenuStyles.PageStyle();
                        styleRule = pageStyleRule;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(pageStyleRule);
                        pageStyleRule.Name = "page";
                        styleSheet.AddStyle(pageStyleRule);
                    }
                    else
                        pageStyleRule = styleRule as MenuPresentationModel.MenuStyles.PageStyle;
                    Margin pageMargin = new Margin();
                    pageMargin.MarginTop = pageStyle.Margin.MarginTop;
                    pageMargin.MarginLeft = pageStyle.Margin.MarginLeft;
                    pageMargin.MarginRight = pageStyle.Margin.MarginRight;
                    pageMargin.MarginBottom = pageStyle.Margin.MarginBottom;
                    pageStyleRule.Margin = pageMargin;
                    pageStyleRule.PageHeight = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), pageStyle.PageHeigthInch.ToString(new System.Globalization.CultureInfo(1033)) + "in");
                    pageStyleRule.PageWidth = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), pageStyle.PageWidthInch.ToString(new System.Globalization.CultureInfo(1033)) + "in");

                    if (!string.IsNullOrWhiteSpace(pageStyle.BackgroundUri))
                    {
                        string pageBackgroundsFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                        if (!System.IO.Directory.Exists(pageBackgroundsFolder))
                            System.IO.Directory.CreateDirectory(pageBackgroundsFolder);
                        pageBackgroundsFolder += "\\DontWaitWater\\PageBackgrounds";
                        if (!System.IO.Directory.Exists(pageBackgroundsFolder))
                            System.IO.Directory.CreateDirectory(pageBackgroundsFolder);



                        MenuPresentationModel.MenuStyles.Resource resource = new MenuPresentationModel.MenuStyles.Resource();
                        resource.Uri = pageStyle.DownloadBackground(pageBackgroundsFolder);
                        resource.Name = pageStyle.BackgroundName;

                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(BackgroundImagesStorage);
                        var Background = (from pageImage in storage.GetObjectCollection<PageImage>()
                                          where pageImage.Name == pageStyle.BackgroundName
                                          select pageImage).FirstOrDefault();

                        storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(pageStyleRule));

                        var styleSheetBackground = (from pageImage in storage.GetObjectCollection<PageImage>()
                                                    where pageImage.Name == pageStyle.BackgroundName
                                                    select pageImage).FirstOrDefault();
                        if (styleSheetBackground == null && Background != null)
                        {
                            styleSheetBackground = new PageImage(Background);
                            ObjectStorage.GetStorageOfObject(pageStyleRule).CommitTransientObjectState(styleSheetBackground);
                        }

                        pageStyleRule.Background = styleSheetBackground;
                        pageStyleRule.BackgroundMargin = pageStyle.BackgroundMargin;

                    }
                    if (!string.IsNullOrWhiteSpace(pageStyle.BorderUri))
                    {
                        string pageBordersFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                        if (!System.IO.Directory.Exists(pageBordersFolder))
                            System.IO.Directory.CreateDirectory(pageBordersFolder);
                        pageBordersFolder += "\\DontWaitWater\\PageBorders";
                        if (!System.IO.Directory.Exists(pageBordersFolder))
                            System.IO.Directory.CreateDirectory(pageBordersFolder);

                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(BordersStorage);
                        var border = (from pageImage in storage.GetObjectCollection<PageImage>()
                                      where pageImage.Name == pageStyle.BorderName
                                      select pageImage).FirstOrDefault();

                        storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(pageStyleRule));

                        var styleSheetBorder = (from pageImage in storage.GetObjectCollection<PageImage>()
                                                where pageImage.Name == pageStyle.BorderName
                                                select pageImage).FirstOrDefault();
                        if (styleSheetBorder == null && border != null)
                        {
                            styleSheetBorder = new PageImage(border);
                            ObjectStorage.GetStorageOfObject(pageStyleRule).CommitTransientObjectState(styleSheetBorder);
                        }
                        //MenuPresentationModel.MenuStyles.Resource resource = new MenuPresentationModel.MenuStyles.Resource();
                        //resource.Uri = pageStyle.DownloadBorder(pageBordersFolder);
                        //resource.Name = pageStyle.BorderName;

                        pageStyleRule.Border = styleSheetBorder;
                        pageStyleRule.BorderMargin = pageStyle.BorderMargin;
                    }

                    MenuPresentationModel.MenuStyles.HeadingStyle titleHeadingStyleRule = null;
                    if (!styleSheet.Styles.TryGetValue("title-heading", out styleRule))
                    {
                        titleHeadingStyleRule = new MenuPresentationModel.MenuStyles.HeadingStyle();
                        styleRule = titleHeadingStyleRule;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(titleHeadingStyleRule);
                        titleHeadingStyleRule.Name = "title-heading";
                        styleSheet.AddStyle(titleHeadingStyleRule);
                    }
                    else
                        titleHeadingStyleRule = styleRule as MenuPresentationModel.MenuStyles.HeadingStyle;

                    CopyHeadingStyleProperties(titleHeadingStyle, titleHeadingStyleRule);


                    MenuPresentationModel.MenuStyles.HeadingStyle headingStyleRule = null;
                    if (!styleSheet.Styles.TryGetValue("heading", out styleRule))
                    {
                        headingStyleRule = new MenuPresentationModel.MenuStyles.HeadingStyle();
                        styleRule = headingStyleRule;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(headingStyleRule);
                        headingStyleRule.Name = "heading";
                        styleSheet.AddStyle(headingStyleRule);
                    }
                    else
                        headingStyleRule = styleRule as MenuPresentationModel.MenuStyles.HeadingStyle;

                    CopyHeadingStyleProperties(headingStyle, headingStyleRule);

                    MenuPresentationModel.MenuStyles.HeadingStyle subHeadingStyleRule = null;
                    if (!styleSheet.Styles.TryGetValue("small-heading", out styleRule))
                    {
                        subHeadingStyleRule = new MenuPresentationModel.MenuStyles.HeadingStyle();
                        styleRule = subHeadingStyleRule;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(subHeadingStyleRule);
                        subHeadingStyleRule.Name = "small-heading";
                        styleSheet.AddStyle(subHeadingStyleRule);
                    }
                    else
                        subHeadingStyleRule = styleRule as MenuPresentationModel.MenuStyles.HeadingStyle;

                    CopyHeadingStyleProperties(subHeadingStyle, subHeadingStyleRule);


                    MenuPresentationModel.MenuStyles.HeadingStyle altFontHeadingStyleRule = null;
                    if (!styleSheet.Styles.TryGetValue("alt-font-heading", out styleRule))
                    {
                        altFontHeadingStyleRule = new MenuPresentationModel.MenuStyles.HeadingStyle();
                        styleRule = altFontHeadingStyleRule;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(altFontHeadingStyleRule);
                        altFontHeadingStyleRule.Name = "alt-font-heading";
                        styleSheet.AddStyle(altFontHeadingStyleRule);
                    }
                    else
                        altFontHeadingStyleRule = styleRule as MenuPresentationModel.MenuStyles.HeadingStyle;

                    MenuPresentationModel.MenuStyles.MenuItemStyle menuItemStyleRule = null;
                    if (!styleSheet.Styles.TryGetValue("menu-item", out styleRule))
                    {
                        menuItemStyleRule = new MenuPresentationModel.MenuStyles.MenuItemStyle();
                        styleRule = menuItemStyleRule;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(menuItemStyleRule);
                        menuItemStyleRule.Name = "menu-item";
                        styleSheet.AddStyle(menuItemStyleRule);
                    }
                    else
                        menuItemStyleRule = styleRule as MenuPresentationModel.MenuStyles.MenuItemStyle;

                    CopyMenuItemStyleProperties(menuItemOptions, menuItemStyleRule);



                    MenuPresentationModel.MenuStyles.LayoutStyle layoutStyle = null;
                    if (!styleSheet.Styles.TryGetValue("layout", out styleRule))
                    {
                        layoutStyle = new LayoutStyle();
                        styleRule = layoutStyle = new LayoutStyle();
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(layoutStyle);
                        layoutStyle.Name = "layout";
                        styleSheet.AddStyle(layoutStyle);
                    }
                    else
                        layoutStyle = styleRule as LayoutStyle;
                    CopyLayoutStyleProperties(layoutOptions, layoutStyle);

                    //"price-options"

                    PriceStyle priceStyle = null;
                    if (!styleSheet.Styles.TryGetValue("price-options", out styleRule))
                    {
                        priceStyle = new PriceStyle();
                        styleRule = priceStyle;
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(styleSheet).CommitTransientObjectState(priceStyle);
                        priceStyle.Name = "price-options";
                        styleSheet.AddStyle(priceStyle);
                    }
                    else
                        priceStyle = styleRule as PriceStyle;

                    CopyPriceStyleProperties(priceOptions, priceStyle);




                    stateTransition.Consistent = true;
                }
            }
        }

        private void DownloadAccents()
        {
            string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
            if (!System.IO.Directory.Exists(appDataPath))
                System.IO.Directory.CreateDirectory(appDataPath);
            appDataPath += "\\DontWaitWater";

            XDocument doc = XDocument.Load(@"E:\MyWindowProfileData\Documents\temp\Accent.xml");
            if (!System.IO.Directory.Exists(appDataPath + @"\dividers"))
                System.IO.Directory.CreateDirectory(appDataPath + @"\dividers");

            foreach (XElement element in doc.Root.Elements("option"))
            {
                string uri = element.Attribute("image").Value;
                if (uri.IndexOf(@"/dividers/") != -1)
                {
                    string accentImageName = element.Value;
                    WebClient client = new WebClient();
                    client.DownloadFile(uri, appDataPath + @"\dividers\" + accentImageName + ".svg");
                }
            }

            if (!System.IO.Directory.Exists(appDataPath + @"\boxes"))
                System.IO.Directory.CreateDirectory(appDataPath + @"\boxes");
            foreach (XElement element in doc.Root.Elements("option"))
            {
                string uri = element.Attribute("image").Value;
                if (uri.IndexOf(@"/boxes/") != -1)
                {
                    string accentImageName = element.Value;
                    WebClient client = new WebClient();
                    client.DownloadFile(uri, appDataPath + @"\boxes\" + accentImageName + ".svg");
                }
            }
            if (!System.IO.Directory.Exists(appDataPath + @"\ornaments"))
                System.IO.Directory.CreateDirectory(appDataPath + @"\ornaments");


            foreach (XElement element in doc.Root.Elements("option"))
            {
                string uri = element.Attribute("image").Value;
                if (uri.IndexOf(@"/ornaments/") != -1)
                {
                    string accentImageName = element.Value;
                    WebClient client = new WebClient();
                    client.DownloadFile(uri, appDataPath + @"\ornaments\" + accentImageName + ".svg");
                }
            }
            if (!System.IO.Directory.Exists(appDataPath + @"\text"))
                System.IO.Directory.CreateDirectory(appDataPath + @"\text");


            foreach (XElement element in doc.Root.Elements("option"))
            {
                string uri = element.Attribute("image").Value;
                if (uri.IndexOf(@"/text/") != -1)
                {
                    string accentImageName = element.Value;
                    WebClient client = new WebClient();
                    client.DownloadFile(uri, appDataPath + @"\text\" + accentImageName + ".svg");
                }
            }

            //"dividers"
            //"boxes"

            //"ornaments";
            //"text"
        }

        private void CopyLayoutStyleProperties(CSSLayoutOptions layoutOptions, LayoutStyle layoutStyle)
        {
            layoutStyle.DescLeftIndent = layoutOptions.desc_left_indent;
            layoutStyle.DescRightIndent = layoutOptions.desc_right_indent;
            layoutStyle.DescSeparator = layoutOptions.desc_separator;
            if (layoutStyle.DescSeparator == "none")
                layoutStyle.DescSeparator = "";
            layoutStyle.ExtrasLeftIndent = layoutOptions.extras_left_indent;
            layoutStyle.ExtrasSeparator = layoutOptions.extras_separator;
            if (layoutStyle.ExtrasSeparator == "none")
                layoutStyle.ExtrasSeparator = "";

            layoutStyle.LineBetweenColumns = layoutOptions.line_between_columns=="yes";
            layoutStyle.LineSpacing = layoutOptions.line_spacing;

            //<select name = "line-type" style="width:53px">
            //				  			<option value = "single" selected="">|</option>
            //				  			<option value = "double" >||</ option >
            //                          <option value = "dashed">¦</option>
            //				  			<option value = "dotted" >⋮</option> <!-- 11.0.58 -->
            //				  			<option value = "arrowend2" >↕</option> <!-- 11.0.58 -->
            //				  			<option value = "arrowend" >↕▴</option> 
            //				  			<option value = "colbox" >⊡</option>
            //							</select>
            switch(layoutOptions.line_type)
            {
                case "single":
                    layoutStyle.SeparationLineType = LineType.Single;
                    break;
                case "double":
                    layoutStyle.SeparationLineType = LineType.Double;
                    break;
                case "dashed":
                    layoutStyle.SeparationLineType = LineType.Dashed;
                    break;
                case "dotted":
                    layoutStyle.SeparationLineType = LineType.Dotted;
                    break;
                case "arrowend2":
                    layoutStyle.SeparationLineType = LineType.Arrowend;
                    break;
                case "arrowend":
                    layoutStyle.SeparationLineType = LineType.ArrowendEx;
                    break;
                case "colbox":
                    layoutStyle.SeparationLineType = LineType.ColumnBox;
                    break;

                default:
                    layoutStyle.SeparationLineType = LineType.Single;
                    break;
            }
            layoutStyle.NameIndent = layoutOptions.name_indent;
            layoutStyle.SpaceBetweenColumns = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), layoutOptions.space_between_columns.ToString(new System.Globalization.CultureInfo(1033)) + "in"); ;

        }

        private CSSLayoutOptions GetLayoutStyleData(ExCSS.StyleSheet styleSheet)
        {
            var layOutStyleRule = (from styleRule in styleSheet.StyleRules
                                   where styleRule.Value == ".style"
                                   select styleRule).FirstOrDefault();
            return new CSSLayoutOptions(layOutStyleRule, Web);
        }

        /// <MetaDataID>{0dbf884a-796f-4b28-b213-82b3afda9a96}</MetaDataID>
        private static void CopyPriceStyleProperties(CSSPriceOptions priceOptions, PriceStyle priceStyle)
        {
            if (priceOptions.currency == "none")
                priceStyle.DisplayCurrencySymbol = false;
            else
                priceStyle.DisplayCurrencySymbol = true;
            if (priceOptions.layout == "with_desc")
                priceStyle.Layout = PriceLayout.WithDescription;

            if (priceOptions.layout == "follow_desc")
                priceStyle.Layout = PriceLayout.FollowDescription;

            if (priceOptions.layout == "with_name")
                priceStyle.Layout = PriceLayout.WithName;

            if (priceOptions.layout == "normal")
                priceStyle.Layout = PriceLayout.Normal;

            if (priceOptions.priceleader == "none")
                priceStyle.PriceLeader = null;
            else
                priceStyle.PriceLeader = priceOptions.priceleader;
            priceStyle.DotsMatchNameColor = priceOptions.dotsmatchnamecolor == "yes";
            priceStyle.DisplayCurrencySymbol = priceOptions.currency != "none";
            priceStyle.Font = GetFontData(priceOptions.FontStyle);
            int betweenDotsSpace = 0;
            int.TryParse(priceOptions.dotspacebetween, out betweenDotsSpace);
            priceStyle.BetweenDotsSpace = betweenDotsSpace;

        }

        /// <MetaDataID>{1dcfea5d-b959-439d-a2c8-42171efb5f1e}</MetaDataID>
        private void CopyMenuItemStyleProperties(CSSMenuItemOptions menuItemOptions, MenuItemStyle menuItemStyleRule)
        {
            menuItemStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Left;
            if (menuItemOptions.nameAlign == "middle")
                menuItemStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Center;
            if (menuItemOptions.nameAlign == "right")
                menuItemStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Right;
            if (menuItemOptions.nameAlign == "end")
                menuItemStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Right;

            IHTMLElement lasttspan = null;
            foreach (IHTMLElement tspan in (Web.Document as mshtml.IHTMLDocument3).getElementsByTagName("tspan"))
            {
                if (lasttspan != null && lasttspan.getAttribute("className") as string == "name" && tspan.getAttribute("className") as string == "description")
                {
                    IHTMLElement text = lasttspan.parentElement;
                    XDocument textDoc = XDocument.Parse(text.outerHTML);
                    string text_anchor = textDoc.Root.Attribute("text-anchor").Value;
                    if (text_anchor == "middle")
                        menuItemStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Center;
                    if (text_anchor == "start")
                        menuItemStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Left;
                    if (text_anchor == "end")
                        menuItemStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Right;


                    break;
                }
                lasttspan = tspan;
            }


            menuItemStyleRule.Font = GetFontData(menuItemOptions.FontStyle);
            menuItemStyleRule.DescriptionFont = GetFontData(menuItemOptions.DescriptionFontStyle);
            menuItemStyleRule.ExtrasFont = GetFontData(menuItemOptions.ExtrasFontStyle);
            menuItemStyleRule.BeforeSpacing = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), menuItemOptions.spaceBefore + "in");
            menuItemStyleRule.AfterSpacing = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), menuItemOptions.spaceAfter + "in");
            menuItemStyleRule.Indent = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), menuItemOptions.indent + "in");
            menuItemStyleRule.NewLineForDescription = menuItemOptions.NewLineForDescription;
        }

        /// <MetaDataID>{b1e62480-55be-4ce3-ac8e-a8713121544b}</MetaDataID>
        private static void CopyHeadingStyleProperties(CSSHeadingStyle headingStyle, MenuPresentationModel.MenuStyles.HeadingStyle headingStyleRule)
        {
            headingStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Left;
            if (headingStyle.HeadingAlign == "middle")
                headingStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Center;
            if (headingStyle.HeadingAlign == "right")
                headingStyleRule.Alignment = MenuPresentationModel.MenuStyles.Alignment.Right;

            headingStyleRule.OverlineImage = headingStyle.OverlineImage;
            headingStyleRule.UnderlineImage = headingStyle.UnderlineImage;
            if (headingStyleRule.Accent != null)
                headingStyleRule.Accent.Height = headingStyle.AccentImageHeight;

            headingStyleRule.BeforeSpacing = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), headingStyle.spaceBefore + "in");
            headingStyleRule.AfterSpacing = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), headingStyle.spaceAfter + "in");

            FontData fontData = GetFontData(headingStyle.FontStyle);

            headingStyleRule.Font = fontData;
            if (headingStyle.AccentImages.Count > 0)
            {
                string accentImagesFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                if (!System.IO.Directory.Exists(accentImagesFolder))
                    System.IO.Directory.CreateDirectory(accentImagesFolder);
                accentImagesFolder += "\\DontWaitWater\\AccentImages";
                if (!System.IO.Directory.Exists(accentImagesFolder))
                    System.IO.Directory.CreateDirectory(accentImagesFolder);

                if (headingStyleRule.Accent == null)
                {
                    MenuPresentationModel.MenuStyles.Accent headingAccent = new MenuPresentationModel.MenuStyles.Accent();
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(headingStyleRule).CommitTransientObjectState(headingAccent);
                    headingStyleRule.Accent = headingAccent;
                }
                CopyHeadingAccent(headingStyle, headingStyleRule.Accent, accentImagesFolder);
            }
        }

        private static void CopyHeadingAccent(CSSHeadingStyle headingStyle, MenuPresentationModel.MenuStyles.IAccent headingAccent, string accentImagesFolder)
        {
            if (headingStyle.AccentImages.Count == 2)
                headingAccent.DoubleImage = true;

            headingAccent.FullRowImage = headingStyle.HorizontalStrech;

            headingAccent.MarginLeft = headingStyle.AccentImageMarginLeft;
            headingAccent.MarginTop = headingStyle.AccentImageMarginTop;
            headingAccent.MarginRight = headingStyle.AccentImageMarginRight;
            headingAccent.MarginBottom = headingStyle.AccentImageMarginBottom;
            headingAccent.OverlineImage = headingStyle.OverlineImage;
            headingAccent.UnderlineImage = headingStyle.UnderlineImage;
            headingAccent.TextBackgroundImage = headingStyle.TextBackgroundImage;
            headingAccent.Name = headingStyle.AccentName.Replace("_", " ");
            headingAccent.AccentColor = headingStyle.AccentColor;
            headingAccent.Height = headingStyle.AccentImageHeight;

            int i = 0;
            foreach (var image in headingStyle.DownloadAccentImages(accentImagesFolder))
            {
                MenuPresentationModel.MenuStyles.MenuImage accenImage = null;

                string color = GetAccent(image.Image);

                if (headingAccent.AccentImages.Count > i)
                {
                    headingAccent.AccentImages[i].Image = image.Image;
                    headingAccent.AccentImages[i].Width = image.Width;
                    headingAccent.AccentImages[i].Height = image.Height;
                }
                else
                {
                    accenImage = new MenuPresentationModel.MenuStyles.MenuImage(image.Image, image.Width, image.Height);
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(headingAccent).CommitTransientObjectState(accenImage);
                    headingAccent.AddAccentImage(accenImage);
                }
                i++;
            }
            while (headingAccent.AccentImages.Count > i + 1)
                headingAccent.DeleteAccentImage(headingAccent.AccentImages[headingAccent.AccentImages.Count - 1]);

        }

        private static string GetAccent(Resource image)
        {
            if (string.IsNullOrWhiteSpace(image.Uri))
                return null;
            if (!System.IO.File.Exists(image.Uri))
                return null;
            SharpVectors.Dom.Svg.SvgDocument doc = new SharpVectors.Dom.Svg.SvgDocument(null);
            doc.Load(image.Uri);

            for (ulong i = 0; i != doc.StyleSheets.Length; i++)
            {
                var staleSheet = doc.StyleSheets[i];
                for (ulong k = 0; k != (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules.Length; k++)
                {
                    var cssStyleRule = (staleSheet as SharpVectors.Dom.Css.CssStyleSheet).CssRules[k] as SharpVectors.Dom.Css.CssStyleRule;
                    if (cssStyleRule.SelectorText == ".st0")
                    {


                        try
                        {
                            string color = cssStyleRule.Style.GetPropertyValue("fill");
                            var conv = ColorConverter.ConvertFromString(color);
                            return color;
                        }
                        catch (Exception error)
                        {
                            try
                            {
                                string color = cssStyleRule.Style.GetPropertyValue("stroke");
                                object conv = ColorConverter.ConvertFromString(color);
                                return color;
                            }
                            catch (Exception innererror)
                            {
                            }
                        }


                    }
                }
            }
            return null;

        }

        /// <MetaDataID>{5618d4e3-2f69-4af3-8581-2ced675d6a87}</MetaDataID>
        private static FontData GetFontData(CSSFontStyle cssFont)
        {
            FontData fontData = new FontData();
            WpfFontStyle wpfFont = WpfFontStyle.GetWpfFont(cssFont);

            fontData.FontFamilyName = wpfFont.fontFamily.FamilyNames.ToArray()[0].Value;
            fontData.FontSize = wpfFont.fontSize;
            fontData.FontSpacing = wpfFont.fontSpacing;
            fontData.FontStyle = wpfFont.fontStyle.ToString();
            fontData.FontWeight = wpfFont.fontWeight.ToString();
            fontData.Foreground = wpfFont.fontForground.ToString();

            if (cssFont.shadow == "yes")
            {
                fontData.ShadowXOffset = (float)cssFont.shadowx.Value;
                fontData.ShadowYOffset = (float)cssFont.shadowy.Value;
                fontData.BlurRadius = (float)cssFont.shadowblur.Value;
                fontData.ShadowColor = cssFont.shadowcolor.ToString();
                fontData.Shadow = true;
            }
            if (cssFont.stroke == "yes")
            {
                fontData.StrokeFill = cssFont.strokeColor.ToString();
                fontData.StrokeThickness = (float)cssFont.strokeThickness.Value;
            }

            if (cssFont.stroke != "none")
                fontData.Stroke = true;
            if (cssFont.allCaps == "yes")
                fontData.AllCaps = true;
            return fontData;
        }

        /// <MetaDataID>{03e945b8-d569-4e74-87e7-6bf2044c54e8}</MetaDataID>
        private ExCSS.StyleSheet GetStyleSheet(out string styleName)
        {
            styleName = "";
            foreach (IHTMLElement linkElement in (Web.Document as mshtml.IHTMLDocument3).getElementsByTagName("link"))
            {
                //< link href = "styles/Zydeco_Kitchen.css" rel = "stylesheet" type = "text/css" title = "origCSS" >
                string tref = linkElement.getAttribute("href") as string;
                string linkTitle = linkElement.getAttribute("title") as string;
                if (linkTitle == "origCSS")
                {
                    styleName = tref.Substring(tref.LastIndexOf("/") + 1).Replace(".css", "").Replace("_", " ");
                    try
                    {
                        //menuborder
                        //background
                        string cssUri = "https://imenupro.com/SVG/" + tref;


                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(cssUri);
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadToEnd();
                        stream.Close();
                        ExCSS.Parser parser = new ExCSS.Parser();

                        return parser.Parse(content.Replace(":&#", ":p_p"));
                    }
                    catch (Exception error)
                    {
                    }
                }
            }
            return null;
        }

        /// <MetaDataID>{c55576cd-45d0-4ad1-ba2f-e806c544cbad}</MetaDataID>
        private static void GetPageMargin(CSSPageStyle pageStyle, ExCSS.StyleSheet styleSheet)
        {
            var pageMargin = (from styleRule in styleSheet.StyleRules
                              where styleRule.Value == ".page-margins"
                              select styleRule).FirstOrDefault();

            string marginTerm = (from declaration in pageMargin.Declarations
                                 where declaration.Name == "margins"
                                 select declaration).FirstOrDefault().Term.ToString();


            string marginValue = marginTerm.Substring(0, marginTerm.IndexOf(","));

            pageStyle.Margin.MarginTop = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), marginValue.ToString(new System.Globalization.CultureInfo(1033)) + "in");

            marginTerm = marginTerm.Substring(marginTerm.IndexOf(",") + 1);

            marginValue = marginTerm.Substring(0, marginTerm.IndexOf(","));

            //pageStyle.Margin.MarginRight = double.Parse(marginValue, new System.Globalization.CultureInfo(1033));
            pageStyle.Margin.MarginRight = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), marginValue.ToString(new System.Globalization.CultureInfo(1033)) + "in");

            marginTerm = marginTerm.Substring(marginTerm.IndexOf(",") + 1);

            marginValue = marginTerm.Substring(0, marginTerm.IndexOf(","));

            //pageStyle.Margin.MarginBottom = double.Parse(marginValue, new System.Globalization.CultureInfo(1033));
            pageStyle.Margin.MarginBottom = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), marginValue.ToString(new System.Globalization.CultureInfo(1033)) + "in");

            marginValue = marginTerm.Substring(marginTerm.IndexOf(",") + 1);

            //pageStyle.Margin.MarginLeft = double.Parse(marginValue, new System.Globalization.CultureInfo(1033));
            pageStyle.Margin.MarginLeft = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), marginValue.ToString(new System.Globalization.CultureInfo(1033)) + "in");

        }

        /// <MetaDataID>{d346147a-3bbc-4c08-b407-62addeb2a919}</MetaDataID>
        private CSSPriceOptions GetPriceStyleData(ExCSS.StyleSheet styleSheet)
        {
            var priceOptionStyleRule = (from styleRule in styleSheet.StyleRules
                                        where styleRule.Value == ".price-options"
                                        select styleRule).FirstOrDefault();

            var priceStyleRule = (from styleRule in styleSheet.StyleRules
                                  where styleRule.Value == ".price"
                                  select styleRule).FirstOrDefault();


            CSSPriceOptions priceOptions = new CSSPriceOptions(priceOptionStyleRule);

            CSSFontStyle thefontStyle = priceOptions.FontStyle;
            GetCSSFontStyle(priceStyleRule, thefontStyle);

            return priceOptions;

        }

        /// <MetaDataID>{aaecd832-d888-4505-a77d-65d996a371fe}</MetaDataID>
        private CSSMenuItemOptions GetMenuItemStyleData(ExCSS.StyleSheet styleSheet)
        {
            var menuItemStyleRule = (from styleRule in styleSheet.StyleRules
                                     where styleRule.Value == ".name"
                                     select styleRule).FirstOrDefault();
            var descriptionStyleRule = (from styleRule in styleSheet.StyleRules
                                        where styleRule.Value == ".description"
                                        select styleRule).FirstOrDefault();

            var extrasStyleRule = (from styleRule in styleSheet.StyleRules
                                   where styleRule.Value == ".extras"
                                   select styleRule).FirstOrDefault();


            return new CSSMenuItemOptions(menuItemStyleRule, descriptionStyleRule, extrasStyleRule, Web);
        }
        /// <MetaDataID>{9b7a7128-c66c-4746-886a-6c1bd3c5ff30}</MetaDataID>
        private void GetPageStyleData(CSSPageStyle pageStyle, ExCSS.StyleSheet styleSheet)
        {
            IHTMLElement textCanvas = (Web.Document as mshtml.IHTMLDocument3).getElementById("textcanvas");
            IHTMLElement border = (Web.Document as mshtml.IHTMLDocument3).getElementById("menuborder");

            IHTMLElement pageHeightWidth = (Web.Document as mshtml.IHTMLDocument3).getElementById("faux-white-background");

            pageStyle.PageWidthInch = double.Parse(XDocument.Parse(pageHeightWidth.outerHTML).Root.Attribute("width").Value.Replace("in", ""), new System.Globalization.CultureInfo(1033));
            pageStyle.PageHeigthInch = double.Parse(XDocument.Parse(pageHeightWidth.outerHTML).Root.Attribute("height").Value.Replace("in", ""), new System.Globalization.CultureInfo(1033));

            if (((Web.Document as mshtml.IHTMLDocument3).getElementById("background-image") as IHTMLElement) != null)
            {
                string backgroundimage = ((Web.Document as mshtml.IHTMLDocument3).getElementById("background-image") as IHTMLElement).outerHTML;


                XElement imageRectElement = XDocument.Parse(((Web.Document as mshtml.IHTMLDocument3).getElementById("background-rect") as IHTMLElement).outerHTML).Root;
                XElement imageElement = XDocument.Parse(((Web.Document as mshtml.IHTMLDocument3).getElementById("background-image") as IHTMLElement).outerHTML).Root;


                double rectX = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageRectElement.Attribute("x").Value);
                double rectY = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageRectElement.Attribute("y").Value);
                double rectWidth = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageRectElement.Attribute("width").Value);
                double rectHeight = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageRectElement.Attribute("height").Value);

                double imageX = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("x").Value);
                double imageY = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("y").Value);
                double imageWidth = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("width").Value);
                double imageHeight = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("height").Value);

                Margin backgroundMargin = new Margin();
                backgroundMargin.MarginTop = imageY - rectY;
                backgroundMargin.MarginLeft = imageX - rectX;
                backgroundMargin.MarginBottom = rectHeight - (backgroundMargin.MarginTop + imageHeight);
                backgroundMargin.MarginRight = rectWidth - (backgroundMargin.MarginLeft + imageWidth);


                pageStyle.BackgroundMargin = backgroundMargin;

                pageStyle.BackgroundUri = XDocument.Parse(backgroundimage).Root.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink")).Value;
                pageStyle.BackgroundFileName = pageStyle.BackgroundUri.Substring(pageStyle.BackgroundUri.LastIndexOf("/") + 1);
                pageStyle.BackgroundName = pageStyle.BackgroundFileName.Substring(0, pageStyle.BackgroundFileName.IndexOf(".")).Replace("_", " ");

                //try
                //{
                //    WebClient client = new WebClient();
                //pageStyle.BackgroundStream = client.OpenRead(backgroundUri);
                //}
                //catch (Exception error)
                //{
                //}
            }
            String bordersvgContent = null;
            string borderHtml = border.innerHTML;
            if (!string.IsNullOrWhiteSpace(borderHtml))
            {
                pageStyle.BorderUri = XDocument.Parse(borderHtml).Root.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink")).Value;

                double x = 0;
                double y = 0;
                double borderWidth = 0;
                double borderHeight = 0;

                if (XDocument.Parse(borderHtml).Root.Attribute("x") != null)
                    double.TryParse( XDocument.Parse(borderHtml).Root.Attribute("x").Value,System.Globalization.NumberStyles.Float,System.Globalization.CultureInfo.GetCultureInfo(1033), out x);

                if (XDocument.Parse(borderHtml).Root.Attribute("y") != null)
                    double.TryParse(XDocument.Parse(borderHtml).Root.Attribute("y").Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.GetCultureInfo(1033), out y);

                if (XDocument.Parse(borderHtml).Root.Attribute("width") != null)
                    double.TryParse(XDocument.Parse(borderHtml).Root.Attribute("width").Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.GetCultureInfo(1033), out borderWidth);

                if (XDocument.Parse(borderHtml).Root.Attribute("height") != null)
                    double.TryParse(XDocument.Parse(borderHtml).Root.Attribute("height").Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.GetCultureInfo(1033), out borderHeight);


                double width = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), pageStyle.PageWidthInch.ToString(new System.Globalization.CultureInfo(1033)) + "in");
                double height = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), pageStyle.PageHeigthInch.ToString(new System.Globalization.CultureInfo(1033)) + "in");

                Margin borderMargin = new UIBaseEx.Margin() { MarginTop = y, MarginLeft = x, MarginBottom = height - borderHeight - y, MarginRight = width - borderWidth - x };
                pageStyle.BorderMargin = borderMargin;

                // try
                // {
                //     WebClient client = new WebClient();
                //     pageStyle.BorderStream = client.OpenRead(borderUri);
                //     StreamReader reader = new StreamReader(pageStyle.BorderStream);
                //     bordersvgContent = reader.ReadToEnd();
                //     reader.Close();
                // }
                // catch (Exception error)
                // {
                //}
            }

            GetPageMargin(pageStyle, styleSheet);

            var pageBorderStyleRule = (from styleRule in styleSheet.StyleRules
                                       where styleRule.Value == ".border"
                                       select styleRule).FirstOrDefault();

            var term = (from declaration in pageBorderStyleRule.Declarations
                        where declaration.Name == "image"
                        select declaration).FirstOrDefault().Term;
            if (term != null && term.ToString() != "none")
            {
                pageStyle.BorderFileName = term.ToString();
                pageStyle.BorderName = pageStyle.BorderFileName.Substring(0, pageStyle.BorderFileName.IndexOf(".")).Replace("_", " ");
            }
        }

        /// <MetaDataID>{f498c42f-23b9-4d6a-8c68-276e37476b60}</MetaDataID>
        private void GetHeadingStyleDataFromStyleSheet(ExCSS.StyleSheet styleSheet, CSSHeadingStyle headingStyles, string headingClass, CSSPageStyle pageStyle)
        {
            var cssHeadingStyle = (from styleRule in styleSheet.StyleRules
                                   where styleRule.Value == string.Format(".{0}", headingClass)
                                   select styleRule).FirstOrDefault();

            CSSFontStyle thefontStyle = headingStyles.FontStyle;
            GetCSSFontStyle(cssHeadingStyle, thefontStyle);

            GetCSSHeadingStyle(cssHeadingStyle, headingStyles);


            var titleHeadingAccentStyle = (from styleRule in styleSheet.StyleRules
                                           where styleRule.Value == string.Format(".{0}-accent", headingClass)
                                           select styleRule).FirstOrDefault();

            GetCSSHeadingAccentStyle(titleHeadingAccentStyle, headingStyles);




            foreach (IHTMLElement tspan in (Web.Document as mshtml.IHTMLDocument3).getElementsByTagName("tspan"))
            {
                if (tspan.getAttribute("className") as string == headingClass /*"title-heading"*/ /*"heading"*/)
                {
                    IHTMLElement gElement = tspan.parentElement.parentElement;
                    XDocument document = XDocument.Parse(gElement.outerHTML);
                    if (document.Root.Attribute("id").Value.IndexOf("headitem_") == 0)
                    {
                        XElement textElement = document.Root.Elements().Where(x => x.Name.LocalName == "text").FirstOrDefault();
                        string headingText = textElement.Elements().Where(x => x.Name.LocalName == "tspan").FirstOrDefault().Value;

                        WpfFontStyle wpfFont = WpfFontStyle.GetWpfFont(headingStyles.FontStyle);
                        string fs = "23pt";
                        double fSize = (double)(new FontSizeConverter().ConvertFrom(fs));
                        fSize = wpfFont.fontSize;
                        Size size = WpfFontStyle.MeasureText(headingText, wpfFont.fontFamily, wpfFont.fontStyle, wpfFont.fontWeight, FontStretches.Normal, fSize, wpfFont.fontSpacing);
                        double fontBaseLine = WpfFontStyle.GetFontBaseLine(headingText, wpfFont.fontFamily, wpfFont.fontStyle, wpfFont.fontWeight, FontStretches.Normal, fSize);
                        double fontBottom = WpfFontStyle.GetFontBottom(headingText, wpfFont.fontFamily, wpfFont.fontStyle, wpfFont.fontWeight, FontStretches.Normal, fSize);
                        double textYpos = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), textElement.Attribute("y").Value);
                        double textXpos = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), textElement.Attribute("x").Value);
                        double textYOffset = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), textElement.Elements().Where(x => x.Name.LocalName == "tspan").FirstOrDefault().Attribute("dy").Value);
                        double textBottom = textYpos + textYOffset;
                        double textTop = textYpos + textYOffset - (size.Height * 0.8);
                        double imageTop = 0;
                        double imageLeft = 0;
                        double imageHeight = 0;
                        double imageWitdh = 0;
                        double imageBottom = 0;

                        double width = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), pageStyle.PageWidthInch.ToString(new System.Globalization.CultureInfo(1033)) + "in");
                        width = width - pageStyle.Margin.MarginLeft - pageStyle.Margin.MarginRight;
                        var imageElements = document.Root.Elements().Where(x => x.Name.LocalName == "image").ToList();
                        foreach (var imageElement in imageElements)
                        {
                            headingStyles.AccentName = imageElement.Attribute("alt-text").Value;
                            string imageUri = imageElement.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink")).Value;
                            imageTop = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("y").Value);
                            imageLeft = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("x").Value);
                            imageHeight = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("height").Value);
                            imageWitdh = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), imageElement.Attribute("width").Value);
                            imageBottom = imageTop + imageHeight;
                            headingStyles.AccentImages.Insert(0, new MenuPresentationModel.MenuStyles.MenuImage(new MenuPresentationModel.MenuStyles.Resource() { Uri = imageUri, Name = "" }, imageWitdh / fSize, imageHeight / fSize));
                            if (imageElements.Count == 1)
                            {
                                headingStyles.HorizontalStrech = double.Parse(imageElement.Attribute("width").Value, new System.Globalization.CultureInfo(1033)) > width * 0.9;
                                headingStyles.AccentImageMarginTop = ((imageHeight - size.Height) / 2) / fSize;//(textYpos + textYOffset - fontBaseLine - imageTop)/fSize ;//
                                headingStyles.AccentImageMarginBottom = ((imageHeight - size.Height) / 2) / fSize;//(imageTop + imageHeight - (textYpos + textYOffset+(fontBottom-fontBaseLine))) / fSize; //
                                headingStyles.AccentImageMarginLeft = ((imageWitdh - size.Width) / 2) / fSize;
                                headingStyles.AccentImageMarginRight = ((imageWitdh - size.Width) / 2) / fSize;
                                headingStyles.AccentImageWidth = imageWitdh;
                                headingStyles.AccentImageHeight = imageHeight;
                            }
                            else
                            {
                                double leftImageMargin = ((double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), textElement.Elements().Where(x => x.Name.LocalName == "tspan").FirstOrDefault().Attribute("x").Value) - (size.Width / 2) - (imageLeft + imageWitdh)) / fSize;
                                if (leftImageMargin > 0)
                                    headingStyles.AccentImageMarginLeft = ((double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), textElement.Elements().Where(x => x.Name.LocalName == "tspan").FirstOrDefault().Attribute("x").Value) - (size.Width / 2) - (imageLeft + imageWitdh)) / fSize; ;

                                double rightImageMargin = (imageLeft - ((double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), textElement.Elements().Where(x => x.Name.LocalName == "tspan").FirstOrDefault().Attribute("x").Value) + (size.Width / 2))) / fSize;
                                if (rightImageMargin > 0)
                                    headingStyles.AccentImageMarginRight = (imageLeft - ((double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), textElement.Elements().Where(x => x.Name.LocalName == "tspan").FirstOrDefault().Attribute("x").Value) + (size.Width / 2))) / fSize;

                            }
                        }


                        headingStyles.UnderlineImage = imageTop > textBottom;
                        headingStyles.OverlineImage = (imageBottom - 10 < textTop && imageHeight < 15);
                        if (!headingStyles.UnderlineImage && !headingStyles.OverlineImage && imageElements.Count == 1)
                        {
                            headingStyles.TextBackgroundImage = true;
                            if (headingStyles.HorizontalStrech)
                            {
                                headingStyles.AccentImageMarginRight = 0;
                                headingStyles.AccentImageMarginLeft = 0;
                            }
                        }
                        else
                        {
                            //headingStyles.AccentImageMarginRight = 0;
                            //headingStyles.AccentImageMarginLeft = 0;
                            //headingStyles.AccentImageMarginTop = 0;
                            //headingStyles.AccentImageMarginBottom = 0;

                        }


                        break;
                    }
                }
            }

            int lopa = 0;

            //font-family
            //font-size
            //font-weight
            //font-style
            //icaps
            //text-decoration
            //itext-shadow
            //fill
            //fill-opacity
            //stroke-opacity
            //letter-spacing
            //stroke
            //stroke-width
            //space-before
            //space-after
            //indent
            //justification
            //white-space
            //text-rendering
            //-moz-font-feature-settings
            //-ms-font-feature-settings
            //font-feature-settings



        }

        /// <MetaDataID>{faa10329-cfc3-4bf1-af67-8c27bedd45ca}</MetaDataID>
        private void GetCSSHeadingStyle(ExCSS.StyleRule headingStyle, CSSHeadingStyle theHeadingStyle)
        {
            theHeadingStyle.spaceBefore = GetStyleRulePropertyValue(headingStyle, "space-before").ToString();
            theHeadingStyle.spaceAfter = GetStyleRulePropertyValue(headingStyle, "space-after").ToString();
            theHeadingStyle.HeadingAlign = GetStyleRulePropertyValue(headingStyle, "justification").ToString();
        }

        /// <MetaDataID>{a6305dbc-c014-4018-ba60-9eb7ad6f35b1}</MetaDataID>
        private void GetCSSHeadingAccentStyle(ExCSS.StyleRule headingStyle, CSSHeadingStyle theHeadingStyle)
        {
            theHeadingStyle.AccentName = GetStyleRulePropertyValue(headingStyle, "name").ToString();
            theHeadingStyle.AccentColor = GetStyleRulePropertyValue(headingStyle, "color").ToString();
            theHeadingStyle.FillOpacity = GetStyleRulePropertyValue(headingStyle, "fill-opacity").ToString();
            theHeadingStyle.StrokeOpacity = GetStyleRulePropertyValue(headingStyle, "stroke-opacity").ToString();

        }



        /// <MetaDataID>{e1594ee5-0a1f-4a48-88dd-d129ff2e04da}</MetaDataID>
        internal static void GetCSSFontStyle(ExCSS.StyleRule titleHeadingStyle, CSSFontStyle thefontStyle)
        {

            thefontStyle.fontFamily = GetStyleRulePropertyValue(titleHeadingStyle, "font-family").ToString();
            thefontStyle.fontSize = GetStyleRulePropertyValue(titleHeadingStyle, "font-size").ToString();
            thefontStyle.fontWeight = GetStyleRulePropertyValue(titleHeadingStyle, "font-weight").ToString();
            thefontStyle.fontStyle = GetStyleRulePropertyValue(titleHeadingStyle, "font-style").ToString();
            thefontStyle.fontForground = GetStyleRulePropertyValue(titleHeadingStyle, "fill").ToString();
            thefontStyle.letterspacing = GetStyleRulePropertyValue(titleHeadingStyle, "letter-spacing").ToString();

            thefontStyle.allCaps = GetStyleRulePropertyValue(titleHeadingStyle, "icaps").ToString();
            var shadowTerm = GetStyleRulePropertyValue(titleHeadingStyle, "itext-shadow");

            if (shadowTerm is ExCSS.TermList)
            {

                thefontStyle.shadowx = (shadowTerm as ExCSS.TermList).ToArray()[0] as ExCSS.PrimitiveTerm;
                thefontStyle.shadowy = (shadowTerm as ExCSS.TermList).ToArray()[1] as ExCSS.PrimitiveTerm;
                thefontStyle.shadowblur = (shadowTerm as ExCSS.TermList).ToArray()[2] as ExCSS.PrimitiveTerm;
                thefontStyle.shadowcolor = (shadowTerm as ExCSS.TermList).ToArray()[3] as ExCSS.HtmlColor;
                thefontStyle.shadow = "yes";
            }
            else
                thefontStyle.shadow = "none";

            var strokeTerm = GetStyleRulePropertyValue(titleHeadingStyle, "stroke");
            if (strokeTerm.ToString() != "none")
            {
                thefontStyle.strokeColor = strokeTerm as ExCSS.HtmlColor;
                thefontStyle.strokeThickness = GetStyleRulePropertyValue(titleHeadingStyle, "stroke-width") as ExCSS.PrimitiveTerm;

                thefontStyle.stroke = "yes";
            }
            else
                thefontStyle.stroke = "none";

        }

        /// <MetaDataID>{3ece4509-54da-42c1-83fc-222654bc3c97}</MetaDataID>
        internal static ExCSS.Term GetStyleRulePropertyValue(ExCSS.StyleRule styleRule, string propertyName)
        {

            //foreach( var mproperty in (from declaration in styleRule.Declarations.Properties
            //                           select declaration))
            //{
            //    System.Diagnostics.Debug.WriteLine(mproperty.Name);
            //}

            ExCSS.Property property = (from declaration in styleRule.Declarations.Properties
                                       where declaration.Name == propertyName
                                       select declaration).FirstOrDefault();
            if (property != null)
                return property.Term;
            return new ExCSS.PrimitiveTerm(ExCSS.UnitType.Ident, "none");
        }

        //private void GetTitleHeadingStyleDataFromStyleSheet(ExCSS.StyleSheet styleSheet, CSSHeadingStyle titleHeadingStylse)
        //{
        //    var cssTitleHeadingStyle = (from styleRule in styleSheet.StyleRules
        //                                where styleRule.Value == ".title-heading"
        //                                select styleRule).FirstOrDefault();

        //    MenuDesigner.CSSFontStyle thefontStyle = titleHeadingStylse.FontStyle;
        //    GetCSSFontStyle(cssTitleHeadingStyle, thefontStyle);

        //    GetCSSHeadingStyle(cssTitleHeadingStyle, titleHeadingStylse);


        //    var titleHeadingAccentStyle = (from styleRule in styleSheet.StyleRules
        //                                   where styleRule.Value == ".title-heading-accent"
        //                                   select styleRule).FirstOrDefault();

        //    GetCSSHeadingAccentStyle(titleHeadingAccentStyle, titleHeadingStylse);




        //    foreach (IHTMLElement tspan in (Web.Document as mshtml.IHTMLDocument3).getElementsByTagName("tspan"))
        //    {
        //        if (tspan.getAttribute("className") as string == "title-heading" /*"heading"*/)
        //        {
        //            IHTMLElement gElement = tspan.parentElement.parentElement;
        //            XDocument document = XDocument.Parse(gElement.outerHTML);
        //            if (document.Root.Attribute("id").Value.IndexOf("headitem_") == 0)
        //            {
        //                foreach (var imageElement in document.Root.Elements().Where(x => x.Name.LocalName == "image"))
        //                {
        //                    string imageUri = imageElement.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink")).Value;
        //                    titleHeadingStylse.AccentImages.Add(imageUri);
        //                }
        //                break;
        //            }
        //        }
        //    }

        //    int lopa = 0;

        //    //font-family
        //    //font-size
        //    //font-weight
        //    //font-style
        //    //icaps
        //    //text-decoration
        //    //itext-shadow
        //    //fill
        //    //fill-opacity
        //    //stroke-opacity
        //    //letter-spacing
        //    //stroke
        //    //stroke-width
        //    //space-before
        //    //space-after
        //    //indent
        //    //justification
        //    //white-space
        //    //text-rendering
        //    //-moz-font-feature-settings
        //    //-ms-font-feature-settings
        //    //font-feature-settings



        //}

        /// <MetaDataID>{2b6f0855-bc98-45d8-b8d7-5f70b7576d88}</MetaDataID>
        void InitFont()
        {
            _FontFamily = this.TheFontFamily;
            _FontWeight = this.FontWeight;
            _FontSize = this.FontSize;
            _FontWeight = this.FontWeight;
            _FontForeground = this.Foreground as SolidColorBrush;
        }

        /// <MetaDataID>{abaefb6d-c84f-4321-8001-8c658a1f8855}</MetaDataID>
        void SetFont(CSSFontStyle cssFont)
        {
            WpfFontStyle wpfFontStyle = WpfFontStyle.GetWpfFont(cssFont);

            FontStyle = wpfFontStyle.fontStyle;
            TheFontFamily = wpfFontStyle.fontFamily;
            TheFontSize = wpfFontStyle.fontSize;
            Color = wpfFontStyle.fontForground;

        }



        /// <MetaDataID>{a45a64ec-c015-46f9-b1cb-d7c3603e8d25}</MetaDataID>
        System.Windows.Media.FontFamily _FontFamily;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{126636ec-1cea-45be-a434-5161da7747ba}</MetaDataID>
        public System.Windows.Media.FontFamily TheFontFamily
        {
            get
            {

                return _FontFamily;
            }
            set
            {
                _FontFamily = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TheFontFamily"));
            }
        }


        /// <MetaDataID>{7e7c3a54-86f6-438d-aecf-3a4974e97d55}</MetaDataID>
        FontWeight _FontWeight;
        /// <MetaDataID>{f4dab907-2cee-48ea-8c25-1fcc8afd0ed8}</MetaDataID>
        public FontWeight FontWeight
        {
            get
            {
                return _FontWeight;
            }
        }
        /// <MetaDataID>{45e28a16-49f3-4664-80a3-0673b24cf3cd}</MetaDataID>
        double _FontSize;
        /// <MetaDataID>{5edaccdc-3a2f-4f0c-afbe-2d38c4622fba}</MetaDataID>
        public double TheFontSize
        {
            get
            {
                return _FontSize;// (double)(new FontSizeConverter().ConvertFrom(FontSize));

            }
            set
            {
                _FontSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TheFontSize)));
            }
        }


        /// <MetaDataID>{deba1f9c-41f1-403c-9091-9bae7d964b28}</MetaDataID>
        bool _AllCaps;
        /// <MetaDataID>{83eeb5b0-848c-4d4b-bab5-89aedf53f685}</MetaDataID>
        public bool AllCaps
        {
            get
            {
                return _AllCaps;
            }
            set
            {
                _AllCaps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCaps)));
            }
        }

        /// <MetaDataID>{82093765-0dae-4bc2-bcd1-4be017480e4c}</MetaDataID>
        public double StrokeThickness
        {
            get
            {
                if (IsStrokeEnabled)
                    return 1;
                else
                    return 0;
            }
        }

        /// <MetaDataID>{8aea7130-b0e1-43dc-8f66-f9716ca693c1}</MetaDataID>
        public System.Windows.Media.Brush Stroke
        {
            get
            {
                return Foreground;
            }
        }


        /// <MetaDataID>{2e94be77-34f7-41a3-ae62-b593f19e43f0}</MetaDataID>
        public System.Windows.Media.Color Color
        {
            get
            {
                if (_FontForeground == null)
                    _FontForeground = Foreground as SolidColorBrush;
                if (_FontForeground != null)
                    return _FontForeground.Color;
                return Colors.Black;
            }
            set
            {
                _FontForeground = new SolidColorBrush(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontForeground)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeFill)));

            }
        }

        /// <MetaDataID>{1320e606-aa45-4f8f-8422-9842cd14a5f4}</MetaDataID>
        System.Windows.Media.SolidColorBrush _FontForeground;
        /// <MetaDataID>{922a210c-bb2a-4517-a9ba-ba741f1bb9a8}</MetaDataID>
        public System.Windows.Media.SolidColorBrush FontForeground
        {
            get
            {
                //if (_FontForeground == null)
                //    return Foreground as SolidColorBrush;
                //else
                return _FontForeground;
            }
        }
        /// <MetaDataID>{bf386e81-bc09-4b52-955b-2667f7a4757c}</MetaDataID>
        bool _Underline;
        /// <MetaDataID>{3eab2a0a-e7ec-4d51-a4ab-ef722e9b45af}</MetaDataID>
        public bool Underline
        {
            get
            {
                return _Underline;
            }
            set
            {
                _Underline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Underline)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Overline)));

            }
        }
        /// <MetaDataID>{c0392675-c1b8-4e09-a248-f60423c9e172}</MetaDataID>
        bool _Overline;
        /// <MetaDataID>{93732f4b-7972-4312-b3a4-15a55756abf8}</MetaDataID>
        public bool Overline
        {
            get
            {
                return _Overline;
            }
            set
            {
                _Overline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Underline)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Overline)));

            }
        }


        /// <MetaDataID>{596f5c97-39d7-48d7-b2d1-3a1f1791bba0}</MetaDataID>
        bool _IsStrokeEnabled;
        /// <MetaDataID>{038691ab-d340-4946-b100-271a7c270d8a}</MetaDataID>
        public bool IsStrokeEnabled
        {
            get
            {
                return _IsStrokeEnabled;
            }
            set
            {
                _IsStrokeEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stroke)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeFill)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeThickness)));
            }
        }

        /// <MetaDataID>{dbad3584-b9cd-4a2a-ac31-86434617013d}</MetaDataID>
        public System.Windows.Media.Brush StrokeFill
        {
            get
            {
                if (IsStrokeEnabled)
                    return new SolidColorBrush(Colors.White);
                else
                    return FontForeground;

            }
        }

        /// <MetaDataID>{38d238b7-bd64-46a6-a4e4-05ebdf605a08}</MetaDataID>
        System.Windows.FontStyle _FontStyle;
        /// <MetaDataID>{cb110c32-610f-4dcb-8952-56cfe5dd5e3e}</MetaDataID>
        public System.Windows.FontStyle TheFontStyle
        {
            get
            {
                return _FontStyle;
            }
            set
            {
                _FontStyle = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TheFontStyle)));
            }
        }

        private void BackgroundBtn_Click(object sender, RoutedEventArgs e)
        {

            OOAdvantech.Linq.Storage mstorage = new OOAdvantech.Linq.Storage(BackgroundImagesStorage);
            var images = (from theResource in mstorage.GetObjectCollection<IImage>()
                          select theResource);

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {
                foreach (var image in images)
                {
                    PageImage pageImage = new PageImage();

                    pageImage.Name = image.Image.Name;
                    pageImage.PortraitImage = image.Image;
                    pageImage.PortraitHeight = 1056;
                    pageImage.PortraitWidth = 824;
                    pageImage.LandscapeImage = image.Image;
                    pageImage.LandscapeHeight = 824;
                    pageImage.LandscapeWidth = 1056;
                    BackgroundImagesStorage.CommitTransientObjectState(pageImage);

                }
                stateTransition.Consistent = true;
            }

            return;



            mshtml.IHTMLStyleSheet ssheet = (Web.Document as mshtml.HTMLDocument).styleSheets.item(0) as mshtml.IHTMLStyleSheet;
            string title = ssheet.title;
            string styleName = null;

            CSSHeadingStyle titleHeadingStyle = new CSSHeadingStyle();
            CSSHeadingStyle headingStyle = new CSSHeadingStyle();



            CSSHeadingStyle subHeadingStyle = new CSSHeadingStyle();
            CSSHeadingStyle altFontHeadingStyle = new CSSHeadingStyle();


            CSSPageStyle pageStyle = new CSSPageStyle();
            CSSPriceOptions priceOptions = null;
            CSSMenuItemOptions menuItemOptions = null;
            CSSLayoutOptions layoutOptions = null;
            ExCSS.StyleSheet cssStyleSheet = GetStyleSheet(out styleName);

            if (cssStyleSheet != null)
            {
                //priceOptions = GetPriceStyleData(cssStyleSheet);
                //menuItemOptions = GetMenuItemStyleData(cssStyleSheet);
                GetPageStyleData(pageStyle, cssStyleSheet);
                string pageBackgroundsFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                if (!System.IO.Directory.Exists(pageBackgroundsFolder))
                    System.IO.Directory.CreateDirectory(pageBackgroundsFolder);
                pageBackgroundsFolder += "\\DontWaitWater\\PageBackgrounds";
                if (!System.IO.Directory.Exists(pageBackgroundsFolder))
                    System.IO.Directory.CreateDirectory(pageBackgroundsFolder);

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(BackgroundImagesStorage);
                MenuPresentationModel.MenuStyles.IImage image = (from theResource in storage.GetObjectCollection<IImage>()
                                                                 where theResource.Image.Name == pageStyle.BackgroundName
                                                                 select theResource).FirstOrDefault();

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                {
                    Resource resource = new Resource();
                    resource.Uri = pageStyle.DownloadBackground(pageBackgroundsFolder);
                    resource.Name = pageStyle.BackgroundName;
                    if (image == null)
                    {
                        image = new MenuImage(resource, 0, 0);
                        BackgroundImagesStorage.CommitTransientObjectState(image);
                    }
                    stateTransition.Consistent = true;
                }

                //resource.Uri = pageStyle.DownloadBackground(pageBackgroundsFolder);
                //resource.Name = pageStyle.BackgroundName;
            }

        }

        private void BorderBtn_Click(object sender, RoutedEventArgs e)
        {

            mshtml.IHTMLStyleSheet ssheet = (Web.Document as mshtml.HTMLDocument).styleSheets.item(0) as mshtml.IHTMLStyleSheet;
            string title = ssheet.title;
            string styleName = null;

            CSSHeadingStyle titleHeadingStyle = new CSSHeadingStyle();
            CSSHeadingStyle headingStyle = new CSSHeadingStyle();



            CSSHeadingStyle subHeadingStyle = new CSSHeadingStyle();
            CSSHeadingStyle altFontHeadingStyle = new CSSHeadingStyle();


            CSSPageStyle pageStyle = new CSSPageStyle();
            CSSPriceOptions priceOptions = null;
            CSSMenuItemOptions menuItemOptions = null;
            CSSLayoutOptions layoutOptions = null;
            ExCSS.StyleSheet cssStyleSheet = GetStyleSheet(out styleName);

            if (cssStyleSheet != null)
            {
                //priceOptions = GetPriceStyleData(cssStyleSheet);
                //menuItemOptions = GetMenuItemStyleData(cssStyleSheet);
                GetPageStyleData(pageStyle, cssStyleSheet);
                string pageBordersFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
                if (!System.IO.Directory.Exists(pageBordersFolder))
                    System.IO.Directory.CreateDirectory(pageBordersFolder);
                pageBordersFolder += "\\DontWaitWater\\PageBorders";
                if (!System.IO.Directory.Exists(pageBordersFolder))
                    System.IO.Directory.CreateDirectory(pageBordersFolder);

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(BordersStorage);
                MenuPresentationModel.MenuStyles.IPageImage image = (from theResource in storage.GetObjectCollection<IPageImage>()
                                                                     where theResource.Name == SelectedBorder.Description
                                                                     select theResource).FirstOrDefault();

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                {
                    Resource resource = new Resource();
                    pageStyle.BorderFileName = "l_" + SelectedBorder.FileName;
                    resource.Uri = pageStyle.DownloadBorder(pageBordersFolder);
                    resource.Name = SelectedBorder.Description;
                    //resource.Name = pageStyle.BorderName;
                    if (image == null)
                    {
                        image = new PageImage();
                        image.LandscapeHeight = 824;
                        image.LandscapeWidth = 1056;
                        image.LandscapeImage = resource;
                        image.Name = SelectedBorder.Description;

                        //image = new MenuImage(resource, 1056, 824);
                        BordersStorage.CommitTransientObjectState(image);
                    }
                    else
                    {
                        image.LandscapeHeight = 824;
                        image.LandscapeWidth = 1056;
                        image.LandscapeImage = resource;

                    }
                    stateTransition.Consistent = true;
                }

                //   .st0{fill:#B33825;}
                //.st1{fill:#53B0A5;}
                //.st2{fill:#221E1F;}
                //.st3{fill:#003671;}
                //.st4{fill:#000001;}

                //   .st0{fill:#a11d28; fill-opacity: 1; stroke-opacity: 1;}
                //.st1{fill:#8f0b16; fill-opacity: 1; stroke-opacity: 1;}
                //.st2{fill:#b32f3a; fill-opacity: 1; stroke-opacity: 1;}
                //.st3{fill:#7d0004; fill-opacity: 1; stroke-opacity: 1;}
                //.st4{fill:#c5414c; fill-opacity: 1; stroke-opacity: 1;}

                //resource.Uri = pageStyle.DownloadBackground(pageBackgroundsFolder);
                //resource.Name = pageStyle.BackgroundName;
            }

        }
    }

    /// <MetaDataID>{09e505f0-5b87-46dd-be3e-584dddb3ea07}</MetaDataID>
    public class CSSFontStyle
    {
        /// <MetaDataID>{009ba0c7-8f02-4236-8eb1-5b96b39e1d41}</MetaDataID>
        public string fontFamily;
        /// <MetaDataID>{8f55454c-dbd1-4e8c-8c5c-e944ad5e494e}</MetaDataID>
        public string fontSize;
        /// <MetaDataID>{70107658-63b9-4706-91e9-36c5b8c0b0a7}</MetaDataID>
        public string letterspacing;
        /// <MetaDataID>{898bfff9-6809-4968-8eb5-c974c38f125d}</MetaDataID>
        public string fontWeight;
        /// <MetaDataID>{3e801fab-eb1a-430c-8910-6a4fb5509cd1}</MetaDataID>
        public string fontStyle;
        /// <MetaDataID>{bc98c80e-eea4-4cbd-9c56-576101266fbf}</MetaDataID>
        public string fontForground;
        /// <MetaDataID>{2ea24263-7eb6-42e8-8524-b69e05358ddd}</MetaDataID>
        public string stroke;
        /// <MetaDataID>{05c79776-198e-41a1-83aa-c619cb38039e}</MetaDataID>
        public ExCSS.HtmlColor strokeColor;
        /// <MetaDataID>{d9b4ea5f-cc83-411b-bfdb-dc24ad75a995}</MetaDataID>
        public ExCSS.PrimitiveTerm strokeThickness;
        /// <MetaDataID>{c7d90456-b40b-4621-b744-4399a99112e6}</MetaDataID>
        public string allCaps;
        /// <MetaDataID>{e425def6-aaf5-498c-b5dc-5af8baeaca10}</MetaDataID>
        public string shadow;
        /// <MetaDataID>{ac6561fc-82a9-4bc7-a1e2-a8154033293d}</MetaDataID>
        internal ExCSS.PrimitiveTerm shadowx;
        /// <MetaDataID>{62dda5bb-a8f2-4ffa-aa6c-7419690a7e2a}</MetaDataID>
        internal ExCSS.PrimitiveTerm shadowy;
        /// <MetaDataID>{3b818243-b24b-4c55-bb6b-e6e7a0a4ad99}</MetaDataID>
        internal ExCSS.PrimitiveTerm shadowblur;
        /// <MetaDataID>{c7e03783-fda4-45c9-85db-194eb0e72ecf}</MetaDataID>
        internal ExCSS.HtmlColor shadowcolor;
    }

    /// <MetaDataID>{dc6a8aad-bd60-41be-ac76-358dc5903ff3}</MetaDataID>
    public class WpfFontStyle
    {
        /// <MetaDataID>{8bca6cd5-2b59-4fc8-869f-fb4987d561df}</MetaDataID>
        public FontFamily fontFamily;
        /// <MetaDataID>{73d20cb4-e69d-40e2-904e-406cf420cd15}</MetaDataID>
        public Double fontSize;
        /// <MetaDataID>{52fe673c-e889-4b15-81cc-a3c2751a003d}</MetaDataID>
        public Double fontSpacing;
        /// <MetaDataID>{dad7bae5-a2f4-4b1d-a7f1-c1f9f302a79b}</MetaDataID>
        public FontWeight fontWeight;
        /// <MetaDataID>{c5bbac51-ef15-423e-b6b3-4f611d99cdb1}</MetaDataID>
        public FontStyle fontStyle;
        /// <MetaDataID>{bad901b8-3940-4f43-8f92-3d35030a6456}</MetaDataID>
        public Color fontForground;
        /// <MetaDataID>{4adb1ed1-7edf-4fe3-9030-212faf1be47b}</MetaDataID>
        public bool stroke;
        /// <MetaDataID>{aacbe405-d418-40d4-abc8-f26168e12c51}</MetaDataID>
        public bool allCaps;
        /// <MetaDataID>{777efae7-3869-4c8f-a090-ebedbbb086dc}</MetaDataID>
        public bool shadow;
        /// <MetaDataID>{b57f15bf-d99f-4fff-a0ef-7c21678f7da6}</MetaDataID>
        public static WpfFontStyle GetWpfFont(CSSFontStyle cssFont)

        {



            WpfFontStyle wpfFontStyle = new WpfFontStyle();

            string fontFamilyName = cssFont.fontFamily.Substring(cssFont.fontFamily.IndexOf("'") + 1, cssFont.fontFamily.LastIndexOf("'") - 1);
            wpfFontStyle.fontFamily = (from fontFamily in StyleableWindow.FontDialog.FontFamilies
                                       where fontFamily.FamilyNames.ToArray()[0].Value == fontFamilyName
                                       select fontFamily).FirstOrDefault();
            if (wpfFontStyle.fontFamily == null)
            {
                if (StyleableWindow.FontDialog.ActualFontFamiliesNames.ContainsKey(fontFamilyName))
                {
                    fontFamilyName = StyleableWindow.FontDialog.ActualFontFamiliesNames[fontFamilyName];
                    wpfFontStyle.fontFamily = (from fontFamily in StyleableWindow.FontDialog.FontFamilies
                                               where fontFamily.FamilyNames.ToArray()[0].Value == fontFamilyName
                                               select fontFamily).FirstOrDefault();
                }
            }
            wpfFontStyle.fontSize = (double)(new FontSizeConverter().ConvertFrom(cssFont.fontSize));
            wpfFontStyle.fontSpacing = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), cssFont.letterspacing);
            wpfFontStyle.fontForground = (Color)ColorConverter.ConvertFromString(cssFont.fontForground);

            if (cssFont.fontStyle == "italic")
                wpfFontStyle.fontStyle = FontStyles.Italic;

            if (cssFont.fontStyle == "normal")
                wpfFontStyle.fontStyle = FontStyles.Normal;

            try
            {
                wpfFontStyle.fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(cssFont.fontWeight);
            }
            catch (Exception error)
            {
            }
            return wpfFontStyle;
        }

        /// <MetaDataID>{9e35be62-2b4f-40bc-90a6-52d676bd3baa}</MetaDataID>
        public static Size MeasureText(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize, double fontSpacing)
        {
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            GlyphTypeface glyphTypeface;

            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                return MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
            }

            double totalWidth = 0;
            double height = 0;

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

                double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

                if (glyphHeight > height)
                {
                    height = glyphHeight;
                }

                totalWidth = totalWidth + width + fontSpacing;
            }
            totalWidth -= fontSpacing;
            return new Size(totalWidth, height);
        }

        /// <MetaDataID>{0f2d063a-376a-48a6-8f7d-54b711850837}</MetaDataID>
        public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                               System.Globalization.CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                 fontSize,
                                                 Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }

        internal static double GetFontBottom(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                              FlowDirection.LeftToRight,
                                              new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                              fontSize,
                                              Brushes.Black);
            double y_bottom = ft.Height;
            double y_baseline = ft.Baseline;

            double y_descent = y_bottom + ft.OverhangAfter;

            return y_descent;
        }
        internal static double GetFontBaseLine(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                              FlowDirection.LeftToRight,
                                              new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                              fontSize,
                                              Brushes.Black);
            double y_bottom = ft.Height;
            double y_baseline = ft.Baseline;

            double y_descent = y_bottom + ft.OverhangAfter;

            return y_baseline;
        }
    }
    /// <MetaDataID>{2042627f-9f63-4059-958e-0d0965ffd731}</MetaDataID>
    public class CSSHeadingStyle
    {
        /// <MetaDataID>{bd6238be-bda8-4751-bef3-3986c733be75}</MetaDataID>
        public List<MenuImage> AccentImages = new List<MenuImage>();
        /// <MetaDataID>{de17377e-cf0e-46de-8e64-a7c8dba6a691}</MetaDataID>
        public string spaceBefore;
        /// <MetaDataID>{ad53b73e-bbfa-45bc-8dba-306623a9e202}</MetaDataID>
        public string spaceAfter;
        /// <MetaDataID>{1aea5afa-5393-4155-9269-7beb207aa83c}</MetaDataID>
        public string HeadingAlign;

        /// <MetaDataID>{bafe9c19-ae7e-455d-b0dc-e527febb1479}</MetaDataID>
        public string AccentName;
        /// <MetaDataID>{5e720d01-b246-4243-931d-61335ac95b8d}</MetaDataID>
        public string AccentColor;
        /// <MetaDataID>{2210430d-6294-4e9b-9d6e-40c75b41c4ef}</MetaDataID>
        public string FillOpacity;
        /// <MetaDataID>{65972513-4e36-4643-a7f9-267773f8cbca}</MetaDataID>
        public string StrokeOpacity;
        /// <MetaDataID>{0bb4ca7c-4d37-4f3a-81ba-3fe80465d3c3}</MetaDataID>
        public bool UnderlineImage;
        /// <MetaDataID>{b74d2584-7830-4bdb-b208-bacda9dd16f6}</MetaDataID>
        public bool OverlineImage;
        /// <MetaDataID>{d8a2146e-0b7e-45b8-aecc-1ace39a4fdd4}</MetaDataID>
        public bool HorizontalStrech;


        /// <MetaDataID>{b7efeef3-b890-4195-8257-4ff813d4f43f}</MetaDataID>
        public CSSFontStyle FontStyle = new CSSFontStyle();

        /// <MetaDataID>{c41be923-a6fd-4edd-adc2-54886b3402b1}</MetaDataID>
        public bool TextBackgroundImage { get; internal set; }
        /// <MetaDataID>{bbacdbd5-9d62-4770-81e4-9143479adf76}</MetaDataID>
        public double AccentImageMarginTop { get; internal set; }
        /// <MetaDataID>{4d4c4fa8-e3d0-4f03-9f07-70d4d0e7614b}</MetaDataID>
        public double AccentImageMarginLeft { get; internal set; }
        /// <MetaDataID>{307a922c-8fc6-4474-bc48-d4de602fd2a4}</MetaDataID>
        public double AccentImageMarginBottom { get; internal set; }
        /// <MetaDataID>{2a655de2-9725-45ca-9a54-d5e0f457c153}</MetaDataID>
        public double AccentImageMarginRight { get; internal set; }

        /// <MetaDataID>{7bdb9c70-4bd2-4cb8-949c-e18b32f152f9}</MetaDataID>
        public double AccentImageWidth { get; internal set; }
        /// <MetaDataID>{033993ce-a0c7-4a96-a8da-db52b159d854}</MetaDataID>
        public double AccentImageHeight { get; internal set; }

        /// <MetaDataID>{30dc5c00-d9a3-426b-8056-54e0ce3e6611}</MetaDataID>
        public List<MenuPresentationModel.MenuStyles.MenuImage> DownloadAccentImages(string folder)
        {
            List<MenuPresentationModel.MenuStyles.MenuImage> downloadedImagesFileNames = new List<MenuPresentationModel.MenuStyles.MenuImage>();

            int i = 1;
            foreach (var accentImageUri in AccentImages)
            {
                string accentImageName = AccentName;
                if (AccentImages.Count > 1)
                    accentImageName += "_" + i.ToString();
                i++;
                WebClient client = new WebClient();
                client.DownloadFile(accentImageUri.Image.Uri, folder + @"\" + accentImageName + ".svg");
                MenuPresentationModel.MenuStyles.Resource accentImage = new MenuPresentationModel.MenuStyles.Resource();
                accentImage.Name = accentImageName;
                accentImage.Uri = folder + @"\" + accentImageName + ".svg";
                downloadedImagesFileNames.Add(new MenuPresentationModel.MenuStyles.MenuImage(accentImage, accentImageUri.Width, accentImageUri.Height));
            }
            return downloadedImagesFileNames;


        }
    }

    /// <MetaDataID>{6a817440-f34c-42ea-927b-db61821e7f19}</MetaDataID>
    public class CSSMargin
    {
        /// <MetaDataID>{aaefd092-ad4f-43a2-9a9c-87f082b42592}</MetaDataID>
        public double MarginTop = 0;
        /// <MetaDataID>{977150a2-d12a-4852-9dd6-c373cb1b8564}</MetaDataID>
        public double MarginBottom = 0;
        /// <MetaDataID>{9b466cda-350e-4a52-ac1b-718df34f9fcf}</MetaDataID>
        public double MarginLeft = 0;
        /// <MetaDataID>{627ec69d-0075-4822-a67e-f5c11a8e1251}</MetaDataID>
        public double MarginRight = 0;
    }

    /// <MetaDataID>{2b81fcf6-e9ba-45b7-b006-0446c030db00}</MetaDataID>
    public class CSSPageStyle
    {

        /// <MetaDataID>{677ac38b-d34a-41a1-9416-e4882d3ac5f9}</MetaDataID>
        public string BackgroundUri = null;
        /// <MetaDataID>{ea3403a3-3248-47bd-bfb4-8724588c83fe}</MetaDataID>
        public string BackgroundFileName = null;
        /// <MetaDataID>{7d03b6c2-aa14-40da-8147-a822669e5b49}</MetaDataID>
        public string BackgroundName = null;

        /// <MetaDataID>{c11b63de-c77d-4711-9000-d785d2b6bb07}</MetaDataID>
        public string BorderUri = null;
        /// <MetaDataID>{54f2949c-161d-4c7b-8a38-9e5f135c64fb}</MetaDataID>
        public string BorderFileName = null;
        /// <MetaDataID>{fdfb7fce-95fa-4953-aaa3-e10583997af1}</MetaDataID>
        public string BorderName = null;


        public Margin BackgroundMargin;

        public Margin BorderMargin;
        /// <MetaDataID>{ab9b24b4-463d-4fa1-bfe9-99ad794859ba}</MetaDataID>
        public double PageWidthInch = 0;
        /// <MetaDataID>{4149d8d1-0b76-4f12-a20f-3e9ff6a73a13}</MetaDataID>
        public double PageHeigthInch = 0;
        /// <MetaDataID>{13135edb-c937-47b4-8877-c8247998aacc}</MetaDataID>
        public string DownloadBackground(string folder)
        {
            if (!string.IsNullOrWhiteSpace(BackgroundUri))
            {
                WebClient client = new WebClient();
                client.DownloadFile(BackgroundUri, folder + @"\" + BackgroundFileName);
                return folder + @"\" + BackgroundFileName;
            }
            else
                return "";
        }

        /// <MetaDataID>{ea61ad7b-cdab-4188-883c-5ef2929d529e}</MetaDataID>
        public string DownloadBorder(string folder)
        {
            if (!string.IsNullOrWhiteSpace(BorderUri))
            {
                WebClient client = new WebClient();
                client.DownloadFile(BorderUri, folder + @"\" + BorderFileName);
                return folder + @"\" + BorderFileName;
            }
            else
                return "";
        }

        /// <MetaDataID>{f367ac70-e1f2-4608-9d6e-a233c3ce5ff4}</MetaDataID>
        public CSSMargin Margin = new CSSMargin();
    }


    /// <MetaDataID>{bc062d05-99f9-4db2-aad3-328614640b08}</MetaDataID>
    public class CSSLayoutOptions
    {

        public double line_spacing;
        public double name_indent;
        public double desc_left_indent;
        public double extras_left_indent;
        public double desc_right_indent;
        public string desc_separator;
        public string extras_separator;
        public double space_between_columns;
        public string line_between_columns;
        public string line_type;

        public CSSLayoutOptions(ExCSS.StyleRule layoutStyleRule, WebBrowser web)
        {
            line_spacing = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "line-spacing").ToString());
            name_indent = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "name-indent").ToString() + "in");
            desc_left_indent = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "desc-left-indent").ToString() + "in");
            extras_left_indent = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "extras-left-indent").ToString() + "in");
            desc_right_indent = (double)new System.Windows.LengthConverter().ConvertFromString(null, new System.Globalization.CultureInfo(1033), DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "desc-right-indent").ToString() + "in");


            desc_separator = DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "desc-separator").ToString();
            if (desc_separator != null)
            {
                if (desc_separator == "none")
                    desc_separator = "";
                else
                {
                    desc_separator = desc_separator.Replace("p_p", "");
                    byte[] buffer = new byte[1] { (byte)int.Parse(desc_separator) };
                    desc_separator = System.Text.Encoding.Default.GetString(buffer);
                }
            }

            extras_separator = DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "extras-separator").ToString();
            if (extras_separator != null)
            {
                if (extras_separator == "none")
                    extras_separator = "";
                else
                {
                    extras_separator = extras_separator.Replace("p_p", "");
                    byte[] buffer = new byte[1] { (byte)int.Parse(extras_separator) };
                    extras_separator = System.Text.Encoding.Default.GetString(buffer);
                }
            }

            space_between_columns = double.Parse(DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "space-between-columns").ToString(), System.Globalization.CultureInfo.GetCultureInfo(1033));
            line_between_columns = DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "line-between-columns").ToString();
            line_type = DownloadStylesWindow.GetStyleRulePropertyValue(layoutStyleRule, "line-type").ToString();
            //   stroke_width:6px;
            //   shorter_line:yes;
            //   fill:#2C243B;

            //   type: next;
            //   line_spacing:1;
            //   name_indent:0;
            //   desc_left_indent:0;
            //   extras_left_indent:0;
            //   desc_right_indent:0.175;
            //   desc_separator:none;
            //   extras_separator:p_p47;
            //   photo_height:1;
            //   width_balance:100;
            //   space_between_columns:0.37;
            //   line_between_columns:no;
            //   line_type:single;
            //   stroke_width:6px;
            //   shorter_line:yes;
            //   fill:#2C243B;
            //stroke - opacity:1;


        }
    }
    /// <MetaDataID>{f083b903-bafa-4842-9637-a0633148ee50}</MetaDataID>
    public class CSSMenuItemOptions
    {
        /// <MetaDataID>{a4727ed5-3fff-4417-aca2-7a6593d280cf}</MetaDataID>
        public CSSMenuItemOptions(ExCSS.StyleRule nameStyleRule, ExCSS.StyleRule descriptionStyleRule, ExCSS.StyleRule extrasStyleRule, WebBrowser web)
        {

            DownloadStylesWindow.GetCSSFontStyle(nameStyleRule, FontStyle);
            DownloadStylesWindow.GetCSSFontStyle(descriptionStyleRule, DescriptionFontStyle);
            DownloadStylesWindow.GetCSSFontStyle(extrasStyleRule, ExtrasFontStyle);

            spaceBefore = DownloadStylesWindow.GetStyleRulePropertyValue(nameStyleRule, "space-before").ToString();
            spaceAfter = DownloadStylesWindow.GetStyleRulePropertyValue(nameStyleRule, "space-after").ToString();
            nameAlign = DownloadStylesWindow.GetStyleRulePropertyValue(nameStyleRule, "justification").ToString();
            indent = DownloadStylesWindow.GetStyleRulePropertyValue(nameStyleRule, "indent").ToString();

            string itemNamedyvalue = null;
            string itemDescriptiondyvalue = null;
            IHTMLElement lasttspan = null; 
            foreach (IHTMLElement tspan in (web.Document as mshtml.IHTMLDocument3).getElementsByTagName("tspan"))
            {
                if (lasttspan!=null && lasttspan.getAttribute("className") as string == "name" && tspan.getAttribute("className") as string == "description")
                {
                    //IHTMLElement text = lasttspan.parentElement;
                    //XDocument textDoc= XDocument.Parse(text.outerHTML);
                    //string text_anchor = textDoc.Root.Attribute("text-anchor").Value;
                    XDocument document = XDocument.Parse(tspan.outerHTML);
                    //"Description for the sample item. Use this to describe your food item • 9.95"
                    itemDescriptiondyvalue = document.Root.Attribute("dy").Value;
                    break;
                }
                lasttspan = tspan;
            }

            NewLineForDescription = itemDescriptiondyvalue !="0";


        }

        /// <MetaDataID>{581ae42b-e663-4a0e-a6b2-c8d00bfc7159}</MetaDataID>
        public CSSFontStyle FontStyle = new CSSFontStyle();
        /// <MetaDataID>{3678670b-6bf5-44fb-9a3a-a1be5d68b1ef}</MetaDataID>
        public CSSFontStyle DescriptionFontStyle = new CSSFontStyle();
        /// <MetaDataID>{b14ec1e6-c4b9-4d8f-90ba-758e33798fe3}</MetaDataID>
        public CSSFontStyle ExtrasFontStyle = new CSSFontStyle();
        /// <MetaDataID>{d2e6fe21-6e5b-4dab-bcfc-84956813ea37}</MetaDataID>
        public readonly string spaceBefore;
        /// <MetaDataID>{7676d431-7a60-4195-a956-d497cd5d1810}</MetaDataID>
        public readonly string spaceAfter;
        /// <MetaDataID>{e3e128a5-783b-4fcf-8b40-92ceefb690bb}</MetaDataID>
        public readonly string nameAlign;
        /// <MetaDataID>{ec9051b9-7809-4492-913e-bfcf39c65e2c}</MetaDataID>
        public readonly string indent;
        /// <MetaDataID>{94826d88-48df-4743-8c79-4cc2b6586d63}</MetaDataID>
        public readonly bool NewLineForDescription;
    }
    /// <MetaDataID>{34d7ed27-c38a-4d8f-b0ab-2349b094415a}</MetaDataID>
    public class CSSPriceOptions
    {
        /// <MetaDataID>{127723a7-dac4-428e-ab2a-992beded405c}</MetaDataID>
        public CSSPriceOptions(ExCSS.StyleRule styleRule)
        {
            layout = GetStyleRulePropertyValue(styleRule, "layout");
            currency = GetStyleRulePropertyValue(styleRule, "currency");
            var priceleaderProperty = GetStyleRuleProperty(styleRule, "price-leader");
            if (priceleaderProperty.Term is ExCSS.PrimitiveTerm)
            {
                priceleader = (priceleaderProperty.Term as ExCSS.PrimitiveTerm).Value as string;
                if (priceleader != null && priceleader != "none")
                {
                    priceleader = priceleader.Replace("p_p", "");
                    if (priceleader == "46")
                        priceleader = "dots";
                    else
                    {
                        byte[] buffer = new byte[1] { (byte)int.Parse(priceleader) };
                        priceleader = System.Text.Encoding.Default.GetString(buffer);
                    }

                }
            }
            else
                priceleader = "dots";

            dotspaceprice = GetStyleRulePropertyValue(styleRule, "dot-space-price");

            dotspaceitem = GetStyleRulePropertyValue(styleRule, "dot-space-item");
            dotspacebetween = GetStyleRulePropertyValue(styleRule, "dot-space-between");
            dotsmatchnamecolor = GetStyleRulePropertyValue(styleRule, "dots-match-name-color");
            spacebetweenprices = GetStyleRulePropertyValue(styleRule, "space-between-prices");

            //{layout:normal;currency:none;price-leader:;dot-space-price:1;dot-space-item:0;dot-space-between:1;dots-match-name-color:no;space-between-prices:0.65;}
        }

        /// <MetaDataID>{ed52d172-f088-4cf8-ad8a-ac63ff057a2b}</MetaDataID>
        public readonly CSSFontStyle FontStyle = new CSSFontStyle();
        /// <MetaDataID>{93836285-ae06-492b-8a76-a93f2fcf657e}</MetaDataID>
        string GetStyleRulePropertyValue(ExCSS.StyleRule styleRule, string propertyName)
        {
            ExCSS.Property property = (from declaration in styleRule.Declarations.Properties
                                       where declaration.Name == propertyName
                                       select declaration).FirstOrDefault();
            if (property != null && property.Term != null)
                return property.Term.ToString().Replace("p_p", "&#");
            return "none";
        }
        /// <MetaDataID>{f305864c-e03b-4cb4-9da5-cdb373f1fa4c}</MetaDataID>
        ExCSS.Property GetStyleRuleProperty(ExCSS.StyleRule styleRule, string propertyName)
        {
            ExCSS.Property property = (from declaration in styleRule.Declarations.Properties
                                       where declaration.Name == propertyName
                                       select declaration).FirstOrDefault();
            //if (property != null && property.Term != null)
            //    return property;
            return property;
        }
        /// <MetaDataID>{63f471cb-9ca9-4c5a-b398-bdb0c870eb72}</MetaDataID>
        public readonly string layout;
        /// <MetaDataID>{5b06503b-60c6-44d7-a24a-95352ff75181}</MetaDataID>
        public readonly string currency;
        /// <MetaDataID>{78e2302a-2590-4210-8a4a-0334345b2c6a}</MetaDataID>
        public readonly string priceleader;
        /// <MetaDataID>{8a8953c2-98a0-41e5-82ac-e15b5a7bcb21}</MetaDataID>
        public readonly string dotspaceprice;
        /// <MetaDataID>{a8c50285-ceae-44a1-a6eb-06dc2b8a6e27}</MetaDataID>
        public readonly string dotspaceitem;
        /// <MetaDataID>{a38c495d-e78c-41e7-b43b-b4cd4b172d3b}</MetaDataID>
        public readonly string dotspacebetween;
        /// <MetaDataID>{d1c0c1dd-13de-4d7e-beec-f032af4c547c}</MetaDataID>
        public readonly string dotsmatchnamecolor;
        /// <MetaDataID>{c5510759-d93f-4c73-9a9f-1dfe5851f732}</MetaDataID>
        public readonly string spacebetweenprices;
    }

    /// <MetaDataID>{4391d2cd-11a3-4c5a-b288-350b2cf4f501}</MetaDataID>
    public class MenuProBorder
    {
        XElement BorderElement = null;
        public MenuProBorder(XElement borderElement)
        {
            BorderElement = borderElement;
        }
        public string Description
        {
            get
            {
                return BorderElement.Value;
            }
        }

        public string FileName
        {
            get
            {
                return BorderElement.Attribute("value").Value;
            }
        }
    }


}


