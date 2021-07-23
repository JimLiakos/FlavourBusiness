using FlavourBusinessFacade.RoomService;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{359a649b-220c-434b-b676-181d2160d449}</MetaDataID>
    public class ServicePointPreparationItems
    {


        /// <MetaDataID>{0f0aafc5-6824-4bf8-b2f1-787b2175f9fd}</MetaDataID>
        public ServicePointPreparationItems(IFoodServiceSession serviceSession , System.Collections.Generic.List<RoomService.IItemPreparation> preparationItems)
        {
            ServicePoint = serviceSession.ServicePoint;
            PreparationItems = preparationItems;
            Description = ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;
            Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(serviceSession).GetPersistentObjectUri(serviceSession);

        }

        public string Uri;


        /// <exclude>Excluded</exclude>
        public string Description;


        [Association("ServicePointItems", Roles.RoleA, "1b18cbff-1a92-4751-b00e-9fba52c6d00a")]
        [RoleAMultiplicityRange(1, 1)]
        public IServicePoint ServicePoint;



        [Association("StationPreparationItems", Roles.RoleA, "f98b8c21-af79-4d47-861a-654378f15fc1")]
        [RoleAMultiplicityRange(1)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        public System.Collections.Generic.List<RoomService.IItemPreparation> PreparationItems;



    }
}