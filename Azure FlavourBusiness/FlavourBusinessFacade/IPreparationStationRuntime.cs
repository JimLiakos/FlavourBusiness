using System;
using System.Collections.Generic;
using FlavourBusinessFacade.RoomService;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{0cf4bde3-81c2-4685-b35d-ea231ae11540}</MetaDataID>
    public interface IPreparationStationRuntime 
    {
        /// <MetaDataID>{c819f9e6-21ed-4c0d-9c72-a240fbd8728d}</MetaDataID>
      //  List<MenuModel.IMenuItem> GetNewerRestaurandMenuData(DateTime newerFromDate);

        [Association("PreparationStationController", Roles.RoleA, "89c04438-c0da-4df4-98d1-fad38f28844d")]
        [RoleAMultiplicityRange(1, 1)]
        [RoleBMultiplicityRange(1, 1)]
        IPreparationStation PreparationStation { get; }

        /// <MetaDataID>{65760984-aeca-473a-8bc5-4ed68ead3080}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; }

        /// <MetaDataID>{806c57e3-5317-4955-9622-97b56e045b98}</MetaDataID>
        IList<ServicePointPreparationItems> GetPreparationItems(List<ItemPreparationAbbreviation> itemsOnDevice);


        event PreparationItemChangeStateHandled PreparationItemChangeState;
    }

    public delegate void PreparationItemChangeStateHandled(IPreparationStationRuntime sender, IItemPreparation itemPreparation, ItemPreparationState itemState);


}