using OOAdvantech.MetaDataRepository;

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
    }
}