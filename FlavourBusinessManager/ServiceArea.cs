using System;
using System.Collections.Generic;
using System.Linq;
using FlavourBusinessFacade.ServicesContextResources;
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
    public class ServiceArea :MarshalByRefObject, IServiceArea,OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <MetaDataID>{b3f32e8c-f23e-4338-8fa3-2d57ba439202}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+6")]
        private string MealTypesUris;




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
        [PersistentMember(300,nameof(_ServicesContextIdentity))]
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
        OOAdvantech.Collections.Generic.Set<IServicePoint> _ServicePoints = new OOAdvantech.Collections.Generic.Set<IServicePoint>();

        /// <MetaDataID>{9c5a8dc9-988a-46a5-9aa8-2084536af2e6}</MetaDataID>
        [PersistentMember(nameof(_ServicePoints))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+1")]
        public IList<IServicePoint> ServicePoints
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
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {

                IMealType mealType = ObjectStorage.GetStorageFromUri(mealTypeUri) as IMealType;
                if (mealType == null)
                    throw new System.Exception("Invalid meal type uri");
                _ServesMealTypes.Add(mealType);
                MealTypesUris = null;
                foreach ( var uri in _ServesMealTypes.Select(x => ObjectStorage.GetStorageOfObject(x).GetPersistentObjectUri(x)))
                {
                    if (MealTypesUris != null)
                        MealTypesUris += " ";
                    MealTypesUris += uri;
                }
                stateTransition.Consistent = true;
            }

            ObjectChangeState?.Invoke(this, nameof(MealTypesUris));
        }

        /// <MetaDataID>{0cc34eba-d947-45f6-bf50-d183f4bc89cd}</MetaDataID>
        public void RemoveMealType(string mealTypeUri)
        {
            int mealTypesCount = ServesMealTypes.Count;//force system to load mealTypes;
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                IMealType mealType = ObjectStorage.GetStorageFromUri(mealTypeUri) as IMealType;
                if (mealType == null)
                    throw new System.Exception("Invalid meal type uri");
                _ServesMealTypes.Remove(mealType);
                MealTypesUris = null;
                foreach (var uri in _ServesMealTypes.Select(x => ObjectStorage.GetStorageOfObject(x).GetPersistentObjectUri(x)))
                {
                    if (MealTypesUris != null)
                        MealTypesUris += " ";
                    MealTypesUris += uri;
                }
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(MealTypesUris));
        }

        /// <exclude>Excluded</exclude>
        List<IMealType> _ServesMealTypes = null;

        public event ObjectChangeStateHandle ObjectChangeState;

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

        public IList<string> ServesMealTypesUris
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MealTypesUris))
                    return new List<string>();
               return MealTypesUris.Split(' ');
            }
        }


        /// <MetaDataID>{fbed765d-b818-45e3-81d9-64ce055ccaea}</MetaDataID>
        public void AddServicePoint(IServicePoint servicePoint)
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServicePoints.Add(servicePoint); 
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{f046286f-6954-4a16-9a98-add0993a03e6}</MetaDataID>
        public void RemoveServicePoint(IServicePoint servicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServicePoints.Remove(servicePoint); 
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{d45fce1f-afc6-45db-9f52-8e22c009f307}</MetaDataID>
        public IServicePoint NewServicePoint()
        {


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                ServicePoint servicePoint = new ServicePoint();
                servicePoint.ServicesContextIdentity = ServicesContextIdentity;
                servicePoint.Description = Properties.Resources.DefaultServicePointDescription;
                servicePoint.ServicesPointIdentity = Guid.NewGuid().ToString("N");
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(servicePoint);
                _ServicePoints.Add(servicePoint); 
                stateTransition.Consistent = true;
                return servicePoint;

            }

        }

        /// <MetaDataID>{7473939f-0ecb-4949-ada5-0715c0d50926}</MetaDataID>
        public List<IServicePoint> GetUnassignedServicePoints(List<string> hallServicesPoints)
        {

            return ServicePoints.Where(x => !hallServicesPoints.Contains(x.ServicesPointIdentity)).ToList();
        }

        public bool MealTypeAssigned(string mealTypeUri)
        {
            throw new NotImplementedException();
        }
    }
}
