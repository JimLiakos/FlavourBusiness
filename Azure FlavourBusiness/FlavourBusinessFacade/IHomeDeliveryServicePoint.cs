using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{50c6d802-9608-43bd-93ec-0bff5800cce4}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{50c6d802-9608-43bd-93ec-0bff5800cce4}")]
    public interface IHomeDeliveryServicePoint : IServicePoint
    {
        /// <MetaDataID>{482d4de9-39ad-4f72-b395-acc6fe8b44d3}</MetaDataID>
        [Association("DeliveryServicePlace", Roles.RoleA, "00e54418-df8f-48ea-aed5-4c9a69e35c38")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        EndUsers.IPlace PlaceOfDistribution { get; set; }

        /// <MetaDataID>{1498e334-33ff-452d-9790-b0434b62cf6b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+5")]
        decimal MinimumOrderValue { get; set; }

        /// <MetaDataID>{71e97fe2-bb20-406a-b82b-a7097049bbd8}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+4")]
        decimal MinimumShippingFee { get; set; }

        /// <MetaDataID>{6491ecf3-29a5-4254-a9e9-c2fa0abf3a34}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
        decimal FreeShippingMinimumOrderValue { get; set; }

        /// <MetaDataID>{59c2d7fb-cff7-4546-98a9-980513766fdd}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        List<EndUsers.Coordinate> ServiceAreaMap { get; set; }

        /// <MetaDataID>{683fcd4f-30ce-4d25-9d78-1f894a7d2623}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        Dictionary<System.DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule { get; set; }
    }


    /// <MetaDataID>{78e89630-8ce4-4984-91c7-42ce083f1a4b}</MetaDataID>
    public struct OpeningHours
    {
        /// <MetaDataID>{7e6ed39c-744d-4ee8-9f08-e27cd6819462}</MetaDataID>
        public System.DateTime SartsAt;
        /// <MetaDataID>{531e0650-2eb1-4fa0-af19-00eec90d272c}</MetaDataID>
        public System.DateTime EndsAt;
    }
}

