using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{9fb9b6d1-dca2-47f1-8275-9c769636dbd1}</MetaDataID>
    [BackwardCompatibilityID("{9fb9b6d1-dca2-47f1-8275-9c769636dbd1}")]
    [Persistent()]
    public class ServingBatch : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IServingBatch
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IItemPreparation> _PreparedItems = new OOAdvantech.Collections.Generic.Set<IItemPreparation>();

        /// <MetaDataID>{2411b3a6-0864-4dcf-b4c3-7ead1eb055c2}</MetaDataID>
        [PersistentMember(nameof(_PreparedItems))]
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public List<IItemPreparation> PreparedItems
        {
            get => _PreparedItems.ToThreadSafeList();

        }

        [CommitObjectStateInStorageCall]
        void CommitObjectState()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var item in (from itemsPreprationContext in ContextsOfPreparedItems
                                      from itemPreparation in itemsPreprationContext.PreparationItems
                                      select itemPreparation))
                {
                    _PreparedItems.Add(item);
                } 
                stateTransition.Consistent = true;
            }


        }
        /// 
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<HumanResources.ServingShiftWork> _ShiftWork = new OOAdvantech.Member<HumanResources.ServingShiftWork>();

        [PersistentMember(nameof(_ShiftWork))]
        [Association("ServingBatchInShiftWork", Roles.RoleB, "5b49aba4-a3de-46da-9a52-6436a3823d6f")]
        public HumanResources.ServingShiftWork ShiftWork => _ShiftWork.Value;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <MetaDataID>{eb3120b1-beb4-4c33-bd96-5b861484b691}</MetaDataID>
        [CachingDataOnClientSide]
        public string Description { get; set; }

        /// <MetaDataID>{46f36229-fd4d-4d54-b0c2-751685130bdc}</MetaDataID>
        [CachingDataOnClientSide]
        public string ServicesPointIdentity { get; set; }

        /// <MetaDataID>{7a3cacaf-eaba-4fcd-a84b-e8bc312e54f5}</MetaDataID>
        [CachingDataOnClientSide]
        public ServicePointType ServicePointType { get; set; }

        [CachingDataOnClientSide]
        public bool IsAssigned
        {
            get
            {
                return ShiftWork != null;
            }
        }


        /// <MetaDataID>{1603a1ea-fc7a-4b83-ac7a-24c143fe7d31}</MetaDataID>
        public IServicePoint ServicePoint { get; set; }

        /// <MetaDataID>{bb994edc-6942-44d7-ab3b-88e6ede82264}</MetaDataID>
        [CachingDataOnClientSide]
        public IMealCourse MealCourse { get; set; }

        /// <MetaDataID>{229605f9-a461-4742-b21c-003791d9f578}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfUnderPreparationItems { get; set; }

        /// <MetaDataID>{1abcf246-6696-41e7-82ae-19bd863348a1}</MetaDataID>
        [CachingDataOnClientSide]
        public IList<ItemsPreparationContext> ContextsOfPreparedItems { get; set; }

        /// <MetaDataID>{2bf04da7-e667-4ae3-8268-339da1a42253}</MetaDataID>
        protected ServingBatch()
        {
        }

        ///// <MetaDataID>{f444def7-8e79-4c23-bd25-7580da6b8351}</MetaDataID>
        //[OOAdvantech.Json.JsonConstructor]
        //public ServingBatch(string servicesPointIdentity, IMealCourse mealCourse, List<IItemPreparation> preparedItems, IList<ItemsPreparationContext> contextsOfPreparedItems, IList<ItemsPreparationContext> contextsOfUnderPreparationItems, string description)
        //{
        //    MealCourse = mealCourse;
        //    ServicesPointIdentity = servicesPointIdentity;
        //    PreparedItems = preparedItems;
        //    Description = description;
        //    ContextsOfPreparedItems = contextsOfPreparedItems;
        //    ContextsOfUnderPreparationItems = contextsOfUnderPreparationItems;
        //}
        public string MealCourseUri { get; }

        /// <MetaDataID>{35670efb-8e94-47ae-af59-82be8a3e6bec}</MetaDataID>
        public ServingBatch(IMealCourse mealCourse, IList<ItemsPreparationContext> preparedItems, IList<ItemsPreparationContext> underPreparationItems)
        {

            MealCourseUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);
            MealCourse = mealCourse;
            
            ContextsOfPreparedItems = preparedItems;
            ContextsOfUnderPreparationItems = underPreparationItems;
            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            ServicesPointIdentity = ServicePoint.ServicesPointIdentity;

          

            Description = mealCourse.Meal.Session.Description + " - " + mealCourse.Name;

            //Description = mealCourse.Name + " " + ServicePoint.ServiceArea.Description + " / " + ServicePoint.Description;
        }



    }
}