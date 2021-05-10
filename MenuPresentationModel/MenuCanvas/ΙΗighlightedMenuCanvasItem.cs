using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{b1762781-0c2d-437e-9db9-98e351a167c9}</MetaDataID>
    public interface IHighlightedMenuCanvasItem : IMenuCanvasItem
    {

        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [Association("MenuCanvasItemAccent", Roles.RoleA, "2a070e47-80ea-4cf6-8213-f19b69db3526")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1)]
        IMenuCanvasAccent MenuCanvasAccent { get; set; }
    }
}