using MenuPresentationModel.MenuCanvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using System.Windows;
using OOAdvantech.Json;
using UIBaseEx;
using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{55dcbb36-ee5f-4b2d-a335-5c21aafe9ac7}</MetaDataID>
    public class MenuCanvasHeading : MenuCanvas.IMenuCanvasHeading
    {


        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{1c4e643d-5b76-4263-bcfc-2dcadfc053e2}</MetaDataID>
        [JsonIgnore]
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }

        /// <MetaDataID>{22e7def2-fbe1-4a31-a92c-2712c1ccbd4e}</MetaDataID>
        public void ResetSize()
        {
        }
        /// <MetaDataID>{288c4cc6-8858-4cc5-b511-9a3f374bfcf2}</MetaDataID>
        public void Remove()
        {
            // (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <MetaDataID>{ddd08265-8c5c-4bab-87d8-9f69c2c445eb}</MetaDataID>
        public MenuCanvasHeading()
        {

        }
        //IMenuPageCanvas _Page;
        /// <MetaDataID>{5152d49c-fefb-4542-8834-dc60480fd95d}</MetaDataID>
        Multilingual _Page = new Multilingual();


#if MenuPresentationModel
        /// <MetaDataID>{6814b024-d67b-44f5-96ba-1b5c5d1c9bef}</MetaDataID>
        public MenuCanvasHeading Init(IRestaurantMenu menu, IMenuCanvasHeading menuCanvasHeading, IMenuPageCanvas page, Dictionary<object, object> mappedObjects)
        {

            if (!string.IsNullOrWhiteSpace(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuCanvasHeading).StorageMetaData.Culture))
            {
                using (OOAdvantech.CultureContext cultureContext = new CultureContext(System.Globalization.CultureInfo.GetCultureInfo(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuCanvasHeading).StorageMetaData.Culture), false))
                {
                    _Page.Def = MultilingualDescription.Def = MultilingualFont.Def = MultilingualHeight.Def = MultilingualWidth.Def = MultilingualXPos.Def = MultilingualYPos.Def = MultilingualFullRowWidth.Def = MultilingualBaseLine.Def = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
                }
            }


            _Page.SetValue<IMenuPageCanvas>(page);
            Description = menuCanvasHeading.Description;
            Font = menuCanvasHeading.Font;
            FontID = (menu as RestaurantMenu).GetFontID(Font);
            Height = menuCanvasHeading.Height;
            Width = menuCanvasHeading.Width;
            XPos = menuCanvasHeading.XPos;
            YPos = menuCanvasHeading.YPos;
            Width = menuCanvasHeading.Width;
            if (menuCanvasHeading.MenuCanvasAccent != null)
            {
                object mappedObject = null;
                if (mappedObjects.TryGetValue(menuCanvasHeading.MenuCanvasAccent, out mappedObject))
                {
                    Accent = mappedObject as JsonMenuPresentation.MenuCanvasHeadingAccent;
                    //if (OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name == (mappedObject as JsonMenuPresentation.MenuCanvasHeadingAccent).Culture)
                        Accent = (mappedObject as MenuCanvasHeadingAccent).Init(menuCanvasHeading.MenuCanvasAccent, mappedObjects);
                    //else
                    //    Accent = new MenuCanvasHeadingAccent().Init(menuCanvasHeading.MenuCanvasAccent, mappedObjects);

                }
                else
                    Accent = new JsonMenuPresentation.MenuCanvasHeadingAccent().Init(menuCanvasHeading.MenuCanvasAccent, mappedObjects);
            }
            CanvasFrameArea = new MenuCanvas.Rect(XPos, YPos, Width, Height);
            Type = GetType().Name;
            BaseLine = menuCanvasHeading.BaseLine;// Font.GetTextBaseLine(menuCanvasHeading.Description);

            return this;
        }
#endif
        /// <MetaDataID>{a481afc2-da1a-4264-8474-55e47a24b2d5}</MetaDataID>
        public string Type { get; set; }



        ///// <MetaDataID>{932fe21d-d047-4c59-96a9-49406925e650}</MetaDataID>
        //public Multilingual MultilingualAccent = new Multilingual();

        public IMenuCanvasAccent _Accent;

        /// <MetaDataID>{a339283f-d5b4-4eae-9f63-2d92240a7d78}</MetaDataID>
        
        public IMenuCanvasAccent Accent { get => _Accent; set => _Accent = value; }


        
        //public IMenuCanvasHeadingAccent Accent { get; set; }

        /// <MetaDataID>{65e7a18b-cd7e-419f-8e27-ee19a0738a80}</MetaDataID>
        [JsonIgnore]
        public MenuCanvas.Rect CanvasFrameArea { get; set; }


        /// <MetaDataID>{932fe21d-d047-4c59-96a9-49406925e650}</MetaDataID>
        public Multilingual MultilingualDescription = new Multilingual();
        /// <MetaDataID>{3e00957d-5c2b-4b6d-ad23-6330d02a86c7}</MetaDataID>
        [JsonIgnore]
        public string Description { get => MultilingualDescription.GetValue<string>(); set => MultilingualDescription.SetValue<string>(value); }

        /// <MetaDataID>{abd9dfd5-7447-4037-9a7d-2ed2dc75a9ca}</MetaDataID>
        [JsonIgnore]
        public Multilingual MultilingualFont = new Multilingual();
        /// <MetaDataID>{cf2b8de3-cb39-48f9-b9b0-b5788dc047d7}</MetaDataID>
        [JsonIgnore]
        public FontData Font { get => MultilingualFont.GetValue<FontData>(); set => MultilingualFont.SetValue<FontData>(value); }

        /// <MetaDataID>{aca302fa-32e9-410d-a3db-da8f4e9e44d2}</MetaDataID>
        public Multilingual MultilingualFontID = new Multilingual();
        /// <MetaDataID>{e9a57c8f-eaeb-4edc-a03a-c180e51f670d}</MetaDataID>
        public int FontID { get => MultilingualFontID.GetValue<int>(); set => MultilingualFontID.SetValue<int>(value); }

        /// <MetaDataID>{3a74b05a-fe60-4124-862b-52c3e7a8e2a2}</MetaDataID>
        public Multilingual MultilingualHeight = new Multilingual();
        /// <MetaDataID>{757cfa06-5348-41b8-9825-9ed44e21f51e}</MetaDataID>
        [JsonIgnore]
        public double Height { get => MultilingualHeight.GetValue<double>(); set => MultilingualHeight.SetValue<double>(value); }


        /// <MetaDataID>{55505d01-6bf4-406b-af71-beb2fae61ecf}</MetaDataID>
        public Multilingual MultilingualWidth = new Multilingual();
        /// <MetaDataID>{888b890b-4099-4156-bfa7-a8d4a9e1ae22}</MetaDataID>
        [JsonIgnore]
        public double Width { get => MultilingualWidth.GetValue<double>(); set => MultilingualWidth.SetValue<double>(value); }

        /// <MetaDataID>{31e21603-d823-4f37-8f70-4993ff0a0a12}</MetaDataID>
        public Multilingual MultilingualXPos = new Multilingual();
        /// <MetaDataID>{5835ad95-dac3-4a39-8f1f-4bbf968782a3}</MetaDataID>
        [JsonIgnore]
        public double XPos { get => MultilingualXPos.GetValue<double>(); set => MultilingualXPos.SetValue<double>(value); }


        /// <MetaDataID>{11eb821c-2be8-4d6a-b2e2-93638555c6d1}</MetaDataID>
        public Multilingual MultilingualYPos = new Multilingual();
        /// <MetaDataID>{f462bd54-5497-4acb-b9c6-8f4310095bb1}</MetaDataID>
        [JsonIgnore]
        public double YPos { get => MultilingualYPos.GetValue<double>(); set => MultilingualYPos.SetValue<double>(value); }



        /// <MetaDataID>{c3389b04-4714-4c0b-b89f-98c31ac5c5a1}</MetaDataID>
        public Multilingual MultilingualFullRowWidth = new Multilingual();
        /// <MetaDataID>{888b890b-4099-4156-bfa7-a8d4a9e1ae22}</MetaDataID>
        [JsonIgnore]
        public double FullRowWidth { get => MultilingualFullRowWidth.GetValue<double>(); set => MultilingualFullRowWidth.SetValue<double>(value); }


        /// <MetaDataID>{54c2f87a-b1d1-4104-8c26-e6d1d41b96d3}</MetaDataID>
        public Multilingual MultilingualBaseLine = new Multilingual();
        /// <MetaDataID>{888b890b-4099-4156-bfa7-a8d4a9e1ae22}</MetaDataID>
        [JsonIgnore]
        public double BaseLine { get => MultilingualBaseLine.GetValue<double>(); set => MultilingualBaseLine.SetValue<double>(value); }


        /// <MetaDataID>{fd92734d-29fb-4dc7-9adc-b8a5379a3876}</MetaDataID>
        public HeadingType HeadingType { get; set; }


        /// <MetaDataID>{c08b08b5-03f7-416c-bf07-1d17074f3e66}</MetaDataID>
        public IList<IMenuCanvasFoodItemsGroup> HostingAreas
        {
            get
            {
                return new List<IMenuCanvasFoodItemsGroup>();
            }
        }

        /// <MetaDataID>{647b8266-3c8c-4036-b527-b915107481ec}</MetaDataID>
        public bool NextColumnOrPage { get; set; }

        /// <MetaDataID>{9b04d94d-3de5-4f46-8eb0-e068edfb8f75}</MetaDataID>
        public int NumberOfFoodColumns { get; set; }

        /// <MetaDataID>{cae42883-8124-4e89-b2b4-25894b61b545}</MetaDataID>
        [JsonIgnore]
        public IMenuPageCanvas Page
        {
            get
            {
                return _Page.GetValue<IMenuPageCanvas>();
            }
        }



#if MenuPresentationModel
        /// <MetaDataID>{e69824c0-6ca5-4395-a475-cad5eda136c6}</MetaDataID>
        public IHeadingStyle Style
        {
            get
            {
                return null;
            }
        }

        /// <MetaDataID>{689efb72-7379-4ff6-b4c3-57ac0ef8c5c0}</MetaDataID>
        [JsonIgnore]
        public bool IsStyleAlignmentOverridden { get; set; }
        /// <MetaDataID>{c224b8fb-0b80-4741-bdd9-9a665eea687f}</MetaDataID>
        [JsonIgnore]
        public Alignment Alignment { get; set; }
        /// <MetaDataID>{57191c44-a58b-44b2-af63-e6ec60b05caa}</MetaDataID>
        public bool CustomSpacing { get; set; }
        /// <MetaDataID>{22067909-b56f-46ab-84d2-e7b030a7d63d}</MetaDataID>
        public double AfterSpacing { get; set; }
        /// <MetaDataID>{45f3016a-a92f-4a20-8296-5251309056d4}</MetaDataID>
        public double BeforeSpacing { get; set; }
        /// <MetaDataID>{16c6e209-0333-44a5-82ec-bb916c0a2143}</MetaDataID>
        public IMenuCanvasAccent MenuCanvasAccent { get; set; }


        public event ObjectChangeStateHandle ObjectChangeState;
        

        /// <MetaDataID>{bbda705d-00a2-4897-8ffc-bdf174d24e7e}</MetaDataID>
        public void AddHostingArea(IMenuCanvasFoodItemsGroup hostingArea)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{604d21ff-76c8-4e1c-96ed-176a757ee1b3}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{529981e5-efe3-4b56-91b6-756bcf983f0d}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{368ce301-38b7-49cb-8d48-bb5f9b3a21ea}</MetaDataID>
        public void RemoveAllHostingAreas()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{5ee7008a-c6ad-4bb6-accc-28a69fc2cec3}</MetaDataID>
        public void RemoveHostingArea(IMenuCanvasFoodItemsGroup hostingArea)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c1cf0920-7577-43b2-95b6-912f1444efd5}</MetaDataID>
        public void ClearAlignment()
        {
            throw new NotImplementedException();
        }
#endif
    }
}
