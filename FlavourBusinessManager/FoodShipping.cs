using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using System;
namespace FlavourBusinessManager.Shipping
{
    /// <MetaDataID>{c36022d2-142c-4f52-8ca3-debd1124b925}</MetaDataID>
    [BackwardCompatibilityID("{c36022d2-142c-4f52-8ca3-debd1124b925}")]
    [Persistent()]
    public class FoodShipping : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.Shipping.IFoodShipping
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _PreparedItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{f439725a-bc72-4521-8ca4-44418cfeaad1}</MetaDataID>
        [PersistentMember(nameof(_PreparedItems))]
        [BackwardCompatibilityID("+3")]
        public List<IItemPreparation> PreparedItems => _PreparedItems.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<FlavourBusinessFacade.HumanResources.IServingShiftWork> _ShiftWork = new Member<FlavourBusinessFacade.HumanResources.IServingShiftWork>();


        /// <MetaDataID>{9f5189cc-ab0f-41db-a31c-9e81926d4412}</MetaDataID>
        [PersistentMember(nameof(_ShiftWork))]
        [BackwardCompatibilityID("+4")]
        public FlavourBusinessFacade.HumanResources.IServingShiftWork ShiftWork => _ShiftWork.Value;

        /// <MetaDataID>{1494396b-fb28-4280-83ad-ee1c39c18f1b}</MetaDataID>
        public FlavourBusinessFacade.ServicesContextResources.ServicePointType ServicePointType { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        /// <exclude>Excluded</exclude>
        IMealCourse _MealCourse;

        /// <MetaDataID>{ebff5c16-abe5-4d72-bcd2-dcca20ca7fa7}</MetaDataID>
        [PersistentMember(nameof(_MealCourse))]
        [BackwardCompatibilityID("+1")]
        public FlavourBusinessFacade.RoomService.IMealCourse MealCourse => _MealCourse;

        /// <MetaDataID>{d8834cae-2fde-4861-81b0-6be2a5c26f12}</MetaDataID>
        public bool IsAssigned => throw new System.NotImplementedException();

        /// <MetaDataID>{a6d410ca-433e-4bc2-a60d-2902bbf42126}</MetaDataID>
        public int SortID => throw new System.NotImplementedException();

        /// <MetaDataID>{f1770ae5-6716-476d-80cb-1542934f27e6}</MetaDataID>
        public string MealCourseUri => throw new System.NotImplementedException();

        /// <MetaDataID>{c31ace5f-8e90-4682-a38c-e374a519580d}</MetaDataID>
        public FlavourBusinessFacade.ServicesContextResources.IServicePoint ServicePoint => throw new System.NotImplementedException();

    
        /// <MetaDataID>{81c8d3f1-bdf1-422b-9c79-baf0371f329a}</MetaDataID>
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems => throw new System.NotImplementedException();

        /// <MetaDataID>{510a7954-2c31-4a10-a898-e2e1a004818d}</MetaDataID>
        public IList<ItemsPreparationContext> ContextsOfPreparedItems => throw new System.NotImplementedException();

        /// <MetaDataID>{c83578bb-5b53-459b-949d-362b78db48fd}</MetaDataID>
        public string Description => throw new System.NotImplementedException();

        /// <MetaDataID>{effa589d-8157-44af-951c-4ca5c212dff6}</MetaDataID>
        public string ServicesPointIdentity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public event ObjectChangeStateHandle ObjectChangeState;
        public event ItemsStateChangedHandle ItemsStateChanged;

        /// <MetaDataID>{cee036f2-fe3c-4668-ac0b-b3250d86ea33}</MetaDataID>
        public void PrintReceiptAgain()
        {
            throw new System.NotImplementedException();
        }
    }
}