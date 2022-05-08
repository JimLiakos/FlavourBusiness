using System;
using System.Collections.Generic;
using System.Text;
//using FlavourBusinessManager.RoomService;

namespace PreparationStationDevice
{
    /// <MetaDataID>{ac8782c6-2ea6-4ce3-a7be-8efc104736f1}</MetaDataID>
    public class ItemsPreparationContextPresentation
    {
        public string ServicesPointIdentity { get; set; }
        public string ServicesContextIdentity { get; set; }
        public string Description { get; set; }

        public List<PreparationStationItem> PreparationItems { get; set; }
        public string Uri { get;  set; }
        public DateTime? MustBeServedAt { get; internal set; }
        public DateTime? StartsAt { get; internal set; }
    }
}
