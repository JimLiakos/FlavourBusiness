namespace FinanceFacade
{
    /// <MetaDataID>{e18940d1-a92b-4eeb-af0b-49ec3dac20df}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{e18940d1-a92b-4eeb-af0b-49ec3dac20df}")]
    public interface IAddress
    {
        /// <MetaDataID>{69e5fe2d-0b47-4c5e-9382-40a405c2aa04}</MetaDataID>
        string LocationName { get; set; }
        /// <MetaDataID>{4280a26a-db01-4208-a396-f7fdfbe4f373}</MetaDataID>
        string StreetName { get; set; }
        /// <MetaDataID>{c83d6f4a-d632-46fb-a772-5e57db53f7c4}</MetaDataID>
        string StreetNumber { get; set; }

        /// <MetaDataID>{1551bb1a-606d-4941-adee-1150e4c599d4}</MetaDataID>
        string Locality { get; set; }
        /// <MetaDataID>{77cb7672-3f4e-4bd7-a48f-ad65e446adc9}</MetaDataID>
        string City { get; set; }
        /// <MetaDataID>{ea1f07ab-0388-4947-867d-2e826343cadc}</MetaDataID>
        string Sub_Area { get; set; }
        /// <MetaDataID>{d0a6f9d1-a354-4ee2-b2a3-b6685abff194}</MetaDataID>
        string Area { get; set; }

        /// <MetaDataID>{285a3b02-fa4e-4bb3-ac93-3d7460717e5f}</MetaDataID>
        string PostalCode { get; set; }

        /// <MetaDataID>{521eec1c-8435-432d-a73c-ec332b467e48}</MetaDataID>
        string Country { get; set; }

        /// <MetaDataID>{05eb9fbf-90df-47e4-9c96-e8686df30359}</MetaDataID>
        string CountryCode { get; set; }

        /// <MetaDataID>{2fca1397-227d-4bf4-989e-5b9d2068c085}</MetaDataID>
        double Latitude { get; set; }

        /// <MetaDataID>{0fa91255-8509-4165-b710-2fb104c45525}</MetaDataID>
        double Longitude { get; set; }



    }
}