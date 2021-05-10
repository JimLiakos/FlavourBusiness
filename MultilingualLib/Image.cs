using OOAdvantech.MetaDataRepository;

namespace Multilingual
{
    /// <MetaDataID>{4c41cf47-4bb8-422d-b557-e07ec9a3c368}</MetaDataID>
    [BackwardCompatibilityID("{4c41cf47-4bb8-422d-b557-e07ec9a3c368}")]
    [Persistent()]
    public class Image
    {

        public Image()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{75e66706-9398-43c9-b597-fc34b1dabfd8}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+4")]
        public string Uri;

        /// <MetaDataID>{ef8cf1cd-79e2-48fe-94ed-4bceeb4db656}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+3")]
        public double Width;

        /// <MetaDataID>{95db5c76-7443-4873-a6d1-ec481f58a31c}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        public double Height;
    }
}