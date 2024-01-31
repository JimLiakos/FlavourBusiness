using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Windows.Media;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{f08312df-4340-488a-b110-58d7fdeecc22}</MetaDataID>
    [BackwardCompatibilityID("{f08312df-4340-488a-b110-58d7fdeecc22}")]
    [Persistent()]
    public class MenuItemInfoStyle : IMenuItemInfoStyle
    {



        /// <MetaDataID>{98e78c59-5c5f-402a-ba98-07f67b770bf3}</MetaDataID>
        public MenuItemInfoStyle()
        {
        }



        MenuItemInfoStyle OrgMenuItemInfoStyle;
        /// <MetaDataID>{dad6aacd-ec93-49b6-a255-c8c0b2ddc434}</MetaDataID>
        MenuItemInfoStyle(MenuItemInfoStyle orgMenuItemInfoStyle)
        {
            OrgMenuItemInfoStyle = orgMenuItemInfoStyle;
            _Name = orgMenuItemInfoStyle.Name;
        }



        /// <exclude>Excluded</exclude>
        FontData? _HeadingFont;
        /// <MetaDataID>{58c3bcb5-d84e-4395-b545-a5ffe02d3e85}</MetaDataID>
        [PersistentMember(nameof(_HeadingFont))]
        [BackwardCompatibilityID("+3")]
        public FontData HeadingFont 
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgMenuItemInfoStyle != null && !_ParagraphFont.HasValue)
                        return OrgMenuItemInfoStyle.HeadingFont;
                    if (!_HeadingFont.HasValue)
                        return default(FontData);
                    return _HeadingFont.Value;
                }
            }
            set
            {
                if (_HeadingFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _HeadingFont = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(HeadingFont));
                }
            }
        }



        /// <exclude>Excluded</exclude>
        FontData? _ParagraphFont;
        /// <MetaDataID>{89b8a884-ac1c-496f-90d1-f7e66cf16546}</MetaDataID>
        [PersistentMember(nameof(_ParagraphFont))]
        [BackwardCompatibilityID("+4")]
        public FontData ParagraphFont
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgMenuItemInfoStyle != null && !_ParagraphFont.HasValue)
                        return OrgMenuItemInfoStyle.ParagraphFont;
                    if (!_ParagraphFont.HasValue)
                        return default(FontData);
                    return _ParagraphFont.Value;
                }
            }
            set
            {
                if (_ParagraphFont != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ParagraphFont = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ParagraphFont));
                }
            }
        }


        /// <exclude>Excluded</exclude>
        IStyleSheet _StyleSheet;

        /// <MetaDataID>{2a88ad98-fb4c-4431-910a-537fc2000809}</MetaDataID>
        [PersistentMember(nameof(_StyleSheet))]
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [OOAdvantech.Json.JsonIgnore]
        public MenuPresentationModel.MenuStyles.IStyleSheet StyleSheet => _StyleSheet;

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{818110b5-2fa7-4ecf-9450-7bb81c0fce9c}</MetaDataID>
        string _Name;

        /// <MetaDataID>{6e79edc9-6785-45f6-a9dc-597b50a48cc4}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+2")]
        public string Name
        {
            get => _Name;
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

        /// <MetaDataID>{bede3e95-4d42-4752-b710-e5d9fb9f8eb5}</MetaDataID>
        public bool IsDerivedStyle
        {
            get
            {
                if (OrgMenuItemInfoStyle!= null)
                    return true;
                else
                    return false;
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{9d7879b2-ceb2-4190-a313-794dc84e32c6}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {
            if (OrgMenuItemInfoStyle != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    UseDefaultValues();
                    stateTransition.Consistent = true;
                }

            }
            OrgMenuItemInfoStyle = style as MenuItemInfoStyle;
        }

        /// <MetaDataID>{c34a6c6f-12d6-4aff-bc3a-fbda98139642}</MetaDataID>
        public void UseDefaultValues()
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HeadingFont = null;
                _ParagraphFont = null;
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{83dbf5dd-6ed1-4cc9-859c-8192c682c30b}</MetaDataID>
        public virtual IStyleRule GetDerivedStyle()
        {
            return new MenuItemInfoStyle(this);
        }
    }
}