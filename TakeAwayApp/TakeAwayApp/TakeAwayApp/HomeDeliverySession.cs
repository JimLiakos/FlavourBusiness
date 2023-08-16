using DontWaitApp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
#if DeviceDotNet

using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;

#else
using System.Windows.Media.TextFormatting;
#endif

namespace TakeAwayApp
{
    /// <MetaDataID>{4cc9fb7d-6b96-44f5-9d05-a5f74fd7034d}</MetaDataID>
    public class HomeDeliverySession : MarshalByRefObject, IHomeDeliverySession, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        

        public HomeDeliverySession(FlavoursServiceOrderTakingStation flavoursServiceOrderTakingStation, IFoodServicesClientSessionViewModel foodServicesClientSessionViewModel, string callerPhone = "")
        {
            FlavoursServiceOrderTakingStation=flavoursServiceOrderTakingStation;
            FoodServiceClientSession=foodServicesClientSessionViewModel;
            _CallerPhone=callerPhone;
            State=CallerCenterSessionState.OrderTaking;
        }

        public IFoodServicesClientSessionViewModel FoodServiceClientSession { get; }

        private FlavoursServiceOrderTakingStation FlavoursServiceOrderTakingStation;

        /// <exclude>Excluded</exclude>
        string _CallerPhone;

        public string CallerPhone
        {
            get => _CallerPhone;
            set
            {
                if (_CallerPhone!= value)
                {
                    _CallerPhone= value;
                    if (!string.IsNullOrWhiteSpace(_CallerPhone))
                    {

                        SearchResultClients = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.FoodServiceClientsSearch(_CallerPhone);

                        var homeDeliveryClient = SearchResultClients.FirstOrDefault();


                        if (homeDeliveryClient.FoodServiceClient==null)
                        {
                            homeDeliveryClient.FoodServiceClient=new FoodServiceClient(homeDeliveryClient.UniqueId);
                            homeDeliveryClient.FoodServiceClient.PhoneNumber=_CallerPhone;
                            SessionClient=new FoodServiceClientVM((FoodServiceClientSession as FoodServicesClientSessionViewModel).FlavoursOrderServer, homeDeliveryClient);
                            SessionClient.PhoneNumber=_CallerPhone;

                            
                            if (FoodServiceClientSession.FoodServicesClientSession == null && SessionClient != null&& _HomeDeliveryServicePoint!=null)
                            {
                                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                                (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetFoodServicesClientSession(SessionClient.FullName, SessionClient.Identity, DeviceType.Desktop, device.FirebaseToken, _HomeDeliveryServicePoint);
                            }


                            //OOAdvantech.Remoting.RemotingServices.IsOutOfProcess()
                        }
                        if (SessionClient.FoodServiceClient!=null)
                            SessionClient=new FoodServiceClientVM((FoodServiceClientSession as FoodServicesClientSessionViewModel).FlavoursOrderServer, homeDeliveryClient);

                        (FoodServiceClientSession as FoodServicesClientSessionViewModel).EndUser=SessionClient;

                    }
                    else
                        SessionClient=null;
                }
            }
        }
        List<FoodServiceClienttUri> SearchResultClients = new List<FoodServiceClienttUri>();

        public FoodServiceClientVM SessionClient { get; set; }





        public System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location)
        {
            return FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetNeighborhoodFoodServers(location);
        }

        public CallerCenterSessionState State
        {
            get;
            set;
        }

        /// <exclude>Excluded</exclude>
        HomeDeliveryServicePointAbbreviation _HomeDeliveryServicePoint;
        public HomeDeliveryServicePointAbbreviation HomeDeliveryServicePoint
        {
            get => _HomeDeliveryServicePoint;


            set
            {
                _HomeDeliveryServicePoint= value;
                if (FoodServiceClientSession.FoodServicesClientSession==null&& SessionClient!=null)
                {
                    OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession=FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetFoodServicesClientSession(SessionClient.FullName, SessionClient.Identity, DeviceType.Desktop, device.FirebaseToken, _HomeDeliveryServicePoint);

                }
            }
        }

        /// <exclude>Excluded</exclude>
        List<HomeDeliveryServicePointAbbreviation> _HomeDeliveryServicePoints;

        public List<HomeDeliveryServicePointAbbreviation> HomeDeliveryServicePoints
        {
            get => _HomeDeliveryServicePoints;
            internal set
            {
                _HomeDeliveryServicePoints= value;
                if(_HomeDeliveryServicePoints?.Count==1)
                    this.HomeDeliveryServicePoint=_HomeDeliveryServicePoints[0];
            }
        }
    }
}