using FlavourBusinessFacade.RoomService;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{a14a8c99-cafc-4905-a1c3-ee2cdf8dff75}</MetaDataID>
    [BackwardCompatibilityID("{a14a8c99-cafc-4905-a1c3-ee2cdf8dff75}")]
    public interface ICashierStation
    {
        [Association("CashierOwner", Roles.RoleA, "d55e4a11-efb3-4bcb-963a-12c7ad00c2d8")]
        [RoleAMultiplicityRange(1, 1)]
        FinanceFacade.IFisicalParty Issuer {get; set;}

        /// <MetaDataID>{94267531-d123-48dd-82d7-228a0c5c0da2}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string Description { get; set; }

        /// <MetaDataID>{99255338-9722-4eba-8e33-e3c09e062747}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string ServicesContextIdentity { get; set; }



        /// <MetaDataID>{e07828e8-94d1-414a-b538-13b08cca6f88}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        string DeviceCredentialKeyAbbreviation { get; }

        /// <MetaDataID>{95bd802c-3612-4ac6-86be-41d758d1f1ca}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string CashierStationIdentity { get; }


        /// <MetaDataID>{736a5de0-6436-4fe5-b045-96b2fa834727}</MetaDataID>
        void SetPrintReceiptCondition(ServicePointType servicePointType, PrintReceiptCondition itemState);

        /// <MetaDataID>{1d80549c-1abf-42ad-9ebe-03c6decb701d}</MetaDataID>
        PrintReceiptCondition GetPrintReceiptCondition(ServicePointType servicePointType);





    }

    /// <MetaDataID>{3903c825-a5c4-438f-a450-965d7a3aacdf}</MetaDataID>
    public class PrintReceiptCondition
    {
        /// <MetaDataID>{a13d9474-ce49-4cf2-8d2e-8f5cba1406fc}</MetaDataID>
        public ServicePointType ServicePointType { get; set; }

        /// <MetaDataID>{66f21529-9dbe-40ac-902b-adf5c556de3e}</MetaDataID>
        public ItemPreparationState? ItemState { get; set; }

        /// <MetaDataID>{a2f81bb6-721b-4690-af9d-d43ab4304b7b}</MetaDataID>
        public bool? IsPaid { get; set; }
    }
}