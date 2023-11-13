using FlavourBusinessFacade.Shipping;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.RoomService
{

    public delegate void NewMealCoursesInProgressHandler(IList<IMealCourse> mealCoursers);

    public delegate void MealCourseChangeStateHandler(IMealCourse mealCourser, string memberName);
    public delegate void MealCourseItemsStateChangedHandler(IMealCourse mealCourser, Dictionary<string, ItemPreparationState> newItemsState);

    /// <MetaDataID>{a078dd80-08f5-4a00-b67f-574a91f11dbe}</MetaDataID>
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IMealsController
    {
        /// <MetaDataID>{1b7e3487-f9a7-406f-ac68-75bca97d4350}</MetaDataID>
        [Association("ControlledMealCourses", Roles.RoleA, "97ee130e-9190-48a1-8a40-2a9bfddbd336")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.List<IMealCourse> MealCoursesInProgress { get; }


        System.Collections.Generic.List<IMealCourse> GetMealCoursesInProgress(string filter, int age );


        event NewMealCoursesInProgressHandler NewMealCoursesInProgress;

        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        event MealCourseChangeStateHandler MealCourseChangeState;
        event MealCourseItemsStateChangedHandler MealCourseItemsStateChanged;

        void MoveCourseBefore(string mealCourseAsReferenceUri, string movedMealCourseUri);
        void MoveCourseAfter(string mealCourseAsReferenceUri, string movedMealCourseUri);



        List<DelayedServingBatchAbbreviation> GetDelayedServingBatchesAtTheCounter(double delayInMins);
        IFoodShipping GetMealCourseFoodShipping(string mealCourseUri);
    }
}