using System.Collections.Generic;
using FlavourBusinessFacade.EndUsers;
using MenuModel;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{eb9f035f-cecb-4687-b88a-1545f71704b2}</MetaDataID>
    [BackwardCompatibilityID("{eb9f035f-cecb-4687-b88a-1545f71704b2}")]
    [GenerateFacadeProxy]
    public interface IServicePoint
    {


        /// <MetaDataID>{c9005e2f-0975-4d5a-a543-4b58935fbd3e}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        [CachingDataOnClientSide]
        System.Collections.Generic.IList<string> ServesMealTypesUris { get; }

        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


  
        [Association("ServicePointMealType", Roles.RoleA, true, "1c10159e-488c-45bd-bd93-af7013cb1ce5")]
        IList<IMealType> ServesMealTypes { get; }

        /// <MetaDataID>{25afee87-8ad0-4abc-9cb0-971aa8a03924}</MetaDataID>
        void AddMealType(string mealTypeUri);

        /// <MetaDataID>{9fd93ae8-9978-40b8-9c69-f879bd3eb10d}</MetaDataID>
        void RemoveMealType(string mealTypeUri);



        /// <MetaDataID>{d4a506ca-d55f-4e33-9354-85724e8630fc}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        ServicePointState State { get; set; }


        /// <MetaDataID>{ffbbbbd1-7687-4c51-a022-b487f11ac0c0}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [CachingDataOnClientSide]
        ServicePointType ServicePointType { get; set; }

        /// <MetaDataID>{5660d599-1687-4185-a1e9-dfba216b6823}</MetaDataID>
        System.Collections.Generic.IList<IFoodServiceClientSession> ActiveFoodServiceClientSessions
        {
            get;
        }
        [RoleAMultiplicityRange(0)]
        [Association("ServicePointSesions", Roles.RoleA, "08fdaee2-f871-4200-9856-8d2cc9754909")]
        System.Collections.Generic.IList<IFoodServiceSession> ServiceSessions { get; }

        /// <MetaDataID>{39fb52e5-2700-4284-b361-09f4049d8178}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        string ServicesPointIdentity { get; set; }


        /// <MetaDataID>{dc907bcc-35cf-4189-8c77-88c1c855be71}</MetaDataID>
        string ServicePointUrl { get; }

        /// <MetaDataID>{52bd3eff-ab52-4b34-8a78-7a5e5020d5a4}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{56fee2b7-7636-44e5-b109-20bb54b24388}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [CachingDataOnClientSide]
        string Description { get; set; }

        /// <MetaDataID>{a6b4c73c-c603-4b5c-bf22-abaddfdd2504}</MetaDataID>
        void AddFoodServiceSession(IFoodServiceSession foodServiceSession);

        /// <MetaDataID>{e2347beb-2b4d-4869-a947-331fbd4aea90}</MetaDataID>
        void RemoveFoodServiceSession(IFoodServiceSession foodServiceSession);

        /// <MetaDataID>{dac5d02b-367f-41b4-ba49-efd0801c3565}</MetaDataID>
        IFoodServiceSession NewFoodServiceSession();
       




        /// <MetaDataID>{5faee0ed-1a08-41b3-8e4b-8339a588df90}</MetaDataID>
        IFoodServiceClientSession NewFoodServiceClientSession(string clientName, string clientDeviceID, string deviceFirebaseToken);


        ///  <MetaDataID>{6329ebf0-633d-4759-887f-89dc0e157ddb}</MetaDataID>
        /// <summary>
        /// Provides a session between client and service point.
        /// In case where there isn't session and the create flag is false return s null;
        /// </summary>
        /// <param name="clientName">
        /// Defines the client nick name
        /// </param>
        /// <param name="mealInvitationSessionID">
        /// Defines the identity which is necessary to make client session part of meal  
        /// </param>
        /// <param name="clientDeviceID">
        /// Defines the identity of device which used from the client to send its order 
        /// </param>
        /// <param name="deviceFirebaseToken">
        /// Defines the device firebase token for push notification facility 
        /// </param>
        /// <param name="clientIdentity">
        /// Defines the identity of client.
        /// Used only in case where client is signed in
        /// </param>
        /// <param name="create">
        /// In case where this flag is true, service points creates a session if there isn't.
        /// </param>
        /// <returns>
        /// return the client session
        /// </returns>
        IFoodServiceClientSession GetFoodServiceClientSession(string clientName, string mealInvitationSessionID, string clientDeviceID, string deviceFirebaseToken, bool create, bool create = false);


        /// <MetaDataID>{56809dc3-ae5d-4abb-a1ec-14a9cabc6e44}</MetaDataID>
        IList<IFoodServiceClientSession> GetServicePointOtherPeople(IFoodServiceClientSession serviceClientSession);


    }



    /// <MetaDataID>{92D4966E-E823-4124-A2BB-A3D17389E9D5}</MetaDataID>
    public interface IHallServicePoint: IServicePoint
    {

        [Association("AreaServicePoints", Roles.RoleB, "27b5c804-1630-41b4-975e-cf64dc1969a0")]
        [RoleBMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        IServiceArea ServiceArea { get; }


        /// <MetaDataID>{b0c0b13d-1963-4765-b73e-f7df3ce3d5e1}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        int Seats { get; set; }


    }

    /// <MetaDataID>{dd9d7ee9-ef6e-488b-b464-200ffd7c722d}</MetaDataID>
    public enum ServicePointState
    {
        Free = 0,
        Laying = 1,
        Conversation = 2,
        ConversationTimeout = 3,
        MealCoursePreparation = 4,
        MealCourseOvertime = 5,
        Served=6,
        TakeAway = 7,
        Delivery = 8
    }

    /// <MetaDataID>{46658b97-1a57-4911-ab0e-ebd3235dcc5c}</MetaDataID>
    public enum ServicePointType
    {
        HallServicePoint = 0,
        TakeAway = 1,
        Delivery = 2
    }

}
