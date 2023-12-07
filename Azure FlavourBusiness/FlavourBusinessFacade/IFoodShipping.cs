using FlavourBusinessFacade.EndUsers;
using OOAdvantech.Json;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System.Globalization;
using System.Collections.Generic;

namespace FlavourBusinessFacade.Shipping
{
    /// <MetaDataID>{fb36f7f0-3762-4db7-9e05-40e87ba43631}</MetaDataID>
    [GenerateFacadeProxy]
    public interface IFoodShipping : RoomService.IServingBatch
    {
        /// <MetaDataID>{e7e83f87-aae7-4e11-90d8-d8a153f8befe}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        System.DateTime? DeliveryTime { get; }

        /// <MetaDataID>{c70d2e13-c4fe-47b4-9ae9-d0592a064613}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        string DistributionIdentity { get; }

        /// <MetaDataID>{009256bd-a668-4532-a0b4-88c5f47ad8eb}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        string ReturnReason { get; set; }

        /// <MetaDataID>{b0e03048-d4c8-447f-a64e-c097498d2514}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        string ReturnReasonID { get; set; } 

        /// <MetaDataID>{1b75fcb5-7f27-419b-9fc8-17261cfac7c4}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Identity { get; }

        /// <MetaDataID>{e43268d5-265d-47da-9084-1bd95f4bb369}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        IPlace Place { get; }
        /// <MetaDataID>{b67cdb6e-ae8b-454e-bfaf-5093d0643bd7}</MetaDataID>
        string ClientFullName { get; }
        /// <MetaDataID>{f8fcc4e5-e6a2-4d60-8516-f89bb658af23}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string PhoneNumber { get; }
        /// <MetaDataID>{7c1f54e2-57a7-42a7-a76c-ec6d61e0d0e2}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string DeliveryRemark { get; }
        /// <MetaDataID>{1df58717-0e1e-4e3f-9965-7f61403a0cd8}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string NotesForClient { get; }

     
    }


    /// <MetaDataID>{b266f1ab-3f43-4dde-8050-866359c6ed1a}</MetaDataID>
    public class ReturnReason
    {

        /// <MetaDataID>{2ed01326-c4e5-4ff7-997b-cf6a01b715cd}</MetaDataID>
        public ReturnReason(string identity, Dictionary<string, string> multilingualDescription)
        {
            Identity = identity;
            foreach (var entry in multilingualDescription)
            {
                CultureInfo culture = CultureInfo.GetCultureInfo(entry.Key);
                using (CultureContext context = new OOAdvantech.CultureContext(culture, true))
                {
                    MultilingualDescription.SetValue(entry.Value);
                }
            }
        }

        /// <MetaDataID>{092a3e66-77a7-4fc3-9270-047d97e7f143}</MetaDataID>
        public ReturnReason()
        {

        }
        /// <MetaDataID>{4716cb2d-c56b-4865-b62d-4b187b553f4c}</MetaDataID>
        public ReturnReason(string identity, Multilingual multilingualDescription)
        {
            Identity = identity;
            MultilingualDescription = multilingualDescription;

        }
        /// <MetaDataID>{d65db9e0-a18e-4dc3-8cbd-d7dc8a9fc3e3}</MetaDataID>
        [JsonProperty]
        Multilingual MultilingualDescription = new Multilingual();


        /// <MetaDataID>{d7c93d93-39fa-431f-a92c-06264f838f6d}</MetaDataID>
        [JsonIgnore]
        public string Description { get => MultilingualDescription.GetValue<string>(); set => MultilingualDescription.SetValue<string>(value); }


        //Λάθος προϊόν,Λαθος Παραγγελία,Αργοποριμένη Παραγγελία,Κακής Ποιότητας Προϊον

        /// <MetaDataID>{f51003f1-7757-4cc6-b7bd-62cdc8e01615}</MetaDataID>
        public string Identity { get; set; }
    }

}