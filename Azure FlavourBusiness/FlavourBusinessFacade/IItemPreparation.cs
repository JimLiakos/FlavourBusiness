using System;
using System.Collections.Generic;
using FinanceFacade;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{794bbf34-5df9-4ab0-9572-5773309ecc4c}</MetaDataID>
    [BackwardCompatibilityID("{794bbf34-5df9-4ab0-9572-5773309ecc4c}")]
    public interface IItemPreparation
    {
    

        /// <MetaDataID>{c73cd53f-286f-4a56-8c08-39a810ca40bf}</MetaDataID>
        void AddPayment(IPayment payment);

        void RemovePayment(IPayment payment);

        [Association("ItemPayment", Roles.RoleA, "52506146-9fd6-4dde-a9fd-a7acf7871e88")]
        [RoleAMultiplicityRange(1)]
        List<IPayment> Payments { get; }

        /// <MetaDataID>{c2c236bd-77c2-4557-97fd-8c8d8e96f67a}</MetaDataID>
        [BackwardCompatibilityID("+19")]
        int PreparatioOrder { get; set; }

        /// <MetaDataID>{673ef44c-06a5-40f4-b2b0-4a8bdc3cb0ac}</MetaDataID>
        [BackwardCompatibilityID("+18")]
        DateTime? CookingStartsAt { get; }

        /// <MetaDataID>{bdda9ee6-b111-4a50-b569-bbc73c0e203a}</MetaDataID>
        [BackwardCompatibilityID("+17")]
        double PreparationTimeSpanInMin { get; set; }

        /// <MetaDataID>{865fa0cc-d3f1-4e1b-abc8-99f1a06a4d2a}</MetaDataID>
        [BackwardCompatibilityID("+16")]
        double CookingTimeSpanInMin { get; set; }

        /// <MetaDataID>{45efae09-2879-4a32-8d4d-e290d076b0d5}</MetaDataID>
        int AppearanceOrder { get; set; }
        /// <summary>
        /// Defines the tracking transfer.
        /// Any time where item is  transferred between service points
        /// the source service point added to tracking data
        /// </summary>
        /// <MetaDataID>{e8dc77ff-fcc4-42be-ae8f-2d691e61bc77}</MetaDataID>
        [BackwardCompatibilityID("+15")]
        string TransferTracking { get; set; }


        [Association("PreparedItemsToServe", Roles.RoleB, "2b36e0e0-e305-45c5-9b91-4f13b7048c84")]
        [RoleBMultiplicityRange(1, 1)]
        IServingBatch ServedInTheBatch { get; }

        /// <MetaDataID>{6e004433-34de-46b4-9264-e4c052bca0f1}</MetaDataID>
        [BackwardCompatibilityID("+13")]
        DateTime? PreparedAtForecast { get; }

        /// <MetaDataID>{1f985a50-1044-4821-bcef-1e24a8f8eda1}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        DateTime? PreparationStartsAt { get; }

        /// <MetaDataID>{3ee58a43-e219-491d-aa71-f7531bd96a29}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        string Comments { get; set; }

        /// <MetaDataID>{2c1b457c-28d5-475a-96cb-4f8edd7c98b8}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        string Name { get; set; }
        [Association("PreparationStationItems", Roles.RoleB, "e7e482f4-cfba-45a5-8270-8346af757d7a")]
        ServicesContextResources.IPreparationStation PreparationStation { get; set; }

        /// <MetaDataID>{6d81b0c8-acd5-4b40-b2e4-dd29ff3dbc8c}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        string SelectedMealCourseTypeUri { get; set; }

        [Association("ServiceSectionFoodItems", Roles.RoleB, "170a4e1d-1241-4efd-a037-01a2c2a3456b")]
        [RoleBMultiplicityRange(1, 1)]
        FlavourBusinessFacade.RoomService.IMealCourse MealCourse { get; set; }

        /// <MetaDataID>{537883cd-8a78-422f-b0b1-e62befeea88d}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        bool CustomItemEnabled { get; set; }

        /// <MetaDataID>{ca27acad-3456-4472-99c1-0b655600604b}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        System.DateTime StateTimestamp { get; }


        [Association("ClientsSharedItems", Roles.RoleB, "d3f8b87e-0f43-4373-939e-950dd9db19b2")]
        [RoleBMultiplicityRange(1)]
        IList<IFoodServiceClientSession> SharedWithClients { get; }
        [Association("ClientFlavourItems", Roles.RoleB, "913f7a7e-cd2a-4833-a0dc-dde7987eefff")]
        [RoleBMultiplicityRange(1, 1)]
        EndUsers.IFoodServiceClientSession ClientSession { get; }



        [Association("ItemPreparationOptionsChange", Roles.RoleA, "8e564693-11c5-445c-8cb3-1f9505a2f2d2")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.CascadeDelete)]
        IList<IOptionChange> OptionsChanges { get; }

        /// <MetaDataID>{34ad6825-ed6f-48be-8624-22ff698ebdfa}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        bool IsShared { get; set; }

        /// <MetaDataID>{a98681a4-dd88-44a4-a6c2-5dbfbdf26fb1}</MetaDataID>
        int NumberOfShares { get; }


        /// <MetaDataID>{708a1905-18e0-448f-af34-b3bbdd819364}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string uid { get; set; }


        /// <MetaDataID>{d86f1b54-4295-4d9c-93f4-2e24e419542c}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        ItemPreparationState State { get; set; }
        /// <MetaDataID>{d6f26cd4-5d10-4d71-a8f4-01a8b13bc9c2}</MetaDataID>
        [BackwardCompatibilityID("+14")]
        string CodeCard { get; set; }



        /// <MetaDataID>{a5d18eca-428e-410b-96f0-1d9ac050af07}</MetaDataID>
        bool IsCooked { get; }

        
    }


    /// <MetaDataID>{80f52b7f-98a7-4212-8875-2fdb6a709d0e}</MetaDataID>
    public enum ItemPreparationState
    {
        /// <summary>
        /// Client temporary select item for order
        /// </summary>
        New = 0,
        /// <summary>
        /// Client committed to order this item
        /// </summary>
        Committed = 1,
        /// <summary>
        /// this item is slow to prepare for meal synchronization.
        /// </summary>
        PreparationDelay = 2,
        /// <summary>
        /// The item is ready to prepared.
        /// </summary>
        PendingPreparation = 3,
        /// <summary>
        /// The item is under preparation
        /// </summary>
        InPreparation = 4,

        /// <summary>
        /// The item is cooked
        /// </summary>
        IsRoasting = 5,

        /// <summary>
        /// The item is prepared
        /// </summary>
        IsPrepared = 6,
        /// <summary>
        /// The item is on serving state
        /// </summary>
        Serving = 7,

        /// <summary>
        /// the item is on road to delivered 
        /// </summary>
        OnRoad = 8,
        /// <summary>
        /// the item was served
        /// </summary>
        Served = 9,

        /// <summary>
        /// the item was billed
        /// </summary>
        IsBilled = 10,

  
        /// <summary>
        /// the item is canceled
        /// </summary>
        Canceled = 12,


    }

    /// <MetaDataID>{02a0de31-8f78-40f0-92cf-9a6cc11846f1}</MetaDataID>
    public static class ItemPreparationStateEx
    {
        /// <MetaDataID>{52d16497-7a11-47e5-9020-8517e3d26493}</MetaDataID>
        static public bool IsInFollowingState(this ItemPreparationState thisState, ItemPreparationState state)
        {
            //following 
            return ((int)thisState) > ((int)state);
        }
        /// <MetaDataID>{b1802d54-39e2-45cc-b67e-4c8740ae9445}</MetaDataID>
        static public bool IsIntheSameOrFollowingState(this ItemPreparationState thisState, ItemPreparationState state)
        {
            //following 
            return ((int)thisState) >= ((int)state);
        }

        /// <MetaDataID>{3e7af055-3c4e-4dc6-a201-64a5c6579372}</MetaDataID>
        static public bool IsInPreviousState(this ItemPreparationState thisState, ItemPreparationState state)
        {
            //previous
            return ((int)thisState) < ((int)state);
        }
        /// <MetaDataID>{16e68989-ae1e-4f10-9ac7-08cf7c3c8f76}</MetaDataID>
        static public bool IsInTheSameOrPreviousState(this ItemPreparationState thisState, ItemPreparationState state)
        {
            //previous
            return ((int)thisState) <= ((int)state);
        }

    }

    /// <MetaDataID>{035494c0-f39d-441e-b659-5dd7213ea78e}</MetaDataID>
    public class ItemPreparationPlan
    {
        public DateTime PreparationStart { get; set; }
        public double Duration { get; set; }
        public double CookingDuration { get; set; }
    }

}