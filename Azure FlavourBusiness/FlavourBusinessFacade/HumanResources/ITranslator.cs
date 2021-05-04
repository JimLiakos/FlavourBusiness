using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{7de03e59-3e8d-486c-b64a-17b1a7907e7c}</MetaDataID>
    [BackwardCompatibilityID("{7de03e59-3e8d-486c-b64a-17b1a7907e7c}")]
    public interface ITranslator : IParty
    {

        /// <MetaDataID>{dc3c2da7-7833-4da3-a77a-c2bcbb806524}</MetaDataID>
        IActivity NewTranslationActivity(string subjectDescription, TranslationType selectedTranslationType, string subjcectIdentity);
        /// <MetaDataID>{86ccb0c4-0006-425b-a062-8245ca158b97}</MetaDataID>
        void RemoveTranslationActivity(ITranslationActivity activity);
    }
}