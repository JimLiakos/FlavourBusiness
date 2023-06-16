using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService.ViewModel;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContextManagerApp
{
    /// <MetaDataID>{ac7629ae-b6fb-4a24-9f62-f94f27f4b9d7}</MetaDataID>
    [HttpVisible]
    public interface IServicesContextPresentation
    {
        /// <MetaDataID>{f0774cb8-3a48-404e-806d-045980e8bf92}</MetaDataID>
        string ServicesContextName { get; set; }

        /// <MetaDataID>{8d6171c5-388e-47ad-bf6f-799f1c326bc5}</MetaDataID>
        List<ISupervisorPresentation> Supervisors { get; }

        /// <MetaDataID>{b77cb491-e799-4e17-be6f-be68b760b1ea}</MetaDataID>
        List<IWaiterPresentation> Waiters { get; }

        List<ITakeawayCashierPresentation> TakeawayCashiers { get; }

        List<MealCourse> MealCoursesInProgress { get; }

        /// <MetaDataID>{059a02f3-5f3c-4768-84bb-681a837bfa0e}</MetaDataID>
        bool RemoveSupervisor(ISupervisorPresentation supervisorPresentation);



        void MoveBefore(string mealCourseUri,string movedMealCourseUri);
        void MoveAfter(string mealCourseUri,string movedMealCourseUri);



        /// <MetaDataID>{bb586ccb-551d-4d69-b2d3-d9a96081df4c}</MetaDataID>
        void MakeSupervisorActive(ISupervisorPresentation supervisorPresentation);

        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        [GenerateEventConsumerProxy]
        event FlavourBusinessFacade.EndUsers.ItemsStateChangedHandle ItemsStateChanged;

        [GenerateEventConsumerProxy]
        event MealCoursesUpdatedHandle MealCoursesUpdated;

        [GenerateEventConsumerProxy]
        event ServicePointChangeStateHandle ServicePointChangeState;

        /// <MetaDataID>{a57b4a0a-49dc-4e61-a6bd-a6a067f045d5}</MetaDataID>
        NewUserCode GetNewWaiterQRCode(string color);

        NewUserCode GetNewTakeAwayCashierQRCode(string color);

        // void AddNewWaiterAsNativeUser(NewUserCode newUserCode,)

        IList<IHallLayout> Halls { get; }


        IWaiter AssignWaiterNativeUser(string waiterAssignKey, string userName, string password, string userFullName);


    }

    public delegate void MealCoursesUpdatedHandle(IList<MealCourse> mealCourses);
    public delegate void ServicePointChangeStateHandle(IServicesContextPresentation servicesContextPresentation, string servicePointIdentity, ServicePointState newState);
}
