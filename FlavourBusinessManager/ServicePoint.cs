using System;
using System.Collections.Generic;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Linq;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.HumanResources;
using FlavourBusinessFacade.HumanResources;
using MenuModel;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{5a6deea9-0208-441b-9b01-77a13dc8c126}</MetaDataID>
    [BackwardCompatibilityID("{5a6deea9-0208-441b-9b01-77a13dc8c126}")]
    [Persistent()]
    public class ServicePoint : MarshalByRefObject, IServicePoint, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        /// <exclude>Excluded</exclude>
        int _Seats;

        /// <MetaDataID>{bc816cfa-a904-4461-a92b-7ea5ad1bbb09}</MetaDataID>
        [PersistentMember(nameof(_Seats))]
        [BackwardCompatibilityID("+6")]
        public int Seats
        {
            get => _Seats;
            set
            {
                if (_Seats != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Seats = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{bb55702d-4be6-4339-94f5-354cf83542ec}</MetaDataID>
        public ServicePoint()
        {
        }





        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink = new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{883aabfe-47f1-45db-80f6-3c3e077b862f}</MetaDataID>
        [PersistentMember(nameof(_Description)), BackwardCompatibilityID("+1")]
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
        /// <MetaDataID>{7e7046e7-023a-4b36-b55e-898075088d56}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [PersistentMember(nameof(_ServicesContextIdentity))]
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
        string _ServicesPointIdentity;
        /// <MetaDataID>{7187498f-52cf-4f19-84a2-1fd301986c46}</MetaDataID>
        [PersistentMember(nameof(_ServicesPointIdentity))]
        [BackwardCompatibilityID("+3")]
        public string ServicesPointIdentity
        {
            get
            {
                if (_ServicesPointIdentity == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesPointIdentity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }

                }

                return _ServicesPointIdentity;
            }

            set
            {
                if (_ServicesPointIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesPointIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{b3a3c408-9ebf-4d2b-af0c-8cc7268f12ab}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<IFoodServiceSession> _ServiceSessions = new OOAdvantech.Collections.Generic.Set<IFoodServiceSession>();
        /// <MetaDataID>{3f818102-6573-4e5a-99cf-8f52c39c0805}</MetaDataID>
        [PersistentMember(nameof(_ServiceSessions))]
        [BackwardCompatibilityID("+4")]
        public System.Collections.Generic.IList<IFoodServiceSession> ServiceSessions
        {
            get
            {
                return _ServiceSessions.AsReadOnly();
            }
        }


        /// <MetaDataID>{950c80cc-27bb-4dcd-88e5-4dc2836c4a0a}</MetaDataID>
        object ServicePointLock = new object();
        /// <exclude>Excluded</exclude>
        List<IFoodServiceClientSession> _ActiveFoodServiceClientSessions;

        /// <MetaDataID>{7beae4f4-7e79-476d-9f83-8c169bd6a8dd}</MetaDataID>
        public System.Collections.Generic.IList<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession> ActiveFoodServiceClientSessions
        {
            get
            {
                lock (ServicePointLock)
                {
                    if (_ActiveFoodServiceClientSessions == null)
                    {
                        var objectStorage = ObjectStorage.GetStorageOfObject(this);

                        OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

                        var activeFoodServiceClientSessions = (from session in servicesContextStorage.GetObjectCollection<FlavourBusinessFacade.EndUsers.IFoodServiceClientSession>()
                                                               where session.ServicePoint == this && session.SessionEnds > System.DateTime.UtcNow
                                                               select session).ToList();


                        _ActiveFoodServiceClientSessions = activeFoodServiceClientSessions;

                    }
                    return _ActiveFoodServiceClientSessions.ToList();
                }

            }
        }

        /// <exclude>Excluded</exclude>
        ServicePointState _State;
        /// <MetaDataID>{b9c9676f-c53d-4952-92e6-b17d22d81e18}</MetaDataID>
        [PersistentMember(nameof(_State))]
        [BackwardCompatibilityID("+7")]
        public ServicePointState State
        {
            get => _State;
            set
            {

                if (_State != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _State = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{4182b8cc-608d-460a-838e-b666eb57c83c}</MetaDataID>
        public void AddFoodServiceSession(IFoodServiceSession foodServiceSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServiceSessions.Add(foodServiceSession);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{9379a27e-c078-423d-afad-b3587d587247}</MetaDataID>
        public void RemoveFoodServiceSession(IFoodServiceSession foodServiceSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServiceSessions.Remove(foodServiceSession);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{0eac815e-114c-40bf-b5f4-a4b72d42e03a}</MetaDataID>
        public IFoodServiceSession NewFoodServiceSession()
        {
            FoodServiceSession foodServiceSession = new FoodServiceSession();
            foodServiceSession.SessionStarts = DateTime.UtcNow;
            foodServiceSession.SessionEnds = DateTime.MinValue;
            AddFoodServiceSession(foodServiceSession);
            return foodServiceSession;
        }

        /// <MetaDataID>{ca0eaef8-e9fa-4c7f-b0e9-ae9e6b5b6d8d}</MetaDataID>
        public void AddServicePointClientSesion(IFoodServiceClientSession foodServiceClientSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ActiveFoodServiceClientSessions.Add(foodServiceClientSession);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{37354422-3d39-4c76-9c59-ba11861a1151}</MetaDataID>
        public void RemoveServicePointClientSesion(IFoodServiceClientSession foodServiceClientSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ActiveFoodServiceClientSessions.Remove(foodServiceClientSession);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{f9251525-75d8-4189-b246-e25d08c268ce}</MetaDataID>
        public IFoodServiceClientSession NewFoodServiceClientSession(string clientName, string clientDeviceID)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Provides a session between client and service point.
        /// In case where there isn't session and the create flag is false return s null;
        /// </summary>
        /// <param name="clientName">
        /// Defines the client nick name
        /// </param>
        /// <param name="mealInvitationSessionID">
        /// Defines the identity which is necessary to make client session part of meal  
        /// </param>
        /// <param name="clientDeviceID">
        /// Defines the identity of device which used from the client to send its order 
        /// </param>
        /// <param name="deviceFirebaseToken">
        /// Defines the device firebase token for push notification facility 
        /// </param>
        /// <param name="clientIdentity">
        /// Defines the identity of client.
        /// Used only in case where client is signed in
        /// </param>
        /// <param name="create">
        /// In case where this flag is true, service points creates a session if there isn't.
        /// </param>
        /// <returns>
        /// return the client session
        /// </returns>
        /// <MetaDataID>{d9fdfbcc-661f-4f13-a29d-2c7e42a886aa}</MetaDataID>
        public IFoodServiceClientSession GetFoodServiceClientSession(string clientName, string mealInvitationSessionID, string clientDeviceID, string deviceFirebaseToken, string clientIdentity, FlavourBusinessFacade.IUser user, bool create = false)
        {
            EndUsers.FoodServiceClientSession fsClientSession = null;
            EndUsers.FoodServiceClientSession messmateClientSesion = null;
            if (string.IsNullOrWhiteSpace(clientIdentity))
            {
                var objectStorage = ObjectStorage.GetStorageOfObject(this);

                fsClientSession = (from session in ActiveFoodServiceClientSessions.OfType<EndUsers.FoodServiceClientSession>()
                                   where session.ServicePoint == this && session.ClientDeviceID == clientDeviceID
                                   select session).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(clientName) && user != null)
                    clientName = user.FullName;



                //fsClientSession = (from session in servicesContextStorage.GetObjectCollection<EndUsers.FoodServiceClientSession>()
                //                   where session.ServicePoint == this && session.ClientDeviceID == clientDeviceID && session.SessionEnds > System.DateTime.UtcNow
                //                   select session).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(mealInvitationSessionID))

                    messmateClientSesion = (from session in ActiveFoodServiceClientSessions.OfType<EndUsers.FoodServiceClientSession>()
                                            where session.ServicePoint == this && session.SessionID == mealInvitationSessionID
                                            select session).FirstOrDefault();


                //messmateClientSesion = (from session in servicesContextStorage.GetObjectCollection<EndUsers.FoodServiceClientSession>()
                //                        where session.ServicePoint == this && session.SessionID == mealInvitationSessionID && session.SessionEnds > System.DateTime.UtcNow
                //                        select session).FirstOrDefault();

                if (fsClientSession == null && create)
                {
                    try
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            fsClientSession = new EndUsers.FoodServiceClientSession();
                            fsClientSession.ClientName = clientName;
                            fsClientSession.ClientDeviceID = clientDeviceID;
                            fsClientSession.DeviceFirebaseToken = deviceFirebaseToken;
                            fsClientSession.SessionStarts = DateTime.UtcNow;
                            fsClientSession.ModificationTime = DateTime.UtcNow;
                            fsClientSession.PreviousYouMustDecideMessageTime = DateTime.UtcNow;

                            if (user is HumanResources.Waiter)
                            {
                                fsClientSession.IsWaiterSession = true;
                                (user as HumanResources.Waiter).AddClientSession(fsClientSession);
                            }



                            fsClientSession.DateTimeOfLastRequest = DateTime.UtcNow;// DateTime.MinValue + TimeSpan.FromDays(28);
                            objectStorage.CommitTransientObjectState(fsClientSession);
                            fsClientSession.ServicePoint = this;

                            if (messmateClientSesion != null && messmateClientSesion.ServicePoint == this)
                                messmateClientSesion.MakePartOfMeal(fsClientSession);



                            stateTransition.Consistent = true;
                        }
                    }
                    catch (OOAdvantech.Transactions.TransactionException error)
                    {
                        throw;
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }
                    lock (ServicePointLock)
                    {
                        
                        if (_ActiveFoodServiceClientSessions.Where(x => !x.IsWaiterSession).Count() == 0&&!fsClientSession.IsWaiterSession)
                            ChangeServicePointState(ServicePointState.Laying);


                        _ActiveFoodServiceClientSessions.Add(fsClientSession);
                    }
                }
                else
                {
                    if (fsClientSession != null)
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            if ((DateTime.UtcNow - fsClientSession.DateTimeOfLastRequest).TotalMinutes > 0.5)
                                fsClientSession.DateTimeOfLastRequest = DateTime.UtcNow;
                            fsClientSession.ClientName = clientName;
                            if (fsClientSession.DeviceFirebaseToken != deviceFirebaseToken)
                            {

                            }
                            fsClientSession.DeviceFirebaseToken = deviceFirebaseToken;
                            if (messmateClientSesion != null && messmateClientSesion.ServicePoint == this)
                                messmateClientSesion.MakePartOfMeal(fsClientSession);
                            stateTransition.Consistent = true;
                        }
                    }
                }
            }
            return fsClientSession;
        }

        /// <MetaDataID>{aed44a3c-a2db-4548-bf09-9975a69848fe}</MetaDataID>
        ServicePointRunTime.ServicesContextRunTime _ServicesContextRunTime;
        /// <MetaDataID>{54737567-c31c-42ab-8d40-8334f928483a}</MetaDataID>
        ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime
        {
            get
            {
                if (_ServicesContextRunTime == null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));
                    _ServicesContextRunTime = (from runTime in storage.GetObjectCollection<ServicePointRunTime.ServicesContextRunTime>() select runTime).FirstOrDefault();
                }
                return _ServicesContextRunTime;

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IMealType> _ServesMealTypes = new OOAdvantech.Collections.Generic.Set<IMealType>();

        /// <MetaDataID>{686cd5b3-a352-4230-9143-a6cf92b45210}</MetaDataID>
        [PersistentMember(nameof(_ServesMealTypes))]
        [BackwardCompatibilityID("+8")]
        public IList<IMealType> ServesMealTypes => _ServesMealTypes.ToThreadSafeList();

        public void AddMealType(IMealType mealType)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServesMealTypes.Add(mealType); 
                stateTransition.Consistent = true;
            }

        }

        public void RemoveMealType(IMealType mealType)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ServesMealTypes.Remove(mealType); 
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{c7d46722-ff23-48b3-a51e-3a7e3bbe7e3a}</MetaDataID>
        private void ChangeServicePointState(ServicePointState newState)
        {
            if (State != newState)
            {
                var oldState = State;
                State = newState;
                switch (State)
                {
                    case ServicePointState.Laying:
                        {

                            this.ServicesContextRunTime.ServicePointChangeState(this, oldState, newState);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        /// <MetaDataID>{34345d6f-bc61-494d-9814-7644ba581c12}</MetaDataID>
        public IList<IFoodServiceClientSession> GetServicePointOtherPeople(IFoodServiceClientSession serviceClientSession)
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this));


            var collection = (from foodServiceClient in storage.GetObjectCollection<FoodServiceClientSession>()
                              where foodServiceClient.ServicePoint == this && foodServiceClient != serviceClientSession && !foodServiceClient.IsWaiterSession
                              select foodServiceClient).ToList();




            return collection.OfType<IFoodServiceClientSession>().ToList();
        }

        /// <MetaDataID>{4bcce2d4-e720-47bd-bafe-b5dea36afbc3}</MetaDataID>
        internal bool IsAssignedTo(IWaiter waiter, IShiftWork shiftWork)
        {
            return true;
        }

    
    }




}
