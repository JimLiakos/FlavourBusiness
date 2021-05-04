using System.Collections.Generic;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{794bbf34-5df9-4ab0-9572-5773309ecc4c}</MetaDataID>
    [BackwardCompatibilityID("{794bbf34-5df9-4ab0-9572-5773309ecc4c}")]
    public interface IItemPreparation
    {
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
        [AssociationEndBehavior(PersistencyFlag.OnConstruction|PersistencyFlag.CascadeDelete)]
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
    }


    /// <MetaDataID>{80f52b7f-98a7-4212-8875-2fdb6a709d0e}</MetaDataID>
    public enum ItemPreparationState
    {
        /// <summary>
        /// Client temporary select item for order
        /// </summary>
        New=0,
        /// <summary>
        /// Client committed to order this item
        /// </summary>
        Committed=1,
        /// <summary>
        /// The item is ready to prepared.
        /// </summary>
        PendingPreparation=2,
        /// <summary>
        /// The item is under preparation
        /// </summary>
        OnPreparation=3,
        /// <summary>
        /// The item is prepared
        /// </summary>
        Prepared=4,
        /// <summary>
        /// The item is on serving state
        /// </summary>
        Serving=5,
        /// <summary>
        /// the item served
        /// </summary>
        Served=6,
        /// <summary>
        /// the item is on road to delivered 
        /// </summary>
        OnRoad=7,
        /// <summary>
        /// the item is canceled
        /// </summary>
        Canceled=8
    }

}