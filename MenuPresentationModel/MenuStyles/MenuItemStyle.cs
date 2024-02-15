using System;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.ComponentModel;
using UIBaseEx;
using FlavourBusinessFacade.RoomService;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{f5bd7723-eb0b-4c9c-bdf5-c8ded473fda4}</MetaDataID>
    [BackwardCompatibilityID("{f5bd7723-eb0b-4c9c-bdf5-c8ded473fda4}")]
    [Persistent()]
    public class MenuItemStyle : IMenuItemStyle
    {


        /// <exclude>Excluded</exclude>
        FontData? _ItemInfoParagraphFont;

        /// <MetaDataID>{7064515a-37e9-45fb-8061-71a23cdba058}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoParagraphFont))]
        [BackwardCompatibilityID("+13")]
        public FontData ItemInfoParagraphFont
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgMenuItemStyle != null && !_ItemInfoParagraphFont.HasValue)
                        return OrgMenuItemStyle.ItemInfoParagraphFont;
                    if (!_ItemInfoParagraphFont.HasValue)
                        return default(FontData);
                    return _ItemInfoParagraphFont.Value;
                }
            }
            set
            {
                if (_ItemInfoParagraphFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoParagraphFont = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(_ItemInfoParagraphFont));

                    Transaction.RunOnTransactionCompleted(() =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(_ItemInfoParagraphFont));
                    });
                }
            }
        }



        /// <exclude>Excluded</exclude>
        FontData? _ItemInfoHeadingFont;



        /// <MetaDataID>{9df4468b-2521-4707-afdf-04d821320866}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoHeadingFont))]
        [BackwardCompatibilityID("+12")]
        public FontData ItemInfoHeadingFont
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgMenuItemStyle != null && !_ItemInfoHeadingFont.HasValue)
                        return OrgMenuItemStyle.ItemInfoHeadingFont;
                    if (!_ItemInfoHeadingFont.HasValue)
                        return default(FontData);
                    return _ItemInfoHeadingFont.Value;
                }
            }
            set
            {
                if (_ItemInfoHeadingFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoHeadingFont = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ItemInfoHeadingFont));
                    Transaction.RunOnTransactionCompleted(() =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(_ItemInfoParagraphFont));
                    });
                }
            }
        }


        /// <MetaDataID>{37733b2d-efc4-4435-82f3-60d7fa11ab3e}</MetaDataID>
        public MenuItemStyle()
        {

        }
        /// <MetaDataID>{74abe2dd-5b25-44cc-b2e9-f2acf9e80dc6}</MetaDataID>
        MenuItemStyle OrgMenuItemStyle;
        /// <MetaDataID>{f7fe6d0e-4522-4bb4-947c-7934ef9dce85}</MetaDataID>
        MenuItemStyle(MenuItemStyle orgMenuItemStyle)
        {
            OrgMenuItemStyle = orgMenuItemStyle;
            _Name = OrgMenuItemStyle.Name;
        }

        /// <MetaDataID>{71385d06-95f4-41fa-ae08-cf247af393e2}</MetaDataID>
        public bool IsDerivedStyle
        {
            get
            {
                if (OrgMenuItemStyle != null)
                    return true;
                else
                    return false;
            }
        }
        /// <MetaDataID>{a30d683e-0339-4fbe-9975-131cac5ffb18}</MetaDataID>
        public void OnCommitObjectState()
        {
        }
        /// <MetaDataID>{fc3f5a3d-f5b7-4cae-a91b-a96e2dec0062}</MetaDataID>
        public void OnActivate()
        {
        }
        /// <MetaDataID>{d6d1f2a5-23df-4439-b694-5c8826afe47f}</MetaDataID>
        public void OnDeleting()
        {
        }
        /// <MetaDataID>{dc68446f-7bd1-4485-a829-dda942929db2}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <MetaDataID>{5bd76912-a41d-417b-819d-e4466cfe3929}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <exclude>Excluded</exclude>
        IStyleSheet _StyleSheet;

        /// <MetaDataID>{a30d84c9-ec68-4113-b814-9caae4fe537a}</MetaDataID>
        [PersistentMember("_StyleSheet")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+11")]
        [OOAdvantech.Json.JsonIgnore]
        public MenuPresentationModel.MenuStyles.IStyleSheet StyleSheet
        {
            get
            {
                return _StyleSheet;
            }
        }


        /// <MetaDataID>{3fa1ba77-58e5-4f04-abbe-50623e40cfe1}</MetaDataID>
        public virtual IStyleRule GetDerivedStyle()
        {
            return new MenuItemStyle(this);
        }
        /// <MetaDataID>{ebcaa05b-9d83-4a9b-a73e-0ca468cf3832}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {
            if (OrgMenuItemStyle != null)
                UseDefaultValues();
            OrgMenuItemStyle = style as MenuItemStyle;

        }

        /// <MetaDataID>{916cda78-1d63-4f53-b821-9f48f8de8139}</MetaDataID>
        public void UseDefaultValues()
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {

                _Font = new MultilingualMember<FontData>();
                _DescriptionFont = new MultilingualMember<FontData>();
                _ExtrasFont = new MultilingualMember<FontData>();
                _AfterSpacing = null;
                _Alignment = null;
                _BeforeSpacing = null;
                _Indent = null;
                _NewLineForDescription = null;
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{331cede6-2399-4b73-b81c-0458dbc13ccc}</MetaDataID>
        public void RestFont()
        {
            if (OrgMenuItemStyle != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Font.ClearValue();
                    _DescriptionFont.ClearValue();
                    _ExtrasFont.ClearValue();
                    stateTransition.Consistent = true;
                }


                //_Font = (FontData?)null;
                //_DescriptionFont = (FontData?)null;
                //_ExtrasFont = (FontData?)null;
            }
        }

        /// <exclude>Excluded</exclude> 
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double? _AfterSpacing;

        /// <MetaDataID>{bb3c1489-969e-48b0-aa53-47e134dc6ae3}</MetaDataID>
        [PersistentMember(nameof(_AfterSpacing))]
        [BackwardCompatibilityID("+1")]
        public double AfterSpacing
        {
            get
            {
                if (OrgMenuItemStyle != null && !_AfterSpacing.HasValue)
                    return OrgMenuItemStyle.AfterSpacing;
                if (!_AfterSpacing.HasValue)
                    return default(double);
                return _AfterSpacing.Value;

            }

            set
            {
                if (_AfterSpacing != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AfterSpacing = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <exclude>Excluded</exclude>
        Alignment? _Alignment;

        /// <MetaDataID>{97e981fd-f531-44db-8eb7-9c0ffa1fa83a}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Alignment))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public MenuPresentationModel.MenuStyles.Alignment Alignment
        {
            get
            {
                if (OrgMenuItemStyle != null && !_Alignment.HasValue)
                    return OrgMenuItemStyle.Alignment;
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
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Alignment)));
                    ObjectChangeState?.Invoke(this, nameof(Alignment));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _BeforeSpacing;

        /// <MetaDataID>{488051e1-cc7c-4743-8b51-6b02820f6e12}</MetaDataID>
        [PersistentMember(nameof(_BeforeSpacing))]
        [BackwardCompatibilityID("+3")]
        public double BeforeSpacing
        {
            get
            {
                if (OrgMenuItemStyle != null && !_BeforeSpacing.HasValue)
                    return OrgMenuItemStyle.BeforeSpacing;
                if (!_BeforeSpacing.HasValue)
                    return default(double);
                return _BeforeSpacing.Value;
            }

            set
            {

                if (_BeforeSpacing != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BeforeSpacing = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<FontData> _Font = new MultilingualMember<FontData>();
        /// <MetaDataID>{5a78b1cc-ee93-4efe-a240-681e2c9edb69}</MetaDataID>
        [PersistentMember(nameof(_Font))]
        [BackwardCompatibilityID("+4")]
        public FontData Font
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgMenuItemStyle != null && !_Font.HasValue)
                        return OrgMenuItemStyle.Font;
                    if (!_Font.HasValue)
                        return default(FontData);
                    return _Font.Value;
                }
            }
            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_Font != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _Font.Value = value;
                            stateTransition.Consistent = true;
                        }
                        ObjectChangeState?.Invoke(this, nameof(Font));
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _Indent;

        /// <MetaDataID>{f2bd4a54-a136-4082-a410-dd4033752306}</MetaDataID>
        [PersistentMember(nameof(_Indent))]
        [BackwardCompatibilityID("+5")]
        public double Indent
        {
            get
            {
                if (OrgMenuItemStyle != null && !_Indent.HasValue)
                    return OrgMenuItemStyle.Indent;
                if (!_Indent.HasValue)
                    return default(double);
                return _Indent.Value;
            }

            set
            {

                if (_Indent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Indent = value;
                        stateTransition.Consistent = true;
                    }
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Indent)));
                    ObjectChangeState?.Invoke(this, nameof(Indent));
                }

            }
        }
        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{4566c16d-b038-424b-98a9-5a0843c47327}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+6")]
        public string Name
        {
            get
            {
                if (OrgMenuItemStyle != null && _Name == null)
                    return OrgMenuItemStyle.Name;
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
        OOAdvantech.MultilingualMember<FontData> _ExtrasFont = new MultilingualMember<FontData>();
        /// <MetaDataID>{32f23d54-0b36-4f13-bf8e-d020c7878278}</MetaDataID>
        [PersistentMember(nameof(_ExtrasFont))]
        [BackwardCompatibilityID("+7")]
        public FontData ExtrasFont
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgMenuItemStyle != null && !_ExtrasFont.HasValue)
                        return OrgMenuItemStyle.ExtrasFont;
                    if (!_ExtrasFont.HasValue)
                        return default(FontData);
                    return _ExtrasFont.Value;
                }
            }

            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_ExtrasFont != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _ExtrasFont.Value = value;
                            stateTransition.Consistent = true;
                        }
                        ObjectChangeState?.Invoke(this, nameof(ExtrasFont));
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        MultilingualMember<FontData> _DescriptionFont = new MultilingualMember<FontData>();


        /// <MetaDataID>{cee014ad-5b7a-4a4f-a07a-edfedb5eaca9}</MetaDataID>
        [PersistentMember(nameof(_DescriptionFont))]
        [BackwardCompatibilityID("+8")]
        public FontData DescriptionFont
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgMenuItemStyle != null && !_DescriptionFont.HasValue)
                        return OrgMenuItemStyle.DescriptionFont;
                    if (!_DescriptionFont.HasValue)
                        return default(FontData);
                    return _DescriptionFont.Value;
                }
            }
            set
            {
                using (new CultureContext(CultureContext.CurrentCultureInfo, false))
                {
                    if (_DescriptionFont != value)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            _DescriptionFont.Value = value;
                            stateTransition.Consistent = true;
                        }
                        ObjectChangeState?.Invoke(this, nameof(DescriptionFont));
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _NewLineForDescription;

        public event ObjectChangeStateHandle ObjectChangeState;
        //public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{493f1ebb-6c0b-4ad6-a9fe-74e2727f06c1}</MetaDataID>
        [PersistentMember("_NewLineForDescription")]
        [BackwardCompatibilityID("+10")]
        public bool NewLineForDescription
        {
            get
            {
                if (OrgMenuItemStyle != null && !_NewLineForDescription.HasValue)
                    return OrgMenuItemStyle.NewLineForDescription;
                if (!_NewLineForDescription.HasValue)
                    return default(bool);
                return _NewLineForDescription.Value;
            }

            set
            {
                if (_NewLineForDescription != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _NewLineForDescription = value;
                        stateTransition.Consistent = true;
                    }
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewLineForDescription)));
                    ObjectChangeState?.Invoke(this, nameof(NewLineForDescription));

                }
            }
        }
        /// <exclude>Excluded</exclude>
        FontData? _ItemInfoParagraphFirstLetterFont;

        /// <MetaDataID>{acf2b8d0-74c9-4fd5-b7e3-78de06bcd295}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoParagraphFirstLetterFont))]
        [BackwardCompatibilityID("+14")]
        public FontData? ItemInfoParagraphFirstLetterFont
        {
            get => _ItemInfoParagraphFirstLetterFont; set
            {
                if (_ItemInfoParagraphFirstLetterFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoParagraphFirstLetterFont = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ItemInfoFirstLetterLeftIndent));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        int? _ItemInfoFirstLetterLeftIndent;

        /// <MetaDataID>{7e9038af-14c2-44ce-aa5a-58e40f94e5ce}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoFirstLetterLeftIndent))]
        [BackwardCompatibilityID("+15")]
        public int? ItemInfoFirstLetterLeftIndent
        {
            get
            {
                if (OrgMenuItemStyle != null && !_ItemInfoFirstLetterLeftIndent.HasValue)
                    return OrgMenuItemStyle.ItemInfoFirstLetterLeftIndent;
                if (!_ItemInfoFirstLetterLeftIndent.HasValue)
                    return default(int);
                return _ItemInfoFirstLetterLeftIndent.Value;
            }

            set
            {
                if (_ItemInfoFirstLetterLeftIndent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoFirstLetterLeftIndent = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ItemInfoFirstLetterLeftIndent));

                }
            }
        }


        /// <exclude>Excluded</exclude>
        int? _ItemInfoFirstLetterRightIndent;

        /// <MetaDataID>{d92a9ff7-0db3-42a8-8edd-3a7071fd0133}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoFirstLetterRightIndent))]
        [BackwardCompatibilityID("+17")]
        public int? ItemInfoFirstLetterRightIndent
        {
            get
            {
                if (OrgMenuItemStyle != null && !_ItemInfoFirstLetterRightIndent.HasValue)
                    return OrgMenuItemStyle.ItemInfoFirstLetterRightIndent;
                if (!_ItemInfoFirstLetterRightIndent.HasValue)
                    return default(int);
                return _ItemInfoFirstLetterRightIndent.Value;
            }

            set
            {
                if (_ItemInfoFirstLetterRightIndent != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoFirstLetterRightIndent = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ItemInfoFirstLetterRightIndent));

                }
            }
        }



        /// <exclude>Excluded</exclude>
        int? _ItemInfoFirstLetterLinesSpan;
        /// <MetaDataID>{73e69a58-59c2-418f-a0a0-f2a2d92da7e2}</MetaDataID>
        [PersistentMember(nameof(_ItemInfoFirstLetterLinesSpan))]
        [BackwardCompatibilityID("+16")]
        public int? ItemInfoFirstLetterLinesSpan
        {
            get
            {
                if (OrgMenuItemStyle != null && !_ItemInfoFirstLetterLinesSpan.HasValue)
                    return OrgMenuItemStyle.ItemInfoFirstLetterLinesSpan;
                if (!_ItemInfoFirstLetterLinesSpan.HasValue)
                    return 1;
                return _ItemInfoFirstLetterLinesSpan.Value;
            }

            set
            {
                if (_ItemInfoFirstLetterLinesSpan != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemInfoFirstLetterLinesSpan = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ItemInfoFirstLetterLinesSpan));

                }
            }
        }
    
    }
}