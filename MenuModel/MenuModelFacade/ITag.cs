using OOAdvantech;

namespace MenuModel
{
    /// <MetaDataID>{ea0831c0-a33c-4ba2-aa25-8df17c1df45a}</MetaDataID>
    public interface ITag
    {
        /// <MetaDataID>{914b7387-d917-4f9c-9c5f-3ab22f641fea}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string Name { get; set; }

        /// <MetaDataID>{5d4f737b-05d3-4648-8460-4b07130196b9}</MetaDataID>
        Multilingual MultilingualName { get; }
    }
}