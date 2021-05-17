using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{86b829b9-37e3-4b0e-8e43-bfda17b6c97d}</MetaDataID>
    public interface IMeal
    {
        [Association("MealCourses", Roles.RoleA, "3c1213a5-f6e9-4d34-8802-72a4f051472b")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1)]
        System.Collections.Generic.List<IMealCourse> Courses { get; }
    }
}