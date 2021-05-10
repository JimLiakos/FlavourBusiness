using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace MenuPresentationModel
{
    /// <MetaDataID>{f59da70f-52d4-4feb-a65b-bb1c816af7d9}</MetaDataID>
    [Persistent()]
    [BackwardCompatibilityID("{f59da70f-52d4-4feb-a65b-bb1c816af7d9}")]
    public class PresentationItemsGroup : PresentationItem
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<PresentationItem> _GroupedItems =new OOAdvantech.Collections.Generic.Set<PresentationItem>();

        public PresentationItemsGroup()
        {
        }
        public PresentationItemsGroup(PresentationItemsGroup copy, System.Collections.Generic.Dictionary<object, object> copiedObjects) : base(copy )
        {
            foreach (var groupedItem in copy.GroupedItems)
            {
                if(copiedObjects.ContainsKey(groupedItem))
                    AddGroupedItem(copiedObjects[groupedItem] as PresentationItem);
                else
                    AddGroupedItem(groupedItem.Copy(copiedObjects));

            }
        }

        [Association("GoupedPresentationItems", Roles.RoleA, "e7a0722a-a900-4d8f-8376-014dcbde76ba")]
        [PersistentMember("_GroupedItems")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0, 1)]
        public OOAdvantech.Collections.Generic.Set<PresentationItem> GroupedItems
        {
            get
            {
                return _GroupedItems.AsReadOnly();
            }
        }

        public void AddGroupedItem(PresentationItem presentationItem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _GroupedItems.Add(presentationItem); 
                stateTransition.Consistent = true;
            }

        }
        public void RemoveGroupedItem(PresentationItem presentationItem)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _GroupedItems.Remove(presentationItem);
                stateTransition.Consistent = true;
            }
        }

        public override PresentationItem Copy(System.Collections.Generic.Dictionary<object, object> copiedObjects)
        {
            PresentationItemsGroup copy = new PresentationItemsGroup(this, copiedObjects);
            copiedObjects[this] = copy;
            return copy;
        }

        public override void OnCommitObjectState()
        {
            base.OnCommitObjectState();

            foreach(var groupedItem in _GroupedItems)
            {
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(groupedItem);
            }
        }
    }
}