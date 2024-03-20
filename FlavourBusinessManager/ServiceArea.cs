using System;
using System.Collections.Generic;
using System.Linq;
using FlavourBusinessFacade;
using FlavourBusinessFacade.PriceList;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.HumanResources;
using MenuModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{9b6776e7-0c74-4bb0-80d3-66f13ed99260}</MetaDataID>
    [BackwardCompatibilityID("{9b6776e7-0c74-4bb0-80d3-66f13ed99260}")]
    [Persistent()]
    public class ServiceArea : MarshalByRefObject, IServiceArea, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
      
        /// <MetaDataID>{b3f32e8c-f23e-4338-8fa3-2d57ba439202}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+6")]
        private string MealTypesUris;



        /// <MetaDataID>{6b88620a-22a4-4057-938a-0ae98f6df236}</MetaDataID>
        [ObjectActivationCall]
        internal void OnActivated()
        {

            foreach (var servicePoint in ServicePoints)
                servicePoint.ObjectChangeState += ServicePoint_ObjectChangeState;


            if (string.IsNullOrWhiteSpace(MealTypesUris))
            {
                var twoCourseMealType = ServicePointRunTime.ServicesContextRunTime.Current.GetTowcoursesMealType();
                if (twoCourseMealType != null)
                    MealTypesUris = (ObjectStorage.GetStorageOfObject(twoCourseMealType).GetPersistentObjectUri(twoCourseMealType));
            }


        }

        /// <MetaDataID>{3fd60574-f95a-403a-8b70-0182221409ee}</MetaDataID>
        private void ServicePoint_ObjectChangeState(object _object, string member)
        {
            if (_object is ServicePoint && member == nameof(ServicePoint.State))
                this._ServicePointChangeState?.Invoke(this, _object as ServicePoint, (_object as ServicePoint).State);

        }



        /// <exclude>Excluded</exclude>
        string _HallLayoutUri;
        /// <MetaDataID>{102075d7-201c-4086-a85d-d1c0cd53149a}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [PersistentMember(nameof(_HallLayoutUri))]
        public string HallLayoutUri
        {
            get
            {
                return _HallLayoutUri;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _HallLayoutUri = value;
                    stateTransition.Consistent = true;
                }
            }
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{25e042bc-0698-4fb3-b368-6b5b6d8e9aab}</MetaDataID>
        [PersistentMember(nameof(_Description)), BackwardCompatibilityID("+2")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Description = value;
                    stateTransition.Consistent = true;
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;
        /// <MetaDataID>{2d69bfb6-e6f7-46b4-89bf-32fc44e3c3ce}</MetaDataID>
        [PersistentMember(300, nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+3")]
        public string ServicesContextIdentity
        {
            get
            {
                return _ServicesContextIdentity;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ServicesContextIdentity = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IHallServicePoint> _ServicePoints = new OOAdvantech.Collections.Generic.Set<IHallServicePoint>();

        /// <MetaDataID>{9c5a8dc9-988a-46a5-9aa8-2084536af2e6}</MetaDataID>
        [PersistentMember(nameof(_ServicePoints))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+1")]
        public IList<IHallServicePoint> ServicePoints
        {
            get
            {
                return _ServicePoints.ToThreadSafeList();
            }
        }

        /// <MetaDataID>{4576e681-bde6-4cbb-ad87-b4dd9c44dcc9}</MetaDataID>
        public void AddMealType(string mealTypeUri)
        {

            int mealTypesCount = ServesMealTypes.Count;//force system to load mealTypes;
            if (ServesMealTypesUris.Contains(mealTypeUri))
                return;

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {

                IMealType mealType = ObjectStorage.GetObjectFromUri(mealTypeUri) as IMealType;
                if (mealType != null)
                    _ServesMealTypes.Add(mealType);

                var servesMealTypesUris = ServesMealTypesUris;
                if (!servesMealTypesUris.Contains(mealTypeUri))
                    servesMealTypesUris.Add(mealTypeUri);

                MealTypesUris = null;
                foreach (var uri in servesMealTypesUris)
                {
                    if (MealTypesUris != null)
                        MealTypesUris += " ";
                    MealTypesUris += uri;
                }
                stateTransition.Consistent = true;
            }

            ObjectChangeState?.Invoke(this, nameof(ServesMealTypesUris));
        }

        /// <MetaDataID>{0cc34eba-d947-45f6-bf50-d183f4bc89cd}</MetaDataID>
        public void RemoveMealType(string mealTypeUri)
        {
            if (!ServesMealTypesUris.Contains(mealTypeUri))
                return;

            int mealTypesCount = ServesMealTypes.Count;//force system to load mealTypes;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                IMealType mealType = ObjectStorage.GetObjectFromUri(mealTypeUri) as IMealType;
                if (mealType != null)
                    _ServesMealTypes.Remove(mealType);

                var servesMealTypesUris = ServesMealTypesUris;
                if (servesMealTypesUris.Contains(mealTypeUri))
                    servesMealTypesUris.Remove(mealTypeUri);

                MealTypesUris = null;
                foreach (var uri in servesMealTypesUris)
                {
                    if (MealTypesUris != null)
                        MealTypesUris += " ";
                    MealTypesUris += uri;
                }
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ServesMealTypesUris));
        }

        /// <exclude>Excluded</exclude>
        List<IMealType> _ServesMealTypes = null;

        public event ObjectChangeStateHandle ObjectChangeState;
        event ServicePointChangeStateHandle _ServicePointChangeState;

        public event ServicePointChangeStateHandle ServicePointChangeState
        {
            add
            {
                _ServicePointChangeState += value;
            }
            remove
            {
                _ServicePointChangeState -= value;
            }
        }

        /// <MetaDataID>{b6136504-398d-4f72-8a67-d9018931d582}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        public IList<IMealType> ServesMealTypes
        {
            get
            {
                if (_ServesMealTypes == null)
                    _ServesMealTypes = (from mealTypeUri in ServesMealTypesUris
                                        select ObjectStorage.GetObjectFromUri(mealTypeUri) as IMealType).Where(x => x != null).ToList();
                return _ServesMealTypes.ToList();
            }
        }

        /// <MetaDataID>{b7d3e8db-4943-4483-929e-2740d1151916}</MetaDataID>
        public IList<string> ServesMealTypesUris
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MealTypesUris))
                    return new List<string>();
                return MealTypesUris.Split(' ').ToList();
            }
        }

        /// <MetaDataID>{24944b0d-f734-44d4-8565-a3ff9a72c3d0}</MetaDataID>
        public List<OrganizationStorageRef> PriceLists => throw new NotImplementedException();


        /// <MetaDataID>{fbed765d-b818-45e3-81d9-64ce055ccaea}</MetaDataID>
        public void AddServicePoint(IHallServicePoint servicePoint)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServicePoints.Add(servicePoint);
                stateTransition.Consistent = true;
            }
            servicePoint.ObjectChangeState += ServicePoint_ObjectChangeState;
        }

        /// <MetaDataID>{f046286f-6954-4a16-9a98-add0993a03e6}</MetaDataID>
        public void RemoveServicePoint(IHallServicePoint servicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServicePoints.Remove(servicePoint);
                stateTransition.Consistent = true;
            }
            servicePoint.ObjectChangeState -= ServicePoint_ObjectChangeState;

        }

        /// <MetaDataID>{d45fce1f-afc6-45db-9f52-8e22c009f307}</MetaDataID>
        public IHallServicePoint NewServicePoint()
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                HallServicePoint servicePoint = new HallServicePoint();
                servicePoint.ServicesContextIdentity = ServicesContextIdentity;
                servicePoint.Description = Properties.Resources.DefaultServicePointDescription;
                servicePoint.ServicesPointIdentity = Guid.NewGuid().ToString("N");
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(servicePoint);
                _ServicePoints.Add(servicePoint);
                servicePoint.ObjectChangeState += ServicePoint_ObjectChangeState;
                stateTransition.Consistent = true;
                return servicePoint;

            }

        }

        /// <MetaDataID>{7473939f-0ecb-4949-ada5-0715c0d50926}</MetaDataID>
        public List<IHallServicePoint> GetUnassignedServicePoints(List<string> hallServicesPoints)
        {

            return ServicePoints.Where(x => !hallServicesPoints.Contains(x.ServicesPointIdentity)).ToList();
        }

        /// <MetaDataID>{4fc0a363-a133-4ab6-ba36-41a95afb79f9}</MetaDataID>
        public bool MealTypeAssigned(string mealTypeUri)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{6f81259c-da76-4ebc-bb6b-a12d616f242c}</MetaDataID>
        public void AssignPriceList(OrganizationStorageRef priceListStorageRef)
        {

            ServicePointRunTime.ServicesContextRunTime.Current.AssignPriceList(priceListStorageRef);

            FlavourBusinessToolKit.RawStorageData rawPriceListData = new FlavourBusinessToolKit.RawStorageData(priceListStorageRef, null);
            OOAdvantech.Linq.Storage restMenusData = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("PriceList", rawPriceListData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider"));
            var priceList = (from m_priceList in restMenusData.GetObjectCollection<IPriceList>()
                                  select m_priceList).FirstOrDefault();


        }

        /// <MetaDataID>{3cab0aaa-91be-48c8-9edb-07dd51a0d642}</MetaDataID>
        public void RemovePriceList(OrganizationStorageRef priceListStorageRef)
        {
            throw new NotImplementedException();
        }
    }
}
