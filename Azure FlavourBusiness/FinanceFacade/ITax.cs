using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{cde10026-ea3b-407e-bd5a-b4976499a44e}</MetaDataID>
    [BackwardCompatibilityID("{cde10026-ea3b-407e-bd5a-b4976499a44e}")]
    public interface ITax
    {
        /// <MetaDataID>{6e27d6f4-fdf7-4c72-81cb-29fd82ce6a7e}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double TaxRate { get; set; }
        /// <MetaDataID>{fb56d83b-c6ef-4265-b462-7b028473f002}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string AccountID { get; set; }

        /// <MetaDataID>{c11c70ad-9373-4a5e-aaf0-1d3cf1089982}</MetaDataID>
        [Association("TaxOverride", Roles.RoleA, "42de5995-db4d-4076-bcef-88e364a59db1")]
        [AssociationClass(typeof(ITaxOverride))]
        IList<ITaxOverride> TaxOverrides { get; }

        /// <MetaDataID>{247e41d4-ffa8-4896-a7e5-60718fbd3334}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }
    }
}