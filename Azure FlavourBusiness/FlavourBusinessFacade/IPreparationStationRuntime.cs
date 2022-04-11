using System;
using System.Collections.Generic;
using FlavourBusinessFacade.RoomService;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{0cf4bde3-81c2-4685-b35d-ea231ae11540}</MetaDataID>
    [GenerateFacadeProxy]
    public interface IPreparationStationRuntime
    {
        /// <summary>
        /// This method change state of itemPreparations to Serving
        /// </summary>
        /// <param name="itemPreparationUris">
        /// Defines the items Uris which will be change state
        /// </param>
        /// <returns>
        /// returns a dictionary with items Uris under the preparation station control and the predicted time span in minutes 
        /// where the items will be ready to serve
        /// </returns>
        /// <MetaDataID>{ac6eb47d-a891-4d66-a8e1-50f5310d4e36}</MetaDataID>
        Dictionary<string, double> ItemsServing(List<string> itemPreparationUris);

        event ObjectChangeStateHandle ObjectChangeState;

        Dictionary<string, List<MenuModel.ITag>> ItemsPreparationTags { get; }

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
        /// <summary>
        /// This method change state of itemPreparations to …nPreparation
        /// </summary>
        /// <param name="itemPreparationUris">
        /// Defines the items Uris which will be change state
        /// </param>
        /// <returns>
        /// returns a dictionary with items Uris under the preparation station control and the predicted time span in minutes 
        /// where the items will be ready to serve
        /// </returns>
        // <MetaDataID>{559e7066-f32e-4ac7-b308-9208d59c9e39}</MetaDataID>
        Dictionary<string,double> Items…nPreparation(List<string> itemPreparationUris);


        /// <summary>
        /// This method change state of itemPreparations to IsRoasting
        /// </summary>
        /// <param name="itemPreparationUris">
        /// Defines the items Uris which will be change state
        /// </param>
        /// <returns>
        /// returns a dictionary with items Uris under the preparation station control and the predicted time span in minutes 
        /// where the items will be ready to serve
        /// </returns>
        /// <MetaDataID>{358c2943-5794-46d6-aacb-402ce2409085}</MetaDataID>
        Dictionary<string, double>  ItemsRoasting(List<string> itemPreparationUris);


        /// <summary>
        /// This method change state of itemPreparations to IsPrepared
        /// </summary>
        /// <param name="itemPreparationUris">
        /// Defines the items Uris which will be change state
        /// </param>
        /// <returns>
        /// returns a dictionary with items Uris under the preparation station control and the predicted time span in minutes 
        /// where the items will be ready to serve
        /// </returns>
        /// <MetaDataID>{ec0f68a2-87b3-4e9c-8c45-14915ca53830}</MetaDataID>
        Dictionary<string, double>  ItemsPrepared(List<string> itemPreparationUris);


        /// <summary>
        /// This method cancels the last state change of itemPreparations 
        /// </summary>
        /// <param name="itemPreparationUris">
        /// Defines the items Uris which will be cancel change state
        /// </param>
        /// <returns>
        /// returns a dictionary with items Uris under the preparation station control and the predicted time span in minutes 
        /// where the items will be ready to serve
        /// </returns>
        /// <MetaDataID>{99ef195b-b347-42d7-b70d-adbcfdb0c54e}</MetaDataID>
        Dictionary<string, double> CancelLastPreparationStep(List<string> itemPreparationUris);
        /// <MetaDataID>{2d9ccb0f-821d-4469-8f74-9087c2be669b}</MetaDataID>
        void AssignCodeCardsToSessions(List<string> codeCards);
    }

    public delegate void PreparationItemsChangeStateHandled(IPreparationStationRuntime sender, string deviceUpdateEtag);


}