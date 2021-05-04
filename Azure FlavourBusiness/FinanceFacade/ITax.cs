using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{cde10026-ea3b-407e-bd5a-b4976499a44e}</MetaDataID>
    [BackwardCompatibilityID("{cde10026-ea3b-407e-bd5a-b4976499a44e}")]
    public interface ITax
    {
        [BackwardCompatibilityID("+3")]
        double TaxRate { get; set; }
        [BackwardCompatibilityID("+2")]
        string AccountID { get; set; }

        /// <MetaDataID>{c11c70ad-9373-4a5e-aaf0-1d3cf1089982}</MetaDataID>
        [Association("TaxOverride", Roles.RoleA, "42de5995-db4d-4076-bcef-88e364a59db1")]
        [AssociationClass(typeof(ITaxOverride))]
        IList<ITaxesContext> TaxOverrides { get; }

        /// <MetaDataID>{247e41d4-ffa8-4896-a7e5-60718fbd3334}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }
    }
}