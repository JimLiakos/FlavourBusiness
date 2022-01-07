using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using FLBAuthentication.ViewModel;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UIBaseEx;

namespace CashierStationDevice.ViewModel
{
    /// <MetaDataID>{5a0acdba-686f-421f-8708-457a6d462606}</MetaDataID>
    public class CashierStationDevicePresentation : MarshalByRefObject, INotifyPropertyChanged
    {

        public WPFUIElementObjectBind.RelayCommand SaveCommand { get; set; }

        /// <MetaDataID>{b083528a-a33b-4db2-aa42-65b598210728}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SettingsCommand { get; set; }

        public WPFUIElementObjectBind.RelayCommand ReportDesignerCommand { get; set; }
        /// <MetaDataID>{5b9797ed-270d-471b-88e3-f47f9488c62d}</MetaDataID>
        public CashierStationDevicePresentation()
        {
            _SignInUserPopup = new SignInUserPopupViewModel(UserData.RoleType.ServiceContextSupervisor);
            _SignInUserPopup.SignedIn += SignInUserPopup_SignedIn;
            _SignInUserPopup.SignedOut += SignInUserPopup_SignedOut;
            if (ApplicationSettings.Current.CompanyHeader != null)
                CompanyHeader = new CompanyHeader()
                {
                    Address = ApplicationSettings.Current.CompanyHeader.Address,
                    ContatInfo = ApplicationSettings.Current.CompanyHeader.ContatInfo,
                    FiscalData = ApplicationSettings.Current.CompanyHeader.FiscalData,
                    Subtitle = ApplicationSettings.Current.CompanyHeader.Subtitle,
                    Thankfull = ApplicationSettings.Current.CompanyHeader.Thankfull,
                    Title = ApplicationSettings.Current.CompanyHeader.Thankfull
                };
            if (ApplicationSettings.Current.CashiersStation != null)
            {
                CashierStations = new List<CashierStationPresentation>() { new CashierStationPresentation(ApplicationSettings.Current.CashiersStation as ICashierStation, null) };
                _SelectedCashierStation = CashierStations[0];
            }

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
            ReportDesignerCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                DXConnectableControls.XtraReports.Design.ReportDesignForm reportDesignForm = new DXConnectableControls.XtraReports.Design.ReportDesignForm();
                reportDesignForm.OpenReport(@"F:\myproject\terpo\OpenVersions\FlavourBusiness\Azure FlavourBusiness\CashierStationDTDevice\Resources\InvoiceReport.repx");

                Invoice invoice = new Invoice((Application.Current as CashierStationDTDevice.App).CashierController.Transactions.FirstOrDefault(), (Application.Current as CashierStationDTDevice.App).CashierController.Issuer);
                reportDesignForm.Report.DataSource = new List<Invoice>() { invoice };

                var report =  DXConnectableControls.XtraReports.UI.Report.FromFile(@"F:\myproject\terpo\OpenVersions\FlavourBusiness\Azure FlavourBusiness\CashierStationDTDevice\Resources\InvoiceReport.repx", true);
                
                report.DataSource = new List<Invoice>() { invoice };
                report.CreateDocument();

                report.Print();
                //report.PrinterName = "Microsoft Print to PDF";
                //report.Print("Microsoft Print to PDF");
                //var printers = PrinterSettings.InstalledPrinters.OfType<string>().ToList();
                //var ss = report.PrinterName


                //OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource = (reportDesignForm.Report as OOAdvantech.UserInterface.ReportObjectDataSource.IReport).ReportDataSource;
                //reportDataSource.AssemblyFullName = typeof(IOrder).Assembly.FullName;
                //reportDataSource.TypeFullName = typeof(IOrder).FullName;
                //reportDesignForm.Report.ResumeDataSource();
                //int tt= reportDataSource.DataSourceMembers.Count;
                //reportDesignForm.OpenReport(@"c:\report1.repx");
                //DXConnectableControls.XtraReports.UI.Report report = DXConnectableControls.XtraReports.UI.Report.FromFile(@"c:\report1.repx",true) as DXConnectableControls.XtraReports.UI.Report;
                //reportDesignForm.OpenReport(report);
                //reportDesignForm.ShowDialog();

            });
            SaveCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                if (EditCompanyHeader)
                {
                    _SelectedCashierStation.CashierStation.CashierStationDeviceData = OOAdvantech.Json.JsonConvert.SerializeObject(CompanyHeader);
                    ApplicationSettings.Current.CompanyHeader = CompanyHeader;
                }

            });
        }

        /// <MetaDataID>{62a7078c-27f5-4726-ac32-66098e2db723}</MetaDataID>
        private void SignInUserPopup_SignedOut(SignInUserPopupViewModel authedication, IUser user)
        {
            EditCompanyHeader = false;
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


                    CashierStations = (role.User as FlavourBusinessFacade.HumanResources.IServiceContextSupervisor).ServicesContextRunTime.CashierStations.Select(x => new CashierStationPresentation(x, (role.User as FlavourBusinessFacade.HumanResources.IServiceContextSupervisor).ServicesContext)).ToList();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CashierStations)));
                    if (CashierStations.Count > 0)
                    {

                        _SelectedCashierStation = CashierStations[0];
                        if (_SelectedCashierStation != null)
                        {
                            EditCompanyHeader = true;
                            GetCompanyHeader();
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCashierStation)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditCompanyHeader)));
                    }


                }
            }

        }
        #endregion

        /// <MetaDataID>{dab55f71-b0a8-4c22-96a7-054f74f15014}</MetaDataID>
        public List<CashierStationPresentation> CashierStations { get; private set; }

        public System.Collections.Generic.List<TransactionPrinterPresentation> TransactionsPrinters
        {
            get
            {
                return (from transactionPrinter in ApplicationSettings.Current.TransactionsPrinters
                        select TransactionPrintersDictionary.GetViewModelFor(transactionPrinter, transactionPrinter)).ToList();
            }
        }
        internal ViewModelWrappers<Model.TransactionPrinter, TransactionPrinterPresentation> TransactionPrintersDictionary = new ViewModelWrappers<Model.TransactionPrinter, TransactionPrinterPresentation>();


        /// <exclude>Excluded</exclude>
        CashierStationPresentation _SelectedCashierStation;
        /// <MetaDataID>{8f2a8cbf-5d47-4a6b-a630-ba41de36eb43}</MetaDataID>
        public CashierStationPresentation SelectedCashierStation
        {
            get
            {

                if (_SelectedCashierStation == null && CashierStations != null && !string.IsNullOrWhiteSpace(ApplicationSettings.Current.CommunicationCredentialKey))
                {
                    _SelectedCashierStation = CashierStations.Where(x => x.CashierStation.CashierStationIdentity == ApplicationSettings.Current.CommunicationCredentialKey).FirstOrDefault();
                    if (_SelectedCashierStation != null)
                        GetCompanyHeader();
                }
                return _SelectedCashierStation;
            }
            set
            {
                if (_SelectedCashierStation != value)
                {
                    _SelectedCashierStation = value;
                    if (_SelectedCashierStation != null)
                    {
                        GetCompanyHeader();

                        ApplicationSettings.Current.CommunicationCredentialKey = _SelectedCashierStation.CashierStation.CashierStationIdentity;
                    }
                    else
                        ApplicationSettings.Current.CommunicationCredentialKey = null;
                }

            }

        }

        private void GetCompanyHeader()
        {
            if (CompanyHeader == null && !string.IsNullOrWhiteSpace(_SelectedCashierStation.CashierStation.CashierStationDeviceData))
            {
                CompanyHeader = OOAdvantech.Json.JsonConvert.DeserializeObject<CompanyHeader>(_SelectedCashierStation.CashierStation.CashierStationDeviceData);
                EditCompanyHeader = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditCompanyHeader)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompanyTitle)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompanySubTitle)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContatInfo)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FiscalData)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Address)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Thankfull)));
            }
            else if (CompanyHeader == null)
            {
                CompanyHeader = new CompanyHeader();
                EditCompanyHeader = true;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditCompanyHeader)));
            }
        }

        public bool EditCompanyHeader { get; set; }
        CompanyHeader CompanyHeader;

        public string CompanyTitle
        {
            get
            {
                return CompanyHeader?.Title;
            }
            set
            {
                CompanyHeader.Title = value;
            }
        }

        public string CompanySubTitle
        {
            get
            {
                return CompanyHeader?.Subtitle;
            }
            set
            {
                CompanyHeader.Subtitle = value;
            }
        }
        public string ContatInfo
        {
            get
            {
                return CompanyHeader?.ContatInfo;
            }
            set
            {
                CompanyHeader.ContatInfo = value;
            }
        }
        public string FiscalData
        {
            get
            {
                return CompanyHeader?.FiscalData;
            }
            set
            {
                CompanyHeader.FiscalData = value;
            }
        }


        public string Address
        {
            get
            {
                return CompanyHeader?.Address;
            }
            set
            {
                CompanyHeader.Address = value;
            }
        }

        public string Thankfull
        {
            get
            {
                return CompanyHeader?.Thankfull;
            }
            set
            {
                CompanyHeader.Thankfull = value;
            }
        }


    }
}
