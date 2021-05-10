using System;
using System.ComponentModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using UIBaseEx;


namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{6ccbac68-9936-4a99-a434-3fff4912f8c8}</MetaDataID>
    [BackwardCompatibilityID("{6ccbac68-9936-4a99-a434-3fff4912f8c8}")]
    [Persistent()]
    public class PageStyle : MarshalByRefObject, IPageStyle, INotifyPropertyChanged, ITransactionNotification, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {
        public static string ResourcesRootPath;

        
        /// <exclude>Excluded</exclude>
        int? _ColumnsUneven;
        

        /// <MetaDataID>{b3024862-1b89-4b36-b7fa-38e471708b2c}</MetaDataID>
        [PersistentMember(nameof(_ColumnsUneven))]
        [BackwardCompatibilityID("+15")]
        public int ColumnsUneven
        {
            get
            {
                if (OrgPageStyle != null && !_ColumnsUneven.HasValue)
                    return OrgPageStyle.ColumnsUneven;
                if (!_ColumnsUneven.HasValue)
                    return 50;
                return _ColumnsUneven.Value;
            }
            set
            {

                if (_ColumnsUneven != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ColumnsUneven = value;
                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColumnsUneven)));
                }
            }
        }
        
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{7ae6777d-8363-43ad-beff-ae92823f3d99}</MetaDataID>
        int? _NumOfPageColumns;
        /// <MetaDataID>{9e1db0be-e748-4d1a-a307-c3413757d6a3}</MetaDataID>
        [PersistentMember(nameof(_NumOfPageColumns))]
        [BackwardCompatibilityID("+14")]
        public int NumOfPageColumns
        {
            get
            {
                if (OrgPageStyle != null && !_NumOfPageColumns.HasValue)
                    return OrgPageStyle.NumOfPageColumns;
                if (!_NumOfPageColumns.HasValue)
                    return 1;
                return _NumOfPageColumns.Value;
            }

            set
            {
                if (_NumOfPageColumns != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NumOfPageColumns = value;
                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumOfPageColumns)));
                }

            }
        }

        
        /// <exclude>Excluded</exclude>
        ImageStretch? _BackgroundStretch;
        /// <MetaDataID>{43951113-057d-4609-8f80-f16e8046f145}</MetaDataID>
        [PersistentMember(nameof(_BackgroundStretch))]
        [BackwardCompatibilityID("+12")]
        public ImageStretch BackgroundStretch
        {
            get
            {
                if (OrgPageStyle != null && !_BackgroundStretch.HasValue)
                    return OrgPageStyle.BackgroundStretch;

                if (!_BackgroundStretch.HasValue)
                    return ImageStretch.Uniform;

                return _BackgroundStretch.Value;
            }
            set
            {

                if (_BackgroundStretch != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BackgroundStretch = value;
                        stateTransition.Consistent = true;
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundStretch)));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        Margin? _BackgroundMargin;

        /// <MetaDataID>{c098c43b-a079-4864-8930-27234b46aef2}</MetaDataID>
        [PersistentMember(nameof(_BackgroundMargin))]
        [BackwardCompatibilityID("+11")]
        public Margin BackgroundMargin
        {
            get
            {

                if (OrgPageStyle != null && !_BackgroundMargin.HasValue)
                    return OrgPageStyle.BackgroundMargin;
                if (!_BackgroundMargin.HasValue)
                    return default(Margin);
                return _BackgroundMargin.Value;
            }

            set
            {

                if (_BackgroundMargin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BackgroundMargin = value;
                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundMargin)));

                }

            }
        }


        /// <MetaDataID>{9e9633a3-85ee-44c6-881a-6579448e92ae}</MetaDataID>
        public System.Windows.Media.Stretch Stretch
        {
            get
            {
                //Stretch="UniformToFill"   preserveAspectRatio = "xMidYMid slice"
                //Stretch="Uniform" preserveAspectRatio = "xMidYMid meet"  Stretch to fit false
                //Stretch="Fill" preserveAspectRatio = "none" Stretch to fit true

                return default(System.Windows.Media.Stretch);
            }
            set
            {
            }
        }


        /// <MetaDataID>{b8183374-7ac3-4c3a-8c67-6b3f439dd500}</MetaDataID>
        public bool IsDerivedStyle
        {
            get
            {
                if (OrgPageStyle != null)
                    return true;
                else
                    return false;
            }
        }

        ///// <exclude>Excluded</exclude>
        //string _BorderColor;
        ///// <MetaDataID>{d009fa35-2556-4c3e-bea0-f16446f46c55}</MetaDataID>
        //[PersistentMember(nameof(_BorderColor))]
        //[BackwardCompatibilityID("+10")]
        //public string BorderColor
        //{
        //    get
        //    {
        //        if (OrgPageStyle != null && _BorderColor==null)
        //            return OrgPageStyle.BorderColor;
        //        return _BorderColor;
        //    }

        //    set
        //    {

        //        if (_BorderColor != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _BorderColor = value;
        //                stateTransition.Consistent = true;
        //            }
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Border)));
        //        }
        //    }
        //}





        /// <MetaDataID>{a3e359c2-4976-4af4-a7d4-599c31f71831}</MetaDataID>
        public bool IsPortrait
        {
            get
            {
                if (PageWidth <= PageHeight)
                    return true;
                return false;
            }
        }


        /// <MetaDataID>{3cda80ee-4f4e-4c6a-915f-9a2342038f3b}</MetaDataID>
        public bool IsLandscape
        {
            get
            {
                return !IsPortrait;
            }
        }

        /// <exclude>Excluded</exclude>
        static System.Collections.Generic.List<PaperSize> _PaperSizes = null;
        /// <MetaDataID>{031f7ff7-7702-4bb2-98b3-c22c1e3aa55d}</MetaDataID>
        public static System.Collections.Generic.List<PaperSize> PaperSizes
        {
            get
            {
                if (_PaperSizes == null)
                {
                    _PaperSizes = new System.Collections.Generic.List<PaperSize>()
                    {
                        new PaperSize(PaperType.Letter,"Letter (216mm x 280mm)",216,280,"mm"),
                        new PaperSize(PaperType.Legal,"Legal (216mm x 355.6mm)",216,355.6,"mm" ),
                        new PaperSize(PaperType.Tabloid,"Tabloid (280mm x 432mm)",280,432 ,"mm"),
                        new PaperSize(PaperType.Statement,"Statement (140mm x 216mm)",140,216 ,"mm"),
                        new PaperSize(PaperType.A3,"A3 (297mm x 420mm)",297,420 ,"mm"),
                        new PaperSize(PaperType.A4,"A4 (210mm x 297mm)",210,297 ,"mm"),
                        new PaperSize(PaperType.A5,"A5 (148mm x 210mm)",148,210 ,"mm"),
                        new PaperSize(PaperType.B4,"B4 (250mm x 353mm)" ,250,353,"mm"),
                        new PaperSize(PaperType.B5,"B5 (176mm x 250mm)" ,176,250,"mm"),
                        new PaperSize(PaperType.HDTV,"HDTV (286mm x 508mm)" ,286,508,"mm"),
                        new PaperSize(PaperType.Custom,"Custom" ,0,0,"mm")
                    };
                }
                return _PaperSizes;
            }
        }
        /// <exclude>Excluded</exclude>
        double? _LineSpacing;

        /// <MetaDataID>{ef8e164b-ea65-4065-b782-456db50b1975}</MetaDataID>
        [PersistentMember(nameof(_LineSpacing))]
        [BackwardCompatibilityID("+8")]
        public double LineSpacing
        {
            get
            {
                if (OrgPageStyle != null && !_LineSpacing.HasValue)
                    return OrgPageStyle.LineSpacing;
                if (!_LineSpacing.HasValue)
                    return default(double);
                return _LineSpacing.Value;
            }

            set
            {
                if (_LineSpacing != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LineSpacing = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{0c1b925d-a61d-43fc-bf3c-eb6c0a494d44}</MetaDataID>
        public PageStyle()
        { }
        /// <MetaDataID>{475670fb-28eb-4fa3-bf27-74f2867155bb}</MetaDataID>
        PageStyle OrgPageStyle;
        /// <MetaDataID>{74b16fe8-db3d-49dc-9d62-54218b2ca9b2}</MetaDataID>
        PageStyle(PageStyle orgPageStyle)
        {
            OrgPageStyle = orgPageStyle;
            _Name = OrgPageStyle.Name;
        }



        public void BeforeCommitObjectState()
        {
        }
        /// <MetaDataID>{b5866938-c958-474b-a1e5-6c287845540a}</MetaDataID>
        public void OnCommitObjectState()
        {
        }
        /// <MetaDataID>{57487f3e-2f7d-4272-b2b9-56d2e31fac32}</MetaDataID>
        public void OnActivate()
        {
            if (_Border != null)
                _Border.ObjectChangeState += Border_ObjectChangeState;

            if (_Background != null)
                _Background.ObjectChangeState += Background_ObjectChangeState;


        }

        /// <MetaDataID>{16c5bc5e-52ef-45ea-90f5-5b5634461586}</MetaDataID>
        private void Border_ObjectChangeState(object _object, string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Border)));
        }
        /// <MetaDataID>{361c5cae-ef53-4bcf-90e0-87216b58d610}</MetaDataID>
        private void Background_ObjectChangeState(object _object, string member)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Background)));
        }

        /// <MetaDataID>{fc71aade-dd59-401c-8a1d-46a991e850f7}</MetaDataID>
        public void OnDeleting()
        {
        }
        /// <MetaDataID>{964c0117-c904-4b02-8b72-54fee9e66ae7}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <MetaDataID>{3a393831-a1c1-4bc7-8d86-9cc7e6ba937b}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <exclude>Excluded</exclude>
        IStyleSheet _StyleSheet;
        /// <MetaDataID>{a30d84c9-ec68-4113-b814-9caae4fe537a}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [OOAdvantech.Json.JsonIgnore]
        public MenuPresentationModel.MenuStyles.IStyleSheet StyleSheet
        {
            get
            {
                return _StyleSheet;
            }
        }


        /// <MetaDataID>{b19665a8-283f-4d83-8c12-cdab60b1c113}</MetaDataID>
        public virtual IStyleRule GetDerivedStyle()
        {
            return new PageStyle(this);
        }
        /// <MetaDataID>{b7ea5e90-ca28-4029-a99e-646657811e19}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {
            if (OrgPageStyle != (style as PageStyle))
            {
                if (OrgPageStyle != null)
                    UseDefaultValues();

                OrgPageStyle = style as PageStyle;
             
            }



        }

        public void UseDefaultValues()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                Background = null;
                _BackgroundMargin = null;
                _BackgroundStretch = null;
                _Border = null;
                _BorderMargin = null;
                _ColumnsUneven = null;
                _LineSpacing = null;
                _Margin = null;
                _NumOfPageColumns = null;
                _PageHeight = null;
                _PageWidth = null;
                _ColumnsUneven = null; 
                stateTransition.Consistent = true;
            }

        }

        /// <exclude>Excluded</exclude>
        Margin? _BorderMargin;
        

        /// <MetaDataID>{e05de20a-4a87-4f5b-b36e-4ec39cfb6d63}</MetaDataID>
        [PersistentMember(nameof(_BorderMargin))]
        [BackwardCompatibilityID("+7")]
        public Margin BorderMargin
        {
            get
            {
                if (OrgPageStyle != null && !_BorderMargin.HasValue)
                    return OrgPageStyle.BorderMargin;
                if (!_BorderMargin.HasValue)
                    return default(Margin);
                return _BorderMargin.Value;
            }
            set
            {
                if (_BorderMargin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BorderMargin = value;
                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderMargin)));
                }
            }
        }


        ///// <MetaDataID>{18b36293-9b24-4734-ab85-e120cfbc1a7e}</MetaDataID>
        //public Margin BackgroundMargin
        //{
        //    get
        //    {
        //        if (OrgPageStyle != null && !_BorderMargin.HasValue)
        //            return OrgPageStyle.BackgroundMargin;
        //        if (!_BorderMargin.HasValue)
        //            return default(Margin);
        //        return _BorderMargin.Value;
        //    }
        //    set
        //    {
        //        if (_BorderMargin != value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _BorderMargin = value;
        //                stateTransition.Consistent = true;
        //            }
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BorderMargin)));
        //        }
        //    }
        //}


        /// <MetaDataID>{677e848a-ed27-42bb-9277-b7920136cf6c}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        IPageImage _Background;

        /// <MetaDataID>{a0041b87-3bb8-4542-9fea-322fccb2058b}</MetaDataID>
        [PersistentMember(nameof(_Background))]
        [BackwardCompatibilityID("+1")]
        public MenuPresentationModel.MenuStyles.IPageImage Background
        {


            get
            {
                if (OrgPageStyle != null && _Background == null)
                    return OrgPageStyle.Background;

                return _Background;
            }

            set
            {
                if (_Background != value)
                {
                    if (_Background != null)
                        _Background.ObjectChangeState -= Background_ObjectChangeState;

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Background = value;
                        stateTransition.Consistent = true;
                    }

                    if (OrgPageStyle != null && OrgPageStyle.Background == _Background)
                        _Background = null;

                    if (_Background != null)
                        _Background.ObjectChangeState += Background_ObjectChangeState;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Background)));
                }

            }

            //get
            //{
            //    if (OrgPageStyle != null && !_Background.HasValue)
            //        return OrgPageStyle.Background;
            //    if (!_Background.HasValue)
            //        return default(Resource);
            //    return _Background.Value;
            //}
            //set
            //{
            //    if (_Background != value)
            //    {
            //        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            //        {
            //            _Background = value;
            //            stateTransition.Consistent = true;
            //        }
            //    }
            //}
        }

        /// <exclude>Excluded</exclude>
        IPageImage _Border;

        /// <MetaDataID>{d292c2ce-4b8c-41ae-aac5-f54b877bdfb1}</MetaDataID>
        [PersistentMember(nameof(_Border))]
        [BackwardCompatibilityID("+2")]
        public IPageImage Border
        {
            get
            {
                if (OrgPageStyle != null && _Border == null)
                    return OrgPageStyle.Border;

                return _Border;
            }

            set
            {
                if (_Border != value)
                {
                    if (_Border != null)
                        _Border.ObjectChangeState -= Border_ObjectChangeState;

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Border = value;
                        stateTransition.Consistent = true;
                    }

                    if (OrgPageStyle != null && OrgPageStyle.Border == _Border)
                        Border = null;

                    if (_Border != null)
                        _Border.ObjectChangeState += Border_ObjectChangeState;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Border)));
                }

            }
        }



        /// <exclude>Excluded</exclude>
        Margin? _Margin;

        
        /// <MetaDataID>{e2c06824-6520-4334-9711-5482553f896a}</MetaDataID>
        [PersistentMember(nameof(_Margin))]
        [BackwardCompatibilityID("+3")]
        public Margin Margin
        {
            get
            {
                if (OrgPageStyle != null && !_Margin.HasValue)
                    return OrgPageStyle.Margin;
                if (!_Margin.HasValue)
                    return default(Margin);
                return _Margin.Value;
            }

            set
            {
                if (_Margin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Margin = value;
                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Margin)));
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;


        /// <MetaDataID>{a721e9ff-a000-4cd9-a11f-7a5b4d46ab41}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+4")]
        public string Name
        {
            get
            {
                if (OrgPageStyle != null && _Name == null)
                    return OrgPageStyle.Name;
                return _Name;
            }

            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        double? _PageHeight;

        /// <MetaDataID>{855fbd36-734b-4c1c-8fb6-6d005e908ab3}</MetaDataID>
        [PersistentMember(nameof(_PageHeight))]
        [BackwardCompatibilityID("+5")]
        public double PageHeight
        {
            get
            {
                if (OrgPageStyle != null && !_PageHeight.HasValue)
                    return OrgPageStyle.PageHeight;
                if (!_PageHeight.HasValue)
                    return default(double);
                return _PageHeight.Value;
            }

            set
            {
                //if ((PageHeight - value) > 10 || (PageHeight - value) < -10)
                {
                    if (_PageHeight != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _PageHeight = value;
                            stateTransition.Consistent = true;
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageHeight)));
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _PageWidth;


        public event PropertyChangedEventHandler PropertyChanged;
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{c62684cc-d805-4631-9af2-29c7d32f0518}</MetaDataID>
        [PersistentMember(nameof(_PageWidth))]
        [BackwardCompatibilityID("+6")]
        public double PageWidth
        {
            get
            {
                if (OrgPageStyle != null && !_PageWidth.HasValue)
                    return OrgPageStyle.PageWidth;
                if (!_PageWidth.HasValue)
                    return default(double);
                return _PageWidth.Value;
            }

            set
            {
                //if ((PageWidth - value) > 10 || (PageWidth - value) < -10)
                {
                    if (_PageWidth != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _PageWidth = value;
                            stateTransition.Consistent = true;
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageWidth)));
                    }
                }
            }
        }

        /// <MetaDataID>{3b75170e-9f52-4b3f-ac25-a77957e809aa}</MetaDataID>
        public PaperSize PageSize
        {
            get
            {
                return new PaperSize(PaperType.Unspecified, "", PageWidth, PageHeight, "px");

            }

            set
            {
                if (value.Height != PageHeight || value.Width != PageWidth)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PageHeight = value.Height;
                        _PageWidth = value.Width;

                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageSize)));
                }
            }
        }

        /// <MetaDataID>{12c67dbf-f3f3-4adb-8467-4a52594de912}</MetaDataID>
        public void ResetLineSpacing()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _LineSpacing = (double?)null; 
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{a9e2093c-0c13-44c9-8f74-2a5919da3faa}</MetaDataID>
        public bool IsOrgPropertyValue(string propertyName, object value)
        {
            if (OrgPageStyle != null)
            {
                if (propertyName == nameof(Border) && OrgPageStyle.Border == value)
                    return true;
            }
            else
            {
                if (propertyName == nameof(Border) && Border == value)
                    return true;
            }



            if (OrgPageStyle != null)
            {
                if (propertyName == nameof(Background) && OrgPageStyle.Background == value)
                    return true;
            }
            else
            {
                if (propertyName == nameof(Background) && Background == value)
                    return true;
            }



            if (OrgPageStyle != null)
            {
                if (propertyName == nameof(BorderMargin) && value is Margin && OrgPageStyle.BorderMargin == (Margin)value)
                    return true;
            }
            else
            {
                if (propertyName == nameof(BorderMargin) && value is Margin && OrgPageStyle.BorderMargin == (Margin)value)
                    return true;
            }


            if (OrgPageStyle != null)
            {
                if (propertyName == nameof(IsLandscape) && value is bool && OrgPageStyle.IsLandscape == (bool)value)
                    return true;
            }
            else
            {
                if (propertyName == nameof(IsLandscape) && value is bool && OrgPageStyle.IsLandscape == (bool)value)
                    return true;
            }

            if (OrgPageStyle != null)
            {
                if (propertyName == nameof(IsPortrait) && value is bool && OrgPageStyle.IsPortrait == (bool)value)
                    return true;
            }
            else
            {
                if (propertyName == nameof(IsPortrait) && value is bool && OrgPageStyle.IsPortrait == (bool)value)
                    return true;
            }



            return false;

        }

        /// <MetaDataID>{9e3fc19c-e975-49a3-a813-f9f06cd762e2}</MetaDataID>
        public void OnTransactionCompletted(Transaction transaction)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Border)));
        }
    }





}