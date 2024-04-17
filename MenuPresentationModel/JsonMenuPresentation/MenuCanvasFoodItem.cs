using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MenuModel;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{d8ad80d8-a03c-4c75-8ce8-b12707564957}</MetaDataID>
    public class MenuCanvasFoodItem : IMenuCanvasItemEx, MenuCanvas.IMenuCanvasFoodItem
    {
        /// <MetaDataID>{65544137-1404-45ce-a428-f0e70c62bdef}</MetaDataID>
        public void ResetSize()
        {

        }
        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{691ed6d6-6ca1-4df5-b7d6-b23a6a47928a}</MetaDataID>
        [JsonIgnore]
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }
        /// <MetaDataID>{33a68314-eb5e-42ac-9971-bc36cbb6a113}</MetaDataID>
        public void Remove()
        {

            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }


        /// <MetaDataID>{26c4d09a-b1e4-460b-9c1a-53fc260cfbdf}</MetaDataID>
        public MenuCanvasFoodItem()
        {

        }

        /// <MetaDataID>{f7dd8e6a-d282-4ce5-aefd-804417771673}</MetaDataID>
        [JsonIgnore]
        public double BaseLine { get; set; }
        /// <MetaDataID>{6a337b73-cc53-4ada-bc70-ed59c905fb38}</MetaDataID>
        public bool CustomSpacing { get; set; }
        /// <MetaDataID>{3f4b139d-9911-482e-ac7a-c63e1a7f14c8}</MetaDataID>
        public double AfterSpacing { get; set; }
        /// <MetaDataID>{fdc53978-ff2c-407e-abac-96f0e31b4c5c}</MetaDataID>
        public double BeforeSpacing { get; set; }


        /// <MetaDataID>{809bbcbd-85d3-4110-8092-4a3a426f37b5}</MetaDataID>
        //IMenuPageCanvas _Page;
        Multilingual _Page = new Multilingual();

#if MenuPresentationModel



        /// <MetaDataID>{56fc74d3-9f45-4e76-b58e-a23a39e069f8}</MetaDataID>
        public MenuCanvasFoodItem Init(IRestaurantMenu menu, IMenuCanvasFoodItem menuCanvasFoodItem, IMenuPageCanvas page, Dictionary<object, object> mappedObjects)
        {
            if (!string.IsNullOrWhiteSpace(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuCanvasFoodItem).StorageMetaData.Culture))
            {
                using (OOAdvantech.CultureContext cultureContext = new CultureContext(System.Globalization.CultureInfo.GetCultureInfo(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuCanvasFoodItem).StorageMetaData.Culture), false))
                {
                    _Page.Def = MultilingualDescription.Def = MultilingualExtraDescription.Def = MultilingualExtras.Def = MultilingualFullDescription.Def = MultilingualFont.Def = MultilingualPriceLeader.Def = _SubTexts.Def = MultilingualHeight.Def = MultilingualWidth.Def = MultilingualXPos.Def = MultilingualYPos.Def = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
                }
            }

            _Page.SetValue<IMenuPageCanvas>(page);

            if (menuCanvasFoodItem.Description != null)
                Description = menuCanvasFoodItem.Description;

            if (menuCanvasFoodItem.Extras != null)
                Extras = menuCanvasFoodItem.Extras;

            if (menuCanvasFoodItem.ExtraDescription != null)
                ExtraDescription = menuCanvasFoodItem.ExtraDescription;

            Font = menuCanvasFoodItem.Font;
            FontID = (menu as RestaurantMenu).GetFontID(Font);
            Height = menuCanvasFoodItem.Height;
            Width = menuCanvasFoodItem.Width;
            XPos = menuCanvasFoodItem.XPos;
            YPos = menuCanvasFoodItem.YPos;
            Width = menuCanvasFoodItem.Width;
            object mappedObject = null;
            if (menuCanvasFoodItem.MenuCanvasAccent != null)
            {
                if (mappedObjects.TryGetValue(menuCanvasFoodItem.MenuCanvasAccent, out mappedObject))
                {
                    //if(OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name== (mappedObject as JsonMenuPresentation.MenuCanvasHeadingAccent).Culture)
                    Accent = (mappedObject as MenuCanvasHeadingAccent).Init(menuCanvasFoodItem.MenuCanvasAccent, mappedObjects);
                    //else
                    //    Accent = new MenuCanvasHeadingAccent().Init(menuCanvasFoodItem.MenuCanvasAccent, mappedObjects);

                }
                else
                    Accent = new MenuCanvasHeadingAccent().Init(menuCanvasFoodItem.MenuCanvasAccent, mappedObjects);
            }


            MaxHeight = Width = menuCanvasFoodItem.MaxHeight;
            _SubTexts.SetValue<List<IMenuCanvasFoodItemText>>((from subtext in menuCanvasFoodItem.SubTexts where !string.IsNullOrWhiteSpace(subtext.Description) select new MenuCanvasFoodItemText(menu, subtext, this)).OfType<IMenuCanvasFoodItemText>().ToList());
            _Prices.SetValue<List<IMenuCanvasFoodItemPrice>>((from price in menuCanvasFoodItem.Prices select new MenuCanvasFoodItemPrice(menu, price, this)).OfType<IMenuCanvasFoodItemPrice>().ToList());

            if (menuCanvasFoodItem.PriceLeader != null)
                PriceLeader = new MenuCanvasPriceLeader(menu, menuCanvasFoodItem.PriceLeader, this);
            Type = GetType().Name;


            if (menuCanvasFoodItem.MenuItem!=null)
            {

                
                if (mappedObjects.TryGetValue(menuCanvasFoodItem.MenuItem, out mappedObject))
                    MenuItem = mappedObject as MenuModel.JsonViewModel.MenuFoodItem;
                else
                    MenuItem = new MenuModel.JsonViewModel.MenuFoodItem(menuCanvasFoodItem.MenuItem, mappedObjects);// menuCanvasFoodItem.MenuItem;// new MenuItemsEditor.JsonViewModel.ItemPreparation(menuCanvasFoodItem.MenuItem, mappedObject);
            }
            else
            {
                //blanck
            }

            return this;
        }

        /// <MetaDataID>{932fe21d-d047-4c59-96a9-49406925e650}</MetaDataID>
        //public Multilingual MultilingualAccent = new Multilingual();
        IMenuCanvasAccent _Accent;
        /// <MetaDataID>{a339283f-d5b4-4eae-9f63-2d92240a7d78}</MetaDataID>

        public IMenuCanvasAccent Accent { get => _Accent; set => _Accent = value; }

#endif
        /// <MetaDataID>{606d4034-2229-48d8-b832-c3f63f1a18c2}</MetaDataID>
        public string Type { get; set; }


        /// <MetaDataID>{0c71efd9-b65a-4812-b013-f365d464e391}</MetaDataID>
        [JsonIgnore]
        public MenuCanvas.Rect CanvasFrameArea
        {
            get
            {
                return new MenuCanvas.Rect(XPos, YPos, Width, Height);
            }
        }

        /// <MetaDataID>{252c3b11-41d1-4da2-a9ac-f7b038825b6c}</MetaDataID>
        public Multilingual MultilingualFullDescription = new Multilingual();
        /// <MetaDataID>{e50d99dd-0a88-4b9b-bc91-8930ed508479}</MetaDataID>
        [JsonIgnore]
        public string FullDescription { get => MultilingualFullDescription.GetValue<string>(); set => MultilingualFullDescription.SetValue<string>(value); }


        /// <MetaDataID>{252c3b11-41d1-4da2-a9ac-f7b038825b6c}</MetaDataID>
        public Multilingual MultilingualExtraDescription = new Multilingual();
        /// <MetaDataID>{e50d99dd-0a88-4b9b-bc91-8930ed508479}</MetaDataID>
        [JsonIgnore]
        public string ExtraDescription { get => MultilingualExtraDescription.GetValue<string>(); set => MultilingualExtraDescription.SetValue<string>(value); }

        /// <MetaDataID>{bb495a05-442e-458c-bbe5-2cb3e7207ebf}</MetaDataID>
        public Multilingual MultilingualExtras = new Multilingual();
        /// <MetaDataID>{854087ba-246d-4b0b-9d55-ee27aa58e774}</MetaDataID>
        [JsonIgnore]
        public string Extras { get => MultilingualExtras.GetValue<string>(); set => MultilingualExtras.SetValue<string>(value); }


        /// <MetaDataID>{7efe6eac-d4f8-4e0a-84bc-371cff7be812}</MetaDataID>
        public Multilingual MultilingualDescription = new Multilingual();
        /// <MetaDataID>{3e00957d-5c2b-4b6d-ad23-6330d02a86c7}</MetaDataID>
        [JsonIgnore]
        public string Description { get => MultilingualDescription.GetValue<string>(); set => MultilingualDescription.SetValue<string>(value); }

        /// <MetaDataID>{151305fd-098b-4f55-aaa3-c7fb3b9c6ec8}</MetaDataID>
        [JsonIgnore]
        public Multilingual MultilingualFont = new Multilingual();
        /// <MetaDataID>{cf2b8de3-cb39-48f9-b9b0-b5788dc047d7}</MetaDataID>
        [JsonIgnore]
        public FontData Font
        {
            get
            {
                if(!MultilingualFont.HasValue)
                {
                    Font= (Page.Menu as RestaurantMenu).GetFont(FontID);
                }
                return MultilingualFont.GetValue<FontData>();
            }

            set => MultilingualFont.SetValue<FontData>(value);
        }
        /// <MetaDataID>{47255e97-ef95-4fd3-9cd0-37066e521327}</MetaDataID>
        public Multilingual MultilingualFontID = new Multilingual();
        /// <MetaDataID>{c6f1d381-8e2c-474a-b2e4-a11520efe66d}</MetaDataID>
        public int FontID { get => MultilingualFontID.GetValue<int>(); set => MultilingualFontID.SetValue<int>(value); }

        /// <MetaDataID>{51987fcf-3e1f-4753-b12f-c656257d3a6b}</MetaDataID>
        public Multilingual MultilingualHeight = new Multilingual();
        /// <MetaDataID>{757cfa06-5348-41b8-9825-9ed44e21f51e}</MetaDataID>
        [JsonIgnore]
        public double Height { get => MultilingualHeight.GetValue<double>(); set => MultilingualHeight.SetValue<double>(value); }


        /// <MetaDataID>{a17a4434-c235-428b-a8ee-5318124b89ee}</MetaDataID>
        public Multilingual MultilingualWidth = new Multilingual();
        /// <MetaDataID>{888b890b-4099-4156-bfa7-a8d4a9e1ae22}</MetaDataID>
        [JsonIgnore]
        public double Width { get => MultilingualWidth.GetValue<double>(); set => MultilingualWidth.SetValue<double>(value); }

        /// <MetaDataID>{be7c376f-9456-46bd-a0f9-2e0b3d489a51}</MetaDataID>
        public Multilingual MultilingualXPos = new Multilingual();

        /// <MetaDataID>{5835ad95-dac3-4a39-8f1f-4bbf968782a3}</MetaDataID>
        [JsonIgnore]
        public double XPos { get => MultilingualXPos.GetValue<double>(); set => MultilingualXPos.SetValue<double>(value); }


        /// <MetaDataID>{c6467271-e306-4aab-aeef-cf1bad1bd2de}</MetaDataID>
        public Multilingual MultilingualYPos = new Multilingual();
        /// <MetaDataID>{f462bd54-5497-4acb-b9c6-8f4310095bb1}</MetaDataID>
        [JsonIgnore]
        public double YPos { get => MultilingualYPos.GetValue<double>(); set => MultilingualYPos.SetValue<double>(value); }




        /// <MetaDataID>{e22aff1f-0932-4dd9-9a28-9759e2482724}</MetaDataID>
        public IMenuCanvasFoodItemsGroup HostingArea
        {
            get
            {
                return null;
            }
        }

        /// <MetaDataID>{75ffa3b1-0e85-4ed7-9255-d126fa3ca0bc}</MetaDataID>
        public IMenuCanvasFoodItemPrice MainPrice { get; set; }

        /// <MetaDataID>{0ff6679c-4036-4111-989a-3d7fa5bbc44f}</MetaDataID>
        public double MaxHeight { get; set; }

        /// <MetaDataID>{128f88c4-93f7-4e84-93df-3280523b1e3a}</MetaDataID>
        IMenuItem _MenuItem;
        /// <MetaDataID>{af6ecfcd-e737-4520-aa2c-fadbbc5ff3fa}</MetaDataID>
        public IMenuItem MenuItem
        {
            get
            {
                return _MenuItem;
            }
            set
            {
                _MenuItem = value;
            }
        }


        /// <MetaDataID>{ef23a176-61a2-4a97-910d-4ebb64d435d7}</MetaDataID>
        public IItemMultiPriceHeading MultiPriceHeading { get; set; }





        /// <MetaDataID>{43b155d6-9860-4276-9643-3e91df05890a}</MetaDataID>
        [JsonIgnore]
        /// <MetaDataID>{64134e4f-183c-4827-8c55-ce29af25267b}</MetaDataID>
        public IMenuPageCanvas Page
        {
            get
            {
                return _Page.GetValue<IMenuPageCanvas>();
            }
            set
            {
                _Page.SetValue<IMenuPageCanvas>(value);
            }
        }


        /// <MetaDataID>{12e5e8fd-7632-406f-affc-356c043952a7}</MetaDataID>
        public Multilingual MultilingualPriceLeader = new Multilingual();

        /// <MetaDataID>{16d6e11c-e677-4be5-ac24-6e2e804fb4b1}</MetaDataID>
        [JsonIgnore]
        public IMenuCanvasPriceLeader PriceLeader { get => MultilingualPriceLeader.GetValue<IMenuCanvasPriceLeader>(); set => MultilingualPriceLeader.SetValue<IMenuCanvasPriceLeader>(value); }


        /// <MetaDataID>{d34c3850-c6da-4dda-9599-d7e48bc1f293}</MetaDataID>
        public Multilingual MultilingualPrices { get => _Prices; set { } }


        /// <MetaDataID>{9d2ddd7e-aecf-48ef-94cb-9e3957fdf35b}</MetaDataID>
        Multilingual _Prices = new Multilingual();
        /// <MetaDataID>{e5a59a7c-3e17-4a28-888a-6ccc90137865}</MetaDataID>
        [JsonIgnore]
        public List<IMenuCanvasFoodItemPrice> Prices
        {
            get
            {
                return _Prices.GetValue<List<IMenuCanvasFoodItemPrice>>();
            }
        }


        /// <MetaDataID>{8dcffc8f-a7c9-4c66-83e8-ba7886832898}</MetaDataID>
        public Multilingual MultilingualSubTexts { get => _SubTexts; set { } }

        /// <exclude>Excluded</exclude>
        Multilingual _SubTexts = new Multilingual();

        /// <MetaDataID>{cfb33bed-c30c-4e47-ac7b-f5c27dbbd89f}</MetaDataID>
        [JsonIgnore]
        public IList<IMenuCanvasFoodItemText> SubTexts
        {
            get
            {
                return _SubTexts.GetValue<List<IMenuCanvasFoodItemText>>().AsReadOnly();
            }
            set
            {
                if (value != null)
                    _SubTexts.SetValue<List<IMenuCanvasFoodItemText>>(value.ToList());
                else
                    _SubTexts.SetValue<List<IMenuCanvasFoodItemText>>(null);
            }
        }


#if MenuPresentationModel
        /// <MetaDataID>{9396e6dd-ddd9-41a2-ab9b-fb2dad498a1c}</MetaDataID>
        public IMenuItemStyle Style
        {
            get
            {
                return null;
            }
        }

        /// <MetaDataID>{1d8411cc-de4e-4f91-b73e-df441e65c7e7}</MetaDataID>
        [JsonIgnore]
        public bool Span { get; set; }


        [JsonIgnore]
        public bool PriceInvisible { get; set; }



        /// <MetaDataID>{940a8f5c-ff9e-44a5-bdee-f87c46f65582}</MetaDataID>
        public IMenuCanvasAccent MenuCanvasAccent { get; set; }
#endif

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{291442e0-28b5-4313-a76f-edd9cbb6348a}</MetaDataID>
        public void AddFoodItemPrice(IMenuCanvasFoodItemPrice foodItemPrice)
        {

            List<IMenuCanvasFoodItemPrice> prices = _Prices.GetValue<List<IMenuCanvasFoodItemPrice>>();
            if (prices == null)
                prices = new List<IMenuCanvasFoodItemPrice>();
            prices.Add(foodItemPrice);
            _Prices.SetValue<List<IMenuCanvasFoodItemPrice>>(prices);
        }

        /// <MetaDataID>{a35ae54b-49e7-4bd9-af06-231cea00e3d9}</MetaDataID>
        public void AddSubText(IMenuCanvasFoodItemText subText)
        {

            List<IMenuCanvasFoodItemText> subTexts = _SubTexts.GetValue<List<IMenuCanvasFoodItemText>>();
            if (subTexts == null)
                subTexts = new List<IMenuCanvasFoodItemText>();
            subTexts.Add(subText);

            _SubTexts.SetValue<List<IMenuCanvasFoodItemText>>(subTexts);
        }


#if MenuPresentationModel
        /// <MetaDataID>{88e613f9-dd4b-47ee-baf7-181965d5db88}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c1424caa-0c07-4cf2-9466-675f0233bc45}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
#endif

        /// <MetaDataID>{fa05ef1d-e71f-4280-99cb-9e7a805583a6}</MetaDataID>
        public void RemoveFoodItemPrice(IMenuCanvasFoodItemPrice foodItemPrice)
        {
            List<IMenuCanvasFoodItemPrice> prices = _Prices.GetValue<List<IMenuCanvasFoodItemPrice>>();
            if (prices != null)
                prices.Remove(foodItemPrice);
        }

        /// <MetaDataID>{893b038a-64d5-4c59-8388-063d979325c7}</MetaDataID>
        public void RemoveSubText(IMenuCanvasFoodItemText subText)
        {
            List<IMenuCanvasFoodItemText> subTexts = _SubTexts.GetValue<List<IMenuCanvasFoodItemText>>();
            if (subTexts != null)
                subTexts.Remove(subText);
        }
    }
}


