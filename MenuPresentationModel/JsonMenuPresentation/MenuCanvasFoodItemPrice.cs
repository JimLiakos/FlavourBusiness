using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MenuModel;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using UIBaseEx;
using OOAdvantech.MetaDataRepository;
#if MenuPresentationModel
using OOAdvantech.Json;
#else
using OOAdvantech.Json;
#endif
using OOAdvantech;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{d5222b52-1264-423d-b472-9463426428ab}</MetaDataID>
    public class MenuCanvasFoodItemPrice : IMenuCanvasItemEx, MenuCanvas.IMenuCanvasFoodItemPrice
    {

        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{d7c4962c-3f7e-4361-a78f-748e71450023}</MetaDataID>
        [JsonIgnore]
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }

        /// <MetaDataID>{d1369b3a-4245-46bf-9a93-1d8eb1f44995}</MetaDataID>
        public void ResetSize()
        {
        }

        /// <MetaDataID>{ada9cbf6-c7d7-46c8-895c-3042192c1bbf}</MetaDataID>
        public MenuCanvasFoodItemPrice()
        {

        }
        /// <MetaDataID>{512a943d-94c7-4663-a4d0-47fcbd3a34d7}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <MetaDataID>{50d132e8-e871-4bda-a4a4-f023031967ca}</MetaDataID>
        public MenuCanvasFoodItemPrice(IRestaurantMenu menu, MenuCanvas.IMenuCanvasFoodItemPrice menuCanvasFoodItemPrice, MenuCanvasFoodItem menuCanvasFoodItem)
        {
            _FoodItem = menuCanvasFoodItem;
            Price = menuCanvasFoodItemPrice.Price;
            Description = menuCanvasFoodItemPrice.Description;
            XPos = menuCanvasFoodItemPrice.XPos;
            YPos = menuCanvasFoodItemPrice.YPos;
            Height = menuCanvasFoodItemPrice.Height;
            Visisble = menuCanvasFoodItemPrice.Visisble;
            
            Font = menuCanvasFoodItemPrice.Font;
            FontID = (menu as RestaurantMenu).GetFontID(menuCanvasFoodItemPrice.Font);
            BaseLine = menuCanvasFoodItemPrice.BaseLine;// Font.GetTextBaseLine(menuCanvasFoodItemPrice.Description);
            Type = GetType().Name;

            MenuItemPriceUri = (menuCanvasFoodItemPrice as MenuCanvas.MenuCanvasFoodItemPrice).MenuItemPriceUri;
        }
        public string MenuItemPriceUri { get; set; }

        /// <MetaDataID>{a595d979-d256-4e0f-9ec4-7086a01dd16e}</MetaDataID>
        public string Type { get; set; }

        /// <MetaDataID>{cb747ad9-49df-45cb-9f0e-8eb12d67f6ea}</MetaDataID>
        public string Description { get; set; }

        /// <MetaDataID>{99f9d873-590a-4c99-a329-f9b979ca59ab}</MetaDataID>
        [JsonIgnore]
        public FontData Font { get; set; }
        /// <MetaDataID>{84cf2cfb-eb50-4c66-a72c-4aff0ae87118}</MetaDataID>
        public int FontID { get; set; }

        /// <exclude>Excluded</exclude> 
        IMenuCanvasFoodItem _FoodItem;
        /// <MetaDataID>{a771258e-9041-40e9-91f7-831bb70bc588}</MetaDataID>
        public IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return _FoodItem;
            }
        }

        /// <MetaDataID>{b512afb9-d749-4e2e-93e5-29aff5bf4a76}</MetaDataID>
        public double Height { get; set; }

#if MenuPresentationModel
        /// <MetaDataID>{d2fa6a97-9ded-4d82-bd26-baec82070556}</MetaDataID>
        public ItemSelectorOption ItemSelection
        {
            get
            {
                return null;
            }
        }
#endif

        /// <MetaDataID>{56769a51-d717-4352-8ac4-7f8778bbdd4f}</MetaDataID>
        [JsonIgnore]
        public IMenuPageCanvas Page
        {
            get
            {
                return _FoodItem.Page;
            }
            set
            {
             
            }
        }

        /// <MetaDataID>{70c78e7a-2bad-4d35-a016-13acdbb8cfa1}</MetaDataID>
        public decimal Price { get; set; }

        /// <MetaDataID>{002f5267-4c5c-434a-a271-c9f4c5cd5f3f}</MetaDataID>
        public bool Visisble { get; set; }

        /// <MetaDataID>{6f41eb60-ed76-48b0-bcef-051ede457807}</MetaDataID>
        public double Width { get; set; }

        /// <MetaDataID>{c1d26fb1-18ec-49f0-82e1-68b93d48b31d}</MetaDataID>
        public double XPos { get; set; }

        /// <MetaDataID>{c83bcd28-ea2e-4837-8360-598cc2880f48}</MetaDataID>
        public double YPos { get; set; }
        /// <MetaDataID>{589cebef-b3c2-4e23-baab-6dbb04ead1f1}</MetaDataID>
        public double BaseLine { get; set; }


        public event ObjectChangeStateHandle ObjectChangeState;



#if MenuPresentationModel
        /// <MetaDataID>{bd24682f-3fad-45d8-8c83-98a6496b2941}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{ad7d5ef9-fbd8-4a67-b476-b423ea281f79}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{356a313f-3887-4bbf-8189-2ef51e759fe5}</MetaDataID>
        public void ResetSize(IPriceStyle style)
        {
            throw new NotImplementedException();
        }
#endif

    }
}
