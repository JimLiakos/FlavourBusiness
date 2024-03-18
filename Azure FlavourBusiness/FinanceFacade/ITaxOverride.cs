using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{aa142034-c096-43cf-b1d0-b7cd60852c66}</MetaDataID>
    [BackwardCompatibilityID("{aa142034-c096-43cf-b1d0-b7cd60852c66}")]
    [AssociationClass(typeof(ITaxesContext), typeof(ITax), "TaxOverride")]
    public interface ITaxOverride
    {
        /// <MetaDataID>{2f7593d4-495f-4d1a-a97d-984118d2f9c4}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        bool IsActive { get; set; }

        /// <MetaDataID>{4b8160b7-52f7-4db2-9ecd-a6e781e8ec13}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        double TaxRate { get; set; }

        /// <MetaDataID>{7f411354-12d6-49cd-b583-eaccc64a3022}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string AccountID { get; set; }

        /// <MetaDataID>{89743e31-6dcf-41f5-b669-69a46adbfcba}</MetaDataID>
        [AssociationClassRole(Roles.RoleB)]
        ITax Tax { get; set; }

         

        /// <MetaDataID>{9b008ecf-f372-4d9b-8ffd-f418278e34e2}</MetaDataID>
        [AssociationClassRole(Roles.RoleA)]
        ITaxesContext TaxesContext { get; set; }
        /// <MetaDataID>{f59c0624-d479-4a1c-b590-3e2cdde391d9}</MetaDataID>
        decimal Fee { get; set; }
    }
}