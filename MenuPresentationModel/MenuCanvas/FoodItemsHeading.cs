using System;
using System.Windows;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Linq;
using System.Linq;
using OOAdvantech;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{9bc28051-1a1d-4f49-9979-344f1ea39924}</MetaDataID>
    [BackwardCompatibilityID("{9bc28051-1a1d-4f49-9979-344f1ea39924}")]
    [Persistent()]
    public class FoodItemsHeading : MarshalByRefObject, IMenuCanvasHeading, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {

        public FoodItemsHeading()
        {

        }
        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{0b19f37b-c5af-444c-8cee-8455b6d1e300}</MetaDataID>
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }


        /// <exclude>Excluded</exclude>
        IAccent _CustomHeadingAccent;
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [PersistentMember(nameof(_CustomHeadingAccent))]
        [Association("FoodItemsHeadingCustomAccent", Roles.RoleA, "1f4cca46-1278-4e09-86b5-969d4823c216")]
        public IAccent CustomHeadingAccent
        {
            get => _CustomHeadingAccent;
            set
            {

                if (_CustomHeadingAccent != value)
                {

                    Accent newHeadingAccent = value as Accent;

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (newHeadingAccent != null && OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(value) != OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this))
                        {
                            newHeadingAccent = MenuStyles.Accent.Clone(value as Accent);
                            if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null)
                                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(newHeadingAccent);
                        }
                        if (_CustomHeadingAccent != null)
                            OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(_CustomHeadingAccent);

                        _CustomHeadingAccent = newHeadingAccent;
                        _Accent = null;
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
        bool _CustomSpacing;

        /// <MetaDataID>{8e0a2e9e-6678-4eeb-ac11-a62f29e3ac19}</MetaDataID>
        [PersistentMember(nameof(_CustomSpacing))]
        [BackwardCompatibilityID("+17")]
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

        /// <MetaDataID>{7d56c5b3-3b1e-408b-a87c-2e9d5019b1f8}</MetaDataID>
        [PersistentMember(nameof(_AfterSpacing))]
        [BackwardCompatibilityID("+16")]
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

        /// <MetaDataID>{48e142e8-45ff-4a5d-b802-4817abdc0522}</MetaDataID>
        [PersistentMember(nameof(_BeforeSpacing))]
        [BackwardCompatibilityID("+15")]
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

        /// <MetaDataID>{9d052e1b-546a-4c97-b68b-a802fa9494aa}</MetaDataID>
        public bool IsStyleAlignmentOverridden
        {
            get
            {
                return _Alignment.HasValue;
            }
        }
        /// <MetaDataID>{ce93fe16-6d00-4dcf-90c1-fc47906e6422}</MetaDataID>
        public void ClearAlignment()
        {
            _Alignment = null;
        }



        /// <exclude>Excluded</exclude>
        Alignment? _Alignment;
        /// <MetaDataID>{bb477ac8-5e66-4d86-9b23-2a61d0d189b2}</MetaDataID>
        [PersistentMember(nameof(_Alignment))]
        [BackwardCompatibilityID("+14")]
        public Alignment Alignment
        {
            get
            {
                if (Style != null && !_Alignment.HasValue)
                    return Style.Alignment;
                if (!_Alignment.HasValue)
                    return default(Alignment);
                return _Alignment.Value;
            }
            set
            {
                if (_Alignment != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Alignment = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{9d7f47bd-8608-4414-975f-55ba0aec62da}</MetaDataID>
        public void RemoveHostingArea(IMenuCanvasFoodItemsGroup hostingArea)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HostingAreas.Remove(hostingArea);
                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{a2a7b4bf-ebee-4bad-a70f-b1b6548605dc}</MetaDataID>
        public void Remove()
        {

            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }

        /// <MetaDataID>{18204363-3e8b-4972-84e8-949a5fde82cc}</MetaDataID>
        public void AddHostingArea(IMenuCanvasFoodItemsGroup hostingArea)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HostingAreas.Add(hostingArea);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{396c0e65-5d6a-444b-95a2-47787d2a2237}</MetaDataID>
        public void RemoveAllHostingAreas()
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HostingAreas.Clear();
                stateTransition.Consistent = true;
            }

        }


        /// <exclude>Excluded</exclude>
        int _NumberOfFoodColumns = 1;

        /// <MetaDataID>{d4be5b25-ccc9-4bec-8cc4-a99f8d7a0fc4}</MetaDataID>
        [PersistentMember(nameof(_NumberOfFoodColumns))]
        [BackwardCompatibilityID("+10")]
        public int NumberOfFoodColumns
        {
            get
            {
                return _NumberOfFoodColumns;
            }
            set
            {

                if (_NumberOfFoodColumns != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NumberOfFoodColumns = value;
                        stateTransition.Consistent = true;
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
        bool _NextColumnOrPage;

        /// <MetaDataID>{bef30981-78cd-4dae-9965-bbb5a7dd0a0c}</MetaDataID>
        [PersistentMember(nameof(_NextColumnOrPage))]
        [BackwardCompatibilityID("+9")]
        public bool NextColumnOrPage
        {
            get
            {
                return _NextColumnOrPage;
            }
            set
            {
                if (_NextColumnOrPage != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NextColumnOrPage = value;
                        stateTransition.Consistent = true;
                    }
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(NextColumnOrPage));
                    }));
                }
            }
        }



        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{365c8134-21f1-4b7b-803f-48a7fadbbeb0}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            _YPos.Value = foodItemLineText.YPos + foodItemLineText.Font.GetTextBaseLine(Description) - Font.GetTextBaseLine(Description);
            BaseLine = Font.GetTextBaseLine(Description);
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
            if (point.X > CanvasFrameArea.X && point.X < CanvasFrameArea.X + CanvasFrameArea.Width)
            {
                if (point.Y < CanvasFrameArea.Y && point.X >= CanvasFrameArea.X && point.X <= CanvasFrameArea.X + CanvasFrameArea.Width)
                    return ItemRelativePos.Before;

                if (point.Y > CanvasFrameArea.Y + CanvasFrameArea.Height && point.X >= CanvasFrameArea.X && point.X <= CanvasFrameArea.X + CanvasFrameArea.Width)
                {
                    return ItemRelativePos.After;
                }

                if (point.Y >= CanvasFrameArea.Y && point.Y <= CanvasFrameArea.Y + CanvasFrameArea.Height)
                {
                    if (point.X >= CanvasFrameArea.X && point.X <= CanvasFrameArea.X + CanvasFrameArea.Width)
                    {
                        if (point.Y <= CanvasFrameArea.Y + CanvasFrameArea.Height / 2)
                            return ItemRelativePos.OnPosUp;
                        else
                            return ItemRelativePos.OnPosDown;
                    }
                    if (point.X < CanvasFrameArea.X)
                        return ItemRelativePos.Before;
                }
                return ItemRelativePos.After;
            }

            if (point.X < CanvasFrameArea.X && point.Y < CanvasFrameArea.Y)
                return ItemRelativePos.Before;
            else
                return ItemRelativePos.After;
        }


        /// <MetaDataID>{2b0323e5-5995-482f-ab85-12d7d2b8dc60}</MetaDataID>
        public void BeforeCommitObjectState()
        {
            if (_MenuCanvasAccent.Value != null)
            {
                if (OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this) != null && OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_MenuCanvasAccent.Value) == null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(_MenuCanvasAccent.Value);
            }
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
        OOAdvantech.MultilingualMember<string> _Description = new MultilingualMember<string>();
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
                        _Description.Value = value;
                        stateTransition.Consistent = true;
                    }
                    var culture = CultureContext.CurrentCultureInfo;
                    var useDefaultCultureValue = CultureContext.UseDefaultCultureValue;
                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(() =>
                    {
                        using (CultureContext cultureContext = new CultureContext(culture, useDefaultCultureValue))
                        {
                            ObjectChangeState?.Invoke(this, nameof(Description));
                        }
                    }));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.MultilingualSet<IMenuCanvasFoodItemsGroup> _HostingAreas = new OOAdvantech.Collections.Generic.MultilingualSet<IMenuCanvasFoodItemsGroup>();

        /// <MetaDataID>{c1bb6859-d202-46fb-b76c-bcc29e109ce8}</MetaDataID>

        [BackwardCompatibilityID("+2")]
        public System.Collections.Generic.IList<MenuPresentationModel.MenuCanvas.IMenuCanvasFoodItemsGroup> HostingAreas
        {
            get
            {
                return _HostingAreas.AsReadOnly();
            }

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _Width = new MultilingualMember<double>();

        /// <MetaDataID>{fff97e39-e79a-4ad9-8f93-101b5652ee43}</MetaDataID>
        [PersistentMember(nameof(_Width))]
        [BackwardCompatibilityID("+3")]
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
                        _Width.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _XPos = new MultilingualMember<double>();

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
                        _XPos.Value = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _YPos = new MultilingualMember<double>();

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
                        _YPos.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuPageCanvas> _Page = new MultilingualMember<IMenuPageCanvas>();

        /// <MetaDataID>{d35fb57a-234d-43e5-8395-2e77b6132a2b}</MetaDataID>
        [PersistentMember(nameof(_Page))]
        [BackwardCompatibilityID("+6")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IMenuPageCanvas Page
        {
            get
            {
                return _Page.Value;
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
        public HeadingType HeadingType
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
        public FontData Font
        {
            get
            {
                if (_Font.HasValue)
                    return _Font.Value;
                else
                {
                    if (Style == null)
                        return default(FontData);

                    return Style.Font;
                }
            }
            set
            {
                _Font = value;
            }
        }


        //public double BaseLine { get; private set; }

        /// <exclude>Excluded</exclude>
        IMenuCanvasAccent _Accent;

        /// <MetaDataID>{8b7b2754-32d1-4453-b519-b76f5f0de104}</MetaDataID>
        [ImplementationMember(nameof(_Accent))]
        [BackwardCompatibilityID("+8")]
        public IMenuCanvasAccent Accent
        {
            get
            {
                if (_CustomHeadingAccent != null)
                {
                    if (_Accent == null)
                    {
                        if (_MenuCanvasAccent.Value == null || _MenuCanvasAccent.Value.Accent == null || !_MenuCanvasAccent.Value.Accent.IsTheSameWith(_CustomHeadingAccent))
                            _Accent = new MenuCanvasAccent(this, _CustomHeadingAccent);
                        else
                            _Accent = _MenuCanvasAccent.Value;

                    }
                    return _Accent;
                }

                if (_Accent == null && Style.Accent != null)
                {
                    if (_MenuCanvasAccent.Value == null || _MenuCanvasAccent.Value.Accent == null || !_MenuCanvasAccent.Value.Accent.IsTheSameWith(Style.Accent))
                        _Accent = new MenuCanvasAccent(this, Style.Accent);
                    else
                        _Accent = _MenuCanvasAccent.Value;
                }

                if (Style.Accent == null)
                    _Accent = null;
                else if (_Accent != null && Style.Accent != _Accent.Accent)
                {
                    if (_MenuCanvasAccent.Value == null || _MenuCanvasAccent.Value.Accent == null || !_MenuCanvasAccent.Value.Accent.IsTheSameWith(Style.Accent))
                        _Accent = new MenuCanvasAccent(this, Style.Accent);
                    else
                        _Accent = _MenuCanvasAccent.Value;
                }
                return _Accent;
            }
            //set
            //{

            //    if (_Accent != value)
            //    {
            //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //        {
            //            _Accent = value;
            //            string name = _Accent.Accent.Name;
            //            stateTransition.Consistent = true;
            //        }
            //        ObjectChangeState?.Invoke(this, nameof(Accent));
            //    }

            //}
        }

        ///// <exclude>Excluded</exclude>
        //string _Color;

        //public string Color
        //{
        //    get
        //    {
        //        if (_Color == null && Style.Accent != null)
        //            return Style.Accent.AccentColor;
        //        return _Color;
        //    }

        //    set
        //    {
        //        if (_Color != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _Color = value;
        //                string name = _Accent.Accent.Name;
        //                stateTransition.Consistent = true;
        //            }
        //            ObjectChangeState?.Invoke(this, nameof(Color));
        //        }
        //    }
        //}


        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _FullRowWidth = new MultilingualMember<double>();
        /// <MetaDataID>{2a661bf9-77e8-43f0-8a38-588cc8e2f649}</MetaDataID>
        [PersistentMember(nameof(_FullRowWidth))]
        [BackwardCompatibilityID("+11")]
        public double FullRowWidth
        {
            get
            {
                return _FullRowWidth;
            }

            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FullRowWidth.Value = value;
                    stateTransition.Consistent = true;
                }


            }
        }

        /// <MetaDataID>{62ed53aa-e38d-447d-ab0b-87d554da1ab5}</MetaDataID>
        public void ResetSize()
        {
            var size = Font.MeasureText(Description);
            Width = size.Width;
            Height = size.Height;
            BaseLine = Font.GetTextBaseLine(Description);


            string description = Description;
            using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, false))
            {
                UnTranslated = description != Description;
            }


        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<double> _BaseLine = new MultilingualMember<double>();
        /// <MetaDataID>{ae92bdb6-e853-4c89-987d-c7c7615c9c99}</MetaDataID>
        [PersistentMember(nameof(_BaseLine))]
        [BackwardCompatibilityID("+13")]
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
        OOAdvantech.MultilingualMember<double> _Height = new MultilingualMember<double>();
        /// <MetaDataID>{e603e1b0-f7a2-4b64-92b9-bbe5e76702df}</MetaDataID>
        [PersistentMember(nameof(_Height))]
        [BackwardCompatibilityID("+12")]
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
                        _Height.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
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

                if (HostingAreas.Count > 0)
                    x = HostingAreas[0].PageColumn.XPos - 10;
                else
                    x = (Page.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle).Margin.MarginLeft;

                y = YPos - 10;
                height = Height + 20;
                if (HostingAreas.Count > 0)
                    width = HostingAreas[0].Width + 20;
                else
                {
                    MenuPresentationModel.MenuStyles.PageStyle pageStyle = Page.Style.Styles["page"] as MenuPresentationModel.MenuStyles.PageStyle;
                    width = pageStyle.PageWidth - (pageStyle.Margin.MarginLeft + pageStyle.Margin.MarginRight);
                }
                return new Rect(x, y, width, height);

            }
        }

        /// <MetaDataID>{3d99db23-5c78-40e6-8695-3997005a310d}</MetaDataID>
        public bool UnTranslated { get; private set; }

        /// <exclude>Excluded</exclude>
        Member<IMenuCanvasAccent> _MenuCanvasAccent = new Member<IMenuCanvasAccent>();

        /// <MetaDataID>{d07e1bd3-f1ea-4fa4-91bc-5e34cce9487c}</MetaDataID>
        [PersistentMember(nameof(_MenuCanvasAccent))]
        public IMenuCanvasAccent MenuCanvasAccent
        {
            get
            {
                if (CustomHeadingAccent != null)
                {
                    if (_MenuCanvasAccent.Value != null && _MenuCanvasAccent.Value.Accent != CustomHeadingAccent)
                    {

                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _MenuCanvasAccent.Value.Accent = CustomHeadingAccent;
                            stateTransition.Consistent = true;
                        }

                    }
                }
                else
                {
                    if (_MenuCanvasAccent.Value != Accent)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _MenuCanvasAccent.Value = Accent;
                            stateTransition.Consistent = true;
                        }
                    }
                }



                return _MenuCanvasAccent.Value;
            }
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

    }

}