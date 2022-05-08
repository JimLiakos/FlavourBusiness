using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{d83d4466-195d-471e-be61-fdceb04c166e}</MetaDataID>
    public class ItemsPreparationContext
    {

        /// <MetaDataID>{8b5f1668-5da4-4e06-a565-03b8a622c502}</MetaDataID>
        public string PreparationStationIdentity { get; set; }
        /// <MetaDataID>{e3ab05aa-5f0e-4c56-b91a-c5cd2937684f}</MetaDataID>
        public List<IItemPreparation> PreparationItems { get; set; }

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
            this.PreparationItems = preparationItems;
            ServicePointDescription = mealCourse.Meal.Session.ServicePoint.Description;
            MealCourseStartsAt = mealCourse.StartsAt;
            ServedAtForecast = mealCourse.ServedAtForecast.Value;

        }
#endif


        [OOAdvantech.Json.JsonConstructor]
        public ItemsPreparationContext(IServicePoint servicePoint, List<IItemPreparation> preparationItems, string description, string uri, System.DateTime servedAtForecast, DateTime mealCourseStartsAt)
        {
            ServicePoint = servicePoint;
            PreparationItems = preparationItems;
            Description = description;
            _Uri = uri;
            // ServedAtForecast = servedAtForecast;
            MealCourseStartsAt = mealCourseStartsAt;
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
            if (!PreparationItems.Contains(flavourItem))
                PreparationItems.Add(flavourItem);
        }

        public void RemovePreparationItem(IItemPreparation flavourItem)
        {
            PreparationItems.Remove(flavourItem);
        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        public string CodeCard
        {
            get
            {
                return PreparationItems.Where(x => !string.IsNullOrWhiteSpace(x.CodeCard)).Select(x => x.CodeCard).FirstOrDefault();
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

        public ItemPreparationState PreparationState;


    }


}