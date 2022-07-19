using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{6412585a-256a-42d3-8bc5-e28467ff30b2}</MetaDataID>
    [BackwardCompatibilityID("{6412585a-256a-42d3-8bc5-e28467ff30b2}")]
    [Persistent()]
    public class HomeDeliveryServicePoint : FlavourBusinessFacade.ServicesContextResources.IHomeDeliveryServicePoint
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        public Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Coordinate> ServiceAreaMap { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<string> ServesMealTypesUris => throw new NotImplementedException();

        public IList<IMealType> ServesMealTypes => throw new NotImplementedException();

        public int Seats { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ServicePointState State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ServicePointType ServicePointType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<IFoodServiceClientSession> ActiveFoodServiceClientSessions => throw new NotImplementedException();

        public IList<IFoodServiceSession> ServiceSessions => throw new NotImplementedException();

        public string ServicesPointIdentity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ServicesContextIdentity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event ObjectChangeStateHandle ObjectChangeState;

        public void AddFoodServiceSession(IFoodServiceSession foodServiceSession)
        {
            throw new NotImplementedException();
        }

        public void AddMealType(string mealTypeUri)
        {
            throw new NotImplementedException();
        }

        public IFoodServiceClientSession GetFoodServiceClientSession(string clientName, string mealInvitationSessionID, string clientDeviceID, string deviceFirebaseToken, bool create = false)
        {
            throw new NotImplementedException();
        }

        public IList<IFoodServiceClientSession> GetServicePointOtherPeople(IFoodServiceClientSession serviceClientSession)
        {
            throw new NotImplementedException();
        }

        public IFoodServiceClientSession NewFoodServiceClientSession(string clientName, string clientDeviceID)
        {
            throw new NotImplementedException();
        }

        public IFoodServiceSession NewFoodServiceSession()
        {
            throw new NotImplementedException();
        }

        public void RemoveFoodServiceSession(IFoodServiceSession foodServiceSession)
        {
            throw new NotImplementedException();
        }

        public void RemoveMealType(string mealTypeUri)
        {
            throw new NotImplementedException();
        }
    }
}