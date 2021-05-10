using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace Multilingual
{
    /// <MetaDataID>{5070b433-fcdb-451c-aaa7-a24697fb8496}</MetaDataID>
    [BackwardCompatibilityID("{5070b433-fcdb-451c-aaa7-a24697fb8496}")]
    [Persistent()]
    public struct ItemResources
    {


        /// <MetaDataID>{7ed67aed-1ea5-4864-af66-8fa8effcbd52}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<Image> _Images;

        [Association("ItemResourcesImages", Roles.RoleA, "f91f040c-204f-42dd-bd33-80095697cf4c")]
        [PersistentMember(nameof(_Images))]
        public IList<Image> Images
        {
            get
            {
                //if (_Images == null)
                //    _Images = new OOAdvantech.Collections.Generic.Set<Image>();

                if (_Images != null)
                    return _Images.AsReadOnly();
                else 
                    return null;
            }

        }




        /// <exclude>Excluded</exclude>
        string _ResourceName;

        /// <MetaDataID>{f4d945a2-28f7-493b-ae2d-d360d4b30128}</MetaDataID>
        [PersistentMember(nameof(_ResourceName))]
        [BackwardCompatibilityID("+1")]
        public string ResourceName
        {
            get => _ResourceName;
            set => _ResourceName = value;

        }

        /// <MetaDataID>{83e4fbb4-8f9d-4f98-acbb-bd1a07e50d6c}</MetaDataID>
        public static bool operator ==(ItemResources left, ItemResources right) => left.ResourceName == right.ResourceName;


        /// <MetaDataID>{4d62459a-8452-46aa-a112-72cf57dadb28}</MetaDataID>
        public static bool operator !=(ItemResources left, ItemResources right) => !(left == right);

        public void AddImage(Image image)
        {
            if (_Images == null)
                _Images = new OOAdvantech.Collections.Generic.Set<Image>();

            _Images.Add(image);
        }
    }
}