using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{85284c72-9e39-4516-bdd9-8bef6da1449e}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{85284c72-9e39-4516-bdd9-8bef6da1449e}")]
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IResourceManager
    {
        /// <MetaDataID>{1b1336c6-bbd8-4d91-b989-3721692c5228}</MetaDataID>
        List<OrganizationStorageRef> GraphicMenus { get; }


        /// <MetaDataID>{bd6625bb-9739-4325-b0b4-9a4edc905059}</MetaDataID>
        OrganizationStorageRef GetStorage(OrganizationStorages dataType);
        /// <MetaDataID>{66117a02-2ac3-4cf6-9f4e-820c055fc936}</MetaDataID>
        OrganizationStorageRef UpdateStorage(string name, string description, string storageIdentity);

        /// <MetaDataID>{430052e4-5142-4421-a8a8-2d2438d3d4f5}</MetaDataID>
        OrganizationStorageRef NewGraphicMenu(string culture);

        /// <MetaDataID>{934d8558-c390-4daf-9f54-3bdb664f854c}</MetaDataID>
        void RemoveGraphicMenu(string storageIdentity);

        /// <MetaDataID>{1ad71aa0-06ac-4e8e-93e9-af361bf18e5a}</MetaDataID>
        void PublishMenu(OrganizationStorageRef storageRef);



    }




    /// <MetaDataID>{58496731-c644-4a38-9d22-cf02004cf33a}</MetaDataID>
    public enum OrganizationStorages
    {
        RestaurantMenus,
        BackgroundImages,
        Borders,
        StyleSheets,
        GraphicMenu,
        HeadingAccents,
        HallLayout,
        OperativeRestaurantMenu

    }

    /// <MetaDataID>{0048265e-8276-4df1-be27-1f809759da02}</MetaDataID>
    public class OrganizationStorageRef
    {
        /// <MetaDataID>{618e44c9-ade8-461f-a9d8-e865af5bee06}</MetaDataID>
        public string Description { get; set; }

        /// <MetaDataID>{0c7efc3a-7fa7-43aa-80b3-0e71bf34a332}</MetaDataID>
        public string StorageIdentity { get; set; }

        /// <MetaDataID>{df5946ae-3f68-4dac-92db-3f568a9a34d0}</MetaDataID>
        public DateTime TimeStamp { get;  set; }

        /// <MetaDataID>{36f5651c-f9b0-4489-a536-be20a8991c22}</MetaDataID>
        public string StorageUrl { get; set; }

        /// <MetaDataID>{9afd7e1c-7541-488f-ba5c-aefa80b3f9a4}</MetaDataID>
        public string Name { get; set; }
        /// <MetaDataID>{d3208c58-156e-436d-ad70-6a90c3470674}</MetaDataID>
        public string Version { get; set; }

        /// <MetaDataID>{42bbfdf4-67a3-4d98-8dcd-935330539a41}</MetaDataID>
        public OrganizationStorages FlavourStorageType { get; set; }

        /// <MetaDataID>{f76e1d7f-4129-4e42-beb5-d411e3fb8326}</MetaDataID>
        public FlavourBusinessFacade.IUploadService UploadService { get; set; }
        public Dictionary<string, string> PropertiesValues { get; set; }

        /// <MetaDataID>{8c56a270-32bc-4192-917d-4c7340696988}</MetaDataID>
        public OrganizationStorageRef Clone()
        {
            return new OrganizationStorageRef() { Description = this.Description, Name = this.Name, StorageIdentity = this.StorageIdentity, StorageUrl = this.StorageUrl, TimeStamp = this.TimeStamp, UploadService= this.UploadService, PropertiesValues=this.PropertiesValues };
        }
    }
}
