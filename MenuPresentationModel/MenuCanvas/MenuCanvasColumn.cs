using System;
using System.Windows;
using System.Windows.Media;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using MenuPresentationModel.MenuStyles;
using System.Collections.Generic;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{ba88bcfc-a3e4-47c5-bfee-8831fbd3a03e}</MetaDataID>
    [BackwardCompatibilityID("{ba88bcfc-a3e4-47c5-bfee-8831fbd3a03e}")]
    [Persistent()]
    public class MenuCanvasColumn : MarshalByRefObject, IMenuCanvasPageColumn
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemsGroup> _FoodItemsGroup = new OOAdvantech.Collections.Generic.Set<IMenuCanvasFoodItemsGroup>();

        /// <MetaDataID>{8b0944d5-d7c4-463c-b816-c2918295fe81}</MetaDataID>
        [PersistentMember(nameof(_FoodItemsGroup))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [BackwardCompatibilityID("+1")]
        public IList<IMenuCanvasFoodItemsGroup> FoodItemsGroup
        {
            get
            {
                return _FoodItemsGroup.AsReadOnly();
            }
        }
        
        [DeleteObjectCall]
        void OnObjectDeleting()
        {
            foreach(var foodItemsGroup in _FoodItemsGroup )
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(foodItemsGroup);
        }


        /// <exclude>Excluded</exclude> 
        OOAdvantech.Collections.Generic.Set<IMenuCanvasItem> _MenuCanvasItems = new OOAdvantech.Collections.Generic.Set<IMenuCanvasItem>();

        /// <MetaDataID>{a5d46ea5-e12f-4d59-8fcc-ff0abd97e5de}</MetaDataID>
        [ImplementationMember(nameof(_MenuCanvasItems))]
        public System.Collections.Generic.IList<IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                return _MenuCanvasItems.AsReadOnly();
            }
        }

        /// <MetaDataID>{c1b36fd7-aba8-4dfe-b09b-4bc7a26d5d28}</MetaDataID>
        public void RemoveMenuCanvasItem(IMenuCanvasItem menuCanvasItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Remove(menuCanvasItem);

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{2786fd9e-5e9c-4b7f-af23-8b60f340543e}</MetaDataID>
        public void AddMenuCanvasItem(IMenuCanvasItem menuCanvasItem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Add(menuCanvasItem);

                stateTransition.Consistent = true;
            }

        }

        /// <exclude>Excluded</exclude>
        double _Height;
        /// <MetaDataID>{ca82772a-3dbc-4e44-9ea5-65eb928d522b}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+7")]
        public double Height
        {
            get
            {
                return _Height;
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{d2bb546b-a1a3-4c99-a769-52fe62d7146e}</MetaDataID>
        public MenuCanvasColumn()
        {

        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuPageCanvas> _Page = new OOAdvantech.Member<IMenuPageCanvas>();
        /// <MetaDataID>{8d2392df-8562-4795-9ed5-adfa98ba84e1}</MetaDataID>
        [PersistentMember(nameof(_Page))]
        [BackwardCompatibilityID("+9")]
        public MenuPresentationModel.MenuCanvas.IMenuPageCanvas Page
        {
            get
            {
                return _Page.Value;
            }
            set
            {
                if (_Page.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Page.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _MaxHeight;
        /// <MetaDataID>{7d2825e5-4dc8-4dee-8e16-bea254e3a40c}</MetaDataID>
        [PersistentMember(nameof(_MaxHeight))]
        [BackwardCompatibilityID("+3")]
        public double MaxHeight
        {
            get => _MaxHeight;
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
        double _Width;
        /// <MetaDataID>{ace5c1f1-7d45-40d0-b58c-e8ca246b571e}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+4")]
        public double Width
        {
            get => _Width;
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
        double _YPos;
        /// <MetaDataID>{b2147d6d-5a14-47d6-85a7-88cbeb6bdfd1}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+5")]
        public double YPos
        {
            get => _YPos;
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
        double _XPos;
        /// <MetaDataID>{74b82075-e53b-4674-a110-905b9652bdcc}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+6")]
        public double XPos
        {
            get => _XPos;
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



        /// <MetaDataID>{f5987660-fdff-416e-af80-525b509a6caa}</MetaDataID>
        public void RenderMenuCanvasItems(System.Collections.Generic.IList<IMenuCanvasItem> menuCanvasItems, IMenuCanvasHeading foodItemsFormatHeading)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                try
                {
                    if (Page.Style != null)
                    {
                        var availableFoodItemsGroups = _FoodItemsGroup.ToList();

                        //foreach (var foodItemsGroup in _FoodItemsGroup)
                        //    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(foodItemsGroup);

                        _FoodItemsGroup.Clear();
                        _MenuCanvasItems.Clear();

                        var pageStyle = (Page.Style.Styles["page"] as MenuStyles.PageStyle);
                        var layoutStyle = (Page.Style.Styles["layout"] as MenuStyles.LayoutStyle);
                        double nextMenuCanvasItemY = YPos;
                        double columnWidth = Width;


                        int i = 0;

                        while (menuCanvasItems.Count > 0)
                        {
                            var nextMenuCanvasItem = menuCanvasItems[0];

                            if (nextMenuCanvasItem is ItemsMultiPriceHeading)
                                menuCanvasItems.RemoveAt(0);
                            if (nextMenuCanvasItem is IMenuCanvasHeading)
                            {
                                #region Render Heading

                                IMenuCanvasHeading menuCanvasHeading = nextMenuCanvasItem as IMenuCanvasHeading;
                                var previousAssignedPage = menuCanvasHeading.Page;
                                int previousAssignedPageItemIndex = 0;
                                if (previousAssignedPage != null)
                                {
                                    previousAssignedPageItemIndex = previousAssignedPage.MenuCanvasItems.IndexOf(menuCanvasHeading);
                                    previousAssignedPage.RemoveMenuItem(menuCanvasHeading);
                                }
                                var tt = menuCanvasHeading.Page;
                                if(tt!=null)
                                {

                                }
                                Page.AddMenuItem(menuCanvasHeading);

                                menuCanvasHeading.ResetSize();

                                nextMenuCanvasItemY += menuCanvasHeading.BeforeSpacing + pageStyle.LineSpacing;
                                if ((i > 0 && menuCanvasHeading.NextColumnOrPage) || nextMenuCanvasItemY + menuCanvasHeading.Style.AfterSpacing + menuCanvasHeading.Height > YPos + MaxHeight)   //((this.Page.Height - this.Page.Margin.MarginBottom)))
                                {
                                    Page.RemoveMenuItem(menuCanvasHeading);
                                    if (previousAssignedPage != null)
                                        previousAssignedPage.InsertMenuItem(previousAssignedPageItemIndex, menuCanvasHeading);

                                    break; //out of column space
                                }

                                //if (menuCanvasHeading.Page != Page)
                                //{
                                //    //remove from the old page
                                //    if (menuCanvasHeading.Page != null)
                                //        menuCanvasHeading.Page.RemoveMenuItem(menuCanvasHeading);
                                //    Page.AddMenuItem(menuCanvasHeading);
                                //}
                                menuCanvasItems.RemoveAt(0);

                                menuCanvasHeading.YPos = nextMenuCanvasItemY;
                                menuCanvasHeading.FullRowWidth = columnWidth;



                                #region Align horizontal
                                if (menuCanvasHeading.Alignment == MenuStyles.Alignment.Center)
                                    menuCanvasHeading.XPos = XPos + (columnWidth / 2) - (menuCanvasHeading.Width / 2);
                                if (menuCanvasHeading.Alignment == MenuStyles.Alignment.Left)
                                    menuCanvasHeading.XPos = XPos;
                                if (menuCanvasHeading.Alignment == MenuStyles.Alignment.Right)
                                    menuCanvasHeading.XPos = XPos + Width - menuCanvasHeading.Width;
                                #endregion

                                nextMenuCanvasItemY += menuCanvasHeading.AfterSpacing + menuCanvasHeading.Height;

                                _Height = (menuCanvasHeading.YPos + menuCanvasHeading.Height) - YPos;//Actual column Height

                                if (menuCanvasHeading.MenuCanvasAccent != null && menuCanvasHeading.MenuCanvasAccent.Accent.UnderlineImage)
                                {
                                    nextMenuCanvasItemY += MenuCanvasAccent.ToPixels(menuCanvasHeading, menuCanvasHeading.MenuCanvasAccent.Accent.MarginBottom) + menuCanvasHeading.MenuCanvasAccent.Accent.Height;
                                    _Height += MenuCanvasAccent.ToPixels(menuCanvasHeading, menuCanvasHeading.MenuCanvasAccent.Accent.MarginBottom) + menuCanvasHeading.MenuCanvasAccent.Accent.Height;
                                }





                                if (menuCanvasHeading.Column != null)
                                    menuCanvasHeading.Column.RemoveMenuCanvasItem(nextMenuCanvasItem);

                                _MenuCanvasItems.Add(menuCanvasHeading);

                                #endregion

                                if (menuCanvasHeading.HeadingType != HeadingType.SubHeading)
                                {
                                    // makes heading food items host area
                                    foodItemsFormatHeading = menuCanvasHeading;
                                    foodItemsFormatHeading.RemoveAllHostingAreas();

                                }

                            }
                            else if (nextMenuCanvasItem is IMenuCanvasFoodItem)
                            {
                                MenuCanvasFoodItemsGroup foodItemsGroop = null;
                                if(availableFoodItemsGroups.Count>0)
                                {
                                    foodItemsGroop = availableFoodItemsGroups[0] as MenuCanvasFoodItemsGroup;
                                    availableFoodItemsGroups.RemoveAt(0);
                                }

                                #region Gets  foodItemsGroop 

                                if (foodItemsFormatHeading != null)
                                {
                                    if(foodItemsGroop==null)
                                        foodItemsGroop = new MenuCanvas.MenuCanvasFoodItemsGroup(this);
                                    if (!(nextMenuCanvasItem as IMenuCanvasFoodItem).Span || foodItemsFormatHeading.NumberOfFoodColumns == 1)
                                        foodItemsGroop. ItemsGroupHeading = foodItemsFormatHeading;

                                    //if ((nextMenuCanvasItem as IMenuCanvasFoodItem).Span && foodItemsFormatHeading.NumberOfFoodColumns > 1)
                                    //    foodItemsGroop = new MenuCanvas.MenuCanvasFoodItemsGroup(this);
                                    //else
                                    //    foodItemsGroop = new MenuCanvas.MenuCanvasFoodItemsGroup(this) { ItemsGroupHeading = foodItemsFormatHeading };

                                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodItemsGroop);
                                    _FoodItemsGroup.Add(foodItemsGroop);
                                    foodItemsFormatHeading.AddHostingArea(foodItemsGroop);
                                    foodItemsGroop.Width = Width;
                                }
                                else
                                {
                                    foodItemsGroop = new MenuCanvas.MenuCanvasFoodItemsGroup(this);
                                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(foodItemsGroop);
                                    _FoodItemsGroup.Add(foodItemsGroop);
                                }
                                #endregion


                                //sets food items group area size and location 
                                foodItemsGroop.YPos = nextMenuCanvasItemY;
                                foodItemsGroop.XPos = XPos;
                                foodItemsGroop.Width = Width;
                                foodItemsGroop.MaxHeight = YPos + MaxHeight - foodItemsGroop.YPos;



                                System.Collections.Generic.List<IMenuCanvasItem> foodItemsGroupMenuCanvasItems = new System.Collections.Generic.List<IMenuCanvasItem>();

                                #region collects consecutive the food items
                                foreach (var menuCanvasItem in menuCanvasItems)
                                {

                                    if (menuCanvasItem is IMenuCanvasFoodItem)
                                    {
                                        if (foodItemsGroop.Columns.Count > 1 && (menuCanvasItem as IMenuCanvasFoodItem).Span)
                                            break;// multi columns food items group ends on span food item 

                                        if (foodItemsFormatHeading != null && foodItemsGroop.ItemsGroupHeading == null && !(menuCanvasItem as IMenuCanvasFoodItem).Span)
                                            break;//No more food items for inserted group area of the food items with span


                                        //removes food item from old food items group area 
                                        if ((menuCanvasItem as IMenuCanvasFoodItem).HostingArea != null)
                                            (menuCanvasItem as IMenuCanvasFoodItem).HostingArea.RemoveGroupedItem((menuCanvasItem as IMenuCanvasFoodItem));


                                        foodItemsGroupMenuCanvasItems.Add(menuCanvasItem);
                                    }
                                    else
                                        break;
                                }

                                foreach(var menuCanvasItem in foodItemsGroop.GroupedItems.OfType<IMenuCanvasFoodItem>().ToList())
                                {
                                    if ((menuCanvasItem as IMenuCanvasFoodItem).HostingArea != null)
                                        (menuCanvasItem as IMenuCanvasFoodItem).HostingArea.RemoveGroupedItem((menuCanvasItem as IMenuCanvasFoodItem));
                                }

                                #endregion

                                //renders grouped items that can fit in food items group area
                                foodItemsGroop.RenderMenuCanvasItems(foodItemsGroupMenuCanvasItems);

                                foreach (IMenuCanvasItem menuCanvasItem in foodItemsGroop.GroupedItems)
                                {
                                    //removes from main collection all menuCanvasItems witch fitted in food items group area
                                    if (menuCanvasItems.Contains(menuCanvasItem))
                                        menuCanvasItems.Remove(menuCanvasItem);

                                    //_MenuCanvasItems.Add(menuCanvasItem);
                                }

                                #region removed Code

                                //if(!foodItemsGroop.GroupedItems.Contains(nextMenuCanvasItem as IMenuCanvasFoodItem))
                                //{
                                //    menuCanvasItems.Insert(0, nextMenuCanvasItem);

                                //    Page.RemoveMenuItem(nextMenuCanvasItem);
                                //    if (previousAssignedPage != null)
                                //        previousAssignedPage.InsertMenuItem(0,nextMenuCanvasItem);

                                //    if (nextMenuCanvasItem is MenuCanvasFoodItem)
                                //        (nextMenuCanvasItem as MenuCanvasFoodItem).MultiPriceHeading = null;
                                //    foodItemsGroop.RemoveGroupedItem(nextMenuCanvasItem as IMenuCanvasFoodItem);
                                //    foodItemsGroop.RenderMenuCanvasItems(menuCanvasItems);
                                //    break;
                                //}
                                //_Height = (foodItemsGroop.YPos + nextMenuCanvasItem.Height) - YPos;
                                //_MenuCanvasItems.Add(nextMenuCanvasItem);
                                //while (menuCanvasItems.Count > 0 && menuCanvasItems[0] is IMenuCanvasFoodItem)
                                //{
                                //    nextMenuCanvasItem = menuCanvasItems[0];
                                //    menuCanvasItems.RemoveAt(0);
                                //    previousAssignedPage = nextMenuCanvasItem.Page;
                                //    if (previousAssignedPage != null)
                                //        previousAssignedPage.RemoveMenuItem(nextMenuCanvasItem);

                                //    Page.AddMenuItem(nextMenuCanvasItem);

                                //    if ((nextMenuCanvasItem as IMenuCanvasFoodItem).HostingArea != null)
                                //        (nextMenuCanvasItem as IMenuCanvasFoodItem).HostingArea.RemoveGroupedItem((nextMenuCanvasItem as IMenuCanvasFoodItem));

                                //    //foodItemsGroop.AddGroupedItem(nextMenuCanvasItem as IMenuCanvasFoodItem);
                                //    foodItemsGroop.RenderMenuCanvasItems(menuCanvasItems);
                                //    if ((foodItemsGroop.Height + foodItemsGroop.YPos) > (this.Page.Height - this.Page.Margin.MarginBottom))
                                //    {
                                //        menuCanvasItems.Insert(0, nextMenuCanvasItem);

                                //        Page.RemoveMenuItem(nextMenuCanvasItem);
                                //        if (previousAssignedPage != null)
                                //            previousAssignedPage.InsertMenuItem(0,nextMenuCanvasItem);

                                //        if (nextMenuCanvasItem is MenuCanvasFoodItem)
                                //            (nextMenuCanvasItem as MenuCanvasFoodItem).MultiPriceHeading = null;
                                //        foodItemsGroop.RemoveGroupedItem(nextMenuCanvasItem as IMenuCanvasFoodItem);
                                //        foodItemsGroop.RenderMenuCanvasItems(menuCanvasItems);

                                //        stateTransition.Consistent = true;
                                //        return;
                                //    }
                                //    _Height = (foodItemsGroop.YPos + nextMenuCanvasItem.Height) - YPos;
                                //    _MenuCanvasItems.Add(nextMenuCanvasItem);
                                //}
                                #endregion

                                _Height = (foodItemsGroop.YPos + foodItemsGroop.Height) - YPos;
                                if (foodItemsGroupMenuCanvasItems.Count > 0)
                                    break;// go to the next column / page , no room for remaining food items
                                else
                                    nextMenuCanvasItemY = foodItemsGroop.YPos + foodItemsGroop.Height;

                            }
                            i++;
                        }


                        foreach (var foodItemsGroup in availableFoodItemsGroups)
                            OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(foodItemsGroup);
                    }
                }
                catch (Exception error)
                {

                    throw;
                }
                stateTransition.Consistent = true;
            }

        }
        struct ColumnsPair
        {
            internal double Bottom
            {
                get
                {


                    double bottommostPos = LeftColumn.YPos + LeftColumn.Height;


                    foreach (IMenuCanvasItem menuCanvasItem in RightColumn.GetDeepMenuCanvasItems())
                    {
                        if (menuCanvasItem is IMenuCanvasFoodItem)
                        {
                            foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                            {
                                if (subText.YPos + subText.BaseLine > bottommostPos)
                                    bottommostPos = subText.YPos + (subText.Height / 2);
                            }
                        }
                        else
                        {
                            if (menuCanvasItem.YPos > bottommostPos)
                                bottommostPos = menuCanvasItem.YPos + (menuCanvasItem.Height / 2); ;
                        }
                    }

                    foreach (IMenuCanvasItem menuCanvasItem in LeftColumn.GetDeepMenuCanvasItems())
                    {
                        if (menuCanvasItem is IMenuCanvasFoodItem)
                        {
                            foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                            {
                                if (subText.YPos + subText.BaseLine > bottommostPos)
                                    bottommostPos = subText.YPos + (subText.Height / 2);
                            }
                        }
                        else
                        {
                            if (menuCanvasItem.YPos > bottommostPos)
                                bottommostPos = menuCanvasItem.YPos + (menuCanvasItem.Height / 2); ;
                        }
                    }

                    return bottommostPos;

                    //if (LeftColumn.YPos + LeftColumn.Height > RightColumn.YPos + RightColumn.Height)
                    //    return LeftColumn.YPos + LeftColumn.Height;
                    //else
                    //    return RightColumn.YPos + RightColumn.Height;
                }
            }

            internal double Top
            {
                get
                {

                    double topmostPos = LeftColumn.YPos + LeftColumn.Height;


                    foreach (IMenuCanvasItem menuCanvasItem in RightColumn.GetDeepMenuCanvasItems())
                    {
                        if (menuCanvasItem is IMenuCanvasFoodItem)
                        {
                            foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                            {
                                if (subText.YPos + subText.BaseLine < topmostPos)
                                    topmostPos = subText.YPos + (subText.Height / 2);
                            }
                        }
                        else
                        {
                            if (menuCanvasItem.YPos < topmostPos)
                                topmostPos = menuCanvasItem.YPos + (menuCanvasItem.Height / 2); ;
                        }
                    }

                    foreach (IMenuCanvasItem menuCanvasItem in LeftColumn.GetDeepMenuCanvasItems())
                    {
                        if (menuCanvasItem is IMenuCanvasFoodItem)
                        {
                            foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                            {
                                if (subText.YPos + subText.BaseLine < topmostPos)
                                    topmostPos = subText.YPos + (subText.Height / 2);
                            }
                        }
                        else
                        {
                            if (menuCanvasItem.YPos < topmostPos)
                                topmostPos = menuCanvasItem.YPos + (menuCanvasItem.Height / 2); ;
                        }
                    }

                    return topmostPos;

                    //if (LeftColumn.YPos > RightColumn.YPos )
                    //    return LeftColumn.YPos ;
                    //else
                    //    return RightColumn.YPos;
                }
            }

            public IMenuCanvasColumn LeftColumn;
            public IMenuCanvasColumn RightColumn;

            public double MidlinePos
            {
                get
                {
                    double leftColumnRightmostPos = 0;
                    double rightColumnLeftmostPos = 0;
                    LayoutStyle layoutStyle = null;
                    if (LeftColumn.GetDeepMenuCanvasItems().Count > 0)
                        layoutStyle = LeftColumn.GetDeepMenuCanvasItems()[0].Page.Style.Styles["layout"] as LayoutStyle;
                    else if (RightColumn.GetDeepMenuCanvasItems().Count > 0)
                        layoutStyle = RightColumn.GetDeepMenuCanvasItems()[0].Page.Style.Styles["layout"] as LayoutStyle;
                    else
                        return LeftColumn.XPos;

                    return RightColumn.XPos - (layoutStyle.SpaceBetweenColumns / 2);

                    //foreach (IMenuCanvasItem menuCanvasItem in LeftColumn.MenuCanvasItems)
                    //{
                    //    if (menuCanvasItem is IMenuCanvasFoodItem)
                    //    {

                    //        foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                    //        {
                    //            if (subText.XPos + subText.Width > leftColumnRightmostPos)
                    //                leftColumnRightmostPos = subText.XPos + subText.Width;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (menuCanvasItem.XPos + menuCanvasItem.Width > leftColumnRightmostPos)
                    //            leftColumnRightmostPos = menuCanvasItem.XPos + menuCanvasItem.Width;
                    //    }
                    //}


                    //foreach (IMenuCanvasItem menuCanvasItem in RightColumn.MenuCanvasItems)
                    //{
                    //    if (menuCanvasItem is IMenuCanvasFoodItem)
                    //    {
                    //        foreach (var subText in (menuCanvasItem as IMenuCanvasFoodItem).SubTexts)
                    //        {
                    //            if (subText.XPos > rightColumnLeftmostPos)
                    //                rightColumnLeftmostPos = subText.XPos;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (menuCanvasItem.XPos > rightColumnLeftmostPos)
                    //            rightColumnLeftmostPos = menuCanvasItem.XPos;
                    //    }
                    //}




                    //return leftColumnRightmostPos + ((rightColumnLeftmostPos - leftColumnRightmostPos) / 2);
                }
            }
        }

        /// <MetaDataID>{678e20fd-3e85-4b57-8921-28405f6ff3ea}</MetaDataID>
        internal static System.Collections.Generic.List<IMenuCanvasLine> RenderSeparationLines(LayoutStyle layoutStyle, System.Collections.Generic.IList<IMenuCanvasColumn> columns)
        {
            System.Collections.Generic.List<ColumnsPair> columnsPairs = new System.Collections.Generic.List<ColumnsPair>();

            System.Collections.Generic.List<IMenuCanvasLine> separationLines = new System.Collections.Generic.List<IMenuCanvasLine>();

            for (int i = 1; i < columns.Count; i++)
            {
                var column = columns[i];
                columnsPairs.Add(new ColumnsPair() { LeftColumn = columns[i - 1], RightColumn = columns[i] });
            }

            foreach (var columnPair in columnsPairs)
            {
                //double xp = 0;
                //double yp1 = -1;
                //double yp2 = 0;
                //x = columnPair.MidlinePos;
                //yp1 = columnPair.Top;
                //yp2 = columnPair.Bottom;
                double strockThickness = layoutStyle.SeparationLineThickness;
                if (strockThickness == 0)
                    strockThickness = 2;
                separationLines.Add(new MenuCanvasLine(columnPair.MidlinePos, columnPair.Top, columnPair.MidlinePos, columnPair.Bottom, layoutStyle.SeparationLineColor, strockThickness, layoutStyle.SeparationLineType));

            }


            //double y1 = -1;
            //double y2 = 0;


            //foreach (var column in columns)
            //{
            //    if (column.MenuCanvasItems.Count > 0)
            //    {
            //        //IMenuCanvasItem v = column.GetTopLeftItem();
            //        //v = column.GetLefMosttItem()
            //        var firstItem = column.MenuCanvasItems[0];
            //        var lastItem = column.MenuCanvasItems[column.MenuCanvasItems.Count - 1];
            //        if (y1 == -1)
            //        {
            //            y1 = firstItem.YPos + (lastItem.Height / 2);
            //        }
            //        else if (y1 > firstItem.YPos + (lastItem.Height / 2))
            //        {
            //            y1 = firstItem.YPos + (lastItem.Height / 2);
            //        }

            //        if (lastItem.YPos + lastItem.Height > y2)
            //            y2 = lastItem.YPos + lastItem.Height;
            //    }

            //}
            //if (y1 > 0)
            //{
            //    for (int i = 1; i < columns.Count; i++)
            //    {
            //        var column = columns[i];
            //       // x = column.XPos - (layoutStyle.SpaceBetweenColumns / 2);
            //        //if (separationLines.Count > 0)
            //        //{
            //        //    IMenuCanvasLine line = separationLines[0];
            //        //    separationLines.RemoveAt(0);
            //        //    line.X1 = x;
            //        //    line.X2 = x;
            //        //    line.Y1 = y1;
            //        //    line.Y2 = y2;
            //        //    line.Stroke = layoutStyle.SeparationLineColor;
            //        //    double strockThickness = layoutStyle.SeparationLineThickness;
            //        //    if (strockThickness == 0)
            //        //        strockThickness = 2;
            //        //    line.StrokeThickness = strockThickness;
            //        //    line.LineType = layoutStyle.SeparationLineType;
            //        //}
            //        //else
            //        {
            //            double strockThickness = layoutStyle.SeparationLineThickness;
            //            if (strockThickness == 0)
            //                strockThickness = 2;
            //            //separationLines.Add(new MenuCanvasLine(x, y1, x, y2, layoutStyle.SeparationLineColor, strockThickness, layoutStyle.SeparationLineType));
            //        }
            //    }
            //}
            foreach (var column in columns.OfType<MenuCanvasColumn>())
            {
                foreach (var foodItemsGroup in column.FoodItemsGroup)
                {
                    separationLines.AddRange(foodItemsGroup.SeparationLines);
                }

            }


            return separationLines;
        }

        /// <MetaDataID>{10144280-61c7-452a-a5ac-166b464c1edc}</MetaDataID>
        public IList<IMenuCanvasItem> GetDeepMenuCanvasItems()
        {
            List<IMenuCanvasItem> menuCanvasItems = new List<IMenuCanvasItem>();
            menuCanvasItems.AddRange(_MenuCanvasItems);
            menuCanvasItems.AddRange((from foodItemsGroup in this._FoodItemsGroup
                                      from column in foodItemsGroup.Columns
                                      from menuCanvasItem in column.MenuCanvasItems
                                      select menuCanvasItem));
            return menuCanvasItems;
        }


        ///// <MetaDataID>{e75559b1-c248-4a15-881f-e3ba85d59783}</MetaDataID>
        //public static Size MeasureText(string text, FontData font)
        //{
        //    if (font.AllCaps)
        //        text = text.ToUpper();
        //    FontFamily fontFamily = FontData.FontFamilies[font.FontFamilyName];
        //    FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(font.FontStyle);
        //    FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(font.FontWeight);
        //    FontStretch fontStretch = FontStretches.Normal;
        //    double fontSize = font.FontSize;
        //    Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
        //    GlyphTypeface glyphTypeface;

        //    if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
        //    {
        //        return MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
        //    }

        //    double totalWidth = 0;
        //    double height = 0;

        //    for (int n = 0; n < text.Length; n++)
        //    {
        //        ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

        //        double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

        //        double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

        //        if (glyphHeight > height)
        //        {
        //            height = glyphHeight;
        //        }

        //        totalWidth = totalWidth + width + font.FontSpacing;
        //    }
        //    totalWidth -= font.FontSpacing;
        //    return new Size(totalWidth, height);
        //}
        ///// <MetaDataID>{8f4b31ad-41b8-405f-b4c3-d34b08d1e7dc}</MetaDataID>
        //public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        //{
        //    FormattedText ft = new FormattedText(text,
        //                                       System.Globalization.CultureInfo.CurrentCulture,
        //                                         FlowDirection.LeftToRight,
        //                                         new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
        //                                         fontSize,
        //                                         Brushes.Black);
        //    return new Size(ft.Width, ft.Height);
        //}
    }
}