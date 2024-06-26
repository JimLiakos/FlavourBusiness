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
        Dictionary<string, ItemPreparationPlan> ItemsServing(List<string> itemPreparationUris);

        [HttpVisible]
        event ObjectChangeStateHandle ObjectChangeState;




        /// <MetaDataID>{db665dba-2fd6-4d32-a941-263cf60a5e18}</MetaDataID>
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
        PreparationStationStatus GetPreparationItems(List<ItemPreparationAbbreviation> itemsOnDevice, string deviceUpdateEtag);

        /// <MetaDataID>{734c631d-41d1-4cbc-bf4c-037cc82dbade}</MetaDataID>
        string RestaurantMenuDataSharedUri { get; }


        [GenerateEventConsumerProxy]
        event PreparationItemsChangeStateHandled PreparationItemsChangeState;
        /// <summary>
        /// This method change state of itemPreparations to ÉnPreparation
        /// </summary>
        /// <param name="itemPreparationUris">
        /// Defines the items Uris which will be change state
        /// </param>
        /// <returns>
        /// returns a dictionary with items Uris under the preparation station control and the predicted time span in minutes 
        /// where the items will be ready to serve
        /// </returns>
        /// <MetaDataID>{d65ab68e-c18d-437d-80c4-f618fb068d7d}</MetaDataID>
        // <MetaDataID>{559e7066-f32e-4ac7-b308-9208d59c9e39}</MetaDataID>
        Dictionary<string, ItemPreparationPlan> ItemsInPreparation(List<string> itemPreparationUris);


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
        Dictionary<string, ItemPreparationPlan> ItemsRoasting(List<string> itemPreparationUris);


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
        Dictionary<string, ItemPreparationPlan> ItemsPrepared(List<string> itemPreparationUris);


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
        Dictionary<string, ItemPreparationPlan> CancelLastPreparationStep(List<string> itemPreparationUris);
        /// <MetaDataID>{2d9ccb0f-821d-4469-8f74-9087c2be669b}</MetaDataID>
        void AssignCodeCardsToSessions(List<string> codeCards);


        /// <MetaDataID>{f917b9bc-9cdb-4d58-958c-29a20f95d173}</MetaDataID>
        double PreparationVelocity { get; }


        /// <MetaDataID>{a423809d-8abf-4ee4-9070-217b72d11707}</MetaDataID>
        void DeviceResume();

        /// <MetaDataID>{6ce3be05-5828-4643-a91d-f45c60d25499}</MetaDataID>
        void DeviceSleep();
        /// <MetaDataID>{50e2e06c-1048-48b3-9b03-1302791e8baf}</MetaDataID>
        [HttpVisible]
        DeviceAppLifecycle DeviceAppState { get; }

        /// <MetaDataID>{0b526c3a-afd2-415a-a8bf-2dc7744603e5}</MetaDataID>
        [HttpVisible]
        int AttachedDevices { get; }


    }

    public delegate void PreparationItemsChangeStateHandled(IPreparationStationRuntime sender, string deviceUpdateEtag);


    /// <MetaDataID>{fbe3b931-7627-4a81-ade7-0c279be64691}</MetaDataID>
    public class PreparationStationStatus
    {
        public IList<ItemsPreparationContext> NewItemsUnderPreparationControl;

        public Dictionary<string, ItemPreparationPlan> ServingTimespanPredictions;

    }







}