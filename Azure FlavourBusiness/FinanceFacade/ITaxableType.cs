
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{983f82cb-182e-4dda-8a07-ff395f43f7e9}</MetaDataID>
    [BackwardCompatibilityID("{983f82cb-182e-4dda-8a07-ff395f43f7e9}")]
    public interface ITaxableType
    {
        [Association("TaxableType", Roles.RoleB, "4451a5bd-476a-4b7a-a0ca-af63e2229c1d")]
        List<ITaxable> TaxableSubjects { get; }

        /// <MetaDataID>{3ea9ef81-0fe3-4077-9666-9a725458b783}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string Description { get; set; }

        /// <MetaDataID>{4731e70a-9e66-4d36-8680-8d6634e33da8}</MetaDataID>
        [Association("TaxableTypeTaxes", Roles.RoleA, "f7cd85a2-93d6-4d24-a571-9ec045818360")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1)]
        IList<ITax> Taxes { get; }


        /// <MetaDataID>{8407a389-b642-4db3-b4e5-0f87206a6e00}</MetaDataID>
        ITax NewTax();

        /// <MetaDataID>{5f52d968-974c-4d83-a06a-f5e5eb1d04a5}</MetaDataID>
        void RemoveTax(ITax tax);
    }
}