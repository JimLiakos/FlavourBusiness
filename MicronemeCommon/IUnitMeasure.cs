namespace Measurement
{
    using OOAdvantech.MetaDataRepository;
    using OOAdvantech.MetaDataRepository;
    /// <MetaDataID>{AA121092-B061-4E47-BDB7-60D4D501FF29}</MetaDataID>
    public interface IUnitMeasure
    {


        /// <MetaDataID>{B8A2C0F3-AB83-4764-A1E7-3BB3E40B6A65}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name
        {
            get;
            set;
        }
        /// <MetaDataID>{fb789404-fbfa-4e99-9618-02661fc39361}</MetaDataID>
        Quantity Convert(Quantity value);
        /// <MetaDataID>{1da9c8f8-66b6-45b1-92d1-efe8b337a8a7}</MetaDataID>
        bool CanConvert(Quantity value);
    }
}
