
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


        ITax NewTax();

        void RemoveTax(ITax tax);
    }
}