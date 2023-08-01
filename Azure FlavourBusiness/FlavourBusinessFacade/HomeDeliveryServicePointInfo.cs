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

    /*
     Λαζαράδων 29
     
    ServiceAreaMapJson  : [{"Longitude":23.749154882460203,"Latitude":37.995717013223384},{"Longitude":23.746118621855345,"Latitude":37.996469498304741},{"Longitude":23.743919210463133,"Latitude":37.996055209720588},{"Longitude":23.742213325529661,"Latitude":37.996097484173156},{"Longitude":23.736054973631468,"Latitude":37.994913790291953},{"Longitude":23.732074575453368,"Latitude":37.995556369340605},{"Longitude":23.7332332897478,"Latitude":38.000138809324874},{"Longitude":23.734713869124022,"Latitude":38.00538881756335},{"Longitude":23.734955267935362,"Latitude":38.006652652297639},{"Longitude":23.740719335108366,"Latitude":38.006483578679067},{"Longitude":23.741502540140715,"Latitude":38.005760784563485},{"Longitude":23.742610292463866,"Latitude":38.005756557676357},{"Longitude":23.743995032319887,"Latitude":38.006542372746324},{"Longitude":23.744778237352236,"Latitude":38.007391963055511},{"Longitude":23.744831881532534,"Latitude":38.008140101593817},{"Longitude":23.746637986944283,"Latitude":38.00805164316818},{"Longitude":23.747614311025703,"Latitude":38.007451441510376},{"Longitude":23.747743057058418,"Latitude":38.006656800770529},{"Longitude":23.7491807210904,"Latitude":38.007299276929025},{"Longitude":23.74970643405732,"Latitude":38.006538449285159},{"Longitude":23.749975516981781,"Latitude":38.00627449090539},{"Longitude":23.75049050111264,"Latitude":38.006502740849264},{"Longitude":23.751327350325287,"Latitude":38.005302307259825},{"Longitude":23.751627757734955,"Latitude":38.005361484024043},{"Longitude":23.751928165144623,"Latitude":38.00283374825802},{"Longitude":23.750258226046459,"Latitude":38.001926190436642},{"Longitude":23.750526446947948,"Latitude":38.001655655021608},{"Longitude":23.74972178424348,"Latitude":38.000776408029971},{"Longitude":23.750377969770994,"Latitude":37.999766813805238},{"Longitude":23.750809926117686,"Latitude":37.998640586271613}]

    PlaceOfDistributionJson : {"ExtensionProperties":{},"CityTown":"Αθήνα","StateProvinceRegion":null,"Description":"Λαζαράδων 29, Αθήνα, Ελλάδα","StreetNumber":"29","Street":"Λαζαράδων","Area":null,"PostalCode":"113 63","Country":"Ελλάδα","Location":{"Longitude":23.7472703,"Latitude":38.00026},"PlaceID":"ChIJxZ_kbbqioRQRYiLJ5_50sQQ","Default":true}

     */

}
