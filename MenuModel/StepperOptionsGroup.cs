using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace MenuModel
{
    /// <MetaDataID>{32711c68-7529-4bee-84c7-82ef06e4ccdb}</MetaDataID>
    [BackwardCompatibilityID("{32711c68-7529-4bee-84c7-82ef06e4ccdb}")]
    [Persistent()]
    public class StepperOptionsGroup : PreparationOptionsGroup
    {

        public IPreparationOptionsGroup NewNestedOptionsGroups()
        {
            PreparationOptionsGroup optionGroup = new PreparationOptionsGroup(Properties.Resources.PreparationOptionsGroupDefaultName);

            OOAdvantech.PersistenceLayer.ObjectStorage objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            if (objectStorage != null)
                objectStorage.CommitTransientObjectState(optionGroup);


            AddNestedOptionsGroup(optionGroup);
            return optionGroup;
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<PreparationOptionsGroup> _NestedOptionsGroups = new OOAdvantech.Collections.Generic.Set<PreparationOptionsGroup>();

        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(0, 1)]
        [PersistentMember(nameof(_NestedOptionsGroups))]
        [Association("StepperNestedOptionsGroups", Roles.RoleA, true, "ce53dbcd-e48a-4a4a-9087-b062d724a08f")]
        public System.Collections.Generic.IList<PreparationOptionsGroup> NestedOptionsGroups => _NestedOptionsGroups.ToThreadSafeList();

        public StepperOptionsGroup(string name) : base(name)
        {
        }
        protected StepperOptionsGroup()
        {

        }


        /// <MetaDataID>{98bef502-c56f-4d7a-9216-bdc7ad947e16}</MetaDataID>
        public void AddNestedOptionsGroup(PreparationOptionsGroup preparationOptionsGroup)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _NestedOptionsGroups.Add(preparationOptionsGroup); 
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{3f177609-866f-40be-844a-516a220c8f60}</MetaDataID>
        public void DeleteNestedOptionsGroup(PreparationOptionsGroup preparationOptionsGroup)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _NestedOptionsGroups.Remove(preparationOptionsGroup); 
                stateTransition.Consistent = true;
            }

        }

        public void MoveNestedOptionsGroup(PreparationOptionsGroup selectedOptionsGroup, int newpos)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _NestedOptionsGroups.Remove(selectedOptionsGroup);
                _NestedOptionsGroups.Insert(newpos, selectedOptionsGroup);
                stateTransition.Consistent = true;
            }
        }
    }
}