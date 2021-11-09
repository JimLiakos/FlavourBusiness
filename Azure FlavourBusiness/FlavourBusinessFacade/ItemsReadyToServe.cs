using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{e547a3a4-13e0-4cdf-97d4-4a44c90a4889}</MetaDataID>
    public class ItemsReadyToServe
    {


        public ItemsReadyToServe(IFoodServiceSession serviceSession, System.Collections.Generic.List<RoomService.IItemPreparation> preparationItems)
        {
            ServicePoint = serviceSession.ServicePoint;
            PreparedItems = preparationItems;
            Description = ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;
            
        }

        public ItemsReadyToServe(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext>  underPreparationItems  )
        {
            
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            //PreparedItems = preparationItems;
            Description = ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;

        }
        [OOAdvantech.Json.JsonConstructor]
        public ItemsReadyToServe(IServicePoint servicePoint, System.Collections.Generic.List<RoomService.IItemPreparation> preparedItems, string description)
        {
            ServicePoint = servicePoint;
            PreparedItems = preparedItems;
            Description = description;
        }

        public string Description;

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