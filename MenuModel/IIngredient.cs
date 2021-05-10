using OOAdvantech.MetaDataRepository;
namespace MenuModel
{
    /// <MetaDataID>{7f1582dc-69c8-4455-8ae8-7782e7b203a7}</MetaDataID>
    public interface IIngredient
    {
        [Association("RecipeIngredient", Roles.RoleB, "b7644e52-742d-4464-a57d-089184b56ed4"), OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        IRecipe PartOfRecipe
        {
            get;
        }
        /// <MetaDataID>{652ed18e-a08d-43ed-b4b4-98f6e3f18a69}</MetaDataID>
        void Foo();
    }
}
