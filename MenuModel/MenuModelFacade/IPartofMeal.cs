using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{5c8afe23-042b-4575-968c-439160500491}</MetaDataID>
    [AssociationClass(typeof(IMealType), typeof(IMenuItem), "PartofMeal")]
    public interface IPartofMeal
    {
        [Association("", Roles.RoleA, "933b04ac-ddd0-48e3-840f-5abea246dfad")]
        [RoleAMultiplicityRange(1, 1)]
        IMealCourseType MealCourseType { get; set; }


        /// <MetaDataID>{5f048e28-3e06-4b21-9c84-5a6b429f75e4}</MetaDataID>
        [AssociationClassRole(Roles.RoleA)]
        IMealType MealType { get; }

        /// <MetaDataID>{5dbbac07-dceb-4285-9448-7684d5906cb6}</MetaDataID>
        [AssociationClassRole(Roles.RoleB)]
        IMenuItem MenuItem { get; }
    }
}