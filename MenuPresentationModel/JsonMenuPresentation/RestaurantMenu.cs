﻿using System;
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
using MenuPresentationModel.MenuStyles;
using System.Windows.Controls.Primitives;



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
        /// <MetaDataID>{f59f5e92-7686-4821-ae48-e8adff55e4d3}</MetaDataID>
        public RestaurantMenu()
        {

        }

#if MenuPresentationModel
        /// <MetaDataID>{53164ae7-d190-4e72-bdca-28baf5febb4a}</MetaDataID>
        public RestaurantMenu(IRestaurantMenu menu)
        {

            ItemExtraInfoStyleSheet = new ItemExtraInfoStyleSheet(this, menu.ItemExtraInfoStyleSheet);

            //ItemExtraInfoStyleSheet= new ItemExtraInfoStyleSheet(this, menu.Style.Styles["menu-item"] as MenuPresentationModel.MenuStyles.MenuItemStyle);
            Dictionary<object, object> mappedObject = new Dictionary<object, object>();
            _MenuCanvasItems = new List<IMenuCanvasItem>();
            if (!string.IsNullOrWhiteSpace(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menu).StorageMetaData.Culture))
            {
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menu).StorageMetaData.Culture), false))
                {
                    _Pages.Def = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
                }
            }

            
            if((menu.Style.Styles["order-pad"] as IOrderPadStyle)?.Background!=null)
                OrderPadBackground = new PageImage((menu.Style.Styles["order-pad"] as IOrderPadStyle).Background); 
            else
            {

            }
            OrderPadBackgroundMargin = (menu.Style.Styles["order-pad"] as IOrderPadStyle).BackgroundMargin;
            OrderPadBackgroundStretch = (menu.Style.Styles["order-pad"] as IOrderPadStyle).BackgroundStretch;

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

        /// <MetaDataID>{76d231f2-08d2-441e-aa09-3314e26a4010}</MetaDataID>
        public List<MenuModel.IMealType> MealTypes { get; set; }

       

        /// <MetaDataID>{84b040cb-32c2-4079-b6d1-c238395a1960}</MetaDataID>
        public List<FontData> MenuFonts { get; set; } = new List<FontData>();
        /// <MetaDataID>{440178af-19d4-422f-ae64-ccaf8a162d92}</MetaDataID>
        internal int GetFontID(FontData font)
        {
            var existingFont = MenuFonts.Where(x => x == font).FirstOrDefault();
            if (MenuFonts.Where(x => x == font).Count() == 0)
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

                font.Uri = contentTask.Result;
                if (font.Uri[0] == '"' && font.Uri[font.Uri.Length - 1] == '"')
                    font.Uri = font.Uri.Substring(1, font.Uri.Length - 2);
                existingFont = font;
                MenuFonts.Add(font);
            }
            int fontID = MenuFonts.IndexOf(existingFont) + 1;
            return fontID;
        }
        /// <MetaDataID>{7532f24b-a99e-4916-8c8d-6192dd5f839a}</MetaDataID>
        internal FontData GetFont(int fontID)
        {
            return MenuFonts[fontID - 1];
        }

#endif

        /// <MetaDataID>{fca0ece9-69a2-4660-8620-d5499bc3cade}</MetaDataID>
        public IPageImage OrderPadBackground { get; set; }
        /// <MetaDataID>{e696c9bc-10b5-450d-946a-b7b2e7b7f258}</MetaDataID>
        public Margin OrderPadBackgroundMargin { get; set; }
        /// <MetaDataID>{0d2c5d81-e777-4735-9867-7f57d437a0e5}</MetaDataID>
        public ImageStretch OrderPadBackgroundStretch { get; set; }

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
        /// <MetaDataID>{c97d0260-6fcb-4471-bdbd-071eba012b22}</MetaDataID>
        public void RemoveMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {
            if (_MenuCanvasItems != null && _MenuCanvasItems.Contains(manuCanvasitem))
                _MenuCanvasItems.Remove(manuCanvasitem);

        }

        /// <MetaDataID>{91b521a0-c094-42fb-8947-3f8fe4b00c46}</MetaDataID>
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


        /// <MetaDataID>{a9725368-63d4-4d4b-ac60-7f90943e65f0}</MetaDataID>
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

        /// <MetaDataID>{58d46375-02ce-464d-bd4b-3979ee3825d2}</MetaDataID>
        public IStyleSheet Style { get; set; }

        /// <MetaDataID>{6ebc7bf5-f0b2-46f2-8a28-1c926b72be1b}</MetaDataID>
        public IItemExtraInfoStyleSheet ItemExtraInfoStyleSheet { get; set; }

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

        /// <MetaDataID>{f0213f02-015c-451a-aa9f-535abb375822}</MetaDataID>
        public IMenuCanvasFoodItem GetMenuCanvasFoodItem(string menuItemUri)
        {
            var init = (from page in Pages
                        from menuCanvasItem in page.MenuCanvasItems
                        select menuCanvasItem).ToList();

            return this.MenuCanvasItems.OfType<IMenuCanvasFoodItem>().Where(x => (x.MenuItem as MenuFoodItem)?.Uri == menuItemUri).FirstOrDefault();
        }

        public void GetItemExtraInfoStylingData()
        {
            throw new NotImplementedException();
        }
    }
}
