using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{1470eb53-1488-4d56-955e-a2cea88fc722}</MetaDataID>
    [BackwardCompatibilityID("{1470eb53-1488-4d56-955e-a2cea88fc722}")]
    public interface ITaxesContext
    {
        [Association("TaxOverride", Roles.RoleB, "42de5995-db4d-4076-bcef-88e364a59db1")]
        [AssociationClass(typeof(ITaxOverride))]
        IList<ITaxOverride> TaxOverrides { get; }

        /// <MetaDataID>{25f342b0-5889-43d0-9233-2ba9f500ca7b}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }



        /// <MetaDataID>{b2cfef85-149a-44ff-8ebe-9d40c2d83b91}</MetaDataID>
        ITaxOverride GetTaxOverride(ITax tax,bool create=false);
        /// <MetaDataID>{662b0a4b-2384-4243-938a-99ddf14bf47b}</MetaDataID>
        void RemoveTaxOverride(ITaxOverride taxOverride);
    }
}