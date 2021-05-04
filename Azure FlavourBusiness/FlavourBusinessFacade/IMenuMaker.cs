namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{16175185-5c62-454b-81bd-62eeb5c5a5c7}</MetaDataID>
    public interface IMenuMaker : IParty
    {

        /// <MetaDataID>{6bfd6889-192c-446d-8bb0-fd81f54cb071}</MetaDataID>
        IActivity NewMenuDesignActivity(IAccountability menuMakingAccountability, string subjectDescription, DesignSubjectType selectedTranslationType, string subjcectIdentity);

        OrganizationStorageRef GetStorage(OrganizationStorages dataType);

        OrganizationStorageRef GetGraphicMenu(IActivity menuMakingActivity);

        OrganizationStorageRef GetGraphicMenuItems(IActivity menuMakingActivity);
    }
}