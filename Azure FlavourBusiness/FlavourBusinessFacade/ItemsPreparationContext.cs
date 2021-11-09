using FlavourBusinessFacade.ServicesContextResources;
using System.Collections.Generic;

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



        /// <MetaDataID>{044b2150-955b-4587-a42e-a4cd1af9eb81}</MetaDataID>
        public string ServicePointDescription { get; set; }

        /// <MetaDataID>{b6e7eb8b-024d-496d-bfaf-a486b1840263}</MetaDataID>
        public string PreparationStationDescription { get; set; }

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
            //this.MealCourse = mealCourse;
            this.PreparationStationIdentity = preparationStation.PreparationStationIdentity;
            PreparationStationDescription = preparationStation.Description;

            this.PreparationItems = preparationItems;
            Description = preparationStation.Description;
            ServicePointDescription = mealCourse.Meal.Session.ServicePoint.Description;

        }
        /// <MetaDataID>{b53c2414-42ee-432c-a6ba-ac82e1f6f1bd}</MetaDataID>
        public ItemsPreparationContext()
        {

        }
    }
}