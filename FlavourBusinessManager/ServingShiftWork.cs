using FinanceFacade;
using FlavourBusinessFacade.HumanResources;
using Microsoft.Extensions.Azure;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Collections.Generic;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{62fc8b78-7e94-4a57-b27c-c9d188b6d637}</MetaDataID>
    [BackwardCompatibilityID("{62fc8b78-7e94-4a57-b27c-c9d188b6d637}")]
    [Persistent()]
    public class ServingShiftWork : ShiftWork, IDebtCollection
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<RoomService.ServingBatch> _ServingBatches = new OOAdvantech.Collections.Generic.Set<RoomService.ServingBatch>();

        [PersistentMember(nameof(_ServingBatches))]
        [RoleAMultiplicityRange(0)]
        [Association("ServingBatchInShiftWork", Roles.RoleA, "5b49aba4-a3de-46da-9a52-6436a3823d6f")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public System.Collections.Generic.List<RoomService.ServingBatch> ServingBatches => _ServingBatches.ToThreadSafeList();

        public List<IPayment> BillingPayments
        {
            get
            {
                this.AddTableServiceClient
            } 
        }



        /// <MetaDataID>{0fcf5c16-341d-4a23-9693-c2141a8f9ebe}</MetaDataID>
        public ServingShiftWork(string name) : base(name)
        {

        }
        /// <MetaDataID>{3801d11d-03cd-4295-9a55-9bba39cca3c0}</MetaDataID>
        public ServingShiftWork()
        {

        }
        /// <MetaDataID>{6b1126db-350d-4b30-8e3e-add90e7d0f0b}</MetaDataID>
        public void AddServingBatch(FlavourBusinessFacade.RoomService.IServingBatch servingBatch)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(servingBatch);
                _ServingBatches.Add(servingBatch as RoomService.ServingBatch);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{80e72316-2082-4d79-bbd3-7a36d6d8b272}</MetaDataID>
        public void RemoveServingBatch(FlavourBusinessFacade.RoomService.IServingBatch servingBatch)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(servingBatch);
                _ServingBatches.Remove(servingBatch as RoomService.ServingBatch);
                
                stateTransition.Consistent = true;
            }
        }
    }
}