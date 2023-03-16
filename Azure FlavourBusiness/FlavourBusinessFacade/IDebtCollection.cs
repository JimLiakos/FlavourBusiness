using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{a3245ab8-ec18-4e83-9893-4b8bb26b5ce7}</MetaDataID>
    public interface IDebtCollection
    {
        [Association("CollectedPayments", Roles.RoleA, "2f6452a6-19f9-415b-ad28-b50d26dd24d6")]
        System.Collections.Generic.List<FinanceFacade.IPayment> BillingPayments { get; }
    }
}