using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{

    public delegate void NewMealCoursesInrogressHandel(IList<IMealCourse> mealCoursers);

    /// <MetaDataID>{a078dd80-08f5-4a00-b67f-574a91f11dbe}</MetaDataID>
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IMealsController
    {
        /// <MetaDataID>{1b7e3487-f9a7-406f-ac68-75bca97d4350}</MetaDataID>
        [Association("ControlledMealCourses", Roles.RoleA, "97ee130e-9190-48a1-8a40-2a9bfddbd336")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.List<IMealCourse> MealCoursesInProgress { get; }


        event NewMealCoursesInrogressHandel NewMealCoursesInrogress;
    }
}