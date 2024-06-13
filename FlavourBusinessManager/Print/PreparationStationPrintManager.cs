using FlavourBusinessManager.ServicesContextResources;
using Microsoft.Extensions.Azure;
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
            if (string.IsNullOrWhiteSpace(preparationStation.Printer))
                return;
            
            foreach (var itemsPreparationContext in preparationStation.FoodItemsInProgress)
            {
                var identity = ItemsPreparationContextSnapshots.GetIdentity(itemsPreparationContext);

                ItemsPreparationContextSnapshots itemsPreparationContextSnapshots = itemsPreparationContextPrintings.Where(x => x.Identity == identity).FirstOrDefault();
                
                if (itemsPreparationContextSnapshots  == null)
                {

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        itemsPreparationContextSnapshots = new ItemsPreparationContextSnapshots(itemsPreparationContext);
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsPreparationContext.MealCourse).CommitTransientObjectState(snapshot);
                        itemsPreparationContextPrintings.Add(itemsPreparationContextSnapshots);

                        Transaction.RunOnTransactionCompleted(() =>
                        {
                            (ServicePointRunTime.ServicesContextRunTime.Current.InternalPrintManager as PrintManager).Print(itemsPreparationContextSnapshots);
                        });

                        stateTransition.Consistent = true;
                    }
                }

                itemsPreparationContextSnapshots.Update(itemsPreparationContext);
                

            }
        }


        /// <MetaDataID>{42542205-e829-42e2-94c5-20c5185861b7}</MetaDataID>
        List<ItemsPreparationContextSnapshots> itemsPreparationContextPrintings = new List<ItemsPreparationContextSnapshots>();

        /// <MetaDataID>{999312c1-0935-4ddb-9636-34c36d1bbdff}</MetaDataID>
        public PreparationStationPrintManager(List<ItemsPreparationContextSnapshots> itemsPreparationContextSnapshots)
        {
            this.itemsPreparationContextPrintings = itemsPreparationContextSnapshots;
        }

        /// <MetaDataID>{88a295a9-3298-419d-aafb-3ec763529850}</MetaDataID>
        public PreparationStationPrintManager(PreparationStation preparationStation)
        {
            OnPreparationItemsChangeState(preparationStation);
        }
    }


}
