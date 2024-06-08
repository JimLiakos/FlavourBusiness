using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessManager.Printing
{
    /// <MetaDataID>{0bfc72b5-d3d6-4279-9000-8ef442daa588}</MetaDataID>
    public class PreparationStationPrintManager
    {
        /// <MetaDataID>{f3adcb85-71e5-454c-a9fb-a232b76364f2}</MetaDataID>
        internal void OnPreparationItemsChangeState(PreparationStation preparationStation)
        {
            foreach (var itemsPreparationContext in preparationStation.FoodItemsInProgress)
            {
                var identity = ItemsPreparationContextSnapshot.GetIdentity(itemsPreparationContext);

                var snapshot = itemsPreparationContextSnapshots.Where(x => x.Identity == identity).FirstOrDefault();
                if (snapshot == null)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        snapshot = new ItemsPreparationContextSnapshot(itemsPreparationContext);
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsPreparationContext.MealCourse).CommitTransientObjectState(snapshot);
                        itemsPreparationContextSnapshots.Add(snapshot);

                        Transaction.RunOnTransactionCompleted(() =>
                        {
                            (ServicePointRunTime.ServicesContextRunTime.Current.InternalPrintManager as PrintManager).Print(snapshot);
                        });



                        stateTransition.Consistent = true;
                    }
                }
                if (snapshot.SnapshotIdentity != ItemsPreparationContextSnapshot.GetSnapshotIdentity(itemsPreparationContext))
                {
                    snapshot.Update(itemsPreparationContext);
                }

            }
        }


        /// <MetaDataID>{42542205-e829-42e2-94c5-20c5185861b7}</MetaDataID>
        List<ItemsPreparationContextSnapshot> itemsPreparationContextSnapshots = new List<ItemsPreparationContextSnapshot>();

        /// <MetaDataID>{999312c1-0935-4ddb-9636-34c36d1bbdff}</MetaDataID>
        public PreparationStationPrintManager(List<ItemsPreparationContextSnapshot> itemsPreparationContextSnapshots)
        {
            this.itemsPreparationContextSnapshots = itemsPreparationContextSnapshots;
        }

        /// <MetaDataID>{88a295a9-3298-419d-aafb-3ec763529850}</MetaDataID>
        public PreparationStationPrintManager(PreparationStation preparationStation)
        {
            OnPreparationItemsChangeState(preparationStation);
        }
    }


}
