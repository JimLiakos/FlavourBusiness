using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using System.Windows;
using OOAdvantech;
using MenuModel;
using UIBaseEx;

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
        /// <exclude>Excluded</exclude>
        bool _PriceInvisible;

        /// <MetaDataID>{eb69ccd1-e0ec-4344-bc0f-4f007b11807b}</MetaDataID>
        [PersistentMember(nameof(_PriceInvisible))]
        [BackwardCompatibilityID("+21")]
        public bool PriceInvisible
        {
            get => _PriceInvisible;
            set
            {
                if (_PriceInvisible != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceInvisible = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{0952dd37-886e-4be0-b0b1-927ff9086eb5}</MetaDataID>
        public MenuCanvasFoodItem()
        {

        }

        /// <exclude>Excluded</exclude>
        Member<MenuStyles.IAccent> _AccentType = new Member<MenuStyles.IAccent>();

        [Association("MenuCanvasFoodItemAccent", Roles.RoleA, "f7f1e9f2-1ff8-401b-a27d-10d832e07cf6")]
        [PersistentMember(nameof(_AccentType))]
        public MenuStyles.IAccent AccentType
        {
            get => _AccentType.Value;
            set
            {
                if (_AccentType != value)
                {
                    MenuStyles.Accent newHeadingAccent = value as MenuStyles.Accent;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (newHeadingAccent != null && OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(value) != OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this))
                        {
                            newHeadingAccent = MenuStyles.Accent.Clone(value as MenuStyles.Accent);
                            if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(newHeadingAccent);
                        }
                        if (_AccentType != null)
                            OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(_AccentType);
                        _AccentType.Value = newHeadingAccent;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(AccentType));
                }
                if (_AccentType == null)
                    _MenuCanvasAccent.Value = null;

            }
        }


        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{0d17349c-330a-4a2a-b263-72aff049a151}</MetaDataID>
        [PersistentMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }


        /// <exclude>Excluded</exclude>
        bool _CustomSpacing;
        /// <MetaDataID>{c34a92d7-febf-4f88-a8d4-c12fce9df845}</MetaDataID>
        [PersistentMember(nameof(_CustomSpacing))]
        [BackwardCompatibilityID("+18")]
        public bool CustomSpacing
        {
            get => _CustomSpacing;
            set
            {

                if (_CustomSpacing != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CustomSpacing = value;
                        stateTransition.Consistent = true;
                    }
                    if (value)
                    {
                        _BeforeSpacing = Style.BeforeSpacing;
                        _AfterSpacing = Style.AfterSpacing;
                    }
                    else
                    {
                        _BeforeSpacing = null;
                        _AfterSpacing = null;

                    }
                    var culture = CultureContext.CurrentCultureInfo;
                    var useDefaultCultureValue = CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (CultureContext cultureContext = new CultureContext(culture, useDefaultCultureValue))
                        {
                            ObjectChangeState?.Invoke(this, nameof(CustomSpacing));
                        }
                    }));

                }
            }
        }
        /// <exclude>Excluded</exclude>
        double? _AfterSpacing;

        /// <MetaDataID>{2b654c5c-1b89-4e70-b1c6-4166061f287b}</MetaDataID>
        [PersistentMember(nameof(_AfterSpacing))]
        [BackwardCompatibilityID("+17")]
        public double AfterSpacing
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (Style != null && !_AfterSpacing.HasValue)
                        return Style.AfterSpacing;
                    if (!_AfterSpacing.HasValue)
                        return default(double);
                    return _AfterSpacing.Value;
                }
            }
            set
            {
                if (_AfterSpacing != value && CustomSpacing)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AfterSpacing = value;
                        stateTransition.Consistent = true;
                    }
                    var culture = CultureContext.CurrentCultureInfo;
                    var useDefaultCultureValue = CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (CultureContext cultureContext = new CultureContext(culture, useDefaultCultureValue))
                        {
                            ObjectChangeState?.Invoke(this, nameof(AfterSpacing));
                        }
                    }));
                }
            }
        }


        /// <exclude>Excluded</exclude>
        double? _BeforeSpacing;
        /// <MetaDataID>{c81f16fa-32ac-4d07-9d53-9477946cbb92}</MetaDataID>
        [PersistentMember(nameof(_BeforeSpacing))]
        [BackwardCompatibilityID("+16")]
        public double BeforeSpacing
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (Style != null && !_BeforeSpacing.HasValue)
                        return Style.BeforeSpacing;
                    if (!_BeforeSpacing.HasValue)
                        return default(double);
                    return _BeforeSpacing.Value;
                }
            }
            set
            {
                if (_BeforeSpacing != value && CustomSpacing)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BeforeSpacing = value;
                        stateTransition.Consistent = true;
                    }
                    var culture = CultureContext.CurrentCultureInfo;
                    var useDefaultCultureValue = CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (CultureContext cultureContext = new CultureContext(culture, useDefaultCultureValue))
                        {
                            ObjectChangeState?.Invoke(this, nameof(BeforeSpacing));
                        }
                    }));

                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Span;
        /// <MetaDataID>{e8aa8e7d-ef5c-4ed0-a125-f8b19afce158}</MetaDataID>
        [PersistentMember(nameof(_Span))]
        [BackwardCompatibilityID("+15")]
        public bool Span
        {
            get => _Span;
            set
            {

                if (_Span != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Span = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{b5de309f-06b4-4071-a983-2161442737c2}</MetaDataID>
        public void Remove()
        {

            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
            MultiPriceHeading = null;
        }

        /// <MetaDataID>{cec01aaa-98be-48f2-982a-74c6f64a56d1}</MetaDataID>
        public void ResetSize()
        {
            BaseLine = Font.GetTextBaseLine(Description);
        }

        /// <exclude>Excluded</exclude>
        double _BaseLine;
        /// <MetaDataID>{35d71915-8bc5-4ee2-bd85-413cd4228781}</MetaDataID>
        [PersistentMember(nameof(_BaseLine))]
        [BackwardCompatibilityID("+14")]
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
                        _BaseLine = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.MultilingualSet<IMenuCanvasFoodItemText> _SubTexts = new OOAdvantech.Collections.Generic.MultilingualSet<IMenuCanvasFoodItemText>();
        /// <MetaDataID>{e18b046d-947d-403b-9483-53861c78bc09}</MetaDataID>
        [PersistentMember(nameof(_SubTexts))]
        [BackwardCompatibilityID("+8")]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItemText> SubTexts
        {
            get
            {
                return _SubTexts.AsReadOnly().ToList();
            }
        }



        /// <exclude>Excluded</exclude>
        IMenuCanvasFoodItemPrice _MainPrice;
        /// <MetaDataID>{c7c179e5-95d1-4b6b-b225-12351416ab45}</MetaDataID>
        public IMenuCanvasFoodItemPrice MainPrice
        {
            get
            {


                return _MainPrice;
            }

            set
            {
                _MainPrice = value;
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{070539fb-b66e-4af1-81ad-f45e8be6363b}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _MaxHeight = new MultilingualMember<double>();
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
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_MaxHeight != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _MaxHeight.Value = value;
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        FontData? _Font;
        /// <MetaDataID>{b517a0c2-ff4c-4a66-9c4e-207ead3ec97b}</MetaDataID>
        public FontData Font
        {
            get
            {
                if (_Font.HasValue)
                    return _Font.Value;
                else if (Style != null)
                    return Style.Font;
                else
                    return default(FontData);
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
                else if (RestaurantMenu.ConntextStyleSheet != null)
                    styleSheet = RestaurantMenu.ConntextStyleSheet;
                if (styleSheet != null)
                    return styleSheet.Styles["menu-item"] as MenuStyles.IMenuItemStyle;
                else
                    return null;
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Description = new MultilingualMember<string>();

        /// <MetaDataID>{978876dd-3f6e-4c75-90f4-05d9cd74106d}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                string description = _Description;
                if (MenuItem == null)//blank item
                {
                    if (description == null)
                        return Properties.Resources.BlankItemName;
                    else
                        return description;
                }
                if (string.IsNullOrWhiteSpace(description))
                    description = MenuItem.Name;
                if (string.IsNullOrWhiteSpace(description))
                {
                    //description = "...";
                }
                return description;
            }
            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_Description != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _Description.Value = value;
                            stateTransition.Consistent = true;
                        }
                        FontData font = Font;
                        font.Foreground = "#80141C";
                        //Font = font;
                        ObjectChangeState?.Invoke(this, nameof(Description));

                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasFoodItemsGroup> _HostingArea = new OOAdvantech.MultilingualMember<IMenuCanvasFoodItemsGroup>();



        /// <MetaDataID>{a3931068-a9d7-46e3-b329-2945dc96be4e}</MetaDataID>
        [PersistentMember(nameof(_HostingArea))]
        public IMenuCanvasFoodItemsGroup HostingArea
        {
            get
            {
                return _HostingArea.Value;
            }
            set
            {
                if (_HostingArea != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HostingArea.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        ///When is true the extra description starts at price line otherwise start at next line
        /// <MetaDataID>{f5e5899f-4ea8-4c02-a390-e25dff4d05eb}</MetaDataID>
        bool ExtraDescriptionInPriceLine;

        /// <MetaDataID>{d2e51f5e-4aef-45fb-a77d-8ca02dcfe2db}</MetaDataID>
        List<string> GetExtraDescriptionLinesText(double width, PriceLine priceLine, List<IMenuCanvasFoodItemText> existingSubTexts)
        {
            MenuStyles.ILayoutStyle layoutStyle = Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle;
            double lineSpacing = (layoutStyle.LineSpacing - 1) * 20;

            string descriptionSeparator = "";
            if (!string.IsNullOrWhiteSpace(ExtraDescription))
                descriptionSeparator = layoutStyle.DescSeparator;

            if (!string.IsNullOrWhiteSpace(descriptionSeparator))
            {
                descriptionSeparator += " ";
                if (!Style.NewLineForDescription)
                    descriptionSeparator = " " + descriptionSeparator;
            }
            var spaceWidth = Font.MeasureText(" ").Width;
            string extraDescription = descriptionSeparator + ExtraDescription;
            if (string.IsNullOrWhiteSpace(extraDescription))
                extraDescription = null;
            ExtraDescriptionInPriceLine = false;

            if (extraDescription != null && !Style.NewLineForDescription && (FoodItemAlignment != MenuStyles.Alignment.Center))
            {
                //mix fist line description with last item name line
                MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                IMenuCanvasFoodItemText foodItemLineText = SubTexts.Last();
                double foodItemPricesWithLeaderWitdh = 0;
                bool foodItemPriceLeaderAtNewLine = false;
                bool foodItemPriceAtNewLine = false;
                double foodItemPriceWidhtAtNewLine = 0;

                if (PriceLineIndex.HasValue && SubTexts.Count - 1 == PriceLineIndex.Value)
                {
                    foodItemPricesWithLeaderWitdh = FoodItemPricesWithLeaderWitdh;
                    ExtraDescriptionInPriceLine = true;
                    if (FoodItemPriceLayout == MenuStyles.PriceLayout.WithName && FoodItemAlignment != MenuStyles.Alignment.Center)
                    {
                        bool foodItemLineTextWithPrice = (foodItemPricesWithLeaderWitdh + foodItemLineText.Width + layoutStyle.NameIndent) < width;
                        bool foodItemLineTextithPriceLider = (PriceLeaderWidth + foodItemLineText.Width + layoutStyle.NameIndent) < width;
                        foodItemPriceLeaderAtNewLine = !foodItemLineTextithPriceLider;
                        foodItemPriceAtNewLine = !foodItemLineTextWithPrice;
                        foodItemPriceWidhtAtNewLine = ((foodItemPriceLeaderAtNewLine ? 1 : 0) * PriceLeaderWidth + (foodItemPriceAtNewLine ? 1 : 0) * FoodItemPricesWitdh);
                    }
                }
                double descriptionWidth;
                if (!foodItemPriceLeaderAtNewLine && !foodItemPriceAtNewLine)
                    descriptionWidth = Width - (foodItemPricesWithLeaderWitdh + foodItemLineText.Width + spaceWidth + layoutStyle.NameIndent); //mixedDescriptionPart at last item name line
                else
                    descriptionWidth = Width - foodItemPriceWidhtAtNewLine;//mixedDescriptionPart at new line for price

                string mixedDescriptionPart = GetLinesText(extraDescription, descriptionWidth, Style.DescriptionFont, false).FirstOrDefault();

                if (mixedDescriptionPart != null)
                {
                    MenuCanvasFoodItemText subText = GetMenuCanvasFoodItemText(mixedDescriptionPart, 0, 0, Style.DescriptionFont, existingSubTexts);
                    subText.AlignOnBaseline(foodItemLineText);
                    _SubTexts.Add(subText);



                    if (foodItemPriceLeaderAtNewLine || foodItemPriceAtNewLine)
                    {
                        //mixedDescriptionPart at new line for price

                        #region mixedDescriptionPart at new line for price
                        double priceWidth = ((foodItemPriceLeaderAtNewLine ? 1 : 0) * PriceLeaderWidth + (foodItemPriceAtNewLine ? 1 : 0) * FoodItemPricesWitdh);
                        Size lineTextSize = new Size(priceWidth + subText.Width, subText.Height);
                        subText.XPos = GetLineTextXpos(lineTextSize, FoodItemAlignment) + priceWidth + spaceWidth;
                        subText.AlignOnBaseline(MainPrice); //Align mixedDescriptionPart to price

                        if (FoodItemAlignment == MenuStyles.Alignment.Right)
                        {
                            //Align  price to mixedDescriptionPart
                            if (foodItemPriceLeaderAtNewLine)
                            {
                                if (PriceLeader != null)
                                    PriceLeader.XPos -= subText.Width;
                            }
                            if (foodItemPriceAtNewLine)
                            {
                                if (MainPrice.Visisble)
                                    MainPrice.XPos -= subText.Width;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        Size lineTextSize = new Size(foodItemLineText.Width + foodItemPricesWithLeaderWitdh + subText.Width, foodItemLineText.Height);
                        foodItemLineText.XPos = GetLineTextXpos(lineTextSize, FoodItemAlignment);

                        if (FoodItemPriceLayout == MenuStyles.PriceLayout.Normal)
                            subText.XPos = foodItemLineText.XPos + foodItemLineText.Width + spaceWidth; //Price at the end of extra description
                        else
                            subText.XPos = foodItemLineText.XPos + foodItemLineText.Width + foodItemPricesWithLeaderWitdh + spaceWidth; //Price before extra description

                        if (PriceLineIndex != null && SubTexts.IndexOf(foodItemLineText) == PriceLineIndex.Value && FoodItemPriceLayout == MenuStyles.PriceLayout.Normal)
                            PriceLineIndex = SubTexts.IndexOf(subText);
                    }

                }



                if (mixedDescriptionPart != null && mixedDescriptionPart.Length >= extraDescription.Length)
                    return new List<string>();//Entire description at item name last line
                else
                {
                    string newLineDescription;
                    if (mixedDescriptionPart == null)
                        newLineDescription = extraDescription;
                    else
                        newLineDescription = extraDescription.Substring(mixedDescriptionPart.Length).TrimStart();

                    double indentWidth = layoutStyle.DescLeftIndent;
                    if (FoodItemAlignment == MenuStyles.Alignment.Right)
                        indentWidth = layoutStyle.DescRightIndent;

                    if (FoodItemAlignment == MenuStyles.Alignment.Center)
                        return GetLinesText(newLineDescription, width, priceLine, Style.DescriptionFont);
                    else
                        return GetLinesText(newLineDescription, width - indentWidth, priceLine, Style.DescriptionFont);
                }


            }
            else
            {

                var indentWidth = layoutStyle.DescLeftIndent;
                //if (FoodItemAlignment == MenuStyles.Alignment.Right)
                indentWidth += layoutStyle.DescRightIndent;

                if (FoodItemAlignment == MenuStyles.Alignment.Center)
                    return GetLinesText(extraDescription, width, priceLine, Style.DescriptionFont);
                else
                    return GetLinesText(extraDescription, width - indentWidth, priceLine, Style.DescriptionFont);
            }
        }

        /// <MetaDataID>{d6f216ae-4c15-4124-9db4-4e75578f3445}</MetaDataID>
        List<string> GetLinesText(string text, double width, FontData font, bool allowFirstLineOverlap = true)
        {
            return GetLinesText(text, width, PriceLine.NoPrice, font, allowFirstLineOverlap);
        }
        /// <MetaDataID>{8fd240a8-5a2d-4e55-9ced-d066a8890db1}</MetaDataID>
        List<string> GetLinesText(string text, double width, PriceLine priceLine, FontData font, bool allowFirstLineOverlap = true)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();


            string sentence = text;// "This item uses Market for price. The price field can be any number or text, and can even be multiple prices (separate them with a semi-colon)";
            List<string> wrappedText = new List<string>();
            string lineSentence = null;

            string[] words = sentence.Split(' ');
            int i = 0;
            foreach (var word in words)
            {
                bool lastWord = i == (words.Length - 1);
                var size = font.MeasureText(lineSentence + word + " ");
                double lineWitdh = width;
                if (priceLine == PriceLine.FirstLine && wrappedText.Count == 0)
                    lineWitdh -= FoodItemPricesWithLeaderWitdh;

                ////Calculate the last line width when price displayed at last line of text 
                ////the last line is defined by the last word
                //if (priceLinePosition == PriceLine.LastLine && lastWord )
                //    lineWitdh -= priceWidth;


                if (size.Width < lineWitdh || (lineSentence == null && allowFirstLineOverlap))
                {
                    if (word.IndexOf("\r\n") != -1)
                    {
                        if (word.IndexOf("\r\n") != 0)
                            lineSentence += word.Substring(0, word.IndexOf("\r\n"));

                        string lineText = lineSentence.Substring(0, lineSentence.Length);
                        lineSentence = word.Substring(word.IndexOf("\r\n") + 2) + " ";
                        if (lineText.Length > 0)
                            wrappedText.Add(lineText);
                    }
                    else if (word.IndexOf("\n") != -1)
                    {
                        if (word.IndexOf("\n") != 0)
                            lineSentence += word.Substring(0, word.IndexOf("\n"));

                        string lineText = lineSentence.Substring(0, lineSentence.Length);
                        lineSentence = word.Substring(word.IndexOf("\n") + 2) + " ";
                        if (lineText.Length > 0)
                            wrappedText.Add(lineText);
                    }
                    else
                    {
                        if (i + 1 != words.Length)
                            lineSentence += word + " ";
                        else
                            lineSentence += word;
                    }
                }
                else
                {
                    if (lineSentence == null)
                        return new List<string>();
                    string lineText = lineSentence.Substring(0, lineSentence.Length - 1);
                    if (lineText.Length > 0)
                        wrappedText.Add(lineText);
                    if (i + 1 != words.Length)
                        lineSentence = word + " ";
                    else
                        lineSentence = word;
                }
                i++;
            }
            if (lineSentence.Length > 0)
                wrappedText.Add(lineSentence);

            if (FoodItemPricesWithLeaderWitdh != 0 && priceLine == PriceLine.LastLine && wrappedText.Count > 0)
            {
                //Adds pseudo line for price
                string lastLine = wrappedText[wrappedText.Count - 1];
                var size = font.MeasureText(lastLine);
                double lineWitdh = width;
                //if (size.Width + priceWidth > lineWitdh)
                //    wrappedText.Add("");
            }
            //if (priceWidth != 0 && priceLinePosition == PriceLine.LastLine && wrappedText.Count > 0)
            //{
            //    string lastLine = wrappedText[wrappedText.Count - 1];
            //    var size = font.MeasureText(lastLine);
            //    double lineWitdh = width - priceWidth;
            //    if (size.Width > lineWitdh)
            //    {
            //        wrappedText.RemoveAt(wrappedText.Count - 1);
            //        string newLastline = null;
            //        lineSentence = "";
            //        foreach (var word in lastLine.Split(' ').Reverse())
            //        {
            //            size = font.MeasureText(word + " " + sentence);
            //            if (size.Width < priceWidth)
            //            {
            //                sentence = word + " " + sentence;
            //            }
            //            else
            //            {
            //                if (newLastline == null)
            //                {
            //                    newLastline = word + " " + sentence;
            //                    sentence = "";
            //                }
            //                else
            //                {
            //                    sentence = word + " " + sentence;
            //                }
            //            }
            //        }
            //        if (newLastline.Length > 0)
            //            wrappedText.Add(newLastline);
            //        if (sentence.Length > 0)
            //            wrappedText.Add(sentence);


            //    }
            //}
            return wrappedText;
        }
        /// <MetaDataID>{a0714699-f0af-4018-b3a8-1fb14c81f44b}</MetaDataID>
        bool PriceAtFirstLineofItem
        {
            get
            {
                MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                return FoodItemPriceLayout == MenuStyles.PriceLayout.Normal && FoodItemAlignment != MenuStyles.Alignment.Center;
            }
        }


        /// <MetaDataID>{f1d49947-77ba-4654-8caa-358db9f5daef}</MetaDataID>
        bool PriceAtLastLineOfItem
        {
            get
            {
                MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                return FoodItemPriceLayout == MenuStyles.PriceLayout.WithName;// && FoodItemAlignment != MenuStyles.Alignment.Center;
            }
        }


        /// <MetaDataID>{6e9ed1c1-3a39-46f7-a200-d13130ce25d7}</MetaDataID>
        MenuCanvasFoodItemText GetMenuCanvasFoodItemText(string text, double xPos, double yPos, FontData font, List<IMenuCanvasFoodItemText> existingSubTexts)
        {
            var lineTextSize = font.MeasureText(text);
            if (string.IsNullOrEmpty(text))
            {
                lineTextSize = font.MeasureText(" ");
                lineTextSize.Width = 0;
            }

            if (existingSubTexts.Count == 0)
            {
                var subText = new MenuCanvasFoodItemText(text, xPos, yPos, font);
                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(subText);

                subText.Width = lineTextSize.Width;
                subText.Height = lineTextSize.Height;
                double fontSpacingCorrection = font.Html5GetFontSpacingCorrection(text, lineTextSize.Width);
                subText.FontSpacingCorrection = fontSpacingCorrection;


                return subText;
            }
            else
            {
                IMenuCanvasFoodItemText subText = existingSubTexts[0];
                existingSubTexts.RemoveAt(0);
                subText.Description = text;
                subText.XPos = xPos;
                subText.YPos = yPos;
                subText.Font = font;

                subText.Width = lineTextSize.Width;
                subText.Height = lineTextSize.Height;

                double fontSpacingCorrection = font.Html5GetFontSpacingCorrection(text, lineTextSize.Width);
                subText.FontSpacingCorrection = fontSpacingCorrection;
                return subText as MenuCanvasFoodItemText;
            }

        }

        public MenuStyles.Alignment FoodItemAlignment
        {
            get
            {
                return Style.Alignment;
            }
        }

        public MenuStyles.PriceLayout FoodItemPriceLayout
        {
            get
            {
                return PriceStyle.Layout;
            }
        }


        /// <MetaDataID>{4dad34a6-5c60-4d9e-bb2a-f8f5a5a4a3af}</MetaDataID>
        public void RenderMenuCanvasItems(IItemMultiPriceHeading multiPriceHeading)
        {
            lock (this)
            {

                List<IMenuCanvasFoodItemText> existingSubTexts = _SubTexts.ToList();
                _SubTexts.Clear();
                PriceLineIndex = null;
                MenuStyles.ILayoutStyle layoutStyle = Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle;
                MenuStyles.IMenuItemStyle menuItemStyle = Page.Style.Styles["menu-item"] as MenuStyles.IMenuItemStyle;
                ResetPriceLeaderDescription();




                double nextMenuCanvasItemX = XPos;
                double nextMenuCanvasItemY = YPos;
                List<string> itemNameLinesText = null;
                #region Create lines texts for Description
                double lineSpacing = (layoutStyle.LineSpacing - 1) * 20;
                string description = Description;
                if (string.IsNullOrWhiteSpace(description))
                    description = "...";
                if (description != null)
                    description = description.Trim();
                if (MenuItem == null)
                    description = "  ";
                if (PriceAtFirstLineofItem)
                {
                    itemNameLinesText = GetLinesText(description, Width, PriceLine.FirstLine, Font);
                    PriceLineIndex = 0;

                }
                else if (PriceAtLastLineOfItem)
                {
                    itemNameLinesText = GetLinesText(description, Width, PriceLine.LastLine, Font);
                    PriceLineIndex = itemNameLinesText.Count - 1;
                    if (itemNameLinesText[PriceLineIndex.Value] == "")
                        PriceLineIndex = PriceLineIndex.Value - 1;

                }
                else
                    itemNameLinesText = GetLinesText(description, Width, PriceLine.NoPrice, Font);



                bool unTranslated = false;
                if (MenuItem != null)
                {
                    using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, false))
                    {
                        unTranslated = description != Description?.Trim();
                    }
                }
                int i = 0;
                foreach (string itemNameLineText in itemNameLinesText)
                {


                    MenuCanvasFoodItemText foodItemNameSubText = GetMenuCanvasFoodItemText(itemNameLineText, XPos, nextMenuCanvasItemY, Font, existingSubTexts);

                    Size lineTextSize = new Size(foodItemNameSubText.Width, foodItemNameSubText.Height);
                    bool priceInNewLine = false;
                    if (PriceLineIndex.HasValue && PriceLineIndex.Value == i)
                    {

                        if (foodItemNameSubText.Width + FoodItemPricesWithLeaderWitdh < Width)
                        {
                            lineTextSize = new Size(lineTextSize.Width + FoodItemPricesWithLeaderWitdh, lineTextSize.Height);    //item text with price and price leader
                        }
                        else if (foodItemNameSubText.Width + FoodItemPricesWithLeaderWitdh > Width && PriceAtFixedPosition)
                        {
                            //item text with price and price leader 
                            foodItemNameSubText.AdjustDescriptionForWidth(Width - FoodItemPricesWithLeaderWitdh);//cut unwrapped text to fit
                            lineTextSize = new Size(foodItemNameSubText.Width + FoodItemPricesWithLeaderWitdh, foodItemNameSubText.Height);
                        }
                        else if (FoodItemPriceLayout == MenuStyles.PriceLayout.WithName && (lineTextSize.Width + GetPriceLeaderWidth(PriceStyle) <= Width))
                        {
                            lineTextSize = new Size(lineTextSize.Width + GetPriceLeaderWidth(PriceStyle), lineTextSize.Height); //item text with price and price leader 
                                                                                                                                //used only when style is price WithName
                            nextMenuCanvasItemY += foodItemNameSubText.Height + lineSpacing;//new line for price
                        }
                        else
                        {
                            lineTextSize = new Size(lineTextSize.Width, lineTextSize.Height);
                            nextMenuCanvasItemY += foodItemNameSubText.Height + lineSpacing;//new line for leader ad price
                        }
                        foodItemNameSubText.XPos = GetItemNameLineTextXpos(lineTextSize, FoodItemAlignment);

                        //if (FoodItemAlignment == MenuStyles.Alignment.Right && PriceAtFixedPosition)
                        //    foodItemNameSubText.XPos -= (GetPriceLeaderWidth(PriceStyle) + FoodItemPricesWitdh);


                    }
                    else
                        foodItemNameSubText.XPos = GetItemNameLineTextXpos(lineTextSize, FoodItemAlignment);



                    foodItemNameSubText.UnTranslated = unTranslated;
                    _SubTexts.Add(foodItemNameSubText);
                    //priceInNewLine = false;
                    //if (priceInNewLine)
                    //{
                    //    nextMenuCanvasItemY += subText.Height + lineSpacing;
                    //    //PriceLineIndex = i + 1;
                    //    //subText = GetMenuCanvasFoodItemText("", XPos, nextMenuCanvasItemY, FoodItemPriceFont, existingSubTexts);
                    //    //lineTextSize = new Size(FoodItemPricesWitdh + GetPriceLeaderWidth(priceStyle), subText.Height);
                    //    //subText.XPos = GetItemNameLineTextXpos(lineTextSize, FoodItemAlignment);
                    //    //_SubTexts.Add(subText);
                    //}
                    nextMenuCanvasItemY += foodItemNameSubText.Height + lineSpacing;
                    i++;
                }
                #endregion


                BuildPrices(multiPriceHeading);

                List<string> descriptionLinesText;

                #region Gets description lines and evaluate thr price position
                if (FoodItemPriceLayout == MenuStyles.PriceLayout.FollowDescription && FoodItemAlignment == MenuStyles.Alignment.Left)
                {
                    descriptionLinesText = GetExtraDescriptionLinesText(Width, PriceLine.NoPrice, existingSubTexts);
                    PriceLineIndex = _SubTexts.Count + descriptionLinesText.Count - 1;//price at the last description text line in case when can fit in "PriceLine.NoPrice"

                }
                else if (FoodItemPriceLayout == MenuStyles.PriceLayout.Normal && FoodItemAlignment == MenuStyles.Alignment.Center)
                {
                    //price at the end of description text at new line
                    descriptionLinesText = GetExtraDescriptionLinesText(Width, PriceLine.FirstLine, existingSubTexts);
                    descriptionLinesText.Add("");//Price in new Line subtext
                    PriceLineIndex = _SubTexts.Count + descriptionLinesText.Count - 1;
                }
                else if (FoodItemPriceLayout == MenuStyles.PriceLayout.WithDescription || (FoodItemPriceLayout == MenuStyles.PriceLayout.FollowDescription && FoodItemAlignment != MenuStyles.Alignment.Left))
                {
                    descriptionLinesText = GetExtraDescriptionLinesText(Width, PriceLine.NoPrice, existingSubTexts);
                    if (descriptionLinesText.Count == 0)
                        descriptionLinesText.Add("");
                    PriceLineIndex = _SubTexts.Count + descriptionLinesText.Count - 1;
                }
                else if (!Style.NewLineForDescription && PriceAtFirstLineofItem)
                    descriptionLinesText = GetExtraDescriptionLinesText(Width, PriceLine.FirstLine, existingSubTexts);
                else
                    descriptionLinesText = GetExtraDescriptionLinesText(Width, PriceLine.NoPrice, existingSubTexts);
                #endregion

                i = SubTexts.Count;

                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    unTranslated = string.IsNullOrEmpty(ExtraDescription);
                }

                foreach (string descriptionLineText in descriptionLinesText)
                {

                    #region creates  a subtext for description line text 
                    MenuCanvasFoodItemText subText = GetMenuCanvasFoodItemText(descriptionLineText, XPos, nextMenuCanvasItemY, Style.DescriptionFont, existingSubTexts);
                    Size lineTextSize = new Size(subText.Width, subText.Height);

                    if (PriceLineIndex.HasValue && PriceLineIndex.Value == i)
                    {
                        #region evaluate the size of line with price and price leader

                        // evaluate lineTextSize when price displayed at this line
                        if (PriceAtFixedPosition || (lineTextSize.Width + FoodItemPricesWithLeaderWitdh <= Width)) //When description text and prices fit in width
                            lineTextSize = new Size(lineTextSize.Width + FoodItemPricesWithLeaderWitdh, lineTextSize.Height);    //description text with price and price leader
                        else if (FoodItemPriceLayout == MenuStyles.PriceLayout.WithDescription && (lineTextSize.Width + GetPriceLeaderWidth(PriceStyle) <= Width))
                        {
                            lineTextSize = new Size(lineTextSize.Width + GetPriceLeaderWidth(PriceStyle), lineTextSize.Height); //description text with  price leader 
                                                                                                                                //used only when style is price with WithDescription
                        }
                        else
                            lineTextSize = new Size(lineTextSize.Width, lineTextSize.Height);//description text only 
                        #endregion
                    }

                    if (FoodItemAlignment == MenuStyles.Alignment.Left)
                        subText.XPos = GetLineTextXpos(lineTextSize, FoodItemAlignment) + layoutStyle.DescLeftIndent;
                    else if (FoodItemAlignment == MenuStyles.Alignment.Right)
                        subText.XPos = GetLineTextXpos(lineTextSize, FoodItemAlignment) - layoutStyle.DescRightIndent;
                    else if (FoodItemAlignment == MenuStyles.Alignment.Center)
                        subText.XPos = GetLineTextXpos(lineTextSize, FoodItemAlignment);

                    subText.UnTranslated = unTranslated;
                    _SubTexts.Add(subText);
                    #endregion

                    nextMenuCanvasItemY += lineTextSize.Height + lineSpacing;
                    i++;
                }

                double indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasLeftIndent;
                //indentWidth = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasRightIndent;

                string extras = (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasSeparator + " " + Extras;
                if (FoodItemAlignment == MenuStyles.Alignment.Center)
                    extras = extras + " " + (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).ExtrasSeparator;

                if (!PricesTextBuilt)
                    BuildPrices(multiPriceHeading);


                if (PriceLineIndex == SubTexts.Count - 1 && MainPrice != null)
                {
                    if (nextMenuCanvasItemY < MainPrice.YPos + MainPrice.Height + lineSpacing)
                        nextMenuCanvasItemY = MainPrice.YPos + MainPrice.Height + lineSpacing;
                }

                List<string> extrasLinesText;
                if (Extras != null && Extras.Length > 0)
                    extrasLinesText = GetLinesText(extras, Width - indentWidth, Style.ExtrasFont);
                else
                    extrasLinesText = new List<string>();

                foreach (string extrasLineText in extrasLinesText)
                {
                    //Size lineTextSize = Style.ExtrasFont.MeasureText(lineText);
                    //if (string.IsNullOrEmpty(lineText))
                    //{
                    //    lineTextSize = Style.ExtrasFont.MeasureText(" ");
                    //    lineTextSize.Width = 0;
                    //}
                    //if (PriceLineIndex.HasValue && PriceLineIndex.Value == i)
                    //    lineTextSize = new Size(lineTextSize.Width + GetPriceLeaderWidth(priceStyle) + FoodItemPricesWitdh, lineTextSize.Height);

                    //double lineTextXpos = GetLineTextXpos(lineTextSize) + (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescLeftIndent;


                    MenuCanvasFoodItemText subText = GetMenuCanvasFoodItemText(extrasLineText, 0, nextMenuCanvasItemY, Style.ExtrasFont, existingSubTexts);
                    _SubTexts.Add(subText);

                    var lineTextSize = new Size(subText.Width, subText.Height);
                    if (PriceLineIndex.HasValue && PriceLineIndex.Value == i)
                        lineTextSize = new Size(lineTextSize.Width + GetPriceLeaderWidth(PriceStyle) + FoodItemPricesWitdh, lineTextSize.Height);
                    subText.XPos = GetLineTextXpos(lineTextSize, FoodItemAlignment) + (Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle).DescLeftIndent;


                    nextMenuCanvasItemY += lineTextSize.Height + lineSpacing;
                    i++;
                }
                _Height = nextMenuCanvasItemY - YPos;
            }


            // BuildPrices(pageStyle, columnWidth, priceStyle, menuCanvasItem);
        }

        /// <MetaDataID>{8e9fab2a-d311-4938-a8bb-c89a32171b48}</MetaDataID>
        private double GetItemNameLineTextXpos(Size lineTextSize, MenuStyles.Alignment alignment)
        {
            MenuStyles.ILayoutStyle layoutStyle = Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle;

            double lineTextXpos = XPos;
            if (alignment == MenuStyles.Alignment.Center)
                lineTextXpos = XPos + (Width / 2) - (lineTextSize.Width / 2);
            if (alignment == MenuStyles.Alignment.Left)
                lineTextXpos = XPos + layoutStyle.NameIndent;
            if (alignment == MenuStyles.Alignment.Right)
                lineTextXpos = XPos + Width - lineTextSize.Width;

            return lineTextXpos;
        }

        /// <MetaDataID>{31d395e0-b7e5-41db-b4e1-c385948c0031}</MetaDataID>
        private double GetLineTextXpos(Size lineTextSize, MenuStyles.Alignment alignment)
        {
            double lineTextXpos = XPos;
            if (alignment == MenuStyles.Alignment.Center)
                lineTextXpos = XPos + (Width / 2) - (lineTextSize.Width / 2);
            if (alignment == MenuStyles.Alignment.Left)
                lineTextXpos = XPos;
            if (alignment == MenuStyles.Alignment.Right)
                lineTextXpos = XPos + Width - lineTextSize.Width;

            return lineTextXpos;
        }
        internal MenuStyles.IPriceStyle PriceStyle
        {
            get
            {
                return Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
            }
        }

        /// <MetaDataID>{375f7739-ce4d-4982-8bdf-e086db2f757a}</MetaDataID>
        void BuildPrices(IItemMultiPriceHeading multiPriceHeading)
        {
            if (MainPrice != null && PriceLineIndex != null)
            {
                if (!PriceInvisible)
                {
                    MenuStyles.IPriceStyle priceStyle = (MainPrice as MenuCanvasFoodItemPrice).Style;
                    if (FoodItemPriceLayout == MenuStyles.PriceLayout.WithName || FoodItemPriceLayout == MenuStyles.PriceLayout.WithDescription)
                    {
                        SetPricesWithLineText(priceStyle, multiPriceHeading);
                    }
                    else if (FoodItemPriceLayout == MenuStyles.PriceLayout.Normal || FoodItemPriceLayout == MenuStyles.PriceLayout.FollowDescription)
                    {
                        SetPricesFollowLineText(priceStyle, multiPriceHeading);
                    }
                    //else if (FoodItemPriceLayout == MenuStyles.PriceLayout.WithDescription || (FoodItemPriceLayout == MenuStyles.PriceLayout.FollowDescription && FoodItemAlignment != MenuStyles.Alignment.Left))
                    //{
                    //    SetPricesWithLineText(priceStyle, multiPriceHeading);
                    //}
                    else if (FoodItemPriceLayout == MenuStyles.PriceLayout.DoNotDisplay)
                    {
                        foreach (var price in Prices)
                            price.Visisble = false;
                        if (MainPrice != null)
                            MainPrice.Visisble = false;
                        PriceLeader = null;
                    }
                }
                else
                {
                    foreach (var price in Prices)
                        price.Visisble = false;
                    if (MainPrice != null)
                        MainPrice.Visisble = false;
                    PriceLeader = null;
                }
                PricesTextBuilt = true;
            }
            else
            {
                if (PriceLeader != null)
                    PriceLeader.ResetSize();
            }

        }


        //private void SetPricesFollowLineText1(MenuStyles.IPriceStyle priceStyle, IItemMultiPriceHeading multiPriceHeading)
        //{
        //    var spaceWidth = Font.MeasureText(" ").Width;
        //    IMenuCanvasFoodItemPrice leftMostPrice = null;
        //    if (priceStyle.PriceLeader != null && priceStyle.PriceLeader != "none")
        //    {
        //        if (PriceLeader == null)
        //        {
        //            PriceLeader = new MenuCanvasPriceLeader();
        //            if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
        //                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(PriceLeader);
        //        }
        //        if (priceStyle.PriceLeader == "dots")
        //            PriceLeader.Description = "...";
        //        else
        //            PriceLeader.Description = priceStyle.PriceLeader;
        //    }
        //    else
        //        PriceLeader = null;

        //    foreach (var itemPrice in Prices)
        //        itemPrice.Visisble = false;

        //    if (multiPriceHeading != null)
        //    {

        //        //double xPos= XPos + Width - (multiPriceHeading as IMenuCanvasItem) .Width;
        //        int i = 0;
        //        foreach (var price in Prices)
        //        {
        //            price.XPos = (multiPriceHeading.PriceHeadings[i] as IMenuCanvasItem).XPos;

        //            var priceSize = price.Font.MeasureText(price.Description);
        //            price.Width = priceSize.Width;
        //            price.Height = priceSize.Height;

        //            price.Visisble = true;
        //            price.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
        //            i++;
        //        }
        //        if (Prices.Count > 0)
        //            leftMostPrice = Prices[0];

        //        //if (priceStyle.PriceLeader == "dots")
        //        //    ExpandDotsToFillTheGap(priceStyle, Prices[0]);
        //        //else
        //        //    PriceLeader.XPos = Prices[0].XPos - (PriceLeader.Width + spaceWidth);
        //        //PriceLeader.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
        //    }
        //    else if (MainPrice != null)
        //    {
        //        leftMostPrice = MainPrice;
        //        var priceSize = MainPrice.Font.MeasureText(MainPrice.Description);
        //        MainPrice.Width = priceSize.Width;
        //        MainPrice.Height = priceSize.Height;
        //        MainPrice.Visisble = true;
        //        if (FoodItemAlignment == MenuStyles.Alignment.Center)
        //        {
        //            MainPrice.XPos = XPos + (Width / 2) - (priceSize.Width / 2);
        //        }
        //        else
        //        {


        //            MainPrice.XPos = XPos + Width - MainPrice.Width;
        //        }

        //        if (PriceAtFixedPosition || (SubTexts[PriceLineIndex.Value].Width + SubTexts[PriceLineIndex.Value].XPos) <= MainPrice.XPos)
        //            MainPrice.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
        //        else
        //        {
        //            //Moves price in next line when price overlaps 
        //            MainPrice.ResetSize();
        //            MenuStyles.ILayoutStyle layoutStyle = Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle;
        //            double lineSpacing = (layoutStyle.LineSpacing - 1) * 20;
        //            MainPrice.YPos = SubTexts[PriceLineIndex.Value].YPos + SubTexts[PriceLineIndex.Value].Height + lineSpacing;
        //        }

        //        MainPrice.Visisble = true;



        //        //if (priceStyle.PriceLeader == "dots")
        //        //    ExpandDotsToFillTheGap(priceStyle, MainPrice);
        //        //else if (PriceLeader != null)
        //        //    PriceLeader.XPos = MainPrice.XPos - (PriceLeader.Width + spaceWidth);

        //    }
        //    if (PriceLeader != null && leftMostPrice != null)
        //    {
        //        if (priceStyle.PriceLeader == "dots" && PriceLineIndex.Value == 0)
        //        {
        //            PriceLeader.XPos = SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width;
        //            ExpandDotsToFillTheGap(priceStyle, leftMostPrice);
        //        }
        //        else
        //            PriceLeader.XPos = leftMostPrice.XPos - (PriceLeader.Width + spaceWidth);
        //        PriceLeader.AlignOnBaseline(leftMostPrice);
        //    }
        //}


        /// <MetaDataID>{5e898c84-ccc5-4ca4-9ed7-39f06e893220}</MetaDataID>
        private void SetPricesFollowLineText(MenuStyles.IPriceStyle priceStyle, IItemMultiPriceHeading multiPriceHeading)
        {
            var spaceWidth = Font.MeasureText(" ").Width;
            MenuStyles.ILayoutStyle layoutStyle = Page.Style.Styles["layout"] as MenuStyles.ILayoutStyle;
            double lineSpacing = (layoutStyle.LineSpacing - 1) * 20;

            IMenuCanvasFoodItemPrice priceLeaderAlignedWithPrice = null;
            ResetPriceLeaderDescription();

            foreach (var itemPrice in Prices)
                itemPrice.Visisble = false;

            var priceAlignedWithSubtext = SubTexts[PriceLineIndex.Value];

            if (multiPriceHeading != null)
            {
                #region align price to price header in x axis
                int i = 0;
                foreach (var price in Prices)
                {
                    price.Visisble = true;
                    price.ResetSize();
                    price.XPos = (multiPriceHeading.PriceHeadings[i] as IMenuCanvasItem).XPos;
                    price.AlignOnBaseline(SubTexts[PriceLineIndex.Value]);
                    i++;
                }
                if (Prices.Count > 0)
                    priceLeaderAlignedWithPrice = Prices[0];
                #endregion
            }
            else if (MainPrice != null)
            {
                MainPrice.Visisble = true;
                priceLeaderAlignedWithPrice = MainPrice;
                MainPrice.ResetSize();
                if (FoodItemAlignment == MenuStyles.Alignment.Center)
                {
                    if (PriceAtFixedPosition || (priceAlignedWithSubtext.Width + FoodItemPricesWithLeaderWitdh) > Width)
                        MainPrice.XPos = XPos + (Width / 2) - (FoodItemPricesWithLeaderWitdh / 2) + GetPriceLeaderWidth(priceStyle);//price and leader at new line
                    else
                        MainPrice.XPos = priceAlignedWithSubtext.XPos + priceAlignedWithSubtext.Width + GetPriceLeaderWidth(priceStyle);
                }
                else
                    MainPrice.XPos = XPos + Width - MainPrice.Width;

                if (PriceAtFixedPosition || (priceAlignedWithSubtext.Width + FoodItemPricesWithLeaderWitdh) <= Width)
                    MainPrice.AlignOnBaseline(priceAlignedWithSubtext);
                else
                {
                    //Moves price in next line when price overlaps 
                    MainPrice.ResetSize();
                    MainPrice.YPos = priceAlignedWithSubtext.YPos + priceAlignedWithSubtext.Height + lineSpacing;
                }

                MainPrice.Visisble = true;

            }

            if (PriceLeader != null && priceLeaderAlignedWithPrice != null)
            {
                if (priceStyle.PriceLeader == "dots" && FoodItemAlignment == MenuStyles.Alignment.Left && FoodItemPriceLayout == MenuStyles.PriceLayout.Normal)
                {
                    PriceLeader.XPos = priceAlignedWithSubtext.XPos + priceAlignedWithSubtext.Width + spaceWidth;
                    ExpandDotsToFillTheGap(priceStyle, priceLeaderAlignedWithPrice);
                }
                else
                    PriceLeader.XPos = priceLeaderAlignedWithPrice.XPos - (PriceLeader.Width + spaceWidth);
                PriceLeader.AlignOnBaseline(priceLeaderAlignedWithPrice);
            }
        }


        /// <MetaDataID>{3d1dd4a8-ccca-4fe0-9a85-6cc777c72463}</MetaDataID>
        private void SetPricesWithLineText(MenuStyles.IPriceStyle priceStyle, IItemMultiPriceHeading multiPriceHeading)
        {
            foreach (var itemPrice in Prices)
                itemPrice.Visisble = false;
            MainPrice.Visisble = true;
            var spaceWidth = Font.MeasureText(" ").Width;
            bool priceInNewLine = false;
            var layoutStyle = (Page.Style.Styles["layout"] as MenuStyles.LayoutStyle);
            double lineSpacing = (layoutStyle.LineSpacing - 1) * 20;
            ResetPriceLeaderDescription();
            var subtextforPriceAlignment = SubTexts[PriceLineIndex.Value];
            if (PriceLeader != null)
            {

                #region Sets price leader position
                PriceLeader.XPos = subtextforPriceAlignment.XPos + subtextforPriceAlignment.Width + spaceWidth * (1 + priceStyle.DotsSpaceFromItem);
                if (PriceLeader.XPos + PriceLeader.Width > XPos + Width) //price leader is out of space 
                {
                    //price leader at new line
                    PriceLeader.YPos = SubTexts[PriceLineIndex.Value].YPos + SubTexts[PriceLineIndex.Value].Height + lineSpacing;
                    //gets x position in the new line and take into account the price text
                    PriceLeader.XPos = GetLineTextXpos(new Size(FoodItemPricesWithLeaderWitdh, PriceLeader.Height), FoodItemAlignment) + spaceWidth;
                }
                else
                {
                    //priceLeader align with subtext
                    PriceLeader.AlignOnBaseline(subtextforPriceAlignment);//price leader with text and price at newline
                    if (PriceLeader.XPos + PriceLeader.Width + FoodItemPricesWitdh > XPos + Width)
                        priceInNewLine = true;
                }
                #endregion
            }
            else
            {
                if (subtextforPriceAlignment.XPos + subtextforPriceAlignment.Width + spaceWidth + FoodItemPricesWitdh > XPos + Width)
                    priceInNewLine = true;
            }


            MainPrice.ResetSize();
            MainPrice.Visisble = true;
            if (priceInNewLine)
            {
                //price at new line
                MainPrice.XPos = GetLineTextXpos(new Size(FoodItemPricesWitdh, MainPrice.Height), FoodItemAlignment);// SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width + spaceWidth;
                MainPrice.YPos = subtextforPriceAlignment.YPos + subtextforPriceAlignment.Height + lineSpacing;
            }
            else
            {
                if (PriceLeader != null)
                {
                    //price align with PriceLeader
                    MainPrice.XPos = PriceLeader.XPos + PriceLeader.Width + ((1 + priceStyle.DotsSpaceFromPrice) * spaceWidth);
                    MainPrice.AlignOnBaseline(PriceLeader);
                }
                else
                {
                    //price align with subtext
                    MainPrice.XPos = subtextforPriceAlignment.XPos + subtextforPriceAlignment.Width + spaceWidth;
                    MainPrice.AlignOnBaseline(subtextforPriceAlignment);
                }
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
            PriceLeader.Description = ".";
            Double priceLeaderWidht = PriceLeader.Font.MeasureText(PriceLeader.Description + dotString).Width;
            do
            {
                PriceLeader.Description += dotString;
                priceLeaderWidht = PriceLeader.Font.MeasureText(PriceLeader.Description + dotString).Width;
            }
            while (PriceLeader.XPos + priceLeaderWidht + spaceWidth < itemPrice.XPos);

            //double html5PriceLeaderWidht = PriceLeader.Font.Html5MeasureText(PriceLeader.Description).Width;
            //if (priceLeaderWidht > html5PriceLeaderWidht)
            //{
            //    PriceLeader.Description = PriceLeader.Description.Substring(0, PriceLeader.Description.Length - 1);
            //    priceLeaderWidht = PriceLeader.Font.MeasureText(PriceLeader.Description + dotString).Width;
            //}


            //#region Correct PriceLeader position in middle of gap

            //if (ExtraDescriptionInPriceLine)
            //{
            //    double available = itemPrice.XPos - (SubTexts[PriceLineIndex.Value + 1].XPos + SubTexts[PriceLineIndex.Value + 1].Width + spaceWidth);
            //    PriceLeader.XPos += ( PriceLeader.Width- available) / 2;
            //}
            //else
            //{
            //    //double available = itemPrice.XPos - (SubTexts[PriceLineIndex.Value].XPos + SubTexts[PriceLineIndex.Value].Width);
            //    //PriceLeader.XPos += (available - PriceLeader.Width) / 2;
            //}

            //#endregion
        }

        void ResetPriceLeaderDescription()
        {
            var spaceWidth = Font.MeasureText(" ").Width;
            var priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;

            if (priceStyle.PriceLeader != null && priceStyle.PriceLeader != "none")
            {
                if (PriceLeader == null)
                {
                    PriceLeader = new MenuCanvasPriceLeader();
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this)?.CommitTransientObjectState(PriceLeader);
                }
                if (priceStyle.PriceLeader == "dots")
                    PriceLeader.Description = "...";
                else
                    PriceLeader.Description = priceStyle.PriceLeader;
                PriceLeader.ResetSize();
            }
            else
                PriceLeader = null;

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
                if (_Height == 0 && SubTexts.Count > 0)
                {
                    var firstSubText = SubTexts.OrderBy(x => x.YPos).FirstOrDefault();
                    var LastSubText = SubTexts.OrderByDescending(x => x.YPos).FirstOrDefault();
                    return LastSubText.YPos + LastSubText.Height - firstSubText.YPos;
                }
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

        bool PricesTextBuilt = false;
        /// <MetaDataID>{46453625-16f6-42e6-b256-067d06a0a7fe}</MetaDataID>
        int? _PriceLineIndex;
        int? PriceLineIndex
        {
            get => _PriceLineIndex;
            set
            {
                if (_PriceLineIndex != value)
                {
                    _PriceLineIndex = value;
                    PricesTextBuilt = false;
                }
                //foreach (var price in Prices)
                //    price.Visisble = false;
                //if (MainPrice != null)
                //    MainPrice.Visisble = false;
            }
        }
        private bool PriceAtFixedPosition
        {
            get
            {
                if (Page != null && Page.Style != null)
                {
                    MenuStyles.IPriceStyle priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                    if (FoodItemPriceLayout == MenuStyles.PriceLayout.Normal)
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
        }
        //double PriceLineTextWidth;

        //double PriceLineTextHeight;


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _XPos = new MultilingualMember<double>();


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
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
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
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _YPos = new MultilingualMember<double>();


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
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
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
        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuPageCanvas> _Page = new MultilingualMember<IMenuPageCanvas>();

        /// <MetaDataID>{f97dc905-cc15-4d5f-b07f-0f2222a28872}</MetaDataID>
        [PersistentMember(nameof(_Page))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+11")]

        public IMenuPageCanvas Page
        {
            get
            {
                return _Page.Value;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<IMenuCanvasPriceLeader> _PriceLeader = new OOAdvantech.MultilingualMember<IMenuCanvasPriceLeader>();
        /// <MetaDataID>{383742c6-8eb7-4abd-82fd-b9e1f2c0cc35}</MetaDataID>
        [PersistentMember(nameof(_PriceLeader))]
        [BackwardCompatibilityID("+4")]
        public MenuPresentationModel.MenuCanvas.IMenuCanvasPriceLeader PriceLeader
        {
            get
            {
                return _PriceLeader.Value;
            }
            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
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
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemPrice> _Prices = new OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemPrice>();

        /// <MetaDataID>{b4bdd387-b2c1-4244-a896-ceeb72ec23cd}</MetaDataID>
        [PersistentMember(nameof(_Prices))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [BackwardCompatibilityID("+5")]
        public List<IMenuCanvasFoodItemPrice> Prices
        {
            get
            {
                if (MenuItem != null)
                {
                    if (PricesRecalulation)
                    {
                        int i = 0;


                        {
                            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                            {

                                _MainPrice = null;
                                //if (_Prices.Count > 1)
                                //    _Prices.Clear();
                                i = 0;
                                foreach (MenuItemPrice menuPrice in MenuItem.Prices)
                                {
                                    MenuCanvasFoodItemPrice foodItemPrice = null;
                                    if (_Prices.Count <= i)
                                    {
                                        foodItemPrice = new MenuCanvasFoodItemPrice(menuPrice);
                                        var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                                        if (objectStorage != null)
                                            objectStorage.CommitTransientObjectState(foodItemPrice);
                                        AddFoodItemPrice(foodItemPrice);
                                    }
                                    else
                                        foodItemPrice = _Prices[i] as MenuCanvasFoodItemPrice;

                                    foodItemPrice.Price = menuPrice.Price;
                                    foodItemPrice.MenuItemPrice = menuPrice;
                                    if (_MainPrice == null)
                                    {
                                        _MainPrice = foodItemPrice;

                                        MenuStyles.IPriceStyle priceStyle = (MainPrice as MenuCanvasFoodItemPrice).Style;

                                        if (!PriceInvisible && FoodItemPriceLayout != MenuStyles.PriceLayout.DoNotDisplay)
                                            _MainPrice.Visisble = true;
                                    }

                                    if (menuPrice.IsDefaultPrice)//.ItemSelector != null && !menuPrice.ItemSelector.GetInitialFor(MenuItem).UncheckOption)
                                    {
                                        MenuStyles.IPriceStyle priceStyle = (MainPrice as MenuCanvasFoodItemPrice).Style;
                                        _MainPrice = foodItemPrice;
                                        if (!PriceInvisible || FoodItemPriceLayout != MenuStyles.PriceLayout.DoNotDisplay)
                                            _MainPrice.Visisble = true;
                                    }

                                    if (!_Prices.Contains(foodItemPrice))
                                        _Prices.Add(foodItemPrice);
                                    i++;
                                }
                                while (_Prices.Count > i)
                                    _Prices.RemoveAt(i);

                                stateTransition.Consistent = true;
                            }
                        }
                    }
                }

                // foreach()

                return _Prices.AsReadOnly().ToList();
            }
        }

        /// <MetaDataID>{4012bef9-99b2-4416-af45-c491994247ad}</MetaDataID>
        bool PricesRecalulation
        {
            get
            {

                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null && OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).IsReadonly)
                    return false;

                int it = 0;
                if (this.Description == "Margherita ")
                {
                    it++;
                }


                var mainPrice = _MainPrice;
                if (MenuItem != null)
                {

                    _MainPrice = null;
                    //if (_Prices.Count > 1)
                    //    _Prices.Clear();
                    int i = 0;
                    if (MenuItem.Prices.OfType<MenuItemPrice>().Where(x => x.ItemSelector != null).Count() > 0)
                    {
                        if (MenuItem.Prices.OfType<MenuItemPrice>().Where(x => x.ItemSelector == null).Count() > 0)
                        {
                            foreach (var menuItemPrice in MenuItem.Prices.OfType<MenuItemPrice>().Where(x => x.ItemSelector == null).ToList())
                                MenuItem.RemoveMenuItemPrice(menuItemPrice);

                            return true;
                        }
                    }
                    foreach (MenuItemPrice menuPrice in MenuItem.Prices)
                    {
                        MenuCanvasFoodItemPrice foodItemPrice = null;
                        if (_Prices.Count <= i)
                            return true;
                        else
                        {
                            foodItemPrice = (from price in _Prices.OfType<MenuCanvasFoodItemPrice>()
                                             where price.Price == menuPrice.Price
                                             select price).FirstOrDefault();

                            //foodItemPrice = _Prices[i] as MenuCanvasFoodItemPrice;

                            if (foodItemPrice == null)
                                return true;

                            if (foodItemPrice.Price != menuPrice.Price)
                                return true;

                            if (menuPrice.IsDefaultPrice)
                            {
                                _MainPrice = foodItemPrice;
                                // _MainPrice.Visisble = true;
                            }
                            else
                            if (menuPrice.ItemSelector == null)
                                return true;


                            if (!_Prices.Contains(foodItemPrice))
                                return true;
                            foodItemPrice.MenuItemPrice = menuPrice;
                        }
                        i++;
                    }
                    if (_Prices.Count > i)
                        return true;


                }

                return false;


            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Extras = new MultilingualMember<string>();
        /// <MetaDataID>{e3bb99be-7367-4452-91cb-91c50a83468b}</MetaDataID>
        [PersistentMember(nameof(_Extras))]
        [BackwardCompatibilityID("+6")]
        public string Extras
        {
            get
            {
                if (_Extras.Value == null && MenuItem != null)
                    return MenuItem.ExtrasDescription;
                return _Extras;
            }

            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_Extras != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _Extras.Value = value;
                            stateTransition.Consistent = true;
                        }
                        OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                        {
                            ObjectChangeState?.Invoke(this, nameof(Extras));
                        }));

                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _FullDescription = new MultilingualMember<string>();
        /// <MetaDataID>{03c2d938-1ee7-4261-99ba-a7368f9f2a45}</MetaDataID>
        [PersistentMember(nameof(_FullDescription))]
        [BackwardCompatibilityID("+20")]
        public string FullDescription
        {
            get
            {
                if (_FullDescription.Value == null && MenuItem != null)
                    return MenuItem.FullName;

                return _FullDescription;
            }

            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_FullDescription != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _FullDescription.Value = value;
                            stateTransition.Consistent = true;
                        }
                        OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                        {
                            ObjectChangeState?.Invoke(this, nameof(FullDescription));
                        }));

                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _ExtraDescription = new MultilingualMember<string>();
        /// <MetaDataID>{6b66ad2b-cd4f-49ba-b87d-9ff1631fe9a5}</MetaDataID>
        [PersistentMember(nameof(_ExtraDescription))]
        [BackwardCompatibilityID("+7")]
        public string ExtraDescription
        {
            get
            {
                if (_ExtraDescription.Value == null && MenuItem != null)
                    return MenuItem.Description;

                return _ExtraDescription;
            }

            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_ExtraDescription != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            if (string.IsNullOrWhiteSpace(value))
                                _ExtraDescription.Value = null;
                            else
                                _ExtraDescription.Value = value;

                            stateTransition.Consistent = true;
                        }
                        OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                        {


                            ObjectChangeState?.Invoke(this, nameof(ExtraDescription));
                        }));

                    }
                }

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
            if (point.X > CanvasFrameArea.X && point.X < CanvasFrameArea.X + CanvasFrameArea.Width)
            {
                if (point.Y < YPos)
                    return ItemRelativePos.Before;

                if (point.Y > YPos + CanvasFrameArea.Height)
                    return ItemRelativePos.After;

                if (point.Y >= YPos && point.Y <= YPos + Height)
                {
                    if (point.X >= CanvasFrameArea.X && point.X <= CanvasFrameArea.X + CanvasFrameArea.Width)
                    {
                        if (point.Y <= YPos + SubTexts[0].Height / 2)
                            return ItemRelativePos.OnPosUp;
                        else
                            return ItemRelativePos.OnPosDown;

                    }
                    if (point.X < CanvasFrameArea.X)
                        return ItemRelativePos.Before;
                }
                return ItemRelativePos.After;
            }

            //if (point.Y > CanvasFrameArea.Y)
            //    return ItemRelativePos.After;
            //else
            //    return ItemRelativePos.Before;

            if (point.Y > CanvasFrameArea.Y)
                return ItemRelativePos.After;
            else
                if (Column!=null)
            {
                if (point.X >= Column.XPos && point.X <= Column.XPos + Column.Width)
                    return ItemRelativePos.Before;
                else
                    return ItemRelativePos.After;
            }
            else
                return ItemRelativePos.Before;



            //if (point.X < CanvasFrameArea.X)
            //    return ItemRelativePos.Before;
            //else
            //{
            //    if (point.Y > CanvasFrameArea.Y)
            //        return ItemRelativePos.After;
            //    else
            //        return ItemRelativePos.Before;

            //}
        }

        /// <MetaDataID>{515744a8-3c3d-4d9f-8b13-697c7e7a52bd}</MetaDataID>
        public void OnCommitObjectState()
        {

            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            foreach (var price in this.Prices)
                objectStorage.CommitTransientObjectState(price);


        }


        /// <MetaDataID>{04c21baa-dd0a-4eee-90d4-20ff36e03d9f}</MetaDataID>
        public void OnActivate()
        {
            if (MenuItem is MenuModel.MenuItem)
                (MenuItem as MenuModel.MenuItem).ObjectChangeState += MenuCanvasFoodItem_ObjectChangeState;

        }

        /// <MetaDataID>{9e9d6f7e-21da-4e7c-bc2e-f1f6ac27dbf5}</MetaDataID>
        private void MenuCanvasFoodItem_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(MenuModel.MenuItem.Name))
                this.ObjectChangeState?.Invoke(this, nameof(Description));
            if (member == nameof(MenuModel.MenuItem.FullName))
                this.ObjectChangeState?.Invoke(this, nameof(FullDescription));
            if (member == nameof(MenuModel.MenuItem.ExtrasDescription))
                this.ObjectChangeState?.Invoke(this, nameof(Extras));

        }

        /// <MetaDataID>{ae3750ae-399a-45c9-afeb-9af70de7bf27}</MetaDataID>
        public void OnDeleting()
        {

        }

        /// <MetaDataID>{6c21a1ce-e75f-4b4c-b3a3-3f7044137204}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
            if (Description == "Diavolo" && associationEnd.Name == "Page")
            {
                var pages = (linkedObject as MenuPage).Menu.Pages.Where(x => x.MenuCanvasItems.Contains(this)).ToArray();
            }

            ObjectChangeState?.Invoke(this, nameof(Page));
        }

        /// <MetaDataID>{50194156-7ea3-4895-8c8c-6cc7c69bc701}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

            if (Description == "Diavolo" && associationEnd.Name == "Page")
            {
                var pages = (linkedObject as MenuPage).Menu.Pages.Where(x => x.MenuCanvasItems.Contains(this)).ToArray();
            }


            ObjectChangeState?.Invoke(this, nameof(Page));
        }

        /// <MetaDataID>{5a7513c7-e681-4552-abd1-7c7dfbd97998}</MetaDataID>
        public void Refresh()
        {
            ObjectChangeState?.Invoke(this, null);
        }

        private double FoodItemPricesWithLeaderWitdh
        {
            get => GetPriceLeaderWidth(PriceStyle) + FoodItemPricesWitdh;
        }
        private double PriceLeaderWidth
        {
            get => GetPriceLeaderWidth(PriceStyle);
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
                if (MultiPriceHeading != null)
                {
                    width = ((MultiPriceHeading.PriceHeadings[MultiPriceHeading.PriceHeadings.Count - 1] as IMenuCanvasItem).XPos + (MultiPriceHeading.PriceHeadings[MultiPriceHeading.PriceHeadings.Count - 1] as IMenuCanvasItem).Width)
                        - (MultiPriceHeading.PriceHeadings[0] as IMenuCanvasItem).XPos;



                }
                else
                {
                    foreach (var fooItemPrice in Prices)
                    {
                        if (fooItemPrice.Visisble)
                        {
                            if (spaceWidth == 0)
                                spaceWidth = fooItemPrice.Font.MeasureText(" ").Width;
                            else
                                width += spaceWidth;
                            fooItemPrice.ResetSize();
                            width += fooItemPrice.Font.MeasureText(fooItemPrice.Description).Width;
                        }

                    }
                    if (width == 0 && MainPrice.Visisble)
                    {
                        if (spaceWidth == 0)
                            spaceWidth = MainPrice.Font.MeasureText(" ").Width;
                        else
                            width += spaceWidth;
                        MainPrice.ResetSize();
                        width += MainPrice.Font.MeasureText(MainPrice.Description).Width;
                    }
                }
                //if (PriceLeader != null)
                //    width += PriceLeader.Font.MeasureText(PriceLeader.Description).Width;
                return width;
            }
        }

        private FontData FoodItemPriceFont
        {
            get
            {

                foreach (var fooItemPrice in Prices)
                {
                    if (fooItemPrice.Visisble)
                        return fooItemPrice.Font;
                }
                if (MainPrice != null)
                    return MainPrice.Font;
                if (PriceLeader != null)
                    return PriceLeader.Font;
                return Font;
            }
        }

        /// <MetaDataID>{851ce937-4a6f-4381-a9ba-d9266cfa1d6e}</MetaDataID>
        public Rect CanvasFrameArea
        {
            get
            {
                double x = Column.XPos - 10;
                double y = SubTexts[0].YPos - BeforeSpacing;
                double height = Height + AfterSpacing + BeforeSpacing;
                double width = Column.Width + 20;

                return new Rect(x, y, width, height);

            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuItem> _MenuItem = new Member<IMenuItem>();

        /// <MetaDataID>{a5209a7d-4d7f-413c-9996-88fbf7526601}</MetaDataID>
        [PersistentMember(nameof(_MenuItem))]
        [BackwardCompatibilityID("+12")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public MenuModel.IMenuItem MenuItem
        {
            get
            {
                return _MenuItem.Value;
            }

            set
            {
                if (_MenuItem != value)
                {
                    if (MenuItem is MenuModel.MenuItem)
                        (MenuItem as MenuModel.MenuItem).ObjectChangeState -= MenuCanvasFoodItem_ObjectChangeState;

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuItem.Value = value;
                        stateTransition.Consistent = true;
                    }
                    if (MenuItem is MenuModel.MenuItem)
                        (MenuItem as MenuModel.MenuItem).ObjectChangeState += MenuCanvasFoodItem_ObjectChangeState;

                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IItemMultiPriceHeading> _MultiPriceHeading = new Member<IItemMultiPriceHeading>();

        /// <MetaDataID>{66944cd2-9966-4f3b-b065-f2bb5140a468}</MetaDataID>
        [PersistentMember(nameof(_MultiPriceHeading))]
        [BackwardCompatibilityID("+13")]
        public IItemMultiPriceHeading MultiPriceHeading
        {
            get
            {
                return _MultiPriceHeading.Value;
            }
            set
            {
                if (_MultiPriceHeading != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MultiPriceHeading.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        Member<IMenuCanvasAccent> _MenuCanvasAccent = new Member<IMenuCanvasAccent>();

        /// <MetaDataID>{854dcab4-57f1-4e4d-9be3-637ffa429fcb}</MetaDataID>
        [PersistentMember(nameof(_MenuCanvasAccent))]
        public IMenuCanvasAccent MenuCanvasAccent
        {
            get => _MenuCanvasAccent.Value;
            set
            {

                if (_MenuCanvasAccent.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MenuCanvasAccent.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{34333426-c1f4-4fa5-a2e8-5416f6218b15}</MetaDataID>
        public void BeforeCommitObjectState()
        {
            if (_MenuCanvasAccent.Value != null)
            {
                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null && OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_MenuCanvasAccent.Value) == null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(_MenuCanvasAccent.Value);
            }
        }
        //[BeforeCommitObjectStateInStorageCall]
        //public void BeforeCommitObjectState()
        //{

        //}
    }
}


//<select name = "leader" style="width:78px" /// <MetaDataID>{98be9ed5-c0ca-4b8a-acc0-19f83f1b6f53}</MetaDataID>
//class="notranslate">
//                              <option value = "." selected="">Dots</option>
//                              <option value = "–" >–</option> <!-- ie warning use &#8211; instead -->
//                              <option value = "—" >—</option>
//                              <option value = "•" >•</option>
//									  <option value = ":" > : </option>
//                              <option value = "/" >⁄</option>
//									  <option value = "|" > | </ option >
//                              < option value="…">…</option> <!-- ie warning use &#8230; instead -->
//                              <option value = "none" > none </ option >
//                        </ select >
