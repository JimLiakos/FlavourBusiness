using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{86b829b9-37e3-4b0e-8e43-bfda17b6c97d}</MetaDataID>
    public interface IMeal
    {
        /// <MetaDataID>{4ae5928f-9678-4f30-b90a-022e6f47810a}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name { get; set; }

        [Association("SessionMeal", Roles.RoleB, "b11e0deb-6ec0-4653-a06b-02610d68abcb")]
        [RoleBMultiplicityRange(1, 1)]
        ServicesContextResources.IFoodServiceSession Session { get; }
        [RoleBMultiplicityRange(1, 1)]
        [Association("MealCourses", Roles.RoleA, "3c1213a5-f6e9-4d34-8802-72a4f051472b")]
        [RoleAMultiplicityRange(1)]
        System.Collections.Generic.List<IMealCourse> Courses { get; }
    }
}