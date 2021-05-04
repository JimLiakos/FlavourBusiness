

using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{9ad5edca-fcf1-4d91-83af-6397a36bc23b}</MetaDataID>
    [BackwardCompatibilityID("{9ad5edca-fcf1-4d91-83af-6397a36bc23b}")]
    public interface ITaxable
    {
        /// <MetaDataID>{b8f154e5-9cd8-492f-a02e-0770a93cbf09}</MetaDataID>
        [Association("TaxableType", Roles.RoleA, "4451a5bd-476a-4b7a-a0ca-af63e2229c1d")]
        [RoleAMultiplicityRange(1, 1)]
        ITaxableType TaxableType { get; set; }
    }
}