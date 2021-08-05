using System;
using System.Collections.Generic;
using FlavourBusinessFacade.RoomService;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{0cf4bde3-81c2-4685-b35d-ea231ae11540}</MetaDataID>
    [GenerateFacadeProxy]
    public interface IPreparationStationRuntime
    {


        /// <MetaDataID>{c819f9e6-21ed-4c0d-9c72-a240fbd8728d}</MetaDataID>
        //  List<MenuModel.IMenuItem> GetNewerRestaurandMenuData(DateTime newerFromDate);

        //[Association("PreparationStationController", Roles.RoleA, "89c04438-c0da-4df4-98d1-fad38f28844d")]
        //[RoleAMultiplicityRange(1, 1)]
        //[RoleBMultiplicityRange(1, 1)]
        //IPreparationStation PreparationStation { get; }

        /// <MetaDataID>{65760984-aeca-473a-8bc5-4ed68ead3080}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; }

        /// <MetaDataID>{806c57e3-5317-4955-9622-97b56e045b98}</MetaDataID>
        IList<ServicePointPreparationItems> GetPreparationItems(List<ItemPreparationAbbreviation> itemsOnDevice, string deviceUpdateEtag);

        /// <MetaDataID>{734c631d-41d1-4cbc-bf4c-037cc82dbade}</MetaDataID>
        string RestaurantMenuDataSharedUri { get; }


        [GenerateEventConsumerProxy]
        event PreparationItemsChangeStateHandled PreparationItemsChangeState;

        /// <MetaDataID>{559e7066-f32e-4ac7-b308-9208d59c9e39}</MetaDataID>
        void Items…nPreparation(List<string> itemPreparationUris);

        /// <MetaDataID>{ec0f68a2-87b3-4e9c-8c45-14915ca53830}</MetaDataID>
        void ItemsPrepared(List<string> itemPreparationUris);


        /// <MetaDataID>{99ef195b-b347-42d7-b70d-adbcfdb0c54e}</MetaDataID>
        void CancelLastPreparationStep(List<string> itemPreparationUris);
        /// <MetaDataID>{2d9ccb0f-821d-4469-8f74-9087c2be669b}</MetaDataID>
        void AssignCodeCardsToSessions(List<string> codeCards);
    }

    public delegate void PreparationItemsChangeStateHandled(IPreparationStationRuntime sender, string deviceUpdateEtag);


}