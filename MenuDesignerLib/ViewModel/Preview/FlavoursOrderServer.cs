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

  

        /// <MetaDataID>{77e8f92c-1f58-488b-98fb-9e4cbc081a42}</MetaDataID>
        public MarshalByRefObject GetObjectFromUri(string uri)
        {
            return this;
        }

        /// <MetaDataID>{6e1731e5-2a1e-4830-a55f-c93ed9359705}</MetaDataID>
        [CachingDataOnClientSide]
        public bool WaiterView => false;

        /// <MetaDataID>{c3de3741-c6d6-4ac6-a5f6-9d60e8a89b0e}</MetaDataID>
        public string ISOCurrencySymbol => System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol;


        /// <MetaDataID>{f211330f-4913-49e3-bd21-745bd8fcfaf6}</MetaDataID>
        public string Name { get => "Preview"; set { } }
        /// <MetaDataID>{4095e02d-2e21-4199-a39c-14acd23fade6}</MetaDataID>
        public string Trademark { get; set; }

        /// <MetaDataID>{0758702b-6834-4306-ad59-4d1af57872d7}</MetaDataID>
        string lan = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
        /// <MetaDataID>{3bdb7c96-29f2-4077-ae59-b7bc98f810e8}</MetaDataID>
        public string Language { get { return lan; } set { lan=value; } }
        /// <MetaDataID>{11884fe9-3ee9-49bf-9578-563dded8c8bd}</MetaDataID>
        public string DefaultLanguage { get { return "en"; } }
        /// <MetaDataID>{97b412c4-ff9d-4e02-8ef5-1e0ffdb38742}</MetaDataID>
        public string FontsLink { get; set; } = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";




        /// <MetaDataID>{48072147-aa20-4fc6-ba05-d7cdd6c63149}</MetaDataID>
        public string Path { get => "7f9bde62e6da45dc8c5661ee2220a7b0;9967813ee9d943db823ca97779eb9fd7"; set { } }
        /// <MetaDataID>{0ec4e6ff-b5ac-49ef-8fba-f8cbf0c14664}</MetaDataID>
        public IUser EndUser { get => null; set { } }

        /// <MetaDataID>{47646414-c673-4f6b-8f66-f3ddb256c78f}</MetaDataID>
        public IList<ItemPreparation> PreparationItems => new List<ItemPreparation>();

        /// <MetaDataID>{babd436a-b5f9-4de9-9cda-f7ef894f149e}</MetaDataID>
        public string AppIdentity => throw new NotImplementedException();

        /// <MetaDataID>{fe4a6110-398e-4df7-a29d-6399841c272e}</MetaDataID>
        public IList<IHallLayout> Halls { get => new List<IHallLayout>(); set { } }

        /// <MetaDataID>{7f2ecd84-7d05-485b-bcba-04a13acd0068}</MetaDataID>
        public ServicePointState ServicePointState => throw new NotImplementedException();

        /// <MetaDataID>{36d1286c-b2c6-4e85-a490-1708134d1821}</MetaDataID>
        public IFoodServiceSession MainSession => throw new NotImplementedException();

        /// <MetaDataID>{dade01e7-7ed4-47c4-9770-f7e1e66f188f}</MetaDataID>
        public List<IPlace> DeliveryPlaces => throw new NotImplementedException();

        /// <MetaDataID>{8890d8c9-15c0-41f1-b9e7-206823f86ebc}</MetaDataID>
        internal FoodServicesClientSessionViewModel FoodServicesClientSessionViewModel { get; private set; } = new FoodServicesClientSessionViewModel();
        IFoodServicesClientSessionViewModel IFlavoursOrderServer.CurrentFoodServicesClientSession => FoodServicesClientSessionViewModel;

        public List<IFoodServicesClientSessionViewModel> ActiveSessions => FoodServicesClientSessionViewModel != null ? new List<IFoodServicesClientSessionViewModel>() { FoodServicesClientSessionViewModel } : new List<IFoodServicesClientSessionViewModel>();

        Task<List<IFoodServicesClientSessionViewModel>> IFlavoursOrderServer.ActiveSessions => throw new NotImplementedException();

        ISecureUser IFlavoursOrderServer.EndUser{ get => null; set { } }

        public event PartOfMealRequestHandle PartOfMealRequest;
        public event MenuItemProposalHandle MenuItemProposal;
        public event SharedItemChangedHandle SharedItemChanged;
        public event MessmatesWaitForYouToDecideHandle MessmatesWaitForYouToDecide;
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{74d0b408-ec1a-4a84-b298-b6bc836c5c07}</MetaDataID>
        public void AcceptInvitation(Messmate messmate, string messageID)
        {



        }

        /// <MetaDataID>{253619bf-823e-4dd4-a8d0-83b124a1f40c}</MetaDataID>
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

        /// <MetaDataID>{bb7383a5-2985-4a1d-a990-a8fc47abce4a}</MetaDataID>
        public void AddItem(ItemPreparation item)
        {
        }

        /// <MetaDataID>{381f6813-a944-4b11-a36b-7fd552333e31}</MetaDataID>
        public void AddSharingItem(ItemPreparation item)
        {
        }

        /// <MetaDataID>{86dce4d2-e02f-4dcc-bc17-f81fe459b4f5}</MetaDataID>
        public void CancelMealInvitation(Messmate messmate)
        {
        }

        /// <MetaDataID>{850f6586-3991-418a-9e8b-eae0a102ad89}</MetaDataID>
        public Task<bool> CheckPermissionsForServicePointScan()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{11890a9b-d496-434b-a493-9d0907c2f68d}</MetaDataID>
        public Task<bool> CheckPermissionsPassivePushNotification()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{b52aa855-4b50-4c35-ae56-ad555a080df1}</MetaDataID>
        public Task<bool> ConnectToServicePoint(string servicePointIdentity = "")
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{3a2d6eac-5265-4aa8-9981-474d4412073d}</MetaDataID>
        public void DenyInvitation(Messmate messmate, string messageID)
        {
        }

        /// <MetaDataID>{1132fbad-390d-4d4d-bba8-da6fcf8ffc70}</MetaDataID>
        public void EndOfMenuItemProposal(Messmate messmate, string messageID)
        {
        }

        /// <MetaDataID>{77b95a8d-c00a-486a-9219-85fafa4838ea}</MetaDataID>
        public string Foo()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{466a22af-2650-4c33-83ce-5a55728e668f}</MetaDataID>
        public IList<Messmate> GetCandidateMessmates()
        {
            return new List<Messmate>();
        }

        /// <MetaDataID>{8a996f4f-fbeb-48fa-b61b-c42e97e1ac51}</MetaDataID>
        public string GetClientSessionQRCode(string color)
        {
            return "";
        }

        /// <MetaDataID>{3478e309-8e7a-451d-8648-6fabdd01ec6c}</MetaDataID>
        public Task<string> GetFriendlyName()
        {
            return Task.FromResult("Preview");
        }

        /// <MetaDataID>{7600f3d7-f80e-42a9-812a-474fb7fe0126}</MetaDataID>
        public Task<HallLayout> GetHallLayout()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{81760ee1-1db2-43e1-b35b-5bcc2226a071}</MetaDataID>
        public IList<Messmate> GetMessmates()
        {
            return new List<Messmate>();
        }

        /// <MetaDataID>{a173a980-dddd-4182-bba4-42a2e85b7df4}</MetaDataID>
        public Task<bool> GetServicePointData(string servicePointIdentity)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{0ce59d41-70ca-40a0-ab5b-a5809741b903}</MetaDataID>
        public Task<bool> IsSessionActive()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{0cb3e0cc-745c-4c22-aa9d-3b389e5c2e6e}</MetaDataID>
        public void ItemChanged(ItemPreparation item)
        {

        }

        /// <MetaDataID>{1844399b-3f66-4340-88d7-9ee12243f74e}</MetaDataID>
        public void MealInvitation(Messmate messmate)
        {

        }

        /// <MetaDataID>{a5ab09ee-4730-4bf6-936d-d22518322e4f}</MetaDataID>
        public void RefreshMessmates()
        {

        }

        /// <MetaDataID>{b6b6d1f8-5c44-49b4-8b37-83b166e62d52}</MetaDataID>
        public void RemoveItem(ItemPreparation item)
        {

        }

        /// <MetaDataID>{a5816714-e1df-4004-ac3b-e24afff23ab8}</MetaDataID>
        public Task<bool> RequestPermissionsForServicePointScan()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{cccc4bf5-a1d2-41c9-8da2-f52774909903}</MetaDataID>
        public Task<bool> RequestPermissionsPassivePushNotification()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c9ac6557-3cd8-4973-af58-baec83790927}</MetaDataID>
        public Task<bool> SendItemForPreparation()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{dccbb1db-4a1e-4a9f-b468-ea05a7eca7ff}</MetaDataID>
        public void SetFriendlyName(string firendlyName)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{7594681a-f9b5-42a3-acec-9c9e86ba242c}</MetaDataID>
        public void Speak(string text)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{759834ba-2f0c-4d1e-9da0-49897e6e1624}</MetaDataID>
        public void SuggestMenuItem(Messmate messmate, string menuItemUri)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{8d143539-ae90-4577-a88c-49bbdc857029}</MetaDataID>
        public void WebViewAttached()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{9a8a6009-8065-434e-9f73-785d0aba654b}</MetaDataID>
        public void WebViewLoaded()
        {

        }

        /// <MetaDataID>{325a70f5-d4a1-4524-ba6b-9567f6c240fd}</MetaDataID>
        public  async Task Initialize()
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

        /// <MetaDataID>{eeee47ec-e37b-4cdd-a0c4-6a9502d2b062}</MetaDataID>
        public string GetTranslation(string langCountry)
        {
            return null;
        }

        /// <MetaDataID>{ac079d04-03f8-4e0e-a92d-9ecc03b48339}</MetaDataID>
        public string GetString(string langCountry, string key)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{7a79589f-c9cf-4afa-8032-ba2cde1828c8}</MetaDataID>
        public void SetString(string langCountry, string key, string newValue)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{06410ad8-e36d-4765-b6c7-c07a8a549bf8}</MetaDataID>
        public void TransferItems(List<string> itemsPreparationsIDs, string targetServicePointIdentity)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{9438a9cc-a2bf-4e5a-9565-88b32e19b327}</MetaDataID>
        public void TransferSession(string targetServicePointIdentity)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{d6571b83-1b81-485e-baea-76011fb07c55}</MetaDataID>
        public void UpdateHallsServicePointStates(Dictionary<string, ServicePointState> hallsServicePointsState)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{a0bcb3e8-2ad1-4666-b870-1e0b1992d614}</MetaDataID>
        public Task GetServicePointDataEx(string foodServiceClientSessionUri)
        {
            throw new NotImplementedException();
        }


        /// <MetaDataID>{489df895-174b-4b72-9fb6-5a0f53a18ff5}</MetaDataID>
        public string GetMealInvitationQRCode(string color)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c83e2e52-0913-44b0-9dc2-e83dc0607f12}</MetaDataID>
        public Task<Location> GetCurrentLocation()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{6ef1ca8d-10cc-4c47-bab9-6b5ca678dd6f}</MetaDataID>
        public Task<bool> CheckPermissionsToAccessCurrentLocation()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{f51dc0be-fecd-4735-85a9-fd722c18a213}</MetaDataID>
        public Task<bool> RequestPermissionsToAccessCurrentLocation()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{aa57166d-0c3b-4265-b135-f65fce9cb64c}</MetaDataID>
        public void SaveDelivaryPlace(FlavourBusinessManager.EndUsers.Place deliveryPlace)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{39569622-0d38-4958-b5de-047ce04309c6}</MetaDataID>
        public void RemoveDelivaryPlace(Place deliveryPlace)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{aa434f18-402a-44e7-8ffb-aa940809e330}</MetaDataID>
        public void SelectDelivaryPlace(Place deliveryPlace)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{3e2488c5-3eb4-4953-a089-115252689802}</MetaDataID>
        public Task<List<FlavourBusinessFacade.HomeDeliveryServicePointInfo>> GetNeighborhoodFoodServers(Coordinate location)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{418d6bc8-aeaa-4985-8d30-b976abe0f06e}</MetaDataID>
        public void SendMealInvitationMessage(InvitationChannel channel)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{9bcd9462-224c-4397-a135-a2d0279a24e0}</MetaDataID>
        public void SendMealInvitationMessage(InvitationChannel channel, string endPoint)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{f4414e52-13fe-4111-b66b-a88b6f4ca611}</MetaDataID>
        public Task<Contact> PickContact()
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{79b6e9ed-ff27-41b1-abf0-710bf1315e5a}</MetaDataID>
        Task<bool> IFlavoursOrderServer.AcceptInvitation(Messmate messmate, string messageID)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{80d1e29e-2cdf-4d64-aef7-d348a0a26a4a}</MetaDataID>
        public Task<bool> GetFoodServicesClientSessionData(string foodServiceClientSessionUri)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OpenFoodServicesClientSession(string clientSessionID)
        {
            throw new NotImplementedException();
        }
    }
}
