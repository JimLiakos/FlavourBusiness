using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{b57ff0bc-5343-49a4-ba51-910c805375b2}</MetaDataID>
    public class HomeDeliveryServicePointInfo
    {
        public string BrandName;
        public string LogoBackgroundImageUrl;
        public string LogoImageUrl;
        public Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule;

        public string ServicePointIdentity;
        public HomeDeliveryServicePointInfo(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {
            ServicePointIdentity = homeDeliveryServicePoint.ServicesContextIdentity + ";" + homeDeliveryServicePoint.ServicesPointIdentity;
            LogoBackgroundImageUrl = homeDeliveryServicePoint.LogoBackgroundImageUrl;
            LogoImageUrl = homeDeliveryServicePoint.LogoImageUrl;
            BrandName = homeDeliveryServicePoint.BrandName;

            WeeklyDeliverySchedule = homeDeliveryServicePoint.WeeklyDeliverySchedule;

        }
        public HomeDeliveryServicePointInfo()
        {

        }

    }
}
