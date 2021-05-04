using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{73004dbf-f472-4786-a6b3-3a456419d382}</MetaDataID>
    [BackwardCompatibilityID("{73004dbf-f472-4786-a6b3-3a456419d382}")]
    public interface ICallerIDServer
    {
        /// <MetaDataID>{a596fe5b-406e-4af2-8e43-0a5035a1dae6}</MetaDataID>
        void AddCallerIDLine(ICallerIDLine callerIDLine);

        /// <MetaDataID>{c895dfca-7eff-464f-9887-6f54c2e476f7}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{c199ae7a-0b4d-4158-a516-870ce19a7783}</MetaDataID>
        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.CascadeDelete)]
        [RoleAMultiplicityRange(0)]
        [Association("CallerIDLines", Roles.RoleA, "dcdfe3a5-f556-4845-a07c-c0d27ceed5d8")]
        [RoleBMultiplicityRange(1, 1)]

        IList<ICallerIDLine> Lines { get; }


       

        /// <MetaDataID>{040e1917-7732-4233-840f-132e00411180}</MetaDataID>
        void RemoveCallerIDLine(ICallerIDLine callerIDLine);

        /// <MetaDataID>{21e3439e-5bd7-446b-bda8-77971b291210}</MetaDataID>
        ICallerIDLine NewCallerIDLine();

        event OOAdvantech.ObjectChangeStateHandle ObjectStateChanged;


    }
}