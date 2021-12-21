using FlavourBusinessFacade;
using FLBAuthentication.ViewModel;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierStationDevice.ViewModel
{
    /// <MetaDataID>{5a0acdba-686f-421f-8708-457a6d462606}</MetaDataID>
    public class CashierStationDevicePresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        /// <MetaDataID>{b083528a-a33b-4db2-aa42-65b598210728}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SettingsCommand { get; set; }
        /// <MetaDataID>{5b9797ed-270d-471b-88e3-f47f9488c62d}</MetaDataID>
        public CashierStationDevicePresentation()
        {
            _SignInUserPopup = new SignInUserPopupViewModel(UserData.RoleType.ServiceContextSupervisor);
            _SignInUserPopup.SignedIn += SignInUserPopup_SignedIn;
            _SignInUserPopup.SignedOut += SignInUserPopup_SignedOut;

            SettingsCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
              {
                  System.Windows.Window win = System.Windows.Window.GetWindow(SettingsCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                  var pageDialogFrame = WPFUIElementObjectBind.ObjectContext.FindChilds<StyleableWindow.PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();


                  using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                  {

                      var cashierStationSettingsPage = new CashierStationDevice.Views.CashierStationSettingsPage();
                      cashierStationSettingsPage.GetObjectContext().SetContextInstance(this);
                      pageDialogFrame.ShowDialogPage(cashierStationSettingsPage);
                      stateTransition.Consistent = true;
                  }

              });

        }

        /// <MetaDataID>{62a7078c-27f5-4726-ac32-66098e2db723}</MetaDataID>
        private void SignInUserPopup_SignedOut(SignInUserPopupViewModel authedication, IUser user)
        {

        }
        #region Authentication
        /// <exclude>Excluded</exclude>
        SignInUserPopupViewModel _SignInUserPopup;
        /// <MetaDataID>{a22a75d5-ffbe-44a3-88c8-66c6961fca1a}</MetaDataID>
        public SignInUserPopupViewModel SignInUser
        {
            get
            {
                return _SignInUserPopup;
            }
            set
            {
                if (_SignInUserPopup != null)
                    _SignInUserPopup.SignedIn -= SignInUserPopup_SignedIn;
                _SignInUserPopup = value;

                if (_SignInUserPopup != null)
                    _SignInUserPopup.SignedIn += SignInUserPopup_SignedIn;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{8c515cd5-ba0c-4853-b99c-377ae891e2c8}</MetaDataID>
        private void SignInUserPopup_SignedIn(SignInUserPopupViewModel authedication, IUser user)
        {

            foreach (var role in user.Roles)
            {
                if (role.RoleType == UserData.RoleType.ServiceContextSupervisor)
                {
                    CashierStations = (role.User as FlavourBusinessFacade.HumanResources.IServiceContextSupervisor).ServicesContextRunTime.CashierStations.Select(x => new CashierStationPresentation(x)).ToList();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CashierStations)));
                    if (SelectedCashierStation == null && CashierStations.Count > 0)
                    {
                        SelectedCashierStation = CashierStations[0];
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCashierStation)));
                    }

                    
                }
            }

        }
        #endregion

        /// <MetaDataID>{dab55f71-b0a8-4c22-96a7-054f74f15014}</MetaDataID>
        public List<CashierStationPresentation> CashierStations { get; private set; }

         public System.Collections.Generic.List<Model.TransactionPrinter> TransactionsPrinters => ApplicationSettings.Current?.TransactionsPrinters;

        /// <exclude>Excluded</exclude>
        CashierStationPresentation _SelectedCashierStation;
        /// <MetaDataID>{8f2a8cbf-5d47-4a6b-a630-ba41de36eb43}</MetaDataID>
        public CashierStationPresentation SelectedCashierStation
        {
            get
            {
                
                if (_SelectedCashierStation == null && CashierStations != null && !string.IsNullOrWhiteSpace(ApplicationSettings.Current.CommunicationCredentialKey))
                    _SelectedCashierStation = CashierStations.Where(x => x.CashierStation.CashierStationIdentity == ApplicationSettings.Current.CommunicationCredentialKey).FirstOrDefault();
                return _SelectedCashierStation;
            }
            set
            {
                if (_SelectedCashierStation != value)
                {
                    _SelectedCashierStation = value;
                    if (_SelectedCashierStation != null)
                        ApplicationSettings.Current.CommunicationCredentialKey = _SelectedCashierStation.CashierStation.CashierStationIdentity;
                    else
                        ApplicationSettings.Current.CommunicationCredentialKey = null;
                }

            }

        }
    }
}
