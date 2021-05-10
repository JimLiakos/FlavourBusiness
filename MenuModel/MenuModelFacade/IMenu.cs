using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{ef3af014-723e-4e4b-986e-bacaed0c6e3c}</MetaDataID>
    public interface IMenu
    {
        
        [Association("MenuTaxes", Roles.RoleA, "9c43c481-6bbd-4f0d-b507-9b2731a79fd4")]
        [RoleAMultiplicityRange(1, 1)]
        FinanceFacade.ITaxAuthority TaxAuthority { get; set; }


        /// <MetaDataID>{9cb46c63-624e-4128-bbea-851c37784c35}</MetaDataID>
        string Name { get; set; }
        [Association("MenuRootCategory", Roles.RoleA, "54a8fde1-4dde-4502-818a-6b4ab18f4abd")]
        [RoleAMultiplicityRange(1, 1)]
        IItemsCategory RootCategory { get; set; }

    }
}