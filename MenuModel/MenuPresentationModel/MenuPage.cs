using System;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using MenuPresentationModel.MenuCanvas;
using OOAdvantech;

namespace MenuPresentationModel
{
    /// <MetaDataID>{718f41a2-9bce-4c18-a03a-9c62d123341b}</MetaDataID>
    [BackwardCompatibilityID("{718f41a2-9bce-4c18-a03a-9c62d123341b}")]
    [Persistent()]
    public class MenuPage : MenuCanvas.IMenuPageCanvas, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {
        /// <MetaDataID>{5e1f2742-3104-4cd6-ab95-bfa3a2d99e8a}</MetaDataID>
        public MenuPage()
        {

        }
        /// <exclude>Excluded</exclude>
        int _NumberofColumns = 1;
        /// <MetaDataID>{6a84e284-1bca-4196-a97f-0b8e7fbdc7f1}</MetaDataID>
        [PersistentMember(nameof(_NumberofColumns))]
        [BackwardCompatibilityID("+3")]
        public int NumberofColumns
        {
            get
            {
                return _NumberofColumns;
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
        OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasItem> _MenuCanvasItems = new OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasItem>();

        /// <MetaDataID>{a5404bb4-bc8d-44ef-b51a-ef67d41551ef}</MetaDataID>
        [PersistentMember(nameof(_MenuCanvasItems))]
        [BackwardCompatibilityID("+2")]
        public System.Collections.Generic.IList<MenuCanvas.IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                return _MenuCanvasItems.AsReadOnly();
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasColumn> _Columns = new OOAdvantech.Collections.Generic.Set<MenuCanvas.IMenuCanvasColumn>();

        /// <MetaDataID>{2d055cd5-70cd-4f7f-a166-a66a56c0af84}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [ImplementationMember(nameof(_Columns))]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasColumn> Columns
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
                _MenuCanvasItems.Remove(manuCanvasItem);
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
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Insert(pos, manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;
        }

        private void ManuCanvasitem_ObjectChangeState(object _object, string member)
        {

            //System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            //{


            if (member == nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItem.Description) ||
                member == nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItem.ExtraDescription) ||
                member == nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItem.Extras) ||
                member == nameof(MenuPresentationModel.MenuCanvas.IMenuCanvasHeading.Accent) )
            {
                if (ObjectChangeState != null)
                {
                    RenderMenuCanvasItems();
                    ObjectChangeState?.Invoke(this, nameof(MenuCanvasItems));
                }
            }
            // }));
        }

        /// <MetaDataID>{eff91be1-f837-4004-ae68-7e2556cbe184}</MetaDataID>
        public void Delete(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Remove(manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState -= ManuCanvasitem_ObjectChangeState;
        }

        /// <MetaDataID>{26f82366-dba8-494c-ae7f-d2548c3349e3}</MetaDataID>
        public void AddMenuItem(MenuCanvas.IMenuCanvasItem manuCanvasitem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuCanvasItems.Add(manuCanvasitem);
                stateTransition.Consistent = true;
            }
            manuCanvasitem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

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
        OOAdvantech.Member<RestaurantMenu> _Menu = new OOAdvantech.Member<RestaurantMenu>();

        public event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{b4f5f7f4-74f4-4eb9-9296-85c2ba5c2e70}</MetaDataID>
        [Association("MenuPages", Roles.RoleB, "9537b191-c58c-42ac-a37d-e2c62a4dcdf4")]
        [PersistentMember(nameof(_Menu))]
        [RoleBMultiplicityRange(1, 1)]
        public RestaurantMenu Menu
        {
            get
            {
                return _Menu;
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
                if (Menu != null)
                    return Menu.Style;
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

        /// <MetaDataID>{11ead16b-d7fa-44f2-9d6f-2e7d68a12e09}</MetaDataID>
        public void RenderMenuCanvasItems()
        {

            foreach (var menuCanvasItem in this.MenuCanvasItems)
            {
                if (menuCanvasItem is MenuCanvas.IMenuCanvasHeading && (menuCanvasItem as MenuCanvas.IMenuCanvasHeading).HeadingType == MenuCanvas.HeadingType.Title)
                {

                }
                else
                {
                    break;
                }
            }
            var pageStyle = (Style.Styles["page"] as MenuStyles.PageStyle);
            double columnsWidth = pageStyle.PageWidth - pageStyle.Margin.MarginLeft - pageStyle.Margin.MarginRight;

            var menuCanvasItems = MenuCanvasItems.ToList();
            double nextMenuCanvasItemY = pageStyle.Margin.MarginTop;
            double xPos = pageStyle.Margin.MarginLeft;
            MenuCanvas.IMenuCanvasHeading menuCanvasHeading = null;
            if (menuCanvasItems.Count > 0 && menuCanvasItems[0] is MenuCanvas.IMenuCanvasHeading && (menuCanvasItems[0] as MenuCanvas.IMenuCanvasHeading).HeadingType == MenuCanvas.HeadingType.Title)
            {
                menuCanvasHeading = menuCanvasItems[0] as MenuCanvas.IMenuCanvasHeading;
                menuCanvasItems.RemoveAt(0);
                nextMenuCanvasItemY += menuCanvasHeading.Style.BeforeSpacing + pageStyle.LineSpacing;
                menuCanvasHeading.YPos = nextMenuCanvasItemY;
                nextMenuCanvasItemY += menuCanvasHeading.Style.AfterSpacing + menuCanvasHeading.Height;

                //if (menuCanvasHeading.Style.Accent != null)
                //{
                //    var accent = new MenuCanvas.MenuCanvasHeadingAccent(menuCanvasHeading, menuCanvasHeading.Style.Accent);
                //    menuCanvasHeading.Accent = accent;
                //}
                //else
                //    menuCanvasHeading.Accent = null;
                if (menuCanvasHeading.Accent != null)
                    menuCanvasHeading.Accent.FullRowWidth = columnsWidth;

                if (menuCanvasHeading.Style.Alignment == MenuStyles.Alignment.Center)
                    menuCanvasHeading.XPos = xPos + (columnsWidth / 2) - (menuCanvasHeading.Width / 2);
                if (menuCanvasHeading.Style.Alignment == MenuStyles.Alignment.Left)
                    menuCanvasHeading.XPos = xPos;
                if (menuCanvasHeading.Style.Alignment == MenuStyles.Alignment.Right)
                    menuCanvasHeading.XPos = xPos + columnsWidth - menuCanvasHeading.Width;
            }

            foreach (var column in Columns)
            {
                column.Width = columnsWidth / Columns.Count;
                column.YPos = nextMenuCanvasItemY;
                column.XPos = pageStyle.Margin.MarginLeft;
                column.MaxHeight = pageStyle.PageHeight - pageStyle.Margin.MarginTop - pageStyle.Margin.MarginBottom;

                column.RenderMenuCanvasItems(menuCanvasItems, menuCanvasHeading);
                break;
            }
        }
        public void InsertCanvasItemTo(IMenuCanvasItem movingMenuCanvasItem, System.Windows.Point point)
        {
            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            objectStorage.CommitTransientObjectState(movingMenuCanvasItem);

            foreach (var pageItem in MenuCanvasItems)
            {
                ItemRelativePos relPos = pageItem.GetRelativePos(point);
                if ((relPos == ItemRelativePos.OnPos || relPos == ItemRelativePos.Before))
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        int newpos = _MenuCanvasItems.IndexOf(pageItem);
                        _MenuCanvasItems.Insert(newpos, movingMenuCanvasItem);
                        movingMenuCanvasItem.ObjectChangeState += ManuCanvasitem_ObjectChangeState;
                        stateTransition.Consistent = true;

                    }
                    return;
                }
            }
            AddMenuItem(movingMenuCanvasItem);
        }



        public void MoveCanvasItemTo(IMenuCanvasItem movingMenuCanvasItem, System.Windows.Point point)
        {
            int movingItemIndex = MenuCanvasItems.IndexOf(movingMenuCanvasItem);
            foreach (var pageItem in MenuCanvasItems)
            {
                if (movingMenuCanvasItem != pageItem)
                {
                    int pageItemIndex = MenuCanvasItems.IndexOf(pageItem);

                    ItemRelativePos relPos = pageItem.GetRelativePos(point);

                    if (pageItemIndex < movingItemIndex && (relPos == ItemRelativePos.OnPos || relPos == ItemRelativePos.Before))
                    {
                        MoveMenuItem(movingMenuCanvasItem, pageItemIndex);
                        RenderMenuCanvasItems();
                        return;
                    }

                }
            }
            foreach (var pageItem in MenuCanvasItems.Reverse())
            {
                if (movingMenuCanvasItem != pageItem)
                {
                    int pageItemIndex = MenuCanvasItems.IndexOf(pageItem);

                    ItemRelativePos relPos = pageItem.GetRelativePos(point);


                    if (pageItemIndex > movingItemIndex && (relPos == ItemRelativePos.OnPos || relPos == ItemRelativePos.After))
                    {
                        //if (relPos == ItemRelativePos.OnPos)
                        //    pageItemIndex++;


                        MoveMenuItem(movingMenuCanvasItem, pageItemIndex);
                        RenderMenuCanvasItems();
                        break;
                    }
                }
            }
        }



        public void OnCommitObjectState()
        {

        }

        public void OnActivate()
        {

        }

        public void OnDeleting()
        {

        }

        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
            if (associationEnd.Name == nameof(Menu))
                (linkedObject as RestaurantMenu).MenuStyleChanged += MenuPage_MenuStyleChanged;
        }

        private void MenuPage_MenuStyleChanged(IStyleSheet oldStyle, IStyleSheet newStyle)
        {

        }

        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
            if (associationEnd.Name == nameof(Menu))
                (linkedObject as RestaurantMenu).MenuStyleChanged -= MenuPage_MenuStyleChanged;

        }
    }
}