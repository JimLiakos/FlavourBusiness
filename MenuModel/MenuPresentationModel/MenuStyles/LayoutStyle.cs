using System;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{15d86b89-2d28-4973-a757-8a98a3b5a780}</MetaDataID>
    [BackwardCompatibilityID("{15d86b89-2d28-4973-a757-8a98a3b5a780}")]
    [Persistent()]
    public class LayoutStyle : ILayoutStyle
    {
        /// <MetaDataID>{dd5946d0-de89-414f-b1b5-924342d98913}</MetaDataID>
        public LayoutStyle()
        {

        }
        /// <MetaDataID>{2b0507ce-f58b-4bbc-923d-0ff4bdcbc184}</MetaDataID>
        LayoutStyle OrgLayoutStyle;
        /// <MetaDataID>{e29d18aa-a718-43be-9109-9d3452fc770e}</MetaDataID>
        LayoutStyle(LayoutStyle orgLayoutStyle)
        {
            OrgLayoutStyle = orgLayoutStyle;
            _Name = OrgLayoutStyle.Name;
        }


        /// <MetaDataID>{589edb9d-8596-4be5-bf30-f99011fba95d}</MetaDataID>
        public void OnCommitObjectState()
        {
        }
        /// <MetaDataID>{fd71562b-3243-4762-9d14-f9c5c3a3a4ff}</MetaDataID>
        public void OnActivate()
        {
        }
        /// <MetaDataID>{ad985849-75c9-4a63-bd04-46b9441feac8}</MetaDataID>
        public void OnDeleting()
        {
        }
        /// <MetaDataID>{0ae0e630-d51d-4e44-bcc9-59a0e7083c45}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <MetaDataID>{ca3f4081-c0e2-432e-9daf-a2337ce34eef}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <exclude>Excluded</exclude>
        IStyleSheet _StyleSheet;
        /// <MetaDataID>{a30d84c9-ec68-4113-b814-9caae4fe537a}</MetaDataID>
        [PersistentMember("_StyleSheet")]
        [BackwardCompatibilityID("+12")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public MenuPresentationModel.MenuStyles.IStyleSheet StyleSheet
        {
            get
            {
                return _StyleSheet;
            }
        }


        /// <MetaDataID>{f0411746-1ae9-4447-83d2-2d618c831560}</MetaDataID>
        public virtual IStyleRule GetDerivedStyle()
        {
            return new LayoutStyle(this);
        }
        /// <MetaDataID>{4b924b48-15b7-4732-84fa-8c391a3d9ad7}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {
            OrgLayoutStyle = style as LayoutStyle;
        }

        /// <exclude>Excluded</exclude> 
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double? _DescLeftIndent;
        /// <MetaDataID>{91e60843-6bee-4742-b58a-e731bf370d1d}</MetaDataID>
        [PersistentMember("_DescLeftIndent")]
        [BackwardCompatibilityID("+1")]
        public double DescLeftIndent
        {
            get
            {
                if (OrgLayoutStyle != null && !_DescLeftIndent.HasValue)
                    return OrgLayoutStyle.DescLeftIndent;
                if (!_DescLeftIndent.HasValue)
                    return default(double);
                return _DescLeftIndent.Value;
            }

            set
            {

                if (_DescLeftIndent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DescLeftIndent = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _DescRightIndent;
        /// <MetaDataID>{3a6a8c3a-6904-4c9d-8fad-e4966a1d2da7}</MetaDataID>
        [PersistentMember("_DescRightIndent")]
        [BackwardCompatibilityID("+2")]
        public double DescRightIndent
        {
            get
            {
                if (OrgLayoutStyle != null && !_DescRightIndent.HasValue)
                    return OrgLayoutStyle.DescRightIndent;
                if (!_DescRightIndent.HasValue)
                    return default(double);
                return _DescRightIndent.Value;
            }

            set
            {

                if (_DescRightIndent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DescRightIndent = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _DescSeparator;
        /// <MetaDataID>{932d3120-01b8-44b7-bff9-6481ebef3c9e}</MetaDataID>
        [PersistentMember("_DescSeparator")]
        [BackwardCompatibilityID("+3")]
        public string DescSeparator
        {
            get
            {
                return _DescSeparator;
            }

            set
            {

                if (_DescSeparator != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DescSeparator = value;
                        stateTransition.Consistent = true;
                    }
                }


            }
        }

        /// <exclude>Excluded</exclude>
        double? _ExtrasLeftIndent;

        /// <MetaDataID>{d54cdcd0-d0aa-4fb4-a6b3-be838ba1d313}</MetaDataID>
        [PersistentMember("_ExtrasLeftIndent")]
        [BackwardCompatibilityID("+4")]
        public double ExtrasLeftIndent
        {
            get
            {
                if (OrgLayoutStyle != null && !_ExtrasLeftIndent.HasValue)
                    return OrgLayoutStyle.ExtrasLeftIndent;
                if (!_ExtrasLeftIndent.HasValue)
                    return default(double);
                return _ExtrasLeftIndent.Value;
            }

            set
            {

                if (_ExtrasLeftIndent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ExtrasLeftIndent = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _ExtrasSeparator;

        /// <MetaDataID>{6e12bfb6-c1ae-4a59-aea5-e8ec451ab51f}</MetaDataID>
        [PersistentMember("_ExtrasSeparator")]
        [BackwardCompatibilityID("+5")]
        public string ExtrasSeparator
        {
            get
            {
                if (OrgLayoutStyle != null && _ExtrasSeparator == null)
                    return OrgLayoutStyle.ExtrasSeparator;
                return _ExtrasSeparator;
            }

            set
            {

                if (_ExtrasSeparator != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ExtrasSeparator = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _LineBetweenColumns;
        /// <MetaDataID>{e72337b5-e739-49d9-98a8-6da0cf396c7f}</MetaDataID>
        [PersistentMember("_LineBetweenColumns")]
        [BackwardCompatibilityID("+6")]
        public string LineBetweenColumns
        {
            get
            {
                if (OrgLayoutStyle != null && _LineBetweenColumns == null)
                    return OrgLayoutStyle.LineBetweenColumns;
                return _LineBetweenColumns;
            }

            set
            {

                if (_LineBetweenColumns != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LineBetweenColumns = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <exclude>Excluded</exclude>
        double? _LineSpacing;
        /// <MetaDataID>{b4a2df2f-2df9-49c7-8525-8b8c3ee9ff55}</MetaDataID>
        [PersistentMember("_LineSpacing")]
        [BackwardCompatibilityID("+7")]
        public double LineSpacing
        {
            get
            {
                if (OrgLayoutStyle != null && !_LineSpacing.HasValue)
                    return OrgLayoutStyle.LineSpacing;
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

        /// <exclude>Excluded</exclude>
        string _LineType;
        /// <MetaDataID>{0a029eb6-cb06-45cc-a476-308b1afea977}</MetaDataID>
        [PersistentMember("_LineType")]
        [BackwardCompatibilityID("+8")]
        public string LineType
        {
            get
            {
                if (OrgLayoutStyle!= null && _LineType == null)
                    return OrgLayoutStyle.LineType;
                return _LineType;
            }

            set
            {

                if (_LineType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _LineType = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{9687dc6c-acc8-44e0-8029-b8fb9413aef5}</MetaDataID>
        [PersistentMember("_Name")]
        [BackwardCompatibilityID("+9")]
        public string Name
        {
            get
            {
                if (OrgLayoutStyle!= null && _Name == null)
                    return OrgLayoutStyle.Name;
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
        double? _NameIndent;
        /// <MetaDataID>{74fad8b1-7360-4169-ab5e-24bbb170d459}</MetaDataID>
        [PersistentMember("_NameIndent")]
        [BackwardCompatibilityID("+10")]
        public double NameIndent
        {
            get
            {
                if (OrgLayoutStyle != null && !_NameIndent.HasValue)
                    return OrgLayoutStyle.NameIndent;
                if (!_NameIndent.HasValue)
                    return default(double);
                return _NameIndent.Value;
            }

            set
            {

                if (_NameIndent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NameIndent = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _SpaceBetweenColumns;

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{08e8eed7-434b-4c45-af9c-d42eeebd93e2}</MetaDataID>
        [PersistentMember("_SpaceBetweenColumns")]
        [BackwardCompatibilityID("+11")]
        public double SpaceBetweenColumns
        {
            get
            {
                if (OrgLayoutStyle != null && !_SpaceBetweenColumns.HasValue)
                    return OrgLayoutStyle.SpaceBetweenColumns; 
                if (!_SpaceBetweenColumns.HasValue)
                    return default(double);
                return _SpaceBetweenColumns.Value;
            }

            set
            {
                if (_SpaceBetweenColumns != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SpaceBetweenColumns = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}