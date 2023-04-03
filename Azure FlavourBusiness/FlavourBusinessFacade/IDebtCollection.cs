using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{a3245ab8-ec18-4e83-9893-4b8bb26b5ce7}</MetaDataID>
    [BackwardCompatibilityID("{a3245ab8-ec18-4e83-9893-4b8bb26b5ce7}")]
    [GenerateFacadeProxy]
    [HttpVisible]
    public interface IDebtCollection
    {
        /// <MetaDataID>{d493e25f-862c-47f2-a7ac-090d1f39c587}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        decimal CardsUserDeclared { get; }

        /// <MetaDataID>{8d6341ee-8f50-4b23-a623-e144bbae51ad}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        decimal CardsTipsUserDeclared { get; }
        [Association("CollectedPayments", Roles.RoleA, "2f6452a6-19f9-415b-ad28-b50d26dd24d6")]
        System.Collections.Generic.List<FinanceFacade.IPayment> BillingPayments { get; }


        /// <MetaDataID>{30aa32e1-97e9-4efa-af8e-8b0d9d273445}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        decimal OpeningBalanceFloatCash { get; }

        /// <MetaDataID>{8e903988-21d6-4902-b654-9328d7669169}</MetaDataID>
        void CashierClose();

        /// <MetaDataID>{c26fdb45-3cb0-4c27-9378-c63700b11d2f}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        bool AccountIsClosed { get; }
        /// <MetaDataID>{66cf46f7-8b9c-48b5-b214-629aa2dd8d1a}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        decimal Cash { get; }

        /// <MetaDataID>{984756f2-5f5c-464c-9971-5da905705605}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        decimal Cards { get; }

        /// <MetaDataID>{afb59f33-8b3c-45f1-9ed6-bc76fed2dbf5}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        decimal CardsTips { get; }

        /// <MetaDataID>{994117a3-e348-47bb-a131-f2c261db870b}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        decimal CashTips { get; }


    }
}
//https://docs.oracle.com/cd/E53547_01/opera_5_04_03_core_help/close_cashier.htm