using System;
using System.Globalization;
using System.Windows;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{9b983f75-3393-47c5-b557-6c43efb7d122}</MetaDataID>
    [BackwardCompatibilityID("{9b983f75-3393-47c5-b557-6c43efb7d122}")]
    [Persistent()]
    public class MenuCanvasFoodItemPrice : MarshalByRefObject, IMenuCanvasFoodItemPrice
    {

        /// <MetaDataID>{b6c7c3f1-125d-473a-b576-9779b58ede5b}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{6491e0ff-56de-46f2-86b3-59de8c11a8cb}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <MetaDataID>{88657a77-8ca2-47a0-9cdb-4593d9a0cba5}</MetaDataID>
        public bool Visisble { get; set; }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7ac351dc-ee90-4526-a2e3-19e00adb65a6}</MetaDataID>
        decimal _Price;
        /// <MetaDataID>{c039a008-9ddc-4582-b2fd-3f51b73f1418}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Price))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+5")]
        public decimal Price
        {
            get
            {
                return _Price;
            }

            set
            {

                if (_Price != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Price = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }





        /// <exclude>Excluded</exclude>
        MenuStyles.FontData? _Font;

        /// <MetaDataID>{30b65b7b-7fae-49bf-bb1f-d372f026ae3d}</MetaDataID>
        public MenuStyles.FontData Font
        {
            get
            {
                if (!_Font.HasValue)
                {
                    if (Style.Layout == MenuStyles.PriceLayout.WithDescription || Style.Layout == MenuStyles.PriceLayout.FollowDescription)
                        return (Page.Style.Styles["menu-item"] as MenuStyles.IMenuItemStyle).DescriptionFont;
                    else
                        return Style.Font;
                }
                return _Font.Value;
            }
            set
            {
                _Font = value;
            }
        }

        /// <MetaDataID>{420ec760-a8b8-4f1a-a820-259684106831}</MetaDataID>
       internal MenuStyles.IPriceStyle Style
        {
            get
            {
                return Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
            }
        }

        /// <MetaDataID>{bb14e260-ddcf-4a57-8a89-5d09f5785f7b}</MetaDataID>
        static NumberFormatInfo nfi;
        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{33ffcf51-318f-4256-bbf0-cacd845e4a65}</MetaDataID>
        public string Description
        {
            get
            {
                string description = "";
                if (Style.DisplayCurrencySymbol)
                    description= string.Format("{0:c}", Price);
                else
                {
                    if (nfi == null)
                    {
                        nfi = CultureInfo.CurrentCulture.NumberFormat;
                        nfi = (NumberFormatInfo)nfi.Clone();
                        nfi.CurrencySymbol = "";
                    }
                    description= string.Format(nfi, "{0:c}", Price);
                }
                if(Style.Layout==MenuStyles.PriceLayout.FollowDescription||Style.Layout==MenuStyles.PriceLayout.WithDescription)
                {
                    if ((Page.Style.Styles["menu-item"] as MenuStyles.IMenuItemStyle).Alignment == MenuStyles.Alignment.Center)
                        description += (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescSeparator;
                }
                return description;
            }

            set
            {

                //if (_Description != value)
                //{
                //    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                //    {
                //        _Description = value;
                //        stateTransition.Consistent = true;
                //    }
                //}

            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuCanvasFoodItem> _FoodItem = new OOAdvantech.Member<IMenuCanvasFoodItem>();
        
        
        /// <MetaDataID>{feb28a9b-9c2d-4184-a1a5-2b4b56e65c61}</MetaDataID>
        [PersistentMember(nameof(_FoodItem))]
        [BackwardCompatibilityID("+6")]
        public MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItem FoodItem
        {
            get
            {
                return _FoodItem.Value;
            }
        }

        /// <MetaDataID>{60b15155-356a-4d7e-9f00-2f68d399f6f3}</MetaDataID>
        public IMenuPageCanvas Page
        {
            get
            {
                return FoodItem.Page;
            }
        }
        /// <exclude>Excluded</exclude>
        double? _Height;
        /// <MetaDataID>{7951398c-0a66-4af8-87ea-06220ab4e95d}</MetaDataID>
        public double Height
        {
            get
            {
                if (!_Height.HasValue)
                    return Font.MeasureText(Description).Height;
                return _Height.Value;
            }

            set
            {

                if (_Height != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Height = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        double? _Width;
        /// <MetaDataID>{0c3b87d0-0792-4ecc-b93d-252072a28b91}</MetaDataID>
        public double Width
        {
            get
            {
                if (!_Width.HasValue)
                    return Font.MeasureText(Description).Width;
                return _Width.Value;
            }
            set
            {
                if (_Width != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Width = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
        }
        /// <exclude>Excluded</exclude>
        double _XPos;
        /// <MetaDataID>{ea07ebec-6b81-4138-8eab-cf0f4d252f4f}</MetaDataID>
        public double XPos
        {
            get
            {
                return _XPos;
            }

            set
            {

                if (_XPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _XPos = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        double _YPos;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{a470e9f9-8236-44a8-9ab3-34afc6f9df68}</MetaDataID>
        public double YPos
        {
            get
            {
                return _YPos;
            }

            set
            {

                if (_YPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _YPos = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    }
}