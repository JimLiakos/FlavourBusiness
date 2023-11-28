using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HomeDelivery;
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
        CallCenterStationWatchingOrders GetWatchingOrders(List<WatchingOrderAbbreviation> candidateToRemoveWatchingOrders = null);


        List<ReturnReason> ReturnReasons { get; }

    }





}

