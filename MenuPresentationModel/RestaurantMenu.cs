using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using OOAdvantech;
using MenuPresentationModel.MenuCanvas;
using System.Collections.Generic;
using FlavourBusinessToolKit;
using System.Xml.Linq;
using System.IO;
using OOAdvantech.Json;
using OOAdvantech.Json;
using MenuPresentationModel.JsonMenuPresentation;
using SvgAccentModifier;
using System.Globalization;
using FlavourBusinessFacade;
using System.Net.Http;

namespace MenuPresentationModel
{

    public delegate void MenuStyleChangedHandle(MenuStyles.IStyleSheet oldStyle, MenuStyles.IStyleSheet newStyle);

    public delegate void MenuCanvasItemChangedHandle(MenuCanvas.IMenuCanvasItem menuCanvasitem, string member);




    /// <MetaDataID>{a5e33434-afd5-4612-8375-01d50a7e7cfb}</MetaDataID>
    [BackwardCompatibilityID("{a5e33434-afd5-4612-8375-01d50a7e7cfb}")]
    [Persistent()]
    public class RestaurantMenu : MarshalByRefObject, IRestaurantMenu, IRestaurantMenuPublisher, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {
        /// <MetaDataID>{a22edac9-3678-4ff1-9de4-3d224c7ad58e}</MetaDataID>
        public void RemoveAvailableHeading(FoodItemsHeading heading)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _AvailableHeadings.Remove(heading);
                stateTransition.Consistent = true;
            }


        }

        /// <MetaDataID>{8d61ed85-1f6d-4442-ad39-9d0156245177}</MetaDataID>
        public void AddAvailableHeading(FoodItemsHeading heading)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _AvailableHeadings.Add(heading);
                stateTransition.Consistent = true;
            }


        }

     
        

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<FoodItemsHeading> _AvailableHeadings = new OOAdvantech.Collections.Generic.Set<FoodItemsHeading>();


        [AssociationEndBehavior(PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [PersistentMember(nameof(_AvailableHeadings))]
        [Association("MenuAvailableHeadings", Roles.RoleA, "87b703b0-a2c3-4108-bd5e-8252d8e8d327")]
        [RoleBMultiplicityRange(1, 1)]
        public IList<FoodItemsHeading> AvailableHeadings
        {
            get
            {
                return _AvailableHeadings.AsReadOnly();
            }
        }



        /// <MetaDataID>{8b7c0f36-b4f4-4311-beb2-ec2dec9125fe}</MetaDataID>
        public void PublishMenu(string serverStorageFolder, string previousVersionServerStorageFolder, string menuResourcesPrefix, IFileManager fileManager, string menuName)
        {
            string rootUri = "http://localhost/devstoreaccount1";
            if (fileManager != null)
                rootUri = fileManager.RootUri;
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
            if (!String.IsNullOrWhiteSpace(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).StorageMetaData.Culture))
                culture = System.Globalization.CultureInfo.GetCultureInfo(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).StorageMetaData.Culture);

            JsonMenuPresentation.RestaurantMenu jsonRestaurantMenu = null;
            using (CultureContext cultureContext = new CultureContext(culture, true))
            {
                jsonRestaurantMenu = new JsonMenuPresentation.RestaurantMenu(this);


                Dictionary<string, string> pageImages = new Dictionary<string, string>();
                foreach (var entry in jsonRestaurantMenu.MultilingualPages.Values.Keys)
                {
                    

                        foreach (var page in (from pages in jsonRestaurantMenu.MultilingualPages.Values.Values.OfType<List<IMenuPageCanvas>>() from page in pages.OfType<JsonMenuPresentation.MenuPageCanvas>() select page))
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
                }


                foreach (string uri in pageImages.Keys.ToArray())
                {
                    var aboluteImageUri = rootUri + "/graphicmenusresources/" + uri;
                    var imageFileName = aboluteImageUri.Substring(aboluteImageUri.LastIndexOf('/') + 1);
                    string newImageUri = serverStorageFolder + imageFileName;
                    if (fileManager != null)
                        fileManager.Copy(aboluteImageUri, newImageUri);
                    pageImages[uri] = menuResourcesPrefix + imageFileName;

                }
                foreach (var page in (from pages in jsonRestaurantMenu.MultilingualPages.Values.Values.OfType<List<IMenuPageCanvas>>() from page in pages.OfType<JsonMenuPresentation.MenuPageCanvas>() select page))
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

                var headingsAccentImages = (from menuCanvasAccent in (from heading in jsonRestaurantMenu.MenuCanvasItems.OfType<JsonMenuPresentation.MenuCanvasHeading>()
                                                                      select heading.Accent).OfType<MenuCanvasHeadingAccent>()
                                            where menuCanvasAccent != null && menuCanvasAccent.Accent != null && !menuCanvasAccent.MultipleItemsAccent
                                            from accentImageEntry in (menuCanvasAccent.Accent as HeadingAccent).MultilingualAccentImages.Values
                                            where accentImageEntry.Value is List<MenuStyles.IImage>
                                            from accentImage in accentImageEntry.Value as List<MenuStyles.IImage>
                                            select new { accentImage, color = (menuCanvasAccent).AccentColor, imageUri = accentImage.Uri }).ToList();

                headingsAccentImages.AddRange((from menuCanvasAccent in (from foodItem in jsonRestaurantMenu.MenuCanvasItems.OfType<JsonMenuPresentation.MenuCanvasFoodItem>()
                                                                         select foodItem.Accent).OfType<MenuCanvasHeadingAccent>()
                                               where (menuCanvasAccent) != null && (menuCanvasAccent).Accent != null && !menuCanvasAccent.MultipleItemsAccent
                                               from accentImageEntry in (menuCanvasAccent.Accent as HeadingAccent).MultilingualAccentImages.Values
                                               where accentImageEntry.Value is List<MenuStyles.IImage>
                                               from accentImage in accentImageEntry.Value as List<MenuStyles.IImage>
                                               select new { accentImage, color = (menuCanvasAccent).AccentColor, imageUri = accentImage.Uri }).ToList());


                var colorHeadingAccentImages = (from headingAccentImage in headingsAccentImages
                                                group headingAccentImage by new { headingAccentImage.imageUri, headingAccentImage.color } into accentImages
                                                select new { accentImageAsKey = accentImages.Key, accentImages });

                foreach (var colorHeadingAccentImage in colorHeadingAccentImages)
                {
                    //colorHeadingAccentImage.accentImageAsKey.
                    var aboluteImageUri = rootUri + "/graphicmenusresources/" + colorHeadingAccentImage.accentImageAsKey.imageUri;

                    var imageFileName = aboluteImageUri.Substring(aboluteImageUri.LastIndexOf('/') + 1);
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

                    string targetImageUri = serverStorageFolder + menuImageFile;
                    string newImageUri = menuResourcesPrefix + menuImageFile;
                    if (fileManager != null)
                        fileManager.Upload(targetImageUri, accentImageStream, "image/svg+xml");

                    foreach (var accentImage in colorHeadingAccentImage.accentImages)
                    {
                        (accentImage.accentImage as MenuImage).Uri = newImageUri;
                        (accentImage.accentImage as MenuImage).Image = new MenuPresentationModel.MenuStyles.Resource()
                        {
                            Name = (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Image.Name,
                            Uri = newImageUri
                        };
                    }
                }


                var multipleItemsAccentAccentImages = (from menuCanvasAccent in (from heading in jsonRestaurantMenu.MenuCanvasItems.OfType<JsonMenuPresentation.MenuCanvasHeading>()
                                                                                 select heading.Accent).OfType<MenuCanvasHeadingAccent>()
                                                       where menuCanvasAccent != null && menuCanvasAccent.Accent != null && menuCanvasAccent.MultipleItemsAccent
                                                       from accentImageEntry in (menuCanvasAccent.Accent as HeadingAccent).MultilingualAccentImages.Values
                                                       where accentImageEntry.Value is List<MenuStyles.IImage>
                                                       from accentImage in accentImageEntry.Value as List<MenuStyles.IImage>
                                                       select new { accentImage, color = (menuCanvasAccent as MenuCanvasHeadingAccent).AccentColor, imageUri = accentImage.Uri, (accentImage as MenuImage).Size }).ToList();

                multipleItemsAccentAccentImages.AddRange((from menuCanvasAccent in (from foodItem in jsonRestaurantMenu.MenuCanvasItems.OfType<JsonMenuPresentation.MenuCanvasFoodItem>()
                                                                                    select foodItem.Accent).OfType<MenuCanvasHeadingAccent>()
                                                          where (menuCanvasAccent) != null && (menuCanvasAccent).Accent != null && menuCanvasAccent.MultipleItemsAccent
                                                          from accentImageEntry in (menuCanvasAccent.Accent as HeadingAccent).MultilingualAccentImages.Values
                                                          where accentImageEntry.Value is List<MenuStyles.IImage>
                                                          from accentImage in accentImageEntry.Value as List<MenuStyles.IImage>
                                                          select new { accentImage, color = (menuCanvasAccent).AccentColor, imageUri = accentImage.Uri, (accentImage as MenuImage).Size }).ToList());







                var colorMultipleItemsAccentAccentImages = (from headingAccentImage in multipleItemsAccentAccentImages
                                                            group headingAccentImage by new { headingAccentImage.imageUri, headingAccentImage.color, headingAccentImage.Size } into accentImages
                                                            select new { accentImageAsKey = accentImages.Key, accentImages });

                foreach (var colorHeadingAccentImage in colorMultipleItemsAccentAccentImages)
                {
                    //colorHeadingAccentImage.accentImageAsKey.
                    var aboluteImageUri = rootUri + "/graphicmenusresources/" + colorHeadingAccentImage.accentImageAsKey.imageUri;

                    var imageFileName = aboluteImageUri.Substring(aboluteImageUri.LastIndexOf('/') + 1);
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

                    string targetImageUri = serverStorageFolder + menuImageFile;
                    string newImageUri = menuResourcesPrefix + menuImageFile;
                    if (fileManager != null)
                        fileManager.Upload(targetImageUri, accentImageStream, "image/svg+xml");

                    foreach (var accentImage in colorHeadingAccentImage.accentImages)
                    {
                        (accentImage.accentImage as MenuImage).Uri = newImageUri;
                        (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Image = new MenuPresentationModel.MenuStyles.Resource()
                        {
                            Name = (accentImage.accentImage as MenuPresentationModel.JsonMenuPresentation.MenuImage).Image.Name,
                            Uri = newImageUri
                        };
                    }
                }
            }



            

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
            }
            catch (Exception error)
            {
            }

            string json = JsonConvert.SerializeObject(jsonRestaurantMenu, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize, PreserveReferencesHandling = PreserveReferencesHandling.All });

           //

            MemoryStream jsonRestaurantMenuStream = new MemoryStream();

            byte[] jsonBuffer = System.Text.Encoding.UTF8.GetBytes(json);
            jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
            jsonRestaurantMenuStream.Position = 0;
            string jsonFileName = serverStorageFolder + menuName + ".json";
            if (fileManager != null)
                fileManager.Upload(jsonFileName, jsonRestaurantMenuStream, "application/json");


            json = JsonConvert.SerializeObject(jsonRestaurantMenu, Formatting.None, OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings);

            //OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefDeserializeSettings

            jsonRestaurantMenuStream = new MemoryStream();

            jsonBuffer = System.Text.Encoding.UTF8.GetBytes(json);
            jsonRestaurantMenuStream.Write(jsonBuffer, 0, jsonBuffer.Length);
            jsonRestaurantMenuStream.Position = 0;
            jsonFileName = serverStorageFolder + menuName + "_t.json";
            if (fileManager != null)
                fileManager.Upload(jsonFileName, jsonRestaurantMenuStream, "application/json");



            if (fileManager != null)
            {
                List<FlavourBusinessToolKit.FileInfo> blobs = fileManager.ListBlobs(previousVersionServerStorageFolder.Substring(0, previousVersionServerStorageFolder.Length - 1));
                if (blobs != null)
                {
                    foreach (var blob in blobs)
                    {
                        if (blob.LastModifiedUtc.HasValue && ((DateTime.UtcNow - blob.LastModifiedUtc.Value).Minutes > 0.3))
                            blob.DeleteIfExists();
                    }
                }
            }

        }



        /// <MetaDataID>{3c6d04ca-722b-4e04-9e97-c471d29bb9b5}</MetaDataID>
        public void AddMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Add(manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState += ManuCanvasItemChangeState;
        }

        /// <MetaDataID>{bf114fea-b6d5-48e2-b78d-7350de076798}</MetaDataID>
        public void MoveMenuItem(MenuPresentationModel.MenuCanvas.IMenuCanvasItem manuCanvasItem, int newpos)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var tmp = _MenuCanvasItems.ToArray();

                _MenuCanvasItems.Remove(manuCanvasItem);
                tmp = _MenuCanvasItems.ToArray();

                _MenuCanvasItems.Insert(newpos, manuCanvasItem);
                stateTransition.Consistent = true;

            }
        }

        /// <MetaDataID>{5c319d0b-ff11-4d19-9552-8224c5275e52}</MetaDataID>
        private void ManuCanvasItemChangeState(object _object, string member)
        {
            MenuCanvasItemChanged?.Invoke(_object as MenuCanvas.IMenuCanvasItem, member);
        }
        public event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{0c4fa683-a53a-4230-a789-8d95eba1e308}</MetaDataID>
        public void RemoveBlankPages()
        {
            while (this.Pages.Last().MenuCanvasItems.Count == 0 && this.Pages.Count > 1)
            {
                RemovePage(this.Pages.Last());
            }

        }
        /// <MetaDataID>{a2df9bf7-7ad3-4405-8464-9d410f0392ee}</MetaDataID>
        public void InsertMenuItemAfter(MenuCanvas.IMenuCanvasItem manuCanvasitem, MenuCanvas.IMenuCanvasItem newManuCanvasitem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                int pos = 0;
                if (manuCanvasitem != null)
                    pos = _MenuCanvasItems.IndexOf(manuCanvasitem);
                _MenuCanvasItems.Insert(pos + 1, newManuCanvasitem);

                var itemsDescription = _MenuCanvasItems.Select(x => x.Description).ToArray();
                stateTransition.Consistent = true;
            }
            newManuCanvasitem.ObjectChangeState += ManuCanvasItemChangeState;
        }

        /// <MetaDataID>{c87bcefb-ae4d-468c-8628-0d1ec0034b4c}</MetaDataID>
        public void RemoveMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Remove(manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState -= ManuCanvasItemChangeState;
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasItem> _MenuCanvasItems = new OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasItem>();

        /// <MetaDataID>{710a59a4-c75b-40f0-98ea-69926edf3b05}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember(nameof(_MenuCanvasItems))]
        public System.Collections.Generic.IList<MenuCanvas.IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                
                return _MenuCanvasItems.AsReadOnly();
            }
        }

        /// <MetaDataID>{89d139e3-b25f-43ad-b3ae-1e0ddccade64}</MetaDataID>
        public void CheckMenuCanvasItemsIndexes()
        {
            _MenuCanvasItems.CheckIndexes();
        }



        /// <MetaDataID>{24808386-2bff-4ade-ad5f-cd4a01f65f92}</MetaDataID>
        public static MenuStyles.IStyleSheet ConntextStyleSheet;

        public event MenuStyleChangedHandle MenuStyleChanged;

        public event MenuCanvasItemChangedHandle MenuCanvasItemChanged;
        /// <exclude>Excluded</exclude>
        MenuStyles.IStyleSheet _Style;


        [Association("MenuStyle", Roles.RoleA, "d0d8fa48-b643-4d17-aa7e-df449e985844")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember(nameof(_Style))]
        public MenuPresentationModel.MenuStyles.IStyleSheet Style
        {
            get
            {
                return _Style;
            }

            set
            {

                if (_Style != value)
                {

                    var oldStyle = _Style;
                    var newStyle = value;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Style = value;
                        stateTransition.Consistent = true;
                    }
                    MenuStyleChanged?.Invoke(oldStyle, newStyle);
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{bde5666b-a84e-40ab-b978-cf94e41e6d10}</MetaDataID>
        [PersistentMember("_Name")]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.MultilingualSet<IMenuPageCanvas> _Pages = new OOAdvantech.Collections.Generic.MultilingualSet<IMenuPageCanvas>();

        /// <MetaDataID>{b9633df5-9dab-4687-b680-b5e803e5d604}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [PersistentMember(nameof(_Pages))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public System.Collections.Generic.IList<IMenuPageCanvas> Pages
        {
            get
            {
                using (var cultureConext = new OOAdvantech.CultureContext(OOAdvantech.CultureContext.CurrentCultureInfo, false))
                {
                    return _Pages.AsReadOnly();
                }
            }
        }

        /// <MetaDataID>{677a98ed-2d48-4c2b-bd57-9bb3808c9e08}</MetaDataID>
        public Multilingual MultilingualPages
        {
            get
            {
                return new Multilingual(_Pages);
            }
        }


        /// <MetaDataID>{4952e0dd-c560-4bd5-8f69-930e8faa101d}</MetaDataID>
        public void AddPage(IMenuPageCanvas page)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Pages.Add(page);


                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(Pages));
        }

        //private void Page_ObjectChangeState(object _object, string member)
        //{
        //    if (member == nameof(MenuCanvas.IMenuPageCanvas.MenuCanvasItems))
        //    {
        //        ReBuildMenuPages();
        //    }
        //}

        /// <MetaDataID>{529e0142-498d-489c-9b5b-be8321011e1c}</MetaDataID>
        public void MovePage(int newPos, IMenuPageCanvas page)
        {
            if (newPos == page.Ordinal)
                return;

            if (newPos > _Pages.Count - 2)
            {
                /// move page to end.
                RemovePage(page);
                AddPage(page);
            }
            else
            {
                RemovePage(page);
                InsertPage(newPos, page);
            }
        }

        /// <MetaDataID>{d2b2be3f-38c4-440f-924a-29e8746fbf4d}</MetaDataID>
        public void InsertPage(int index, IMenuPageCanvas page)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Pages.Insert(index, page);
                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{ebe69fc5-a014-4ff1-84e9-784d199bb6e6}</MetaDataID>
        public void RemovePage(IMenuPageCanvas page)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Pages.Remove(page);

                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(page);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(Pages));
        }



        /// <MetaDataID>{4a9af651-1484-47a7-8781-870f61ee2bb1}</MetaDataID>
        internal IMenuCanvasHeading GetLastFoodItemsHeading(MenuPage menuPage)
        {
            int pagePos = _Pages.IndexOf(menuPage);
            if (pagePos == 0)
                return null;
            MenuPage previousPage = _Pages[pagePos - 1] as MenuPage;
            if (previousPage.MenuCanvasItems.Count == 0)
                return GetLastFoodItemsHeading(previousPage);

            IMenuCanvasItem lastMenuCanvasItem = previousPage.MenuCanvasItems.Last();

            if (lastMenuCanvasItem is IMenuCanvasHeading && (lastMenuCanvasItem as IMenuCanvasHeading).HeadingType != HeadingType.SubHeading)
                return lastMenuCanvasItem as IMenuCanvasHeading;
            if (lastMenuCanvasItem is IGroupedItem)
                return (lastMenuCanvasItem as IGroupedItem).HostingArea.ItemsGroupHeading;
            return null;

        }

        /// <exclude>Excluded</exclude>
        public void OnCommitObjectState()
        {

        }

        /// <MetaDataID>{7d6e57f7-565d-4c84-ade5-ee47eedcf14a}</MetaDataID>
        public void BeforeCommitObjectState()
        {
        }

        /// <exclude>Excluded</exclude>
        public void OnActivate()
        {
            foreach (var menuCanvasItem in _MenuCanvasItems)
                menuCanvasItem.ObjectChangeState += ManuCanvasItemChangeState;
        }

        /// <exclude>Excluded</exclude>
        public void OnDeleting()
        {

        }

        /// <exclude>Excluded</exclude>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <exclude>Excluded</exclude>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{4aa8d191-d05e-4835-a59a-bd56b25f7b55}</MetaDataID>
        public IMenuCanvasFoodItem GetMenuCanvasFoodItem(string menuItemUri)
        {
            throw new NotImplementedException();
        }

       
    }
}