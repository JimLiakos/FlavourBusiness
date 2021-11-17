using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{e547a3a4-13e0-4cdf-97d4-4a44c90a4889}</MetaDataID>
    public class ServingBatch
    {


        /// <MetaDataID>{1abcf246-6696-41e7-82ae-19bd863348a1}</MetaDataID>
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; private set; }
        /// <MetaDataID>{229605f9-a461-4742-b21c-003791d9f578}</MetaDataID>
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; private set; }
        /// <MetaDataID>{bb994edc-6942-44d7-ab3b-88e6ede82264}</MetaDataID>
        public IMealCourse MealCourse { get; private set; }
        /// <MetaDataID>{35670efb-8e94-47ae-af59-82be8a3e6bec}</MetaDataID>
        public ServingBatch(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {

            MealCourse = mealCourse;
            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;

            PreparedItems = (from itemsPreparationContext in preparedItems
                             from itemPreparation in itemsPreparationContext.PreparationItems
                             select itemPreparation).ToList();

            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

            //Description = mealCourse.Name + " " + ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;

        }
        /// <MetaDataID>{f444def7-8e79-4c23-bd25-7580da6b8351}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public ServingBatch(string servicesPointIdentity, IMealCourse mealCourse, List<IItemPreparation> preparedItems, IList<ItemsPreparationContext> contextsOfPreparedItems, IList<ItemsPreparationContext> contextsOfUnderPreparationItems, string description)
        {
            MealCourse = mealCourse;
            ServicesPointIdentity = servicesPointIdentity;
            PreparedItems = preparedItems;
            Description = description;
            ContextsOfPreparedItems = contextsOfPreparedItems;
            ContextsOfUnderPreparationItems = contextsOfUnderPreparationItems;
        }
        /// <MetaDataID>{2b57a144-6413-42d5-ba9b-0d8fe379ceac}</MetaDataID>
        public ServicePointType ServicePointType { get; set; } = ServicePointType.HallServicePoint;

        /// <MetaDataID>{c3ca037a-cd9e-4dd1-83a7-8ce074517b1e}</MetaDataID>
        public string Description;

        [OOAdvantech.Json.JsonIgnore]
        [Association("", Roles.RoleA, "48d7a02e-c7e1-4272-af2e-fb0ef5ee917b")]
        [RoleAMultiplicityRange(1, 1)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        public ServicesContextResources.IServicePoint ServicePoint;

        /// <MetaDataID>{9ccdd411-eea1-496e-8b67-445582a1702a}</MetaDataID>
        public string ServicesPointIdentity { get; set; }

        [Association("PreparedItemsToServe", Roles.RoleA, "2b36e0e0-e305-45c5-9b91-4f13b7048c84")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        public System.Collections.Generic.List<IItemPreparation> PreparedItems;



     
    }
    /// <MetaDataID>{bb7eff35-3ac4-4ac7-90a1-5fb3a4a8f122}</MetaDataID>
    public class ServingBatchUpdates
    {
        [OOAdvantech.Json.JsonConstructor]
        public ServingBatchUpdates(List<ServingBatch> servingBatches, List<ItemPreparationAbbreviation> removedServingItems)
        {
            RemovedServingItems = removedServingItems;
            ServingBatches = servingBatches;
        }
        public List<ItemPreparationAbbreviation> RemovedServingItems { get; }
        public List<ServingBatch> ServingBatches { get; }

    }
}