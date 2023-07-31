using DontWaitApp;
using OOAdvantech.Collections.Generic;
using System;
#if DeviceDotNet

using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;

#else
#endif

namespace TakeAwayApp
{
    /// <MetaDataID>{4cc9fb7d-6b96-44f5-9d05-a5f74fd7034d}</MetaDataID>
    public class HomeDeliverySession : IHomeDeliverySession
    {

        public HomeDeliverySession(IFoodServicesClientSessionViewModel foodServiceClientSession, string callerPhone = "")
        {
            FoodServiceClientSession = foodServiceClientSession;
            CallerPhone=callerPhone;
            State=CallerCenterSessionState.OrderTaking;
        }
        public IFoodServicesClientSessionViewModel FoodServiceClientSession { get; }

        public string CallerPhone { get; set; }

        public CallerCenterSessionState State
        {
            get;
            set;
        }
    }
}