using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessFacade.RoomService
{

    /// <summary>
    /// Defines 
    /// </summary>
    /// <MetaDataID>{d83d4466-195d-471e-be61-fdceb04c166e}</MetaDataID>
    public class ItemsPreparationContext
    {

        /// <MetaDataID>{8b5f1668-5da4-4e06-a565-03b8a622c502}</MetaDataID>
        public string PreparationStationIdentity { get; set; }

        object preparationItemLock = new object();
        List<IItemPreparation> _PreparationItems;
        /// <MetaDataID>{e3ab05aa-5f0e-4c56-b91a-c5cd2937684f}</MetaDataID>
        public List<IItemPreparation> PreparationItems
        {
            get
            {

                lock (preparationItemLock)
                {
                    return _PreparationItems.ToList();
                }
            }
            set
            {
                lock (preparationItemLock)
                {
                    _PreparationItems = value;
                }
            }
        }

        /// <MetaDataID>{41c8c8ed-0445-4e08-a995-cb44fe6d1837}</MetaDataID>
        public string Description { get; set; }

        public string MealCourseDescription { get; set; }



        /// <MetaDataID>{044b2150-955b-4587-a42e-a4cd1af9eb81}</MetaDataID>
        public string ServicePointDescription { get; set; }
        public DateTime? MealCourseStartsAt { get; private set; }

        /// <MetaDataID>{b6e7eb8b-024d-496d-bfaf-a486b1840263}</MetaDataID>
        public string PreparationStationDescription { get; set; }
        public IMealCourse MealCourse { get; }

        public IServicePoint ServicePoint;

        public const string TradeProductsStationIdentity = "772E94BEEA1C4A64B8FE5D808A9CDC61";
#if !FlavourBusinessDevice

        /// <summary>
        /// Defines items collection with common meal course and common preparation station.
        /// </summary>
        /// <param name="mealCourse">
        /// Defines the meal course of preparation items
        /// </param>
        /// <param name="preparationStation">
        /// Defines the preparation station where item prepared
        /// </param>
        /// <param name="preparationItems">
        /// Defines the preparation items collection.
        /// </param>
        /// <MetaDataID>{57de3627-5720-4605-a451-fec19548023e}</MetaDataID>
        public ItemsPreparationContext(IMealCourse mealCourse, IPreparationStation preparationStation, List<IItemPreparation> preparationItems)
        {
            if(mealCourse==null)
            {

            }
            this.MealCourse = mealCourse;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            MealCourse.ObjectChangeState += MealCourse_ObjectChangeState;

            if (preparationStation != null)
            {
                this.PreparationStationIdentity = preparationStation.PreparationStationIdentity;
                PreparationStationDescription = preparationStation.Description;

                //Description = MealCourse.Meal.Session.ServicePoint.ServiceArea.Description + " / " + MealCourse.Meal.Session.ServicePoint.Description;

                MealCourseDescription = MealCourse.Meal.Session.ServicePoint.ServiceArea.Description + " / " + MealCourse.Meal.Session.ServicePoint.Description;
                Description = preparationStation.Description;
            }
            else
            {
                Description = Resource.FoodItemInstantlyAvailable;
                this.PreparationStationIdentity = TradeProductsStationIdentity;
            }
            _PreparationItems = preparationItems;
            ServicePointDescription = mealCourse.Meal.Session.ServicePoint.Description;
            MealCourseStartsAt = mealCourse.StartsAt;
            ServedAtForecast = mealCourse.ServedAtForecast;
            PreparatioOrder = PreparatioOrder;

        }
#endif


        [OOAdvantech.Json.JsonConstructor]
        public ItemsPreparationContext(IServicePoint servicePoint, List<IItemPreparation> preparationItems, string description, string uri, System.DateTime? servedAtForecast, DateTime mealCourseStartsAt)
        {
            ServicePoint = servicePoint;
            _PreparationItems = preparationItems;
            Description = description;
            _Uri = uri;
            // ServedAtForecast = servedAtForecast;
            MealCourseStartsAt = mealCourseStartsAt;
            PreparatioOrder = PreparatioOrder;
        }

        /// <MetaDataID>{b53c2414-42ee-432c-a6ba-ac82e1f6f1bd}</MetaDataID>
        public ItemsPreparationContext()
        {

        }
        private void MealCourse_ObjectChangeState(object _object, string member)
        {
            if (nameof(IFoodServiceSession.ServicePoint) == member)
            {
                Description = MealCourse.Meal.Session.ServicePoint.ServiceArea.Description + " / " + MealCourse.Meal.Session.ServicePoint.Description;
                ServicePoint = MealCourse.Meal.Session.ServicePoint;
                ObjectChangeState?.Invoke(this, null);
            }

        }

        public void AddPreparationItem(IItemPreparation flavourItem)
        {
            Transaction.RunOnTransactionCompleted(() =>
            {
                lock (preparationItemLock)
                {
                    if (!_PreparationItems.Contains(flavourItem))
                    {
                        flavourItem.PreparatioOrder = PreparatioOrder;
                        _PreparationItems.Add(flavourItem);
                    }
                }
            });
        }

        public void RemovePreparationItem(IItemPreparation flavourItem)
        {
            lock (preparationItemLock)
            {
                _PreparationItems.Remove(flavourItem);
            }
        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        public string CodeCard
        {
            get
            {
                lock (preparationItemLock)
                {

                    return _PreparationItems.Where(x => !string.IsNullOrWhiteSpace(x.CodeCard)).Select(x => x.CodeCard).FirstOrDefault();
                }
            }
            set
            {
                foreach (var preparationItem in PreparationItems)
                    preparationItem.CodeCard = value;

            }
        }


        /// <exclude>Excluded</exclude>
        [OOAdvantech.Json.JsonIgnore]
        string _Uri;

        public string Uri
        {
            get
            {

                if (_Uri == null)
                    _Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MealCourse)?.GetPersistentObjectUri(MealCourse);
                return _Uri;
            }
        }


        public System.DateTime? ServedAtForecast { get; set; }
        public DateTime PreparedAtForecast { get; set; }
        public bool PreparationOrderCommited { get; set; }
        public int PreparatioOrder
        {
            get
            {
                int preparatioOrder = 0;
                var itemPreparation = PreparationItems.OrderBy(x => x.PreparatioOrder).LastOrDefault();
                if (itemPreparation != null)
                    preparatioOrder = itemPreparation.PreparatioOrder;
                return preparatioOrder;

            }
            set
            {
                Transaction.RunOnTransactionCompleted(() =>
                {
                    lock (preparationItemLock)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var itemPreparation in _PreparationItems)
                                itemPreparation.PreparatioOrder = value;
                            stateTransition.Consistent = true;
                        }
                    }
                });
            }
        }

        public ItemPreparationState PreparationState;


    }


}