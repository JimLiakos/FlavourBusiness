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
    }
}