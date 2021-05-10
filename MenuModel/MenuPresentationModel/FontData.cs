using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel
{
    /// <MetaDataID>{032cf3b1-dc37-4fd7-b806-62dc7c512dda}</MetaDataID>
    [BackwardCompatibilityID("{032cf3b1-dc37-4fd7-b806-62dc7c512dda}")]
    [Persistent()]
    public class FontData
    {
        /// <exclude>Excluded</exclude>
        int _FontSize;
        /// <MetaDataID>{7c86d356-60a4-40b6-bbb4-be07d049a90f}</MetaDataID>
        [PersistentMember(nameof(_FontSize))]
        [BackwardCompatibilityID("+4")]
        public int FontSize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontSize = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _FontFamilyName;
        /// <MetaDataID>{5ef08afa-7d55-4753-a3a2-618391e917d6}</MetaDataID>
        [PersistentMember(nameof(_FontFamilyName))]
        [BackwardCompatibilityID("+1")]
        public string FontFamilyName
        {
            get
            {
                return _FontFamilyName;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontFamilyName = value; 
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _FontWeight;
        /// <MetaDataID>{5d426456-b1f0-4e1b-bd56-67106b91e1e9}</MetaDataID>
        [PersistentMember(nameof(_FontWeight))]
        [BackwardCompatibilityID("+2")]
        public string FontWeight
        {
            get
            {
                return _FontWeight;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontWeight = value; 
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _FontStyle;
        /// <MetaDataID>{5ca1a878-bcf9-417a-b450-e3a6d973aeaf}</MetaDataID>
        [PersistentMember(nameof(_FontStyle))]
        [BackwardCompatibilityID("+3")]
        public string FontStyle
        {
            get
            {
                return _FontStyle;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _FontStyle = value; 
                    stateTransition.Consistent = true;
                }

            }
        }
    }
}