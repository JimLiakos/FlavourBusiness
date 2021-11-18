using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{62fc8b78-7e94-4a57-b27c-c9d188b6d637}</MetaDataID>
    [BackwardCompatibilityID("{62fc8b78-7e94-4a57-b27c-c9d188b6d637}")]
    [Persistent()]
    public class ServingShiftWork : ShiftWork
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<RoomService.ServingBatch> _ServingBatches = new OOAdvantech.Collections.Generic.Set<RoomService.ServingBatch>();

        [PersistentMember(nameof(_ServingBatches))]
        [RoleAMultiplicityRange(0)]
        [Association("ServingBatchInShiftWork", Roles.RoleA, "5b49aba4-a3de-46da-9a52-6436a3823d6f")]
        public System.Collections.Generic.List<RoomService.ServingBatch> ServingBatches => _ServingBatches.ToThreadSafeList();



        public ServingShiftWork(string name) : base(name)
        {

        }
        public ServingShiftWork()
        {

        }
        public void AddServingBatch(FlavourBusinessFacade.RoomService.IServingBatch servingBatch)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServingBatches.Add(servingBatch as RoomService.ServingBatch);
                stateTransition.Consistent = true;
            }
        }

        public void RemoveServingBatch(FlavourBusinessFacade.RoomService.IServingBatch servingBatch)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServingBatches.Remove(servingBatch as RoomService.ServingBatch);
                stateTransition.Consistent = true;
            }
        }
    }
}