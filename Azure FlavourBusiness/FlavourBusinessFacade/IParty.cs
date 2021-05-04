using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{e89c2155-17ba-471f-8252-99814326598f}</MetaDataID>
    [BackwardCompatibilityID("{e89c2155-17ba-471f-8252-99814326598f}")]
    [HttpVisible]
    [GenerateFacadeProxy]
    public interface IParty
    {

        [Association("ResponsibleRole", Roles.RoleB, "b5b25319-7011-4daf-b68b-69cc2a5c3b22")]
        [RoleBMultiplicityRange(0)]
        System.Collections.Generic.List<FlavourBusinessFacade.HumanResources.IAccountability> Responsibilities { get; }


        /// <MetaDataID>{384bcaa2-db9f-433c-8333-4b0032366dfb}</MetaDataID>

        
        [Association("CommissionerRole", Roles.RoleA, "67c84e44-6d7d-41c0-bcf0-f7f685f0a720")]
        [RoleAMultiplicityRange(0)]
        System.Collections.Generic.List<FlavourBusinessFacade.HumanResources.IAccountability> Commissions { get; }

        /// <MetaDataID>{20c2b981-2b68-4cd2-b9a3-3d70f86e0277}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        string Name { get; set; }


    
    }
}