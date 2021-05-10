using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace Multilingual
{
    /// <MetaDataID>{c1938e13-b9c0-482c-9488-7b0db9fc626b}</MetaDataID>
    [BackwardCompatibilityID("{c1938e13-b9c0-482c-9488-7b0db9fc626b}")]
    [Persistent()]
    public class Item
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
       OOAdvantech.MultilingualMember< ItemResources> _Resource =new OOAdvantech.MultilingualMember<ItemResources>();
        /// <MetaDataID>{7a3da0a4-d2d2-4f93-aa42-d6c68008429c}</MetaDataID>
        [PersistentMember(nameof(_Resource))]
        [BackwardCompatibilityID("+2")]
        public ItemResources Resource
        {
            get => _Resource;
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Resource.Value = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Name ;

        /// <MetaDataID>{ea942b69-21c5-4592-9b69-0e4f4fe62c90}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
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


    
    }
}