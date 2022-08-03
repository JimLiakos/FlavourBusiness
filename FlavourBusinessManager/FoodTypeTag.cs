using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{d0c9a997-b615-4213-9207-be6426fdc83b}</MetaDataID>
    [BackwardCompatibilityID("{d0c9a997-b615-4213-9207-be6426fdc83b}")]
    [Persistent()]
    public class FoodTypeTag : FlavourBusinessFacade.IFoodTypeTag
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{b7a64207-d93d-44ef-b958-9198d4d8cc19}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get => _Name;
            set
            {
                
                if (_Name.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value;
                        if (string.IsNullOrWhiteSpace(value))
                            _Name.ClearValue();

                        stateTransition.Consistent = true;
                    }
                    
                }
            }
        }
    }
}