using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{9e1b8a90-b4ec-4ecc-aee3-9459af3645dc}</MetaDataID>
    [BackwardCompatibilityID("{9e1b8a90-b4ec-4ecc-aee3-9459af3645dc}")]
    [Persistent()]
    public struct Resource
    {
        /// <MetaDataID>{adc2913e-f634-4f1f-9690-53929916c723}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+3")]
        public System.DateTime TimeStamp;


        /// <MetaDataID>{38640846-da24-45e4-ab27-b961cd08481b}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+2")]
        public string Uri;

        /// <MetaDataID>{deb19f4a-ffaf-4b47-8d2f-f95e3cf2bde8}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+1")]
        public string Name;


        /// <MetaDataID>{d3792d82-054c-4ee5-893b-0a73f268046b}</MetaDataID>
        public static bool operator ==(Resource left, Resource right)
        {
            if (left.Name == right.Name &&
                left.Uri == right.Uri&&left.TimeStamp==right.TimeStamp)
                return true;
            else
                return false;
        }

        /// <MetaDataID>{2eebe3f8-0629-4c3b-a069-a55880781a4e}</MetaDataID>
        public static bool operator !=(Resource left, Resource right)
        {
            return !(left == right);
        }
    }




}