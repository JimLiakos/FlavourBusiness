using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using System.Threading.Tasks;
using OOAdvantech.MetaDataRepository;
using FlavourBusinessManager.RoomService;
using OOAdvantech.Remoting.RestApi;

namespace PreparationStationDevice
{
    /// <MetaDataID>{293c7b92-a89a-4179-a8ff-616948355d82}</MetaDataID>
    public class FlavoursPreparationStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public FlavoursPreparationStation()
        {
            var communicationCredentialKey = this.CommunicationCredentialKey;
        }

        //static string AzureServerUrl = "http://localhost:8090/api/";
        //static string AzureServerUrl = "http://192.168.2.5:8090/api/";
        //static strinb AzureServerUrl = "http://192.168.2.10:8090/api/";

        static string AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string AzureServerUrl = "http://192.168.2.4:8090/api/";//Braxati
        //static string AzureServerUrl = "http://10.0.0.13:8090/api/";//work



        IPreparationStationRuntime _PreparationStation;
        IPreparationStationRuntime PreparationStation
        {
            get => _PreparationStation;
            set
            {

                if (_PreparationStation != value)
                {
                    if (_PreparationStation is ITransparentProxy)
                        (_PreparationStation as ITransparentProxy).Reconnected -= PreparationStation_Reconnected;
                    
                    _PreparationStation = value;

                    if (_PreparationStation is ITransparentProxy)
                        (_PreparationStation as ITransparentProxy).Reconnected += PreparationStation_Reconnected;
                }
            }
        }

        private void PreparationStation_Reconnected(object sender)
        {


        }

        List<ServicePointPreparationItems> ServicePointsPreparationItems = new List<ServicePointPreparationItems>();

        [OOAdvantech.MetaDataRepository.HttpVisible]
        public Task<List<PreparationItemsPerServicePoint>> WebUIAttached()
        {

            return Task<List<PreparationItemsPerServicePoint>>.Run(() =>
            {
                if (PreparationStation == null && !string.IsNullOrWhiteSpace(CommunicationCredentialKey))
                {

                    string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                    string serverUrl = AzureServerUrl;
                    IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                    PreparationStation = servicesContextManagment.GetPreparationStationRuntime(CommunicationCredentialKey);
                    Title = PreparationStation.Description;

                    ServicePointsPreparationItems = PreparationStation.GetPreparationItems(new List<ItemPreparationAbbreviation>()).ToList();

                    //var menuItems = PreparationStation.GetNewerRestaurandMenuData(DateTime.MinValue);
                }
                var preparationItemsPerServicePoint = (from servicePointItems in ServicePointsPreparationItems
                                                       select new PreparationItemsPerServicePoint()
                                                       {
                                                           Description = servicePointItems.ServicePoint.Description,
                                                           ServicesContextIdentity = servicePointItems.ServicePoint.ServicesContextIdentity,
                                                           ServicesPointIdentity = servicePointItems.ServicePoint.ServicesPointIdentity,
                                                           PreparationItems = servicePointItems.PreparationItems.OfType<ItemPreparation>().Select(x => new PreparationStationItem(x, servicePointItems)).ToList()
                                                       }).ToList();
                return preparationItemsPerServicePoint;
            });
        }
        
        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string Title
        {
            
            get => ApplicationSettings.Current.PreparationStationTitle;
            set => ApplicationSettings.Current.PreparationStationTitle = value; 
        }




        [GenerateEventConsumerProxy]
        event PreparationItemsLoadedHandle PreparationItemsLoaded;



        [OOAdvantech.MetaDataRepository.HttpVisible]
        public string CommunicationCredentialKey
        {
            get
            {
                return ApplicationSettings.Current.CommunicationCredentialKey;
            }
            set
            {
                ApplicationSettings.Current.CommunicationCredentialKey = value;
            }
        }
    }

    public delegate void PreparationItemsLoadedHandle(FlavoursPreparationStation sender);


}
