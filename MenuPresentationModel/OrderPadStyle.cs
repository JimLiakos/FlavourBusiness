using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.ComponentModel;
using System.Windows.Controls;
using UIBaseEx;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{ad122580-5256-4855-9299-49f6ab15595e}</MetaDataID>
    [BackwardCompatibilityID("{ad122580-5256-4855-9299-49f6ab15595e}")]
    [Persistent()]
    public class OrderPadStyle : IOrderPadStyle, INotifyPropertyChanged
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        public OrderPadStyle()
        { }
        /// <MetaDataID>{475670fb-28eb-4fa3-bf27-74f2867155bb}</MetaDataID>
        OrderPadStyle OrgOrderPadStyle;
        /// <MetaDataID>{74b16fe8-db3d-49dc-9d62-54218b2ca9b2}</MetaDataID>
        OrderPadStyle(OrderPadStyle orgOrderPadStyle)
        {
            OrgOrderPadStyle = orgOrderPadStyle;
            _Name = OrgOrderPadStyle.Name;
        }

        /// <exclude>Excluded</exclude>
        IPageImage _Background;

        /// <MetaDataID>{9c76fbeb-726d-4200-8512-03ef201bbdc6}</MetaDataID>
        [PersistentMember(nameof(_Background))]
        [BackwardCompatibilityID("+1")]
        public MenuPresentationModel.MenuStyles.IPageImage Background
        {
            get => _Background;
            set
            {
                if (_Background!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Background=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Margin? _BackgroundMargin;

        /// <MetaDataID>{a7fa4f3d-b041-49b0-9a1b-5da7ea1e69e5}</MetaDataID>
        [PersistentMember(nameof(_BackgroundMargin))]
        [BackwardCompatibilityID("+2")]
        public UIBaseEx.Margin BackgroundMargin
        {
            get
            {

                if (OrgOrderPadStyle != null && !_BackgroundMargin.HasValue)
                    return OrgOrderPadStyle.BackgroundMargin;
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

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{8bdf1873-dbd5-45e0-b67d-0cf5b0f6beee}</MetaDataID>
        ImageStretch? _BackgroundStretch;

        public event PropertyChangedEventHandler PropertyChanged;
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{1a41aac7-0ab2-4cb1-89cc-66aad5c290f6}</MetaDataID>
        [PersistentMember(nameof(_BackgroundStretch))]
        [BackwardCompatibilityID("+3")]
        public MenuPresentationModel.MenuStyles.ImageStretch BackgroundStretch
        {
            get
            {
                if (OrgOrderPadStyle!= null && !_BackgroundStretch.HasValue)
                    return OrgOrderPadStyle.BackgroundStretch;

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
        IStyleSheet _StyleSheet;

        /// <MetaDataID>{7f9479e1-a994-4994-8837-7a8598ce80df}</MetaDataID>
        [PersistentMember(nameof(_StyleSheet))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [OOAdvantech.Json.JsonIgnore]
        [BackwardCompatibilityID("+4")]
        public IStyleSheet StyleSheet => _StyleSheet;

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{1cb88808-4593-4206-8602-1ebc9bc8cf1b}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+5")]
        public string Name
        {
            get
            {
                if (OrgOrderPadStyle != null && _Name == null)
                    return OrgOrderPadStyle.Name;
                return _Name;
            }
            set
            {
                if (_Name!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{feed08d6-ffcf-4c7a-bdef-6adaa3ec88b2}</MetaDataID>
        public bool IsDerivedStyle
        {
            get
            {
                if (OrgOrderPadStyle!= null)
                    return true;
                else
                    return false;
            }
        }

        /// <MetaDataID>{a8540ce5-f357-40a9-81c2-4a0b43923330}</MetaDataID>
        public IStyleRule GetDerivedStyle()
        {
            return new OrderPadStyle(this);
        }

        /// <MetaDataID>{196be6d2-14fc-4726-a512-f903add8495a}</MetaDataID>
        public void ChangeOrgStyle(IStyleRule style)
        {
            if (OrgOrderPadStyle != (style as OrderPadStyle))
            {
                if (OrgOrderPadStyle != null)
                    UseDefaultValues();

                OrgOrderPadStyle = style as OrderPadStyle;
            }
        }


        public void UseDefaultValues()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                Background = null;
                _BackgroundMargin = null;
                _BackgroundStretch = null;
              
                stateTransition.Consistent = true;
            }

        }
    }
}