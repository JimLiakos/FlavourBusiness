using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade.HomeDelivery
{
    public class WatchingOrder
    {
        public IHomeDeliveryServicePoint HomeDeliveryServicePoint;
        public string TimeStamp;
        IPlace DeleiveryPlace;
    }
}


//var ticks = new DateTime(2022, 1, 1).Ticks;
//var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
