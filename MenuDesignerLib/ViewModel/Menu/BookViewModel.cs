using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using FlavourBusinessToolKit;
using GenWebBrowser;
using FLBManager.ViewModel;
using MenuPresentationModel;
using MenuPresentationModel.MenuCanvas;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using MenuDesigner.Views;
using System.Windows;
using RoutedCommand = WPFUIElementObjectBind.RoutedCommand;
using System.IO;
using SvgAccentModifier;
using OOAdvantech.Json;

namespace MenuDesigner.ViewModel.MenuCanvas
{



    /// <MetaDataID>{fe45389d-e764-41a0-9cf1-c6476ffe25d2}</MetaDataID>
    public class BookViewModel : OOAdvantech.UserInterface.Runtime.PresentationObject<MenuPresentationModel.RestaurantMenu>, INotifyPropertyChanged, FlavourBusinessUI.ViewModel.IScaledArea
    {

        public string MenuName
        {
            get
            {

                return GraphicMenustorageRef.Name;
            }

        }
        public string DefaultLanguage
        {
            get
            {
                string defLang = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RealObject).StorageMetaData.Culture;
                return string.Format(Properties.Resources.GraphicMenuDefaultLanguageLabel, defLang);
            }
        }
        public BookViewModel() : base(default(MenuPresentationModel.RestaurantMenu))
        {
        }
        public RelayCommand ClickPseudoCommand { get; private set; } = new RelayCommand((object sender) => { });
        public RelayCommand PreviewCommand { get; private set; }

        public RelayCommand PublishCommand { get; private set; }

        public bool PublishAllowed { get; set; }

        public readonly FlavourBusinessFacade.OrganizationStorageRef GraphicMenustorageRef;
        /// <MetaDataID>{b7e805c6-e1a0-4341-871a-fecec35256b1}</MetaDataID>
        public BookViewModel(MenuPresentationModel.RestaurantMenu menu, FlavourBusinessFacade.OrganizationStorageRef graphicMenustorageRef)
                    : base(menu)
        {
            GraphicMenustorageRef = graphicMenustorageRef;



            StyleSelectionCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.StyleSelection(); });
            BeforeSaveCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { menu.Name = MenuName; });

            if (menu.Style != null)
            {
                var style = menu.Style;

                MenuStylesheet = style as MenuPresentationModel.MenuStyles.StyleSheet;
                MenuStylesheet.ObjectChangeState += MenuStylesheetChangeState;
                RestaurantMenu.ConntextStyleSheet = MenuStylesheet;
                _SelectedMenuStyle = MenuStylesheet.OrgStyleSheet;

                if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("page"))
                    (MenuStylesheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged += PageStyle_PropertyChanged;
                if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("menu-item"))
                    (MenuStylesheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).ObjectChangeState += MenuItemStyle_PropertyChanged;
                if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("heading"))
                    (MenuStylesheet.Styles["heading"] as MenuPresentationModel.MenuStyles.HeadingStyle).ObjectChangeState += HeadingStyle_ObjectChangeState1;


                if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("layout"))
                    (MenuStylesheet.Styles["layout"] as MenuPresentationModel.MenuStyles.LayoutStyle).PropertyChanged += LayoutStyle_PropertyChanged;
                if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("price-options"))
                    (MenuStylesheet.Styles["price-options"] as MenuPresentationModel.MenuStyles.PriceStyle).ObjectChangeState += BookViewModel_PropertyChanged;

                PagePresenttion.UpdateCanvasItems(RealObject.Pages.OfType<MenuPage>().ToArray()[0]);
                CheckMenuItems();


            }
            else
            {
                if (MenuPresentationModel.MenuStyles.StyleSheet.StyleSheets.Count > 0)
                    SelectedMenuStyle = MenuPresentationModel.MenuStyles.StyleSheet.StyleSheets[0];

            }
            menu.MenuCanvasItemChanged += MenuCanvasItemChanged;
            menu.ObjectChangeState += RestaurantMenuChangeState;

            PreviousPageCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.MoveToPreviousPage(); });
            NextPageCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.MoveToNextPage(); });
            ShrinkLineSpaceCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.ShrinkLineSpace(); }, (object seneder) => { return !IsReadonly; });
            ExpandLineSpaceCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.ExpandLineSpace(); }, (object seneder) => { return !IsReadonly; });
            ResetLineSpaceCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.ResetLineSpace(); }, (object seneder) => { return !IsReadonly; });
            ShrinkPageFontsSizesCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.ShrinkPageFontsSizes(); }, (object seneder) => { return !IsReadonly; });
            ExpandPageFontsSizesCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.ExpandPageFontsSizes(); }, (object seneder) => { return !IsReadonly; });
            ResetPageFontsSizesCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.ResetPageFontsSizes(); }, (object seneder) => { return !IsReadonly; });
            PreviewCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.Preview(); });
            PublishCommand = new WPFUIElementObjectBind.RelayCommand((object seneder) => { this.Publish(); });
            //Enum.GetValues(AbstractionsAndPersistency.OrderState)

        }



        private void Publish()
        {
            if (PublishAllowed)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            ClickPseudoCommand.UserInterfaceObjectConnection.Save();
                            (GraphicMenustorageRef.UploadService as FlavourBusinessFacade.IResourceManager).PublishMenu(GraphicMenustorageRef);
                        }));
                    });
                    stateTransition.Consistent = true;
                }

            }

        }

        internal void CreateMenuPreview(string menuRoot, string menuName, Dictionary<string, MemoryStream> graphickMenuResources)
        {

            string menuResourcesPrefix = "";
            string serverStorageFolder = MenuPresentationModel.MenuStyles.PageStyle.ResourcesRootPath;
            IFileManager fileManager = null;
            string rootUri = MenuPresentationModel.MenuStyles.PageStyle.ResourcesRootPath;
            if (fileManager != null)
                rootUri = fileManager.RootUri;
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
            if (!String.IsNullOrWhiteSpace(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RealObject).StorageMetaData.Culture))
                culture = System.Globalization.CultureInfo.GetCultureInfo(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RealObject).StorageMetaData.Culture);

            MenuPresentationModel.JsonMenuPresentation.RestaurantMenu jsonRestaurantMenu = null;
            using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, true))
            {
                jsonRestaurantMenu = new MenuPresentationModel.JsonMenuPresentation.RestaurantMenu(RealObject);


                Dictionary<string, string> pageImages = new Dictionary<string, string>();

                foreach (var page in (from pages in jsonRestaurantMenu.MultilingualPages.Values.Values.OfType<List<IMenuPageCanvas>>() from page in pages.OfType<MenuPresentationModel.JsonMenuPresentation.MenuPageCanvas>() select page))
                {
                    if (page.Background != null)
                    {
                        if (page.Background.LandscapeImage != null && !string.IsNullOrWhiteSpace(page.Background.LandscapeImage.Uri))
                            pageImages[page.Background.LandscapeImage.Uri] = page.Background.LandscapeImage.Uri;

                        if (page.Background.PortraitImage != null && !string.IsNullOrWhiteSpace(page.Background.PortraitImage.Uri))
                            pageImages[page.Background.PortraitImage.Uri] = page.Background.PortraitImage.Uri;
                    }

                    if (page.Border != null)
                    {

                        if (page.Border.LandscapeImage != null && !string.IsNullOrWhiteSpace(page.Border.LandscapeImage.Uri))
                            pageImages[page.Border.LandscapeImage.Uri] = page.Border.LandscapeImage.Uri;

                        if (page.Border.PortraitImage != null && !string.IsNullOrWhiteSpace(page.Border.PortraitImage.Uri))
                            pageImages[page.Border.PortraitImage.Uri] = page.Border.PortraitImage.Uri;
                    }
                }

                foreach (string uri in pageImages.Keys.ToArray())
                {
                    var aboluteImageUri = rootUri + uri.Replace("/", @"\");
                    var imageFileName = aboluteImageUri.Substring(aboluteImageUri.LastIndexOf('\\') + 1);
                    string newImageUri = menuRoot + imageFileName;

                    MemoryStream ms = new MemoryStream();
                    using (FileStream file = new FileStream(aboluteImageUri, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        ms.Write(bytes, 0, (int)file.Length);
                    }
                    ms.Position = 0;
                    graphickMenuResources[newImageUri.ToLower()] = ms;
                    pageImages[uri] = menuResourcesPrefix + imageFileName;

                }
                foreach (var page in (from pages in jsonRestaurantMenu.MultilingualPages.Values.Values.OfType<List<IMenuPageCanvas>>() from page in pages.OfType<MenuPresentationModel.JsonMenuPresentation.MenuPageCanvas>() select page))
                {
                    if (page.Background != null)
                    {
                        if (page.Background.LandscapeImage != null && !string.IsNullOrWhiteSpace(page.Background.LandscapeImage.Uri))
                            page.Background.LandscapeImage = new MenuPresentationModel.MenuStyles.Resource() { Name = page.Background.LandscapeImage.Name, Uri = pageImages[page.Background.LandscapeImage.Uri], TimeStamp = DateTime.Now };
                        if (page.Background.PortraitImage != null && !string.IsNullOrWhiteSpace(page.Background.PortraitImage.Uri))
                            page.Background.PortraitImage = new MenuPresentationModel.MenuStyles.Resource() { Name = page.Background.PortraitImage.Name, Uri = pageImages[page.Background.PortraitImage.Uri], TimeStamp = DateTime.Now };
                    }
                    if (page.Border != null)
                    {
                        if (page.Border.LandscapeImage != null && !string.IsNullOrWhiteSpace(page.Border.LandscapeImage.Uri))
                            page.Border.LandscapeImage = new MenuPresentationModel.MenuStyles.Resource() { Name = page.Border.LandscapeImage.Name, Uri = pageImages[page.Border.LandscapeImage.Uri], TimeStamp = DateTime.Now };
                        if (page.Border.PortraitImage != null && !string.IsNullOrWhiteSpace(page.Border.PortraitImage.Uri))
                            page.Border.PortraitImage = new MenuPresentationModel.MenuStyles.Resource() { Name = page.Border.PortraitImage.Name, Uri = pageImages[page.Border.PortraitImage.Uri], TimeStamp = DateTime.Now };
                    }
                }


                List<string> menuAccentImages = new List<string>();

                var headingsAccentImages = (from menuCanvasAccent in (from heading in jsonRestaurantMenu.MenuCanvasItems.OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasHeading>()
                                                                      select heading.Accent).OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasHeadingAccent>()
                                            where menuCanvasAccent != null && menuCanvasAccent.Accent != null && !menuCanvasAccent.MultipleItemsAccent
                                            from accentImageEntry in (menuCanvasAccent.Accent as MenuPresentationModel.JsonMenuPresentation.HeadingAccent).MultilingualAccentImages.Values
                                            where accentImageEntry.Value is List<MenuPresentationModel.MenuStyles.IImage>
                                            from accentImage in accentImageEntry.Value as List<MenuPresentationModel.MenuStyles.IImage>
                                            select new { accentImage, color = (menuCanvasAccent).AccentColor, imageUri = accentImage.Uri }).ToList();

                headingsAccentImages.AddRange((from menuCanvasAccent in (from foodItem in jsonRestaurantMenu.MenuCanvasItems.OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasFoodItem>()
                                                                         select foodItem.Accent).OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasHeadingAccent>()
                                               where (menuCanvasAccent) != null && (menuCanvasAccent).Accent != null && !menuCanvasAccent.MultipleItemsAccent
                                               from accentImageEntry in (menuCanvasAccent.Accent as MenuPresentationModel.JsonMenuPresentation.HeadingAccent).MultilingualAccentImages.Values
                                               where accentImageEntry.Value is List<MenuPresentationModel.MenuStyles.IImage>
                                               from accentImage in accentImageEntry.Value as List<MenuPresentationModel.MenuStyles.IImage>
                                               select new { accentImage, color = (menuCanvasAccent).AccentColor, imageUri = accentImage.Uri }).ToList());


                var colorHeadingAccentImages = (from headingAccentImage in headingsAccentImages
                                                group headingAccentImage by new { headingAccentImage.imageUri, headingAccentImage.color } into accentImages
                                                select new { accentImageAsKey = accentImages.Key, accentImages });

                foreach (var colorHeadingAccentImage in colorHeadingAccentImages)
                {
                    //colorHeadingAccentImage.accentImageAsKey.
                    var aboluteImageUri = rootUri + colorHeadingAccentImage.accentImageAsKey.imageUri.Replace("/", @"\"); ;

                    var imageFileName = aboluteImageUri.Substring(aboluteImageUri.LastIndexOf('\\') + 1);
                    //var accebtImageDoc = XDocument.Load(aboluteImageUri);
                    SharpVectors.Dom.Svg.SvgDocument accebtImageDoc = new SharpVectors.Dom.Svg.SvgDocument(null);
                    accebtImageDoc.Load(aboluteImageUri);
                    ISvgModifier svgModifier = SvgModifier.GetModifier(accebtImageDoc.DocumentElement as SharpVectors.Dom.Svg.SvgSvgElement);
                    if (!string.IsNullOrWhiteSpace(colorHeadingAccentImage.accentImageAsKey.color) && colorHeadingAccentImage.accentImageAsKey.color != "none")
                        svgModifier.Color = colorHeadingAccentImage.accentImageAsKey.color;

                    //SvgUtilities.SetColor(accebtImageDoc, colorHeadingAccentImage.accentImageAsKey.color);
                    MemoryStream accentImageStream = new MemoryStream();
                    accebtImageDoc.Save(accentImageStream);
                    accentImageStream.Position = 0;
                    int i = 1;
                    string menuImageFile = imageFileName;
                    while (menuAccentImages.Contains(menuImageFile))
                    {
                        menuImageFile = "a" + i.ToString() + imageFileName;
                        i++;
                    }
                    menuAccentImages.Add(imageFileName);

                    string targetImageUri = menuRoot + menuImageFile;
                    string newImageUri = menuResourcesPrefix + menuImageFile;

                    accentImageStream.Position = 0;
                    graphickMenuResources[targetImageUri.ToLower()] = accentImageStream;


                    foreach (var accentImage in colorHeadingAccentImage.accentImages)
                    {
                        (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Uri = newImageUri;
                        (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Image = new MenuPresentationModel.MenuStyles.Resource()
                        {
                            Name = (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Image.Name,
                            Uri = newImageUri
                        };
                    }
                }


                var multipleItemsAccentAccentImages = (from menuCanvasAccent in (from heading in jsonRestaurantMenu.MenuCanvasItems.OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasHeading>()
                                                                                 select heading.Accent).OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasHeadingAccent>()
                                                       where menuCanvasAccent != null && menuCanvasAccent.Accent != null && menuCanvasAccent.MultipleItemsAccent
                                                       from accentImageEntry in (menuCanvasAccent.Accent as MenuPresentationModel.JsonMenuPresentation.HeadingAccent).MultilingualAccentImages.Values
                                                       where accentImageEntry.Value is List<MenuPresentationModel.MenuStyles.IImage>
                                                       from accentImage in accentImageEntry.Value as List<MenuPresentationModel.MenuStyles.IImage>
                                                       select new { accentImage, color = (menuCanvasAccent as MenuPresentationModel.JsonMenuPresentation.MenuCanvasHeadingAccent).AccentColor, imageUri = accentImage.Uri, (accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Size }).ToList();

                multipleItemsAccentAccentImages.AddRange((from menuCanvasAccent in (from foodItem in jsonRestaurantMenu.MenuCanvasItems.OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasFoodItem>()
                                                                                    select foodItem.Accent).OfType<MenuPresentationModel.JsonMenuPresentation.MenuCanvasHeadingAccent>()
                                                          where (menuCanvasAccent) != null && (menuCanvasAccent).Accent != null && menuCanvasAccent.MultipleItemsAccent
                                                          from accentImageEntry in (menuCanvasAccent.Accent as MenuPresentationModel.JsonMenuPresentation.HeadingAccent).MultilingualAccentImages.Values
                                                          where accentImageEntry.Value is List<MenuPresentationModel.MenuStyles.IImage>
                                                          from accentImage in accentImageEntry.Value as List<MenuPresentationModel.MenuStyles.IImage>
                                                          select new { accentImage, color = (menuCanvasAccent).AccentColor, imageUri = accentImage.Uri, (accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Size }).ToList());







                var colorMultipleItemsAccentAccentImages = (from headingAccentImage in multipleItemsAccentAccentImages
                                                            group headingAccentImage by new { headingAccentImage.imageUri, headingAccentImage.color, headingAccentImage.Size } into accentImages
                                                            select new { accentImageAsKey = accentImages.Key, accentImages });

                foreach (var colorHeadingAccentImage in colorMultipleItemsAccentAccentImages)
                {
                    //colorHeadingAccentImage.accentImageAsKey.
                    var aboluteImageUri = rootUri + colorHeadingAccentImage.accentImageAsKey.imageUri.Replace("/", @"\");


                    var imageFileName = aboluteImageUri.Substring(aboluteImageUri.LastIndexOf('\\') + 1);
                    //var accebtImageDoc = XDocument.Load(aboluteImageUri);
                    //SvgUtilities.SetColor(accebtImageDoc, colorHeadingAccentImage.accentImageAsKey.color);


                    SharpVectors.Dom.Svg.SvgDocument accebtImageDoc = new SharpVectors.Dom.Svg.SvgDocument(null);
                    accebtImageDoc.Load(aboluteImageUri);
                    ISvgModifier svgModifier = SvgModifier.GetModifier(accebtImageDoc.DocumentElement as SharpVectors.Dom.Svg.SvgSvgElement);

                    if (!string.IsNullOrWhiteSpace(colorHeadingAccentImage.accentImageAsKey.color) && colorHeadingAccentImage.accentImageAsKey.color != "none")
                        svgModifier.Color = colorHeadingAccentImage.accentImageAsKey.color;

                    svgModifier.Size = colorHeadingAccentImage.accentImageAsKey.Size;
                    MemoryStream accentImageStream = new MemoryStream();
                    accebtImageDoc.Save(accentImageStream);
                    accentImageStream.Position = 0;

                    int i = 1;
                    string menuImageFile = imageFileName;
                    while (menuAccentImages.Contains(menuImageFile))
                    {
                        menuImageFile = "a" + i.ToString() + imageFileName;
                        i++;
                    }
                    menuAccentImages.Add(imageFileName);

                    string targetImageUri = menuRoot + menuImageFile;
                    string newImageUri = menuResourcesPrefix + menuImageFile;
                    accentImageStream.Position = 0;
                    graphickMenuResources[targetImageUri.ToLower()] = accentImageStream;

                    //if (fileManager != null)
                    //    fileManager.Upload(targetImageUri, accentImageStream, "image/svg+xml");

                    foreach (var accentImage in colorHeadingAccentImage.accentImages)
                    {
                        (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Uri = newImageUri;
                        (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Image = new MenuPresentationModel.MenuStyles.Resource()
                        {
                            Name = (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Image.Name,
                            Uri = newImageUri
                        };
                    }
                }
            }



            string json = JsonConvert.SerializeObject(jsonRestaurantMenu, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.All });

            try
            {
                //var jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { ReferenceLoopHandling = OOAdvantech.Json.ReferenceLoopHandling.Serialize, TypeNameHandling = OOAdvantech.Json.TypeNameHandling.None, Binder = new OOAdvantech.Remoting.RestApi.Serialization.SerializationBinder(OOAdvantech.Remoting.RestApi.Serialization.JsonSerializationFormat.TypeScriptJsonSerialization), ContractResolver = new OOAdvantech.Remoting.RestApi.Serialization.JsonContractResolver(OOAdvantech.Remoting.RestApi.Serialization.JsonContractType.Serialize, null, OOAdvantech.Remoting.RestApi.Serialization.JsonSerializationFormat.TypeScriptJsonSerialization), ReferenceResolver = new OOAdvantech.Remoting.RestApi.Serialization.ReferenceResolver() };

                //jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                //jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

                //string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(jsonRestaurantMenu, jSetttings);

                //jSetttings = new OOAdvantech.Json.JsonSerializerSettings() { TypeNameHandling = OOAdvantech.Json.TypeNameHandling.None, ContractResolver = new OOAdvantech.Remoting.RestApi.Serialization.JsonContractResolver(OOAdvantech.Remoting.RestApi.Serialization.JsonContractType.Deserialize, null, OOAdvantech.Remoting.RestApi.Serialization.JsonSerializationFormat.TypeScriptJsonSerialization) };

                //jSetttings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK";
                //jSetttings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                //jsonEx = System.IO.File.ReadAllText(@"C:\menu.txt");

                //var sss = OOAdvantech.Json.JsonConvert.DeserializeObject<MenuPresentationModel.JsonMenuPresentation.RestaurantMenu>(jsonEx, jSetttings);

                ////var ddf = JsonConvert.DeserializeObject<MenuPresentationModel.JsonMenuPresentation.RestaurantMenu>(json, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.All });

            }
            catch (Exception error)
            {


            }
            MemoryStream jsonRestaurantMenuStream = new MemoryStream();

            byte[] jsonBuffer = System.Text.Encoding.UTF8.GetBytes(json);
            jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
            jsonRestaurantMenuStream.Position = 0;
            string jsonFileName = menuRoot + menuName + ".json";
            graphickMenuResources[jsonFileName.ToLower()] = jsonRestaurantMenuStream;


        }



        private void Preview()
        {
            var menuDesignerPage = PreviewCommand.UserInterfaceObjectConnection.ContainerControl as StyleableWindow.PageDialogViewEmulator;
            if (menuDesignerPage == null)
                menuDesignerPage = WPFUIElementObjectBind.ObjectContext.FindParent<StyleableWindow.PageDialogViewEmulator>(PreviewCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);

            var graphicMenuPreviewPage = new Views.GraphicMenuPreviewPage();
            graphicMenuPreviewPage.GetObjectContext().SetContextInstance(this);
            menuDesignerPage.NavigationWindow.Navigate(graphicMenuPreviewPage);
        }

        internal void MoveUpMenuCanvasItem(IMenuCanvasItem menuCanvasItem, MenuPage menuPage)
        {
            if (menuPage.MoveCanvasItemUp(menuCanvasItem))
            {


                MenuItemMoveOnPage(menuPage, menuCanvasItem);
            }
        }


        internal void MoveDownMenuCanvasItem(IMenuCanvasItem menuCanvasItem, MenuPage menuPage)
        {
            if (menuPage.MoveCanvasItemDown(menuCanvasItem))
                MenuItemMoveOnPage(menuPage, menuCanvasItem);
        }

        internal void StyleSelection()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                System.Windows.Window win = Window.GetWindow(StyleSelectionCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
                MenuStyleWindow menuStyleWindow = new MenuStyleWindow();
                menuStyleWindow.Owner = win;
                menuStyleWindow.GetObjectContext().SetContextInstance(this);

                if (menuStyleWindow.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }




            // SelectionService.TitleHeadingFonts(sender, e);
        }
        private void HeadingStyle_ObjectChangeState1(object _object, string member)
        {
            if (member == nameof(MenuPresentationModel.MenuStyles.IHeadingStyle.Accent))
            {
                RebuildAllPages();
            }
        }

        private void BookViewModel_ObjectChangeState(object _object, string member)
        {
            throw new NotImplementedException();
        }

        internal void RemoveMenuCanvasItem(IMenuCanvasItem menuCanvasItem, MenuPage menuPage)
        {
            menuCanvasItem.Remove();

            RebuildPageAndAllToTherRight(menuPage);
        }
        public static WebBrowserOverlay HtmlView;





        /// <exclude>Excluded</exclude>
        MenuHeadingsPresentation _MenuHeadings;
        public MenuHeadingsPresentation MenuHeadings
        {
            get
            {
                if (_MenuHeadings == null)
                    _MenuHeadings = new MenuHeadingsPresentation(RealObject);
                return _MenuHeadings;
            }
        }




        /// <summary>
        /// Informs restaurant menu viewmodel that a item has change its position on page
        /// </summary>
        /// <param name="bookPageViewModel">
        /// Defines the page where item is
        /// </param>
        internal void MenuItemMoveOnPage(MenuPage menuPage, IMenuCanvasItem menuCanvasItem)
        {
            if (menuPage.MenuCanvasItems.IndexOf(menuCanvasItem) == 0 && AllPages.IndexOf(menuPage) > 0)//First item in page 
                menuPage = AllPages[AllPages.IndexOf(menuPage) - 1];
            RebuildPageAndAllToTherRight(menuPage);
        }

        //private void RebuildPage(MenuPage menuPage)
        //{
        //    var menuCanvasItems = menuPage.MenuCanvasItems.ToList();
        //    menuPage.RenderMenuCanvasItems(menuCanvasItems);
        //    if (SeletedMenuPage == menuPage)
        //        PagePresenttion.UpdateCanvasItems(menuPage);
        //}


        public int PageNumber
        {
            get
            {
                if (SeletedMenuPage != null)
                    return AllPages.IndexOf(SeletedMenuPage) + 1;
                return 1;
            }
        }

        public int PagesCount
        {
            get
            {

                return AllPages.Count;

            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<MenuPage> _SelectedMenuPage = new OOAdvantech.MultilingualMember<MenuPage>();
        MenuPage SeletedMenuPage
        {


            get
            {
                if (_SelectedMenuPage.Value == null)
                {
                    if (AllPages.Count > 0)
                    {
                        _SelectedMenuPage.Value = AllPages[0];
                        SelectedPageCulture = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                    }
                }
                if (SelectedPageCulture != OOAdvantech.CultureContext.CurrentCultureInfo || UseDefaultCultureValue != OOAdvantech.CultureContext.UseDefaultCultureValue)
                {
                    SelectedPageCulture = OOAdvantech.CultureContext.CurrentCultureInfo;
                    UseDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                }
                return _SelectedMenuPage;
            }
            set
            {
                if (_SelectedMenuPage.Value != value)
                {
                    _SelectedMenuPage.Value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageNumber)));
                }
            }
        }

        private void RebuildPageAndAllToTherRight(MenuPage menuPage)
        {
            if (IsReadonly)
                return;

            bool refreshSelected = false;
            List<IMenuCanvasItem> menuCanvasItems = new List<IMenuCanvasItem>();// bookPageViewModel.MenuPage.MenuCanvasItems.ToList();
            var allPages = AllPages;
            IMenuCanvasItem firstMenuCanvasItem = null;


            int npos = AllPages.IndexOf(menuPage);
            for (; npos < allPages.Count; npos++)
            {
                foreach (var menuCanvasItem in allPages[npos].MenuCanvasItems)
                {
                    firstMenuCanvasItem = menuCanvasItem;
                    break;
                }
                if (firstMenuCanvasItem != null)
                    break;
            }
            if (firstMenuCanvasItem != null)
            {
                int firstItemPos = RestaurantMenu.MenuCanvasItems.IndexOf(firstMenuCanvasItem);
                var listoFItems = RestaurantMenu.MenuCanvasItems.ToList();
                menuCanvasItems = listoFItems.GetRange(firstItemPos, listoFItems.Count - firstItemPos);
            }



            npos = AllPages.IndexOf(menuPage);
            for (; npos < allPages.Count; npos++)
            {
                int numOfPageMenuCanvasItems = allPages[npos].MenuCanvasItems.Count;
                if (menuCanvasItems.Count == 0 && AllPages.Count > 1)
                {
                    if (SeletedMenuPage == allPages[npos])
                    {
                        int previusPageIndex = RestaurantMenu.Pages.IndexOf(allPages[npos]) - 1;
                        if (previusPageIndex >= 0)
                        {
                            SeletedMenuPage = RestaurantMenu.Pages[previusPageIndex] as MenuPage;
                            refreshSelected = true;
                        }
                    }
                    RestaurantMenu.RemovePage(allPages[npos]);
                    // RestaurantMenu.ReBuildMenuPages();
                }
                else
                {
                    allPages[npos].RenderMenuCanvasItems(menuCanvasItems);
                    if (allPages[npos] == SeletedMenuPage)
                        refreshSelected = true;

                    if (allPages[npos].MenuCanvasItems.Count == numOfPageMenuCanvasItems)
                        break;

                    while (npos == allPages.Count - 1 && menuCanvasItems.Count > 0)
                    {
                        var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu);
                        MenuPresentationModel.MenuPage newPage = new MenuPage();
                        objectStorage.CommitTransientObjectState(newPage);
                        RestaurantMenu.AddPage(newPage);
                        newPage.RenderMenuCanvasItems(menuCanvasItems);

                    }
                }
            }
            RestaurantMenu.RemoveBlankPages();
            if (refreshSelected)
                PagePresenttion.UpdateCanvasItems(SeletedMenuPage);

            CheckMenuItems();

        }

        private void CheckMenuItems()
        {
            var menuItemsDes = (from page in AllPages
                                from menuItem in page.MenuCanvasItems
                                select menuItem.Description).ToArray();

            var menuItemsDes2 = (from menuItem in this.RestaurantMenu.MenuCanvasItems
                                 select menuItem.Description).ToArray();
            int i = 0;
            foreach (var menuItem in menuItemsDes)
            {
                if (menuItemsDes2.Length > i)
                {
                    if (menuItem != menuItemsDes2[i])
                    {

                    }
                    System.Diagnostics.Debug.WriteLine(menuItem + " " + menuItemsDes2[i++]);
                }
                else
                    System.Diagnostics.Debug.WriteLine(menuItem);
            }
        }

        public System.Globalization.CultureInfo SelectedPageCulture;

        bool UseDefaultCultureValue;

        int RebuildAlreadRuns = 0;
        bool ContinueRebuild;

        public bool IsReadonly
        {
            get
            {
                return OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu) == null || OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu).IsReadonly;
            }
        }
        internal void RebuildAllPages()
        {
            if (IsReadonly)
                return;


            //var items = RestaurantMenu.MenuCanvasItems.ToList().Select(x => x.Description).ToList();

            //string output=null;
            //foreach(var item in  RestaurantMenu.MenuCanvasItems)
            //{
            //   var s_ref =  OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(item);
            //    if(s_ref!=null)
            //    {
            //        if (output != null)
            //            output += System.Environment.NewLine;
            //        output += s_ref.ObjectID.GetPartValue(0).ToString() + " :  " + item.Description;
            //    }


            //}



            if (RebuildAlreadRuns == 0)
            {
                RebuildAlreadRuns++;
                var culture = OOAdvantech.CultureContext.CurrentCultureInfo;
                var useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                Transaction.RunAsynch(new Action(() =>
                 {



                     try
                     {
                         using (var cultureConext = new OOAdvantech.CultureContext(culture, useDefaultCultureValue))
                         {
                             do
                             {
                                 try
                                 {
                                     lock (this)
                                     {
                                         var menuCanvasItems = RestaurantMenu.MenuCanvasItems.ToList();
                                         var sitems = RestaurantMenu.MenuCanvasItems.Select(x => new { x, x.Description, id = OOAdvantech.PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(x)?.PersistentObjectID?.ToString() }).ToArray();

                                         foreach (var s in sitems)
                                         {
                                             System.Diagnostics.Debug.WriteLine(s.id + "," + s.Description);

                                         }
                                         RestaurantMenu.CheckMenuCanvasItemsIndexes();

                                         var itemsMultiplePriceHeadings = (from MenuCanvasFoodItem foodItem in menuCanvasItems.OfType<MenuCanvasFoodItem>()
                                                                           where foodItem.MultiPriceHeading != null
                                                                           select foodItem.MultiPriceHeading).Distinct().ToList();
                                         foreach (var itemMultiplePriceHeading in itemsMultiplePriceHeadings)
                                             itemMultiplePriceHeading.ObjectChangeState -= MultiplePriceHeadingsChangeState;


                                         //menuCanvasItems.AddRange(itemsMultiplePriceHeadings);
                                         if (RestaurantMenu.Pages.Count == 0)
                                         {
                                             var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu);
                                             MenuPresentationModel.MenuPage newPage = new MenuPage();
                                             objectStorage.CommitTransientObjectState(newPage);
                                             RestaurantMenu.AddPage(newPage);
                                         }
                                         List<MenuPage> allPages = RestaurantMenu.Pages.OfType<MenuPage>().ToList();

                                         List<MenuPage> selectedPageAndAllToTheLeft = new List<MenuPage>();

                                         foreach (var page in RestaurantMenu.Pages.OfType<MenuPage>())
                                         {
                                             selectedPageAndAllToTheLeft.Add(page);
                                             allPages.Remove(page);
                                             if (PagePresenttion.MenuPage == page)
                                                 break;
                                         }
                                         foreach (var page in selectedPageAndAllToTheLeft)
                                             page.RenderMenuCanvasItems(menuCanvasItems);

                                         if (SeletedMenuPage.MenuCanvasItems.Count == 0 && RestaurantMenu.Pages.IndexOf(SeletedMenuPage) != 0)
                                             SeletedMenuPage = AllPages[AllPages.IndexOf(SeletedMenuPage) - 1];

                                         PagePresenttion.UpdateCanvasItems(SeletedMenuPage);


                                         while (menuCanvasItems.Count > 0)
                                         {
                                             if (allPages.Count == 0)
                                             {
                                                 var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu);
                                                 MenuPresentationModel.MenuPage newPage = new MenuPage();
                                                 objectStorage.CommitTransientObjectState(newPage);
                                                 RestaurantMenu.AddPage(newPage);
                                                 newPage.RenderMenuCanvasItems(menuCanvasItems);
                                             }
                                             else
                                             {
                                                 foreach (var page in allPages.ToList())
                                                 {
                                                     if (page.Menu != null)
                                                     {
                                                         page.RenderMenuCanvasItems(menuCanvasItems);
                                                         allPages.Remove(page);
                                                     }
                                                     else
                                                     {

                                                     }
                                                 }
                                             }
                                         }
                                         RestaurantMenu.RemoveBlankPages();

                                         foreach (var itemMultiplePriceHeading in itemsMultiplePriceHeadings)
                                             itemMultiplePriceHeading.ObjectChangeState -= MultiplePriceHeadingsChangeState;

                                         itemsMultiplePriceHeadings = (from MenuCanvasFoodItem foodItem in RestaurantMenu.MenuCanvasItems.OfType<MenuCanvasFoodItem>()
                                                                       where foodItem.MultiPriceHeading != null
                                                                       select foodItem.MultiPriceHeading).Distinct().ToList();
                                         foreach (var itemMultiplePriceHeading in itemsMultiplePriceHeadings)
                                         {
                                             itemMultiplePriceHeading.ObjectChangeState -= MultiplePriceHeadingsChangeState;
                                             itemMultiplePriceHeading.ObjectChangeState += MultiplePriceHeadingsChangeState;
                                         }

                                     }
                                     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PagesCount)));

                                     RebuildAlreadRuns--;
                                 }
                                 catch (Exception error)
                                 {
                                     RebuildAlreadRuns--;
                                 }

                                 CheckMenuItems();

                             }
                             while (RebuildAlreadRuns > 0);
                         }
                     }
                     catch (OOAdvantech.Transactions.TransactionException error)
                     {
                     }
                     catch (Exception error)
                     {
                         RebuildAlreadRuns--;
                         throw;
                     }

                 }));
            }
            else
            {
                if (RebuildAlreadRuns < 4)
                    RebuildAlreadRuns++;
            }
        }

        private void MultiplePriceHeadingsChangeState(object _object, string member)
        {
            MenuCanvasItemChanged(_object as IMenuCanvasItem, member);
        }

        internal void MenuItemDropOnPage(MenuPage menuPage)
        {
            RebuildPageAndAllToTherRight(menuPage);
        }


        private void RestaurantMenuChangeState(object _object, string member)
        {
            if (member == nameof(MenuPresentationModel.RestaurantMenu.Pages))
            {
                if (PreviousPageMenuCommand != null)
                    PreviousPageMenuCommand.Header = PreviousPages;
                if (NextPageMenuCommand != null)
                    NextPageMenuCommand.Header = NextPages;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviousPages)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NextPages)));

                if (!RestaurantMenu.Pages.Contains(PagePresenttion.MenuPage))
                {
                    SeletedMenuPage = AllPages.Last();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                }
            }
        }
        //class MenuCanvasItemChangedData
        //{
        //    public IMenuCanvasItem menuCanvasitem;
        //    public string Member;
        //}
        //Dictionary<string, List<MenuCanvasItemChangedData>> TransactionCompleteMenuCanvasItemChangedData = new Dictionary<string, List<MenuCanvasItemChangedData>>();
        private void MenuCanvasItemChanged(IMenuCanvasItem menuCanvasitem, string member)
        {

            if (ClickPseudoCommand == null ||
                ClickPseudoCommand.UserInterfaceObjectConnection == null ||
                ClickPseudoCommand.UserInterfaceObjectConnection.State == OOAdvantech.UserInterface.Runtime.ViewControlObjectState.Passive)
                return;


            {
                //RunUnderTransaction != OOAdvantech.Transactions.Transaction.Current
                if (member == nameof(IMenuCanvasFoodItem.Description) ||
                  member == nameof(IMenuCanvasFoodItem.ExtraDescription) ||
                  member == nameof(IMenuCanvasFoodItem.Extras) ||
                  member == nameof(MenuCanvasFoodItem.AccentType) ||
                  member == nameof(IMenuCanvasHeading.Accent) ||
                  member == nameof(IMenuCanvasHeading.BeforeSpacing) ||
                  member == nameof(IMenuCanvasHeading.AfterSpacing) ||
                  member == nameof(IMenuCanvasHeading.CustomSpacing) ||
                  member == nameof(IMenuCanvasHeading.NextColumnOrPage) ||
                  (menuCanvasitem is IItemMultiPriceHeading && member == nameof(IItemMultiPriceHeading.PriceHeadingsAngle)) ||
                  (menuCanvasitem is IItemMultiPriceHeading && member == nameof(IItemMultiPriceHeading.PriceHeadingsBottomMargin)) ||
                  (menuCanvasitem is IItemMultiPriceHeading && member == nameof(IItemMultiPriceHeading.PriceHeadingsTopMargin)) ||
                  (menuCanvasitem is IItemMultiPriceHeading && member == nameof(IItemMultiPriceHeading.PriceHeadingsHorizontalPos)) ||
                  (menuCanvasitem is IItemMultiPriceHeading && member == nameof(IItemMultiPriceHeading.TransformOrigin)) ||
                  (menuCanvasitem is IItemMultiPriceHeading && member == null))
                {
                    if (menuCanvasitem.Page != null)
                    {
                        if (Transaction.Current != null && Transaction.Current != RunUnderTransaction && Transaction.Current.Status == TransactionStatus.Continue)
                        {

                            Transaction.Current.TransactionCompleted += MenuCanvasItemChanged_TransactionCompleted;
                        }
                        else
                        {
                            var menuCanvasItems = RestaurantMenu.MenuCanvasItems.ToList();
                            MenuPage menuPage = menuCanvasitem.Page as MenuPage;

                            if (menuCanvasitem is IMenuCanvasHeading &&
                                member == nameof(IMenuCanvasHeading.NextColumnOrPage) &&
                                !(menuCanvasitem as IMenuCanvasHeading).NextColumnOrPage &&
                                AllPages.IndexOf(menuPage) > 0)
                            {
                                menuPage = AllPages[AllPages.IndexOf(menuPage) - 1];
                            }
                            RebuildPageAndAllToTherRight(menuPage);
                        }
                    }
                }
            }

        }

        private void MenuCanvasItemChanged_TransactionCompleted(Transaction transaction)
        {
            transaction.TransactionCompleted -= MenuCanvasItemChanged_TransactionCompleted;
            if (RunUnderTransaction != null && RunUnderTransaction.Status == TransactionStatus.Continue)
            {
                using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(RunUnderTransaction))
                    {
                        RebuildAllPages();
                        stateTransition.Consistent = true;
                    }
                    suppressStateTransition.Consistent = true;
                }
            }


        }

        public List<MenuPresentationModel.MenuStyles.IStyleSheet> MenuStyles
        {
            get
            {
                return MenuPresentationModel.MenuStyles.StyleSheet.StyleSheets;
            }
        }





        public RestaurantMenu RestaurantMenu
        {
            get
            {
                return RealObject;
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.MultilingualSet<MenuPage> _Pages;// = new OOAdvantech.Collections.Generic.MultilingualSet<MenuPage>();

        object pagesLock = new object();


        /// <MetaDataID>{4a875097-2293-464d-be4d-dd45107e1543}</MetaDataID>
        public List<BookPageViewModel> Pages
        {
            get
            {
                if (PagePresenttion != null)
                    return new List<BookPageViewModel>() { PagePresenttion };
                else
                    return new List<BookPageViewModel>();
            }
        }

        public string PreviousPages
        {
            get
            {
                if (AllPages.IndexOf(SeletedMenuPage) > 0)
                    return AllPages.IndexOf(SeletedMenuPage).ToString();
                else
                    return "";
            }
        }

        public string NextPages
        {
            get
            {
                if (PagePresenttion == null || AllPages.IndexOf(SeletedMenuPage) == -1 || AllPages.IndexOf(SeletedMenuPage) == AllPages.Count - 1)
                    return "";
                return (AllPages.IndexOf(SeletedMenuPage) + 2).ToString();
            }
        }

        public List<MenuPage> AllPages
        {
            get
            {

                lock (pagesLock)
                {
                    if (_Pages == null)
                    {
                        _Pages = new OOAdvantech.Collections.Generic.MultilingualSet<MenuPage>();
                        // RebuildAllPages();
                    }
                    if (RealObject != null)
                    {
                        if (_Pages.Count == 0)
                            _Pages.AddRange(RealObject.Pages.OfType<MenuPage>());
                        else
                        {
                            _Pages.Clear();
                            _Pages.AddRange(RealObject.Pages.OfType<MenuPage>());
                        }
                    }
                }

                return _Pages.ToList();
            }
        }

        //public List<BookPageViewModel> AllPages
        //{
        //    get
        //    {
        //        lock (_Pages)
        //        {


        //            if (_Pages.Count == 0)
        //                _Pages.AddRange((from menuPage in RealObject.Pages select new BookPageViewModel(menuPage, this)).ToList());
        //            else
        //            {
        //                var menuPageMap = (from bookPage in _Pages select bookPage).ToDictionary(bookPage => bookPage.MenuPage);
        //                _Pages.Clear();
        //                foreach (var menuPage in RealObject.Pages.OfType<MenuPage>())
        //                {
        //                    BookPageViewModel bookPageViewModel = null;
        //                    if (menuPageMap.TryGetValue(menuPage, out bookPageViewModel))
        //                        _Pages.Add(bookPageViewModel);
        //                    else
        //                        _Pages.Add(new BookPageViewModel(menuPage, this));
        //                }
        //            }
        //        }


        //        return _Pages.ToList();
        //    }
        //}

        /// <exclude>Excluded</exclude>
        //OOAdvantech.MultilingualMember<BookPageViewModel> _SelectedPage = new OOAdvantech.MultilingualMember<BookPageViewModel>();

        /// <exclude>Excluded</exclude>
        BookPageViewModel _PagePresenttion;

        public BookPageViewModel PagePresenttion
        {
            get
            {
                if (_PagePresenttion == null)
                    _PagePresenttion = new BookPageViewModel(this);

                //if (_SelectedPage. == null)
                //{
                //    if (AllPages.Count > 0)
                //    {
                //        _SelectedPage.Value = AllPages[0];
                //        SelectedPageCulture = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
                //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                //    }
                //}
                //if (SelectedPageCulture != OOAdvantech.CultureContext.CurrentCultureInfo || UseDefaultCultureValue != OOAdvantech.CultureContext.UseDefaultCultureValue)
                //{
                //    SelectedPageCulture = OOAdvantech.CultureContext.CurrentCultureInfo;
                //    UseDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;

                //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                //}
                return _PagePresenttion;
            }
            //set
            //{
            //    if (_SelectedPage.Value != value)
            //    {
            //        _SelectedPage.Value = value;
            //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
            //    }
            //}
        }

        internal MenuPresentationModel.MenuStyles.IStyleSheet EditStyleSheet
        {
            get
            {
                if (MenuStylesheet != null)
                    return MenuStylesheet;
                else
                    return _SelectedMenuStyle;
            }
        }





        public MenuPresentationModel.MenuStyles.StyleSheet MenuStylesheet;

        MenuPresentationModel.MenuStyles.IStyleSheet _SelectedMenuStyle;
        public MenuPresentationModel.MenuStyles.IStyleSheet SelectedMenuStyle
        {
            get
            {
                return _SelectedMenuStyle;
            }
            set
            {

                SuspendUpdateFromStyleSheetChange = true;
                try
                {
                    if (_SelectedMenuStyle == null)
                    {

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            var objectStorage = ObjectStorage.GetStorageOfObject(RealObject);
                            _SelectedMenuStyle = value;
                            MenuStylesheet = (value as MenuPresentationModel.MenuStyles.StyleSheet).CreateDerivedStyleSheet();
                            MenuStylesheet.ObjectChangeState += MenuStylesheetChangeState;
                            RestaurantMenu.ConntextStyleSheet = MenuStylesheet;
                            var styl = RestaurantMenu.Style;
                            RestaurantMenu.Style = MenuStylesheet;
                            if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("page"))
                                (MenuStylesheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).PropertyChanged += PageStyle_PropertyChanged;

                            if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("menu-item"))
                                (MenuStylesheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle).ObjectChangeState += MenuItemStyle_PropertyChanged;

                            if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("layout"))
                                (MenuStylesheet.Styles["layout"] as MenuPresentationModel.MenuStyles.LayoutStyle).PropertyChanged += LayoutStyle_PropertyChanged;

                            if (MenuStylesheet != null && MenuStylesheet.Styles.ContainsKey("price-options"))
                                (MenuStylesheet.Styles["price-options"] as MenuPresentationModel.MenuStyles.PriceStyle).ObjectChangeState += BookViewModel_PropertyChanged;


                            objectStorage.CommitTransientObjectState(MenuStylesheet);
                            stateTransition.Consistent = true;
                        }

                    }
                    else
                    {
                        _SelectedMenuStyle = value;
                        MenuStylesheet.ChangeOrgStyle(value as MenuPresentationModel.MenuStyles.StyleSheet);
                    }
                }
                finally
                {
                    SuspendUpdateFromStyleSheetChange = false;
                }

                RebuildAllPages();


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedMenuStyle)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStyleName)));
            }
        }

        private void BookViewModel_PropertyChanged(object _object, string member)
        {
            if (member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.Layout) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.Font) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.PriceHeadingTransformOrigin) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.PriceHeadingAngle) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.PriceHeadingHorizontalPos) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.PriceHeadingsBottomMargin) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.MultiPriceSpacing) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.PriceLeader) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.ShowMultiplePrices) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.DotsSpaceFromItem) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.DotsSpaceFromPrice) ||
                member == nameof(MenuPresentationModel.MenuStyles.IPriceStyle.BetweenDotsSpace))
            {
                RebuildAllPages();
            }
        }

        private void LayoutStyle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.NameIndent) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.LineSpacing) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.DescLeftIndent) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.DescRightIndent) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.ExtrasSeparator) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.DescSeparator) ||
                 e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.SpaceBetweenColumns) ||
                 e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.LineBetweenColumns) ||
                 e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.SeparationLineThickness) ||
                 e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.SeparationLineColor) ||
                 e.PropertyName == nameof(MenuPresentationModel.MenuStyles.ILayoutStyle.SeparationLineType) ||
                 e.PropertyName == "all")
            {
                RebuildAllPages();
            }
        }

        private void MenuItemStyle_PropertyChanged(object _object, string member)
        {
            if (member == nameof(MenuPresentationModel.MenuStyles.IMenuItemStyle.Indent) ||
                member == nameof(MenuPresentationModel.MenuStyles.IMenuItemStyle.NewLineForDescription) ||
                member == nameof(MenuPresentationModel.MenuStyles.IMenuItemStyle.Alignment) ||
                member == nameof(MenuPresentationModel.MenuStyles.IMenuItemStyle.Font))
            {
                RebuildAllPages();
            }
        }

        private void PageStyle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(MenuPresentationModel.MenuStyles.IPageStyle.Border) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.IPageStyle.BorderMargin) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.IPageStyle.Background) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.IPageStyle.BackgroundMargin) ||
                e.PropertyName == nameof(MenuPresentationModel.MenuStyles.IPageStyle.BackgroundStretch))
            {
                return;
            }
            else if (e.PropertyName == nameof(MenuPresentationModel.MenuStyles.IPageStyle.ColumnsUneven))
            {
                if (PagePresenttion != null && PagePresenttion.MenuPage.NumberofColumns > 1)
                    RebuildAllPages();
            }
            else
            {
                RebuildAllPages();
            }
            if (e.PropertyName == nameof(MenuPresentationModel.MenuStyles.IPageStyle.PageSize))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageSize)));
            }
        }


        private void MenuStylesheetChangeState(object _object, string member)
        {
            if (!SuspendUpdateFromStyleSheetChange)
                RebuildAllPages();
        }
        bool OnUpdateMenuPagesStyle;
        private void UpdateMenuPagesStyle()
        {
            if (OnUpdateMenuPagesStyle)
                return;

            OnUpdateMenuPagesStyle = true;
            try
            {
                RestaurantMenu.ConntextStyleSheet = MenuStylesheet;

                RebuildAllPages();

                //  PagePresenttion.UpdateCanvasItems(SeletedMenuPage);
            }
            finally
            {
                OnUpdateMenuPagesStyle = false;
            }
        }
        /// <exclude>Excluded</exclude>
        bool _PopUp;

        public bool ShowPopUpMessage
        {
            get
            {
                return _PopUp;
            }
            set
            {
                _PopUp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowPopUpMessage)));
            }
        }
        public string SelectedStyleName
        {
            get
            {
                if (_SelectedMenuStyle != null)
                    return _SelectedMenuStyle.Name;
                else
                    return "none";
            }
        }


        /// <exclude>Excluded</exclude>
        double _ZoomPercentage;
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

        public string ZoomPercentageLabel
        {
            get
            {
                return ZoomPercentage.ToString("N2") + "%";
            }
        }

        /// <MetaDataID>{a3bb9949-2a2a-4546-822b-5eeb01ee5d45}</MetaDataID>
        internal void AddPageAfter(BookPageViewModel page)
        {


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                int pageIndex = RestaurantMenu.Pages.IndexOf(page.MenuPage);
                var menuPage = new MenuPage();
                var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(RestaurantMenu);
                objectStorage.CommitTransientObjectState(menuPage);
                RestaurantMenu.InsertPage(pageIndex, menuPage);
                stateTransition.Consistent = true;
            }
            UpdateMenuPagesStyle();

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Pages"));

        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{9496b7e3-63ea-41be-b3e7-6ea2fdf7a4f5}</MetaDataID>
        internal void MoveSelectedPageLeft(BookPageViewModel page)
        {


            if (RestaurantMenu.Pages.IndexOf(page.MenuPage) > 0)
            {
                int index = RestaurantMenu.Pages.IndexOf(page.MenuPage);

                RestaurantMenu.MovePage(index - 1, page.MenuPage);

                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Pages"));

            }

        }

        /// <MetaDataID>{28afd124-eb12-4ab2-ad20-a960f1370da1}</MetaDataID>
        internal void LoadMenu(System.Xml.Linq.XElement menu)
        {
            //_Pages = new ObservableCollection<BookPageViewModel>((from pageXml in menu.Element("MenuPages").Elements("Page")
            //          select new BookPageViewModel(pageXml)).ToList());

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Pages"));
        }


        internal void MoveToPreviousPage()
        {
            if (AllPages.IndexOf(SeletedMenuPage) > 0)
            {
                SeletedMenuPage = AllPages[AllPages.IndexOf(SeletedMenuPage) - 1];

                if (PreviousPageMenuCommand != null)
                    PreviousPageMenuCommand.Header = PreviousPages;
                if (NextPageMenuCommand != null)
                    NextPageMenuCommand.Header = NextPages;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NextPages)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviousPages)));

                Transaction.RunAsynch(new Action(() =>
                {
                    lock (this)
                    {
                        PagePresenttion.UpdateCanvasItems(SeletedMenuPage);
                    }
                }));
            }
        }

        internal void MoveToNextPage()
        {
            if (AllPages.IndexOf(SeletedMenuPage) != AllPages.Count - 1)
            {
                SeletedMenuPage = AllPages[AllPages.IndexOf(SeletedMenuPage) + 1];

                if (PreviousPageMenuCommand != null)
                    PreviousPageMenuCommand.Header = PreviousPages;
                if (NextPageMenuCommand != null)
                    NextPageMenuCommand.Header = NextPages;


                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pages)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NextPages)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviousPages)));
                Transaction.RunAsynch(new Action(() =>
                {
                    lock (this)
                    {
                        PagePresenttion.UpdateCanvasItems(SeletedMenuPage);
                    }
                }));
            }
        }



        internal void ResetPageFontsSizes()
        {
            SuspendUpdateFromStyleSheetChange = true;
            try
            {
                var menuItemStyle = EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle;
                var priceStyle = EditStyleSheet.Styles["price-options"] as MenuPresentationModel.MenuStyles.IPriceStyle;
                var titleHeadingStyle = EditStyleSheet.Styles["title-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var headingStyle = EditStyleSheet.Styles["heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var smallHeadingStyle = EditStyleSheet.Styles["small-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var altFontHeadingStyle = EditStyleSheet.Styles["alt-font-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;

                menuItemStyle.RestFont();
                priceStyle.RestFont();
                titleHeadingStyle.RestFont();
                headingStyle.RestFont();
            }
            finally
            {
                SuspendUpdateFromStyleSheetChange = false;
            }
            RebuildAllPages();

        }

        internal void ExpandPageFontsSizes()
        {
            SuspendUpdateFromStyleSheetChange = true;
            try
            {
                var menuItemStyle = EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle;
                var priceStyle = EditStyleSheet.Styles["price-options"] as MenuPresentationModel.MenuStyles.IPriceStyle;
                var titleHeadingStyle = EditStyleSheet.Styles["title-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var headingStyle = EditStyleSheet.Styles["heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var smallHeadingStyle = EditStyleSheet.Styles["small-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var altFontHeadingStyle = EditStyleSheet.Styles["alt-font-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;

                var font = menuItemStyle.Font;
                font.FontSize = font.FontSize * 1.1;
                menuItemStyle.Font = font;

                font = menuItemStyle.ExtrasFont;
                font.FontSize = font.FontSize * 1.1;
                menuItemStyle.ExtrasFont = font;

                font = menuItemStyle.DescriptionFont;
                font.FontSize = font.FontSize * 1.1;
                menuItemStyle.DescriptionFont = font;

                font = menuItemStyle.ExtrasFont;
                font.FontSize = font.FontSize * 1.1;
                menuItemStyle.ExtrasFont = font;

                font = priceStyle.Font;
                font.FontSize = font.FontSize * 1.1;
                priceStyle.Font = font;

                font = titleHeadingStyle.Font;
                font.FontSize = font.FontSize * 1.1;
                titleHeadingStyle.Font = font;

                font = headingStyle.Font;
                font.FontSize = font.FontSize * 1.1;
                headingStyle.Font = font;
            }
            finally
            {
                SuspendUpdateFromStyleSheetChange = false;
            }
            RebuildAllPages();
        }

        bool SuspendUpdateFromStyleSheetChange;
        internal void ShrinkPageFontsSizes()
        {
            SuspendUpdateFromStyleSheetChange = true;
            try
            {
                var menuItemStyle = EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle;
                var priceStyle = EditStyleSheet.Styles["price-options"] as MenuPresentationModel.MenuStyles.IPriceStyle;
                var titleHeadingStyle = EditStyleSheet.Styles["title-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var headingStyle = EditStyleSheet.Styles["heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var smallHeadingStyle = EditStyleSheet.Styles["small-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;
                var altFontHeadingStyle = EditStyleSheet.Styles["alt-font-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle;

                var font = menuItemStyle.Font;
                font.FontSize = font.FontSize / 1.1;
                menuItemStyle.Font = font;

                font = menuItemStyle.ExtrasFont;
                font.FontSize = font.FontSize / 1.1;
                menuItemStyle.ExtrasFont = font;

                font = menuItemStyle.DescriptionFont;
                font.FontSize = font.FontSize / 1.1;
                menuItemStyle.DescriptionFont = font;

                font = menuItemStyle.ExtrasFont;
                font.FontSize = font.FontSize / 1.1;
                menuItemStyle.ExtrasFont = font;

                font = priceStyle.Font;
                font.FontSize = font.FontSize / 1.1;
                priceStyle.Font = font;

                font = titleHeadingStyle.Font;
                font.FontSize = font.FontSize / 1.1;
                titleHeadingStyle.Font = font;

                font = headingStyle.Font;
                font.FontSize = font.FontSize / 1.1;
                headingStyle.Font = font;

            }
            finally
            {
                SuspendUpdateFromStyleSheetChange = false;
            }
            RebuildAllPages();
        }

        internal void ExpandLineSpace()
        {

            var lineSpacing = Math.Round(LayoutOptionsPresentation.PixelToMM((EditStyleSheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).LineSpacing), 1);
            lineSpacing += 0.3;

            (EditStyleSheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).LineSpacing = LayoutOptionsPresentation.mmToPixel(lineSpacing);

            UpdateMenuPagesStyle();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineSpacing)));
        }

        public double LineSpacing
        {
            get
            {
                return Math.Round(LayoutOptionsPresentation.PixelToMM((EditStyleSheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).LineSpacing) / 10, 2);
            }
        }

        MenuCommand PreviousPageMenuCommand;
        MenuCommand NextPageMenuCommand;


        public RelayCommand PreviousPageCommand { get; private set; }
        public RelayCommand NextPageCommand { get; private set; }
        public RelayCommand ShrinkLineSpaceCommand { get; private set; }
        public RelayCommand ExpandLineSpaceCommand { get; private set; }
        public RelayCommand ResetLineSpaceCommand { get; private set; }
        public RelayCommand ShrinkPageFontsSizesCommand { get; private set; }
        public RelayCommand ExpandPageFontsSizesCommand { get; private set; }
        public RelayCommand ResetPageFontsSizesCommand { get; private set; }
        void sss()
        {


        }

        WPFUIElementObjectBind.MenuCommand _MenuDesignerToolBar;
        public WPFUIElementObjectBind.MenuCommand MenuDesignerToolBar
        {
            get
            {
                if (_MenuDesignerToolBar == null)
                {
                    PreviousPageMenuCommand = new MenuCommand()
                    {
                        Header = PreviousPages,
                        ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/PreviousPage.png")),
                        Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.MoveToPreviousPage(); }),
                        ToolTipText = Properties.Resources.ToolTipMoveToPreviousPage,
                        DataTemplateStaticResource = "PreviousPageButton"

                    };
                    NextPageMenuCommand = new MenuCommand()
                    {
                        Header = NextPages,
                        ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/NextPage.png")),
                        Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.MoveToNextPage(); }),
                        ToolTipText = Properties.Resources.ToolTipMoveToNextPage,
                        DataTemplateStaticResource = "NextPageButton"
                    };

                    _MenuDesignerToolBar = new WPFUIElementObjectBind.MenuCommand()
                    {
                        SubMenuCommands = new List<WPFUIElementObjectBind.MenuCommand>()
                        {
                           PreviousPageMenuCommand,
                            NextPageMenuCommand,
                            null,
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/ShrinkLineSpace.png")),
                                Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.ShrinkLineSpace(); },(object seneder)=>{ return !IsReadonly; }),
                                ToolTipText = Properties.Resources.ToolTipShrinkLineSpace
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/ExpandLineSpace.png")),
                                Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.ExpandLineSpace(); },(object seneder)=>{ return !IsReadonly; }),
                                ToolTipText = Properties.Resources.ToolTipExpandLineSpace
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/ResetLineSpace.png")),
                                Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.ResetLineSpace(); },(object seneder)=>{ return !IsReadonly; }),
                                ToolTipText = Properties.Resources.ToolTipResetLineSpace
                            },
                            null,
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/ShrinkFontSize.png")),
                                Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.ShrinkPageFontsSizes(); },(object seneder)=>{ return !IsReadonly; }),
                                ToolTipText = Properties.Resources.ToolTipShrinkPageFontsSizes
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/ExpanFontSize.png")),
                                Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.ExpandPageFontsSizes(); },(object seneder)=>{ return !IsReadonly; }),
                                ToolTipText = Properties.Resources.ToolTipExpandPageFontsSizes
                            },
                            new MenuCommand()
                            {
                                ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/ResetFontSize.png")),
                                Command = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.N, ModifierKeys.Control) }, (object seneder) => { this.ResetPageFontsSizes(); },(object seneder)=>{ return !IsReadonly; }),
                                ToolTipText = Properties.Resources.ToolTipResetPageFontsSizes
                            }
                        }
                    };

                }

                return _MenuDesignerToolBar;
            }
        }
        RoutedCommand BackgroundSelectionCommand;
        RoutedCommand BorderSelectionCommand;
        RoutedCommand LayoutOptionsCommand;
        RoutedCommand HeadingTypesAccentsSelectionCommand;
        RoutedCommand StyleSheetAccentsSelectionCommand;
        MenuCommand _DesignMenu;
        public MenuCommand DesignMenu
        {
            get
            {

                //<MenuItem Header="{x:Static Resx:Resources.BackgroundMenuTitle}" Command="{x:Static s:DesignerCanvas.BackgroundSelection}" CommandTarget="{Binding ElementName=MyPageDesigner}"></MenuItem>
                //<MenuItem Header="{x:Static Resx:Resources.BorderMenuTitle}" Command="{x:Static s:DesignerCanvas.BorderSelection}" CommandTarget="{Binding ElementName=MyPageDesigner}"></MenuItem>
                //<MenuItem Header="{x:Static Resx:Resources.LayoutOptionsMenuTitle}" Command="{x:Static s:DesignerCanvas.LayoutOptions}" CommandTarget="{Binding ElementName=MyPageDesigner}"></MenuItem>
                //<MenuItem Header="{x:Static Resx:Resources.HeadingsAccentsMenuTitle}" Command="{x:Static s:DesignerCanvas.HeadingTypesAccents}" CommandTarget="{Binding ElementName=MyPageDesigner}"></MenuItem>
                //<MenuItem Header="{x:Static Resx:Resources.StyleSheetAccentsMenuTitle}" Command="{x:Static s:DesignerCanvas.StyleSheetAccents}" CommandTarget="{Binding ElementName=MyPageDesigner}"></MenuItem>
                //<MenuItem Header="Download Style" Command="{x:Static s:DesignerCanvas.DownloadStyleSheet}" CommandTarget="{Binding ElementName=MyPageDesigner}"></MenuItem>
                //<MenuItem Header="SignIn / SignUp" Command="{x:Static s:DesignerCanvas.SignInSignUp}" CommandTarget="{Binding ElementName=MyPageDesigner}"></MenuItem>



                if (_DesignMenu == null)
                {
                    BackgroundSelectionCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.B, ModifierKeys.Control) }, (object sender) => { this.BackgroundSelection(); }, null, "BackgroundSelection");
                    BorderSelectionCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.B, ModifierKeys.Control) }, (object sender) => { this.BorderSelection(); });
                    LayoutOptionsCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.B, ModifierKeys.Control) }, (object sender) => { this.ChangeLayoutOptions(); });
                    HeadingTypesAccentsSelectionCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.B, ModifierKeys.Control) }, (object sender) => { this.HeadingTypesAccentsSelection(); });
                    StyleSheetAccentsSelectionCommand = new WPFUIElementObjectBind.RoutedCommand(new List<InputGesture>() { new KeyGesture(Key.B, ModifierKeys.Control) }, (object sender) => { this.StyleSheetAccentsSelectionSelection(); });

                    _DesignMenu = new WPFUIElementObjectBind.MenuCommand()
                    {
                        Header = Properties.Resources.DesignMenuTitle,
                        SubMenuCommands = new List<WPFUIElementObjectBind.MenuCommand>()
                        {
                            new MenuCommand()
                            {
                                Header=Properties.Resources.BackgroundMenuTitle,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/background16.png")), Width = 16, Height = 16 },
                                Command = BackgroundSelectionCommand,
                                ToolTipText = Properties.Resources.ToolTipShrinkLineSpace
                            },
                             new MenuCommand()
                            {
                                Header=Properties.Resources.BorderMenuTitle,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/frame16.png")), Width = 16, Height = 16 },
                                Command = BorderSelectionCommand,
                                ToolTipText = Properties.Resources.ToolTipShrinkLineSpace
                            }
                             ,
                             new MenuCommand()
                            {
                                Header=Properties.Resources.LayoutOptionsMenuTitle,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/layout16.png")), Width = 16, Height = 16 },
                                Command = LayoutOptionsCommand,
                                ToolTipText = Properties.Resources.LayoutOptionsMenuTitle
                            }
                             ,
                             new MenuCommand()
                            {
                                Header=Properties.Resources.HeadingsAccentsMenuTitle,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/header16.png")), Width = 16, Height = 16 },
                                Command = HeadingTypesAccentsSelectionCommand,
                                ToolTipText = Properties.Resources.HeadingsAccentsMenuTitle
                            }
                             ,
                            new MenuCommand()
                            {
                                Header=Properties.Resources.StyleSheetAccentsMenuTitle,
                                Icon = new System.Windows.Controls.Image() { Source = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/study16.png")), Width = 16, Height = 16 },
                                Command = StyleSheetAccentsSelectionCommand,
                                ToolTipText = Properties.Resources.StyleSheetAccentsMenuTitle
                            },
                                new MenuCommand()
                            {
                                Header="Download Style",
                                Command = new WPFUIElementObjectBind.RoutedCommand( (object sender) => {
                                    Window win = Window.GetWindow(StyleSheetAccentsSelectionCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
                                    DownloadStylesWindow DfontDialog = new DownloadStylesWindow();
                                    DfontDialog.Owner = win;
                                    DfontDialog.ShowDialog();
                                }),
                            }
                        }
                    };
                }
                return _DesignMenu;
            }
        }

        /// <exclude>Excluded</exclude>
        MenuCommand _FontsMenu;
        public MenuCommand FontsMenu
        {
            get
            {

                if (_FontsMenu == null)
                {
                    TitleHeadingFontsCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => { this.TitleHeadingFonts(); });
                    NormalHeadingFontsCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => { this.NormalHeadingFonts(); });
                    SubheadingFontsCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => { this.SubHeadingFonts(); });
                    FoodItemNameFontsCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => { this.FoodItemNameFonts(); });
                    FoodItemDescriptionFontsCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => { this.FoodItemDescriptionFonts(); });
                    FoodItemExtrasFontsCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => { this.FoodItemExtrasFonts(); });
                    FoodItemPriceFontsCommand = new WPFUIElementObjectBind.RoutedCommand((object sender) => { this.FoodItemPriceFonts(); });
                    _FontsMenu = new WPFUIElementObjectBind.MenuCommand()
                    {
                        Header = Properties.Resources.FontsMenuItemHeader,
                        SubMenuCommands = new List<WPFUIElementObjectBind.MenuCommand>()
                        {
                            new MenuCommand()
                            {
                                Header=Properties.Resources.TitleHeadingFontsMenuItemHeader,
                                Command = TitleHeadingFontsCommand,
                            },
                            new MenuCommand()
                            {
                                Header=Properties.Resources.NormalHeadingFontsMenuItemHeader,
                                Command = NormalHeadingFontsCommand,
                            },
                            new MenuCommand()
                            {
                                Header=Properties.Resources.SubheadingFontsMenuItemHeader,
                                Command = SubheadingFontsCommand,
                            },
                            null,
                            new MenuCommand()
                            {
                                Header=Properties.Resources.FoodItemNameFontsMenuItemHeader,
                                Command = FoodItemNameFontsCommand,
                            },
                            new MenuCommand()
                            {
                                Header=Properties.Resources.FoodItemDescriptionFontsMenuItemHeader,
                                Command = FoodItemDescriptionFontsCommand,
                            },
                            new MenuCommand()
                            {
                                Header=Properties.Resources.FoodItemExtrasMenuItemHeader,
                                Command = FoodItemExtrasFontsCommand,
                            },
                            new MenuCommand()
                            {
                                Header=Properties.Resources.FoodItemPriceMenuItemHeader,
                                Command = FoodItemPriceFontsCommand,
                            }
                    }
                    };
                }
                return _FontsMenu;
            }
        }


        public List<MenuCommand> MenuItems
        {
            get
            {
                return new List<MenuCommand>() { DesignMenu, FontsMenu };
            }
        }
        private void StyleSheetAccentsSelectionSelection()
        {
            Window win = Window.GetWindow(StyleSheetAccentsSelectionCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
            using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    Views.StyleSheetAccentsWindow styleSheetAccentsWindow = new Views.StyleSheetAccentsWindow();
                    try
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        styleSheetAccentsWindow.Owner = win;
                        StyleSheetAccentsAccentViewModel styleSheetAccentsAccentViewModel = new StyleSheetAccentsAccentViewModel();
                        styleSheetAccentsWindow.GetObjectContext().SetContextInstance(styleSheetAccentsAccentViewModel);
                    }
                    finally
                    {
                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    }
                    if (styleSheetAccentsWindow.ShowDialog().Value)
                        stateTransition.Consistent = true;
                }
                suppressStateTransition.Consistent = true;
            }
        }

        private void HeadingTypesAccentsSelection()
        {
            MenuPresentationModel.MenuStyles.StyleSheet styleSheet = MenuStylesheet;
            Window win = Window.GetWindow(HeadingTypesAccentsSelectionCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
            SelectHeadingTypesAccents headingTypesAccents = new SelectHeadingTypesAccents();
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                HeadingTypesAccentsViewModel headingTypesAccentsViewModel = new HeadingTypesAccentsViewModel((styleSheet.Styles["title-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle), (styleSheet.Styles["heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle), (styleSheet.Styles["small-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle));
                headingTypesAccents.Owner = win;
                headingTypesAccents.GetObjectContext().SetContextInstance(headingTypesAccentsViewModel);
            }
            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            headingTypesAccents.ShowDialog();
        }

        private void ChangeLayoutOptions()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                LayoutOptionsForm layoutOptions = new LayoutOptionsForm();
                try
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Window win = Window.GetWindow(LayoutOptionsCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
                    LayoutOptionsPresentation layoutOptionsPresentation = new LayoutOptionsPresentation(this,
                                                                                                                RealObject.Style.Styles["layout"] as MenuPresentationModel.MenuStyles.LayoutStyle,
                                                                                                                RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle,
                                                                                                                RealObject.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle,
                                                                                                                RealObject.Style.Styles["price-options"] as MenuPresentationModel.MenuStyles.PriceStyle);
                    layoutOptions.Owner = win;
                    layoutOptions.GetObjectContext().SetContextInstance(layoutOptionsPresentation);
                }
                finally
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                }
                if (layoutOptions.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }

        }

        private void BorderSelection()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                BorderSelectorWindow borderSelection = new Views.BorderSelectorWindow();

                try
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Window win = Window.GetWindow(BorderSelectionCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
                    BorderSelectionViewModel borderSelectionViewModel = new BorderSelectionViewModel(RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle);
                    borderSelection.Owner = win;
                    borderSelection.GetObjectContext().SetContextInstance(borderSelectionViewModel);
                }
                finally
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                }

                if (borderSelection.ShowDialog().Value)
                    stateTransition.Consistent = true;

            }
        }

        internal void BackgroundSelection()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                BackgroundSelectorWindow borderSelection = new Views.BackgroundSelectorWindow();
                try
                {
                    Window win = Window.GetWindow(BackgroundSelectionCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    BackgroundSelectionViewModel borderSelectionViewModel = new BackgroundSelectionViewModel(RealObject.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle);
                    borderSelection.Owner = win;
                    borderSelection.GetObjectContext().SetContextInstance(borderSelectionViewModel);
                }
                finally
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                }

                if (borderSelection.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }

        }

        Window OwnerWin
        {
            get => Window.GetWindow(ClickPseudoCommand.UserInterfaceObjectConnection.ContainerControl as DependencyObject);
        }




        internal void NormalHeadingFonts()
        {
            Window win = OwnerWin;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                var book = this;
                var headingStyle = (book.EditStyleSheet.Styles["heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle);
                StyleFontUpdater styleFontUpdater = new StyleFontUpdater(headingStyle, new StyleableWindow.FontPresantation() { Font = headingStyle.Font, TitlebarText = Properties.Resources.NormalHeadingFontsMenuItemHeader + " Fonts" }, "Font");
                fontDialog.GetObjectContext().SetContextInstance(styleFontUpdater.FontPresantation);
                fontDialog.Owner = OwnerWin;
                if (fontDialog.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }
        }

        internal void SubHeadingFonts()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                var headingStyle = (EditStyleSheet.Styles["small-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle);
                StyleFontUpdater styleFontUpdater = new StyleFontUpdater(headingStyle, new StyleableWindow.FontPresantation() { Font = headingStyle.Font, TitlebarText = Properties.Resources.SubheadingFontsMenuItemHeader + " Fonts" }, "Font");
                fontDialog.GetObjectContext().SetContextInstance(styleFontUpdater.FontPresantation);
                fontDialog.Owner = OwnerWin;
                if (fontDialog.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }
        }

        internal void TitleHeadingFonts()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                var headingStyle = (EditStyleSheet.Styles["title-heading"] as MenuPresentationModel.MenuStyles.IHeadingStyle);
                StyleFontUpdater styleFontUpdater = new StyleFontUpdater(headingStyle, new StyleableWindow.FontPresantation() { Font = headingStyle.Font, TitlebarText = Properties.Resources.TitleHeadingFontsMenuItemHeader + " Fonts" }, "Font");

                fontDialog.GetObjectContext().SetContextInstance(styleFontUpdater.FontPresantation);
                fontDialog.Owner = OwnerWin;
                if (fontDialog.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }


        }

        internal void FoodItemNameFonts()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                var menuItemStyle = (EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle);
                StyleFontUpdater styleFontUpdater = new StyleFontUpdater(menuItemStyle, new StyleableWindow.FontPresantation() { Font = menuItemStyle.Font, TitlebarText = Properties.Resources.FoodItemNameFontsMenuItemHeader + " Fonts" }, "Font");

                fontDialog.GetObjectContext().SetContextInstance(styleFontUpdater.FontPresantation);
                // fontDialog.Font = (book.EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle).Font;
                fontDialog.Owner = OwnerWin;
                if (fontDialog.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }
        }

        internal void FoodItemDescriptionFonts()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                var menuItemStyle = (EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle);
                StyleFontUpdater styleFontUpdater = new StyleFontUpdater(menuItemStyle, new StyleableWindow.FontPresantation() { Font = menuItemStyle.DescriptionFont, TitlebarText = Properties.Resources.FoodItemDescriptionFontsMenuItemHeader + " Fonts" }, "DescriptionFont");

                fontDialog.GetObjectContext().SetContextInstance(styleFontUpdater.FontPresantation);
                // fontDialog.Font = (book.EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle).Font;
                fontDialog.Owner = OwnerWin;
                if (fontDialog.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }
        }

        internal void FoodItemExtrasFonts()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                var menuItemStyle = (EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle);
                StyleFontUpdater styleFontUpdater = new StyleFontUpdater(menuItemStyle, new StyleableWindow.FontPresantation() { Font = menuItemStyle.ExtrasFont, TitlebarText = Properties.Resources.FoodItemExtrasMenuItemHeader + " Fonts" }, "ExtrasFont");

                fontDialog.GetObjectContext().SetContextInstance(styleFontUpdater.FontPresantation);
                fontDialog.Owner = OwnerWin;
                if (fontDialog.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }
        }
        internal void FoodItemPriceFonts()
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
            {
                StyleableWindow.FontDialog fontDialog = new StyleableWindow.FontDialog();
                var menuItemStyle = (EditStyleSheet.Styles["price-options"] as MenuPresentationModel.MenuStyles.IPriceStyle);
                StyleFontUpdater styleFontUpdater = new StyleFontUpdater(menuItemStyle, new StyleableWindow.FontPresantation() { Font = menuItemStyle.Font, TitlebarText = Properties.Resources.FoodItemPriceMenuItemHeader + " Fonts" }, "Font");

                fontDialog.GetObjectContext().SetContextInstance(styleFontUpdater.FontPresantation);
                // fontDialog.Font = (book.EditStyleSheet.Styles["menu-item"] as MenuPresentationModel.MenuStyles.IMenuItemStyle).Font;
                fontDialog.Owner = OwnerWin;
                if (fontDialog.ShowDialog().Value)
                    stateTransition.Consistent = true;
            }
        }


        internal void ShrinkLineSpace()
        {

            var lineSpacing = Math.Round(LayoutOptionsPresentation.PixelToMM((EditStyleSheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).LineSpacing), 1);
            lineSpacing -= 0.3;

            (EditStyleSheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).LineSpacing = LayoutOptionsPresentation.mmToPixel(lineSpacing);
            UpdateMenuPagesStyle();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineSpacing)));
        }

        public string PageSize
        {
            get
            {
                var pageStyle = EditStyleSheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle;
                var pageHeight = Math.Round(LayoutOptionsPresentation.PixelToMM(pageStyle.PageHeight) / 10, 2);
                var pageWidth = Math.Round(LayoutOptionsPresentation.PixelToMM(pageStyle.PageWidth) / 10, 2);
                return string.Format("{0}cm X {1}cm", pageWidth, pageHeight);
            }
        }

        public WPFUIElementObjectBind.RelayCommand StyleSelectionCommand { get; }
        public RoutedCommand TitleHeadingFontsCommand { get; private set; }
        public RoutedCommand NormalHeadingFontsCommand { get; private set; }
        public RoutedCommand SubheadingFontsCommand { get; private set; }
        public RoutedCommand FoodItemNameFontsCommand { get; private set; }
        public RoutedCommand FoodItemDescriptionFontsCommand { get; private set; }
        public RoutedCommand FoodItemExtrasFontsCommand { get; private set; }
        public RoutedCommand FoodItemPriceFontsCommand { get; private set; }
        public Transaction RunUnderTransaction
        {
            get
            {
                if (ClickPseudoCommand != null && ClickPseudoCommand.UserInterfaceObjectConnection != null)
                    return ClickPseudoCommand.UserInterfaceObjectConnection.Transaction;
                else
                    return null;
            }
        }

        public RelayCommand BeforeSaveCommand { get; protected set; }

        internal void ResetLineSpace()
        {
            (EditStyleSheet.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).ResetLineSpacing();
            UpdateMenuPagesStyle();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineSpacing)));
        }

        static public BookViewModel OpenMenu(RawStorageData graphicMenuStorageData, bool publishAllowed = false)
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("RestMenu", graphicMenuStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));


            MenuPresentationModel.RestaurantMenu restaurantMenu = (from menu in storage.GetObjectCollection<MenuPresentationModel.RestaurantMenu>()
                                                                   select menu).FirstOrDefault();


            //using (SystemStateTransition suppressStateTransition = new SystemStateTransition(TransactionOption.Suppress))
            //{

            //    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //    {
            //        restaurantMenu.Name = menuName;
            //        stateTransition.Consistent = true;
            //    }

            //    suppressStateTransition.Consistent = true;
            //}


            int pageCount = restaurantMenu.Pages.Count;
            return new BookViewModel(restaurantMenu, graphicMenuStorageData.StorageRef);
        }


    }
}
