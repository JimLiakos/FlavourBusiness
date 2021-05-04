using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{4d8977ad-04fa-4d10-9985-32b40be8783b}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{4d8977ad-04fa-4d10-9985-32b40be8783b}")]
    public interface IActivity
    {
        [Association("ÁssignmentActivity", Roles.RoleB, "6f55ad2f-f9c6-4804-a25c-1567968b17fe")]
        [RoleBMultiplicityRange(1, 1)]
        FlavourBusinessFacade.HumanResources.IAccountability Accountability { get; }

        /// <MetaDataID>{dc30a26a-e1fb-4006-990f-e9adfdd18ac5}</MetaDataID>
        [CachingDataOnClientSide]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string Name { get; set; }
    }
}