using FlavourBusinessFacade.RoomService;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;

namespace FlavourBusinessManager.RoomService
{



    /// <summary>
    ///Defines a service section with time constraints and includes the preparation of the food-coffee items, the serving - takeaway - home delivery.
    /// </summary>
    ///<MetaDataID>{986d1f45-2bef-4302-9d5c-b98141c24555}</MetaDataID>
    [BackwardCompatibilityID("{986d1f45-2bef-4302-9d5c-b98141c24555}")]
    [Persistent()]
    public class MealCourse : System.MarshalByRefObject, IMealCourse
    {
        /// <MetaDataID>{064a3eea-736b-48e1-a66a-ad75a5925297}</MetaDataID>
        public void Fosotpd()
        {

        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _FoodItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{1f941000-ce6f-4ec6-85f6-736ad57cf9a6}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        public System.Collections.Generic.IList<FlavourBusinessFacade.RoomService.IItemPreparation> FoodItems => _FoodItems.ToThreadSafeList();


        /// <exclude>Excluded</exclude>
        DateTime _StartsAt;

        /// <MetaDataID>{963bb0ae-3234-4bf5-b7e4-9a313dcb9531}</MetaDataID>
        [PersistentMember(nameof(_StartsAt))]
        [BackwardCompatibilityID("+2")]
        public System.DateTime StartsAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{2b29e1c8-9c66-4412-96f3-cf276f528e44}</MetaDataID>
        DateTime _PreparedAt;

        /// <MetaDataID>{f5802cfc-5c51-4da8-891f-c48fa3abd3b3}</MetaDataID>
        [PersistentMember(nameof(_PreparedAt))]
        [BackwardCompatibilityID("+3")]
        public System.DateTime PreparedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <MetaDataID>{fa1a0f37-108e-478c-83d4-4f095498cef6}</MetaDataID>
        public void AddItem(IItemPreparation itemPreparation)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FoodItems.Add(itemPreparation);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{9b5fa430-11c3-4ad7-98c5-749b4d06c186}</MetaDataID>
        public void RemoveItem(IItemPreparation itemPreparation)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FoodItems.Remove(itemPreparation);
                stateTransition.Consistent = true;
            }
        }

        internal static void AssignMealCourseToItem(ItemPreparation flavourItem)
        {
            //flavourItem.ClientSession.ServicePoint.
        }
    }
}