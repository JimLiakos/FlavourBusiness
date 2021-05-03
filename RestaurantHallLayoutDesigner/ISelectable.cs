
namespace FloorLayoutDesigner
{
    // Common interface for items that can be selected
    // on the DesignerCanvas; used by DesignerItem and Connection
    /// <MetaDataID>{b7e2c4c3-478a-415b-a339-f27bb6d57d55}</MetaDataID>
    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}
