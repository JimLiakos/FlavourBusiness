using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{61ed3b43-fd65-4e67-9300-d184762648b9}</MetaDataID>
    [BackwardCompatibilityID("{61ed3b43-fd65-4e67-9300-d184762648b9}")]
    [Persistent()]
    public class Tag : ITag
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();
        
        /// <MetaDataID>{ff0c0bbc-f0fd-40de-bf5b-682454e3bbd3}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    }
}