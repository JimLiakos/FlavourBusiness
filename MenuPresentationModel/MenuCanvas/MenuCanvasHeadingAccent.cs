using System;
using System.Windows;
using System.Linq;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.Transactions;
using OOAdvantech.MetaDataRepository;
using UIBaseEx;
using System.Collections.Generic;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{f468a1b0-9b22-4196-bfeb-251e14611369}</MetaDataID>
    [BackwardCompatibilityID("{f468a1b0-9b22-4196-bfeb-251e14611369}")]
    [Persistent()]
    public class MenuCanvasAccent : MarshalByRefObject, IMenuCanvasAccent
    {

        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{f0713ffc-0d6f-47f9-ac4a-423bdaa03a6c}</MetaDataID>
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }


        /// <MetaDataID>{9fc194f5-973f-4770-ad4f-7122b2e6672b}</MetaDataID>
        public void ResetSize()
        {
            HeadingTextHeight = Heading.Font.MeasureText(Heading.Description).Height;
            HeadingTextMedline = Heading.Font.GetTextMedline(Heading.Description);
            BaseLine = Font.GetTextBaseLine(Description);
        }
        /// <MetaDataID>{7d77b87c-04bb-4787-84a7-6accc5f7b480}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _BaseLine = new MultilingualMember<double>();
        /// <MetaDataID>{e7c7602c-4df5-498a-8a58-fe8790af7e63}</MetaDataID>
        [PersistentMember(nameof(_BaseLine))]
        [BackwardCompatibilityID("+8")]
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


        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{b9ba31cb-78b9-41a7-93d5-0881437e8e7f}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{ff832c90-ae41-496d-b561-b0aa9f8694b0}</MetaDataID>
        public MenuCanvasAccent(IMenuCanvasHeading heading, IAccent accent)
        {
            _Heading = heading;
            _Accent = accent;

            var v = _Accent.AccentImages;
            if (_Accent != null)
                _Accent.ObjectChangeState += Accent_ObjectChangeState;
        }

        /// <MetaDataID>{34608788-931e-4cac-8a23-0deb94ffdc55}</MetaDataID>
        public MenuCanvasAccent(IAccent accent)
        {
            _Accent = accent;
            var v = _Accent.AccentImages;
            if (_Accent != null)
                _Accent.ObjectChangeState += Accent_ObjectChangeState;
        }

        protected MenuCanvasAccent()
        {

        }


        /// <MetaDataID>{e7cecd76-4e6e-41c6-83ac-9d9098735eed}</MetaDataID>
        private void Accent_ObjectChangeState(object _object, string member)
        {

            if (member == nameof(IAccent.AccentColor))
                ObjectChangeState?.Invoke(this, nameof(AccentColor));
        }


        /// <MetaDataID>{baa3bf88-7a4b-4b31-aac1-9f72c57d936f}</MetaDataID>
        public string AccentColor
        {
            get
            {
                if (_Accent != null)
                    return _Accent.AccentColor;
                return null;
            }
        }
        /// <exclude>Excluded</exclude>
        IAccent _Accent;
        /// <MetaDataID>{95c0b33a-9cac-4256-9f05-86e02c5ecab0}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        public IAccent Accent
        {
            get
            {
                if (_Accent == null)
                {
                    if (HighlightedItems.Count > 0)
                    {
                        if (HighlightedItems[0] is FoodItemsHeading)
                        {
                            if ((HighlightedItems[0] as FoodItemsHeading).CustomHeadingAccent != null)
                                _Accent = (HighlightedItems[0] as FoodItemsHeading).CustomHeadingAccent;
                            else
                            if ((HighlightedItems[0] as FoodItemsHeading).Style != null)
                                _Accent = (HighlightedItems[0] as FoodItemsHeading).Style.Accent;
                        }
                        if (HighlightedItems[0] is MenuCanvasFoodItem)
                        {
                            _Accent = (HighlightedItems[0] as MenuCanvasFoodItem).AccentType;
                        }
                    }
                }
                return _Accent;
            }
            set
            {
                if (_Accent != value)
                {
                    if (_Accent != null)
                        _Accent.ObjectChangeState -= Accent_ObjectChangeState;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Accent = value;
                        stateTransition.Consistent = true;
                    }
                    if (_Accent != null)
                        _Accent.ObjectChangeState += Accent_ObjectChangeState;
                }
            }
        }



        /// <MetaDataID>{bfed063c-d596-41c4-b109-283616377a64}</MetaDataID>
        public string Description
        {
            get
            {
                return "";
            }

            set
            {

            }
        }

        /// <exclude>Excluded</exclude>
        IMenuCanvasHeading _Heading;
        /// <MetaDataID>{85b61f6c-133b-4285-b81e-3dc6158c28c9}</MetaDataID>
        public IMenuCanvasHeading Heading
        {
            get
            {
                return _Heading;
            }

            set
            {

                if (_Heading != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Heading = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <MetaDataID>{e2c50ac9-0c0c-43f4-a63c-4a0d18a1c16a}</MetaDataID>
        public IMenuPageCanvas Page
        {
            get
            {
                return HighlightedItems[0].Page;
            }

            set
            {
                throw new NotSupportedException();

            }
        }


        /// <exclude>Excluded</exclude>
        double _Width;
        /// <MetaDataID>{80fafdb6-0ef7-45d7-a5ac-80719030e9b6}</MetaDataID>
        public double Width
        {
            get
            {
                return _Width;
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
        OOAdvantech.MultilingualMember<double> _XPos = new MultilingualMember<double>();
        /// <MetaDataID>{da01c6f0-fe03-42b3-a72c-07597b9d3106}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+6")]
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
        OOAdvantech.MultilingualMember<double> _YPos = new MultilingualMember<double>();

        /// <MetaDataID>{b802bc50-5a7f-4d63-8f5a-2b05aff45b6b}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+7")]
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

        internal static double ToPixels(MenuPresentationModel.MenuCanvas.IMenuCanvasHeading menuCanvasHeading, double value)
        {
            if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.em)
            {
                return value * menuCanvasHeading.Font.FontSize;// (textSize.Width * 1.5 - textSize.Width) / 2;
            }
            else if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.px)
            {
                return value;
            }
            else if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.mm)
            {
                return UIBaseEx.SizeUtil.mmToPixel(value);
            }
            else if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.cm)
            {
                return UIBaseEx.SizeUtil.cmToPixel(value);
            }
            else if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.inch)
            {
                return UIBaseEx.SizeUtil.InchToPixel(value);
            }
            else if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.vw)
            {
                return value;
            }
            else if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.vh)
            {
                return value;
            }
            else if (menuCanvasHeading.Accent.Accent.MarginUnit == Unit.vwvh)
            {
                return value;
            }
            return value;
        }



        /// <MetaDataID>{6587780d-638a-4ab1-9c7b-479b2e520e6e}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        public bool FullRowImage
        {
            get
            {
                if (_Accent != null)
                    return _Accent.FullRowImage;
                else
                    return false;
            }

            set
            {
                if (_Accent != null)
                    _Accent.FullRowImage = value;

            }
        }

        /// <MetaDataID>{ced0f2c7-22fb-4df3-a63c-a5e2408da5eb}</MetaDataID>
        public double Height
        {
            get
            {
                if (_Accent != null)
                    return _Accent.Height;
                else
                    return 0;
            }

            set
            {

            }
        }

        /// <MetaDataID>{bdc1e806-f7f8-4e32-8ca9-143bc70acf9e}</MetaDataID>
        public FontData Font
        {
            get
            {
                return default(FontData);
            }
            set
            {

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _HeadingTextMedline = new MultilingualMember<double>();
        /// <MetaDataID>{2618b916-ff95-44e3-8632-0324f6217366}</MetaDataID>
        [PersistentMember(nameof(_HeadingTextMedline))]
        [BackwardCompatibilityID("+4")]
        public double HeadingTextMedline
        {
            set
            {

                if (_HeadingTextMedline != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HeadingTextMedline.Value = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
            get
            {

                return _HeadingTextMedline;
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _HeadingTextHeight = new MultilingualMember<double>();

        /// <MetaDataID>{30f9c6ea-1f8f-4cce-a180-065376bb19e8}</MetaDataID>
        [PersistentMember(nameof(_HeadingTextHeight))]
        [BackwardCompatibilityID("+5")]
        public double HeadingTextHeight
        {
            set
            {

                if (_HeadingTextHeight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HeadingTextHeight.Value = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
            get
            {

                return _HeadingTextHeight;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IHighlightedMenuCanvasItem> _HighlightedItems = new OOAdvantech.Collections.Generic.Set<IHighlightedMenuCanvasItem>();


        /// <MetaDataID>{2897b619-8a7b-45cb-bcd5-6491655fe196}</MetaDataID>
        [PersistentMember(nameof(_HighlightedItems))]
        public List<IHighlightedMenuCanvasItem> HighlightedItems
        {
            get
            {
                return _HighlightedItems.ToList();
            }
        }



        /// <MetaDataID>{79c8928f-430f-4c40-b4a9-c549186e4191}</MetaDataID>
        internal double MarginTop;

        /// <MetaDataID>{1d2debec-c8e0-412f-a79a-f69b6ca77f33}</MetaDataID>
        internal double MarginLeft;

        /// <MetaDataID>{79b02d9c-ddd0-473f-acca-f2a4102e55e6}</MetaDataID>
        internal double MarginRight;

        /// <MetaDataID>{088f19ee-3b0a-4b2f-bc00-96810369c307}</MetaDataID>
        internal double MarginBottom;


        /// <MetaDataID>{cf386d34-6e1f-42a3-9fef-d4f6d874a7a7}</MetaDataID>
        double FontSize
        {
            get
            {
                double fontSize = 0;
                foreach (var highlightedItem in HighlightedItems)
                {
                    if (highlightedItem.Font.FontSize > fontSize)
                        fontSize = highlightedItem.Font.FontSize;
                }
                return fontSize;
            }
        }


        public double HighlightedItemsLeftMost
        {
            get
            {
                double? leftmostPos = null; ;
                foreach (IMenuCanvasItem menuCanvasItem in HighlightedItems)
                {
                    if (menuCanvasItem is IMenuCanvasFoodItem)
                    {
                        foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                        {
                            if (leftmostPos == null || subText.XPos < leftmostPos)
                                leftmostPos = subText.XPos;
                        }

                        foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).Prices)
                        {
                            if (leftmostPos == null || subText.XPos < leftmostPos)
                                leftmostPos = subText.XPos;
                        }
                        if ((menuCanvasItem as IMenuCanvasFoodItem).PriceLeader != null && (leftmostPos == null || (menuCanvasItem as IMenuCanvasFoodItem).PriceLeader.XPos < leftmostPos))
                            leftmostPos = (menuCanvasItem as IMenuCanvasFoodItem).PriceLeader.XPos;

                    }
                    else
                    {
                        if (leftmostPos == null || menuCanvasItem.XPos < leftmostPos)
                            leftmostPos = menuCanvasItem.XPos;
                    }
                }
                if (leftmostPos == null)
                    return 0;
                return leftmostPos.Value;

            }

        }

        public double HighlightedItemsRightMost
        {
            get
            {
                double rightmostPos = 0;
                foreach (IMenuCanvasItem menuCanvasItem in HighlightedItems)
                {
                    if (menuCanvasItem is IMenuCanvasFoodItem)
                    {
                        foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                        {
                            if (subText.XPos + subText.Width > rightmostPos)
                                rightmostPos = subText.XPos + subText.Width;
                        }

                        foreach (var price in (menuCanvasItem as IMenuCanvasFoodItem).Prices)
                        {
                            if (price.XPos + price.Width > rightmostPos)
                                rightmostPos = price.XPos + price.Width;
                        }

                        if ((menuCanvasItem as IMenuCanvasFoodItem).PriceLeader != null && (menuCanvasItem as IMenuCanvasFoodItem).PriceLeader.XPos + (menuCanvasItem as IMenuCanvasFoodItem).PriceLeader.Width > rightmostPos)
                            rightmostPos = (menuCanvasItem as IMenuCanvasFoodItem).PriceLeader.XPos + (menuCanvasItem as IMenuCanvasFoodItem).PriceLeader.Width;
                    }
                    else
                    {
                        if (menuCanvasItem.XPos + menuCanvasItem.Width > rightmostPos)
                            rightmostPos = menuCanvasItem.XPos + menuCanvasItem.Width;
                    }
                }
                return rightmostPos;

            }

        }

        /// <MetaDataID>{409bd387-98cd-49b1-b97f-d624bae3bfdc}</MetaDataID>
        public double FullRowLeft
        {
            get
            {

                if (HighlightedItems[0].Column == null)
                {
                    if (Page != null && Page.Style != null)
                    {
                        var pageStyle = (Page.Style.Styles["page"] as MenuStyles.PageStyle);
                        return pageStyle.Margin.MarginLeft;
                    }
                    else
                        return 0;

                }
                return HighlightedItems[0].Column.XPos;
            }
        }

        /// <MetaDataID>{15961bd1-ad0b-4157-8db9-f053d4c39445}</MetaDataID>
        public double FullRowWidth
        {
            set
            {

            }
            get
            {
                if (HighlightedItems[0].Column != null)
                    return HighlightedItems[0].Column.Width;
                else
                {

                    if (HighlightedItems[0] is IMenuCanvasHeading)
                        return (HighlightedItems[0] as IMenuCanvasHeading).FullRowWidth;
                    else if (Page != null && Page.Style != null)
                    {
                        var pageStyle = (Page.Style.Styles["page"] as MenuStyles.PageStyle);

                        return pageStyle.PageWidth - pageStyle.Margin.MarginLeft - pageStyle.Margin.MarginRight;
                    }
                    else
                        return 0;
                }

            }
        }

        /// <MetaDataID>{404cb746-87d2-4a3a-9802-fefa5f8626f3}</MetaDataID>
        public Rect GetAccentImageRect(int accentImageIndex)
        {
            double left = 0;
            double top = 0;
            double width = 0;
            double height = 0;
            if (HighlightedItems == null || HighlightedItems.Count == 0)
                return default(Rect);
            var highlightedItemsHeight = HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().YPos + HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().Height - HighlightedItems.OrderBy(x => x.YPos).FirstOrDefault().YPos;
            var highlightedItemswidth = HighlightedItems.OrderBy(x => x.Width).FirstOrDefault().Width;
            if (_Accent != null)
            {
                double fontTextMedline = HeadingTextMedline;
                double fontMedline = HeadingTextHeight / 2;

                if (Accent.MarginUnit == Unit.em)
                {
                    MarginLeft = _Accent.MarginLeft * FontSize;// (textSize.Width * 1.5 - textSize.Width) / 2;
                    MarginRight = Accent.MarginRight * FontSize;// (textSize.Width * 1.5 - textSize.Width) / 2;
                    MarginTop = Accent.MarginTop * FontSize; //(textSize.Height * 1.2 - textSize.Height) / 2;
                    MarginBottom = Accent.MarginBottom * FontSize;
                    MarginTop += fontMedline - fontTextMedline;
                    MarginBottom -= (fontMedline - fontTextMedline);
                }
                else if (Accent.MarginUnit == Unit.px)
                {
                    MarginLeft = _Accent.MarginLeft;
                    MarginRight = Accent.MarginRight;
                    MarginTop = Accent.MarginTop;
                    MarginBottom = Accent.MarginBottom;
                }
                else if (Accent.MarginUnit == Unit.mm)
                {

                    MarginLeft = UIBaseEx.SizeUtil.mmToPixel(_Accent.MarginLeft);
                    MarginRight = UIBaseEx.SizeUtil.mmToPixel(_Accent.MarginRight);
                    MarginTop = UIBaseEx.SizeUtil.mmToPixel(_Accent.MarginTop);
                    MarginBottom = UIBaseEx.SizeUtil.mmToPixel(_Accent.MarginBottom);
                }
                else if (Accent.MarginUnit == Unit.cm)
                {
                    MarginLeft = UIBaseEx.SizeUtil.cmToPixel(_Accent.MarginLeft);
                    MarginRight = UIBaseEx.SizeUtil.cmToPixel(_Accent.MarginRight);
                    MarginTop = UIBaseEx.SizeUtil.cmToPixel(_Accent.MarginTop);
                    MarginBottom = UIBaseEx.SizeUtil.cmToPixel(_Accent.MarginBottom);
                }
                else if (Accent.MarginUnit == Unit.inch)
                {
                    MarginLeft = UIBaseEx.SizeUtil.InchToPixel(_Accent.MarginLeft);
                    MarginRight = UIBaseEx.SizeUtil.InchToPixel(_Accent.MarginRight);
                    MarginTop = UIBaseEx.SizeUtil.InchToPixel(_Accent.MarginTop);
                    MarginBottom = UIBaseEx.SizeUtil.InchToPixel(_Accent.MarginBottom);
                }
                else if (Accent.MarginUnit == Unit.vw)
                {
                    MarginLeft = _Accent.MarginLeft * (highlightedItemswidth / 100);
                    MarginRight = Accent.MarginRight * (highlightedItemswidth / 100);
                    MarginTop = Accent.MarginTop * (highlightedItemswidth / 100);
                    MarginBottom = Accent.MarginBottom * (highlightedItemswidth / 100);
                }
                else if (Accent.MarginUnit == Unit.vh)
                {
                    MarginLeft = _Accent.MarginLeft * (highlightedItemsHeight / 100);
                    MarginRight = Accent.MarginRight * (highlightedItemsHeight / 100);
                    MarginTop = Accent.MarginTop * (highlightedItemsHeight / 100);
                    MarginBottom = Accent.MarginBottom * (highlightedItemsHeight / 100);
                }
                else if (Accent.MarginUnit == Unit.vwvh)
                {
                    MarginLeft = _Accent.MarginLeft * (highlightedItemswidth / 100);
                    MarginRight = Accent.MarginRight * (highlightedItemswidth / 100);
                    MarginTop = Accent.MarginTop * (highlightedItemsHeight / 100);
                    MarginBottom = Accent.MarginBottom * (highlightedItemsHeight / 100);
                }


                if (_Accent.DoubleImage)
                {
                    if (accentImageIndex == 0)
                    {
                        left = HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().XPos - Accent.AccentImages[accentImageIndex].Width * FontSize;
                        left -= Accent.MarginLeft * FontSize;
                        width = Accent.AccentImages[accentImageIndex].Width * FontSize;
                        height = Accent.AccentImages[accentImageIndex].Height * FontSize;
                        top = HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().YPos + (HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().Height - Accent.AccentImages[accentImageIndex].Height * FontSize) / 2;
                    }
                    if (accentImageIndex == 1)
                    {
                        left = HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().XPos + HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().Width;// Accent.AccentImages[accentImageIndex].Width * FontSize;
                        left += Accent.MarginLeft * FontSize;
                        width = Accent.AccentImages[accentImageIndex].Width * FontSize;
                        height = Accent.AccentImages[accentImageIndex].Height * FontSize;
                        top = HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().YPos + (HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().Height - Accent.AccentImages[accentImageIndex].Height * FontSize) / 2;


                    }
                }
                else if (_Accent.OrgSize)
                {
                    left = HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().XPos;
                    width = HighlightedItems.OrderBy(x => x.Width).FirstOrDefault().Width;
                    double hcenter = left + (width / 2);
                    left = hcenter - (_Accent.Width / 2);
                    width = _Accent.Width;
                    if (_Accent.UnderlineImage)
                        top = HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().YPos + HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().Height + MarginBottom;
                    else if (_Accent.OverlineImage)
                        top = HighlightedItems.OrderBy(x => x.YPos).FirstOrDefault().YPos - MarginTop;
                    else
                        top = HighlightedItems.OrderBy(x => x.GetTextYpos()).FirstOrDefault().GetTextYpos() - MarginTop;
                    height = _Accent.Height;

                }
                else
                {



                    if (_Accent.FullRowImage)
                    {
                        if (_Accent.UnderlineImage || _Accent.OverlineImage)
                            left = FullRowLeft;
                        else
                        {
                            if (HighlightedItemsLeftMost - FullRowLeft < MarginLeft)
                                left = FullRowLeft - MarginLeft;
                            else
                                left = FullRowLeft;

                        }
                    }
                    else
                    {
                        left = HighlightedItems.OrderBy(x => x.XPos).FirstOrDefault().XPos - MarginLeft;

                        if (left < FullRowLeft)
                            if (_Accent.UnderlineImage || _Accent.OverlineImage)
                                left = FullRowLeft;
                        if (left < 0)
                            left = 0;
                    }
                    if(HighlightedItems.Count>1)
                    {
                        top = HighlightedItems.OrderBy(x => x.GetTextYpos()).FirstOrDefault().GetTextYpos() - MarginTop;
                    }
                    if (_Accent.UnderlineImage)
                        top = HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().YPos + HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().Height + MarginBottom;
                    else if (_Accent.OverlineImage)
                        top = HighlightedItems.OrderBy(x => x.YPos).FirstOrDefault().YPos - MarginTop;
                    else
                        top = HighlightedItems.OrderBy(x => x.GetTextYpos()).FirstOrDefault().GetTextYpos() - MarginTop;

                    if (HighlightedItems.Count > 1)
                    {
                        top = HighlightedItems.OrderBy(x => x.GetTextYpos()).FirstOrDefault().GetTextYpos() - MarginTop;
                    }

                    if (_Accent.FullRowImage)
                    {
                        if (_Accent.UnderlineImage || _Accent.OverlineImage)
                            width = FullRowWidth;
                        else
                        {
                            width = FullRowWidth;
                            if (HighlightedItemsLeftMost - FullRowLeft < MarginLeft)
                                width += MarginLeft;

                            if (FullRowLeft + FullRowWidth - HighlightedItemsRightMost < MarginRight)
                                width += MarginRight;
                        }

                    }
                    else
                    {
                        width = HighlightedItems.OrderBy(x => x.Width).FirstOrDefault().Width + MarginLeft + MarginRight;

                        if (Page != null)
                        {
                            if (width > Page.Width)
                                width = Page.Width;
                        }
                        else
                        {

                        }

                        if (width > FullRowWidth)
                            if (_Accent.UnderlineImage || _Accent.OverlineImage)
                                width = FullRowWidth;
                    }
                    double minWidth = _Accent.MinWidth;
                    if (FullRowWidth < minWidth)
                        minWidth = FullRowWidth;

                    if (width < minWidth)
                    {
                        left -= (minWidth - width) / 2;
                        width = minWidth;
                    }


                    if (_Accent.UnderlineImage || _Accent.OverlineImage)
                        height = _Accent.Height;
                    else
                        height = HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().YPos + HighlightedItems.OrderByDescending(x => x.YPos).FirstOrDefault().Height - HighlightedItems.OrderBy(x => x.YPos).FirstOrDefault().YPos + MarginTop + MarginBottom;
                }

            }

            return new Rect(left, top, width, height);


        }

        /// <MetaDataID>{f21f1f1c-11ab-4eda-bdcf-e37a3782fe62}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
    }



    /// <MetaDataID>{8f32355c-b504-4bde-96b5-0572ee58c541}</MetaDataID>
    public static class MenuCanvasItemExtender
    {

        /// <MetaDataID>{254189d3-307f-4f3a-a99f-18e90be2f82f}</MetaDataID>
        public static double GetTextYpos(this IMenuCanvasItem menuCanvasItem)
        {
            if (menuCanvasItem.BaseLine != 0)
            {
                double yposCorrection = menuCanvasItem.YPos + (menuCanvasItem.BaseLine - (menuCanvasItem.Height * 0.85));
                return yposCorrection;
            }
            if (menuCanvasItem is MenuCanvasFoodItem && (menuCanvasItem as MenuCanvasFoodItem).SubTexts.Count > 0)
            {
                return (menuCanvasItem as MenuCanvasFoodItem).SubTexts.OrderBy(x=>x.YPos).FirstOrDefault() .GetTextYpos();
            }


            return menuCanvasItem.YPos;

        }
    }
}