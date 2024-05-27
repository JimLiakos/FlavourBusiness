using System.Collections.Generic;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{bbb5694e-4424-4705-8edf-19067834682e}</MetaDataID>
    [BackwardCompatibilityID("{bbb5694e-4424-4705-8edf-19067834682e}")]
    [GenerateFacadeProxy]

    public interface IPreparationStation
    {
        [Association("PreparationSubStation", Roles.RoleB, "cd533f23-6824-4431-b904-818421f4eb42")]
        [RoleBMultiplicityRange(1, 1)]
        IPreparationStation MainStation { get; }

        /// <MetaDataID>{a361dd77-8fe0-48ee-abb0-01e87675322e}</MetaDataID>
        string ShortIdentity { get; set; }

        /// <summary>
        /// Defines the time interval between the first item forecast preparation time and the last item of group
        /// Food preparer can filter the product where displayed on screen to decide if worth to prepare all in session.
        /// If the food preparer uses the tags to group products, 
        /// the time interval between the preparation time of the first prediction and the last product must not exceed the GroupingTimeSpan.
        /// Products with a predicted preparation time that is outside the GroupingTimeSpan are not displayed.
        /// </summary>
        /// <MetaDataID>{f5ae4b4f-ef6a-493e-9283-4f3ced15af67}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double GroupingTimeSpan { get; set; }



        [Association("PreparationSubStation", Roles.RoleA, "cd533f23-6824-4431-b904-818421f4eb42")]
        [RoleBMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        [HttpVisible]
        List<IPreparationStation> SubStations { get; }

        /// <MetaDataID>{a4bb2a23-cd26-445c-b582-b277e6100c83}</MetaDataID>
        IPreparationStation NewSubStation();

        /// <MetaDataID>{667002c2-bdaa-402d-bcc6-d869c4ad0704}</MetaDataID>
        void RemoveSubStation(IPreparationStation preparationStation);


        [RoleBMultiplicityRange(1, 1)]
        [RoleAMultiplicityRange(0)]
        [Association("PreparationStationPrepareFor", Roles.RoleA, "d953fcd5-84d0-429b-a436-09c0bfa66a5a")]
        List<IPreparationForInfo> PreparationForInfos { get; }

        /// <MetaDataID>{71851174-e519-466b-a801-8fac2fa6501e}</MetaDataID>
        void RemovePreparationInfo(IItemsPreparationInfo itemsInfoObjectUri);

        //"6046c42e-6e1c-4fbc-86ee-8e1ef74628c0"
        [RoleBMultiplicityRange(1, 1)]
        [Association("PreparationStationItemsInfo", Roles.RoleA, "ff137010-8488-45be-b0e8-a65b28956ec0")]

        IList<IItemsPreparationInfo> ItemsPreparationInfos { get; }

        /// <MetaDataID>{bb9df3c1-7e4a-418f-a3f4-6d9c07bf3427}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        [HttpVisible]
        string Description { get; set; }

        /// <MetaDataID>{0d8307bb-48a8-4fb3-a327-c09d64ee426d}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{7b55d81f-cd95-4b08-abef-85997e459a58}</MetaDataID>
        string PreparationStationIdentity { get; }


        /// <MetaDataID>{55a15f9b-ae42-4cfe-97b4-1e95dd50a278}</MetaDataID>
        DeviceAppLifecycle DeviceAppState { get; }



        ///// <MetaDataID>{e7c40cd7-9b03-4db2-9404-69ed0dee8e88}</MetaDataID>
        //IItemsPreparationInfo NewPreparationInfo(string itemsInfoObjectUri);

        /// <MetaDataID>{0798d4b0-01fc-4cc2-abc5-e63de3dec97d}</MetaDataID>
        IItemsPreparationInfo NewPreparationInfo(string itemsInfoObjectUri, ItemsPreparationInfoType itemsPreparationInfoType);
        /// <MetaDataID>{e20e36c9-34e7-4d91-8e5e-85399c0566ed}</MetaDataID>
        void RemovePreparationForInfo(IPreparationForInfo PreparationForInfo);

        /// <MetaDataID>{173ecea0-545e-44f8-9623-bb108c3e46aa}</MetaDataID>
        void RemovePreparationForInfos(List<IPreparationForInfo> PreparationForInfos);

        /// <MetaDataID>{fbea817c-1e40-45d0-a939-deae8a96af4b}</MetaDataID>
        void RemovePreparationInfos(List<IItemsPreparationInfo> itemsPreparationInfos);

        /// <MetaDataID>{883ddfc5-8927-47fd-b38a-1c9e38e5b6d2}</MetaDataID>
        IPreparationForInfo NewServiceAreaPreparationForInfo(IServiceArea serviceArea, PreparationForInfoType PreparationForInfoType);


        /// <MetaDataID>{7c1c1517-a551-4e9e-8a6f-0320fd0692ad}</MetaDataID>
        IPreparationForInfo NewServicePointPreparationForInfo(IServicePoint servicePoint, PreparationForInfoType PreparationForInfoType);



#if !FlavourBusinessDevice
        /// <MetaDataID>{7b6f9f61-6a17-4fd3-b90e-5a89e46238d1}</MetaDataID>
        bool CanPrepareItem(MenuModel.IMenuItem menuItem);

        /// <MetaDataID>{6e3c9ec8-54b3-4b59-8c04-dd5095811769}</MetaDataID>
        bool CanPrepareItemFor(MenuModel.IMenuItem menuItem, IServicePoint servicePoint);

#endif



    }
}




namespace FlavourBusinessFacade
{
    /// <MetaDataID>{b870fcd0-671e-4ef8-abd5-d8bb26cef2fd}</MetaDataID>
    public enum DeviceAppLifecycle
    {
        InUse,
        Sleep,
        Shutdown
    }
}