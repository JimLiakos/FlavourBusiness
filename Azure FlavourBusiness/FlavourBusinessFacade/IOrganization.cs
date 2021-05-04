using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using OOAdvantech;
using FlavourBusinessFacade.HumanResources;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{bb5a5a32-329d-40b7-befa-612818c38f00}</MetaDataID>
    [BackwardCompatibilityID("{bb5a5a32-329d-40b7-befa-612818c38f00}")]
    [HttpVisible]
    [GenerateFacadeProxy]
    public interface IOrganization : IParty, IUser
    {
      
        /// <MetaDataID>{4ad24667-981e-4578-8400-7c4c3e01c570}</MetaDataID>
        List<ITranslator> GetTranslators(WorkerState state );



        /// <MetaDataID>{463ea8ba-656b-4a34-b53c-c82743a947c8}</MetaDataID>
        List<HumanResources.IAccountability> GetMenuMakers(WorkerState state);

        /// <MetaDataID>{652d896e-d50f-46b6-8985-393b09210331}</MetaDataID>
        HumanResources.ITranslator AssignTranslatorRoleToUser(UserData userData);

        /// <MetaDataID>{47da4876-7b08-49d1-b29f-7c0885206904}</MetaDataID>
        HumanResources.IAccountability AssignMenuMakerRoleToUser(UserData userData);

        /// <MetaDataID>{8d00bce1-770f-4765-b476-dddfb22f03ea}</MetaDataID>
        IFlavoursServicesContext GetFlavoursServicesContext(string servicesContextIdentity);

        /// <MetaDataID>{a94d6637-00e9-409a-9753-62c33de30f87}</MetaDataID>
        string NewSupervisor(string servicesContextIdentity);

        /// <MetaDataID>{1b1336c6-bbd8-4d91-b989-3721692c5228}</MetaDataID>
        List<OrganizationStorageRef> GraphicMenus { get; }

        /// <MetaDataID>{934d8558-c390-4daf-9f54-3bdb664f854c}</MetaDataID>
        void RemoveGraphicMenu(string storageIdentity);

        /// <MetaDataID>{430052e4-5142-4421-a8a8-2d2438d3d4f5}</MetaDataID>
        OrganizationStorageRef NewGraphicMenu(string culture);

        /// <MetaDataID>{873ebb97-df8b-43eb-ad78-fb02a3849c8a}</MetaDataID>
        OrganizationStorageRef UpdateStorage(string name, string description, string storageIdentity);

        /// <MetaDataID>{bd6625bb-9739-4325-b0b4-9a4edc905059}</MetaDataID>
        OrganizationStorageRef GetStorage(OrganizationStorages dataType);

        /// <MetaDataID>{3a545216-8803-4775-843c-6b2c5ca94e81}</MetaDataID>
        IUploadSlot GetUploadSlotFor(OrganizationStorageRef storageRef);

        /// <MetaDataID>{fbe9af00-028a-4e3a-9870-5295d7167e6a}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string SignUpUserIdentity { get; }

        event ObjectChangeStateHandle ObjectChangeState;
    

        /// <MetaDataID>{26644a5c-b6b5-4ef6-9fd1-f63fd2618f6c}</MetaDataID>
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.CascadeDelete)]
        [RoleAMultiplicityRange(0)]
        [Association("OrganizationServicesContext",OOAdvantech.MetaDataRepository.Roles.RoleA, "ee8ad5cc-cee0-4fa8-9c66-8cb708a061cb")]
        [RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.IList<FlavourBusinessFacade.IFlavoursServicesContext> ServicesContexts { get; }

        /// <MetaDataID>{96f09ff2-a902-4a79-bd0f-4e229bdead11}</MetaDataID>
        IFlavoursServicesContext NewFlavoursServicesContext();

        /// <MetaDataID>{53e001d0-d4b9-4c87-8076-6b720dfb43a3}</MetaDataID>
        void DeleteServicesContext(IFlavoursServicesContext servicesContext);
        /// <MetaDataID>{21506bdc-1cae-4e38-9fb4-f9d31c911199}</MetaDataID>
        void RemoveTranslator(ITranslator translator);


        /// <MetaDataID>{32265818-886c-4786-9cef-1e8ec935cde3}</MetaDataID>
        void RemoveMenuMaker(IAccountability menuMakingAccountability);


        /// <MetaDataID>{6257849b-a066-4bff-a9ac-c85c62c3c85e}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Trademark { get; set; }
        /// <MetaDataID>{7287ce4c-b6dc-48dc-b896-15706a75d4d3}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string Address { get; set; }


    }


    /// <MetaDataID>{8bb59d23-3d40-4feb-80ae-082b65047fad}</MetaDataID>
    public enum WorkerState
    {
        All = 0,
        Active = 1
    }





}