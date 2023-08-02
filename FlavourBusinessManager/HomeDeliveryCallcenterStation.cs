using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicePointRunTime;
using MenuPresentationModel;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{eacd7ffb-4107-4ce3-ae6b-154b56792f54}</MetaDataID>
    [BackwardCompatibilityID("{eacd7ffb-4107-4ce3-ae6b-154b56792f54}")]
    [Persistent()]
    public class HomeDeliveryCallCenterStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IHomeDeliveryCallCenterStation
    {

        public HomeDeliveryCallCenterStation(ServicesContextRunTime servicesContextRunTime)

        {
            _CallcenterStationIdentity = servicesContextRunTime.ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            ServicesContextIdentity = servicesContextRunTime.ServicesContextIdentity;
        }
        protected HomeDeliveryCallCenterStation()
        {

        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IHomeDeliveryServicePoint> _HomeDeliveryServicePoints = new OOAdvantech.Collections.Generic.Set<IHomeDeliveryServicePoint>();

        /// <MetaDataID>{e3266bfc-45a7-4348-8a96-b44092eb9ae7}</MetaDataID>
        [PersistentMember(nameof(_HomeDeliveryServicePoints))]
        [BackwardCompatibilityID("+1")]
        public List<IHomeDeliveryServicePoint> HomeDeliveryServicePoints => _HomeDeliveryServicePoints.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{5c6d7b59-3a2b-40b6-90f0-392978df84ee}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get => _Description; set
            {
                if (_Description!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;


        /// <MetaDataID>{ce07ba48-ffb9-4383-b9cb-e61fc01386b1}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+3")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity; set
            {
                if (_ServicesContextIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        public System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location)
        {
            List<HomeDeliveryServicePointAbbreviation> deliveryServicePoints = new List<HomeDeliveryServicePointAbbreviation>();

            foreach (var deliveryServicePoint in HomeDeliveryServicePoints)
            {
                var placeOfDistribution = deliveryServicePoint.PlaceOfDistribution;
                if (placeOfDistribution!=null)
                {
                    double distance = Coordinate.CalDistance(location.Latitude, location.Longitude, placeOfDistribution.Location.Latitude, placeOfDistribution.Location.Longitude);

                    var polyGon = new MapPolyGon(deliveryServicePoint.ServiceAreaMap);

                    if (polyGon.FindPoint(location.Latitude, location.Longitude))
                        deliveryServicePoints.Add(new HomeDeliveryServicePointAbbreviation() { Description=deliveryServicePoint.Description,Distance=distance,ServicesContextIdentity=deliveryServicePoint.ServicesContextIdentity,ServicesPointIdentity=deliveryServicePoint.ServicesPointIdentity });
                }

            }
            return deliveryServicePoints;

        }

        /// <exclude>Excluded</exclude>
        string _CallcenterStationIdentity;
        /// <MetaDataID>{519ff5fa-ec0c-40b7-9131-6c9d156bf632}</MetaDataID>
        [PersistentMember(nameof(_CallcenterStationIdentity))]
        [BackwardCompatibilityID("+4")]
        public string CallcenterStationIdentity
        {
            get => _CallcenterStationIdentity; set
            {
                if (_CallcenterStationIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CallcenterStationIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _GraphicMenuStorageIdentity;

        /// <MetaDataID>{4e8369d3-e6a4-45ac-8a03-e79a3c051eff}</MetaDataID>
        [PersistentMember(nameof(_GraphicMenuStorageIdentity))]
        [BackwardCompatibilityID("+6")]
        public string GraphicMenuStorageIdentity
        {
            get => _GraphicMenuStorageIdentity; set
            {
                if (_GraphicMenuStorageIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _GraphicMenuStorageIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{ff543df5-ff7a-4610-99df-47046b84bce3}</MetaDataID>
        public void AddHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HomeDeliveryServicePoints.Add(homeDeliveryServicePoint);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{06f8a5e7-45a5-4ab8-97fd-3e75409531fa}</MetaDataID>
        public void RemoveHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HomeDeliveryServicePoints.Remove(homeDeliveryServicePoint);
                stateTransition.Consistent = true;
            }

        }

        public void CancelHomeDeliverFoodServicesClientSession(IFoodServiceClientSession foodServicesClientSession)
        {
            throw new NotImplementedException();
        }

        public IFoodServiceClientSession NewHomeDeliverFoodServicesClientSession()
        {
            throw new NotImplementedException();
        }

        DeviceType ClientDeviceType = DeviceType.Desktop;
        /// <exclude>Excluded</exclude>
        OrganizationStorageRef _Menu;

        public OrganizationStorageRef Menu
        {
            get
            {
                if (_Menu == null)
                {
                    OrganizationStorageRef graphicMenu = null;
                    if (ServicesContextRunTime.Current.GraphicMenus.Count==1)
                        graphicMenu = ServicesContextRunTime.Current.GraphicMenus.FirstOrDefault();
                    else
                    {
                        //var Portrait = null;
                        //var Landscape = null;

                        if (ClientDeviceType==DeviceType.Phone)
                            graphicMenu= ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsPortrait(x)).FirstOrDefault();

                        if (ClientDeviceType==DeviceType.Desktop)
                            graphicMenu= ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (ClientDeviceType==DeviceType.Tablet)
                            graphicMenu= ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (ClientDeviceType==DeviceType.TV)
                            graphicMenu= ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (graphicMenu==null)
                            graphicMenu= ServicesContextRunTime.Current.GraphicMenus.FirstOrDefault();


                    }


                    string versionSuffix = "";
                    if (!string.IsNullOrWhiteSpace(graphicMenu.Version))
                        versionSuffix = "/" + graphicMenu.Version;
                    else
                        versionSuffix = "";
                    graphicMenu.StorageUrl = RawStorageCloudBlob.RootUri + string.Format("/usersfolder/{0}/Menus/{1}{3}/{2}.json", ServicesContextRunTime.Current.OrganizationIdentity, graphicMenu.StorageIdentity, graphicMenu.Name, versionSuffix);
                    _Menu = graphicMenu;
                }
                return _Menu;
            }
        }



        //public System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location)
        //{
        //    System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> deliveryServicePoints = new OOAdvantech.Collections.Generic.List<HomeDeliveryServicePointAbbreviation>();
        //    foreach (var deliveryServicePoint in this.HomeDeliveryServicePoints)
        //    {
        //        if (deliveryServicePoint != null)
        //        {
        //            var placeOfDistribution = deliveryServicePoint.PlaceOfDistribution;
        //            if (placeOfDistribution!=null)
        //            {
        //                double distance = Coordinate.CalDistance(location.Latitude, location.Longitude, placeOfDistribution.Location.Latitude, placeOfDistribution.Location.Longitude);

        //                var polyGon = new MapPolyGon(deliveryServicePoint.ServiceAreaMap);

        //                if (polyGon.FindPoint(location.Latitude, location.Longitude))
        //                    deliveryServicePoints.Add(new HomeDeliveryServicePointAbbreviation() { Description=deliveryServicePoint.Description,Distance=distance,ServicesContextIdentity=deliveryServicePoint.ServicesContextIdentity,ServicesPointIdentity=deliveryServicePoint.ServicesPointIdentity });
        //            }
        //        }
        //    }
        //    return deliveryServicePoints;
        //}
         
        /*
         lazaradon 

            ServiceAreaMapJson: [{"Longitude":23.749154882460203,"Latitude":37.995717013223384},{"Longitude":23.746118621855345,"Latitude":37.996469498304741},{"Longitude":23.743919210463133,"Latitude":37.996055209720588},{"Longitude":23.742213325529661,"Latitude":37.996097484173156},{"Longitude":23.736054973631468,"Latitude":37.994913790291953},{"Longitude":23.732074575453368,"Latitude":37.995556369340605},{"Longitude":23.7332332897478,"Latitude":38.000138809324874},{"Longitude":23.734713869124022,"Latitude":38.00538881756335},{"Longitude":23.734955267935362,"Latitude":38.006652652297639},{"Longitude":23.740719335108366,"Latitude":38.006483578679067},{"Longitude":23.741502540140715,"Latitude":38.005760784563485},{"Longitude":23.742610292463866,"Latitude":38.005756557676357},{"Longitude":23.743995032319887,"Latitude":38.006542372746324},{"Longitude":23.744778237352236,"Latitude":38.007391963055511},{"Longitude":23.744831881532534,"Latitude":38.008140101593817},{"Longitude":23.746637986944283,"Latitude":38.00805164316818},{"Longitude":23.747614311025703,"Latitude":38.007451441510376},{"Longitude":23.747743057058418,"Latitude":38.006656800770529},{"Longitude":23.7491807210904,"Latitude":38.007299276929025},{"Longitude":23.74970643405732,"Latitude":38.006538449285159},{"Longitude":23.749975516981781,"Latitude":38.00627449090539},{"Longitude":23.75049050111264,"Latitude":38.006502740849264},{"Longitude":23.751327350325287,"Latitude":38.005302307259825},{"Longitude":23.751627757734955,"Latitude":38.005361484024043},{"Longitude":23.751928165144623,"Latitude":38.00283374825802},{"Longitude":23.750258226046459,"Latitude":38.001926190436642},{"Longitude":23.750526446947948,"Latitude":38.001655655021608},{"Longitude":23.74972178424348,"Latitude":38.000776408029971},{"Longitude":23.750377969770994,"Latitude":37.999766813805238},{"Longitude":23.750809926117686,"Latitude":37.998640586271613}]

            PlaceOfDistributionJson: {"ExtensionProperties":{},"CityTown":"Αθήνα","StateProvinceRegion":null,"Description":"Λαζαράδων 29, Αθήνα, Ελλάδα","StreetNumber":"29","Street":"Λαζαράδων","Area":null,"PostalCode":"113 63","Country":"Ελλάδα","Location":{"Longitude":23.7472703,"Latitude":38.00026},"PlaceID":"ChIJxZ_kbbqioRQRYiLJ5_50sQQ","Default":true}

         */
    }
}