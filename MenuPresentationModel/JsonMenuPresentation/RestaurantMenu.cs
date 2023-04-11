using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MenuPresentationModel.MenuCanvas;

using System.Xml.Linq;
using System.IO;
using UIBaseEx;
using System.Net.Http;
using MenuModel.JsonViewModel;
using System.Security.Cryptography;



#if MenuPresentationModel
using FlavourBusinessToolKit;
using OOAdvantech.Json;
#else
using OOAdvantech.Json;
#endif
namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{5a627b98-f614-431d-8b68-0176bdc88aac}</MetaDataID>
    public class RestaurantMenu : IRestaurantMenu
    {


        //[OOAdvantech.Json.JsonConstructor]
        //public RestaurantMenu(string type, double pageWidth, double PageHeight, List<IMenuCanvasItem> menuCanvasItems, string name, List<IMenuPageCanvas> pages)
        //{

        //}
        public RestaurantMenu()
        {

        }

#if MenuPresentationModel
        /// <MetaDataID>{53164ae7-d190-4e72-bdca-28baf5febb4a}</MetaDataID>
        public RestaurantMenu(IRestaurantMenu menu)
        {
            Dictionary<object, object> mappedObject = new Dictionary<object, object>();
            _MenuCanvasItems = new List<IMenuCanvasItem>();
            if (!string.IsNullOrWhiteSpace(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menu).StorageMetaData.Culture))
            {
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menu).StorageMetaData.Culture), false))
                {
                    _Pages.Def = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
                }
            }

            foreach (var languageEntry in menu.MultilingualPages.Values)
            {
                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(languageEntry.Key), false))
                {
                    List<IMenuPageCanvas> pages = (languageEntry.Value as System.Collections.IList).OfType<IMenuPageCanvas>().ToList();
                    List<IMenuPageCanvas> jsonPages = new List<IMenuPageCanvas>();

                    foreach (var page in pages)
                    {
                        var jsonPage = new JsonMenuPresentation.MenuPageCanvas(this, page, mappedObject);
                        jsonPages.Add(jsonPage);

                        foreach (var menuCanvasItem in jsonPage.MenuCanvasItems)
                        {
                            if (!_MenuCanvasItems.Contains(menuCanvasItem))
                                _MenuCanvasItems.Add(menuCanvasItem);
                        }
                    }
                    _Pages.SetValue(jsonPages);
                }
            }
            _PageWidth = ((menu as MenuPresentationModel.RestaurantMenu).Style.Styles["page"] as MenuStyles.PageStyle).PageWidth;
            _PageHeight = ((menu as MenuPresentationModel.RestaurantMenu).Style.Styles["page"] as MenuStyles.PageStyle).PageHeight;
            Type = GetType().Name;

            MenuModel.IMenuItem menuItem = mappedObject.Keys.Where(x => x is MenuModel.IMenuItem).OfType<MenuModel.IMenuItem>().FirstOrDefault();
            MealTypes = new List<MenuModel.IMealType>();
            if (menuItem!=null)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem));
                foreach (var mealType in (from a_mealType in storage.GetObjectCollection<MenuModel.IMealType>()
                                          select a_mealType))
                {
                    MenuModel.IMealType jsonMealType = null;
                    if (mappedObject.ContainsKey(mealType))
                        jsonMealType = mappedObject[mealType] as MenuModel.IMealType;
                    else
                        jsonMealType = new MenuModel.JsonViewModel.MealType(mealType, mappedObject) as MenuModel.IMealType;
                    MealTypes.Add(jsonMealType);
                }
            }
        }

        public List<MenuModel.IMealType> MealTypes { get; set; }

        public List<FontData> MenuFonts { get; set; } = new List<FontData>();
        internal int GetFontID(FontData font)
        {
            var existingFont = MenuFonts.Where(x => x==font).FirstOrDefault();
            if (MenuFonts.Where(x => x==font).Count()==0)
            {

                var client = new HttpClient();
                string menuModelFontUrl = string.Format("http://{0}:8090/api/MenuModel/Font", FlavourBusinessFacade.ComputingResources.EndPoint.Server);
                var request = new HttpRequestMessage(HttpMethod.Post, menuModelFontUrl);

                var content = new StringContent(OOAdvantech.Json.JsonConvert.SerializeObject(font), null, "application/json");
                request.Content = content;
                var responseTask = client.SendAsync(request);
                responseTask.Wait();
                var response = responseTask.Result;
                response.EnsureSuccessStatusCode();
                var contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();

                font.Uri=contentTask.Result;
                if (font.Uri[0]=='"'&&font.Uri[font.Uri.Length-1]=='"')
                    font.Uri =font.Uri.Substring(1, font.Uri.Length-2);
                existingFont=font;
                MenuFonts.Add(font);
            }
            int fontID = MenuFonts.IndexOf(existingFont) + 1;
            return fontID;
        }
        internal FontData GetFont(int fontID)
        {
            return MenuFonts[fontID-1];
        }

#endif

        /// <MetaDataID>{3b146064-345b-4565-b25b-8ac17b33f3e0}</MetaDataID>
        public string Type { get; set; }


        /// <MetaDataID>{89a29e23-afde-4cc4-a313-76ee506a54cb}</MetaDataID>
        double _PageWidth;
        /// <MetaDataID>{eb3aedbf-5543-42fb-9b8f-e83a6a4e4b37}</MetaDataID>
        public double PageWidth
        {
            get
            {
                return _PageWidth;
            }
            set
            {
                _PageWidth = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _PageHeight;
        /// <MetaDataID>{ba063941-fe6f-4534-8567-e76eec030e7e}</MetaDataID>
        public double PageHeight
        {
            get
            {
                return _PageHeight;
            }
            set
            {
                _PageHeight = value;
            }
        }

        /// <MetaDataID>{501a260c-35de-446b-896b-61a46890ad94}</MetaDataID>
        List<IMenuCanvasItem> _MenuCanvasItems = new List<IMenuCanvasItem>();
        /// <MetaDataID>{e022a38f-805c-4416-afc2-011448928ccb}</MetaDataID>
        public IList<IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                return _MenuCanvasItems;
            }
            set
            {
                if (value != null)
                    _MenuCanvasItems = value.ToList();
                else
                    _MenuCanvasItems = null;
            }
        }
        public void RemoveMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {
            if (_MenuCanvasItems != null && _MenuCanvasItems.Contains(manuCanvasitem))
                _MenuCanvasItems.Remove(manuCanvasitem);

        }

        public void AddMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {
            if (_MenuCanvasItems != null && !_MenuCanvasItems.Contains(manuCanvasitem))
                _MenuCanvasItems.Add(manuCanvasitem);

        }

        /// <MetaDataID>{13fe68bf-0847-40b0-9c6f-4a25037d0d6d}</MetaDataID>
        string _Name;
        /// <MetaDataID>{ee89bf56-0f91-4f0f-abbf-35378860a0c4}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }


        public OOAdvantech.Multilingual MultilingualPages { get => _Pages; set => _Pages = value; }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Multilingual _Pages = new OOAdvantech.Multilingual();
        /// <MetaDataID>{04e1cf05-11f1-4773-99cb-916fe7e23b5f}</MetaDataID>
        [JsonIgnore]
        public IList<IMenuPageCanvas> Pages
        {
            get
            {
                if (_Pages.GetValue<List<MenuPageCanvas>>() == null)
                {
                    var typedCollection = new List<IMenuPageCanvas>();
                    var collection = _Pages.GetValue<object>() as System.Collections.IEnumerable;
                    if (collection!=null)
                    {
                        foreach (IMenuPageCanvas page in collection)
                            typedCollection.Add(page);
                        _Pages.SetValue<List<IMenuPageCanvas>>(typedCollection);

                        return typedCollection;
                    }
                    else
                        return new List<IMenuPageCanvas>();
                }
                return _Pages.GetValue<List<MenuPageCanvas>>().AsReadOnly().OfType<IMenuPageCanvas>().ToList();
            }
            set
            {
                if (value != null)
                    _Pages.SetValue<List<IMenuPageCanvas>>(value.ToList());
                else
                    _Pages = null;
            }
        }

        /// <MetaDataID>{da319689-b8dc-4c45-bd80-a935edeb05af}</MetaDataID>
        public void AddPage(IMenuPageCanvas page)
        {

        }

        /// <MetaDataID>{a8c57183-69d7-4945-92a3-1840f61f483c}</MetaDataID>
        public void InsertPage(int index, IMenuPageCanvas page)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{231e2eaf-368e-4de0-a5af-60367336009e}</MetaDataID>
        public void RemovePage(IMenuPageCanvas page)
        {
            throw new NotImplementedException();
        }

        public IMenuCanvasFoodItem GetMenuCanvasFoodItem(string menuItemUri)
        {
            var init = (from page in Pages
                        from menuCanvasItem in page.MenuCanvasItems
                        select menuCanvasItem).ToList();

            return this.MenuCanvasItems.OfType<IMenuCanvasFoodItem>().Where(x => (x.MenuItem as MenuFoodItem)?.Uri== menuItemUri).FirstOrDefault();
        }
    }
}
