using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using System.Windows;
using OOAdvantech;
using MenuModel;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{282fdf5c-d88f-4ff9-be4a-7487ffb9c0c4}</MetaDataID>
    public enum PriceLine
    {
        FirstLine,
        LastLine,
        NoPrice
    }
    /// <MetaDataID>{bf6f6241-b72a-4995-b785-1d7f9a3c8794}</MetaDataID>
    [BackwardCompatibilityID("{bf6f6241-b72a-4995-b785-1d7f9a3c8794}")]
    [Persistent()]
    public class MenuCanvasFoodItem : MarshalByRefObject, IMenuCanvasFoodItem, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {

        public event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{070539fb-b66e-4af1-81ad-f45e8be6363b}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {

        }
        /// <exclude>Excluded</exclude>
        double _MaxHeight;
        /// <MetaDataID>{cbdad5ac-a5ac-4125-abf0-5f63b6aa1dec}</MetaDataID>
        [PersistentMember(nameof(_MaxHeight))]
        [BackwardCompatibilityID("+10")]
        public double MaxHeight
        {
            get
            {
                return _MaxHeight;
            }
            set
            {

                if (_MaxHeight != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MaxHeight = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        MenuStyles.FontData? _Font;
        /// <MetaDataID>{b517a0c2-ff4c-4a66-9c4e-207ead3ec97b}</MetaDataID>
        public MenuStyles.FontData Font
        {
            get
            {
                if (_Font.HasValue)
                    return _Font.Value;
                else if (Style != null)
                    return Style.Font;
                else
                    return default(MenuStyles.FontData);
            }
            set
            {
                _Font = value;
            }
        }

        /// <MetaDataID>{2e5c8521-1019-4f82-9e42-83583aeb9c97}</MetaDataID>
        public MenuStyles.IMenuItemStyle Style
        {
            get
            {
                MenuStyles.IStyleSheet styleSheet = null;
                if (Page != null)
                    styleSheet = Page.Style;
                if (RestaurantMenu.ConntextStyleSheet != null)
                    styleSheet = RestaurantMenu.ConntextStyleSheet;
                if (styleSheet != null)
                    return styleSheet.Styles["menu-item"] as MenuStyles.IMenuItemStyle;
                else
                    return null;
            }
        }


        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{978876dd-3f6e-4c75-90f4-05d9cd74106d}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {

                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }

                    ObjectChangeState?.Invoke(this, nameof(Description));

                }
            }
        }


        /// <exclude>Excluded</exclude>
        MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItemsGroup _HostingArea;


        /// <MetaDataID>{a3931068-a9d7-46e3-b329-2945dc96be4e}</MetaDataID>
        [ImplementationMember(nameof(_HostingArea))]
      
        public MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItemsGroup HostingArea
        {
            get
            {
                return _HostingArea;
            }
            set
            {

                if (_HostingArea != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HostingArea = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{f5e5899f-4ea8-4c02-a390-e25dff4d05eb}</MetaDataID>
        bool ExtraDescriptionInPriceLine;

        /// <MetaDataID>{d2e51f5e-4aef-45fb-a77d-8ca02dcfe2db}</MetaDataID>
        List<string> GetExtraDescriptionLinesText(double width, double linePriceGap, PriceLine firstLineGap)
        {
            string extraDescription = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescSeparator + ExtraDescription;

            ExtraDescriptionInPriceLine = false;
            if (!Style.NewLineForDescription && Style.Alignment != MenuStyles.Alignment.Center && !string.IsNullOrWhiteSpace(extraDescription))
            {
                var spaceWidth = Font.MeasureText(" ").Width;

                if (PriceLineIndex.HasValue && SubTexts.Count - 1 == PriceLineIndex.Value)
                {
                    ExtraDescriptionInPriceLine = true;
                    MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;

                    #region Insert description first line 
                    IMenuCanvasFoodItemText foodItemLineText = SubTexts.Last();

                    double priceLeaderWidth = 0;
                    if (priceStyle.PriceLeader != "dots" || priceStyle.Layout != MenuStyles.PriceLayout.Normal || Style.Alignment != MenuStyles.Alignment.Left)
                        priceLeaderWidth = GetPriceLeaderWidth(priceStyle);

                    double descriptionWidth = Width - (priceLeaderWidth + FoodItemPricesWitdh + foodItemLineText.Width + spaceWidth);
                    string mixedDescriptionPart = GetLinesText(extraDescription, descriptionWidth, Style.DescriptionFont).First();
                    var lineTextSize = Style.DescriptionFont.MeasureText(mixedDescriptionPart);
                    double lineTextXpos = 0;
                    if (priceStyle.Layout == MenuStyles.PriceLayout.Normal && Style.Alignment == MenuStyles.Alignment.Left)
                        lineTextXpos = foodItemLineText.XPos + foodItemLineText.Width + spaceWidth; //Price at the end of extra description
                    else
                        lineTextXpos = foodItemLineText.XPos + foodItemLineText.Width + priceLeaderWidth + FoodItemPricesWitdh + spaceWidth; //Price before extra description

                    //foodItemLineText.AlignOnBaseline()
                    //double nextMenuCanvasItemY = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(foodItemLineText.Description) - Style.DescriptionFont.GetTextBaseLine(mixedDescriptionPart);
                    MenuCanvasFoodItemText subText = new MenuCanvasFoodItemText(mixedDescriptionPart, lineTextXpos, 0, Style.DescriptionFont);
                    subText.AlignOnBaseline(foodItemLineText);

                    subText.Width = lineTextSize.Width;
                    subText.Height = lineTextSize.Height;
                    _SubTexts.Add(subText);

                    #endregion

                    if (mixedDescriptionPart.Length >= extraDescription.Length)
                    {
                        return new List<string>();
                    }
                    else
                    {
                        string newLineDescription = extraDescription.Substring(mixedDescriptionPart.Length);
                        double indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescLeftIndent;
                        indentWidth += (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescRightIndent;
                        if (Style.Alignment == MenuStyles.Alignment.Center)
                            return GetLinesText(newLineDescription, width, linePriceGap, firstLineGap, Style.DescriptionFont);
                        else
                            return GetLinesText(newLineDescription, width + indentWidth, linePriceGap, firstLineGap, Style.DescriptionFont);

                    }
                }
                else
                {
                    double descriptionWidth = Width - SubTexts.Last().Width - spaceWidth;
                    string mixedDescriptionPart = GetLinesText(extraDescription, descriptionWidth, Style.DescriptionFont).First();

                    var lineTextSize = Style.DescriptionFont.MeasureText(mixedDescriptionPart);
                    double lineTextXpos = SubTexts.Last().YPos + SubTexts.Last().Width + spaceWidth;
                    double nextMenuCanvasItemY = SubTexts.Last().XPos + SubTexts.Last().Height - lineTextSize.Height;
                    MenuCanvasFoodItemText subText = new MenuCanvasFoodItemText(mixedDescriptionPart, lineTextXpos, nextMenuCanvasItemY, Style.DescriptionFont);
                    subText.Width = lineTextSize.Width;
                    subText.Height = lineTextSize.Height;
                    _SubTexts.Add(subText);

                    if (mixedDescriptionPart.Length >= extraDescription.Length)
                    {
                        return new List<string>();
                    }
                    else
                    {
                        string newLineDescription = extraDescription.Substring(mixedDescriptionPart.Length);
                        double indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescLeftIndent;
                        indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescRightIndent;
                        if (Style.Alignment == MenuStyles.Alignment.Center)
                            return GetLinesText(newLineDescription, width , linePriceGap, firstLineGap, Style.DescriptionFont);
                        else
                            return GetLinesText(newLineDescription, width - indentWidth, linePriceGap, firstLineGap, Style.DescriptionFont);

                    }
                }
            }
            else
            {
                double indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescLeftIndent;
                indentWidth += (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescRightIndent;
                if (Style.Alignment == MenuStyles.Alignment.Center)
                    return GetLinesText(extraDescription, width , linePriceGap, firstLineGap, Style.DescriptionFont);
                else
                    return GetLinesText(extraDescription, width - indentWidth, linePriceGap, firstLineGap, Style.DescriptionFont);

            }
        }

        /// <MetaDataID>{d6f216ae-4c15-4124-9db4-4e75578f3445}</MetaDataID>
        List<string> GetLinesText(string text, double width, MenuStyles.FontData font)
        {
            return GetLinesText(text, width, 0, PriceLine.NoPrice, font);
        }
        /// <MetaDataID>{8fd240a8-5a2d-4e55-9ced-d066a8890db1}</MetaDataID>
        List<string> GetLinesText(string text, double width, double linePriceGap, PriceLine firstLinePriceGap, MenuStyles.FontData font)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            string sendex = text;// "This item uses Market for price. The price field can be any number or text, and can even be multiple prices (separate them with a semi-colon)";
            List<string> wrappedText = new List<string>();
            string sendecs = null;

            foreach (var word in sendex.Split(' '))
            {
                var size = font.MeasureText(sendecs + word + " ");

                double lineWitdh = width;
                if (firstLinePriceGap == PriceLine.FirstLine && wrappedText.Count == 0)
                    lineWitdh -= linePriceGap;
                if (size.Width < lineWitdh || sendecs == null)
                    sendecs += word + " ";
                else
                {
                    string lineText = sendecs.Substring(0, sendecs.Length - 1);
                    if (lineText.Length > 0)
                        wrappedText.Add(lineText);
                    sendecs = word + " ";
                }
            }
            if (sendecs.Length > 0)
                wrappedText.Add(sendecs);

            if (linePriceGap != 0 && firstLinePriceGap == PriceLine.LastLine && wrappedText.Count > 0)
            {
                string lastLine = wrappedText[wrappedText.Count - 1];
                var size = font.MeasureText(lastLine);
                double lineWitdh = width - linePriceGap;
                if (size.Width > lineWitdh)
                {
                    wrappedText.RemoveAt(wrappedText.Count - 1);
                    string newLastline = null;
                    sendecs = "";
                    foreach (var word in lastLine.Split(' ').Reverse())
                    {
                        size = font.MeasureText(word + " " + sendex);
                        if (size.Width < linePriceGap)
                        {
                            sendex = word + " " + sendex;
                        }
                        else
                        {
                            if (newLastline == null)
                            {
                                newLastline = word + " " + sendex;
                                sendex = "";
                            }
                            else
                            {
                                sendex = word + " " + sendex;
                            }
                        }
                    }
                    if(newLastline.Length>0)
                        wrappedText.Add(newLastline);
                    if (sendex.Length > 0)
                        wrappedText.Add(sendex);


                }
            }
            return wrappedText;
        }
        /// <MetaDataID>{a0714699-f0af-4018-b3a8-1fb14c81f44b}</MetaDataID>
        bool PriceInItemFirstLine
        {
            get
            {
                MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                return priceStyle.Layout == MenuStyles.PriceLayout.Normal && Style.Alignment == MenuStyles.Alignment.Left;
            }
        }
        /// <MetaDataID>{f1d49947-77ba-4654-8caa-358db9f5daef}</MetaDataID>
        bool PriceInItemLastLine
        {
            get
            {
                MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                return priceStyle.Layout == MenuStyles.PriceLayout.WithName || (priceStyle.Layout == MenuStyles.PriceLayout.Normal && Style.Alignment != MenuStyles.Alignment.Left);
            }
        }
        /// <MetaDataID>{4dad34a6-5c60-4d9e-bb2a-f8f5a5a4a3af}</MetaDataID>
        internal void RenderMenuCanvasItems()
        {
            _SubTexts.Clear();

            MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
            MenuStyles.ILayoutStyle layoutStyle = Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle;

            double nextMenuCanvasItemX = XPos;
            double nextMenuCanvasItemY = YPos;
            List<string> linesText = null;

            double lineSpacing = (layoutStyle.LineSpacing - 1) * 20;

            if (PriceInItemFirstLine)
            {
                linesText = GetLinesText(Description, Width, FoodItemPricesWitdh, PriceLine.FirstLine, Font);
                PriceLineIndex = 0;
            }
            else if (PriceInItemLastLine)
            {
                linesText = GetLinesText(Description, Width, FoodItemPricesWitdh, PriceLine.LastLine, Font);
                PriceLineIndex = linesText.Count - 1;
            }
            else
                linesText = GetLinesText(Description, Width, Font);
            int i = 0;
            foreach (string lineText in linesText)
            {
                Size lineTextSize = Style.Font.MeasureText(lineText);
                if (PriceLineIndex.HasValue && PriceLineIndex.Value == i)
                    lineTextSize = new Size(lineTextSize.Width + FoodItemPricesWitdh + GetPriceLeaderWidth(priceStyle), lineTextSize.Height);

                double lineTextXpos = GetLineTextXpos(lineTextSize);
                MenuCanvasFoodItemText subText = new MenuCanvasFoodItemText(lineText, lineTextXpos, nextMenuCanvasItemY, Font);
                lineTextSize = Style.Font.MeasureText(lineText);
                subText.Width = lineTextSize.Width;
                subText.Height = lineTextSize.Height;
                _SubTexts.Add(subText);
                nextMenuCanvasItemY += lineTextSize.Height + lineSpacing;
                i++;
            }

            if (priceStyle.Layout == MenuStyles.PriceLayout.FollowDescription && Style.Alignment == MenuStyles.Alignment.Left)
            {

                linesText = GetExtraDescriptionLinesText(Width, FoodItemPricesWitdh, PriceLine.FirstLine);
                if (linesText.Count == 0)
                    linesText.Add("");
                PriceLineIndex = _SubTexts.Count;
            }
            else if (priceStyle.Layout == MenuStyles.PriceLayout.WithDescription || (priceStyle.Layout == MenuStyles.PriceLayout.FollowDescription && Style.Alignment != MenuStyles.Alignment.Left))
            {
                linesText = GetExtraDescriptionLinesText(Width, 0, PriceLine.NoPrice);
                if (linesText.Count == 0)
                    linesText.Add("");
                PriceLineIndex = _SubTexts.Count + linesText.Count - 1;
            }
            else
                linesText = GetExtraDescriptionLinesText(Width, 0, PriceLine.NoPrice);

            foreach (string lineText in linesText)
            {
                
                Size lineTextSize = Style.DescriptionFont.MeasureText(lineText);
                if (string.IsNullOrEmpty(lineText))
                {
                    lineTextSize = Style.DescriptionFont.MeasureText(" ");
                    lineTextSize.Width = 0;
                }
                if (PriceLineIndex.HasValue && PriceLineIndex.Value == i)
                    lineTextSize = new Size(lineTextSize.Width + FoodItemPricesWitdh + GetPriceLeaderWidth(priceStyle), lineTextSize.Height);

                double lineTextXpos = 0;
                if (Style.Alignment == MenuStyles.Alignment.Center)
                    lineTextXpos =GetLineTextXpos(lineTextSize) ;
                else
                    lineTextXpos = GetLineTextXpos(lineTextSize) + (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescLeftIndent;


                lineTextSize = Style.DescriptionFont.MeasureText(lineText);
                if (string.IsNullOrEmpty(lineText))
                {
                    lineTextSize = Style.DescriptionFont.MeasureText(" ");
                    lineTextSize.Width = 0;
                }
                MenuCanvasFoodItemText subText = new MenuCanvasFoodItemText(lineText, lineTextXpos, nextMenuCanvasItemY, Style.DescriptionFont);
                subText.Width = lineTextSize.Width;
                subText.Height = lineTextSize.Height;

                _SubTexts.Add(subText);
                nextMenuCanvasItemY += lineTextSize.Height + lineSpacing; ;
                i++;
            }

            double indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasLeftIndent;
            //indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasRightIndent;

            string extras = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasSeparator + Extras;
            if (Style.Alignment == MenuStyles.Alignment.Center)
                extras = extras + (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasSeparator;

            
            foreach (var itemPrice in Prices)
                itemPrice.Visisble = false;

            BuildPrices();
            if (PriceLineIndex == SubTexts.Count - 1 && Prices.Count > 0)
                nextMenuCanvasItemY = Prices[0].YPos + Prices[0].Height + lineSpacing;
            if (Extras != null && Extras.Length > 0)
                linesText = GetLinesText(extras, Width - indentWidth, Style.ExtrasFont);
            else
                linesText = new List<string>();

            foreach (string lineText in linesText)
            {
                Size lineTextSize = Style.ExtrasFont.MeasureText(lineText);
                if (string.IsNullOrEmpty(lineText))
                {
                    lineTextSize = Style.ExtrasFont.MeasureText(" ");
                    lineTextSize.Width = 0;
                }
                if (PriceLineIndex.HasValue && PriceLineIndex.Value == i)
                    lineTextSize = new Size(lineTextSize.Width + FoodItemPricesWitdh + GetPriceLeaderWidth(priceStyle), lineTextSize.Height);

                double lineTextXpos = GetLineTextXpos(lineTextSize) + (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescLeftIndent;

                lineTextSize = Style.DescriptionFont.MeasureText(lineText);
                if (string.IsNullOrEmpty(lineText))
                {
                    lineTextSize = Style.DescriptionFont.MeasureText(" ");
                    lineTextSize.Width = 0;
                }
                MenuCanvasFoodItemText subText = new MenuCanvasFoodItemText(lineText, lineTextXpos, nextMenuCanvasItemY, Style.ExtrasFont);
                subText.Width = lineTextSize.Width;
                subText.Height = lineTextSize.Height;

                _SubTexts.Add(subText);
                nextMenuCanvasItemY += lineTextSize.Height + lineSpacing; 
                i++;
            }



            _Height = nextMenuCanvasItemY - YPos;

            // BuildPrices(pageStyle, columnWidth, priceStyle, menuCanvasItem);
        }

        /// <MetaDataID>{8e9fab2a-d311-4938-a8bb-c89a32171b48}</MetaDataID>
        private double GetLineTextXpos(Size lineTextSize)
        {
            double lineTextXpos = XPos;

            if (Style.Alignment == MenuStyles.Alignment.Center)
                lineTextXpos = XPos + (Width / 2) - (lineTextSize.Width / 2);
            if (Style.Alignment == MenuStyles.Alignment.Left)
                lineTextXpos = XPos;
            if (Style.Alignment == MenuStyles.Alignment.Right)
                lineTextXpos = XPos + Width - lineTextSize.Width;

            return lineTextXpos;
        }

        /// <MetaDataID>{375f7739-ce4d-4982-8bdf-e086db2f757a}</MetaDataID>
        void BuildPrices()
        {

            if (Prices.Count > 0)
            {
                MenuStyles.IPriceStyle priceStyle = (Prices[0] as MenuCanvasFoodItemPrice).Style;
                if (priceStyle.Layout == MenuStyles.PriceLayout.WithName || (priceStyle.Layout == MenuStyles.PriceLayout.FollowDescription && Style.Alignment != MenuStyles.Alignment.Left))
                {
                    SetPricesWithLineText(priceStyle);
                }
                else if (priceStyle.Layout == MenuStyles.PriceLayout.Normal && Style.Alignment == MenuStyles.Alignment.Left)
                {
                    SetPricesFollowLineText(priceStyle);
                }
                else if (priceStyle.Layout == MenuStyles.PriceLayout.FollowDescription && Style.Alignment == MenuStyles.Alignment.Left)
                {
                    SetPricesFollowLineText(priceStyle);
                }
                else if (priceStyle.Layout == MenuStyles.PriceLayout.WithDescription || (priceStyle.Layout == MenuStyles.PriceLayout.FollowDescription && Style.Alignment != MenuStyles.Alignment.Left))
                {
                    SetPricesWithLineText(priceStyle);
                }


            }
        }

        /// <MetaDataID>{5e898c84-ccc5-4ca4-9ed7-39f06e893220}</MetaDataID>
        private void SetPricesFollowLineText(MenuStyles.IPriceStyle priceStyle)
        {
            var spaceWidth = Font.MeasureText(" ").Width;

            if (priceStyle.PriceLeader != null)
            {
                if (priceStyle.PriceLeader == "dots")
                {
                    if (PriceLeader == null)
                        PriceLeader = new MenuCanvasPriceLeader();
                    PriceLeader.Description = ".";
                }
                else
                {
                    if (PriceLeader == null)
                        PriceLeader = new MenuCanvasPriceLeader();
                    PriceLeader.Description = priceStyle.PriceLeader;
                }

                if (ExtraDescriptionInPriceLine)
                    PriceLeader.XPos = SubTexts[PriceLineIndex.Value + 1].XPos+ SubTexts[PriceLineIndex.Value + 1].Width +spaceWidth ;
                else
                    PriceLeader.XPos = SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width ;
                Size priceLeaderSize = PriceLeader.Font.MeasureText(PriceLeader.Description);
                PriceLeader.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
            }
            else
                PriceLeader = null;

            foreach (var itemPrice in Prices)
            {
                 itemPrice.XPos = XPos + Width - itemPrice.Width;
                itemPrice.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
                itemPrice.Visisble = true;
                if (priceStyle.PriceLeader == "dots")
                    ExpandDotsToFillTheGap(priceStyle, itemPrice);
            }
        }

        /// <MetaDataID>{264ef958-a145-492f-bb64-43f2232ec320}</MetaDataID>
        private void ExpandDotsToFillTheGap(MenuStyles.IPriceStyle priceStyle, IMenuCanvasFoodItemPrice itemPrice)
        {
            var spaceWidth = Font.MeasureText(" ").Width;

            string dotString = "";
            switch (priceStyle.BetweenDotsSpace)
            {
                case 1:
                    dotString += " .";
                    break;
                case 2:
                    dotString += "  .";
                    break;
                default:
                    dotString += ".";
                    break;
            }

            Double priceLeaderWidht = PriceLeader.Font.MeasureText(PriceLeader.Description + dotString).Width;
            while (PriceLeader.XPos + priceLeaderWidht + spaceWidth < itemPrice.XPos)
            {
                PriceLeader.Description += dotString;
                priceLeaderWidht = PriceLeader.Font.MeasureText(PriceLeader.Description + dotString).Width;
            }

            #region Correct PriceLeader pos in midle of gap

            if (ExtraDescriptionInPriceLine)
            {
                double available = itemPrice.XPos - (SubTexts[PriceLineIndex.Value + 1].XPos + SubTexts[PriceLineIndex.Value + 1].Width + spaceWidth);
                PriceLeader.XPos += (available - PriceLeader.Width) / 2;
            }
            else
            {
                double available = itemPrice.XPos - (SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width);
                PriceLeader.XPos += (available - PriceLeader.Width) / 2;
            }

            #endregion
        }

        /// <MetaDataID>{3d1dd4a8-ccca-4fe0-9a85-6cc777c72463}</MetaDataID>
        private void SetPricesWithLineText(MenuStyles.IPriceStyle priceStyle)
        {
            var spaceWidth = Font.MeasureText(" ").Width;
            bool priceInNewLine = false;
            if (priceStyle.PriceLeader != null && SubTexts[PriceLineIndex.Value].Width > 0)
            {
                if (priceStyle.PriceLeader == "dots")
                {
                    if (PriceLeader == null)
                        PriceLeader = new MenuCanvasPriceLeader();
                    PriceLeader.Description = "...";
                }
                else
                {
                    //string priceLeader = System.Web.HttpUtility.HtmlDecode(priceStyle.PriceLeader + ";");
                    if(PriceLeader==null)
                        PriceLeader = new MenuCanvasPriceLeader();
                    PriceLeader.Description = priceStyle.PriceLeader;
                }
                Size priceLeaderSize = PriceLeader.Font.MeasureText(PriceLeader.Description);
                PriceLeader.Height = priceLeaderSize.Height;
                PriceLeader.Width = priceLeaderSize.Width;

                PriceLeader.XPos = SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width + spaceWidth;
                if (PriceLeader.XPos + PriceLeader.Width + FoodItemPricesWitdh > XPos + Width)
                    priceInNewLine = true;
                else
                PriceLeader.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
            }
            else
                PriceLeader = null;

            double priceYpos = 0;
            if (priceInNewLine)
            {
                var layoutStyle = (Page.Style.Styles["layout"] as MenuStyles.LayoutStyle);
                double lineSpacing = (layoutStyle.LineSpacing - 1) * 20;
                priceYpos = SubTexts[PriceLineIndex.Value].YPos + SubTexts[PriceLineIndex.Value].Height + lineSpacing;

                if(PriceLeader!=null)
                    PriceLeader.XPos= GetLineTextXpos(new Size(PriceLeader.Width + spaceWidth + FoodItemPricesWitdh, PriceLeader.Height));
            }

            foreach (var itemPrice in Prices)
            {

                if (priceInNewLine)
                {
                    if (PriceLeader == null)
                        itemPrice.XPos = GetLineTextXpos(new Size(FoodItemPricesWitdh, itemPrice.Height));// SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width + spaceWidth;
                    else
                        itemPrice.XPos = PriceLeader.XPos + PriceLeader.Width + spaceWidth;
                    itemPrice.YPos = priceYpos;
                    if (PriceLeader != null)
                        PriceLeader.AlignOnBaseline(itemPrice);
                }
                else
                {
                    if (PriceLeader == null)
                        itemPrice.XPos = SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width + spaceWidth;
                    else
                        itemPrice.XPos = PriceLeader.XPos + PriceLeader.Width + spaceWidth;

                    itemPrice.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
                }




                itemPrice.Visisble = true;
            }
        }

        /// <MetaDataID>{00cc5c62-ea93-4f0c-a5c4-0e945722de76}</MetaDataID>
        private double GetPriceLeaderWidth(MenuStyles.IPriceStyle priceStyle)
        {
            if (priceStyle.PriceLeader == null)
                return 0;
            var spaceWidth = Font.MeasureText(" ").Width;
            if (priceStyle.PriceLeader == "dots")
                return priceStyle.Font.MeasureText("...").Width + 2 * spaceWidth;
            else
            {
                //string priceLeader = System.Web.HttpUtility.HtmlDecode(priceStyle.PriceLeader + ";");
                return priceStyle.Font.MeasureText(priceStyle.PriceLeader).Width + 2 * spaceWidth;
            }
        }


        /// <exclude>Excluded</exclude>
        double _Width;


        /// <MetaDataID>{6a54b03b-c7ea-4e87-b0a3-77db0ca1f4fe}</MetaDataID>
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
        double _Height;

        /// <MetaDataID>{2fa01f04-54cd-4c13-b98d-9cfeba1e6367}</MetaDataID>
        public double Height
        {
            get
            {
                return _Height;
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

        /// <MetaDataID>{46453625-16f6-42e6-b256-067d06a0a7fe}</MetaDataID>
        int? PriceLineIndex;
        //double PriceLineTextWidth;

        //double PriceLineTextHeight;


        /// <exclude>Excluded</exclude>
        double _XPos;


        /// <MetaDataID>{f377ab1a-3669-45c4-81d9-1504fcd359f9}</MetaDataID>
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
                        _XPos = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        double _YPos;


        /// <MetaDataID>{81d219b9-cf63-4b1d-a09f-1de11f631cb4}</MetaDataID>
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
                        _YPos = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        IMenuPageCanvas _Page;

        /// <MetaDataID>{f97dc905-cc15-4d5f-b07f-0f2222a28872}</MetaDataID>
        [PersistentMember(nameof(_Page))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+11")]

        public IMenuPageCanvas Page
        {
            get
            {
                return _Page;
            }


        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuCanvasPriceLeader> _PriceLeader = new OOAdvantech.Member<IMenuCanvasPriceLeader>();


        /// <MetaDataID>{383742c6-8eb7-4abd-82fd-b9e1f2c0cc35}</MetaDataID>
        [ImplementationMember(nameof(_PriceLeader))]
        [BackwardCompatibilityID("+4")]
        public IMenuCanvasPriceLeader PriceLeader
        {
            get
            {
                return _PriceLeader.Value;
            }
            set
            {
                if (_PriceLeader != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceLeader.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemPrice> _Prices=new OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemPrice>();

        /// <MetaDataID>{b4bdd387-b2c1-4244-a896-ceeb72ec23cd}</MetaDataID>
        [PersistentMember(nameof(_Prices))]
        [BackwardCompatibilityID("+5")]
        public List<IMenuCanvasFoodItemPrice> Prices
        {
            get
            {
                return _Prices.AsReadOnly().ToList();
            }
        }


        /// <exclude>Excluded</exclude>
        string _Extras;
        /// <MetaDataID>{e3bb99be-7367-4452-91cb-91c50a83468b}</MetaDataID>
        [PersistentMember(nameof(_Extras))]
        [BackwardCompatibilityID("+6")]
        public string Extras
        {
            get
            {
                return _Extras;
            }

            set
            {

                if (_Extras != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Extras = value;
                        stateTransition.Consistent = true;
                    }
                    OOAdvantech.Transactions.Transaction.ExecuteAsynch(new Action(() => 
                    {
                        ObjectChangeState?.Invoke(this, nameof(Extras));
                    }));
                   
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _ExtraDescription;
        /// <MetaDataID>{6b66ad2b-cd4f-49ba-b87d-9ff1631fe9a5}</MetaDataID>
        [PersistentMember(nameof(_ExtraDescription))]
        [BackwardCompatibilityID("+7")]
        public string ExtraDescription
        {
            get
            {
                return _ExtraDescription;
            }

            set
            {

                if (_ExtraDescription != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ExtraDescription = value;
                        stateTransition.Consistent = true;
                    }
                    OOAdvantech.Transactions.Transaction.ExecuteAsynch(new Action(() =>
                    {
                        

                        ObjectChangeState?.Invoke(this, nameof(ExtraDescription));
                    }));
                    
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemText> _SubTexts = new OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemText>();
        /// <MetaDataID>{e18b046d-947d-403b-9483-53861c78bc09}</MetaDataID>
        [ImplementationMember(nameof(_SubTexts))]
        [BackwardCompatibilityID("+8")]
        public System.Collections.Generic.List<MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItemText> SubTexts
        {
            get
            {
                return _SubTexts.AsReadOnly().ToList();
            }
        }

        /// <MetaDataID>{396b54e8-b0f0-4622-831f-995ad868d01f}</MetaDataID>
        public void AddFoodItemPrice(IMenuCanvasFoodItemPrice foodItemPrice)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Prices.Add(foodItemPrice);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{df58c39d-d668-4a9c-aa1f-3a51e517b478}</MetaDataID>
        public void RemoveFoodItemPrice(IMenuCanvasFoodItemPrice foodItemPrice)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Prices.Remove(foodItemPrice);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{a985577a-91e5-4aa5-8846-307f3ab7c6c8}</MetaDataID>
        public void AddSubText(IMenuCanvasFoodItemText subText)
        {
            _SubTexts.Add(subText);
        }

        /// <MetaDataID>{78b792ec-b472-40eb-b388-ec142f0c6dc7}</MetaDataID>
        public void RemoveSubText(IMenuCanvasFoodItemText subText)
        {
            _SubTexts.Remove(subText);
        }

        /// <MetaDataID>{2d3f6497-defc-40ce-92f1-a45864293f1b}</MetaDataID>
        public void WrapFoodItemTexts()
        {

        }

        /// <MetaDataID>{5b04441d-9976-46a9-bdec-9a142713e8c0}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            //if (point.Y < YPos)
            //    return ItemRelativePos.Before;

            //if (point.Y > YPos + Height)
            //    return ItemRelativePos.After;


            //if (point.Y >= YPos && point.Y <= YPos + Height)
            //{

            //    if (point.X >= XPos && point.X <= XPos + Width)
            //        return ItemRelativePos.OnPos;
            //    if (point.X < XPos)
            //        return ItemRelativePos.Before;

            //}

            //return ItemRelativePos.After;

            //if (point.Y < YPos)
            //    return ItemRelativePos.Before;

            //if (point.Y > YPos + Height)
            //    return ItemRelativePos.After;

            //if (point.Y >= YPos && point.Y <= YPos + Height)
            //{

            //    if (point.X >= XPos && point.X <= XPos + Width)
            //        return ItemRelativePos.OnPos;
            //    if (point.X < XPos)
            //        return ItemRelativePos.Before;
            //}

            if (point.Y < CanvasFrameArea.Y)
                return ItemRelativePos.Before;

            if (point.Y > CanvasFrameArea.Y + CanvasFrameArea.Height)
                return ItemRelativePos.After;

            if (point.Y >= CanvasFrameArea.Y && point.Y <= CanvasFrameArea.Y + CanvasFrameArea.Height)
            {
                if (point.X >= CanvasFrameArea.X && point.X <= CanvasFrameArea.X + CanvasFrameArea.Width)
                    return ItemRelativePos.OnPos;
                if (point.X < CanvasFrameArea.X)
                    return ItemRelativePos.Before;
            }


            return ItemRelativePos.After;
        }

        public void OnCommitObjectState()
        {

            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            foreach (var price in this.Prices)
                objectStorage.CommitTransientObjectState(price);


        }

        public void OnActivate()
        {
            
        }

        public void OnDeleting()
        {
            
        }

        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

            ObjectChangeState?.Invoke(this, nameof(Page));
        }

        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
            ObjectChangeState?.Invoke(this, nameof(Page));
        }

        /// <MetaDataID>{408347c0-ace9-403c-abf4-f4ba25265019}</MetaDataID>
        private double FoodItemPricesWitdh
        {
            get
            {
                if (Prices.Count == 0)
                    return 0;
                double spaceWidth = 0;
                double width = 0;
                foreach (var fooItemPrice in Prices)
                {
                    if (spaceWidth == 0)
                        spaceWidth = fooItemPrice.Font.MeasureText(" ").Width;
                    else
                        width += spaceWidth;

                    width += fooItemPrice.Font.MeasureText(fooItemPrice.Description).Width;

                }
                return width;
            }
        }

        /// <MetaDataID>{851ce937-4a6f-4381-a9ba-d9266cfa1d6e}</MetaDataID>
        public Rect CanvasFrameArea
        {
            get
            {
                double x = HostingArea.Column.XPos - 10;
                double y = SubTexts[0].YPos - 20;
                double height = Height + 20;
                double width = HostingArea.Column.Width + 20;

                return new Rect(x, y, width, height);

            }
        }
        /// <exclude>Excluded</exclude>
        IMenuItem _MenuItem;

        public IMenuItem MenuItem
        {
            get
            {
                return _MenuItem;
            }

            set
            {
                if (_MenuItem != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuItem = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}
