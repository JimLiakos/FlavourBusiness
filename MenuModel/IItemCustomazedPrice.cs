using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{926dacdd-f0a3-41cc-b26d-b4c8843521d6}</MetaDataID>
    [BackwardCompatibilityID("{926dacdd-f0a3-41cc-b26d-b4c8843521d6}")]
    [OOAdvantech.MetaDataRepository.AssociationClass(typeof(IPricedSubject), typeof(IPricingContext), "CustomazedPrice")]
    public interface ICustomizedPrice
    {
        /// <MetaDataID>{e360765b-8d70-4b5c-894d-6113db590e97}</MetaDataID>
        [AssociationClassRole(Roles.RoleA)]
        [BackwardCompatibilityID("+2")]
        IPricedSubject PricedSubject { get; }

        /// <MetaDataID>{52be9be4-34d2-4264-8d5a-dabaabbc3686}</MetaDataID>
        [AssociationClassRole(Roles.RoleB)]
        [BackwardCompatibilityID("+1")]
        IPricingContext PricingContext { get; }

        /// <MetaDataID>{727a67fc-527b-45a5-9232-495d639b330f}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        decimal Price { get; set; }
      
    }
}
