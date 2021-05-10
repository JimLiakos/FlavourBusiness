using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.Json;
#if MenuPresentationModel
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using UIBaseEx;
#else
using OOAdvantech.Json;
#endif

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{18c6023e-802a-479a-8d33-b6b6239d4bbf}</MetaDataID>
    public class MenuCanvasFoodItemText : MenuCanvas.IMenuCanvasFoodItemText
    {

        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{9320751e-3f88-47a4-b1aa-36e995e6e1f5}</MetaDataID>
        [JsonIgnore]
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }
        /// <MetaDataID>{b491c55b-a7d0-4dfb-a548-7f1ade5d92b3}</MetaDataID>
        public void ResetSize()
        {
        }
        /// <MetaDataID>{0899ff22-93cf-4738-8152-8bfd44b3e941}</MetaDataID>
        public MenuCanvasFoodItemText()
        {
        }


        /// <MetaDataID>{88359ecd-7ba6-4945-91dc-92255b0491bd}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }



        /// <MetaDataID>{9a082779-6a58-4054-9521-fbc33a5e285c}</MetaDataID>
        JsonMenuPresentation.MenuCanvasFoodItem MenuCanvasFoodItem;
        /// <MetaDataID>{e38d41e8-5cc2-45a3-b14c-6c16de4c0c54}</MetaDataID>
        public MenuCanvasFoodItemText(IRestaurantMenu menu, MenuCanvas.IMenuCanvasFoodItemText subText, JsonMenuPresentation.MenuCanvasFoodItem menuCanvasFoodItem)
        {
            MenuCanvasFoodItem = menuCanvasFoodItem;
            Height = subText.Height;
            Width = subText.Width;
            XPos = subText.XPos;
            YPos = subText.YPos;
            Description = subText.Description;
            FontSpacingCorrection = subText.FontSpacingCorrection;

            FontID = (menu as RestaurantMenu).GetFontID(subText.Font);
            Font = subText.Font;
            BaseLine = subText.BaseLine;// Font.GetTextBaseLine(subText.Description);
            Type = GetType().Name;
        }
        /// <MetaDataID>{73b1808a-9c33-4b11-bcdc-a9c7ff80b853}</MetaDataID>
        public string Type { get; set; }

        /// <MetaDataID>{59ecdf30-2002-495f-a2ed-7bd8733f3a5e}</MetaDataID>
        public string Description { get; set; }
        /// <MetaDataID>{7f5318d1-ac86-4da9-8589-44182c54645c}</MetaDataID>
        public int FontID { get; set; }
        /// <MetaDataID>{491ccc8f-427c-4a2a-96a3-c98ea4a3339f}</MetaDataID>
        [JsonIgnore]
        public FontData Font { get; set; }

        /// <MetaDataID>{03d17e57-14a5-4f3c-b2f2-45c1fa3cfb3e}</MetaDataID>
        public IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return MenuCanvasFoodItem;
            }
            set
            {
                MenuCanvasFoodItem = value as JsonMenuPresentation.MenuCanvasFoodItem;
            }
        }

        /// <MetaDataID>{4762a8b7-0c53-4bb3-9976-525edbe04624}</MetaDataID>
        public double Height { get; set; }


        /// <MetaDataID>{6d6d68f2-683e-4c62-8625-3281b5fb6005}</MetaDataID>
        [JsonIgnore]
        public IMenuPageCanvas Page
        {
            get
            {
                return MenuCanvasFoodItem.Page;
            }
        }

        /// <MetaDataID>{3783ac9b-f64c-438d-93d5-93a512ee12ef}</MetaDataID>
        public double Width { get; set; }

        /// <MetaDataID>{808acd58-f8ea-468e-8254-30955100d3bf}</MetaDataID>
        public double XPos { get; set; }

        /// <MetaDataID>{22b2b5b2-358a-4c57-ab93-85cfd9a6d444}</MetaDataID>
        public double YPos { get; set; }
        /// <MetaDataID>{37cb0057-c911-44de-829f-3e0bf43ecd3e}</MetaDataID>
        public double BaseLine { get; set; }

        /// <MetaDataID>{f27e8fdc-28cd-4c75-9d45-a6d4a9a6ef2b}</MetaDataID>
        public double FontSpacingCorrection { get; set; }


#if MenuPresentationModel
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{29de1910-60ac-4019-8f54-09552d484a04}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{0b16c4ac-3d92-4524-985f-8d459e968fdd}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
