using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.HomeDelivery
{
    /// <MetaDataID>{e87404cd-1460-49dc-9cee-fc1c4ad4f7fd}</MetaDataID>
    public class WatchingOrder
    {
        public HomeDeliveryServicePointAbbreviation HomeDeliveryServicePoint { get; set; }

        public string TimeStamp { get; set; }

        public IPlace DeliveryPlace { get; set; }

        public string SessionID { get; set; }
        public SessionType SessionType { get; set; }
        public List<IMealCourse> MealCourses { get; set; }

        public DateTime EntryDateTime { get; set; }

        public DateTime? DistributionDateTime { get; set; }
        public WatchingOrderState State { get; set; }
    }
    public enum WatchingOrderState
    {
        Scheduled = 1,
        InProggres = 2
    }
    public class WatchingOrderAbbreviation
    {
        public string SessionID { get; set; }
        public string TimeStamp { get; set; }
    }

    public class CallCenterStationWatchingOrders
    {
        public List<WatchingOrder> WatchingOrders { get; set; }

        public List<WatchingOrderAbbreviation> RemovedWatchingOrders { get; set; }


    }
}




//var ticks = new DateTime(2022, 1, 1).Ticks;
//var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
