using System;
using System.ComponentModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{6ccbac68-9936-4a99-a434-3fff4912f8c8}</MetaDataID>
    [BackwardCompatibilityID("{6ccbac68-9936-4a99-a434-3fff4912f8c8}")]
    [Persistent()]
    public class PageStyle : MarshalByRefObject, IPageStyle, INotifyPropertyChanged, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {
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
                        new PaperSize(PaperType.Letter,"Letter (216mm x 280mm)",216,280),
                        new PaperSize(PaperType.Legal,"Legal (216mm x 355.6mm)",216,355.6 ),
                        new PaperSize(PaperType.Tabloid,"Tabloid (280mm x 432mm)",280,432 ),
                        new PaperSize(PaperType.Statement,"Statement (140mm x 216mm)",140,216 ),
                        new PaperSize(PaperType.A3,"A3 (297mm x 420mm)",297,420 ),
                        new PaperSize(PaperType.A4,"A4 (210mm x 297mm)",210,297 ),
                        new PaperSize(PaperType.A5,"A5 (148mm x 210mm)",148,210 ),
                        new PaperSize(PaperType.B4,"B4 (250mm x 353mm)" ,250,353),
                        new PaperSize(PaperType.B5,"B5 (176mm x 250mm)" ,176,250),
                        new PaperSize(PaperType.HDTV,"HDTV (286mm x 508mm)" ,286,508),
                        new PaperSize(PaperType.Custom,"Custom" ,0,0)
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
        public PageStyle() { }
        /// <MetaDataID>{475670fb-28eb-4fa3-bf27-74f2867155bb}</MetaDataID>
        PageStyle OrgPageStyle;
        /// <MetaDataID>{74b16fe8-db3d-49dc-9d62-54218b2ca9b2}</MetaDataID>
        PageStyle(PageStyle orgPageStyle)
        {
            OrgPageStyle = orgPageStyle;
            _Name = OrgPageStyle.Name;
        }


        /// <MetaDataID>{b5866938-c958-474b-a1e5-6c287845540a}</MetaDataID>
        public void OnCommitObjectState()
        {
        }
        /// <MetaDataID>{57487f3e-2f7d-4272-b2b9-56d2e31fac32}</MetaDataID>
        public void OnActivate()
        {
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
        [PersistentMember("_StyleSheet")]
        [BackwardCompatibilityID("+9")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
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
            OrgPageStyle = style as PageStyle;
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
                }
            }
        }


        /// <MetaDataID>{677e848a-ed27-42bb-9277-b7920136cf6c}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        Resource? _Background;

        /// <MetaDataID>{a0041b87-3bb8-4542-9fea-322fccb2058b}</MetaDataID>
        [PersistentMember(nameof(_Background))]
        [BackwardCompatibilityID("+1")]
        public Resource Background
        {
            get
            {
                if (OrgPageStyle != null && !_Background.HasValue)
                    return OrgPageStyle.Background;
                if (!_Background.HasValue)
                    return default(Resource);
                return _Background.Value;
            }
            set
            {
                if (_Background != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Background = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Resource? _Border;

        /// <MetaDataID>{d292c2ce-4b8c-41ae-aac5-f54b877bdfb1}</MetaDataID>
        [PersistentMember(nameof(_Border))]
        [BackwardCompatibilityID("+2")]
        public Resource Border
        {
            get
            {
                if (OrgPageStyle != null && !_Border.HasValue)
                    return OrgPageStyle.Border;
                if (!_Border.HasValue)
                    return default(Resource);
                return _Border.Value;
            }

            set
            {
                if (_Border != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Border = value;
                        stateTransition.Consistent = true;
                    }
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
        double? _PageHeigth;

        /// <MetaDataID>{855fbd36-734b-4c1c-8fb6-6d005e908ab3}</MetaDataID>
        [PersistentMember(nameof(_PageHeigth))]
        [BackwardCompatibilityID("+5")]
        public double PageHeight
        {
            get
            {
                if (OrgPageStyle != null && !_PageHeigth.HasValue)
                    return OrgPageStyle.PageHeight;
                if (!_PageHeigth.HasValue)
                    return default(double);
                return _PageHeigth.Value;
            }

            set
            {
                if (_PageHeigth != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PageHeigth = value;
                        stateTransition.Consistent = true;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageHeight)));
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

        /// <MetaDataID>{12c67dbf-f3f3-4adb-8467-4a52594de912}</MetaDataID>
        public void ResetLineSpacing()
        {
            _LineSpacing = (double?)null;
        }
    }


   

   
}