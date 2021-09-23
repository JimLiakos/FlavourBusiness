using FlavourBusinessFacade.ServicesContextResources;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{d83d4466-195d-471e-be61-fdceb04c166e}</MetaDataID>
    public class ItemsPreparationContext
    {
        
        public string PreparationStationIdentity { get; set; }
        public List<IItemPreparation> PreparationItems { get; set; }

        public string Description { get; set; }

        

        public string ServicePointDescription { get; set; }

        public string PreparationStationDescription { get; set; }

        public ItemsPreparationContext(IMealCourse mealCourse, IPreparationStation preparationStation, List<IItemPreparation> preparationItems)
        {
            //this.MealCourse = mealCourse;
            this.PreparationStationIdentity = preparationStation.PreparationStationIdentity;
            PreparationStationDescription = preparationStation.Description;

            this.PreparationItems = preparationItems;
            Description = preparationStation.Description;
            ServicePointDescription = mealCourse.Meal.Session.ServicePoint.Description;
            
        }
        public ItemsPreparationContext()
        {

        }
    }
}