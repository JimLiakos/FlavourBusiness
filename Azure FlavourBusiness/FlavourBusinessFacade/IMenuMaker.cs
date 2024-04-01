using System.Collections.Generic;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{16175185-5c62-454b-81bd-62eeb5c5a5c7}</MetaDataID>
    public interface IMenuMaker : IParty
    {

        /// <MetaDataID>{6bfd6889-192c-446d-8bb0-fd81f54cb071}</MetaDataID>
        IActivity NewMenuDesignActivity(IAccountability menuMakingAccountability, string subjectDescription, DesignSubjectType selectedTranslationType, string subjcectIdentity);

        /// <MetaDataID>{cdb01e59-d4ff-4a2c-8dd6-04585cb47766}</MetaDataID>
        OrganizationStorageRef GetStorage(OrganizationStorages dataType);

        /// <MetaDataID>{c3a25f17-d70e-4773-8ab0-d6ad6bfc39f2}</MetaDataID>
        OrganizationStorageRef GetGraphicMenu(IActivity menuMakingActivity);

        /// <MetaDataID>{02b8db90-d5b6-46f6-b8dc-70bd4d250169}</MetaDataID>
        OrganizationStorageRef GetGraphicMenuItems(IActivity menuMakingActivity);


       List< OrganizationStorageRef> GetPriceLists(IActivity menuMakingActivity);

        /// <MetaDataID>{3d31eae6-9a26-4e0d-8079-33bde813e90a}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string OAuthUserIdentity { get; }

    }
}