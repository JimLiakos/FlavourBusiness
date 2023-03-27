using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{195083e4-bb86-4ad2-8ec2-b388389d3cde}</MetaDataID>
    [GenerateFacadeProxy]
    [HttpVisible]
    public interface IServingShiftWork : IShiftWork
    {
        [RoleAMultiplicityRange(0)]
        [Association("ServingBatchInShiftWork", Roles.RoleA, "5b49aba4-a3de-46da-9a52-6436a3823d6f")]
        System.Collections.Generic.List<RoomService.IServingBatch> ServingBatches { get; }

        void RecalculateDeptData();
    }
}