using System.Collections.Generic;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{50c6d802-9608-43bd-93ec-0bff5800cce4}</MetaDataID>
    public interface IHomeDeliveryServicePoint : IServicePoint
    {

        /// <MetaDataID>{683fcd4f-30ce-4d25-9d78-1f894a7d2623}</MetaDataID>
        Dictionary<System.DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule { get; set; }

        /// <MetaDataID>{59c2d7fb-cff7-4546-98a9-980513766fdd}</MetaDataID>
        List<EndUsers.Coordinate> ServiceAreaMap { get; set; }
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

