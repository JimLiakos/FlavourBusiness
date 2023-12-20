using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HomeDelivery;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.Shipping;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{50c6d802-9608-43bd-93ec-0bff5800cce4}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{50c6d802-9608-43bd-93ec-0bff5800cce4}")]
    [GenerateFacadeProxy]
    public interface IHomeDeliveryServicePoint : IServicePoint
    {
        /// <MetaDataID>{b9bc9dc2-b444-4109-99c6-1a976c0ca7b5}</MetaDataID>
        string BrandName { get; set; }

        /// <MetaDataID>{1b9b4e82-ea23-4e58-affd-673fa5154d12}</MetaDataID>
        string LogoImageUrl { get; }

        /// <MetaDataID>{024a78c6-04ff-455e-8e14-8740031b1648}</MetaDataID>
        string LogoBackgroundImageUrl { get; }

        /// <MetaDataID>{482d4de9-39ad-4f72-b395-acc6fe8b44d3}</MetaDataID>
        [Association("DeliveryServicePlace", Roles.RoleA, "00e54418-df8f-48ea-aed5-4c9a69e35c38")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        EndUsers.IPlace PlaceOfDistribution { get; set; }

        /// <MetaDataID>{1498e334-33ff-452d-9790-b0434b62cf6b}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        decimal MinimumOrderValue { get; set; }

        /// <MetaDataID>{71e97fe2-bb20-406a-b82b-a7097049bbd8}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        decimal ShippingCost { get; set; }

        /// <MetaDataID>{6491ecf3-29a5-4254-a9e9-c2fa0abf3a34}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        decimal FreeShippingMinimumOrderValue { get; set; }

        /// <MetaDataID>{59c2d7fb-cff7-4546-98a9-980513766fdd}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        List<Coordinate> ServiceAreaMap { get; set; }

        /// <MetaDataID>{683fcd4f-30ce-4d25-9d78-1f894a7d2623}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        Dictionary<System.DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule { get; set; }



        /// <MetaDataID>{e742d9f1-b736-4768-b430-1e9800fef424}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
        EndUsers.Coordinate? MapCenter { get; set; }

        /// <MetaDataID>{15e0af3c-2358-437f-a9c5-ea2e6b8276e9}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        [CachingDataOnClientSide]
        double Zoom { get; set; }

        /// <MetaDataID>{3b8096e5-9f00-4b91-a370-272611acb091}</MetaDataID>
        void Update(string brandName,IPlace placeOfDistribution, Coordinate? mapCenter, List<Coordinate> serviceAreaMap, bool isPolyline, double zoom, Dictionary<System.DayOfWeek, List<OpeningHours>> weeklyDeliverySchedule, decimal minimumOrderValue, decimal shippingCost, decimal freeShippingMinimumOrderValue);

        /// <MetaDataID>{36217abe-f4f6-4029-b841-a99a609a540a}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        [CachingDataOnClientSide]
        bool IsPolyline { get; set; }


        /// <MetaDataID>{e2a99b55-15ed-48a7-9ec0-5b9b2e8c62a2}</MetaDataID>
        IUploadSlot GetUploadSlotForLogoImage();
        /// <MetaDataID>{e1a5d545-2883-4c58-bdb5-da1d8f5d61a7}</MetaDataID>
        IUploadSlot GetUploadSlotForLogoBackgroundImage();
        /// <MetaDataID>{705ac1ed-dbd3-49b5-9331-397ea7c34593}</MetaDataID>
        CallCenterStationWatchingOrders GetWatchingOrders(List<WatchingOrderAbbreviation> candidateToRemoveWatchingOrders = null);
        /// <MetaDataID>{12a55af9-dcfe-4418-93d8-be4d74151e6e}</MetaDataID>
        CourierShippingPair GetCourierShipping(string scannedCode);


        /// <MetaDataID>{d24a60bd-6fea-4e5d-9175-21d4b59a4243}</MetaDataID>
        List<ReturnReason> ReturnReasons { get; }

    }

    /// <MetaDataID>{fef2fcfc-ee9b-4e21-a43e-9de61f54c304}</MetaDataID>
    public class CourierShippingPair
    {
        public IFoodShipping FoodShipping { get; set; }

        public ICourier Courier { get; set; }

    }


    /// <MetaDataID>{a3782d3d-5eba-46bb-b2a9-cd8a8665d4fe}</MetaDataID>
    public class FoodShippingAlreadyAssignedException : OOAdvantech.Remoting.RestApi.SerializableException
    {
        public FoodShippingAlreadyAssignedException(string message, string courierIdentity, string courierName) : base(message)
        {

            CourierIdentity = courierIdentity;
            CourierFullName = courierName;
        }

        /// <MetaDataID>{fcbb4bef-c1d3-463d-bada-dea2de26f2f1}</MetaDataID>
        public FoodShippingAlreadyAssignedException(string message, System.Exception innerException, string courierIdentity, string courierName) : base(message, innerException)
        {
            CourierIdentity = courierIdentity;
            CourierFullName = courierName;
        }

        public FoodShippingAlreadyAssignedException(string message, System.Exception innerException) : base(message, innerException)
        {

        }

        public string CourierIdentity { get; set; }
        public string CourierFullName { get; set; }
    }




}

