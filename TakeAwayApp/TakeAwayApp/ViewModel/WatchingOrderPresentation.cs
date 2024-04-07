using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade;
using System;
using System.Collections.Generic;
using System.Text;

using OOAdvantech.Json;
using FlavourBusinessFacade.ServicesContextResources;

namespace TakeAwayApp.ViewModel
{
    /// <MetaDataID>{22697e86-8646-4738-a7a7-189eaef3638b}</MetaDataID>
    public class WatchingOrderPresentation
    {

        public WatchingOrderPresentation(WatchingOrder watchingOrder, List<FlavourBusinessManager.RoomService.ViewModel.MealCourse> mealCourses)
        {
            ServerWatchingOrder = watchingOrder;
            ServicePoint = watchingOrder.ServicePoint;
            TimeStamp = watchingOrder.TimeStamp;
            DeliveryPlace = watchingOrder.DeliveryPlace;
            SessionID = watchingOrder.SessionID;
            SessionType = watchingOrder.SessionType;
            MealCourses = mealCourses;
            //MealCourses.AddRange(mealCourses);
            EntryDateTime = watchingOrder.EntryDateTime;
            DistributionDateTime = watchingOrder.DistributionDateTime;
            State = watchingOrder.State;
            Description = watchingOrder.DeliveryPlace?.Description;
            ClientPhone = watchingOrder.ClientPhone;
            OrderCode = watchingOrder.OrderCode;

            OrderTotal = watchingOrder.OrderTotal;





        }
        public string OrderCode { get; set; }
        public PayAmount OrderTotal { get; set; }
        public WatchingOrderState State { get; set; }
        public string Description { get; }
        public string ClientPhone { get; set; }
        public ServicePointAbbreviation ServicePoint { get; set; }

        public string TimeStamp { get; set; }

        public IPlace DeliveryPlace { get; set; }

        public string SessionID { get; set; }
        public SessionType SessionType { get; set; }
        public List<FlavourBusinessManager.RoomService.ViewModel.MealCourse> MealCourses { get; set; }

        public DateTime EntryDateTime { get; set; }

        public DateTime? DistributionDateTime { get; set; }

        [JsonIgnore]
        public WatchingOrder ServerWatchingOrder { get; }
    }
}
