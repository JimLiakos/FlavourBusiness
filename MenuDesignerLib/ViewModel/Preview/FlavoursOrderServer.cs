using DontWaitApp;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.ViewModel;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.RoomService;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using RestaurantHallLayoutModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HallLayout = System.Object;
namespace MenuDesigner.ViewModel.Preview
{
    /// <MetaDataID>{d2f12f7e-f68b-40d1-88b0-07ae207eb60f}</MetaDataID>
    public class FlavoursOrderServer : MarshalByRefObject, IFlavoursOrderServer, FlavourBusinessFacade.ViewModel.ILocalization, OOAdvantech.Remoting.IExtMarshalByRefObject, OOAdvantech.Remoting.RestApi.IBoundObject
    {


        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            return this;
        }

        [CachingDataOnClientSide]
        public bool WaiterView => false;

        public string ISOCurrencySymbol => System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol;

        public string Identity { get; set; }
        public string Name { get => "Preview"; set { } }
        public string Trademark { get; set; }

        string lan = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
        public string Language { get { return lan; } }
        public string DefaultLanguage { get { return "en"; } }
        public string FontsLink { get; set; } = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";

        public MenuData MenuData { get; set; }


        public string Path { get => "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7"; set { } }
        public IUser EndUser { get => null; set { } }

        public IList<ItemPreparation> PreparationItems => new List<ItemPreparation>();

        public string AppIdentity => throw new NotImplementedException();

        public IList<IHallLayout> Halls { get => new List<IHallLayout>(); set { } }

        public ServicePointState ServicePointState => throw new NotImplementedException();

        public IFoodServiceSession MainSession => throw new NotImplementedException();

        public List<IPlace> DeliveryPlaces => throw new NotImplementedException();

        public event PartOfMealRequestHandle PartOfMealRequest;
        public event MenuItemProposalHandle MenuItemProposal;
        public event SharedItemChangedHandle SharedItemChanged;
        public event MessmatesWaitForYouToDecideHandle MessmatesWaitForYouToDecide;
        public event ObjectChangeStateHandle ObjectChangeState;

        public void AcceptInvitation(Messmate messmate, string messageID)
        {



        }

        public string LocalGet()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var responseTask = httpClient.GetStringAsync("http://localhost/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/Menus/0bb39514-b297-436c-8554-c5e5a52486ac/v109/Marzano Phone.json");
                responseTask.Wait();
                string data = responseTask.Result;
                return data;

            }
            catch (Exception error)
            {

                throw;
            }
        }

        public void AddItem(ItemPreparation item)
        {
        }

        public void AddSharingItem(ItemPreparation item)
        {
        }

        public void CancelMealInvitation(Messmate messmate)
        {
        }

        public Task<bool> CheckPermissionsForServicePointScan()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckPermissionsPassivePushNotification()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConnectToServicePoint(string servicePointIdentity = "")
        {
            throw new NotImplementedException();
        }

        public void DenyInvitation(Messmate messmate, string messageID)
        {
        }

        public void EndOfMenuItemProposal(Messmate messmate, string messageID)
        {
        }

        public string Foo()
        {
            throw new NotImplementedException();
        }

        public IList<Messmate> GetCandidateMessmates()
        {
            return new List<Messmate>();
        }

        public string GetClientSessionQRCode(string color)
        {
            return "";
        }

        public Task<string> GetFriendlyName()
        {
            return Task.FromResult("Preview");
        }

        public Task<HallLayout> GetHallLayout()
        {
            throw new NotImplementedException();
        }

        public IList<Messmate> GetMessmates()
        {
            return new List<Messmate>();
        }

        public Task<bool> GetServicePointData(string servicePointIdentity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSessionActive()
        {
            throw new NotImplementedException();
        }

        public void ItemChanged(ItemPreparation item)
        {
            
        }

        public void MealInvitation(Messmate messmate)
        {
            
        }

        public void RefreshMessmates()
        {
            
        }

        public void RemoveItem(ItemPreparation item)
        {
            
        }

        public Task<bool> RequestPermissionsForServicePointScan()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestPermissionsPassivePushNotification()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendItemForPreparation()
        {
            throw new NotImplementedException();
        }

        public void SetFriendlyName(string firendlyName)
        {
            throw new NotImplementedException();
        }

        public void Speak(string text)
        {
            throw new NotImplementedException();
        }

        public void SuggestMenuItem(Messmate messmate, string menuItemUri)
        {
            throw new NotImplementedException();
        }

        public void WebViewAttached()
        {
            throw new NotImplementedException();
        }

        public void WebViewLoaded()
        {

        }

        internal async void Initialize()
        {
            //if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.LastServicePoinMenuData.ServicePointIdentity))
            //{
            //    while (!await GetServicePointData(MenuData.ServicePointIdentity))
            //    {
            //        if (string.IsNullOrWhiteSpace(Path) || Path.Split('/')[0] != MenuData.ServicePointIdentity)
            //            break;
            //    }
            //    if (this.FoodServiceClientSession != null)
            //    {
            //        GetMessages();

            //    }
            //    else
            //    {

            //        ObjectChangeState?.Invoke(this, nameof(FoodServiceClientSession));
            //        ApplicationSettings.Current.LastServicePoinMenuData = new MenuData();
            //        ApplicationSettings.Current.LastClientSessionID = "";
            //        Path = "";

            //    }
            //}
        }

        public string GetTranslation(string langCountry)
        {
            return null;
        }

        public string GetString(string langCountry, string key)
        {
            throw new NotImplementedException();
        }

        public void SetString(string langCountry, string key, string newValue)
        {
            throw new NotImplementedException();
        }

        public void TransferItems(List<string> itemsPreparationsIDs, string targetServicePointIdentity)
        {
            throw new NotImplementedException();
        }

        public void TransferSession(string targetServicePointIdentity)
        {
            throw new NotImplementedException();
        }

        public void UpdateHallsServicePointStates(Dictionary<string, ServicePointState> hallsServicePointsState)
        {
            throw new NotImplementedException();
        }

        public Task GetServicePointDataEx(string foodServiceClientSessionUri)
        {
            throw new NotImplementedException();
        }

        Task<bool> IFlavoursOrderServer.GetServicePointDataEx(string foodServiceClientSessionUri)
        {
            throw new NotImplementedException();
        }

        public string GetMealInvitationQRCode(string color)
        {
            throw new NotImplementedException();
        }

        public Task<Location> GetCurrentLocation()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckPermissionsToAccessCurrentLocation()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RequestPermissionsToAccessCurrentLocation()
        {
            throw new NotImplementedException();
        }

        public void SaveDelivaryPlace(FlavourBusinessManager.EndUsers.Place deliveryPlace)
        {
            throw new NotImplementedException();
        }

        public void RemoveDelivaryPlace(Place deliveryPlace)
        {
            throw new NotImplementedException();
        }

        public void SelectDelivaryPlace(Place deliveryPlace)
        {
            throw new NotImplementedException();
        }

        public Task<List<FlavourBusinessFacade.HomeDeliveryServicePointInfo>> GetNeighborhoodFoodServers(Coordinate location)
        {
            throw new NotImplementedException();
        }
    }
}
