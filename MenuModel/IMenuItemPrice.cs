using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{454e5c30-a878-4863-af44-39cd5df101cb}</MetaDataID>
    public interface IMenuItemPrice : IPricingContext, IPricedSubject
    {
        [Association("ItemPrice", Roles.RoleB, "0964beb4-bfb2-475c-a1d7-5ba113b512ce")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuItem MenuItem { get; }


        /// <MetaDataID>{f16b3dd6-cf60-44c3-8e93-847e63d233b1}</MetaDataID>
        bool IsDefaultPrice { get; }
    }
}