using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{aa142034-c096-43cf-b1d0-b7cd60852c66}</MetaDataID>
    [BackwardCompatibilityID("{aa142034-c096-43cf-b1d0-b7cd60852c66}")]
    [OOAdvantech.MetaDataRepository.AssociationClass(typeof(ITaxesContext), typeof(ITax), "TaxOverride")]
    public interface ITaxOverride
    {
        [BackwardCompatibilityID("+2")]
        double TaxRate { get; set; }

        [BackwardCompatibilityID("+1")]
        string AccountID { get; set; }

        /// <MetaDataID>{89743e31-6dcf-41f5-b669-69a46adbfcba}</MetaDataID>
        [AssociationClassRole(Roles.RoleB)]
        ITax Tax { get; set; }



        /// <MetaDataID>{9b008ecf-f372-4d9b-8ffd-f418278e34e2}</MetaDataID>
        [AssociationClassRole(Roles.RoleA)]
        ITaxesContext TaxesContext { get; set; }
    }
}