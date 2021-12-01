using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{55f92046-b447-4988-bfaa-355da614ed9a}</MetaDataID>
    public interface IFisicalParty
    {
        [Association("FisicalPartyAddress", Roles.RoleA, "13449730-7348-487c-bc20-9a7f3b898778")]
        [RoleAMultiplicityRange(1, 1)]
        IAddress Address { get; set; }

        /// <MetaDataID>{a498b640-6fdb-4d0f-b214-9202d00b909d}</MetaDataID>
        string Branch { get; set; }

        /// <MetaDataID>{fe25c29f-7596-46e6-a3eb-771e1b347f5f}</MetaDataID>
        string CountryCode { get; set; }

        /// <MetaDataID>{166dab6e-f381-4ef5-8dc6-bfef9f092ab9}</MetaDataID>
        string VATNumber { get; set; }
        string Name { get; set; }
    }
}