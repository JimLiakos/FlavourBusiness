using OOAdvantech.Transactions;

namespace FlavourBusinessFacade.EndUsers
{
    /// <MetaDataID>{4b012629-4bd8-4338-8f0e-94ea9a201831}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{4b012629-4bd8-4338-8f0e-94ea9a201831}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public struct Notification
    {

        /// <MetaDataID>{7842f220-2ddb-447c-ac94-3bb7f987d1f0}</MetaDataID>
        public static bool operator ==(Notification left, Notification right)
        {
            return left._Body == right._Body && left._Title == right._Title;
        }

        /// <MetaDataID>{5cd33065-b21e-44a7-8397-510bc9050327}</MetaDataID>
        public static bool operator !=(Notification left, Notification right)
        {
            return !(left == right);
        }

        /// <exclude>Excluded</exclude>
        string _Body;
        /// <MetaDataID>{3c6531d2-6da7-4e5b-be0f-e26d18f5b772}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Body))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+4")]
        public string Body
        {
            get => _Body;
            set
            {
                _Body = value;
            }
        }



        /// <exclude>Excluded</exclude>
        string _Title;
        /// <MetaDataID>{d7e8b16b-4d65-44f2-9b0a-80f148d0fad0}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Title))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
            }
        }






    }
}