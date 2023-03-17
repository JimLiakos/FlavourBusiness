using FinanceFacade;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessManager.EndUsers;
using Microsoft.Extensions.Azure;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{62fc8b78-7e94-4a57-b27c-c9d188b6d637}</MetaDataID>
    [BackwardCompatibilityID("{62fc8b78-7e94-4a57-b27c-c9d188b6d637}")]
    [Persistent()]
    public class ServingShiftWork : ShiftWork, IDebtCollection
    {
        /// <MetaDataID>{d3dd1bff-a913-47fa-a3bc-ed0952ef0efd}</MetaDataID>
        public IWaiter Waiter => Worker as IWaiter;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<EndUsers.FoodServiceClientSession> _WaiterFoodServiceClientSessions = new OOAdvantech.Collections.Generic.Set<EndUsers.FoodServiceClientSession>();

        [RoleAMultiplicityRange(0)]
        [Association("FoodServiceClientSessionInTheShiftWork", Roles.RoleA, "2076b2c6-2c5e-415a-93ab-187654f7c04a")]
        [RoleBMultiplicityRange(0, 1)]
        [PersistentMember(nameof(_WaiterFoodServiceClientSessions))]
        public List<EndUsers.FoodServiceClientSession> WaiterFoodServiceClientSessions => _WaiterFoodServiceClientSessions.ToThreadSafeList();

        /// <MetaDataID>{ddc56525-f246-48c0-b41d-71f82b87f8cb}</MetaDataID>
        public void RemoveClientSession(EndUsers.FoodServiceClientSession clientSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _WaiterFoodServiceClientSessions.Remove(clientSession);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{688b3974-5e1e-437c-8788-03d19c9092f2}</MetaDataID>
        public void AddClientSession(EndUsers.FoodServiceClientSession clientSession)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _WaiterFoodServiceClientSessions.Add(clientSession);
                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<RoomService.ServingBatch> _ServingBatches = new OOAdvantech.Collections.Generic.Set<RoomService.ServingBatch>();

        [PersistentMember(nameof(_ServingBatches))]
        [RoleAMultiplicityRange(0)]
        [Association("ServingBatchInShiftWork", Roles.RoleA, "5b49aba4-a3de-46da-9a52-6436a3823d6f")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public System.Collections.Generic.List<RoomService.ServingBatch> ServingBatches => _ServingBatches.ToThreadSafeList();

        /// <MetaDataID>{20eac476-b8fa-4d08-9cac-b2465d9762fe}</MetaDataID>
        public List<IPayment> BillingPayments
        {
            get
            {
                //WaiterFoodServiceClientSessions[0].
                var billingPayments =(from foodServiceClientSession in WaiterFoodServiceClientSessions
                 from payment in foodServiceClientSession.GetPayments()
                 where payment.State==PaymentState.Completed
                 select payment).OfType<IPayment>().ToList();

                return billingPayments;
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