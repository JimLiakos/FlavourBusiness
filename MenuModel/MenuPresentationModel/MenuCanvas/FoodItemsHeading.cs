using System;
using System.Windows;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Linq;
using System.Linq;
using OOAdvantech;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{9bc28051-1a1d-4f49-9979-344f1ea39924}</MetaDataID>
    [BackwardCompatibilityID("{9bc28051-1a1d-4f49-9979-344f1ea39924}")]
    [Persistent()]
    public class FoodItemsHeading : MarshalByRefObject, IMenuCanvasHeading,OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {


        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{365c8134-21f1-4b7b-803f-48a7fadbbeb0}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
        }

        /// <MetaDataID>{294b984f-72c9-4f9c-9339-94fbee60b4a5}</MetaDataID>
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

        /// <MetaDataID>{f6347d3e-f872-46dd-bff4-77d991df06d2}</MetaDataID>
        public void OnCommitObjectState()
        {

        }

        /// <MetaDataID>{9c231f19-8780-4043-bcd6-a41445a2971c}</MetaDataID>
        public void OnActivate()
        {

        }

        /// <MetaDataID>{90db6985-4e82-4e2b-84c1-e3c75db4ed18}</MetaDataID>
        public void OnDeleting()
        {

        }

        /// <MetaDataID>{c060f76a-14aa-4c77-9886-fbae23a1b684}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

            ObjectChangeState?.Invoke(this, nameof(Page));
        }

        /// <MetaDataID>{86e0a1a0-21f4-43f2-81e7-62e1e4debde8}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
            ObjectChangeState?.Invoke(this, nameof(Page));
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{86c88868-397d-409a-a410-3750c4db1876}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                //if (Font.AllCaps&& _Description!=null)
                //    return _Description.ToUpper();
                //else
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
                    OOAdvantech.Transactions.Transaction.ExecuteAsynch(new Action(() =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(Description));
                    }));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItemsGroup _HostingArea;

        /// <MetaDataID>{c1bb6859-d202-46fb-b76c-bcc29e109ce8}</MetaDataID>

        [BackwardCompatibilityID("+2")]
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
        /// <exclude>Excluded</exclude>
        double _Width;

        /// <MetaDataID>{fff97e39-e79a-4ad9-8f93-101b5652ee43}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+3")]
        public double Width
        {
            get
            {
                return Font.MeasureText(Description).Width;

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

        /// <MetaDataID>{ae837f01-d67d-49a1-88ce-0c542739d218}</MetaDataID>
        [PersistentMember(nameof(_XPos))]
        [BackwardCompatibilityID("+4")]
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

        /// <MetaDataID>{9b64f2a7-7bab-45ff-97c0-b06f6dc38e9a}</MetaDataID>
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
        /// <exclude>Excluded</exclude>
        IMenuPageCanvas _Page;

        /// <MetaDataID>{d35fb57a-234d-43e5-8395-2e77b6132a2b}</MetaDataID>
        [PersistentMember(nameof(_Page))]
        [BackwardCompatibilityID("+6")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IMenuPageCanvas Page
        {
            get
            {
                return _Page;
            }

            //set
            //{

            //    if (_Page != value)
            //    {
            //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //        {
            //            _Page = value;
            //            stateTransition.Consistent = true;
            //        }
            //    }

            //}
        }

        /// <exclude>Excluded</exclude>
        HeadingType _HeadingType;
        /// <MetaDataID>{17aa57e7-a172-497c-ab00-bacdf265476d}</MetaDataID>
        [PersistentMember(nameof(_HeadingType))]
        [BackwardCompatibilityID("+7")]
        public MenuPresentationModel.MenuCanvas.HeadingType HeadingType
        {
            get
            {
                return _HeadingType;
            }

            set
            {

                if (_HeadingType != value && value != HeadingType.SubHeading)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HeadingType = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        FontData? _Font;
        /// <MetaDataID>{e6cce4ad-93a2-49d9-9b89-801299499bc8}</MetaDataID>
        public MenuStyles.FontData Font
        {
            get
            {
                if (_Font.HasValue)
                    return _Font.Value;
                else
                {
                    if (Style == null)
                        return default(MenuStyles.FontData);

                    return Style.Font;
                }
            }
            set
            {
                _Font = value;
            }
        }

        /// <exclude>Excluded</exclude>
        IMenuCanvasHeadingAccent _Accent;

        /// <MetaDataID>{8b7b2754-32d1-4453-b519-b76f5f0de104}</MetaDataID>
        [PersistentMember(nameof(_Accent))]
        [BackwardCompatibilityID("+8")]
        public MenuPresentationModel.MenuCanvas.IMenuCanvasHeadingAccent Accent
        {
            get
            {
                if(_Accent==null&&Style.Accent!=null)
                    return new MenuCanvasHeadingAccent(this, Style.Accent);
                return _Accent;
            }

            set
            {

                if (_Accent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Accent = value;
                        string name = _Accent.Accent.Name;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Accent));
                }

             
                
            }
        }

        /// <MetaDataID>{e603e1b0-f7a2-4b64-92b9-bbe5e76702df}</MetaDataID>
        public double Height
        {
            get
            {
                return Font.MeasureText(Description).Height;

            }

            set
            {
                //throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{82b7e39d-8672-48d7-8e0e-e6be64b7ab10}</MetaDataID>
        public IHeadingStyle Style
        {
            get
            {
                MenuStyles.IStyleSheet styleSheet = null;
                if (Page != null)
                    styleSheet = Page.Style;
                else
                {
                    if (RestaurantMenu.ConntextStyleSheet != null)
                        styleSheet = RestaurantMenu.ConntextStyleSheet;
                    else
                        return null;

                    //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));
                    //RestaurantMenu restaurantMenu = (from menu in storage.GetObjectCollection<RestaurantMenu>()
                    //                                 select menu).FirstOrDefault();
                    //styleSheet = restaurantMenu.Style;
                }

                if (HeadingType == HeadingType.Title)
                    return (styleSheet.Styles["title-heading"] as MenuStyles.IHeadingStyle);
                if (HeadingType == HeadingType.Normal)
                    return (styleSheet.Styles["heading"] as MenuStyles.IHeadingStyle);
                if (HeadingType == HeadingType.SubHeading)
                    return (styleSheet.Styles["small-heading"] as MenuStyles.IHeadingStyle);
                if (HeadingType == HeadingType.AltFont)
                    return (styleSheet.Styles["alt-font-heading"] as MenuStyles.IHeadingStyle);
                return null;
            }
        }

        /// <MetaDataID>{815e8b26-4dc0-4e91-9ecc-3025ff32588a}</MetaDataID>
        public Rect CanvasFrameArea
        {
            get
            {
                double x = 0;
                double y = 0;
                double height = 0;
                double width = 0;

                if (HostingArea != null)
                    x = HostingArea.Column.XPos - 10;
                else
                    x = (Page.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).Margin.MarginLeft;

                y = YPos - 10;
                height = Height + 20;
                if (HostingArea != null)
                    width = HostingArea.Width + 20;
                else
                {
                    MenuPresentationModel.MenuStyles.PageStyle pageStyle = Page.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle;
                    width = pageStyle.PageWidth - (pageStyle.Margin.MarginLeft + pageStyle.Margin.MarginRight);
                }
                return new Rect(x, y, width, height);

            }
        }
    }

}