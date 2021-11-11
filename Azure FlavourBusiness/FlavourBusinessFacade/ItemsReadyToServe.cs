using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{e547a3a4-13e0-4cdf-97d4-4a44c90a4889}</MetaDataID>
    public class ItemsReadyToServe
    {


  

        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; private set; }
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; private set; }
        public IMealCourse MealCourse { get; private set; }
        public ItemsReadyToServe(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {
            MealCourse = mealCourse;
            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;

            PreparedItems = (from itemsPreparationContext in preparedItems
                             from itemPreparation in itemsPreparationContext.PreparationItems
                             select itemPreparation).ToList();

            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

            //Description = mealCourse.Name + " " + ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;

        }
        [OOAdvantech.Json.JsonConstructor]
        public ItemsReadyToServe(IServicePoint servicePoint, IMealCourse mealCourse, List<IItemPreparation> preparedItems, IList<ItemsPreparationContext> contextsOfPreparedItems, IList<ItemsPreparationContext> contextsOfUnderPreparationItems, string description)
        {
            MealCourse = mealCourse;
            ServicePoint = servicePoint;
            PreparedItems = preparedItems;
            Description = description;
            ContextsOfPreparedItems = contextsOfPreparedItems;
            ContextsOfUnderPreparationItems = contextsOfUnderPreparationItems;
        }
        public ServicePointType ServicePointType { get; set; } = ServicePointType.HallServicePoint;

        public string Description;

        [OOAdvantech.Json.JsonIgnore]
        [Association("", Roles.RoleA, "48d7a02e-c7e1-4272-af2e-fb0ef5ee917b")]
        [RoleAMultiplicityRange(1, 1)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        public ServicesContextResources.IServicePoint ServicePoint;
        
        [Association("PreparedItemsToServe", Roles.RoleA, "2b36e0e0-e305-45c5-9b91-4f13b7048c84")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        public System.Collections.Generic.List<IItemPreparation> PreparedItems;






    }
}