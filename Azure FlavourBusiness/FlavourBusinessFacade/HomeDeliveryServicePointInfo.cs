using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{b57ff0bc-5343-49a4-ba51-910c805375b2}</MetaDataID>
    public class HomeDeliveryServicePointInfo
    {
        public string BrandName;
        public string LogoBackgroundImageUrl;
        public string LogoImageUrl;
        public IFlavoursServicesContextRuntime FlavoursServices;
        public Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule;

        public string ServicePointIdentity;
        public HomeDeliveryServicePointInfo(IHomeDeliveryServicePoint homeDeliveryServicePoint, IFlavoursServicesContextRuntime flavoursServicesContextRuntime)
        {
            ServicePointIdentity = homeDeliveryServicePoint.ServicesContextIdentity + ";" + homeDeliveryServicePoint.ServicesPointIdentity;
            LogoBackgroundImageUrl = homeDeliveryServicePoint.LogoBackgroundImageUrl;
            LogoImageUrl = homeDeliveryServicePoint.LogoImageUrl;
            BrandName = homeDeliveryServicePoint.BrandName;
            WeeklyDeliverySchedule = homeDeliveryServicePoint.WeeklyDeliverySchedule;
            FlavoursServices=flavoursServicesContextRuntime;

        }
        public HomeDeliveryServicePointInfo()
        {

        }

    }



    public class HomeDeliveryServicePointAbbreviation
    {
        public string Description { get; set; }
        public string ServicesContextIdentity { get; set; }
        public string ServicesPointIdentity { get; set; }

        public double Distance { get; set; }

    }

}
