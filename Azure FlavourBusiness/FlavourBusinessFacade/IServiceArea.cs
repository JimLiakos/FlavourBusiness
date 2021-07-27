using System.Collections.Generic;
using MenuModel;
using OOAdvantech.MetaDataRepository;



namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{dfd73d38-4a9b-4da1-82a4-8a2fb6bf5657}</MetaDataID>
    [BackwardCompatibilityID("{dfd73d38-4a9b-4da1-82a4-8a2fb6bf5657}")]
    [GenerateFacadeProxy]
    public interface IServiceArea
    {
        [Association("ServiceAreaMealType", Roles.RoleA, true, "e184cdc3-8ed3-49a3-82ae-b169da1c1efd")]
        [RoleAMultiplicityRange(1)]
        IList<IMealType> ServesMealTypes { get; }

        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


        event ServicePointChangeStateHandle ServicePointChangeState;

        [CachingDataOnClientSide]
        IList<string> ServesMealTypesUris { get; }

        /// <MetaDataID>{86f446ae-4501-441d-b102-0d702c53651e}</MetaDataID>
        void AddMealType(string mealTypeUri);

        /// <MetaDataID>{bf64d7fd-1493-411e-af4c-178a8704fa85}</MetaDataID>
        void RemoveMealType(string mealTypeUri);

        bool MealTypeAssigned(string mealTypeUri);

        /// <MetaDataID>{86fcf48a-56e6-4faf-ae9d-8221c07d7fa9}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string ServicesContextIdentity { get; set; }


        /// <MetaDataID>{73329b00-3049-4c8f-ba3b-bf106d2470e0}</MetaDataID>
        void AddServicePoint(IServicePoint servicePoint);

        /// <MetaDataID>{d647d6e0-713f-490b-b41a-31dc0acd1cb7}</MetaDataID>
        IServicePoint NewServicePoint();


        /// <MetaDataID>{439de996-d72f-4566-b068-1ef9dfe7abe8}</MetaDataID>
        void RemoveServicePoint(IServicePoint servicePoint);




        [Association("AreaServicePoints", Roles.RoleA, "27b5c804-1630-41b4-975e-cf64dc1969a0")]
        [RoleBMultiplicityRange(1, 1)]
        IList<IServicePoint> ServicePoints { get; }


        /// <MetaDataID>{db297d40-fa68-4081-bd56-9db25db803e2}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string Description { get; set; }

        /// <MetaDataID>{aa692cf4-56de-4e86-abda-d690ce7a5465}</MetaDataID>
        List<IServicePoint> GetUnassignedServicePoints(List<string> hallServicesPoints);
    }

    public delegate void ServicePointChangeStateHandle(object _object, IServicePoint servicePoint);

}