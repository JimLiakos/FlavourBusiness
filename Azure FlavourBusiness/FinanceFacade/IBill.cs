using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.RoomService
{
    /// <MetaDataID>{46e2c831-0295-420b-b4b9-f6f469b0c6e1}</MetaDataID>
    [HttpVisible]
    [GenerateFacadeProxy]
    public interface IBill
    {
        [Association("BillPayments", Roles.RoleA, "34270487-a2a1-40fe-a098-e8ddc37cfca6")]
        [RoleAMultiplicityRange(1)]
        System.Collections.Generic.List<FinanceFacade.IPayment> Payments { get; }
    }
}