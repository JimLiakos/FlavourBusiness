using System;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using MenuPresentationModel.MenuCanvas;
using OOAdvantech;
using System.Windows;
using UIBaseEx;
using System.Collections.Generic;

namespace MenuPresentationModel
{
    /// <MetaDataID>{718f41a2-9bce-4c18-a03a-9c62d123341b}</MetaDataID>
    [BackwardCompatibilityID("{718f41a2-9bce-4c18-a03a-9c62d123341b}")]
    [Persistent()]
    public class MenuPage : MenuCanvas.IMenuPageCanvas, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMenuCanvasLine> _SeparationLines = new OOAdvantech.Collections.Generic.Set<IMenuCanvasLine>();
        /// <MetaDataID>{524aee7a-152f-49be-9d5f-66d185e2d3b3}</MetaDataID>
        [PersistentMember(nameof(_SeparationLines))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [BackwardCompatibilityID("+4")]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasLine> SeparationLines
        {
            get
            {
                return _SeparationLines.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<IMenuCanvasLine> _TranslationLines = new System.Collections.Generic.List<IMenuCanvasLine>();

        /// <MetaDataID>{fac1ba59-afff-4634-9d4a-37bd08265792}</MetaDataID>
        public System.Collections.Generic.IList<IMenuCanvasLine> TranslationLines
        {
            get
            {
                return _TranslationLines.AsReadOnly();
            }
        }


        /// <MetaDataID>{5e1f2742-3104-4cd6-ab95-bfa3a2d99e8a}</MetaDataID>
        public MenuPage()
        {

        }
        /// <exclude>Excluded</exclude>
        int? _NumberofColumns;
        /// <MetaDataID>{6a84e284-1bca-4196-a97f-0b8e7fbdc7f1}</MetaDataID>
        [PersistentMember(nameof(_NumberofColumns))]
        [BackwardCompatibilityID("+3")]
        public int NumberofColumns
        {
            get
            {
                var pageStyle = (Style.Styles["page"] as MenuStyles.PageStyle);

                if (!_NumberofColumns.HasValue)
                    return pageStyle.NumOfPageColumns;
                if (!_NumberofColumns.HasValue)
                    return 1;

                return _NumberofColumns.Value;
            }

            set
            {
                if (_NumberofColumns > 0)
                {
                    if (_NumberofColumns != value)
                    {

                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _NumberofColumns = value;
                            stateTransition.Consistent = true;
                        }

                    }
                }
            }

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.MultilingualSet<MenuCanvas.IMenuCanvasItem> _MenuCanvasItems = new OOAdvantech.Collections.Generic.MultilingualSet<MenuCanvas.IMenuCanvasItem>();
        //OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasItem> _MenuCanvasItems = new OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasItem>();

        /// <MetaDataID>{a5404bb4-bc8d-44ef-b51a-ef67d41551ef}</MetaDataID>
        [PersistentMember(nameof(_MenuCanvasItems))]
        [BackwardCompatibilityID("+2")]
        public IList<IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                return _MenuCanvasItems.AsReadOnly();
            }
        }





        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasPageColumn> _Columns = new OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasPageColumn>();

        /// <MetaDataID>{2d055cd5-70cd-4f7f-a166-a66a56c0af84}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [PersistentMember(nameof(_Columns))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]

        public IList<IMenuCanvasPageColumn> Columns
        {
            get
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    while (_Columns.Count > NumberofColumns)
                        _Columns.RemoveAt(_Columns.Count - 1);

                    while (_Columns.Count < NumberofColumns)
                    {
                        MenuCanvas.MenuCanvasColumn column = new MenuCanvas.MenuCanvasColumn();

                        var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                        if (objectStorage != null)
                            objectStorage.CommitTransientObjectState(column);

                        _Columns.Add(column);
                    }
                    stateTransition.Consistent = true;
                }


                return _Columns.AsReadOnly();
            }
        }

        /// <MetaDataID>{50478184-c3b2-406e-b02b-c9ca0326ac67}</MetaDataID>
        public void MoveMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasItem, int newpos)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var tmp = _MenuCanvasItems.ToArray();

                _MenuCanvasItems.Remove(manuCanvasItem);
                tmp = _MenuCanvasItems.ToArray();

                _MenuCanvasItems.Insert(newpos, manuCanvasItem);
                stateTransition.Consistent = true;

            }
        }
        /// <MetaDataID>{c7166fd0-2de9-44d3-aa66-37e8cd395eba}</MetaDataID>
        public void InsertMenuItemAfter(MenuCanvas.IMenuCanvasItem manuCanvasitem, MenuCanvas.IMenuCanvasItem newManuCanvasitem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                int pos = _MenuCanvasItems.IndexOf(manuCanvasitem);
                _MenuCanvasItems.Insert(pos, newManuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;
        }

        /// <MetaDataID>{e87fa1f8-0ec1-4d55-b537-4a33592e836a}</MetaDataID>
        public void InsertMenuItem(int pos, MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {

            if (manuCanvasitem != null)
            {
                string name = "insert " + manuCanvasitem.Description;
                int npos = this.Menu.Pages.IndexOf(this);
            }


            if (manuCanvasitem != null && manuCanvasitem.Description == "Diavolo")
            {

                int npos = this.Menu.Pages.IndexOf(this);
            }
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Insert(pos, manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;
        }

        /// <MetaDataID>{05793a14-6b4f-4f5d-b725-3a9f2369ead9}</MetaDataID>
        private void ManuCanvasitem_ObjectChangeState(object _object, string member)
        {

            //System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{


            //if (member == nameof(IMenuCanvasFoodItem.Description) ||
            //    member == nameof(IMenuCanvasFoodItem.ExtraDescription) ||
            //    member == nameof(IMenuCanvasFoodItem.Extras) ||
            //    member == nameof(IMenuCanvasHeading.Accent) ||
            //    member == nameof(IMenuCanvasHeading.NextColumnOrPage))
            //{
            //    if (ObjectChangeState != null)
            //    {
            //        var menuCanvasItems = MenuCanvasItems.ToList();
            //        RenderMenuCanvasItems(menuCanvasItems);
            //        ObjectChangeState?.Invoke(this, nameof(MenuCanvasItems));
            //    }
            //}
            //// }));
        }

        /// <MetaDataID>{eff91be1-f837-4004-ae68-7e2556cbe184}</MetaDataID>
        public void RemoveMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {
            if (manuCanvasitem != null)
            {
                string name = "remove " + manuCanvasitem.Description;
                int npos = this.Menu.Pages.IndexOf(this);
            }

            if (manuCanvasitem != null && manuCanvasitem.Description == "Diavolo")
            {
                int npos = this.Menu.Pages.IndexOf(this);

            }

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                //if(manuCanvasitem is MenuCanvas.ItemsMultiPriceHeading)
                //    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(manuCanvasitem);

                _MenuCanvasItems.Remove(manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState -= ManuCanvasitem_ObjectChangeState;
        }

        /// <MetaDataID>{26f82366-dba8-494c-ae7f-d2548c3349e3}</MetaDataID>
        public void AddMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {
            if (manuCanvasitem != null && manuCanvasitem.Description == "Diavolo")
            {
                int npos = this.Menu.Pages.IndexOf(this);

            }
            if (this.Menu != null && manuCanvasitem != null)
            {
                string name = "add " + manuCanvasitem.Description;
                int npos = this.Menu.Pages.IndexOf(this);
            }


            if (manuCanvasitem is IItemMultiPriceHeading)
            {

            }
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Add(manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;

        }

        /// <exclude>Excluded</exclude>
        ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<MenuPresentationModel.PresentationItem> _PresentationItems = new OOAdvantech.Collections.Generic.Set<MenuPresentationModel.PresentationItem>();

        [Association("PagePresentationItem", Roles.RoleA, true, "76ffd4e1-8d50-4503-a24c-113cb653b584")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [PersistentMember(nameof(_PresentationItems))]
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        public OOAdvantech.Collections.Generic.Set<MenuPresentationModel.PresentationItem> PresentationItems
        {
            get
            {
                return _PresentationItems.AsReadOnly();
            }
        }

        /// <MetaDataID>{08077765-42a9-4df6-87f0-9a4d0222671e}</MetaDataID>
        public void AddPresentationItem(MenuPresentationModel.PresentationItem presentationItem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PresentationItems.Add(presentationItem);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{5b6dd64d-f22c-4897-9187-e062e438cf82}</MetaDataID>
        public int ItemsCount
        {
            get
            {
                return _PresentationItems.Count;
            }
        }

        /// <MetaDataID>{2775c15a-8317-4575-95b2-3e9a44b9d4fb}</MetaDataID>
        public void MovePresentationItem(int newPos, PresentationItem presentationItem)
        {
            if (newPos == _PresentationItems.IndexOf(presentationItem))
                return;

            if (newPos > _PresentationItems.Count - 2)
            {
                /// move page to end.
                RemovePresentationItem(presentationItem);
                AddPresentationItem(presentationItem);
            }
            else
            {
                RemovePresentationItem(presentationItem);
                InsertPresentationItem(newPos, presentationItem);
            }
        }

        /// <MetaDataID>{18ecc086-f66d-4a49-8271-8e5839c215bd}</MetaDataID>
        public void InsertPresentationItem(int newPos, PresentationItem presentationItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PresentationItems.Insert(newPos, presentationItem);

                int index = _PresentationItems.IndexOf(presentationItem);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{b1fcbad1-3a3c-484e-8d7b-caaa005a5b70}</MetaDataID>
        public void RemovePresentationItem(PresentationItem presentationItem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PresentationItems.Remove(presentationItem);
                stateTransition.Consistent = true;
            }
        }


        /// <exclude>Excluded</exclude>
        MultilingualMember<IRestaurantMenu> _Menu = new MultilingualMember<IRestaurantMenu>();

        public event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{2b41d8a2-4c05-442c-8fa9-ef42b4504f2a}</MetaDataID>
        [PersistentMember(nameof(_Menu))]
        [BackwardCompatibilityID("+5")]
        public MenuPresentationModel.MenuCanvas.IRestaurantMenu Menu
        {
            get
            {
                return _Menu.Value;
            }
        }

        /// <summary>
        ///  Gets the (zero-based) position of the Page in the MenuPresentationModel.RestaurantMenu.Pages
        ///  collection. 
        ///  Gets -1 if the Page is not a member of a collection
        /// </summary>
        /// <MetaDataID>{403c5845-be03-46b1-9283-0e7ffecd5397}</MetaDataID>
        public int Ordinal
        {

            get
            {
                return Menu.Pages.IndexOf(this);
            }
        }


        /// <MetaDataID>{72d24578-0b38-479d-a450-3880e9e7b38c}</MetaDataID>
        public IStyleSheet Style
        {
            get
            {
                if (Menu is RestaurantMenu)
                    return (Menu as RestaurantMenu).Style;
                else
                    return null;
            }
            //set
            //{
            //    if (_Style != value)
            //    {
            //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //        {
            //            _Style = value;
            //            stateTransition.Consistent = true;
            //        }

            //       // RenderMenuCanvasItems();
            //    }
            //}
        }

        /// <MetaDataID>{907fd8ba-f3a3-4387-aeab-c213c47a8373}</MetaDataID>
        public Margin Margin
        {
            get
            {
                if (Style != null && Style.Styles.ContainsKey("page"))
                {
                    var pageStyle = (Style.Styles["page"] as MenuStyles.PageStyle);
                    return pageStyle.Margin;
                }
                return new Margin() { MarginLeft = 0, MarginBottom = 0, MarginRight = 0, MarginTop = 0 };
            }
        }

        /// <MetaDataID>{d18db507-99cc-47de-8fb2-784c636cf1a9}</MetaDataID>
        public double Height
        {
            get
            {
                if (Style != null && Style.Styles.ContainsKey("page"))
                {
                    var pageStyle = (Style.Styles["page"] as MenuStyles.PageStyle);
                    return pageStyle.PageHeight;
                }
                return 800;
            }
        }

        /// <MetaDataID>{c2d5ca1a-01f1-40e7-be09-6ad0c5157c06}</MetaDataID>
        public double Width
        {
            get
            {
                if (Style != null && Style.Styles.ContainsKey("page"))
                {
                    var pageStyle = (Style.Styles["page"] as MenuStyles.PageStyle);
                    return pageStyle.PageWidth;
                }
                return 600;
            }
        }
        //double GetColumnRect


        /// <MetaDataID>{451bf5bf-b700-44d0-97ee-2cc94c9c24bd}</MetaDataID>
        public void BeforeCommitObjectState()
        {
        }

        /// <MetaDataID>{11ead16b-d7fa-44f2-9d6f-2e7d68a12e09}</MetaDataID>
        public void RenderMenuCanvasItems(System.Collections.Generic.IList<MenuCanvas.IMenuCanvasItem> menuCanvasItems)
        {
            IMenuCanvasHeading lastFoodItemsHeading = (Menu as RestaurantMenu).GetLastFoodItemsHeading(this);
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                try
                {
                    foreach (var item in MenuCanvasItems)
                        RemoveMenuItem(item);

                    var allMenuCanvasItems = menuCanvasItems.ToList();


                    var pageStyle = (Style.Styles["page"] as MenuStyles.PageStyle);
                    var layoutStyle = (Style.Styles["layout"] as MenuStyles.LayoutStyle);
                    double columnsWidth = pageStyle.PageWidth - pageStyle.Margin.MarginLeft - pageStyle.Margin.MarginRight - (layoutStyle.SpaceBetweenColumns * (Columns.Count - 1));

                    //var menuCanvasItems = MenuCanvasItems.ToList();
                    double nextMenuCanvasItemY = pageStyle.Margin.MarginTop;
                    double xPos = pageStyle.Margin.MarginLeft;
                    MenuCanvas.IMenuCanvasHeading menuCanvasHeading = lastFoodItemsHeading;

                    MenuCanvas.IMenuCanvasItem menuCanvasItem = menuCanvasItems.FirstOrDefault();
                    if (menuCanvasItem is IMenuCanvasHeading)
                        menuCanvasHeading = menuCanvasItem as IMenuCanvasHeading;

                    if (menuCanvasHeading != null && menuCanvasHeading.HeadingType == MenuCanvas.HeadingType.Title)
                    {
                        menuCanvasHeading = menuCanvasItems[0] as MenuCanvas.IMenuCanvasHeading;
                        menuCanvasHeading.ResetSize();
                        menuCanvasItems.RemoveAt(0);
                        AddMenuItem(menuCanvasHeading);
                        menuCanvasHeading.RemoveAllHostingAreas();
                        nextMenuCanvasItemY += menuCanvasHeading.BeforeSpacing + pageStyle.LineSpacing;
                        menuCanvasHeading.YPos = nextMenuCanvasItemY;
                        nextMenuCanvasItemY += menuCanvasHeading.AfterSpacing + menuCanvasHeading.Height;

                        if (menuCanvasHeading.MenuCanvasAccent != null)
                            menuCanvasHeading.FullRowWidth = columnsWidth;

                        if (menuCanvasHeading.Alignment == MenuStyles.Alignment.Center)
                            menuCanvasHeading.XPos = xPos + (columnsWidth / 2) - (menuCanvasHeading.Width / 2);
                        if (menuCanvasHeading.Alignment == MenuStyles.Alignment.Left)
                            menuCanvasHeading.XPos = xPos;
                        if (menuCanvasHeading.Alignment == MenuStyles.Alignment.Right)
                            menuCanvasHeading.XPos = xPos + columnsWidth - menuCanvasHeading.Width;
                    }
                    menuCanvasItem = menuCanvasItems.FirstOrDefault();

                    if (menuCanvasItem is IMenuCanvasHeading && (menuCanvasItem as IMenuCanvasHeading).NextColumnOrPage&& MenuCanvasItems.Count!=0)
                        return;

                    double nextColumnXpos = pageStyle.Margin.MarginLeft;
                    int i = 0;
                    System.Collections.Generic.List<double> columnsWidths = null;
                    if (Columns.Count > 1)
                    {
                        double uneven = pageStyle.ColumnsUneven;
                        double ratio = 150;
                        double columnWidth = columnsWidth / Columns.Count;
                        int columnsCount = NumberofColumns;
                        if (columnsCount > 3)
                            columnsCount = 3;
                        if (uneven < 50)
                        {
                            double narrow = columnWidth / (1 + ((50 - uneven) / ratio));
                            columnsWidths = new System.Collections.Generic.List<double> { narrow };
                            for (i = 0; i != columnsCount - 1; i++)
                                columnsWidths.Add((columnWidth * columnsCount - narrow) / (columnsCount - 1));
                        }
                        else
                        {
                            double narrow = columnWidth / (1 + ((uneven - 50) / ratio));
                            columnsWidths = new System.Collections.Generic.List<double>() { narrow };
                            for (i = 0; i != columnsCount - 1; i++)
                                columnsWidths.Insert(0, (columnWidth * columnsCount - narrow) / (columnsCount - 1));
                        }
                    }
                    else
                        columnsWidths = new System.Collections.Generic.List<double>() { (columnsWidth / Columns.Count) };

                    i = 0;
                    foreach (var column in Columns)
                    {
                        column.Width = columnsWidths[i++];// columnsWidth / Columns.Count;
                        column.YPos = nextMenuCanvasItemY;
                        column.XPos = nextColumnXpos;
                        column.MaxHeight = pageStyle.PageHeight - pageStyle.Margin.MarginBottom - column.YPos;
                        nextColumnXpos = nextColumnXpos + column.Width + layoutStyle.SpaceBetweenColumns;
                        if (menuCanvasItems.Count == 0)
                            continue;
                        column.RenderMenuCanvasItems(menuCanvasItems, menuCanvasHeading);

                    }

                    RenderSeparationLines();


                    var availableTranslationLines = _TranslationLines.ToList();
                    foreach (var foodItemsHeading in this.MenuCanvasItems.OfType<FoodItemsHeading>().Where(x => x.UnTranslated))
                        AddTranslationLine(foodItemsHeading, availableTranslationLines);

                    foreach (var foodItem in this.MenuCanvasItems.OfType<MenuCanvasFoodItem>())
                    {
                        foreach (var subText in foodItem.SubTexts.OfType<MenuCanvasFoodItemText>().Where(x => x.UnTranslated))
                            AddTranslationLine(subText, availableTranslationLines);
                    }

                    foreach (var line in availableTranslationLines)
                        _TranslationLines.Remove(line);
                }
                finally
                {
                    stateTransition.Consistent = true;
                }
            }

        }

        private void AddTranslationLine(IMenuCanvasItem menuCanvasItem, List<IMenuCanvasLine> availableTranslationLines)
        {
            var layoutStyle = (Style.Styles["layout"] as MenuStyles.LayoutStyle);
            double x1 = menuCanvasItem.XPos - 2;
            double x2 = menuCanvasItem.XPos + menuCanvasItem.Width;
            double y1 = menuCanvasItem.YPos + menuCanvasItem.BaseLine;
            double y2 = menuCanvasItem.YPos + menuCanvasItem.BaseLine;

            if (availableTranslationLines.Count > 0)
            {
                IMenuCanvasLine line = availableTranslationLines[0];
                availableTranslationLines.RemoveAt(0);
                line.X1 = x1;
                line.X2 = x2;
                line.Y1 = y1;
                line.Y2 = y2;
                line.Stroke = layoutStyle.SeparationLineColor;
                double strockThickness = layoutStyle.SeparationLineThickness;
                if (strockThickness == 0)
                    strockThickness = 2;
                line.StrokeThickness = strockThickness;
                line.LineType = layoutStyle.SeparationLineType;
            }
            else
            {
                double strockThickness = layoutStyle.SeparationLineThickness;
                if (strockThickness == 0)
                    strockThickness = 2;

                _TranslationLines.Add(new MenuCanvasLine(x1, y1, x2, y2, layoutStyle.SeparationLineColor, strockThickness, layoutStyle.SeparationLineType));
            }
        }

        /// <MetaDataID>{4d835798-b1a7-494c-b90e-e0981497ac2f}</MetaDataID>
        private void RenderSeparationLines()
        {
            var separationLines = _SeparationLines.ToList();
            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);

            var layoutStyle = (Style.Styles["layout"] as MenuStyles.LayoutStyle);
            if (layoutStyle.LineBetweenColumns)
            {
                List<IMenuCanvasLine> separationLiness = MenuCanvasColumn.RenderSeparationLines(layoutStyle, _Columns.OfType<IMenuCanvasColumn>().ToList());
                foreach (var separationline in separationLiness)
                {
                    if (separationLines.Count > 0)
                    {
                        IMenuCanvasLine line = separationLines[0];
                        separationLines.RemoveAt(0);
                        line.X1 = separationline.X1;
                        line.X2 = separationline.X2;
                        line.Y1 = separationline.Y1;
                        line.Y2 = separationline.Y2;
                        line.Stroke = separationline.Stroke;
                        double strockThickness = layoutStyle.SeparationLineThickness;
                        line.StrokeThickness = separationline.StrokeThickness;
                        line.LineType = layoutStyle.SeparationLineType;
                    }
                    else
                    {
                        objectStorage.CommitTransientObjectState(separationline);

                        _SeparationLines.Add(separationline);
                    }
                }
            }


            foreach (var line in separationLines)
                _SeparationLines.Remove(line);
        }


        /// <MetaDataID>{1db34772-1be8-4864-9954-b2f69cd1da33}</MetaDataID>
        public void InsertCanvasItemTo(IMenuCanvasItem movingMenuCanvasItem, System.Windows.Point point)
        {
            IList<IMenuCanvasItem> menuCanvasItems = MenuCanvasItems;
            var column = GetColumn(point);
            if (column!=null)
            {
                menuCanvasItems=column.GetDeepMenuCanvasItems();
                menuCanvasItems= MenuCanvasItems.Where(x => menuCanvasItems.Contains(x)).ToList();
            }
            IMenuCanvasItem positionMenuCanvasItem = null;
            foreach (var pageItem in menuCanvasItems)
            {
                ItemRelativePos relPos = pageItem.GetRelativePos(point);
                if ((relPos == ItemRelativePos.OnPosUp || relPos == ItemRelativePos.Before))
                {
                    positionMenuCanvasItem=pageItem;
                    break;
                }
            }
            if (positionMenuCanvasItem!=null)
            {
                //insert before positionMenuCanvasItem
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    int newpos = _MenuCanvasItems.IndexOf(positionMenuCanvasItem);
                    _MenuCanvasItems.Insert(newpos, movingMenuCanvasItem);

                    movingMenuCanvasItem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;
                    stateTransition.Consistent = true;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(movingMenuCanvasItem);
            }
            else if (column!=null)
            {

                //Adds at the end of column  
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    menuCanvasItems=column.GetDeepMenuCanvasItems();
                    menuCanvasItems= MenuCanvasItems.Where(x => menuCanvasItems.Contains(x)).ToList();

                    int newpos = _MenuCanvasItems.IndexOf(menuCanvasItems.Last())+1;
                    _MenuCanvasItems.Insert(newpos, movingMenuCanvasItem);
                    movingMenuCanvasItem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;
                    stateTransition.Consistent = true;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(movingMenuCanvasItem);

            }
            else
            {
                AddMenuItem(movingMenuCanvasItem);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(movingMenuCanvasItem);
            }


        }


        /// <MetaDataID>{59db591f-e754-4800-b63e-48f4df0a10ad}</MetaDataID>
        public IMenuCanvasColumn GetMenuCanvasItemAt(Point point)
        {
            foreach (var pageItem in MenuCanvasItems)
            {
                if (pageItem.Column != null)
                {

                }

                //ItemRelativePos relPos = pageItem.GetRelativePos(point);
                //if (relPos == ItemRelativePos.OnPosUp || relPos == ItemRelativePos.OnPosDown)
                //    return pageItem;
            }
            return null;
        }

        /// <MetaDataID>{7577b3f4-afe5-4882-84bb-7b82a1b51bf5}</MetaDataID>
        public bool MoveCanvasItemUp(IMenuCanvasItem movingMenuCanvasItem)
        {

            int pageItemIndex = MenuCanvasItems.IndexOf(movingMenuCanvasItem);
            if (MenuCanvasItems.First() != movingMenuCanvasItem)
            {
                if (MenuCanvasItems[pageItemIndex - 1].Column == null)
                    return false;
                MoveMenuItem(movingMenuCanvasItem, pageItemIndex - 1);
                int menuItemIndex = Menu.MenuCanvasItems.IndexOf(movingMenuCanvasItem);
                (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex - 1);
                return true;
            }
            else
            {
                int menuItemIndex = Menu.MenuCanvasItems.IndexOf(movingMenuCanvasItem);

                if (menuItemIndex != 0)
                {
                    (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex - 1);
                    return true;
                }

                return false;

            }

        }
        /// <MetaDataID>{3555e675-ca40-415e-bff5-6c3a74184e76}</MetaDataID>
        public bool MoveCanvasItemDown(IMenuCanvasItem movingMenuCanvasItem)
        {

            int pageItemIndex = MenuCanvasItems.IndexOf(movingMenuCanvasItem);
            if (MenuCanvasItems.Last() != movingMenuCanvasItem)
            {
                MoveMenuItem(movingMenuCanvasItem, pageItemIndex + 1);
                int menuItemIndex = Menu.MenuCanvasItems.IndexOf(movingMenuCanvasItem);
                (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex + 1);
                return true;
            }
            else if ((Menu as RestaurantMenu).MenuCanvasItems.Last() != movingMenuCanvasItem)
            {
                int menuItemIndex = Menu.MenuCanvasItems.IndexOf(movingMenuCanvasItem);
                (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex + 1);
                return true;
            }
            else
                return false;

        }
        /// <MetaDataID>{2cdb5288-bdc0-42c5-bce6-9b77b47d70b9}</MetaDataID>
        public bool MoveCanvasItemTo(IMenuCanvasItem movingMenuCanvasItem, System.Windows.Point point)
        {
            bool up = false;
            bool down = false;

            if (movingMenuCanvasItem.YPos + movingMenuCanvasItem.Height / 2 < point.Y)
                down = true;
            else
                up = true;
            down = true;

            try
            {
                if (down)
                {
                    IMenuCanvasItem lastAfterOrOnPosDown = null;
                    foreach (var pageItem in MenuCanvasItems)
                    {
                        if (pageItem.Column == null) //Title Headings
                            continue;

                        if (point.X >= pageItem.Column.XPos && point.X <= pageItem.Column.XPos + pageItem.Column.Width)//mouse is in pageItem Column
                        {

                            ItemRelativePos relPos = pageItem.GetRelativePos(point);
                            if (relPos == ItemRelativePos.After || relPos == ItemRelativePos.OnPosDown)
                                lastAfterOrOnPosDown = pageItem;
                            if (relPos == ItemRelativePos.OnPosUp)
                            {
                                int lastAfterOrOnPosDownIndex = MenuCanvasItems.IndexOf(pageItem) - 1;
                                if (lastAfterOrOnPosDownIndex >= 0)
                                {
                                    lastAfterOrOnPosDown = MenuCanvasItems[lastAfterOrOnPosDownIndex];
                                    if (lastAfterOrOnPosDown == movingMenuCanvasItem)
                                        return false;
                                }

                            }
                        }
                    }
                    //lastAfterOrOnPosDown = null;
                    //if (lastAfterOrOnPosDown == null)
                    //    return false;
                    if (lastAfterOrOnPosDown != null)
                    {

                        int pageItemIndex = MenuCanvasItems.IndexOf(lastAfterOrOnPosDown);
                        if (MenuCanvasItems.IndexOf(movingMenuCanvasItem) != pageItemIndex)
                        {
                            if (MenuCanvasItems.IndexOf(movingMenuCanvasItem) > pageItemIndex)
                            {
                                MoveMenuItem(movingMenuCanvasItem, pageItemIndex + 1);
                                int menuItemIndex = Menu.MenuCanvasItems.IndexOf(lastAfterOrOnPosDown);
                                (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex + 1);
                                return true;
                            }
                            else
                            {
                                MoveMenuItem(movingMenuCanvasItem, pageItemIndex);
                                int menuItemIndex = Menu.MenuCanvasItems.IndexOf(lastAfterOrOnPosDown);
                                (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        var pageFirstItem = MenuCanvasItems[0];
                        if (pageFirstItem is IMenuCanvasHeading && (pageFirstItem as IMenuCanvasHeading).HeadingType == HeadingType.Title)
                            return false;

                        MoveMenuItem(movingMenuCanvasItem, 0);
                        int menuItemIndex = Menu.MenuCanvasItems.IndexOf(pageFirstItem);
                        (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex);
                        return true;
                    }
                }
                if (up)
                {
                    IMenuCanvasItem lastBeforOrOnPosUp = null;
                    foreach (var pageItem in MenuCanvasItems.Reverse())
                    {
                        if (pageItem.Column == null) //Title Headings
                            continue;

                        if (movingMenuCanvasItem != pageItem)
                        {
                            if (point.X >= pageItem.Column.XPos && point.X <= pageItem.Column.XPos + pageItem.Column.Width)//mouse is in pageItem Column
                            {
                                ItemRelativePos relPos = pageItem.GetRelativePos(point);
                                if (relPos == ItemRelativePos.Before || relPos == ItemRelativePos.OnPosUp)
                                    lastBeforOrOnPosUp = pageItem;
                            }
                        }
                    }
                    //lastBeforOrOnPosUp = null;
                    if (lastBeforOrOnPosUp != null)
                    {
                        int pageItemIndex = MenuCanvasItems.IndexOf(lastBeforOrOnPosUp);
                        if (MenuCanvasItems.IndexOf(movingMenuCanvasItem) != pageItemIndex)
                        {
                            MoveMenuItem(movingMenuCanvasItem, pageItemIndex);
                            int menuItemIndex = Menu.MenuCanvasItems.IndexOf(lastBeforOrOnPosUp);
                            (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex);
                            return true;
                        }
                    }
                }
            }
            finally
            {
                int i = 0;

                var menuItemsDes = (from page in this.Menu.Pages
                                    from menuItem in page.MenuCanvasItems
                                    select menuItem.Description).ToArray();

                var menuItemsDes2 = (from menuItem in this.Menu.MenuCanvasItems
                                     select menuItem.Description).ToArray();
                foreach (var menuItem in menuItemsDes)
                {
                    if (menuItemsDes2.Length > i)
                        System.Diagnostics.Debug.WriteLine(menuItem + " " + menuItemsDes2[i++]);
                    else
                        System.Diagnostics.Debug.WriteLine(menuItem);


                }

            }
            //int movingItemIndex = MenuCanvasItems.IndexOf(movingMenuCanvasItem);
            //foreach (var pageItem in MenuCanvasItems)
            //{
            //    if (movingMenuCanvasItem != pageItem)
            //    {
            //        int pageItemIndex = MenuCanvasItems.IndexOf(pageItem);

            //        ItemRelativePos relPos = pageItem.GetRelativePos(point);

            //        if (pageItemIndex < movingItemIndex && (relPos == ItemRelativePos.OnPosUp || relPos == ItemRelativePos.Before))
            //        {
            //            MoveMenuItem(movingMenuCanvasItem, pageItemIndex);
            //            var menuCanvasItems = MenuCanvasItems.ToList();
            //            //RenderMenuCanvasItems(menuCanvasItems);
            //            int menuItemIndex = Menu.MenuCanvasItems.IndexOf(pageItem);
            //            (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex);
            //            return true; ;
            //        }

            //    }
            //}
            //foreach (var pageItem in MenuCanvasItems.Reverse())
            //{
            //    if (movingMenuCanvasItem != pageItem)
            //    {
            //        int pageItemIndex = MenuCanvasItems.IndexOf(pageItem);

            //        ItemRelativePos relPos = pageItem.GetRelativePos(point);


            //        if (pageItemIndex > movingItemIndex && (relPos == ItemRelativePos.OnPosDown             || relPos == ItemRelativePos.After))
            //        {

            //            MoveMenuItem(movingMenuCanvasItem, pageItemIndex);
            //            var menuCanvasItems = MenuCanvasItems.ToList();
            //            //RenderMenuCanvasItems(menuCanvasItems);
            //            int menuItemIndex = Menu.MenuCanvasItems.IndexOf(pageItem);
            //            (Menu as RestaurantMenu).MoveMenuItem(movingMenuCanvasItem, menuItemIndex);
            //            return true; ;
            //        }
            //    }
            //}



            return false;
        }



        /// <MetaDataID>{52c07213-9960-4d1b-a0f8-5c25c7b8c3ed}</MetaDataID>
        public void OnCommitObjectState()
        {

        }

        /// <MetaDataID>{1421c05b-abd0-49d6-942b-7e9d4d5cb9c7}</MetaDataID>
        public void OnActivate()
        {

        }

        /// <MetaDataID>{21b0e928-e727-4ac5-a53c-741d274d74ce}</MetaDataID>
        public void OnDeleting()
        {
            foreach (var column in _Columns)
            {
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(column);
            }
        }

        /// <MetaDataID>{126dbf66-8a5b-4de9-b6b3-5fe8a02905d5}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
            if (associationEnd.Name == nameof(Menu))
                (linkedObject as RestaurantMenu).MenuStyleChanged += MenuPage_MenuStyleChanged;
        }


        public IMenuCanvasColumn GetColumn(Point point)
        {
            foreach (var pageItem in MenuCanvasItems.Reverse())
            {
                //Search for FoodItemsGroupColumn
                if (pageItem.Column is MenuCanvas.FoodItemsGroupColumn)
                {
                    var column = pageItem.Column;
                    if (point.X >= column.XPos && point.X < column.XPos + column.Width &&
                        point.Y >= column.YPos && point.Y < column.YPos + column.Height+50)
                    {
                        return column;
                    }
                }
            }
            foreach (var pageItem in MenuCanvasItems.Reverse())
            {
                if (pageItem.Column!=null)
                {
                    //Search for MenuCanvasPageColum
                    if (pageItem.Column is IMenuCanvasPageColumn)
                    {
                        var column = pageItem.Column;
                        if (point.X >= column.XPos && point.X < column.XPos + column.Width &&
                            point.Y >= column.YPos && point.Y < column.YPos + column.Height)
                        {

                            return column;
                        }
                    }
                }
            }

            foreach (var column in Columns)
            {
                if (point.X >= column.XPos && point.X < column.XPos + column.Width &&
                        point.Y >= column.YPos && point.Y < column.YPos + column.MaxHeight)
                {

                    return column;
                }
            }
            return null;
        }

        /// <MetaDataID>{ef691387-ce55-46c1-bc80-b5b4a29e21af}</MetaDataID>
        public MenuCanvas.Rect GetDropRectangle(Point point)
        {
            foreach (var pageItem in MenuCanvasItems.Reverse())
            {
                if (pageItem.Column is MenuCanvas.FoodItemsGroupColumn)
                {
                    var column = pageItem.Column;
                    if (point.X >= column.XPos && point.X < column.XPos + column.Width &&
                        point.Y >= column.YPos && point.Y < column.YPos + column.Height+50)
                    {

                        return new MenuCanvas.Rect(column.XPos - 10, column.YPos, column.Width + 20, column.Height+50);
                    }
                }
            }

            foreach (var pageItem in MenuCanvasItems)
            {
                if (pageItem.Column != null)
                {
                    var column = pageItem.Column;
                    if (point.X >= column.XPos && point.X < column.XPos + column.Width&&
                      point.Y >= column.YPos && point.Y < column.YPos + column.Height)
                    {

                        return new MenuCanvas.Rect(column.XPos - 10, column.YPos, column.Width + 20, column.MaxHeight);
                    }
                }
            }

            foreach (var column in _Columns)
            {
                if (point.X >= column.XPos && point.X < column.XPos + column.Width)
                {

                    return new MenuCanvas.Rect(column.XPos - 10, column.YPos, column.Width + 20, column.MaxHeight);
                }
            }
            return new MenuCanvas.Rect(_Columns[0].XPos - 10, _Columns[0].YPos, _Columns[0].Width + 20, _Columns[0].MaxHeight);
        }

        /// <MetaDataID>{b1b3e09c-cf48-440b-a5fd-ecbffebaea0d}</MetaDataID>
        private void MenuPage_MenuStyleChanged(IStyleSheet oldStyle, IStyleSheet newStyle)
        {

        }

        /// <MetaDataID>{1ea69410-8790-4413-86a5-0f7410f39c9a}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
            if (associationEnd.Name == nameof(Menu))
                (linkedObject as RestaurantMenu).MenuStyleChanged -= MenuPage_MenuStyleChanged;

        }
    }
}