using OOAdvantech.MetaDataRepository;
namespace MenuModel
{
    /// <MetaDataID>{bb183748-af5e-4f0c-b51d-4fde7a8cbf27}</MetaDataID>
    public interface IRecipe
    {
        /// <MetaDataID>{0e7550ac-06d0-4748-a325-b34732c0b8cf}</MetaDataID>
        [Association("RecipeIngredient", Roles.RoleA, "b7644e52-742d-4464-a57d-089184b56ed4"), RoleAMultiplicityRange(0), OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.List<IIngredient> Ingredients
        {
            get;
        }
        /// <MetaDataID>{3b84c11d-7e1d-4222-9447-fc9760394083}</MetaDataID>
        string Name
        {
            get;
        }
    }
}
