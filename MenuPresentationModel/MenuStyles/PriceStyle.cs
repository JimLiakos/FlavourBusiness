using System;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.ComponentModel;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{3821da7a-7021-4bfd-9544-207a183abbea}</MetaDataID>
    [BackwardCompatibilityID("{3821da7a-7021-4bfd-9544-207a183abbea}")]
    [Persistent()]
    public class PriceStyle : MarshalByRefObject, IPriceStyle
    {
        /// <exclude>Excluded</exclude>
        double? _MultiPriceSpacing;

        /// <MetaDataID>{e9b1e6a1-3d5b-403d-a64f-df406beb5e49}</MetaDataID>
        [PersistentMember(nameof(_MultiPriceSpacing))]
        [BackwardCompatibilityID("+17")]
        public double MultiPriceSpacing
        {
            get
            {
                if (OrgPriceStyle != null && !_MultiPriceSpacing.HasValue)
                    return OrgPriceStyle.MultiPriceSpacing;
                if (!_MultiPriceSpacing.HasValue)
                {
                    FontData font;
                    if (Layout == MenuStyles.PriceLayout.WithDescription || Layout == MenuStyles.PriceLayout.FollowDescription)
                        font = (this.StyleSheet.Styles["menu-item"] as MenuStyles.IMenuItemStyle).DescriptionFont;
                    else
                        font = Font;
                    return font.MeasureText(" ").Width;
                }
                return _MultiPriceSpacing.Value;

            }
            set
            {

                if (_MultiPriceSpacing != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MultiPriceSpacing = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(MultiPriceSpacing));
                }

            }
        }


        /// <exclude>Excluded</exclude>
        double? _PriceHeadingsBottomMargin;

        /// <MetaDataID>{e72bc6e7-b14e-4f5c-99c0-2639b678572c}</MetaDataID>
        [PersistentMember(nameof(_PriceHeadingsBottomMargin))]
        [BackwardCompatibilityID("+16")]
        public double PriceHeadingsBottomMargin
        {
            get
            {
                if (OrgPriceStyle != null && !_PriceHeadingsBottomMargin.HasValue)
                    return OrgPriceStyle.PriceHeadingsBottomMargin;
                if (!_PriceHeadingsBottomMargin.HasValue)
                    return default(double);
                return _PriceHeadingsBottomMargin.Value;
            }
            set
            {

                if (_PriceHeadingsBottomMargin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceHeadingsBottomMargin = value;
                        stateTransition.Consistent = true;
                    }
                }
                ObjectChangeState?.Invoke(this, nameof(PriceHeadingsBottomMargin));
            }
        }

        /// <exclude>Excluded</exclude>
        MenuCanvas.TransformOrigin? _PriceHeadingTransformOrigin;

        /// <MetaDataID>{94313987-2ecf-422a-8805-7241831b92a6}</MetaDataID>
        [PersistentMember(nameof(_PriceHeadingTransformOrigin))]
        [BackwardCompatibilityID("+14")]
        public MenuCanvas.TransformOrigin PriceHeadingTransformOrigin
        {
            get
            {
                if (OrgPriceStyle != null && !_PriceHeadingTransformOrigin.HasValue)
                    return OrgPriceStyle.PriceHeadingTransformOrigin;
                if (!_PriceHeadingTransformOrigin.HasValue)
                    return new MenuCanvas.TransformOrigin("left", "bottom");
                return _PriceHeadingTransformOrigin.Value;

            }
            set
            {

                if (_PriceHeadingTransformOrigin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceHeadingTransformOrigin = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(PriceHeadingTransformOrigin));
                }

            }
        }
        /// <exclude>Excluded</exclude>
        double? _PriceHeadingAngle;

        /// <MetaDataID>{b7f21f3f-adc6-4928-9e84-711f50e4aaae}</MetaDataID>
        [PersistentMember(nameof(_PriceHeadingAngle))]
        [BackwardCompatibilityID("+13")]
        public double PriceHeadingAngle
        {
            get
            {
                if (OrgPriceStyle != null && !_PriceHeadingAngle.HasValue)
                    return OrgPriceStyle.PriceHeadingAngle;
                if (!_PriceHeadingAngle.HasValue)
                    return 310;
                return _PriceHeadingAngle.Value;
            }

            set
            {

                if (_PriceHeadingAngle != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceHeadingAngle = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(PriceHeadingAngle));
                }

            }
        }



        /// <MetaDataID>{a5b4f74e-9697-4bf2-a18f-39a2aef0c3d8}</MetaDataID>
        public PriceStyle()
        {
        }
        /// <MetaDataID>{45b6d356-589a-4982-8a79-557fadc61931}</MetaDataID>
        PriceStyle OrgPriceStyle;
        /// <MetaDataID>{650afb34-c09e-47c0-adb4-a38671cd9e45}</MetaDataID>
        PriceStyle(PriceStyle orgPriceStyle)
        {
            OrgPriceStyle = orgPriceStyle;
            _Name = orgPriceStyle.Name;
        }


        /// <MetaDataID>{8e7c9f14-f12e-4cd3-b882-a518450ea834}</MetaDataID>
        public void OnCommitObjectState()
        {
        }
        /// <MetaDataID>{69e350b7-9c3d-4c8d-a861-1d6e483168a4}</MetaDataID>
        public void OnActivate()
        {
        }
        /// <MetaDataID>{1c2c3398-12c9-45a2-8824-0d184bb9a27b}</MetaDataID>
        public void OnDeleting()
        {
        }
        /// <MetaDataID>{28b0e66e-bb90-4e23-8d78-dfced7b8bcbe}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <MetaDataID>{660a996d-49e2-4643-ba1d-c70027472e51}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {
        }
        /// <exclude>Excluded</exclude>
        IStyleSheet _StyleSheet;
        /// <MetaDataID>{a30d84c9-ec68-4113-b814-9caae4fe537a}</MetaDataID>
        [PersistentMember("_StyleSheet")]
        [BackwardCompatibilityID("+11")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [OOAdvantech.Json.JsonIgnore]
        public MenuPresentationModel.MenuStyles.IStyleSheet StyleSheet
        {
            get
            {
                return _StyleSheet;
            }
        }

        /// <MetaDataID>{72b84108-1d8d-4cfe-9999-61b4298a8f84}</MetaDataID>
        public virtual IStyleRule GetDerivedStyle()
        {
            return new PriceStyle(this);
        }

        /// <MetaDataID>{4797a588-92cf-48f3-9e8d-81578ba24997}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {

            if (OrgPriceStyle != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    UseDefaultValues();
                    stateTransition.Consistent = true;
                }

            }
            OrgPriceStyle = style as PriceStyle;


        }

        public void UseDefaultValues()
        {
            _DisplayCurrencySymbol = null;

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _BetweenDotsSpace = null;
                _DisplayCurrencySymbol = null;
                _DotsMatchNameColor = null;
                _DotsSpaceFromItem = null;
                _DotsSpaceFromPrice = null;
                _Font = null;
                _Layout = null;
                _MultiPriceSpacing = null;
                _PriceHeadingAngle = null;
                _PriceHeadingHorizontalPos = null;
                _PriceHeadingsBottomMargin = null;
                _PriceHeadingTransformOrigin = null;
                _PriceLeader = null;
                _ShowMultiplePrices = null; 
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{790b336b-38a7-45c0-8bee-85dd030fdbe4}</MetaDataID>
        public void RestFont()
        {
            if (OrgPriceStyle != null)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Font = (FontData?)null; 
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        bool? _DisplayCurrencySymbol;

        /// <MetaDataID>{97707ffe-e54f-4892-bb84-bc97274a083c}</MetaDataID>
        [PersistentMember(nameof(_DisplayCurrencySymbol))]
        [BackwardCompatibilityID("+1")]
        public bool DisplayCurrencySymbol
        {
            get
            {
                if (OrgPriceStyle != null && !_DisplayCurrencySymbol.HasValue)
                    return OrgPriceStyle.DisplayCurrencySymbol;
                if (!_DisplayCurrencySymbol.HasValue)
                    return default(bool);
                return _DisplayCurrencySymbol.Value;
            }

            set
            {

                if (_DisplayCurrencySymbol != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DisplayCurrencySymbol = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool? _DotsMatchNameColor;
        /// <MetaDataID>{1db520f7-315e-48ec-8e2e-b32416143b8a}</MetaDataID>
        [PersistentMember(nameof(_DotsMatchNameColor))]
        [BackwardCompatibilityID("+2")]
        public bool DotsMatchNameColor
        {
            get
            {
                if (OrgPriceStyle != null && !_DotsMatchNameColor.HasValue)
                    return OrgPriceStyle.DotsMatchNameColor;
                if (!_DotsMatchNameColor.HasValue)
                    return default(bool);
                return _DotsMatchNameColor.Value;
            }

            set
            {
                if (_DotsMatchNameColor != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DotsMatchNameColor = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        PriceLayout? _Layout;
        /// <MetaDataID>{10c8cc40-b7c9-44a3-b131-88a48772086f}</MetaDataID>
        [PersistentMember(nameof(_Layout))]
        [BackwardCompatibilityID("+3")]
        public PriceLayout Layout
        {
            get
            {
                if (OrgPriceStyle != null && !_Layout.HasValue)
                    return OrgPriceStyle.Layout;
                if (!_Layout.HasValue)
                    return default(PriceLayout);
                return _Layout.Value;

            }

            set
            {
                if (_Layout != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Layout = value;
                        stateTransition.Consistent = true;
                    }
                    //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Layout)));
                    ObjectChangeState?.Invoke(this, nameof(Layout));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        FontData? _Font;

        /// <MetaDataID>{022a7274-67aa-4172-a9bd-f07c3e722f46}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [PersistentMember("_Font")]
        public FontData Font
        {
            get
            {
                using (CultureContext cultureContext = new CultureContext(CultureContext.CurrentCultureInfo, true))
                {
                    if (OrgPriceStyle != null && !_Font.HasValue)
                        return OrgPriceStyle.Font;
                    if (!_Font.HasValue)
                        return default(FontData);
                    return _Font.Value;
                }
            }
            set
            {
                if (_Font != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Font = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(Font));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{eabbde66-ca29-43b0-8cc0-2fec09d42bb5}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+4")]
        public string Name
        {
            get
            {
                if (OrgPriceStyle != null && _Name == null)
                    return OrgPriceStyle.Name;
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
        string _PriceLeader;
        /// <MetaDataID>{2c8e20bf-6ee1-4601-a144-b9b8494711a0}</MetaDataID>
        [PersistentMember(nameof(_PriceLeader))]
        [BackwardCompatibilityID("+5")]
        public string PriceLeader
        {
            get
            {
                if (OrgPriceStyle != null && _PriceLeader == null)
                    return OrgPriceStyle.PriceLeader;
                return _PriceLeader;
            }

            set
            {
                if (_PriceLeader != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceLeader = value;
                        if (value != null && value.ToLower() == "dots")
                            _PriceLeader = "dots";
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(PriceHeadingTransformOrigin));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        int? _BetweenDotsSpace;
        /// <MetaDataID>{e2d42cec-17c4-464d-af43-2071174745ae}</MetaDataID>
        [PersistentMember(nameof(_BetweenDotsSpace))]
        [BackwardCompatibilityID("+6")]
        public int BetweenDotsSpace
        {
            get
            {
                if (OrgPriceStyle != null && !_BetweenDotsSpace.HasValue)
                    return OrgPriceStyle.BetweenDotsSpace;
                if (!_BetweenDotsSpace.HasValue)
                    return default(int);
                return _BetweenDotsSpace.Value;
            }
            set
            {
                if (_BetweenDotsSpace != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BetweenDotsSpace = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(BetweenDotsSpace));
                }
            }
        }
        /// <exclude>Excluded</exclude>
        double? _DotsSpaceFromPrice;

        /// <MetaDataID>{f5a86789-68f6-42dd-9c8f-bbeb3b8e5f90}</MetaDataID>
        [PersistentMember(nameof(_DotsSpaceFromPrice))]
        [BackwardCompatibilityID("+7")]
        public double DotsSpaceFromPrice
        {
            get
            {
                if (OrgPriceStyle != null && !_DotsSpaceFromPrice.HasValue)
                    return OrgPriceStyle.DotsSpaceFromPrice;
                if (!_DotsSpaceFromPrice.HasValue)
                    return default(int);
                return _DotsSpaceFromPrice.Value;
            }

            set
            {

                if (_DotsSpaceFromPrice != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DotsSpaceFromPrice = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(DotsSpaceFromPrice));
                }

            }
        }

        /// <exclude>Excluded</exclude>
        double? _DotsSpaceFromItem;

        public event ObjectChangeStateHandle ObjectChangeState;
        //public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{f36ce0c1-af98-4c6e-a29a-d46eb21252ca}</MetaDataID>
        [PersistentMember(nameof(_DotsSpaceFromItem))]
        [BackwardCompatibilityID("+8")]
        public double DotsSpaceFromItem
        {
            get
            {
                if (OrgPriceStyle != null && !_DotsSpaceFromItem.HasValue)
                    return OrgPriceStyle.DotsSpaceFromItem;
                if (!_DotsSpaceFromItem.HasValue)
                    return default(int);
                return _DotsSpaceFromItem.Value;
            }
            set
            {

                if (_DotsSpaceFromItem != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DotsSpaceFromItem = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(DotsSpaceFromItem));
                }

            }
        }

        /// <MetaDataID>{2446003e-a273-4bdc-a762-43a5f1c0986a}</MetaDataID>
        public bool IsDerivedStyle
        {
            get
            {
                if (OrgPriceStyle != null)
                    return true;
                else
                    return false;
            }
        }

        /// <exclude>Excluded</exclude>
        double? _PriceHeadingHorizontalPos;

        /// <MetaDataID>{d10705a9-b006-4181-9aa2-e51972a87c92}</MetaDataID>
        [PersistentMember(nameof(_PriceHeadingHorizontalPos))]
        [BackwardCompatibilityID("+15")]
        public double PriceHeadingHorizontalPos
        {
            get
            {
                if (OrgPriceStyle != null && !_PriceHeadingHorizontalPos.HasValue)
                    return OrgPriceStyle.PriceHeadingHorizontalPos;
                if (!_PriceHeadingHorizontalPos.HasValue)
                    return 50;
                return _PriceHeadingHorizontalPos.Value;
            }
            set
            {
                if (_PriceHeadingHorizontalPos != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PriceHeadingHorizontalPos = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(PriceHeadingHorizontalPos));
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool? _ShowMultiplePrices;
        /// <MetaDataID>{7cbc6a3c-3bfc-4c6f-815a-895da77995ed}</MetaDataID>
        [PersistentMember(nameof(_ShowMultiplePrices))]
        [BackwardCompatibilityID("+18")]
        public bool ShowMultiplePrices
        {
            get
            {
                if (OrgPriceStyle != null && !_ShowMultiplePrices.HasValue)
                    return OrgPriceStyle.ShowMultiplePrices;
                if (!_ShowMultiplePrices.HasValue)
                    return false;
                return _ShowMultiplePrices.Value;
            }
            set
            {

                if (_ShowMultiplePrices != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ShowMultiplePrices = value;
                        stateTransition.Consistent = true;
                    }
                    ObjectChangeState?.Invoke(this, nameof(ShowMultiplePrices));
                }
            }
        }
    }
}