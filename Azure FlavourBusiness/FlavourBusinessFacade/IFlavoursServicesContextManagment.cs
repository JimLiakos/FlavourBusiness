using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{bb9967b0-cacd-4aec-9b32-09e990b3b38d}</MetaDataID>
    [GenerateFacadeProxy]
    public interface IFlavoursServicesContextManagment
    {
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{6406c68d-3514-4539-8fff-128f41750b06}</MetaDataID>
        System.Collections.Generic.List<IFoodTypeTag> FoodTypeTags { get; }

        ///// <MetaDataID>{8ddeb708-1d66-4e8c-8db1-073987c7a9d1}</MetaDataID>
        //OrganizationStorageRef GetMenu(string servicePointIdentity);

        /// <MetaDataID>{4287e2b6-5363-4add-9fbc-a3f1ef30ef81}</MetaDataID>
        ServicesContextResources.IServicePoint GetServicePoint(string servicePointIdentity);


        /// <MetaDataID>{283eb213-1969-41be-b534-628956c13ea4}</MetaDataID>
        ServicesContextResources.IPreparationStationRuntime GetPreparationStationRuntime(string preparationStationCredentialKey);

        


        /// <MetaDataID>{9a58a699-0bbb-486b-be8e-0755cb0fcac1}</MetaDataID>
        IFlavoursServicesContextRuntime GetServicesContextRuntime(string storageName, string storageLocation, string servicePointIdentity, string organizationIdentity, OrganizationStorageRef restaurantMenusDataStorageRef, bool create = false);
        /// <MetaDataID>{a92703b9-7329-4e49-813d-f2c5925f2545}</MetaDataID>
        ICashiersStationRuntime GetCashiersStationRuntime(string communicationCredentialKey);

        /// <MetaDataID>{2f1dea32-0587-40be-8b27-148c1962fcab}</MetaDataID>
        EndUsers.ClientSessionData GetClientSession(string servicePointIdentity, string clientName, string clientDeviceID,string deviceFirebaseToken, bool endUser,  bool create);
        /// <MetaDataID>{918bd192-2efb-4ff5-adbe-ea6bbe041465}</MetaDataID>
        IHallLayout GetHallLayout(string servicePoint);

        /// <MetaDataID>{2f7b5ce7-77fc-462f-90de-9d56f7bb6ded}</MetaDataID>
        HumanResources.IServiceContextSupervisor AssignSupervisorUser(string supervisorAssignKey);
        /// <MetaDataID>{9879b845-4ce0-421c-974d-0054a8664a83}</MetaDataID>
        HumanResources.IWaiter AssignWaiterUser(string waiterAssignKey);


        /// <MetaDataID>{f1791b25-d9b2-423a-b4d2-b6a5b4012d70}</MetaDataID>
        System.Collections.Generic.List<HomeDeliveryServicePointInfo> GetNeighborhoodFoodServers(Coordinate location);
        IFoodServiceClientSession GetMealInvitationInviter(string invitationUri);
    }

  
}