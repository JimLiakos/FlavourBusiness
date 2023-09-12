using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
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



    /// <MetaDataID>{4380724a-34f1-4b05-aa10-d22c0cafa111}</MetaDataID>
    public class HomeDeliveryServicePointAbbreviation
    {
        public string Description { get; set; }
        public string ServicesContextIdentity { get; set; }
        public string ServicesPointIdentity { get; set; }

        public double DistanceInKm { get; set; }

        public bool OutOfDeliveryRange { get; set; } = true;
        public Coordinate Location { get;set; }
    }

    /*
     Λαζαράδων 29
     
    ServiceAreaMapJson  : [{"Longitude":23.749154882460203,"Latitude":37.995717013223384},{"Longitude":23.746118621855345,"Latitude":37.996469498304741},{"Longitude":23.743919210463133,"Latitude":37.996055209720588},{"Longitude":23.742213325529661,"Latitude":37.996097484173156},{"Longitude":23.736054973631468,"Latitude":37.994913790291953},{"Longitude":23.732074575453368,"Latitude":37.995556369340605},{"Longitude":23.7332332897478,"Latitude":38.000138809324874},{"Longitude":23.734713869124022,"Latitude":38.00538881756335},{"Longitude":23.734955267935362,"Latitude":38.006652652297639},{"Longitude":23.740719335108366,"Latitude":38.006483578679067},{"Longitude":23.741502540140715,"Latitude":38.005760784563485},{"Longitude":23.742610292463866,"Latitude":38.005756557676357},{"Longitude":23.743995032319887,"Latitude":38.006542372746324},{"Longitude":23.744778237352236,"Latitude":38.007391963055511},{"Longitude":23.744831881532534,"Latitude":38.008140101593817},{"Longitude":23.746637986944283,"Latitude":38.00805164316818},{"Longitude":23.747614311025703,"Latitude":38.007451441510376},{"Longitude":23.747743057058418,"Latitude":38.006656800770529},{"Longitude":23.7491807210904,"Latitude":38.007299276929025},{"Longitude":23.74970643405732,"Latitude":38.006538449285159},{"Longitude":23.749975516981781,"Latitude":38.00627449090539},{"Longitude":23.75049050111264,"Latitude":38.006502740849264},{"Longitude":23.751327350325287,"Latitude":38.005302307259825},{"Longitude":23.751627757734955,"Latitude":38.005361484024043},{"Longitude":23.751928165144623,"Latitude":38.00283374825802},{"Longitude":23.750258226046459,"Latitude":38.001926190436642},{"Longitude":23.750526446947948,"Latitude":38.001655655021608},{"Longitude":23.74972178424348,"Latitude":38.000776408029971},{"Longitude":23.750377969770994,"Latitude":37.999766813805238},{"Longitude":23.750809926117686,"Latitude":37.998640586271613}]

    PlaceOfDistributionJson : {"ExtensionProperties":{},"CityTown":"Αθήνα","StateProvinceRegion":null,"Description":"Λαζαράδων 29, Αθήνα, Ελλάδα","StreetNumber":"29","Street":"Λαζαράδων","Area":null,"PostalCode":"113 63","Country":"Ελλάδα","Location":{"Longitude":23.7472703,"Latitude":38.00026},"PlaceID":"ChIJxZ_kbbqioRQRYiLJ5_50sQQ","Default":true}


    Λάκωνος
    
    ServiceAreaMapJson  : [{"Longitude":23.756942191409244,"Latitude":37.997785513901349},{"Longitude":23.754968085574284,"Latitude":37.997075316210584},{"Longitude":23.753895201968326,"Latitude":37.996314382481863},{"Longitude":23.753444590853825,"Latitude":37.9951306921011},{"Longitude":23.753873744296207,"Latitude":37.9936087763995},{"Longitude":23.752500453280582,"Latitude":37.990564850264242},{"Longitude":23.751749434756412,"Latitude":37.988247999610365},{"Longitude":23.75780049829401,"Latitude":37.986962707840867},{"Longitude":23.76495109988965,"Latitude":37.985793796814477},{"Longitude":23.766410221593752,"Latitude":37.987079109060993},{"Longitude":23.767182697790041,"Latitude":37.987890873615555},{"Longitude":23.767526020543947,"Latitude":37.9884658680742},{"Longitude":23.771388401525392,"Latitude":37.990867266805964},{"Longitude":23.775637020604982,"Latitude":37.993031840552639},{"Longitude":23.769113888280764,"Latitude":37.998206265799595},{"Longitude":23.765594830053224,"Latitude":38.001114606002155},{"Longitude":23.76396404697217,"Latitude":38.001114606002155},{"Longitude":23.759887089269533,"Latitude":38.000810250221747},{"Longitude":23.758814205663576,"Latitude":37.999897175301392},{"Longitude":23.758556713598146,"Latitude":37.998950270783524},{"Longitude":23.757784237401857,"Latitude":37.998104809989734}]

    PlaceOfDistributionJson : {"ExtensionProperties":{},"CityTown":"Αθήνα","StateProvinceRegion":null,"Description":"Λάκωνος 26, Αθήνα, Ελλάδα ","StreetNumber":"26","Street":"Λάκωνος","Area":null,"PostalCode":"115 24","Country":"Ελλάδα","Location":{"Longitude":23.765026,"Latitude":37.9960904},"PlaceID":"ChIJq1RxdAiYoRQRvVR_EYQH2gw","Default":true}


     */

}
