using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade;
using System;
using System.Collections.Generic;
using System.Text;
using FlavourBusinessFacade.HomeDelivery;
using OOAdvantech.Json;

namespace TakeAwayApp.ViewModel
{
    public class WatchingOrderPresentation
    {  

        public WatchingOrderPresentation(WatchingOrder watchingOrder, List<FlavourBusinessManager.RoomService.ViewModel.MealCourse> mealCourses)
        {
            ServerWatchingOrder = watchingOrder;
            HomeDeliveryServicePoint = watchingOrder.HomeDeliveryServicePoint;
            TimeStamp = watchingOrder.TimeStamp;
            DeliveryPlace = watchingOrder.DeliveryPlace;
            SessionID = watchingOrder.SessionID;
            SessionType = watchingOrder.SessionType;
            MealCourses = mealCourses;
            MealCourses.AddRange(mealCourses);
            EntryDateTime = watchingOrder.EntryDateTime;
            DistributionDateTime = watchingOrder.DistributionDateTime;
            State = watchingOrder.State;
            Description = watchingOrder.DeliveryPlace?.Description;
            ClientPhone=watchingOrder.ClientPhone;
            OrderCode = watchingOrder.OrderCode;

            OrderTotal=watchingOrder.OrderTotal;
            
            



        }
        public string OrderCode { get; set; }
        public PayAmount OrderTotal { get; set; }
        public WatchingOrderState State { get; set; }
        public string Description { get; }
        public string ClientPhone { get; set; }
        public HomeDeliveryServicePointAbbreviation HomeDeliveryServicePoint { get; set; }

        public string TimeStamp { get; set; }

        public IPlace DeliveryPlace { get; set; }

        public string SessionID { get; set; }
        public SessionType SessionType { get;  set; }
        public List<FlavourBusinessManager.RoomService.ViewModel.MealCourse> MealCourses { get; set; }

        public DateTime EntryDateTime { get; set; }

        public DateTime? DistributionDateTime { get; set; }

        [JsonIgnore]
        public WatchingOrder ServerWatchingOrder { get; }
    }
}
