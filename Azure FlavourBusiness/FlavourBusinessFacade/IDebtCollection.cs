using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{a3245ab8-ec18-4e83-9893-4b8bb26b5ce7}</MetaDataID>
    [GenerateFacadeProxy]
    [HttpVisible]
    public interface IDebtCollection
    { 
        [Association("CollectedPayments", Roles.RoleA, "2f6452a6-19f9-415b-ad28-b50d26dd24d6")]
        System.Collections.Generic.List<FinanceFacade.IPayment> BillingPayments { get; }
         

        /// <MetaDataID>{30aa32e1-97e9-4efa-af8e-8b0d9d273445}</MetaDataID>
        decimal OpeningBalanceFloatCash { get; }

        /// <MetaDataID>{8e903988-21d6-4902-b654-9328d7669169}</MetaDataID>
        void CashierClose();

        /// <MetaDataID>{66cf46f7-8b9c-48b5-b214-629aa2dd8d1a}</MetaDataID>
        decimal Cash { get; }

        /// <MetaDataID>{984756f2-5f5c-464c-9971-5da905705605}</MetaDataID>
        decimal Cards { get; }

        /// <MetaDataID>{afb59f33-8b3c-45f1-9ed6-bc76fed2dbf5}</MetaDataID>
        decimal CardsTips { get; }

        /// <MetaDataID>{994117a3-e348-47bb-a131-f2c261db870b}</MetaDataID>
        decimal CashTips { get; }


    }
}
//https://docs.oracle.com/cd/E53547_01/opera_5_04_03_core_help/close_cashier.htm