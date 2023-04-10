using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
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
    /// <MetaDataID>{b43290a1-ca00-47aa-962a-ee6218065bce}</MetaDataID>
    public class MenuCanvasPriceLeader : IMenuCanvasItemEx, MenuCanvas.IMenuCanvasPriceLeader
    {


        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{82c0f3a6-a27c-438e-829c-28f3fe62bdb7}</MetaDataID>
        [JsonIgnore]
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }

        /// <MetaDataID>{3359313f-aa1c-42cf-be5a-cdfaf4a62336}</MetaDataID>
        public void ResetSize()
        {
        }
        /// <MetaDataID>{6a7101ee-db4b-4947-86a9-4107bbc5e472}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <MetaDataID>{16c1cddf-fe43-4ce2-bf8e-e6b83b6e7ba1}</MetaDataID>
        public MenuCanvasPriceLeader()
        {

        }
        /// <MetaDataID>{82319753-2ea6-4f1d-b845-78345bf6a241}</MetaDataID>
        JsonMenuPresentation.MenuCanvasFoodItem MenuCanvasFoodItem;
        /// <MetaDataID>{2885e5b7-d16d-4dad-b45c-07e096abd512}</MetaDataID>
        public MenuCanvasPriceLeader(IRestaurantMenu menu, MenuCanvas.IMenuCanvasPriceLeader priceLeader, JsonMenuPresentation.MenuCanvasFoodItem menuCanvasFoodItem)
        {
            MenuCanvasFoodItem = menuCanvasFoodItem;
            Description = priceLeader.Description;
            Height = priceLeader.Height;
            Width = priceLeader.Width;
            XPos = priceLeader.XPos;
            YPos = priceLeader.YPos;
            Font = priceLeader.Font;
            FontID = (menu as RestaurantMenu).GetFontID(priceLeader.Font);
            Visisble = priceLeader.Visisble;
            BaseLine = priceLeader.BaseLine;// Font.GetTextBaseLine(priceLeader.Description);
            Type = GetType().Name;
        }
        /// <MetaDataID>{55b2c7cd-b74e-4072-87f5-e03c75edb785}</MetaDataID>
        public string Type { get; set; }


        /// <MetaDataID>{ad0c31c6-81cc-4a95-9775-c3cbf370a103}</MetaDataID>
        public IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return MenuCanvasFoodItem;
            }
        }


        /// <MetaDataID>{0974d54c-fadd-438b-87d8-c3a1f20cf8d8}</MetaDataID>
        public string Description { get; set; }

        /// <MetaDataID>{5c3a4d5b-0911-420c-8602-3ac045257765}</MetaDataID>
        [JsonIgnore]
        public FontData Font { get; set; }
        /// <MetaDataID>{f117f167-976c-4ba0-bd68-00c401850527}</MetaDataID>
        public int FontID { get; set; }
        /// <MetaDataID>{0cf2aa54-7edb-44b3-a74f-97e88c7d9f40}</MetaDataID>
        public double Height { get; set; }


        /// <MetaDataID>{5f041df4-5257-49d5-bf07-aa8255512ddd}</MetaDataID>
        [JsonIgnore]
        public IMenuPageCanvas Page
        {
            get
            {
                return MenuCanvasFoodItem.Page;
            }
            set
            {
                
            }
        }

        /// <MetaDataID>{5e070059-d7ca-4cd7-8df0-cefde353a5f0}</MetaDataID>
        public bool Visisble { get; set; }

        /// <MetaDataID>{debe9590-5cae-469b-969f-1ae3914355bd}</MetaDataID>
        public double Width { get; set; }

        /// <MetaDataID>{54896210-0e7b-4356-8c16-57f906b782d7}</MetaDataID>
        public double XPos { get; set; }

        /// <MetaDataID>{36259273-4d88-410d-9527-63f438e92727}</MetaDataID>
        public double YPos { get; set; }
        /// <MetaDataID>{67b4dd63-5c6e-499c-bae9-ad6de36fa1ff}</MetaDataID>
        public double BaseLine { get; set; }


#if MenuPresentationModel
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{8108ba0c-ac91-4d39-90eb-9ad878086c78}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{aac13898-259b-444f-a471-069b980278fa}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
