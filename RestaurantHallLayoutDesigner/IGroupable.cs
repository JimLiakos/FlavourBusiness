using System;

namespace FloorLayoutDesigner
{
    /// <MetaDataID>{48c97313-1b96-4a60-be7f-3a33775d7dac}</MetaDataID>
    public interface IGroupable
    {
        Guid ID { get; }
        Guid ParentID { get; set; }
        bool IsGroup { get; set; }
    }
}
