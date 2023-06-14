using FlavourBusinessFacade.HumanResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{5fed1c2d-6b02-4b07-b13e-0dca8bf8782f}</MetaDataID>
    public class TakeawayCashier : ITakeawayCashier
    {

        /// <exclude>Excluded</exclude>
        bool _Suspended;

        /// <MetaDataID>{f9555559-cc44-404e-b3e9-8ede2d3d52a9}</MetaDataID>
        [PersistentMember(nameof(_Suspended))]
        [BackwardCompatibilityID("+5")]
        public bool Suspended
        {
            get => _Suspended; set
            {
                if (_Suspended!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Suspended=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAccountability> _Responsibilities = new OOAdvantech.Collections.Generic.Set<IAccountability>();

        /// <MetaDataID>{ccdf75df-49e2-43af-b45c-12061579634e}</MetaDataID>
        [PersistentMember(nameof(_Responsibilities))]
        [BackwardCompatibilityID("+3")]
        public List<IAccountability> Responsibilities => _Responsibilities.ToThreadSafeList();



        /// <MetaDataID>{7e0bb09a-102d-4e24-9cc3-954b9763a962}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        public List<IAccountability> Commissions => new List<IAccountability>();


        /// <exclude>Excluded</exclude>
        string _WorkerAssignKey;

        /// <MetaDataID>{29cdf983-6d05-405d-b3c6-6cde8c5cf522}</MetaDataID>
        [PersistentMember(nameof(_WorkerAssignKey))]
        [BackwardCompatibilityID("+4")]
        public string WorkerAssignKey
        {
            get => _WorkerAssignKey; set
            {
                if (_WorkerAssignKey!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _WorkerAssignKey=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;
        /// <MetaDataID>{078d4774-50c4-41b9-9633-5e6b98ae0b3d}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+7")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity; set
            {
                if (_ServicesContextIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IShiftWork> _ShiftWorks = new OOAdvantech.Collections.Generic.Set<IShiftWork>();

        /// <MetaDataID>{1909309d-6473-4d8d-8ec9-eaf329ef9f42}</MetaDataID>
        [PersistentMember(nameof(_ShiftWorks))]
        [BackwardCompatibilityID("+1")]
        public IList<IShiftWork> ShiftWorks => _ShiftWorks;


        /// <MetaDataID>{f233ab03-3b88-44a3-a035-b018afa68f5d}</MetaDataID>
        List<ShiftWork> RecentlyShiftWorks;

        /// <MetaDataID>{acb9a452-7452-47e9-be6f-bd4c05a720c3}</MetaDataID>
        public IShiftWork ActiveShiftWork
        {
            get
            {
                var mileStoneDate = System.DateTime.UtcNow - TimeSpan.FromDays(1);
                var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                if (objectStorage != null)
                {
                    if (RecentlyShiftWorks == null)
                    {
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                        RecentlyShiftWorks = (from shiftWork in storage.GetObjectCollection<ShiftWork>()
                                              where shiftWork.StartsAt > mileStoneDate && shiftWork.Worker == this
                                              select shiftWork).ToList();
                    }

                    if (RecentlyShiftWorks.Count > 0)
                    {
                    }


                    return RecentlyShiftWorks.OrderBy(x => x.StartsAt).LastOrDefault();
                }
                else
                    return _ShiftWorks.ToThreadSafeList().Where(x => x.StartsAt > mileStoneDate).Last();
            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{88c23b6a-47b2-46c0-9649-c2be0fe3e298}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+8")]
        public string Name
        {
            get => _Name; set
            {
                if (_Name!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{1478a073-3a9b-4f68-9d77-b1859d666c17}</MetaDataID>
        public void ChangeSiftWork(IShiftWork shiftWork, DateTime startedAt, double timespanInHours)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                (shiftWork as ShiftWork).StartsAt = startedAt;
                (shiftWork as ShiftWork).PeriodInHours = timespanInHours;
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));
        }

        /// <MetaDataID>{22cc3ca5-1bc7-43cc-b0da-f3cd5272cf48}</MetaDataID>
        public IShiftWork NewShiftWork(DateTime startedAt, double timespanInHours)
        {
            ShiftWork shiftWork = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                shiftWork = new ServingShiftWork(Name);
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(shiftWork);
                shiftWork.StartsAt = startedAt;
                shiftWork.PeriodInHours = timespanInHours;
                AddShiftWork(shiftWork);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));


            ServicePointRunTime.ServicesContextRunTime.Current.WaiterSiftWorkUpdated(this);

            return shiftWork;
        }
        public IShiftWork NewShiftWork(DateTime startedAt, double timespanInHours, decimal openingBalanceFloatCash)
        {
            ServingShiftWork shiftWork = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                shiftWork = new ServingShiftWork(Name);
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(shiftWork);
                shiftWork.StartsAt = startedAt;
                shiftWork.PeriodInHours = timespanInHours;
                shiftWork.OpeningBalanceFloatCash=openingBalanceFloatCash;
                AddShiftWork(shiftWork);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ActiveShiftWork));


            ServicePointRunTime.ServicesContextRunTime.Current.WaiterSiftWorkUpdated(this);

            return shiftWork;


        }

        internal void AddShiftWork(ShiftWork shiftWork)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Add(shiftWork);

                stateTransition.Consistent = true;
            }
            RecentlyShiftWorks = null;

        }

        /// <MetaDataID>{bf3a1c9c-a784-45a6-b94a-a9683a18332a}</MetaDataID>
        public void RemoveShiftWork(IShiftWork shiftWork)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Remove(shiftWork);
                stateTransition.Consistent = true;
            }
            RecentlyShiftWorks = null;
        }
    }
}