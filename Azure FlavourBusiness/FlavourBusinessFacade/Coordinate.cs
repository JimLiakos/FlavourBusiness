using OOAdvantech.MetaDataRepository;
using System;

namespace FlavourBusinessFacade.EndUsers
{
    /// <MetaDataID>{4acd340b-d7fb-48de-a153-bc2b81ae37b4}</MetaDataID>
    [BackwardCompatibilityID("{4acd340b-d7fb-48de-a153-bc2b81ae37b4}")]
    [Persistent()]
    public struct Coordinate
    {
        /// <MetaDataID>{370aae2f-86f9-47cd-8662-a9028c12e573}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+2")]
        public double Longitude;

        /// <MetaDataID>{276d70e9-ba8c-47e3-b2a9-6abd46bec92a}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        public double Latitude;

        public bool IsEmpty { get =>Latitude==0&&Longitude==0; }


        /// <MetaDataID>{d5c6bdbd-b97b-4cf0-86ea-22b8deb159fb}</MetaDataID>
        public static bool operator ==(Coordinate left, Coordinate right)
        {
            if (left.Latitude == right.Latitude && left.Longitude == right.Longitude)
                return true;
            else
                return false;
        }
        /// <MetaDataID>{5243a861-9be3-4987-8593-30f7f08560cb}</MetaDataID>
        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            int num = -1162279000;
            num = (-1521134295 * num) + GetHashCode(Latitude);
            num = (-1521134295 * num) + GetHashCode(Longitude);
        
            return num;
        }

        private int GetHashCode(object partValue)
        {
            if (partValue == null)
                return 0;
            else
                return partValue.GetHashCode();
        }

        /// <MetaDataID>{d1f5e252-7a72-430c-950f-3efabc2835e0}</MetaDataID>
        public static double CalDistance(double Lat1, double Long1, double Lat2, double Long2)
        {
            /*
                The Haversine formula according to Dr. Math.
                http://mathforum.org/library/drmath/view/51879.html
                
                dlon = lon2 - lon1
                dlat = lat2 - lat1
                a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
                c = 2 * atan2(sqrt(a), sqrt(1-a)) 
                d = R * c
                
                Where
                    * dlon is the change in longtitude
                    * dlat is the change in latitude
                    * c is the great circle distance in Radians.
                    * R is the radius of a spherical Earth.
                    * The locations of the two points in 
                        spherical coordinates (longtitude and 
                        latitude) are lon1,lat1 and lon2, lat2.
            */
            double dDistance = Double.MinValue;
            double dLat1InRad = Lat1 * (Math.PI / 180.0);
            double dLong1InRad = Long1 * (Math.PI / 180.0);
            double dLat2InRad = Lat2 * (Math.PI / 180.0);
            double dLong2InRad = Long2 * (Math.PI / 180.0);

            double dlongtitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                       Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                       Math.Pow(Math.Sin(dlongtitude / 2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // Distance.
            // const Double kEarthRadiusMiles = 3956.0;
            const Double kEarthRadiusKms = 6376.5;
            dDistance = kEarthRadiusKms * c;

            return dDistance;
        }

       
    }
}