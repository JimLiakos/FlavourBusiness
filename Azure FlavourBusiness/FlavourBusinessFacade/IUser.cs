using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{87146d61-e4a0-448e-b3ef-7e88b867e5a1}</MetaDataID>
    [BackwardCompatibilityID("{87146d61-e4a0-448e-b3ef-7e88b867e5a1}")]
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IUser
    {
        /// <MetaDataID>{15203774-3abf-4857-ba78-d75c592b8dcd}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        string PhotoUrl { get; set; }


        /// <MetaDataID>{57584c7b-659b-4170-8cbb-f02896b9ad08}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string FullName { get; set; }

        /// <MetaDataID>{b868bbf4-97a8-4ae7-8904-ba63ca5f7748}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [CachingDataOnClientSide]
        string Email { get; set; }

        /// <MetaDataID>{4d4beaf2-fd59-478f-aab0-3bd99b9cd0d8}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [CachingDataOnClientSide]
        string PhoneNumber { get; set; }

        /// <MetaDataID>{9a5f6d03-7855-49c8-981f-81dd4884e61f}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        string UserName { get; set; }

        /// <MetaDataID>{c47b71cd-37d1-4645-b031-76dad9a3f2fc}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        string Identity { get; }


        /// <MetaDataID>{1b60fecf-7877-424a-a125-6b17c8f27325}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
        System.Collections.Generic.List<UserData.UserRole> Roles { get; }
    }
}