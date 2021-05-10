using System;
using System.Collections.Generic;
using System.Linq;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{b88924fa-424f-40bf-875c-0706ae52cf4a}</MetaDataID>
    [BackwardCompatibilityID("{b88924fa-424f-40bf-875c-0706ae52cf4a}")]
    [Persistent()]
    public class FoodItemsGroupColumn : IFoodItemsGroupColumn
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{b1e4aa50-0724-410f-939f-c3075db0c924}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        public IItemMultiPriceHeading MultiPriceHeading;

        [DeleteObjectCall]
        void OnObjectDeleting()
        {

        }

        /// <MetaDataID>{c8398d9e-f13e-4be6-b21e-6d9195cd0a55}</MetaDataID>
        public FoodItemsGroupColumn()
        {

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IMenuCanvasFoodItemsGroup> _FoodItemGroup = new OOAdvantech.Member<IMenuCanvasFoodItemsGroup>();

        /// <MetaDataID>{492f5527-5f9a-458f-96a5-166aab7e0276}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember(nameof(_FoodItemGroup))]
        public MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItemsGroup FoodItemGroup
        {
            get
            {
                return _FoodItemGroup.Value;
            }
            set
            {

                if (_FoodItemGroup != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FoodItemGroup.Value = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
        }

        /// <exclude>Excluded</exclude>
        double _MaxHeight;
        /// <MetaDataID>{69df2857-79d5-4489-8b35-1f8952d1b51a}</MetaDataID>
        [PersistentMember(nameof(_MaxHeight))]
        [BackwardCompatibilityID("+8")]
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
        double _Width;
        /// <MetaDataID>{0db548b6-f324-488d-ba66-cda4ebb1135e}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+7")]
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
        double _XPos;
        /// <MetaDataID>{5da03ce5-013e-4550-894c-4aaf15039575}</MetaDataID>
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
                        _XPos = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double _YPos;
        /// <MetaDataID>{664c80de-384b-4397-90df-5aa22fa0f163}</MetaDataID>
        [PersistentMember(nameof(_YPos))]
        [BackwardCompatibilityID("+5")]
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


        /// <MetaDataID>{f9130938-6145-46a8-9f9d-09e6335bbf06}</MetaDataID>
        public bool DisplayMultiPriceHeadings
        {
            get
            {
                var priceStyle = Page.Style.Styles["price-options"] as MenuStyles.IPriceStyle;
                var menuItemStyle = Page.Style.Styles["menu-item"] as MenuStyles.IMenuItemStyle;

                if (priceStyle.ShowMultiplePrices && priceStyle.Layout == MenuStyles.PriceLayout.Normal && menuItemStyle.Alignment == MenuStyles.Alignment.Left)
                    return true;
                else if (priceStyle.ShowMultiplePrices && priceStyle.Layout == MenuStyles.PriceLayout.FollowDescription && menuItemStyle.Alignment == MenuStyles.Alignment.Left)
                    return true;
                return false;

            }
        }

        /// <exclude>Excluded</exclude> 
        OOAdvantech.Collections.Generic.MultilingualSet<IMenuCanvasItem> _MenuCanvasItems = new OOAdvantech.Collections.Generic.MultilingualSet<IMenuCanvasItem>();

        /// <MetaDataID>{57c3bfdf-2480-45da-9943-b2e6c5bfaab1}</MetaDataID>

        [BackwardCompatibilityID("+3")]
        [PersistentMember(nameof(_MenuCanvasItems))]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                return _MenuCanvasItems.ToThreadSafeList();
            }
        }


        /// <MetaDataID>{24f9afa2-2671-4a31-a61f-c646c5332d98}</MetaDataID>
        public void RenderMenuCanvasItems(IList<IMenuCanvasItem> menuCanvasItems, IList<IMenuCanvasItem> allItemMultiPriceHeadings)
        {
            ItemsMultiPriceHeading itemMultiPriceHeading = null;
            //List<ItemsMultiPriceHeading> usedItemsMultiPriceHeadings = new List<ItemsMultiPriceHeading>();
            double nextMenuCanvasItemY = YPos;//+= menuItemStyle.BeforeSpacing;

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Clear();
                stateTransition.Consistent = true;
            }


            IHighlightedMenuCanvasItem previousHighlightedMenuCanvasItem = null;
            while (menuCanvasItems.Count > 0)
            {
                var nextMenuCanvasItem = menuCanvasItems[0];
                menuCanvasItems.RemoveAt(0);

                var previousAssignedPage = nextMenuCanvasItem.Page;
                int previousAssignedPageItemIndex = 0;
                if (previousAssignedPage != Page)
                {
                    if (previousAssignedPage != null)
                    {
                        previousAssignedPageItemIndex = previousAssignedPage.MenuCanvasItems.IndexOf(nextMenuCanvasItem);
                        previousAssignedPage.RemoveMenuItem(nextMenuCanvasItem);
                    }
                    Page.AddMenuItem(nextMenuCanvasItem);
                }



                if (nextMenuCanvasItem is MenuCanvasFoodItem)
                {
                    MenuCanvasFoodItem foodItem = nextMenuCanvasItem as MenuCanvasFoodItem;
                    var pageStyle = (Page.Style.Styles["page"] as MenuStyles.PageStyle);

                    //Multiple Price Heading
                    if (DisplayMultiPriceHeadings && foodItem.MenuItem != null && foodItem.Prices.Count > 1 && foodItem.Prices[0].ItemSelection != null)
                    {

                        //Create new multiple price heading every time where the food item has multiple prices of item selection optionsGroup
                        //which is different from previous item or this item is the first in column

                        var itemSelectorOptionsGroup = foodItem.Prices[0].ItemSelection.OptionGroup as MenuModel.ItemSelectorOptionsGroup;
                        if (itemMultiPriceHeading == null || itemMultiPriceHeading.Source != itemSelectorOptionsGroup)
                        {

                            #region Creates Multi Price Heading 

                            if (foodItem.MultiPriceHeading != null && (foodItem.MultiPriceHeading.Column == this || foodItem.MultiPriceHeading.Column == null) &&
                                foodItem.MultiPriceHeading.Source == itemSelectorOptionsGroup &&
                                MenuCanvasItems.Contains(foodItem.MultiPriceHeading))
                            {
                                itemMultiPriceHeading = foodItem.MultiPriceHeading as ItemsMultiPriceHeading;
                                if (itemMultiPriceHeading.Column == null)
                                    this.AddMenuCanvasItem(itemMultiPriceHeading);

                            }
                            else
                            {
                                itemMultiPriceHeading = new ItemsMultiPriceHeading(itemSelectorOptionsGroup);
                                OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(nextMenuCanvasItem);
                                objectStorage.CommitTransientObjectState(itemMultiPriceHeading);
                                this.AddMenuCanvasItem(itemMultiPriceHeading);
                            }

                            #endregion

                            foodItem.MultiPriceHeading = itemMultiPriceHeading;

                            //get remaining food item with the same item selection Options group;
                            var remainingFoodtItems = menuCanvasItems.OfType<MenuCanvas.MenuCanvasFoodItem>().TakeWhile(x => x.Prices[0].ItemSelection != null && x.Prices[0].ItemSelection.OptionGroup == itemMultiPriceHeading.Source).ToList();

                            remainingFoodtItems.Insert(0, foodItem);


                            itemMultiPriceHeading.YPos = nextMenuCanvasItemY;
                            itemMultiPriceHeading.CalculatePricesArea(remainingFoodtItems, this);
                            nextMenuCanvasItemY = itemMultiPriceHeading.YPos + itemMultiPriceHeading.Height + itemMultiPriceHeading.PriceHeadingsBottomMargin;
                            nextMenuCanvasItemY = itemMultiPriceHeading.BaseLine + itemMultiPriceHeading.PriceHeadingsBottomMargin;
                        }
                        else
                        {
                            foodItem.MultiPriceHeading = itemMultiPriceHeading;
                            nextMenuCanvasItemY += foodItem.BeforeSpacing + pageStyle.LineSpacing;
                        }
                    }
                    else
                    {
                        itemMultiPriceHeading = null;
                        foodItem.MultiPriceHeading = null;
                        nextMenuCanvasItemY += foodItem.BeforeSpacing + pageStyle.LineSpacing;
                    }

                    foodItem.XPos = XPos;
                    foodItem.YPos = nextMenuCanvasItemY;
                    foodItem.Width = Width;
                    foodItem.MaxHeight = MaxHeight - (foodItem.YPos - YPos);
                    foodItem.RenderMenuCanvasItems(itemMultiPriceHeading);
                    nextMenuCanvasItemY = foodItem.YPos + foodItem.Height + foodItem.AfterSpacing;
                }
                if (MaxHeight < nextMenuCanvasItemY - YPos)
                {
                    menuCanvasItems.Insert(0, nextMenuCanvasItem);
                    Page.RemoveMenuItem(nextMenuCanvasItem);
                    if (previousAssignedPage != Page)
                    {
                        if (previousAssignedPage != null)
                            previousAssignedPage.InsertMenuItem(previousAssignedPageItemIndex, nextMenuCanvasItem);
                    }

                    if (nextMenuCanvasItem is MenuCanvasFoodItem)
                        (nextMenuCanvasItem as MenuCanvasFoodItem).MultiPriceHeading = null;

                    break;
                }
                else
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (nextMenuCanvasItem.Column != null)
                            nextMenuCanvasItem.Column.RemoveMenuCanvasItem(nextMenuCanvasItem);
                        _MenuCanvasItems.Add(nextMenuCanvasItem);
                        stateTransition.Consistent = true;
                    }

                    if (nextMenuCanvasItem is MenuCanvasFoodItem)
                    {
                        if ((nextMenuCanvasItem as MenuCanvasFoodItem).AccentType != null)
                        {
                            if (previousHighlightedMenuCanvasItem != null)
                            {
                                if (previousHighlightedMenuCanvasItem is MenuCanvasFoodItem &&
                                    (nextMenuCanvasItem as MenuCanvasFoodItem).AccentType.IsTheSameWith((previousHighlightedMenuCanvasItem as MenuCanvasFoodItem).AccentType)
                                     && (nextMenuCanvasItem as MenuCanvasFoodItem).AccentType.MultipleItemsAccent)
                                {
                                    (nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent = previousHighlightedMenuCanvasItem.MenuCanvasAccent;
                                }
                                else
                                {
                                    if ((nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent == null || !(nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent.Accent.IsTheSameWith((nextMenuCanvasItem as MenuCanvasFoodItem).AccentType))
                                        (nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent = new MenuCanvasAccent((nextMenuCanvasItem as MenuCanvasFoodItem).AccentType);
                                }
                            }
                            else //if ((nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent == null || !(nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent.Accent.IsTheSameWith((nextMenuCanvasItem as MenuCanvasFoodItem).AccentType))
                                (nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent = new MenuCanvasAccent((nextMenuCanvasItem as MenuCanvasFoodItem).AccentType);

                            previousHighlightedMenuCanvasItem = nextMenuCanvasItem as MenuCanvasFoodItem;
                        }
                        else
                        {
                            (nextMenuCanvasItem as MenuCanvasFoodItem).MenuCanvasAccent = null;
                            previousHighlightedMenuCanvasItem = null;
                        }
                    }
                    Height = nextMenuCanvasItemY - YPos;
                }
            }
        }

        /// <MetaDataID>{eff7e0f3-9118-4a62-87e0-4537a8ffe6e2}</MetaDataID>
        public IList<IMenuCanvasItem> GetDeepMenuCanvasItems()
        {
            return this.MenuCanvasItems;
        }

        /// <MetaDataID>{31d71450-5639-4606-9803-5756322b84e0}</MetaDataID>
        public void RemoveMenuCanvasItem(IMenuCanvasItem menuCanvasItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Remove(menuCanvasItem);

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{24cd1392-8397-4105-b610-17cbc05f6940}</MetaDataID>
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

        /// <MetaDataID>{99dd6f64-e2be-43fe-947c-17d8464389d2}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+4")]
        public double Height
        {
            get => _Height; 
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

        /// <MetaDataID>{02a6855e-cb81-43de-b4fa-4da2e5e030f4}</MetaDataID>
        public IMenuPageCanvas Page
        {
            get
            {
                return FoodItemGroup.PageColumn.Page;
            }
        }


    }
}