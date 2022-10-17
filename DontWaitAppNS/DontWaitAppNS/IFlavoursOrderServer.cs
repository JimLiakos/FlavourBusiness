using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

using OOAdvantech.Remoting.RestApi;
using FlavourBusinessFacade.RoomService;
using System.Configuration;

using OOAdvantech.BinaryFormatter;
using System.Drawing;
using OOAdvantech;
using FlavourBusinessManager.RoomService;





#if DeviceDotNet
using Xamarin.Forms;
using Xamarin.Essentials;
using ZXing.Net.Mobile.Forms;
using ZXing;
using ZXing.QrCode;
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
using FlavourBusinessFacade.ServicesContextResources;

#else
using System.IO;
using System.Drawing.Imaging;
using System;
using MenuModel;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;

#endif

#if FlavourBusinessDevice
using RestaurantHallLayoutModel;
#else
using HallLayout=System.Object;
#endif 

namespace DontWaitApp
{



    public delegate void PartOfMealRequestHandle(IFlavoursOrderServer flavoursOrderServer, Messmate messmate, string messageID);

    public delegate void MenuItemProposalHandle(IFoodServicesClientSessionViewModel flavoursOrderServer, Messmate messmate, string menuItemUri, string messageID);

    public delegate void SharedItemChangedHandle(IFoodServicesClientSessionViewModel flavoursOrderServer, Messmate messmate, string sharedItemUid, string messageID);

    public delegate void MessmatesWaitForYouToDecideHandle(IFoodServicesClientSessionViewModel flavoursOrderServer, List<Messmate> messmates, string messageID);



    /// <MetaDataID>{90fe460d-7996-49ca-a085-054466973111}</MetaDataID>
    [HttpVisible]
    public interface IFlavoursOrderServer
    {
         /// <MetaDataID>{3bbf03c4-e258-4f8f-89f7-68ad0e1c8e1b}</MetaDataID>
        void WebViewLoaded();


        /// <MetaDataID>{8fedff9e-af0f-4f35-98de-a9bf18486a23}</MetaDataID>
        IFoodServicesClientSessionViewModel CurrentFoodServicesClientSession { get; }

        /// <MetaDataID>{c4fef46e-2e44-414e-89a8-cdebcff380cc}</MetaDataID>
        bool WaiterView { get; }

        /// <MetaDataID>{71dba63a-ee1a-457e-aa4a-dc758bc11a06}</MetaDataID>
        string ISOCurrencySymbol { get; }

        /// <MetaDataID>{ee325a62-08c4-4121-aed8-03e1f94eb337}</MetaDataID>
        string Language { get; }

        /// <MetaDataID>{07d8eaf5-a147-4f73-9176-9700a86ebe64}</MetaDataID>
        string FontsLink { get; }



        /// <MetaDataID>{9022a0ee-4e3b-4ebf-9fdc-b51bf6e4443e}</MetaDataID>
        void WebViewAttached();


        /// <MetaDataID>{6c4436d3-a29e-4835-9076-68c772696479}</MetaDataID>
        string Path { get; set; }


        /// <MetaDataID>{e7762c6c-3f6e-49b7-b795-ade0e705aac8}</MetaDataID>
        Task<bool> ConnectToServicePoint(string servicePointIdentity = "");


        /// <MetaDataID>{9006f63a-f41f-4ca5-81ba-91578660d4d4}</MetaDataID>
        void Speak(string text);


        #region Permissions
        /// <summary>
        /// Check if application is granted to access infrastructure for service point scanning 
        /// </summary>
        /// <returns>
        /// for granted  return true
        /// else return false
        /// </returns>
        /// <MetaDataID>{f5ade777-4b64-49ed-8a59-248d2140ec49}</MetaDataID>
        Task<bool> CheckPermissionsForServicePointScan();

        /// <summary>
        /// Request Permission to access infrastructure for service point scanning 
        /// </summary>
        /// <returns>
        /// for granted  return true
        /// else return false
        /// </returns>
        /// <MetaDataID>{ba8d35eb-61c2-4e41-9464-66a58f9e7e7b}</MetaDataID>
        Task<bool> RequestPermissionsForServicePointScan();

        /// <MetaDataID>{55d5e7c5-5e7f-45c5-a6cc-9e9dcb322cd0}</MetaDataID>
        Task<bool> CheckPermissionsPassivePushNotification();
        /// <MetaDataID>{1f1dc909-3bb8-480d-897b-8b48b65ad373}</MetaDataID>
        Task<bool> RequestPermissionsPassivePushNotification();


        /// <MetaDataID>{a330bb9e-1b46-439b-b7a8-1792b7bb011f}</MetaDataID>
        Task<bool> CheckPermissionsToAccessCurrentLocation();
        /// <MetaDataID>{6932fafa-61f2-49fe-916c-4fea36a28676}</MetaDataID>
        Task<bool> RequestPermissionsToAccessCurrentLocation();
        #endregion

        #region  Meal invitation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        /// <MetaDataID>{9a168c2f-de48-47d1-ab4c-d46d48292364}</MetaDataID>
        string GetMealInvitationQRCode(string color);

        /// <MetaDataID>{8fabd5f0-b381-439c-a726-932a70dcdf4d}</MetaDataID>
        void MealInvitation(Messmate messmate);

        /// <MetaDataID>{8b5ff64f-4f61-432f-b325-0c60437e55c0}</MetaDataID>
        void SendMealInvitationMessage(InvitationChannel channel, string endPoint);

        /// <MetaDataID>{540090f5-2088-4e12-9219-e9c7264b7076}</MetaDataID>
        Task<Contact> PickContact();

        /// <MetaDataID>{009b6efd-a074-450e-b2d7-58755a212bb3}</MetaDataID>
        Task<bool> AcceptInvitation(Messmate messmate, string messageID);

        /// <MetaDataID>{149a02ff-8930-49cb-ae78-fa52cbea5b39}</MetaDataID>
        void DenyInvitation(Messmate messmate, string messageID);

        /// <MetaDataID>{a7914fd9-b836-4504-ab6a-407c4803f4f6}</MetaDataID>
        void CancelMealInvitation(Messmate messmate);
        #endregion


        /// <MetaDataID>{f4667ff4-da0b-4ea8-9f99-b892383facf2}</MetaDataID>
        Task<string> GetFriendlyName();

        /// <MetaDataID>{46d4ae2e-cbe8-4597-bf34-a3d4755e098b}</MetaDataID>
        void SetFriendlyName(string firendlyName);

        /// <MetaDataID>{8491fd19-2882-4b78-ac5e-84db4b54ccde}</MetaDataID>
        Task<List<HomeDeliveryServicePointInfo>> GetNeighborhoodFoodServers(Coordinate location);



        [GenerateEventConsumerProxy]
        event PartOfMealRequestHandle PartOfMealRequest;









        /// <MetaDataID>{ab9ec6c8-5f98-4020-9832-dd90ac068f28}</MetaDataID>
        string Name { get; set; }
        /// <MetaDataID>{10656240-faac-4a11-94d7-168b7788c05b}</MetaDataID>
        string Trademark { get; set; }

    

 



   

     


        /// <MetaDataID>{232e53ac-ddc8-4e58-9083-dbbb26002e43}</MetaDataID>
        Task<HallLayout> GetHallLayout();

        /// <MetaDataID>{86b22218-7670-4c8d-be56-e4e68c2ad7fc}</MetaDataID>
        IList<FlavourBusinessFacade.ServicesContextResources.IHallLayout> Halls
        {
            get;
            set;
        }










        /// <MetaDataID>{68a07293-c4b2-44e2-a86a-aa1b7d858fb9}</MetaDataID>
        Task<bool> IsSessionActive();


        /// <MetaDataID>{9c56bc8e-7682-4589-8e34-a76996e6b77a}</MetaDataID>
        FlavourBusinessFacade.ViewModel.IUser EndUser { get; set; }



  



        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{1296cf3c-a0c6-4483-a8ec-3c2ac3874660}</MetaDataID>
        void UpdateHallsServicePointStates(Dictionary<string, ServicePointState> hallsServicePointsState);
        /// <MetaDataID>{d4579596-157a-494d-ba17-908edbf4586b}</MetaDataID>
    

    }

    /// <MetaDataID>{b955c48a-3bb5-4c3e-b97e-9799ad7a1cbe}</MetaDataID>
    public class Contact
    {
        string displayName;

        public Contact()
        {
        }
#if DeviceDotNet
        public Contact(
            string id,
            string namePrefix,
            string givenName,
            string middleName,
            string familyName,
            string nameSuffix,
            IEnumerable<ContactPhone> phones,
            IEnumerable<ContactEmail> email,
            string displayName = null)
        {
            Id = id;
            NamePrefix = namePrefix;
            GivenName = givenName;
            MiddleName = middleName;
            FamilyName = familyName;
            NameSuffix = nameSuffix;
            Phones.AddRange(phones?.Select(x=>x.PhoneNumber).ToList());
            Emails.AddRange(email?.Select(x => x.EmailAddress).ToList());
            DisplayName = displayName;
        }
#endif

        public string Id { get; set; }

        public string DisplayName
        {
            get => !string.IsNullOrWhiteSpace(displayName) ? displayName : BuildDisplayName();
            private set => displayName = value;
        }

        public string NamePrefix { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string FamilyName { get; set; }

        public string NameSuffix { get; set; }

        public List<string> Phones { get; set; } = new List<string>();

        public List<string> Emails { get; set; } = new List<string>();

        public override string ToString() => DisplayName;

        string BuildDisplayName()
        {
            if (string.IsNullOrWhiteSpace(GivenName))
                return FamilyName;
            if (string.IsNullOrWhiteSpace(FamilyName))
                return GivenName;

            return String.Format("{0} {1}", GivenName, FamilyName);
        }
    }

    public delegate void WebViewLoadedHandle();



    /// <MetaDataID>{5718fadd-9a57-4d87-a6ea-ba669ab3388a}</MetaDataID>
    [BackwardCompatibilityID("{5718fadd-9a57-4d87-a6ea-ba669ab3388a}")]
    [Persistent()]
    public struct MenuData
    {
        //public MenuData()
        //{
        //    _OrderItems = new OOAdvantech.Collections.Generic.Set<ItemPreparation>();
        //    this.ServicePointIdentity = null;
        //    this.MenuName = null;
        //    this.MenuFile = null;
        //    this.MenuRoot = null;
        //    this.ClientSessionID = null;
        //}


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ItemPreparation> _OrderItems;

        [Association("ClientSesionOrderItems", Roles.RoleA, "e3768b00-5653-493c-b641-d21fdca0dbb1")]
        [PersistentMember(nameof(_OrderItems))]
        public List<ItemPreparation> OrderItems
        {
            get
            {
                if (_OrderItems != null)
                    return _OrderItems.ToList();
                else
                    return new List<ItemPreparation>();
            }
            set => _OrderItems = new OOAdvantech.Collections.Generic.Set<ItemPreparation>(value);
        }



        /// <MetaDataID>{b7cd60c7-28c0-4857-a66b-c185898f3105}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+10")]
        public string FoodServiceClientSessionUri;

        /// <MetaDataID>{8e686499-7cc6-4df6-a9de-89e00c76896e}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+11")]
        public string MainSessionID;


        /// <MetaDataID>{e369fe65-855e-4cc1-ba68-cf95b145ee10}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+9")]
        string ServedMealTypesUrisStream;

        /// <MetaDataID>{002deed5-f826-4e71-96b7-cad17bbf201d}</MetaDataID>
        public List<string> ServedMealTypesUris
        {
            get
            {
                if (ServedMealTypesUrisStream == null)
                    return new List<string>();

                return ServedMealTypesUrisStream.Split(';').ToList();
            }
            internal set
            {
                ServedMealTypesUrisStream = "";
                foreach (var uri in value)
                {
                    if (string.IsNullOrWhiteSpace(ServedMealTypesUrisStream))
                        ServedMealTypesUrisStream = uri;
                    else
                        ServedMealTypesUrisStream += ";" + uri;

                }
            }
        }
        /// <MetaDataID>{94b21849-ddf7-4553-9b1b-76bf018e6f84}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+12")]
        public SessionType SessionType;

        /// <MetaDataID>{2507296d-28a6-4771-841d-0bf3c1f5f4d4}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+8")]
        public string DefaultMealTypeUri;


        /// <MetaDataID>{dc96cae9-9570-4397-96dc-219cf7d056d5}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+6")]
        public string ServicesPointName;

        /// <MetaDataID>{35e3eb55-7d07-42a3-b092-403261fe8c94}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+7")]
        public string ServicesContextLogo;


        /// <MetaDataID>{da3211d2-7ef0-46ae-9279-66491523d95e}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+1")]
        public string ServicePointIdentity;

        /// <MetaDataID>{6f56b98f-ffc3-417b-8b1e-df15ed74ae15}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+2")]
        public string MenuName;
        /// <MetaDataID>{a00057a4-46ce-4127-8636-b7942011d69d}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+3")]
        public string MenuFile;
        /// <MetaDataID>{6be9f859-d936-4de3-b35a-8e4c26c4582e}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+4")]
        public string MenuRoot;
        /// <MetaDataID>{20f5d8a6-899a-484a-91a0-fa01d431bc44}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+5")]
        public string ClientSessionID;



        ///// <MetaDataID>{bc390286-df9f-4e1a-9a37-6dcc0b32a7ad}</MetaDataID>
        //public List<ItemPreparation> OrderItems;

        /// <MetaDataID>{d367de1a-32b5-43bd-a482-b29a81ff2634}</MetaDataID>
        List<ItemPreparation> GetOrderItems()
        {
            if (_OrderItems == null)
                return new List<ItemPreparation>();
            else
                return _OrderItems.ToList();
        }

        /// <MetaDataID>{f77fbc5a-f8dd-4e39-89d8-37c5f078062c}</MetaDataID>
        public static bool operator ==(MenuData left, MenuData right)
        {
            var equal = left.ServicePointIdentity == right.ServicePointIdentity &&
                left.MenuFile == right.MenuFile &&
                left.MenuRoot == right.MenuRoot &&
                left.ClientSessionID == right.ClientSessionID &&
                left.MainSessionID == right.MainSessionID &&
                left.GetOrderItems().Count == right.GetOrderItems().Count;
            if (!equal)
                return false;
            foreach (var item in left.GetOrderItems())
            {
                if (!right.GetOrderItems().Contains(item))
                    return false;
            }
            return true;

        }
        /// <MetaDataID>{026e94fe-73f5-4cf0-9563-14c72db1ea5b}</MetaDataID>
        public static bool operator !=(MenuData left, MenuData right)
        {
            return !(left == right);
        }


    }


    /// <MetaDataID>{62dfcf63-566a-45f7-aa85-16ce37400804}</MetaDataID>
    public class Location
    {
        public double Latitude;
        public double Longitude;
    }

    /// <MetaDataID>{8cada9df-78e0-4eef-9482-ae9093cef026}</MetaDataID>
    [HttpVisible]

    public interface IServicePointSupervisor
    {
        void TransferItems(List<SessionItemPreparationAbbreviation> itemPreparations, string targetServicePointIdentity);
        //void TransferSession(string sourceServicePointIdentity, string targetServicePointIdentity);

        bool TransferPartialSession(string partialSessionID, string targetSessionID);

    }


    /// <MetaDataID>{94fb1d70-f4e2-400d-b228-aea0bfac4bc9}</MetaDataID>
    public enum InvitationChannel
    {
        SMS = 1,
        Email = 2,
        FBMessenger = 3
    }
}

