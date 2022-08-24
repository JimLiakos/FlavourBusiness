using FlavourBusinessFacade;
using System;
using System.Collections.Generic;
using System.Text;

namespace DontWaitAppNS.ViewModel
{
    /// <MetaDataID>{b57ff0bc-5343-49a4-ba51-910c805375b2}</MetaDataID>
    public class HomeDeliveryServicePointInfo
    {
        string BrandName;
        public string LogoBackgroundImageUrl;
        public string LogoImageUrl;
        public Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule;
    }
}
