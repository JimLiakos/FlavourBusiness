using FinanceFacade;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessManager.EndUsers;
using Microsoft.Extensions.Azure;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Transactions;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.AxHost;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{62fc8b78-7e94-4a57-b27c-c9d188b6d637}</MetaDataID>
    [BackwardCompatibilityID("{62fc8b78-7e94-4a57-b27c-c9d188b6d637}")]
    [Persistent()]
    public class ServingShiftWork : ShiftWork, IDebtCollection, IServingShiftWork
    {


        /// <exclude>Excluded</exclude>
        bool _AccountIsClosed;

        /// <MetaDataID>{c33f6105-9c68-44fc-8ffe-faeacfe44294}</MetaDataID>
        [PersistentMember(nameof(_AccountIsClosed))]
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
        public bool AccountIsClosed
        {
            get => _AccountIsClosed;

        }


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
        OOAdvantech.Collections.Generic.Set<FlavourBusinessFacade.RoomService.IServingBatch> _ServingBatches = new OOAdvantech.Collections.Generic.Set<FlavourBusinessFacade.RoomService.IServingBatch>();


        /// <MetaDataID>{f9b53695-3a22-43a9-a1eb-d434314dc1a6}</MetaDataID>
        [PersistentMember(nameof(_ServingBatches))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public List<FlavourBusinessFacade.RoomService.IServingBatch> ServingBatches => _ServingBatches.ToThreadSafeList();

        /// <MetaDataID>{20eac476-b8fa-4d08-9cac-b2465d9762fe}</MetaDataID>
        public System.Collections.Generic.List<FinanceFacade.IPayment> BillingPayments
        {
            get
            {
                //WaiterFoodServiceClientSessions[0].
                var billingPayments = (from foodServiceClientSession in WaiterFoodServiceClientSessions
                                       from payment in foodServiceClientSession.GetPayments()
                                       where payment.State==PaymentState.Completed
                                       select payment).OfType<IPayment>().ToList();

                return billingPayments;
            }
        }

        /// <exclude>Excluded</exclude>
        decimal _OpeningBalanceFloatCash;

        /// <MetaDataID>{a4c50e38-83fc-4884-bddf-bf08aeae753f}</MetaDataID>
        [PersistentMember(nameof(_OpeningBalanceFloatCash))]
        [BackwardCompatibilityID("+1")]
        public decimal OpeningBalanceFloatCash { get; set; }

        /// <exclude>Excluded</exclude>
        decimal _Cash;

        /// <MetaDataID>{83b2b8fe-dd1b-4d70-b981-0dc636695f3f}</MetaDataID>
        [PersistentMember(nameof(_Cash))]
        [BackwardCompatibilityID("+2")]
        public decimal Cash => _Cash;

        /// <exclude>Excluded</exclude>
        decimal _Cards;

        /// <MetaDataID>{a4fc26c7-9de3-49e0-8838-21326ef9b414}</MetaDataID>
        [PersistentMember(nameof(_Cards))]
        [BackwardCompatibilityID("+3")]
        public decimal Cards => _Cards;



        /// <exclude>Excluded</exclude>
        decimal _CardsTips;

        /// <MetaDataID>{008bf01f-bc11-43c8-aa1f-d68eae6fc488}</MetaDataID>
        [PersistentMember(nameof(_CardsTips))]
        [BackwardCompatibilityID("+4")]
        public decimal CardsTips => _CardsTips;



        /// <exclude>Excluded</exclude>
        decimal _CashTips;

        /// <MetaDataID>{58f4084f-7005-480e-a8f9-ba13a615381b}</MetaDataID>
        [PersistentMember(nameof(_CashTips))]
        [BackwardCompatibilityID("+5")]
        public decimal CashTips => _CashTips;



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

        /// <MetaDataID>{8f53aa8b-c8e3-438d-9dc1-8faffeb597b0}</MetaDataID>
        public void CashierClose()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _AccountIsClosed=true;
                OOAdvantech.Transactions.Transaction.RunOnTransactionCompleted(() =>
                {
                    OnObjectChangeState(this, null);
                });
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{fff63019-df73-4af4-ae72-05ba83333c37}</MetaDataID>
        public void RecalculateDeptData()
        {
            //if()


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Cash=0;
                _CashTips=0;
                _Cards=0;
                _CardsTips=0;

                foreach (var payment in BillingPayments)
                {
                    if (payment.PaymentType==PaymentType.Cash)
                    {
                        _Cash+=payment.Amount;
                        _CashTips=payment.TipsAmount;
                    }

                    if (payment.PaymentType==PaymentType.CreditCard||payment.PaymentType==PaymentType.DebitCard)
                    {
                        _Cards+=payment.Amount;
                        _CardsTips=payment.TipsAmount;
                    }

                }
                stateTransition.Consistent = true;
            }


        }
    }
}