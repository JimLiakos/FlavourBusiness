using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{bbb5694e-4424-4705-8edf-19067834682e}</MetaDataID>
    [BackwardCompatibilityID("{bbb5694e-4424-4705-8edf-19067834682e}")]
    public interface IPreparationStation
    {
        /// <MetaDataID>{71851174-e519-466b-a801-8fac2fa6501e}</MetaDataID>
        void RemovePreparationInfo(IItemsPreparationInfo itemsInfoObjectUri);

        //"6046c42e-6e1c-4fbc-86ee-8e1ef74628c0"
        [RoleBMultiplicityRange(1, 1)]
        [Association("PreparationStationItemsInfo", Roles.RoleA, "ff137010-8488-45be-b0e8-a65b28956ec0")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        IList<IItemsPreparationInfo> ItemsPreparationInfos { get; }

        /// <MetaDataID>{bb9df3c1-7e4a-418f-a3f4-6d9c07bf3427}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        string Description { get; set; }

        /// <MetaDataID>{0d8307bb-48a8-4fb3-a327-c09d64ee426d}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{7b55d81f-cd95-4b08-abef-85997e459a58}</MetaDataID>
        string PreparationStationIdentity { get;  }


        ///// <MetaDataID>{e7c40cd7-9b03-4db2-9404-69ed0dee8e88}</MetaDataID>
        //IItemsPreparationInfo NewPreparationInfo(string itemsInfoObjectUri);

        /// <MetaDataID>{0798d4b0-01fc-4cc2-abc5-e63de3dec97d}</MetaDataID>
        IItemsPreparationInfo NewPreparationInfo(string itemsInfoObjectUri, ItemsPreparationInfoType itemsPreparationInfoType);
        /// <MetaDataID>{fbea817c-1e40-45d0-a939-deae8a96af4b}</MetaDataID>
        void RemovePreparationInfos(List<IItemsPreparationInfo> itemsPreparationInfos);


#if !FlavourBusinessDevice
        /// <MetaDataID>{7b6f9f61-6a17-4fd3-b90e-5a89e46238d1}</MetaDataID>
        bool CanPrepareItem(MenuModel.IMenuItem menuItem);
#endif

    }
}