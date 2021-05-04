using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{e2f5fd0a-5bdc-4ad2-9ed4-37bf92496b57}</MetaDataID>
    [BackwardCompatibilityID("{e2f5fd0a-5bdc-4ad2-9ed4-37bf92496b57}")]
    public interface ITaxAuthority
    {
        /// <MetaDataID>{d6e530e0-2086-4366-bcc6-766144f6f7fe}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Identity { get; set; }

        /// <MetaDataID>{44221c56-42e6-4090-8a84-211b40842ab9}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string Notes { get; set; }

        /// <MetaDataID>{ff93dd07-b966-4204-a5fc-aaf7deebd8a6}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name { get; set; }

        /// <MetaDataID>{8af037bb-2f5c-4fe3-b72a-414fa0e9c721}</MetaDataID>
        void AddTaxableType(ITaxableType taxableType);


        /// <MetaDataID>{d3202754-2e96-4724-a705-74fa6da60951}</MetaDataID>
        ITaxableType NewTaxableType();

        /// <MetaDataID>{e97fb40a-0e0c-46dd-8db6-98d3b7c536d3}</MetaDataID>
        void RemoveTaxableType(ITaxableType taxableType);

        [Association("AuthorityTaxableTypes", Roles.RoleA, "4b415a9e-1195-4e8e-be2b-07edf585e114")]
        [RoleBMultiplicityRange(1, 1)]
        IList<ITaxableType> TaxableTypes { get; }
   
    }
}