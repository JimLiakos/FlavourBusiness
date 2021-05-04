using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{52d7b472-b47f-41f7-a620-244b2bfc6519}</MetaDataID>
    public interface IAccountability
    {
        
        [Association("ResponsibleRole", Roles.RoleA, "b5b25319-7011-4daf-b68b-69cc2a5c3b22")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [RoleAMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        IParty Responsible { set; get; }


        /// <MetaDataID>{41de5b38-6dfd-4cd4-ba61-66c98cebc540}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Description { get; set; }

        /// <MetaDataID>{c0206fea-e2e8-4491-bd89-3d3a16b97825}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [Association("ÁssignmentActivity", Roles.RoleA, "6f55ad2f-f9c6-4804-a25c-1567968b17fe")]
        System.Collections.Generic.List<IActivity> Activities { get; }






        /// <MetaDataID>{6e5d7d5b-5709-4767-ad75-97179c0ee750}</MetaDataID>
        [Association("CommissionerRole", Roles.RoleB, "67c84e44-6d7d-41c0-bcf0-f7f685f0a720")]
        [RoleBMultiplicityRange(1, 1)]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [CachingDataOnClientSide]
        IParty Commissioner { get; set; }


        /// <MetaDataID>{0b61518e-7644-4a11-846d-912f28844046}</MetaDataID>
        void AddActivity(IActivity activity);

        /// <MetaDataID>{2be2a460-38d7-492d-97a0-8dbf8a0cf0bb}</MetaDataID>
        void RemoveActivity(IActivity activity);


    }
}