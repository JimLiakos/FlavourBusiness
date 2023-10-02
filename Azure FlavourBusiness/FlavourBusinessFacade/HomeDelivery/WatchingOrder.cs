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
        public string OrderCode { get; set; } = "082439A";

        public HomeDeliveryServicePointAbbreviation HomeDeliveryServicePoint { get; set; }

        public string TimeStamp { get; set; }

        public IPlace DeliveryPlace { get; set; }

        public string SessionID { get; set; }
        public SessionType SessionType { get; set; }
        public List<IMealCourse> MealCourses { get; set; }

        public DateTime EntryDateTime { get; set; }

        public DateTime? DistributionDateTime { get; set; }
        public WatchingOrderState State { get; set; }
        public string ClientPhone { get; set; }
        public PayAmount OrderTotal { get; set; }
        public string ClientIdentity { get; set; }
    }
    /// <MetaDataID>{7538441d-4d07-4d1b-94db-178635ea46b4}</MetaDataID>
    public enum WatchingOrderState
    {
        Scheduled = 1,
        InProggres = 2
    }
    /// <MetaDataID>{f8b64534-b67c-4224-87e8-1d4ad2f57330}</MetaDataID>
    public class WatchingOrderAbbreviation
    {
        public string SessionID { get; set; }
        public string TimeStamp { get; set; }
    }
     
    /// <MetaDataID>{991b4aaf-6eae-4a72-b4f7-7e3df79aaae5}</MetaDataID>
    public class CallCenterStationWatchingOrders
    {
        public List<WatchingOrder> WatchingOrders { get; set; } = new List<WatchingOrder>();

        public List<WatchingOrderAbbreviation> MissingWatchingOrders { get; set; } = new List<WatchingOrderAbbreviation>();


    }
}




//var ticks = new DateTime(2022, 1, 1).Ticks;
//var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
