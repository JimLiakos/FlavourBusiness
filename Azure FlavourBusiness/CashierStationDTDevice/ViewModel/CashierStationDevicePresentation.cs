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

        /// <MetaDataID>{70bbcb66-446f-41c4-8519-cd86d7f016dc}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SaveCommand { get; set; }

        /// <MetaDataID>{b083528a-a33b-4db2-aa42-65b598210728}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand SettingsCommand { get; set; }

        public WPFUIElementObjectBind.RelayCommand ZReportCommand { get; set; }

        public string ZReportErrorMesage { get; set; }
        public Visibility ZReportErrorMesageVisible { get; set; }


        /// <MetaDataID>{324001e9-7d0b-4556-8425-f0efa25df95f}</MetaDataID>
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CashierStations)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCashierStation)));
            }
            ZReportCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                if (ApplicationSettings.Current.DocumentSignerType == typeof(RBSDocSigner).Name)
                {
                    var rbsDocSigner = new RBSDocSigner();
                    rbsDocSigner.Start(ApplicationSettings.Current.DocumentSignerDeviceIPAddress, ApplicationSettings.Current.AESKey, ApplicationSettings.Current.AADESendDataUrl);
                    string message = null;
                    if (!string.IsNullOrWhiteSpace(ApplicationSettings.Current.DocumentSignerOutputFolder))
                        rbsDocSigner.SetOutputFolder(ApplicationSettings.Current.DocumentSignerOutputFolder);

                    if (rbsDocSigner.IssueZreport(out message))
                    {
                        CashierStationDevice.DocumentSignDevice.Init(rbsDocSigner);
                        ZReportErrorMesage = "";
                        ZReportErrorMesageVisible = Visibility.Collapsed;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZReportErrorMesage)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZReportErrorMesageVisible)));
                    }
                    else
                    {
                        ZReportErrorMesage = message;
                        ZReportErrorMesageVisible = Visibility.Visible;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZReportErrorMesage)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZReportErrorMesageVisible)));

                    }
                }


            });
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

                var report = DXConnectableControls.XtraReports.UI.Report.FromFile(@"F:\myproject\terpo\OpenVersions\FlavourBusiness\Azure FlavourBusiness\CashierStationDTDevice\Resources\InvoiceReport.repx", true);

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
                        _SelectedCashierStation = CashierStations.Where(x => x.CashierStation.CashierStationIdentity == ApplicationSettings.Current.CommunicationCredentialKey).FirstOrDefault();
                        if (_SelectedCashierStation == null)
                        {
                            _SelectedCashierStation = CashierStations[0];
                            ApplicationSettings.Current.CommunicationCredentialKey = _SelectedCashierStation.CashierStation.CashierStationIdentity;
                        }
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


        /// <MetaDataID>{52c82882-6416-4a2e-97ac-7ed34295542e}</MetaDataID>
        public List<Type> DocSigners
        {
            get
            {
                return new List<Type> { typeof(SamtecNext), typeof(RBSDocSigner) };
            }
        }

        /// <MetaDataID>{19e51f85-0453-4768-b13f-d9c7ba642b72}</MetaDataID>
        public Type SelectedDocSigner
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ApplicationSettings.Current.DocumentSignerType))
                {
                    DocumentSignerOutputFolderVisible = Visibility.Collapsed;
                    DocumentSignerDeviceIPAddressVisible = Visibility.Collapsed;
                    ZReportCommandVisible = Visibility.Collapsed;
                    return typeof(SamtecNext);
                }
                if (ApplicationSettings.Current.DocumentSignerType == "SamtecNext")
                {
                    DocumentSignerOutputFolderVisible = Visibility.Collapsed;
                    DocumentSignerDeviceIPAddressVisible = Visibility.Collapsed;
                    ZReportCommandVisible = Visibility.Collapsed;
                    AESKeyVisible = Visibility.Collapsed;
                    AADESendDataUrlVisible = Visibility.Collapsed;
                    return typeof(SamtecNext);
                }
                if (ApplicationSettings.Current.DocumentSignerType == "RBSDocSigner")
                {
                    DocumentSignerOutputFolderVisible = Visibility.Visible;
                    DocumentSignerDeviceIPAddressVisible = Visibility.Visible;
                    ZReportCommandVisible = Visibility.Visible;
                    AESKeyVisible= Visibility.Visible;
                    AADESendDataUrlVisible = Visibility.Visible;

                    return typeof(RBSDocSigner);
                }

                return typeof(SamtecNext);

                //  ApplicationSettings.Current.DocumentSignerCommunicationData
            }
            set
            {
                ApplicationSettings.Current.DocumentSignerType = value.Name;
                if (value == typeof(RBSDocSigner))
                {
                    DocumentSignerOutputFolderVisible = Visibility.Visible;
                    DocumentSignerDeviceIPAddressVisible = Visibility.Visible;
                    ZReportCommandVisible = Visibility.Visible;
                    AESKeyVisible = Visibility.Visible;
                    AADESendDataUrlVisible = Visibility.Visible;
                }
                else
                {
                    DocumentSignerOutputFolderVisible = Visibility.Collapsed;
                    DocumentSignerDeviceIPAddressVisible = Visibility.Collapsed;
                    ZReportCommandVisible = Visibility.Collapsed;
                    AESKeyVisible = Visibility.Collapsed;
                    AADESendDataUrlVisible = Visibility.Collapsed;
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditCompanyHeader)));
            }
        }

        System.Windows.Visibility _ZReportCommandVisible;
        public System.Windows.Visibility ZReportCommandVisible
        {
            get => _ZReportCommandVisible;
            set
            {
                _ZReportCommandVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZReportCommandVisible)));
            }
        }
        System.Windows.Visibility _DocumentSignerDeviceIPAddressVisible;
        public System.Windows.Visibility DocumentSignerDeviceIPAddressVisible
        {
            get => _DocumentSignerDeviceIPAddressVisible;
            set
            {
                _DocumentSignerDeviceIPAddressVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DocumentSignerDeviceIPAddressVisible)));
            }
        }
        System.Windows.Visibility _AESKeyVisible;
        public System.Windows.Visibility AESKeyVisible
        {
            get => _AESKeyVisible;
            set
            {
                _AESKeyVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AESKeyVisible)));
            }
        }

        System.Windows.Visibility _AADESendDataUrlVisible;
        public System.Windows.Visibility AADESendDataUrlVisible
        {
            get => _AADESendDataUrlVisible;
            set
            {
                _AADESendDataUrlVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AADESendDataUrlVisible)));
            }
        }

        //string AESKey

        System.Windows.Visibility _DocumentSignerOutputFolderVisible;
        public System.Windows.Visibility DocumentSignerOutputFolderVisible
        {
            get => _DocumentSignerOutputFolderVisible;
            set
            {
                _DocumentSignerOutputFolderVisible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DocumentSignerOutputFolderVisible)));
            }
        }



        /// <MetaDataID>{611b9e37-2871-426d-9d29-9059c622c32d}</MetaDataID>
        public System.Collections.Generic.List<TransactionPrinterPresentation> TransactionsPrinters
        {
            get
            {
                return (from transactionPrinter in ApplicationSettings.Current.TransactionsPrinters
                        select TransactionPrintersDictionary.GetViewModelFor(transactionPrinter, transactionPrinter)).ToList();
            }
        }
        /// <MetaDataID>{3fdb413b-43e3-4888-809e-88955d4deae2}</MetaDataID>
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

        /// <MetaDataID>{9fc293d1-700f-4c0c-94f1-e37f06e497a3}</MetaDataID>
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

        /// <MetaDataID>{f949112c-8ee5-48ff-a81d-85111c553ed9}</MetaDataID>
        public bool EditCompanyHeader { get; set; }
        /// <MetaDataID>{ac54a396-f2d3-43eb-b0ff-804d757864bb}</MetaDataID>
        CompanyHeader CompanyHeader;

        /// <MetaDataID>{1954dcbe-ea19-4e13-b627-eefa3029e2ac}</MetaDataID>
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

        /// <MetaDataID>{84be1d4b-23ad-48dd-a39e-3e06401205f8}</MetaDataID>
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
        /// <MetaDataID>{d4b9e39d-d298-4066-8f6a-d61606ae7535}</MetaDataID>
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
        /// <MetaDataID>{825eb813-2dec-4dd2-9418-da129f533f28}</MetaDataID>
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


        /// <MetaDataID>{835d27af-3da7-40ce-8966-57a1eaa83aa4}</MetaDataID>
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

        /// <MetaDataID>{3238e1cc-f476-4f37-8c57-5376866cf780}</MetaDataID>
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

        public string DeviceIPAddress
        {
            get
            {
                return ApplicationSettings.Current.DocumentSignerDeviceIPAddress;
            }
            set
            {
                ApplicationSettings.Current.DocumentSignerDeviceIPAddress = value;
            }
        }


        public string AESKey
        {
            get
            {
                return ApplicationSettings.Current.AESKey;
            }
            set
            {
                ApplicationSettings.Current.AESKey = value;
            }
        }

        public string AADESendDataUrl
        {
            get
            {
                return ApplicationSettings.Current.AADESendDataUrl;
            }
            set
            {
                ApplicationSettings.Current.AADESendDataUrl = value;
            }
        }


        string invalidOrder;
        public string DocumentSignerOutputFolder
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(invalidOrder))
                    return invalidOrder;
                return ApplicationSettings.Current.DocumentSignerOutputFolder;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !System.IO.Directory.Exists(value))
                {
                    invalidOrder = "Μη έγκυρος φάκελος.";
                    ApplicationSettings.Current.DocumentSignerOutputFolder = null;
                    Task.Run(() =>
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DocumentSignerOutputFolder)));
                    });
                }
                else
                {
                    invalidOrder = null;
                    ApplicationSettings.Current.DocumentSignerOutputFolder = value;
                }
            }
        }


    }
}
