using System;
using System.Globalization;
using System.Windows;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{9b983f75-3393-47c5-b557-6c43efb7d122}</MetaDataID>
    [BackwardCompatibilityID("{9b983f75-3393-47c5-b557-6c43efb7d122}")]
    [Persistent()]
    public class MenuCanvasFoodItemPrice : MarshalByRefObject, IMenuCanvasFoodItemPrice
    {
        /// <exclude>Excluded</exclude> 
        string _MenuItemPriceUri;
        /// <MetaDataID>{284c67b3-7d74-427d-a6fb-2cb43a1e5922}</MetaDataID>
        [PersistentMember(nameof(_MenuItemPriceUri))]
        [BackwardCompatibilityID("+12")]
        public string MenuItemPriceUri
        {
            get => _MenuItemPriceUri;
            set
            {
                if (_MenuItemPriceUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuItemPriceUri = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        MenuModel.MenuItemPrice _MenuItemPrice;
        /// <MetaDataID>{315fadd0-bd32-4b46-bdd6-42d3c0c638c7}</MetaDataID>
        public MenuModel.MenuItemPrice MenuItemPrice
        {
            get
            {
                return _MenuItemPrice;
            }
            internal set
            {
                _MenuItemPrice = value;

                MenuItemPriceUri= ObjectStorage.GetStorageOfObject(_MenuItemPrice).GetPersistentObjectUri(_MenuItemPrice);
            }
        }

        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{cdf6bff3-ebd8-48d7-aa53-a7ed235bf571}</MetaDataID>
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }

        /// <MetaDataID>{99684d96-f829-43cb-99f4-a437891f7721}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <MetaDataID>{3d66b12d-bb40-4478-a5a8-e9c2da9ad09d}</MetaDataID>
        public MenuCanvasFoodItemPrice(MenuModel.MenuItemPrice menuItemPrice)
        {
            _MenuItemPrice = menuItemPrice;
        }
        /// <MetaDataID>{3fab66e1-d77d-458d-9c2b-cc8f04629705}</MetaDataID>
        protected MenuCanvasFoodItemPrice()
        {

        }


        /// <MetaDataID>{da2603d9-ff77-4ffd-a4a5-b7a4ca140758}</MetaDataID>
        public MenuModel.ItemSelectorOption ItemSelection
        {
            get
            {
                if (_MenuItemPrice != null)
                    return _MenuItemPrice.ItemSelector;
                else
                    return null;
            }
        }

        /// <MetaDataID>{b6c7c3f1-125d-473a-b576-9779b58ede5b}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos.Value = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{6491e0ff-56de-46f2-86b3-59de8c11a8cb}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <exclude>Excluded</exclude>
        bool _Visisble;
        /// <MetaDataID>{88657a77-8ca2-47a0-9cdb-4593d9a0cba5}</MetaDataID>
        [PersistentMember(nameof(_Visisble))]
        [BackwardCompatibilityID("+7")]
        public bool Visisble
        {
            get
            {
                return _Visisble;
            }
            set
            {
                _Visisble = value;
            }
        }
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
        FontData? _Font;

        /// <MetaDataID>{30b65b7b-7fae-49bf-bb1f-d372f026ae3d}</MetaDataID>
        public FontData Font
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
                if(Page==null)
                {

                }
                return Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
            }
        }

        /// <exclude>Excluded</exclude>
        static NumberFormatInfo nfi;
        /// <MetaDataID>{9042c660-ed67-485b-9655-78076177c5f9}</MetaDataID>
        static CultureInfo nfiCulture;
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Description=new MultilingualMember<string>();
        /// <MetaDataID>{33ffcf51-318f-4256-bbf0-cacd845e4a65}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+4")]
        public string Description
        {
            get
            {
                string description = "";
                description = GetDescription();
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

        /// <MetaDataID>{13cd5aa3-a06c-479d-97be-a890b5c5c2b2}</MetaDataID>
        private string GetDescription(MenuStyles.IPriceStyle style = null)
        {
            if (style == null)
                style = Style;

            string description;
            if (style.DisplayCurrencySymbol)
                description = string.Format(CultureContext.CurrentCultureInfo, "{0:c}", Price);
            else
            {
                if (nfi == null|| nfiCulture!= CultureContext.CurrentCultureInfo)
                {
                    nfi = CultureContext.CurrentCultureInfo.NumberFormat;
                    nfi = (NumberFormatInfo)nfi.Clone();
                    nfi.CurrencySymbol = "";
                    nfiCulture = CultureContext.CurrentCultureInfo;
                }
                description = string.Format(nfi, "{0:c}", Price);
            }
            if (style.Layout == MenuStyles.PriceLayout.FollowDescription || style.Layout == MenuStyles.PriceLayout.WithDescription)
            {
                if ((style.StyleSheet.Styles["menu-item"] as MenuStyles.IMenuItemStyle).Alignment == MenuStyles.Alignment.Center)
                    description += (style.StyleSheet.Styles["layout"] as MenuStyles.ILayoutStyle).DescSeparator;
            }

            return description;
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
        public MenuPresentationModel.MenuCanvas.IMenuPageCanvas Page
        {
            get
            {

                if(FoodItem==null|| FoodItem.Page==null)
                {
                    string name = null;
                    if (FoodItem != null)
                        name = FoodItem.Description;

                }

                return FoodItem.Page;
            }
        }

        /// <MetaDataID>{112ccb3b-5610-4500-984d-dcaf3764f560}</MetaDataID>
        public void ResetSize()
        {
            ResetSize(Style);
        }
        /// <MetaDataID>{252f47d7-8e72-48cc-8797-220a3651753c}</MetaDataID>
        public void ResetSize(MenuStyles.IPriceStyle style)
        {
            FontData font;
            if (style.Layout == MenuStyles.PriceLayout.WithDescription || style.Layout == MenuStyles.PriceLayout.FollowDescription)
                font=(style.StyleSheet.Styles["menu-item"] as MenuStyles.IMenuItemStyle).DescriptionFont;
            else
                font= style.Font;

            var size = font.MeasureText(GetDescription(style));
            Height = size.Height;
            Width = size.Width;
            BaseLine = font.GetTextBaseLine(GetDescription(style));
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _BaseLine=new MultilingualMember<double>();
        /// <MetaDataID>{42ceadb4-2ea7-4b33-9559-ab79fc86e7d9}</MetaDataID>
        [PersistentMember(nameof(_BaseLine))]
        [BackwardCompatibilityID("+11")]
        public double BaseLine
        {
            get
            {
                return _BaseLine;
            }
            set
            {
                if (_BaseLine != value)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BaseLine.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _Height=new MultilingualMember<double>();
        /// <MetaDataID>{7951398c-0a66-4af8-87ea-06220ab4e95d}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+8")]
        public double Height
        {
            get
            {
                //if (!_Height.HasValue)
                //    return Font.MeasureText(Description).Height;
                return _Height;//.Value;
            }
            set
            {
                if (_Height != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Height.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _Width=new MultilingualMember<double>();
        /// <MetaDataID>{0c3b87d0-0792-4ecc-b93d-252072a28b91}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+9")]
        public double Width
        {
            get
            {
                //if (!_Width.HasValue)
                //    return Font.MeasureText(Description).Width;
                return _Width;//.Value;
            }
            set
            {
                if (_Width != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Width.Value = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _XPos=new MultilingualMember<double>();
        /// <MetaDataID>{ea07ebec-6b81-4138-8eab-cf0f4d252f4f}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+2")]
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
                        _XPos.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _YPos=new MultilingualMember<double>();

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{a470e9f9-8236-44a8-9ab3-34afc6f9df68}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+3")]
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
                        _YPos.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    }
}