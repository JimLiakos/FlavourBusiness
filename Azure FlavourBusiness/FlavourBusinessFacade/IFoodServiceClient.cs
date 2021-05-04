
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.EndUsers
{
    /// <MetaDataID>{8ebba0ed-26f3-45ae-98da-4e890296e72b}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{8ebba0ed-26f3-45ae-98da-4e890296e72b}")]
    public interface IFoodServiceClient
    {
        /// <MetaDataID>{494166a4-2018-4e76-b9fc-d54697a29c5d}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [CachingDataOnClientSide]
        SIMCardData SIMCardData { get; set; }

        /// <MetaDataID>{672e0963-b347-4315-9528-893063e6ad99}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string Email { get; set; }

        /// <MetaDataID>{8fdcc10d-71b6-4909-93a3-b940f53856c9}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string Address { get; set; }

        /// <MetaDataID>{bcbe7058-ff31-4643-80cc-a2bd0b917832}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string PhoneNumber { get; set; }

        /// <MetaDataID>{cff6a3b8-d3b3-49ba-8882-38a316db8923}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string Name { get; set; }

        /// <MetaDataID>{44faf246-0cd0-48d7-a728-76e716c1eb10}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Identity { get; }


    }

    /// <MetaDataID>{d2445458-7d5d-4cc1-ae85-45143f5d2944}</MetaDataID>
    [BackwardCompatibilityID("{d2445458-7d5d-4cc1-ae85-45143f5d2944}")]
    [Persistent()]
    public struct SIMCardData
    {
        /// <MetaDataID>{f948c57e-dcec-498d-a329-1955cecf047a}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        public string SIMCardIdentity;
        /// <MetaDataID>{63f275ba-64f5-468b-afcd-df3039c6390f}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+2")]
        public string SIMCardPhoneNumber;
        /// <MetaDataID>{89a814c5-8f76-4560-aff3-d9e29ba840d2}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+3")]
        public string SIMCardDescription;


        public static bool operator ==(SIMCardData left, SIMCardData right)
        {
            if (left.SIMCardIdentity == right.SIMCardIdentity)
                return true;
            else
                return false;
        }

        public static bool operator !=(SIMCardData left, SIMCardData right)
        {
            return !(left == right);
        }
    }
}