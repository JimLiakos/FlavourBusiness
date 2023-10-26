using CourierApp.ViewModel;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService.ViewModel;
using OOAdvantech.MetaDataRepository;
using ServiceContextManagerApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using WaiterApp.ViewModel;

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

        /// <MetaDataID>{3ea873d4-60b0-4b09-9c56-cef23527b81a}</MetaDataID>
        List<ITakeawayCashierPresentation> TakeawayCashiers { get; }

        /// <MetaDataID>{389d2beb-9076-45ff-adbe-fb0a4fe64424}</MetaDataID>
        List<ICourierPresentation> Couriers { get; }


        void ShiftWorkStart(IWorkerPresentation worker, DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{199ed84e-71be-47a6-9aa8-79baa01af5bd}</MetaDataID>
        List<MealCourse> MealCoursesInProgress { get; }

        /// <MetaDataID>{059a02f3-5f3c-4768-84bb-681a837bfa0e}</MetaDataID>
        bool RemoveSupervisor(ISupervisorPresentation supervisorPresentation);

        void IWillTakeCare(string messageID);



        /// <MetaDataID>{d97fe7f6-6751-4709-99f7-86ab667f7e94}</MetaDataID>
        void MoveBefore(string mealCourseUri, string movedMealCourseUri);
        /// <MetaDataID>{f01ed829-964f-41c1-8a2d-ef9ea1297fb2}</MetaDataID>
        void MoveAfter(string mealCourseUri, string movedMealCourseUri);


        ISupervisorPresentation SignedInSupervisor { get; }

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

        /// <MetaDataID>{96811d18-cf1f-474e-b0be-7432e88f86f3}</MetaDataID>
        NewUserCode GetNewTakeAwayCashierQRCode(string color);

        /// <MetaDataID>{133db335-fd6c-4ecf-832d-3da1d8c3f431}</MetaDataID>
        NewUserCode GetNewCourierQRCode(string color);

        /// <MetaDataID>{d4999189-2e89-4ba7-b5d4-0020b56dc3f2}</MetaDataID>
        NewUserCode GetNewNativeUserQRCode(IWorkerPresentation worker, string color);


        // void AddNewWaiterAsNativeUser(NewUserCode newUserCode,)

        /// <MetaDataID>{3a7f890b-c57c-4bf8-b2fd-c9ce2b82ea8d}</MetaDataID>
        IList<IHallLayout> Halls { get; }


        /// <MetaDataID>{64f4f0cc-35a1-4253-b701-e32ee952fb60}</MetaDataID>
        IWaiter AssignWaiterNativeUser(string waiterAssignKey, string userName, string password, string userFullName);

        /// <MetaDataID>{de581aad-2071-4f90-9fcb-6dc3d20f5f8f}</MetaDataID>
        ITakeawayCashier AssignTakeAwayCashierNativeUser(string takeAwayCashierAssignKey, string userName, string password, string userFullName);

        /// <MetaDataID>{4151f8dc-d587-48ce-9e39-c13449564d0c}</MetaDataID>
        ICourier AssignCourierNativeUser(string takeAwayCashierAssignKey, string userName, string password, string userFullName);


        [GenerateEventConsumerProxy]
        event DelayedMealAtTheCounterHandle DelayedMealAtTheCounter;

        List<DelayedServingBatchAbbreviation> DelayedServingBatchesAtTheCounter { get; }

        FoodShippingPresentation GetFoodShipping(DelayedServingBatchAbbreviation delayedServingBatch);
        ServingBatchPresentation GetServingBatch(DelayedServingBatchAbbreviation delayedServingBatch);

    }

    public delegate void DelayedMealAtTheCounterHandle(ISupervisorPresentation supervisorPresentation, string messageID);
    public delegate void ServicePointChangeStateHandle(IServicesContextPresentation servicesContextPresentation, string servicePointIdentity, ServicePointState newState);

}
