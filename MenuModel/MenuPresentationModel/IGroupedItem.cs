using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{7004be85-1c8f-48d2-a5aa-37c57f50bb6a}</MetaDataID>
    public interface IGroupedItem
    {
        /// <MetaDataID>{69eb4798-123a-4673-b10c-f198c89565bb}</MetaDataID>
        [Association("GroupedItem", Roles.RoleB, "320ef910-eea5-48db-9f18-73aa0f181e8f")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasFoodItemsGroup HostingArea { get; }
    }
}