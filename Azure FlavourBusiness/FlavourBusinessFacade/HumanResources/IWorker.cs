using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{bf0e7da0-bb6e-4649-9b9c-333cc2a75ce1}</MetaDataID>
    [BackwardCompatibilityID("{bf0e7da0-bb6e-4649-9b9c-333cc2a75ce1}")]
    public interface IWorker
    {
        
        [RoleAMultiplicityRange(0)]
        [Association("ResponsibleRole", Roles.RoleA, "660714da-a4e3-402f-a5e9-9fd1244e6b0d")]
        System.Collections.Generic.List<IAccountability> Responsibilities { get; }

        /// <MetaDataID>{6408fdc1-96c2-4f24-a9c0-3dbedb08980a}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name { get; set; }

    }
}